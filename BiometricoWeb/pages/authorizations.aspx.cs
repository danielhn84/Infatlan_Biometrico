﻿using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace BiometricoWeb.pages
{
    public partial class authorizations : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    CargarAutorizaciones();

                    if (GVBusqueda.Rows.Count > 0){
                        GVBusqueda.UseAccessibleHeader = true;
                        GVBusqueda.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }
                }
            }
        }
        
        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        public void CerrarModal(String vModal)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#" + vModal + "').modal('hide');", true);
        }

        void CargarAutorizaciones(){
            try{
                String vQuery = "RSP_ObtenerPermisos 1," + Session["USUARIO"];
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                //begin wpadilla
                vDatos.Columns.Add("Detalle");

                for (int i = 0; i < vDatos.Rows.Count; i++){
                    DateTime desde = Convert.ToDateTime(vDatos.Rows[i]["FechaInicio"]);
                    DateTime hasta = Convert.ToDateTime(vDatos.Rows[i]["FechaRegreso"]);

                    DateTime inicio = desde;
                    int dias = 0;

                    while (inicio <= hasta){
                        if (inicio.DayOfWeek != DayOfWeek.Saturday && inicio.DayOfWeek != DayOfWeek.Sunday)
                            dias++;

                        inicio = inicio.AddDays(1);
                    }

                    TimeSpan ts = Convert.ToDateTime(hasta) - Convert.ToDateTime(desde);
                    int days = 1;
                    if (ts.Days >= 1)
                        days = dias; //ts.Days + 1 - 
                    else if (ts.Hours > 0 || ts.Minutes > 0)
                        days = 0;
                    
                    vDatos.Rows[i]["Detalle"] = days + " días, " + ts.Hours + " horas, " + ts.Minutes + " minutos";
                }
                //end wpadilla

                GVBusqueda.DataSource = vDatos;
                GVBusqueda.DataBind();
                drawActions();
                DataTable vDatosLogin = (DataTable)Session["AUTHCLASS"];
                if (!vDatosLogin.Rows[0]["tipoEmpleado"].ToString().Equals("")){
                    if (vDatosLogin.Rows[0]["tipoEmpleado"].ToString().Equals("1")){
                        GVBusqueda.Columns[0].Visible = true;
                    }
                }

                Session["DATOSAUTORIZAR"] = vDatos;
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                if (e.CommandName == "AutorizarEmpleado"){
                    string vIdPermiso = e.CommandArgument.ToString();
                    LbNumeroPermiso.Text = vIdPermiso;
                    UpdateLabelPermiso.Update();
                    DDLOpciones.SelectedValue = "1";
                    DivMotivoJefe.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }

                if (e.CommandName == "AutorizarEmpleadoRecursosHumanos"){
                    String vQuery = "RSP_ObtenerPermisos 3," + Session["USUARIO"] + "," + e.CommandArgument.ToString();
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                    if (!Convert.ToBoolean(vDatos.Rows[0]["Autorizado"].ToString()))
                        throw new Exception("Este permiso no ha sido autorizado por el jefe inmediato.");

                    string vIdPermiso = e.CommandArgument.ToString();
                    LbFinalizarPermiso.Text = vIdPermiso;
                    UpdatePanel1.Update();
                    DDlFinalizarPermiso.SelectedValue = "1";
                    DivMotivo.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openFinalizarModal();", true);
                }
                
                if (e.CommandName == "MotivoPermiso"){
                    string vIdPermiso = e.CommandArgument.ToString();

                    String vQuery = "RSP_ObtenerPermisos 3," + Session["USUARIO"] + "," + vIdPermiso;
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                    String vMotivo = "Ningun motivo";
                    if (!vDatos.Rows[0]["Motivo"].ToString().Equals(""))
                        vMotivo = vDatos.Rows[0]["Motivo"].ToString();

                    String vMensaje = String.Empty;
                    vMensaje += "Motivo: " + vMotivo + "\\n";
                    vMensaje += "Fecha Solicitud: " + vDatos.Rows[0]["FechaSolicitud"].ToString() + "\\n";

                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "window.alert('" + vMensaje + "')", true);
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
                    }
                    else
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "window.alert('No existe documento en este permiso')", true);
                }
                CargarAutorizaciones();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GVBusqueda.PageIndex = e.NewPageIndex;
                GVBusqueda.DataSource = (DataTable)Session["DATOSAUTORIZAR"];
                GVBusqueda.DataBind();
                drawActions();
                
                DataTable vDatosLogin = (DataTable)Session["AUTHCLASS"];
                if (!vDatosLogin.Rows[0]["tipoEmpleado"].ToString().Equals("")){
                    if (vDatosLogin.Rows[0]["tipoEmpleado"].ToString().Equals("1")){
                        GVBusqueda.Columns[0].Visible = true;
                    }
                }
            }catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnPermisos_Click(object sender, EventArgs e){
            try{
                Response.Redirect("/pages/permissions.aspx");
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnAutorizarPermiso_Click(object sender, EventArgs e){
            try{
                SmtpService vService = new SmtpService();
                String vQuery = "RSP_ObtenerPermisos 2," 
                    + Session["USUARIO"] + "," 
                    + LbNumeroPermiso.Text + "," 
                    + DDLOpciones.SelectedValue + ",'" + TxMotivoJefe.Text + "'";
                int vDatos = vConexion.ejecutarSql(vQuery);

                if (vDatos.Equals(1)){
                    if (DDLOpciones.SelectedValue.Equals("1")){
                        vQuery = "RSP_ObtenerPermisos 3," + Session["USUARIO"] + "," + LbNumeroPermiso.Text;
                        DataTable vDatosBusqueda = vConexion.obtenerDataTable(vQuery);

                        foreach (DataRow item in vDatosBusqueda.Rows){
                            vService.EnviarMensaje(ConfigurationManager.AppSettings["RHMail"],
                                    typeBody.RecursosHumanos,
                                    "Recursos Humanos",
                                    item["Empleado"].ToString()
                                    );

                            vQuery = "RSP_ObtenerGenerales 8,'" + item["EmpleadoCodigo"].ToString() + "'";
                            DataTable vDatosEmpleado = vConexion.obtenerDataTable(vQuery);
                            foreach (DataRow itemEmpleado in vDatosEmpleado.Rows){
                                vService.EnviarMensaje(itemEmpleado["correo"].ToString(),
                                    typeBody.Aprobado,
                                    itemEmpleado["nombre"].ToString(),
                                    ""
                                    );
                            }
                        }

                        Mensaje("El empleado ha sido autorizado", WarningType.Success);
                        CerrarModal("AutorizarModal");

                    }else{
                        if (TxMotivoJefe.Text != "" || TxMotivoJefe.Text != string.Empty){
                            vQuery = "RSP_ObtenerPermisos 7,''," + LbNumeroPermiso.Text + ",0,'" + TxMotivoJefe.Text + "'";
                            vDatos = vConexion.ejecutarSql(vQuery);

                            vQuery = "RSP_ObtenerPermisos 3," + Session["USUARIO"] + "," + LbNumeroPermiso.Text;
                            DataTable vDatosBusqueda = vConexion.obtenerDataTable(vQuery);

                            foreach (DataRow item in vDatosBusqueda.Rows){
                                vQuery = "RSP_ObtenerGenerales 8,'" + item["EmpleadoCodigo"].ToString() + "'";
                                DataTable vDatosEmpleado = vConexion.obtenerDataTable(vQuery);
                                foreach (DataRow itemEmpleado in vDatosEmpleado.Rows){
                                    vService.EnviarMensaje(itemEmpleado["correo"].ToString(),
                                        typeBody.Rechazado,
                                        itemEmpleado["nombre"].ToString(),
                                        "Razón de Cancelación: " + TxMotivo.Text
                                        );
                                }

                                // DEVOLVER TIEMPO COMPENSATORIO
                                if (vDatosBusqueda.Rows[0]["TipoPermiso"].ToString() == "DÍAS/HORAS COMPENSATORIOS"){
                                    TimeSpan tsHorario = Convert.ToDateTime(item["FechaRegreso"]) - Convert.ToDateTime(item["FechaInicio"]);
                                    Decimal vDiasHoras = tsHorario.Hours + (Convert.ToDecimal(tsHorario.Minutes) / 60);
                                    String vCalculo = vDiasHoras.ToString().Contains(",") ? vDiasHoras.ToString().Replace(",", ".") : vDiasHoras.ToString();

                                    vQuery = "[RSP_Compensatorio] 1,'" + item["CodigoSAP"].ToString() + "', 2,NULL,'" + Session["USUARIO"].ToString() + "',NULL," + vCalculo + "," + item["idPermiso"].ToString();
                                    int vInfo = vConexion.ejecutarSql(vQuery);
                                }
                            }

                            Mensaje("El empleado no ha sido autorizado", WarningType.Success);
                            CerrarModal("AutorizarModal");
                        }else
                            LbAutorizarMensaje.Text = "Favor ingresar el motivo de cancelación.";
                    }
                }
                CargarAutorizaciones();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void TxBuscarEmpleado_TextChanged(object sender, EventArgs e){
            try{
                CargarAutorizaciones();
                String vBusqueda = TxBuscarEmpleado.Text;
                DataTable vDatos = (DataTable)Session["DATOSAUTORIZAR"];

                if (vBusqueda.Equals("")){
                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                    drawActions();
                    UpdateGridView.Update();
                }else{
                    EnumerableRowCollection<DataRow> filtered = vDatos.AsEnumerable()
                        .Where(r => r.Field<String>("Empleado").Contains(vBusqueda.ToUpper()));

                    DataTable vDatosFiltrados = new DataTable();
                    vDatosFiltrados.Columns.Add("idPermiso");
                    vDatosFiltrados.Columns.Add("Empleado");
                    vDatosFiltrados.Columns.Add("TipoPermiso");
                    vDatosFiltrados.Columns.Add("FechaInicio");
                    vDatosFiltrados.Columns.Add("FechaRegreso");
                    vDatosFiltrados.Columns.Add("FechaSolicitud");
                    vDatosFiltrados.Columns.Add("Autorizado");
                    vDatosFiltrados.Columns.Add("AutorizadoSAP");
                    vDatosFiltrados.Columns.Add("Detalle");

                    foreach (DataRow item in filtered){
                        vDatosFiltrados.Rows.Add(
                            item["idPermiso"].ToString(),
                            item["Empleado"].ToString(),
                            item["TipoPermiso"].ToString(),
                            item["FechaInicio"].ToString(),
                            item["FechaRegreso"].ToString(),
                            item["FechaSolicitud"].ToString(),
                            item["Autorizado"].ToString(),
                            item["AutorizadoSAP"].ToString(),
                            item["Detalle"].ToString()
                            );
                    }
                    GVBusqueda.DataSource = vDatosFiltrados;
                    GVBusqueda.DataBind();
                    Session["DATOSAUTORIZAR"] = vDatosFiltrados;
                    drawActions();
                    UpdateGridView.Update();
                }
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        protected void BtnFinalizarPermiso_Click(object sender, EventArgs e){
            try{
                if (DDlFinalizarPermiso.SelectedValue.Equals("1")){
                    String vQuery = "RSP_ObtenerPermisos 3," + Session["USUARIO"] + "," + LbFinalizarPermiso.Text;
                    DataTable vDatosPermiso = vConexion.obtenerDataTable(vQuery);

                    String vResponse = String.Empty;
                    String vMensaje = String.Empty;
                    foreach (DataRow item in vDatosPermiso.Rows){
                        SapConnector vConnector = new SapConnector();
                        vResponse = vConnector.postPermiso(
                            Convert.ToDateTime(item["fechaInicio"].ToString()),
                            Convert.ToDateTime(item["fechaRegreso"].ToString()),
                            item["CodigoSAP"].ToString(),
                            item["TipoPermisoCodigo"].ToString(),
                            item["SubTipoPermiso"].ToString(),
                            item["Motivo"].ToString(),
                            ref vMensaje
                            );
                    }

                    switch (vResponse){
                        case "":
                            Mensaje("Error al enviar a SAP", WarningType.Success);
                            break;
                        case "0":

                            vQuery = "RSP_ObtenerPermisos 4,"
                                + Session["USUARIO"] + ","
                                + LbFinalizarPermiso.Text + ",1";
                            int vDatos = vConexion.ejecutarSql(vQuery);

                            if (vDatos.Equals(1))
                                Mensaje("El empleado ha sido autorizado en SAP", WarningType.Success);
                            else
                                Mensaje("El empleado ha sido autorizado en SAP, pero no inserto la verificación", WarningType.Success);
                            
                            break;
                        case "1":
                            Mensaje(vMensaje, WarningType.Success);
                            break;
                    }

                    CerrarModal("FinalizarModal");
                    CargarAutorizaciones();
                }else{

                    if (TxMotivo.Text == string.Empty || TxMotivo.Text == "")
                        Label2.Text = "Favor ingresar el motivo de cancelación.";
                    else { 

                        String vQuery = "RSP_ObtenerPermisos 4,"
                                    + Session["USUARIO"] + ","
                                    + LbFinalizarPermiso.Text + ",0,'" + TxMotivo.Text + "'";
                        int vDatos = vConexion.ejecutarSql(vQuery);

                        if (vDatos.Equals(1)){

                            String vQueryCancelacion = "RSP_ObtenerPermisos 2,"
                                + Session["USUARIO"] + ","
                                + LbFinalizarPermiso.Text + ","
                                + 0;
                            int vDatosCancelacion = vConexion.ejecutarSql(vQueryCancelacion);

                            vQuery = "RSP_ObtenerPermisos 3," + Session["USUARIO"] + "," + LbFinalizarPermiso.Text;
                            DataTable vDatosBusqueda = vConexion.obtenerDataTable(vQuery);

                            foreach (DataRow itemEmpleado in vDatosBusqueda.Rows){
                                SmtpService vService = new SmtpService();
                                vService.EnviarMensaje(itemEmpleado["correo"].ToString(),
                                    typeBody.Rechazado,
                                    itemEmpleado["nombre"].ToString(),
                                    "Razón de Cancelación: " + TxMotivo.Text
                                    );

                                if (vDatosBusqueda.Rows[0]["TipoPermiso"].ToString() == "DÍAS/HORAS COMPENSATORIOS"){
                                    Boolean vDia = itemEmpleado["FechaRegreso"].ToString() == itemEmpleado["FechaInicio"].ToString() ? true : false ;
                                    TimeSpan tsHorario = Convert.ToDateTime(itemEmpleado["FechaRegreso"]) - Convert.ToDateTime(itemEmpleado["FechaInicio"]);
                                    Decimal vDiasHoras = tsHorario.Hours + (Convert.ToDecimal(tsHorario.Minutes) / 60);
                                    vDiasHoras = vDia ? 8 : vDiasHoras;
                                    String vCalculo = vDiasHoras.ToString().Contains(",") ? vDiasHoras.ToString().Replace(",", ".") : vDiasHoras.ToString();

                                    String vEmpleado = "";
                                    if (itemEmpleado["idEmpleadoJefe"].ToString() == itemEmpleado["usuarioCreacion"].ToString())
                                        vEmpleado = itemEmpleado["idEmpleadoJefe"].ToString();
                                    else
                                        vEmpleado = itemEmpleado["CodigoSAP"].ToString();

                                    vQuery = "[RSP_Compensatorio] 1,'" + vEmpleado + "', 2,NULL,'" + Session["USUARIO"].ToString() + "',NULL," + vCalculo + "," + itemEmpleado["idPermiso"].ToString();
                                    int vInfo = vConexion.ejecutarSql(vQuery);
                                }
                            }

                            Mensaje("Se ha cancelado el permiso", WarningType.Success);
                        }else{
                            Mensaje("No se ha podido cancelar el servicio en el sistema, contacte a sistemas", WarningType.Success);
                        }

                        CerrarModal("FinalizarModal");
                        CargarAutorizaciones();
                    }
                }
                
            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
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

        protected void DDlFinalizarPermiso_SelectedIndexChanged(object sender, EventArgs e){
            DivMotivo.Visible = DDlFinalizarPermiso.SelectedValue == "0" ? true : false;
            Label2.Text = string.Empty;
            TxMotivo.Text = string.Empty;
        }

        protected void DDLOpciones_SelectedIndexChanged(object sender, EventArgs e){
            DivMotivoJefe.Visible = DDLOpciones.SelectedValue == "0" ? true : false;
            LbAutorizarMensaje.Text = string.Empty;
            TxMotivoJefe.Text = string.Empty;
        }

        void drawActions() {
            try{
                foreach (GridViewRow row in GVBusqueda.Rows){
                    String vQuery = "RSP_ObtenerPermisos 3," + Session["USUARIO"] + "," + row.Cells[4].Text;
                    DataTable vDatosBusqueda = vConexion.obtenerDataTable(vQuery);

                    foreach (DataRow item in vDatosBusqueda.Rows){
                        if (item["Autorizado"].ToString().Equals("True")){
                            Button button = row.FindControl("BtnAutorizar") as Button;
                            button.Text = "Listo";
                            button.CssClass = "btn btn-inverse-success mr-2 ";
                            button.Enabled = false;
                            button.CommandName = "Cerrado";
                        }
                        
                        if (item["autorizadoSAP"].ToString().Equals("True")){
                            Button button = row.FindControl("BtnAutorizarRecursosHumanos") as Button;
                            button.Text = "Listo";
                            button.CssClass = "btn btn-inverse-success mr-2 ";
                            button.Enabled = false;
                            button.CommandName = "Cerrado";
                        }
                    }
                }
            }catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }

        private Decimal Calculo(DataTable vDatosSIM) {
            Decimal vRes;
            DateTime desde, hasta;
            desde = Convert.ToDateTime(vDatosSIM.Rows[0]["FechaInicio"]);
            hasta = Convert.ToDateTime(vDatosSIM.Rows[0]["FechaRegreso"]);
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
    }
}