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
    public partial class buscarRecibo : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                limpiarForm();
                cargarData();
            }
        }

        void limpiarSession()
        {
            Session["VIATICOS_LIQ_CODIGO"] = null;
            Session["VIATICOS_LIQ_TOTAL"] = null;
            Session["VIATICOS_LIQ_TIPOVIAJE"] = null;
            Session["VIATICOS_LIQ_EMPLEADO"] = null;
            Session["VIATICOS_LIQ_CORREO"] = null;
            Session["VIATICOS_LIQ_IDJEFE"] = null;
            Session["VIATICOS_LIQ_SAP"] = null;
            Session["VIATICOS_LIQ_PUESTO"] = null;
            Session["VIATICOS_LIQ_IDTIPOVIAJE"] = null;
        }
        void limpiarForm()
        {
            Session["VIATICOS_LIQ_CODIGO"] = null;
            Session["VIATICOS_LIQ_TOTAL"] = null;
            Session["VIATICOS_LIQ_TIPOVIAJE"] = null;
            Session["VIATICOS_LIQ_EMPLEADO"] = null;
            Session["VIATICOS_LIQ_CORREO"] = null;
            Session["VIATICOS_LIQ_IDJEFE"] = null;
            Session["VIATICOS_LIQ_SAP"] = null;
            Session["VIATICOS_LIQ_PUESTO"] = null;
            Session["VIATICOS_LIQ_IDTIPOVIAJE"] = null;
            Session["MONTOLIQUIDADOR"] = null;
            Session["IMG_RECIBO"] = null;
            Session["COMENTARIO_RECIBO"] = null;
            Session["VIATICOS_COSTODEPRE"] = null;
        }
        void cargarData()
        {
            string vUsuario = Convert.ToString(Session["USUARIO"]);
            string vEstadoSolicitud = "";
            if (vUsuario == "389")
            {
                vEstadoSolicitud = "12";
                DataTable vDato2 = new DataTable();
                vDato2 = vConexion.obtenerDataTable("VIATICOS_ObtenerGenerales 19, '" + vEstadoSolicitud + "'");
                GVARecibo.DataSource = vDato2;
                GVARecibo.DataBind();
                Session["VIATICOS_RECIBOS_APROBAR"] = vDato2;
            }
           
                vEstadoSolicitud = "11";
                DataTable vDato = new DataTable();
                vDato = vConexion.obtenerDataTable("VIATICOS_ObtenerGenerales 18, '" + vEstadoSolicitud + "','" + vUsuario + "'");
                GVSolicitud.DataSource = vDato;
                GVSolicitud.DataBind();
                Session["VIATICOS_RECIBOS_LIQUIDAR"] = vDato;
           
        }
        protected void GVSolicitud_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            string codViaticos = e.CommandArgument.ToString();
            if (e.CommandName == "Crear")
            {
                try
                {
                    DataTable vDatos = new DataTable();
                    vDatos = vConexion.obtenerDataTable("VIATICOS_ObtenerGenerales 16,'" + codViaticos + "'");
                    //vDatos = vConexion.ObtenerTabla(vQuery);
                    foreach (DataRow item in vDatos.Rows)
                    {

                        Session["VIATICOS_LIQ_CODIGO"] = codViaticos;                                              
                        Session["VIATICOS_LIQ_TOTAL"] = item["Total"].ToString();
                        Session["VIATICOS_LIQ_TIPOVIAJE"] = item["TipoViaje"].ToString();
                        Session["VIATICOS_LIQ_EMPLEADO"] = item["Empleado"].ToString();
                        Session["VIATICOS_LIQ_CORREO"] = item["Correo"].ToString();
                        Session["VIATICOS_LIQ_IDJEFE"] = item["Jefe"].ToString();                       
                        Session["VIATICOS_LIQ_SAP"] = item["CodSAP"].ToString();                        
                        Session["VIATICOS_LIQ_PUESTO"] = item["Puesto"].ToString();                        
                        Session["VIATICOS_LIQ_IDTIPOVIAJE"] = item["IDTipoViaje"].ToString();
                        Session["VIATICOS_COSTODEPRE"] = item["CostoDepre"].ToString();
                    }



                    Response.Redirect("recibos.aspx");
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        protected void GVARecibo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string codViaticos = e.CommandArgument.ToString();
            if (e.CommandName == "Crear")
            {
                try
                {
                    DataTable vDatos = new DataTable();
                    vDatos = vConexion.obtenerDataTable("VIATICOS_ObtenerGenerales 16,'" + codViaticos + "'");
                    //vDatos = vConexion.ObtenerTabla(vQuery);
                    foreach (DataRow item in vDatos.Rows)
                    {

                        Session["VIATICOS_LIQ_CODIGO"] = codViaticos;
                        Session["VIATICOS_LIQ_TOTAL"] = item["Total"].ToString();
                        Session["VIATICOS_LIQ_TIPOVIAJE"] = item["TipoViaje"].ToString();
                        Session["VIATICOS_LIQ_EMPLEADO"] = item["Empleado"].ToString();
                        Session["VIATICOS_LIQ_CORREO"] = item["Correo"].ToString();
                        Session["VIATICOS_LIQ_IDJEFE"] = item["Jefe"].ToString();
                        Session["VIATICOS_LIQ_SAP"] = item["CodSAP"].ToString();
                        Session["VIATICOS_LIQ_PUESTO"] = item["Puesto"].ToString();
                        Session["VIATICOS_LIQ_IDTIPOVIAJE"] = item["IDTipoViaje"].ToString();
                        Session["VIATICOS_COSTODEPRE"] = item["CostoDepre"].ToString();
                    }



                    Response.Redirect("recibos.aspx?id=1&tipo=1");
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}