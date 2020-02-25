using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BiometricoWeb.clases;
using System.Data;

namespace BiometricoWeb.pages
{
    public partial class politicaDetalle : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            if (!Page.IsPostBack){
                LbTitulo.Text = "POLITICA DE VESTIMENTA";
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void BtnEnviarPV_Click(object sender, EventArgs e){
            try{
                String vQuery = "RSP_Politicas 3," + Session["USUARIO"].ToString();
                DataTable vData = vConexion.obtenerDataTable(vQuery);
                if (vData.Rows.Count < 1){
                    if (!CBVestimenta.Checked){
                        Mensaje("Falta confirmar en la casilla de lectura.", WarningType.Danger);
                    }else{
                        vQuery = "RSP_Politicas 1, 1, " + Session["USUARIO"].ToString() +
                        ",'" + CBVestimenta.Checked + "'";
                        int vInfo = vConexion.ejecutarSql(vQuery);
                        if (vInfo == 1)
                            Mensaje("Respuesta enviada con éxito!", WarningType.Success);
                    }
                }else{
                    Mensaje("Ya se ha registrado su respuesta!", WarningType.Warning);
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
    }
}