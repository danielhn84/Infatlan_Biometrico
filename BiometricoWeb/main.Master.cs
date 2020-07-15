using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BiometricoWeb.clases;

namespace BiometricoWeb
{
    public partial class main : System.Web.UI.MasterPage
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            if (!Convert.ToBoolean(Session["AUTH"])){
                Response.Redirect("/login.aspx");
            }

            if (!Page.IsPostBack){
                String vError = "";
                try{
                    DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                    String vQuery = "RSP_Perfiles 1," + Session["USUARIO"].ToString();
                    DataTable vDatosPerfil = vConexion.obtenerDataTable(vQuery);
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

                            LIViaticos.Visible = true;
                            LIViatAprobacion.Visible = true;
                            LIViatCotizacion.Visible = true;
                            LIViatMantenimiento.Visible = true;
                            LITExReportes.Visible = true;

                            if (vDatos.Rows[0]["idEmpleado"].ToString() == "80037"){
                                LIViatAprobacion.Visible = false;
                                LIViatCotizacion.Visible = false;
                                LIViatMantenimiento.Visible = false;
                            }
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
                            LIViaticos.Visible = false;
                        }

                        if (vDatos.Rows[0]["tipoEmpleado"].ToString().Equals("3")) {
                            LISecurity.Visible = true;
                            LISEC_Aprobaciones.Visible = true;
                            LIPermisos.Visible = true;
                            LIConfig.Visible = false;
                            LIConstancias.Visible = true;
                            LITEx.Visible = false;
                            LIViaticos.Visible = true;
                            LIEstructura.Visible = true;
                        }
                        extraordinarios(vDatos);
                        viaticos(vDatosPerfil);
                    
                    }else{
                        LIAutorizaciones.Visible = true;
                        LIPermisos.Visible = true;
                        LIConfig.Visible = false;
                        LIConstancias.Visible = true;
                        LITEx.Visible = false;
                        LIEstructura.Visible = true;
                        LIViaticos.Visible = true;
                        extraordinarios(vDatos);
                        viaticos(vDatosPerfil);
                    }
                }catch (Exception Ex){
                    vError = Ex.Message;
                }
            }
        }

        private void extraordinarios(DataTable vDatos) {
            if (vDatos.Rows[0]["idTExPerfil"].ToString() != ""){
                LITEx.Visible = true;
                if (vDatos.Rows[0]["idTExPerfil"].ToString().Equals("1")) {
                    LITExJefatura.Visible = true;
                    LITExMantenimiento.Visible = true;
                    LITExManEquipos.Visible = true;
                }else if (vDatos.Rows[0]["idTExPerfil"].ToString().Equals("2")) {
                    LITExJefatura.Visible = true;
                    LITExMantenimiento.Visible = true;
                    LITExManEquipos.Visible = true;
                    LITExSubgerencia.Visible = true;

                    LITExManPropuesta.Visible = vDatos.Rows[0]["idEmpleado"].ToString() == "389" || vDatos.Rows[0]["idEmpleado"].ToString() == "391" ? true : false;
                }else if (vDatos.Rows[0]["idTExPerfil"].ToString().Equals("3")){
                    LITExReportes.Visible = true;
                    LITExManFeriados.Visible = true;
                    LITExManPropuesta.Visible = true;
                    LITExManProyectos.Visible = true;
                    LITExRRHH.Visible = true;
                    LITExMantenimiento.Visible = true;
                }else if (vDatos.Rows[0]["idTExPerfil"].ToString().Equals("5")){
                    LITExMantenimiento.Visible = true;
                    LITExSubgerencia.Visible = true;
                }
            }
        }

        private void viaticos(DataTable vDatos) {
            for (int i = 0; i < vDatos.Rows.Count; i++){
                if (vDatos.Rows[i]["idAplicacion"].ToString() == "2"){
                    if (vDatos.Rows[0]["idPerfil"].ToString().Equals("1")){
                        LIViatAprobacion.Visible = true;
                    }else if (vDatos.Rows[0]["idPerfil"].ToString().Equals("2")){
                        LIViatAprobacion.Visible = true;
                        LIViatCotizacion.Visible = true;
                    }else if (vDatos.Rows[0]["idPerfil"].ToString().Equals("3")){
                        LIViatAprobacion.Visible = true;
                        LIViatCotizacion.Visible = true;
                        LIViatMantenimiento.Visible = true;
                    }else if (vDatos.Rows[0]["idPerfil"].ToString().Equals("4")){ 
                        LIViatMantenimiento.Visible = true;
                    }
                    break;
                }
            }

            
        }
    }
}