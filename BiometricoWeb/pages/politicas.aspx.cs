using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BiometricoWeb.pages
{
    public partial class politicas : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    generales vGenerales = new generales();
                    DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                    if (!vGenerales.PermisosRecursosHumanos(vDatos))
                        Response.Redirect("/default.aspx");
                }
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void BtnEnviarPV_Click(object sender, EventArgs e){
            try{
                if (!CBVestimenta.Checked) { 
                    Mensaje("Falta confirmar en la casilla de lectura.", WarningType.Danger);
                }else{
                    String vQuery = "RSP_Politicas 1, " + Session["USUARIO"].ToString() +
                    ",'" + CBVestimenta.Checked + "'";
                    int vInfo = vConexion.ejecutarSql(vQuery);
                    if (vInfo == 1)
                        Mensaje("Gracias!", WarningType.Success);
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
    }
}