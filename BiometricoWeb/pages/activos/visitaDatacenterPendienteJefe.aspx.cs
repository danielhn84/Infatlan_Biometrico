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
    public partial class visitaDatacenterPendienteJefe : System.Web.UI.Page
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
        void CargarSolicitudesPendientesAprobar()
        {
            try
            {
                DataTable vDatos = new DataTable();
                String vQuery = "RSP_ActivosDC 13,'" + Convert.ToString(Session["USUARIO"]) + "'";
                vDatos = vConexion.obtenerDataTable(vQuery);

                if (vDatos.Rows.Count > 0)
                {
                    GVBusquedaPendientesJefe.DataSource = vDatos;
                    GVBusquedaPendientesJefe.DataBind();
                    UpdateDivBusquedasJefes.Update();
                    Session["ACTIVO_DC_PENDIENTES_JEFES"] = vDatos;
                }

            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void TxSolicitud_TextChanged(object sender, EventArgs e)
        {
            CargarSolicitudesPendientesAprobar();
            String vBusqueda = TxSolicitud.Text;
            DataTable vDatos = (DataTable)Session["ACTIVO_DC_PENDIENTES_JEFES"];

            if (vBusqueda.Equals(""))
            {
                GVBusquedaPendientesJefe.DataSource = vDatos;
                GVBusquedaPendientesJefe.DataBind();
                UpdateDivBusquedasJefes.Update();
            }
            else
            {
                EnumerableRowCollection<DataRow> filtered = vDatos.AsEnumerable()
                    .Where(r => r.Field<String>("nombre").Contains(vBusqueda.ToUpper()));

                Boolean isNumeric = int.TryParse(vBusqueda, out int n);

                if (isNumeric)
                {
                    if (filtered.Count() == 0)
                    {
                        filtered = vDatos.AsEnumerable().Where(r =>
                            Convert.ToInt32(r["idSolicitud"]) == Convert.ToInt32(vBusqueda));
                    }
                }

                DataTable vDatosFiltrados = new DataTable();
                vDatosFiltrados.Columns.Add("idSolicitud");
                vDatosFiltrados.Columns.Add("fechaInicio");
                vDatosFiltrados.Columns.Add("fechaFin");
                vDatosFiltrados.Columns.Add("acceso");
                vDatosFiltrados.Columns.Add("peticion");
                vDatosFiltrados.Columns.Add("trabajo");
                vDatosFiltrados.Columns.Add("motivo");
                vDatosFiltrados.Columns.Add("nombre");


                foreach (DataRow item in filtered)
                {
                    vDatosFiltrados.Rows.Add(
                        item["idSolicitud"].ToString(),
                        item["fechaInicio"].ToString(),
                        item["fechaFin"].ToString(),
                        item["acceso"].ToString(),
                        item["peticion"].ToString(),
                        item["trabajo"].ToString(),
                        item["motivo"].ToString(),
                        item["nombre"].ToString()
                        );
                }
                GVBusquedaPendientesJefe.DataSource = vDatosFiltrados;
                GVBusquedaPendientesJefe.DataBind();
                Session["ACTIVO_DC_PENDIENTES_JEFES"] = vDatosFiltrados;
                UpdateDivBusquedasJefes.Update();
            }
        }

        protected void GVBusquedaPendientesJefe_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVBusquedaPendientesJefe.PageIndex = e.NewPageIndex;
                GVBusquedaPendientesJefe.DataSource = (DataTable)Session["ACTIVO_DC_PENDIENTES_JEFES"];
                GVBusquedaPendientesJefe.DataBind();
                UpdateDivBusquedasJefes.Update();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusquedaPendientesJefe_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vIdSolicitud = e.CommandArgument.ToString();
            if (e.CommandName == "Aprobar")
            {
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


                Response.Redirect("/pages/activos/visitaDatacenter.aspx?ex=1");


            }
        }


    }
}