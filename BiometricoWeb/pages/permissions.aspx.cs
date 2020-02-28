using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace BiometricoWeb.pages
{
    
    public partial class permissions : System.Web.UI.Page
    {
        db vConexion;
        protected void Page_Load(object sender, EventArgs e){
            vConexion = new db();
            String vEx = Request.QueryString["ex"];

            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    CargarEmpleados();
                    CargarTipoPermisos();
                    CargarPermisos();
                    CargarDiasSAP();
                    CargarCompensatorio();

                    if (vEx != null){
                        if (vEx.Equals("1"))
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + "Archivo subido con exito" + "')", true);
                        else if (vEx.Equals("2"))
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + "Error al subir archivo, por favor entregarlo en fisico a recursos humanos." + "')", true);
                        else if (vEx.Equals("3"))
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + "Permiso ingresado con exito." + "')", true);
                        else if (vEx.Equals("4"))
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + "Debe ingresar un documento que justifique el permiso." + "')", true);
                    }
                }
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        public void MensajeBlock(string vMensaje, WarningType type){
            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        public void CerrarModal(String vModal)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#" + vModal + "').modal('hide');", true);
        }
        
        void CargarEmpleados(){
            try{
                DataTable vDatos = new DataTable();
                String vQuery = "RSP_ObtenerGenerales 8,'" + Convert.ToString(Session["USUARIO"]) + "'";

                vDatos = vConexion.obtenerDataTable(vQuery);
                Session["PERMISOSCGS"] = Convert.ToBoolean(vDatos.Rows[0]["PermisosCGS"]);
                
                DDLEmpleado.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLEmpleado.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["idEmpleado"].ToString() + " - " + item["nombre"].ToString() });
                }

                DDLJefe.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLJefe.Items.Add(new ListItem { Value = item["idJefe"].ToString(), Text = item["idJefe"].ToString() + " - " + item["jefeNombre"].ToString() });
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        void CargarTipoPermisos(){
            try{
                DataTable vDatos = new DataTable();
                String vQuery = "RSP_ObtenerGenerales 4";
                vDatos = vConexion.obtenerDataTable(vQuery);

                DDLTipoPermiso.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLTipoPermiso.Items.Add(new ListItem { Value = item["idTipoPermiso"].ToString(), Text = item["idTipoPermiso"].ToString() + " - " + item["descripcion"].ToString() });
                }

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        void CargarPermisos(){
            try{
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerGenerales 5,'" + Convert.ToString(Session["USUARIO"]) + "'"); //2902
                GVBusqueda.DataSource = vDatos;
                GVBusqueda.DataBind();
                Session["DATAOSPERMISOS"] = vDatos;
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); } // swift reiniciado, se creo una politica
        }

        void CargarDiasSAP(){
            try{
                SapConnector vTest = new SapConnector();
                //String vDias = vTest.getDiasVacaciones(Convert.ToString(Session["CODIGOSAP"]));
                String vDias = "12.62";
                LbNumeroVaciones.Text = vDias;
                Session["DIASSAP"] = vDias;
            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        void CargarCompensatorio() {
            String vQuery = "RSP_ObtenerGenerales 18," + Convert.ToString(Session["USUARIO"]);
            DataTable vData = vConexion.obtenerDataTable(vQuery);
            if (vData.Rows[0][0].ToString() != ""){
                LbCompensatorio.Text = vData.Rows[0][0].ToString();
            }
            Session["DIASCOMPENSATORIO"] = LbCompensatorio.Text;
        }

        protected void BtnCrearPermiso_Click(object sender, EventArgs e){
            try{
                String vTipo = DDLTipoPermiso.SelectedValue;
                String vEmpleado = DDLEmpleado.SelectedValue;

                String vFI = TxFechaInicio.Text != "" ? TxFechaInicio.Text : "1999-01-01 00:00:00";
                String vFF = TxFechaRegreso.Text != "" ? TxFechaRegreso.Text : "1999-01-01 00:00:00";

                DateTime desde = Convert.ToDateTime(vFI);
                DateTime hasta = Convert.ToDateTime(vFF);
                DateTime vFechaInicio = Convert.ToDateTime(vFI);

                TimeSpan tsHorario = hasta - desde;
                ValidacionesPermisos(vEmpleado, desde, hasta, vTipo, tsHorario);
                
                int dias = 0;
                int days = 1;

                while (desde <= hasta){
                    if (desde.DayOfWeek != DayOfWeek.Saturday && desde.DayOfWeek != DayOfWeek.Sunday)
                        dias++;

                    desde = desde.AddDays(1);
                }

                if (tsHorario.Days >= 1)
                    days = dias; //ts.Days + 1 - 
                else if (tsHorario.Hours > 0 || tsHorario.Minutes > 0){
                    days = 0;
                }

                generales vGenerales = new generales();
                int vDias = vGenerales.BusinessDaysUntil(Convert.ToDateTime(TxFechaInicio.Text), Convert.ToDateTime(TxFechaRegreso.Text));

                LbInformacionPermiso.Text = "Informacion de Permiso de empleado " + DDLEmpleado.SelectedValue + "<br /><br />" +
                    "Fechas solicitadas del <b>" + vFechaInicio.ToString("yyyy-MM-dd HH:mm:ss") + "</b> al <b>" + hasta.ToString("yyyy-MM-dd HH:mm:ss") + "</b><br /><br />" +
                    "Total: <b>" + days + "</b> días <b>" + tsHorario.Hours + "</b> horas <b>" + tsHorario.Minutes + "</b> minutos<br /><br />" +
                    "Tipo de permiso solicitado: <b>" + DDLTipoPermiso.SelectedItem.Text + "</b><br /><br />" +
                    "¿Estas seguro que estas fechas quieres solicitar?";

                if (DDLTipoPermiso.SelectedValue.Equals("1003")){
                    if (vDias > 3){
                        LbIncapacidadInformacion.Text = "Es obligatorio que tramites tu subsidio del 66% de incapacidad en el seguro social a la mayor brevedad posible y sea depositado a la cuenta de INFATLAN, caso contrario se te deducirá de tu planilla";
                    }else
                        LbIncapacidadInformacion.Text = String.Empty;
                }else
                    LbIncapacidadInformacion.Text = String.Empty;

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Pop", "openModal();", true);

            }catch (Exception Ex) {
                Mensaje(Ex.Message.StartsWith("Longitud") ? "El Token ingresado no es válido." : Ex.Message, WarningType.Danger);
            }
        }

        private void ValidacionesPermisos(String vEmpleado, DateTime vFechaInicio, DateTime vFechaRegreso, String vTipo, TimeSpan tsHorario){
            Decimal vDiasDisponibles = Convert.ToDecimal((LbNumeroVaciones.Text == String.Empty ? "0" : LbNumeroVaciones.Text));
            //GENERALES
            if (vEmpleado.Equals("0"))
                throw new Exception("Seleccione un empleado valido para el permiso.");
            if (DDLJefe.SelectedValue.Equals("0"))
                throw new Exception("Seleccione un jefe valido para el permiso.");
            if (vTipo.Equals("0"))
                throw new Exception("Seleccione un tipo de permiso valido");
            if (TxFechaInicio.Text.Equals(""))
                throw new Exception("Ingrese una fecha de inicio valida");
            if (TxFechaRegreso.Text.Equals(""))
                throw new Exception("Ingrese una fecha de regreso valida");
            if (vFechaRegreso < vFechaInicio)
                throw new Exception("Las fechas ingresadas son incorrectas, el regreso es menor que el inicio");

            Decimal vDiasHoras = Calculo(null, -1);

            //ESPECIFICAS
            if (vTipo != "1002" || vTipo != "1004" || vTipo != "1012" || vTipo != "1013" || vTipo != "1018" || vTipo != "1022"){
                if ((tsHorario.Days >= 1 && tsHorario.Hours > 0) || (tsHorario.Days >= 1 && tsHorario.Minutes > 0)){
                    throw new Exception("Debe solicitar permisos por día y hora individualmente");
                }

                if (vFechaInicio.Day != vFechaRegreso.Day){
                    if (vFechaInicio.Hour != 0 || vFechaInicio.Minute != 0 || vFechaRegreso.Hour != 0 || vFechaRegreso.Minute != 0){
                        throw new Exception("Debe solicitar permisos por día y hora individualmente");
                    }
                }
            }

            if (vTipo == "1004" || vTipo == "1007"){
                if (vDiasHoras > vDiasDisponibles)
                    throw new Exception("Usted no cuenta con suficientes vacaciones disponibles.");
                if (vDiasDisponibles <= 0)
                    throw new Exception("Usted no cuenta con vacaciones disponibles.");

                String vQuerySIM = "RSP_ObtenerGenerales 15, " + vEmpleado;
                DataTable vDatosSIM = vConexion.obtenerDataTable(vQuerySIM);
                Decimal vDiasHoras2 = 0;

                if (vDatosSIM.Rows.Count > 0){
                    for (int i = 0; i < vDatosSIM.Rows.Count; i++){
                        vDiasHoras2 += Calculo(vDatosSIM, i);
                    }
                }

                Decimal vDias = Convert.ToDecimal(LbNumeroVaciones.Text) - vDiasHoras2 - vDiasHoras;
                if (vDias < 0)
                    throw new Exception("La cantidad de días solicitados sobrepasa los disponibles. " +
                        "Para consultar los permisos pendientes comuníquese con Recursos Humanos.");
            }

            if ((vTipo == "1001" || vTipo == "1010" || vTipo == "1018") && !Convert.ToBoolean(Session["PERMISOSCGS"]))
                throw new Exception("Debe solicitar acceso a RRHH para ingresar este permiso.");

            if (vTipo == "1006"){
                String vAnual = DateTime.Now.Year.ToString();
                String vMes = DateTime.Now.Month.ToString().Length > 1 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month.ToString();

                String vQuery = "RSP_ValidacionesPermisos 2," + vEmpleado + ",'" + vAnual + "',0";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                if (Convert.ToInt32(vDatos.Rows[0][0].ToString()) > 15)
                    throw new Exception("Ya ha realizado los 15 permisos de calamidad domestica este año.");

                vQuery = "RSP_ValidacionesPermisos 3," + vEmpleado + ",'" + vMes + "',0";
                vDatos = vConexion.obtenerDataTable(vQuery);

                if (Convert.ToInt32(vDatos.Rows[0][0].ToString()) > 2)
                    throw new Exception("Ya ha realizado los 2 permisos de calamidad domestica este mes.");
            }

            if (tsHorario.Hours < 4 && tsHorario.Days < 1 && vFechaInicio != vFechaRegreso && (vTipo == "1001" || vTipo == "1002" || vTipo == "1006" || vTipo == "1007" || vTipo == "1008" || vTipo == "1010" || vTipo == "1018" || vTipo == "1019" || vTipo == "1020" || vTipo == "1021"))
                throw new Exception("No se pueden agregar permisos menores a 4 horas.");

            // VALIDACION COMPENSATORIO
            if (vTipo == "1011"){
                int days = Convert.ToInt32(vDiasHoras);
                days = days != 0 ? days * 8 : days;

                Decimal vTotal = days + tsHorario.Hours + (Convert.ToDecimal(tsHorario.Minutes) / 60);
                if (Convert.ToDecimal(vTotal) > Convert.ToDecimal(LbCompensatorio.Text)) 
                    throw new Exception("No tiene suficientes horas de tiempo compensatorio.");
                Session["HORAS_COMPENSATORIO"] = vTotal;
            }
            
            //EMERGENCIAS
            if (CbEmergencias.Checked){
                if (vTipo == "1004" || vTipo == "1007" || vTipo == "1011"){
                    if (TxToken.Text != "" || TxToken.Text != string.Empty){
                        CryptoToken.CryptoToken vDec = new CryptoToken.CryptoToken();
                        String vWord = ConfigurationManager.AppSettings["TokenWord"].ToString();
                        String vPass = ConfigurationManager.AppSettings["TokenPass"].ToString();
                        String vTok = vDec.Decrypt(TxToken.Text, vPass);
                        String vQuery2 = "RSP_ObtenerGenerales 17," + vEmpleado;
                        DataTable vDatosVerificacion = vConexion.obtenerDataTable(vQuery2);
                        if (vDatosVerificacion.Rows.Count > 0){
                            Session["IDTOKEN"] = vDatosVerificacion.Rows[0]["id"].ToString();
                            DateTime vFecVence = Convert.ToDateTime(vDatosVerificacion.Rows[0]["fechaVencimiento"].ToString());
                            if (vFecVence > DateTime.Now){
                                if (vTok == vWord && TxToken.Text == vDatosVerificacion.Rows[0]["idToken"].ToString()){
                                    TimeSpan ts = Convert.ToDateTime(TxFechaInicio.Text) - DateTime.Now;
                                    if (ts.Days < -15)
                                        throw new Exception("No se pueden agregar permisos por incumplimiento de politica (15 dias maximo para ingresar permisos pasados)");
                                }else
                                    throw new Exception("El Token ingresado no es válido.");
                            }else
                                throw new Exception("Token vencido.");
                        }else
                            throw new Exception("No tiene token asignado, comuníquese con Recursos Humanos para generar uno nuevo.");
                    }else
                        throw new Exception("Debe ingresar un token. Solicítelo a Recursos Humanos");
                }
            }else{
                String vFormato = "MM/dd/yyyy HH:mm:ss"; //"dd/MM/yyyy HH:mm:ss"
                String vIni = Convert.ToDateTime(vFechaInicio).ToString(vFormato);
                String vFin = Convert.ToDateTime(vFechaRegreso).ToString(vFormato);
                
                String vQuery = "RSP_ValidacionesPermisos 1," + vEmpleado + ",'" + vIni + "','" + vFin + "'";
                DataTable vDatosVerificacion = vConexion.obtenerDataTable(vQuery);

                if (vDatosVerificacion.Rows.Count > 0){
                    if (vDatosVerificacion.Rows[0][0].ToString().Equals("1"))
                        throw new Exception("Ya existe un permiso en el tiempo seleccionado");
                }

                if (Convert.ToInt32(vTipo) < 2000 && vTipo != "1003" && vTipo != "1005" && vTipo != "1011" && vTipo != "1023" && vTipo != "1024" && vTipo != "1025"){
                    TimeSpan ts = Convert.ToDateTime(TxFechaInicio.Text) - DateTime.Now;
                    if (ts.Days < -15)
                        throw new Exception("No se pueden agregar permisos por incumplimiento de politica (15 dias maximo para ingresar permisos pasados)");
                    if (ts.Days < 2)
                        throw new Exception("No se pueden agregar permisos por incumplimiento de politica (48 Horas antes)");
                    if (ts.Days > 15)
                        throw new Exception("No se pueden agregar permisos por incumplimiento de politica (15 dias maximo de anticipación)");
                }
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e){
            try{
                LimpiarPermiso();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        private void LimpiarPermiso(){
            DDLEmpleado.SelectedIndex = -1;
            DDLJefe.SelectedIndex = -1;
            DDLTipoPermiso.SelectedIndex = -1;
            TxFechaInicio.Text = String.Empty;
            TxFechaRegreso.Text = String.Empty;
            TxMotivo.Text = String.Empty;

            DDLParientes.SelectedIndex = -1;
            DIVCompensacion.Visible = false;
            DIVCompensacionFecha.Visible = false;
            DIVDocumentos.Visible = false;
            DIVParientes.Visible = false;

        }

        protected void BtnEnviarPermiso_Click(object sender, EventArgs e){
            try{
                String vNombreDepot1 = String.Empty;
                HttpPostedFile bufferDeposito1T = FUDocumentoPermiso.PostedFile;
                byte[] vFileDeposito1 = null;
                String vExtension = String.Empty;

                if (bufferDeposito1T != null){
                    vNombreDepot1 = FUDocumentoPermiso.FileName;
                    Stream vStream = bufferDeposito1T.InputStream;
                    BinaryReader vReader = new BinaryReader(vStream);
                    vFileDeposito1 = vReader.ReadBytes((int)vStream.Length);
                    vExtension = System.IO.Path.GetExtension(FUDocumentoPermiso.FileName);
                }

                if (DDLTipoPermiso.SelectedValue == "1006" && !FUDocumentoPermiso.HasFiles)
                    Response.Redirect("/pages/permissions.aspx?ex=4");

                String vArchivo = String.Empty;
                if (vFileDeposito1 != null)
                    vArchivo = Convert.ToBase64String(vFileDeposito1);

                Int32 vEmergencia = CbEmergencias.Checked ? 1 : 0;

                String vFormato = "yyyy-MM-dd HH:mm:ss"; //"dd-MM-yyyy HH:mm:ss"; 
                String vFeINI = Convert.ToDateTime(TxFechaInicio.Text).ToString(vFormato);
                String vFeFIN = Convert.ToDateTime(TxFechaRegreso.Text).ToString(vFormato);

                String vQuery = "RSP_IngresarPermisos 1," + DDLEmpleado.SelectedValue + "," +
                    "" + DDLJefe.SelectedValue + "," +
                    "" + DDLTipoPermiso.SelectedValue + "," +
                    "'" + TxMotivo.Text + "'," +
                    "'" + vFeINI + "'," +
                    "'" + vFeFIN + "'," +
                    "'" + Convert.ToString(Session["USUARIO"]) + "'," +
                    "" + DDLCompensacion.SelectedValue + "," +
                    "'" + TxFechaCompensacion.Text + "'," +
                    "" + DDLParientes.SelectedValue + "," + 
                    "'" + vArchivo + "'," +
                    "'" + vExtension + "'," + vEmergencia ;

                Int32 vInformacion = vConexion.ejecutarSql(vQuery);
                //-PROD- Int32 vInformacion = 1;

                if (vInformacion == 1) {
                    String vRe = "";

                    // begin wpadilla
                    vQuery = "[RSP_IngresarEmpleados] 4," + DDLEmpleado.SelectedValue + ", '0'";
                    vConexion.ejecutarSql(vQuery);

                    if (CbEmergencias.Checked && Session["IDTOKEN"] != null){
                        vQuery = "[RSP_IngresaMantenimientos] 6, " + Session["IDTOKEN"].ToString();
                        vConexion.ejecutarSql(vQuery);
                    }
                    
                    if (DDLTipoPermiso.SelectedValue == "1011"){
                        String vTiempo = Session["HORAS_COMPENSATORIO"].ToString();
                        String vCalculo = vTiempo.Contains(",") ? vTiempo.Replace(",", ".") : vTiempo;

                        vQuery = "RSP_ObtenerGenerales 19," + DDLEmpleado.SelectedValue;
                        DataTable vIdPermiso = vConexion.obtenerDataTable(vQuery);
                        
                        vQuery = "[RSP_Compensatorio] 1, '" + Session["CODIGOSAP"].ToString() + "',0,null,'" + Session["USUARIO"].ToString() + "',Null," + vCalculo + "," + vIdPermiso.Rows[0]["idPermiso"].ToString();
                        int vRespuesta = vConexion.ejecutarSql(vQuery);
                        
                        if (vRespuesta == 2)
                            vRe = ", se actualizó el tiempo compensatorio.";
                        else
                            vRe = ", NO se actualizó el tiempo compensatorio.";
                    }
                    // end  wpadilla

                    vQuery = "RSP_ObtenerEmpleados 2," + DDLJefe.SelectedValue;
                    DataTable vDatosJefatura = vConexion.obtenerDataTable(vQuery);
                    vQuery = "RSP_ObtenerEmpleados 2," + DDLEmpleado.SelectedValue;
                    DataTable vDatosEmpleado = vConexion.obtenerDataTable(vQuery);

                    SmtpService vService = new SmtpService();
                    Boolean vFlagEnvioSupervisor = false;

                    foreach (DataRow item in vDatosJefatura.Rows){
                        if (!item["emailEmpresa"].ToString().Trim().Equals("")){
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Supervisor,
                                item["nombre"].ToString(),
                                vDatosEmpleado.Rows[0]["nombre"].ToString()
                                );
                            vFlagEnvioSupervisor = true;
                        }
                    }
                    
                    if (vFlagEnvioSupervisor){
                        foreach (DataRow item in vDatosEmpleado.Rows){
                            if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                                vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                    typeBody.Solicitante,
                                    item["nombre"].ToString(),
                                    item["nombre"].ToString()
                                    );
                        }
                        
                        Mensaje("Permiso ingresado con exito" + vRe, WarningType.Success);
                        Response.Redirect("/pages/permissions.aspx?ex=3");
                    }else
                        Mensaje("Permiso ingresado con exito, Fallo envio de correo ponte en contacto con tu Jefe" + vRe, WarningType.Success);
                }else
                    Mensaje("Permiso no se ingreso o ya está creado, revise sus permisos.", WarningType.Success);
                
                LimpiarPermiso();
                CargarPermisos();
                CargarDiasSAP();
                CargarCompensatorio();
                CerrarModal("InformativoModal");
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void DDLTipoPermiso_SelectedIndexChanged(object sender, EventArgs e){
            try{
                DIVCompensacion.Visible = false;
                DIVCompensacionFecha.Visible = false;
                DIVDocumentos.Visible = false;
                DIVParientes.Visible = false;

                switch (DDLTipoPermiso.SelectedValue){
                    case "1000":
                        TxFechaInicio.TextMode = TextBoxMode.DateTimeLocal;
                        TxFechaRegreso.TextMode = TextBoxMode.DateTimeLocal;
                        UpdatePanelFechas.Update();
                        break;          
                    case "1001":
                        Mensaje("Debe solicitar acceso a RRHH para ingresar este permiso. Si ya lo tiene, continue.", WarningType.Warning);
                        break;
                    case "1002":
                        TxFechaInicio.TextMode = TextBoxMode.Date;
                        TxFechaRegreso.TextMode = TextBoxMode.Date;
                        Mensaje("Tener en cuenta que al tomar este permiso te va impactar en los calculos del 13avo, 14avo y 15avo mes de salario", WarningType.Warning);
                        UpdatePanelFechas.Update();
                        break;
                    case "1003":
                        TxFechaInicio.TextMode = TextBoxMode.DateTimeLocal;
                        TxFechaRegreso.TextMode = TextBoxMode.DateTimeLocal;
                        UpdatePanelFechas.Update();
                        DIVDocumentos.Visible = true;
                        break;
                    case "1004":
                        if (CbEmergencias.Checked)
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#ModalToken').modal('show');", true);
                        
                        TxFechaInicio.TextMode = TextBoxMode.Date;
                        TxFechaRegreso.TextMode = TextBoxMode.Date;
                        UpdatePanelFechas.Update();
                        break;
                    case "1005":
                        TxFechaInicio.TextMode = TextBoxMode.DateTimeLocal;
                        TxFechaRegreso.TextMode = TextBoxMode.DateTimeLocal;
                        UpdatePanelFechas.Update();
                        break;
                    case "1006":
                        TxFechaInicio.TextMode = TextBoxMode.DateTimeLocal;
                        TxFechaRegreso.TextMode = TextBoxMode.DateTimeLocal;
                        UpdatePanelFechas.Update();
                        DIVDocumentos.Visible = true;
                        break;
                    case "1007":
                        if (CbEmergencias.Checked)
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#ModalToken').modal('show');", true);

                        TxFechaInicio.TextMode = TextBoxMode.DateTimeLocal;
                        TxFechaRegreso.TextMode = TextBoxMode.DateTimeLocal;
                        UpdatePanelFechas.Update();
                        break;
                    case "1008":
                        TxFechaInicio.TextMode = TextBoxMode.DateTimeLocal;
                        TxFechaRegreso.TextMode = TextBoxMode.DateTimeLocal;
                        UpdatePanelFechas.Update();
                        DIVParientes.Visible = true;
                        DIVDocumentos.Visible = true;
                        break;
                    case "1010":
                        Mensaje("Debe solicitar acceso a RRHH para ingresar este permiso. Si ya lo tiene, continue.", WarningType.Warning);
                        TxFechaInicio.TextMode = TextBoxMode.DateTimeLocal;
                        TxFechaRegreso.TextMode = TextBoxMode.DateTimeLocal;
                        UpdatePanelFechas.Update();
                        DIVDocumentos.Visible = true;
                        break;
                    case "1011":
                        if (CbEmergencias.Checked)
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#ModalToken').modal('show');", true);
                        TxFechaInicio.TextMode = TextBoxMode.DateTimeLocal;
                        TxFechaRegreso.TextMode = TextBoxMode.DateTimeLocal;
                        UpdatePanelFechas.Update();
                        DIVCompensacion.Visible = true;
                        DIVCompensacionFecha.Visible = true;
                        break;
                    case "1012":
                    case "1013":
                        TxFechaInicio.TextMode = TextBoxMode.Date;
                        TxFechaRegreso.TextMode = TextBoxMode.Date;
                        UpdatePanelFechas.Update();
                        break;
                    
                    case "1019":
                        TxFechaInicio.TextMode = TextBoxMode.DateTimeLocal;
                        TxFechaRegreso.TextMode = TextBoxMode.DateTimeLocal;
                        UpdatePanelFechas.Update();
                        Mensaje("Tener en cuenta que al tomar este permiso te va impactar en los calculos del 13avo, 14avo y 15avo mes de salario", WarningType.Warning);
                        break;
                    case "1014":
                    case "1018":
                        Mensaje("Debe solicitar acceso a RRHH para ingresar este permiso. Si ya lo tiene, continue.", WarningType.Warning);
                        break;
                    case "1020":
                    case "1021":
                        TxFechaInicio.TextMode = TextBoxMode.DateTimeLocal;
                        TxFechaRegreso.TextMode = TextBoxMode.DateTimeLocal;
                        UpdatePanelFechas.Update();
                        break;
                    case "1022":
                        TxFechaInicio.TextMode = TextBoxMode.Date;
                        TxFechaRegreso.TextMode = TextBoxMode.Date;
                        UpdatePanelFechas.Update();
                        break;
                    case "1023":
                    case "1024":
                    case "1025":
                        TxFechaInicio.TextMode = TextBoxMode.DateTimeLocal;
                        TxFechaRegreso.TextMode = TextBoxMode.DateTimeLocal;
                        UpdatePanelFechas.Update();
                        DIVDocumentos.Visible = true;
                        break;

                    default:
                        break;

                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e){
            if (e.CommandName == "MotivoPermiso"){
                string vIdPermiso = e.CommandArgument.ToString();

                String vQuery = "RSP_ObtenerPermisos 3," + Session["USUARIO"] + "," + vIdPermiso;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                String vMotivo = "Ningun motivo";
                if (!vDatos.Rows[0]["Motivo"].ToString().Equals(""))
                    vMotivo = vDatos.Rows[0]["Motivo"].ToString();

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "window.alert('" + vMotivo + "')", true);
            }

            if (e.CommandName == "EditarPermiso"){
                string vIdPermiso = e.CommandArgument.ToString();
                LbPermisoSubir.Text = vIdPermiso;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openEdicionModal();", true);
            }

            if (e.CommandName == "DocumentoPermiso"){
                string vIdPermiso = e.CommandArgument.ToString();


                String vQuery = "RSP_ObtenerPermisos 3," + Session["USUARIO"] + "," + vIdPermiso;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                String vDocumento = "";
                if (!vDatos.Rows[0]["documento"].ToString().Equals(""))
                    vDocumento = vDatos.Rows[0]["documento"].ToString();

                if (!vDocumento.Equals("")){
                    LbPermisoDescarga.Text = vIdPermiso;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openDescargarModal();", true);
                }else
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "window.alert('No existe documento en este permiso')", true);
            }
        }

        private string GetExtension(string Extension){
            switch (Extension){
                case ".doc":
                    return "application/ms-word";
                case ".xls":
                    return "application/vnd.ms-excel";
                case ".ppt":
                    return "application/mspowerpoint";
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".zip":
                    return "application/zip";
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case ".wav":
                    return "audio/wav";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                    return "application/xml";
                default:
                    return "application/octet-stream";
            }
        }

        protected void BtnEditarPermiso_Click(object sender, EventArgs e){
            try{
                String vNombreDepot1 = String.Empty;
                HttpPostedFile bufferDeposito1T = FUSubirArchivoEdicion.PostedFile;
                byte[] vFileDeposito1 = null;
                String vExtension = String.Empty;
                if (bufferDeposito1T != null){
                    vNombreDepot1 = FUSubirArchivoEdicion.FileName;
                    Stream vStream = bufferDeposito1T.InputStream;
                    BinaryReader vReader = new BinaryReader(vStream);
                    vFileDeposito1 = vReader.ReadBytes((int)vStream.Length);
                    vExtension = System.IO.Path.GetExtension(FUSubirArchivoEdicion.FileName);
                }

                String vArchivo = String.Empty;
                if (vFileDeposito1 != null)
                    vArchivo = Convert.ToBase64String(vFileDeposito1);

                String vQuery = "RSP_IngresarPermisosDocumentos 1," + LbPermisoSubir.Text + "," +
                    "'" + vArchivo + "'," +
                    "'" + vExtension + "'";

                Int32 vInformacion = vConexion.ejecutarSql(vQuery);
                if (vInformacion.Equals(1))
                {
                    Response.Redirect("/pages/permissions.aspx?ex=1");
                }
                else
                {
                    Response.Redirect("/pages/permissions.aspx?ex=2");
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnDescargarArchivo_Click(object sender, EventArgs e){
            try{
                string vIdPermiso = LbPermisoDescarga.Text;

                String vQuery = "RSP_ObtenerPermisos 3," + Session["USUARIO"] + "," + vIdPermiso;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                String vDocumento = "";
                if (!vDatos.Rows[0]["documento"].ToString().Equals(""))
                    vDocumento = vDatos.Rows[0]["documento"].ToString();

                if (!vDocumento.Equals("")){
                    String vDocumentoArchivo = "DocumentoRRHH" + vDatos.Rows[0]["documentoExtension"].ToString();

                    byte[] fileData = Convert.FromBase64String(vDocumento);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    GetExtension(vDatos.Rows[0]["documentoExtension"].ToString().ToLower());
                    byte[] bytFile = fileData;
                    Response.OutputStream.Write(bytFile, 0, bytFile.Length);
                    Response.AddHeader("Content-disposition", "attachment;filename=" + vDocumentoArchivo);
                    Response.End();

                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
            finally { CerrarModal("DescargaModal"); }
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GVBusqueda.PageIndex = e.NewPageIndex;
                GVBusqueda.DataSource = (DataTable)Session["DATAOSPERMISOS"];
                GVBusqueda.DataBind();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        private Decimal Calculo(DataTable vDatosSIM, int vSec) {
            Decimal vRes;
            DateTime desde, hasta;

            if (vSec < 0){
                desde = Convert.ToDateTime(TxFechaInicio.Text);
                hasta = Convert.ToDateTime(TxFechaRegreso.Text);
            }else{
                desde = Convert.ToDateTime(vDatosSIM.Rows[vSec]["fechaInicio"]);
                hasta = Convert.ToDateTime(vDatosSIM.Rows[vSec]["fechaRegreso"]);
            }
            Int32 tsDia = hasta.Day - desde.Day;
            Int32 tsHora = hasta.Hour - desde.Hour;
            Int32 tsMinuto = hasta.Minute - desde.Minute;
            TimeSpan tsHorario = Convert.ToDateTime(hasta) - Convert.ToDateTime(desde);

            int dias = 0, days = 1;
            while (desde <= hasta){
                if (desde.DayOfWeek != DayOfWeek.Saturday && desde.DayOfWeek != DayOfWeek.Sunday)
                    dias++;

                desde = desde.AddDays(1);
            }

            if (tsHorario.Days >= 1)
                days = dias; //ts.Days + 1 - 
            else if (tsHorario.Hours > 0 || tsHorario.Minutes > 0)
                days = 0;

            float vDiaSAP = float.Parse("8") / float.Parse("24") ;
            float vHorasSAP = (float.Parse(tsHorario.Hours.ToString()) / 24 ) * (vDiaSAP) ;
            vRes = Convert.ToDecimal(days + vHorasSAP);

            return vRes;
        }

        protected void BtnContinuar_Click(object sender, EventArgs e){
            try{
                if (TxToken.Text.Equals("")){
                    LbMensajeToken.Text = "Ingrese un token";
                }else{
                    LbMensajeToken.Text = String.Empty;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "$('#ModalToken').modal('hide');", true);
                    MensajeBlock("Token ingresado.", WarningType.Success);
                }

            }catch (Exception ex) { 
                Mensaje(ex.Message, WarningType.Danger); 
            }
        }

        protected void BtnCancelTk_Click(object sender, EventArgs e){
            Response.Redirect(Request.RawUrl);
        }
    }
}