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
    public partial class visitaDatacenterPendienteResponsable : System.Web.UI.Page
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
                CargarSolicitudesPendientesAprobar();

            }
        }

        void CargarSolicitudesPendientesAprobar(){
            try{
                DataTable vDatos = new DataTable();
                String vQuery = "RSP_ActivosDC 18,'" + Convert.ToString(Session["USUARIO"]) + "'";
                vDatos = vConexion.obtenerDataTable(vQuery);

                if (vDatos.Rows.Count > 0){
                    GVBusquedaPendientesResponsables.DataSource = vDatos;
                    GVBusquedaPendientesResponsables.DataBind();
                    UpdateDivBusquedasResponsables.Update();
                    Session["ACTIVO_DC_PENDIENTES_RESPONSABLES"] = vDatos;
                }

            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void GVBusquedaPendientesResponsables_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vIdSolicitud = e.CommandArgument.ToString();
            if (e.CommandName == "Aprobar"){
                DataTable vDatos = new DataTable();
                //DATOS GENERALES
                string vQuery = "RSP_ActivosDC 14," + vIdSolicitud;
                vDatos = vConexion.obtenerDataTable(vQuery);
                Session["ACTIVO_DC_SOLI_DATOS_GENERALES"] = vDatos;


                string vResponsable = vDatos.Rows[0]["responsable"].ToString();
                string vCopia = vDatos.Rows[0]["copia"].ToString();
                string vSupervisar = vDatos.Rows[0]["supervisorProveedor"].ToString();

                vQuery = "RSP_ActivosDC 1,'" + vResponsable + "'";
                vDatos = vConexion.obtenerDataTable(vQuery);
                Session["ACTIVO_DC_SOLI_DATOS_RESPONSABLE"] = vDatos;

                vQuery = "RSP_ActivosDC 3,'" + vCopia + "'";
                vDatos = vConexion.obtenerDataTable(vQuery);
                Session["ACTIVO_DC_SOLI_DATOS_COPIA"] = vDatos;

                vQuery = "RSP_ActivosDC 3,'" + vSupervisar + "'";
                vDatos = vConexion.obtenerDataTable(vQuery);
                Session["ACTIVO_DC_SOLI_DATOS_SUPERVISOR"] = vDatos;

                vQuery = "RSP_ActivosDC 15,'" + vIdSolicitud + "'";
                vDatos = vConexion.obtenerDataTable(vQuery);
                Session["ACTIVO_DC_SOLI_PERSONAL_EXTERNO"] = vDatos;

                vQuery = "RSP_ActivosDC 16,'" + vIdSolicitud + "'";
                vDatos = vConexion.obtenerDataTable(vQuery);
                Session["ACTIVO_DC_SOLI_PERSONAL_INTERNO"] = vDatos;


                Response.Redirect("/pages/activos/visitaDatacenter.aspx?ex=2");


            }
        }
    }
}