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
using System.Net;

namespace BiometricoWeb.pages.tiempoExtraordinario
{
    public partial class reportes : System.Web.UI.Page
    {
        db vConexion;
        protected void Page_Load(object sender, EventArgs e)
        {
            vConexion = new db();

            if (!Page.IsPostBack)
            {

            }
        }        
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        //protected void DdlMes_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string vFechaReporte = "";
        //    String vFormato = "dd/MM/yyyy";
        //    DateTime moment = DateTime.Now;
        //    int year = moment.Year;
        //    DateTime vEnero = DateTime.Now;
        //    DateTime vFebrero = DateTime.Now;
        //    DateTime vMarzo = DateTime.Now;
        //    DateTime vAbril = DateTime.Now;
        //    DateTime vMayo = DateTime.Now;
        //    DateTime vJunio = DateTime.Now;
        //    DateTime vJulio = DateTime.Now;
        //    DateTime vAgosto = DateTime.Now;
        //    DateTime vSeptiembre = DateTime.Now;
        //    DateTime vOctubre = DateTime.Now;
        //    DateTime vNoviembre = DateTime.Now;
        //    DateTime vDiciembe = DateTime.Now;

        //    if (DdlMes.SelectedValue.Equals("1"))
        //    {
        //        vEnero = new DateTime(year, 1, 1, 00, 00, 00);
        //        vFechaReporte = Convert.ToDateTime(vEnero).ToString(vFormato);
        //    }
        //    else if (DdlMes.SelectedValue.Equals("2"))
        //    {
        //        vFebrero = new DateTime(year, 1, 2, 00, 00, 00);
        //        vFechaReporte = Convert.ToDateTime(vFebrero).ToString(vFormato);
        //    }
        //    else if (DdlMes.SelectedValue.Equals("3"))
        //    {
        //        vMarzo = new DateTime(year, 1, 3, 00, 00, 00);
        //        vFechaReporte = Convert.ToDateTime(vMarzo).ToString(vFormato);
        //    }
        //    else if (DdlMes.SelectedValue.Equals("4"))
        //    {
        //        vAbril = new DateTime(year, 1, 4, 00, 00, 00);
        //        vFechaReporte = Convert.ToDateTime(vAbril).ToString(vFormato);
        //    }
        //    else if (DdlMes.SelectedValue.Equals("5"))
        //    {
        //        vMayo = new DateTime(year, 1, 5, 00, 00, 00);
        //        vFechaReporte = Convert.ToDateTime(vMayo).ToString(vFormato);
        //    }
        //    else if (DdlMes.SelectedValue.Equals("6"))
        //    {
        //        vJunio = new DateTime(year, 1, 6, 00, 00, 00);
        //        vFechaReporte = Convert.ToDateTime(vJunio).ToString(vFormato);
        //    }
        //    else if (DdlMes.SelectedValue.Equals("7"))
        //    {
        //        vJulio = new DateTime(year, 1, 7, 00, 00, 00);
        //        vFechaReporte = Convert.ToDateTime(vJulio).ToString(vFormato);
        //    }
        //    else if (DdlMes.SelectedValue.Equals("8"))
        //    {
        //        vAgosto = new DateTime(year, 1, 8, 00, 00, 00);
        //        vFechaReporte = Convert.ToDateTime(vAgosto).ToString(vFormato);
        //    }
        //    else if (DdlMes.SelectedValue.Equals("9"))
        //    {
        //        vSeptiembre = new DateTime(year, 1, 9, 00, 00, 00);
        //        vFechaReporte = Convert.ToDateTime(vSeptiembre).ToString(vFormato);
        //    }
        //    else if (DdlMes.SelectedValue.Equals("10"))
        //    {
        //        vOctubre = new DateTime(year, 1, 10, 00, 00, 00);
        //        vFechaReporte = Convert.ToDateTime(vOctubre).ToString(vFormato);
        //    }
        //    else if (DdlMes.SelectedValue.Equals("11"))
        //    {
        //        vNoviembre = new DateTime(year, 1, 11, 00, 00, 00);
        //        vFechaReporte = Convert.ToDateTime(vNoviembre).ToString(vFormato);
        //    }
        //    else if (DdlMes.SelectedValue.Equals("12"))
        //    {
        //        vDiciembe = new DateTime(year, 1, 12, 00, 00, 00);
        //        vFechaReporte = Convert.ToDateTime(vDiciembe).ToString(vFormato);
        //    }

        //    Session["STEFECHAREPORTE"] = vFechaReporte;

        //}
        void validaciones(){
            
            if (DdlReporte.SelectedValue.Equals("0"))
                throw new Exception("Falta que seleccione el tipo de reporte que desea descargar.");

            if (DdlReporte.SelectedValue.Equals("1") && DdlMes.SelectedValue.Equals("0"))
                throw new Exception("Falta que seleccione el mes del cual desea descargar el reporte.");

            if (DdlReporte.SelectedValue.Equals("1") && DdlQuincena.SelectedValue.Equals("0"))
                throw new Exception("Falta que seleccione de que quincena desea descargar el reporte.");

            if (DdlReporte.SelectedValue.Equals("2") && DdlFactBanco.SelectedValue.Equals("0"))
                throw new Exception("Falta que seleccione el mes del cual desea descargar el reporte de Facturacion al Banco.");

            if (DdlReporte.SelectedValue.Equals("3") && DdlMes.SelectedValue.Equals("0"))
                throw new Exception("Falta que seleccione el mes del cual desea enviar el consolidado.");

            if (DdlReporte.SelectedValue.Equals("3") && DdlQuincena.SelectedValue.Equals("0"))
                throw new Exception("Falta que seleccione de que quincena desea enviar el consolidado.");

            String vFormato = "MM/dd/yyyy";
            DateTime moment = DateTime.Now;
            int year = moment.Year;
            int mes = moment.Month;

            int mesSeleccionado = Convert.ToInt32(DdlMes.SelectedValue);
            int siguienteMes = mesSeleccionado + 1;
            DateTime vInicioSiguienteQuincena = DateTime.Now;
            DateTime vFinSiguienteQuincena = DateTime.Now;
            DateTime vInicioQuincenaActual = DateTime.Now;

            string Parametro1 = "";
            string Parametro2 = "";
            string Parametro3 = "";
            string Parametro4 = "";
            //Parametro2 = Convert.ToDateTime(moment).ToString(vFormato);

            if (DdlReporte.SelectedValue.Equals("1") || DdlReporte.SelectedValue.Equals("3"))
            {
                DateTime vMaxAprobacion1 = DateTime.Now;
                DateTime vMaxAprobacion2 = DateTime.Now;

                if (DdlQuincena.SelectedValue.Equals("1"))
                {
                    vMaxAprobacion1 = new DateTime(year, mesSeleccionado, 10, 00, 00, 00);
                    vMaxAprobacion2 = new DateTime(year, mesSeleccionado, 22, 00, 00, 00);

                    vInicioQuincenaActual = new DateTime(year, mesSeleccionado, 1, 00, 00, 00);
                    //vFinSiguienteQuincena = vInicioQuincenaActual.AddMonths(1).AddDays(-1);
                    vInicioSiguienteQuincena = new DateTime(year, mesSeleccionado, 16, 00, 00, 00);

                }
                else
                {
                    vMaxAprobacion1 = new DateTime(year, mesSeleccionado, 22, 00, 00, 00);
                    vMaxAprobacion2 = new DateTime(year, siguienteMes, 10, 00, 00, 00);

                    vInicioQuincenaActual = new DateTime(year, mesSeleccionado, 16, 00, 00, 00);
                    //vFinSiguienteQuincena = vInicioQuincenaActual.AddDays(29);
                    vInicioSiguienteQuincena = new DateTime(year, siguienteMes, 1, 00, 00, 00);

                }
                Parametro1 = Convert.ToDateTime(vInicioSiguienteQuincena).ToString(vFormato);
                Parametro2 = Convert.ToDateTime(vMaxAprobacion1).ToString(vFormato);
                Parametro3 = Convert.ToDateTime(vInicioQuincenaActual).ToString(vFormato);
                Parametro4 = Convert.ToDateTime(vMaxAprobacion2).ToString(vFormato);

                Session["STEPARAMETRO1"] = Parametro1;
                Session["STEPARAMETRO2"] = Parametro2;
                Session["STEPARAMETRO3"] = Parametro3;
                Session["STEPARAMETRO4"] = Parametro4;
            }
            else if (DdlReporte.SelectedValue.Equals("2"))
            {
                DateTime vInicioQuincenaMes = DateTime.Now;
                DateTime vFinQuincenaMes = DateTime.Now;
                DateTime vMaxAprobacion1 = DateTime.Now;
                DateTime vMaxAprobacion2 = DateTime.Now;

                int vMes = Convert.ToInt32(DdlFactBanco.SelectedValue);
                int vSiguienteMes = vMes + 1;

                vInicioQuincenaMes = new DateTime(year, vMes, 16, 00, 00, 00);
                vFinQuincenaMes = new DateTime(year, vSiguienteMes, 15, 00, 00, 00);
                vMaxAprobacion1 = new DateTime(year, vSiguienteMes, 22, 00, 00, 00);
                vMaxAprobacion2 = new DateTime(year, vMes, 22, 00, 00, 00);

                Parametro1 = Convert.ToDateTime(vInicioQuincenaMes).ToString(vFormato);
                Parametro2 = Convert.ToDateTime(vFinQuincenaMes).ToString(vFormato);
                Parametro3 = Convert.ToDateTime(vMaxAprobacion1).ToString(vFormato);
                Parametro4 = Convert.ToDateTime(vMaxAprobacion2).ToString(vFormato);

                Session["STEPARAMETRO1"] = Parametro1;
                Session["STEPARAMETRO2"] = Parametro2;
                Session["STEPARAMETRO3"] = Parametro3;
                Session["STEPARAMETRO4"] = Parametro4;
            }

        }       
        protected void BtnDescargar_Click(object sender, EventArgs e)
        {
            String vError = String.Empty;
            try
            {
                validaciones();

                if (DdlReporte.SelectedValue.Equals("1") || DdlReporte.SelectedValue.Equals("2"))
                {
                    LbInformacion.Text = "Buen dia. " + "<br /><br />" +
                    "Reporte a descargar: <b>" + DdlReporte.SelectedItem + "</b><br /><br />";
                    LbInformacionPregunta.Text = "<b>¿Está seguro que desea descargar el reporte seleccionado?</b>";
                    UpdatePanel24.Update();
                }
                else if (DdlReporte.SelectedValue.Equals("3"))
                {
                    LbInformacion.Text = "Buen dia. " + "<br /><br />" +
                    "Reporte a enviar: <b>" + DdlReporte.SelectedItem + "</b><br /><br />";
                    LbInformacionPregunta.Text = "<b>¿Está seguro que desea enviar el consolidado a todos los colaboradores que ingresaron tiempo extraordinario?</b>";
                    UpdatePanel24.Update();
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "OpenInformacion();", true);
                limpiar();
                UpdatePanel21.Update();

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }       
        protected void BtnDescargarModal_Click(object sender, EventArgs e){           
            try {
                if (Session["STEIDREPORTE"].Equals("1"))
                {
                    string Parametro1 = Session["STEPARAMETRO1"].ToString();
                    string Parametro2 = Session["STEPARAMETRO2"].ToString();
                    string Parametro3 = Session["STEPARAMETRO3"].ToString();
                    string Parametro4 = Session["STEPARAMETRO4"].ToString();

                    ReportExecutionService.ReportExecutionService vRSE = new ReportExecutionService.ReportExecutionService();
                    vRSE.Credentials = new NetworkCredential("report_user", "kEbn2HUzd$Fs2T", "adbancat.hn");
                    vRSE.Url = "http://10.128.0.52/reportserver/reportexecution2005.asmx";

                    vRSE.ExecutionHeaderValue = new ReportExecutionService.ExecutionHeader();
                    var vEInfo = new ReportExecutionService.ExecutionInfo();
                    vEInfo = vRSE.LoadReport("/Recursos Humanos Interno/TiempoExtraordinarioCargaSap", null);

                    List<ReportExecutionService.ParameterValue> vParametros = new List<ReportExecutionService.ParameterValue>();
                    vParametros.Add(new ReportExecutionService.ParameterValue { Name = "D1", Value = Parametro1 });
                    vParametros.Add(new ReportExecutionService.ParameterValue { Name = "D2", Value = Parametro2 });
                    vParametros.Add(new ReportExecutionService.ParameterValue { Name = "D3", Value = Parametro3 });
                    vParametros.Add(new ReportExecutionService.ParameterValue { Name = "D4", Value = Parametro4 });

                    vRSE.SetExecutionParameters(vParametros.ToArray(), "en-US");

                    String deviceinfo = "<DeviceInfo><Toolbar>false</Toolbar></DeviceInfo>";
                    String mime;
                    String encoding;
                    string[] stream;
                    ReportExecutionService.Warning[] warning;

                    byte[] vResultado = vRSE.Render("EXCEL", deviceinfo, out mime, out encoding, out encoding, out warning, out stream);

                    //File.WriteAllBytes("c:\\files\\test.pdf", vResultado);

                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.AppendHeader("Content-Type", "application/vnd.ms-excel");
                    byte[] bytFile = vResultado;
                    Response.OutputStream.Write(bytFile, 0, bytFile.Length);
                    Response.AddHeader("Content-disposition", "attachment;filename=CargaSap.xls");
                    Response.End();
                }
                else if (Session["STEIDREPORTE"].Equals("2"))
                {

                    string Parametro1 = Session["STEPARAMETRO1"].ToString();
                    string Parametro2 = Session["STEPARAMETRO2"].ToString();
                    string Parametro3 = Session["STEPARAMETRO3"].ToString();
                    string Parametro4 = Session["STEPARAMETRO4"].ToString();

                    ReportExecutionService.ReportExecutionService vRSE = new ReportExecutionService.ReportExecutionService();
                    vRSE.Credentials = new NetworkCredential("report_user", "kEbn2HUzd$Fs2T", "adbancat.hn");
                    vRSE.Url = "http://10.128.0.52/reportserver/reportexecution2005.asmx";

                    vRSE.ExecutionHeaderValue = new ReportExecutionService.ExecutionHeader();
                    var vEInfo = new ReportExecutionService.ExecutionInfo();
                    vEInfo = vRSE.LoadReport("/Recursos Humanos Interno/TiempoExtraordinarioFacturacionBanco", null);

                    List<ReportExecutionService.ParameterValue> vParametros = new List<ReportExecutionService.ParameterValue>();
                    vParametros.Add(new ReportExecutionService.ParameterValue { Name = "D1", Value = Parametro1 });
                    vParametros.Add(new ReportExecutionService.ParameterValue { Name = "D2", Value = Parametro2 });
                    vParametros.Add(new ReportExecutionService.ParameterValue { Name = "D3", Value = Parametro3 });
                    vParametros.Add(new ReportExecutionService.ParameterValue { Name = "D4", Value = Parametro4 });

                    vRSE.SetExecutionParameters(vParametros.ToArray(), "en-US");

                    String deviceinfo = "<DeviceInfo><Toolbar>false</Toolbar></DeviceInfo>";
                    String mime;
                    String encoding;
                    string[] stream;
                    ReportExecutionService.Warning[] warning;

                    byte[] vResultado = vRSE.Render("EXCEL", deviceinfo, out mime, out encoding, out encoding, out warning, out stream);

                    //File.WriteAllBytes("c:\\files\\test.pdf", vResultado);

                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.AppendHeader("Content-Type", "application/vnd.ms-excel");
                    byte[] bytFile = vResultado;
                    Response.OutputStream.Write(bytFile, 0, bytFile.Length);
                    Response.AddHeader("Content-disposition", "attachment;filename=FactBanco.xls");
                    Response.End();
                }
                else if (Session["STEIDREPORTE"].Equals("3")){
                    SmtpService vService = new SmtpService();

                    string Parametro1 = Session["STEPARAMETRO1"].ToString();
                    string Parametro2 = Session["STEPARAMETRO2"].ToString();
                    string Parametro3 = Session["STEPARAMETRO3"].ToString();
                    string Parametro4 = Session["STEPARAMETRO4"].ToString();

                    DataTable vDatos = new DataTable();
                    String vQuery = "RSP_TiempoExtraordinarioConsolidado 1,'" + Parametro1 + "','" + Parametro2 + "','" + Parametro3 + "','" + Parametro4 + "'";
                    vDatos = vConexion.obtenerDataTable(vQuery);

                    DataTable vDatosSolicitudes = new DataTable();
                    for (int i = 0; i < vDatos.Rows.Count; i++){
                        string vCodigoSAP= vDatos.Rows[i]["codigo"].ToString();

                        String vQuerySolicitudes = "RSP_TiempoExtraordinarioConsolidado 2,'" + Parametro1 + "','" + Parametro2 + "','" + Parametro3 + "','" + Parametro4 + "','"+ vCodigoSAP+"'";
                        vDatosSolicitudes = vConexion.obtenerDataTable(vQuerySolicitudes);
                    }
                    vService.EnviarMensaje(
                                "wpadilla@bancatlan.hn",
                                typeBody.Reporte,
                                "Wendel Padilla",
                                "Reporte de Consolidado",
                                null, null, vDatosSolicitudes
                            );
                }
            }catch (Exception Ex) {
               
                Mensaje(Ex.Message, WarningType.Danger);

            }        
        }
        private void limpiar()
        {
            DdlReporte.SelectedIndex = -1;
            DdlMes.SelectedIndex = -1;
            DdlQuincena.SelectedIndex = -1;

            DdlMes.Visible = false;
            DdlQuincena.Visible = false;
            LbQuincena.Visible = false;
            LbMes.Visible = false;

            DdlFactBanco.SelectedIndex = -1;
            LbFactBanco.Visible = false;
            DdlFactBanco.Visible = false;
        }        
        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            limpiar();
            UpdatePanel21.Update();
        }       
        protected void DdlReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["STEIDREPORTE"] = DdlReporte.SelectedValue;
            if (DdlReporte.SelectedValue.Equals("1"))
            {
                DdlMes.Visible = true;
                DdlQuincena.Visible = true;
                LbQuincena.Visible = true;
                LbMes.Visible = true;

                LbFactBanco.Visible =false;
                DdlFactBanco.Visible = false;
               
                RowBotones.Visible = true;
                UpdatePanel48.Update();

                RowProyectoPropuesta.Visible = false;
                UpdateDivProyectoPropuesta.Update();

            }
            else if (DdlReporte.SelectedValue.Equals("2"))
            {
                DdlMes.Visible = false;
                DdlQuincena.Visible = false;
                LbQuincena.Visible = false;
                LbMes.Visible = false;

                LbFactBanco.Visible = true;
                DdlFactBanco.Visible = true;

                RowBotones.Visible = true;
                UpdatePanel48.Update();

                RowProyectoPropuesta.Visible = false;
                UpdateDivProyectoPropuesta.Update();
            }
            else if (DdlReporte.SelectedValue.Equals("3"))
            {
                DdlMes.Visible = true;
                DdlQuincena.Visible = true;
                LbQuincena.Visible = true;
                LbMes.Visible = true;

                LbFactBanco.Visible = false;
                DdlFactBanco.Visible = false;

                RowBotones.Visible = true;
                UpdatePanel48.Update();

                RowProyectoPropuesta.Visible = false;
                UpdateDivProyectoPropuesta.Update();
            }
            else if (DdlReporte.SelectedValue.Equals("4"))
            {
                DdlMes.Visible = false;
                DdlQuincena.Visible = false;
                LbQuincena.Visible = false;
                LbMes.Visible = false;

                LbFactBanco.Visible = false;
                DdlFactBanco.Visible = false;
               
                RowBotones.Visible = false;
                UpdatePanel48.Update();

                RowProyectoPropuesta.Visible = true;
                cargarProyectoPropuesta();
                UpdateDivProyectoPropuesta.Update();

  

            }
        }
        void cargarProyectoPropuesta()
        {
            try
            {
                DataTable vDatos = new DataTable();
                String vQuery = "RSP_TiempoExtraordinarioGenerales 58";
                vDatos = vConexion.obtenerDataTable(vQuery);

                if (vDatos.Rows.Count > 0)
                {
                    GVProyectoPropuesta.DataSource = vDatos;
                    GVProyectoPropuesta.DataBind();
                    UpdateDivProyectoPropuesta.Update();
                    Session["STEPROYECTOPROPUESTA"] = vDatos;
                }

            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }
        protected void GVProyectoPropuesta_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVProyectoPropuesta.PageIndex = e.NewPageIndex;
                GVProyectoPropuesta.DataSource = (DataTable)Session["STEPROYECTOPROPUESTA"];
                GVProyectoPropuesta.DataBind();
                UpdateDivProyectoPropuesta.Update();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        protected void GVProyectoPropuesta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                String Key = e.Row.Cells[5].Text;
              
                if (Convert.ToDouble(Key) <= 10.0)
                {
                    e.Row.Cells[5].BackColor = Color.Red;
                }
                else
                {
                    e.Row.Cells[5].BackColor = Color.Green;
                }
             
            }
        }
    }
}



       