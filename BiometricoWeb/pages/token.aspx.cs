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
    public partial class token : System.Web.UI.Page{

        db vConexion;
        protected void Page_Load(object sender, EventArgs e){
            vConexion = new db();
            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    generales vGenerales = new generales();
                    DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                    if (!vGenerales.PermisosRecursosHumanos(vDatos))
                        Response.Redirect("/default.aspx");

                    Cargar();
                }
            }
        }

        private void Cargar() {
            try{
                String vQuery = "[RSP_ObtenerGenerales] 1";
                DataTable vData = vConexion.obtenerDataTable(vQuery);
                DDLEmpleado.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vData.Rows){
                    DDLEmpleado.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
                }

            }catch (Exception ex){

            }
        }

        protected void BtnGenerar_Click(object sender, EventArgs e){
            DDLEmpleado.Enabled = true;
            CryptoToken.CryptoToken vCrypto = new CryptoToken.CryptoToken();
            String vPass = ConfigurationManager.AppSettings["TokenPass"].ToString();
            String vTok = vCrypto.Encrypt(ConfigurationManager.AppSettings["TokenWord"].ToString(), vPass);
            //String vTok2 = vCrypto.Decrypt(vTok, vPass);

            Session["TOKEN"] = vTok;
            TxToken.Text = vTok;
        }

        protected void BtnEnviar_Click(object sender, EventArgs e){
            try{
                String vQuery = "RSP_ObtenerEmpleados 2," + DDLEmpleado.SelectedValue;
                DataTable vDatosEmpleado = vConexion.obtenerDataTable(vQuery);

                SmtpService vService = new SmtpService();
                Boolean vFlagEnviado = false;
                
                foreach (DataRow item in vDatosEmpleado.Rows){
                    if (!item["emailEmpresa"].ToString().Trim().Equals("")){
                        vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                            typeBody.Token,
                            item["nombre"].ToString(),
                            Convert.ToString(Session["TOKEN"])
                            );
                        vFlagEnviado = true;
                    }
                }
                
                if (vFlagEnviado){
                    vQuery = "[RSP_IngresaMantenimientos] 5,'"+ Session["TOKEN"].ToString() + "'," + DDLEmpleado.SelectedValue;
                    int vVerificacion = vConexion.ejecutarSql(vQuery);
                    if (vVerificacion == 1){
                        Mensaje("Token enviado con exito!", WarningType.Success);
                        TxToken.Text = String.Empty;
                        DDLEmpleado.Enabled = false;
                    }

                }
            }catch (Exception ex){
                Mensaje("Error al enviar correo.", WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
    }
}