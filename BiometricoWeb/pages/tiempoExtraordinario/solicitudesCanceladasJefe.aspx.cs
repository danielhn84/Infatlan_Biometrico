using BiometricoWeb.clases;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using System.Globalization;
using System.Text;



namespace BiometricoWeb.pages.tiempoExtraordinario
{
    public partial class solicitudesCanceladasJefe : System.Web.UI.Page
    {
        db vConexion;
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            vConexion = new db();
            cargarSolicitudesCanceladasJefe();
        }
        void cargarSolicitudesCanceladasJefe()
        {
            try
            {
                DataTable vDatos = new DataTable();
                String vQuery = "RSP_TiempoExtraordinarioGenerales 23,'" + Convert.ToString(Session["USUARIO"]) + "'";
                vDatos = vConexion.obtenerDataTable(vQuery);

                GVBusquedaCanceladasJefes.DataSource = vDatos;
                GVBusquedaCanceladasJefes.DataBind();
                UpBusquedaCanceladasJefes.Update();
                Session["STESOLICITUDESCANCELADASJEFE"] = vDatos;
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        protected void GVBusquedaCanceladasJefes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Display the company name in italics.
                e.Row.Cells[3].Text = new System.Globalization.CultureInfo("es-ES", false).TextInfo.ToTitleCase(e.Row.Cells[3].Text.ToLower());
            }
        }
        protected void GVBusquedaCanceladasJefes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVBusquedaCanceladasJefes.PageIndex = e.NewPageIndex;
                GVBusquedaCanceladasJefes.DataSource = (DataTable)Session["STESOLICITUDESCANCELADASJEFE"];
                GVBusquedaCanceladasJefes.DataBind();
                UpBusquedaCanceladasJefes.Update();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        protected void GVBusquedaCanceladasJefes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vIdSolicitud = e.CommandArgument.ToString();
            if (e.CommandName == "HojaServicio")
            {
                String vQuery = "RSP_TiempoExtraordinarioGenerales 12," + vIdSolicitud;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                String vDocumento = "";
                if (!vDatos.Rows[0]["hojaServicio"].ToString().Equals(""))
                    vDocumento = vDatos.Rows[0]["hojaServicio"].ToString();

                if (!vDocumento.Equals(""))
                {
                    LbHojaDescarga.Text = vIdSolicitud;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openDescargarHojaServicioModal();", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "window.alert('No existe documento en este permiso')", true);
            }
            else if (e.CommandName == "Solicitud")
            {
                String vQuery = "RSP_TiempoExtraordinarioGenerales 39," + vIdSolicitud;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                LbMasInformacion.Text = "Mas Información solicitud- " + vIdSolicitud;
                LbMensaje1.Text =
                     "Total de horas solicitadas: <b> " + vDatos.Rows[0]["totalHrsSolicitadas"].ToString() + "</b><br />" + "Horas Diurnas: <b>" + vDatos.Rows[0]["totalHrsSolicitadasDiurnas"].ToString() + "</b>, Horas Noc: <b>" + vDatos.Rows[0]["totalHrsSolicitadasNoc"].ToString() + "</b>, Horas NocNoc: <b>" + vDatos.Rows[0]["totalHrsSolicitadasNocNoc"].ToString() + "</b>, Horas Domingos Feriados: <b>" + vDatos.Rows[0]["totalHrsSolicitadasDomingoFeriado"].ToString() + " </b>" + "<br /><br />" +
                     "Motivo cancelacion: <b> " + vDatos.Rows[0]["observacionAprobacionJefe"].ToString() + " </b>" + "<br /><br />" +
                     "Trabajo realizado: <b> " + vDatos.Rows[0]["nombreTrabajo"].ToString() + " </b>" + "<br /><br />" +
                     "Detalle: <b> " + vDatos.Rows[0]["detalleTrabajo"].ToString() + " </b>";

                UpTitulo.Update();
                UpMensaje.Update();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "OpenMasInformacion();", true);
            }



        }
        protected void TxBuscarEmpleado_TextChanged(object sender, EventArgs e)
        {
            cargarSolicitudesCanceladasJefe();
            String vBusqueda = TxBuscarEmpleado.Text;
            DataTable vDatos = (DataTable)Session["STESOLICITUDESCANCELADASJEFE"];

            if (vBusqueda.Equals(""))
            {
                GVBusquedaCanceladasJefes.DataSource = vDatos;
                GVBusquedaCanceladasJefes.DataBind();
                UpBusquedaCanceladasJefes.Update();
            }
            else
            {
                EnumerableRowCollection<DataRow> filtered = vDatos.AsEnumerable()
                    .Where(r => r.Field<String>("nombre").Contains(vBusqueda.ToUpper()));

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
                vDatosFiltrados.Columns.Add("nombre");
                vDatosFiltrados.Columns.Add("descripcion");
                vDatosFiltrados.Columns.Add("fechaInicio");
                vDatosFiltrados.Columns.Add("fechaFin");
                vDatosFiltrados.Columns.Add("nombreTrabajo");
                vDatosFiltrados.Columns.Add("fechaSolicitud");
                vDatosFiltrados.Columns.Add("horaJefeAprobo");
                vDatosFiltrados.Columns.Add("motivoCancelacionJefe");
                vDatosFiltrados.Columns.Add("observacionAprobacionJefe");

                foreach (DataRow item in filtered)
                {
                    vDatosFiltrados.Rows.Add(
                        item["idSolicitud"].ToString(),
                        item["nombre"].ToString(),
                        item["descripcion"].ToString(),
                        item["fechaInicio"].ToString(),
                        item["fechaFin"].ToString(),
                        item["nombreTrabajo"].ToString(),
                        item["fechaSolicitud"].ToString(),
                        item["horaJefeAprobo"].ToString(),
                        item["motivoCancelacionJefe"].ToString(),
                        item["observacionAprobacionJefe"].ToString()
                        );
                }
                GVBusquedaCanceladasJefes.DataSource = vDatosFiltrados;
                GVBusquedaCanceladasJefes.DataBind();
                Session["STESOLICITUDESCANCELADASJEFE"] = vDatosFiltrados;
                UpBusquedaCanceladasJefes.Update();
            }
        }
        protected void BtnDescarga_Click(object sender, EventArgs e)
        {
            try
            {
                string vIdSolicitud = LbHojaDescarga.Text;

                String vQuery = "RSP_TiempoExtraordinarioGenerales 12," + vIdSolicitud;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                String vDocumento = "";
                if (!vDatos.Rows[0]["hojaServicio"].ToString().Equals(""))
                    vDocumento = vDatos.Rows[0]["hojaServicio"].ToString();

                if (!vDocumento.Equals(""))
                {
                    String vDocumentoArchivo = "HojaServicio-" + vIdSolicitud + vDatos.Rows[0]["extension"].ToString();

                    byte[] fileData = Convert.FromBase64String(vDocumento);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    GetExtension(vDatos.Rows[0]["extension"].ToString().ToLower());
                    byte[] bytFile = fileData;
                    Response.OutputStream.Write(bytFile, 0, bytFile.Length);
                    Response.AddHeader("Content-disposition", "attachment;filename=" + vDocumentoArchivo);
                    Response.End();
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
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
    }
}