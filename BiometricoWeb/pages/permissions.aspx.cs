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

namespace BiometricoWeb.pages
{
    
    public partial class permissions : System.Web.UI.Page
    {
        db vConexion;
        protected void Page_Load(object sender, EventArgs e)
        {
            vConexion = new db();
            String vEx = Request.QueryString["ex"];
            

            if (!Page.IsPostBack)
            {
                if (Convert.ToBoolean(Session["AUTH"]))
                {
                    CargarEmpleados();
                    CargarTipoPermisos();
                    CargarPermisos();
                    CargarDiasSAP();

                    if (vEx != null)
                    {
                        if (vEx.Equals("1"))
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + "Archivo subido con exito" + "')", true);
                        else if (vEx.Equals("2"))
                        {
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + "Error al subir archivo, por favor entregarlo en fisico a recursos humanos." + "')", true);
                        }
                        else if (vEx.Equals("3"))
                        {
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + "Permiso ingresado con exito." + "')", true);
                        }
                    }

                }
            }
        }

        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        public void CerrarModal(String vModal)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#" + vModal + "').modal('hide');", true);
        }
        void CargarEmpleados()
        {
            try
            {
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerGenerales 8,'" + Convert.ToString(Session["USUARIO"]) + "'");

                DDLEmpleado.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows)
                {
                    DDLEmpleado.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["idEmpleado"].ToString() + " - " + item["nombre"].ToString() });
                }

                DDLJefe.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows)
                {
                    DDLJefe.Items.Add(new ListItem { Value = item["idJefe"].ToString(), Text = item["idJefe"].ToString() + " - " + item["jefeNombre"].ToString() });
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        void CargarTipoPermisos()
        {
            try
            {
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerGenerales 4");

                DDLTipoPermiso.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows)
                {
                    DDLTipoPermiso.Items.Add(new ListItem { Value = item["idTipoPermiso"].ToString(), Text = item["idTipoPermiso"].ToString() + " - " + item["descripcion"].ToString() });
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        void CargarPermisos()
        {
            try
            {
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerGenerales 5,'" + Convert.ToString(Session["USUARIO"]) + "'"); //2902
                GVBusqueda.DataSource = vDatos;
                GVBusqueda.DataBind();
                Session["DATAOSPERMISOS"] = vDatos;
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); } // swift reiniciado, se creo una politica
        }

        void CargarDiasSAP()
        {
            try
            {
                SapConnector vTest = new SapConnector();
                String vDias = vTest.getDiasVacaciones(Convert.ToString(Session["CODIGOSAP"]));
                LbNumeroVaciones.Text = vDias;
                Session["DIASSAP"] = vDias;
            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void BtnCrearPermiso_Click(object sender, EventArgs e)
        {
            try
            {

                String vFechaInicio = Convert.ToDateTime(TxFechaInicio.Text).ToString("yyyy-MM-dd HH:mm:ss");
                String vFechaRegreso = Convert.ToDateTime(TxFechaRegreso.Text).ToString("yyyy-MM-dd HH:mm:ss");
                ValidacionesPermisos(DDLEmpleado.SelectedValue, vFechaInicio, vFechaRegreso, DDLTipoPermiso.SelectedValue);


                DateTime desde = Convert.ToDateTime(TxFechaInicio.Text);
                DateTime hasta = Convert.ToDateTime(TxFechaRegreso.Text);

                DateTime inicio = desde;
                int dias = 0;

                while (inicio <= hasta)
                {

                    if (inicio.DayOfWeek != DayOfWeek.Saturday && inicio.DayOfWeek != DayOfWeek.Sunday)
                        dias++;

                    inicio = inicio.AddDays(1);
                }

                TimeSpan ts = Convert.ToDateTime(vFechaRegreso) - Convert.ToDateTime(vFechaInicio);
                
                int days = 1;
                if (ts.Days >= 1)
                    days = dias; //ts.Days + 1 - 
                else if (ts.Hours > 0)
                {
                    days = 0;
                }

                if (days > 0 && ts.Hours > 0)
                    throw new Exception("Debes de crear permisos para dias y horas por separado");


                generales vGenerales = new generales();
                int vDias = vGenerales.BusinessDaysUntil(Convert.ToDateTime(TxFechaInicio.Text), Convert.ToDateTime(TxFechaRegreso.Text));


                LbInformacionPermiso.Text = "Informacion de Permiso de empleado " + DDLEmpleado.SelectedValue + "<br /><br />" +
                    "Fechas solicitadas del <b>" + vFechaInicio + "</b> al <b>" + vFechaRegreso + "</b><br /><br />" +
                    "Total de dias: <b>" + days + "</b> Horas: <b>" + ts.Hours + "</b> Minutos: <b>" + ts.Minutes + "</b><br /><br />" +
                    "Tipo de permiso solicitado: <b>" + DDLTipoPermiso.SelectedItem.Text + "</b><br /><br />" +
                    "¿Estas seguro que estas fechas quieres solicitar?";

                if (DDLTipoPermiso.SelectedValue.Equals("1003"))
                {
                    if (vDias > 3)
                    {
                        LbIncapacidadInformacion.Text = "Es obligatorio que tramites tu subsidio del 66% de incapacidad en el seguro social a la mayor brevedad posible y sea depositado a la cuenta de INFATLAN, caso contrario se te deducirá de tu planilla";
                    }
                    else
                        LbIncapacidadInformacion.Text = String.Empty;
                }
                else
                    LbIncapacidadInformacion.Text = String.Empty;


                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        private void ValidacionesPermisos(String vEmpleado, String vFechaInicio, String vFechaRegreso, String vTipoPermiso)
        {
            if (CbEmergencias.Checked)
            {
                if (DDLEmpleado.SelectedValue.Equals("0"))
                    throw new Exception("Seleccione un empleado valido para el permiso.");
                if (DDLJefe.SelectedValue.Equals("0"))
                    throw new Exception("Seleccione un jefe valido para el permiso.");
                if (DDLTipoPermiso.SelectedValue.Equals("0"))
                    throw new Exception("Seleccione un tipo de permiso valido");
                if (TxFechaInicio.Text.Equals(""))
                    throw new Exception("Ingrese una fecha de inicio valida");
                if (TxFechaRegreso.Text.Equals(""))
                    throw new Exception("Ingrese una fecha de regreso valida");
                if (Convert.ToDateTime(TxFechaRegreso.Text) < Convert.ToDateTime(TxFechaInicio.Text))
                    throw new Exception("Las fechas ingresadas son incorrectas, el regreso es menor que el inicio");

                TimeSpan ts = Convert.ToDateTime(TxFechaInicio.Text) - DateTime.Now;
                if (ts.Days < -15)
                    throw new Exception("No se pueden agregar permisos por incumplimiento de politica (15 dias maximo para ingresar permisos pasados)");
            }
            else
            {
                if (DDLEmpleado.SelectedValue.Equals("0"))
                    throw new Exception("Seleccione un empleado valido para el permiso.");
                if (DDLJefe.SelectedValue.Equals("0"))
                    throw new Exception("Seleccione un jefe valido para el permiso.");
                if (DDLTipoPermiso.SelectedValue.Equals("0"))
                    throw new Exception("Seleccione un tipo de permiso valido");
                if (TxFechaInicio.Text.Equals(""))
                    throw new Exception("Ingrese una fecha de inicio valida");
                if (TxFechaRegreso.Text.Equals(""))
                    throw new Exception("Ingrese una fecha de regreso valida");

                if (Convert.ToDateTime(TxFechaRegreso.Text) < Convert.ToDateTime(TxFechaInicio.Text))
                    throw new Exception("Las fechas ingresadas son incorrectas, el regreso es menor que el inicio");

                
                String vQuery = "RSP_ValidacionesPermisos 1," + vEmpleado + ",'" + vFechaInicio + "','" + vFechaRegreso + "'";
                DataTable vDatosVerificacion = vConexion.obtenerDataTable(vQuery);

                if (vDatosVerificacion.Rows.Count > 0)
                {
                    if (vDatosVerificacion.Rows[0][0].ToString().Equals("1"))
                        throw new Exception("Ya existe un permiso en el tiempo seleccionado");
                }

                if (Convert.ToInt32(vTipoPermiso) < 2000)
                {
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

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarPermiso();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        private void LimpiarPermiso()
        {
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

        protected void BtnEnviarPermiso_Click(object sender, EventArgs e)
        {
            try
            {
                String vNombreDepot1 = String.Empty;
                HttpPostedFile bufferDeposito1T = FUDocumentoPermiso.PostedFile;
                byte[] vFileDeposito1 = null;
                String vExtension = String.Empty;
                if (bufferDeposito1T != null)
                {
                    vNombreDepot1 = FUDocumentoPermiso.FileName;
                    Stream vStream = bufferDeposito1T.InputStream;
                    BinaryReader vReader = new BinaryReader(vStream);
                    vFileDeposito1 = vReader.ReadBytes((int)vStream.Length);
                    vExtension = System.IO.Path.GetExtension(FUDocumentoPermiso.FileName);
                }

                String vArchivo = String.Empty;
                if (vFileDeposito1 != null)
                    vArchivo = Convert.ToBase64String(vFileDeposito1);

                String vQuery = "RSP_IngresarPermisos 1," + DDLEmpleado.SelectedValue + "," +
                    "" + DDLJefe.SelectedValue + "," +
                    "" + DDLTipoPermiso.SelectedValue + "," +
                    "'" + TxMotivo.Text + "'," +
                    "'" + Convert.ToDateTime(TxFechaInicio.Text).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                    "'" + Convert.ToDateTime(TxFechaRegreso.Text).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                    "'" + Convert.ToString(Session["USUARIO"]) + "'," +
                    "" + DDLCompensacion.SelectedValue + "," +
                    "'" + TxFechaCompensacion.Text + "'," +
                    "" + DDLParientes.SelectedValue + "," + 
                    "'" + vArchivo + "'," +
                    "'" + vExtension + "'";
                    

                Int32 vInformacion = vConexion.ejecutarSql(vQuery);

                if (vInformacion == 1)
                {
                    vQuery = "RSP_ObtenerEmpleados 2," + DDLJefe.SelectedValue;
                    DataTable vDatosJefatura = vConexion.obtenerDataTable(vQuery);
                    vQuery = "RSP_ObtenerEmpleados 2," + DDLEmpleado.SelectedValue;
                    DataTable vDatosEmpleado = vConexion.obtenerDataTable(vQuery);

                    SmtpService vService = new SmtpService();
                    Boolean vFlagEnvioSupervisor = false;
                    foreach (DataRow item in vDatosJefatura.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Supervisor,
                                item["nombre"].ToString(),
                                vDatosEmpleado.Rows[0]["nombre"].ToString()
                                );
                            vFlagEnvioSupervisor = true;
                        }
                    }
                    if (vFlagEnvioSupervisor)
                    {
                        foreach (DataRow item in vDatosEmpleado.Rows)
                        {
                            if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                                vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                    typeBody.Solicitante,
                                    item["nombre"].ToString(),
                                    item["nombre"].ToString()
                                    );
                        }
                        Mensaje("Permiso ingresado con exito", WarningType.Success);
                        Response.Redirect("/pages/permissions.aspx?ex=3");
                    }
                    else
                        Mensaje("Permiso ingresado con exito, Fallo envio de correo ponte en contacto con tu Jefe", WarningType.Success);
                }
                else
                {
                    Mensaje("Permiso no se ingreso, contacte a Recursos Humanos", WarningType.Success);
                }
                LimpiarPermiso();
                CargarPermisos();
                CargarDiasSAP();

                CerrarModal("InformativoModal");
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void DDLTipoPermiso_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DIVCompensacion.Visible = false;
                DIVCompensacionFecha.Visible = false;
                DIVDocumentos.Visible = false;
                DIVParientes.Visible = false;

                switch (DDLTipoPermiso.SelectedValue)
                {
                    case "1000":
                        TxFechaInicio.TextMode = TextBoxMode.DateTimeLocal;
                        TxFechaRegreso.TextMode = TextBoxMode.DateTimeLocal;
                        UpdatePanelFechas.Update();
                        break;          
                    case "1001":
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
                        break;
                    case "1007":
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
                        TxFechaInicio.TextMode = TextBoxMode.DateTimeLocal;
                        TxFechaRegreso.TextMode = TextBoxMode.DateTimeLocal;
                        UpdatePanelFechas.Update();
                        DIVDocumentos.Visible = true;
                        break;
                    case "1011":
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

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "MotivoPermiso")
            {
                string vIdPermiso = e.CommandArgument.ToString();

                String vQuery = "RSP_ObtenerPermisos 3," + Session["USUARIO"] + "," + vIdPermiso;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                String vMotivo = "Ningun motivo";
                if (!vDatos.Rows[0]["Motivo"].ToString().Equals(""))
                    vMotivo = vDatos.Rows[0]["Motivo"].ToString();

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "window.alert('" + vMotivo + "')", true);
            }

            if (e.CommandName == "EditarPermiso")
            {
                string vIdPermiso = e.CommandArgument.ToString();
                LbPermisoSubir.Text = vIdPermiso;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openEdicionModal();", true);
            }
            if (e.CommandName == "DocumentoPermiso")
            {
                string vIdPermiso = e.CommandArgument.ToString();


                String vQuery = "RSP_ObtenerPermisos 3," + Session["USUARIO"] + "," + vIdPermiso;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                String vDocumento = "";
                if (!vDatos.Rows[0]["documento"].ToString().Equals(""))
                    vDocumento = vDatos.Rows[0]["documento"].ToString();

                if (!vDocumento.Equals(""))
                {
                    LbPermisoDescarga.Text = vIdPermiso;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openDescargarModal();", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "window.alert('No existe documento en este permiso')", true);
            }

        }

        private string GetExtension(string Extension)
        {
            switch (Extension)
            {
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

        protected void BtnEditarPermiso_Click(object sender, EventArgs e)
        {
            try
            {
                String vNombreDepot1 = String.Empty;
                HttpPostedFile bufferDeposito1T = FUSubirArchivoEdicion.PostedFile;
                byte[] vFileDeposito1 = null;
                String vExtension = String.Empty;
                if (bufferDeposito1T != null)
                {
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

        protected void BtnDescargarArchivo_Click(object sender, EventArgs e)
        {
            try
            {
                string vIdPermiso = LbPermisoDescarga.Text;

                String vQuery = "RSP_ObtenerPermisos 3," + Session["USUARIO"] + "," + vIdPermiso;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                String vDocumento = "";
                if (!vDatos.Rows[0]["documento"].ToString().Equals(""))
                    vDocumento = vDatos.Rows[0]["documento"].ToString();

                if (!vDocumento.Equals(""))
                {
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

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVBusqueda.PageIndex = e.NewPageIndex;
                GVBusqueda.DataSource = (DataTable)Session["DATAOSPERMISOS"];
                GVBusqueda.DataBind();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
    }
}