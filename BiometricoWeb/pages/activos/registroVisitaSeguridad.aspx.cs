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
        db vConexion;
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            vConexion = new db();

            if (!IsPostBack)
            {
                CargarSolicitudesAutorizadas();

            }
        }

        void CargarSolicitudesAutorizadas()
        {
            try
            {
                DataTable vDatos = new DataTable();
                String vQuery = "RSP_ActivosDC 24" ;
                vDatos = vConexion.obtenerDataTable(vQuery);

                if (vDatos.Rows.Count > 0)
                {
                    GVBusquedas.DataSource = vDatos;
                    GVBusquedas.DataBind();
                    UpdateDivAutorizadas.Update();
                    Session["ACTIVO_DC_SOLICITUDES_AUTORIZADAS"] = vDatos;
                }

            }
            catch (Exception Ex)
            {
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
    }
}