using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.IO;
using System.Data.Sql;
using System.Data.SqlClient;

namespace BiometricoWeb.pages.mantenimiento
{
    public partial class reportes : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["CARGAR_DATA_REPORTES"] = null;
            if (!Page.IsPostBack)
            {
                if (Convert.ToBoolean(Session["AUTH"])){
                    cargarData();
                }else{
                    Response.Redirect("/login.aspx");
                }
            }
        }
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        void cargarData()
        {
          if (HttpContext.Current.Session["CARGAR_DATA_REPORTES"] == null)
          {
            
            //DEPARTAMENTOS
            DDLReporteDepto.Items.Clear();
            String vQuery = "VIATICOS_ObtenerGenerales 45";
            DataTable vDatos = vConexion.obtenerDataTable(vQuery);
            DDLReporteDepto.Items.Add(new ListItem { Value = "0", Text = "Seleccione departamento..." });
            foreach (DataRow item in vDatos.Rows)
            {
                DDLReporteDepto.Items.Add(new ListItem { Value = item["idDepartamento"].ToString(), Text = item["nombre"].ToString() });
            }

            //EMPLEADO
            DDLReporteEmpleado.Items.Clear();
            String vQuery2 = "VIATICOS_ObtenerGenerales 42";
            DataTable vDatos2 = vConexion.obtenerDataTable(vQuery2);
            DDLReporteEmpleado.Items.Add(new ListItem { Value = "0", Text = "Seleccione empleado..." });
            foreach (DataRow item in vDatos2.Rows)
            {
                DDLReporteEmpleado.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
            }

            //MOTIVO DE VIAJE
            DDLMotivoViaje.Items.Clear();
            String vQuery3 = "VIATICOS_ObtenerGenerales 4";
            DataTable vDatos3 = vConexion.obtenerDataTable(vQuery3);
            DDLMotivoViaje.Items.Add(new ListItem { Value = "0", Text = "Seleccione motivo de viaje..." });
            foreach (DataRow item in vDatos3.Rows)
            {
                DDLMotivoViaje.Items.Add(new ListItem { Value = item["idMotivoViaje"].ToString(), Text = item["nombre"].ToString() });
            }
             Session["CARGAR_DATA_REPORTES"] = "1";
          }
        }
        protected void btnReporteDeptos_Click(object sender, EventArgs e)
        {
            if (DDLReporteDepto.SelectedValue == "0")
            {
                DDLReporteDepto.SelectedValue = "0";
                Mensaje("Debe seleccionar el departamento al que generará reporte", WarningType.Warning);
            }
            else
            {
                String vError = String.Empty;
                try
                {
                    ReportExecutionService.ReportExecutionService vRSE = new ReportExecutionService.ReportExecutionService();
                    vRSE.Credentials = new NetworkCredential("report_user", "kEbn2HUzd$Fs2T", "adbancat.hn");
                    vRSE.Url = "http://10.128.0.52/reportserver/reportexecution2005.asmx";



                    vRSE.ExecutionHeaderValue = new ReportExecutionService.ExecutionHeader();
                    var vEInfo = new ReportExecutionService.ExecutionInfo();
                    vEInfo = vRSE.LoadReport("/Recursos Humanos/viaticosArea", null);


                    String vDepto = DDLReporteDepto.SelectedItem.Text;
                    List<ReportExecutionService.ParameterValue> vParametros = new List<ReportExecutionService.ParameterValue>();
                    vParametros.Add(new ReportExecutionService.ParameterValue { Name = "Area", Value = vDepto });



                    vRSE.SetExecutionParameters(vParametros.ToArray(), "en-US");



                    String deviceinfo = "<DeviceInfo><Toolbar>false</Toolbar></DeviceInfo>";
                    String mime;
                    String encoding;
                    string[] stream;
                    ReportExecutionService.Warning[] warning;



                    //byte[] vResultado = vRSE.Render("EXCEL", deviceinfo, out mime, out encoding, out encoding, out warning, out stream);
                    byte[] vResultado = vRSE.Render("pdf", deviceinfo, out mime, out encoding, out encoding, out warning, out stream);

                    //File.WriteAllBytes("c:\\files\\test.pdf", vResultado);

                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //Response.AppendHeader("Content-Type", "application/vnd.ms-excel");
                    Response.AppendHeader("Content-Type", "application/pdf");
                    byte[] bytFile = vResultado;
                    Response.OutputStream.Write(bytFile, 0, bytFile.Length);
                    //Response.AddHeader("Content-disposition", "attachment;filename=DescargaPDFTest.xls");
                    Response.AddHeader("Content-disposition", "attachment;filename=DescargaPDFViaticosArea.pdf");
                    Response.End();
                }
                catch (Exception Ex) { vError = Ex.Message; }
                DDLReporteDepto.SelectedValue = "0";
            }
        }

        protected void btnReporteEmpleados_Click(object sender, EventArgs e)
        {
            if (DDLReporteEmpleado.SelectedValue == "0")
            {
                DDLReporteEmpleado.SelectedValue = "0";
                Mensaje("Debe seleccionar empleado al que generará reporte", WarningType.Warning);
            }
            else
            {
                String vError = String.Empty;
                try
                {
                    ReportExecutionService.ReportExecutionService vRSE = new ReportExecutionService.ReportExecutionService();
                    vRSE.Credentials = new NetworkCredential("report_user", "kEbn2HUzd$Fs2T", "adbancat.hn");
                    vRSE.Url = "http://10.128.0.52/reportserver/reportexecution2005.asmx";



                    vRSE.ExecutionHeaderValue = new ReportExecutionService.ExecutionHeader();
                    var vEInfo = new ReportExecutionService.ExecutionInfo();
                    vEInfo = vRSE.LoadReport("/Recursos Humanos/viaticosEmpleado", null);


                    String vEmpleado = DDLReporteEmpleado.SelectedValue;
                    List<ReportExecutionService.ParameterValue> vParametros = new List<ReportExecutionService.ParameterValue>();
                    vParametros.Add(new ReportExecutionService.ParameterValue { Name = "Empleado", Value = vEmpleado });



                    vRSE.SetExecutionParameters(vParametros.ToArray(), "en-US");



                    String deviceinfo = "<DeviceInfo><Toolbar>false</Toolbar></DeviceInfo>";
                    String mime;
                    String encoding;
                    string[] stream;
                    ReportExecutionService.Warning[] warning;



                    //byte[] vResultado = vRSE.Render("EXCEL", deviceinfo, out mime, out encoding, out encoding, out warning, out stream);
                    byte[] vResultado = vRSE.Render("pdf", deviceinfo, out mime, out encoding, out encoding, out warning, out stream);

                    //File.WriteAllBytes("c:\\files\\test.pdf", vResultado);

                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //Response.AppendHeader("Content-Type", "application/vnd.ms-excel");
                    Response.AppendHeader("Content-Type", "application/pdf");
                    byte[] bytFile = vResultado;
                    Response.OutputStream.Write(bytFile, 0, bytFile.Length);
                    //Response.AddHeader("Content-disposition", "attachment;filename=DescargaPDFTest.xls");
                    Response.AddHeader("Content-disposition", "attachment;filename=DescargaPDFViaticosEmpleado.pdf");
                    Response.End();
                }
                catch (Exception Ex) { vError = Ex.Message; }
                DDLReporteEmpleado.SelectedValue = "0";
            }
        }

        protected void btnReporteMotivoViaje_Click(object sender, EventArgs e)
        {
            if (DDLMotivoViaje.SelectedValue == "0")
            {
                DDLMotivoViaje.SelectedValue = "0";
                Mensaje("Debe seleccionar motivo de viaje al que generará reporte", WarningType.Warning);
            }
            else
            {
                String vError = String.Empty;
                try
                {
                    ReportExecutionService.ReportExecutionService vRSE = new ReportExecutionService.ReportExecutionService();
                    vRSE.Credentials = new NetworkCredential("report_user", "kEbn2HUzd$Fs2T", "adbancat.hn");
                    vRSE.Url = "http://10.128.0.52/reportserver/reportexecution2005.asmx";



                    vRSE.ExecutionHeaderValue = new ReportExecutionService.ExecutionHeader();
                    var vEInfo = new ReportExecutionService.ExecutionInfo();
                    vEInfo = vRSE.LoadReport("/Recursos Humanos/viaticosMotivoViaje", null);


                    String vMotivo = DDLMotivoViaje.SelectedValue;
                    List<ReportExecutionService.ParameterValue> vParametros = new List<ReportExecutionService.ParameterValue>();
                    vParametros.Add(new ReportExecutionService.ParameterValue { Name = "Motivo", Value = vMotivo });



                    vRSE.SetExecutionParameters(vParametros.ToArray(), "en-US");



                    String deviceinfo = "<DeviceInfo><Toolbar>false</Toolbar></DeviceInfo>";
                    String mime;
                    String encoding;
                    string[] stream;
                    ReportExecutionService.Warning[] warning;



                    //byte[] vResultado = vRSE.Render("EXCEL", deviceinfo, out mime, out encoding, out encoding, out warning, out stream);
                    byte[] vResultado = vRSE.Render("pdf", deviceinfo, out mime, out encoding, out encoding, out warning, out stream);

                    //File.WriteAllBytes("c:\\files\\test.pdf", vResultado);

                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //Response.AppendHeader("Content-Type", "application/vnd.ms-excel");
                    Response.AppendHeader("Content-Type", "application/pdf");
                    byte[] bytFile = vResultado;
                    Response.OutputStream.Write(bytFile, 0, bytFile.Length);
                    //Response.AddHeader("Content-disposition", "attachment;filename=DescargaPDFTest.xls");
                    Response.AddHeader("Content-disposition", "attachment;filename=DescargaPDFMotivoViaje.pdf");
                    Response.End();
                }
                catch (Exception Ex) { vError = Ex.Message; }
                DDLMotivoViaje.SelectedValue = "0";
            }
        }
    }
}