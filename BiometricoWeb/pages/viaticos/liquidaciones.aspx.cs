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
    public partial class liquidaciones : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Convert.ToBoolean(Session["AUTH"])){
                    cargarData();
                }else{
                    Response.Redirect("/login.aspx");
                }
            }
        }
        void cargarData()
        {
            string vUsuario = Convert.ToString(Session["USUARIO"]);
            string vEstadoSolicitud = "7";
            DataTable vDato = new DataTable();
            vDato = vConexion.obtenerDataTable("VIATICOS_ObtenerGenerales 18, '" + vEstadoSolicitud + "','"+ vUsuario + "'");
            GVSolicitud.DataSource = vDato;
            GVSolicitud.DataBind();
            Session["VIATICOS_BUSCAR_LIQUIDAR"] = vDato;
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
                        Session["VIATICOS_CODIGO"] = codViaticos;
                        Session["VIATICOS_LIQ_FECHA_INICIO"] = Convert.ToDateTime(item["FechaI"]).ToString("dd/MM/yyyy HH:mm:ss");
                        Session["VIATICOS_LIQ_FECHA_FIN"] = Convert.ToDateTime(item["FechaF"]).ToString("dd/MM/yyyy HH:mm:ss");
                        Session["VIATICOS_LIQ_IDEMPLEADO"] = item["IDEmpleado"].ToString();
                        Session["VIATICOS_LIQ_EMERGENCIA"] = item["Emergencia"].ToString();
                        Session["VIATICOS_LIQ_NEWPAIS"] = item["NewPais"].ToString();
                        Session["VIATICOS_LIQ_NEWHOTEL"] = item["NewHotel"].ToString();
                        Session["VIATICOS_LIQ_DESAYUNO"] = item["Desayuno"].ToString();
                        Session["VIATICOS_LIQ_COMENTARIOVEHICULO"] = item["ComentarioVehiculo"].ToString();
                        Session["VIATICOS_LIQ_COSTOHOSPEDAJE"] = item["CostoHospedaje"].ToString();
                        Session["VIATICOS_LIQ_COSTODESAYUNO"] = item["CostoDesayuno"].ToString();
                        Session["VIATICOS_LIQ_COSTOCENA"] = item["CostoCena"].ToString();
                        Session["VIATICOS_LIQ_COSTOALMUERZO"] = item["CostoAlmuerzo"].ToString();
                        Session["VIATICOS_LIQ_COSTODEPRE"] = item["CostoDepre"].ToString();
                        Session["VIATICOS_COSTODEPRE"] = item["CostoDepre"].ToString();
                        Session["VIATICOS_LIQ_COSTOTRANSPORTE"] = item["CostoTransporte"].ToString();
                        Session["VIATICOS_LIQ_COSTOEMERGENCIA"] = item["CostoEmergencia"].ToString();
                        Session["VIATICOS_LIQ_COSTOPEAJE"] = item["CostoPeaje"].ToString();
                        Session["VIATICOS_LIQ_COSTOCIRCULA"] = item["CostoCirculacion"].ToString();
                        Session["VIATICOS_LIQ_SUBTOTAL"] = item["SubTotal"].ToString();
                        Session["VIATICOS_LIQ_TOTAL"] = item["Total"].ToString();
                        Session["VIATICOS_LIQ_MOTIVOVIAJE"] = item["MotivoViaje"].ToString();
                        Session["VIATICOS_LIQ_TIPOVIAJE"] = item["TipoViaje"].ToString();
                        Session["VIATICOS_LIQ_EMPLEADO"] = item["Empleado"].ToString();
                        Session["VIATICOS_LIQ_CORREO"] = item["Correo"].ToString();
                        Session["VIATICOS_LIQ_IDJEFE"] = item["Jefe"].ToString();
                        Session["VIATICOS_LIQ_DESTINOI"] = item["DestinoI"].ToString();
                        Session["VIATICOS_LIQ_DESTINOF"] = item["DestinoF"].ToString();
                        Session["VIATICOS_LIQ_SAP"] = item["CodSAP"].ToString();
                        Session["VIATICOS_LIQ_HOTEL"] = item["Hotel"].ToString();
                        Session["VIATICOS_LIQ_HABITACION"] = item["Habitacion"].ToString();
                        Session["VIATICOS_LIQ_VEHICULO"] = item["Vehiculo"].ToString();
                        Session["VIATICOS_LIQ_PUESTO"] = item["Puesto"].ToString();
                        Session["VIATICOS_LIQ_IDMOTIVOVIAJE"] = item["IDMotivoViaje"].ToString();
                        Session["VIATICOS_LIQ_IDTIPOVIAJE"] = item["IDTipoViaje"].ToString();
                        Session["VIATICOS_LIQ_IDVEHICULO"] = item["IDVehiculo"].ToString();
                        //Session["VIATICOS_LIQ_IDDESTINO"] = item["IDDestino"].ToString();
                        Session["VIATICOS_LIQ_IDTRANSPORTE"] = item["Transporte"].ToString();
                        Session["CATEGORIA_LIQ_VIATICOS"] = item["Categoria"].ToString();
                        Session["VIATICOS_LIQ_COMJEFE"] = item["ComentarioJefe"].ToString();
                        Session["VIATICOS_LIQ_COMRRHH"] = item["ComentarioRRHH"].ToString();
                        Session["VIATICOS_LIQ_COMCONTA"] = item["ComentarioConta"].ToString();
                        Session["VIATICOS _LIQ_COMGERENTE"] = item["ComentarioGerente"].ToString();
                    }



                    Response.Redirect("liquidar.aspx");
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}