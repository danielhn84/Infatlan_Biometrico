using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Net;

namespace BiometricoWeb.pages.documentacion
{
    public partial class docReportes : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                    cargarDatos();
                }
            }
        }

        private void cargarDatos() {
            try{
                String vQuery = "[RSP_Documentacion] 1";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                DDLTipoPDoc.Items.Clear();
                DDLTipoPDoc.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLTipoPDoc.Items.Add(new ListItem { Value = item["idTipoDoc"].ToString(), Text = item["nombre"].ToString() });
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void DDLTipoPDoc_SelectedIndexChanged(object sender, EventArgs e){
            try{
                String vQuery = "[RSP_Documentacion] 11," + DDLTipoPDoc.SelectedValue;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                Session["DOCUMENTO_REPORTE"] = vDatos;

                DDLDocumento.Items.Clear();
                DDLDocumento.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLDocumento.Items.Add(new ListItem { Value = item["idDocumento"].ToString(), Text = item["nombre"].ToString() });
                }
            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void BtnCrearReporte_Click(object sender, EventArgs e){
            try{
                validarDatos();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        private void validarDatos() {
            if (DDLTipoPDoc.SelectedValue == "0")
                throw new Exception("Favor seleccione un tipo de documento.");            
            if (DDLDocumento.SelectedValue == "0")
                throw new Exception("Favor seleccione un documento.");
        }

        private void limpiarDatos(){
           DDLTipoPDoc.SelectedIndex = 0;       
           DDLDocumento.SelectedIndex = 0;
        }

        protected void BtnConfirmar_Click(object sender, EventArgs e){
            try{
                DataTable vDatos = (DataTable)Session["DOCUMENTO_REPORTE"];
                string Parametro1 = vDatos.Rows[0]["idCategoria"].ToString(); 
                string Parametro2 = DDLDocumento.SelectedValue;

                ReportExecutionService.ReportExecutionService vRSE = new ReportExecutionService.ReportExecutionService();
                vRSE.Credentials = new NetworkCredential("report_user", "kEbn2HUzd$Fs2T", "adbancat.hn");
                vRSE.Url = "http://10.128.0.52/reportserver/reportexecution2005.asmx";

                vRSE.ExecutionHeaderValue = new ReportExecutionService.ExecutionHeader();
                var vEInfo = new ReportExecutionService.ExecutionInfo();
                vEInfo = vRSE.LoadReport("/Recursos Humanos Interno/GestionDocumentalConsultas", null);

                List<ReportExecutionService.ParameterValue> vParametros = new List<ReportExecutionService.ParameterValue>();
                vParametros.Add(new ReportExecutionService.ParameterValue { Name = "D1", Value = Parametro1 });
                vParametros.Add(new ReportExecutionService.ParameterValue { Name = "D2", Value = Parametro2 });

                vRSE.SetExecutionParameters(vParametros.ToArray(), "en-US");
                String deviceinfo = "<DeviceInfo><Toolbar>false</Toolbar></DeviceInfo>";
                String mime;
                String encoding;
                string[] stream;
                ReportExecutionService.Warning[] warning;

                byte[] vResultado = vRSE.Render("EXCEL", deviceinfo, out mime, out encoding, out encoding, out warning, out stream);


                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.AppendHeader("Content-Type", "application/vnd.ms-excel");
                byte[] bytFile = vResultado;
                Response.OutputStream.Write(bytFile, 0, bytFile.Length);
                Response.AddHeader("Content-disposition", "attachment;filename=Reporte.xls");
                Response.End();

            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void BtnCancelarModal_Click(object sender, EventArgs e){
            try{
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);
                limpiarDatos();
            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }
    }
}