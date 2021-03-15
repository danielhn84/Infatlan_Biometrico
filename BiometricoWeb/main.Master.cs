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

                    if (!vDatos.Rows[0]["tipoEmpleado"].ToString().Equals("")) 
                        administrador(vDatos);

                    viaticos(vDatosPerfil);
                    documentacion(vDatosPerfil);
                    extraordinarios(vDatosPerfil);
                    EntradaSalidas(vDatosPerfil); 
                    desarrollo(vDatosPerfil);
                }catch (Exception Ex){
                    vError = Ex.Message;
                }
            }
        }

        private void extraordinarios(DataTable vDatos) {
            for (int i = 0; i < vDatos.Rows.Count; i++){ 
                if (vDatos.Rows[i]["idAplicacion"].ToString() == "1"){
                    LITEx.Visible = true;
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
                    if (vDatos.Rows[i]["idPerfil"].ToString().Equals("1")){
                        LIViatAprobacion.Visible = true;
                    }else if (vDatos.Rows[i]["idPerfil"].ToString().Equals("2")){
                        LIViatAprobacion.Visible = true;
                        LIViatCotizacion.Visible = true;
                    }else if (vDatos.Rows[i]["idPerfil"].ToString().Equals("3")){
                        LIViatAprobacion.Visible = true;
                        LIViatCotizacion.Visible = true;
                        LIViatMantenimiento.Visible = true;
                    }else if (vDatos.Rows[i]["idPerfil"].ToString().Equals("4")){ 
                        LIViatMantenimiento.Visible = true;
                    }
                    break;
                }
            }
        }

        private void EntradaSalidas(DataTable vDatos) {
            for (int i = 0; i < vDatos.Rows.Count; i++){
                if (vDatos.Rows[i]["idAplicacion"].ToString() == "4"){
                    LISecurity.Visible = true;
                    if (vDatos.Rows[i]["idPerfil"].ToString() == "6"){
                        LISEC_Aprobaciones.Visible = true;
                    }else if (vDatos.Rows[i]["idPerfil"].ToString() == "7") {
                        LISEC_Entradas.Visible = true;
                        LISEC_Salidas.Visible = true;
                        
                        LIDashboard.Visible = false;
                        LIServicios.Visible = false;
                        LIBuzon.Visible = false;
                        LIDocumentacion.Visible = false;
                        LIEstructura.Visible = false;
                    }
                    break;
                }
            }
        }
        
        private void documentacion(DataTable vDatos) {
            try{
                for (int i = 0; i < vDatos.Rows.Count; i++){
                    if (vDatos.Rows[i]["idPerfil"].ToString() == "5") { 
                        LIDocumentosReportes.Visible = true;
                        LIDocumentacionAjustes.Visible = true;
                    }
                    break;
                }
            }catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }

        private void administrador(DataTable vDatos) {
            if (vDatos.Rows[0]["tipoEmpleado"].ToString().Equals("1")){
                LIConfig.Visible = true;
                LIEmpleados.Visible = true;
                LISecurity.Visible = true;
                LISEC_Aprobaciones.Visible = true;
                LISEC_Entradas.Visible = true;
                LISEC_Salidas.Visible = true;
                LISEC_Historico.Visible = true;

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
                LIDocumentosReportes.Visible = true;
                LIDocumentacionAjustes.Visible = true;
            }
        }
        
        private void desarrollo(DataTable vDatos) {
            try{
                for (int i = 0; i < vDatos.Rows.Count; i++){
                    if (vDatos.Rows[i]["idPerfil"].ToString() == "15"){
                        LIActivos.Visible = true;
                    }
                }
            }catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }
    }
}