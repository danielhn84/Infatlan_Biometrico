using BiometricoWeb.clases;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;

namespace BiometricoWeb.pages.activos
{
    public partial class visitaDatacenter : System.Web.UI.Page
    {
        db vConexion = new db();
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            String vEx = Request.QueryString["ex"];
            if (!IsPostBack)
            {
                CargarInformacionGeneral();
                Session["ACTIVO_DC_ID_ESTADO"] = "1";

                if (string.IsNullOrEmpty(vEx))
                {
                    CargarInformacionGeneral();
                    Session["ACTIVO_DC_ID_ESTADO"] = "1";
                }
                else if (vEx.Equals("1"))
                {
                    CargarInformacionGeneral();
                    camposDeshabilitados();
                    cargarDataVista();
                    divPersonalExterno.Visible = false;
                    divPersonalInterno.Visible = false;
                    DivCrearSolicitud.Visible = false;
                    DivAprobarSolicitudJefe.Visible = true;
                }
                else if (vEx.Equals("2"))
                {
                    CargarInformacionGeneral();
                    camposDeshabilitados();
                    cargarDataVista();
                    divPersonalExterno.Visible = false;
                    divPersonalInterno.Visible = false;
                    DivCrearSolicitud.Visible = false;
                    DivAprobarSolicitudJefe.Visible = false;
                }

            }
        }
        void CargarInformacionGeneral()
        {
            try
            {
                String vQuery = "RSP_ActivosDC 1,'" + Convert.ToString(Session["USUARIO"]) + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                TxResponsable.Text = vDatos.Rows[0]["nombre"].ToString();
                TxIdentidadResponsable.Text = vDatos.Rows[0]["identidad"].ToString();
                TxSubgerencia.Text = vDatos.Rows[0]["area"].ToString();
                TxJefe.Text = vDatos.Rows[0]["jefeNombre"].ToString();
                Session["ACTIVO_DC_EMAIL_RESPONSABLE"] = vDatos.Rows[0]["emailEmpresa"].ToString();
                Session["ACTIVO_DC_EMAIL_JEFE_RESPONSABLE"] = vDatos.Rows[0]["jefecorreo"].ToString();
                Session["ACTIVO_DC_CODIGO_JEFE"] = vDatos.Rows[0]["idJefe"].ToString();
                //TxFechaSolicitud.Text= DateTime.Today.ToString("dd/MM/yyyy");

                DdlNombreCopia.Items.Clear();
                DdlSupervisar.Items.Clear();
                vQuery = "RSP_ActivosDC 2,'" + TxSubgerencia.Text + "'";
                DataTable vDatosCopia = vConexion.obtenerDataTable(vQuery);
                DdlNombreCopia.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                DdlSupervisar.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                if (vDatosCopia.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosCopia.Rows)
                    {
                        DdlNombreCopia.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
                        DdlSupervisar.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
                    }
                }

                vQuery = "RSP_ActivosDC 4";
                DataTable vDatosEmpresas = vConexion.obtenerDataTable(vQuery);
                DdlEmpresaVisita.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                if (vDatosEmpresas.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosEmpresas.Rows)
                    {
                        DdlEmpresaVisita.Items.Add(new ListItem { Value = item["idEmpresa"].ToString(), Text = item["empresa"].ToString() });
                    }
                }

                vQuery = "RSP_ActivosDC 5";
                DataTable vDatosEquipo = vConexion.obtenerDataTable(vQuery);
                DdlEquipo.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                if (vDatosEquipo.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosEquipo.Rows)
                    {
                        DdlEquipo.Items.Add(new ListItem { Value = item["idDetalle"].ToString(), Text = item["descripcion"].ToString() });
                    }
                }


                ddlPersonalInterno.Items.Clear();
                vQuery = "RSP_ActivosDC 6";
                DataTable vDatosInterno = vConexion.obtenerDataTable(vQuery);
                ddlPersonalInterno.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                if (vDatosInterno.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosInterno.Rows)
                    {
                        ddlPersonalInterno.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
                    }
                }

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        protected void DdlNombreCopia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                String vQuery = "RSP_ActivosDC 3,'" + DdlNombreCopia.SelectedValue + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                TxIdentidadCopia.Text = vDatos.Rows[0]["identidad"].ToString();
                Session["ACTIVO_DC_EMAIL_COPIA"] = vDatos.Rows[0]["emailEmpresa"].ToString();

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        void calculoHoras()
        {
            if (TxInicio.Text != "" && TxFin.Text != "")
            {
                String vFI = TxInicio.Text != "" ? TxInicio.Text : "1999-01-01 00:00:00";
                String vFF = TxFin.Text != "" ? TxFin.Text : "1999-01-01 00:00:00";

                DateTime vdesde = Convert.ToDateTime(vFI);
                DateTime vhasta = Convert.ToDateTime(vFF);

                String vFormato = "dd/MM/yyyy"; //"DD/MM/YYYY HH:mm:ss"            

                String vIni = Convert.ToDateTime(vdesde).ToString(vFormato);
                String vFin = Convert.ToDateTime(vhasta).ToString(vFormato);

                TimeSpan difFechas = Convert.ToDateTime(vFin) - Convert.ToDateTime(vIni);
                int dias = difFechas.Days;

                if (dias==0 || dias==1){
                    LbNotaDias_01.Visible = true;
                    LbNotaDiasMenor16.Visible = false;
                    LbNotaDiasMayor16.Visible = false;
                }
                else if (dias >=2 && dias <16){
                    LbNotaDiasMenor16.Visible = true;
                    LbNotaDias_01.Visible = false;
                    LbNotaDiasMayor16.Visible = false;
                    TxInicio.Text = String.Empty;
                    TxFin.Text = String.Empty;

                }
                else if (dias >= 16){
                    LbNotaDiasMenor16.Visible = false;
                    LbNotaDias_01.Visible = false;
                    LbNotaDiasMayor16.Visible = true;
                }             
            }
        }
        void agregarRowDatagrid()
        {
            DataTable vData = new DataTable();
            DataTable vDatos = (DataTable)Session["ACTIVO_DC_PERSONAL_EXTERNO"];

            vData.Columns.Add("nombre");
            vData.Columns.Add("identidad");
            vData.Columns.Add("empresa");
            vData.Columns.Add("ingresoEquipo");
            vData.Columns.Add("permisoCel");

            if (vDatos == null)
                vDatos = vData.Clone();
            if (vDatos != null)
            {
                if (vDatos.Rows.Count < 1)
                {
                    vDatos.Rows.Add(TxNombreVisita.Text, TxIdentidadVisita.Text, DdlEmpresaVisita.SelectedValue, RbIngresoEquipoVisita.SelectedValue, RbPermisoCelular.SelectedValue);

                }
                else
                {
                    Boolean vRegistered = false;
                    //for (int i = 0; i < vDatos.Rows.Count; i++)
                    //{
                    //    if (vNombreMaterialMatriz == vDatos.Rows[i]["nombre"].ToString())
                    //    {
                    //        //vDatos.Rows[i]["cantidad"] = Convert.ToDecimal(vDatos.Rows[i]["cantidad"].ToString()) + Convert.ToDecimal(TxCantidad.Text);

                    //        lbCantidad.Text = "El material seleccionado: " + vNombreMaterialMatriz + " ya esta agregado en la lista, favor verificar";
                    //        DivAlertaCantidad.Visible = true;
                    //        UpCantidadMaxima.Update();

                    //        vRegistered = true;
                    //    }
                    //}

                    if (!vRegistered)
                        vDatos.Rows.Add(TxNombreVisita.Text, TxIdentidadVisita.Text, DdlEmpresaVisita.SelectedValue, RbIngresoEquipoVisita.SelectedValue, RbPermisoCelular.SelectedValue);
                }
            }
            GvVisitas.DataSource = vDatos;
            GvVisitas.DataBind();
            Session["ACTIVO_DC_PERSONAL_EXTERNO"] = vDatos;
            UPVisitas.Update();
            //UpdatePanel1.Update();
        }
        protected void TxInicio_TextChanged(object sender, EventArgs e)
        {
            try
            {
             calculoHoras();
            }
            catch (Exception Ex) {               
                String vEx = Request.QueryString["ex"];
                Mensaje(Ex.Message, WarningType.Danger);              
            }            
        }
        protected void TxFin_TextChanged(object sender, EventArgs e)
        {
            calculoHoras();
        }
        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (RbIngresoEquipoVisita.SelectedValue == "1")
                {
                    if (HttpContext.Current.Session["ACTIVO_DC_EQUIPO_REGISTRADO"] != null)
                    {
                        string vIdentidad = TxIdentidadVisita.Text;
                        DataTable vDatos = (DataTable)Session["ACTIVO_DC_EQUIPO_REGISTRADO"];
                        EnumerableRowCollection<DataRow> filtered = vDatos.AsEnumerable()
                               .Where(r => r.Field<String>("identidad").Contains(vIdentidad));

                        DataTable vDatosFiltrados = new DataTable();
                        vDatosFiltrados.Columns.Add("identidad");
                        vDatosFiltrados.Columns.Add("id");
                        vDatosFiltrados.Columns.Add("equipo");
                        vDatosFiltrados.Columns.Add("serie");
                        vDatosFiltrados.Columns.Add("inventario");

                        foreach (DataRow item in filtered)
                        {
                            vDatosFiltrados.Rows.Add(
                                item["identidad"].ToString(),
                                item["id"].ToString(),
                                item["equipo"].ToString(),
                                item["serie"].ToString(),
                                item["inventario"].ToString()
                                );
                        }
                        GvEquipo.DataSource = vDatosFiltrados;
                        GvEquipo.DataBind();
                        UPVisitas.Update();
                        UpdatePanel4.Update();                      
                    }
                 
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "abrirModalRegistroEquipo();", true);
                }
                else { agregarRowDatagrid(); }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        //protected void RbIngresoEquipo_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (RbIngresoEquipo.SelectedValue=="1")
        //        {
        //            Session["ACTIVO_DC_EQUIPO_REGISTRADO"] = null;
        //            UpdatePanel7.Update();
        //            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "abrirModalRegistroEquipo();", true);
        //        }
        //        else
        //        {
        //            Session["ACTIVO_DC_EQUIPO_REGISTRADO"] = null;
        //            UpdatePanel7.Update();
        //        }
        //    }
        //    catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }


        //}

        protected void BtnAgregarEquipo_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable vData = new DataTable();
                DataTable vDatos = (DataTable)Session["ACTIVO_DC_EQUIPO_REGISTRADO"];
                vData.Columns.Add("id");
                vData.Columns.Add("equipo");
                vData.Columns.Add("serie");
                vData.Columns.Add("inventario");
                vData.Columns.Add("identidad");

                if (vDatos == null )
                    vDatos = vData.Clone();

                if (vDatos != null)
                {
                    if (vDatos.Rows.Count < 1)
                    {
                        vDatos.Rows.Add("1", DdlEquipo.SelectedValue, TxSerie.Text, TxInventario.Text, TxIdentidadVisita.Text);
                        //vDatosGenerales.Rows.Add("1", DdlEquipo.SelectedItem, TxSerie.Text, TxInventario.Text, TxIdentidadVisita.Text);
                    }
                    else
                    {
                        Boolean vRegistered = false;
                        //for (int i = 0; i < vDatos.Rows.Count; i++)
                        //{
                        //    if (vNombreMaterialMatriz == vDatos.Rows[i]["nombre"].ToString())
                        //    {
                        //        //vDatos.Rows[i]["cantidad"] = Convert.ToDecimal(vDatos.Rows[i]["cantidad"].ToString()) + Convert.ToDecimal(TxCantidad.Text);

                        //        lbCantidad.Text = "El material seleccionado: " + vNombreMaterialMatriz + " ya esta agregado en la lista, favor verificar";
                        //        DivAlertaCantidad.Visible = true;
                        //        UpCantidadMaxima.Update();

                        //        vRegistered = true;
                        //    }
                        //}

                        if (!vRegistered)
                            vDatos.Rows.Add((vDatos.Rows.Count)+1, DdlEquipo.SelectedValue, TxSerie.Text, TxInventario.Text, TxIdentidadVisita.Text);       

                    }
                }

                GvEquipo.DataSource = vDatos;
                GvEquipo.DataBind();
                Session["ACTIVO_DC_EQUIPO_REGISTRADO"] = vDatos;
                //Session["ACTIVO_DC_EQUIPO_REGISTRADO_GLOBAL"] = vDatosGenerales;
                UPVisitas.Update();
                UpdatePanel4.Update();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        //protected void BtnModificarEquipo_Click(object sender, EventArgs e)
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "abrirModalRegistroEquipo();", true);
        //}

        protected void BtnAceptarEquipo_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "cerrarModalRegistroEquipo();", true);
                agregarRowDatagrid();

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnCancelarEquipo_Click(object sender, EventArgs e)
        {
            try
            {                
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "cerrarModalRegistroEquipo();", true);
                Session["ACTIVO_DC_EQUIPO_REGISTRADO"] = null;
                GvEquipo.DataSource = null;
                GvEquipo.DataBind();
                UpdatePanel4.Update();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        protected void GvVisitas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "informacion")
                {
                    string vIdentidad = e.CommandArgument.ToString();
                    DataTable vDatos = (DataTable)Session["ACTIVO_DC_EQUIPO_REGISTRADO"];
                    EnumerableRowCollection<DataRow> filtered = vDatos.AsEnumerable()
                           .Where(r => r.Field<String>("identidad").Contains(vIdentidad));

                    DataTable vDatosFiltrados = new DataTable();
                    vDatosFiltrados.Columns.Add("identidad");
                    vDatosFiltrados.Columns.Add("id");
                    vDatosFiltrados.Columns.Add("equipo");
                    vDatosFiltrados.Columns.Add("serie");
                    vDatosFiltrados.Columns.Add("inventario");

                    foreach (DataRow item in filtered)
                    {
                        vDatosFiltrados.Rows.Add(
                            item["identidad"].ToString(),
                            item["id"].ToString(),
                            item["equipo"].ToString(),
                            item["serie"].ToString(),
                            item["inventario"].ToString()
                            );
                    }
                    GvMasInfo.DataSource = vDatosFiltrados;
                    GvMasInfo.DataBind();
                    UpdatePanel5.Update();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "abrirModalMasInfo();", true);
                }
                else if (e.CommandName == "eliminar")
                {
                    //GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    //int RowIndex = oItem.RowIndex;
                    //GvVisitas.DeleteRow(RowIndex);
                    //GvVisitas.DataBind();
                  
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GvVisitas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GvVisitas_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void ddlPersonalInterno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                String vQuery = "RSP_ActivosDC 3,'" + ddlPersonalInterno.SelectedValue + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                txtIdentidadInterno.Text = vDatos.Rows[0]["identidad"].ToString();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

       

        protected void lbAddPersonalInterno_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable vData = new DataTable();
                DataTable vDatos = (DataTable)Session["ACTIVO_DC_PERSONAL_INTERNO"];
                vData.Columns.Add("codigoEmpleado");
                vData.Columns.Add("nombreEmpleado");
                vData.Columns.Add("identidadInterno");

                if (vDatos == null)
                    vDatos = vData.Clone();

                if (vDatos != null)
                {
                    if (vDatos.Rows.Count < 1)
                    {
                        vDatos.Rows.Add( ddlPersonalInterno.SelectedValue, ddlPersonalInterno.SelectedItem, txtIdentidadInterno.Text);
                    }
                    else
                    {
                        Boolean vRegistered = false;
                        //for (int i = 0; i < vDatos.Rows.Count; i++)
                        //{
                        //    if (vNombreMaterialMatriz == vDatos.Rows[i]["nombre"].ToString())
                        //    {
                        //        //vDatos.Rows[i]["cantidad"] = Convert.ToDecimal(vDatos.Rows[i]["cantidad"].ToString()) + Convert.ToDecimal(TxCantidad.Text);

                        //        lbCantidad.Text = "El material seleccionado: " + vNombreMaterialMatriz + " ya esta agregado en la lista, favor verificar";
                        //        DivAlertaCantidad.Visible = true;
                        //        UpCantidadMaxima.Update();

                        //        vRegistered = true;
                        //    }
                        //}

                        if (!vRegistered)
                            vDatos.Rows.Add(ddlPersonalInterno.SelectedValue, ddlPersonalInterno.SelectedItem, txtIdentidadInterno.Text);

                    }
                }

                GvPersonalInterno.DataSource = vDatos;
                GvPersonalInterno.DataBind();
                Session["ACTIVO_DC_PERSONAL_INTERNO"] = vDatos;
                UpdatePanel9.Update();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        protected void DdlSupervisar_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                String vQuery = "RSP_ActivosDC 3,'" + DdlSupervisar.SelectedValue + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                TxIdentidadSupervisar.Text = vDatos.Rows[0]["identidad"].ToString();
                Session["ACTIVO_DC_EMAIL_SUPERVISOR"] = vDatos.Rows[0]["emailEmpresa"].ToString();

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        protected void BtnCrearSolicitud_Click(object sender, EventArgs e)
        {
            try
            {
                String vFormato = "dd/MM/yyyy HH:mm"; //"dd/MM/yyyy HH:mm:ss"
                string vFechaInicio = Convert.ToDateTime(TxInicio.Text).ToString(vFormato);
                string vFechaFin = Convert.ToDateTime(TxFin.Text).ToString(vFormato);
                string vInterno = "";
                if (chInterno.Items[0].Selected)
                    vInterno = "1";
                else
                    vInterno = "0";

                string vExterno = "";
                if (chExterno.Items[0].Selected)
                    vExterno = "2";
                else
                    vExterno = "0";

                String vQuery = "RSP_ActivosDC 7,'" 
                               + Session["USUARIO"].ToString()
                               + "','"+ DdlNombreCopia.SelectedValue
                               + "','" + DdlSupervisar.SelectedValue
                               + "','" + vFechaInicio
                               + "','" + vFechaFin
                               + "','" + rbAcceso.SelectedValue
                               + "','" + rbTipoTarea.SelectedValue
                               + "','" + vExterno
                               + "','" + vInterno
                               + "','" + txPeticion.Text
                               + "','" + txTrabajo.Text
                               + "','" + TxMotivo.Text
                               + "','" + txTareasRealizar.Text
                               + "','" + Session["ACTIVO_DC_ID_ESTADO"].ToString()+"','"+ Session["ACTIVO_DC_CODIGO_JEFE"].ToString()+"'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                string vidSolicitud = vDatos.Rows[0]["idSolicitud"].ToString();


                DataTable vDatosPI = (DataTable)Session["ACTIVO_DC_PERSONAL_INTERNO"];
                if (vDatosPI != null)
                {
                    for (int num = 0; num < vDatosPI.Rows.Count; num++)
                    {
                        string vCodigoEmpleado = vDatosPI.Rows[num]["codigoEmpleado"].ToString();
                        vQuery = "RSP_ActivosDC 8,'" + vidSolicitud + "','" + vCodigoEmpleado + "'";
                        Int32 vRespuesta = vConexion.ejecutarSql(vQuery);
                    }
                }


                DataTable vDatosPE = (DataTable)Session["ACTIVO_DC_PERSONAL_EXTERNO"];
                if (vDatosPE != null)
                {
                    for (int num = 0; num < vDatosPE.Rows.Count; num++)
                    {
                        string vNombre = vDatosPE.Rows[num]["nombre"].ToString();
                        string vIdentidad = vDatosPE.Rows[num]["identidad"].ToString();
                        string vEmpresa = vDatosPE.Rows[num]["empresa"].ToString();
                        string vIngresoEquipo = vDatosPE.Rows[num]["ingresoEquipo"].ToString();
                        string vPermisoCelular = vDatosPE.Rows[num]["permisoCel"].ToString();
                        vQuery = "RSP_ActivosDC 9,'" + vidSolicitud + "','" + vNombre + "','" + vIdentidad + "','" + vEmpresa + "','" + vPermisoCelular + "','" + vIngresoEquipo + "'";
                        Int32 vRespuesta = vConexion.ejecutarSql(vQuery);
                    }
                }


                DataTable vDatosPEActivos = (DataTable)Session["ACTIVO_DC_EQUIPO_REGISTRADO"];
                if (vDatosPEActivos != null)
                {
                    for (int num = 0; num < vDatosPEActivos.Rows.Count; num++)
                    {
                        string vIdentidad = vDatosPEActivos.Rows[num]["identidad"].ToString();
                        string vEquipo = vDatosPEActivos.Rows[num]["equipo"].ToString();
                        string vSerie = vDatosPEActivos.Rows[num]["serie"].ToString();
                        string vInventario = vDatosPEActivos.Rows[num]["inventario"].ToString();
                        vQuery = "RSP_ActivosDC 10,'" + vidSolicitud + "','" + vIdentidad + "','" + vEquipo + "','" + vSerie + "','" + vInventario + "'";
                        Int32 vRespuesta = vConexion.ejecutarSql(vQuery);
                    }
                }

               string vEstado= Session["ACTIVO_DC_ID_ESTADO"].ToString();
                string vCopia = "";
                string vPara = "";
                if (vEstado=="1")
                {
                    //vPara = Session["ACTIVO_DC_EMAIL_JEFE_RESPONSABLE"].ToString();
                    vPara = "acamador@bancatlan.hn";
                } else
                {
                    if (rbAcceso.SelectedValue=="1" || rbAcceso.SelectedValue == "2"){
                        vPara ="acamador@bancatlan.hn";
                    } else {
                        vPara = "acamador@bancatlan.hn";
                    }
                }
                vCopia = vCopia + Session["ACTIVO_DC_EMAIL_RESPONSABLE"].ToString() + ";" + Session["ACTIVO_DC_EMAIL_COPIA"].ToString() + ";" + Session["ACTIVO_DC_EMAIL_SUPERVISOR"].ToString();

                vQuery = "RSP_ActivosDC 11,'"
                + vPara
                + "','" + vCopia
                + "','" + "Aprobación Solicitud Acceso Data Center"
                + "','" + "Favor con la respectiva aprobación"
                + "','" + "0"
                + "','" + vidSolicitud
                + "'";
                vDatos = vConexion.obtenerDataTable(vQuery);

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void TxPermisoExtendido_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Session["ACTIVO_DC_ID_ESTADO"] = "2";
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }



        void camposDeshabilitados()
        {
            TxResponsable.ReadOnly = true;
            TxIdentidadResponsable.ReadOnly = true;
            TxSubgerencia.ReadOnly = true;
            TxJefe.ReadOnly = true;
            DdlNombreCopia.Enabled = false;
            DdlNombreCopia.CssClass = "form-control";
            TxIdentidadCopia.ReadOnly = true;

            DdlSupervisar.Enabled = false;
            DdlSupervisar.CssClass = "form-control";

            TxIdentidadSupervisar.ReadOnly = true;
            DivPermisoExtendido.Visible = false;

            TxInicio.ReadOnly = true;
            TxFin.ReadOnly = true;
            rbAcceso.Enabled = false;
            rbTipoTarea.Enabled = false;
            chInterno.Enabled = false;
            chExterno.Enabled = false;

            txPeticion.ReadOnly = true;
            txTrabajo.ReadOnly = true;
            TxMotivo.ReadOnly = true;
            txTareasRealizar.ReadOnly = true;

        }


        void cargarDataVista()
        {
            try
            {
                DataTable vDatosGenerales = new DataTable();
                vDatosGenerales = (DataTable)Session["ACTIVO_DC_SOLI_DATOS_GENERALES"];
                string vFormato = "yyyy-MM-ddTHH:mm";

                string vIdSolicitud = vDatosGenerales.Rows[0]["idSolicitud"].ToString();
                Session["ACTIVO_DC_ID_SOLICITUD"] = vIdSolicitud;

                string vFechaInicio = vDatosGenerales.Rows[0]["fechaInicio"].ToString();
                string vFechaInicioConvertida = Convert.ToDateTime(vFechaInicio).ToString(vFormato);
                TxInicio.Text = vFechaInicioConvertida;

                string vFechaFin = vDatosGenerales.Rows[0]["fechaFin"].ToString();
                string vFechaFinConvertida = Convert.ToDateTime(vFechaFin).ToString(vFormato);
                TxFin.Text= vFechaFinConvertida;
                 
                rbAcceso.SelectedValue = vDatosGenerales.Rows[0]["acceso"].ToString();
                rbTipoTarea.SelectedValue = vDatosGenerales.Rows[0]["trabajo"].ToString();
                string vExterno= vDatosGenerales.Rows[0]["personalExterno"].ToString();
                string vInterno = vDatosGenerales.Rows[0]["personalInterno"].ToString();
                chInterno.SelectedValue = vInterno;
                chExterno.SelectedValue  = vExterno;
                txPeticion.Text = vDatosGenerales.Rows[0]["peticion"].ToString();
                txTrabajo.Text = vDatosGenerales.Rows[0]["trabajo"].ToString();
                TxMotivo.Text = vDatosGenerales.Rows[0]["motivo"].ToString();
                txTareasRealizar.Text = vDatosGenerales.Rows[0]["tareasRealizar"].ToString();

                DataTable vDatosResponsable = new DataTable();
                vDatosResponsable = (DataTable)Session["ACTIVO_DC_SOLI_DATOS_RESPONSABLE"];
                TxResponsable.Text = vDatosResponsable.Rows[0]["nombre"].ToString();
                TxIdentidadResponsable.Text = vDatosResponsable.Rows[0]["identidad"].ToString();
                TxSubgerencia.Text = vDatosResponsable.Rows[0]["area"].ToString();
                TxJefe.Text = vDatosResponsable.Rows[0]["jefeNombre"].ToString();

                DataTable vDatosCopia = new DataTable();
                vDatosCopia = (DataTable)Session["ACTIVO_DC_SOLI_DATOS_COPIA"];
                DdlNombreCopia.SelectedValue = vDatosCopia.Rows[0]["idEmpleado"].ToString();
                TxIdentidadCopia.Text = vDatosCopia.Rows[0]["identidad"].ToString();

                DataTable vDatosSupervisor = new DataTable();
                vDatosSupervisor = (DataTable)Session["ACTIVO_DC_SOLI_DATOS_SUPERVISOR"];
                DdlSupervisar.SelectedValue = vDatosSupervisor.Rows[0]["idEmpleado"].ToString();
                TxIdentidadSupervisar.Text = vDatosSupervisor.Rows[0]["identidad"].ToString();


                DivGvExternoLectura.Visible = true;
                DivExternoNoLectura.Visible = false;
                DataTable vDatosPersonalExterno = new DataTable();
                vDatosPersonalExterno = (DataTable)Session["ACTIVO_DC_SOLI_PERSONAL_EXTERNO"];
                GvExternoLectura.DataSource = vDatosPersonalExterno;
                GvExternoLectura.DataBind();
                UpdatePanel9.Update();


                DivGvInternoLectura.Visible = true;
                DivGvInternoNoLectura.Visible = false;
                DataTable vDatosPersonalInterno = new DataTable();
                vDatosPersonalInterno = (DataTable)Session["ACTIVO_DC_SOLI_PERSONAL_INTERNO"];
                GvInternoLectura.DataSource = vDatosPersonalInterno;
                GvInternoLectura.DataBind();
                UpdatePanel9.Update();


            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }

        }

        protected void BtnAprobar_Click(object sender, EventArgs e)
        {
            try
            {

                TituloAprobacionJefe.Text = "Solicitud número " + Session["ACTIVO_DC_ID_SOLICITUD"].ToString();

                String vFI = TxInicio.Text != "" ? TxInicio.Text : "1999-01-01 00:00:00";
                String vFF = TxFin.Text != "" ? TxFin.Text : "1999-01-01 00:00:00";

                DateTime desde = Convert.ToDateTime(vFI);
                DateTime hasta = Convert.ToDateTime(vFF);

                LbAprobarJefe.Text = "Buen dia <b> " + TxJefe .Text+ "</b><br /><br />" +
                     "Fechas inicio <b>" + desde.ToString("yyyy-MM-dd HH:mm") + "</b> al <b>" + hasta.ToString("yyyy-MM-dd HH:mm") + "</b> <br />" +
                     "Trabajo a realizar: <b>" + txTrabajo.Text + "</b><br /><br />";
                LbAprobarJefePregunta.Text = "<b>¿Está seguro que desea " + Session["ACTIVO_DC_ESTADO_JEFE"].ToString() + "<b> ,en el rango de fechas y horas detalladas?</b>";
                UpdateAprobarJefe.Update();
                UpTituloAprobarJefeModal.Update();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openAprobarJefeModal();", true);


            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void DdlAccionJefe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DdlAccionJefe.SelectedValue == "1") {
                TxMotivoJefe.Visible = false;

                if (rbAcceso.SelectedValue=="1" || rbAcceso.SelectedValue == "2")
                {
                    Session["ACTIVO_DC_ESTADO_JEFE"] = "Aprobar la solicitud; Nota:Se actualizará al estado Pendiente Aprobar Responsable DataCenter";
                    Session["ACTIVO_DC_ESTADO_JEFE_ID"] = "3";
                    Session["ACTIVO_DC_ID_EMPLEADO_RESPONSABLE"] = "315";
                }
                else
                {
                    Session["ACTIVO_DC_ESTADO_JEFE"] = "Aprobar la solicitud;Nota:Se actualizará al estado Pendiente Aprobar Responsable Cableado";
                    Session["ACTIVO_DC_ESTADO_JEFE_ID"] = "2";
                    Session["ACTIVO_DC_ID_EMPLEADO_RESPONSABLE"] = "3790";
                }
            }
            else
            {
                TxMotivoJefe.Visible = true;
                Session["ACTIVO_DC_ESTADO_JEFE"] = "Cancelar la solicitud";
                Session["ACTIVO_DC_ESTADO_JEFE_ID"] = "5";
            }
               
        }

        protected void BtnAprobarJefeModal_Click(object sender, EventArgs e)
        {
            try
            {
                String vQuery = "RSP_ActivosDC 17,'" + Session["ACTIVO_DC_ID_SOLICITUD"].ToString() + "','"+ DdlAccionJefe.SelectedValue+"','"+ TxMotivoJefe.Text+"','"+ Session["ACTIVO_DC_ID_EMPLEADO_RESPONSABLE"].ToString()+"','"+ Session["ACTIVO_DC_ESTADO_JEFE_ID"].ToString()+"'";
                Int32 vRespuesta = vConexion.ejecutarSql(vQuery);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeAprobarJefeModal();", true);

                Response.Redirect("/pages/activos/visitaDatacenterPendienteJefe.aspx?ex=1");
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
    }

            





}