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
                CargarSolicitudesIngresadas();
                //Session["ACTIVO_DC_ID_ESTADO"] = "1";

                if (string.IsNullOrEmpty(vEx))
                {
                    CargarInformacionGeneral();
                    Session["ACTIVO_DC_ID_ESTADO"] = "1";
                    
                }
                else if (vEx.Equals("1"))
                {
                    //CargarInformacionGeneral();
                    camposDeshabilitados();
                    cargarDataVista();
                    divPersonalExterno.Visible = false;
                    divPersonalInterno.Visible = false;
                    DivCrearSolicitud.Visible = false;
                    DivAprobarSolicitudJefe.Visible = true;
                    
                }
                else if (vEx.Equals("2"))
                {
                    //CargarInformacionGeneral();
                    camposDeshabilitados();
                    cargarDataVista();
                    divPersonalExterno.Visible = false;
                    divPersonalInterno.Visible = false;
                    DivCrearSolicitud.Visible = false;
                    DivAprobarSolicitudJefe.Visible = false;
                    DivAprobacionGestor.Visible = true;
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

                if (dias!=0 && dias!=1)
                    throw new Exception("Deberá ingresar la solicitud de forma diaria.");
            }
        }

        void validarAgregarDatagrid()
        {
            if (TxNombreVisita.Text.Equals(""))
                throw new Exception("Ingrese nombre del visitante externo.");

            if (TxIdentidadVisita.Text.Equals(""))
                throw new Exception("Ingrese identidad del visitante externo.");

            if (TxIdentidadVisita.Text.Equals(""))
                throw new Exception("Ingrese identidad del visitante externo.");

            if (DdlEmpresaVisita.SelectedValue.Equals("0"))
                throw new Exception("Favor seleccione la empresa del cual es el visitante.");

            if (RbIngresoEquipoVisita.SelectedValue.Equals(""))
                throw new Exception("Favor seleccione si ingresará equipo.");

            if (RbPermisoCelular.SelectedValue.Equals(""))
                throw new Exception("Favor seleccione si tendra permiso de portar el celular.");
        }
        void agregarRowDatagrid()
        {
            validarAgregarDatagrid();
            DataTable vData = new DataTable();
            DataTable vDatos = (DataTable)Session["ACTIVO_DC_PERSONAL_EXTERNO"];

            vData.Columns.Add("nombre");
            vData.Columns.Add("identidad");
            vData.Columns.Add("empresa");
            vData.Columns.Add("ingresoEquipo");
            vData.Columns.Add("permisoCel");
            vData.Columns.Add("permisoCel_tabla");
            vData.Columns.Add("ingresoEquipo_tabla");
            vData.Columns.Add("empresa_tabla");

            if (vDatos == null)
                vDatos = vData.Clone();
            if (vDatos != null)
            {
                if (vDatos.Rows.Count < 1)
                {
                    vDatos.Rows.Add(TxNombreVisita.Text, TxIdentidadVisita.Text, DdlEmpresaVisita.SelectedItem, RbIngresoEquipoVisita.SelectedItem, RbPermisoCelular.SelectedItem, RbPermisoCelular.SelectedValue, RbIngresoEquipoVisita.SelectedValue, DdlEmpresaVisita.SelectedValue);

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
                        vDatos.Rows.Add(TxNombreVisita.Text, TxIdentidadVisita.Text, DdlEmpresaVisita.SelectedItem, RbIngresoEquipoVisita.SelectedItem, RbPermisoCelular.SelectedItem, RbPermisoCelular.SelectedValue, RbIngresoEquipoVisita.SelectedValue, DdlEmpresaVisita.SelectedValue);
                }
            }
            GvVisitas.DataSource = vDatos;
            GvVisitas.DataBind();
            Session["ACTIVO_DC_PERSONAL_EXTERNO"] = vDatos;
            UPVisitas.Update();

            TxNombreVisita.Text = String.Empty;
            TxIdentidadVisita.Text = String.Empty;
            DdlEmpresaVisita.SelectedIndex = -1;
            RbIngresoEquipoVisita.SelectedIndex = -1;
            RbPermisoCelular.SelectedIndex = -1;
            UpdatePanel15.Update();
        }
        protected void TxInicio_TextChanged(object sender, EventArgs e)
        {
            try
            {
             calculoHoras();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        protected void TxFin_TextChanged(object sender, EventArgs e)
        {
            try
            {
                calculoHoras();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
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
                vData.Columns.Add("equipo_tabla");

                if (vDatos == null )
                    vDatos = vData.Clone();

                if (vDatos != null)
                {
                    if (vDatos.Rows.Count < 1)
                    {
                        vDatos.Rows.Add("1", DdlEquipo.SelectedItem, TxSerie.Text, TxInventario.Text, TxIdentidadVisita.Text, DdlEquipo.SelectedValue);
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
                            vDatos.Rows.Add((vDatos.Rows.Count)+1, DdlEquipo.SelectedItem, TxSerie.Text, TxInventario.Text, TxIdentidadVisita.Text, DdlEquipo.SelectedValue);

                        TxSerie.Text = String.Empty;
                        TxInventario.Text = String.Empty;
                        DdlEquipo.SelectedIndex = -1;
                        UpdatePanel7.Update();


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
                TxCorreoInterno.Text = vDatos.Rows[0]["emailEmpresa"].ToString();
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
                vData.Columns.Add("correoInterno");

                if (vDatos == null)
                    vDatos = vData.Clone();

                if (vDatos != null)
                {
                    if (vDatos.Rows.Count < 1)
                    {
                        vDatos.Rows.Add( ddlPersonalInterno.SelectedValue, ddlPersonalInterno.SelectedItem, txtIdentidadInterno.Text, TxCorreoInterno.Text);
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
                            vDatos.Rows.Add(ddlPersonalInterno.SelectedValue, ddlPersonalInterno.SelectedItem, txtIdentidadInterno.Text, TxCorreoInterno.Text);


                    }
                }

                GvPersonalInterno.DataSource = vDatos;
                GvPersonalInterno.DataBind();
                Session["ACTIVO_DC_PERSONAL_INTERNO"] = vDatos;
                txtIdentidadInterno.Text = string.Empty;
                TxCorreoInterno.Text = string.Empty;
                ddlPersonalInterno.SelectedIndex = -1;
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
                TxFechaInicioModal.Text = TxInicio.Text;
                TxFechaFinModal.Text = TxFin.Text;
                TxTareaModal.Text = txTrabajo.Text;
                TxMotivoModal.Text = TxMotivo.Text;
                lbTitulo.Text = "Crear solicitud, visita al: "+ rbAcceso.SelectedItem;
                UpdatePanel17.Update();
                UpdatePanel16.Update();

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openEnviarSolicitudModal();", true);





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
                string vQuery = "RSP_ActivosDC 6" ;
                DataTable vDatosCopiaVista = vConexion.obtenerDataTable(vQuery);
                if (vDatosCopiaVista.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosCopiaVista.Rows)
                    {
                        DdlNombreCopia.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
                        DdlSupervisar.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
                    }
                }

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



                chInterno.SelectedValue = vInterno;
                chExterno.SelectedValue = vExterno;

                if (chInterno.SelectedValue=="1")
                {
                    DivGvInterLectura.Visible = true;
                    DivGvInternoNoLectura.Visible = false;
                    DataTable vDatosPersonalInterno = new DataTable();
                    vDatosPersonalInterno = (DataTable)Session["ACTIVO_DC_SOLI_PERSONAL_INTERNO"];
                    GvInternoLectura.DataSource = vDatosPersonalInterno;
                    GvInternoLectura.DataBind();
                }
                else
                {
                    DivGvInterLectura.Visible = false;
                }



                if (chExterno.SelectedValue == "2")
                {
                    DivGvExternoLectura.Visible = true;
                    DataTable vDatosPersonalExterno = new DataTable();
                    vDatosPersonalExterno = (DataTable)Session["ACTIVO_DC_SOLI_PERSONAL_EXTERNO"];
                    GvExternoLectura.DataSource = vDatosPersonalExterno;
                    GvExternoLectura.DataBind();
                    UpdatePanel9.Update();
                }
                else
                {
                    DivGvExternoLectura.Visible = false;
                }


                DivParticipantesVista.Visible = true;              
                UpdatePanel20.Update();

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }

        }


        void validarAprobacionJefe()
        {
            if (DdlAccionJefe.SelectedValue == "0")
                throw new Exception("Falta que seleccione acción a realizar");

            if (DdlAccionJefe.SelectedValue=="2" && TxMotivoJefe.Text == string.Empty)
                throw new Exception("Falta que ingrese el motivo de cancelacion de la solicitud");
        }

        void limpiarAprobacionJefe()
        {
            DdlAccionJefe.SelectedIndex = -1;
            TxMotivoJefe.Text = string.Empty;               
        }
        protected void BtnAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                validarAprobacionJefe();
                TituloAprobacionJefe.Text = "Solicitud número " + Session["ACTIVO_DC_ID_SOLICITUD"].ToString();

                String vFI = TxInicio.Text != "" ? TxInicio.Text : "1999-01-01 00:00:00";
                String vFF = TxFin.Text != "" ? TxFin.Text : "1999-01-01 00:00:00";

                DateTime desde = Convert.ToDateTime(vFI);
                DateTime hasta = Convert.ToDateTime(vFF);

                LbAprobarJefe.Text = "Buen dia <b> " + TxJefe .Text+ "</b><br /><br />" +
                     "Fechas inicio solicitud <b>" + desde.ToString("yyyy-MM-dd HH:mm") + "</b> al <b>" + hasta.ToString("yyyy-MM-dd HH:mm") + "</b> <br />" +
                     "Trabajo a realizar: <b>" + txTrabajo.Text + "</b><br /><br />";
                LbAprobarJefePregunta.Text = "<b>¿Está seguro que desea " + Session["ACTIVO_DC_ESTADO_JEFE"].ToString() + "<b> ,en el rango de fechas y horas detalladas?</b>";
                UpdateAprobarJefe.Update();
                UpTituloAprobarJefeModal.Update();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openAprobarJefeModal();", true);
                limpiarAprobacionJefe();

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

                vQuery = "RSP_ActivosDC 21,'" + Session["ACTIVO_DC_ID_SOLICITUD"].ToString() + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                string vCopia = vDatos.Rows[0]["para"].ToString();
                string vPara = vDatos.Rows[0]["copia"].ToString();

                vQuery = "RSP_ActivosDC 11,'"
                + vPara
                + "','" + vCopia
                + "','" + "Estado Aprobación Solicitud Jefe Inmediato"
                + "','" + "Favor con la respectiva aprobación"
                + "','" + "0"
                + "','" + Session["ACTIVO_DC_ID_SOLICITUD"].ToString()
                + "'";
                Int32 vInfo = vConexion.ejecutarSql(vQuery);

                Response.Redirect("/pages/activos/visitaDatacenterPendienteJefe.aspx?ex=1");
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }


        
        protected void chInterno_SelectedIndexChanged(object sender, EventArgs e)
        {
           if (chInterno.SelectedValue == "1")
            {
                tabInterno.Visible = true;
                DivParticipantes.Visible = true;
                UpdatePanel1.Update();
            }
            else
            {
                tabInterno.Visible = false;
                DivParticipantes.Visible = true;
                UpdatePanel1.Update();
            }       
        }

        protected void chExterno_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chExterno.SelectedValue == "2")
            {   tabExterno.Visible = true;
                DivParticipantes.Visible = true;
                UpdatePanel1.Update();               
            }
            else
            {
                tabExterno.Visible = false;
                DivParticipantes.Visible = true;
                UpdatePanel1.Update();
            }
        }

        protected void BtnEnviarSoli_Click(object sender, EventArgs e)
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
                               + "','" + DdlNombreCopia.SelectedValue
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
                               + "','" + Session["ACTIVO_DC_ID_ESTADO"].ToString() + "','" + Session["ACTIVO_DC_CODIGO_JEFE"].ToString() + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                string vidSolicitud = vDatos.Rows[0]["idSolicitud"].ToString();


                string vCorreosInternos = "";
                DataTable vDatosPI = (DataTable)Session["ACTIVO_DC_PERSONAL_INTERNO"];
                if (vDatosPI != null)
                {
                    for (int num = 0; num < vDatosPI.Rows.Count; num++)
                    {
                        string vCodigoEmpleado = vDatosPI.Rows[num]["codigoEmpleado"].ToString();
                        vQuery = "RSP_ActivosDC 8,'" + vidSolicitud + "','" + vCodigoEmpleado + "'";
                        Int32 vRespuesta = vConexion.ejecutarSql(vQuery);
                        vCorreosInternos = vCorreosInternos + ";"+ vDatosPI.Rows[num]["correoInterno"].ToString();
                    }
                }


                DataTable vDatosPE = (DataTable)Session["ACTIVO_DC_PERSONAL_EXTERNO"];
                if (vDatosPE != null)
                {
                    for (int num = 0; num < vDatosPE.Rows.Count; num++)
                    {
                        string vNombre = vDatosPE.Rows[num]["nombre"].ToString();
                        string vIdentidad = vDatosPE.Rows[num]["identidad"].ToString();
                        string vEmpresa = vDatosPE.Rows[num]["empresa_tabla"].ToString();
                        string vIngresoEquipo = vDatosPE.Rows[num]["ingresoEquipo_tabla"].ToString();
                        string vPermisoCelular = vDatosPE.Rows[num]["permisoCel_tabla"].ToString();
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
                        string vEquipo = vDatosPEActivos.Rows[num]["equipo_tabla"].ToString();
                        string vSerie = vDatosPEActivos.Rows[num]["serie"].ToString();
                        string vInventario = vDatosPEActivos.Rows[num]["inventario"].ToString();
                        vQuery = "RSP_ActivosDC 10,'" + vidSolicitud + "','" + vIdentidad + "','" + vEquipo + "','" + vSerie + "','" + vInventario + "'";
                        Int32 vRespuesta = vConexion.ejecutarSql(vQuery);
                    }
                }

                string vEstado = Session["ACTIVO_DC_ID_ESTADO"].ToString();
                string vCopia = "";
                string vPara = "";
                if (vEstado == "1")
                {
                    //vPara = Session["ACTIVO_DC_EMAIL_JEFE_RESPONSABLE"].ToString();
                    vPara = "acamador@bancatlan.hn";
                }
                else
                {
                    if (rbAcceso.SelectedValue == "1" || rbAcceso.SelectedValue == "2")
                    {
                        vPara = "acamador@bancatlan.hn";
                    }
                    else
                    {
                        vPara = "acamador@bancatlan.hn";
                    }
                }


                
                vCopia = vCopia + Session["ACTIVO_DC_EMAIL_RESPONSABLE"].ToString() + ";" + Session["ACTIVO_DC_EMAIL_COPIA"].ToString() + ";" + Session["ACTIVO_DC_EMAIL_SUPERVISOR"].ToString()+";"+ vCorreosInternos; ;

                vQuery = "RSP_ActivosDC 11,'"
                + vPara
                + "','" + vCopia
                + "','" + "Aprobación Solicitud Acceso Data Center"
                + "','" + "Favor con la respectiva aprobación"
                + "','" + "0"
                + "','" + vidSolicitud
                + "'";
                //vDatos = vConexion.obtenerDataTable(vQuery);
                Int32 vInfo = vConexion.ejecutarSql(vQuery);

                if (vInfo == 1)
                {
                    Mensaje("Solicitud enviada con éxito.", WarningType.Success);
                               
                }
                else
                {
                    Mensaje("No se pudo enviar la solicitud, favor contactarse con el administrador del sistema", WarningType.Danger);
                }

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeEnviarSolicitudModal();", true);
                limpiarSolicitud();
                CargarInformacionGeneral();
                UpdatePanel2.Update();

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }


        private void limpiarSolicitud()
        {
            DivParticipantes.Visible = false;
            TxResponsable.Text = String.Empty;
            TxIdentidadResponsable.Text = String.Empty;
            TxSubgerencia.Text = String.Empty;
            TxJefe.Text = String.Empty;
            DdlNombreCopia.SelectedIndex = -1;
            TxIdentidadCopia.Text = String.Empty;
            DdlSupervisar.SelectedIndex = -1;
            TxIdentidadSupervisar.Text = String.Empty;
            TxPermisoExtendido.Text = String.Empty;
            TxInicio.Text = String.Empty;
            TxFin.Text = String.Empty;
            rbAcceso.SelectedIndex = -1;
            rbTipoTarea.SelectedIndex = -1;
            chInterno.Items[0].Selected = false;
            chExterno.Items[0].Selected = false;

            txPeticion.Text = String.Empty;
            txTrabajo.Text = String.Empty;
            TxMotivo.Text = String.Empty;
            txTareasRealizar.Text = String.Empty;

            GvPersonalInterno.DataSource = null;
            GvPersonalInterno.DataBind();

            GvVisitas.DataSource = null;
            GvVisitas.DataBind();
            

            //Session.Clear();

            UpdatePanel2.Update();
            UpdatePanel1.Update();
            UpdatePanel15.Update();
            UpdatePanel9.Update();
        }

        protected void BtnCancelarSolicitud_Click(object sender, EventArgs e)
        {
            try
            {
                Mensaje("Solicitud cancelada con éxito.", WarningType.Success);
                limpiarSolicitud();
                CargarInformacionGeneral();
                UpdatePanel2.Update();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }


        void CargarSolicitudesIngresadas()
        {
            try
            {
                DataTable vDatos = new DataTable();
                String vQuery = "RSP_ActivosDC 19,'" + Convert.ToString(Session["USUARIO"]) + "'";
                vDatos = vConexion.obtenerDataTable(vQuery);

                if (vDatos.Rows.Count > 0)
                {
                    GVSolicitudes.DataSource = vDatos;
                    GVSolicitudes.DataBind();
                    UpBusquedaSolicitudes.Update();
                    Session["ACTIVO_DC_SOLICITUDES_INGRESADAS"] = vDatos;
                }

            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void TxBuscarSolicitud_TextChanged(object sender, EventArgs e)
        {
            CargarSolicitudesIngresadas();
            String vBusqueda = TxBuscarSolicitud.Text;
            DataTable vDatos = (DataTable)Session["ACTIVO_DC_SOLICITUDES_INGRESADAS"];

            if (vBusqueda.Equals(""))
            {
                GVSolicitudes.DataSource = vDatos;
                GVSolicitudes.DataBind();
                UpBusquedaSolicitudes.Update();
            }
            else
            {
                EnumerableRowCollection<DataRow> filtered = vDatos.AsEnumerable()
                    .Where(r => r.Field<String>("supervisorProveedor").Contains(vBusqueda.ToUpper()));

                Boolean isNumeric = int.TryParse(vBusqueda, out int n);
                if (isNumeric)
                {
                    if (filtered.Count() == 0)
                    {
                        filtered = vDatos.AsEnumerable().Where(r =>
                            Convert.ToInt32(r["idSolicitud"]) == Convert.ToInt32(vBusqueda));
                    }
                }

                DataTable vDatosFiltrados = new DataTable();
                vDatosFiltrados.Columns.Add("idSolicitud");
                vDatosFiltrados.Columns.Add("fechaInicio");
                vDatosFiltrados.Columns.Add("fechaFin");
                vDatosFiltrados.Columns.Add("acceso");
                vDatosFiltrados.Columns.Add("peticion");
                vDatosFiltrados.Columns.Add("trabajo");
                vDatosFiltrados.Columns.Add("motivo");
                vDatosFiltrados.Columns.Add("tareasRealizar");
                vDatosFiltrados.Columns.Add("ingreso");
                vDatosFiltrados.Columns.Add("copia");
                vDatosFiltrados.Columns.Add("supervisorProveedor");
                vDatosFiltrados.Columns.Add("estado");

                foreach (DataRow item in filtered)
                {
                    vDatosFiltrados.Rows.Add(
                        item["idSolicitud"].ToString(),
                        item["fechaInicio"].ToString(),
                        item["fechaFin"].ToString(),
                        item["acceso"].ToString(),
                        item["peticion"].ToString(),
                        item["trabajo"].ToString(),
                        item["motivo"].ToString(),
                        item["tareasRealizar"].ToString(),
                        item["ingreso"].ToString(),
                        item["copia"].ToString(),
                        item["supervisorProveedor"].ToString(),
                        item["estado"].ToString()
                        );
                }
                GVSolicitudes.DataSource = vDatosFiltrados;
                GVSolicitudes.DataBind();
                Session["ACTIVO_DC_SOLICITUDES_INGRESADAS"] = vDatosFiltrados;
                UpBusquedaSolicitudes.Update();
            }
        }

        protected void GVSolicitudes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                limpiarAprobacionJefe();
                Response.Redirect("/pages/activos/visitaDatacenterPendienteJefe.aspx?ex=1");
            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void BtnAprobarGestor_Click(object sender, EventArgs e)
        {
            try
            {
                validarAprobacionGestor();
                TituloAprobacionGestor.Text = "Solicitud número " + Session["ACTIVO_DC_ID_SOLICITUD"].ToString();

                String vFI = TxInicio.Text != "" ? TxInicio.Text : "1999-01-01 00:00:00";
                String vFF = TxFin.Text != "" ? TxFin.Text : "1999-01-01 00:00:00";

                DateTime desde = Convert.ToDateTime(vFI);
                DateTime hasta = Convert.ToDateTime(vFF);

                LbAprobarGestor.Text = "Buen dia <b> " + "</b><br /><br />"+
                     "Fechas inicio solicitud <b>" + desde.ToString("yyyy-MM-dd HH:mm") + "</b> al <b>" + hasta.ToString("yyyy-MM-dd HH:mm") + "</b> <br />" +
                     "Trabajo a realizar: <b>" + txTrabajo.Text + "</b><br /><br />";
                LbAprobarGestorPregunta.Text = "<b>¿Está seguro que desea " + Session["ACTIVO_DC_ESTADO_GESTOR"].ToString() + "<b> ,en el rango de fechas y horas detalladas?</b>";
                UpdatePanel24.Update();
                UpdatePanel23.Update();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openAprobarGestorModal();", true);
                limpiarAprobacionGestor();

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }

        }

        void validarAprobacionGestor()
        {
            if (DdlAccionGestor.SelectedValue == "0")
                throw new Exception("Falta que seleccione acción a realizar");

            if (DdlAccionGestor.SelectedValue == "2" && TxtMotivoGestor.Text == string.Empty)
                throw new Exception("Falta que ingrese el motivo de cancelacion de la solicitud");
        }

        void limpiarAprobacionGestor()
        {
            DdlAccionGestor.SelectedIndex = -1;
            TxtMotivoGestor.Text = string.Empty;
        }

        protected void DdlAccionGestor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DdlAccionGestor.SelectedValue == "1")
            {
                TxtMotivoGestor.Visible = false;
                if (rbAcceso.SelectedValue == "1" || rbAcceso.SelectedValue == "2")
                {
                    Session["ACTIVO_DC_ESTADO_GESTOR"] = "Aprobar la solicitud; Nota:Se autorizará la visita personal de Segurida realizará la respectiva verificación del equipo.";
                    Session["ACTIVO_DC_ESTADO_GESTOR_ID"] = "4";
                }
                else
                {
                    Session["ACTIVO_DC_ESTADO_GESTOR"] = "Aprobar la solicitud; Nota:Se autorizará la visita personal de Segurida realizará la respectiva verificación del equipo.";
                    Session["ACTIVO_DC_ESTADO_GESTOR_ID"] = "8";
                }
            }
            else
            {
                TxtMotivoGestor.Visible = true;
                if (rbAcceso.SelectedValue == "1" || rbAcceso.SelectedValue == "2")
                {
                    Session["ACTIVO_DC_ESTADO_GESTOR"] = "Cancelar la solicitud";
                    Session["ACTIVO_DC_ESTADO_GESTOR_ID"] = "6";
                }
                if (rbAcceso.SelectedValue == "1" || rbAcceso.SelectedValue == "2")
                {
                    Session["ACTIVO_DC_ESTADO_GESTOR"] = "Cancelar la solicitud";
                    Session["ACTIVO_DC_ESTADO_GESTOR_ID"] = "7";
                }

            }
        }

        protected void BtnEnviarGestor_Click(object sender, EventArgs e)
        {
            try
            {
                String vQuery = "RSP_ActivosDC 22,'" + Session["ACTIVO_DC_ID_SOLICITUD"].ToString() + "','" + Session["ACTIVO_DC_ESTADO_GESTOR_ID"].ToString() + "','" + TxtMotivoGestor.Text+"'";
                Int32 vRespuesta = vConexion.ejecutarSql(vQuery);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeAprobarGestorModal();", true);

                vQuery = "RSP_ActivosDC 23,'" + Session["ACTIVO_DC_ID_SOLICITUD"].ToString() + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                string vCopia = vDatos.Rows[0]["para"].ToString();
                string vPara = vDatos.Rows[0]["copia"].ToString();

                vQuery = "RSP_ActivosDC 11,'"
                + vPara
                + "','" + vCopia
                + "','" + "Estado Aprobación Solicitud Gestor"
                + "','" + "Favor con la respectiva aprobación"
                + "','" + "0"
                + "','" + Session["ACTIVO_DC_ID_SOLICITUD"].ToString()
                + "'";
                Int32 vInfo = vConexion.ejecutarSql(vQuery);
                Response.Redirect("/pages/activos/visitaDatacenterPendienteResponsable.aspx?ex=2");
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnCancelar_Click1(object sender, EventArgs e)
        {

        }
    }


    

}