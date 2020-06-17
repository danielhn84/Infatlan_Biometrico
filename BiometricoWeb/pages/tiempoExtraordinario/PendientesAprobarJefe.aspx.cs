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
    public partial class PendientesAprobarJefe : System.Web.UI.Page
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
                CargarSolicitudesPendientesAprobar();


                if (vEx == null)
                {
                    CargarSolicitudesPendientesAprobar();

                }
                else if (vEx.Equals("5"))
                {
                    vEx = null;                                 
                    String vRe = "La solicitud de tiempo extraordinario se ha cancelado con exito (Solicitud no se va a pagar).";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + vRe + "')", true);
                 
                }
                else if (vEx.Equals("6"))
                {
                    vEx = null;
                    String vRe = "La solicitud de tiempo extraordinario se ha aprobado con exito, RRHH ya podra proceder con la aprobación.";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + vRe + "')", true);
                }

                else if (vEx.Equals("7"))
                {
                    vEx = null;
                    String vRe = "El estado de la solicitud se ha actualizado con exito, el colaborador podrá visualizar en su bandeja la solicitud que debe de modificar.";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + vRe + "')", true);
                }
                else if (vEx.Equals("8"))
                {
                    vEx = null;
                    String vRe = "El estado de la solicitud se ha actualizado con exito, la solicitud se va requerir aprobación del subgerente ya que la fecha de la solicitud supera los rangos establecidos.";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + vRe + "')", true);
                }
                else if (vEx.Equals("9"))
                {
                    vEx = null;
                    String vRe = "No se pudo enviar la respuesta, pongase en contacto con el administrador de la plataforma para que verifique el problema.";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + vRe + "')", true);
                }
            }



           
        }
        
        void CargarSolicitudesPendientesAprobar(){
            try{
                DataTable vDatos = new DataTable();
                String vQuery = "RSP_TiempoExtraordinarioGenerales 18,'" + Convert.ToString(Session["USUARIO"]) + "'" ;
                vDatos = vConexion.obtenerDataTable(vQuery);

                if (vDatos.Rows.Count > 0){
                    GVBusquedaPendientesJefe.DataSource = vDatos;
                    GVBusquedaPendientesJefe.DataBind();
                    UpdateDivBusquedasJefes.Update();
                    Session["STESOLICITUDESPENDIENTESJEFE"] = vDatos;
                }
                
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }
        
        protected void GVBusquedaPendientesJefe_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vIdSolicitud =e.CommandArgument.ToString();
  

            if (e.CommandName == "Aprobar")
            {                     
                DataTable vDatos = new DataTable();
                //DATOS GENERALES
                string vQuery = "RSP_TiempoExtraordinarioGenerales 19," + vIdSolicitud;
                vDatos = vConexion.obtenerDataTable(vQuery);
                string vCodigo= vDatos.Rows[0]["codigoSAP"].ToString();
                Session["STEDATOSSOLICITUDINDIVIDUAL"] = vDatos;
               
                vQuery = "RSP_TiempoExtraordinarioGenerales 1,'" + vCodigo + "'";
                vDatos = vConexion.obtenerDataTable(vQuery);
                Session["STEDATOSGENERALESCOLABORADOR"] = vDatos;

                Response.Redirect("/pages/tiempoExtraordinario/solicitudTE.aspx?ex=1");

            }

        }
        
        protected void GVBusquedaPendientesJefe_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Display the company name in italics.
                e.Row.Cells[4].Text = new System.Globalization.CultureInfo("es-ES", false).TextInfo.ToTitleCase(e.Row.Cells[4].Text.ToLower());
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

        }
        
        protected void GVBusquedaPendientesJefe_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVBusquedaPendientesJefe.PageIndex = e.NewPageIndex;
                GVBusquedaPendientesJefe.DataSource = (DataTable)Session["STESOLICITUDESPENDIENTESJEFE"];
                GVBusquedaPendientesJefe.DataBind();
                UpdateDivBusquedasJefes.Update();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        
        protected void TxBuscarEmpleado_TextChanged(object sender, EventArgs e)
        {
            CargarSolicitudesPendientesAprobar();
            String vBusqueda = TxBuscarEmpleado.Text;
            DataTable vDatos = (DataTable)Session["STESOLICITUDESPENDIENTESJEFE"];

            if (vBusqueda.Equals(""))
            {
                GVBusquedaPendientesJefe.DataSource = vDatos;
                GVBusquedaPendientesJefe.DataBind();
                UpdateDivBusquedasJefes.Update();
            }
            else
            {
                EnumerableRowCollection<DataRow> filtered = vDatos.AsEnumerable()
                    .Where(r => r.Field<String>("nombre").Contains(vBusqueda.ToUpper()));

                DataTable vDatosFiltrados = new DataTable();
                vDatosFiltrados.Columns.Add("aprobacionSubgerente");
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
                        item["aprobacionSubgerente"].ToString(),
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
                GVBusquedaPendientesJefe.DataSource = vDatosFiltrados;
                GVBusquedaPendientesJefe.DataBind();
                Session["STESOLICITUDESPENDIENTESJEFE"] = vDatosFiltrados;
                UpdateDivBusquedasJefes.Update();
            }
        }
    }
}