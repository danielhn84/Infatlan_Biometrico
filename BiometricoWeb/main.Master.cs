using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BiometricoWeb
{
    public partial class main : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e){
            if (!Convert.ToBoolean(Session["AUTH"])){
                Response.Redirect("/login.aspx");
            }

            if (!Page.IsPostBack){
                String vError = "";
                try{
                    DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                    LitUsuario.Text = ((DataRow)vDatos.Rows[0])["nombre"].ToString();

                    if (!vDatos.Rows[0]["tipoEmpleado"].ToString().Equals("")){
                        if (vDatos.Rows[0]["tipoEmpleado"].ToString().Equals("1")){
                            LIBiometricos.Visible = true;
                            LIEmpleados.Visible = true;
                            LIAutorizaciones.Visible = true;
                            LIPermisos.Visible = true;
                            LIMantenimientos.Visible = true;
                            LIToken.Visible = true;
                            LISecurity.Visible = true;
                        }

                        //MODULO PARA SEGURIDAD
                        if (vDatos.Rows[0]["tipoEmpleado"].ToString().Equals("2")){
                            LIBiometricos.Visible = false;
                            LIEmpleados.Visible = false;
                            LIAutorizaciones.Visible = false;
                            LIPermisos.Visible = false;
                            LIMantenimientos.Visible = false;
                            LIToken.Visible = false;
                            LIDashboard.Visible = false;
                            LISecurity.Visible = true;
                        }
                    }else{
                        LIAutorizaciones.Visible = true;
                        LIPermisos.Visible = true;
                    }

                }catch (Exception Ex){
                    vError = Ex.Message;
                }
            }
        }
    }
}