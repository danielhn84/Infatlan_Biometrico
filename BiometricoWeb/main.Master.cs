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

                    if (!vDatos.Rows[0]["tipoEmpleado"].ToString().Equals("")) {
                        viaticos(vDatosPerfil);
                        documentacion(vDatosPerfil);
                        extraordinarios(vDatosPerfil);
                        EntradaSalidas(vDatosPerfil);
                        if (vDatos.Rows[0]["tipoEmpleado"].ToString().Equals("1")) {
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
                        }

                    }else{
                        LIAutorizaciones.Visible = true;
                        LIPermisos.Visible = true;
                        LIConfig.Visible = false;
                        LIConstancias.Visible = true;
                        LITEx.Visible = false;
                        LIEstructura.Visible = true;

                        viaticos(vDatosPerfil);
                        documentacion(vDatosPerfil);
                        extraordinarios(vDatosPerfil);
                        EntradaSalidas(vDatosPerfil);
                    } 
                }catch (Exception Ex){
                    vError = Ex.Message;
                }
            }
        }

        private void extraordinarios(DataTable vDatos) {
            LITEx.Visible = true;
            for (int i = 0; i < vDatos.Rows.Count; i++){ 
                if (vDatos.Rows[i]["idAplicacion"].ToString() == "1"){
                    if (vDatos.Rows[i]["idPerfil"].ToString().Equals("8")) {
                        LITExJefatura.Visible = true;
                        LITExMantenimiento.Visible = true;
                        LITExManEquipos.Visible = true;
                    }else if (vDatos.Rows[i]["idPerfil"].ToString().Equals("9")) {
                        LITExJefatura.Visible = true;
                        LITExMantenimiento.Visible = true;
                        LITExManEquipos.Visible = true;
                        LITExSubgerencia.Visible = true;

                        LITExManPropuesta.Visible = Session["USUARIO"].ToString() == "389" || Session["USUARIO"].ToString() == "391" ? true : false;
                    }else if (vDatos.Rows[i]["idPerfil"].ToString().Equals("10")){
                        LITExReportes.Visible = true;
                        LITExManFeriados.Visible = true;
                        LITExManPropuesta.Visible = true;
                        LITExManProyectos.Visible = true;
                        LITExRRHH.Visible = true;
                        LITExMantenimiento.Visible = true;
                    }else if (vDatos.Rows[i]["idPerfil"].ToString().Equals("12")){
                        LITExMantenimiento.Visible = true;
                        LITExSubgerencia.Visible = true;
                    }
                }
            }
        }

        private void viaticos(DataTable vDatos) {
            for (int i = 0; i < vDatos.Rows.Count; i++){
                if (vDatos.Rows[i]["idAplicacion"].ToString() == "2"){
                    LIViaticos.Visible = true;
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

        private void EntradaSalidas(DataTable vDatos) {
            for (int i = 0; i < vDatos.Rows.Count; i++){
                if (vDatos.Rows[i]["idAplicacion"].ToString() == "4"){
                    if (vDatos.Rows[i]["idPerfil"].ToString() == "6"){
                        LISecurity.Visible = true;
                        LISEC_Aprobaciones.Visible = true;
                        LIPermisos.Visible = true;
                        LIConfig.Visible = false;
                        LIConstancias.Visible = true;
                        LITEx.Visible = false;
                        LIEstructura.Visible = true;
                    }else if (vDatos.Rows[i]["idPerfil"].ToString() == "7") {
                        LIEmpleados.Visible = false;
                        LIDashboard.Visible = false;
                        LISecurity.Visible = true;
                        LISEC_Entradas.Visible = true;
                        LISEC_Salidas.Visible = true;
                        LISEC_Historico.Visible = false;
                        LIConfig.Visible = false;
                        LIServicios.Visible = false;
                        LIBuzon.Visible = false;
                        LITEx.Visible = false;
                        LIViaticos.Visible = false;
                        LIDocumentacion.Visible = false;
                    }
                    break;
                }
            }
        }
        
        private void documentacion(DataTable vDatos) {
            for (int i = 0; i < vDatos.Rows.Count; i++){
                if (vDatos.Rows[i]["idAplicacion"].ToString() == "3"){
                    if (vDatos.Rows[i]["idPerfil"].ToString() == "5"){
                        LIDocumentacion.Visible = true;
                        LIDocumentosReportes.Visible = true;
                    }
                    break;
                }
            }
        }
    }
}