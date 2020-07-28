using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace BiometricoWeb.pages.viaticos
{
    public partial class cotizacion : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Convert.ToBoolean(Session["AUTH"])){
                    limpiarSession();
                    cargarData();
                } else{
                    Response.Redirect("/login.aspx");
                }
            }
        }
        void limpiarSession()
        {
            Session["VIATICOS_CODIGO"] = null;
            Session["VIATICOS_FECHA_INICIO"] = null;
            Session["VIATICOS_FECHA_FIN"] = null;
            Session["VIATICOS_EMPLEADO"] = null;
            Session["VIATICOS_CORREO"] = null;
            Session["VIATICOS_MOTIVOVIAJE"] = null;
            Session["VIATICOS_TIPOVIAJE"] = null;
            Session["VIATICOS_DESTINO"] = null;
            Session["VIATICOS_COMENTARIORRHH"] = null;
            Session["VIATICOS_NEWPAIS"] = null;
            Session["VIATICOS_IDEMPLEADO"] = null;
        }
        void cargarData()
        {
            if (Convert.ToString(Session["USUARIO"]) == "305")
            {
                //string vUsuario = Convert.ToString(Session["USUARIO"]);
                string vEstadoSolicitud = "3";
                DataTable vDato = new DataTable();
                vDato = vConexion.obtenerDataTable("VIATICOS_ObtenerGenerales 19, '" + vEstadoSolicitud + "'");
                GVSolicitud.DataSource = vDato;
                GVSolicitud.DataBind();
                Session["VIATICOS_COTIZACION"] = vDato;
            }
        }
        protected void GVSolicitud_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string codViaticos = e.CommandArgument.ToString();
            if (e.CommandName == "Aprobar")
            {
                try
                {
                    DataTable vDatos = new DataTable();
                    vDatos = vConexion.obtenerDataTable("VIATICOS_ObtenerGenerales 16,'" + codViaticos + "'");
                    //vDatos = vConexion.ObtenerTabla(vQuery);
                    foreach (DataRow item in vDatos.Rows)
                    {

                        Session["VIATICOS_CODIGO"] = codViaticos;
                        Session["VIATICOS_FECHA_INICIO"] = Convert.ToDateTime(item["FechaI"]).ToString("dd/MM/yyyy HH:mm:ss");
                        Session["VIATICOS_FECHA_FIN"] = Convert.ToDateTime(item["FechaF"]).ToString("dd/MM/yyyy HH:mm:ss");                                                                    
                        Session["VIATICOS_EMPLEADO"] = item["Empleado"].ToString();
                        Session["VIATICOS_IDEMPLEADO"] = item["IDEmpleado"].ToString(); 
                        Session["VIATICOS_CORREO"] = item["Correo"].ToString();
                        Session["VIATICOS_MOTIVOVIAJE"] = item["MotivoViaje"].ToString();
                        Session["VIATICOS_TIPOVIAJE"] = item["TipoViaje"].ToString();
                        Session["VIATICOS_DESTINOF"] = item["DestinoF"].ToString();
                        Session["VIATICOS_COMENTARIORRHH"] = item["ComentarioRRHH"].ToString();
                        Session["VIATICOS_NEWPAIS"] = item["NewPais"].ToString();
                    }



                    Response.Redirect("cotizarViaje.aspx");
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}