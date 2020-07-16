using BiometricoWeb.clases;using System;using System.Data;using System.Web.UI;using System.Web.UI.WebControls;using System.Drawing;using System.Collections.Generic;using System.IO;using System.Linq;using System.Threading;using System.Threading.Tasks;using System.Web;using System.Configuration;namespace BiometricoWeb.pages.tiempoExtraordinario{    public partial class solicitudTE : System.Web.UI.Page    {        db vConexion;        db vConexionSysAid;
        SmtpService vService = new SmtpService();
        public void Mensaje(string vMensaje, WarningType type)        {            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);        }
        protected void Page_Load(object sender, EventArgs e)        {            vConexion = new db();            vConexionSysAid = new db();

            String vEx = Request.QueryString["ex"];            if (!IsPostBack)
            {                CargarSolicitudes();                CargarSolicitudesModificar();                DDLEmpleadoSolicitud.SelectedIndex = -1;                if (string.IsNullOrEmpty(vEx)){                    TituloPagina.Text = "Solicitud Tiempo Extraordinario";                    CargarEmpleados();                    btnDescargarHoja.Visible = false;                }else if (vEx.Equals("1")){                    TituloPagina.Text = "Aprobación Solicitud Tiempo Extraordinario Jefe";
                    nav_tecnicos_tab.Visible = false;                    nav_modificarSolicitud_tab.Visible = false;                    btnDescargarHoja.Visible = true;                    DivCrearSolicitud.Visible = false;                    DivAprobarJefe.Visible = true;                    camposDeshabilitados();                    cargarDataVista();                    calculoHoras();                }else if (vEx.Equals("2")){                    TituloPagina.Text = "Aprobación Solicitud Tiempo Extraordinario RRHH";                    nav_tecnicos_tab.Visible = false;                    nav_modificarSolicitud_tab.Visible = false;                    btnDescargarHoja.Visible = true;                    DivCrearSolicitud.Visible = false;                    DivAprobarRRHH.Visible = true;                    DivAprobarJefe.Visible = false;                    DivAprobacionRealRRHH.Visible = true;                    camposDeshabilitados();                    cargarDataVista();                    calculoHoras();                }else if (vEx.Equals("3")){                    TituloPagina.Text = "Solicitud Tiempo Extraordinario";                    limpiarCrearSolicitud();                    CargarEmpleados();                    btnDescargarHoja.Visible = false;                    String vRe = "Se ha creado con exito la solicitud de tiempo extraordinado.";                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + vRe + "')", true);                    Response.Redirect("/pages/tiempoExtraordinario/solicitudTE.aspx");                }
                else if (vEx.Equals("4")){                    limpiarCrearSolicitud();                    vEx = null;                    TituloPagina.Text = "Solicitud Tiempo Extraordinario";                    CargarEmpleados();                    btnDescargarHoja.Visible = false;                    String vRe = "No se pudo crear la solicitud de tiempo extraordinario, pongase en contacto con el administrador de la plataforma.";                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + vRe + "')", true);                }else if (vEx.Equals("5")){                    TituloPagina.Text = "Aprobación Solicitud Tiempo Extraordinario Subgerente";                    nav_tecnicos_tab.Visible = false;                    DivComentarioJefe.Visible = true;                    btnDescargarHoja.Visible = true;                    DivCrearSolicitud.Visible = false;                    DivAprobarRRHH.Visible = false;                    DivAprobarJefe.Visible = false;                    DivComentarioJefe.Visible = true;                    DivAprobacionSubgerenteSolicitud.Visible = true;                    camposDeshabilitados();                    cargarDataVista();                    calculoHoras();                }else if (vEx.Equals("6")){
                    TituloPagina.Text = "Solicitud Tiempo Extraordinario";                    CargarEmpleados();                    btnDescargarHoja.Visible = false;
                    String vRe = "Solicitud cancelada con exito.";                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + vRe + "')", true);                }else if (vEx.Equals("7")){                    TituloPagina.Text = "Modificar Solicitud Tiempo Extraordinario";                    nav_tecnicos_tab.Visible = false;
                    nav_modificarSolicitud_tab.Visible = false;                    LbModificar.Visible = true;                    DivComentarioJefe.Visible = true;                    btnDescargarHoja.Visible = true;                    DivCrearSolicitud.Visible = false;                    DivAprobarRRHH.Visible = false;                    DivAprobarJefe.Visible = false;                    DivComentarioJefe.Visible = true;                    DivSolicitudModificada.Visible = true;                    DivAprobacionSubgerenteSolicitud.Visible = false;
                    cargarDataVista();                    calculoHoras();                }else if (vEx.Equals("8")){                    vEx = null;                    TituloPagina.Text = "Solicitud Tiempo Extraordinario";                    limpiarCrearSolicitud();                    CargarEmpleados();                    btnDescargarHoja.Visible = false;                    String vRe = "Se ha modificado con exito la solicitud de tiempo extraordinado.";                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + vRe + "')", true);                }else if (vEx.Equals("9")){                    limpiarCrearSolicitud();                    vEx = null;                    TituloPagina.Text = "Solicitud Tiempo Extraordinario";                    CargarEmpleados();                    btnDescargarHoja.Visible = false;                    String vRe = "No se pudo modificar la solicitud de tiempo extraordinario, pongase en contacto con el administrador de la plataforma.";                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + vRe + "')", true);                }
                else if (vEx.Equals("10"))
                {                    TituloPagina.Text = "Solicitud Tiempo Extraordinario";                    CargarEmpleados();                    btnDescargarHoja.Visible = false;
                    String vRe = "Solicitud cancelada con exito";                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + vRe + "')", true);                }            }            Page.Form.Attributes.Add("enctype", "multipart/form-data");            ScriptManager scriptManager = ScriptManager.GetCurrent(this);            if (scriptManager != null)
            {                scriptManager.RegisterPostBackControl(BtnEnviarSolicitud);            }

        }
        void CargarEmpleados()        {            try            {
                //Seteo la opcion de "No"
                RbCambioTurno.SelectedIndex = 1;                DDLEmpleadoSolicitud.SelectedIndex = 1;

                if (Session["USUARIO"].ToString().Equals("389"))
                {
                    Titulo.Visible = true;
                    DDLEmpleadoSolicitud.Visible = true;
                    //DivDina.Visible = true;
                }
                else
                {
                    Titulo.Visible = false;
                    DDLEmpleadoSolicitud.Visible = false;
                    //DivDina.Visible = false;
                }                DataTable vDatos = new DataTable();                String vQuery = "RSP_TiempoExtraordinarioGenerales 1,'" + Convert.ToString(Session["USUARIO"]) + "'";                vDatos = vConexion.obtenerDataTable(vQuery);                TxEmpleado.Text = vDatos.Rows[0]["idEmpleado"].ToString() + " - " + vDatos.Rows[0]["nombre"].ToString();                TxJefe.Text = vDatos.Rows[0]["idJefe"].ToString() + " - " + vDatos.Rows[0]["jefeNombre"].ToString();                TxTurno.Text = vDatos.Rows[0]["nombreTurno"].ToString();                TxSubgerencia.Text = vDatos.Rows[0]["area"].ToString();                Session["TIEMPO_EX_TECNICO"] = vDatos;                Session["STECIUDAD"] = vDatos.Rows[0]["ciudad"].ToString();                Session["STETURNO"] = vDatos.Rows[0]["nombreTurno"].ToString();                Session["STEIDJEFE"] = vDatos.Rows[0]["idJefe"].ToString();                Session["STECODIGOSAP"] = vDatos.Rows[0]["idEmpleado"].ToString();                Session["STECODIGOSAPBIOMETRICO"] = vDatos.Rows[0]["codigoSAP"].ToString();                Session["STEHORAENTRADA"] = vDatos.Rows[0]["horaInicio"].ToString();                Session["STEHORASALIDA"] = vDatos.Rows[0]["horaFinal"].ToString();                Session["STENOMBREJEFE"] = vDatos.Rows[0]["jefeNombre"].ToString();                Session["STEDIASTURNO"] = vDatos.Rows[0]["dias"].ToString();                Session["STEPUESTOJEFE"] = vDatos.Rows[0]["puestoJefe"].ToString();                Session["STEPUESTOCOLABORADOR"] = vDatos.Rows[0]["idPuesto"].ToString();                Session["STESUBGERENCIA"] = vDatos.Rows[0]["subGerencia"].ToString();
                Session["STEIDJEFESUBGERENCIA"] = vDatos.Rows[0]["subgerente"].ToString();

                //DECISION PARA OCULTAR O MOSTRAR LAS OPCIONES DE CONDUCTORES
                if (Session["STEPUESTOJEFE"].ToString().Equals("20000383") || Session["STEPUESTOJEFE"].ToString().Equals("20000395") || Session["STEPUESTOCOLABORADOR"].ToString().Equals("20000409"))
                {
                    DivConductor.Visible = false;                }
                else
                {                    DivConductor.Visible = true;                }

                //DECISION PARA OCULTAR O MOSTRAR EL CAMPO DE SYSAID Y HOJA DE SERVICIO
                if (Session["STESUBGERENCIA"].ToString().Equals("4") || Session["STESUBGERENCIA"].ToString().Equals("2"))
                {                    DivSysAid.Visible = true;                    lbHojaServicio.Visible = true;                    FuHojaServicio.Visible = true;                    btnVisualizarHoja.Visible = true;
                }                else
                {                    DivSysAid.Visible = false;                    lbHojaServicio.Visible = false;                    FuHojaServicio.Visible = false;                    btnVisualizarHoja.Visible = false;
                }
                String vTest = string.Empty;


                //LLENAR LISTA DESPLEGABLE PARA DINA 
                vQuery = "RSP_TiempoExtraordinarioGenerales 40";                vDatos = vConexion.obtenerDataTable(vQuery);
                DDLEmpleadoSolicitud.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });                foreach (DataRow item in vDatos.Rows)                {                    DDLEmpleadoSolicitud.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["idEmpleado"].ToString() + " - " + item["nombre"].ToString() });                }



                //LLENAR LISTA DESPLEGABLE DE COLABORADORES DEL MISMO JEFE PARA CAMBIO DE TURNO
                DDLCambioTurnoColaborador.Items.Clear();                vQuery = "RSP_TiempoExtraordinarioGenerales 2,'" + Session["STEIDJEFE"] + "'";                vDatos = vConexion.obtenerDataTable(vQuery);                DDLCambioTurnoColaborador.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });                foreach (DataRow item in vDatos.Rows)                {                    DDLCambioTurnoColaborador.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["idEmpleado"].ToString() + " - " + item["nombre"].ToString() });                }                DdlTipoTrabajo.Items.Clear();                vQuery = "RSP_TiempoExtraordinarioGenerales 3";                vDatos = vConexion.obtenerDataTable(vQuery);                DdlTipoTrabajo.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });                foreach (DataRow item in vDatos.Rows)                {                    DdlTipoTrabajo.Items.Add(new ListItem { Value = item["idTipoTrabajo"].ToString(), Text = item["nombreTrabajo"].ToString() });                }            }            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }        }
        protected void DDLCambioTurnoColaborador_SelectedIndexChanged(object sender, EventArgs e)        {            try            {                DataTable vDatos = new DataTable();                String vQuery = "RSP_TiempoExtraordinarioGenerales 1,'" + DDLCambioTurnoColaborador.SelectedItem.Value + "'";                vDatos = vConexion.obtenerDataTable(vQuery);                TxCambioTurno.Text = vDatos.Rows[0]["nombreTurno"].ToString();                Session["STETURNO"] = vDatos.Rows[0]["nombreTurno"].ToString();                Session["STEHORAENTRADA"] = vDatos.Rows[0]["horaInicio"].ToString();                Session["STEHORASALIDA"] = vDatos.Rows[0]["horaFinal"].ToString();
                Session["STEDIASTURNO"] = vDatos.Rows[0]["dias"].ToString();                Session["STEIDTURNOCAMBIO"] = vDatos.Rows[0]["idTurno"].ToString();            }            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }        }
        protected void RbCambioTurno_SelectedIndexChanged(object sender, EventArgs e)        {            try            {
                String vEx = Request.QueryString["ex"];                if (RbCambioTurno.SelectedValue.Equals("1")){
                    rowSolicitudCambioTurno.Visible = true;                    LimpiarRowSolicitudCambioTurno();

                    if (!string.IsNullOrEmpty(vEx)){
                        if (vEx.Equals("7")){
                            limpiarModificarSolicitud();
                            TxFechaInicio.Text = String.Empty;
                            TxFechaRegreso.Text = String.Empty;
                            RbFormaTrabajo.SelectedIndex = -1;

                            DataTable vDatosSolicitud = new DataTable();
                            vDatosSolicitud = (DataTable)Session["STEDATOSSOLICITUDINDIVIDUAL"];
                            string vCambioTurno = vDatosSolicitud.Rows[0]["cambioTurno"].ToString();
                            TxMotivoCambioTurno.Text = vDatosSolicitud.Rows[0]["motivoCambioTurno"].ToString();
                            if (vCambioTurno == "1")
                            {
                                DDLCambioTurnoColaborador.SelectedValue = vDatosSolicitud.Rows[0]["codigoSapCambioTurno"].ToString();
                                DataTable vDatos = new DataTable();
                                String vQuery = "RSP_TiempoExtraordinarioGenerales 1,'" + DDLCambioTurnoColaborador.SelectedValue + "'";
                                vDatos = vConexion.obtenerDataTable(vQuery);

                                Session["STETURNO"] = vDatos.Rows[0]["nombreTurno"].ToString();
                                Session["STEHORAENTRADA"] = vDatos.Rows[0]["horaInicio"].ToString();
                                Session["STEHORASALIDA"] = vDatos.Rows[0]["horaFinal"].ToString();
                                Session["STEDIASTURNO"] = vDatos.Rows[0]["dias"].ToString();

                                TxCambioTurno.Text = vDatos.Rows[0]["nombreTurno"].ToString();

                                string vFormato = "yyyy-MM-ddTHH:mm";
                                string vFechaInicio = vDatosSolicitud.Rows[0]["fechaInicio"].ToString();
                                string vFechaFin = vDatosSolicitud.Rows[0]["fechaFin"].ToString();
                                string vFechaInicioConvertida = Convert.ToDateTime(vFechaInicio).ToString(vFormato);
                                string vFechaFinConvertida = Convert.ToDateTime(vFechaFin).ToString(vFormato);

                                TxFechaInicio.Text = vFechaInicioConvertida;
                                TxFechaRegreso.Text = vFechaFinConvertida;
                                RbFormaTrabajo.SelectedValue = vDatosSolicitud.Rows[0]["formaTrabajo"].ToString();
                                UpdatePanelFechas.Update();

                                calculoHoras();
                                DivUnaFecha.Visible = true;
                                UpDivUnaFecha.Update();
                            }
                        }
                    }
                                        }else{

                    rowSolicitudCambioTurno.Visible = false;                    LimpiarRowSolicitudCambioTurno();                    DataTable vDatos = new DataTable();                    String vQuery = "RSP_TiempoExtraordinarioGenerales 1,'" + Convert.ToString(Session["USUARIO"]) + "'";                    vDatos = vConexion.obtenerDataTable(vQuery);                    Session["STETURNO"] = vDatos.Rows[0]["nombreTurno"].ToString();                    Session["STEHORAENTRADA"] = vDatos.Rows[0]["horaInicio"].ToString();                    Session["STEHORASALIDA"] = vDatos.Rows[0]["horaFinal"].ToString();                    Session["STEDIASTURNO"] = vDatos.Rows[0]["dias"].ToString();

                    if (!string.IsNullOrEmpty(vEx)){
                        if (vEx.Equals("7")){
                            DataTable vDatosSolicitud = new DataTable();
                            vDatosSolicitud = (DataTable)Session["STEDATOSSOLICITUDINDIVIDUAL"];

                            string vFormato = "yyyy-MM-ddTHH:mm";
                            string vFechaInicio = vDatosSolicitud.Rows[0]["fechaInicio"].ToString();
                            string vFechaFin = vDatosSolicitud.Rows[0]["fechaFin"].ToString();
                            string vFechaInicioConvertida = Convert.ToDateTime(vFechaInicio).ToString(vFormato);
                            string vFechaFinConvertida = Convert.ToDateTime(vFechaFin).ToString(vFormato);
                            rowSolicitudCambioTurno.Visible = false;
                            TxFechaInicio.Text = vFechaInicioConvertida;
                            TxFechaRegreso.Text = vFechaFinConvertida;
                            RbFormaTrabajo.SelectedValue = vDatosSolicitud.Rows[0]["formaTrabajo"].ToString();
                            UpdatePanelFechas.Update();
                            calculoHoras();
                            DivUnaFecha.Visible = true;
                            UpDivUnaFecha.Update();
                        }
                    }
                }            }catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }        }
        private void LimpiarRowSolicitudCambioTurno()        {            DDLCambioTurnoColaborador.SelectedIndex = -1;            TxCambioTurno.Text = String.Empty;            TxMotivoCambioTurno.Text = String.Empty;        }
        protected void TxPeticion_TextChanged(object sender, EventArgs e)        {            try            {                DataTable vDatos = new DataTable();                String vQuery = "RSP_TiempoExtraordinarioGeneralesBiometrico 1,'" + TxPeticion.Text + "'";                vDatos = vConexionSysAid.obtenerDataTableSysAid(vQuery);                if (vDatos.Rows.Count > 0)                {                    TxTituloSysaid.Text = vDatos.Rows[0]["title"].ToString() + " - (" + vDatos.Rows[0]["categoria"].ToString() + " )";                }                else                {                    TxTituloSysaid.Text.Equals("");                }
            }            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }        }
        protected void TxFechaInicio_TextChanged(object sender, EventArgs e)        {            try            {

                DateTime vHIS = Convert.ToDateTime(TxFechaInicio.Text);                Session["STEHRINICIO"] = vHIS.Hour + ":" + vHIS.Minute;                validacionInicioTurno();                calculoHoras();            }            catch (Exception Ex)            {                String vEx = Request.QueryString["ex"];                Mensaje(Ex.Message, WarningType.Danger);                TxFechaInicio.Text = String.Empty;                LbFechaRangoBien.Visible = false;                LbFechaRangoMal.Visible = false;                if (!string.IsNullOrEmpty(vEx)){
                    if (vEx.Equals("7")){
                        limpiarModificarSolicitud();
                    }else{
                        limpiarCrearSolicitud();
                    }
                }                

            }        }
        protected void TxFechaRegreso_TextChanged(object sender, EventArgs e)        {            try            {                DateTime vHFS = Convert.ToDateTime(TxFechaRegreso.Text);                Session["STEHRFIN"] = vHFS.Hour + ":" + vHFS.Minute;                validacionFinTurno();                calculoHoras();            }            catch (Exception Ex)            {                String vEx = Request.QueryString["ex"];                Mensaje(Ex.Message, WarningType.Danger);                TxFechaRegreso.Text = String.Empty;
                LbFechaRangoBien.Visible = false;                LbFechaRangoMal.Visible = false;

                if (!string.IsNullOrEmpty(vEx)){
                    if (vEx.Equals("7")){
                        limpiarModificarSolicitud();
                    }else{
                        limpiarCrearSolicitud();
                    }
                }
            }        }
        void validacionInicioTurno()        {            if (RbFormaTrabajo.SelectedValue.Equals(""))
                throw new Exception("Falta que seleccione como realizo el trabajo si de forma remota o presencial ");

            string vFechaActual = DateTime.Today.ToString("dd/MM/yyyy");

            DateTime vtoday = DateTime.Now;            String vFormato = "dd/MM/yyyy"; //"dd/MM/yyyy HH:mm:ss"
            String vFormatoBio = "yyyy-MM-dd"; //"dd/MM/yyyy HH:mm:ss"
            String vFormatoHrInicial = "HH:mm:ss"; //"dd/MM/yyyy HH:mm:ss"

            DateTime vHIS = Convert.ToDateTime(TxFechaInicio.Text);            String vFechaInicial = Convert.ToDateTime(vHIS).ToString(vFormato);
            String vHrInicial = Convert.ToDateTime(vHIS).ToString(vFormatoHrInicial);
            int diaInicio = vHIS.Day;            int mesInicio = vHIS.Month;            int añoInicio = vHIS.Year;

            String vIniBio = Convert.ToDateTime(vHIS).ToString(vFormatoBio);

            //Dia de la semana
            string dia = vHIS.ToString("dddd");            string STEDIASTURNO = Session["STEDIASTURNO"].ToString();            STEDIASTURNO = STEDIASTURNO.Replace(',', '-');            string STEHORAENTRADA = Session["STEHORAENTRADA"].ToString();            string STEHORASALIDA = Session["STEHORASALIDA"].ToString();            string vHoraInicioTurno = STEHORAENTRADA.Substring(0, 2);            string vMinutoInicioTurno = STEHORAENTRADA.Substring(3, 2);            string vHoraFinTurno = STEHORASALIDA.Substring(0, 2);            string vMinutoFinTurno = STEHORASALIDA.Substring(3, 2);            DateTime vHIT = DateTime.Now;            DateTime vHFT = DateTime.Now;            vHIT = new DateTime(añoInicio, mesInicio, diaInicio, Convert.ToInt32(vHoraInicioTurno), Convert.ToInt32(vMinutoInicioTurno), 00);            vHFT = new DateTime(añoInicio, mesInicio, diaInicio, Convert.ToInt32(vHoraFinTurno), Convert.ToInt32(vMinutoFinTurno), 00);


            string vFeriadovFI = "no";

            //FERIADO FECHA INICIAL
            //FERIADO GLOBAL
            DataTable vDatos = new DataTable();
            String vQuery = "RSP_TiempoExtraordinarioGenerales 15," + diaInicio + "," + mesInicio;
            vDatos = vConexion.obtenerDataTable(vQuery);

            if (vDatos.Rows.Count > 0)
            {
                vFeriadovFI = "si";
                Session["STEFERIADOGLOBAL"] = vDatos.Rows[0]["feriadoGlobal"].ToString();
                Session["STECIUDADFERIADO"] = vDatos.Rows[0]["ciudad"].ToString();
            }

            //FERIADO REGIONAL
            vQuery = "RSP_TiempoExtraordinarioGenerales 16," + diaInicio + "," + mesInicio + ",'" + Session["STECIUDAD"].ToString() + "'";
            vDatos = vConexion.obtenerDataTable(vQuery);

            if (vDatos.Rows.Count > 0)
            {
                vFeriadovFI = "si";
                Session["STEFERIADOGLOBAL"] = vDatos.Rows[0]["feriadoGlobal"].ToString();
                Session["STECIUDADFERIADO"] = vDatos.Rows[0]["ciudad"].ToString();
            }


            vQuery = "SELECT dbo.ObtenerHoraEntrada('" + Session["STECODIGOSAPBIOMETRICO"] + "','" + vIniBio + "') horaentrada";
            vDatos = vConexion.obtenerDataTable(vQuery);

            string HrBiometricoEntrada = vDatos.Rows[0]["horaentrada"].ToString();
            TimeSpan vHrBiometricoEntrada = TimeSpan.Parse(HrBiometricoEntrada);
            TimeSpan vTimeHrsInicioSolicitud = TimeSpan.Parse(vHrInicial);
            if ((vTimeHrsInicioSolicitud < vHrBiometricoEntrada) && RbFormaTrabajo.SelectedValue.Equals("2"))
                throw new Exception("Favor verificar la hora de la solicitud, Su hora de entrada fue a las: " + vHrBiometricoEntrada + " y su hora de incio de la solicitud es: " + vTimeHrsInicioSolicitud + " las horas no cuadran, la hora inicial de la solicitud debe ser igual o mayor a la hora de entrada del biometrico.");

            if (vFechaInicial == vFechaActual)
                throw new Exception("La fecha inicial es igual al dia de hoy, debe ingresar la solicitud hasta el dia de mañana, debe esperar que finalice todo el ciclo de la jornada para que pueda visualizar las entradas, salidas, disminución de entradas tarde, salidas tempranas y disminucion de almuerzo.");

            if (STEDIASTURNO != "L-V" && STEDIASTURNO != "L-D" && STEDIASTURNO != "L-M-V-D")                throw new Exception("Favor contactarse con el administrador de la plataforma y notifiquele que la combinación de días: " + STEDIASTURNO + " del turno asignado o si realizo cambio: " + Session["STETURNO"].ToString() + ", no esta registrado en la plataforma.");

            if (vHIS > vtoday)
                throw new Exception("La fecha y hora inicial seleccionada es mayor a la fecha y hora actual del sistema, primero debe realizar el trabajo para posterior ingreso del tiempo extraordinario");


            if ((vHIS > vHIT && vHIS < vHFT) && STEDIASTURNO == "L-V" && (dia == "lunes" || dia == "martes" || dia == "miércoles" || dia == "jueves" || dia == "viernes") && vFeriadovFI == "no")
            {                throw new Exception("Hemos detectado un traslape con su hora inicial de solicitud: " + vHIS.ToString("dd/MM/yyyy HH:mm:ss") + ". Su turno asignado o si realizo cambio es: " + Session["STETURNO"].ToString() + " , favor verificar que su hora inicial de solicitud sea menor que su hora inicio turno o su hora inicial de solicitud sea mayor que su hora final de turno");            }            else if ((vHIS > vHIT && vHIS < vHFT) && STEDIASTURNO == "L-D" && (dia == "lunes" || dia == "martes" || dia == "miércoles" || dia == "jueves" || dia == "viernes" || dia == "sábado" || dia == "domingo") && vFeriadovFI == "no")            {                throw new Exception("Hemos detectado un traslape con su hora inicial de solicitud: " + vHIS.ToString("dd/MM/yyyy HH:mm:ss") + ". Su turno asignado o si realizo cambio es: " + Session["STETURNO"].ToString() + " , favor verificar que su hora inicial de solicitud sea menor que su hora inicio turno o su hora inicial de solicitud sea mayor que su hora final de turno");            }            else if ((vHIS > vHIT && vHIS < vHFT) && STEDIASTURNO == "L-M-V-D" && (dia == "lunes" || dia == "miércoles" || dia == "viernes" || dia == "domingo") && vFeriadovFI == "no")            {                throw new Exception("Hemos detectado un traslape con su hora inicial de solicitud: " + vHIS.ToString("dd/MM/yyyy HH:mm:ss") + ". Su turno asignado o si realizo cambio es: " + Session["STETURNO"].ToString() + " , favor verificar que su hora inicial de solicitud sea menor que su hora inicio turno o su hora inicial de solicitud sea mayor que su hora final de turno");            }


            String vEx = Request.QueryString["ex"];
            if (!string.IsNullOrEmpty(vEx)){
                if (vEx.Equals("7")){
                    DataTable vDatosSolicitud = new DataTable();
                    vDatosSolicitud = (DataTable)Session["STEDATOSSOLICITUDINDIVIDUAL"];

                    //DIV CUANDO LA SOLICITUD ESTA FUERA DE RANGO
                    if (vDatosSolicitud.Rows[0]["aprobacionSubgerente"].ToString() == "1"){
                        LbFechaRangoMal.Visible = true;
                        DivAprobacionSubGerente.Visible = true;
                    }else{
                        LbFechaRangoBien.Visible = true;
                        DivAprobacionSubGerente.Visible = false;
                    }                }
            }
        }
        void validacionFinTurno()        {
            String vEx = Request.QueryString["ex"];

            if (RbFormaTrabajo.SelectedValue.Equals(""))
                throw new Exception("Falta que seleccione como realizo el trabajo si de forma remota o presencial ");
            Session["STEAPROBACIONSUBGERENTE"] = false;

            DateTime vtoday = DateTime.Now;
            string vFechaActual = vtoday.ToString("dd/MM/yyyy");
            //string vFechaActual = "13/06/2020";

            DateTime vHFS = Convert.ToDateTime(TxFechaRegreso.Text);
            String vFormato = "dd/MM/yyyy"; //"dd/MM/yyyy HH:mm:ss"
            String vFechaFinal = Convert.ToDateTime(vHFS).ToString(vFormato);
            byte vdiaEvaluar = (byte)vHFS.DayOfWeek;// Resultado: 1
            string vHFSMas3Dia = "";            string vHFSMas2Dia = "";            if (vdiaEvaluar == 5)//Viernes
            {                DateTime HFSMas3Dia = vHFS.AddDays(3);
                vHFSMas3Dia = HFSMas3Dia.ToString("dd/MM/yyyy");            }
            else if (vdiaEvaluar == 6)//Sabado
            {                DateTime HFSMas2Dia = vHFS.AddDays(2);                vHFSMas2Dia = HFSMas2Dia.ToString("dd/MM/yyyy");            }            DateTime HFSMas1Dia = vHFS.AddDays(1);            string vHFSMas1Dia = HFSMas1Dia.ToString("dd/MM/yyyy");            DateTime HFSMas2DiaOtrasSubgerencias = vHFS.AddDays(2);            string vHFSMas2DiaOtrasSubgerencias = HFSMas2DiaOtrasSubgerencias.ToString("dd/MM/yyyy");            int diaInicio = vHFS.Day;            int mesInicio = vHFS.Month;            int añoInicio = vHFS.Year;

            string vFeriadovFF = "no";

            DataTable vDatos = new DataTable();
            //FERIADO FECHA FINAL
            //FERIADO GLOBAL
            string vQuery = "RSP_TiempoExtraordinarioGenerales 15," + diaInicio + "," + mesInicio;
            vDatos = vConexion.obtenerDataTable(vQuery);

            if (vDatos.Rows.Count > 0)
            {
                vFeriadovFF = "si";
                Session["STEFERIADOGLOBAL"] = vDatos.Rows[0]["feriadoGlobal"].ToString();
                Session["STECIUDADFERIADO"] = vDatos.Rows[0]["ciudad"].ToString();
            }

            //FERIADO REGION
            vQuery = "RSP_TiempoExtraordinarioGenerales 16," + diaInicio + "," + mesInicio + ",'" + Session["STECIUDAD"].ToString() + "'";
            vDatos = vConexion.obtenerDataTable(vQuery);

            if (vDatos.Rows.Count > 0)
            {
                vFeriadovFF = "si";
                Session["STEFERIADOGLOBAL"] = vDatos.Rows[0]["feriadoGlobal"].ToString();
                Session["STECIUDADFERIADO"] = vDatos.Rows[0]["ciudad"].ToString();
            }


            //Dia de la semana
            string dia = vHFS.ToString("dddd");            string STEDIASTURNO = Session["STEDIASTURNO"].ToString();            STEDIASTURNO = STEDIASTURNO.Replace(',', '-');            string STEHORAENTRADA = Session["STEHORAENTRADA"].ToString();            string STEHORASALIDA = Session["STEHORASALIDA"].ToString();            string vHoraInicioTurno = STEHORAENTRADA.Substring(0, 2);            string vMinutoInicioTurno = STEHORAENTRADA.Substring(3, 2);            string vHoraFinTurno = STEHORASALIDA.Substring(0, 2);            string vMinutoFinTurno = STEHORASALIDA.Substring(3, 2);            DateTime vHIT = DateTime.Now;            DateTime vHFT = DateTime.Now;            vHIT = new DateTime(añoInicio, mesInicio, diaInicio, Convert.ToInt32(vHoraInicioTurno), Convert.ToInt32(vMinutoInicioTurno), 00);            vHFT = new DateTime(añoInicio, mesInicio, diaInicio, Convert.ToInt32(vHoraFinTurno), Convert.ToInt32(vMinutoFinTurno), 00);



            if (vFechaFinal == vFechaActual)
                throw new Exception("La fecha final es igual al dia de hoy ,debe ingresar la solicitud hasta el dia de mañana, debe esperar que finalice todo el ciclo de la jornada para que pueda visualizar las entradas, salidas, disminución de entradas tarde, salidas tempranas y disminucion de almuerzo.");            if (string.IsNullOrEmpty(vEx) || vEx != "7"){
                if (Session["STESUBGERENCIA"].ToString().Equals("4") || Session["STESUBGERENCIA"].ToString().Equals("2")){
                    if (vHFSMas1Dia == vFechaActual || vHFSMas3Dia == vFechaActual || vHFSMas2Dia == vFechaActual){
                        DivAprobacionSubGerente.Visible = false;
                        LbFechaRangoBien.Visible = true;
                        LbFechaRangoMal.Visible = false;
                        UpdatePanelFechas.Update();
                    }else{
                        Session["STEAPROBACIONSUBGERENTE"] = true;
                        DivAprobacionSubGerente.Visible = true;
                        LbFechaRangoBien.Visible = false;
                        LbFechaRangoMal.Visible = true;
                        UpdatePanelFechas.Update();
                    }
                }else{
                    if (vHFSMas1Dia == vFechaActual || vHFSMas2DiaOtrasSubgerencias == vFechaActual || vHFSMas3Dia == vFechaActual || vHFSMas2Dia == vFechaActual){
                        DivAprobacionSubGerente.Visible = false;
                        LbFechaRangoBien.Visible = true;
                        LbFechaRangoMal.Visible = false;
                        UpdatePanelFechas.Update();
                    }else{
                        Session["STEAPROBACIONSUBGERENTE"] = true;
                        DivAprobacionSubGerente.Visible = true;
                        LbFechaRangoBien.Visible = false;
                        LbFechaRangoMal.Visible = true;
                        UpdatePanelFechas.Update();
                    }
                }

            }else if(vEx == "7"){
                DataTable vDatosSolicitud = (DataTable)Session["STEDATOSSOLICITUDINDIVIDUAL"];
                if (vDatosSolicitud.Rows[0]["aprobacionSubgerente"].ToString() == "1"){
                    LbFechaRangoMal.Visible = true;
                    DivAprobacionSubGerente.Visible = true;
                }else{
                    LbFechaRangoBien.Visible = true;
                    DivAprobacionSubGerente.Visible = false;
                }
                //DIV CUANDO LA SOLICITUD ESTA FUERA DE RANGO
            }

            if (STEDIASTURNO != "L-V" && STEDIASTURNO != "L-D" && STEDIASTURNO != "L-M-V-D")                throw new Exception("Favor contactarse con el administrador de la plataforma y notifiquele que la combinación de días: " + STEDIASTURNO + " del turno asignado o si realizo cambio: " + Session["STETURNO"].ToString() + ", no esta registrado en la plataforma.");

            if (vHFS > vtoday)                throw new Exception("La fecha y hora final seleccionada es mayor a la fecha y hora actual del sistema, primero debe finalizar el trabajo para posterior ingreso del tiempo extraordinario");

            if ((vHFS > vHIT && vHFS < vHFT) && STEDIASTURNO == "L-V" && (dia == "lunes" || dia == "martes" || dia == "miércoles" || dia == "jueves" || dia == "viernes") && vFeriadovFF == "no")            {                throw new Exception("Hemos detectado un traslape con su hora inicial de solicitud: " + vHFS.ToString("dd/MM/yyyy HH:mm:ss") + " del turno asignado o si realizo cambio: " + Session["STETURNO"].ToString() + " , favor verificar que su hora inicial de solicitud sea menor que su hora inicio turno o su hora inicial de solicitud sea mayor que su hora final de turno");            }            else if ((vHFS > vHIT && vHFS < vHFT) && STEDIASTURNO == "L-D" && (dia == "lunes" || dia == "martes" || dia == "miércoles" || dia == "jueves" || dia == "viernes" || dia == "sábado" || dia == "domingo") && vFeriadovFF == "no")            {                throw new Exception("Hemos detectado un traslape con su hora inicial de solicitud: " + vHFS.ToString("dd/MM/yyyy HH:mm:ss") + " del turno asignado o si realizo cambio: " + Session["STETURNO"].ToString() + " , favor verificar que su hora inicial de solicitud sea menor que su hora inicio turno o su hora inicial de solicitud sea mayor que su hora final de turno");            }            else if ((vHFS > vHIT && vHFS < vHFT) && STEDIASTURNO == "L-M-V-D" && (dia == "lunes" || dia == "miércoles" || dia == "viernes" || dia == "domingo") && vFeriadovFF == "no")            {                throw new Exception("Hemos detectado un traslape con su hora inicial de solicitud: " + vHFS.ToString("dd/MM/yyyy HH:mm:ss") + " del turno asignado o si realizo cambio: " + Session["STETURNO"].ToString() + " , favor verificar que su hora inicial de solicitud sea menor que su hora inicio turno o su hora inicial de solicitud sea mayor que su hora final de turno");            }        }
        void ValidacionesTE(DateTime vFechaInicio, DateTime vFechaRegreso, int dias)        {
            if (vFechaRegreso < vFechaInicio)                throw new Exception("Las fechas ingresadas son incorrectas, el regreso es menor que el inicio");            if (dias > 1)                throw new Exception("Debe solicitar tiempo extraordinario que este en el rango establecido de 24 Hrs");
        }
        void calculoHoras()        {            if (TxFechaInicio.Text != "" && TxFechaRegreso.Text != "")            {                String vEx = Request.QueryString["ex"];                String vFI = TxFechaInicio.Text != "" ? TxFechaInicio.Text : "1999-01-01 00:00:00";                String vFF = TxFechaRegreso.Text != "" ? TxFechaRegreso.Text : "1999-01-01 00:00:00";                DateTime vdesde = Convert.ToDateTime(vFI);                DateTime vhasta = Convert.ToDateTime(vFF);


                String vFormatoFechaHr = "dd/MM/yyyy HH:mm:ss"; //"DD/MM/YYYY HH:mm:ss"
                String vFormato = "dd/MM/yyyy"; //"DD/MM/YYYY HH:mm:ss"
                String vFormatoBio = "yyyy-MM-dd"; //"dd/MM/yyyy HH:mm:ss"

                String vIni = Convert.ToDateTime(vdesde).ToString(vFormato);                String vInHr = Convert.ToDateTime(vdesde).ToString(vFormatoFechaHr);                String vIniBio = Convert.ToDateTime(vdesde).ToString(vFormatoBio);                Session["STEHRINICIAL"] = vIniBio;                String vFin = Convert.ToDateTime(vhasta).ToString(vFormato);
                String vFinHr = Convert.ToDateTime(vhasta).ToString(vFormatoFechaHr);                String vFinBio = Convert.ToDateTime(vhasta).ToString(vFormatoBio);                TimeSpan difFechas = Convert.ToDateTime(vFin) - Convert.ToDateTime(vIni);                int dias = difFechas.Days;                ValidacionesTE(vdesde, vhasta, dias);


                if (string.IsNullOrEmpty(vEx)){
                    DataTable vDatos1 = new DataTable();
                    string vQuery1 = "RSP_TiempoExtraordinarioGenerales 41,'" + vInHr + "','" + vFinHr + "','" + Session["STECODIGOSAP"].ToString() + "'";
                    vDatos1 = vConexion.obtenerDataTable(vQuery1);

                    if (vDatos1.Rows.Count > 0)
                        throw new Exception("Usted ya tiene ingresado una solicitud en el rango de fechas y horas detalladas fecha y hora de inicio: " + vInHr + " fecha y hora final: " + vFinHr + " .Revisar la solicitud No. en la sección Mis Solicitudes: " + vDatos1.Rows[0]["idSolicitud"].ToString());
                }

                DateTime vHIS = Convert.ToDateTime(TxFechaInicio.Text);                int diaInicio = vHIS.Day;                int mesInicio = vHIS.Month;                int añoInicio = vHIS.Year;
                Session["STEHRINICIO"] = vHIS.Hour + ":" + vHIS.Minute;                DateTime vHFS = Convert.ToDateTime(TxFechaRegreso.Text);                int diaFin = vHFS.Day;                int mesFin = vHFS.Month;                int añoFin = vHFS.Year;
                Session["STEHRFIN"] = vHFS.Hour + ":" + vHFS.Minute;

                //JORNADAS HORA INICIO
                DateTime vInicioNocNoc1 = DateTime.Now;                DateTime vFinNocNoc1 = DateTime.Now;                DateTime vInicioDiurna = DateTime.Now;                DateTime vFinDiurna = DateTime.Now;                DateTime vInicioNoc = DateTime.Now;                DateTime vFinNoc = DateTime.Now;                DateTime vInicioNocNoc2 = DateTime.Now;                DateTime vFinNocNoc2 = DateTime.Now;                string vEstadoHIS = "";                string vEstadoHFS = "";                string vDomingovFI = "no";                string vDomingovFF = "no";                if (vdesde.DayOfWeek == DayOfWeek.Sunday)                {                    vDomingovFI = "si";                }                if (vhasta.DayOfWeek == DayOfWeek.Sunday)                {                    vDomingovFF = "si";                }                string vFeriadovFI = "no";                string vFeriadovFF = "no";

                //FERIADO FECHA INICIAL
                //FERIADO GLOBAL
                DataTable vDatos = new DataTable();                string vQuery = "RSP_TiempoExtraordinarioGenerales 15," + diaInicio + "," + mesInicio;                vDatos = vConexion.obtenerDataTable(vQuery);                if (vDatos.Rows.Count > 0)                {                    vFeriadovFI = "si";                    Session["STEFERIADOGLOBAL"] = vDatos.Rows[0]["feriadoGlobal"].ToString();                    Session["STECIUDADFERIADO"] = vDatos.Rows[0]["ciudad"].ToString();                }

                //FERIADO REGIONAL
                vQuery = "RSP_TiempoExtraordinarioGenerales 16," + diaInicio + "," + mesInicio + ",'" + Session["STECIUDAD"].ToString() + "'";                vDatos = vConexion.obtenerDataTable(vQuery);                if (vDatos.Rows.Count > 0)                {                    vFeriadovFI = "si";                    Session["STEFERIADOGLOBAL"] = vDatos.Rows[0]["feriadoGlobal"].ToString();                    Session["STECIUDADFERIADO"] = vDatos.Rows[0]["ciudad"].ToString();                }

                //FERIADO FECHA FINAL
                //FERIADO GLOBAL
                vQuery = "RSP_TiempoExtraordinarioGenerales 15," + diaFin + "," + mesFin;                vDatos = vConexion.obtenerDataTable(vQuery);                if (vDatos.Rows.Count > 0)                {                    vFeriadovFF = "si";                    Session["STEFERIADOGLOBAL"] = vDatos.Rows[0]["feriadoGlobal"].ToString();                    Session["STECIUDADFERIADO"] = vDatos.Rows[0]["ciudad"].ToString();                }

                //FERIADO REGION
                vQuery = "RSP_TiempoExtraordinarioGenerales 16," + diaFin + "," + mesFin + ",'" + Session["STECIUDAD"].ToString() + "'";                vDatos = vConexion.obtenerDataTable(vQuery);                if (vDatos.Rows.Count > 0)                {                    vFeriadovFF = "si";                    Session["STEFERIADOGLOBAL"] = vDatos.Rows[0]["feriadoGlobal"].ToString();                    Session["STECIUDADFERIADO"] = vDatos.Rows[0]["ciudad"].ToString();                }                if (dias == 0)                {                    vInicioNocNoc1 = new DateTime(añoInicio, mesInicio, diaInicio, 00, 00, 00);                    vFinNocNoc1 = new DateTime(añoFin, mesFin, diaFin, 05, 00, 00);                    vInicioDiurna = new DateTime(añoInicio, mesInicio, diaInicio, 05, 00, 00);                    vFinDiurna = new DateTime(añoFin, mesFin, diaFin, 19, 00, 00);                    vInicioNoc = new DateTime(añoInicio, mesInicio, diaInicio, 19, 00, 00);                    vFinNoc = new DateTime(añoFin, mesFin, diaFin, 22, 00, 00);                    vInicioNocNoc2 = new DateTime(añoInicio, mesInicio, diaInicio, 22, 00, 00);                    vFinNocNoc2 = new DateTime(añoFin, mesFin, diaFin, 23, 59, 00);


                    //ESTADO PARA SABER DONDE COMIENZA LA SOLICITUD
                    if (vHIS >= vInicioNocNoc1 && vHIS <= vFinNocNoc1)                    {                        vEstadoHIS = "NocNoc1";                    }                    else if (vHIS >= vInicioDiurna && vHIS <= vFinDiurna)                    {                        vEstadoHIS = "Diurnas";                    }                    else if (vHIS >= vInicioNoc && vHIS <= vFinNoc)                    {                        vEstadoHIS = "Noc";                    }                    else if (vHIS >= vInicioNocNoc2 && vHIS <= vFinNocNoc2)                    {                        vEstadoHIS = "NocNoc2";                    }

                    //ESTADO PARA SABER DONDE FINALIZA LA SOLICITUD
                    if (vHFS >= vInicioNocNoc1 && vHFS <= vFinNocNoc1)                    {                        vEstadoHFS = "NocNoc1";                    }                    else if (vHFS >= vInicioDiurna && vHFS <= vFinDiurna)                    {                        vEstadoHFS = "Diurnas";                    }                    else if (vHFS >= vInicioNoc && vHFS <= vFinNoc)                    {                        vEstadoHFS = "Noc";                    }                    else if (vHFS >= vInicioNocNoc2 && vHFS <= vFinNocNoc2)                    {                        vEstadoHFS = "NocNoc2";                    }                }                else                {                    vInicioNocNoc1 = new DateTime(añoInicio, mesInicio, diaInicio, 00, 00, 00);                    vFinNocNoc1 = new DateTime(añoInicio, mesInicio, diaInicio, 05, 00, 00);                    vInicioDiurna = new DateTime(añoInicio, mesInicio, diaInicio, 05, 00, 00);                    vFinDiurna = new DateTime(añoInicio, mesInicio, diaInicio, 19, 00, 00);                    vInicioNoc = new DateTime(añoInicio, mesInicio, diaInicio, 19, 00, 00);                    vFinNoc = new DateTime(añoInicio, mesInicio, diaInicio, 22, 00, 00);                    vInicioNocNoc2 = new DateTime(añoInicio, mesInicio, diaInicio, 22, 00, 00);                    vFinNocNoc2 = new DateTime(añoInicio, mesInicio, diaInicio, 23, 59, 00);

                    //ESTADO PARA SABER DONDE COMIENZA LA SOLICITUD
                    if (vHIS >= vInicioNocNoc1 && vHIS <= vFinNocNoc1)                    {                        vEstadoHIS = "NocNoc1";                    }                    else if (vHIS >= vInicioDiurna && vHIS <= vFinDiurna)                    {                        vEstadoHIS = "Diurnas";                    }                    else if (vHIS >= vInicioNoc && vHIS <= vFinNoc)                    {                        vEstadoHIS = "Noc";                    }                    else if (vHIS >= vInicioNocNoc2 && vHIS <= vFinNocNoc2)                    {                        vEstadoHIS = "NocNoc2";                    }                    vInicioNocNoc1 = new DateTime(añoFin, mesFin, diaFin, 00, 00, 00);                    vFinNocNoc1 = new DateTime(añoFin, mesFin, diaFin, 05, 00, 00);                    vInicioDiurna = new DateTime(añoFin, mesFin, diaFin, 05, 00, 00);                    vFinDiurna = new DateTime(añoFin, mesFin, diaFin, 19, 00, 00);                    vInicioNoc = new DateTime(añoFin, mesFin, diaFin, 19, 00, 00);                    vFinNoc = new DateTime(añoFin, mesFin, diaFin, 22, 00, 00);                    vInicioNocNoc2 = new DateTime(añoFin, mesFin, diaFin, 22, 00, 00);                    vFinNocNoc2 = new DateTime(añoFin, mesFin, diaFin, 23, 59, 00);

                    //ESTADO PARA SABER DONDE FINALIZA LA SOLICITUD
                    if (vHFS >= vInicioNocNoc1 && vHFS <= vFinNocNoc1)                    {                        vEstadoHFS = "NocNoc1";                    }                    else if (vHFS >= vInicioDiurna && vHFS <= vFinDiurna)                    {                        vEstadoHFS = "Diurnas";                    }                    else if (vHFS >= vInicioNoc && vHFS <= vFinNoc)                    {                        vEstadoHFS = "Noc";                    }                    else if (vHFS >= vInicioNocNoc2 && vHFS <= vFinNocNoc2)                    {                        vEstadoHFS = "NocNoc2";                    }                }                Int32 transcurridoMinutosNoc_Noc1 = 0;                Int32 transcurridoHorasNoc_Noc1 = 0;                Int32 transcurridoMinutosDiurnas = 0;                Int32 transcurridoHorasDiurnas = 0;                Int32 transcurridoMinutosNoc = 0;                Int32 transcurridoHorasNoc = 0;                Int32 transcurridoMinutosNoc_Noc2 = 0;                Int32 transcurridoHorasNoc_Noc2 = 0;                Int32 transcurridoMinutosDomingosFeriados = 0;                Int32 transcurridoHorasDomingosFeriados = 0;                Int32 acu_horasNoc_Noc = 0;                Int32 acu_minutosNoc_Noc = 0;                Int32 acu_horasDiurnas = 0;                Int32 acu_minutosDiurnas = 0;                Int32 acu_horasNoc = 0;                Int32 acu_minutosNoc = 0;                Int32 acu_horasDomingosFeriados = 0;                Int32 acu_minutosDomingosFeriados = 0;                if (vFeriadovFI == "si" && vFeriadovFF == "si" && Convert.ToInt32(Session["STEFERIADOGLOBAL"]) == 1 && dias == 0)                {                    transcurridoMinutosDomingosFeriados = vHFS.Minute - vHIS.Minute;                    transcurridoHorasDomingosFeriados = vHFS.Hour - vHIS.Hour;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }                    if (transcurridoHorasDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados = transcurridoHorasDomingosFeriados + 24;                    }

                    //ACUMULANDO VALORES NOC_NOC
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;                }                else if (vFeriadovFI == "si" && vFeriadovFF == "si" && Convert.ToInt32(Session["STEFERIADOGLOBAL"]) == 1 && dias == 1)                {                    transcurridoMinutosDomingosFeriados = 0 - vHIS.Minute;                    transcurridoHorasDomingosFeriados = 0 - vHIS.Hour;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }                    if (transcurridoHorasDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados = transcurridoHorasDomingosFeriados + 24;                    }

                    //ACUMULANDO DOMINGO FERIADO
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;


                    ////////////////////////////////////////////////

                    transcurridoMinutosDomingosFeriados = vHFS.Minute - 0;                    transcurridoHorasDomingosFeriados = vHFS.Hour - 0;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }

                    //ACUMULANDO DOMINGO FERIADO
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;                }                else if (vFeriadovFI == "si" && vFeriadovFF == "si" && Convert.ToInt32(Session["STEFERIADOGLOBAL"]) == 0 && Session["STECIUDAD"].ToString() == Session["STECIUDADFERIADO"].ToString() && dias == 0)                {                    transcurridoMinutosDomingosFeriados = vHFS.Minute - vHIS.Minute;                    transcurridoHorasDomingosFeriados = vHFS.Hour - vHIS.Hour;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }                    if (transcurridoHorasDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados = transcurridoHorasDomingosFeriados + 24;                    }

                    //ACUMULANDO VALORES NOC_NOC
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;                }                else if (vFeriadovFI == "si" && vFeriadovFF == "si" && Convert.ToInt32(Session["STEFERIADOGLOBAL"]) == 0 && Session["STECIUDAD"].ToString() == Session["STECIUDADFERIADO"].ToString() && dias == 1)                {                    transcurridoMinutosDomingosFeriados = 0 - vHIS.Minute;                    transcurridoHorasDomingosFeriados = 0 - vHIS.Hour;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }                    if (transcurridoHorasDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados = transcurridoHorasDomingosFeriados + 24;                    }

                    //ACUMULANDO DOMINGO FERIADO
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;


                    ////////////////////////////////////////////////

                    transcurridoMinutosDomingosFeriados = vHFS.Minute - 0;                    transcurridoHorasDomingosFeriados = vHFS.Hour - 0;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }

                    //ACUMULANDO DOMINGO FERIADO
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;                }                else if (vFeriadovFI == "si" && vDomingovFF == "si" && Convert.ToInt32(Session["STEFERIADOGLOBAL"]) == 1)                {                    transcurridoMinutosDomingosFeriados = 0 - vHIS.Minute;                    transcurridoHorasDomingosFeriados = 0 - vHIS.Hour;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }                    if (transcurridoHorasDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados = transcurridoHorasDomingosFeriados + 24;                    }

                    //ACUMULANDO DOMINGO FERIADO
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;

                    ////////////////////////////////////////////////

                    transcurridoMinutosDomingosFeriados = vHFS.Minute - 0;                    transcurridoHorasDomingosFeriados = vHFS.Hour - 0;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }

                    //ACUMULANDO DOMINGO FERIADO
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;                }                else if (vFeriadovFI == "si" && vDomingovFF == "si" && Convert.ToInt32(Session["STEFERIADOGLOBAL"]) == 0 && Session["STECIUDAD"].ToString() == Session["STECIUDADFERIADO"].ToString())                {                    transcurridoMinutosDomingosFeriados = 0 - vHIS.Minute;                    transcurridoHorasDomingosFeriados = 0 - vHIS.Hour;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }                    if (transcurridoHorasDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados = transcurridoHorasDomingosFeriados + 24;                    }

                    //ACUMULANDO DOMINGO FERIADO
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;

                    ////////////////////////////////////////////////

                    transcurridoMinutosDomingosFeriados = vHFS.Minute - 0;                    transcurridoHorasDomingosFeriados = vHFS.Hour - 0;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }

                    //ACUMULANDO DOMINGO FERIADO
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;                }                else if (vDomingovFI == "si" && vFeriadovFF == "si" && Convert.ToInt32(Session["STEFERIADOGLOBAL"]) == 1)                {                    transcurridoMinutosDomingosFeriados = 0 - vHIS.Minute;                    transcurridoHorasDomingosFeriados = 0 - vHIS.Hour;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }                    if (transcurridoHorasDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados = transcurridoHorasDomingosFeriados + 24;                    }

                    //ACUMULANDO DOMINGO FERIADO
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;


                    ////////////////////////////////////////////////

                    transcurridoMinutosDomingosFeriados = vHFS.Minute - 0;                    transcurridoHorasDomingosFeriados = vHFS.Hour - 0;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }

                    //ACUMULANDO DOMINGO FERIADO
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;                }                else if (vDomingovFI == "si" && vFeriadovFF == "si" && Convert.ToInt32(Session["STEFERIADOGLOBAL"]) == 0 && Session["STECIUDAD"].ToString() == Session["STECIUDADFERIADO"].ToString())                {                    transcurridoMinutosDomingosFeriados = 0 - vHIS.Minute;                    transcurridoHorasDomingosFeriados = 0 - vHIS.Hour;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }                    if (transcurridoHorasDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados = transcurridoHorasDomingosFeriados + 24;                    }

                    //ACUMULANDO DOMINGO FERIADO
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;


                    ////////////////////////////////////////////////

                    transcurridoMinutosDomingosFeriados = vHFS.Minute - 0;                    transcurridoHorasDomingosFeriados = vHFS.Hour - 0;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }

                    //ACUMULANDO DOMINGO FERIADO
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;                }                else if (vDomingovFI == "no" && vFeriadovFI == "no" && vFeriadovFF == "si" && Convert.ToInt32(Session["STEFERIADOGLOBAL"]) == 1)                {                    if (vEstadoHIS == "NocNoc1")                    {
                        //SACAR HORAS NOC_NOC_1 +DIURNAS+NOC+NOC NOC2
                        //***HORAS NOC_NOC1 ***// 
                        transcurridoMinutosNoc_Noc1 = vFinNocNoc1.Minute - vHIS.Minute;                        transcurridoHorasNoc_Noc1 = vFinNocNoc1.Hour - vHIS.Minute;                        if (transcurridoMinutosNoc_Noc1 < 0)                        {                            transcurridoHorasNoc_Noc1--;                            transcurridoMinutosNoc_Noc1 = 60 + transcurridoMinutosNoc_Noc1;                        }

                        //ACUMULANDO VALORES NOC_NOC
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;

                        //***HORAS DIURNAS***//  
                        transcurridoMinutosDiurnas = 0;                        transcurridoHorasDiurnas = 14;

                        //ACUMULANDO VALORES DIURNAS
                        acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                        acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;


                        //***HORAS NOC***//  
                        transcurridoMinutosNoc = 0;                        transcurridoHorasNoc = 3;

                        //ACUMULANDO VALORES NOC
                        acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                        acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;

                        //***HORAS NOC_NOC2 ***// 
                        transcurridoMinutosNoc_Noc2 = 0;                        transcurridoHorasNoc_Noc2 = 2;

                        //ACUMULANDO VALORES NOC_NOC 
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;                    }                    else if (vEstadoHIS == "Diurnas")                    {
                        //SACAR DIURNAS+NOC+NOC NOC2
                        //***HORAS DIURNAS***//  
                        transcurridoMinutosDiurnas = vFinDiurna.Minute - vHIS.Minute;                        transcurridoHorasDiurnas = vFinDiurna.Hour - vHIS.Hour;                        if (transcurridoMinutosDiurnas < 0)                        {                            transcurridoHorasDiurnas--;                            transcurridoMinutosDiurnas = 60 + transcurridoMinutosDiurnas;                        }

                        //ACUMULANDO VALORES DIURNAS
                        acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                        acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;


                        //***HORAS NOC***//  
                        transcurridoMinutosNoc = 0;                        transcurridoHorasNoc = 3;

                        //ACUMULANDO VALORES NOC
                        acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                        acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;

                        //***HORAS NOC_NOC2 ***// 
                        transcurridoMinutosNoc_Noc2 = 0;                        transcurridoHorasNoc_Noc2 = 2;

                        //ACUMULANDO VALORES NOC_NOC 
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;                    }                    else if (vEstadoHIS == "Noc")                    {
                        //SACAR NOC+NOC NOC2
                        //***HORAS NOC***//  
                        transcurridoMinutosNoc = vFinNoc.Minute - vHIS.Minute;                        transcurridoHorasNoc = vFinNoc.Hour - vHIS.Hour;                        if (transcurridoMinutosNoc < 0)                        {                            transcurridoHorasNoc--;                            transcurridoMinutosNoc = 60 + transcurridoMinutosNoc;                        }

                        //ACUMULANDO VALORES NOC
                        acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                        acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;


                        //***HORAS NOC_NOC2 ***// 
                        transcurridoMinutosNoc_Noc2 = 0;                        transcurridoHorasNoc_Noc2 = 2;

                        //ACUMULANDO VALORES NOC_NOC 
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;                    }                    else if (vEstadoHIS == "NocNoc2")                    {
                        //SACAR NOC NOC2
                        //***HORAS NOC_NOC2 ***// 
                        transcurridoMinutosNoc_Noc2 = vFinNocNoc2.Minute - vHIS.Minute;                        transcurridoHorasNoc_Noc2 = vFinNocNoc2.Hour - vHIS.Hour;                        if (transcurridoMinutosNoc_Noc2 < 0)                        {                            transcurridoHorasNoc_Noc2--;                            transcurridoMinutosNoc_Noc2 = 60 + transcurridoMinutosNoc_Noc2;                        }

                        //ACUMULANDO VALORES NOC_NOC 
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;                    }                    transcurridoMinutosDomingosFeriados = vHFS.Minute - 0;                    transcurridoHorasDomingosFeriados = vHFS.Hour - 0;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }                    if (transcurridoHorasDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados = transcurridoHorasDomingosFeriados + 24;                    }

                    //ACUMULANDO VALORES NOC_NOC
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;                }                else if (vDomingovFI == "no" && vFeriadovFI == "no" && vFeriadovFF == "si" && Convert.ToInt32(Session["STEFERIADOGLOBAL"]) == 0 && Session["STECIUDAD"].ToString() == Session["STECIUDADFERIADO"].ToString())                {                    if (vEstadoHIS == "NocNoc1")                    {
                        //SACAR HORAS NOC_NOC_1 +DIURNAS+NOC+NOC NOC2
                        //***HORAS NOC_NOC1 ***// 
                        transcurridoMinutosNoc_Noc1 = vFinNocNoc1.Minute - vHIS.Minute;                        transcurridoHorasNoc_Noc1 = vFinNocNoc1.Hour - vHIS.Minute;                        if (transcurridoMinutosNoc_Noc1 < 0)                        {                            transcurridoHorasNoc_Noc1--;                            transcurridoMinutosNoc_Noc1 = 60 + transcurridoMinutosNoc_Noc1;                        }

                        //ACUMULANDO VALORES NOC_NOC
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;

                        //***HORAS DIURNAS***//  
                        transcurridoMinutosDiurnas = 0;                        transcurridoHorasDiurnas = 14;

                        //ACUMULANDO VALORES DIURNAS
                        acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                        acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;


                        //***HORAS NOC***//  
                        transcurridoMinutosNoc = 0;                        transcurridoHorasNoc = 3;

                        //ACUMULANDO VALORES NOC
                        acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                        acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;

                        //***HORAS NOC_NOC2 ***// 
                        transcurridoMinutosNoc_Noc2 = 0;                        transcurridoHorasNoc_Noc2 = 2;

                        //ACUMULANDO VALORES NOC_NOC 
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;                    }                    else if (vEstadoHIS == "Diurnas")                    {
                        //SACAR DIURNAS+NOC+NOC NOC2
                        //***HORAS DIURNAS***//  
                        transcurridoMinutosDiurnas = vFinDiurna.Minute - vHIS.Minute;                        transcurridoHorasDiurnas = vFinDiurna.Hour - vHIS.Hour;                        if (transcurridoMinutosDiurnas < 0)                        {                            transcurridoHorasDiurnas--;                            transcurridoMinutosDiurnas = 60 + transcurridoMinutosDiurnas;                        }

                        //ACUMULANDO VALORES DIURNAS
                        acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                        acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;


                        //***HORAS NOC***//  
                        transcurridoMinutosNoc = 0;                        transcurridoHorasNoc = 3;

                        //ACUMULANDO VALORES NOC
                        acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                        acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;

                        //***HORAS NOC_NOC2 ***// 
                        transcurridoMinutosNoc_Noc2 = 0;                        transcurridoHorasNoc_Noc2 = 2;

                        //ACUMULANDO VALORES NOC_NOC 
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;                    }                    else if (vEstadoHIS == "Noc")                    {
                        //SACAR NOC+NOC NOC2
                        //***HORAS NOC***//  
                        transcurridoMinutosNoc = vFinNoc.Minute - vHIS.Minute;                        transcurridoHorasNoc = vFinNoc.Hour - vHIS.Hour;                        if (transcurridoMinutosNoc < 0)                        {                            transcurridoHorasNoc--;                            transcurridoMinutosNoc = 60 + transcurridoMinutosNoc;                        }

                        //ACUMULANDO VALORES NOC
                        acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                        acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;


                        //***HORAS NOC_NOC2 ***// 
                        transcurridoMinutosNoc_Noc2 = 0;                        transcurridoHorasNoc_Noc2 = 2;

                        //ACUMULANDO VALORES NOC_NOC 
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;                    }                    else if (vEstadoHIS == "NocNoc2")                    {
                        //SACAR NOC NOC2
                        //***HORAS NOC_NOC2 ***// 
                        transcurridoMinutosNoc_Noc2 = vFinNocNoc2.Minute - vHIS.Minute;                        transcurridoHorasNoc_Noc2 = vFinNocNoc2.Hour - vHIS.Hour;                        if (transcurridoMinutosNoc_Noc2 < 0)                        {                            transcurridoHorasNoc_Noc2--;                            transcurridoMinutosNoc_Noc2 = 60 + transcurridoMinutosNoc_Noc2;                        }

                        //ACUMULANDO VALORES NOC_NOC 
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;                    }                    transcurridoMinutosDomingosFeriados = vHFS.Minute - 0;                    transcurridoHorasDomingosFeriados = vHFS.Hour - 0;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }                    if (transcurridoHorasDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados = transcurridoHorasDomingosFeriados + 24;                    }

                    //ACUMULANDO VALORES NOC_NOC
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;                }                else if (vFeriadovFI == "si" && vDomingovFF == "no" && vFeriadovFF == "no" && Convert.ToInt32(Session["STEFERIADOGLOBAL"]) == 1)                {                    transcurridoMinutosDomingosFeriados = 0 - vHIS.Minute;                    transcurridoHorasDomingosFeriados = 0 - vHIS.Hour;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }                    if (transcurridoHorasDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados = transcurridoHorasDomingosFeriados + 24;                    }

                    //ACUMULANDO VALORES DOMINGOS Y FERIADOS
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;

                    //////////////////////////////////////////////////////////////////////////////////////////////

                    if (vEstadoHFS == "NocNoc1")                    {
                        //SACAR HORAS NOC_NOC_1
                        //***HORAS NOC_NOC1 ***// 
                        transcurridoMinutosNoc_Noc1 = vHFS.Minute - 0;                        transcurridoHorasNoc_Noc1 = vHFS.Hour - 0;                        if (transcurridoMinutosNoc_Noc1 < 0)                        {                            transcurridoHorasNoc_Noc1--;                            transcurridoMinutosNoc_Noc1 = 60 + transcurridoMinutosNoc_Noc1;                        }

                        //ACUMULANDO VALORES NOC_NOC
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;                    }                    else if (vEstadoHFS == "Diurnas")                    {
                        //SACAR HORAS NOC_NOC_1 + DIURNAS
                        //***HORAS NOC_NOC1 ***// 
                        transcurridoMinutosNoc_Noc1 = 0;                        transcurridoHorasNoc_Noc1 = 5;

                        //ACUMULANDO VALORES NOC_NOC
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;

                        //***HORAS DIURNAS***//  
                        transcurridoMinutosDiurnas = vHFS.Minute - vInicioDiurna.Minute;                        transcurridoHorasDiurnas = vHFS.Hour - vInicioDiurna.Hour;                        if (transcurridoMinutosDiurnas < 0)                        {                            transcurridoHorasDiurnas--;                            transcurridoMinutosDiurnas = 60 + transcurridoMinutosDiurnas;                        }

                        //ACUMULANDO VALORES DIURNAS
                        acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                        acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;                    }                    else if (vEstadoHFS == "Noc")                    {
                        //SACAR HORAS NOC_NOC_1 + DIURNAS+NOC
                        //***HORAS NOC_NOC1 ***// 
                        transcurridoMinutosNoc_Noc1 = 0;                        transcurridoHorasNoc_Noc1 = 5;

                        //ACUMULANDO VALORES NOC_NOC
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;


                        //***HORAS DIURNAS***//  
                        transcurridoMinutosDiurnas = 0;                        transcurridoHorasDiurnas = 14;

                        //ACUMULANDO VALORES DIURNAS
                        acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                        acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;

                        //***HORAS NOC***//  
                        transcurridoMinutosNoc = vHFS.Minute - vInicioNoc.Minute;                        transcurridoHorasNoc = vHFS.Hour - vInicioNoc.Hour;                        if (transcurridoMinutosNoc < 0)                        {                            transcurridoHorasNoc--;                            transcurridoMinutosNoc = 60 + transcurridoMinutosNoc;                        }

                        //ACUMULANDO VALORES NOC
                        acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                        acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;                    }                    else if (vEstadoHFS == "NocNoc2")                    {
                        //SACAR HORAS NOC_NOC_1 + DIURNAS+NOC+NOC_NOC_2
                        //***HORAS NOC_NOC1 ***// 
                        transcurridoMinutosNoc_Noc1 = 0;                        transcurridoHorasNoc_Noc1 = 5;

                        //ACUMULANDO VALORES NOC_NOC
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;


                        //***HORAS DIURNAS***//  
                        transcurridoMinutosDiurnas = 0;                        transcurridoHorasDiurnas = 14;

                        //ACUMULANDO VALORES DIURNAS
                        acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                        acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;

                        //***HORAS NOC***//  
                        transcurridoMinutosNoc = 0;                        transcurridoHorasNoc = 3;

                        //ACUMULANDO VALORES NOC
                        acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                        acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;

                        //***HORAS NOC_NOC2 ***// 
                        transcurridoMinutosNoc_Noc2 = vHFS.Minute - vInicioNocNoc2.Minute;                        transcurridoHorasNoc_Noc2 = vHFS.Hour - vInicioNocNoc2.Hour;                        if (transcurridoMinutosNoc_Noc2 < 0)                        {                            transcurridoHorasNoc_Noc2--;                            transcurridoMinutosNoc_Noc2 = 60 + transcurridoMinutosNoc_Noc2;                        }

                        //ACUMULANDO VALORES NOC_NOC 
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;                    }                }                else if (vFeriadovFI == "si" && vDomingovFF == "no" && vFeriadovFF == "no" && Convert.ToInt32(Session["STEFERIADOGLOBAL"]) == 0 && Session["STECIUDAD"].ToString() == Session["STECIUDADFERIADO"].ToString())                {                    transcurridoMinutosDomingosFeriados = 0 - vHIS.Minute;                    transcurridoHorasDomingosFeriados = 0 - vHIS.Hour;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }                    if (transcurridoHorasDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados = transcurridoHorasDomingosFeriados + 24;                    }

                    //ACUMULANDO VALORES DOMINGOS Y FERIADOS
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;

                    //////////////////////////////////////////////////////////////////////////////////////////////

                    if (vEstadoHFS == "NocNoc1")                    {
                        //SACAR HORAS NOC_NOC_1
                        //***HORAS NOC_NOC1 ***// 
                        transcurridoMinutosNoc_Noc1 = vHFS.Minute - 0;                        transcurridoHorasNoc_Noc1 = vHFS.Hour - 0;                        if (transcurridoMinutosNoc_Noc1 < 0)                        {                            transcurridoHorasNoc_Noc1--;                            transcurridoMinutosNoc_Noc1 = 60 + transcurridoMinutosNoc_Noc1;                        }

                        //ACUMULANDO VALORES NOC_NOC
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;                    }                    else if (vEstadoHFS == "Diurnas")                    {
                        //SACAR HORAS NOC_NOC_1 + DIURNAS
                        //***HORAS NOC_NOC1 ***// 
                        transcurridoMinutosNoc_Noc1 = 0;                        transcurridoHorasNoc_Noc1 = 5;

                        //ACUMULANDO VALORES NOC_NOC
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;

                        //***HORAS DIURNAS***//  
                        transcurridoMinutosDiurnas = vHFS.Minute - vInicioDiurna.Minute;                        transcurridoHorasDiurnas = vHFS.Hour - vInicioDiurna.Hour;                        if (transcurridoMinutosDiurnas < 0)                        {                            transcurridoHorasDiurnas--;                            transcurridoMinutosDiurnas = 60 + transcurridoMinutosDiurnas;                        }

                        //ACUMULANDO VALORES DIURNAS
                        acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                        acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;                    }                    else if (vEstadoHFS == "Noc")                    {
                        //SACAR HORAS NOC_NOC_1 + DIURNAS+NOC
                        //***HORAS NOC_NOC1 ***// 
                        transcurridoMinutosNoc_Noc1 = 0;                        transcurridoHorasNoc_Noc1 = 5;

                        //ACUMULANDO VALORES NOC_NOC
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;


                        //***HORAS DIURNAS***//  
                        transcurridoMinutosDiurnas = 0;                        transcurridoHorasDiurnas = 14;

                        //ACUMULANDO VALORES DIURNAS
                        acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                        acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;

                        //***HORAS NOC***//  
                        transcurridoMinutosNoc = vHFS.Minute - vInicioNoc.Minute;                        transcurridoHorasNoc = vHFS.Hour - vInicioNoc.Hour;                        if (transcurridoMinutosNoc < 0)                        {                            transcurridoHorasNoc--;                            transcurridoMinutosNoc = 60 + transcurridoMinutosNoc;                        }

                        //ACUMULANDO VALORES NOC
                        acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                        acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;                    }                    else if (vEstadoHFS == "NocNoc2")                    {
                        //SACAR HORAS NOC_NOC_1 + DIURNAS+NOC+NOC_NOC_2
                        //***HORAS NOC_NOC1 ***// 
                        transcurridoMinutosNoc_Noc1 = 0;                        transcurridoHorasNoc_Noc1 = 5;

                        //ACUMULANDO VALORES NOC_NOC
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;


                        //***HORAS DIURNAS***//  
                        transcurridoMinutosDiurnas = 0;                        transcurridoHorasDiurnas = 14;

                        //ACUMULANDO VALORES DIURNAS
                        acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                        acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;

                        //***HORAS NOC***//  
                        transcurridoMinutosNoc = 0;                        transcurridoHorasNoc = 3;

                        //ACUMULANDO VALORES NOC
                        acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                        acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;

                        //***HORAS NOC_NOC2 ***// 
                        transcurridoMinutosNoc_Noc2 = vHFS.Minute - vInicioNocNoc2.Minute;                        transcurridoHorasNoc_Noc2 = vHFS.Hour - vInicioNocNoc2.Hour;                        if (transcurridoMinutosNoc_Noc2 < 0)                        {                            transcurridoHorasNoc_Noc2--;                            transcurridoMinutosNoc_Noc2 = 60 + transcurridoMinutosNoc_Noc2;                        }

                        //ACUMULANDO VALORES NOC_NOC 
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;                    }                }                else if (vDomingovFI == "si" && vDomingovFF == "si" && dias == 0)                {                    transcurridoMinutosDomingosFeriados = vHFS.Minute - vHIS.Minute;                    transcurridoHorasDomingosFeriados = vHFS.Hour - vHIS.Hour;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }                    if (transcurridoHorasDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados = transcurridoHorasDomingosFeriados + 24;                    }

                    //ACUMULANDO VALORES NOC_NOC
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;                }                else if (vDomingovFI == "si" && vDomingovFF == "si" && dias == 1)                {                    transcurridoMinutosDomingosFeriados = 0 - vHIS.Minute;                    transcurridoHorasDomingosFeriados = 0 - vHIS.Hour;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }                    if (transcurridoHorasDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados = transcurridoHorasDomingosFeriados + 24;                    }

                    //ACUMULANDO DOMINGO FERIADO
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;


                    ////////////////////////////////////////////////

                    transcurridoMinutosDomingosFeriados = vHFS.Minute - 0;                    transcurridoHorasDomingosFeriados = vHFS.Hour - 0;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }

                    //ACUMULANDO DOMINGO FERIADO
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;                }                else if (vDomingovFI == "si" && vDomingovFF == "no")                {                    transcurridoMinutosDomingosFeriados = 0 - vHIS.Minute;                    transcurridoHorasDomingosFeriados = 0 - vHIS.Hour;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }                    if (transcurridoHorasDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados = transcurridoHorasDomingosFeriados + 24;                    }

                    //ACUMULANDO VALORES NOC_NOC
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;

                    //////////////////////////////////////////////////////////////////////////////////////////////

                    if (vEstadoHFS == "NocNoc1")                    {
                        //SACAR HORAS NOC_NOC_1
                        //***HORAS NOC_NOC1 ***// 
                        transcurridoMinutosNoc_Noc1 = vHFS.Minute - 0;                        transcurridoHorasNoc_Noc1 = vHFS.Hour - 0;                        if (transcurridoMinutosNoc_Noc1 < 0)                        {                            transcurridoHorasNoc_Noc1--;                            transcurridoMinutosNoc_Noc1 = 60 + transcurridoMinutosNoc_Noc1;                        }

                        //ACUMULANDO VALORES NOC_NOC
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;                    }                    else if (vEstadoHFS == "Diurnas")                    {
                        //SACAR HORAS NOC_NOC_1 + DIURNAS
                        //***HORAS NOC_NOC1 ***// 
                        transcurridoMinutosNoc_Noc1 = 0;                        transcurridoHorasNoc_Noc1 = 5;

                        //ACUMULANDO VALORES NOC_NOC
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;

                        //***HORAS DIURNAS***//  
                        transcurridoMinutosDiurnas = vHFS.Minute - vInicioDiurna.Minute;                        transcurridoHorasDiurnas = vHFS.Hour - vInicioDiurna.Hour;                        if (transcurridoMinutosDiurnas < 0)                        {                            transcurridoHorasDiurnas--;                            transcurridoMinutosDiurnas = 60 + transcurridoMinutosDiurnas;                        }

                        //ACUMULANDO VALORES DIURNAS
                        acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                        acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;                    }                    else if (vEstadoHFS == "Noc")                    {
                        //SACAR HORAS NOC_NOC_1 + DIURNAS+NOC
                        //***HORAS NOC_NOC1 ***// 
                        transcurridoMinutosNoc_Noc1 = 0;                        transcurridoHorasNoc_Noc1 = 5;

                        //ACUMULANDO VALORES NOC_NOC
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;


                        //***HORAS DIURNAS***//  
                        transcurridoMinutosDiurnas = 0;                        transcurridoHorasDiurnas = 14;

                        //ACUMULANDO VALORES DIURNAS
                        acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                        acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;

                        //***HORAS NOC***//  
                        transcurridoMinutosNoc = vHFS.Minute - vInicioNoc.Minute;                        transcurridoHorasNoc = vHFS.Hour - vInicioNoc.Hour;                        if (transcurridoMinutosNoc < 0)                        {                            transcurridoHorasNoc--;                            transcurridoMinutosNoc = 60 + transcurridoMinutosNoc;                        }

                        //ACUMULANDO VALORES NOC
                        acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                        acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;                    }                    else if (vEstadoHFS == "NocNoc2")                    {
                        //SACAR HORAS NOC_NOC_1 + DIURNAS+NOC+NOC_NOC_2
                        //***HORAS NOC_NOC1 ***// 
                        transcurridoMinutosNoc_Noc1 = 0;                        transcurridoHorasNoc_Noc1 = 5;

                        //ACUMULANDO VALORES NOC_NOC
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;


                        //***HORAS DIURNAS***//  
                        transcurridoMinutosDiurnas = 0;                        transcurridoHorasDiurnas = 14;

                        //ACUMULANDO VALORES DIURNAS
                        acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                        acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;

                        //***HORAS NOC***//  
                        transcurridoMinutosNoc = 0;                        transcurridoHorasNoc = 3;

                        //ACUMULANDO VALORES NOC
                        acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                        acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;

                        //***HORAS NOC_NOC2 ***// 
                        transcurridoMinutosNoc_Noc2 = vHFS.Minute - vInicioNocNoc2.Minute;                        transcurridoHorasNoc_Noc2 = vHFS.Hour - vInicioNocNoc2.Hour;                        if (transcurridoMinutosNoc_Noc2 < 0)                        {                            transcurridoHorasNoc_Noc2--;                            transcurridoMinutosNoc_Noc2 = 60 + transcurridoMinutosNoc_Noc2;                        }

                        //ACUMULANDO VALORES NOC_NOC 
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;                    }                }                else if (vDomingovFI == "no" && vDomingovFF == "si")                {                    if (vEstadoHIS == "NocNoc1")                    {
                        //SACAR HORAS NOC_NOC_1 +DIURNAS+NOC+NOC NOC2
                        //***HORAS NOC_NOC1 ***// 
                        transcurridoMinutosNoc_Noc1 = vFinNocNoc1.Minute - vHIS.Minute;                        transcurridoHorasNoc_Noc1 = vFinNocNoc1.Hour - vHIS.Minute;                        if (transcurridoMinutosNoc_Noc1 < 0)                        {                            transcurridoHorasNoc_Noc1--;                            transcurridoMinutosNoc_Noc1 = 60 + transcurridoMinutosNoc_Noc1;                        }

                        //ACUMULANDO VALORES NOC_NOC
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;

                        //***HORAS DIURNAS***//  
                        transcurridoMinutosDiurnas = 0;                        transcurridoHorasDiurnas = 14;

                        //ACUMULANDO VALORES DIURNAS
                        acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                        acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;


                        //***HORAS NOC***//  
                        transcurridoMinutosNoc = 0;                        transcurridoHorasNoc = 3;

                        //ACUMULANDO VALORES NOC
                        acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                        acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;

                        //***HORAS NOC_NOC2 ***// 
                        transcurridoMinutosNoc_Noc2 = 0;                        transcurridoHorasNoc_Noc2 = 2;

                        //ACUMULANDO VALORES NOC_NOC 
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;                    }                    else if (vEstadoHIS == "Diurnas")                    {
                        //SACAR DIURNAS+NOC+NOC NOC2
                        //***HORAS DIURNAS***//  
                        transcurridoMinutosDiurnas = vFinDiurna.Minute - vHIS.Minute;                        transcurridoHorasDiurnas = vFinDiurna.Hour - vHIS.Hour;                        if (transcurridoMinutosDiurnas < 0)                        {                            transcurridoHorasDiurnas--;                            transcurridoMinutosDiurnas = 60 + transcurridoMinutosDiurnas;                        }

                        //ACUMULANDO VALORES DIURNAS
                        acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                        acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;


                        //***HORAS NOC***//  
                        transcurridoMinutosNoc = 0;                        transcurridoHorasNoc = 3;

                        //ACUMULANDO VALORES NOC
                        acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                        acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;

                        //***HORAS NOC_NOC2 ***// 
                        transcurridoMinutosNoc_Noc2 = 0;                        transcurridoHorasNoc_Noc2 = 2;

                        //ACUMULANDO VALORES NOC_NOC 
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;                    }                    else if (vEstadoHIS == "Noc")                    {
                        //SACAR NOC+NOC NOC2
                        //***HORAS NOC***//  
                        transcurridoMinutosNoc = vFinNoc.Minute - vHIS.Minute;                        transcurridoHorasNoc = vFinNoc.Hour - vHIS.Hour;                        if (transcurridoMinutosNoc < 0)                        {                            transcurridoHorasNoc--;                            transcurridoMinutosNoc = 60 + transcurridoMinutosNoc;                        }

                        //ACUMULANDO VALORES NOC
                        acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                        acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;


                        //***HORAS NOC_NOC2 ***// 
                        transcurridoMinutosNoc_Noc2 = 0;                        transcurridoHorasNoc_Noc2 = 2;

                        //ACUMULANDO VALORES NOC_NOC 
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;                    }                    else if (vEstadoHIS == "NocNoc2")                    {
                        //SACAR NOC NOC2
                        //***HORAS NOC_NOC2 ***// 
                        transcurridoMinutosNoc_Noc2 = vFinNocNoc2.Minute - vHIS.Minute;                        transcurridoHorasNoc_Noc2 = vFinNocNoc2.Hour - vHIS.Hour;                        if (transcurridoMinutosNoc_Noc2 < 0)                        {                            transcurridoHorasNoc_Noc2--;                            transcurridoMinutosNoc_Noc2 = 60 + transcurridoMinutosNoc_Noc2;                        }

                        //ACUMULANDO VALORES NOC_NOC 
                        acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                        acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;                    }                    transcurridoMinutosDomingosFeriados = vHFS.Minute - 0;                    transcurridoHorasDomingosFeriados = vHFS.Hour - 0;                    if (transcurridoMinutosDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados--;                        transcurridoMinutosDomingosFeriados = 60 + transcurridoMinutosDomingosFeriados;                    }                    if (transcurridoHorasDomingosFeriados < 0)                    {                        transcurridoHorasDomingosFeriados = transcurridoHorasDomingosFeriados + 24;                    }

                    //ACUMULANDO VALORES NOC_NOC
                    acu_horasDomingosFeriados = acu_horasDomingosFeriados + transcurridoHorasDomingosFeriados;                    acu_minutosDomingosFeriados = acu_minutosDomingosFeriados + transcurridoMinutosDomingosFeriados;                }                if (vEstadoHIS == "NocNoc1" && vEstadoHFS == "NocNoc1" && vDomingovFI == "no" && vDomingovFF == "no" && vFeriadovFI == "no" && vFeriadovFF == "no" && dias == 0)                {
                    //SACAR HORAS NOC_NOC_1
                    //***HORAS NOC_NOC1 ***// 
                    transcurridoMinutosNoc_Noc1 = vHFS.Minute - vHIS.Minute;                    transcurridoHorasNoc_Noc1 = vHFS.Hour - vHIS.Hour;                    if (transcurridoMinutosNoc_Noc1 < 0)                    {                        transcurridoHorasNoc_Noc1--;                        transcurridoMinutosNoc_Noc1 = 60 + transcurridoMinutosNoc_Noc1;                    }

                    //ACUMULANDO VALORES NOC_NOC
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;                }                else if (vEstadoHIS == "NocNoc1" && vEstadoHFS == "Diurnas" && vDomingovFI == "no" && vDomingovFF == "no" && vFeriadovFI == "no" && vFeriadovFF == "no")                {
                    //SACAR HORAS NOC_NOC_1 + DIURNAS
                    //***HORAS NOC_NOC1 ***// 
                    transcurridoMinutosNoc_Noc1 = vFinNocNoc1.Minute - vHIS.Minute;                    transcurridoHorasNoc_Noc1 = vFinNocNoc1.Hour - vHIS.Hour;                    if (transcurridoMinutosNoc_Noc1 < 0)                    {                        transcurridoHorasNoc_Noc1--;                        transcurridoMinutosNoc_Noc1 = 60 + transcurridoMinutosNoc_Noc1;                    }

                    //ACUMULANDO VALORES NOC_NOC
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;


                    //***HORAS DIURNAS***//  
                    transcurridoMinutosDiurnas = vHFS.Minute - vInicioDiurna.Minute;                    transcurridoHorasDiurnas = vHFS.Hour - vInicioDiurna.Hour;                    if (transcurridoMinutosDiurnas < 0)                    {                        transcurridoHorasDiurnas--;                        transcurridoMinutosDiurnas = 60 + transcurridoMinutosDiurnas;                    }

                    //ACUMULANDO VALORES DIURNAS
                    acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                    acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;                }                else if (vEstadoHIS == "NocNoc1" && vEstadoHFS == "Noc" && vDomingovFI == "no" && vDomingovFF == "no" && vFeriadovFI == "no" && vFeriadovFF == "no")                {
                    //SACAR HORAS NOC_NOC_1 + DIURNAS+NOC
                    //***HORAS NOC_NOC1 ***// 
                    transcurridoMinutosNoc_Noc1 = vFinNocNoc1.Minute - vHIS.Minute;                    transcurridoHorasNoc_Noc1 = vFinNocNoc1.Hour - vHIS.Hour;                    if (transcurridoMinutosNoc_Noc1 < 0)                    {                        transcurridoHorasNoc_Noc1--;                        transcurridoMinutosNoc_Noc1 = 60 + transcurridoMinutosNoc_Noc1;                    }

                    //ACUMULANDO VALORES NOC_NOC
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;


                    //***HORAS DIURNAS***//  
                    transcurridoMinutosDiurnas = 0;                    transcurridoHorasDiurnas = 14;

                    //ACUMULANDO VALORES DIURNAS
                    acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                    acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;

                    //***HORAS NOC***//  
                    transcurridoMinutosNoc = vHFS.Minute - vInicioNoc.Minute;                    transcurridoHorasNoc = vHFS.Hour - vInicioNoc.Hour;                    if (transcurridoMinutosNoc < 0)                    {                        transcurridoHorasNoc--;                        transcurridoMinutosNoc = 60 + transcurridoMinutosNoc;                    }

                    //ACUMULANDO VALORES NOC
                    acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                    acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;                }                else if (vEstadoHIS == "NocNoc1" && vEstadoHFS == "NocNoc2" && vDomingovFI == "no" && vDomingovFF == "no" && vFeriadovFI == "no" && vFeriadovFF == "no")                {
                    //SACAR HORAS NOC_NOC_1 + DIURNAS+NOC+NOC_NOC_2
                    //***HORAS NOC_NOC1 ***// 
                    transcurridoMinutosNoc_Noc1 = vFinNocNoc1.Minute - vHIS.Minute;                    transcurridoHorasNoc_Noc1 = vFinNocNoc1.Hour - vHIS.Hour;                    if (transcurridoMinutosNoc_Noc1 < 0)                    {                        transcurridoHorasNoc_Noc1--;                        transcurridoMinutosNoc_Noc1 = 60 + transcurridoMinutosNoc_Noc1;                    }

                    //ACUMULANDO VALORES NOC_NOC
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;


                    //***HORAS DIURNAS***//  
                    transcurridoMinutosDiurnas = 0;                    transcurridoHorasDiurnas = 14;

                    //ACUMULANDO VALORES DIURNAS
                    acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                    acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;

                    //***HORAS NOC***//  
                    transcurridoMinutosNoc = 0;                    transcurridoHorasNoc = 3;

                    //ACUMULANDO VALORES NOC
                    acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                    acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;

                    //***HORAS NOC_NOC2 ***// 
                    transcurridoMinutosNoc_Noc2 = vHFS.Minute - vInicioNocNoc2.Minute;                    transcurridoHorasNoc_Noc2 = vHFS.Hour - vInicioNocNoc2.Hour;                    if (transcurridoMinutosNoc_Noc2 < 0)                    {                        transcurridoHorasNoc_Noc2--;                        transcurridoMinutosNoc_Noc2 = 60 + transcurridoMinutosNoc_Noc2;                    }

                    //ACUMULANDO VALORES NOC_NOC 
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;                }                else if (vEstadoHIS == "NocNoc1" && vEstadoHFS == "NocNoc1" && vDomingovFI == "no" && vDomingovFF == "no" && vFeriadovFI == "no" && vFeriadovFF == "no" && dias == 1)                {
                    //SACAR HORAS NOC_NOC_1 + DIURNAS+NOC+NOC_NOC_2+NOC_NOC_1 
                    //***HORAS NOC_NOC1 ***// 
                    transcurridoMinutosNoc_Noc1 = vFinNocNoc1.Minute - vHIS.Minute;                    transcurridoHorasNoc_Noc1 = vFinNocNoc1.Hour - vHIS.Hour;                    if (transcurridoMinutosNoc_Noc1 < 0)                    {                        transcurridoHorasNoc_Noc1--;                        transcurridoMinutosNoc_Noc1 = 60 + transcurridoMinutosNoc_Noc1;                    }

                    //ACUMULANDO VALORES NOC_NOC
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;


                    //***HORAS DIURNAS***//  
                    transcurridoMinutosDiurnas = 0;                    transcurridoHorasDiurnas = 14;

                    //ACUMULANDO VALORES DIURNAS
                    acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                    acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;

                    //***HORAS NOC***//  
                    transcurridoMinutosNoc = 0;                    transcurridoHorasNoc = 3;

                    //ACUMULANDO VALORES NOC
                    acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                    acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;

                    //***HORAS NOC_NOC2 ***// 
                    transcurridoMinutosNoc_Noc2 = 0;                    transcurridoHorasNoc_Noc2 = 2;

                    //ACUMULANDO VALORES NOC_NOC 
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;

                    //***HORAS NOC_NOC1 ***// 
                    transcurridoMinutosNoc_Noc1 = vHFS.Minute - vInicioNocNoc1.Minute;                    transcurridoHorasNoc_Noc1 = vHFS.Hour - vInicioNocNoc1.Hour;                    if (transcurridoMinutosNoc_Noc1 < 0)                    {                        transcurridoHorasNoc_Noc1--;                        transcurridoMinutosNoc_Noc1 = 60 + transcurridoMinutosNoc_Noc1;                    }

                    //ACUMULANDO VALORES NOC_NOC
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;                }                else if (vEstadoHIS == "Diurnas" && vEstadoHFS == "Diurnas" && vDomingovFI == "no" && vDomingovFF == "no" && vFeriadovFI == "no" && vFeriadovFF == "no" && dias == 0)                {
                    //SACAR HORAS DIURNAS
                    //***HORAS DIURNAS***//  
                    transcurridoMinutosDiurnas = vHFS.Minute - vHIS.Minute;                    transcurridoHorasDiurnas = vHFS.Hour - vHIS.Hour;                    if (transcurridoMinutosDiurnas < 0)                    {                        transcurridoHorasDiurnas--;                        transcurridoMinutosDiurnas = 60 + transcurridoMinutosDiurnas;                    }

                    //ACUMULANDO VALORES DIURNAS
                    acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                    acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;                }                else if (vEstadoHIS == "Diurnas" && vEstadoHFS == "Noc" && vDomingovFI == "no" && vDomingovFF == "no" && vFeriadovFI == "no" && vFeriadovFF == "no")                {
                    //SACAR HORAS DIURNAS+NOC
                    //***HORAS DIURNAS***//  
                    transcurridoMinutosDiurnas = vFinDiurna.Minute - vHIS.Minute;                    transcurridoHorasDiurnas = vFinDiurna.Hour - vHIS.Hour;                    if (transcurridoMinutosDiurnas < 0)                    {                        transcurridoHorasDiurnas--;                        transcurridoMinutosDiurnas = 60 + transcurridoMinutosDiurnas;                    }

                    //ACUMULANDO VALORES DIURNAS
                    acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                    acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;

                    //***HORAS NOC***//  
                    transcurridoMinutosNoc = vHFS.Minute - vInicioNoc.Minute;                    transcurridoHorasNoc = vHFS.Hour - vInicioNoc.Hour;                    if (transcurridoMinutosNoc < 0)                    {                        transcurridoHorasNoc--;                        transcurridoMinutosNoc = 60 + transcurridoMinutosNoc;                    }

                    //ACUMULANDO VALORES NOC
                    acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                    acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;                }                else if (vEstadoHIS == "Diurnas" && vEstadoHFS == "NocNoc2" && vDomingovFI == "no" && vDomingovFF == "no" && vFeriadovFI == "no" && vFeriadovFF == "no")                {
                    //SACAR HORAS DIURNAS+NOC+NOC_NOC_2
                    //***HORAS DIURNAS***//  
                    transcurridoMinutosDiurnas = vFinDiurna.Minute - vHIS.Minute;                    transcurridoHorasDiurnas = vFinDiurna.Hour - vHIS.Hour;                    if (transcurridoMinutosDiurnas < 0)                    {                        transcurridoHorasDiurnas--;                        transcurridoMinutosDiurnas = 60 + transcurridoMinutosDiurnas;                    }

                    //ACUMULANDO VALORES DIURNAS
                    acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                    acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;


                    //***HORAS NOC***//  
                    transcurridoMinutosNoc = 0;                    transcurridoHorasNoc = 3;

                    //ACUMULANDO VALORES NOC
                    acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                    acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;


                    //***HORAS NOC_NOC2 ***// 
                    transcurridoMinutosNoc_Noc2 = vHFS.Minute - vInicioNocNoc2.Minute;                    transcurridoHorasNoc_Noc2 = vHFS.Hour - vInicioNocNoc2.Hour;                    if (transcurridoMinutosNoc_Noc2 < 0)                    {                        transcurridoHorasNoc_Noc2--;                        transcurridoMinutosNoc_Noc2 = 60 + transcurridoMinutosNoc_Noc2;                    }

                    //ACUMULANDO VALORES NOC_NOC 
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;                }                else if (vEstadoHIS == "Diurnas" && vEstadoHFS == "NocNoc1" && vDomingovFI == "no" && vDomingovFF == "no" && vFeriadovFI == "no" && vFeriadovFF == "no")                {
                    //SACAR HORAS DIURNAS+NOC+NOC_NOC_2+NOC_NOC_1
                    //***HORAS DIURNAS***//  
                    transcurridoMinutosDiurnas = vFinDiurna.Minute - vHIS.Minute;                    transcurridoHorasDiurnas = vFinDiurna.Hour - vHIS.Hour;                    if (transcurridoMinutosDiurnas < 0)                    {                        transcurridoHorasDiurnas--;                        transcurridoMinutosDiurnas = 60 + transcurridoMinutosDiurnas;                    }

                    //ACUMULANDO VALORES DIURNAS
                    acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                    acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;


                    //***HORAS NOC***//  
                    transcurridoMinutosNoc = 0;                    transcurridoHorasNoc = 3;

                    //ACUMULANDO VALORES NOC
                    acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                    acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;


                    //***HORAS NOC_NOC2 ***// 
                    transcurridoMinutosNoc_Noc2 = 0;                    transcurridoHorasNoc_Noc2 = 2;

                    //ACUMULANDO VALORES NOC_NOC 
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;


                    //***HORAS NOC_NOC1 ***// 
                    transcurridoMinutosNoc_Noc1 = vHFS.Minute - vInicioNocNoc1.Minute;                    transcurridoHorasNoc_Noc1 = vHFS.Hour - vInicioNocNoc1.Hour;                    if (transcurridoMinutosNoc_Noc1 < 0)                    {                        transcurridoHorasNoc_Noc1--;                        transcurridoMinutosNoc_Noc1 = 60 + transcurridoMinutosNoc_Noc1;                    }

                    //ACUMULANDO VALORES NOC_NOC
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;                }                else if (vEstadoHIS == "Diurnas" && vEstadoHFS == "Diurnas" && vDomingovFI == "no" && vDomingovFF == "no" && vFeriadovFI == "no" && vFeriadovFF == "no" && dias == 1)                {
                    //SACAR HORAS DIURNAS+NOC+NOC_NOC_2+NOC_NOC_1+DIURNAS
                    //***HORAS DIURNAS***//  
                    transcurridoMinutosDiurnas = vFinDiurna.Minute - vHIS.Minute;                    transcurridoHorasDiurnas = vFinDiurna.Hour - vHIS.Hour;                    if (transcurridoMinutosDiurnas < 0)                    {                        transcurridoHorasDiurnas--;                        transcurridoMinutosDiurnas = 60 + transcurridoMinutosDiurnas;                    }

                    //ACUMULANDO VALORES DIURNAS
                    acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                    acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;


                    //***HORAS NOC***//  
                    transcurridoMinutosNoc = 0;                    transcurridoHorasNoc = 3;

                    //ACUMULANDO VALORES NOC
                    acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                    acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;


                    //***HORAS NOC_NOC2 ***// 
                    transcurridoMinutosNoc_Noc2 = 0;                    transcurridoHorasNoc_Noc2 = 2;

                    //ACUMULANDO VALORES NOC_NOC 
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;


                    //***HORAS NOC_NOC1 ***// 
                    transcurridoMinutosNoc_Noc1 = 0;                    transcurridoHorasNoc_Noc1 = 5;

                    //ACUMULANDO VALORES NOC_NOC
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;


                    //***HORAS DIURNAS***//  
                    transcurridoMinutosDiurnas = vHFS.Minute - vInicioDiurna.Minute;                    transcurridoHorasDiurnas = vHFS.Hour - vInicioDiurna.Hour;                    if (transcurridoMinutosDiurnas < 0)                    {                        transcurridoHorasDiurnas--;                        transcurridoMinutosDiurnas = 60 + transcurridoMinutosDiurnas;                    }

                    //ACUMULANDO VALORES DIURNAS
                    acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                    acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;                }                else if (vEstadoHIS == "Noc" && vEstadoHFS == "Noc" && vDomingovFI == "no" && vDomingovFF == "no" && vFeriadovFI == "no" && vFeriadovFF == "no" && dias == 0)                {
                    //***HORAS NOC***//  
                    transcurridoMinutosNoc = vHFS.Minute - vHIS.Minute;                    transcurridoHorasNoc = vHFS.Hour - vHIS.Hour;                    if (transcurridoMinutosNoc < 0)                    {                        transcurridoHorasNoc--;                        transcurridoMinutosNoc = 60 + transcurridoMinutosNoc;                    }

                    //ACUMULANDO VALORES NOC
                    acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                    acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;                }                else if (vEstadoHIS == "Noc" && vEstadoHFS == "NocNoc2" && vDomingovFI == "no" && vDomingovFF == "no" && vFeriadovFI == "no" && vFeriadovFF == "no")                {
                    //***HORAS NOC***//  
                    transcurridoMinutosNoc = vFinNoc.Minute - vHIS.Minute;                    transcurridoHorasNoc = vFinNoc.Hour - vHIS.Hour;                    if (transcurridoMinutosNoc < 0)                    {                        transcurridoHorasNoc--;                        transcurridoMinutosNoc = 60 + transcurridoMinutosNoc;                    }

                    //ACUMULANDO VALORES NOC
                    acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                    acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;


                    //***HORAS NOC_NOC2 ***// 
                    transcurridoMinutosNoc_Noc2 = vHFS.Minute - vInicioNocNoc2.Minute;                    transcurridoHorasNoc_Noc2 = vHFS.Hour - vInicioNocNoc2.Hour;                    if (transcurridoMinutosNoc_Noc2 < 0)                    {                        transcurridoHorasNoc_Noc2--;                        transcurridoMinutosNoc_Noc2 = 60 + transcurridoMinutosNoc_Noc2;                    }

                    //ACUMULANDO VALORES NOC_NOC 
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;                }                else if (vEstadoHIS == "Noc" && vEstadoHFS == "NocNoc1" && vDomingovFI == "no" && vDomingovFF == "no" && vFeriadovFI == "no" && vFeriadovFF == "no")                {
                    //***HORAS NOC***//  
                    transcurridoMinutosNoc = vFinNoc.Minute - vHIS.Minute;                    transcurridoHorasNoc = vFinNoc.Hour - vHIS.Hour;                    if (transcurridoMinutosNoc < 0)                    {                        transcurridoHorasNoc--;                        transcurridoMinutosNoc = 60 + transcurridoMinutosNoc;                    }

                    //ACUMULANDO VALORES NOC
                    acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                    acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;

                    //***HORAS NOC_NOC2 ***// 
                    transcurridoMinutosNoc_Noc2 = 0;                    transcurridoHorasNoc_Noc2 = 2;

                    //ACUMULANDO VALORES NOC_NOC 
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;


                    //***HORAS NOC_NOC1 ***// 
                    transcurridoMinutosNoc_Noc1 = vHFS.Minute - vInicioNocNoc1.Minute;                    transcurridoHorasNoc_Noc1 = vHFS.Hour - vInicioNocNoc1.Hour;                    if (transcurridoMinutosNoc_Noc1 < 0)                    {                        transcurridoHorasNoc_Noc1--;                        transcurridoMinutosNoc_Noc1 = 60 + transcurridoMinutosNoc_Noc1;                    }

                    //ACUMULANDO VALORES NOC_NOC
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;                }                else if (vEstadoHIS == "Noc" && vEstadoHFS == "Diurnas" && vDomingovFI == "no" && vDomingovFF == "no" && vFeriadovFI == "no" && vFeriadovFF == "no")                {
                    //***HORAS NOC***//  
                    transcurridoMinutosNoc = vFinNoc.Minute - vHIS.Minute;                    transcurridoHorasNoc = vFinNoc.Hour - vHIS.Hour;                    if (transcurridoMinutosNoc < 0)                    {                        transcurridoHorasNoc--;                        transcurridoMinutosNoc = 60 + transcurridoMinutosNoc;                    }

                    //ACUMULANDO VALORES NOC
                    acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                    acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;

                    //***HORAS NOC_NOC2 ***// 
                    transcurridoMinutosNoc_Noc2 = 0;                    transcurridoHorasNoc_Noc2 = 2;

                    //ACUMULANDO VALORES NOC_NOC 
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;

                    //***HORAS NOC_NOC1 ***// 
                    transcurridoMinutosNoc_Noc1 = 0;                    transcurridoHorasNoc_Noc1 = 5;

                    //ACUMULANDO VALORES NOC_NOC
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;


                    //***HORAS DIURNAS***//  
                    transcurridoMinutosDiurnas = vHFS.Minute - vInicioDiurna.Minute;                    transcurridoHorasDiurnas = vHFS.Hour - vInicioDiurna.Hour;                    if (transcurridoMinutosDiurnas < 0)                    {                        transcurridoHorasDiurnas--;                        transcurridoMinutosDiurnas = 60 + transcurridoMinutosDiurnas;                    }

                    //ACUMULANDO VALORES DIURNAS
                    acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                    acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;                }                else if (vEstadoHIS == "Noc" && vEstadoHFS == "Noc" && vDomingovFI == "no" && vDomingovFF == "no" && vFeriadovFI == "no" && vFeriadovFF == "no" && dias == 1)                {
                    //***HORAS NOC***//  
                    transcurridoMinutosNoc = vFinNoc.Minute - vHIS.Minute;                    transcurridoHorasNoc = vFinNoc.Hour - vHIS.Hour;                    if (transcurridoMinutosNoc < 0)                    {                        transcurridoHorasNoc--;                        transcurridoMinutosNoc = 60 + transcurridoMinutosNoc;                    }

                    //ACUMULANDO VALORES NOC
                    acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                    acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;

                    //***HORAS NOC_NOC2 ***// 
                    transcurridoMinutosNoc_Noc2 = 0;                    transcurridoHorasNoc_Noc2 = 2;

                    //ACUMULANDO VALORES NOC_NOC 
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;

                    //***HORAS NOC_NOC1 ***// 
                    transcurridoMinutosNoc_Noc1 = 0;                    transcurridoHorasNoc_Noc1 = 5;

                    //ACUMULANDO VALORES NOC_NOC
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;


                    //***HORAS DIURNAS***//  
                    transcurridoMinutosDiurnas = 0;                    transcurridoHorasDiurnas = 14;

                    //ACUMULANDO VALORES DIURNAS
                    acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                    acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;

                    //***HORAS NOC***//  
                    transcurridoMinutosNoc = vHFS.Minute - vInicioNoc.Minute;                    transcurridoHorasNoc = vHFS.Hour - vInicioNoc.Hour;                    if (transcurridoMinutosNoc < 0)                    {                        transcurridoHorasNoc--;                        transcurridoMinutosNoc = 60 + transcurridoMinutosNoc;                    }

                    //ACUMULANDO VALORES NOC
                    acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                    acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;                }                else if (vEstadoHIS == "NocNoc2" && vEstadoHFS == "NocNoc2" && vDomingovFI == "no" && vDomingovFF == "no" && vFeriadovFI == "no" && vFeriadovFF == "no" && dias == 0)                {
                    //***HORAS NOC_NOC2 ***// 
                    transcurridoMinutosNoc_Noc2 = vHFS.Minute - vHIS.Minute;                    transcurridoHorasNoc_Noc2 = vHFS.Hour - vHIS.Hour;                    if (transcurridoMinutosNoc_Noc2 < 0)                    {                        transcurridoHorasNoc_Noc2--;                        transcurridoMinutosNoc_Noc2 = 60 + transcurridoMinutosNoc_Noc2;                    }

                    //ACUMULANDO VALORES NOC_NOC 
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;                }                else if (vEstadoHIS == "NocNoc2" && vEstadoHFS == "NocNoc1" && vDomingovFI == "no" && vDomingovFF == "no" && vFeriadovFI == "no" && vFeriadovFF == "no")                {
                    //***HORAS NOC_NOC2 ***// 

                    transcurridoMinutosNoc_Noc2 = vFinNocNoc2.Minute - vHIS.Minute;                    transcurridoHorasNoc_Noc2 = vFinNocNoc2.Hour - vHIS.Hour;                    transcurridoMinutosNoc_Noc2 = transcurridoMinutosNoc_Noc2 + 1;                    if (transcurridoMinutosNoc_Noc2 < 0)                    {                        transcurridoHorasNoc_Noc2--;                        transcurridoMinutosNoc_Noc2 = 60 + transcurridoMinutosNoc_Noc2;                    }                    if (transcurridoMinutosNoc_Noc2 == 60)                    {                        transcurridoMinutosNoc_Noc2 = 0;                        transcurridoHorasNoc_Noc2 = transcurridoHorasNoc_Noc2 + 1;                    }

                    //ACUMULANDO VALORES NOC_NOC 
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;


                    //***HORAS NOC_NOC1 ***// 
                    transcurridoMinutosNoc_Noc1 = vHFS.Minute - vInicioNocNoc1.Minute;                    transcurridoHorasNoc_Noc1 = vHFS.Hour - vInicioNocNoc1.Hour;                    if (transcurridoMinutosNoc_Noc1 < 0)                    {                        transcurridoHorasNoc_Noc1--;                        transcurridoMinutosNoc_Noc1 = 60 + transcurridoMinutosNoc_Noc1;                    }

                    //ACUMULANDO VALORES NOC_NOC
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;                }                else if (vEstadoHIS == "NocNoc2" && vEstadoHFS == "Diurnas" && vDomingovFI == "no" && vDomingovFF == "no" && vFeriadovFI == "no" && vFeriadovFF == "no")                {
                    //***HORAS NOC_NOC2 ***// 
                    transcurridoMinutosNoc_Noc2 = vFinNocNoc2.Minute - vHIS.Minute;                    transcurridoHorasNoc_Noc2 = vFinNocNoc2.Hour - vHIS.Hour;                    transcurridoMinutosNoc_Noc2 = transcurridoMinutosNoc_Noc2 + 1;                    if (transcurridoMinutosNoc_Noc2 < 0)                    {                        transcurridoHorasNoc_Noc2--;                        transcurridoMinutosNoc_Noc2 = 60 + transcurridoMinutosNoc_Noc2;                    }                    if (transcurridoMinutosNoc_Noc2 == 60)                    {                        transcurridoMinutosNoc_Noc2 = 0;                        transcurridoHorasNoc_Noc2 = transcurridoHorasNoc_Noc2 + 1;                    }


                    //ACUMULANDO VALORES NOC_NOC 
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;

                    //***HORAS NOC_NOC1 ***// 
                    transcurridoMinutosNoc_Noc1 = 0;                    transcurridoHorasNoc_Noc1 = 5;

                    //ACUMULANDO VALORES NOC_NOC
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;


                    //***HORAS DIURNAS***//  
                    transcurridoMinutosDiurnas = vHFS.Minute - vInicioDiurna.Minute;                    transcurridoHorasDiurnas = vHFS.Hour - vInicioDiurna.Hour;                    if (transcurridoMinutosDiurnas < 0)                    {                        transcurridoHorasDiurnas--;                        transcurridoMinutosDiurnas = 60 + transcurridoMinutosDiurnas;                    }

                    //ACUMULANDO VALORES DIURNAS
                    acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                    acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;                }                else if (vEstadoHIS == "NocNoc2" && vEstadoHFS == "Noc" && vDomingovFI == "no" && vDomingovFF == "no" && vFeriadovFI == "no" && vFeriadovFF == "no")                {
                    //***HORAS NOC_NOC2 ***// 
                    transcurridoMinutosNoc_Noc2 = vFinNocNoc2.Minute - vHIS.Minute;                    transcurridoHorasNoc_Noc2 = vFinNocNoc2.Hour - vHIS.Hour;                    transcurridoMinutosNoc_Noc2 = transcurridoMinutosNoc_Noc2 + 1;                    if (transcurridoMinutosNoc_Noc2 < 0)                    {                        transcurridoHorasNoc_Noc2--;                        transcurridoMinutosNoc_Noc2 = 60 + transcurridoMinutosNoc_Noc2;                    }                    if (transcurridoMinutosNoc_Noc2 == 60)                    {                        transcurridoMinutosNoc_Noc2 = 0;                        transcurridoHorasNoc_Noc2 = transcurridoHorasNoc_Noc2 + 1;                    }


                    //ACUMULANDO VALORES NOC_NOC 
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;

                    //***HORAS NOC_NOC1 ***// 
                    transcurridoMinutosNoc_Noc1 = 0;                    transcurridoHorasNoc_Noc1 = 5;

                    //ACUMULANDO VALORES NOC_NOC
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;

                    //***HORAS DIURNAS***//  
                    transcurridoMinutosDiurnas = 0;                    transcurridoHorasDiurnas = 14;

                    //ACUMULANDO VALORES DIURNAS
                    acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                    acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;


                    //***HORAS NOC***//  
                    transcurridoMinutosNoc = vHFS.Minute - vInicioNoc.Minute;                    transcurridoHorasNoc = vHFS.Hour - vInicioNoc.Hour;                    if (transcurridoMinutosNoc < 0)                    {                        transcurridoHorasNoc--;                        transcurridoMinutosNoc = 60 + transcurridoMinutosNoc;                    }

                    //ACUMULANDO VALORES NOC
                    acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                    acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;                }                else if (vEstadoHIS == "NocNoc2" && vEstadoHFS == "NocNoc2" && vDomingovFI == "no" && vDomingovFF == "no" && vFeriadovFI == "no" && vFeriadovFF == "no" && dias == 1)                {
                    //***HORAS NOC_NOC2 ***// 
                    transcurridoMinutosNoc_Noc2 = vFinNocNoc2.Minute - vHIS.Minute;                    transcurridoHorasNoc_Noc2 = vFinNocNoc2.Hour - vHIS.Hour;                    transcurridoMinutosNoc_Noc2 = transcurridoMinutosNoc_Noc2 + 1;                    if (transcurridoMinutosNoc_Noc2 < 0)                    {                        transcurridoHorasNoc_Noc2--;                        transcurridoMinutosNoc_Noc2 = 60 + transcurridoMinutosNoc_Noc2;                    }                    if (transcurridoMinutosNoc_Noc2 == 60)                    {                        transcurridoMinutosNoc_Noc2 = 0;                        transcurridoHorasNoc_Noc2 = transcurridoHorasNoc_Noc2 + 1;                    }


                    //ACUMULANDO VALORES NOC_NOC 
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;

                    //***HORAS NOC_NOC1 ***// 
                    transcurridoMinutosNoc_Noc1 = 0;                    transcurridoHorasNoc_Noc1 = 5;

                    //ACUMULANDO VALORES NOC_NOC
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc1;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc1;

                    //***HORAS DIURNAS***//  
                    transcurridoMinutosDiurnas = 0;                    transcurridoHorasDiurnas = 14;

                    //ACUMULANDO VALORES DIURNAS
                    acu_horasDiurnas = acu_horasDiurnas + transcurridoHorasDiurnas;                    acu_minutosDiurnas = acu_minutosDiurnas + transcurridoMinutosDiurnas;


                    //***HORAS NOC***//  
                    transcurridoMinutosNoc = 0;                    transcurridoHorasNoc = 3;                    if (transcurridoMinutosNoc < 0)                    {                        transcurridoHorasNoc--;                        transcurridoMinutosNoc = 60 + transcurridoMinutosNoc;                    }

                    //ACUMULANDO VALORES NOC
                    acu_horasNoc = acu_horasNoc + transcurridoHorasNoc;                    acu_minutosNoc = acu_minutosNoc + transcurridoMinutosNoc;


                    //***HORAS NOC_NOC2 ***// 
                    transcurridoMinutosNoc_Noc2 = vHFS.Minute - vInicioNocNoc2.Minute;                    transcurridoHorasNoc_Noc2 = vHFS.Hour - vInicioNocNoc2.Hour;                    transcurridoMinutosNoc_Noc2 = transcurridoMinutosNoc_Noc2 + 1;                    if (transcurridoMinutosNoc_Noc2 < 0)                    {                        transcurridoHorasNoc_Noc2--;                        transcurridoMinutosNoc_Noc2 = 60 + transcurridoMinutosNoc_Noc2;                    }                    if (transcurridoMinutosNoc_Noc2 == 60)                    {                        transcurridoMinutosNoc_Noc2 = 0;                        transcurridoHorasNoc_Noc2 = transcurridoHorasNoc_Noc2 + 1;                    }

                    //ACUMULANDO VALORES NOC_NOC 
                    acu_horasNoc_Noc = acu_horasNoc_Noc + transcurridoHorasNoc_Noc2;                    acu_minutosNoc_Noc = acu_minutosNoc_Noc + transcurridoMinutosNoc_Noc2;                }                decimal cociente = 0;                int cociente_trun = 0;                int residuo = 0;                Int32 acu_horasNoc_Noc_Resumen = 0;                Int32 acu_minutosNoc_Noc_Resumen = 0;                Double NocNoc_Resumen = 0.0;                Int32 acu_horasDiurnas_Resumen = 0;                Int32 acu_minutosDiurnas_Resumen = 0;                Double Diurnas_Resumen = 0.0;                Int32 acu_horasNoc_Resumen = 0;                Int32 acu_minutosNoc_Resumen = 0;                Double Noc_Resumen = 0.0;                Int32 acu_horasDomingosFeriados_Resumen = 0;                Int32 acu_minutosDomingosFeriados_Resumen = 0;                Double DomingosFeriados_Resumen = 0.0;

                //COCIENTE Y RESIDUO  NOCTURNAS_NOCTURNAS
                if (acu_minutosNoc_Noc >= 60)                {                    cociente = acu_minutosNoc_Noc / 60;                    cociente_trun = Convert.ToInt32(Math.Truncate(cociente));                    residuo = acu_minutosNoc_Noc % 60;                    acu_horasNoc_Noc_Resumen = acu_horasNoc_Noc + cociente_trun;                    acu_minutosNoc_Noc_Resumen = residuo;                }                else                {                    acu_horasNoc_Noc_Resumen = acu_horasNoc_Noc;                    acu_minutosNoc_Noc_Resumen = acu_minutosNoc_Noc;                }                NocNoc_Resumen = ((acu_horasNoc_Noc_Resumen * 60) + acu_minutosNoc_Noc_Resumen);                NocNoc_Resumen = NocNoc_Resumen / 60;

                //COCIENTE Y RESIDUO  DIURNAS
                if (acu_minutosDiurnas >= 60)                {                    cociente = acu_minutosDiurnas / 60;                    cociente_trun = Convert.ToInt32(Math.Truncate(cociente));                    residuo = acu_minutosDiurnas % 60;                    acu_horasDiurnas_Resumen = acu_horasDiurnas + cociente_trun;                    acu_minutosDiurnas_Resumen = residuo;                }                else                {                    acu_horasDiurnas_Resumen = acu_horasDiurnas;                    acu_minutosDiurnas_Resumen = acu_minutosDiurnas;                }                Diurnas_Resumen = ((acu_horasDiurnas_Resumen * 60) + acu_minutosDiurnas_Resumen);                Diurnas_Resumen = Diurnas_Resumen / 60;

                //COCIENTE Y RESIDUO  NOC 
                if (acu_minutosNoc >= 60)                {                    cociente = acu_minutosNoc / 60;                    cociente_trun = Convert.ToInt32(Math.Truncate(cociente));                    residuo = acu_minutosNoc % 60;                    acu_horasNoc_Resumen = acu_horasNoc + cociente_trun;                    acu_minutosNoc_Resumen = residuo;                }                else                {                    acu_horasNoc_Resumen = acu_horasNoc;                    acu_minutosNoc_Resumen = acu_minutosNoc;                }                Noc_Resumen = ((acu_horasNoc_Resumen * 60) + acu_minutosNoc_Resumen);                Noc_Resumen = Noc_Resumen / 60;

                //COCIENTE Y RESIDUO  DOMINGOS Y FERIADOS
                if (acu_minutosDomingosFeriados >= 60)                {                    cociente = acu_minutosDomingosFeriados / 60;                    cociente_trun = Convert.ToInt32(Math.Truncate(cociente));                    residuo = acu_minutosDomingosFeriados % 60;                    acu_horasDomingosFeriados_Resumen = acu_horasDomingosFeriados + cociente_trun;                    acu_minutosDomingosFeriados_Resumen = residuo;                }                else                {                    acu_horasDomingosFeriados_Resumen = acu_horasDomingosFeriados;                    acu_minutosDomingosFeriados_Resumen = acu_minutosDomingosFeriados;                }                DomingosFeriados_Resumen = ((acu_horasDomingosFeriados_Resumen * 60) + acu_minutosDomingosFeriados_Resumen);                DomingosFeriados_Resumen = DomingosFeriados_Resumen / 60;

                //TOTAL HORAS SOLICITADAS
                Int32 acu_horasTotales = 0;                Int32 acu_minutosTotales = 0;                Int32 acu_horasTotales_Resumen = 0;                Int32 acu_minutosTotales_Resumen = 0;                Double Totales_Resumen = 0.0;                acu_horasTotales = acu_horasNoc_Noc_Resumen + acu_horasDiurnas_Resumen + acu_horasNoc_Resumen + acu_horasDomingosFeriados_Resumen;                acu_minutosTotales = acu_minutosNoc_Noc_Resumen + acu_minutosDiurnas_Resumen + acu_minutosNoc_Resumen + acu_minutosDomingosFeriados_Resumen;                if (acu_minutosTotales >= 60)                {                    cociente = acu_minutosTotales / 60;                    cociente_trun = Convert.ToInt32(Math.Truncate(cociente));                    residuo = acu_minutosTotales % 60;                    acu_horasTotales_Resumen = acu_horasTotales + cociente_trun;                    acu_minutosTotales_Resumen = residuo;                }                else                {                    acu_horasTotales_Resumen = acu_horasTotales;                    acu_minutosTotales_Resumen = acu_minutosTotales;                }                Totales_Resumen = ((acu_horasTotales_Resumen * 60) + acu_minutosTotales_Resumen);                Totales_Resumen = Totales_Resumen / 60;                TxHrNocNoc.Text = acu_horasNoc_Noc_Resumen + ":" + acu_minutosNoc_Noc_Resumen + " (" + NocNoc_Resumen.ToString("N2") + ")";                TxHrDiurnas.Text = acu_horasDiurnas_Resumen + ":" + acu_minutosDiurnas_Resumen + " (" + Diurnas_Resumen.ToString("N2") + ")";                TxHrNoc.Text = acu_horasNoc_Resumen + ":" + acu_minutosNoc_Resumen + " (" + Noc_Resumen.ToString("N2") + ")";                TxHrDomFeriado.Text = acu_horasDomingosFeriados_Resumen + ":" + acu_minutosDomingosFeriados_Resumen + " (" + DomingosFeriados_Resumen.ToString("N2") + ")";                TxTotalHoras.Text = acu_horasTotales_Resumen + ":" + acu_minutosTotales_Resumen + " (" + Totales_Resumen.ToString("N2") + ")";                UpdatePanel2.Update();                Session["STEHRNOCNOCSOLICITADAS"] = acu_horasNoc_Noc_Resumen;                Session["STEMINNOCNOCSOLICITADAS"] = acu_minutosNoc_Noc_Resumen;                Session["STEHRDIURNASOLICITADAS"] = acu_horasDiurnas_Resumen;                Session["STEMINDIURNASOLICITADAS"] = acu_minutosDiurnas_Resumen;                Session["STEHRNOCSOLICITADAS"] = acu_horasNoc_Resumen;                Session["STEMINNOCSOLICITADAS"] = acu_minutosNoc_Resumen;                Session["STEHRDOMINGOFERIADOSOLICITADAS"] = acu_horasDomingosFeriados_Resumen;                Session["STEMINDOMINGOFERIADOSOLICITADAS"] = acu_minutosDomingosFeriados_Resumen;                Session["STEHRTOTALSOLICITADAS"] = acu_horasTotales_Resumen;                Session["STEMINTOTALSOLICITADAS"] = acu_minutosTotales_Resumen;

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                string vHoraSalidaBio = "";                if (dias == 0)                {                    DivUnaFecha.Visible = true;                    UpDivUnaFecha.Update();                    vQuery = "RSP_TiempoExtraordinarioGenerales 9,'" + Session["STECODIGOSAPBIOMETRICO"] + "','" + vIniBio + "'";                    vDatos = vConexion.obtenerDataTable(vQuery);                    string vEntradas = "";                    if (vDatos.Rows.Count > 0)                    {                        foreach (DataRow item in vDatos.Rows)                        {                            vEntradas = vEntradas + item["hora"].ToString() + "      ";                        }                    }                    else                    {                        vEntradas = "No hay registros";                    }                    vQuery = "RSP_TiempoExtraordinarioGenerales 10,'" + Session["STECODIGOSAPBIOMETRICO"] + "','" + vFinBio + "'";                    vDatos = vConexion.obtenerDataTable(vQuery);                    string vSalidas = "";                    if (vDatos.Rows.Count > 0)                    {                        foreach (DataRow item in vDatos.Rows)                        {                            vSalidas = vSalidas + item["hora"].ToString() + "      ";                        }                    }                    else                    {                        vSalidas = "No hay registros";                    }                    vQuery = "SELECT dbo.ObtenerHoraSalida('" + Session["STECODIGOSAPBIOMETRICO"] + "','" + vFinBio + "') horasalida";                    vDatos = vConexion.obtenerDataTable(vQuery);                    vHoraSalidaBio = vDatos.Rows[0]["horasalida"].ToString();                    string vHrSalidaBio = vHoraSalidaBio.Substring(0, 2);                    string vMinSalidaBio = vHoraSalidaBio.Substring(3, 2);                    vHoraSalidaBio = vHrSalidaBio + ":" + vMinSalidaBio + ":00";
                    TimeSpan vHrBiometricoSalida = TimeSpan.Parse(vHoraSalidaBio);

                    String vFormatoHrFinal = "HH:mm:ss"; //"dd/MM/yyyy HH:mm:ss"
                    String vHrFinal = Convert.ToDateTime(vHFS).ToString(vFormatoHrFinal);


                    String vFormatoHrInicial = "HH:mm:ss"; //"dd/MM/yyyy HH:mm:ss"      
                    String vHrInicial = Convert.ToDateTime(vHIS).ToString(vFormatoHrInicial);
                    TimeSpan vTimeHrsInicioSolicitud = TimeSpan.Parse(vHrInicial);
                    if ((vTimeHrsInicioSolicitud > vHrBiometricoSalida) && vHrBiometricoSalida != TimeSpan.Zero && RbFormaTrabajo.SelectedValue.Equals("2"))
                        throw new Exception("Favor verificar la hora de la solicitud, Su hora de salida fue a las: " + vHrBiometricoSalida + " y su hora de incio de la solicitud es: " + vTimeHrsInicioSolicitud + " las horas no cuadran, la hora inicial de la solicitud debe ser igual o menor a la hora de salida del biometrico.");

                    TimeSpan vTimeHrsFinSolicitud = TimeSpan.Parse(vHrFinal);
                    if ((vTimeHrsFinSolicitud > vHrBiometricoSalida) && vHrBiometricoSalida != TimeSpan.Zero && RbFormaTrabajo.SelectedValue.Equals("2"))
                        throw new Exception("Favor verificar la hora de la solicitud, Su hora de salida fue a las: " + vHrBiometricoSalida + " y su hora de final de la solicitud es: " + vTimeHrsFinSolicitud + " las horas no cuadran, la hora final de la solicitud debe ser menor  o igual a la hora de salida  del biometrico.");


                    TxSalidaBio.Text = vHoraSalidaBio;                    TxEntradas.Text = vEntradas;                    TxSalidas.Text = vSalidas;                }                if (dias == 1)                {                    DivUnaFecha.Visible = true;                    UpDivUnaFecha.Update();                    vQuery = "RSP_TiempoExtraordinarioGenerales 9,'" + Session["STECODIGOSAPBIOMETRICO"] + "','" + vIniBio + "'";                    vDatos = vConexion.obtenerDataTable(vQuery);                    string vEntradas = "";                    if (vDatos.Rows.Count > 0)                    {                        foreach (DataRow item in vDatos.Rows)                        {                            vEntradas = vEntradas + item["hora"].ToString() + "      ";                        }                    }                    else                    {                        vEntradas = "No hay registros";                    }                    vQuery = "RSP_TiempoExtraordinarioGenerales 10,'" + Session["STECODIGOSAPBIOMETRICO"] + "','" + vIniBio + "'";                    vDatos = vConexion.obtenerDataTable(vQuery);                    DataTable vDatos1 = new DataTable();                    vQuery = "RSP_TiempoExtraordinarioGenerales 17,'" + Session["STECODIGOSAPBIOMETRICO"] + "','" + vFinBio + "'";                    vDatos1 = vConexion.obtenerDataTable(vQuery);                    vHoraSalidaBio = vDatos1.Rows[0]["hora"].ToString();                    string vHrSalidaBio = vHoraSalidaBio.Substring(0, 2);                    string vMinSalidaBio = vHoraSalidaBio.Substring(3, 2);                    vHoraSalidaBio = vHrSalidaBio + ":" + vMinSalidaBio + ":00";



                    String vFormatoHr = "HH:mm:ss"; //"dd/MM/yyyy HH:mm:ss"
                    String vHrFinal = Convert.ToDateTime(vHFS).ToString(vFormatoHr);

                    TimeSpan vHrBiometricoSalida = TimeSpan.Parse(vHoraSalidaBio);
                    TimeSpan vTimeHrsFinSolicitud = TimeSpan.Parse(vHrFinal);
                    if (vTimeHrsFinSolicitud > vHrBiometricoSalida && vHrBiometricoSalida != TimeSpan.Zero && RbFormaTrabajo.SelectedValue.Equals("2"))
                        throw new Exception("Favor verificar la hora de la solicitud, Su hora de salida fue a las: " + vHrBiometricoSalida + " y su hora de final de la solicitud es: " + vTimeHrsFinSolicitud + "las horas no cuadran, la hora final de la solicitud debe ser menor  o igual a la hora de salida  del biometrico.");



                    String vHrInicial = Convert.ToDateTime(vHIS).ToString(vFormatoHr);
                    TimeSpan vTimeHrsInicioSolicitud = TimeSpan.Parse(vHrInicial);
                    if ((vTimeHrsInicioSolicitud < vHrBiometricoSalida) && vHrBiometricoSalida != TimeSpan.Zero && RbFormaTrabajo.SelectedValue.Equals("2"))
                        throw new Exception("Favor verificar la hora de la solicitud, Su hora de salida fue a las: " + vHrBiometricoSalida + " y su hora de incio de la solicitud es: " + vTimeHrsInicioSolicitud + " las horas no cuadran, la hora inicial de la solicitud debe ser igual o menor a la hora de salida del biometrico.");                    vDatos.Merge(vDatos1);                    string vSalidas = "";                    if (vDatos.Rows.Count > 0)                    {                        foreach (DataRow item in vDatos.Rows)                        {                            vSalidas = vSalidas + item["hora"].ToString() + "      ";                        }                    }                    else                    {                        vSalidas = "No hay registros";                    }                    TxEntradas.Text = vEntradas;                    TxSalidas.Text = vSalidas;                    TxSalidaBio.Text = vHoraSalidaBio;                }                vQuery = "SELECT dbo.ObtenerHoraEntrada('" + Session["STECODIGOSAPBIOMETRICO"] + "','" + vIniBio + "') horaentrada";                vDatos = vConexion.obtenerDataTable(vQuery);                string vHoraEntradaBio = vDatos.Rows[0]["horaentrada"].ToString();                string vHrEntradaBio = vHoraEntradaBio.Substring(0, 2);                string vMinEntradaBio = vHoraEntradaBio.Substring(3, 2);                vHoraEntradaBio = vHrEntradaBio + ":" + vMinEntradaBio + ":00";                vQuery = "SELECT dbo.ObtenerHoraAlmuerzoEntrada('" + Session["STECODIGOSAPBIOMETRICO"] + "','" + vIniBio + "') entradat";                vDatos = vConexion.obtenerDataTable(vQuery);                string vHoraEntradaAlmuerzo = vDatos.Rows[0]["entradat"].ToString();                vQuery = "SELECT dbo.ObtenerHoraAlmuerzoSalida('" + Session["STECODIGOSAPBIOMETRICO"] + "','" + vIniBio + "') salidat";                vDatos = vConexion.obtenerDataTable(vQuery);                string vHoraSalidaAlmuerzo = vDatos.Rows[0]["salidat"].ToString();                vQuery = "SELECT dbo.ObtenerHorasAlmuerzo('" + Session["STECODIGOSAPBIOMETRICO"] + "','" + vIniBio + "') total";                vDatos = vConexion.obtenerDataTable(vQuery);                string vHoraTotalAlmuerzo = vDatos.Rows[0]["total"].ToString();
                string vHrAlmuerzo = vHoraTotalAlmuerzo.Substring(0, 2);
                string vMinAlmuerzo = vHoraTotalAlmuerzo.Substring(3, 2);
                vHoraTotalAlmuerzo = vHrAlmuerzo + ":" + vMinAlmuerzo + ":00";


                TxEntradaTurno.Text = Session["STEHORAENTRADA"].ToString();                TxSalidaTurno.Text = Session["STEHORASALIDA"].ToString();                TxEntradaBio.Text = vHoraEntradaBio;                TxEntradaAlmuerzo.Text = vHoraEntradaAlmuerzo;                TxSalidaAlmuerzo.Text = vHoraSalidaAlmuerzo;

                TimeSpan vTimeHIT = TimeSpan.Parse(TxEntradaTurno.Text);                TimeSpan vTimeHFT = TimeSpan.Parse(TxSalidaTurno.Text);                TimeSpan vTimeHEB = TimeSpan.Parse(vHoraEntradaBio);                TimeSpan vTimeHSB = TimeSpan.Parse(vHoraSalidaBio);                TimeSpan vTimeHIS = vHIS.TimeOfDay;                TimeSpan vTimeHFS = vHFS.TimeOfDay;                TimeSpan vDescontarSalidaTemprana = TimeSpan.Zero;                TimeSpan vDescontarEntradaTarde = TimeSpan.Zero;                TimeSpan vTimeHIP = TimeSpan.Zero;                TimeSpan vTimeHFP = TimeSpan.Zero;                TimeSpan vDescontarSalidaTempranaAcumulador = TimeSpan.Zero;                TimeSpan vDescontarEntradasTardesAcumulador = TimeSpan.Zero;                TimeSpan vDescontarAlmuerzoAcumulador = TimeSpan.Zero;                string vAcumuladorPermisos = "";                string vAcumuladorPermisosEntradas = "";                string vAcumuladorPermisosAlmuerzo = "";                string vCantidadRealDescontar = "";                string vPermisoInicial = "";                string vPermisoInicialEntrada = "";

                string dia = vHIS.ToString("dddd");
                string STEDIASTURNO = Session["STEDIASTURNO"].ToString();
                STEDIASTURNO = STEDIASTURNO.Replace(',', '-');

                //ENTRADAS TARDES
                string vCantidadRealDescontarEntrada = "";                if (vFeriadovFI == "si" && vFeriadovFF == "si")
                {
                    vDescontarEntradaTarde = TimeSpan.Zero;
                }
                else if (vFeriadovFI == "si")
                {
                    vDescontarEntradaTarde = TimeSpan.Zero;
                }
                else if (STEDIASTURNO == "L-V" && (dia != "lunes" && dia != "martes" && dia != "miércoles" && dia != "jueves" && dia != "viernes"))
                {
                    vDescontarEntradaTarde = TimeSpan.Zero;
                }
                else if (STEDIASTURNO == "L-D" && (dia != "lunes" && dia != "martes" && dia != "miércoles" && dia != "jueves" && dia != "viernes" && dia != "sábado" && dia != "domingo"))
                {
                    vDescontarEntradaTarde = TimeSpan.Zero;
                }
                else if (STEDIASTURNO == "L-M-V-D" && (dia != "lunes" && dia != "miércoles" && dia != "viernes" && dia != "domingo"))
                {
                    vDescontarEntradaTarde = TimeSpan.Zero;
                }
                else if (vTimeHEB == TimeSpan.Zero)                {                    vDescontarEntradaTarde = TimeSpan.Zero;                }
                else if (vTimeHEB < vTimeHIT)                {                    vDescontarEntradaTarde = TimeSpan.Zero;                }
                else if (vTimeHEB > vTimeHIT)                {                    vDescontarEntradaTarde = vTimeHEB - vTimeHIT;
                    vCantidadRealDescontarEntrada = "TIEMPO TOTAL A DESCONTAR SIN PERMISOS: " + vDescontarEntradaTarde;

                    vQuery = "RSP_TiempoExtraordinarioGenerales 13,'" + Session["STECODIGOSAP"] + "','" + vIniBio + "'";
                    vDatos = vConexion.obtenerDataTable(vQuery);

                    if (vDatos.Rows.Count > 0)
                    {
                        foreach (DataRow item in vDatos.Rows)
                        {
                            string vidPermiso = item["idPermiso"].ToString();
                            string vTipo = item["idTipoPermiso"].ToString();
                            string vDescripcion = item["descripcion"].ToString();
                            vTimeHIP = TimeSpan.Parse(item["horaInicio"].ToString());
                            vTimeHFP = TimeSpan.Parse(item["horaFin"].ToString());

                            vPermisoInicialEntrada = "<br> PERMISO REINTEGRADO: No.(" + vidPermiso + ") " + vTipo + "-" + vDescripcion + " ,HORA INICIO: " + vTimeHIP + " HORA FIN: " + vTimeHFP;

                            if (vTimeHEB > vTimeHIT && vTimeHEB <= vTimeHFP)
                            {
                                vDescontarEntradaTarde = TimeSpan.Zero;
                                LbJustificacionHEB.Text = vCantidadRealDescontarEntrada + vPermisoInicialEntrada;
                                DivDescontarEntrada.Visible = true;
                                break;
                            }
                            else if (vTimeHEB > vTimeHIT && vTimeHEB > vTimeHFP)
                            {
                                vDescontarEntradaTarde = vDescontarEntradaTarde - (vTimeHFP - vTimeHIP);

                                vQuery = "RSP_TiempoExtraordinarioGenerales 14,'" + Session["STECODIGOSAP"] + "','" + vIniBio + "','" + vTimeHFP + "','" + vTimeHEB + "'";
                                vDatos = vConexion.obtenerDataTable(vQuery);

                                if (vDatos.Rows.Count > 0)
                                {
                                    foreach (DataRow i in vDatos.Rows)
                                    {
                                        vidPermiso = i["idPermiso"].ToString();
                                        vDescripcion = i["descripcion"].ToString();
                                        vTipo = i["idTipoPermiso"].ToString();
                                        vTimeHIP = TimeSpan.Parse(i["horaInicio"].ToString());
                                        vTimeHFP = TimeSpan.Parse(i["horaFin"].ToString());
                                        vDescontarEntradasTardesAcumulador = vDescontarEntradasTardesAcumulador + (vTimeHFP - vTimeHIP);
                                        vAcumuladorPermisosEntradas = vAcumuladorPermisosEntradas + "<br> PERMISO REINTEGRADO: " + vTipo + "-" + vDescripcion + " ,HORA INICIO: " + vTimeHIP + " HORA FIN: " + vTimeHFP;
                                    }

                                    vDescontarEntradaTarde = vDescontarEntradaTarde - vDescontarEntradasTardesAcumulador;
                                    LbJustificacionHEB.Text = vCantidadRealDescontarEntrada + vPermisoInicialEntrada + vAcumuladorPermisosEntradas;
                                    DivDescontarEntrada.Visible = true;

                                }
                                else
                                {
                                    vDescontarEntradaTarde = vDescontarEntradaTarde;
                                    LbJustificacionHEB.Text = vCantidadRealDescontarEntrada + vPermisoInicialEntrada;                                    DivDescontarEntrada.Visible = true;
                                }
                                break;
                            }
                        }
                    }
                }
                TxDescontarEntrada.Text = Convert.ToString(vDescontarEntradaTarde);

                //SALIDAS TEMPRANAS
                if (vFeriadovFI == "si" && vFeriadovFF == "si")                {                    vDescontarSalidaTemprana = TimeSpan.Zero;                }
                else if (vFeriadovFI == "si")
                {
                    vDescontarSalidaTemprana = TimeSpan.Zero;
                }
                else if (STEDIASTURNO == "L-V" && (dia != "lunes" && dia != "martes" && dia != "miércoles" && dia != "jueves" && dia != "viernes"))
                {
                    vDescontarSalidaTemprana = TimeSpan.Zero;
                }
                else if (STEDIASTURNO == "L-D" && (dia != "lunes" && dia != "martes" && dia != "miércoles" && dia != "jueves" && dia != "viernes" && dia != "sábado" && dia != "domingo"))
                {
                    vDescontarSalidaTemprana = TimeSpan.Zero;
                }
                else if (STEDIASTURNO == "L-M-V-D" && (dia != "lunes" && dia != "miércoles" && dia != "viernes" && dia != "domingo"))
                {
                    vDescontarSalidaTemprana = TimeSpan.Zero;
                }
                else if (vTimeHSB > vTimeHFT)
                {
                    vDescontarSalidaTemprana = TimeSpan.Zero;
                }
                else if (vTimeHSB == TimeSpan.Zero)
                {
                    vDescontarSalidaTemprana = TimeSpan.Zero;
                }
                else if (vTimeHSB < vTimeHFT && dias == 1)
                {
                    vDescontarSalidaTemprana = TimeSpan.Zero;                    TxJustificacionHSB.Text = "NOTA:COLABORADOR SALIO HASTA EL DIA SIGUIENTE.";                    DivDescontarSalida.Visible = true;
                }
                else if (vTimeHSB < vTimeHFT && dias == 0)
                {
                    vDescontarSalidaTemprana = vTimeHFT - vTimeHSB;
                    vCantidadRealDescontar = "TIEMPO TOTAL A DESCONTAR SIN PERMISOS: " + vDescontarSalidaTemprana;

                    vQuery = "RSP_TiempoExtraordinarioGenerales 13,'" + Session["STECODIGOSAP"] + "','" + vIniBio + "'";                    vDatos = vConexion.obtenerDataTable(vQuery);

                    if (vDatos.Rows.Count > 0)                    {
                        foreach (DataRow item in vDatos.Rows)                        {
                            string vidPermiso = item["idPermiso"].ToString();                            string vTipo = item["idTipoPermiso"].ToString();                            string vDescripcion = item["descripcion"].ToString();                            vTimeHIP = TimeSpan.Parse(item["horaInicio"].ToString());                            vTimeHFP = TimeSpan.Parse(item["horaFin"].ToString());
                            vPermisoInicial = "<br> PERMISO REINTEGRADO: No.(" + vidPermiso + ") " + vTipo + "-" + vDescripcion + " ,HORA INICIO: " + vTimeHIP + " HORA FIN: " + vTimeHFP;

                            if (((vTimeHSB >= vTimeHIP) && (vTimeHSB < vTimeHFP)) && vTimeHFP >= vTimeHFT)                            {                                vDescontarSalidaTemprana = TimeSpan.Zero;                                TxJustificacionHSB.Text = vCantidadRealDescontar + vPermisoInicial;                                DivDescontarSalida.Visible = true;                                break;                            }
                            else if (((vTimeHSB >= vTimeHIP) && (vTimeHSB < vTimeHFP)) && vTimeHFP < vTimeHFT)
                            {
                                vDescontarSalidaTemprana = vDescontarSalidaTemprana - (vTimeHFP - vTimeHIP);

                                vQuery = "RSP_TiempoExtraordinarioGenerales 14,'" + Session["STECODIGOSAP"] + "','" + vIniBio + "','" + vTimeHFP + "','" + vTimeHFT + "'";                                vDatos = vConexion.obtenerDataTable(vQuery);
                                if (vDatos.Rows.Count > 0)                                {                                    foreach (DataRow i in vDatos.Rows)                                    {                                        vidPermiso = item["idPermiso"].ToString();                                        vDescripcion = i["descripcion"].ToString();                                        vTipo = i["idTipoPermiso"].ToString();                                        vTimeHIP = TimeSpan.Parse(i["horaInicio"].ToString());                                        vTimeHFP = TimeSpan.Parse(i["horaFin"].ToString());                                        vDescontarSalidaTempranaAcumulador = vDescontarSalidaTempranaAcumulador + (vTimeHFP - vTimeHIP);                                        vAcumuladorPermisos = vAcumuladorPermisos + "<br> PERMISO REINTEGRADO: No.(" + vidPermiso + ") " + vTipo + "-" + vDescripcion + " ,HORA INICIO: " + vTimeHIP + " HORA FIN: " + vTimeHFP;                                    }
                                    vDescontarSalidaTemprana = vDescontarSalidaTemprana - vDescontarSalidaTempranaAcumulador;                                    TxJustificacionHSB.Text = vCantidadRealDescontar + vPermisoInicial + vAcumuladorPermisos;                                    DivDescontarSalida.Visible = true;
                                }
                                else
                                {
                                    TxJustificacionHSB.Text = vCantidadRealDescontar + vPermisoInicial;                                    DivDescontarSalida.Visible = true;
                                }
                                break;
                            }
                            else if ((vTimeHSB < vTimeHIP) && (vTimeHFP >= vTimeHFT))
                            {
                                vDescontarSalidaTemprana = vDescontarSalidaTemprana - (vTimeHFP - vTimeHIP);
                                vQuery = "RSP_TiempoExtraordinarioGenerales 14,'" + Session["STECODIGOSAP"] + "','" + vIniBio + "','" + vTimeHSB + "','" + vTimeHIP + "'";                                vDatos = vConexion.obtenerDataTable(vQuery);

                                if (vDatos.Rows.Count > 0)                                {                                    foreach (DataRow i in vDatos.Rows)                                    {
                                        vidPermiso = item["idPermiso"].ToString();                                        vDescripcion = i["descripcion"].ToString();                                        vTipo = i["idTipoPermiso"].ToString();                                        vTimeHIP = TimeSpan.Parse(i["horaInicio"].ToString());                                        vTimeHFP = TimeSpan.Parse(i["horaFin"].ToString());                                        vDescontarSalidaTempranaAcumulador = vDescontarSalidaTempranaAcumulador + (vTimeHFP - vTimeHIP);                                        vAcumuladorPermisos = vAcumuladorPermisos + "<br> PERMISO REINTEGRADO: No.(" + vidPermiso + ") " + vTipo + "-" + vDescripcion + " ,HORA INICIO: " + vTimeHIP + " HORA FIN: " + vTimeHFP;                                    }                                    vDescontarSalidaTemprana = vDescontarSalidaTemprana - vDescontarSalidaTempranaAcumulador;                                    TxJustificacionHSB.Text = vCantidadRealDescontar + vPermisoInicial + vAcumuladorPermisos;                                    DivDescontarSalida.Visible = true;                                }
                                else
                                {
                                    TxJustificacionHSB.Text = vCantidadRealDescontar + vPermisoInicial;                                    DivDescontarSalida.Visible = true;
                                }
                                break;
                            }
                            else if ((vTimeHSB < vTimeHIP) && (vTimeHFP < vTimeHFT))
                            {

                                vDescontarSalidaTemprana = vDescontarSalidaTemprana - ((vTimeHIP - vTimeHSB) + (vTimeHFT - vTimeHFP));

                                vQuery = "RSP_TiempoExtraordinarioGenerales 14,'" + Session["STECODIGOSAP"] + "','" + vIniBio + "','" + vTimeHSB + "','" + vTimeHIP + "'";                                vDatos = vConexion.obtenerDataTable(vQuery);                                if (vDatos.Rows.Count > 0)                                {                                    foreach (DataRow i in vDatos.Rows)                                    {
                                        vidPermiso = i["idPermiso"].ToString();                                        vDescripcion = i["descripcion"].ToString();                                        vTipo = i["idTipoPermiso"].ToString();                                        vTimeHIP = TimeSpan.Parse(i["horaInicio"].ToString());                                        vTimeHFP = TimeSpan.Parse(i["horaFin"].ToString());                                        vDescontarSalidaTempranaAcumulador = vDescontarSalidaTempranaAcumulador + (vTimeHFP - vTimeHIP);                                        vAcumuladorPermisos = vAcumuladorPermisos + "<br> PERMISO REINTEGRADO: No.(" + vidPermiso + ") " + vTipo + "-" + vDescripcion + " ,HORA INICIO: " + vTimeHIP + " HORA FIN: " + vTimeHFP;                                    }                                }


                                vQuery = "RSP_TiempoExtraordinarioGenerales 14,'" + Session["STECODIGOSAP"] + "','" + vIniBio + "','" + vTimeHFP + "','" + vTimeHFT + "'";                                vDatos = vConexion.obtenerDataTable(vQuery);                                if (vDatos.Rows.Count > 0)                                {                                    foreach (DataRow i in vDatos.Rows)                                    {
                                        vidPermiso = item["idPermiso"].ToString();                                        vDescripcion = item["descripcion"].ToString();                                        vTipo = item["idTipoPermiso"].ToString();                                        vTimeHIP = TimeSpan.Parse(i["horaInicio"].ToString());                                        vTimeHFP = TimeSpan.Parse(i["horaFin"].ToString());                                        vDescontarSalidaTempranaAcumulador = vDescontarSalidaTempranaAcumulador + (vTimeHFP - vTimeHIP);                                        vAcumuladorPermisos = vAcumuladorPermisos + "<br> PERMISO REINTEGRADO: No.(" + vidPermiso + ") " + vTipo + "-" + vDescripcion + " ,HORA INICIO: " + vTimeHIP + " HORA FIN: " + vTimeHFP;                                    }                                }

                                vDescontarSalidaTemprana = vDescontarSalidaTemprana - vDescontarSalidaTempranaAcumulador;                                TxJustificacionHSB.Text = vCantidadRealDescontar + vPermisoInicial + vAcumuladorPermisos;                                DivDescontarSalida.Visible = true;
                                break;
                            }

                        }
                    }
                }
                TxDescontarSalida.Text = Convert.ToString(vDescontarSalidaTemprana);


                //ALMUERZO MAS DE 30 MIN
                TimeSpan vTimeAlmuerzo = TimeSpan.Parse(vHoraTotalAlmuerzo);                TimeSpan vTimeAlmuerzoAprobado = TimeSpan.Parse("00:30:00");

                TimeSpan vTimeAlmuerzoInicial = TimeSpan.Parse("11:30:00");
                TimeSpan vTimeAlmuerzoFinal = TimeSpan.Parse("14:00:00");

                TimeSpan vDescAlmuerzo = TimeSpan.Zero;
                TimeSpan vTimeTotalAlmuerzo = TimeSpan.Zero;

                if (vTimeAlmuerzo > vTimeAlmuerzoAprobado)                {                    vTimeTotalAlmuerzo = vTimeAlmuerzo - vTimeAlmuerzoAprobado;                    LbTotalAlmuerzo.Text = "TOTAL ALMUERZO: " + vHoraTotalAlmuerzo + ". NOTA: DEL TOTAL SE DESCONTO 30 MIN TIEMPO ESTABLECIDO EN POLITICA PARA TOMAR SUS ALIMENTOS. SI SUPERA LOS 30 MIN, Y SI NO TIENE PERMISOS INGRESADOS SE DESCONTARA DE LA HORA EXTRA";                }                else                {                    vTimeTotalAlmuerzo = TimeSpan.Zero;                    LbTotalAlmuerzo.Text = "TOTAL ALMUERZO: " + vHoraTotalAlmuerzo + ". NOTA: USTED ESTA DENTRO DE LO ESTABLECIDO EN LA POLITICA 30 MIN PARA TOMAR SUS ALIMENTOS.";                }

                if (vFeriadovFI == "si" && vFeriadovFF == "si")
                {
                    vDescAlmuerzo = TimeSpan.Zero;
                }
                else if (vFeriadovFI == "si")
                {
                    vDescAlmuerzo = TimeSpan.Zero;
                }
                else if (STEDIASTURNO == "L-V" && (dia != "lunes" && dia != "martes" && dia != "miércoles" && dia != "jueves" && dia != "viernes"))
                {
                    vDescAlmuerzo = TimeSpan.Zero;
                }
                else if (STEDIASTURNO == "L-D" && (dia != "lunes" && dia != "martes" && dia != "miércoles" && dia != "jueves" && dia != "viernes" && dia != "sábado" && dia != "domingo"))
                {
                    vDescAlmuerzo = TimeSpan.Zero;
                }
                else if (STEDIASTURNO == "L-M-V-D" && (dia != "lunes" && dia != "miércoles" && dia != "viernes" && dia != "domingo"))
                {
                    vDescAlmuerzo = TimeSpan.Zero;
                }
                else if (vTimeTotalAlmuerzo == TimeSpan.Zero)
                {
                    vDescAlmuerzo = TimeSpan.Zero;
                }
                else if (vTimeTotalAlmuerzo != TimeSpan.Zero)
                {
                    vDescAlmuerzo = vTimeTotalAlmuerzo;

                    vQuery = "RSP_TiempoExtraordinarioGenerales 13,'" + Session["STECODIGOSAP"] + "','" + vIniBio + "'";                    vDatos = vConexion.obtenerDataTable(vQuery);

                    if (vDatos.Rows.Count > 0)                    {
                        foreach (DataRow item in vDatos.Rows)                        {
                            string vidPermiso = item["idPermiso"].ToString();                            string vTipo = item["idTipoPermiso"].ToString();                            string vDescripcion = item["descripcion"].ToString();                            vTimeHIP = TimeSpan.Parse(item["horaInicio"].ToString());                            vTimeHFP = TimeSpan.Parse(item["horaFin"].ToString());

                            if ((vTimeHIP >= vTimeAlmuerzoInicial) && (vTimeHFP <= vTimeAlmuerzoFinal))
                            {
                                vDescontarAlmuerzoAcumulador = vDescontarAlmuerzoAcumulador + (vTimeHFP - vTimeHIP);
                                vAcumuladorPermisosAlmuerzo = vAcumuladorPermisosAlmuerzo + "<br> PERMISO REINTEGRADO: No.(" + vidPermiso + ") " + vTipo + "-" + vDescripcion + " ,HORA INICIO: " + vTimeHIP + " HORA FIN: " + vTimeHFP;
                            }
                            else if (vTimeHIP < vTimeAlmuerzoInicial && vTimeHFP > vTimeAlmuerzoFinal)
                            {
                                vDescontarAlmuerzoAcumulador = vDescontarAlmuerzoAcumulador + (vTimeAlmuerzoFinal - vTimeAlmuerzoInicial);
                                vAcumuladorPermisosAlmuerzo = vAcumuladorPermisosAlmuerzo + "<br> PERMISO REINTEGRADO: No.(" + vidPermiso + ") " + vTipo + "-" + vDescripcion + " ,HORA INICIO: " + vTimeHIP + " HORA FIN: " + vTimeHFP;
                            }
                            else if (vTimeHIP < vTimeAlmuerzoInicial && vTimeHFP >= vTimeAlmuerzoInicial && vTimeHFP <= vTimeAlmuerzoFinal)
                            {
                                vDescontarAlmuerzoAcumulador = vDescontarAlmuerzoAcumulador + (vTimeHFP - vTimeAlmuerzoInicial);
                                vAcumuladorPermisosAlmuerzo = vAcumuladorPermisosAlmuerzo + "<br> PERMISO REINTEGRADO: No.(" + vidPermiso + ") " + vTipo + "-" + vDescripcion + " ,HORA INICIO: " + vTimeHIP + " HORA FIN: " + vTimeHFP;
                            }
                            else if (vTimeHIP >= vTimeAlmuerzoInicial && vTimeHFP > vTimeAlmuerzoFinal && vTimeHIP != vTimeAlmuerzoFinal)
                            {
                                vDescontarAlmuerzoAcumulador = vDescontarAlmuerzoAcumulador + (vTimeAlmuerzoFinal - vTimeHIP);
                                vAcumuladorPermisosAlmuerzo = vAcumuladorPermisosAlmuerzo + "<br> PERMISO REINTEGRADO: No.(" + vidPermiso + ") " + vTipo + "-" + vDescripcion + " ,HORA INICIO: " + vTimeHIP + " HORA FIN: " + vTimeHFP;
                            }

                        }
                    }

                    if (vDescontarAlmuerzoAcumulador > vTimeTotalAlmuerzo)
                    {
                        vDescAlmuerzo = TimeSpan.Zero;
                    }
                    else
                    {
                        vDescAlmuerzo = vTimeTotalAlmuerzo - vDescontarAlmuerzoAcumulador;
                    }
                }

                LbReintegroAlmuerzo.Text = vAcumuladorPermisosAlmuerzo;
                DivAlmuerzo.Visible = true;
                TxTotalAlmuerzo.Text = Convert.ToString(vDescAlmuerzo);


                ////////////////////////////////////REAL APROBAR     

                TimeSpan STEHORAENTRADA = TimeSpan.Parse(Session["STEHORAENTRADA"].ToString());                TimeSpan STEHORASALIDA = TimeSpan.Parse(Session["STEHORASALIDA"].ToString());                string vCatDescontarEntrada = "";                string vCatDescontarSalida = "";                TimeSpan vAcuDesDiurnas = TimeSpan.Zero;                TimeSpan vAcuDesNoc = TimeSpan.Zero;                TimeSpan vAcuDesNocNoc = TimeSpan.Zero;                TimeSpan vFaltaDiurnas = TimeSpan.Zero;                TimeSpan vFaltaNoc = TimeSpan.Zero;                TimeSpan vFaltaNocNoc = TimeSpan.Zero;

                TimeSpan vFaltaDiurnasRRHH = TimeSpan.Zero;                TimeSpan vFaltaNocRRHH = TimeSpan.Zero;                TimeSpan vFaltaNocNocRRHH = TimeSpan.Zero;                string vDiurnasTot = Session["STEHRDIURNASOLICITADAS"].ToString() + ":" + Session["STEMINDIURNASOLICITADAS"].ToString() + ":00";                string vNocTot = Session["STEHRNOCSOLICITADAS"].ToString() + ":" + Session["STEMINNOCSOLICITADAS"].ToString() + ":00";                string vNocNocTot = Session["STEHRNOCNOCSOLICITADAS"].ToString() + ":" + Session["STEMINNOCNOCSOLICITADAS"].ToString() + ":00";                TimeSpan STETOTDIURNAS = TimeSpan.Parse(vDiurnasTot);                TimeSpan STETOTNOC = TimeSpan.Parse(vNocTot);                TimeSpan STETOTNOCNOC = TimeSpan.Parse(vNocNocTot);

                TimeSpan STETOTDIURNASRRHH = TimeSpan.Parse(vDiurnasTot);                TimeSpan STETOTNOCRRHH = TimeSpan.Parse(vNocTot);                TimeSpan STETOTNOCNOCRRHH = TimeSpan.Parse(vNocNocTot);                TimeSpan vRealAprobarDiurnas = TimeSpan.Parse(vDiurnasTot);                TimeSpan vRealAprobarNoc = TimeSpan.Parse(vNocTot);                TimeSpan vRealAprobarNocNoc = TimeSpan.Parse(vNocNocTot);

                TimeSpan vRealAprobarDiurnasRRHH = TimeSpan.Parse(vDiurnasTot);                TimeSpan vRealAprobarNocRRHH = TimeSpan.Parse(vNocTot);                TimeSpan vRealAprobarNocNocRRHH = TimeSpan.Parse(vNocNocTot);

                if (STEHORAENTRADA >= TimeSpan.Parse("05:00:00") && STEHORAENTRADA <= TimeSpan.Parse("19:00:00") && vDescAlmuerzo > TimeSpan.Zero)                {                    vCatDescontarEntrada = "Diurnas";                    vAcuDesDiurnas = vAcuDesDiurnas + vDescAlmuerzo;                }                else if (STEHORAENTRADA >= TimeSpan.Parse("19:01:00") && STEHORAENTRADA <= TimeSpan.Parse("22:00:00") && vDescAlmuerzo > TimeSpan.Zero)                {                    vCatDescontarEntrada = "Noc";                    vAcuDesNoc = vAcuDesNoc + vDescAlmuerzo;                }                else if (STEHORAENTRADA >= TimeSpan.Parse("22:01:00") && STEHORAENTRADA <= TimeSpan.Parse("04:59:00") && vDescAlmuerzo > TimeSpan.Zero)                {                    vCatDescontarEntrada = "NocNoc";                    vAcuDesNocNoc = vAcuDesNocNoc + vDescAlmuerzo;                }                if (STEHORAENTRADA >= TimeSpan.Parse("05:00:00") && STEHORAENTRADA <= TimeSpan.Parse("19:00:00"))                {                    vCatDescontarEntrada = "Diurnas";                    vAcuDesDiurnas = vAcuDesDiurnas + vDescontarEntradaTarde;                }                else if (STEHORAENTRADA >= TimeSpan.Parse("19:01:00") && STEHORAENTRADA <= TimeSpan.Parse("22:00:00"))                {                    vCatDescontarEntrada = "Noc";                    vAcuDesNoc = vAcuDesNoc + vDescontarEntradaTarde;                }                else if (STEHORAENTRADA >= TimeSpan.Parse("22:01:00") && STEHORAENTRADA <= TimeSpan.Parse("04:59:00"))                {                    vCatDescontarEntrada = "NocNoc";                    vAcuDesNocNoc = vAcuDesNocNoc + vDescontarEntradaTarde;                }                if (STEHORASALIDA >= TimeSpan.Parse("05:00:00") && STEHORASALIDA <= TimeSpan.Parse("19:00:00"))                {                    vCatDescontarSalida = "Diurnas";                    vAcuDesDiurnas = vAcuDesDiurnas + vDescontarSalidaTemprana;                }                else if (STEHORASALIDA >= TimeSpan.Parse("19:01:00") && STEHORASALIDA <= TimeSpan.Parse("22:00:00"))                {                    vCatDescontarSalida = "Noc";                    vAcuDesNoc = vAcuDesNoc + vDescontarSalidaTemprana;                }                else if (STEHORASALIDA >= TimeSpan.Parse("22:01:00") && STEHORASALIDA <= TimeSpan.Parse("04:59:00"))                {                    vCatDescontarSalida = "NocNoc";                    vAcuDesNocNoc = vAcuDesNocNoc + vDescontarSalidaTemprana;                }                if (vAcuDesDiurnas > TimeSpan.Parse("00:00:00"))                {                    if (STETOTDIURNAS != TimeSpan.Parse("00:00:00"))                    {                        if (vAcuDesDiurnas < STETOTDIURNAS)                        {                            vRealAprobarDiurnas = STETOTDIURNAS - vAcuDesDiurnas;                        }                        else                        {                            vRealAprobarDiurnas = TimeSpan.Zero;                            vFaltaDiurnas = vAcuDesDiurnas - STETOTDIURNAS;                            if (STETOTNOC != TimeSpan.Parse("00:00:00"))                            {                                if (vFaltaDiurnas < STETOTNOC)                                {                                    vRealAprobarNoc = STETOTNOC - vFaltaDiurnas;                                }                                else                                {                                    vRealAprobarNoc = TimeSpan.Zero;                                    vFaltaNoc = vFaltaDiurnas - STETOTNOC;                                    if (STETOTNOCNOC != TimeSpan.Parse("00:00:00"))                                    {                                        if (vFaltaNoc < STETOTNOCNOC)                                        {                                            vRealAprobarNocNoc = STETOTNOCNOC - vFaltaNoc;                                        }                                        else                                        {                                            vRealAprobarNocNoc = TimeSpan.Zero;                                            vFaltaNocNoc = vFaltaNoc - STETOTNOCNOC;                                        }                                    }                                }                            }                            else if (STETOTNOCNOC != TimeSpan.Parse("00:00:00"))                            {                                if (vFaltaDiurnas < STETOTNOCNOC)                                {                                    vRealAprobarNocNoc = STETOTNOCNOC - vFaltaDiurnas;                                }                                else                                {                                    vRealAprobarNocNoc = TimeSpan.Zero;                                    vFaltaNocNoc = vFaltaDiurnas - STETOTNOCNOC;                                }                            }                        }                    }                    else if (STETOTNOC != TimeSpan.Parse("00:00:00"))                    {                        if (vAcuDesDiurnas < STETOTNOC)                        {                            vRealAprobarNoc = STETOTNOC - vAcuDesDiurnas;                        }                        else                        {                            vRealAprobarNoc = TimeSpan.Zero;                            vFaltaNoc = vAcuDesDiurnas - STETOTNOC;                            if (STETOTNOCNOC != TimeSpan.Parse("00:00:00"))                            {                                if (vFaltaNoc < STETOTNOCNOC)                                {                                    vRealAprobarNocNoc = STETOTNOCNOC - vFaltaNoc;                                }                                else                                {                                    vRealAprobarNocNoc = TimeSpan.Zero;                                    vFaltaNocNoc = vFaltaNoc - STETOTNOCNOC;                                }                            }                        }                    }                    else if (STETOTNOCNOC != TimeSpan.Parse("00:00:00"))                    {                        if (vAcuDesDiurnas < STETOTNOCNOC)                        {                            vRealAprobarNocNoc = STETOTNOCNOC - vAcuDesDiurnas;                        }                        else                        {                            vRealAprobarNocNoc = TimeSpan.Zero;                            vFaltaNocNoc = vAcuDesDiurnas - STETOTNOCNOC;                        }                    }                    STETOTDIURNAS = vRealAprobarDiurnas;                    STETOTNOC = vRealAprobarNoc;                    STETOTNOCNOC = vRealAprobarNocNoc;                }

                if (vAcuDesNoc > TimeSpan.Parse("00:00:00"))                {                    if (STETOTNOC != TimeSpan.Parse("00:00:00"))                    {                        if (vAcuDesNoc < STETOTNOC)                        {                            vRealAprobarNoc = STETOTNOC - vAcuDesNoc;                        }                        else                        {                            vRealAprobarNoc = TimeSpan.Zero;                            vFaltaNoc = vAcuDesNoc - STETOTNOC;                            if (STETOTNOCNOC != TimeSpan.Parse("00:00:00"))                            {                                if (vFaltaNoc < STETOTNOCNOC)                                {                                    vRealAprobarNocNoc = STETOTNOCNOC - vFaltaNoc;                                }                                else                                {                                    vRealAprobarNocNoc = TimeSpan.Zero;                                    vFaltaNocNoc = vFaltaNoc - STETOTNOCNOC;                                    if (STETOTDIURNAS != TimeSpan.Parse("00:00:00"))                                    {                                        if (vFaltaNocNoc < STETOTDIURNAS)                                        {                                            vRealAprobarDiurnas = STETOTDIURNAS - vFaltaNocNoc;                                        }                                        else                                        {                                            vRealAprobarDiurnas = TimeSpan.Zero;                                            vFaltaDiurnas = vFaltaNocNoc - STETOTDIURNAS;                                        }                                    }                                }                            }                            else if (STETOTDIURNAS != TimeSpan.Parse("00:00:00"))
                            {                                if (vFaltaNoc < STETOTDIURNAS)                                {                                    vRealAprobarDiurnas = STETOTDIURNAS - vFaltaNoc;                                }                                else                                {                                    vRealAprobarDiurnas = TimeSpan.Zero;                                    vFaltaDiurnas = vFaltaNoc - STETOTDIURNAS;                                }                            }                        }                    }                    else if (STETOTNOCNOC != TimeSpan.Parse("00:00:00"))                    {                        if (vAcuDesNoc < STETOTNOCNOC)                        {                            vRealAprobarNocNoc = STETOTNOCNOC - vAcuDesNoc;                        }                        else                        {                            vRealAprobarNocNoc = TimeSpan.Zero;                            vFaltaNocNoc = vAcuDesNoc - STETOTNOCNOC;                            if (vFaltaNocNoc < STETOTDIURNAS)                            {                                vRealAprobarDiurnas = STETOTDIURNAS - vFaltaNocNoc;                            }                            else                            {                                vRealAprobarDiurnas = TimeSpan.Zero;                                vFaltaDiurnas = vFaltaNocNoc - STETOTDIURNAS;                            }                        }                    }                    else if (STETOTDIURNAS != TimeSpan.Parse("00:00:00"))                    {                        if (vAcuDesNoc < STETOTDIURNAS)                        {                            vRealAprobarDiurnas = STETOTDIURNAS - vAcuDesNoc;                        }                        else                        {                            vRealAprobarDiurnas = TimeSpan.Zero;                            vFaltaDiurnas = vAcuDesNoc - STETOTDIURNAS;                        }                    }                    STETOTDIURNAS = vRealAprobarDiurnas;                    STETOTNOC = vRealAprobarNoc;                    STETOTNOCNOC = vRealAprobarNocNoc;                }                if (vAcuDesNocNoc > TimeSpan.Parse("00:00:00"))                {                    if (STETOTNOCNOC != TimeSpan.Parse("00:00:00"))                    {                        if (vAcuDesNocNoc < STETOTNOCNOC)                        {                            vRealAprobarNocNoc = STETOTNOCNOC - vAcuDesNocNoc;                        }                        else                        {                            vRealAprobarNocNoc = TimeSpan.Zero;                            vFaltaNocNoc = vAcuDesNocNoc - STETOTNOCNOC;                            if (STETOTDIURNAS != TimeSpan.Parse("00:00:00"))                            {                                if (vFaltaNocNoc < STETOTDIURNAS)                                {                                    vRealAprobarDiurnas = STETOTDIURNAS - vFaltaNocNoc;                                }                                else                                {                                    vRealAprobarDiurnas = TimeSpan.Zero;                                    vFaltaDiurnas = vFaltaNocNoc - STETOTDIURNAS;                                    if (vFaltaDiurnas < STETOTNOC)                                    {                                        vRealAprobarNoc = STETOTNOC - vFaltaDiurnas;                                    }                                    else                                    {                                        vRealAprobarNoc = TimeSpan.Zero;                                        vFaltaNoc = vFaltaDiurnas - STETOTNOC;                                    }                                }                            }                            else if (STETOTNOC != TimeSpan.Parse("00:00:00"))                            {                                if (vFaltaNocNoc < STETOTNOC)                                {                                    vRealAprobarNoc = STETOTNOC - vFaltaNocNoc;                                }                                else                                {                                    vRealAprobarNoc = TimeSpan.Zero;                                    vFaltaNoc = vFaltaNocNoc - STETOTNOC;                                }                            }                        }                    }                    else if (STETOTDIURNAS != TimeSpan.Parse("00:00:00"))                    {                        if (vAcuDesNocNoc < STETOTDIURNAS)                        {                            vRealAprobarDiurnas = STETOTDIURNAS - vAcuDesNocNoc;                        }                        else                        {                            vRealAprobarDiurnas = TimeSpan.Zero;                            vFaltaDiurnas = vAcuDesNocNoc - STETOTDIURNAS;                            if (vFaltaDiurnas < STETOTNOC)                            {                                vRealAprobarNoc = STETOTNOC - vFaltaDiurnas;                            }                            else                            {                                vRealAprobarNoc = TimeSpan.Zero;                                vFaltaNoc = vFaltaDiurnas - STETOTNOC;                            }                        }                    }                    else if (STETOTNOC != TimeSpan.Parse("00:00:00"))                    {                        if (vAcuDesNocNoc < STETOTNOC)                        {                            vRealAprobarNoc = STETOTNOC - vAcuDesNocNoc;                        }                        else                        {                            vRealAprobarNoc = TimeSpan.Zero;                            vFaltaNoc = vAcuDesNocNoc - STETOTNOC;                        }                    }                    STETOTDIURNAS = vRealAprobarDiurnas;                    STETOTNOC = vRealAprobarNoc;                    STETOTNOCNOC = vRealAprobarNocNoc;                }                if (vAcuDesDiurnas > TimeSpan.Parse("00:00:00") || vAcuDesNoc > TimeSpan.Parse("00:00:00") || vAcuDesNocNoc > TimeSpan.Parse("00:00:00"))                {                    string vRealHrsDiurnas = Convert.ToString(STETOTDIURNAS).Substring(0, 2);                    string vRealMinDiurnas = Convert.ToString(STETOTDIURNAS).Substring(3, 2);                    Double vRealDiurnas_Resumen = ((Convert.ToInt32(vRealHrsDiurnas) * 60) + Convert.ToInt32(vRealMinDiurnas));                    vRealDiurnas_Resumen = vRealDiurnas_Resumen / 60;                    string vRealHrsNoc = Convert.ToString(STETOTNOC).Substring(0, 2);                    string vRealMinNoc = Convert.ToString(STETOTNOC).Substring(3, 2);                    Double vRealNoc_Resumen = ((Convert.ToInt32(vRealHrsNoc) * 60) + Convert.ToInt32(vRealMinNoc));                    vRealNoc_Resumen = vRealNoc_Resumen / 60;                    string vRealHrsNocNoc = Convert.ToString(STETOTNOCNOC).Substring(0, 2);                    string vRealMinNocNoc = Convert.ToString(STETOTNOCNOC).Substring(3, 2);                    Double vRealNocNoc_Resumen = ((Convert.ToInt32(vRealHrsNocNoc) * 60) + Convert.ToInt32(vRealMinNocNoc));                    vRealNocNoc_Resumen = vRealNocNoc_Resumen / 60;                    int vRealHrsTotal = Convert.ToInt32(vRealHrsDiurnas) + Convert.ToInt32(vRealHrsNoc) + Convert.ToInt32(vRealHrsNocNoc) + acu_horasDomingosFeriados_Resumen;                    int vRealMinTotal = Convert.ToInt32(vRealMinDiurnas) + Convert.ToInt32(vRealMinNoc) + Convert.ToInt32(vRealMinNocNoc) + acu_minutosDomingosFeriados_Resumen;                    Double vRealTot_Resumen = ((vRealHrsTotal * 60) + vRealMinTotal);                    vRealTot_Resumen = vRealTot_Resumen / 60;                    TxRealDiurnas.Text = vRealHrsDiurnas + ":" + vRealMinDiurnas + " (" + vRealDiurnas_Resumen.ToString("N2") + ")";                    TxRealNoc.Text = vRealHrsNoc + ":" + vRealMinNoc + " (" + vRealNoc_Resumen.ToString("N2") + ")";                    TxRealNocNoc.Text = vRealHrsNocNoc + ":" + vRealMinNocNoc + " (" + vRealNocNoc_Resumen.ToString("N2") + ")";                    TxRealDomingoFeriados.Text = acu_horasDomingosFeriados_Resumen + ":" + acu_minutosDomingosFeriados_Resumen + " (" + DomingosFeriados_Resumen.ToString("N2") + ")";                    TxRealTotal.Text = vRealHrsTotal + ":" + vRealMinTotal + " (" + vRealTot_Resumen.ToString("N2") + ")";                    Session["STEHRDIURNASREAL"] = vRealHrsDiurnas;                    Session["STEMINDIURNASREAL"] = vRealMinDiurnas;                    Session["STEHRNOCREAL"] = vRealHrsNoc;                    Session["STEMINNOCREAL"] = vRealMinNoc;                    Session["STEHRNOCNOCREAL"] = vRealHrsNocNoc;                    Session["STEMINNOCNOCREAL"] = vRealMinNocNoc;                    Session["STEHRDOMINGOFERIADOREAL"] = acu_horasDomingosFeriados_Resumen;                    Session["STEMINDOMINGOFERIADOREAL"] = acu_minutosDomingosFeriados_Resumen;                    Session["STEHRTOTREAL"] = vRealHrsTotal;                    Session["STEMINTOTREAL"] = vRealMinTotal;                }                else                {                    TxRealNocNoc.Text = acu_horasNoc_Noc_Resumen + ":" + acu_minutosNoc_Noc_Resumen + " (" + NocNoc_Resumen.ToString("N2") + ")";                    TxRealDiurnas.Text = acu_horasDiurnas_Resumen + ":" + acu_minutosDiurnas_Resumen + " (" + Diurnas_Resumen.ToString("N2") + ")";                    TxRealNoc.Text = acu_horasNoc_Resumen + ":" + acu_minutosNoc_Resumen + " (" + Noc_Resumen.ToString("N2") + ")";                    TxRealDomingoFeriados.Text = acu_horasDomingosFeriados_Resumen + ":" + acu_minutosDomingosFeriados_Resumen + " (" + DomingosFeriados_Resumen.ToString("N2") + ")";                    TxRealTotal.Text = acu_horasTotales_Resumen + ":" + acu_minutosTotales_Resumen + " (" + Totales_Resumen.ToString("N2") + ")";                    Session["STEHRDIURNASREAL"] = acu_horasDiurnas_Resumen;                    Session["STEMINDIURNASREAL"] = acu_minutosDiurnas_Resumen;                    Session["STEHRNOCREAL"] = acu_horasNoc_Resumen;                    Session["STEMINNOCREAL"] = acu_minutosNoc_Resumen;                    Session["STEHRNOCNOCREAL"] = acu_horasNoc_Noc_Resumen;                    Session["STEMINNOCNOCREAL"] = acu_minutosNoc_Noc_Resumen;                    Session["STEHRDOMINGOFERIADOREAL"] = acu_horasDomingosFeriados_Resumen;                    Session["STEMINDOMINGOFERIADOREAL"] = acu_minutosDomingosFeriados_Resumen;                    Session["STEHRTOTREAL"] = acu_horasTotales_Resumen;                    Session["STEMINTOTREAL"] = acu_minutosTotales_Resumen;                }

                Session["STEFALTANTEDIURNAS"] = Convert.ToString(vFaltaDiurnas);
                Session["STEFALTANTENOC"] = Convert.ToString(vFaltaNoc);
                Session["STEFALTANTENOCNOC"] = Convert.ToString(vFaltaNocNoc);

                if (!string.IsNullOrEmpty(vEx)){
                    if (vEx.Equals("2"))
                    {
                        vQuery = "RSP_TiempoExtraordinarioGenerales 43,'" + Session["STECODIGOSAP"] + "','" + vIniBio + "'";
                        vDatos = vConexion.obtenerDataTable(vQuery);
                        Session["STESOLICITUDESHrsFALTANTES"] = vDatos;

                        string vSolicitudes = "";
                        TimeSpan vFaltaDiurnasHr = TimeSpan.Zero;
                        TimeSpan vFaltaNocHr = TimeSpan.Zero;
                        TimeSpan vFaltaNocNocHr = TimeSpan.Zero;

                        if (vDatos.Rows.Count > 0)
                        {
                            foreach (DataRow item in vDatos.Rows)                            {
                                string solicitud = item["idSolicitud"].ToString();
                                string vDiurnas = item["faltanteHrsDiurnas"].ToString();
                                string vNoc = item["faltanteHrsNoc"].ToString();
                                string vNocNoc = item["faltanteHrsNocNoc"].ToString();

                                TimeSpan vDiurnasConver = TimeSpan.Parse(vDiurnas);
                                TimeSpan vNocConver = TimeSpan.Parse(vNoc);
                                TimeSpan vNocNocConver = TimeSpan.Parse(vNocNoc);

                                vSolicitudes = vSolicitudes + solicitud + "  ";
                                vFaltaDiurnasHr = vFaltaDiurnasHr + vDiurnasConver;
                                vFaltaNocHr = vFaltaNocHr + vNocConver;
                                vFaltaNocNocHr = vFaltaNocNocHr + vNocNocConver;
                            }

                            if (vFaltaDiurnasHr == TimeSpan.Zero && vFaltaNocHr == TimeSpan.Zero && vFaltaNocNocHr == TimeSpan.Zero)
                            {
                                TxTotRRHH.Text = TxTotalHoras.Text;
                                TxTotDiurnasRRHH.Text = TxHrDiurnas.Text;
                                TxTotNocRRHH.Text = TxHrNoc.Text;
                                TxTotNocNocRRHH.Text = TxHrNocNoc.Text;
                                TxTotDomFeriadoRRHH.Text = TxHrDomFeriado.Text;
                                UpdatePanel26.Update();
                                LbMensajeRRHH.Text = "La solicitud se va aprobar en su totalidad, las reducciones se efectuaron en las primeras solicitudes que aprobo del colaborador: " + TxEmpleado.Text + "  dia de inicio de la solicitud: " + vIniBio + ". Id de solicitudes aprobadas: " + vSolicitudes + " , no tiene horas pendientes que disminuir por concepto de entrada tarde, salida temprana o almuerzo";

                                Session["STEHRDIURNASREAL"] = Session["STEHRDIURNASOLICITADAS"].ToString();
                                Session["STEMINDIURNASREAL"] = Session["STEMINDIURNASOLICITADAS"].ToString();
                                Session["STEHRNOCREAL"] = Session["STEHRNOCSOLICITADAS"].ToString();
                                Session["STEMINNOCREAL"] = Session["STEMINNOCSOLICITADAS"].ToString();
                                Session["STEHRNOCNOCREAL"] = Session["STEHRNOCNOCSOLICITADAS"].ToString();
                                Session["STEMINNOCNOCREAL"] = Session["STEMINNOCNOCSOLICITADAS"].ToString();
                                Session["STEHRDOMINGOFERIADOREAL"] = Session["STEHRDOMINGOFERIADOSOLICITADAS"].ToString();
                                Session["STEMINDOMINGOFERIADOREAL"] = Session["STEMINDOMINGOFERIADOSOLICITADAS"].ToString();

                                Session["STEHRTOTREAL"] = Session["STEHRTOTALSOLICITADAS"].ToString();
                                Session["STEMINTOTREAL"] = Session["STEMINTOTALSOLICITADAS"].ToString();

                                Session["STEFALTANTEDIURNAS"] = TimeSpan.Zero;
                                Session["STEFALTANTENOC"] = TimeSpan.Zero;
                                Session["STEFALTANTENOCNOC"] = TimeSpan.Zero;
                            }
                            else
                            {
                                if (vFaltaDiurnasHr > TimeSpan.Parse("00:00:00"))
                                {
                                    if (STETOTDIURNASRRHH != TimeSpan.Parse("00:00:00"))
                                    {
                                        if (vFaltaDiurnasHr < STETOTDIURNASRRHH)
                                        {
                                            vRealAprobarDiurnasRRHH = STETOTDIURNASRRHH - vFaltaDiurnasHr;
                                        }
                                        else
                                        {
                                            vRealAprobarDiurnasRRHH = TimeSpan.Zero;
                                            vFaltaDiurnasRRHH = vFaltaDiurnasHr - STETOTDIURNASRRHH;
                                            if (STETOTNOCRRHH != TimeSpan.Parse("00:00:00"))
                                            {
                                                if (vFaltaDiurnasRRHH < STETOTNOCRRHH)
                                                {
                                                    vRealAprobarNocRRHH = STETOTNOCRRHH - vFaltaDiurnasRRHH;
                                                }
                                                else
                                                {
                                                    vRealAprobarNocRRHH = TimeSpan.Zero;
                                                    vFaltaNocRRHH = vFaltaDiurnasRRHH - STETOTNOCRRHH;
                                                    if (STETOTNOCNOCRRHH != TimeSpan.Parse("00:00:00"))
                                                    {
                                                        if (vFaltaNocRRHH < STETOTNOCNOCRRHH)
                                                        {
                                                            vRealAprobarNocNocRRHH = STETOTNOCNOCRRHH - vFaltaNocRRHH;
                                                        }
                                                        else
                                                        {
                                                            vRealAprobarNocNocRRHH = TimeSpan.Zero;
                                                            vFaltaNocNocRRHH = vFaltaNocRRHH - STETOTNOCNOC;
                                                        }
                                                    }

                                                }
                                            }
                                            else if (STETOTNOCNOCRRHH != TimeSpan.Parse("00:00:00"))
                                            {
                                                if (vFaltaDiurnasRRHH < STETOTNOCNOCRRHH)
                                                {
                                                    vRealAprobarNocNocRRHH = STETOTNOCNOCRRHH - vFaltaDiurnasRRHH;
                                                }
                                                else
                                                {
                                                    vRealAprobarNocNocRRHH = TimeSpan.Zero;
                                                    vFaltaNocNocRRHH = vFaltaDiurnasRRHH - STETOTNOCNOCRRHH;
                                                }
                                            }
                                        }
                                    }
                                    else if (STETOTNOCRRHH != TimeSpan.Parse("00:00:00"))
                                    {
                                        if (vFaltaDiurnasHr < STETOTNOCRRHH)
                                        {
                                            vRealAprobarNocRRHH = STETOTNOCRRHH - vFaltaDiurnasHr;
                                        }
                                        else
                                        {
                                            vRealAprobarNocRRHH = TimeSpan.Zero;
                                            vFaltaNocRRHH = vFaltaDiurnasHr - STETOTNOCRRHH;
                                            if (STETOTNOCNOCRRHH != TimeSpan.Parse("00:00:00"))
                                            {
                                                if (vFaltaNocRRHH < STETOTNOCNOCRRHH)
                                                {
                                                    vRealAprobarNocNocRRHH = STETOTNOCNOCRRHH - vFaltaNocRRHH;
                                                }
                                                else
                                                {
                                                    vRealAprobarNocNocRRHH = TimeSpan.Zero;
                                                    vFaltaNocNocRRHH = vFaltaNocRRHH - STETOTNOCNOCRRHH;
                                                }
                                            }
                                        }
                                    }
                                    else if (STETOTNOCNOCRRHH != TimeSpan.Parse("00:00:00"))
                                    {
                                        if (vFaltaDiurnasHr < STETOTNOCNOCRRHH)
                                        {
                                            vRealAprobarNocNocRRHH = STETOTNOCNOCRRHH - vFaltaDiurnasHr;
                                        }
                                        else
                                        {
                                            vRealAprobarNocNocRRHH = TimeSpan.Zero;
                                            vFaltaNocNocRRHH = vFaltaDiurnasHr - STETOTNOCNOCRRHH;
                                        }
                                    }

                                    STETOTDIURNASRRHH = vRealAprobarDiurnasRRHH;
                                    STETOTNOCRRHH = vRealAprobarNocRRHH;
                                    STETOTNOCNOCRRHH = vRealAprobarNocNocRRHH;
                                }

                                if (vFaltaNocHr > TimeSpan.Parse("00:00:00"))
                                {
                                    if (STETOTNOCRRHH != TimeSpan.Parse("00:00:00"))
                                    {
                                        if (vFaltaNocHr < STETOTNOCRRHH)
                                        {
                                            vRealAprobarNocRRHH = STETOTNOCRRHH - vFaltaNocHr;
                                        }
                                        else
                                        {
                                            vRealAprobarNocRRHH = TimeSpan.Zero;
                                            vFaltaNocRRHH = vFaltaNocHr - STETOTNOCRRHH;
                                            if (STETOTNOCNOCRRHH != TimeSpan.Parse("00:00:00"))
                                            {
                                                if (vFaltaNocRRHH < STETOTNOCNOCRRHH)
                                                {
                                                    vRealAprobarNocNocRRHH = STETOTNOCNOCRRHH - vFaltaNocRRHH;
                                                }
                                                else
                                                {
                                                    vRealAprobarNocNocRRHH = TimeSpan.Zero;
                                                    vFaltaNocNocRRHH = vFaltaNocRRHH - STETOTNOCNOCRRHH;
                                                    if (STETOTDIURNASRRHH != TimeSpan.Parse("00:00:00"))
                                                    {
                                                        if (vFaltaNocNocRRHH < STETOTDIURNASRRHH)
                                                        {
                                                            vRealAprobarDiurnasRRHH = STETOTDIURNASRRHH - vFaltaNocNocRRHH;
                                                        }
                                                        else
                                                        {
                                                            vRealAprobarDiurnasRRHH = TimeSpan.Zero;
                                                            vFaltaDiurnas = vFaltaNocNocRRHH - STETOTDIURNASRRHH;
                                                        }
                                                    }
                                                }
                                            }
                                            else if (STETOTDIURNASRRHH != TimeSpan.Parse("00:00:00"))
                                            {
                                                if (vFaltaNocRRHH < STETOTDIURNASRRHH)
                                                {
                                                    vRealAprobarDiurnasRRHH = STETOTDIURNASRRHH - vFaltaNocRRHH;
                                                }
                                                else
                                                {
                                                    vRealAprobarDiurnasRRHH = TimeSpan.Zero;
                                                    vFaltaDiurnasRRHH = vFaltaNocRRHH - STETOTDIURNASRRHH;
                                                }
                                            }
                                        }
                                    }
                                    else if (STETOTNOCNOCRRHH != TimeSpan.Parse("00:00:00"))
                                    {
                                        if (vFaltaNocHr < STETOTNOCNOCRRHH)
                                        {
                                            vRealAprobarNocNocRRHH = STETOTNOCNOCRRHH - vFaltaNocHr;
                                        }
                                        else
                                        {
                                            vRealAprobarNocNocRRHH = TimeSpan.Zero;
                                            vFaltaNocNocRRHH = vFaltaNocHr - STETOTNOCNOCRRHH;
                                            if (vFaltaNocNocRRHH < STETOTDIURNASRRHH)
                                            {
                                                vRealAprobarDiurnasRRHH = STETOTDIURNASRRHH - vFaltaNocNocRRHH;
                                            }
                                            else
                                            {
                                                vRealAprobarDiurnasRRHH = TimeSpan.Zero;
                                                vFaltaDiurnasRRHH = vFaltaNocNocRRHH - STETOTDIURNASRRHH;
                                            }
                                        }
                                    }
                                    else if (STETOTDIURNASRRHH != TimeSpan.Parse("00:00:00"))
                                    {
                                        if (vFaltaNocHr < STETOTDIURNASRRHH)
                                        {
                                            vRealAprobarDiurnasRRHH = STETOTDIURNASRRHH - vFaltaNocHr;
                                        }
                                        else
                                        {
                                            vRealAprobarDiurnasRRHH = TimeSpan.Zero;
                                            vFaltaDiurnasRRHH = vFaltaNocHr - STETOTDIURNASRRHH;
                                        }
                                    }

                                    STETOTDIURNASRRHH = vRealAprobarDiurnasRRHH;
                                    STETOTNOCRRHH = vRealAprobarNocRRHH;
                                    STETOTNOCNOCRRHH = vRealAprobarNocNocRRHH;
                                }

                                if (vFaltaNocNocHr > TimeSpan.Parse("00:00:00"))
                                {
                                    if (STETOTNOCNOCRRHH != TimeSpan.Parse("00:00:00"))
                                    {
                                        if (vFaltaNocNocHr < STETOTNOCNOCRRHH)
                                        {
                                            vRealAprobarNocNocRRHH = STETOTNOCNOCRRHH - vFaltaNocNocHr;
                                        }
                                        else
                                        {
                                            vRealAprobarNocNocRRHH = TimeSpan.Zero;
                                            vFaltaNocNocRRHH = vFaltaNocNocHr - STETOTNOCNOCRRHH;
                                            if (STETOTDIURNASRRHH != TimeSpan.Parse("00:00:00"))
                                            {
                                                if (vFaltaNocNocRRHH < STETOTDIURNASRRHH)
                                                {
                                                    vRealAprobarDiurnasRRHH = STETOTDIURNASRRHH - vFaltaNocNocRRHH;
                                                }
                                                else
                                                {
                                                    vRealAprobarDiurnasRRHH = TimeSpan.Zero;
                                                    vFaltaDiurnasRRHH = vFaltaNocNocRRHH - STETOTDIURNASRRHH;
                                                    if (vFaltaDiurnasRRHH < STETOTNOCRRHH)
                                                    {
                                                        vRealAprobarNocRRHH = STETOTNOCRRHH - vFaltaDiurnasRRHH;
                                                    }
                                                    else
                                                    {
                                                        vRealAprobarNocRRHH = TimeSpan.Zero;
                                                        vFaltaNocRRHH = vFaltaDiurnasRRHH - STETOTNOCRRHH;
                                                    }
                                                }

                                            }
                                            else if (STETOTNOCRRHH != TimeSpan.Parse("00:00:00"))
                                            {
                                                if (vFaltaNocNocRRHH < STETOTNOCRRHH)
                                                {
                                                    vRealAprobarNocRRHH = STETOTNOCRRHH - vFaltaNocNocRRHH;
                                                }
                                                else
                                                {
                                                    vRealAprobarNocRRHH = TimeSpan.Zero;
                                                    vFaltaNocRRHH = vFaltaNocNocRRHH - STETOTNOCRRHH;
                                                }
                                            }
                                        }
                                    }
                                    else if (STETOTDIURNASRRHH != TimeSpan.Parse("00:00:00"))
                                    {
                                        if (vFaltaNocNocHr < STETOTDIURNASRRHH)
                                        {
                                            vRealAprobarDiurnasRRHH = STETOTDIURNASRRHH - vFaltaNocNocHr;
                                        }
                                        else
                                        {
                                            vRealAprobarDiurnasRRHH = TimeSpan.Zero;
                                            vFaltaDiurnasRRHH = vFaltaNocNocHr - STETOTDIURNASRRHH;
                                            if (vFaltaDiurnasRRHH < STETOTNOCRRHH)
                                            {
                                                vRealAprobarNocRRHH = STETOTNOCRRHH - vFaltaDiurnasRRHH;
                                            }
                                            else
                                            {
                                                vRealAprobarNocRRHH = TimeSpan.Zero;
                                                vFaltaNocRRHH = vFaltaDiurnasRRHH - STETOTNOCRRHH;
                                            }
                                        }
                                    }
                                    else if (STETOTNOCRRHH != TimeSpan.Parse("00:00:00"))
                                    {
                                        if (vFaltaNocNocHr < STETOTNOCRRHH)
                                        {
                                            vRealAprobarNocRRHH = STETOTNOCRRHH - vFaltaNocNocHr;
                                        }
                                        else
                                        {
                                            vRealAprobarNocRRHH = TimeSpan.Zero;
                                            vFaltaNocRRHH = vFaltaNocNocHr - STETOTNOCRRHH;
                                        }
                                    }

                                    STETOTDIURNASRRHH = vRealAprobarDiurnasRRHH;
                                    STETOTNOCRRHH = vRealAprobarNocRRHH;
                                    STETOTNOCNOCRRHH = vRealAprobarNocNocRRHH;
                                }

                                if (vFaltaDiurnasHr > TimeSpan.Parse("00:00:00") || vFaltaNocHr > TimeSpan.Parse("00:00:00") || vFaltaNocNocHr > TimeSpan.Parse("00:00:00"))
                                {
                                    string vRealHrsDiurnas = Convert.ToString(STETOTDIURNASRRHH).Substring(0, 2);
                                    string vRealMinDiurnas = Convert.ToString(STETOTDIURNASRRHH).Substring(3, 2);

                                    Double vRealDiurnas_Resumen = ((Convert.ToInt32(vRealHrsDiurnas) * 60) + Convert.ToInt32(vRealMinDiurnas));
                                    vRealDiurnas_Resumen = vRealDiurnas_Resumen / 60;

                                    string vRealHrsNoc = Convert.ToString(STETOTNOCRRHH).Substring(0, 2);
                                    string vRealMinNoc = Convert.ToString(STETOTNOCRRHH).Substring(3, 2);

                                    Double vRealNoc_Resumen = ((Convert.ToInt32(vRealHrsNoc) * 60) + Convert.ToInt32(vRealMinNoc));
                                    vRealNoc_Resumen = vRealNoc_Resumen / 60;


                                    string vRealHrsNocNoc = Convert.ToString(STETOTNOCNOCRRHH).Substring(0, 2);
                                    string vRealMinNocNoc = Convert.ToString(STETOTNOCNOCRRHH).Substring(3, 2);

                                    Double vRealNocNoc_Resumen = ((Convert.ToInt32(vRealHrsNocNoc) * 60) + Convert.ToInt32(vRealMinNocNoc));
                                    vRealNocNoc_Resumen = vRealNocNoc_Resumen / 60;


                                    int vRealHrsTotal = Convert.ToInt32(vRealHrsDiurnas) + Convert.ToInt32(vRealHrsNoc) + Convert.ToInt32(vRealHrsNoc) + acu_horasDomingosFeriados_Resumen;
                                    int vRealMinTotal = Convert.ToInt32(vRealMinDiurnas) + Convert.ToInt32(vRealMinNoc) + Convert.ToInt32(vRealMinNocNoc) + acu_minutosDomingosFeriados_Resumen;
                                    Double vRealTot_Resumen = ((vRealHrsTotal * 60) + vRealMinTotal);
                                    vRealTot_Resumen = vRealTot_Resumen / 60;


                                    TxTotDiurnasRRHH.Text = vRealHrsDiurnas + ":" + vRealMinDiurnas + " (" + vRealDiurnas_Resumen.ToString("N2") + ")";
                                    TxTotNocRRHH.Text = vRealHrsNoc + ":" + vRealMinNoc + " (" + vRealNoc_Resumen.ToString("N2") + ")";
                                    TxTotNocNocRRHH.Text = vRealHrsNocNoc + ":" + vRealMinNocNoc + " (" + vRealNocNoc_Resumen.ToString("N2") + ")";
                                    TxTotDomFeriadoRRHH.Text = acu_horasDomingosFeriados_Resumen + ":" + acu_minutosDomingosFeriados_Resumen + " (" + DomingosFeriados_Resumen.ToString("N2") + ")";
                                    TxTotRRHH.Text = vRealHrsTotal + ":" + vRealMinTotal + " (" + vRealTot_Resumen.ToString("N2") + ")";
                                    UpdatePanel26.Update();

                                    Session["STEHRDIURNASREAL"] = vRealHrsDiurnas;
                                    Session["STEMINDIURNASREAL"] = vRealMinDiurnas;
                                    Session["STEHRNOCREAL"] = vRealHrsNoc;
                                    Session["STEMINNOCREAL"] = vRealMinNoc;
                                    Session["STEHRNOCNOCREAL"] = vRealHrsNocNoc;
                                    Session["STEMINNOCNOCREAL"] = vRealMinNocNoc;
                                    Session["STEHRDOMINGOFERIADOREAL"] = acu_horasDomingosFeriados_Resumen;
                                    Session["STEMINDOMINGOFERIADOREAL"] = acu_minutosDomingosFeriados_Resumen;

                                    Session["STEHRTOTREAL"] = vRealHrsTotal;
                                    Session["STEMINTOTREAL"] = vRealMinTotal;

                                    Session["STEFALTANTEDIURNAS"] = vFaltaDiurnasRRHH;
                                    Session["STEFALTANTENOC"] = vFaltaNocRRHH;
                                    Session["STEFALTANTENOCNOC"] = vFaltaNocNocRRHH;

                                    LbMensajeRRHH.Text = "La solicitud no se va aprobar en su totalidad, el colaborador: " + TxEmpleado.Text + " tiene pendientes horas para descontar (Tiempo que no cubrieron en su totalidad las anteriores solicitudes aprobadas) por motivos de llagada tarde, salida temprana o almuerzo.  Dia de inicio de la solicitud: " + vIniBio + ". Id de solicitudes aprobadas: " + vSolicitudes;

                                }
                                else
                                {
                                    TxTotRRHH.Text = TxTotalHoras.Text;
                                    TxTotDiurnasRRHH.Text = TxHrDiurnas.Text;
                                    TxTotNocRRHH.Text = TxHrNoc.Text;
                                    TxTotNocNocRRHH.Text = TxHrNocNoc.Text;
                                    TxTotDomFeriadoRRHH.Text = TxHrDomFeriado.Text;
                                    UpdatePanel26.Update();
                                    LbMensajeRRHH.Text = "La solicitud se va aprobar en su totalidad, las reducciones se efectuaron en las primeras solicitudes que aprobo del colaborador: " + TxEmpleado.Text + "  dia de inicio de la solicitud: " + vIniBio + ". Id de solicitudes aprobadas: " + vSolicitudes + " , no tiene horas pendientes que disminuir por concepto de entrada tarde, salida temprana o almuerzo";

                                    Session["STEHRDIURNASREAL"] = Session["STEHRDIURNASOLICITADAS"].ToString();
                                    Session["STEMINDIURNASREAL"] = Session["STEMINDIURNASOLICITADAS"].ToString();
                                    Session["STEHRNOCREAL"] = Session["STEHRNOCSOLICITADAS"].ToString();
                                    Session["STEMINNOCREAL"] = Session["STEMINNOCSOLICITADAS"].ToString();
                                    Session["STEHRNOCNOCREAL"] = Session["STEHRNOCNOCSOLICITADAS"].ToString();
                                    Session["STEMINNOCNOCREAL"] = Session["STEMINNOCNOCSOLICITADAS"].ToString();
                                    Session["STEHRDOMINGOFERIADOREAL"] = Session["STEHRDOMINGOFERIADOSOLICITADAS"].ToString();
                                    Session["STEMINDOMINGOFERIADOREAL"] = Session["STEMINDOMINGOFERIADOSOLICITADAS"].ToString();

                                    Session["STEHRTOTREAL"] = Session["STEHRTOTALSOLICITADAS"].ToString();
                                    Session["STEMINTOTREAL"] = Session["STEMINTOTALSOLICITADAS"].ToString();

                                    Session["STEFALTANTEDIURNAS"] = TimeSpan.Zero;
                                    Session["STEFALTANTENOC"] = TimeSpan.Zero;
                                    Session["STEFALTANTENOCNOC"] = TimeSpan.Zero;

                                }
                            }

                        }
                        else
                        {
                            TxTotRRHH.Text = TxRealTotal.Text;
                            TxTotDiurnasRRHH.Text = TxRealDiurnas.Text;
                            TxTotNocRRHH.Text = TxRealNoc.Text;
                            TxTotNocNocRRHH.Text = TxRealNocNoc.Text;
                            TxTotDomFeriadoRRHH.Text = TxRealDomingoFeriados.Text;
                            UpdatePanel26.Update();
                            LbMensajeRRHH.Text = "No se ha aprobado ninguna solicitud del colaborador: " + TxEmpleado.Text + " dia de la solicitud: " + vIniBio + ". El total a aprobar sera igual a las horas con las reducciones en caso que tenga.";
                        }


                    }
                }
            }        }
        protected void DdlTipoTrabajo_SelectedIndexChanged(object sender, EventArgs e)        {            DdlTipoDescripcion.Items.Clear();            string vQuery = "RSP_TiempoExtraordinarioGenerales 4," + DdlTipoTrabajo.SelectedValue;            DataTable vDatos = vConexion.obtenerDataTable(vQuery);            if (vDatos.Rows.Count > 0)            {                DivCategoria.Visible = true;                TituloCategoria.Text = DdlTipoTrabajo.SelectedItem.Text + ":";                DdlTipoDescripcion.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });                foreach (DataRow item in vDatos.Rows)                {                    DdlTipoDescripcion.Items.Add(new ListItem { Value = item["idTipoTrabajoDescripcion"].ToString(), Text = item["nombreTrabajo"].ToString() });                }            }            else            {                DivCategoria.Visible = false;            }


            //DECISION PARA OCULTAR O MOSTRAR EL CAMPO DE SYSAID Y HOJA DE SERVICIO
            if (DdlTipoTrabajo.SelectedValue.Equals("7") && (Session["STESUBGERENCIA"].ToString() != "4" || Session["STESUBGERENCIA"].ToString() != "2"))            {                DivSysAid.Visible = true;                UpdatePanel3.Update();                UpdatePanel6.Update();                FuHojaServicio.Visible = true;                lbHojaServicio.Visible = true;                FuHojaServicio.Visible = true;                btnVisualizarHoja.Visible = true;

                TxHojaServicio.Value = string.Empty;
                Img1.Src = "/images/vistaPrevia1.JPG";                UpdatePanel12.Update();


            }            else if (Session["STESUBGERENCIA"].ToString() == "4" || Session["STESUBGERENCIA"].ToString() == "2")
            {                DivSysAid.Visible = true;
                //DivHojaServicio.Visible = true;

                FuHojaServicio.Visible = true;                lbHojaServicio.Visible = true;                btnVisualizarHoja.Visible = true;
                UpdatePanel3.Update();                UpdatePanel6.Update();                UpdatePanel12.Update();            }            else
            {                DivSysAid.Visible = false;                TxPeticion.Text = "";                TxTituloSysaid.Text = "";
                //DivHojaServicio.Visible = false;

                lbHojaServicio.Visible = false;                FuHojaServicio.Visible = false;                btnVisualizarHoja.Visible = false;
                TxHojaServicio.Value = string.Empty;
                Img1.Src = "/images/vistaPrevia1.JPG";



                UpdatePanel12.Update();                UpdatePanel3.Update();            }        }
        protected void BtnCerrar_Click(object sender, EventArgs e)        {
            CerrarModal("VisualizarImagen");
            //Checkbox1.Checked = false;
            //UpClimatizacion.Update();
        }
        public void CerrarModal(String vModal)        {            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#" + vModal + "').modal('hide');", true);

        }
        protected void BtnCrearSolicitud_Click(object sender, EventArgs e)        {            try            {                validacionesCrearSolicitud();                String vFI = TxFechaInicio.Text != "" ? TxFechaInicio.Text : "1999-01-01 00:00:00";                String vFF = TxFechaRegreso.Text != "" ? TxFechaRegreso.Text : "1999-01-01 00:00:00";                DateTime desde = Convert.ToDateTime(vFI);                DateTime hasta = Convert.ToDateTime(vFF);                DateTime vFechaInicio = Convert.ToDateTime(vFI);                LbInformacionTE.Text = "Buen dia <b> " + TxEmpleado.Text + "</b><br /><br />" +                   "Fechas solicitadas del <b>" + desde.ToString("yyyy-MM-dd HH:mm:ss") + "</b> al <b>" + hasta.ToString("yyyy-MM-dd HH:mm:ss") + "</b>" + " .Total de horas <b> " + TxTotalHoras.Text + "</b> <br /><br />" +                   "Trabajo realizado: <b>" + TxDescripcionTrabajo.Text + "</b><br /><br />";                LbInformacionPreguntaTE.Text = "<b>¿Está seguro que desea enviar la solicitud en el rango de fechas y horas detalladas?</b>";                UpdateAutorizarMensaje.Update();                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);

            }            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }        }
        private void validacionesCrearSolicitud()        {

            String vEx = Request.QueryString["ex"];

            if (RbFormaTrabajo.SelectedValue.Equals(""))
                throw new Exception("Falta que seleccione como realizo el trabajo si de forma remota o presencial.");            if (Session["STEAPROBACIONSUBGERENTE"].ToString() == "True" && DdlMotivoAprobacionSubgerente.SelectedValue.Equals("0"))                throw new Exception("Falta que seleccione motivo por el cual no pudo ingresar la solicitud en el tiempo estipulado en la politica");            if (Session["STEAPROBACIONSUBGERENTE"].ToString() == "True" && TxSoliAprobacionSubGerente.Text.Equals(""))                throw new Exception("Falta que ingrese detalle por el cual no pudo ingresar la solicitud en el tiempo estipulado en la politica");            if (RbCambioTurno.Text.Equals("1") && DDLCambioTurnoColaborador.SelectedValue.Equals("0"))                throw new Exception("Falta que seleccione el colaborador con quien realizo cambio de turno.");

            if (RbCambioTurno.Text.Equals("1") && TxMotivoCambioTurno.Text.Equals(""))                throw new Exception("Falta que ingrese motivo del cambio de turno que solicito.");            if (TxFechaInicio.Text.Equals(""))                throw new Exception("Ingrese una fecha de inicio valida.");            if (TxFechaRegreso.Text.Equals(""))                throw new Exception("Ingrese una fecha de fin valida.");

            //DECISION PARA OCULTAR O MOSTRAR LAS OPCIONES DE CONDUCTORES
            if ((Session["STEPUESTOJEFE"].ToString() != "20000383" && Session["STEPUESTOJEFE"].ToString() != "20000395" && Session["STEPUESTOCOLABORADOR"].ToString() != "20000409") && DdlConductor.SelectedValue.Equals("2"))                throw new Exception("Falta que seleccione la opción Solicito Conductor");            if ((Session["STEPUESTOJEFE"].ToString() != "20000383" && Session["STEPUESTOJEFE"].ToString() != "20000395" && Session["STEPUESTOCOLABORADOR"].ToString() != "20000409") && DdlConductor.SelectedValue == "1" && DdlConductorNombre.SelectedValue.Equals("0"))                throw new Exception("Falta que seleccione nombre del conductor.");

            //PARA EL TIPO DE TRABAJO
            if (DdlTipoTrabajo.SelectedValue.Equals("0"))                throw new Exception("Falta que seleccione Tipo de Trabajo.");            string vQuery = "RSP_TiempoExtraordinarioGenerales 4," + DdlTipoTrabajo.SelectedValue;            DataTable vDatos = vConexion.obtenerDataTable(vQuery);            if (vDatos.Rows.Count > 0 && DdlTipoDescripcion.SelectedValue.Equals("0"))                throw new Exception("Falta que seleccione opción de " + DdlTipoTrabajo.SelectedItem.Text + " de la lista.");            if (TxDescripcionTrabajo.Text.Equals(""))                throw new Exception("Ingrese el detalle del trabajo que realizo.");

            //PARA EL SYSAID
            if ((Session["STESUBGERENCIA"].ToString().Equals("4") || Session["STESUBGERENCIA"].ToString().Equals("2")) && TxPeticion.Text.Equals(""))                throw new Exception("Falta que ingrese el número de petición.");            if ((Session["STESUBGERENCIA"].ToString().Equals("4") || Session["STESUBGERENCIA"].ToString().Equals("2")) && TxPeticion.Text != "" && TxTituloSysaid.Text.Equals(""))                throw new Exception("Favor verificar el número de SysAid sea valido.");            if (DdlTipoTrabajo.SelectedValue.Equals("7") && (Session["STESUBGERENCIA"].ToString() != "4" || Session["STESUBGERENCIA"].ToString() != "2") && TxPeticion.Text.Equals(""))                throw new Exception("Falta que ingrese el número de petición.");            if (DdlTipoTrabajo.SelectedValue.Equals("7") && (Session["STESUBGERENCIA"].ToString() != "4" || Session["STESUBGERENCIA"].ToString() != "2") && TxPeticion.Text != "" && TxTituloSysaid.Text.Equals(""))                throw new Exception("Favor verificar el número de SysAid sea valido.");

            if (string.IsNullOrEmpty(vEx) || vEx != "7"){
                vQuery = "RSP_TiempoExtraordinarioGenerales 6,'" + TxPeticion.Text + "','" + Session["STECODIGOSAP"] + "'";
                vDatos = vConexion.obtenerDataTable(vQuery);
                string cantSysAid = vDatos.Rows[0]["cantSysAid"].ToString();

                vQuery = "RSP_TiempoExtraordinarioGeneralesBiometrico 1,'" + TxPeticion.Text + "'";
                vDatos = vConexionSysAid.obtenerDataTableSysAid(vQuery);
                //string vstatus = vDatos.Rows[0]["status"].ToString();

                if (Convert.ToInt32(cantSysAid) > 0 && (vDatos.Rows[0]["status"].ToString() == "3" || vDatos.Rows[0]["status"].ToString() == "4" || vDatos.Rows[0]["status"].ToString() == "8"))
                    throw new Exception("Usted ya tiene ingresada una solicitud de tiempo extraordinario con el mismo número de SysAid: " + TxPeticion.Text + " y el estatus de la petición es Cerrada");


                //SOLO PARA PROYECTO Y PROPUESTAS
                if (DdlTipoTrabajo.SelectedValue.Equals("1") || DdlTipoTrabajo.SelectedValue.Equals("2"))
                {
                    vQuery = "RSP_TiempoExtraordinarioGenerales 7," + DdlTipoDescripcion.SelectedValue;
                    vDatos = vConexion.obtenerDataTable(vQuery);
                    string cantHrsProyectoRegistradas = vDatos.Rows[0]["RESULTADO"].ToString();
                    if (cantHrsProyectoRegistradas == "")
                    {
                        cantHrsProyectoRegistradas = "0";
                    }


                    vQuery = "RSP_TiempoExtraordinarioGenerales 8," + DdlTipoDescripcion.SelectedValue;
                    vDatos = vConexion.obtenerDataTable(vQuery);
                    string cantHrsProyecto = vDatos.Rows[0]["totalHrs"].ToString();

                    Decimal faltante = Convert.ToDecimal(cantHrsProyecto) - Convert.ToDecimal(cantHrsProyectoRegistradas);

                    //Decimal vHrssolicitadas = (Convert.ToDecimal(Session["STEHRTOTALSOLICITADAS"].ToString()) *60 )+ Convert.ToDecimal(Session["STEMINTOTALSOLICITADAS"].ToString());
                    Decimal vHrssolicitadas = ((Convert.ToDecimal(Session["STEHRTOTALSOLICITADAS"].ToString()) * 60) + Convert.ToDecimal(Session["STEMINTOTALSOLICITADAS"].ToString())) / 60;

                    if (vHrssolicitadas > faltante)
                        throw new Exception("La cantidad de horas solicitadas superan la cantidad de horas disponibles para el proyecto o propuesta, Total horas disponibles: " + faltante);
                }

                if ((Session["STESUBGERENCIA"].ToString().Equals("4") || Session["STESUBGERENCIA"].ToString().Equals("2")) && TxHojaServicio.Value == string.Empty)
                    throw new Exception("Falta que suba la hoja de servicio.");

                if (DdlTipoTrabajo.SelectedValue.Equals("7") && (Session["STESUBGERENCIA"].ToString() != "4" || Session["STESUBGERENCIA"].ToString() != "2") && TxHojaServicio.Value == string.Empty)
                    throw new Exception("Falta que suba la hoja de servicio.");

            }
            else
            {
                DataTable vDatosSolicitud = new DataTable();
                vDatosSolicitud = (DataTable)Session["STEDATOSSOLICITUDINDIVIDUAL"];

                string vSysAid = vDatosSolicitud.Rows[0]["sysAid"].ToString();

                string vTipoTrbajo = vDatosSolicitud.Rows[0]["idTipoTrabajo"].ToString();
                string vDescTipoTrabajo = vDatosSolicitud.Rows[0]["idTipoTrabajoDescripcion"].ToString();

                Decimal vHrs = Convert.ToDecimal(vDatosSolicitud.Rows[0]["hrsTotalSolicitado"].ToString());
                Decimal vMin = Convert.ToDecimal(vDatosSolicitud.Rows[0]["minTotalSolicitado"].ToString());
                Decimal vHrsSolicitadasSinModificar = ((vHrs * 60) + vMin) / 60;
                Decimal vHrssolicitadas = ((Convert.ToDecimal(Session["STEHRTOTALSOLICITADAS"].ToString()) * 60) + Convert.ToDecimal(Session["STEMINTOTALSOLICITADAS"].ToString())) / 60;



                //SOLO PARA PROYECTO Y PROPUESTAS   
                if (vDescTipoTrabajo == DdlTipoDescripcion.SelectedValue && vHrsSolicitadasSinModificar > vHrssolicitadas)
                {
                    Decimal vHrsExcedentes = vHrssolicitadas - vHrsSolicitadasSinModificar;

                    vQuery = "RSP_TiempoExtraordinarioGenerales 7," + DdlTipoDescripcion.SelectedValue;
                    vDatos = vConexion.obtenerDataTable(vQuery);
                    string cantHrsProyectoRegistradas = vDatos.Rows[0]["RESULTADO"].ToString();
                    if (cantHrsProyectoRegistradas=="")
                    {
                        cantHrsProyectoRegistradas = "0";
                    }

                    vQuery = "RSP_TiempoExtraordinarioGenerales 8," + DdlTipoDescripcion.SelectedValue;
                    vDatos = vConexion.obtenerDataTable(vQuery);
                    string cantHrsProyecto = vDatos.Rows[0]["totalHrs"].ToString();

                    Decimal faltante = Convert.ToDecimal(cantHrsProyecto) - Convert.ToDecimal(cantHrsProyectoRegistradas);

                    if (vHrsExcedentes > faltante)
                        throw new Exception("La cantidad de horas solicitadas superan la cantidad de horas disponibles para el proyecto o propuesta " + ", total horas disponibles: " + faltante);
                }
                else if (vDescTipoTrabajo != DdlTipoDescripcion.SelectedValue)
                {
                    vQuery = "RSP_TiempoExtraordinarioGenerales 7," + DdlTipoDescripcion.SelectedValue;
                    vDatos = vConexion.obtenerDataTable(vQuery);
                    string cantHrsProyectoRegistradas = vDatos.Rows[0]["RESULTADO"].ToString();

                    vQuery = "RSP_TiempoExtraordinarioGenerales 8," + DdlTipoDescripcion.SelectedValue;
                    vDatos = vConexion.obtenerDataTable(vQuery);
                    string cantHrsProyecto = vDatos.Rows[0]["totalHrs"].ToString();

                    Decimal faltante = Convert.ToDecimal(cantHrsProyecto) - Convert.ToDecimal(cantHrsProyectoRegistradas);

                    //Decimal vHrssolicitadas = (Convert.ToDecimal(Session["STEHRTOTALSOLICITADAS"].ToString()) *60 )+ Convert.ToDecimal(Session["STEMINTOTALSOLICITADAS"].ToString());
                    Decimal vHrssolicitada = ((Convert.ToDecimal(Session["STEHRTOTALSOLICITADAS"].ToString()) * 60) + Convert.ToDecimal(Session["STEMINTOTALSOLICITADAS"].ToString())) / 60;

                    if (vHrssolicitada > faltante)
                        throw new Exception("La cantidad de horas solicitadas superan la cantidad de horas disponibles para el proyecto o propuesta, Total horas disponibles: " + faltante);
                }


                if (vSysAid != TxPeticion.Text)
                {
                    vQuery = "RSP_TiempoExtraordinarioGenerales 6,'" + TxPeticion.Text + "','" + Session["STECODIGOSAP"] + "'";
                    vDatos = vConexion.obtenerDataTable(vQuery);
                    string cantSysAid = vDatos.Rows[0]["cantSysAid"].ToString();

                    vQuery = "RSP_TiempoExtraordinarioGeneralesBiometrico 1,'" + TxPeticion.Text + "'";
                    vDatos = vConexionSysAid.obtenerDataTableSysAid(vQuery);
                    //string vstatus = vDatos.Rows[0]["status"].ToString();

                    if (Convert.ToInt32(cantSysAid) > 0 && (vDatos.Rows[0]["status"].ToString() == "3" || vDatos.Rows[0]["status"].ToString() == "4" || vDatos.Rows[0]["status"].ToString() == "8"))
                        throw new Exception("Usted ya tiene ingresada una solicitud de tiempo extraordinario con el mismo número de SysAid: " + TxPeticion.Text + " y el estatus de la petición es Cerrada");

                }

                if ((Session["STESUBGERENCIA"].ToString().Equals("4") || Session["STESUBGERENCIA"].ToString().Equals("2")) && TxHojaServicio.Value == string.Empty && RbCambioHoja.SelectedValue.Equals("1"))
                    throw new Exception("Falta que suba la hoja de servicio.");

                if (DdlTipoTrabajo.SelectedValue.Equals("7") && (Session["STESUBGERENCIA"].ToString() != "4" || Session["STESUBGERENCIA"].ToString() != "2") && TxHojaServicio.Value == string.Empty && RbCambioHoja.SelectedValue.Equals("1"))
                    throw new Exception("Falta que suba la hoja de servicio.");
            }
        }
        protected void DdlConductor_SelectedIndexChanged(object sender, EventArgs e)        {            if (DdlConductor.SelectedValue.Equals("1"))            {                DdlConductorNombre.Visible = true;                TxMotivoNoConductor.Visible = false;            }            else            {                DdlConductorNombre.Visible = false;                TxMotivoNoConductor.Visible = true;                TxMotivoNoConductor.Text = "No se requirio conductor para esta solicitud.";            }        }
        private void limpiarCrearSolicitud()
        {
            RbCambioTurno.SelectedIndex = -1;
            TxMotivoCambioTurno.Text = String.Empty;
            DDLCambioTurnoColaborador.SelectedIndex = -1;
            TxFechaInicio.Text = String.Empty;
            TxFechaRegreso.Text = String.Empty;
            DdlMotivoAprobacionSubgerente.SelectedIndex = -1;
            TxSoliAprobacionSubGerente.Text = String.Empty;
            DdlConductor.SelectedIndex = -1;
            DdlConductorNombre.SelectedIndex = -1;
            DdlTipoTrabajo.SelectedIndex = -1;
            TituloCategoria.Text = String.Empty;
            DdlTipoDescripcion.SelectedIndex = -1;
            TxDescripcionTrabajo.Text = String.Empty;
            TxPeticion.Text = String.Empty;
            TxTituloSysaid.Text = String.Empty;
            TxImagenSubida.Text = String.Empty;
            TxHojaServicio.Value = String.Empty;
            DdEstadoSoliJefe.SelectedIndex = -1;
            DdlMotivosCancelacion.SelectedIndex = -1;
            TxObservacion.Text = String.Empty;

            LbFechaRangoBien.Visible = false;
            LbFechaRangoMal.Visible = false;
            DivAprobacionSubGerente.Visible = false;
            DivUnaFecha.Visible = false;

            TxMotivoNoConductor.Text = String.Empty;

            TxTotalHoras.Text = "00:00 (0.0 Hrs)";
            TxHrDiurnas.Text = "00:00 (0.0 Hrs)";
            TxHrNoc.Text = "00:00 (0.0 Hrs)";
            TxHrNocNoc.Text = "00:00 (0.0 Hrs)";
            TxHrDomFeriado.Text = "00:00 (0.0 Hrs)";

            FuHojaServicio.ID = "";

            UpDivUnaFecha.Update();
            UpdatePrincipalBotones.Update();
            UpdatePanelFechas.Update();
            UpdatePanel2.Update();
            UpdatePanel3.Update();

            UpdatePanel21.Update();
            //UpdatePanel22.Update();
        }
        private string GetExtension(string Extension)        {            switch (Extension)            {                case ".doc":                    return "application/ms-word";                case ".xls":                    return "application/vnd.ms-excel";                case ".ppt":                    return "application/mspowerpoint";                case "jpeg":                    return "image/jpeg";                case ".bmp":                    return "image/bmp";                case ".zip":                    return "application/zip";                case ".log":                    return "text/HTML";                case ".txt":                    return "text/plain";                case ".tiff":                case ".tif":                    return "image/tiff";                case ".asf":                    return "video/x-ms-asf";                case ".avi":                    return "video/avi";                case ".gif":                    return "image/gif";                case ".jpg":                case ".wav":                    return "audio/wav";                case ".pdf":                    return "application/pdf";                case ".fdf":                    return "application/vnd.fdf";                case ".dwg":                    return "image/vnd.dwg";                case ".msg":                    return "application/msoutlook";                case ".xml":                    return "application/xml";                default:                    return "application/octet-stream";            }        }
        protected void BtnEnviarSolicitud_Click(object sender, EventArgs e)        {            try            {                String vNombreDepot1 = String.Empty;                HttpPostedFile bufferDeposito1T = FuHojaServicio.PostedFile;                byte[] vFileDeposito1 = null;                String vExtension = String.Empty;                if (bufferDeposito1T != null)                {                    vNombreDepot1 = FuHojaServicio.FileName;                    Stream vStream = bufferDeposito1T.InputStream;                    BinaryReader vReader = new BinaryReader(vStream);                    vFileDeposito1 = vReader.ReadBytes((int)vStream.Length);                    vExtension = System.IO.Path.GetExtension(FuHojaServicio.FileName);                }                String vArchivo = String.Empty;                if (vFileDeposito1 != null)                    vArchivo = Convert.ToBase64String(vFileDeposito1);                String vFormato = "yyyy-MM-dd HH:mm:ss"; //"dd-MM-yyyy HH:mm:ss"; 
                String vFeINI = Convert.ToDateTime(TxFechaInicio.Text).ToString(vFormato);                String vFeFIN = Convert.ToDateTime(TxFechaRegreso.Text).ToString(vFormato);                DataTable vDatos = new DataTable();                String vQuery = "RSP_TiempoExtraordinarioGenerales 5,'"                    + Session["STECODIGOSAP"]                    + "'," + RbCambioTurno.SelectedValue                     + "," + DDLCambioTurnoColaborador.SelectedValue                    + ",'" + TxMotivoCambioTurno.Text                    + "','" + TxTotalHoras.Text                    + "','" + TxHrDiurnas.Text                    + "','" + TxHrNoc.Text                    + "','" + TxHrNocNoc.Text                    + "','" + TxHrDomFeriado.Text                    + "','" + vFeINI                    + "','" + vFeFIN                    + "','" + Session["STEHRINICIO"]                    + "','" + Session["STEHRFIN"]                    + "'," + Session["STEHRDIURNASOLICITADAS"]                    + "," + Session["STEMINDIURNASOLICITADAS"]                    + "," + Session["STEHRNOCSOLICITADAS"]                    + "," + Session["STEMINNOCSOLICITADAS"]                    + "," + Session["STEHRNOCNOCSOLICITADAS"]                    + "," + Session["STEMINNOCNOCSOLICITADAS"]                    + "," + Session["STEHRDOMINGOFERIADOSOLICITADAS"]                    + "," + Session["STEMINDOMINGOFERIADOSOLICITADAS"]                    + "," + Session["STEHRTOTALSOLICITADAS"]                    + "," + Session["STEMINTOTALSOLICITADAS"]                    + ",'" + TxPeticion.Text                    + "','" + TxTituloSysaid.Text                    + "'," + DdlConductor.SelectedValue                    + "," + DdlConductorNombre.SelectedValue                    + "," + DdlTipoTrabajo.SelectedValue                    + ",'" + DdlTipoDescripcion.SelectedValue                    + "','" + TxDescripcionTrabajo.Text                    + "','" + vArchivo                    + "',1,'" + vExtension                    + "','" + Session["STEIDJEFE"]                    + "','" + Session["STEIDTURNOCAMBIO"]                    + "','" + vNombreDepot1
                    + "','" + DdlMotivoAprobacionSubgerente.SelectedValue                    + "','" + TxSoliAprobacionSubGerente.Text                    + "'," + Session["STEAPROBACIONSUBGERENTE"]
                    + "," + Session["STEIDJEFESUBGERENCIA"]
                    + "," + RbFormaTrabajo.SelectedValue
                    + ",'" + Session["USUARIO"].ToString() + "'";
                //vDatos = vConexion.obtenerDataTable(vQuery);
                Int32 vRespuesta = vConexion.ejecutarSql(vQuery);

                if (vRespuesta == 1)
                {                    DataTable vData = (DataTable)Session["TIEMPO_EX_TECNICO"];                    vQuery = "RSP_ObtenerEmpleados 2," + vData.Rows[0]["idJefe"].ToString();                    DataTable vDatosJefe = vConexion.obtenerDataTable(vQuery);                                        Boolean vFlagEnvio = false;

                    foreach (DataRow item in vDatosJefe.Rows){                        if (!item["emailEmpresa"].ToString().Trim().Equals("")){                            vService.EnviarMensaje(                                item["emailEmpresa"].ToString(), // CORREO DEL JEFE                                typeBody.TiempoExtraordinario,                                item["nombre"].ToString(), // NOMBRE DEL JEFE                                "El empleado " + vData.Rows[0]["nombre"].ToString() + " ha creado una solicitud de Tiempo Extraordinario.",                                "Le informamos que la solicitud debe ser autorizada, para que sea procesada por Recursos Humanos.",                                vData.Rows[0]["emailEmpresa"].ToString() // CORREO DEL SOLICITANTE - COPIADO                                );                            vFlagEnvio = true;                        }                    }

                    if (vFlagEnvio){
                        Response.Redirect("/pages/tiempoExtraordinario/solicitudTE.aspx?ex=3");                    }                }
                else
                {                    Response.Redirect("/pages/tiempoExtraordinario/solicitudTE.aspx?ex=4");                }            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, WarningType.Danger);
            }        }
        void CargarSolicitudes()        {            try            {                string usuario = Session["USUARIO"].ToString();                DataTable vDatos = new DataTable();                vDatos = vConexion.obtenerDataTable("RSP_TiempoExtraordinarioGenerales 11,'" + Convert.ToString(Session["USUARIO"]) + "'"); //2902
                GVBusqueda.DataSource = vDatos;                GVBusqueda.DataBind();                UpdateDivBusquedas.Update();
                Session["STESOLICITUDESCREADAS"] = vDatos;
            }            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e)        {            if (e.CommandName == "HojaServicio")            {                string vIdSolicitud = e.CommandArgument.ToString();                String vQuery = "RSP_TiempoExtraordinarioGenerales 12," + vIdSolicitud;                DataTable vDatos = vConexion.obtenerDataTable(vQuery);                String vDocumento = "";                if (!vDatos.Rows[0]["hojaServicio"].ToString().Equals(""))                    vDocumento = vDatos.Rows[0]["hojaServicio"].ToString();                if (!vDocumento.Equals(""))                {                    LbPermisoDescarga.Text = vIdSolicitud;                    UpdatePanel5.Update();                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openDescargarModal();", true);                }                else                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "window.alert('No existe hoja de servicio para esta solicitud')", true);            }

            if (e.CommandName == "DetalleSolicitud")            {                string vIdSolicitud = e.CommandArgument.ToString();

                String vQuery = "RSP_TiempoExtraordinarioGenerales 25," + vIdSolicitud;                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                string vidEstadoSolicitud = vDatos.Rows[0]["idEstado"].ToString();

                vQuery = "RSP_TiempoExtraordinarioGenerales 39," + vIdSolicitud;
                vDatos = vConexion.obtenerDataTable(vQuery);                if (vidEstadoSolicitud == "5")//Aprobada RRHH
                {

                    string vPermisoReintegradoAlmuero = vDatos.Rows[0]["permisoAmuerzoRrhh"].ToString();
                    string vPermisoReintegradoSalida = vDatos.Rows[0]["permisoSalidaRrhh"].ToString();
                    string vPermisoReintegradoEntrada = vDatos.Rows[0]["permisoEntradaRrhh"].ToString();

                    if (vPermisoReintegradoAlmuero == "")
                    {
                        vPermisoReintegradoAlmuero = "NINGUNO";
                    }
                    else
                    {
                        vPermisoReintegradoAlmuero = vDatos.Rows[0]["permisoAmuerzoRrhh"].ToString();
                    }


                    if (vPermisoReintegradoSalida == "")
                    {
                        vPermisoReintegradoSalida = "NINGUNO";
                    }
                    else
                    {
                        vPermisoReintegradoSalida = vDatos.Rows[0]["permisoSalidaRrhh"].ToString();
                    }

                    if (vPermisoReintegradoEntrada == "")
                    {
                        vPermisoReintegradoEntrada = "NINGUNO";
                    }
                    else
                    {
                        vPermisoReintegradoEntrada = vDatos.Rows[0]["permisoEntradaRrhh"].ToString();
                    }

                    LbMasInformacionColaboraador.Text = "Más Informacion Solicitud - " + vIdSolicitud;
                    LbMensaje1.Text =
                  "Total de horas solicitadas: <b> " + vDatos.Rows[0]["totalHrsSolicitadas"].ToString() + "</b><br />" + "Horas Diurnas: <b>" + vDatos.Rows[0]["totalHrsSolicitadasDiurnas"].ToString() + "</b>, Horas Noc: <b>" + vDatos.Rows[0]["totalHrsSolicitadasNoc"].ToString() + "</b>, Horas NocNoc: <b>" + vDatos.Rows[0]["totalHrsSolicitadasNocNoc"].ToString() + "</b>, Horas Domingos Feriados: <b>" + vDatos.Rows[0]["totalHrsSolicitadasDomingoFeriado"].ToString() + " </b>" + "<br /><br />" + "Total de horas aprobadas: <b> " + vDatos.Rows[0]["totalHrsAprobadas"].ToString() + "</b><br />" + "Horas Diurnas: <b>" + vDatos.Rows[0]["totalHrsAprobadasDiurnas"].ToString() + "</b>, Horas Noc: <b>" + vDatos.Rows[0]["totalHrsAprobadasNoc"].ToString() + "</b>, Horas NocNoc: <b>" + vDatos.Rows[0]["totalHrsAprobadasNocNoc"].ToString() + "</b>, Horas Domingos Feriados: <b>" + vDatos.Rows[0]["totalHrsAprobadasDomingoFeriado"].ToString() + "</b><br /><br />" +

                  "Hr Aprobo RRHH: <b>" + vDatos.Rows[0]["horaRrhhAprobo"].ToString() + "</b><br /><br />" +
                  "<hr> " +
                  "<center> <b>Pemisos Reintegrados: </b> </center><br />" +
                  "<b>Entrada Tarde: </b><br />" +
                  vPermisoReintegradoEntrada + "</b><br /><br />" +
                   "<b>Salida Temprana: </b><br />" +
                  vPermisoReintegradoSalida + "</b><br /><br />" +
                   "<b>Almuerzo: </b><br />" +
                  vPermisoReintegradoAlmuero + "</b><br /><br />";

                    UpdatePanel16.Update();
                    UpdatePanel15.Update();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "OpenMasInformacionColaborador();", true);
                }
                else if (vidEstadoSolicitud == "6")//Cancelada RRHH
                {
                    LbMasInformacionColaboraador.Text = "Más Informacion Solicitud - " + vIdSolicitud;
                    LbMensaje1.Text =
                  "Total de horas solicitadas: <b> " + vDatos.Rows[0]["totalHrsSolicitadas"].ToString() + "</b><br />" + "Horas Diurnas: <b>" + vDatos.Rows[0]["totalHrsSolicitadasDiurnas"].ToString() + "</b>, Horas Noc: <b>" + vDatos.Rows[0]["totalHrsSolicitadasNoc"].ToString() + "</b>, Horas NocNoc: <b>" + vDatos.Rows[0]["totalHrsSolicitadasNocNoc"].ToString() + "</b>, Horas Domingos Feriados: <b>" + vDatos.Rows[0]["totalHrsSolicitadasDomingoFeriado"].ToString() + " </b>" + "<br /><br />" + "Total de horas aprobadas: <b> " + vDatos.Rows[0]["totalHrsAprobadas"].ToString() + "</b><br />" + "Horas Diurnas: <b>" + vDatos.Rows[0]["totalHrsAprobadasDiurnas"].ToString() + "</b>, Horas Noc: <b>" + vDatos.Rows[0]["totalHrsAprobadasNoc"].ToString() + "</b>, Horas NocNoc: <b>" + vDatos.Rows[0]["totalHrsAprobadasNocNoc"].ToString() + "</b>, Horas Domingos Feriados: <b>" + vDatos.Rows[0]["totalHrsAprobadasDomingoFeriado"].ToString() + "</b><br /><br />" +

                  "Hr cancelo RRHH: <b>" + vDatos.Rows[0]["horaRrhhAprobo"].ToString() + "</b><br /><br />" +
                  "Observacion cancelacion: <b>" + vDatos.Rows[0]["observacionRrhh"].ToString() + "</b><br /><br />";

                    UpdatePanel16.Update();
                    UpdatePanel15.Update();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "OpenMasInformacionColaborador();", true);
                }
                else if (vidEstadoSolicitud == "7")//Cancelada Jefe
                {
                    LbMasInformacionColaboraador.Text = "Más Informacion Solicitud - " + vIdSolicitud;
                    LbMensaje1.Text =
                  "Total de horas solicitadas: <b> " + vDatos.Rows[0]["totalHrsSolicitadas"].ToString() + "</b><br />" + "Horas Diurnas: <b>" + vDatos.Rows[0]["totalHrsSolicitadasDiurnas"].ToString() + "</b>, Horas Noc: <b>" + vDatos.Rows[0]["totalHrsSolicitadasNoc"].ToString() + "</b>, Horas NocNoc: <b>" + vDatos.Rows[0]["totalHrsSolicitadasNocNoc"].ToString() + "</b>, Horas Domingos Feriados: <b>" + vDatos.Rows[0]["totalHrsSolicitadasDomingoFeriado"].ToString() + " </b>" + "<br /><br />" +

                  "Hr cancelo: <b>" + vDatos.Rows[0]["horaJefeAprobo"].ToString() + "</b><br /><br />" +
                  "Observacion cancelacion: <b>" + vDatos.Rows[0]["observacionAprobacionJefe"].ToString() + "</b><br /><br />";

                    UpdatePanel16.Update();
                    UpdatePanel15.Update();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "OpenMasInformacionColaborador();", true);
                }
                else if (vidEstadoSolicitud == "4")//Pendiente Modificar Colaborador
                {
                    LbMasInformacionColaboraador.Text = "Más Informacion Solicitud - " + vIdSolicitud;
                    LbMensaje1.Text =
                  "Total de horas solicitadas: <b> " + vDatos.Rows[0]["totalHrsSolicitadas"].ToString() + "</b><br />" + "Horas Diurnas: <b>" + vDatos.Rows[0]["totalHrsSolicitadasDiurnas"].ToString() + "</b>, Horas Noc: <b>" + vDatos.Rows[0]["totalHrsSolicitadasNoc"].ToString() + "</b>, Horas NocNoc: <b>" + vDatos.Rows[0]["totalHrsSolicitadasNocNoc"].ToString() + "</b>, Horas Domingos Feriados: <b>" + vDatos.Rows[0]["totalHrsSolicitadasDomingoFeriado"].ToString() + " </b>" + "<br /><br />" +

                  "Hr aprobo modificacion: <b>" + vDatos.Rows[0]["horaJefeAprobo"].ToString() + "</b><br /><br />" +
                  "Cambio a realizar: <b>" + vDatos.Rows[0]["observacionAprobacionJefe"].ToString() + "</b><br /><br />";

                    UpdatePanel16.Update();
                    UpdatePanel15.Update();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "OpenMasInformacionColaborador();", true);
                }
                else if (vidEstadoSolicitud == "1" || vidEstadoSolicitud == "2" || vidEstadoSolicitud == "3")//Pendiente Aprobar Jefe, Pendiente Aprobar Subgerente,Pendiente Aprobar RRHH
                {

                    LbMasInformacionColaboraador.Text = "Más Informacion Solicitud - " + vIdSolicitud;
                    LbMensaje1.Text =
                  "Total de horas solicitadas: <b> " + vDatos.Rows[0]["totalHrsSolicitadas"].ToString() + "</b><br />" + "Horas Diurnas: <b>" + vDatos.Rows[0]["totalHrsSolicitadasDiurnas"].ToString() + "</b>, Horas Noc: <b>" + vDatos.Rows[0]["totalHrsSolicitadasNoc"].ToString() + "</b>, Horas NocNoc: <b>" + vDatos.Rows[0]["totalHrsSolicitadasNocNoc"].ToString() + "</b>, Horas Domingos Feriados: <b>" + vDatos.Rows[0]["totalHrsSolicitadasDomingoFeriado"].ToString() + " </b>" + "<br /><br />" +
                  "Descripcion Trabajo: <b>" + vDatos.Rows[0]["detalleTrabajo"].ToString() + "</b><br /><br />" +
                  "Hr envio colaborador: <b>" + vDatos.Rows[0]["fechaSolicitud"].ToString() + "</b><br /><br />";

                    UpdatePanel16.Update();
                    UpdatePanel15.Update();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "OpenMasInformacionColaborador();", true);
                }
                else if (vidEstadoSolicitud == "8")//Cancelada Subgerente
                {
                    LbMasInformacionColaboraador.Text = "Más Informacion Solicitud - " + vIdSolicitud;
                    LbMensaje1.Text =
                  "Total de horas solicitadas: <b> " + vDatos.Rows[0]["totalHrsSolicitadas"].ToString() + "</b><br />" + "Horas Diurnas: <b>" + vDatos.Rows[0]["totalHrsSolicitadasDiurnas"].ToString() + "</b>, Horas Noc: <b>" + vDatos.Rows[0]["totalHrsSolicitadasNoc"].ToString() + "</b>, Horas NocNoc: <b>" + vDatos.Rows[0]["totalHrsSolicitadasNocNoc"].ToString() + "</b>, Horas Domingos Feriados: <b>" + vDatos.Rows[0]["totalHrsSolicitadasDomingoFeriado"].ToString() + " </b>" + "<br /><br />" +

                  "Hr cancelo: <b>" + vDatos.Rows[0]["horaAproboSubgerente"].ToString() + "</b><br /><br />" +
                  "Observacion cancelacion: <b>" + vDatos.Rows[0]["motivoCanceloSubgerente"].ToString() + "</b><br /><br />";

                    UpdatePanel16.Update();
                    UpdatePanel15.Update();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "OpenMasInformacionColaborador();", true);
                }

            }        }
        protected void BtnDescargarArchivo_Click(object sender, EventArgs e)        {            try            {                String vEx = Request.QueryString["ex"];                if (string.IsNullOrEmpty(vEx)){                    string vIdSolicitud = LbPermisoDescarga.Text;                    String vQuery = "RSP_TiempoExtraordinarioGenerales 12," + vIdSolicitud;                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);                    String vDocumento = "";                    if (!vDatos.Rows[0]["hojaServicio"].ToString().Equals(""))                        vDocumento = vDatos.Rows[0]["hojaServicio"].ToString();                    if (!vDocumento.Equals("")){                        String vDocumentoArchivo = "HojaServicio-" + vIdSolicitud + vDatos.Rows[0]["extension"].ToString();                        byte[] fileData = Convert.FromBase64String(vDocumento);                        Response.Cache.SetCacheability(HttpCacheability.NoCache);                        GetExtension(vDatos.Rows[0]["extension"].ToString().ToLower());                        byte[] bytFile = fileData;                        Response.OutputStream.Write(bytFile, 0, bytFile.Length);                        Response.AddHeader("Content-disposition", "attachment;filename=" + vDocumentoArchivo);                        Response.End();                    }                }else if (vEx.Equals("1")){                    DataTable vDatosSolicitud = new DataTable();                    vDatosSolicitud = (DataTable)Session["STEDATOSSOLICITUDINDIVIDUAL"];                    String vDocumentoArchivo = "HojaServicio-" + vDatosSolicitud.Rows[0]["idSolicitud"].ToString() + vDatosSolicitud.Rows[0]["extension"].ToString();                    string vDocumentoAprobar = vDatosSolicitud.Rows[0]["hojaServicio"].ToString();                    byte[] fileData = Convert.FromBase64String(vDocumentoAprobar);                    Response.Cache.SetCacheability(HttpCacheability.NoCache);                    GetExtension(vDatosSolicitud.Rows[0]["extension"].ToString().ToLower());                    byte[] bytFile = fileData;                    Response.OutputStream.Write(bytFile, 0, bytFile.Length);                    Response.AddHeader("Content-disposition", "attachment;filename=" + vDocumentoArchivo);                    Response.End();                }else if (vEx.Equals("2")){                    DataTable vDatosSolicitud = new DataTable();                    vDatosSolicitud = (DataTable)Session["STEDATOSSOLICITUDINDIVIDUAL"];                    String vDocumentoArchivo = "HojaServicio-" + vDatosSolicitud.Rows[0]["idSolicitud"].ToString() + vDatosSolicitud.Rows[0]["extension"].ToString();                    string vDocumentoAprobar = vDatosSolicitud.Rows[0]["hojaServicio"].ToString();                    byte[] fileData = Convert.FromBase64String(vDocumentoAprobar);                    Response.Cache.SetCacheability(HttpCacheability.NoCache);                    GetExtension(vDatosSolicitud.Rows[0]["extension"].ToString().ToLower());                    byte[] bytFile = fileData;                    Response.OutputStream.Write(bytFile, 0, bytFile.Length);                    Response.AddHeader("Content-disposition", "attachment;filename=" + vDocumentoArchivo);                    Response.End();                }            }            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }            finally { CerrarModal("DescargaModal"); }        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        void cargarDataVista()        {            try            {                DataTable vDatosGenerales = new DataTable();                vDatosGenerales = (DataTable)Session["STEDATOSGENERALESCOLABORADOR"];                TxEmpleado.Text = vDatosGenerales.Rows[0]["idEmpleado"].ToString() + " - " + vDatosGenerales.Rows[0]["nombre"].ToString();                TxJefe.Text = vDatosGenerales.Rows[0]["idJefe"].ToString() + " - " + vDatosGenerales.Rows[0]["jefeNombre"].ToString();                TxSubgerencia.Text = vDatosGenerales.Rows[0]["area"].ToString();                TxTurno.Text = vDatosGenerales.Rows[0]["nombreTurno"].ToString();

                Session["STENOMBRESUBGERENTE"] = vDatosGenerales.Rows[0]["jefeNombreSubgerente"].ToString();
                Session["STENOMBRECOLABORADOR"] = vDatosGenerales.Rows[0]["nombre"].ToString();                Session["STEIDJEFE"] = vDatosGenerales.Rows[0]["idJefe"].ToString();                Session["STECIUDAD"] = vDatosGenerales.Rows[0]["ciudad"].ToString();                Session["STECODIGOSAP"] = vDatosGenerales.Rows[0]["idEmpleado"].ToString();                Session["STECODIGOSAPBIOMETRICO"] = vDatosGenerales.Rows[0]["codigoSAP"].ToString();                Session["STENOMBREJEFE"] = vDatosGenerales.Rows[0]["jefeNombre"].ToString();                Session["STEPUESTOJEFE"] = vDatosGenerales.Rows[0]["puestoJefe"].ToString();                Session["STEPUESTOCOLABORADOR"] = vDatosGenerales.Rows[0]["idPuesto"].ToString();                Session["STESUBGERENCIA"] = vDatosGenerales.Rows[0]["subGerencia"].ToString();                Session["STEHORAENTRADA"] = vDatosGenerales.Rows[0]["horaInicio"].ToString();                Session["STEHORASALIDA"] = vDatosGenerales.Rows[0]["horaFinal"].ToString();                Session["STEDIASTURNO"] = vDatosGenerales.Rows[0]["dias"].ToString();

                DataTable vDatosSolicitud = new DataTable();                vDatosSolicitud = (DataTable)Session["STEDATOSSOLICITUDINDIVIDUAL"];                DdlMotivoAprobacionSubgerente.SelectedValue = vDatosSolicitud.Rows[0]["motivoAprobacionSubgerente"].ToString();                TxSoliAprobacionSubGerente.Text = vDatosSolicitud.Rows[0]["detalleMotivoAprobacion"].ToString();                RbFormaTrabajo.SelectedValue = vDatosSolicitud.Rows[0]["formaTrabajo"].ToString();                Session["STEAPROBACIONSUBGERENTE"] = vDatosSolicitud.Rows[0]["aprobacionSubgerente"].ToString();                Session["STENUMSOLICITUD"] = vDatosSolicitud.Rows[0]["idSolicitud"].ToString();                string vFormato = "yyyy-MM-ddTHH:mm";                RbCambioTurno.SelectedValue = vDatosSolicitud.Rows[0]["cambioTurno"].ToString();                string vFechaInicio = vDatosSolicitud.Rows[0]["fechaInicio"].ToString();                string vFechaFin = vDatosSolicitud.Rows[0]["fechaFin"].ToString();                string vFechaInicioConvertida = Convert.ToDateTime(vFechaInicio).ToString(vFormato);                string vFechaFinConvertida = Convert.ToDateTime(vFechaFin).ToString(vFormato);                TxComentarioJefe.Text = vDatosSolicitud.Rows[0]["observacionAprobacionJefe"].ToString();                TxFechaInicio.Text = vFechaInicioConvertida;                TxFechaRegreso.Text = vFechaFinConvertida;

                //DIV CUANDO LA SOLICITUD ESTA FUERA DE RANGO
                if (vDatosSolicitud.Rows[0]["aprobacionSubgerente"].ToString() == "1")
                {
                    LbFechaRangoMal.Visible = true;
                    DivAprobacionSubGerente.Visible = true;
                }
                else
                {
                    LbFechaRangoBien.Visible = true;
                    DivAprobacionSubGerente.Visible = false;
                }

                DdlConductor.SelectedValue = vDatosSolicitud.Rows[0]["conductor"].ToString();
                //DECISION PARA OCULTAR O MOSTRAR LAS OPCIONES DE CONDUCTORES
                if (Session["STEPUESTOJEFE"].ToString().Equals("20000383") || Session["STEPUESTOJEFE"].ToString().Equals("20000395") || Session["STEPUESTOCOLABORADOR"].ToString().Equals("20000409"))                {                    DivConductor.Visible = false;                    DdlConductor.SelectedValue = "0";                }                else                {                    DivConductor.Visible = true;                }                if (DdlConductor.SelectedValue.Equals("1"))                {                    DdlConductorNombre.SelectedValue = vDatosSolicitud.Rows[0]["idCondutor"].ToString(); ;                    DdlConductorNombre.Visible = true;                    TxMotivoNoConductor.Visible = false;                }                else                {                    DdlConductorNombre.Visible = false;                    TxMotivoNoConductor.Visible = true;                    TxMotivoNoConductor.Text = "No se requirio conductor para esta solicitud.";                    DdlConductor.SelectedValue = "0";                }



                //LLENA EL COMBOBOX DE EMPLEADOS
                DDLCambioTurnoColaborador.Items.Clear();                string vQuery = "RSP_TiempoExtraordinarioGenerales 2,'" + Session["STEIDJEFE"] + "'";                DataTable vDatosEmpleados = vConexion.obtenerDataTable(vQuery);                DDLCambioTurnoColaborador.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });                foreach (DataRow item in vDatosEmpleados.Rows)                {                    DDLCambioTurnoColaborador.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["idEmpleado"].ToString() + " - " + item["nombre"].ToString() });                }

                //VALIDACION DE SI REALIZO CAMBIO DE TURNO SI O NO
                if (RbCambioTurno.SelectedValue.Equals("1"))                {                    rowSolicitudCambioTurno.Visible = true;                    DDLCambioTurnoColaborador.SelectedValue = vDatosSolicitud.Rows[0]["codigoSapCambioTurno"].ToString();
                    TxMotivoCambioTurno.Text = vDatosSolicitud.Rows[0]["motivoCambioTurno"].ToString();                    int idTurnoCambio = Convert.ToInt32(vDatosSolicitud.Rows[0]["idTurnoCambio"].ToString());

                    vQuery = "RSP_TiempoExtraordinarioGenerales 20,'" + idTurnoCambio + "'";                    DataTable vDatosCambioTurno = vConexion.obtenerDataTable(vQuery);                    TxCambioTurno.Text = vDatosCambioTurno.Rows[0]["nombreTurno"].ToString();                    Session["STEHORAENTRADA"] = vDatosCambioTurno.Rows[0]["horaInicio"].ToString();                    Session["STEHORASALIDA"] = vDatosCambioTurno.Rows[0]["horaFinal"].ToString();                    Session["STEDIASTURNO"] = vDatosCambioTurno.Rows[0]["dias"].ToString();                }                else                {                    rowSolicitudCambioTurno.Visible = false;                }                DdlTipoTrabajo.Items.Clear();                vQuery = "RSP_TiempoExtraordinarioGenerales 3";                DataTable vDatosTipoTrabajo = vConexion.obtenerDataTable(vQuery);                DdlTipoTrabajo.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });                foreach (DataRow item in vDatosTipoTrabajo.Rows)                {                    DdlTipoTrabajo.Items.Add(new ListItem { Value = item["idTipoTrabajo"].ToString(), Text = item["nombreTrabajo"].ToString() });                }                DdlTipoTrabajo.SelectedValue = vDatosSolicitud.Rows[0]["idTipoTrabajo"].ToString();                TxDescripcionTrabajo.Text = vDatosSolicitud.Rows[0]["detalleTrabajo"].ToString();                DdlTipoDescripcion.Items.Clear();                vQuery = "RSP_TiempoExtraordinarioGenerales 4," + DdlTipoTrabajo.SelectedValue;                DataTable vDatos = vConexion.obtenerDataTable(vQuery);                if (vDatos.Rows.Count > 0)                {                    DivCategoria.Visible = true;                    TituloCategoria.Text = DdlTipoTrabajo.SelectedItem.Text + ":";                    DdlTipoDescripcion.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });                    foreach (DataRow item in vDatos.Rows)                    {                        DdlTipoDescripcion.Items.Add(new ListItem { Value = item["idTipoTrabajoDescripcion"].ToString(), Text = item["nombreTrabajo"].ToString() });                    }                    DdlTipoDescripcion.SelectedValue = vDatosSolicitud.Rows[0]["idTipoTrabajoDescripcion"].ToString();                }                else                {                    DivCategoria.Visible = false;                }


                //DECISION PARA OCULTAR O MOSTRAR EL CAMPO DE SYSAID Y HOJA DE SERVICIO
                if (Session["STESUBGERENCIA"].ToString().Equals("4") || Session["STESUBGERENCIA"].ToString().Equals("2"))                {                    DivSysAid.Visible = true;                    lbHojaServicio.Visible = true;                    FuHojaServicio.Visible = true;                    btnVisualizarHoja.Visible = true;                    btnDescargarHoja.Visible = true;                    UpdatePanel12.Update();
                    //DivHojaServicio.Visible = true;


                }                else                {                    DivSysAid.Visible = false;                    lbHojaServicio.Visible = false;                    FuHojaServicio.Visible = false;                    btnVisualizarHoja.Visible = false;                    btnDescargarHoja.Visible = false;                    UpdatePanel12.Update();
                    //DivHojaServicio.Visible = false;
                }


                //DECISION PARA OCULTAR O MOSTRAR EL CAMPO DE SYSAID Y HOJA DE SERVICIO
                if (DdlTipoTrabajo.SelectedValue.Equals("7") && (Session["STESUBGERENCIA"].ToString() != "4" || Session["STESUBGERENCIA"].ToString() != "2"))                {                    DivSysAid.Visible = true;
                    //DivHojaServicio.Visible = true;
                    FuHojaServicio.Visible = false;                    TxImagenSubida.Visible = true;                    UpdatePanel3.Update();                    lbHojaServicio.Visible = true;                    btnVisualizarHoja.Visible = true;                    btnDescargarHoja.Visible = true;                    UpdatePanel12.Update();                }                else if (Session["STESUBGERENCIA"].ToString() == "4" || Session["STESUBGERENCIA"].ToString() == "2")                {                    DivSysAid.Visible = true;
                    //DivHojaServicio.Visible = true;

                    FuHojaServicio.Visible = false;                    TxImagenSubida.Visible = true;                    UpdatePanel3.Update();                    lbHojaServicio.Visible = true;                    btnVisualizarHoja.Visible = true;                    btnDescargarHoja.Visible = true;                    UpdatePanel12.Update();                }                else                {                    DivSysAid.Visible = false;
                    //DivHojaServicio.Visible = false;
                    FuHojaServicio.Visible = false;                    lbHojaServicio.Visible = false;                    btnVisualizarHoja.Visible = false;                    btnDescargarHoja.Visible = false;                    UpdatePanel12.Update();                }                TxPeticion.Text = vDatosSolicitud.Rows[0]["sysAid"].ToString();                TxTituloSysaid.Text = vDatosSolicitud.Rows[0]["categoriasSysaid"].ToString();                TxImagenSubida.Text = vDatosSolicitud.Rows[0]["nombreHojaServicio"].ToString();


                String vHojaServicio = vDatosSolicitud.Rows[0]["hojaServicio"].ToString();                string srcHojaServicio = "data:image;base64," + vHojaServicio;                Img1.Src = srcHojaServicio;


                DdlMotivosCancelacionRRHH.Items.Clear();                vQuery = "RSP_TiempoExtraordinarioGenerales 26";                vDatos = vConexion.obtenerDataTable(vQuery);                if (vDatos.Rows.Count > 0)                {                    DdlMotivosCancelacionRRHH.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });                    foreach (DataRow item in vDatos.Rows)                    {                        DdlMotivosCancelacionRRHH.Items.Add(new ListItem { Value = item["idEstadoRRHH"].ToString(), Text = item["motivo"].ToString() });                    }                }

                String vEx = Request.QueryString["ex"];
                if (!string.IsNullOrEmpty(vEx)){
                    if (vEx.Equals("7")){
                        lbHojaServicio.Visible = true;
                        FuHojaServicio.Visible = false;
                        TxImagenSubida.Visible = true;
                        btnVisualizarHoja.Visible = true;
                        btnDescargarHoja.Visible = true;
                        LbCambiarHoja.Visible = true;
                        RbCambioHoja.Visible = true;
                        RbCambioHoja.SelectedValue = "2";
                    }

                    if (vEx.Equals("1")){
                        if (vDatosSolicitud.Rows[0]["flagModiSoli"].ToString() == "1")
                        {
                            LbModificarSolicitud.Visible = true;
                            LbMensajeModificar.Visible = true;
                            LbMensajeModificar.Text = vDatosSolicitud.Rows[0]["observacionAprobacionJefe"].ToString();
                        }
                    }
                }
            }catch (Exception ex){                Mensaje(ex.Message, WarningType.Danger);            }        }
        void camposDeshabilitados()        {
            RbFormaTrabajo.Enabled = false;            RbCambioTurno.Enabled = false;            DDLCambioTurnoColaborador.Enabled = false;            DDLCambioTurnoColaborador.CssClass = "form-control";            TxMotivoCambioTurno.ReadOnly = true;            TxFechaInicio.ReadOnly = true;            TxFechaRegreso.ReadOnly = true;            DdlConductor.Enabled = false;            DdlConductor.CssClass = "form-control";            DdlConductorNombre.Enabled = false;            DdlConductorNombre.CssClass = "form-control";            DdlTipoTrabajo.Enabled = false;            DdlTipoTrabajo.CssClass = "form-control";            DdlTipoDescripcion.Enabled = false;            DdlTipoDescripcion.CssClass = "form-control";            TxDescripcionTrabajo.ReadOnly = true;            TxPeticion.ReadOnly = true;            TxSoliAprobacionSubGerente.ReadOnly = true;            DdlMotivoAprobacionSubgerente.Enabled = false;            DdlMotivoAprobacionSubgerente.CssClass = "form-control";        }
       protected void btnVisualizarHoja_Click(object sender, EventArgs e)        {            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openModalImagen();", true);        }
        protected void DdEstadoSoliJefe_SelectedIndexChanged(object sender, EventArgs e)        {            limpiarAprobacionJefe();            if (DdEstadoSoliJefe.SelectedValue == "1" && Session["STEAPROBACIONSUBGERENTE"].Equals("1"))            {                LbAlerta.Visible = true;                TxObservacion.Visible = true;                DivMotivo.Visible = false;                Session["STELBESTADOJEFE"] = "aprobar";                Session["STEESTADOBDJEFE"] = "1";                Session["STEESTADOBDSOLICITUD"] = "2"; //Pendiente Aprobar Subgerente
                TituloAprobacionJefe.Text = "Aprobar solicitud número " + Session["STENUMSOLICITUD"].ToString();                Session["STEESTADOBDMODIFICARSOLICITUD"] = "0";
            }            else if (DdEstadoSoliJefe.SelectedValue == "2")            {                LbAlerta.Visible = false;                TxObservacion.Visible = true;                DivMotivo.Visible = true;                Session["STELBESTADOJEFE"] = "cancelar";                Session["STEESTADOBDJEFE"] = "2";                TituloAprobacionJefe.Text = "Cancelar solicitud número " + Session["STENUMSOLICITUD"].ToString();                Session["STEESTADOBDSOLICITUD"] = "7";//Cancelada Jefe
                Session["STEESTADOBDMODIFICARSOLICITUD"] = "0";
            }            else if (DdEstadoSoliJefe.SelectedValue == "3")            {                LbAlerta.Visible = false;                TxObservacion.Visible = true;                DivMotivo.Visible = false;                Session["STELBESTADOJEFE"] = "devolver";                Session["STEESTADOBDJEFE"] = "3";                TituloAprobacionJefe.Text = "Devolver solicitud número " + Session["STENUMSOLICITUD"].ToString();                Session["STEESTADOBDSOLICITUD"] = "4";//Pendiente Modificar Colaborador
                Session["STEESTADOBDMODIFICARSOLICITUD"] = "1";
            }            else if (DdEstadoSoliJefe.SelectedValue == "1" && Session["STEAPROBACIONSUBGERENTE"].Equals("0"))            {                LbAlerta.Visible = false;                TxObservacion.Visible = false;                DivMotivo.Visible = false;                Session["STELBESTADOJEFE"] = "aprobar";                Session["STEESTADOBDJEFE"] = "1";                Session["STEESTADOBDSOLICITUD"] = "3";//Pendiente Aprobar RRHH
                TituloAprobacionJefe.Text = "Aprobar solicitud número " + Session["STENUMSOLICITUD"].ToString();                Session["STEESTADOBDMODIFICARSOLICITUD"] = "0";            }        }
        void limpiarAprobacionJefe()        {            DdlMotivosCancelacion.SelectedIndex = -1;            TxObservacion.Text = string.Empty;        }
        void validacionAprobacionJefe()        {            if (DdEstadoSoliJefe.SelectedValue.Equals("0"))                throw new Exception("Falta que seleccione acción si esta seguro de autorizar la solicitud.");            if (DdEstadoSoliJefe.SelectedValue == "1" && Session["STEAPROBACIONSUBGERENTE"].Equals("1") && TxObservacion.Text.Equals(""))                throw new Exception("Falta que ingrese observación para retroalimentar al subgerente y proceda con la aprobación de la solicitud.");            if (DdEstadoSoliJefe.SelectedValue == "2" && DdlMotivosCancelacion.SelectedValue.Equals("0"))                throw new Exception("Falta que seleccione motivo de cancelación");            if (DdEstadoSoliJefe.SelectedValue == "2" && TxObservacion.Text.Equals(""))                throw new Exception("Falta que ingrese observación para retroalimentar al colobador de la decisión cancelación de la solicitud.");            if (DdEstadoSoliJefe.SelectedValue == "3" && TxObservacion.Text.Equals(""))                throw new Exception("Falta que ingrese observación para retroalimentar al colaborador de los campos que tiene que modificar.");        }
        protected void BtnEnviarAprobacionJefe_Click(object sender, EventArgs e)        {            try            {                validacionAprobacionJefe();                String vFI = TxFechaInicio.Text != "" ? TxFechaInicio.Text : "1999-01-01 00:00:00";                String vFF = TxFechaRegreso.Text != "" ? TxFechaRegreso.Text : "1999-01-01 00:00:00";                DateTime desde = Convert.ToDateTime(vFI);                DateTime hasta = Convert.ToDateTime(vFF);

                LbAprobarJefe.Text = "Buen dia <b> " + Session["STENOMBREJEFE"] + "</b><br /><br />" +                  "Fechas solicitadas del <b>" + desde.ToString("yyyy-MM-dd HH:mm:ss") + "</b> al <b>" + hasta.ToString("yyyy-MM-dd HH:mm:ss") + "</b>" + " .Total de horas <b> " + TxTotalHoras.Text + "</b> <br /><br />" +                  "Trabajo realizado: <b>" + TxDescripcionTrabajo.Text + "</b><br /><br />";                LbAprobarJefePregunta.Text = "<b>¿Está seguro que desea " + Session["STELBESTADOJEFE"].ToString() + " la solicitud de:</ b > " + Session["STENOMBRECOLABORADOR"] + "<b> ,en el rango de fechas y horas detalladas?</b>";                UpdateAprobarJefe.Update();                UpTituloAprobarJefeModal.Update();                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openAprobarJefeModal();", true);            }            catch (Exception ex)            {                Mensaje(ex.Message, WarningType.Danger);            }        }
        protected void BtnAprobarJefeModal_Click(object sender, EventArgs e)        {            try            {                String vQuery = "RSP_TiempoExtraordinarioGenerales 21,'"                   + Session["STENUMSOLICITUD"]                   + "','" + Session["USUARIO"]                   + "','" + Session["STEESTADOBDJEFE"]                   + "','" + TxObservacion.Text                   + "'," + DdlMotivosCancelacion.SelectedValue                   + "," + Session["STEESTADOBDSOLICITUD"]                   + "," + Session["STEESTADOBDMODIFICARSOLICITUD"];                Int32 vRespuesta = vConexion.ejecutarSql(vQuery);                String vRe = "";

                if (vRespuesta == 1){
                    //Cancelada Jefe
                    if (Session["STEESTADOBDSOLICITUD"].ToString() == "7")
                    {
                        Boolean vFlagEnvio = false;
                        

                        DataTable vData = (DataTable)Session["STEDATOSGENERALESCOLABORADOR"];
                        vQuery = "RSP_ObtenerEmpleados 2," + vData.Rows[0]["idJefe"].ToString();
                        DataTable vDatosJefe = vConexion.obtenerDataTable(vQuery);

                        foreach (DataRow item in vDatosJefe.Rows){
                            if (!item["emailEmpresa"].ToString().Trim().Equals("")){
                                vService.EnviarMensaje(
                                    vData.Rows[0]["emailEmpresa"].ToString(), // CORREO DEL TECNICO
                                    typeBody.TiempoExtraordinario,
                                    vData.Rows[0]["nombre"].ToString(), // NOMBRE DEL TECNICO
                                     "Se notifica que el jefe/suplente ha cancelado la solicitud de Tiempo Extraordinario, por el siguiente motivo:( " + DdlMotivosCancelacion.SelectedItem + ") " + TxObservacion.Text,
                                     "Si tiene alguna duda con respecto a la decision abocarse con su jefe inmediato.",
                                    item["emailEmpresa"].ToString() // CORREO DEL JEFE - COPIADO
                                );
                                vFlagEnvio = true;
                            }
                        }
                        if (vFlagEnvio){
                            Response.Redirect("/pages/tiempoExtraordinario/PendientesAprobarJefe.aspx?ex=5");
                        }
                    }else if (Session["STEESTADOBDSOLICITUD"].ToString() == "3") //Pendiente Aprobar RRHH
                        {
                        Boolean vFlagEnvio = false;
                        DataTable vData = (DataTable)Session["STEDATOSGENERALESCOLABORADOR"];
                        vQuery = "RSP_ObtenerEmpleados 2," + vData.Rows[0]["idJefe"].ToString();
                        DataTable vDatosJefe = vConexion.obtenerDataTable(vQuery);

                        foreach (DataRow item in vDatosJefe.Rows){
                            if (!item["emailEmpresa"].ToString().Trim().Equals("")){
                                vService.EnviarMensaje(
                                    ConfigurationManager.AppSettings["RHMail"].ToString(), // CORREO RRHH
                                    typeBody.TiempoExtraordinario,
                                    "Señores de RRHH", // NOMBRE DEL PERSONAL DE RRHH
                                    "Se notifica que ya puede proceder con la aprobación de la solicitud de tiempo extraordinario del colaborador: " + vData.Rows[0]["nombre"].ToString(),
                                    "",
                                    vData.Rows[0]["emailEmpresa"].ToString() // CORREO DEL TECNICO - COPIADO
                                );
                                vFlagEnvio = true;
                            }
                        }

                        if (vFlagEnvio){
                            Response.Redirect("/pages/tiempoExtraordinario/PendientesAprobarJefe.aspx?ex=6");
                        }
                    }
                    else if (Session["STEESTADOBDSOLICITUD"].ToString() == "4") //Pendiente Modificar Colaborador
                    {
                        Boolean vFlagEnvio = false;
                        DataTable vData = (DataTable)Session["STEDATOSGENERALESCOLABORADOR"];
                        vQuery = "RSP_ObtenerEmpleados 2," + vData.Rows[0]["idJefe"].ToString();
                        DataTable vDatosJefe = vConexion.obtenerDataTable(vQuery);

                        foreach (DataRow item in vDatosJefe.Rows){
                            if (!item["emailEmpresa"].ToString().Trim().Equals("")){
                                vService.EnviarMensaje(
                                    vData.Rows[0]["emailEmpresa"].ToString(), // CORREO DEL TECNICO
                                    typeBody.TiempoExtraordinario,
                                    vData.Rows[0]["nombre"].ToString(), // NOMBRE DEL TECNICO
                                    "Se notifica que el jefe/suplente ha devuelto la solicitud de tiempo extraordinario por el siguiente motivo: " + TxObservacion.Text,
                                    "Favor realizar las respectivas modificaciones los mas pronto posible.",
                                    item["emailEmpresa"].ToString() // CORREO DEL JEFE - COPIADO
                                );
                                vFlagEnvio = true;
                            }
                        }

                        if (vFlagEnvio){
                            Response.Redirect("/pages/tiempoExtraordinario/PendientesAprobarJefe.aspx?ex=7");
                        }

                    } //Cancelada Jefe
                    else if (Session["STEESTADOBDSOLICITUD"].ToString() == "2") //Pendiente Aprobar Subgerente
                    {
                        DataTable vData = (DataTable)Session["STEDATOSGENERALESCOLABORADOR"];
                        vQuery = "RSP_ObtenerEmpleados 2," + vData.Rows[0]["subgerente"].ToString();
                        DataTable vDatosSubgerente = vConexion.obtenerDataTable(vQuery);

                        Boolean vFlagEnvio = false;
                        foreach (DataRow item in vDatosSubgerente.Rows){
                            if (!item["emailEmpresa"].ToString().Trim().Equals("")){
                                vService.EnviarMensaje(
                                    item["emailEmpresa"].ToString(), // CORREO DEL SUBGERNETE
                                    typeBody.TiempoExtraordinario,
                                    item["nombre"].ToString(), // NOMBRE DEL SUBGERNETE
                                    "El empleado " + vData.Rows[0]["nombre"].ToString() + " ha creado una solicitud de Tiempo Extraordinario fuera del rango establecido por el siguiente motivo: (" + DdlMotivoAprobacionSubgerente.SelectedItem + ") " + TxSoliAprobacionSubGerente.Text,
                                    "Le informamos que la solicitud debe ser autorizada, para que sea procesada por Recursos Humanos.",
                                    vData.Rows[0]["emailEmpresa"].ToString() // CORREO DEL SOLICITANTE - COPIADO         
                                );
                                vFlagEnvio = true;
                            }
                        }

                        if (vFlagEnvio){
                            Response.Redirect("/pages/tiempoExtraordinario/PendientesAprobarJefe.aspx?ex=8");
                        }
                    }
                }else{
                    limpiarAprobacionJefe();
                    Response.Redirect("/pages/tiempoExtraordinario/PendientesAprobarJefe.aspx?ex=9");
                }
            }catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }        }
        protected void BtnDesHoja_Click(object sender, EventArgs e)        {            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openDescargarModal();", true);        }
        protected void btnDescargarHoja_Click(object sender, EventArgs e)        {            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openDescargarModal();", true);        }
        protected void DdlAccionRRHH_SelectedIndexChanged(object sender, EventArgs e)        {            if (DdlAccionRRHH.SelectedValue == "1")            {                LimpiarAprobacionRRHH();                calculoHoras();                DivMotivoCancelacionRRHH.Visible = false;                TxMotivoRRHH.Visible = false;                Session["STELBESTADORRHH"] = "aprobar";                LBTituloRRHH.Text = "Aprobar solicitud número " + Session["STENUMSOLICITUD"].ToString();            }            else if (DdlAccionRRHH.SelectedValue == "2")            {                LimpiarAprobacionRRHH();                DivMotivoCancelacionRRHH.Visible = true;                TxMotivoRRHH.Visible = true;                UpdatePanel13.Update();                Session["STELBESTADORRHH"] = "cancelar";                LBTituloRRHH.Text = "Cancelar solicitud número " + Session["STENUMSOLICITUD"].ToString();

                TxTotRRHH.Text = "00:00 (0.0)";                TxTotDiurnasRRHH.Text = "00:00 (0.0)";                TxTotNocRRHH.Text = "00:00 (0.0)";                TxTotNocNocRRHH.Text = "00:00 (0.0)";                TxTotDomFeriadoRRHH.Text = "00:00 (0.0)";                UpdatePanel26.Update();            }        }
        private void LimpiarAprobacionRRHH()        {            DdlMotivosCancelacionRRHH.SelectedIndex = -1;            TxMotivoRRHH.Text = String.Empty;        }
        protected void BtnEnviarRRHH_Click(object sender, EventArgs e)        {            validacionAprobacionRRHH();            String vFI = TxFechaInicio.Text != "" ? TxFechaInicio.Text : "1999-01-01 00:00:00";            String vFF = TxFechaRegreso.Text != "" ? TxFechaRegreso.Text : "1999-01-01 00:00:00";            DateTime desde = Convert.ToDateTime(vFI);            DateTime hasta = Convert.ToDateTime(vFF);            if (DdlAccionRRHH.SelectedValue == "1")            {
                Session["STELBESTADORRHH"] = "aprobar";                LBTituloRRHH.Text = "Aprobar solicitud número " + Session["STENUMSOLICITUD"].ToString();                Session["STEBDESTADORRHH"] = "1";                Session["STEIDESTADORRHH"] = "5";//Aprobada RRHH
                Session["STELBESTADORRHHCORREO"] = "Aprobacion RRHH";
            }            else if (DdlAccionRRHH.SelectedValue == "2")            {                Session["STEBDESTADORRHH"] = "2";                Session["STEIDESTADORRHH"] = "6";//Cancelada RRHH
                Session["STELBESTADORRHH"] = "cancelar";                LBTituloRRHH.Text = "Cancelar solicitud número " + Session["STENUMSOLICITUD"].ToString();
                Session["STELBESTADORRHHCORREO"] = "Cancelada RRHH";

                Session["STEHRDIURNASREAL"] = 0;                Session["STEMINDIURNASREAL"] = 0;                Session["STEHRNOCREAL"] = 0;                Session["STEMINNOCREAL"] = 0;                Session["STEHRNOCNOCREAL"] = 0;                Session["STEMINNOCNOCREAL"] = 0;                Session["STEHRDOMINGOFERIADOREAL"] = 0;                Session["STEMINDOMINGOFERIADOREAL"] = 0;                Session["STEHRTOTREAL"] = 0;                Session["STEMINTOTREAL"] = 0;            }            LbAprobarRRHH.Text =
              "Fechas solicitadas del <b>" + desde.ToString("yyyy-MM-dd HH:mm:ss") + "</b> al <b>" + hasta.ToString("yyyy-MM-dd HH:mm:ss") + "</b><br /><br />" + "Total de horas solicitadas: <b> " + TxTotalHoras.Text + "</b><br />" + "Horas Diurnas: <b>" + TxHrDiurnas.Text + "</b>, Horas Noc: <b>" + TxHrNoc.Text + "</b>, Horas NocNoc: <b>" + TxHrNocNoc.Text + "</b>, Horas Domingos Feriados: <b>" + TxHrDomFeriado.Text + " </b>" + "<br /><br />" + "Total de horas aprobadas: <b> " + TxTotRRHH.Text + "</b><br />" + "Horas Diurnas: <b>" + TxTotDiurnasRRHH.Text + "</b>, Horas Noc: <b>" + TxTotNocRRHH.Text + "</b>, Horas NocNoc: <b>" + TxTotNocNocRRHH.Text + "</b>, Horas Domingos Feriados: <b>" + TxTotDomFeriadoRRHH.Text + "</b><br /><br />" +              "Trabajo realizado: <b>" + TxDescripcionTrabajo.Text + "</b><br /><br />";            LbAprobarRRHHPregunta.Text = "<b>¿Está seguro que desea " + Session["STELBESTADORRHH"].ToString() + " la solicitud de:</ b > " + TxEmpleado.Text + "<b> ,en el rango de fechas y horas detalladas?</b>";            UpdateAprobarRRHH.Update();            UpTituloAprobarRRHHModal.Update();            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openAprobarRRHHModal();", true);        }
        void validacionAprobacionRRHH()        {            if (DdlAccionRRHH.SelectedValue.Equals("0"))                throw new Exception("Falta que seleccione acción si esta seguro de autorizar la solicitud.");            if (DdlAccionRRHH.SelectedValue == "2" && DdlMotivosCancelacionRRHH.SelectedValue.Equals("0"))                throw new Exception("Falta que seleccione motivo de cancelación");            if (DdlAccionRRHH.SelectedValue == "2" && TxMotivoRRHH.Text.Equals(""))                throw new Exception("Falta que ingrese observación para retroalimentar al colobador de la decisión cancelación de la solicitud");        }

        protected void BtnAprobarRrhhModal_Click(object sender, EventArgs e)        {            try            {
                string vUbicacion = "";                DataTable vDatosSysAid = new DataTable();                String vQuerySysAid = "RSP_TiempoExtraordinarioGeneralesBiometrico 2,'" + TxPeticion.Text + "'";                vDatosSysAid = vConexionSysAid.obtenerDataTableSysAid(vQuerySysAid);

                if (vDatosSysAid.Rows.Count > 0){
                    vUbicacion = vDatosSysAid.Rows[0]["value_caption"].ToString();
                } else{
                    vUbicacion = "";
                }

                DataTable vDatos = new DataTable();                String vQuery = "RSP_TiempoExtraordinarioGenerales 27,"                    + Session["STENUMSOLICITUD"]                    + ",'" + Session["USUARIO"]                    + "','" + Session["STEBDESTADORRHH"]                    + "','" + TxMotivoRRHH.Text                    + "','" + DdlMotivosCancelacionRRHH.SelectedValue                    + "','" + Session["STEIDESTADORRHH"]                    + "','" + TxTotRRHH.Text                    + "','" + TxTotDiurnasRRHH.Text                    + "','" + TxTotNocRRHH.Text                    + "','" + TxTotNocNocRRHH.Text                    + "','" + TxTotDomFeriadoRRHH.Text                    + "','" + Session["STEHRDIURNASREAL"]                    + "','" + Session["STEMINDIURNASREAL"]                    + "','" + Session["STEHRNOCREAL"]                    + "','" + Session["STEMINNOCREAL"]                    + "','" + Session["STEHRNOCNOCREAL"]                    + "','" + Session["STEMINNOCNOCREAL"]                    + "','" + Session["STEHRDOMINGOFERIADOREAL"]                    + "','" + Session["STEMINDOMINGOFERIADOREAL"]                    + "','" + Session["STEHRTOTREAL"]                    + "','" + Session["STEMINTOTREAL"]
                    + "','" + TxJustificacionHSB.Text                    + "','" + LbJustificacionHEB.Text                    + "','" + LbReintegroAlmuerzo.Text
                    + "','" + vUbicacion + "'";                Int32 vRespuesta = vConexion.ejecutarSql(vQuery);


                if (vRespuesta == 1)                {
                    if (DdlAccionRRHH.SelectedValue == "1")
                    {
                        DataTable vDatosSolicitudFaltante = new DataTable();
                        vDatosSolicitudFaltante = (DataTable)Session["STESOLICITUDESHrsFALTANTES"];

                        if (vDatosSolicitudFaltante.Rows.Count > 0)
                        {
                            foreach (DataRow item in vDatosSolicitudFaltante.Rows)
                            {
                                string solicitud = item["idSolicitud"].ToString();
                                string vDiurnas = item["faltanteHrsDiurnas"].ToString();
                                string vNoc = item["faltanteHrsNoc"].ToString();
                                string vNocNoc = item["faltanteHrsNocNoc"].ToString();

                                if (vDiurnas != "00:00:00" || vNoc != "00:00:00" || vNocNoc != "00:00:00")
                                {
                                    string vQueryUpHrsFaltantes = "RSP_TiempoExtraordinarioGenerales 45,"
                                    + solicitud
                                    + ",'00:00:00'"
                                    + ",'00:00:00'"
                                    + ",'00:00:00'";
                                    vDatos = vConexion.obtenerDataTable(vQueryUpHrsFaltantes);
                                }

                            }
                        }

                        string vQueryHrsFaltantes = "RSP_TiempoExtraordinarioGenerales 44,"
                                                    + Session["STENUMSOLICITUD"]
                                                    + ",'" + Session["STEHRINICIAL"]
                                                    + "','" + Session["STEFALTANTEDIURNAS"]
                                                    + "','" + Session["STEFALTANTENOC"]
                                                    + "','" + Session["STEFALTANTENOCNOC"]
                                                    + "'," + Session["STECODIGOSAP"];
                        vDatos = vConexion.obtenerDataTable(vQueryHrsFaltantes);

                        DataTable vDataColaborador = (DataTable)Session["STEDATOSGENERALESCOLABORADOR"];
                        string vQuerySuscripcion = "RSP_TiempoExtraordinarioGenerales 59,'Solicitud Tiempo Extraordinario','"
                            + vDataColaborador.Rows[0]["emailEmpresa"].ToString()
                            + "','egutierrez@bancatlan.hn','Solicitud Tiempo Extraordinario RRHH','" + Session["STELBESTADORRHHCORREO"]
                            + "','0','" + Session["STENUMSOLICITUD"]+"'" ;
                        vDatos = vConexion.obtenerDataTable(vQuerySuscripcion);


                        DataTable vData = (DataTable)Session["STEDATOSGENERALESCOLABORADOR"];
                        vQuery = "RSP_ObtenerEmpleados 2," + vData.Rows[0]["idJefe"].ToString();
                        DataTable vDatosJefe = vConexion.obtenerDataTable(vQuery);

                        Boolean vFlagEnvio = false;
                        foreach (DataRow item in vDatosJefe.Rows){
                            if (!item["emailEmpresa"].ToString().Trim().Equals("")){
                                vService.EnviarMensaje(
                                    vData.Rows[0]["emailEmpresa"].ToString(), // CORREO DEL TECNICO
                                    typeBody.TiempoExtraordinario,
                                    vData.Rows[0]["nombre"].ToString(), // NOMBRE DEL TECNICO
                                    "Se notifica que la solicitud de tiempo extraordinario numero: " + Session["STENUMSOLICITUD"] + " ha sido aprobada por RRHH.",
                                    "Puede ver el estado de todas sus solicitudes en la sección 'Mis Solicitudes'.",
                                    item["emailEmpresa"].ToString() // CORREO DEL JEFE - COPIADO
                                );
                                vFlagEnvio = true;
                            }
                        }

                        if (vFlagEnvio){
                            Response.Redirect("/pages/tiempoExtraordinario/pendientesAprobarRRHH.aspx?ex=1");
                        }                    }else if (DdlAccionRRHH.SelectedValue == "2"){
                        DataTable vData = (DataTable)Session["STEDATOSGENERALESCOLABORADOR"];
                        vQuery = "RSP_ObtenerEmpleados 2," + vData.Rows[0]["idJefe"].ToString();
                        DataTable vDatosJefe = vConexion.obtenerDataTable(vQuery);

                        Boolean vFlagEnvio = false;
                        foreach (DataRow item in vDatosJefe.Rows){
                            if (!item["emailEmpresa"].ToString().Trim().Equals("")){
                                vService.EnviarMensaje(
                                    vData.Rows[0]["emailEmpresa"].ToString(), // CORREO DEL TECNICO
                                    typeBody.TiempoExtraordinario,
                                    vData.Rows[0]["nombre"].ToString(), // NOMBRE DEL TECNICO
                                    "Se notifica que la solicitud de tiempo extraordinario numero: " + Session["STENUMSOLICITUD"] + " fue cancelado por RRHH, por el siguiente motivo: (" + DdlMotivosCancelacionRRHH.SelectedItem + ") " + TxMotivoRRHH.Text,
                                    "Puede ver el estado de todas sus solicitudes en la sección 'Mis Solicitudes'.",
                                    item["emailEmpresa"].ToString() // CORREO DEL JEFE - COPIADO
                                );
                                vFlagEnvio = true;
                            }
                        }

                        if (vFlagEnvio){
                            Response.Redirect("/pages/tiempoExtraordinario/pendientesAprobarRRHH.aspx?ex=2");
                        }
                    }
                }else{
                    Response.Redirect("/pages/tiempoExtraordinario/pendientesAprobarRRHH.aspx?ex=3");
                }            }            catch (Exception ex)            {                Mensaje(ex.Message, WarningType.Danger);            }        }

        protected void BtnCancelarSolicitud_Click(object sender, EventArgs e)
        {
            try            {
                //limpiarCrearSolicitud();     
                Response.Redirect("/pages/tiempoExtraordinario/solicitudTE.aspx?ex=6");
            }
            catch (Exception ex)            {                Mensaje(ex.Message, WarningType.Danger);            }
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVBusqueda.PageIndex = e.NewPageIndex;
                GVBusqueda.DataSource = (DataTable)Session["STESOLICITUDESCREADAS"];
                GVBusqueda.DataBind();
                UpdateDivBusquedas.Update();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void DdlAprobacionSubgerente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DdlAprobacionSubgerente.SelectedValue == "1")
            {
                TxCancelacionSubgerente.Visible = false;
                Session["STELBESTADOSUBGERENTE"] = "aprobar";
                LbTituloSubGerente.Text = "Aprobar solicitud número " + Session["STENUMSOLICITUD"].ToString();
                Session["STEESTADOBDSUBGERNETE"] = "1"; //Para las aprobadas subgernete
                Session["STEESTADOBDSOLICITUD"] = "3";/*Pendiente Aprobar RRHH*/


            }
            else
            {
                TxCancelacionSubgerente.Visible = true;
                Session["STELBESTADOSUBGERENTE"] = "cancelar";
                LbTituloSubGerente.Text = "Cancelar solicitud número " + Session["STENUMSOLICITUD"].ToString();
                Session["STEESTADOBDSUBGERNETE"] = "2"; //Para las canceladas subgernete
                Session["STEESTADOBDSOLICITUD"] = "8";//Cancelada Subgerente
            }
        }

        void validacionAprobacionSubGerente()        {            if (DdlAprobacionSubgerente.SelectedValue.Equals("0"))                throw new Exception("Falta que seleccione acción si esta seguro de autorizar la solicitud.");            if (DdlAprobacionSubgerente.SelectedValue == "2" && TxCancelacionSubgerente.Text.Equals(""))                throw new Exception("Falta que seleccione motivo de cancelación");        }

        protected void BtnAprobacionSubGerente_Click(object sender, EventArgs e)
        {
            try
            {
                validacionAprobacionSubGerente();
                String vFI = TxFechaInicio.Text != "" ? TxFechaInicio.Text : "1999-01-01 00:00:00";                String vFF = TxFechaRegreso.Text != "" ? TxFechaRegreso.Text : "1999-01-01 00:00:00";                DateTime desde = Convert.ToDateTime(vFI);                DateTime hasta = Convert.ToDateTime(vFF);

                LbAprobarSubgerente.Text = "Buen dia <b> " + Session["STENOMBRESUBGERENTE"] + "</b><br /><br />" +
                 "Fechas solicitadas del <b>" + desde.ToString("yyyy-MM-dd HH:mm:ss") + "</b> al <b>" + hasta.ToString("yyyy-MM-dd HH:mm:ss") + "</b>" + " .Total de horas <b> " + TxTotalHoras.Text + "</b> <br /><br />" +
                 "Trabajo realizado: <b>" + TxDescripcionTrabajo.Text + "</b><br /><br />";                LbAprobarSubgerentePregunta.Text = "<b>¿Está seguro que desea " + Session["STELBESTADOSUBGERENTE"].ToString() + " la solicitud de:</ b > " + Session["STENOMBRECOLABORADOR"] + "<b> ,en el rango de fechas y horas detalladas?</b>";                UpdateAprobarSubgerente.Update();                UpTituloSubgerente.Update();                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openAprobarSubgerenteModal();", true);

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnEnviarAprobaciónSubgerente_Click(object sender, EventArgs e)
        {
            try
            {
                String vQuery = "RSP_TiempoExtraordinarioGenerales 33,'"
                    + Session["STENUMSOLICITUD"]
                    + "','" + Session["USUARIO"]
                    + "','" + Session["STEESTADOBDSUBGERNETE"]
                    + "','" + TxCancelacionSubgerente.Text
                    + "'," + Session["STEESTADOBDSOLICITUD"];                Int32 vRespuesta = vConexion.ejecutarSql(vQuery);                String vRe = "";

                if (vRespuesta == 1)                {
                    if (Session["STEESTADOBDSOLICITUD"].ToString() == "3")
                    {
                        DataTable vData = (DataTable)Session["STEDATOSGENERALESCOLABORADOR"];
                        vQuery = "RSP_ObtenerEmpleados 2," + vData.Rows[0]["idJefe"].ToString();
                        DataTable vDatosJefe = vConexion.obtenerDataTable(vQuery);
                        
                        Boolean vFlagEnvio = false;
                        foreach (DataRow item in vDatosJefe.Rows){
                            if (!item["emailEmpresa"].ToString().Trim().Equals("")){
                                vService.EnviarMensaje(
                                    ConfigurationManager.AppSettings["RHMail"].ToString(), // CORREO RRHH
                                    typeBody.TiempoExtraordinario,
                                    "Señores de RRHH", // NOMBRE DEL PERSONAL DE RRHH
                                    "Se notifica que ya puede proceder con la aprobación de la solicitud de tiempo extraordinario del colaborador: " + vData.Rows[0]["nombre"].ToString(),
                                    "",
                                    vData.Rows[0]["emailEmpresa"].ToString() // CORREO DEL TECNICO - COPIADO
                                );
                                vFlagEnvio = true;
                            }
                        }

                        if (vFlagEnvio){
                            Response.Redirect("/pages/tiempoExtraordinario/pendientesAprobarSubgerente.aspx?ex=1");
                        }
                    }else if (Session["STEESTADOBDSOLICITUD"].ToString() == "8"){
                        DataTable vData = (DataTable)Session["STEDATOSGENERALESCOLABORADOR"];
                        vQuery = "RSP_ObtenerEmpleados 2," + vData.Rows[0]["idJefe"].ToString();
                        DataTable vDatosJefe = vConexion.obtenerDataTable(vQuery);
                        
                        Boolean vFlagEnvio = false;
                        foreach (DataRow item in vDatosJefe.Rows){
                            if (!item["emailEmpresa"].ToString().Trim().Equals("")){
                                vService.EnviarMensaje(
                                    vData.Rows[0]["emailEmpresa"].ToString(), // CORREO DEL TECNICO
                                    typeBody.TiempoExtraordinario,
                                    vData.Rows[0]["nombre"].ToString(), // NOMBRE DEL TECNICO
                                    "Se notifica que el subgerente/suplente ha cancelado la solicitud de Tiempo Extraordinario, por el siguiente motivo: " + TxCancelacionSubgerente.Text,
                                    "Si tiene alguna duda con respecto a la decision abocarse con su jefe inmediato para que le consulte con mayor detalle al subgerente.",
                                    item["emailEmpresa"].ToString() // CORREO DEL JEFE - COPIADO
                                );
                                vFlagEnvio = true;
                            }
                        }

                        if (vFlagEnvio){
                            Response.Redirect("/pages/tiempoExtraordinario/pendientesAprobarSubgerente.aspx?ex=2");
                        }
                    }
                }
                else
                {
                    limpiarAprobacionJefe();
                    Response.Redirect("/pages/tiempoExtraordinario/pendientesAprobarSubgerente.aspx?ex=3");
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        void CargarSolicitudesModificar()        {            try            {                string usuario = Session["USUARIO"].ToString();                DataTable vDatos = new DataTable();                vDatos = vConexion.obtenerDataTable("RSP_TiempoExtraordinarioGenerales 38,'" + Convert.ToString(Session["USUARIO"]) + "'"); //2902
                GVPendienteModificar.DataSource = vDatos;                GVPendienteModificar.DataBind();                UpPendienteModificar.Update();
                Session["STESOLICITUDPENDIENTEMODIFICAR"] = vDatos;
            }            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnCancelarJefe_Click(object sender, EventArgs e)
        {
            try
            {
                DdEstadoSoliJefe.SelectedIndex = -1;
                DdlMotivosCancelacion.SelectedIndex = -1;
                TxObservacion.Text = String.Empty;
                Response.Redirect("/pages/tiempoExtraordinario/PendientesAprobarJefe.aspx");
            }            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void RbFormaTrabajo_SelectedIndexChanged(object sender, EventArgs e)
        {

            TxFechaInicio.Text = String.Empty;
            TxFechaRegreso.Text = String.Empty;
            LbFechaRangoBien.Visible = false;
            LbFechaRangoMal.Visible = false;
            DivAprobacionSubGerente.Visible = false;
            DdlMotivoAprobacionSubgerente.SelectedIndex = -1;
            TxSoliAprobacionSubGerente.Text = String.Empty;

            TxTotalHoras.Text = "00:00 (0.0 Hrs)";
            TxHrDiurnas.Text = "00:00 (0.0 Hrs)";
            TxHrNoc.Text = "00:00 (0.0 Hrs)";
            TxHrNocNoc.Text = "00:00 (0.0 Hrs)";
            TxHrDomFeriado.Text = "00:00 (0.0 Hrs)";
            DivUnaFecha.Visible = false;
            UpdatePanelFechas.Update();
            UpDivUnaFecha.Update();
            String vEx = Request.QueryString["ex"];

            if (!String.IsNullOrEmpty(vEx)){
                if (vEx.Equals("7")){
                    DataTable vDatosSolicitud = new DataTable();
                    vDatosSolicitud = (DataTable)Session["STEDATOSSOLICITUDINDIVIDUAL"];

                    string vTipoTrabajo = vDatosSolicitud.Rows[0]["formaTrabajo"].ToString();

                    if (vTipoTrabajo == "1" && RbFormaTrabajo.SelectedValue.Equals("1")){
                        string vFormato = "yyyy-MM-ddTHH:mm";
                        string vFechaInicio = vDatosSolicitud.Rows[0]["fechaInicio"].ToString();
                        string vFechaFin = vDatosSolicitud.Rows[0]["fechaFin"].ToString();
                        string vFechaInicioConvertida = Convert.ToDateTime(vFechaInicio).ToString(vFormato);
                        string vFechaFinConvertida = Convert.ToDateTime(vFechaFin).ToString(vFormato);

                        TxFechaInicio.Text = vFechaInicioConvertida;
                        TxFechaRegreso.Text = vFechaFinConvertida;

                        UpdatePanelFechas.Update();
                        calculoHoras();
                        DivUnaFecha.Visible = true;
                        UpDivUnaFecha.Update();

                    }else if (vTipoTrabajo == "2" && RbFormaTrabajo.SelectedValue.Equals("2")){
                        string vFormato = "yyyy-MM-ddTHH:mm";
                        string vFechaInicio = vDatosSolicitud.Rows[0]["fechaInicio"].ToString();
                        string vFechaFin = vDatosSolicitud.Rows[0]["fechaFin"].ToString();
                        string vFechaInicioConvertida = Convert.ToDateTime(vFechaInicio).ToString(vFormato);
                        string vFechaFinConvertida = Convert.ToDateTime(vFechaFin).ToString(vFormato);

                        TxFechaInicio.Text = vFechaInicioConvertida;
                        TxFechaRegreso.Text = vFechaFinConvertida;

                        UpdatePanelFechas.Update();
                        calculoHoras();
                        DivUnaFecha.Visible = true;
                        UpDivUnaFecha.Update();

                    }else{
                        TxFechaInicio.Text = string.Empty;
                        TxFechaRegreso.Text = string.Empty;
                        DivUnaFecha.Visible = false;
                        UpDivUnaFecha.Update();
                    }

                    if (vDatosSolicitud.Rows[0]["aprobacionSubgerente"].ToString() == "1"){
                        LbFechaRangoMal.Visible = true;
                        DivAprobacionSubGerente.Visible = true;
                    }else{
                        LbFechaRangoBien.Visible = true;
                        DivAprobacionSubGerente.Visible = false;
                    }
                }
            }
        }

        protected void DDLEmpleadoSolicitud_SelectedIndexChanged(object sender, EventArgs e)
        {
            try            {
                DataTable vDatos = new DataTable();
                String vQuery = "RSP_TiempoExtraordinarioGenerales 1,'" + DDLEmpleadoSolicitud.SelectedValue + "'";
                vDatos = vConexion.obtenerDataTable(vQuery);

                TxEmpleado.Text = vDatos.Rows[0]["idEmpleado"].ToString() + " - " + vDatos.Rows[0]["nombre"].ToString();
                TxJefe.Text = vDatos.Rows[0]["idJefe"].ToString() + " - " + vDatos.Rows[0]["jefeNombre"].ToString();
                TxTurno.Text = vDatos.Rows[0]["nombreTurno"].ToString();
                TxSubgerencia.Text = vDatos.Rows[0]["area"].ToString();
                Session["TIEMPO_EX_TECNICO"] = vDatos;

                Session["STECIUDAD"] = vDatos.Rows[0]["ciudad"].ToString();
                Session["STETURNO"] = vDatos.Rows[0]["nombreTurno"].ToString();
                Session["STEIDJEFE"] = vDatos.Rows[0]["idJefe"].ToString();

                Session["STECODIGOSAP"] = vDatos.Rows[0]["idEmpleado"].ToString();
                Session["STECODIGOSAPBIOMETRICO"] = vDatos.Rows[0]["codigoSAP"].ToString();

                Session["STEHORAENTRADA"] = vDatos.Rows[0]["horaInicio"].ToString();
                Session["STEHORASALIDA"] = vDatos.Rows[0]["horaFinal"].ToString();
                Session["STENOMBREJEFE"] = vDatos.Rows[0]["jefeNombre"].ToString();
                Session["STEDIASTURNO"] = vDatos.Rows[0]["dias"].ToString();

                Session["STEPUESTOJEFE"] = vDatos.Rows[0]["puestoJefe"].ToString();
                Session["STEPUESTOCOLABORADOR"] = vDatos.Rows[0]["idPuesto"].ToString();

                Session["STESUBGERENCIA"] = vDatos.Rows[0]["subGerencia"].ToString();
                Session["STEIDJEFESUBGERENCIA"] = vDatos.Rows[0]["subgerente"].ToString();

                //DECISION PARA OCULTAR O MOSTRAR LAS OPCIONES DE CONDUCTORES
                if (Session["STEPUESTOJEFE"].ToString().Equals("20000383") || Session["STEPUESTOJEFE"].ToString().Equals("20000395") || Session["STEPUESTOCOLABORADOR"].ToString().Equals("20000409"))
                {
                    DivConductor.Visible = false;
                }
                else
                {
                    DivConductor.Visible = true;
                }

                //DECISION PARA OCULTAR O MOSTRAR EL CAMPO DE SYSAID Y HOJA DE SERVICIO
                if (Session["STESUBGERENCIA"].ToString().Equals("4") || Session["STESUBGERENCIA"].ToString().Equals("2"))
                {
                    DivSysAid.Visible = true;
                    lbHojaServicio.Visible = true;
                    FuHojaServicio.Visible = true;
                    btnVisualizarHoja.Visible = true;
                }
                else
                {
                    DivSysAid.Visible = false;
                    lbHojaServicio.Visible = false;
                    FuHojaServicio.Visible = false;
                    btnVisualizarHoja.Visible = false;
                }
                String vTest = string.Empty;

                //LLENAR LISTA DESPLEGABLE DE COLABORADORES DEL MISMO JEFE PARA CAMBIO DE TURNO
                DDLCambioTurnoColaborador.Items.Clear();
                vQuery = "RSP_TiempoExtraordinarioGenerales 2,'" + Session["STEIDJEFE"] + "'";
                vDatos = vConexion.obtenerDataTable(vQuery);

                DDLCambioTurnoColaborador.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows)
                {
                    DDLCambioTurnoColaborador.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["idEmpleado"].ToString() + " - " + item["nombre"].ToString() });
                }

                DdlTipoTrabajo.Items.Clear();
                vQuery = "RSP_TiempoExtraordinarioGenerales 3";
                vDatos = vConexion.obtenerDataTable(vQuery);
                DdlTipoTrabajo.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows)
                {
                    DdlTipoTrabajo.Items.Add(new ListItem { Value = item["idTipoTrabajo"].ToString(), Text = item["nombreTrabajo"].ToString() });
                }

            }            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }        }

        protected void GVPendienteModificar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVPendienteModificar.PageIndex = e.NewPageIndex;
                GVPendienteModificar.DataSource = (DataTable)Session["STESOLICITUDPENDIENTEMODIFICAR"];
                GVPendienteModificar.DataBind();
                UpPendienteModificar.Update();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVPendienteModificar_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vIdSolicitud = e.CommandArgument.ToString();
            if (e.CommandName == "DetalleSolicitud")
            {
                DataTable vDatos = new DataTable();
                //DATOS GENERALES
                string vQuery = "RSP_TiempoExtraordinarioGenerales 19," + vIdSolicitud;
                vDatos = vConexion.obtenerDataTable(vQuery);
                string vCodigo = vDatos.Rows[0]["codigoSAP"].ToString();
                Session["STEDATOSSOLICITUDINDIVIDUAL"] = vDatos;

                vQuery = "RSP_TiempoExtraordinarioGenerales 1,'" + vCodigo + "'";
                vDatos = vConexion.obtenerDataTable(vQuery);
                Session["STEDATOSGENERALESCOLABORADOR"] = vDatos;

                Response.Redirect("/pages/tiempoExtraordinario/solicitudTE.aspx?ex=7");

            }
        }

        private void limpiarModificarSolicitud()
        {
            TxTotalHoras.Text = "00:00 (0.0 Hrs)";
            TxHrDiurnas.Text = "00:00 (0.0 Hrs)";
            TxHrNoc.Text = "00:00 (0.0 Hrs)";
            TxHrNocNoc.Text = "00:00 (0.0 Hrs)";
            TxHrDomFeriado.Text = "00:00 (0.0 Hrs)";
            UpdatePanel2.Update();

            DivUnaFecha.Visible = false;
            UpDivUnaFecha.Update();


        }

        protected void RbCambioHoja_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RbCambioHoja.SelectedValue.Equals("1"))
            {
                lbHojaServicio.Visible = true;
                FuHojaServicio.Visible = true;
                TxImagenSubida.Visible = false;
                btnVisualizarHoja.Visible = true;
                btnDescargarHoja.Visible = false;
            }
            else
            {
                lbHojaServicio.Visible = true;
                FuHojaServicio.Visible = false;
                TxImagenSubida.Visible = true;
                btnVisualizarHoja.Visible = true;
                btnDescargarHoja.Visible = true;
            }
        }

        protected void BtnEnviarModificada_Click(object sender, EventArgs e)
        {
            try
            {
                validacionesCrearSolicitud();
                String vFI = TxFechaInicio.Text != "" ? TxFechaInicio.Text : "1999-01-01 00:00:00";                String vFF = TxFechaRegreso.Text != "" ? TxFechaRegreso.Text : "1999-01-01 00:00:00";                DateTime desde = Convert.ToDateTime(vFI);                DateTime hasta = Convert.ToDateTime(vFF);                DateTime vFechaInicio = Convert.ToDateTime(vFI);                LbInformacionTEModificada.Text = "Buen dia <b> " + TxEmpleado.Text + "</b><br /><br />" +                   "Fechas solicitadas del <b>" + desde.ToString("yyyy-MM-dd HH:mm:ss") + "</b> al <b>" + hasta.ToString("yyyy-MM-dd HH:mm:ss") + "</b>" + " .Total de horas <b> " + TxTotalHoras.Text + "</b> <br /><br />" +                   "Trabajo realizado: <b>" + TxDescripcionTrabajo.Text + "</b><br /><br />";                LbInformacionPreguntaTEModificada.Text = "<b>¿Está seguro que desea enviar la solicitud modificada en el rango de fechas y horas detalladas?</b>";                UpdatePanel24.Update();                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "OpenSolicitudModificada();", true);
            }            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnEnviarSoliModificada_Click(object sender, EventArgs e)
        {
            try            {
                DataTable vDatosSolicitud = new DataTable();
                vDatosSolicitud = (DataTable)Session["STEDATOSSOLICITUDINDIVIDUAL"];

                String vNombreDepot1 = String.Empty;                String vExtension = String.Empty;                String vArchivo = String.Empty;                if (RbCambioHoja.SelectedValue == "1")
                {
                    HttpPostedFile bufferDeposito1T = FuHojaServicio.PostedFile;
                    byte[] vFileDeposito1 = null;

                    if (bufferDeposito1T != null)
                    {
                        vNombreDepot1 = FuHojaServicio.FileName;
                        Stream vStream = bufferDeposito1T.InputStream;
                        BinaryReader vReader = new BinaryReader(vStream);
                        vFileDeposito1 = vReader.ReadBytes((int)vStream.Length);
                        vExtension = System.IO.Path.GetExtension(FuHojaServicio.FileName);
                    }

                    if (vFileDeposito1 != null)
                        vArchivo = Convert.ToBase64String(vFileDeposito1);
                }
                else
                {

                    vNombreDepot1 = vDatosSolicitud.Rows[0]["nombreHojaServicio"].ToString();
                    vExtension = vDatosSolicitud.Rows[0]["extension"].ToString();
                    vArchivo = vDatosSolicitud.Rows[0]["hojaServicio"].ToString();
                }


                String vFormato = "yyyy-MM-dd HH:mm:ss.000"; //"dd-MM-yyyy HH:mm:ss.000"; 
                String vFeINI = Convert.ToDateTime(TxFechaInicio.Text).ToString(vFormato);                String vFeFIN = Convert.ToDateTime(TxFechaRegreso.Text).ToString(vFormato);                DataTable vDatos = new DataTable();                String vQuery = "RSP_TiempoExtraordinarioGenerales 42,'"                    + vDatosSolicitud.Rows[0]["idSolicitud"].ToString()
                    + "','" + RbCambioTurno.SelectedValue                    + "','" + Session["STEIDTURNOCAMBIO"]                    + "'," + DDLCambioTurnoColaborador.SelectedValue                    + ",'" + TxMotivoCambioTurno.Text                    + "','" + TxTotalHoras.Text                    + "','" + TxHrDiurnas.Text                    + "','" + TxHrNoc.Text                    + "','" + TxHrNocNoc.Text                    + "','" + TxHrDomFeriado.Text                    + "','" + vFeINI                    + "','" + vFeFIN                    + "','" + Session["STEHRINICIO"]                    + "','" + Session["STEHRFIN"]                    + "'," + Session["STEHRDIURNASOLICITADAS"]                    + "," + Session["STEMINDIURNASOLICITADAS"]                    + "," + Session["STEHRNOCSOLICITADAS"]                    + "," + Session["STEMINNOCSOLICITADAS"]                    + "," + Session["STEHRNOCNOCSOLICITADAS"]                    + "," + Session["STEMINNOCNOCSOLICITADAS"]                    + "," + Session["STEHRDOMINGOFERIADOSOLICITADAS"]                    + "," + Session["STEMINDOMINGOFERIADOSOLICITADAS"]                    + "," + Session["STEHRTOTALSOLICITADAS"]                    + "," + Session["STEMINTOTALSOLICITADAS"]                    + ",'" + TxPeticion.Text                    + "','" + TxTituloSysaid.Text                    + "'," + DdlConductor.SelectedValue                    + "," + DdlConductorNombre.SelectedValue                    + "," + DdlTipoTrabajo.SelectedValue                    + "," + DdlTipoDescripcion.SelectedValue                    + ",'" + TxDescripcionTrabajo.Text                    + "','" + vArchivo                    + "',1,'" + vExtension                    + "','" + Session["STEIDJEFE"]
                    + "','" + vNombreDepot1
                    + "','" + DdlMotivoAprobacionSubgerente.SelectedValue                    + "','" + TxSoliAprobacionSubGerente.Text                    + "','" + vDatosSolicitud.Rows[0]["aprobacionSubgerente"].ToString()
                    + "'," + Session["STEIDJEFESUBGERENCIA"]
                    + "," + RbFormaTrabajo.SelectedValue;
                //vDatos = vConexion.obtenerDataTable(vQuery);
                Int32 vRespuesta = vConexion.ejecutarSql(vQuery);

                if (vRespuesta == 1)
                {



                    DataTable vData = (DataTable)Session["TIEMPO_EX_TECNICO"];
                    vQuery = "RSP_ObtenerEmpleados 2," + vData.Rows[0]["idJefe"].ToString();
                    DataTable vDatosJefe = vConexion.obtenerDataTable(vQuery);

                    
                    Boolean vFlagEnvio = false;

                    foreach (DataRow item in vDatosJefe.Rows){                        if (!item["emailEmpresa"].ToString().Trim().Equals("")){                            vService.EnviarMensaje(                                item["emailEmpresa"].ToString(), // CORREO DEL JEFE                                typeBody.TiempoExtraordinario,                                item["nombre"].ToString(), // NOMBRE DEL JEFE                                "El empleado " + vData.Rows[0]["nombre"].ToString() + " ha modificado la solicitud de Tiempo Extraordinario como se lo indico.",                                "Le informamos que la solicitud debe ser autorizada, para que sea procesada por Recursos Humanos.",                                vData.Rows[0]["emailEmpresa"].ToString() // CORREO DEL SOLICITANTE - COPIADO                                );                            vFlagEnvio = true;                        }                    }

                    if (vFlagEnvio){
                        Response.Redirect("/pages/tiempoExtraordinario/solicitudTE.aspx?ex=8");
                    }                }
                else

                {
                    Response.Redirect("/pages/tiempoExtraordinario/solicitudTE.aspx?ex=9");                }

                limpiarCrearSolicitud();
                nav_tecnicos_tab.Visible = true;
                nav_modificarSolicitud_tab.Visible = true;
                btnDescargarHoja.Visible = true;
            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, WarningType.Danger);
            }

        }

        protected void BtnCancelarModificada_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("/pages/tiempoExtraordinario/solicitudTE.aspx?ex=10");
            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            DdlMotivosCancelacionRRHH.SelectedIndex = -1;
            DdlAccionRRHH.SelectedIndex = -1;
            TxMotivoRRHH.Text = String.Empty;
            Response.Redirect("/pages/tiempoExtraordinario/pendientesAprobarRRHH.aspx");
        }
    }
}