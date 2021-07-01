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
    public partial class nuevoActivo : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){

        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void DDLProceso_SelectedIndexChanged(object sender, EventArgs e){
            try{
                if (DDLProceso.SelectedValue == "1")
                    Response.Redirect("/pages/activos/activosInternos.aspx");
                if (DDLProceso.SelectedValue == "2")
                    Response.Redirect("/pages/activos/visitas.aspx");
                if (DDLProceso.SelectedValue == "3")
                    Response.Redirect("/pages/activos/registroVisitaSeguridad.aspx");
                if (DDLProceso.SelectedValue == "4")
                    Response.Redirect("/pages/activos/soporte.aspx");
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
    }
}