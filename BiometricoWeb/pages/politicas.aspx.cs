using BiometricoWeb.clases;
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
    public partial class politicas : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    cargar();
                }
            }
        }

        private void cargar() {
            try{
                String vQuery = "[RSP_Politicas] 2";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                vQuery = "[RSP_Politicas] 3," + Session["USUARIO"].ToString();
                DataTable vInfo = vConexion.obtenerDataTable(vQuery);
                String vEstado = vInfo.Rows.Count > 0 ? "Aceptado" : "Pendiente";

                vDatos.Columns.Add("estadoLeido");
                vDatos.Rows[0]["estadoLeido"] = vEstado;

                Session["POLITICAS"] = vDatos;
                GVBusqueda.DataSource = vDatos;
                GVBusqueda.DataBind();
                
            }catch (Exception ex){
                Mensaje(ex.Message , WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                if (e.CommandName == "verPolitica"){
                    Session["TITULO_POLITICA"] = e.CommandArgument;
                    Response.Redirect("politicaDetalle.aspx");
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
    }
}