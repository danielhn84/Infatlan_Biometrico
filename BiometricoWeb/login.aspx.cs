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
    public partial class login : System.Web.UI.Page
    {
        db vConexion;
        protected void Page_Load(object sender, EventArgs e)
        {
            vConexion = new db();
            if (!Page.IsPostBack)
            {
               
            }
        }

        protected void BtnLogin_Click(object sender, EventArgs e){
            try{
                generales vGenerales = new generales();
                LdapService vLdap = new LdapService();
                //Boolean vLogin = vLdap.ValidateCredentials("ADBancat.hn", TxUsername.Text, TxPassword.Text);
                Boolean vLogin = true;

                if (vLogin){
                    DataTable vDatos = new DataTable();
                    vDatos = vConexion.obtenerDataTable("RSP_Login '" + TxUsername.Text + "','" + vGenerales.MD5Hash(TxPassword.Text) + "'");

                    foreach (DataRow item in vDatos.Rows){
                        Session["AUTHCLASS"] = vDatos;
                        Session["USUARIO"] = item["idEmpleado"].ToString();
                        Session["CODIGOSAP"] = item["codigoSAP"].ToString();
                        Session["AUTH"] = true;
                        Response.Redirect("/default.aspx");
                    }
                }else{
                    Session["AUTH"] = false;
                    throw new Exception("Usuario o contraseña incorrecta.");
                }
            }catch (Exception Ex){
                LbMensaje.Text = "Usuario o contraseña incorrecta.";
                String vErrorLog = Ex.Message;
            }
        }
    }
}