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
    public partial class solicitudesAprobadasRRHH : System.Web.UI.Page
    {
        db vConexion;
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            vConexion = new db();
            cargarSolicitudesAprobadasRRHH();
        }


        void cargarSolicitudesAprobadasRRHH()
        {
            try
            {
                DataTable vDatos = new DataTable();
                String vQuery = "RSP_TiempoExtraordinarioGenerales 28";
                vDatos = vConexion.obtenerDataTable(vQuery);

                GVBusquedaAprobadasRRHH.DataSource = vDatos;
                GVBusquedaAprobadasRRHH.DataBind();
                UpBusquedaAprobadasRRHH.Update();
                Session["STESOLICITUDESAPROBADASRRHH"] = vDatos;
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusquedaAprobadasRRHH_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVBusquedaAprobadasRRHH.PageIndex = e.NewPageIndex;
                GVBusquedaAprobadasRRHH.DataSource = (DataTable)Session["STESOLICITUDESAPROBADASRRHH"];
                GVBusquedaAprobadasRRHH.DataBind();
                UpBusquedaAprobadasRRHH.Update();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusquedaAprobadasRRHH_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Display the company name in italics.
                e.Row.Cells[3].Text = new System.Globalization.CultureInfo("es-ES", false).TextInfo.ToTitleCase(e.Row.Cells[3].Text.ToLower());
            }
        }

        protected void GVBusquedaAprobadasRRHH_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "window.alert('No existe hoja de servicio para esta solicitud')", true);
            }
            else if (e.CommandName == "Solicitud")
            {

                String vQuery = "RSP_TiempoExtraordinarioGenerales 31," + vIdSolicitud;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                string vHrsDiurnasSolicitadas = vDatos.Rows[0]["totalHrsSolicitadasDiurnas"].ToString();
                string vHrsNocSolicitadas = vDatos.Rows[0]["totalHrsSolicitadasNoc"].ToString();
                string vHrsNocNocSolicitadas = vDatos.Rows[0]["totalHrsSolicitadasNocNoc"].ToString();
                string vHrsDomingoFeriadoSolicitadas = vDatos.Rows[0]["totalHrsSolicitadasDomingoFeriado"].ToString();
                string vHrsTotSolicitadas = vDatos.Rows[0]["totalHrsSolicitadas"].ToString();


                string vHrsDiurnasAprobadas = vDatos.Rows[0]["totalHrsAprobadasDiurnas"].ToString();
                string vHrsNocAprobadas = vDatos.Rows[0]["totalHrsAprobadasNoc"].ToString();
                string vHrsNocNocAprobadas = vDatos.Rows[0]["totalHrsAprobadasNocNoc"].ToString();
                string vHrsDomingoFeriadoAprobadas = vDatos.Rows[0]["totalHrsAprobadasDomingoFeriado"].ToString();
                string vHrsTotAprobadas = vDatos.Rows[0]["totalHrsAprobadas"].ToString();

                string vUsuarioAprobo = vDatos.Rows[0]["nombre"].ToString();
                string vPermisoReintegradoAlmuero = vDatos.Rows[0]["permisoAmuerzoRrhh"].ToString();
                string vPermisoReintegradoSalida = vDatos.Rows[0]["permisoSalidaRrhh"].ToString();
                string vPermisoReintegradoEntrada = vDatos.Rows[0]["permisoEntradaRrhh"].ToString();

                if (vPermisoReintegradoAlmuero=="")
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


                LBTituloRRHH.Text = "Más Informacion Solicitud - " + vIdSolicitud;
                LbMsgMasInformacion.Text =
              "Total de horas solicitadas: <b> " + vHrsTotSolicitadas + "</b><br />" + "Horas Diurnas: <b>" + vHrsDiurnasSolicitadas + "</b>, Horas Noc: <b>" + vHrsNocSolicitadas + "</b>, Horas NocNoc: <b>" + vHrsNocNocSolicitadas + "</b>, Horas Domingos Feriados: <b>" + vHrsDomingoFeriadoSolicitadas + " </b>" + "<br /><br />" + "Total de horas aprobadas: <b> " + vHrsTotAprobadas + "</b><br />" + "Horas Diurnas: <b>" + vHrsDiurnasAprobadas + "</b>, Horas Noc: <b>" + vHrsNocAprobadas + "</b>, Horas NocNoc: <b>" + vHrsNocNocAprobadas + "</b>, Horas Domingos Feriados: <b>" + vHrsDomingoFeriadoAprobadas + "</b><br /><br />" +
              "Usuario RRHH aprobo: <b>" + vUsuarioAprobo + "</b><br /><br />" +
              "<hr> " +
              "<center> <b>Pemisos Reintegrados: </b> </center><br />" +
              "<b>Entrada Tarde: </b><br />" +
              vPermisoReintegradoEntrada + "</b><br /><br />" +
               "<b>Salida Temprana: </b><br />" +
              vPermisoReintegradoSalida + "</b><br /><br />" +
               "<b>Almuerzo: </b><br />" + 
              vPermisoReintegradoAlmuero + "</b><br /><br />" 
              ;
                //LbAprobarRRHHPregunta.Text = "<b>¿Está seguro que desea " + Session["STELBESTADORRHH"].ToString() + " la solicitud de:</ b > " + TxEmpleado.Text + "<b> ,en el rango de fechas y horas detalladas?</b>";
                UpdateMensaje.Update();
                UpTitulo.Update();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMasInformacionModal();", true);
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

        protected void TxBuscarEmpleado_TextChanged(object sender, EventArgs e)
        {
            cargarSolicitudesAprobadasRRHH();
            String vBusqueda = TxBuscarEmpleado.Text;
            DataTable vDatos = (DataTable)Session["STESOLICITUDESAPROBADASRRHH"];

            if (vBusqueda.Equals(""))
            {
                GVBusquedaAprobadasRRHH.DataSource = vDatos;
                GVBusquedaAprobadasRRHH.DataBind();
                UpBusquedaAprobadasRRHH.Update();
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
                vDatosFiltrados.Columns.Add("aprobadas");
                vDatosFiltrados.Columns.Add("fechaInicio");
                vDatosFiltrados.Columns.Add("fechaFin");
                vDatosFiltrados.Columns.Add("nombreTrabajo");
                vDatosFiltrados.Columns.Add("fechaSolicitud");
                vDatosFiltrados.Columns.Add("horaRrhhAprobo");

                foreach (DataRow item in filtered)
                {
                    vDatosFiltrados.Rows.Add(
                        item["idSolicitud"].ToString(),
                        item["nombre"].ToString(),
                        item["descripcion"].ToString(),
                        item["aprobadas"].ToString(),
                        item["fechaInicio"].ToString(),
                        item["fechaFin"].ToString(),
                        item["nombreTrabajo"].ToString(),
                        item["fechaSolicitud"].ToString(),
                        item["horaRrhhAprobo"].ToString()
   

                        );
                }
                GVBusquedaAprobadasRRHH.DataSource = vDatosFiltrados;
                GVBusquedaAprobadasRRHH.DataBind();
                Session["STESOLICITUDESAPROBADASRRHH"] = vDatosFiltrados;
                UpBusquedaAprobadasRRHH.Update();
            }
        }
    }
}