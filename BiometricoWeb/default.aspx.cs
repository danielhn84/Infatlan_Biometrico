using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BiometricoWeb
{
    public partial class _default : System.Web.UI.Page
    {
        db vConexion;
        protected void Page_Load(object sender, EventArgs e){
            vConexion = new db();

            


            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    

                    CargarMarcajes();
                    String vQuery = "RSP_ObtenerGenerales 6,'" + Convert.ToString(Session["USUARIO"]) + "'";
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                    LitFechaPermisos.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    LitPermisosCreados.Text = vDatos.Rows[0]["TotalPermisos"].ToString();
                    LitPermisosFinalizados.Text = vDatos.Rows[0]["Autorizados"].ToString();
                }
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        public void CerrarModal(String vModal){
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#" + vModal + "').modal('hide');", true);
        }

        void CargarMarcajes(){
            try{
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerGenerales 7,'" + Convert.ToString(Session["USUARIO"]) + "'"); 
                GVBusqueda.DataSource = vDatos;
                GVBusqueda.DataBind();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); } 
        }
    }
}