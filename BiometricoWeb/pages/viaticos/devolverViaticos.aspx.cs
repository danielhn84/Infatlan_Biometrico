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
    public partial class devolverViaticos : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                limpiarSession();
                cargarData();
            }
        }
        void limpiarSession()
        {
            Session["VIATICOS_LIQ_COSTODEPRE"] = null;
            Session["VIATICOS_LIQ_CODIGO"] = null;
            Session["VIATICOS_CODIGO"] = null;
            Session["VIATICOS_FECHA_INICIO"] = null;
            Session["VIATICOS_FECHA_FIN"] = null;
            Session["VIATICOS_IDEMPLEADO"] = null;
            Session["VIATICOS_EMERGENCIA"] = null;
            Session["VIATICOS_NEWPAIS"] = null;
            Session["VIATICOS_NEWHOTEL"] = null;
            Session["VIATICOS_DESAYUNO"] = null;
            Session["VIATICOS_COMENTARIOVEHICULO"] = null;
            Session["VIATICOS_COSTOHOSPEDAJE"] = null;
            Session["VIATICOS_COSTODESAYUNO"] = null;
            Session["VIATICOS_COSTOCENA"] = null;
            Session["VIATICOS_COSTOALMUERZO"] = null;
            Session["VIATICOS_COSTODEPRE"] = null;
            Session["VIATICOS_COSTOTRANSPORTE"] = null;
            Session["VIATICOS_COSTOEMERGENCIA"] = null;
            Session["VIATICOS_COSTOPEAJE"] = null;
            Session["VIATICOS_COSTOCIRCULA"] = null;
            Session["VIATICOS_SUBTOTAL"] = null;
            Session["VIATICOS_TOTAL"] = null;
            Session["VIATICOS_MOTIVOVIAJE"] = null;
            Session["VIATICOS_TIPOVIAJE"] = null;
            Session["VIATICOS_EMPLEADO"] = null;
            Session["VIATICOS_CORREO"] = null;
            Session["VIATICOS_IDJEFE"] = null;
            Session["VIATICOS_DESTINO"] = null;
            Session["VIATICOS_SAP"] = null;
            Session["VIATICOS_HOTEL"] = null;
            Session["VIATICOS_HABITACION"] = null;
            Session["VIATICOS_VEHICULO"] = null;
            Session["VIATICOS_PUESTO"] = null;
            Session["VIATICOS_IDMOTIVOVIAJE"] = null;
            Session["VIATICOS_IDTIPOVIAJE"] = null;
            Session["VIATICOS_IDVEHICULO"] = null;
            Session["VIATICOS_IDDESTINO"] = null;
            Session["VIATICOS_IDTRANSPORTE"] = null;
            Session["CATEGORIA_VIATICOS"] = null;
            Session["VIATICOS_COMJEFE"] = null;
            Session["VIATICOS_COMRRHH"] = null;
            Session["VIATICOS_COMCONTA"] = null;
            Session["VIATICOS_COMGERENTE"] = null;
            Session["VIATICOS_LIQ_TOTAL"] = null;
        }
        void cargarData()
        {
            //CARGAR SOLICITUDES A APROBAR
            string vEstadoSolicitud = "14";
            string vUsuario = Convert.ToString(Session["USUARIO"]);
            DataTable vDato = new DataTable();
            vDato = vConexion.obtenerDataTable("VIATICOS_ObtenerGenerales 18, '" + vEstadoSolicitud + "', '" + vUsuario + "'");
            GVSolicitud.DataSource = vDato;
            GVSolicitud.DataBind();
            Session["VIATICOS_APROB_SOLICITUD"] = vDato;

            //CARGAR LIQUIDACIONES A APROBAR
            string vEstadoSolicitudL = "15";
            DataTable vDato2 = new DataTable();
            vDato2 = vConexion.obtenerDataTable("VIATICOS_ObtenerGenerales 18, '" + vEstadoSolicitudL + "', '" + vUsuario + "'");
            GVLiquidacion.DataSource = vDato2;
            GVLiquidacion.DataBind();
            Session["VIATICOS_APROB_LIQUIDACION"] = vDato2;
        }
        protected void GVSolicitud_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable vDataaaa = (DataTable)Session["VIATICOS_APROB_SOLICITUD"];
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
                        Session["VIATICOS_IDEMPLEADO"] = item["IDEmpleado"].ToString();
                        Session["VIATICOS_EMERGENCIA"] = item["Emergencia"].ToString();
                        Session["VIATICOS_NEWPAIS"] = item["NewPais"].ToString();
                        Session["VIATICOS_NEWHOTEL"] = item["NewHotel"].ToString();
                        Session["VIATICOS_DESAYUNO"] = item["Desayuno"].ToString();
                        Session["VIATICOS_COMENTARIOVEHICULO"] = item["ComentarioVehiculo"].ToString();
                        Session["VIATICOS_COSTOHOSPEDAJE"] = item["CostoHospedaje"].ToString();
                        Session["VIATICOS_COSTODESAYUNO"] = item["CostoDesayuno"].ToString();
                        Session["VIATICOS_COSTOCENA"] = item["CostoCena"].ToString();
                        Session["VIATICOS_COSTOALMUERZO"] = item["CostoAlmuerzo"].ToString();
                        Session["VIATICOS_COSTODEPRE"] = item["CostoDepre"].ToString();
                        Session["VIATICOS_COSTOTRANSPORTE"] = item["CostoTransporte"].ToString();
                        Session["VIATICOS_COSTOEMERGENCIA"] = item["CostoEmergencia"].ToString();
                        Session["VIATICOS_COSTOPEAJE"] = item["CostoPeaje"].ToString();
                        Session["VIATICOS_COSTOCIRCULA"] = item["CostoCirculacion"].ToString();
                        Session["VIATICOS_SUBTOTAL"] = item["SubTotal"].ToString();
                        Session["VIATICOS_TOTAL"] = item["Total"].ToString();
                        Session["VIATICOS_MOTIVOVIAJE"] = item["MotivoViaje"].ToString();
                        Session["VIATICOS_TIPOVIAJE"] = item["TipoViaje"].ToString();
                        Session["VIATICOS_EMPLEADO"] = item["Empleado"].ToString();
                        Session["VIATICOS_CORREO"] = item["Correo"].ToString();
                        Session["VIATICOS_IDJEFE"] = item["Jefe"].ToString();
                        Session["VIATICOS_DESTINOI"] = item["DestinoI"].ToString();
                        Session["VIATICOS_DESTINOF"] = item["DestinoF"].ToString();
                        Session["VIATICOS_SAP"] = item["CodSAP"].ToString();
                        Session["VIATICOS_HOTEL"] = item["Hotel"].ToString();
                        Session["VIATICOS_HABITACION"] = item["Habitacion"].ToString();
                        Session["VIATICOS_VEHICULO"] = item["Vehiculo"].ToString();
                        Session["VIATICOS_PUESTO"] = item["Puesto"].ToString();
                        Session["VIATICOS_IDMOTIVOVIAJE"] = item["IDMotivoViaje"].ToString();
                        Session["VIATICOS_IDTIPOVIAJE"] = item["IDTipoViaje"].ToString();
                        Session["VIATICOS_IDVEHICULO"] = item["IDVehiculo"].ToString();
                        //Session["VIATICOS_IDDESTINO"] = item["IDDestino"].ToString();
                        Session["VIATICOS_IDTRANSPORTE"] = item["Transporte"].ToString();
                        Session["CATEGORIA_VIATICOS"] = item["Categoria"].ToString();
                        Session["VIATICOS_COMJEFE"] = item["ComentarioJefe"].ToString();
                        Session["VIATICOS_COMRRHH"] = item["ComentarioRRHH"].ToString();
                        Session["VIATICOS_COMCONTA"] = item["ComentarioConta"].ToString();
                        Session["VIATICOS_COMGERENTE"] = item["ComentarioGerente"].ToString();
                    }



                    Response.Redirect("solicitudViaticos.aspx?id=1&tipo=2");
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        protected void GVLiquidacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GVLiquidacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable vDataaaa = (DataTable)Session["VIATICOS_APROB_LIQUIDACION"];
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
                        Session["VIATICOS_LIQ_CODIGO"] = codViaticos;
                        Session["VIATICOS_CODIGO"] = codViaticos;
                        Session["VIATICOS_FECHA_INICIO"] = Convert.ToDateTime(item["FechaI"]).ToString("dd/MM/yyyy HH:mm:ss");
                        Session["VIATICOS_FECHA_FIN"] = Convert.ToDateTime(item["FechaF"]).ToString("dd/MM/yyyy HH:mm:ss");
                        Session["VIATICOS_IDEMPLEADO"] = item["IDEmpleado"].ToString();
                        Session["VIATICOS_EMERGENCIA"] = item["Emergencia"].ToString();
                        Session["VIATICOS_NEWPAIS"] = item["NewPais"].ToString();
                        Session["VIATICOS_NEWHOTEL"] = item["NewHotel"].ToString();
                        Session["VIATICOS_DESAYUNO"] = item["Desayuno"].ToString();
                        Session["VIATICOS_COMENTARIOVEHICULO"] = item["ComentarioVehiculo"].ToString();
                        Session["VIATICOS_COSTOHOSPEDAJE"] = item["CostoHospedaje"].ToString();
                        Session["VIATICOS_COSTODESAYUNO"] = item["CostoDesayuno"].ToString();
                        Session["VIATICOS_COSTOCENA"] = item["CostoCena"].ToString();
                        Session["VIATICOS_COSTOALMUERZO"] = item["CostoAlmuerzo"].ToString();
                        Session["VIATICOS_LIQ_COSTODEPRE"] = item["CostoDepre"].ToString();
                        Session["VIATICOS_COSTOTRANSPORTE"] = item["CostoTransporte"].ToString();
                        Session["VIATICOS_COSTOEMERGENCIA"] = item["CostoEmergencia"].ToString();
                        Session["VIATICOS_COSTOPEAJE"] = item["CostoPeaje"].ToString();
                        Session["VIATICOS_COSTOCIRCULA"] = item["CostoCirculacion"].ToString();
                        Session["VIATICOS_SUBTOTAL"] = item["SubTotal"].ToString();
                        Session["VIATICOS_LIQ_TOTAL"] = item["Total"].ToString();
                        Session["VIATICOS_MOTIVOVIAJE"] = item["MotivoViaje"].ToString();
                        Session["VIATICOS_TIPOVIAJE"] = item["TipoViaje"].ToString();
                        Session["VIATICOS_LIQ_EMPLEADO"] = item["Empleado"].ToString();
                        Session["VIATICOS_CORREO"] = item["Correo"].ToString();
                        Session["VIATICOS_IDJEFE"] = item["Jefe"].ToString();
                        Session["VIATICOS_DESTINOI"] = item["DestinoI"].ToString();
                        Session["VIATICOS_DESTINOF"] = item["DestinoF"].ToString();
                        Session["VIATICOS_LIQ_SAP"] = item["CodSAP"].ToString();
                        Session["VIATICOS_HOTEL"] = item["Hotel"].ToString();
                        Session["VIATICOS_HABITACION"] = item["Habitacion"].ToString();
                        Session["VIATICOS_VEHICULO"] = item["Vehiculo"].ToString();
                        Session["VIATICOS_LIQ_PUESTO"] = item["Puesto"].ToString();
                        Session["VIATICOS_IDMOTIVOVIAJE"] = item["IDMotivoViaje"].ToString();
                        Session["VIATICOS_LIQ_IDTIPOVIAJE"] = item["IDTipoViaje"].ToString();
                        Session["VIATICOS_IDVEHICULO"] = item["IDVehiculo"].ToString();
                        //Session["VIATICOS_IDDESTINO"] = item["IDDestino"].ToString();
                        Session["VIATICOS_IDTRANSPORTE"] = item["Transporte"].ToString();
                        Session["CATEGORIA_VIATICOS"] = item["Categoria"].ToString();
                        Session["VIATICOS_COMJEFE"] = item["ComentarioJefe"].ToString();
                        Session["VIATICOS_COMRRHH"] = item["ComentarioRRHH"].ToString();
                        Session["VIATICOS_COMCONTA"] = item["ComentarioConta"].ToString();
                        Session["VIATICOS_COMGERENTE"] = item["ComentarioGerente"].ToString();
                        Session["VIATICOS_ESTADO"] = item["Estado"].ToString();
                        
                        
                    }



                    Response.Redirect("liquidar.aspx?id=1&tipo=2");
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}