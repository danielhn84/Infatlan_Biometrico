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
using System.Configuration;

namespace BiometricoWeb.pages.viaticos
{
    public partial class solicitudViaticos : System.Web.UI.Page
    {
        db vConexion = new db();
        db vConexion2 = new db();
        SmtpService vService = new SmtpService();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["CARGAR_DATA_VIATICOS"] = null;
            if (!Page.IsPostBack)
            {
                if (Convert.ToBoolean(Session["AUTH"])) {
                    cargarData();

                    Session["VIATICOS_HRS"] = "0";
                    Session["VIATICOS_DIAS"] = "0";
                    Session["PRECIO_VIATICOS"] = "0";

                    String vEstado = "";
                    string usu = Convert.ToString(Session["USUARIO"]);
                    //VALIDAR NO TENGA SOLICITUD PENDIENTE
                    String vQueryE = "VIATICOS_ObtenerGenerales 14, '" + usu + "'";
                    DataTable vDatosE = vConexion.obtenerDataTable(vQueryE);
                    foreach (DataRow item in vDatosE.Rows)
                    {
                        vEstado = item["estado"].ToString();
                    }


                    if (vEstado == "1" || vEstado == "2" || vEstado == "3" || vEstado == "4" || vEstado == "5" || vEstado == "6" || vEstado == "14")
                    {
                        btnCalcular.Enabled = false;
                        BtnCrearPermiso.Enabled = false;
                        LBEstado.Visible = true;
                    }
                    else
                    {
                        btnCalcular.Enabled = true;
                        BtnCrearPermiso.Enabled = true;
                        LBEstado.Visible = false;
                    }

                    string id = Request.QueryString["id"];
                    string tipo = Request.QueryString["tipo"];
                    switch (tipo)
                    {
                        case "1":
                            btnCalcular.Enabled = true;
                            BtnCrearPermiso.Enabled = true;
                            LBEstado.Visible = false;
                            cargarAprobacion();
                            deshabilitarForm();
                            EncontrarPeajes();
                            BtnCancelarSolicitud.Visible = true;
                            BtnCrearPermiso.Text = "Aprobar Solicitud";
                            btnModarEnviar.Text = "Aprobar";
                            string vEstadoSolicitud = Session["VIATICOS_ESTADO"].ToString();
                            txtNewHotel.Text = Convert.ToString(Session["VIATICOS_NEWHOTEL"]);
                            if (Session["USUARIO"].ToString() == "3627" && vEstadoSolicitud == "4" && DDLTransporte.SelectedValue == "4" || vEstadoSolicitud == "6" && DDLTransporte.SelectedValue == "4")
                            {
                                DIVCotiza.Visible = true;
                                BtnDevolverCotizacion.Visible = true;
                                string vViaticos = Session["VIATICOS_CODIGO"].ToString();
                                String vQuery = "VIATICOS_ObtenerGenerales 22, '" + vViaticos + "'";
                                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                                foreach (DataRow item in vDatos.Rows)
                                {
                                    txtCompañia.Text = item["empresa"].ToString();
                                    txtcosto.Text = string.Format("{0:N2}", Convert.ToDecimal(item["costo"].ToString()));
                                    txtcomentario.Text = item["comentario"].ToString();
                                }
                            }
                            //calcularTotal();
                            break;
                        case "2":
                            btnCalcular.Enabled = true;
                            BtnCrearPermiso.Enabled = true;
                            LBEstado.Visible = false;
                            cargarAprobacion();
                            EncontrarPeajes();
                            DIVVehiculo.Visible = false;
                            DIVComentarioAprob.Visible = false;
                            BtnCancelar.Visible = false;
                            LBComentarioJefe.Text = "Comentario:  " + Convert.ToString(Session["VIATICOS_COMJEFE"]);
                            DDLDestinoI.Enabled = false;
                            txtNewHotel.Text = Convert.ToString(Session["VIATICOS_NEWHOTEL"]);
                            if (Convert.ToInt32(Session["VIATICOS_DIAS"]) > 0 && Convert.ToString(Session["VIATICOS_IDTIPOVIAJE"]) == "1")
                            {
                                DDLHotel.Enabled = true;
                                DDLHabitacion.Enabled = true;
                            }
                            //calcularTotal();
                            break;

                    }

                } else {
                    Response.Redirect("/login.aspx");
                }
            }
        }

        void deshabilitarForm()
        {
            CBEmergencia.Enabled = false;
            TxFechaInicio.Enabled = false;
            TxFechaRegreso.Enabled = false;
            DDLMotivoViaje.Enabled = false;
            DDLTipoViaje.Enabled = false;
            DDLTransporte.Enabled = false;
            txtMotivoVehiculo.Enabled = false;
            DDLEmpleado.Enabled = false;
            DDLDestinoI.Enabled = false;
            DDLDestinoF.Enabled = false;
            DDLHotel.Enabled = false;
            DDLHabitacion.Enabled = false;
            btnCalcular.Enabled = false;
            txtNewHotel.Enabled = false;
            txtNewPais.Enabled = false;
            CBDesayuno.Enabled = false;
            string vEstadoSolicitud = Session["VIATICOS_ESTADO"].ToString();
            if (vEstadoSolicitud == "4")
                DDLVehiculo.Enabled = false;
        }
        void cargarAprobacion()
        {
            //Session["VIATICOS_CODIGO"]

            String vFormato = "yyyy-MM-ddTHH:mm";   //"dd/MM/yyyy HH:mm";
            //String vFormato = "dd/MM/yyyy HH:mm:ss"; //LOCAL
            string vFechaInicio = Convert.ToDateTime(Session["VIATICOS_FECHA_INICIO"]).ToString(vFormato);
            string vFechaFin = Convert.ToDateTime(Session["VIATICOS_FECHA_FIN"]).ToString(vFormato);
            TxFechaInicio.Text = vFechaInicio.ToString();
            TxFechaRegreso.Text = vFechaFin;
            if (Session["VIATICOS_EMERGENCIA"].ToString() == "1")
            {
                CBEmergencia.Checked = true;
            }
            txtNewPais.Text = Convert.ToString(Session["VIATICOS_NEWPAIS"]);
            txtNewHotel.Text = Convert.ToString(Session["VIATICOS_NEWHOTEL"]);
            if (Session["VIATICOS_DESAYUNO"].ToString() == "1")
            {
                CBDesayuno.Checked = true;
            }
            txtMotivoVehiculo.Text = Convert.ToString(Session["VIATICOS_COMENTARIOVEHICULO"]);
            DDLTipoViaje.SelectedIndex = CargarInformacionDDL(DDLTipoViaje, Convert.ToString(Session["VIATICOS_IDTIPOVIAJE"]));
            cargarDestino();
            if (DDLTipoViaje.SelectedValue == "2")
            {
                DDLTransporte.Enabled = false;
                DIVPais.Visible = true;
            }
            DDLTransporte.SelectedIndex = CargarInformacionDDL(DDLTransporte, Convert.ToString(Session["VIATICOS_IDTRANSPORTE"]));
            DDLDestinoI.SelectedIndex = CargarInformacionDDL(DDLDestinoI, Convert.ToString(Session["VIATICOS_DESTINOI"]));
            DDLDestinoF.SelectedIndex = CargarInformacionDDL(DDLDestinoF, Convert.ToString(Session["VIATICOS_DESTINOF"]));
            cargarGeneralesDestino();
            DDLMotivoViaje.SelectedIndex = CargarInformacionDDL(DDLMotivoViaje, Convert.ToString(Session["VIATICOS_IDMOTIVOVIAJE"]));



            //HOTELES
            String vQuery5 = "VIATICOS_ObtenerGenerales 20,'" + DDLDestinoF.SelectedValue + "'";
            DataTable vDatos5 = vConexion.obtenerDataTable(vQuery5);
            DDLHotel.Items.Add(new ListItem { Value = "0", Text = "Seleccione hoteles..." });
            DDLHotel.Items.Add(new ListItem { Value = "x", Text = "Otros" });
            foreach (DataRow item in vDatos5.Rows)
            {
                DDLHotel.Items.Add(new ListItem { Value = item["idHotel"].ToString(), Text = item["nombre"].ToString() });

            }

            txtPuesto.Text = Convert.ToString(Session["VIATICOS_PUESTO"]);
            txtCodSAP.Text = Convert.ToString(Session["VIATICOS_SAP"]);

            string saber = Session["VIATICOS_SUBTOTAL"].ToString();

            DDLHotel.SelectedIndex = CargarInformacionDDL(DDLHotel, Convert.ToString(Session["VIATICOS_HOTEL"]));
            cargarHotelesHabitaciones();
            DDLHabitacion.SelectedIndex = CargarInformacionDDL(DDLHabitacion, Convert.ToString(Session["VIATICOS_HABITACION"]));
            cargarPrecioHoteles();
            cargarDiasHrs();
            cargarCalculo();
            DDLEmpleado.SelectedIndex = CargarInformacionDDL(DDLEmpleado, Convert.ToString(Session["VIATICOS_IDEMPLEADO"]));
            DDLVehiculo.SelectedIndex = CargarInformacionDDL(DDLVehiculo, Convert.ToString(Session["VIATICOS_IDVEHICULO"]));

            string vVehiculo = Session["VIATICOS_IDVEHICULO"].ToString();
            String vQuery = "VIATICOS_ObtenerGenerales 11, '" + vVehiculo + "'";
            DataTable vDatos = vConexion.obtenerDataTable(vQuery);
            foreach (DataRow item in vDatos.Rows)
            {
                txtplaca.Text = item["placa"].ToString();
                txtSerie.Text = item["serie"].ToString();
            }
            saber = Session["VIATICOS_SUBTOTAL"].ToString();
            //LBHospedaje.Text = Convert.ToString(Session["VIATICOS_COSTOHOSPEDAJE"]).Contains(",") ? Convert.ToString(Session["VIATICOS_COSTOHOSPEDAJE"]).Replace(',', '.') : Convert.ToString(Session["VIATICOS_COSTOHOSPEDAJE"]);
            //LBDesayuno.Text = Convert.ToString(Session["VIATICOS_COSTODESAYUNO"]).Contains(",") ? Convert.ToString(Session["VIATICOS_COSTODESAYUNO"]).Replace(',', '.') : Convert.ToString(Session["VIATICOS_COSTODESAYUNO"]);
            //LBCena.Text = Convert.ToString(Session["VIATICOS_COSTOCENA"]).Contains(",") ? Convert.ToString(Session["VIATICOS_COSTOCENA"]).Replace(',', '.') : Convert.ToString(Session["VIATICOS_COSTOCENA"]);
            //LBAlmuerzo.Text = Convert.ToString(Session["VIATICOS_COSTOALMUERZO"]).Contains(",") ? Convert.ToString(Session["VIATICOS_COSTOALMUERZO"]).Replace(',', '.') : Convert.ToString(Session["VIATICOS_COSTOALMUERZO"]);
            //LBDepresiacion.Text = Convert.ToString(Session["VIATICOS_COSTODEPRE"]).Contains(",") ? Convert.ToString(Session["VIATICOS_COSTODEPRE"]).Replace(',', '.') : Convert.ToString(Session["VIATICOS_COSTODEPRE"]);
            //LBTransporte.Text = Convert.ToString(Session["VIATICOS_COSTOTRANSPORTE"]).Contains(",") ? Convert.ToString(Session["VIATICOS_COSTOTRANSPORTE"]).Replace(',', '.') : Convert.ToString(Session["VIATICOS_COSTOTRANSPORTE"]);
            //LBEmergencia.Text = Convert.ToString(Session["VIATICOS_COSTOEMERGENCIA"]).Contains(",") ? Convert.ToString(Session["VIATICOS_COSTOEMERGENCIA"]).Replace(',', '.') : Convert.ToString(Session["VIATICOS_COSTOEMERGENCIA"]);
            //LBPeaje.Text = Convert.ToString(Session["VIATICOS_COSTOPEAJE"]).Contains(",") ? Convert.ToString(Session["VIATICOS_COSTOPEAJE"]).Replace(',', '.') : Convert.ToString(Session["VIATICOS_COSTOPEAJE"]);
            //LBCirculacion.Text = Convert.ToString(Session["VIATICOS_COSTOCIRCULA"]).Contains(",") ? Convert.ToString(Session["VIATICOS_COSTOCIRCULA"]).Replace(',', '.') : Convert.ToString(Session["VIATICOS_COSTOCIRCULA"]);

            //LBHospedaje.Text = Convert.ToString(Session["VIATICOS_COSTOHOSPEDAJE"]);
            //LBDesayuno.Text = Convert.ToString(Session["VIATICOS_COSTODESAYUNO"]);
            //LBCena.Text = Convert.ToString(Session["VIATICOS_COSTOCENA"]);
            //LBAlmuerzo.Text = Convert.ToString(Session["VIATICOS_COSTOALMUERZO"]);
            //LBDepresiacion.Text = Convert.ToString(Session["VIATICOS_COSTODEPRE"]);
            //LBTransporte.Text = Convert.ToString(Session["VIATICOS_COSTOTRANSPORTE"]);
            //LBEmergencia.Text = Convert.ToString(Session["VIATICOS_COSTOEMERGENCIA"]);
            //LBPeaje.Text = Convert.ToString(Session["VIATICOS_COSTOPEAJE"]);
            //LBCirculacion.Text = Convert.ToString(Session["VIATICOS_COSTOCIRCULA"]);

            //Decimal vSubTotal = Convert.ToDecimal(Session["VIATICOS_SUBTOTAL"]).ToString().Contains(",") ? Convert.ToDecimal(Session["VIATICOS_SUBTOTAL"].ToString().Replace(',', '.')) : Convert.ToDecimal(Session["VIATICOS_SUBTOTAL"]);
            //LBSubTotal.Text = Session["VIATICOS_SUBTOTAL"].ToString();
            //Decimal vTotal = Convert.ToDecimal(Session["VIATICOS_TOTAL"]).ToString().Contains(",") ? Convert.ToDecimal(Session["VIATICOS_TOTAL"].ToString().Replace(',', '.')) : Convert.ToDecimal(Session["VIATICOS_TOTAL"]);
            //LBTotal.Text = Session["VIATICOS_TOTAL"].ToString();


            txtNewHotel.Text = Convert.ToString(Session["VIATICOS_NEWHOTEL"]);

            if (Session["VIATICOS_IDTRANSPORTE"].ToString() == "2")
            {
                DIVMotivoVehiculo.Visible = true;
            }
            if (Session["VIATICOS_IDTRANSPORTE"].ToString() == "1")
            {
                DIVVehiculo.Visible = true;
            }
            if (Session["VIATICOS_IDTIPOVIAJE"].ToString() == "1" && Session["VIATICOS_HOTEL"].ToString() == "x")
            {
                DIVNuevoHotel.Visible = true;
            }
            if (Session["VIATICOS_IDTIPOVIAJE"].ToString() == "2" && Session["VIATICOS_HOTEL"].ToString() == "x")
            {
                DIVNuevoHotel.Visible = true;
                DIVPais.Visible = true;
            }

            BtnCancelar.Visible = true;
            DIVComentarioAprob.Visible = true;
        }

        void cargarDestino()
        {
            if (DDLTipoViaje.SelectedValue == "2")
            {


                DDLDestinoF.Items.Clear();
                //DESTINOS INTERNACIONAL
                String vQuery5 = "VIATICOS_ObtenerGenerales 12,'" + DDLTipoViaje.SelectedValue + "'";
                DataTable vDatos5 = vConexion.obtenerDataTable(vQuery5);
                DDLDestinoF.Items.Add(new ListItem { Value = "0", Text = "Seleccione destino..." });
                foreach (DataRow item in vDatos5.Rows)
                {
                    DDLDestinoF.Items.Add(new ListItem { Value = item["idDestino"].ToString(), Text = item["nombre"].ToString() });

                }


                DDLHotel.SelectedValue = "x";
                DDLHotel.Enabled = false;
                DDLHabitacion.SelectedValue = "0";
                DDLHabitacion.Enabled = false;
                if (Session["VIATICOS_DIAS"].ToString() == "0")
                {
                    DIVNuevoHotel.Visible = false;
                    txtNewHotel.Text = string.Empty;
                    DIVPais.Visible = false;
                }
                else
                {
                    DIVNuevoHotel.Visible = true;
                    txtNewHotel.Text = string.Empty;
                    DIVPais.Visible = true;
                }
                txtNewHotel.Text = Convert.ToString(Session["VIATICOS_NEWHOTEL"]);
                txtNewPais.Text = Convert.ToString(Session["VIATICOS_NEWPAIS"]);
                LBMonedaAlmuerzo.Text = "$";
                LBMonedaCena.Text = "$";
                LBMonedaCirculacion.Text = "$";
                LBMonedaDepresiacion.Text = "$";
                LBMonedaDesayuno.Text = "$";
                LBMonedaEmergencia.Text = "$";
                LBMonedaHospedaje.Text = "$";
                LBMonedaPeaje.Text = "$";
                LBMonedaSubTotal.Text = "$";
                LBMonedaTotal.Text = "$";
                LBMonedaTransporte.Text = "$";
                txtCostoHotel.Text = "$ 0";
                LBMonedaTSoli.Text = "$";
            }
            else
            {
                if (Session["VIATICOS_DIAS"].ToString() != "0")
                {
                    DDLHotel.SelectedValue = "0";
                    DDLHotel.Enabled = true;
                    DDLHabitacion.SelectedValue = "0";
                    DDLHabitacion.Enabled = true;
                    DIVNuevoHotel.Visible = false;
                    DIVPais.Visible = false;
                }
                LBMonedaAlmuerzo.Text = "L.";
                LBMonedaCena.Text = "L.";
                LBMonedaCirculacion.Text = "L.";
                LBMonedaDepresiacion.Text = "L.";
                LBMonedaDesayuno.Text = "L.";
                LBMonedaEmergencia.Text = "L.";
                LBMonedaHospedaje.Text = "L.";
                LBMonedaPeaje.Text = "L.";
                LBMonedaSubTotal.Text = "L.";
                LBMonedaTotal.Text = "L.";
                LBMonedaTransporte.Text = "L.";
                txtCostoHotel.Text = "L. 0";
                LBMonedaTSoli.Text = "L.";
            }
        }

        void cargarGeneralesDestino()
        {
            if (DDLEmpleado.SelectedValue != "0")
            {

                try
                {
                    //HOTELES
                    String vQuery5 = "VIATICOS_ObtenerGenerales 20,'" + DDLDestinoF.SelectedValue + "'";
                    DataTable vDatos5 = vConexion.obtenerDataTable(vQuery5);
                    DDLHotel.Items.Add(new ListItem { Value = "0", Text = "Seleccione hoteles..." });
                    DDLHotel.Items.Add(new ListItem { Value = "x", Text = "Otros" });
                    foreach (DataRow item in vDatos5.Rows)
                    {
                        DDLHotel.Items.Add(new ListItem { Value = item["idHotel"].ToString(), Text = item["nombre"].ToString() });

                    }
                }
                catch (Exception Ex)
                {

                    Mensaje(Ex.Message, WarningType.Danger);
                }
                if (DDLTipoViaje.SelectedValue == "2")
                    DDLHotel.SelectedValue = "x";

            }
        }

        void cargarPrecioHoteles()
        {
            if (DDLHotel.SelectedValue == "x" && DDLTipoViaje.SelectedValue == "1")
            {
                String vQuery = "VIATICOS_ObtenerGenerales 43,'" + Session["CATEGORIA_VIATICOS"] + "','" + DDLTipoViaje.SelectedValue + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                foreach (DataRow item in vDatos.Rows)
                {
                    txtCostoHotel.Text = "L." + string.Format("{0:N2}", Convert.ToDecimal(item["hospedaje"].ToString())); 
                }

            }
            else if (DDLHotel.SelectedValue == "x" && DDLTipoViaje.SelectedValue == "2")
            {
                txtCostoHotel.Text = "L.  0";
            }
            else
            {
                try
                {
                    //HABITACIONES
                    String vQuery = "VIATICOS_ObtenerGenerales 7,'" + DDLHabitacion.SelectedValue + "'";
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                    foreach (DataRow item in vDatos.Rows)
                    {
                        Session["PRECIO_VIATICOS"] = Convert.ToInt32(item["precio"].ToString()) * (24.93);
                        txtCostoHotel.Text = "L. " + string.Format("{0:N2}", Convert.ToDecimal(Session["PRECIO_VIATICOS"]));
                    }
                }
                catch (Exception Ex)
                {

                    Mensaje(Ex.Message, WarningType.Danger);
                }
            }
        }

        void cargarHotelesHabitaciones()
        {
            if (DDLHotel.SelectedValue == "x")
            {
                String vQuery = "VIATICOS_ObtenerGenerales 43,'" + Session["CATEGORIA_VIATICOS"] + "','" + DDLTipoViaje.SelectedValue + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                foreach (DataRow item in vDatos.Rows)
                {
                    Session["PRECIO_VIATICOS"] = item["hospedaje"].ToString();
                }
                txtCostoHotel.Text = "L. " + string.Format("{0:N2}", Convert.ToDecimal(Session["PRECIO_VIATICOS"]));
                DIVNuevoHotel.Visible = true;
            }
            else
            {
                DIVNuevoHotel.Visible = false;
                try
                {
                    //HABITACIONES
                    String vQuery = "VIATICOS_ObtenerGenerales 6,'" + DDLHotel.SelectedValue + "'";
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                    DDLHabitacion.Items.Add(new ListItem { Value = "0", Text = "Seleccione hoteles..." });
                    foreach (DataRow item in vDatos.Rows)
                    {
                        DDLHabitacion.Items.Add(new ListItem { Value = item["idHabitacion"].ToString(), Text = item["nombre"].ToString() });

                    }
                }
                catch (Exception Ex)
                {

                    Mensaje(Ex.Message, WarningType.Danger);
                }
            }
        }

        void cargarDiasHrs()
        {
            string vFormatDate = "yyyy-MM-dd";
            DateTime vToday = DateTime.Today;
            DateTime vFechaInicio = Convert.ToDateTime(TxFechaInicio.Text);
            string vFechaInicioS = Convert.ToDateTime(TxFechaInicio.Text).ToString(vFormatDate);


            DateTime vFechaFin = Convert.ToDateTime(TxFechaRegreso.Text);
            string vFechaFinS = Convert.ToDateTime(TxFechaRegreso.Text).ToString(vFormatDate);
            TimeSpan vDiff = vFechaFin - vFechaInicio;
            int vDias = vDiff.Days;

            Session["VIATICOS_HRS"] = vDiff.Hours;
            Session["VIATICOS_DIAS"] = vDiff.Days;

            if (vDias == 0 && vFechaFinS != vFechaInicioS)
            {
                Session["VIATICOS_DIAS"] = vDiff.Days + 1;
                vDias = Convert.ToInt32(Session["VIATICOS_DIAS"]);
            }

            txtTiempoViaticos.Text = "Dias: " + vDias.ToString() + "   horas: " + vDiff.Hours;

            if (DDLTipoViaje.SelectedValue == "2" && Session["VIATICOS_DIAS"].ToString() == "0")
            {
                DIVNuevoHotel.Visible = false;
                txtNewHotel.Text = string.Empty;
            }
            if (DDLTipoViaje.SelectedValue == "2" && Convert.ToInt32(Session["VIATICOS_DIAS"].ToString()) >= 1)
            {
                DIVNuevoHotel.Visible = true;
                txtNewHotel.Text = string.Empty;
            }

        }

        void cargarCalculo()
        {
            String vQuery = "VIATICOS_ObtenerGenerales 8,'" + Session["CATEGORIA_VIATICOS"].ToString() + "','" + DDLTipoViaje.SelectedValue + "'";
            DataTable vDatos = vConexion.obtenerDataTable(vQuery);
            foreach (DataRow item in vDatos.Rows)
            {
                Session["CIRCULACION_VIATICOS"] = Convert.ToInt32(item["circulacion"].ToString());
                Session["TRANSPORTE_VIATICOS"] = Convert.ToInt32(item["transporte"].ToString());
                Session["ALIMENTO_VIATICOS"] = Convert.ToInt32(item["alimento"].ToString());
                //Session["DEPRECIACION_VIATICOS"] = Convert.ToDecimal(item["depreciacion"]).ToString().Contains(",") ? Convert.ToDecimal(item["depreciacion"].ToString().Replace(",",".")): Convert.ToDecimal(item["depreciacion"]);
                Session["DEPRECIACION_VIATICOS"] = Convert.ToDecimal(item["depreciacion"]).ToString();
                Session["PEAJE_VIATICOS"] = Convert.ToInt32(item["peaje"].ToString());
                Session["CABELICE_VIATICOS"] = Convert.ToInt32(item["CA_Belice"].ToString());
                Session["PAISDOLAR_VIATICOS"] = Convert.ToInt32(item["pais_dolar"].ToString());
                Session["PAISNODOLAR_VIATICOS"] = Convert.ToInt32(item["pais_Nodolar"].ToString());
            }
            if (DDLTipoViaje.SelectedValue == "1")
            {
                calcularCirculacion();
                calcularHospedaje();
                calcularAlimento();
                calcularTransporte();
                calcularDepreciacion();
                calcularPeaje();
                calcularEmergencia();
                calcularTotal();
            }
            else
            {
                calculoInternacional();
                calcularTotal();
            }
        }
        void validar()
        {
            if (TxFechaInicio.Text == "")
                throw new Exception("Favor ingrese fecha y hora de inicio.");
            if (TxFechaRegreso.Text == "")
                throw new Exception("Favor ingrese fecha y hora que finaliza.");
            if (DDLMotivoViaje.SelectedValue == "0")
                throw new Exception("Favor seleccione motivo de viaje.");
            if (DDLTipoViaje.SelectedValue == "0")
                throw new Exception("Favor seleccione tipo de viaje.");
            if (DDLTransporte.SelectedValue == "0")
                throw new Exception("Favor seleccione transporte.");
            if (DDLTransporte.SelectedValue == "2")
            {
                if (txtMotivoVehiculo.Text == "")
                    throw new Exception("Favor ingrese el motivo por usar vehículo personal.");
            }
            if (DDLEmpleado.SelectedValue == "0")
                throw new Exception("Favor seleccione empleado.");
            if (DDLTipoViaje.SelectedValue == "1")
            {
                if (DDLDestinoI.SelectedValue == "0")
                    throw new Exception("Favor seleccione donde inicia viaje.");
            }
            if (DDLDestinoF.SelectedValue == "0")
                throw new Exception("Favor seleccione donde finaliza viaje.");

            if (Session["VIATICOS_DIAS"].ToString() != "0")
            {
                if (DDLTipoViaje.SelectedValue == "1")
                {
                    if (DDLHotel.SelectedValue == "0")
                        throw new Exception("Favor seleccione hotel.");
                }
                if (DDLHotel.SelectedValue != "x" && DDLTipoViaje.SelectedValue == "1")
                {
                    if (DDLHabitacion.SelectedValue == "0")
                        throw new Exception("Favor seleccione habitación de hotel.");
                }
                if (DDLHotel.SelectedValue == "x" && Convert.ToInt32(Session["VIATICOS_DIAS"].ToString()) >= 1 && DDLTipoViaje.SelectedValue == "1")
                {
                    if (txtNewHotel.Text == string.Empty)
                        throw new Exception("Favor ingrese hotel donde se hospedó.");
                }
                if (DDLHotel.SelectedValue == "x" && Convert.ToInt32(Session["VIATICOS_DIAS"].ToString()) >= 1 && DDLTipoViaje.SelectedValue == "2")
                {
                    if (txtNewPais.Text == string.Empty)
                        throw new Exception("Favor ingrese país de destino.");
                    if (txtNewHotel.Text == string.Empty)
                        throw new Exception("Favor ingrese hotel donde se hospedó.");
                }
            }

        }

        void cargarData()
        {



            if (HttpContext.Current.Session["CARGAR_DATA_VIATICOS"] == null)
            {
                string id = Request.QueryString["id"];
                string tipo = Request.QueryString["tipo"];
                try
                {
                    //CARGAR SOLICITUDES FINALIZADAS
                    DataTable vDato = new DataTable();
                    vDato = vConexion.obtenerDataTable("VIATICOS_ObtenerGenerales 9, '" + Session["USUARIO"].ToString() + "'");
                    GVViaticosTerminados.DataSource = vDato;
                    GVViaticosTerminados.DataBind();

                    if (tipo == "1")
                    {
                        //USUARIO
                        String vQuery = "VIATICOS_ObtenerGenerales 23";
                        DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                        DDLEmpleado.Items.Add(new ListItem { Value = "0", Text = "Seleccione Empleado..." });
                        foreach (DataRow item in vDatos.Rows)
                        {
                            DDLEmpleado.Items.Add(new ListItem { Value = item["Usuario"].ToString(), Text = item["Nombre"].ToString() });

                        }
                    }
                    else
                    {
                        //USUARIO
                        String vQuery = "VIATICOS_ObtenerGenerales 1, '" + Session["USUARIO"].ToString() + "'";
                        DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                        DDLEmpleado.Items.Add(new ListItem { Value = "0", Text = "Seleccione Empleado..." });
                        foreach (DataRow item in vDatos.Rows)
                        {
                            DDLEmpleado.Items.Add(new ListItem { Value = item["Usuario"].ToString(), Text = item["Nombre"].ToString() });

                        }
                    }
                    //TRANSPORTE
                    String vQuery2 = "VIATICOS_ObtenerGenerales 2";
                    DataTable vDatos2 = vConexion.obtenerDataTable(vQuery2);
                    DDLTransporte.Items.Add(new ListItem { Value = "0", Text = "Seleccione Transporte..." });
                    foreach (DataRow item in vDatos2.Rows)
                    {
                        DDLTransporte.Items.Add(new ListItem { Value = item["idTransporte"].ToString(), Text = item["nombre"].ToString() });

                    }

                    //TIPO VIAJE
                    String vQuery3 = "VIATICOS_ObtenerGenerales 3";
                    DataTable vDatos3 = vConexion.obtenerDataTable(vQuery3);
                    DDLTipoViaje.Items.Add(new ListItem { Value = "0", Text = "Seleccione tipo de viaje..." });
                    foreach (DataRow item in vDatos3.Rows)
                    {
                        DDLTipoViaje.Items.Add(new ListItem { Value = item["idtipoViaje"].ToString(), Text = item["nombre"].ToString() });

                    }

                    //MOTIVO VIAJE
                    String vQuery4 = "VIATICOS_ObtenerGenerales 4";
                    DataTable vDatos4 = vConexion.obtenerDataTable(vQuery4);
                    DDLMotivoViaje.Items.Add(new ListItem { Value = "0", Text = "Seleccione motivo de viaje..." });
                    foreach (DataRow item in vDatos4.Rows)
                    {
                        DDLMotivoViaje.Items.Add(new ListItem { Value = item["idMotivoViaje"].ToString(), Text = item["nombre"].ToString() });

                    }

                    //CARGAR VEHICULOS
                    String vQuery8 = "VIATICOS_ObtenerGenerales 10";
                    DataTable vDatos8 = vConexion.obtenerDataTable(vQuery8);
                    DDLVehiculo.Items.Add(new ListItem { Value = "0", Text = "Seleccione Vehículo..." });
                    foreach (DataRow item in vDatos8.Rows)
                    {
                        DDLVehiculo.Items.Add(new ListItem { Value = item["idVehiculo"].ToString(), Text = item["nombre"].ToString() });

                    }

                    //CARGAR DESTINO INICIAL
                    String vQuery9 = "STEISP_ATM_Generales 12";
                    DataTable vDatos9 = vConexion2.obtenerDataTableSTEI(vQuery9);
                    DDLDestinoI.Items.Add(new ListItem { Value = "0", Text = "Seleccione destino..." });
                    foreach (DataRow item in vDatos9.Rows)
                    {
                        DDLDestinoI.Items.Add(new ListItem { Value = item["idMunicipio"].ToString(), Text = item["nombre"].ToString() });

                    }


                    //CARGAR DESTINO FINAL
                    String vQuery10 = "STEISP_ATM_Generales 12";
                    DataTable vDatos10 = vConexion2.obtenerDataTableSTEI(vQuery10);
                    DDLDestinoF.Items.Add(new ListItem { Value = "0", Text = "Seleccione destino..." });
                    foreach (DataRow item in vDatos10.Rows)
                    {
                        DDLDestinoF.Items.Add(new ListItem { Value = item["idMunicipio"].ToString(), Text = item["nombre"].ToString() });

                    }

                }
                catch (Exception Ex)
                {
                    Mensaje(Ex.Message, WarningType.Danger);
                }
                Session["CARGAR_DATA_VIATICOS"] = "1";
            }
        }

        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        int CargarInformacionDDL(DropDownList vList, String vValue)
        {
            int vIndex = 0;
            try
            {
                int vContador = 0;
                foreach (ListItem item in vList.Items)
                {
                    if (item.Value.Equals(vValue))
                    {
                        vIndex = vContador;
                    }
                    vContador++;
                }
            }
            catch { throw; }
            return vIndex;
        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void TxFechaInicio_TextChanged(object sender, EventArgs e)
        {

            limpiarCalculos();
            if (TxFechaInicio.Text != "" || TxFechaInicio.Text != string.Empty)
            {
                DateTime vToday = DateTime.Today;
                DateTime vFechaInicio = Convert.ToDateTime(TxFechaInicio.Text);

                if (vFechaInicio.Date <= vToday.Date)
                {
                    Mensaje("Fecha no permitida", WarningType.Warning);
                    TxFechaInicio.Text = string.Empty;
                    txtTiempoViaticos.Text = "Dias: 0    horas: 0";
                    Session["VIATICOS_HRS"] = "0";
                    Session["VIATICOS_DIAS"] = "0";
                }
                else
                {
                    if (TxFechaInicio.Text == "" || TxFechaInicio.Text == string.Empty && TxFechaRegreso.Text == "" || TxFechaRegreso.Text == string.Empty)
                        txtTiempoViaticos.Text = "Dias: 0    horas: 0";
                    else
                    {
                        DateTime vFechaFin = Convert.ToDateTime(TxFechaRegreso.Text);
                        TimeSpan vDiff = vFechaFin - vFechaInicio;
                        int vDias = vDiff.Days;
                        Session["VIATICOS_HRS"] = vDiff.Hours;
                        Session["VIATICOS_DIAS"] = vDiff.Days;

                        if (vDias == 0 && vFechaFin.Date > vFechaInicio.Date)
                        {
                            Session["VIATICOS_DIAS"] = vDiff.Days + 1;
                            vDias = Convert.ToInt32(Session["VIATICOS_DIAS"]);
                        }

                        txtTiempoViaticos.Text = "Dias: " + vDias.ToString() + "   horas: " + vDiff.Hours;

                        if (DDLTipoViaje.SelectedValue == "2" && Session["VIATICOS_DIAS"].ToString() == "0")
                        {
                            DIVNuevoHotel.Visible = false;
                            txtNewHotel.Text = string.Empty;
                        }
                        if (DDLTipoViaje.SelectedValue == "2" && Convert.ToInt32(Session["VIATICOS_DIAS"].ToString()) >= 1)
                        {
                            DIVNuevoHotel.Visible = true;
                            txtNewHotel.Text = string.Empty;
                        }
                    }
                }
            }
            else
                txtTiempoViaticos.Text = "Dias: 0    horas: 0";

            if (Session["VIATICOS_DIAS"].ToString() == "0")
            {
                DDLHotel.Enabled = false;
                DDLHabitacion.Enabled = false;
                DDLHotel.SelectedValue = "0";
                DDLHabitacion.SelectedValue = "0";
                txtCostoHotel.Text = "L. 0";
                DIVNuevoHotel.Visible = false;
                txtNewPais.Visible = false;
                txtNewPais.Text = "";
                txtNewHotel.Text = "";
            }
            else
            {
                DDLHotel.Enabled = true;
                DDLHabitacion.Enabled = true;
            }
        }

        protected void TxFechaRegreso_TextChanged(object sender, EventArgs e)
        {

            limpiarCalculos();
            if (TxFechaInicio.Text != "" && TxFechaRegreso.Text != "")
            {
                DateTime vFechaInicio = Convert.ToDateTime(TxFechaInicio.Text);
                DateTime vFechaFin = Convert.ToDateTime(TxFechaRegreso.Text);
                if (vFechaFin.Date < vFechaInicio.Date)
                {
                    Mensaje("Fecha no permitida", WarningType.Warning);
                    TxFechaRegreso.Text = string.Empty;
                    txtTiempoViaticos.Text = "Dias: 0    horas: 0";
                    Session["VIATICOS_HRS"] = "0";
                    Session["VIATICOS_DIAS"] = "0";
                }
                else
                {
                    if (TxFechaInicio.Text == "" || TxFechaInicio.Text == string.Empty && TxFechaRegreso.Text == "" || TxFechaRegreso.Text == string.Empty)
                        txtTiempoViaticos.Text = "Dias: 0    horas: 0";
                    else
                    {

                        TimeSpan vDiff = vFechaFin - vFechaInicio;
                        int vDias = vDiff.Days;
                        Session["VIATICOS_HRS"] = vDiff.Hours;
                        Session["VIATICOS_DIAS"] = vDiff.Days;

                        if (vDias == 0 && vFechaFin.Date > vFechaInicio.Date)
                        {
                            Session["VIATICOS_DIAS"] = vDiff.Days + 1;
                            vDias = Convert.ToInt32(Session["VIATICOS_DIAS"]);
                        }

                        txtTiempoViaticos.Text = "Dias: " + vDias.ToString() + "   horas: " + vDiff.Hours;

                        if (DDLTipoViaje.SelectedValue == "2" && Session["VIATICOS_DIAS"].ToString() == "0")
                        {
                            DIVNuevoHotel.Visible = false;
                            txtNewHotel.Text = string.Empty;
                        }
                        if (DDLTipoViaje.SelectedValue == "2" && Convert.ToInt32(Session["VIATICOS_DIAS"].ToString()) >= 1)
                        {
                            DIVNuevoHotel.Visible = true;
                            txtNewHotel.Text = string.Empty;
                        }
                    }
                }
            }
            else
                txtTiempoViaticos.Text = "Dias: 0    horas: 0";


            if (Session["VIATICOS_DIAS"].ToString() == "0")
            {
                DDLHotel.Enabled = false;
                DDLHabitacion.Enabled = false;
                DDLHotel.SelectedValue = "0";
                DDLHabitacion.SelectedValue = "0";
                txtCostoHotel.Text = "L. 0";
                DIVNuevoHotel.Visible = false;
                txtNewPais.Visible = false;
                txtNewPais.Text = "";
                txtNewHotel.Text = "";
            }
            else
            {
                DDLHotel.Enabled = true;
                DDLHabitacion.Enabled = true;
            }
        }

        protected void DDLEmpleado_TextChanged(object sender, EventArgs e)
        {

            limpiarCalculos();
            try
            {
                String vQuery = "VIATICOS_ObtenerGenerales 1, '" + Session["USUARIO"].ToString() + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                foreach (DataRow item in vDatos.Rows)
                {
                    txtCodSAP.Text = item["SAP"].ToString();
                    txtPuesto.Text = item["Puesto"].ToString();
                    Session["CATEGORIA_VIATICOS"] = item["Categoria"].ToString();
                    Session["JEFE_VIATICOS"] = item["Jefe"].ToString();
                    Session["CORREO_VIATICOS"] = item["Correo"].ToString();
                    Session["ID_PUESTO"] = item["IDPuesto"].ToString();
                }

                //HOTELES
                String vQuery5 = "VIATICOS_ObtenerGenerales 5,'" + Session["CATEGORIA_VIATICOS"].ToString() + "','" + DDLDestinoF.SelectedValue + "'";
                DataTable vDatos5 = vConexion.obtenerDataTable(vQuery5);
                DDLHotel.Items.Add(new ListItem { Value = "0", Text = "Seleccione hoteles..." });
                DDLHotel.Items.Add(new ListItem { Value = "x", Text = "Otros" });
                foreach (DataRow item in vDatos5.Rows)
                {
                    DDLHotel.Items.Add(new ListItem { Value = item["idHotel"].ToString(), Text = item["nombre"].ToString() });

                }

            }
            catch (Exception Ex)
            {

                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void DDLTransporte_TextChanged(object sender, EventArgs e)
        {

            limpiarCalculos();
            txtKmPeaje.Text = "Km: 0    Peajes: 0";
            txtMotivoVehiculo.Text = string.Empty;
            if (DDLTransporte.SelectedValue == "2")
                DIVMotivoVehiculo.Visible = true;
            else
                DIVMotivoVehiculo.Visible = false;

            if (DDLTipoViaje.SelectedValue == "1" && DDLTransporte.SelectedValue == "2")
            {
                Session["VIATICOS_KM"] = "0";
                Session["VIATICOS_PEAJE"] = "0";

                if (DDLDestinoI.SelectedValue == "110" && DDLDestinoF.SelectedValue == "63" || DDLDestinoI.SelectedValue == "63" && DDLDestinoF.SelectedValue == "110")
                    Session["VIATICOS_PEAJE"] = 6;
                if (DDLDestinoI.SelectedValue == "110" && DDLDestinoF.SelectedValue == "1" || DDLDestinoI.SelectedValue == "1" && DDLDestinoF.SelectedValue == "110")
                    Session["VIATICOS_PEAJE"] = 6;
                if (DDLDestinoI.SelectedValue == "110" && DDLDestinoF.SelectedValue == "19" || DDLDestinoI.SelectedValue == "19" && DDLDestinoF.SelectedValue == "110")
                    Session["VIATICOS_PEAJE"] = 2;
                if (DDLDestinoI.SelectedValue == "110" && DDLDestinoF.SelectedValue == "36" || DDLDestinoI.SelectedValue == "36" && DDLDestinoF.SelectedValue == "110")
                    Session["VIATICOS_PEAJE"] = 4;
                if (DDLDestinoI.SelectedValue == "110" && DDLDestinoF.SelectedValue == "40" || DDLDestinoI.SelectedValue == "40" && DDLDestinoF.SelectedValue == "110")
                    Session["VIATICOS_PEAJE"] = 2;
                if (DDLDestinoI.SelectedValue == "19" && DDLDestinoF.SelectedValue == "36" || DDLDestinoI.SelectedValue == "36" && DDLDestinoF.SelectedValue == "19")
                    Session["VIATICOS_PEAJE"] = 2;

                txtKmPeaje.Text = "Km: " + Convert.ToString(Session["VIATICOS_KM"]) + "    Peajes: " + Convert.ToString(Session["VIATICOS_PEAJE"]);

            }
            if(DDLTransporte.SelectedValue=="2")
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openModalTransporte();", true);
        }



        protected void DDLHotel_TextChanged(object sender, EventArgs e)
        {
            DDLHabitacion.Items.Clear();
            txtCostoHotel.Text = "L. 0";
            limpiarCalculos();
            txtNewHotel.Text = string.Empty;

            if (DDLHotel.SelectedValue == "x")
            {
                String vQuery = "VIATICOS_ObtenerGenerales 43,'" + Session["CATEGORIA_VIATICOS"] + "','" + DDLTipoViaje.SelectedValue + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                foreach (DataRow item in vDatos.Rows)
                {
                    Session["PRECIO_VIATICOS"] = item["hospedaje"].ToString();
                }
                txtCostoHotel.Text = "L. " + string.Format("{0:N2}", Convert.ToDecimal(Session["PRECIO_VIATICOS"]));
                DIVNuevoHotel.Visible = true;
            }
            else
            {
                LBDesayuno.Text = "0";
                DIVNuevoHotel.Visible = false;
                try
                {
                    //HABITACIONES
                    String vQuery = "VIATICOS_ObtenerGenerales 6,'" + DDLHotel.SelectedValue + "'";
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                    DDLHabitacion.Items.Add(new ListItem { Value = "0", Text = "Seleccione hoteles..." });
                    foreach (DataRow item in vDatos.Rows)
                    {
                        DDLHabitacion.Items.Add(new ListItem { Value = item["idHabitacion"].ToString(), Text = item["nombre"].ToString() });

                    }
                }
                catch (Exception Ex)
                {

                    Mensaje(Ex.Message, WarningType.Danger);
                }
            }
        }

        protected void DDLHabitacion_TextChanged(object sender, EventArgs e)
        {
            txtCostoHotel.Text = "L. 0";
            limpiarCalculos();
            try
            {
                //HABITACIONES
                String vQuery = "VIATICOS_ObtenerGenerales 7,'" + DDLHabitacion.SelectedValue + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                foreach (DataRow item in vDatos.Rows)
                {
                    Session["PRECIO_VIATICOS"] = Convert.ToInt32(item["precio"].ToString()) * (24.93);
                    txtCostoHotel.Text = "L. " + string.Format("{0:N2}", Convert.ToDecimal(Session["PRECIO_VIATICOS"]));
                }
            }
            catch (Exception Ex)
            {

                Mensaje(Ex.Message, WarningType.Danger);
            }

        }
        void limpiarPantalla()
        {
            Session["ID_PUESTO"] = null;
            CBEmergencia.Checked = false;
            TxFechaInicio.Text = string.Empty;
            TxFechaRegreso.Text = string.Empty;
            DDLMotivoViaje.SelectedValue = "0";
            DDLTipoViaje.SelectedValue = "0";
            DDLTransporte.SelectedValue = "0";
            txtMotivoVehiculo.Text = string.Empty;
            DDLEmpleado.SelectedValue = "0";
            txtCodSAP.Text = string.Empty;
            txtPuesto.Text = string.Empty;
            DDLDestinoI.SelectedValue = "0";
            DDLDestinoF.SelectedValue = "0";
            txtKmPeaje.Text = "Km: 0    Peajes: 0";
            DDLHotel.SelectedValue = "0";
            DDLHabitacion.SelectedValue = "0";
            txtCostoHotel.Text = "L.  0";
            txtNewPais.Text = string.Empty;
            txtNewHotel.Text = string.Empty;
            CBDesayuno.Checked = false;
            Session["CIRCULACION_VIATICOS"] = null;
            Session["TRANSPORTE_VIATICOS"] = null;
            Session["ALIMENTO_VIATICOS"] = null;
            Session["DEPRECIACION_VIATICOS"] = null;
            Session["PEAJE_VIATICOS"] = null;
            Session["CABELICE_VIATICOS"] = null;
            Session["PAISDOLAR_VIATICOS"] = null;
            Session["PAISNODOLAR_VIATICOS"] = null;
            Session["VIATICOS_KM"] = null;
            Session["VIATICOS_PEAJE"] = null;
            Session["VIATICOS_DIAS"] = null;
            Session["VIATICOS_HRS"] = null;
            Session["PRECIO_VIATICOS"] = null;
            Session["JEFE_VIATICOS"] = null;
            Session["CORREO_VIATICOS"] = null;
            DIVMotivoVehiculo.Visible = false;
            DIVNuevoHotel.Visible = false;
            DIVPais.Visible = false;
            DIVVehiculo.Visible = false;
            txtTiempoViaticos.Text = "Dias: 0    horas: 0";
            LBTSoli.Text = "0";
        }

        protected void BtnCrearPermiso_Click(object sender, EventArgs e)
        {

            string id = Request.QueryString["id"];
            string tipo = Request.QueryString["tipo"];
            Session["BAG"] = "1";
            try
            {

                if (tipo == "1")
                {
                    if (DDLTransporte.SelectedValue == "1")
                    {
                        if (DDLVehiculo.SelectedValue == "0")
                            Mensaje("Seleccione vehículo con el que viajará.", WarningType.Warning);
                        else
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openModal();", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openModal();", true);
                    }
                }
                else
                {
                    if (LBSubTotal.Text == "0" && LBTotal.Text == "0")
                    {
                        Mensaje("Debe realizar el calculo", WarningType.Warning);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openModal();", true);
                    }

                }
            }
            catch (Exception Ex)
            {

                Mensaje(Ex.Message, WarningType.Danger);
            }
        }
        void calcularHospedaje()
        {
            Decimal vHospedaje = 0;
            Decimal vPrecio = 0;
            int vDias = 0;

            if (Session["VIATICOS_DIAS"].ToString() == "0")
            {
                //LBHospedaje.Text = Session["PRECIO_VIATICOS"].ToString().Contains(",") ? Session["PRECIO_VIATICOS"].ToString().Replace(",", ".") : Session["PRECIO_VIATICOS"].ToString();
                LBHospedaje.Text = string.Format("{0:N2}", Convert.ToDecimal(Session["PRECIO_VIATICOS"])); 
                Session["HOSPEDAJE_C"] = Session["PRECIO_VIATICOS"].ToString();
            }
            else
            {
                vPrecio = Convert.ToDecimal(Session["PRECIO_VIATICOS"]);
                vDias = Convert.ToInt32(Session["VIATICOS_DIAS"]);
                vHospedaje = vPrecio * vDias;
                //LBHospedaje.Text = vHospedaje.ToString().Contains(",") ? vHospedaje.ToString().Replace(",", ".") : vHospedaje.ToString();
                LBHospedaje.Text = string.Format("{0:N2}",vHospedaje);
                Session["HOSPEDAJE_C"] = vHospedaje;
            }
        }
        void calcularCirculacion()
        {
            Decimal vHospedaje = 0;
            int vCirculacion = 0;
            Decimal vPrecio = 0;
            int vDias = 0;
            int vMediaJornada = 0;
            int vResultadoCirculacion = 0;
            //int vCirculacionHrs = 0;
            if (Session["VIATICOS_DIAS"].ToString() == "0")
            {
                LBHospedaje.Text = string.Format("{0:N2}", Convert.ToDecimal(Session["PRECIO_VIATICOS"]));
                // LBHospedaje.Text = Session["PRECIO_VIATICOS"].ToString().Contains(",") ? Session["PRECIO_VIATICOS"].ToString().Replace(",", ".") : Session["PRECIO_VIATICOS"].ToString();
            }
            else
            {
                //vPrecio = Convert.ToDecimal(Session["PRECIO_VIATICOS"]).ToString().Contains(",") ? Convert.ToDecimal(Session["PRECIO_VIATICOS"].ToString().Replace(",", ".")) : Convert.ToDecimal(Session["PRECIO_VIATICOS"]);
                vPrecio = Convert.ToDecimal(Session["PRECIO_VIATICOS"]);
                vDias = Convert.ToInt32(Session["VIATICOS_DIAS"]);
                vHospedaje = vPrecio * vDias;
                LBHospedaje.Text = string.Format("{0:N2}", vHospedaje);
                //LBHospedaje.Text = vHospedaje.ToString().Contains(",") ? vHospedaje.ToString().Replace(",", ".") : vHospedaje.ToString();
            }
            vDias = Convert.ToInt32(Session["VIATICOS_DIAS"]);
            if (Convert.ToInt32(Session["VIATICOS_HRS"]) > 4 && vDias == 0)
            {

                vCirculacion = Convert.ToInt32(Session["CIRCULACION_VIATICOS"].ToString());
                LBCirculacion.Text = string.Format("{0:N2}", vCirculacion);
            }
            if (Convert.ToInt32(Session["VIATICOS_HRS"]) <= 4 && vDias == 0)
            {
                vMediaJornada = Convert.ToInt32(Session["CIRCULACION_VIATICOS"]) / 2;
                LBCirculacion.Text = string.Format("{0:N2}", vMediaJornada);
            }
            if (Convert.ToInt32(Session["VIATICOS_HRS"]) <= 4 && vDias > 0)
            {
                vMediaJornada = Convert.ToInt32(Session["CIRCULACION_VIATICOS"]) / 2;
                vCirculacion = Convert.ToInt32(Session["CIRCULACION_VIATICOS"].ToString()) * Convert.ToInt32(Session["VIATICOS_DIAS"].ToString());
                vResultadoCirculacion = vCirculacion + vMediaJornada;
                LBCirculacion.Text = string.Format("{0:N2}", vResultadoCirculacion);
            }
            if (Convert.ToInt32(Session["VIATICOS_HRS"]) > 4 && vDias > 0)
            {
                vMediaJornada = Convert.ToInt32(Session["CIRCULACION_VIATICOS"].ToString());
                vCirculacion = Convert.ToInt32(Session["CIRCULACION_VIATICOS"].ToString()) * Convert.ToInt32(Session["VIATICOS_DIAS"].ToString());
                vResultadoCirculacion = vCirculacion + vMediaJornada;
                LBCirculacion.Text = string.Format("{0:N2}", vResultadoCirculacion);
            }
        }
        void calculoInternacional()
        {
            LBCirculacion.Text = string.Format("{0:N2}", Convert.ToDecimal(Session["CIRCULACION_VIATICOS"]));

        }
        void calcularAlimento()
        {
            int vDesayuno = Convert.ToInt32(Session["ALIMENTO_VIATICOS"]) / 3;
            int vAlmuerzo = Convert.ToInt32(Session["ALIMENTO_VIATICOS"]) / 3;
            int vCena = Convert.ToInt32(Session["ALIMENTO_VIATICOS"]) / 3;
            int vDesayunoTotal = 0;
            int vAlmuerzoTotal = 0;
            int vCenaTotal = 0;
            DateTime vHoraInicio = Convert.ToDateTime(TxFechaInicio.Text);
            DateTime vMinInicio = Convert.ToDateTime(TxFechaInicio.Text);
            DateTime vHoraFin = Convert.ToDateTime(TxFechaRegreso.Text);
            DateTime vMinFin = Convert.ToDateTime(TxFechaRegreso.Text);

            if (Session["VIATICOS_DIAS"].ToString() == "0")//VIATICOS DE UN MISMO DIA
            {

                if (vHoraInicio.Hour <= 7 && vHoraFin.Hour >= 19)
                {
                    LBDesayuno.Text = string.Format("{0:N2}", vDesayuno);
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzo);
                    LBCena.Text = string.Format("{0:N2}", vCena);
                }
                if (vHoraInicio.Hour <= 7 && vHoraFin.Hour < 19)
                {
                    LBDesayuno.Text = string.Format("{0:N2}", vDesayuno);
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzo);
                    LBCena.Text = "0";
                }
                if (vHoraInicio.Hour <= 7 && vHoraFin.Hour < 11)
                {
                    LBDesayuno.Text = string.Format("{0:N2}", vDesayuno);
                    LBAlmuerzo.Text = "0";
                    LBCena.Text = "0";
                }

                if (vHoraInicio.Hour > 7 && vHoraFin.Hour >= 19)
                {
                    LBDesayuno.Text = "0";
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzo);
                    LBCena.Text = string.Format("{0:N2}", vCena);
                }
                if (vHoraInicio.Hour > 7 && vHoraFin.Hour <= 18)
                {
                    LBDesayuno.Text = "0";
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzo);
                    LBCena.Text = "0";
                }
                if (vHoraInicio.Hour > 14 && vHoraFin.Hour >= 19)
                {
                    LBDesayuno.Text = "0";
                    LBAlmuerzo.Text = "0";
                    LBCena.Text = string.Format("{0:N2}", vCena);
                }
                if (vHoraInicio.Hour > 7 && vHoraFin.Hour < 11)
                {
                    LBDesayuno.Text = "0";
                    LBAlmuerzo.Text = "0";
                    LBCena.Text = "0";
                }
                if (vHoraInicio.Hour > 14 && vHoraFin.Hour < 19)
                {
                    LBDesayuno.Text = "0";
                    LBAlmuerzo.Text = "0";
                    LBCena.Text = "0";
                }

            }
            if (Session["VIATICOS_DIAS"].ToString() == "1")//VIATICOS DE UN DIA PARA OTRO
            {
                if (vHoraInicio.Hour <= 7 && vHoraFin.Hour >= 19)
                {
                    vDesayunoTotal = vDesayuno + vDesayuno;
                    LBDesayuno.Text = string.Format("{0:N2}", vDesayunoTotal);
                    vAlmuerzoTotal = vAlmuerzo + vAlmuerzo;
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzoTotal);
                    vCenaTotal = vCena + vCena;
                    LBCena.Text = string.Format("{0:N2}", vCenaTotal);
                }
                if (vHoraInicio.Hour <= 7 && vHoraFin.Hour < 19 && vHoraFin.Hour > 7)
                {
                    vDesayunoTotal = vDesayuno + vDesayuno;
                    LBDesayuno.Text = string.Format("{0:N2}", vDesayunoTotal);
                    vAlmuerzoTotal = vAlmuerzo + vAlmuerzo;
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzoTotal);
                    LBCena.Text = string.Format("{0:N2}", vCena);
                }
                if (vHoraInicio.Hour <= 7 && vHoraFin.Hour < 11 && vHoraFin.Hour > 7)
                {
                    vDesayunoTotal = vDesayuno + vDesayuno;
                    LBDesayuno.Text = string.Format("{0:N2}", vDesayunoTotal);
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzo);
                    LBCena.Text = string.Format("{0:N2}", vCena);
                }

                if (vHoraInicio.Hour > 7 && vHoraFin.Hour >= 19)
                {

                    LBDesayuno.Text = string.Format("{0:N2}", vDesayuno);
                    vAlmuerzoTotal = vAlmuerzo + vAlmuerzo;
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzoTotal);
                    vCenaTotal = vCena + vCena;
                    LBCena.Text = string.Format("{0:N2}", vCenaTotal);
                }
                if (vHoraInicio.Hour > 7 && vHoraFin.Hour >= 14 && vHoraFin.Hour < 19)
                {

                    LBDesayuno.Text = string.Format("{0:N2}", vDesayuno);
                    vAlmuerzoTotal = vAlmuerzo + vAlmuerzo;
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzoTotal);
                    LBCena.Text = string.Format("{0:N2}", vCena);
                }
                if (vHoraInicio.Hour > 7 && vHoraFin.Hour <= 14 && vHoraFin.Hour > 7)
                {

                    LBDesayuno.Text = string.Format("{0:N2}", vDesayuno);
                    vAlmuerzoTotal = vAlmuerzo + vAlmuerzo;
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzoTotal);
                    LBCena.Text = string.Format("{0:N2}", vCena);
                }
                if (vHoraInicio.Hour >= 14 && vHoraFin.Hour >= 19)
                {

                    LBDesayuno.Text = string.Format("{0:N2}", vDesayuno);
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzo);
                    vCenaTotal = vCena + vCena;
                    LBCena.Text = string.Format("{0:N2}", vCenaTotal);
                }
                if (vHoraInicio.Hour > 7 && vHoraFin.Hour < 11 && vHoraFin.Hour > 7)
                {
                    LBDesayuno.Text = string.Format("{0:N2}", vDesayuno);
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzo);
                    LBCena.Text = string.Format("{0:N2}", vCena);
                }
                if (vHoraInicio.Hour >= 14 && vHoraFin.Hour < 12 && vHoraFin.Hour > 7)
                {
                    LBDesayuno.Text = string.Format("{0:N2}", vDesayuno);
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzo);
                    LBCena.Text = string.Format("{0:N2}", vCena);
                }
                if (vHoraInicio.Hour <= 7 && vHoraFin.Hour <= 5)
                {
                    LBDesayuno.Text = "0";
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzo);
                    LBCena.Text = string.Format("{0:N2}", vCena);
                }
                if (vHoraInicio.Hour > 7 && vHoraFin.Hour <= 5)
                {
                    LBDesayuno.Text = "0";
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzo);
                    LBCena.Text = string.Format("{0:N2}", vCena);
                }
                if (vHoraInicio.Hour >= 14 && vHoraFin.Hour <= 7)
                {
                    LBDesayuno.Text = "0";
                    LBAlmuerzo.Text = "0";
                    LBCena.Text = string.Format("{0:N2}", vCena);
                }
            }
            if (Convert.ToInt32(Session["VIATICOS_DIAS"].ToString()) >= 2)//VIATICOS DE MAS DE UN DIA
            {
                int vDiasAlimento = Convert.ToInt32(Session["VIATICOS_DIAS"]) - 1;
                int vDesayunoExtra = 0;
                int vAlmuerzoExtra = 0;
                int vCenaExtra = 0;
                if (vHoraInicio.Hour <= 7 && vHoraFin.Hour >= 19)
                {
                    vDesayunoTotal = vDesayuno + vDesayuno;
                    vDesayunoExtra = (vDesayuno * vDiasAlimento) + vDesayunoTotal;
                    LBDesayuno.Text = string.Format("{0:N2}", vDesayunoExtra);
                    vAlmuerzoTotal = vAlmuerzo + vAlmuerzo;
                    vAlmuerzoExtra = (vAlmuerzo * vDiasAlimento) + vAlmuerzoTotal;
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzoExtra);
                    vCenaTotal = vCena + vCena;
                    vCenaExtra = (vCena * vDiasAlimento) + vCenaTotal;
                    LBCena.Text = string.Format("{0:N2}", vCenaExtra);
                }
                if (vHoraInicio.Hour <= 7 && vHoraFin.Hour < 19)
                {
                    vDesayunoTotal = vDesayuno + vDesayuno;
                    vDesayunoExtra = (vDesayuno * vDiasAlimento) + vDesayunoTotal;
                    LBDesayuno.Text = string.Format("{0:N2}", vDesayunoExtra);
                    vAlmuerzoTotal = vAlmuerzo + vAlmuerzo;
                    vAlmuerzoExtra = (vAlmuerzo * vDiasAlimento) + vAlmuerzoTotal;
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzoExtra);
                    vCenaExtra = (vCena * vDiasAlimento) + vCena;
                    LBCena.Text = string.Format("{0:N2}", vCenaExtra);
                }
                if (vHoraInicio.Hour <= 7 && vHoraFin.Hour < 11)
                {
                    vDesayunoTotal = vDesayuno + vDesayuno;
                    vDesayunoExtra = (vDesayuno * vDiasAlimento) + vDesayunoTotal;
                    LBDesayuno.Text = string.Format("{0:N2}", vDesayunoExtra);
                    vAlmuerzoExtra = (vAlmuerzo * vDiasAlimento) + vAlmuerzo;
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzoExtra);
                    vCenaExtra = (vCena * vDiasAlimento) + vCena;
                    LBCena.Text = string.Format("{0:N2}", vCenaExtra);
                }

                if (vHoraInicio.Hour > 7 && vHoraFin.Hour >= 19)
                {
                    vDesayunoExtra = (vDesayuno * vDiasAlimento) + vDesayuno;
                    LBDesayuno.Text = string.Format("{0:N2}", vDesayunoExtra);
                    vAlmuerzoTotal = vAlmuerzo + vAlmuerzo;
                    vAlmuerzoExtra = (vAlmuerzo * vDiasAlimento) + vAlmuerzoTotal;
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzoExtra);
                    vCenaTotal = vCena + vCena;
                    vCenaExtra = (vCena * vDiasAlimento) + vCenaTotal;
                    LBCena.Text = string.Format("{0:N2}", vCenaExtra);
                }
                if (vHoraInicio.Hour > 7 && vHoraFin.Hour <= 18)
                {
                    vDesayunoExtra = (vDesayuno * vDiasAlimento) + vDesayuno;
                    LBDesayuno.Text = string.Format("{0:N2}", vDesayunoExtra);
                    vAlmuerzoTotal = vAlmuerzo + vAlmuerzo;
                    vAlmuerzoExtra = (vAlmuerzo * vDiasAlimento) + vAlmuerzoTotal;
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzoExtra);
                    vCenaExtra = (vCena * vDiasAlimento) + vCena;
                    LBCena.Text = string.Format("{0:N2}", vCenaExtra);
                }
                if (vHoraInicio.Hour > 14 && vHoraFin.Hour >= 19)
                {
                    vDesayunoExtra = (vDesayuno * vDiasAlimento) + vDesayuno;
                    LBDesayuno.Text = string.Format("{0:N2}", vDesayunoExtra);
                    vAlmuerzoExtra = (vAlmuerzo * vDiasAlimento) + vAlmuerzo;
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzoExtra);
                    vCenaTotal = vCena + vCena;
                    vCenaExtra = (vCena * vDiasAlimento) + vCenaTotal;
                    LBCena.Text = string.Format("{0:N2}", vCenaExtra);
                }
                if (vHoraInicio.Hour > 7 && vHoraFin.Hour < 11)
                {
                    vDesayunoExtra = (vDesayuno * vDiasAlimento) + vDesayuno;
                    LBDesayuno.Text = string.Format("{0:N2}", vDesayunoExtra);
                    vAlmuerzoExtra = (vAlmuerzo * vDiasAlimento) + vAlmuerzo;
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzoExtra);
                    vCenaExtra = (vCena * vDiasAlimento) + vCena;
                    LBCena.Text = string.Format("{0:N2}", vCenaExtra);
                }
                if (vHoraInicio.Hour > 14 && vHoraFin.Hour < 19)
                {
                    vDesayunoExtra = (vDesayuno * vDiasAlimento) + vDesayuno;
                    LBDesayuno.Text = string.Format("{0:N2}", vDesayunoExtra);
                    vAlmuerzoExtra = (vAlmuerzo * vDiasAlimento) + vAlmuerzo;
                    LBAlmuerzo.Text = string.Format("{0:N2}", vAlmuerzoExtra);
                    vCenaExtra = (vCena * vDiasAlimento) + vCena;
                    LBCena.Text = string.Format("{0:N2}", vCenaExtra);
                }
            }
            if (DDLHotel.SelectedValue != "x")
                LBDesayuno.Text = "0";
            if (CBDesayuno.Checked == true)
                LBDesayuno.Text = "0";
        }
        void calcularTransporte()
        {
            if (DDLTransporte.SelectedValue == "3" && DDLTipoViaje.SelectedValue == "1")
                LBTransporte.Text = string.Format("{0:N2}", Convert.ToDecimal( Session["TRANSPORTE_VIATICOS"]));

            if (DDLTransporte.SelectedValue == "2" && DDLTipoViaje.SelectedValue == "1")
                LBTransporte.Text = "0";

        }
        void calcularDepreciacion()
        {
            if (DDLTransporte.SelectedValue == "2" && DDLTipoViaje.SelectedValue == "1")
            {
                Decimal vDepreciacion = Convert.ToDecimal(Session["VIATICOS_KM"]) * Convert.ToDecimal(Session["DEPRECIACION_VIATICOS"]);
                LBDepresiacion.Text = string.Format("{0:N2}", vDepreciacion);
                //LBDepresiacion.Text = vDepreciacion.ToString().Contains(",")? vDepreciacion.ToString().Replace(",","."): vDepreciacion.ToString();
                Session["DEPRECIACION_C"] = vDepreciacion;
               
            }
        }
        void calcularPeaje()
        {
            if (DDLTransporte.SelectedValue == "4" && DDLTipoViaje.SelectedValue == "1")
                LBPeaje.Text = string.Format("{0:N2}", 100);
            if (DDLTransporte.SelectedValue == "2" && DDLTipoViaje.SelectedValue == "1")
            {
                Decimal vPeaje = Convert.ToDecimal(Session["PEAJE_VIATICOS"]) * Convert.ToDecimal(Session["VIATICOS_PEAJE"]);
                LBPeaje.Text = string.Format("{0:N2}", vPeaje);
                //LBPeaje.Text = vPeaje.ToString().Contains(",")? vPeaje.ToString().Replace(",","."): vPeaje.ToString();
            }
            if (DDLTransporte.SelectedValue == "1" && DDLTipoViaje.SelectedValue == "1")
            {
                if (Convert.ToString(Session["ID_PUESTO"]) == "20000409" || Convert.ToString(Session["ID_PUESTO"]) == "20000410")
                {
                    Decimal vPeaje = Convert.ToDecimal(Session["PEAJE_VIATICOS"]) * Convert.ToDecimal(Session["VIATICOS_PEAJE"]);
                    LBPeaje.Text = string.Format("{0:N2}", vPeaje);
                    //LBPeaje.Text = vPeaje.ToString().Contains(",") ? vPeaje.ToString().Replace(",", ".") : vPeaje.ToString();
                }
            }

        }
        void calcularEmergencia()
        {
            if (DDLTipoViaje.SelectedValue == "1" && CBEmergencia.Checked == true)
                LBEmergencia.Text = string.Format("{0:N2}", 500);
            else
                LBEmergencia.Text = "0";
        }
        void calcularTotal()
        {

           
            Decimal vSubTotal = Convert.ToDecimal(Session["HOSPEDAJE_C"]) + Convert.ToDecimal(LBDesayuno.Text) + Convert.ToDecimal(LBAlmuerzo.Text) + Convert.ToDecimal(LBCena.Text) + Convert.ToDecimal(Session["DEPRECIACION_C"]) + Convert.ToDecimal(LBTransporte.Text) + Convert.ToDecimal(LBPeaje.Text) + Convert.ToDecimal(LBCirculacion.Text);
            LBSubTotal.Text = string.Format("{0:N2}", vSubTotal);

            Decimal vTotal = Convert.ToDecimal(Session["HOSPEDAJE_C"]) + Convert.ToDecimal(LBDesayuno.Text) + Convert.ToDecimal(LBAlmuerzo.Text) + Convert.ToDecimal(LBCena.Text) + Convert.ToDecimal(Session["DEPRECIACION_C"]) + Convert.ToDecimal(LBTransporte.Text) + Convert.ToDecimal(LBPeaje.Text) + Convert.ToDecimal(LBCirculacion.Text) + Convert.ToDecimal(LBEmergencia.Text);
            LBTotal.Text = string.Format("{0:N2}", vTotal);

            Decimal vTsoli = vTotal - Convert.ToDecimal(Session["DEPRECIACION_C"]);
            LBTSoli.Text = string.Format("{0:N2}", vTsoli);


            if (DDLTipoViaje.SelectedValue == "2")
            {
                //Session["VIATICOS_DIAS"]
                LBTSoli.Text = "0";
                if (DDLDestinoF.SelectedValue == "272")
                {
                    if (Convert.ToInt32(Session["VIATICOS_DIAS"]) == 0)
                    {
                        Decimal vTotalInternacional = (vTotal + Convert.ToDecimal(Session["CABELICE_VIATICOS"]));
                        LBTotal.Text = string.Format("{0:N2}", vTotalInternacional);
                        LBSubTotal.Text = string.Format("{0:N2}", vTotalInternacional);
                    }
                    else if (Convert.ToInt32(Session["VIATICOS_DIAS"]) >= 1)
                    {
                        Decimal vTotalInternacional = Convert.ToDecimal(LBCirculacion.Text) + (Convert.ToDecimal(Session["CABELICE_VIATICOS"]) * Convert.ToDecimal(Session["VIATICOS_DIAS"]));
                        LBTotal.Text = string.Format("{0:N2}", vTotalInternacional);
                        LBSubTotal.Text = string.Format("{0:N2}", vTotalInternacional);
                    }
                }
                if (DDLDestinoF.SelectedValue == "273")
                {
                    if (Convert.ToInt32(Session["VIATICOS_DIAS"]) == 0)
                    {
                        Decimal vTotalInternacional = vTotal + Convert.ToDecimal(Session["PAISDOLAR_VIATICOS"]);
                        LBTotal.Text = string.Format("{0:N2}", vTotalInternacional);
                        LBSubTotal.Text = string.Format("{0:N2}", vTotalInternacional);
                    }
                    else if (Convert.ToInt32(Session["VIATICOS_DIAS"]) >= 1)
                    {
                        Decimal vTotalInternacional = vTotal + (Convert.ToDecimal(Session["PAISDOLAR_VIATICOS"]) * Convert.ToDecimal(Session["VIATICOS_DIAS"]));
                        LBTotal.Text = string.Format("{0:N2}", vTotalInternacional);
                        LBSubTotal.Text = string.Format("{0:N2}", vTotalInternacional);
                    }
                }
                if (DDLDestinoF.SelectedValue == "274")
                {
                    if (Convert.ToInt32(Session["VIATICOS_DIAS"]) == 0)
                    {
                        Decimal vTotalInternacional = vTotal + Convert.ToDecimal(Session["PAISNODOLAR_VIATICOS"]);
                        LBTotal.Text = string.Format("{0:N2}", vTotalInternacional);
                        LBSubTotal.Text = string.Format("{0:N2}", vTotalInternacional);
                    }
                    else if (Convert.ToInt32(Session["VIATICOS_DIAS"]) >= 1)
                    {
                        Decimal vTotalInternacional = vTotal + (Convert.ToDecimal(Session["PAISNODOLAR_VIATICOS"]) * Convert.ToDecimal(Session["VIATICOS_DIAS"]));
                        LBTotal.Text = string.Format("{0:N2}", vTotalInternacional);
                        LBSubTotal.Text = string.Format("{0:N2}", vTotalInternacional);
                    }
                }


            }
        }

        protected void btnCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                validar();


                //COSTOS
                String vQuery = "VIATICOS_ObtenerGenerales 8,'" + Session["CATEGORIA_VIATICOS"].ToString() + "','" + DDLTipoViaje.SelectedValue + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                foreach (DataRow item in vDatos.Rows)
                {
                    Session["CIRCULACION_VIATICOS"] = Convert.ToInt32(item["circulacion"].ToString());
                    Session["TRANSPORTE_VIATICOS"] = Convert.ToInt32(item["transporte"].ToString());
                    Session["ALIMENTO_VIATICOS"] = Convert.ToInt32(item["alimento"].ToString());
                    Session["DEPRECIACION_VIATICOS"] = Convert.ToInt32(item["depreciacion"].ToString());
                    Session["PEAJE_VIATICOS"] = Convert.ToInt32(item["peaje"].ToString());
                    Session["CABELICE_VIATICOS"] = Convert.ToInt32(item["CA_Belice"].ToString());
                    Session["PAISDOLAR_VIATICOS"] = Convert.ToInt32(item["pais_dolar"].ToString());
                    Session["PAISNODOLAR_VIATICOS"] = Convert.ToInt32(item["pais_Nodolar"].ToString());
                }
                if (DDLTipoViaje.SelectedValue == "1")
                {
                    calcularCirculacion();
                    calcularHospedaje();
                    calcularAlimento();
                    calcularTransporte();
                    calcularDepreciacion();
                    calcularPeaje();
                    calcularEmergencia();
                    calcularTotal();
                }
                else
                {
                    calculoInternacional();
                    calcularTotal();
                }
            }
            catch (Exception Ex)
            {

                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void DDLTipoViaje_TextChanged(object sender, EventArgs e)
        {
            limpiarCalculos();

            DDLDestinoF.Items.Clear();
            DDLDestinoI.SelectedValue = "0";
            DDLDestinoF.SelectedValue = "0";
            txtKmPeaje.Text = "Km: 0    Peajes: 0";
            DDLDestinoI.Enabled = true;
            //CARGAR DESTINO FINAL
            String vQuery10 = "STEISP_ATM_Generales 12";
            DataTable vDatos10 = vConexion.obtenerDataTableSTEI(vQuery10);
            DDLDestinoF.Items.Add(new ListItem { Value = "0", Text = "Seleccione destino..." });
            foreach (DataRow item in vDatos10.Rows)
            {
                DDLDestinoF.Items.Add(new ListItem { Value = item["idMunicipio"].ToString(), Text = item["nombre"].ToString() });

            }

            if (DDLTipoViaje.SelectedValue == "2")
            {
                CBDesayuno.Checked = false;
                CBEmergencia.Checked = false;
                DDLTransporte.SelectedValue = "4";
                DIVMotivoVehiculo.Visible = false;
                txtMotivoVehiculo.Text = "";
                DDLTransporte.Enabled = false;
                DDLDestinoI.Enabled = false;
                DDLDestinoF.Items.Clear();
                //DIVPais.Visible = true;
                //DESTINOS INTERNACIONAL
                String vQuery5 = "VIATICOS_ObtenerGenerales 12,'" + DDLTipoViaje.SelectedValue + "'";
                DataTable vDatos5 = vConexion.obtenerDataTable(vQuery5);
                DDLDestinoF.Items.Add(new ListItem { Value = "0", Text = "Seleccione destino..." });
                foreach (DataRow item in vDatos5.Rows)
                {
                    DDLDestinoF.Items.Add(new ListItem { Value = item["idDestino"].ToString(), Text = item["nombre"].ToString() });

                }
                DDLHotel.SelectedValue = "x";
                DDLHotel.Enabled = false;
                DDLHabitacion.SelectedValue = "0";
                DDLHabitacion.Enabled = false;
                if (Session["VIATICOS_DIAS"].ToString() == "0")
                {
                    DIVNuevoHotel.Visible = false;
                    txtNewHotel.Text = string.Empty;
                    DIVPais.Visible = false;
                }
                else
                {

                    DIVNuevoHotel.Visible = true;
                    txtNewHotel.Text = string.Empty;
                    DIVPais.Visible = true;
                    txtNewPais.Visible = true;
                }
                LBMonedaAlmuerzo.Text = "$";
                LBMonedaCena.Text = "$";
                LBMonedaCirculacion.Text = "$";
                LBMonedaDepresiacion.Text = "$";
                LBMonedaDesayuno.Text = "$";
                LBMonedaEmergencia.Text = "$";
                LBMonedaHospedaje.Text = "$";
                LBMonedaPeaje.Text = "$";
                LBMonedaSubTotal.Text = "$";
                LBMonedaTotal.Text = "$";
                LBMonedaTransporte.Text = "$";
                txtCostoHotel.Text = "$ 0";
                LBMonedaTSoli.Text = "$";
            }
            else
            {
                DDLTransporte.Enabled = true;
                if (Session["VIATICOS_DIAS"].ToString() != "0")
                {
                    DDLHotel.SelectedValue = "0";
                    DDLHotel.Enabled = true;
                    DDLHabitacion.SelectedValue = "0";
                    DDLHabitacion.Enabled = true;
                    DIVNuevoHotel.Visible = false;
                    DIVPais.Visible = false;
                }
                LBMonedaAlmuerzo.Text = "L.";
                LBMonedaCena.Text = "L.";
                LBMonedaCirculacion.Text = "L.";
                LBMonedaDepresiacion.Text = "L.";
                LBMonedaDesayuno.Text = "L.";
                LBMonedaEmergencia.Text = "L.";
                LBMonedaHospedaje.Text = "L.";
                LBMonedaPeaje.Text = "L.";
                LBMonedaSubTotal.Text = "L.";
                LBMonedaTotal.Text = "L.";
                LBMonedaTransporte.Text = "L.";
                txtCostoHotel.Text = "L. 0";
                LBMonedaTSoli.Text = "L.";
            }
        }
        void limpiarCalculos()
        {
            LBAlmuerzo.Text = "0";
            LBCena.Text = "0";
            LBCirculacion.Text = "0";
            LBDepresiacion.Text = "0";
            LBDesayuno.Text = "0";
            LBEmergencia.Text = "0";
            LBHospedaje.Text = "0";
            LBPeaje.Text = "0";
            LBSubTotal.Text = "0";
            LBTotal.Text = "0";
            LBTransporte.Text = "0";
            LBTSoli.Text = "0";
        }

        protected void CBEmergencia_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                if (CBEmergencia.Checked == true && DDLTipoViaje.SelectedValue == "1" && LBTotal.Text != "0")
                {
                    LBEmergencia.Text = "500";
                    calcularCirculacion();
                    calcularHospedaje();
                    calcularAlimento();
                    calcularTransporte();
                    calcularDepreciacion();
                    calcularTotal();
                }
                if (CBEmergencia.Checked == false && DDLTipoViaje.SelectedValue == "1" && LBTotal.Text != "0")
                {
                    LBEmergencia.Text = "0";
                    calcularCirculacion();
                    calcularHospedaje();
                    calcularAlimento();
                    calcularTransporte();
                    calcularDepreciacion();
                    calcularTotal();
                }
            }
            catch (Exception Ex)
            {

                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void CBDesayuno_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                if (CBDesayuno.Checked == true && DDLTipoViaje.SelectedValue == "1" && LBTotal.Text != "0")
                {
                    calcularCirculacion();
                    calcularHospedaje();
                    calcularAlimento();
                    calcularTransporte();
                    calcularDepreciacion();

                    LBDesayuno.Text = "0";
                    calcularTotal();
                }
                if (CBDesayuno.Checked == false && DDLTipoViaje.SelectedValue == "1" && LBTotal.Text != "0")
                {
                    calcularCirculacion();
                    calcularHospedaje();
                    calcularAlimento();
                    calcularTransporte();
                    calcularDepreciacion();
                    calcularTotal();

                }
            }
            catch (Exception Ex)
            {

                Mensaje(Ex.Message, WarningType.Danger);
            }

        }

        protected void DDLVehiculo_TextChanged(object sender, EventArgs e)
        {
            if (DDLVehiculo.SelectedValue == "0")
            {
                txtplaca.Text = String.Empty;
                txtSerie.Text = string.Empty;
            }
            else
            {
                String vQuery = "VIATICOS_ObtenerGenerales 11, '" + DDLVehiculo.SelectedValue + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                foreach (DataRow item in vDatos.Rows)
                {
                    txtplaca.Text = item["placa"].ToString();
                    txtSerie.Text = item["serie"].ToString();

                }
            }
        }

        void estadosViaticos()
        {
            string vEstado = "";
            string vTipoViaje = "";
            string vTransporte = "";
            string vSolicitante = "";
            string vSubGerencia = "";
            string vCategoria = "";
            string vSubGerenciaA = "";
            string vCategoriaA = "";
            string vCotizacion = "";
            string vIDViaticos = Session["VIATICOS_CODIGO"].ToString();
            string vAprobador = Session["USUARIO"].ToString();

            String vQuery = "VIATICOS_Solicitud 9,'" + Session["VIATICOS_CODIGO"].ToString() + "'";
            DataTable vDatos = vConexion.obtenerDataTable(vQuery);
            foreach (DataRow item in vDatos.Rows)
            {
                vEstado = item["estado"].ToString();
                vTipoViaje = item["TipoViaje"].ToString();
                vTransporte = item["Transporte"].ToString();
                vSolicitante = item["Solicitante"].ToString();
                vSubGerencia = item["SubGerencia"].ToString();
                vCategoria = item["Categoria"].ToString();
            }
            String vQuery2 = "VIATICOS_Solicitud 10,'" + Session["VIATICOS_CODIGO"].ToString() + "'";
            DataTable vDatos2 = vConexion.obtenerDataTable(vQuery2);
            foreach (DataRow item in vDatos2.Rows)
            {
                vCotizacion = item["idViaticos"].ToString();
            }

            String vQueryA = "VIATICOS_Solicitud 12,'" + vAprobador + "'";
            DataTable vDatosA = vConexion.obtenerDataTable(vQueryA);
            foreach (DataRow item in vDatosA.Rows)
            {
                vSubGerenciaA = item["SubGerencia"].ToString();
                vCategoriaA= item["Categoria"].ToString();
            }

            //OBTENER CORREOS DE PARTICIPANTES
            string vQueryD = "VIATICOS_ObtenerGenerales 48," + Session["VIATICOS_CODIGO"];
            DataTable vDatosEmpleado = vConexion.obtenerDataTable(vQueryD);
            string vQueryJefe = "VIATICOS_ObtenerGenerales 53," + Session["VIATICOS_CODIGO"];
            DataTable vDatosJefe = vConexion.obtenerDataTable(vQueryJefe);
            string vQuerySG = "VIATICOS_ObtenerGenerales 54," + Session["VIATICOS_CODIGO"];
            DataTable vDatosSubGerencte = vConexion.obtenerDataTable(vQuerySG);
            DataTable vDatosUsuario = (DataTable)Session["AUTHCLASS"];           
            //OBTENER CORREOS DE PARTICIPANTES

            if ((vEstado == "6" && vTransporte != "2" && vTransporte != "4") || (vEstado == "1" && vTransporte != "2" && vTransporte != "4" && (vCategoriaA=="2" || vCategoriaA == "1")))
            {
                String vNewEstado = "7";
                string vQueryNE = "VIATICOS_Solicitud 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "','" + Session["USUARIO"].ToString() + "','" + DDLVehiculo.SelectedValue + "','" + DDLEmpleado.SelectedValue + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A SOLICITANTE
                if (vDatosEmpleado.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosEmpleado.Rows)
                    {
                        if (!item["Email"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["Email"].ToString(),
                                typeBody.Viaticos,
                                item["Nombre"].ToString(),
                                "/pages/viaticos/liquidaciones.aspx",
                                "Se ha aprobado su solicitud de viáticos, favor realizar liquidación."
                                );
                        }
                    }
                }
                //ENVIAR A JEFE APRUEBA
                if (vDatosUsuario.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosUsuario.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha aprobado solicitud de viáticos."
                                );
                        }
                    }
                }

            }
            else if (vCotizacion == vIDViaticos && vSubGerencia == "1" && vEstado == "1")
            {
                String vNewEstado = "4";
                string vQueryNE = "VIATICOS_Solicitud 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "','" + Session["USUARIO"].ToString() + "','" + DDLVehiculo.SelectedValue + "','" + DDLEmpleado.SelectedValue + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A GERENTE               
                            vService.EnviarMensaje(ConfigurationManager.AppSettings["SmtpFromGerencia"],
                                typeBody.Viaticos,
                                "RIGOBERTO PINEDA ZELAYA",
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha enviado solicitud de viáticos para su aprobación."
                                );
                    
                //ENVIAR A JEFE APRUEBA
                if (vDatosUsuario.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosUsuario.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha enviado solicitud de viáticos a gerente para su aprobación."
                                );
                        }
                    }
                }
            }
            else if (vCotizacion == vIDViaticos && vCategoria == "1" && vEstado == "1" && vTransporte == "4")
            {
                String vNewEstado = "7";
                string vQueryNE = "VIATICOS_Solicitud 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "','" + Session["USUARIO"].ToString() + "','" + DDLVehiculo.SelectedValue + "','" + DDLEmpleado.SelectedValue + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A SOLICITANTE
                if (vDatosEmpleado.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosEmpleado.Rows)
                    {
                        if (!item["Email"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["Email"].ToString(),
                                typeBody.Viaticos,
                                item["Nombre"].ToString(),
                                "/pages/viaticos/liquidaciones.aspx",
                                "Se ha aprobado su solicitud de viáticos, favor realizar liquidación."
                                );
                        }
                    }
                }
                //ENVIAR A JEFE 
                if (vDatosUsuario.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosUsuario.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha aprobado solicitud de viáticos."
                                );
                        }
                    }
                }

            }
            else if (vCategoria == "1" && vEstado == "1" && vTransporte != "4")
            {
                String vNewEstado = "7";
                string vQueryNE = "VIATICOS_Solicitud 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "','" + Session["USUARIO"].ToString() + "','" + DDLVehiculo.SelectedValue + "','" + DDLEmpleado.SelectedValue + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A SOLICITANTE
                if (vDatosEmpleado.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosEmpleado.Rows)
                    {
                        if (!item["Email"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["Email"].ToString(),
                                typeBody.Viaticos,
                                item["Nombre"].ToString(),
                                "/pages/viaticos/liquidaciones.aspx",
                                "Se ha aprobado su solicitud de viáticos, favor realizar liquidación."
                                );
                        }
                    }
                }
                //ENVIAR A JEFE APRUEBA
                if (vDatosUsuario.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosUsuario.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha aprobado solicitud de viáticos."
                                );
                        }
                    }
                }

            }
            else if (vCategoria == "1" && vEstado == "1" && vTransporte == "4")
            {
                String vNewEstado = "3";
                string vQueryNE = "VIATICOS_Solicitud 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "','" + Session["USUARIO"].ToString() + "','" + DDLVehiculo.SelectedValue + "','" + DDLEmpleado.SelectedValue + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A RRHH
                vService.EnviarMensaje(ConfigurationManager.AppSettings["SmtpFromDev"],
                                typeBody.Viaticos,
                                "GLADYS YOLANDA CRUZ",
                                "/pages/viaticos/cotizacion.aspx",
                                "Se ha enviado una solicitud para cotización de vuelo"
                                );
                //ENVIAR A JEFE APRUEBA
                if (vDatosUsuario.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosUsuario.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha aprobado solicitud de viáticos."
                                );
                        }
                    }
                }

            }
            else if (vCotizacion == vIDViaticos && vCategoria == "2" && vTransporte == "4")
            {
                String vNewEstado = "7";
                string vQueryNE = "VIATICOS_Solicitud 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "','" + Session["USUARIO"].ToString() + "','" + DDLVehiculo.SelectedValue + "','" + DDLEmpleado.SelectedValue + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A SOLICITANTE
                if (vDatosEmpleado.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosEmpleado.Rows)
                    {
                        if (!item["Email"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["Email"].ToString(),
                                typeBody.Viaticos,
                                item["Nombre"].ToString(),
                                "/pages/viaticos/liquidaciones.aspx",
                                "Se ha aprobado su solicitud de viáticos, favor realizar liquidación."
                                );
                        }
                    }
                }
                //ENVIAR A JEFE APRUEBA
                if (vDatosUsuario.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosUsuario.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha aprobado solicitud de viáticos."
                                );
                        }
                    }
                }

            }
            else if (vCategoria == "2" && vEstado == "1" && vTransporte != "4")
            {
                String vNewEstado = "7";
                string vQueryNE = "VIATICOS_Solicitud 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "','" + Session["USUARIO"].ToString() + "','" + DDLVehiculo.SelectedValue + "','" + DDLEmpleado.SelectedValue + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A SOLICITANTE
                if (vDatosEmpleado.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosEmpleado.Rows)
                    {
                        if (!item["Email"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["Email"].ToString(),
                                typeBody.Viaticos,
                                item["Nombre"].ToString(),
                                "/pages/viaticos/liquidaciones.aspx",
                                "Se ha aprobado su solicitud de viáticos, favor realizar liquidación."
                                );
                        }
                    }
                }
                //ENVIAR A JEFE APRUEBA
                if (vDatosUsuario.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosUsuario.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha aprobado solicitud de viáticos."
                                );
                        }
                    }
                }

            }
            else if (vCotizacion == vIDViaticos && vSubGerencia == "5" && vEstado == "1")
            {
                String vNewEstado = "4";
                string vQueryNE = "VIATICOS_Solicitud 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "','" + Session["USUARIO"].ToString() + "','" + DDLVehiculo.SelectedValue + "','" + DDLEmpleado.SelectedValue + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A GERENTE               
                vService.EnviarMensaje(ConfigurationManager.AppSettings["SmtpFromGerencia"],
                    typeBody.Viaticos,
                    "RIGOBERTO PINEDA ZELAYA",
                    "/pages/viaticos/aprobarViaticos.aspx",
                    "Se ha enviado solicitud de viáticos para su aprobación."
                    );

                //ENVIAR A JEFE APRUEBA
                if (vDatosUsuario.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosUsuario.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha enviado solicitud de viáticos a gerente para su aprobación."
                                );
                        }
                    }
                }

            }
            else if (vCotizacion == vIDViaticos && vSubGerencia != "1" && vSubGerencia != "5" && vEstado == "1")
            {
                String vNewEstado = "6";
                string vQueryNE = "VIATICOS_Solicitud 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "','" + Session["USUARIO"].ToString() + "','" + DDLVehiculo.SelectedValue + "','" + DDLEmpleado.SelectedValue + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A SUBGERENTE
                if (vDatosSubGerencte.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosSubGerencte.Rows)
                    {
                        if (!item["Email"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["Email"].ToString(),
                                typeBody.Viaticos,
                                item["Nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha enviado solicitud de viáticos que requiere su aprobación."
                                );
                        }
                    }
                }
                //ENVIAR A JEFE APRUEBA
                if (vDatosUsuario.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosUsuario.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha aprobado solicitud de viáticos."
                                );
                        }
                    }
                }

            }
            else if (vEstado == "6" && vTransporte == "2")
            {
                String vNewEstado = "4";
                string vQueryNE = "VIATICOS_Solicitud 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "','" + Session["USUARIO"].ToString() + "','" + DDLVehiculo.SelectedValue + "','" + DDLEmpleado.SelectedValue + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A GERENTE               
                vService.EnviarMensaje(ConfigurationManager.AppSettings["SmtpFromGerencia"],
                    typeBody.Viaticos,
                    "RIGOBERTO PINEDA ZELAYA",
                    "/pages/viaticos/aprobarViaticos.aspx",
                    "Se ha enviado solicitud de viáticos para su aprobación."
                    );

                //ENVIAR A JEFE APRUEBA
                if (vDatosUsuario.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosUsuario.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha enviado solicitud de viáticos a gerente para su aprobación."
                                );
                        }
                    }
                }

            }
            else if (vEstado == "6" && vTransporte == "4")
            {
                String vNewEstado = "4";
                string vQueryNE = "VIATICOS_Solicitud 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "','" + Session["USUARIO"].ToString() + "','" + DDLVehiculo.SelectedValue + "','" + DDLEmpleado.SelectedValue + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A GERENTE               
                vService.EnviarMensaje(ConfigurationManager.AppSettings["SmtpFromGerencia"],
                    typeBody.Viaticos,
                    "RIGOBERTO PINEDA ZELAYA",
                    "/pages/viaticos/aprobarViaticos.aspx",
                    "Se ha enviado solicitud de viáticos para su aprobación."
                    );

                //ENVIAR A JEFE APRUEBA
                if (vDatosUsuario.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosUsuario.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha enviado solicitud de viáticos a gerente para su aprobación."
                                );
                        }
                    }
                }

            }
            else if (vEstado == "4")
            {
                String vNewEstado = "7";
                string vQueryNE = "VIATICOS_Solicitud 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "','" + Session["USUARIO"].ToString() + "','" + DDLVehiculo.SelectedValue + "','" + DDLEmpleado.SelectedValue + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A SOLICITANTE
                if (vDatosEmpleado.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosEmpleado.Rows)
                    {
                        if (!item["Email"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["Email"].ToString(),
                                typeBody.Viaticos,
                                item["Nombre"].ToString(),
                                "/pages/viaticos/liquidar.aspx",
                                "Se ha aprobado su solicitud de viáticos, favor realizar liquidación."
                                );
                        }
                    }
                }
                //ENVIAR A JEFE APRUEBA
                if (vDatosUsuario.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosUsuario.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha aprobado solicitud de viáticos."
                                );
                        }
                    }
                }

            }
            else if (vSubGerencia == "1" && vTransporte != "4" && vEstado == "1" && vAprobador != "3627")
            {
                String vNewEstado = "4";
                string vQueryNE = "VIATICOS_Solicitud 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "','" + Session["USUARIO"].ToString() + "','" + DDLVehiculo.SelectedValue + "','" + DDLEmpleado.SelectedValue + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A GERENTE               
                vService.EnviarMensaje(ConfigurationManager.AppSettings["SmtpFromGerencia"],
                    typeBody.Viaticos,
                    "RIGOBERTO PINEDA ZELAYA",
                    "/pages/viaticos/aprobarViaticos.aspx",
                    "Se ha enviado solicitud de viáticos para su aprobación."
                    );

                //ENVIAR A JEFE APRUEBA
                if (vDatosUsuario.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosUsuario.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha enviado solicitud de viáticos a gerente para su aprobación."
                                );
                        }
                    }
                }

            }
            else if (vSubGerencia == "1" && vTransporte != "4" && vEstado == "1" && vAprobador == "3627")
            {
                String vNewEstado = "7";
                string vQueryNE = "VIATICOS_Solicitud 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "','" + Session["USUARIO"].ToString() + "','" + DDLVehiculo.SelectedValue + "','" + DDLEmpleado.SelectedValue + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A SOLICITANTE
                if (vDatosEmpleado.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosEmpleado.Rows)
                    {
                        if (!item["Email"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["Email"].ToString(),
                                typeBody.Viaticos,
                                item["Nombre"].ToString(),
                                "/pages/viaticos/liquidar.aspx",
                                "Se ha aprobado su solicitud de viáticos, favor realizar liquidación."
                                );
                        }
                    }
                }
                //ENVIAR A JEFE APRUEBA
                if (vDatosUsuario.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosUsuario.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha aprobado solicitud de viáticos."
                                );
                        }
                    }
                }

            }
            else if (vTransporte == "4" && vEstado == "1")
            {
                String vNewEstado = "3";
                string vQueryNE = "VIATICOS_Solicitud 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "','" + Session["USUARIO"].ToString() + "','" + DDLVehiculo.SelectedValue + "','" + DDLEmpleado.SelectedValue + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A RRHH
                vService.EnviarMensaje(ConfigurationManager.AppSettings["SmtpFromDev"],
                                typeBody.Viaticos,
                                "GLADYS YOLANDA CRUZ",
                                "/pages/viaticos/cotizacion.aspx",
                                "Se ha enviado una solicitud para cotización de vuelo"
                                );
                //ENVIAR A JEFE APRUEBA
                if (vDatosUsuario.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosUsuario.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha aprobado solicitud de viáticos."
                                );
                        }
                    }
                }

            }
            else if (vSubGerencia == "1" && vTransporte != "4" && vEstado == "2" && vTipoViaje == "1" && vSolicitante != "3627")
            {
                String vNewEstado = "4";
                string vQueryNE = "VIATICOS_Solicitud 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "','" + Session["USUARIO"].ToString() + "','" + DDLVehiculo.SelectedValue + "','" + DDLEmpleado.SelectedValue + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A GERENTE               
                vService.EnviarMensaje(ConfigurationManager.AppSettings["SmtpFromGerencia"],
                    typeBody.Viaticos,
                    "RIGOBERTO PINEDA ZELAYA",
                    "/pages/viaticos/aprobarViaticos.aspx",
                    "Se ha enviado solicitud de viáticos para su aprobación."
                    );

                //ENVIAR A JEFE APRUEBA
                if (vDatosUsuario.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosUsuario.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha enviado solicitud de viáticos a gerente para su aprobación."
                                );
                        }
                    }
                }

            }
            else if (vSolicitante == "3627" && vEstado == "4")
            {
                String vNewEstado = "7";
                string vQueryNE = "VIATICOS_Solicitud 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "','" + Session["USUARIO"].ToString() + "','" + DDLVehiculo.SelectedValue + "','" + DDLEmpleado.SelectedValue + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A SOLICITANTE
                if (vDatosEmpleado.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosEmpleado.Rows)
                    {
                        if (!item["Email"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["Email"].ToString(),
                                typeBody.Viaticos,
                                item["Nombre"].ToString(),
                                "/pages/viaticos/liquidaciones.aspx",
                                "Se ha aprobado su solicitud de viáticos, favor realizar liquidación."
                                );
                        }
                    }
                }
                //ENVIAR A JEFE APRUEBA
                if (vDatosUsuario.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosUsuario.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha aprobado solicitud de viáticos."
                                );
                        }
                    }
                }

            }
            else if (vSolicitante == "3627" && vTransporte != "4" && vEstado == "1")
            {
                String vNewEstado = "7";
                string vQueryNE = "VIATICOS_Solicitud 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "','" + Session["USUARIO"].ToString() + "','" + DDLVehiculo.SelectedValue + "','" + DDLEmpleado.SelectedValue + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A SOLICITANTE
                if (vDatosEmpleado.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosEmpleado.Rows)
                    {
                        if (!item["Email"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["Email"].ToString(),
                                typeBody.Viaticos,
                                item["Nombre"].ToString(),
                                "/pages/viaticos/liquidaciones.aspx",
                                "Se ha aprobado su solicitud de viáticos, favor realizar liquidación."
                                );
                        }
                    }
                }
                //ENVIAR A JEFE APRUEBA
                if (vDatosUsuario.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosUsuario.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha aprobado solicitud de viáticos."
                                );
                        }
                    }
                }

            }
            else if (vSubGerencia != "1" && vTransporte != "4" && vEstado == "1")
            {
                String vNewEstado = "6";
                string vQueryNE = "VIATICOS_Solicitud 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "','" + Session["USUARIO"].ToString() + "','" + DDLVehiculo.SelectedValue + "','" + DDLEmpleado.SelectedValue + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A SUBGERENTE
                if (vDatosSubGerencte.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosSubGerencte.Rows)
                    {
                        if (!item["Email"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["Email"].ToString(),
                                typeBody.Viaticos,
                                item["Nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha enviado solicitud de viáticos que requiere su aprobación."
                                );
                        }
                    }
                }
                //ENVIAR A JEFE APRUEBA
                if (vDatosUsuario.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosUsuario.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha aprobado solicitud de viáticos."
                                );
                        }
                    }
                }

            }
            else if (vCategoria == "2" && vTransporte != "4")
            {
                String vNewEstado = "7";
                string vQueryNE = "VIATICOS_Solicitud 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "','" + Session["USUARIO"].ToString() + "','" + DDLVehiculo.SelectedValue + "','" + DDLEmpleado.SelectedValue + "'";
                vConexion.ejecutarSql(vQueryNE);


                //ENVIAR A SOLICITANTE
                if (vDatosEmpleado.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosEmpleado.Rows)
                    {
                        if (!item["Email"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["Email"].ToString(),
                                typeBody.Viaticos,
                                item["Nombre"].ToString(),
                                "/pages/viaticos/liquidaciones.aspx",
                                "Se ha aprobado su solicitud de viáticos, favor realizar liquidación."
                                );
                        }
                    }
                }
                //ENVIAR A JEFE APRUEBA
                if (vDatosUsuario.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosUsuario.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha aprobado solicitud de viáticos."
                                );
                        }
                    }
                }

            }
            else if (vSolicitante == "3627" && vTransporte != "4")
            {
                String vNewEstado = "7";
                string vQueryNE = "VIATICOS_Solicitud 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "','" + Session["USUARIO"].ToString() + "','" + DDLVehiculo.SelectedValue + "','" + DDLEmpleado.SelectedValue + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A SOLICITANTE
                if (vDatosEmpleado.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosEmpleado.Rows)
                    {
                        if (!item["Email"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["Email"].ToString(),
                                typeBody.Viaticos,
                                item["Nombre"].ToString(),
                                "/pages/viaticos/liquidaciones.aspx",
                                "Se ha aprobado su solicitud de viáticos, favor realizar liquidación."
                                );
                        }
                    }
                }
                //ENVIAR A JEFE APRUEBA
                if (vDatosUsuario.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosUsuario.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha aprobado solicitud de viáticos."
                                );
                        }
                    }
                }

            }
        }

        void EnviarCorreo()
        {
            string id = Request.QueryString["id"];
            string tipo = Request.QueryString["tipo"];

            string vQueryD = "";
            DataTable vDatosEmpleado=null;
            string vQueryJefe = "";
            DataTable vDatosJefe = null;
            DataTable vDatos = null;

            if (id == null)
            {
                 vQueryD = "VIATICOS_ObtenerGenerales 56," + Session["USUARIO"];
                 vDatosEmpleado = vConexion.obtenerDataTable(vQueryD);
                 vQueryJefe = "VIATICOS_ObtenerGenerales 57," + Session["USUARIO"];
                 vDatosJefe = vConexion.obtenerDataTable(vQueryJefe);
                 vDatos = (DataTable)Session["AUTHCLASS"];
            }
            else
            {
                 vQueryD = "VIATICOS_ObtenerGenerales 48," + Session["VIATICOS_CODIGO"];
                 vDatosEmpleado = vConexion.obtenerDataTable(vQueryD);
                 vQueryJefe = "VIATICOS_ObtenerGenerales 53," + Session["VIATICOS_CODIGO"];
                 vDatosJefe = vConexion.obtenerDataTable(vQueryJefe);
                 vDatos = (DataTable)Session["AUTHCLASS"];
            }

            //ENVIAR A JEFE
            if (vDatosJefe.Rows.Count > 0)
            {
                foreach (DataRow item in vDatosJefe.Rows)
                {
                    if (!item["Email"].ToString().Trim().Equals(""))
                    {
                        vService.EnviarMensaje(item["Email"].ToString(),
                            typeBody.Viaticos,
                            item["Nombre"].ToString(),
                            "/pages/viaticos/aprobarViaticos.aspx",
                            "Se ha ingresado solicitud de viáticos que requiere de su aprobación."
                            );
                    }
                }
            }
            //ENVIAR A SOLICITANTE
            if (vDatos.Rows.Count > 0)
            {
                foreach (DataRow item in vDatos.Rows)
                {
                    if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                    {
                        vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                            typeBody.Viaticos,
                            item["nombre"].ToString(),
                            "/pages/viaticos/solicitudViaticos.aspx",
                            "Se ha enviado solicitud de viáticos a su jefe para ser aprobado."
                            );
                    }
                }
            }

        }

        protected void btnModarEnviar_Click(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            string tipo = Request.QueryString["tipo"];
            try
            {
                if (tipo == "1")
                {

                    //string vEmpleado = Convert.ToString(Session["VIATICOS_IDEMPLEADO"]);
                    string vQuery = "VIATICOS_Solicitud 11, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + txtcomentarioAprobar.Text + "','" + Session["USUARIO"].ToString() + "','" + DDLVehiculo.SelectedValue + "','" + DDLEmpleado.SelectedValue + "'";
                    Int32 vInfo = vConexion.ejecutarSql(vQuery);
                    if (vInfo == 1)
                    {

                        estadosViaticos();
                        DataTable vDatosSiguiente = vConexion.obtenerDataTable(vQuery);

                        string vEstadoViaticos = "";
                        String vCorreoSolicitante = "";
                        String vQuery4 = "VIATICOS_ObtenerGenerales 50,'" + Session["VIATICOS_CODIGO"] + "'";
                        DataTable vDatos4 = vConexion.obtenerDataTable(vQuery4);
                        foreach (DataRow item in vDatos4.Rows)
                        {
                            vEstadoViaticos = item["estado"].ToString();
                            vCorreoSolicitante = item["Correo"].ToString();
                        }
                        if (vEstadoViaticos == "7")
                        {

                            string vReporteViaticos = "Recibo Solicitud";
                            string vCorreoAdministrativo = "dzepeda@bancatlan.hn;gcruz@bancatlan.hn";
                            //string vCorreoAdministrativo = "acedillo@bancatlan.hn";
                            string vAsuntoRV = "Recibo de viaje";
                            string vBody = "Aprobación de viaje";
                            int vEstadoSuscripcion = 0;
                            string vQueryRep = "VIATICOS_ObtenerGenerales 51, '" + vReporteViaticos + "','" + vCorreoSolicitante + "','" + vCorreoAdministrativo + "','" + vAsuntoRV + "','" + vBody + "','" + vEstadoSuscripcion + "','" + Session["VIATICOS_CODIGO"] + "'";
                            vConexion.ejecutarSql(vQueryRep);

                            if (DDLTransporte.SelectedValue == "2")
                            {
                                string vReporteDepreciacion = "Recibo Depresiacion";
                                string vAsuntoRD = "Recibo de viaje con vehículo personal";
                                string vQueryRep1 = "VIATICOS_ObtenerGenerales 51, '" + vReporteDepreciacion + "','" + vCorreoSolicitante + "','" + vCorreoAdministrativo + "','" + vAsuntoRD + "','" + vBody + "','" + vEstadoSuscripcion + "','" + Session["VIATICOS_CODIGO"] + "'";
                                vConexion.ejecutarSql(vQueryRep1);
                            }
                        }
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal();", true);
                    Response.Redirect("aprobarViaticos.aspx");
                    }
                   
                }
                else if (tipo == "2")
                {
                    int vEmergencia = 0;
                    int vDesayuno = 0;
                    if (CBEmergencia.Checked == true)
                        vEmergencia = 1;
                    else
                        vEmergencia = 0;

                    if (CBDesayuno.Checked == true)
                        vDesayuno = 1;
                    else
                        vDesayuno = 0;
                    String vFormato = "yyyy-MM-dd HH:mm:ss"; //"dd/MM/yyyy HH:mm:ss"
                    //String vFormato = "dd/MM/yyyy HH:mm:ss"; //LOCAL
                    string vfechaI = Convert.ToDateTime(TxFechaInicio.Text).ToString(vFormato);
                    string vfechaF = Convert.ToDateTime(TxFechaRegreso.Text).ToString(vFormato);

                    string vQuery = "VIATICOS_Solicitud 4, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vfechaI + "'," +
                                    "'" + vfechaF + "','" + DDLMotivoViaje.SelectedValue + "'," +
                                    "'" + DDLTipoViaje.SelectedValue + "','" + DDLTransporte.SelectedValue + "'," +
                                    "'" + txtMotivoVehiculo.Text + "', '" + Session["USUARIO"].ToString() + "'," +
                                    "'" + DDLDestinoI.SelectedValue + "','" + DDLHotel.SelectedValue + "'," +
                                    "'" + DDLHabitacion.SelectedValue + "','" + vEmergencia + "', '" + txtNewPais.Text + "'," +
                                    "'" + txtNewHotel.Text + "','" + Convert.ToDecimal(vDesayuno) + "','" + Convert.ToDecimal(LBHospedaje.Text) + "'," +
                                    "'" + Convert.ToDecimal(LBDesayuno.Text) + "','" + Convert.ToDecimal(LBAlmuerzo.Text) + "','" + Convert.ToDecimal(LBCena.Text) + "','" + Convert.ToDecimal(LBDepresiacion.Text) + "'," +
                                    "'" + Convert.ToDecimal(LBTransporte.Text) + "','" + Convert.ToDecimal(LBEmergencia.Text) + "','" + Convert.ToDecimal(LBPeaje.Text) + "','" + Convert.ToDecimal(LBCirculacion.Text) + "'," +
                                    "'" + Convert.ToDecimal(LBSubTotal.Text) + "','" + Convert.ToDecimal(LBTotal.Text) + "','" + Session["USUARIO"].ToString() + "','" + DDLDestinoF.SelectedValue + "'";
                    Int32 vInfo = vConexion.ejecutarSql(vQuery);
                    if (vInfo != 0)
                    {

                        EnviarCorreo();
                        Response.Redirect("devolverViaticos.aspx");
                    }

                }
                else
                {

                    int vEmergencia = 0;
                    int vDesayuno = 0;
                    if (CBEmergencia.Checked == true)
                        vEmergencia = 1;
                    else
                        vEmergencia = 0;

                    if (CBDesayuno.Checked == true)
                        vDesayuno = 1;
                    else
                        vDesayuno = 0;
                    String vFormato = "yyyy-MM-dd HH:mm:ss"; //"dd/MM/yyyy HH:mm:ss"
                    //String vFormato = "dd/MM/yyyy HH:mm:ss"; //LOCAL
                    string vfechaI = Convert.ToDateTime(TxFechaInicio.Text).ToString(vFormato);
                    string vfechaF = Convert.ToDateTime(TxFechaRegreso.Text).ToString(vFormato);
                    int vEstadoV = 0;
                    if (DDLTipoViaje.SelectedValue == "1")
                        vEstadoV = 1;
                    else
                        vEstadoV = 3;
                    string vQuery = "VIATICOS_Solicitud 1, '" + vEstadoV + "','" + vfechaI + "'," +
                                    "'" + vfechaF + "','" + DDLMotivoViaje.SelectedValue + "'," +
                                    "'" + DDLTipoViaje.SelectedValue + "','" + DDLTransporte.SelectedValue + "'," +
                                    "'" + txtMotivoVehiculo.Text + "', '" + Session["USUARIO"].ToString() + "'," +
                                    "'" + DDLDestinoI.SelectedValue + "','" + DDLHotel.SelectedValue + "'," +
                                    "'" + DDLHabitacion.SelectedValue + "','" + vEmergencia + "', '" + txtNewPais.Text + "'," +
                                    "'" + txtNewHotel.Text + "','" + Convert.ToDecimal(vDesayuno) + "','" + Convert.ToDecimal(LBHospedaje.Text) + "'," +
                                    "'" + Convert.ToDecimal(LBDesayuno.Text) + "','" + Convert.ToDecimal(LBAlmuerzo.Text) + "','" + Convert.ToDecimal(LBCena.Text) + "','" + Convert.ToDecimal(LBDepresiacion.Text) + "'," +
                                    "'" + Convert.ToDecimal(LBTransporte.Text) + "','" + Convert.ToDecimal(LBEmergencia.Text) + "','" + Convert.ToDecimal(LBPeaje.Text) + "','" + Convert.ToDecimal(LBCirculacion.Text) + "'," +
                                    "'" + Convert.ToDecimal(LBSubTotal.Text) + "','" + Convert.ToDecimal(LBTotal.Text) + "','" + Session["USUARIO"].ToString() + "','" + DDLDestinoF.SelectedValue + "'";
                    Int32 vInfo = vConexion.ejecutarSql(vQuery);
                    if (vInfo == 1)
                    {
                        EnviarCorreo();
                    
                    //VERIFICAR SI EXISTE SOLICITUD
                    String vEstado = "";
                    string usu = Convert.ToString(Session["USUARIO"]);
                    //VALIDAR NO TENGA SOLICITUD PENDIENTE
                    String vQueryE = "VIATICOS_ObtenerGenerales 14, '" + usu + "'";
                    DataTable vDatosE = vConexion.obtenerDataTable(vQueryE);
                    foreach (DataRow item in vDatosE.Rows)
                    {
                        vEstado = item["estado"].ToString();
                    }


                    if (vEstado == "1" || vEstado == "2" || vEstado == "3" || vEstado == "4" || vEstado == "5" || vEstado == "6" || vEstado == "14")
                    {
                        btnCalcular.Enabled = false;
                        BtnCrearPermiso.Enabled = false;
                        LBEstado.Visible = true;
                    }
                    else
                    {
                        btnCalcular.Enabled = true;
                        BtnCrearPermiso.Enabled = true;
                        LBEstado.Visible = false;
                    }

                    //VERIFICAR SI EXISTE SOLICITUD

                    Mensaje("Solicitud de viáticos enviada con éxito", WarningType.Success);
                    limpiarCalculos();
                    limpiarPantalla();
                    limpiarSession();
                    UpdatePanelViaticos.Update();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal();", true);
                }
            }
            }
            catch (Exception Ex)
            {
                //throw;
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }
    

        void limpiarSession()
        {
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
        }
        protected void btnModalCerrar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal();", true);
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            if (txtcomentarioAprobar.Text == "" || txtcomentarioAprobar.Text == String.Empty)
            {
                Mensaje("Ingrese el motivo por el que devuelve solicitud", WarningType.Warning);
            }
            else
            {                
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openModal2();", true);
            }
        }

        protected void btnModalDevolver_Click(object sender, EventArgs e)
        {
            

            string vQuery = "VIATICOS_Solicitud 2, '" + Session["VIATICOS_CODIGO"] + "','" + txtcomentarioAprobar.Text + "','" + Session["USUARIO"].ToString() + "'";                              
            Int32 vInfo = vConexion.ejecutarSql(vQuery);
            if (vInfo == 1)
            {
                string vQueryD = "VIATICOS_ObtenerGenerales 48," + Session["VIATICOS_CODIGO"];
                DataTable vDatosEmpleado = vConexion.obtenerDataTable(vQueryD);
                DataTable vDatos = (DataTable)Session["AUTHCLASS"];


                //ENVIAR A JEFE
                if (vDatos.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatos.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/solicitudViaticos.aspx",
                                "Se ha regresado solicitud de viáticos exitosamente."
                                );
                        }
                    }
                }
                //ENVIAR A SOLICITANTE
                if (vDatosEmpleado.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosEmpleado.Rows)
                    {
                        if (!item["Email"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["Email"].ToString(),
                                typeBody.Viaticos,
                                item["Nombre"].ToString(),
                                "/pages/viaticos/devolverViaticos.aspx",
                                "Se ha regresado solicitud de viáticos.</b>Motivo: " + txtcomentarioAprobar.Text
                                );
                        }
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal2();", true);
            Response.Redirect("aprobarViaticos.aspx");
        }

        protected void btnModalCerrarDevolver_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal2();", true);
        }

        protected void BtnDevolverCotizacion_Click(object sender, EventArgs e)
        {
            if(txtcomentarioAprobar.Text=="" || txtcomentarioAprobar.Text==string.Empty)
                Mensaje("Ingrese el motivo por el que devuelve cotización", WarningType.Warning);
            else
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openModal3();", true);
        }

        protected void btnModalCerrarCotiza_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal3();", true);
        }

        protected void btnModalCotizar_Click(object sender, EventArgs e)
        {

            string vQuery = "VIATICOS_ObtenerGenerales 24, '" + Session["VIATICOS_CODIGO"] + "','"+txtcomentarioAprobar.Text+"'";
            Int32 vInfo = vConexion.ejecutarSql(vQuery);
            if (vInfo == 1)
            {

                SmtpService vService = new SmtpService();

                string vQueryD = "VIATICOS_ObtenerGenerales 48," + Session["VIATICOS_CODIGO"];
                DataTable vDatosEmpleado = vConexion.obtenerDataTable(vQueryD);
                DataTable vDatos = (DataTable)Session["AUTHCLASS"];


                //ENVIAR A JEFE
                if (vDatos.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatos.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/solicitudViaticos.aspx",
                                "Se ha regresado cotización de vuelo exitosamente."
                                );
                        }
                    }
                }
                //ENVIAR A RRHH               
                            vService.EnviarMensaje(ConfigurationManager.AppSettings["SmtpFromDev"],
                                typeBody.Viaticos,
                                "GLADYS YOLANDA CRUZ",
                                "/pages/viaticos/cotizacion.aspx",
                                "Se ha regresado cotización de vuelo.</b>Motivo: "+ txtcomentarioAprobar
                                );
                      
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal3();", true);
            Response.Redirect("aprobarViaticos.aspx");
        }

        void EncontrarPeajes()
        {
            if (DDLTipoViaje.SelectedValue == "1" && DDLTransporte.SelectedValue == "2" || DDLTipoViaje.SelectedValue == "1" && DDLTransporte.SelectedValue == "1")
            {
                Session["VIATICOS_KM"] = "0";
                Session["VIATICOS_PEAJE"] = "0";

                if (DDLDestinoI.SelectedValue == "110" && DDLDestinoF.SelectedValue == "63" || DDLDestinoI.SelectedValue == "63" && DDLDestinoF.SelectedValue == "110")
                    Session["VIATICOS_PEAJE"] = 6;
                if (DDLDestinoI.SelectedValue == "110" && DDLDestinoF.SelectedValue == "1" || DDLDestinoI.SelectedValue == "1" && DDLDestinoF.SelectedValue == "110")
                    Session["VIATICOS_PEAJE"] = 6;
                if (DDLDestinoI.SelectedValue == "110" && DDLDestinoF.SelectedValue == "19" || DDLDestinoI.SelectedValue == "19" && DDLDestinoF.SelectedValue == "110")
                    Session["VIATICOS_PEAJE"] = 2;
                if (DDLDestinoI.SelectedValue == "110" && DDLDestinoF.SelectedValue == "36" || DDLDestinoI.SelectedValue == "36" && DDLDestinoF.SelectedValue == "110")
                    Session["VIATICOS_PEAJE"] = 4;
                if (DDLDestinoI.SelectedValue == "110" && DDLDestinoF.SelectedValue == "40" || DDLDestinoI.SelectedValue == "40" && DDLDestinoF.SelectedValue == "110")
                    Session["VIATICOS_PEAJE"] = 2;
                if (DDLDestinoI.SelectedValue == "19" && DDLDestinoF.SelectedValue == "36" || DDLDestinoI.SelectedValue == "36" && DDLDestinoF.SelectedValue == "19")
                    Session["VIATICOS_PEAJE"] = 2;

                txtKmPeaje.Text = "Km: " + Convert.ToString(Session["VIATICOS_KM"]) + "    Peajes: " + Convert.ToString(Session["VIATICOS_PEAJE"]);

            }
        }
        protected void DDLDestinoI_TextChanged(object sender, EventArgs e)
        {
            EncontrarPeajes();
            //DDLHotel.SelectedValue = "0";
            //DDLHabitacion.SelectedValue = "0";
            //txtCostoHotel.Text = "L. 0";

        }

        protected void DDLDestinoF_TextChanged(object sender, EventArgs e)
        {
            EncontrarPeajes();

            if (DDLTipoViaje.SelectedValue == "1")
            {
                DDLHotel.Items.Clear();

                //HOTELES
                String vQuery5 = "VIATICOS_ObtenerGenerales 20,'" + DDLDestinoF.SelectedValue + "'";
                DataTable vDatos5 = vConexion.obtenerDataTable(vQuery5);
                DDLHotel.Items.Add(new ListItem { Value = "0", Text = "Seleccione hoteles..." });
                DDLHotel.Items.Add(new ListItem { Value = "x", Text = "Otros" });
                foreach (DataRow item in vDatos5.Rows)
                {
                    DDLHotel.Items.Add(new ListItem { Value = item["idHotel"].ToString(), Text = item["nombre"].ToString() });

                }
                DDLHotel.SelectedValue = "0";
                //HABITACIONES
                String vQuery = "VIATICOS_ObtenerGenerales 6,'" + DDLHotel.SelectedValue + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                DDLHabitacion.Items.Add(new ListItem { Value = "0", Text = "Seleccione hoteles..." });
                foreach (DataRow item in vDatos.Rows)
                {
                    DDLHabitacion.Items.Add(new ListItem { Value = item["idHabitacion"].ToString(), Text = item["nombre"].ToString() });

                }
                DDLHabitacion.SelectedValue = "0";
                txtCostoHotel.Text = "L. 0";
            }
        }

        protected void BtnCancelarSolicitud_Click(object sender, EventArgs e)
        {
            if (txtcomentarioAprobar.Text == "")
                Mensaje("Ingrese el motivo por el que cancela solicitud", WarningType.Warning);
            else
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openModal4();", true);
        }

        protected void btnModalECancelar_Click(object sender, EventArgs e)
        {
            string vQuery = "VIATICOS_Solicitud 6, '" + Session["VIATICOS_CODIGO"] + "','" + txtcomentarioAprobar.Text + "','" + Session["USUARIO"].ToString() + "'";
            Int32 vInfo = vConexion.ejecutarSql(vQuery);
            if (vInfo == 1)
            {
                string vQueryD = "VIATICOS_ObtenerGenerales 48," + Session["VIATICOS_CODIGO"];
                DataTable vDatosEmpleado = vConexion.obtenerDataTable(vQueryD);
                DataTable vDatos = (DataTable)Session["AUTHCLASS"];


                //ENVIAR A JEFE
                if (vDatos.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatos.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/solicitudViaticos.aspx",
                                "Se ha cancelado solicitud de viáticos exitosamente."
                                );                           
                        }
                    }
                }
                //ENVIAR A SOLICITANTE
                if (vDatosEmpleado.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosEmpleado.Rows)
                    {
                        if (!item["Email"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["Email"].ToString(),
                                typeBody.Viaticos,
                                item["Nombre"].ToString(),
                                "/pages/viaticos/solicitudViaticos.aspx",
                                "Se ha cancelado solicitud de viáticos exitosamente."
                                );
                        }
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal4();", true);
            Response.Redirect("aprobarViaticos.aspx");
            
        }

        protected void btnModalECerrar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal4();", true);
        }

        protected void BtnPrueba_Click(object sender, EventArgs e)
        {

            vService.EnviarMensaje("acedillo@bancatlan.hn",
                            typeBody.Viaticos,
                            "Adan Cedillo",
                            "/pages/viaticos/solicitudViaticos.aspx",
                            "Se ha enviado solicitud de viáticos a su jefe para ser aprobado."
                            );

           
        }

        protected void BtnConfirmarTransporte_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModalTransporte();", true);
        }

        protected void BtnRechazarTransporte_Click(object sender, EventArgs e)
        {
            DDLTransporte.SelectedValue = "0";
            DIVMotivoVehiculo.Visible = false;
            txtMotivoVehiculo.Text = "";
            UpdatePanelViaticos.Update();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModalTransporte();", true);
           
        }
    }
}