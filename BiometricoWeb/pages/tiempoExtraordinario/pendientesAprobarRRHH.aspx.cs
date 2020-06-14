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
    public partial class pendientesAprobarRRHH : System.Web.UI.Page
    {

        db vConexion;
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            vConexion = new db();
            CargarSolicitudesPendientesAprobar();
        }



        void CargarSolicitudesPendientesAprobar()
        {
            try
            {

                DataTable vDatos = new DataTable();
                String vQuery = "RSP_TiempoExtraordinarioGenerales 24";
                vDatos = vConexion.obtenerDataTable(vQuery);

                GVBusquedaPendientesRRHH.DataSource = vDatos;
                GVBusquedaPendientesRRHH.DataBind();
                UpdateDivBusquedasRRHH.Update();
                Session["STESOLICITUDESPENDIENTESRRHH"] = vDatos;


            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusquedaPendientesRRHH_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Display the company name in italics.
                e.Row.Cells[6].Text = new System.Globalization.CultureInfo("es-ES", false).TextInfo.ToTitleCase(e.Row.Cells[6].Text.ToLower());
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                String Key = e.Row.Cells[1].Text;
                var estado = e.Row.FindControl("imgEstado") as Image;

                if (Key == "True")
                {
                    estado.ImageUrl = "/images/icon_azul.png";
                }
                else
                {
                    e.Row.FindControl("imgEstado").Visible = false;
                }
                e.Row.Cells[1].Visible = false;
            }


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                String Key = e.Row.Cells[4].Text;
                var estado = e.Row.FindControl("imgEstadoRRHH") as Image;

                if (Key == "Pendiente Aprobar RRHH" )
                {

                    estado.ImageUrl = "/images/icon_verde.png";

                }
                else
                {
                    e.Row.FindControl("imgEstadoRRHH").Visible = false;
                }

            
                e.Row.Cells[1].Visible = false;
            }



        }

        protected void GVBusquedaPendientesRRHH_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string vIdSolicitud = e.CommandArgument.ToString();
            if (e.CommandName == "Aprobar")
            {
                DataTable vDatos = new DataTable();
                string vQuery = "RSP_TiempoExtraordinarioGenerales 25," + vIdSolicitud;
                vDatos = vConexion.obtenerDataTable(vQuery);

                string vestadoSolicitud = vDatos.Rows[0]["idEstado"].ToString();
                if (vestadoSolicitud !="3")
                    throw new Exception("Todavia no puede proceder aprobar la solicitud,estado de la solicitud: "+ vDatos.Rows[0]["descripcionEstado"].ToString());

                    
                    //DATOS GENERALES
                     vQuery = "RSP_TiempoExtraordinarioGenerales 19," + vIdSolicitud;
                    vDatos = vConexion.obtenerDataTable(vQuery);
                    string vCodigo = vDatos.Rows[0]["codigoSAP"].ToString();
                    Session["STEDATOSSOLICITUDINDIVIDUAL"] = vDatos;

                    vQuery = "RSP_TiempoExtraordinarioGenerales 1,'" + vCodigo + "'";
                    vDatos = vConexion.obtenerDataTable(vQuery);
                    Session["STEDATOSGENERALESCOLABORADOR"] = vDatos;

                    Response.Redirect("/pages/tiempoExtraordinario/solicitudTE.aspx?ex=2");



                }
         }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }

        }

        protected void TxBuscarEmpleado_TextChanged(object sender, EventArgs e)
        {
            CargarSolicitudesPendientesAprobar();
            String vBusqueda = TxBuscarEmpleado.Text;
            DataTable vDatos = (DataTable)Session["STESOLICITUDESPENDIENTESRRHH"];

            if (vBusqueda.Equals(""))
            {
                GVBusquedaPendientesRRHH.DataSource = vDatos;
                GVBusquedaPendientesRRHH.DataBind();
                UpdateDivBusquedasRRHH.Update();
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
                vDatosFiltrados.Columns.Add("aprobacionSubgerente");
                vDatosFiltrados.Columns.Add("estado");
                vDatosFiltrados.Columns.Add("idSolicitud");
                vDatosFiltrados.Columns.Add("nombre");
                vDatosFiltrados.Columns.Add("descripcion");
                vDatosFiltrados.Columns.Add("fechaInicio");
                vDatosFiltrados.Columns.Add("fechaFin");
                vDatosFiltrados.Columns.Add("fechaSolicitud");
                vDatosFiltrados.Columns.Add("nombreTrabajo");
                vDatosFiltrados.Columns.Add("detalleTrabajo");

                foreach (DataRow item in filtered)
                {
                    vDatosFiltrados.Rows.Add(                       
                        item["aprobacionSubgerente"].ToString(),
                        item["estado"].ToString(),                        
                        item["idSolicitud"].ToString(),
                        item["nombre"].ToString(),
                        item["descripcion"].ToString(),
                        item["fechaInicio"].ToString(),
                        item["fechaFin"].ToString(),
                        item["fechaSolicitud"].ToString(),
                        item["nombreTrabajo"].ToString(),
                        item["detalleTrabajo"].ToString()
                        );
                }
                GVBusquedaPendientesRRHH.DataSource = vDatosFiltrados;
                GVBusquedaPendientesRRHH.DataBind();
                Session["STESOLICITUDESPENDIENTESRRHH"] = vDatosFiltrados;
                UpdateDivBusquedasRRHH.Update();
            }
        }
    }
}