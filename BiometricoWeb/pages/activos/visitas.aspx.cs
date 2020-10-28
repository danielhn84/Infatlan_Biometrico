using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

namespace BiometricoWeb.pages.activos
{
    public partial class visitas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e){
            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    generales vGenerales = new generales();
                    DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                    if (!vGenerales.PermisosPersonalSeguridad(vDatos))
                        Response.Redirect("/login.aspx");

                    cargarDatos();
                }
            }
        }

        private void cargarDatos(){
            try{

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        public void MensajeLoad(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", " document.addEventListener(\"DOMContentLoaded\", function (event) { infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "'); });", true);
        }

        protected void BtnGuardar_Click(object sender, EventArgs e){
            try{

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e){
            limpiarFormulario();
        }

        private void limpiarFormulario(){
            DDLArea.SelectedValue = "0";
            TxNombre.Text = string.Empty;
            TxApellido.Text = string.Empty;
            TxIdentidad.Text = string.Empty;

            UpdatePanel1.Update();
        }

        protected void TxBuscaSerie_TextChanged(object sender, EventArgs e){
            try{

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
    }
}