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
        protected void Page_Load(object sender, EventArgs e){
            String vEx = Request.QueryString["ex"];
            if (!IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])) { 
                    CargarSolicitudesIngresadas();
                    if (string.IsNullOrEmpty(vEx)){
                        CargarInformacionGeneral();
                        Session["ACTIVO_DC_ID_ESTADO"] = "1";
                    }else if (vEx.Equals("1")){
                        camposDeshabilitados();
                        cargarDataVista();
                        divPersonalExterno.Visible = false;
                        divPersonalInterno.Visible = false;
                        DivCrearSolicitud.Visible = false;
                        DivAprobarSolicitudJefe.Visible = true;
                    
                    }else if (vEx.Equals("2")){
                        camposDeshabilitados();
                        cargarDataVista();
                        divPersonalExterno.Visible = false;
                        divPersonalInterno.Visible = false;
                        DivCrearSolicitud.Visible = false;
                        DivAprobarSolicitudJefe.Visible = false;
                        DivAprobacionGestor.Visible = true;
                    }
                }else
                    Response.Redirect("/login.aspx");
            }
        }
        
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        
        void CargarInformacionGeneral(){
            try{
                String vQuery = "RSP_ActivosDC 1,'" + Convert.ToString(Session["USUARIO"]) + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                LbResponsable.Text = vDatos.Rows[0]["nombre"].ToString();
                LbIdentidadResponsable.Text = vDatos.Rows[0]["identidad"].ToString();
                LbSubgerencia.Text = vDatos.Rows[0]["departamento"].ToString();
                LbJefe.Text = vDatos.Rows[0]["jefeNombre"].ToString();
                LbCorreo.Text = vDatos.Rows[0]["emailEmpresa"].ToString();
                LbIdJefe.Text = vDatos.Rows[0]["idJefe"].ToString();

                vQuery = "RSP_ActivosDC 2,'" + vDatos.Rows[0]["area"].ToString() + "'";
                DataTable vDatosCopia = vConexion.obtenerDataTable(vQuery);

                DdlNombreCopia.Items.Clear();
                DdlSupervisar.Items.Clear();
                DdlNombreCopia.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                DdlSupervisar.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                if (vDatosCopia.Rows.Count > 0){
                    foreach (DataRow item in vDatosCopia.Rows){
                        DdlNombreCopia.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
                        DdlSupervisar.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
                    }
                }

                vQuery = "RSP_ActivosDC 4";
                DataTable vDatosEmpresas = vConexion.obtenerDataTable(vQuery);
                DdlEmpresaVisita.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                if (vDatosEmpresas.Rows.Count > 0){
                    foreach (DataRow item in vDatosEmpresas.Rows){
                        DdlEmpresaVisita.Items.Add(new ListItem { Value = item["idEmpresa"].ToString(), Text = item["empresa"].ToString() });
                    }
                }

                ddlPersonalInterno.Items.Clear();
                vQuery = "RSP_ActivosDC 6";
                DataTable vDatosInterno = vConexion.obtenerDataTable(vQuery);
                ddlPersonalInterno.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                if (vDatosInterno.Rows.Count > 0){
                    foreach (DataRow item in vDatosInterno.Rows){
                        ddlPersonalInterno.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
                    }
                }
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
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
        
        void agregarRowDatagrid(){
            validarAgregarDatagrid();
            DataTable vData = new DataTable();
            if (Session["ACTIVO_DC_PERSONAL_EXTERNO"] == null){
                vData.Columns.Add("id");
                vData.Columns.Add("nombre");
                vData.Columns.Add("identidad");
                vData.Columns.Add("empresa");
                vData.Columns.Add("ingresoEquipo");
                vData.Columns.Add("permisoCel");
                vData.Columns.Add("permisoCel_tabla");
                vData.Columns.Add("ingresoEquipo_tabla");
                vData.Columns.Add("empresa_tabla");
            }else
                vData = (DataTable)Session["ACTIVO_DC_PERSONAL_EXTERNO"];

            Boolean vFlagInsert = false;
            for (int i = 0; i < vData.Rows.Count; i++){
                if (vData.Rows[i]["identidad"].ToString().Equals(TxIdentidadVisita.Text))
                    vFlagInsert = true;
            }

            if (!vFlagInsert) {
                vData.Rows.Add(vData.Rows.Count + 1, TxNombreVisita.Text, TxIdentidadVisita.Text, DdlEmpresaVisita.SelectedItem, RbIngresoEquipoVisita.SelectedItem, RbPermisoCelular.SelectedItem, RbPermisoCelular.SelectedValue, RbIngresoEquipoVisita.SelectedValue, DdlEmpresaVisita.SelectedValue);
            }else
                throw new Exception("Esta identidad ya ha sido agregada.");

            GvVisitas.DataSource = vData;
            GvVisitas.DataBind();
            Session["ACTIVO_DC_PERSONAL_EXTERNO"] = vData;
            UPVisitas.Update();

            TxNombreVisita.Text = String.Empty;
            TxIdentidadVisita.Text = String.Empty;
            DdlEmpresaVisita.SelectedIndex = -1;
            RbIngresoEquipoVisita.SelectedIndex = -1;
            RbPermisoCelular.SelectedIndex = -1;
            UpdatePanel15.Update();
        }
        
        protected void TxInicio_TextChanged(object sender, EventArgs e){
            try{
                calculoHoras();
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }
        
        protected void TxFin_TextChanged(object sender, EventArgs e){
            try{
                calculoHoras();
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }
        
        void calculoHoras(){
            if (TxInicio.Text != "" && TxFin.Text != ""){
                //String vFormato = "dd/MM/yyyy"; //"DD/MM/YYYY HH:mm:ss"
                String vIni = Convert.ToDateTime(TxInicio.Text).ToString("dd/MM/yyyy");
                String vFin = Convert.ToDateTime(TxFin.Text).ToString("dd/MM/yyyy");

                TimeSpan difFechas = Convert.ToDateTime(vFin) - Convert.ToDateTime(vIni);
                int dias = difFechas.Days;

                if (DDLExtendido.SelectedValue == "0"){
                    if (dias != 0 && dias != 1) {
                        TxFin.Text = "";
                        throw new Exception("Deberá ingresar la solicitud de forma diaria.");
                    }
                }
            }
        }

        protected void BtnAgregar_Click(object sender, EventArgs e){
            try{
                 agregarRowDatagrid();
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }
        
        protected void GvVisitas_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                DataTable vDatos = new DataTable();
                if (e.CommandName == "eliminar"){
                    string vIdEmpleado = e.CommandArgument.ToString();
                    if (Session["ACTIVO_DC_PERSONAL_EXTERNO"] != null){
                        vDatos = (DataTable)Session["ACTIVO_DC_PERSONAL_EXTERNO"];

                        DataRow[] result = vDatos.Select("id = '" + vIdEmpleado + "'");
                        foreach (DataRow row in result){
                            if (row["id"].ToString().Contains(vIdEmpleado))
                                vDatos.Rows.Remove(row);
                        }
                    }

                    for (int i = 0; i < vDatos.Rows.Count; i++){
                        vDatos.Rows[i]["id"] = i + 1;
                    }

                    GvVisitas.DataSource = vDatos;
                    GvVisitas.DataBind();
                    if (vDatos.Rows.Count < 1)
                        Session["ACTIVO_DC_PERSONAL_EXTERNO"] = null;
                    else
                        Session["ACTIVO_DC_PERSONAL_EXTERNO"] = vDatos;
                }
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        protected void GvVisitas_PageIndexChanging(object sender, GridViewPageEventArgs e){

        }

        protected void ddlPersonalInterno_SelectedIndexChanged(object sender, EventArgs e){
            try{
                String vQuery = "RSP_ActivosDC 3,'" + ddlPersonalInterno.SelectedValue + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                txtIdentidadInterno.Text = vDatos.Rows[0]["identidad"].ToString();
                TxCorreoInterno.Text = vDatos.Rows[0]["emailEmpresa"].ToString();
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        protected void lbAddPersonalInterno_Click(object sender, EventArgs e){
            try{
                DataTable vData = new DataTable();
                if (Session["ACTIVO_DC_PERSONAL_INTERNO"] == null){
                    vData.Columns.Add("codigoEmpleado");
                    vData.Columns.Add("nombreEmpleado");
                    vData.Columns.Add("identidadInterno");
                    vData.Columns.Add("correoInterno");
                }else
                    vData = (DataTable)Session["ACTIVO_DC_PERSONAL_INTERNO"];

                Boolean vFlagInsert = false;
                for (int i = 0; i < vData.Rows.Count; i++){
                    if (vData.Rows[i]["codigoEmpleado"].ToString().Equals(ddlPersonalInterno.SelectedValue))
                        vFlagInsert = true;
                }

                if (!vFlagInsert) {
                    vData.Rows.Add(ddlPersonalInterno.SelectedValue, ddlPersonalInterno.SelectedItem, txtIdentidadInterno.Text, TxCorreoInterno.Text);
                }else
                    throw new Exception("Este usuario ya ha sido agregado.");

                GvPersonalInterno.DataSource = vData;
                GvPersonalInterno.DataBind();
                Session["ACTIVO_DC_PERSONAL_INTERNO"] = vData;
                txtIdentidadInterno.Text = string.Empty;
                TxCorreoInterno.Text = string.Empty;
                ddlPersonalInterno.SelectedIndex = -1;
                UpdatePanel9.Update();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        
        protected void BtnCrearSolicitud_Click(object sender, EventArgs e){
            try{
                TxFechaInicioModal.Text = TxInicio.Text;
                TxFechaFinModal.Text = TxFin.Text;
                TxTareaModal.Text = txTrabajo.Text;
                TxMotivoModal.Text = TxMotivo.Text;
                lbTitulo.Text = "Crear solicitud, visita al: "+ DDLAcceso.SelectedItem;
                UpdatePanel17.Update();
                UpdatePanel16.Update();

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openEnviarSolicitudModal();", true);
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        void camposDeshabilitados(){
            DdlNombreCopia.Enabled = false;
            DdlNombreCopia.CssClass = "form-control";

            DdlSupervisar.Enabled = false;
            DdlSupervisar.CssClass = "form-control";

            DivPermisoExtendido.Visible = false;
            TxInicio.ReadOnly = true;
            TxFin.ReadOnly = true;
            DDLAcceso.Enabled = false;
            CbxInterno.Enabled = false;
            CbxExterno.Enabled = false;

            txPeticion.ReadOnly = true;
            txTrabajo.ReadOnly = true;
            TxMotivo.ReadOnly = true;
            txTareasRealizar.ReadOnly = true;
        }

        void cargarDataVista(){
            try{          
                string vQuery = "RSP_ActivosDC 6" ;
                DataTable vDatosCopiaVista = vConexion.obtenerDataTable(vQuery);
                if (vDatosCopiaVista.Rows.Count > 0){
                    foreach (DataRow item in vDatosCopiaVista.Rows){
                        DdlNombreCopia.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
                        DdlSupervisar.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
                    }
                }

                DataTable vDatos = new DataTable();
                vDatos = (DataTable)Session["ACTIVO_DC_SOLI_DATOS_GENERALES"];
                string vFormato = "yyyy-MM-ddTHH:mm";

                string vIdSolicitud = vDatos.Rows[0]["idSolicitud"].ToString();
                Session["ACTIVO_DC_ID_SOLICITUD"] = vIdSolicitud;

                string vFechaInicio = vDatos.Rows[0]["fechaInicio"].ToString();
                string vFechaInicioConvertida = Convert.ToDateTime(vFechaInicio).ToString(vFormato);
                TxInicio.Text = vFechaInicioConvertida;

                string vFechaFin = vDatos.Rows[0]["fechaFin"].ToString();
                string vFechaFinConvertida = Convert.ToDateTime(vFechaFin).ToString(vFormato);
                TxFin.Text= vFechaFinConvertida;
                 
                DDLAcceso.SelectedValue = vDatos.Rows[0]["acceso"].ToString();
                CbxInterno.Checked  = vDatos.Rows[0]["personalInterno"].ToString() == "1" ? true : false;
                CbxExterno.Checked  = vDatos.Rows[0]["personalExterno"].ToString() == "1" ? true : false;
                txPeticion.Text = vDatos.Rows[0]["peticion"].ToString();
                txTrabajo.Text = vDatos.Rows[0]["trabajo"].ToString();
                TxMotivo.Text = vDatos.Rows[0]["motivo"].ToString();
                txTareasRealizar.Text = vDatos.Rows[0]["tareasRealizar"].ToString();

                DataTable vDatosResponsable = new DataTable();
                vDatosResponsable = (DataTable)Session["ACTIVO_DC_SOLI_DATOS_RESPONSABLE"];
                LbResponsable.Text = vDatosResponsable.Rows[0]["nombre"].ToString();
                LbIdentidadResponsable.Text = vDatosResponsable.Rows[0]["identidad"].ToString();
                LbSubgerencia.Text = vDatosResponsable.Rows[0]["departamento"].ToString();
                LbJefe.Text = vDatosResponsable.Rows[0]["jefeNombre"].ToString();

                DataTable vDatosCopia = new DataTable();
                vDatosCopia = (DataTable)Session["ACTIVO_DC_SOLI_DATOS_COPIA"];
                DdlNombreCopia.SelectedValue = vDatosCopia.Rows[0]["idEmpleado"].ToString();

                DataTable vDatosSupervisor = new DataTable();
                vDatosSupervisor = (DataTable)Session["ACTIVO_DC_SOLI_DATOS_SUPERVISOR"];
                DdlSupervisar.SelectedValue = vDatosSupervisor.Rows[0]["idEmpleado"].ToString();

                if (CbxInterno.Checked){
                    DivGvInterLectura.Visible = true;
                    DivGvInternoNoLectura.Visible = false;
                    DataTable vDatosPersonalInterno = new DataTable();
                    vDatosPersonalInterno = (DataTable)Session["ACTIVO_DC_SOLI_PERSONAL_INTERNO"];
                    GvInternoLectura.DataSource = vDatosPersonalInterno;
                    GvInternoLectura.DataBind();
                }else{
                    DivGvInterLectura.Visible = false;
                }

                if (CbxExterno.Checked){
                    DivGvExternoLectura.Visible = true;
                    DataTable vDatosPersonalExterno = new DataTable();
                    vDatosPersonalExterno = (DataTable)Session["ACTIVO_DC_SOLI_PERSONAL_EXTERNO"];
                    GvExternoLectura.DataSource = vDatosPersonalExterno;
                    GvExternoLectura.DataBind();
                    UpdatePanel9.Update();
                }else
                    DivGvExternoLectura.Visible = false;
                
                DivParticipantesVista.Visible = true;              
                UpdatePanel20.Update();

            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        void validarAprobacionJefe(){
            if (DdlAccionJefe.SelectedValue == "0")
                throw new Exception("Falta que seleccione acción a realizar");

            if (DdlAccionJefe.SelectedValue=="2" && TxMotivoJefe.Text == string.Empty)
                throw new Exception("Falta que ingrese el motivo de cancelacion de la solicitud");
        }

        void limpiarAprobacionJefe(){
            DdlAccionJefe.SelectedIndex = -1;
            TxMotivoJefe.Text = string.Empty;               
        }
        
        protected void BtnAprobar_Click(object sender, EventArgs e){
            try{
                validarAprobacionJefe();
                TituloAprobacionJefe.Text = "Solicitud número " + Session["ACTIVO_DC_ID_SOLICITUD"].ToString();

                String vFI = TxInicio.Text != "" ? TxInicio.Text : "1999-01-01 00:00:00";
                String vFF = TxFin.Text != "" ? TxFin.Text : "1999-01-01 00:00:00";

                DateTime desde = Convert.ToDateTime(vFI);
                DateTime hasta = Convert.ToDateTime(vFF);

                LbAprobarJefe.Text = "Buen dia <b> " + LbJefe .Text+ "</b><br /><br />" +
                     "Fechas inicio solicitud <b>" + desde.ToString("yyyy-MM-dd HH:mm") + "</b> al <b>" + hasta.ToString("yyyy-MM-dd HH:mm") + "</b> <br />" +
                     "Trabajo a realizar: <b>" + txTrabajo.Text + "</b><br /><br />";
                LbAprobarJefePregunta.Text = "<b>¿Está seguro que desea " + Session["ACTIVO_DC_ESTADO_JEFE"].ToString() + "<b> ,en el rango de fechas y horas detalladas?</b>";
                UpdateAprobarJefe.Update();
                UpTituloAprobarJefeModal.Update();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openAprobarJefeModal();", true);
                limpiarAprobacionJefe();

            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        protected void DdlAccionJefe_SelectedIndexChanged(object sender, EventArgs e){
            if (DdlAccionJefe.SelectedValue == "1") {
                TxMotivoJefe.Visible = false;
                if (DDLAcceso.SelectedValue=="1" || DDLAcceso.SelectedValue == "2"){
                    Session["ACTIVO_DC_ESTADO_JEFE"] = "Aprobar la solicitud; <br>Nota:Se actualizará al estado Pendiente Aprobar Responsable DataCenter";
                    Session["ACTIVO_DC_ESTADO_JEFE_ID"] = "3";
                    Session["ACTIVO_DC_ID_EMPLEADO_RESPONSABLE"] = "315";
                }else{
                    Session["ACTIVO_DC_ESTADO_JEFE"] = "Aprobar la solicitud; <br>Nota:Se actualizará al estado Pendiente Aprobar Responsable Cableado";
                    Session["ACTIVO_DC_ESTADO_JEFE_ID"] = "2";
                    Session["ACTIVO_DC_ID_EMPLEADO_RESPONSABLE"] = "3790";
                }
            }else{
                TxMotivoJefe.Visible = true;
                Session["ACTIVO_DC_ESTADO_JEFE"] = "Cancelar la solicitud";
                Session["ACTIVO_DC_ESTADO_JEFE_ID"] = "5";
            }
        }

        protected void BtnAprobarJefeModal_Click(object sender, EventArgs e){
            try{
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

        protected void BtnEnviarSoli_Click(object sender, EventArgs e){
            try{
                String vFormato = "dd/MM/yyyy HH:mm"; //"dd/MM/yyyy HH:mm:ss"
                string vFechaInicio = Convert.ToDateTime(TxInicio.Text).ToString(vFormato);
                string vFechaFin = Convert.ToDateTime(TxFin.Text).ToString(vFormato);
                
                string vInterno = CbxInterno.Checked ? "1" : "0";
                string vExterno = CbxExterno.Checked ? "1" : "0";
                String vQuery = "RSP_ActivosDC 7" +
                    ",'" + Session["USUARIO"].ToString() + "'" +
                    ",'" + DdlNombreCopia.SelectedValue + "'" +
                    ",'" + DdlSupervisar.SelectedValue + "'" +
                    ",'" + vFechaInicio + "'" +
                    ",'" + vFechaFin + "'" +
                    ",'" + DDLAcceso.SelectedValue + "'" +
                    ",'1'" +
                    ",'" + vExterno + "'" +
                    ",'" + vInterno + "'" +
                    ",'" + txPeticion.Text + "'" +
                    ",'" + txTrabajo.Text + "'" +
                    ",'" + TxMotivo.Text + "'" + 
                    ",'" + txTareasRealizar.Text + "'" +
                    ",'" + Session["ACTIVO_DC_ID_ESTADO"].ToString() + "'" + 
                    ",'" + LbIdJefe.Text + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                string vidSolicitud = vDatos.Rows[0]["idSolicitud"].ToString();

                string vCorreosInternos = "";
                vDatos = (DataTable)Session["ACTIVO_DC_PERSONAL_INTERNO"];
                if (vDatos != null){
                    for (int num = 0; num < vDatos.Rows.Count; num++){
                        string vCodigoEmpleado = vDatos.Rows[num]["codigoEmpleado"].ToString();
                        vQuery = "RSP_ActivosDC 8,'" + vidSolicitud + "','" + vCodigoEmpleado + "'";
                        Int32 vRespuesta = vConexion.ejecutarSql(vQuery);
                        vCorreosInternos = vCorreosInternos + ";"+ vDatos.Rows[num]["correoInterno"].ToString();
                    }
                }


                vDatos = (DataTable)Session["ACTIVO_DC_PERSONAL_EXTERNO"];
                if (vDatos != null){
                    for (int num = 0; num < vDatos.Rows.Count; num++){
                        string vNombre = vDatos.Rows[num]["nombre"].ToString();
                        string vIdentidad = vDatos.Rows[num]["identidad"].ToString();
                        string vEmpresa = vDatos.Rows[num]["empresa_tabla"].ToString();
                        string vIngresoEquipo = vDatos.Rows[num]["ingresoEquipo_tabla"].ToString();
                        string vPermisoCelular = vDatos.Rows[num]["permisoCel_tabla"].ToString();
                        vQuery = "RSP_ActivosDC 9,'" + vidSolicitud + "','" + vNombre + "','" + vIdentidad + "','" + vEmpresa + "','" + vPermisoCelular + "','" + vIngresoEquipo + "'";
                        Int32 vRespuesta = vConexion.ejecutarSql(vQuery);
                    }
                }

                string vEstado = Session["ACTIVO_DC_ID_ESTADO"].ToString();
               
                string vPara = "";
                if (vEstado == "1"){
                    vPara = "wpadilla@bancatlan.hn";
                }else{
                    if (DDLAcceso.SelectedValue == "1" || DDLAcceso.SelectedValue == "2")
                        vPara = "wpadilla@bancatlan.hn";
                    else
                        vPara = "wpadilla@bancatlan.hn";
                }

                vQuery = "RSP_ActivosDC 3,'" + DdlNombreCopia.SelectedValue + "'";
                vDatos = vConexion.obtenerDataTable(vQuery);
                String vCopiaMail = vDatos.Rows[0]["emailEmpresa"].ToString();
                vQuery = "RSP_ActivosDC 3,'" + DdlSupervisar.SelectedValue + "'";
                vDatos = vConexion.obtenerDataTable(vQuery);
                String vSupervisorMail = vDatos.Rows[0]["emailEmpresa"].ToString();

                String vCopia = LbCorreo.Text + ";" + vCopiaMail + ";" + vSupervisorMail + vCorreosInternos;
                vQuery = "RSP_ActivosDC 11" +
                    ",'" + vPara + "'" +
                    ",'" + vCopia + "'" +
                    ",'Aprobación Solicitud Acceso Data Center'" +
                    ",'Favor con la respectiva aprobación'" +
                    ",'0'" +
                    ",'" + vidSolicitud + "'";
                
                Int32 vInfo = vConexion.ejecutarSql(vQuery);
                if (vInfo == 1)
                    Mensaje("Solicitud enviada con éxito.", WarningType.Success);
                else
                    Mensaje("No se pudo enviar la solicitud, favor contactarse con el administrador del sistema", WarningType.Danger);
                
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeEnviarSolicitudModal();", true);
                limpiarSolicitud();
                CargarInformacionGeneral();
                UpdatePanel2.Update();

            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        private void limpiarSolicitud(){
            DivParticipantes.Visible = false;
            LbResponsable.Text = String.Empty;
            LbIdentidadResponsable.Text = String.Empty;
            LbSubgerencia.Text = String.Empty;
            LbJefe.Text = String.Empty;
            DdlNombreCopia.SelectedIndex = -1;
            DdlSupervisar.SelectedIndex = -1;

            DDLExtendido.SelectedValue = "0";
            TxInicio.Text = String.Empty;
            TxFin.Text = String.Empty;
            DDLAcceso.SelectedIndex = -1;
            CbxInterno.Checked = false;
            CbxExterno.Checked = false;

            txPeticion.Text = String.Empty;
            txTrabajo.Text = String.Empty;
            TxMotivo.Text = String.Empty;
            txTareasRealizar.Text = String.Empty;

            GvPersonalInterno.DataSource = null;
            GvPersonalInterno.DataBind();

            GvVisitas.DataSource = null;
            GvVisitas.DataBind();

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

        void CargarSolicitudesIngresadas(){
            try{
                DataTable vDatos = new DataTable();
                String vQuery = "RSP_ActivosDC 19,'" + Convert.ToString(Session["USUARIO"]) + "'";
                vDatos = vConexion.obtenerDataTable(vQuery);

                if (vDatos.Rows.Count > 0){
                    GVSolicitudes.DataSource = vDatos;
                    GVSolicitudes.DataBind();
                    UpBusquedaSolicitudes.Update();
                    Session["ACTIVO_DC_SOLICITUDES_INGRESADAS"] = vDatos;
                }
            }catch (Exception Ex){
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

        void validarAprobacionGestor(){
            if (DdlAccionGestor.SelectedValue == "0")
                throw new Exception("Falta que seleccione acción a realizar");

            if (DdlAccionGestor.SelectedValue == "2" && TxtMotivoGestor.Text == string.Empty)
                throw new Exception("Falta que ingrese el motivo de cancelacion de la solicitud");
        }

        void limpiarAprobacionGestor(){
            DdlAccionGestor.SelectedIndex = -1;
            TxtMotivoGestor.Text = string.Empty;
        }

        protected void DdlAccionGestor_SelectedIndexChanged(object sender, EventArgs e){
            if (DdlAccionGestor.SelectedValue == "1"){
                TxtMotivoGestor.Visible = false;
                if (DDLAcceso.SelectedValue == "1" || DDLAcceso.SelectedValue == "2"){
                    Session["ACTIVO_DC_ESTADO_GESTOR"] = "Aprobar la solicitud; Nota:Se autorizará la visita personal de Segurida realizará la respectiva verificación del equipo.";
                    Session["ACTIVO_DC_ESTADO_GESTOR_ID"] = "4";
                }else{
                    Session["ACTIVO_DC_ESTADO_GESTOR"] = "Aprobar la solicitud; Nota:Se autorizará la visita personal de Segurida realizará la respectiva verificación del equipo.";
                    Session["ACTIVO_DC_ESTADO_GESTOR_ID"] = "8";
                }
            }else{
                TxtMotivoGestor.Visible = true;
                if (DDLAcceso.SelectedValue == "1" || DDLAcceso.SelectedValue == "2"){
                    Session["ACTIVO_DC_ESTADO_GESTOR"] = "Cancelar la solicitud";
                    Session["ACTIVO_DC_ESTADO_GESTOR_ID"] = "6";
                }
                
                if (DDLAcceso.SelectedValue == "1" || DDLAcceso.SelectedValue == "2"){
                    Session["ACTIVO_DC_ESTADO_GESTOR"] = "Cancelar la solicitud";
                    Session["ACTIVO_DC_ESTADO_GESTOR_ID"] = "7";
                }
            }
        }

        protected void BtnEnviarGestor_Click(object sender, EventArgs e){
            try{
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
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        protected void BtnCancelar_Click1(object sender, EventArgs e){

        }

        protected void DDLExtendido_SelectedIndexChanged(object sender, EventArgs e){
            try{
                TxInicio.Text = "";
                TxFin.Text = "";
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        protected void CbxExterno_CheckedChanged(object sender, EventArgs e){
            if (CbxExterno.Checked){   
                tabExterno.Visible = true;
                DivParticipantes.Visible = true;               
            }else{
                Session["ACTIVO_DC_SOLI_PERSONAL_EXTERNO"] = null;
                tabExterno.Visible = false;
                if (!CbxInterno.Checked)
                    DivParticipantes.Visible = false;
            }
            UpdatePanel1.Update();
        }

        protected void CbxInterno_CheckedChanged(object sender, EventArgs e){
            if (CbxInterno.Checked){
                tabInterno.Visible = true;
                DivParticipantes.Visible = true;
            }else{
                Session["ACTIVO_DC_SOLI_PERSONAL_INTERNO"] = null;
                tabInterno.Visible = false;
                if (!CbxExterno.Checked){
                    DivParticipantes.Visible = false;
                }
            }
            UpdatePanel1.Update();
        }

        protected void GvPersonalInterno_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                DataTable vDatos = new DataTable();
                if (e.CommandName == "eliminar"){
                    string vIdEmpleado = e.CommandArgument.ToString();
                    if (Session["ACTIVO_DC_PERSONAL_INTERNO"] != null){
                        vDatos = (DataTable)Session["ACTIVO_DC_PERSONAL_INTERNO"];

                        DataRow[] result = vDatos.Select("codigoEmpleado = '" + vIdEmpleado + "'");
                        foreach (DataRow row in result){
                            if (row["codigoEmpleado"].ToString().Contains(vIdEmpleado))
                                vDatos.Rows.Remove(row);
                        }
                    }

                    GvPersonalInterno.DataSource = vDatos;
                    GvPersonalInterno.DataBind();
                    if (vDatos.Rows.Count < 1)
                        Session["ACTIVO_DC_PERSONAL_INTERNO"] = null;
                    else
                        Session["ACTIVO_DC_PERSONAL_INTERNO"] = vDatos;
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
    }
}