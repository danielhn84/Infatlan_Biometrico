using BiometricoWeb.clases;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using System.Globalization;
using System.Text;


namespace BiometricoWeb.pages.activos
{
    public partial class registroVisitaSeguridad : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            if (!IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    CargarSolicitudesAutorizadas();
                }
            }
        }

        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        void CargarSolicitudesAutorizadas(){
            try{
                DataTable vDatos = new DataTable();
                String vQuery = "RSP_ActivosDC 24" ;
                vDatos = vConexion.obtenerDataTable(vQuery);

                if (vDatos.Rows.Count > 0){
                    GVBusquedas.DataSource = vDatos;
                    GVBusquedas.DataBind();
                    UpdateDivAutorizadas.Update();
                    Session["ACTIVO_DC_SOLICITUDES_AUTORIZADAS"] = vDatos;
                }

            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void DDLProceso_SelectedIndexChanged(object sender, EventArgs e){
            try{
                if (DDLProceso.SelectedValue == "1")
                    Response.Redirect("/pages/activos/activosInternos.aspx");
                if (DDLProceso.SelectedValue == "2")
                    Response.Redirect("/pages/activos/visitas.aspx");
                if (DDLProceso.SelectedValue == "4")
                    Response.Redirect("/pages/activos/soporte.aspx");
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void TxSolicitud_TextChanged(object sender, EventArgs e){
            try{
                if (TxSolicitud.Text != "" || TxSolicitud.Text != string.Empty){
                    String vQuery = "RSP_ActivosDC 24," + TxSolicitud.Text;
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                    GVBusquedas.DataSource = vDatos;
                    GVBusquedas.DataBind();
                    Session["ACTIVOS_DC_ENTRADA"] = vDatos;
                        
                    if (vDatos.Rows.Count < 1)
                        throw new Exception("La solicitud no esta aprobada o no existe.");
                }else
                    throw new Exception("Por favor ingrese el número de solicitud.");
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GVBusquedas_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                Response.Redirect("visitaDCPersonal.aspx?id=" + TxSolicitud.Text);
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
    }
}