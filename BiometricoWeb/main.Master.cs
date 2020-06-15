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
                            LISEC_Aprobaciones.Visible = true;
                            LISEC_Entradas.Visible = true;
                            LISEC_Salidas.Visible = true;
                            LISEC_Historico.Visible = true;
                            LIConstancias.Visible = true;
                            LIEstructura.Visible = true;

                            LITEx.Visible = true;
                            LITExJefatura.Visible = true;
                            LITExSubgerencia.Visible = true;
                            LITExRRHH.Visible = true;
                            LITExMantenimiento.Visible = true;
                        }

                        //MODULO PARA SEGURIDAD
                        if (vDatos.Rows[0]["tipoEmpleado"].ToString().Equals("2")){
                            LIEmpleados.Visible = false;
                            LIDashboard.Visible = false;
                            LISecurity.Visible = true;
                            LISEC_Entradas.Visible = true;
                            LISEC_Salidas.Visible = true;
                            LISEC_Historico.Visible = false;
                            LIPoliticas.Visible = false;
                            LIConfig.Visible= false;
                            LIServicios.Visible = false;
                            LIBuzon.Visible = false;
                            LITEx.Visible = false;
                        }

                        if (vDatos.Rows[0]["tipoEmpleado"].ToString().Equals("3")) {
                            LISecurity.Visible = true;
                            LISEC_Aprobaciones.Visible = true;
                            LIPermisos.Visible = true;
                            LIConfig.Visible = false;
                            LIConstancias.Visible = true;
                            LITEx.Visible = false;
                            LIEstructura.Visible = true;
                        }

                        extraordinarios(vDatos);
                        
                    }else{
                        LIAutorizaciones.Visible = true;
                        LIPermisos.Visible = true;
                        LIConfig.Visible = false;
                        LIConstancias.Visible = true;
                        LITEx.Visible = false;
                        LIEstructura.Visible = true;
                        extraordinarios(vDatos);
                    }
                }catch (Exception Ex){
                    vError = Ex.Message;
                }
            }
        }

        private void extraordinarios(DataTable vDatos) { 
            if (vDatos.Rows[0]["flagHorasExtra"].ToString().Equals("True")){
                LITEx.Visible = true;
                if (vDatos.Rows[0]["idTExPerfil"].ToString().Equals("1")) {
                    LITExJefatura.Visible = true;
                    LITExMantenimiento.Visible = true;
                    LITExManEquipos.Visible = true;
                }else if (vDatos.Rows[0]["idTExPerfil"].ToString().Equals("2")) {
                    LITExMantenimiento.Visible = true;
                    LITExManEquipos.Visible = true;
                    LITExSubgerencia.Visible = true;

                    LITExManPropuesta.Visible = vDatos.Rows[0]["idEmpleado"].ToString() == "389" && vDatos.Rows[0]["idEmpleado"].ToString() == "391" ? true : false;
                }else if (vDatos.Rows[0]["idTExPerfil"].ToString().Equals("3")){
                    LITExManFeriados.Visible = true;
                    LITExRRHH.Visible = true;
                    LITExMantenimiento.Visible = true;
                }else if (vDatos.Rows[0]["idTExPerfil"].ToString().Equals("5")){
                    LITExMantenimiento.Visible = true;
                    LITExSubgerencia.Visible = true;
                }
            }
        }
    }
}