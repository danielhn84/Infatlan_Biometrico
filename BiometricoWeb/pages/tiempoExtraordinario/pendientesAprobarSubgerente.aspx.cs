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
    public partial class pendientesAprobarSubgerente : System.Web.UI.Page
    {
        db vConexion;
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            vConexion = new db();
            String vEx = Request.QueryString["ex"];
            if (!IsPostBack)
            {               
                if (vEx == null)
                {
                    CargarSolicitudesPendientesAprobarSubgerente();

                }
                else if (vEx.Equals("1"))
                {
                    vEx = null;
                    String vRe = "La solicitud de tiempo extraordinario se ha actualizado con exito, Talento Humano va proceder con la respectiva Aprobación.";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + vRe + "')", true);

                }
                else if (vEx.Equals("2"))
                {
                    vEx = null;
                    String vRe = "La solicitud de tiempo extraordinario se ha cancelado con exito, solicitud no va ser pagada.";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + vRe + "')", true);

                }
                else if (vEx.Equals("3"))
                {
                    vEx = null;
                    String vRe = "No se pudo enviar la respuesta, pongase en contacto con el administrador de la plataforma para que verifique el problema.";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + vRe + "')", true);
                }


            }           
        }
        void CargarSolicitudesPendientesAprobarSubgerente()
        {
            try
            {
                DataTable vDatos = new DataTable();
                String vQuery = "RSP_TiempoExtraordinarioGenerales 61," + Session["USUARIO"].ToString();
                DataTable vDatosPerfil = vConexion.obtenerDataTable(vQuery);

                if (vDatosPerfil.Rows[0]["idPerfil"].ToString().Equals("10"))
                {
                    vQuery = "RSP_TiempoExtraordinarioGenerales 60";
                    vDatos = vConexion.obtenerDataTable(vQuery);

                    GVBusquedaPendientesSubgerente.DataSource = vDatos;
                    GVBusquedaPendientesSubgerente.DataBind();
                    UpdateDivBusquedasSubgerente.Update();
                    Session["STESOLICITUDESPENDIENTESSUBGERENTE"] = vDatos;
                }
                else
                {
                    vQuery = "RSP_TiempoExtraordinarioGenerales 32,'" + Convert.ToString(Session["USUARIO"]) + "'";
                    vDatos = vConexion.obtenerDataTable(vQuery);

                    GVBusquedaPendientesSubgerente.DataSource = vDatos;
                    GVBusquedaPendientesSubgerente.DataBind();
                    UpdateDivBusquedasSubgerente.Update();
                    Session["STESOLICITUDESPENDIENTESSUBGERENTE"] = vDatos;
                }


                

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        protected void GVBusquedaPendientesSubgerente_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVBusquedaPendientesSubgerente.PageIndex = e.NewPageIndex;
                GVBusquedaPendientesSubgerente.DataSource = (DataTable)Session["STESOLICITUDESPENDIENTESSUBGERENTE"];
                GVBusquedaPendientesSubgerente.DataBind();
                UpdateDivBusquedasSubgerente.Update();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        protected void GVBusquedaPendientesSubgerente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Display the company name in italics.
                e.Row.Cells[2].Text = new System.Globalization.CultureInfo("es-ES", false).TextInfo.ToTitleCase(e.Row.Cells[2].Text.ToLower());
            }

        }
        protected void TxBuscarEmpleado_TextChanged(object sender, EventArgs e)
        {
            CargarSolicitudesPendientesAprobarSubgerente();
            String vBusqueda = TxBuscarEmpleado.Text;
            DataTable vDatos = (DataTable)Session["STESOLICITUDESPENDIENTESSUBGERENTE"];

            if (vBusqueda.Equals(""))
            {
                GVBusquedaPendientesSubgerente.DataSource = vDatos;
                GVBusquedaPendientesSubgerente.DataBind();
                UpdateDivBusquedasSubgerente.Update();
            }
            else
            {
                EnumerableRowCollection<DataRow> filtered = vDatos.AsEnumerable()
                    .Where(r => r.Field<String>("nombre").Contains(vBusqueda.ToUpper()));

                DataTable vDatosFiltrados = new DataTable();
                vDatosFiltrados.Columns.Add("idSolicitud");
                vDatosFiltrados.Columns.Add("nombre");
                vDatosFiltrados.Columns.Add("descripcion");
                vDatosFiltrados.Columns.Add("fechaInicio");
                vDatosFiltrados.Columns.Add("fechaFin");
                vDatosFiltrados.Columns.Add("fechaSolicitud");
                vDatosFiltrados.Columns.Add("sysAid");
                vDatosFiltrados.Columns.Add("nombreTrabajo");
                vDatosFiltrados.Columns.Add("detalleTrabajo");
                vDatosFiltrados.Columns.Add("descripcionEstado");

                foreach (DataRow item in filtered)
                {
                    vDatosFiltrados.Rows.Add(
                        item["idSolicitud"].ToString(),
                        item["nombre"].ToString(),
                        item["descripcion"].ToString(),
                        item["fechaInicio"].ToString(),
                        item["fechaFin"].ToString(),
                        item["fechaSolicitud"].ToString(),
                        item["sysAid"].ToString(),
                        item["nombreTrabajo"].ToString(),
                        item["detalleTrabajo"].ToString(),
                        item["descripcionEstado"].ToString()
                        );
                }
                GVBusquedaPendientesSubgerente.DataSource = vDatosFiltrados;
                GVBusquedaPendientesSubgerente.DataBind();
                Session["STESOLICITUDESPENDIENTESSUBGERENTE"] = vDatosFiltrados;
                UpdateDivBusquedasSubgerente.Update();
            }
        }
        protected void GVBusquedaPendientesSubgerente_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vIdSolicitud = e.CommandArgument.ToString();


            if (e.CommandName == "Aprobar")
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

                Response.Redirect("/pages/tiempoExtraordinario/solicitudTE.aspx?ex=5");

            }

        }
    }


}