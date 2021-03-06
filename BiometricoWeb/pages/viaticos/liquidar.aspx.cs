﻿using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

namespace BiometricoWeb.pages.viaticos
{
    public partial class liquidar : System.Web.UI.Page
    {
        db vConexion = new db();
        SmtpService vService = new SmtpService();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["CARGAR_DATA_LIQUIDACION"] = null;

            if (!Page.IsPostBack)
            {
                if (Convert.ToBoolean(Session["AUTH"])){
                    HFVerRecibo.Value = null;
                    cargarData();
                    llenarForm();

                    string id = Request.QueryString["id"];
                    string tipo = Request.QueryString["tipo"];
                    switch (tipo)
                    {
                        case "1":
                            deshabilitar();
                            btnArchivo.Visible = true;
                            llenarFormCompleto();
                            HFVerRecibo.Value = "si";
                            BtnCancelar.Visible = false;
                            HFValidarRecibos.Value = "si";
                            bloquarFactura();
                            break;
                        case "2":
                            HFVerRecibo.Value = "si";
                            BtnCancelar.Visible = false;
                            String vQuery2 = "VIATICOS_ObtenerGeneralesViaticos 6, '" + Session["VIATICOS_CODIGO"].ToString() + "'";
                            DataTable vDatos2 = vConexion.obtenerDataTable(vQuery2);
                            foreach (DataRow item in vDatos2.Rows)
                            {
                                LBComentarioJefe.Text = "Motivo de devolución= " + item["comentarioLiquidacion"].ToString();
                            }
                            llenarFormCompleto();
                            bloquarFactura();
                            break;
                        case "Rep":
                            Response.AddHeader("Content-Type", "application/pdf");
                            Response.AddHeader("Content-Disposition", "inline;");
                            Response.AddHeader("Cache-Control", "private, max-age=0, must-revalidate");
                            Response.AddHeader("Pragma", "public");
                            Response.BinaryWrite(Convert.FromBase64String(Session["LIQ_ARCHIVOS"].ToString()));
                            break;
                    }
                } else{
                    Response.Redirect("/login.aspx");
                }
            }
        }

        void deshabilitar()
        {
            DIVComentarioLiquidacion.Visible = true;
            txtFechaInicioReal.Enabled = false;
            txtFechaRegresaReal.Enabled = false;
            LBComArchivo.Visible = false;
            FULiquidacion.Visible = false;
            DIVLenarLiqui.Visible = false;
            BtnDevolverLiquidacion.Visible = true;
            GVLiquidaciones.Enabled = false;
            LBComentarioJefe.Visible = false;
            BtnCrearLiquidacion.Text = "Aprobar liquidación";
            btnModarEnviar.Text = "Aprobar";
        }
      
        void llenarFormCompleto()
        {
            //LLENAR DETALLE DE GASTOS
            DataTable vDatos = new DataTable();
            vDatos = vConexion.obtenerDataTable("VIATICOS_ObtenerGeneralesViaticos 2, '" + Session["VIATICOS_CODIGO"].ToString() + "'");
            GVLiquidaciones.DataSource = vDatos;
            GVLiquidaciones.DataBind();
            Session["VIATICOS_LIQUIDAR"] = vDatos;

            String vQuery3 = "VIATICOS_ObtenerGeneralesViaticos 6, '" + Session["VIATICOS_CODIGO"].ToString() + "'";
            DataTable vDatos3 = vConexion.obtenerDataTable(vQuery3);
            foreach (DataRow item in vDatos3.Rows)
            {
                Session["LIQ_ARCHIVOS"] =  item["facturas"].ToString();
                Session["LIQ_ESTADO2"]=    item["estado"].ToString();
            }

            if (Session["LIQ_ESTADO2"].ToString() == "2")
            {
                DIVMotivo2.Visible = true;
                txtMotivoCanceloLiquidacion.Text = Convert.ToString(Session["VIATICOS_COMCANCELAR"]);
                btnArchivo.Visible = false;
                H3Cancelado.Visible = true;
            }

            String vFormato = "yyyy-MM-ddTHH:mm";   //"dd/MM/yyyy HH:mm";
            string vFechaInicio = Convert.ToDateTime(Session["VIATICOS_FECHA_INICIO"]).ToString();
            string vFechaFin = Convert.ToDateTime(Session["VIATICOS_FECHA_FIN"]).ToString();
            TxFechaInicio.Text = vFechaInicio.ToString();
            TxFechaRegreso.Text = vFechaFin.ToString();
            Decimal vLiquidarM = 0;

           
            Decimal vLiqTotal = Convert.ToDecimal(Session["VIATICOS_LIQ_TOTAL"]);
            Decimal VDepMonto = Convert.ToDecimal(Session["VIATICOS_LIQ_COSTODEPRE"]);

            if (Session["LIQ_ESTADO2"].ToString() == "2")
              vLiquidarM= Convert.ToDecimal(Session["VIATICOS_LIQ_TOTAL"]);
            else
                vLiquidarM = vLiqTotal - VDepMonto;

           
            LBMontoSolicitado.Text = string.Format("{0:N2}", Convert.ToDecimal(Session["VIATICOS_LIQ_TOTAL"]));

            LBEmemergencia.Text = string.Format("{0:N2}", Convert.ToDecimal(Session["VIATICOS_COSTOEMERGENCIA"])); 
            LBDepreciacion.Text = string.Format("{0:N2}", VDepMonto);

            String vQuery2 = "VIATICOS_ObtenerGeneralesViaticos 3, '" + Session["VIATICOS_CODIGO"].ToString() + "'";
            DataTable vDatos2 = vConexion.obtenerDataTable(vQuery2);
            foreach (DataRow item in vDatos2.Rows)
            {
                txtFechaInicioReal.Text = Convert.ToDateTime(item["fechaInicio"].ToString()).ToString(vFormato);
                txtFechaRegresaReal.Text = Convert.ToDateTime(item["fechaFin"].ToString()).ToString(vFormato);
                LBMontoReal.Text = string.Format("{0:N2}", Convert.ToDecimal(item["total"].ToString())); 
                Session["LIQ_HOSPEDAJE"] = item["costoHospedaje"].ToString();
                Session["LIQ_DESAYUNO"] = item["costoDesayuno"].ToString();
                Session["LIQ_ALMUERZO"] = item["costoAlmuerzo"].ToString();
                Session["LIQ_CENA"] = item["costoCena"].ToString();
                Session["LIQ_TRANSPORTE"] = item["costoTransporte"].ToString();
                Session["LIQ_PEAJE"] = item["costoPeaje"].ToString();
                Session["LIQ_CIRCULACION"] = item["costoCirculacion"].ToString();
            }
          
            //LBMontoReal.Text = LBMontoReal.Text;
            LB1.Text = Convert.ToDecimal(Session["LIQ_CIRCULACION"]).ToString();
            LB2.Text = Convert.ToDecimal(Session["LIQ_HOSPEDAJE"]).ToString();
            LB3.Text = Convert.ToDecimal(Session["LIQ_DESAYUNO"]).ToString();
            LB4.Text = Convert.ToDecimal(Session["LIQ_ALMUERZO"]).ToString();
            LB5.Text = Convert.ToDecimal(Session["LIQ_CENA"]).ToString();
            LB6.Text = Convert.ToDecimal(Session["LIQ_TRANSPORTE"]).ToString();
            LB7.Text = Convert.ToDecimal(Session["LIQ_PEAJE"]).ToString();

            if (Convert.ToString(Session["VIATICOS_LIQ_IDTIPOVIAJE"]) == "1")
            {
              
                Decimal vTotalLiquidar = Convert.ToDecimal(Session["VIATICOS_LIQ_TOTAL"]);
                Decimal vDepreciacion = Convert.ToDecimal(Session["VIATICOS_COSTODEPRE"]);
                Decimal vTotalReal = Convert.ToDecimal(LBMontoReal.Text);
                Decimal vResTotal = 0;

                if (Session["LIQ_ESTADO2"].ToString() == "1")
                    vResTotal = vTotalLiquidar - vTotalReal;
                else
                    vResTotal =vTotalLiquidar;

                    Decimal vResTotalRecibir = vTotalReal - vTotalLiquidar;
               
                String vResTotalF = string.Format("{0:N2}", vResTotal);
                String vResTotalRecibirF = string.Format("{0:N2}", vResTotalRecibir);


                if (vTotalReal <= vTotalLiquidar)
                        txtAlerta.Text = "Viajes nacionales son liquidables, " + Session["VIATICOS_LIQ_EMPLEADO"] + " devolverá L. " + vResTotalF.ToString() + " a Infatlan.";
                    if (vTotalReal > vTotalLiquidar)
                        txtAlerta.Text = "Viajes nacionales son liquidables, Infatlan devolverá L. " + vResTotalRecibirF.ToString() + " a " + Session["VIATICOS_LIQ_EMPLEADO"];

                //LBMontoReal.Text = LBMontoReal.Text.Contains(",") ? LBMontoReal.Text.Replace(',', '.') : LBMontoReal.Text;

            }

        }
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        void cargarData()
        {
            Session["LIQ_HOSPEDAJE"] = 0;
            Session["LIQ_DESAYUNO"] = 0;
            Session["LIQ_ALMUERZO"] = 0;
            Session["LIQ_CENA"] = 0;
            Session["LIQ_TRANSPORTE"] = 0;
            Session["LIQ_PEAJE"] = 0;
            Session["LIQ_CIRCULACION"] = 0;
            if (HttpContext.Current.Session["CARGAR_DATA_LIQUIDACION"] == null)
            {
                //CARGAR TIPOS DE FACTURA
                String vQuery = "VIATICOS_ObtenerGeneralesViaticos 1";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                DDLTipoFactura.Items.Add(new ListItem { Value = "0", Text = "Seleccione factura..." });
                foreach (DataRow item in vDatos.Rows)
                {
                    DDLTipoFactura.Items.Add(new ListItem { Value = item["idtipo"].ToString(), Text = item["nombre"].ToString() });

                }

                Session["CARGAR_DATA_LIQUIDACION"] = "1";
            }
        }
        void llenarForm()
        {
            if (Convert.ToString(Session["VIATICOS_LIQ_IDTIPOVIAJE"]) == "2")
            {
                LBMonedaDepreciacion.Text = "$";
                LBMonedaEmergencia.Text = "$";
                LBMonedaMontoReal.Text = "$";
                LBMonedaMontoSolicitado.Text = "$";
                LBMoneda1.Text = "$";
                LBMoneda2.Text = "$";
                LBMoneda3.Text = "$";
                LBMoneda4.Text = "$";
                LBMoneda5.Text = "$";
                LBMoneda6.Text = "$";
                LBMoneda7.Text = "$";

                String vQuery3 = "VIATICOS_ObtenerGeneralesViaticos 6, '" + Session["VIATICOS_CODIGO"].ToString() + "'";
                DataTable vDatos3 = vConexion.obtenerDataTable(vQuery3);
                foreach (DataRow item in vDatos3.Rows)
                {
                    Session["LIQ_ESTADO2"] = item["estado"].ToString();
                }

                if(Convert.ToString(Session["LIQ_ESTADO2"])=="2")
                    txtAlerta.Text = "Viajes internacional liquidable por ser cancelado, " + Session["VIATICOS_LIQ_EMPLEADO"] + " devolverá $ "+ string.Format("{0:c}", Convert.ToDecimal(Session["VIATICOS_LIQ_TOTAL"])) + " a Infatlan.";
                else
                     txtAlerta.Text = "Viajes internacionales no son liquidables, " + Session["VIATICOS_LIQ_EMPLEADO"] +" devolverá 0 $ a Infatlan.";
            }
            else
            {
                LBMonedaDepreciacion.Text = "L.";
                LBMonedaEmergencia.Text = "L.";
                LBMonedaMontoReal.Text = "L.";
                LBMonedaMontoSolicitado.Text = "L.";
                LBMoneda1.Text = "L.";
                LBMoneda2.Text = "L.";
                LBMoneda3.Text = "L.";
                LBMoneda4.Text = "L.";
                LBMoneda5.Text = "L.";
                LBMoneda6.Text = "L.";
                LBMoneda7.Text = "L.";
                //Session["VIATICOS_LIQ_TOTAL"] = Session["VIATICOS_LIQ_TOTAL"].ToString().Contains(".") ? Session["VIATICOS_LIQ_TOTAL"].ToString().Replace(".", ",") : Session["VIATICOS_LIQ_TOTAL"];
                //Session["VIATICOS_LIQ_COSTODEPRE"] = Session["VIATICOS_LIQ_COSTODEPRE"].ToString().Contains(".") ? Session["VIATICOS_LIQ_COSTODEPRE"].ToString().Replace(".", ",") : Session["VIATICOS_LIQ_COSTODEPRE"];

                decimal vTotalLiquidar = Convert.ToDecimal(Session["VIATICOS_LIQ_TOTAL"]);
                decimal vDepreciacion = Convert.ToDecimal(Session["VIATICOS_LIQ_COSTODEPRE"]);
                decimal vResTotal = vTotalLiquidar - vDepreciacion;
                //String vResTotalF = vResTotal.ToString().Contains(",") ? vResTotal.ToString().Replace(",", ".") : vResTotal.ToString();
                String vResTotalF = vResTotal.ToString();

                //Session["VIATICOS_LIQ_TOTAL"] = Convert.ToDecimal(Session["VIATICOS_LIQ_TOTAL"]) - vDepreciacion;
                //LBMontoReal.Text = vDepreciacion.ToString();
                txtAlerta.Text = "Viajes nacionales son liquidables, " + Session["VIATICOS_LIQ_EMPLEADO"] + " devolverá L. " + string.Format("{0:N2}", vResTotal)  + " a Infatlan.";
            }
           

            TxFechaInicio.Text = Convert.ToString(Session["VIATICOS_LIQ_FECHA_INICIO"]);
            TxFechaRegreso.Text = Convert.ToString(Session["VIATICOS_LIQ_FECHA_FIN"]);

            txtSolicitante.Text = Convert.ToString(Session["VIATICOS_LIQ_EMPLEADO"]);
            txtSAP.Text = Convert.ToString(Session["VIATICOS_LIQ_SAP"]);
            txtPuesto.Text = Convert.ToString(Session["VIATICOS_LIQ_PUESTO"]);

            String vFormato = "yyyy-MM-ddTHH:mm";   //"dd/MM/yyyy HH:mm";
            string vFechaInicio = Convert.ToDateTime(Session["VIATICOS_LIQ_FECHA_INICIO"]).ToString(vFormato);
            string vFechaFin = Convert.ToDateTime(Session["VIATICOS_LIQ_FECHA_FIN"]).ToString(vFormato);
            txtFechaInicioReal.Text = vFechaInicio.ToString();
            txtFechaRegresaReal.Text = vFechaFin.ToString();

            //Session["VIATICOS_LIQ_COSTODEPRE"] = Session["VIATICOS_LIQ_COSTODEPRE"].ToString().Contains(".") ? Session["VIATICOS_LIQ_COSTODEPRE"].ToString().Replace(".", ",") : Session["VIATICOS_LIQ_COSTODEPRE"];

            Decimal CostoEmergencia = Convert.ToDecimal(Session["VIATICOS_LIQ_COSTOEMERGENCIA"]);
            Decimal CostoDepreciacion = Convert.ToDecimal(Session["VIATICOS_LIQ_COSTODEPRE"]);
            Decimal MontoTotalSolicitado = Convert.ToDecimal(Session["VIATICOS_LIQ_TOTAL"]);
            Decimal MontoSolicitado = MontoTotalSolicitado - CostoDepreciacion;

            //LBMontoSolicitado.Text = MontoSolicitado.ToString().Contains(",")? MontoSolicitado.ToString().Replace(",","."): MontoSolicitado.ToString();
            LBEmemergencia.Text = CostoEmergencia.ToString();
            //LBDepreciacion.Text = CostoDepreciacion.ToString().Contains(",")? CostoDepreciacion.ToString().Replace(",","."): CostoDepreciacion.ToString();

            //LBMontoSolicitado.Text = MontoSolicitado.ToString();
            //LBDepreciacion.Text = CostoDepreciacion.ToString();

            //LBMontoSolicitado.Text = Session["VIATICOS_LIQ_TOTAL"].ToString();
            LBMontoSolicitado.Text = string.Format("{0:N2}", Convert.ToDecimal(Session["VIATICOS_LIQ_TOTAL"])).ToString();
            //LBMontoSolicitado.Text = Session["VIATICOS_LIQ_TOTAL"].ToString().Contains(".") ? Session["VIATICOS_LIQ_TOTAL"].ToString().Replace(".", ","): Session["VIATICOS_LIQ_TOTAL"].ToString();
            LBDepreciacion.Text = Convert.ToString(Session["VIATICOS_LIQ_COSTODEPRE"]);
        }

        protected void GVNewMateriales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable vDatos = (DataTable)Session["VIATICOS_LIQUIDAR"];
            if (e.CommandName == "eliminar")
            {
                String vHora = e.CommandArgument.ToString();
                if (Session["VIATICOS_LIQUIDAR"] != null)
                {

                    DataRow[] result = vDatos.Select("Hora = '" + vHora + "'");
                    foreach (DataRow row in result)
                    {
                        if (row["Hora"].ToString().Contains(vHora))
                        {

                            Decimal vMontoExistente = Convert.ToDecimal(LBMontoReal.Text);
                            Decimal vMontoRestar = Convert.ToDecimal(row["Monto"].ToString());
                            Decimal vResult = vMontoExistente - vMontoRestar;

                            LBMontoReal.Text = string.Format("{0:N2}", vResult);

                            //LBMontoReal.Text = vResult.ToString().Contains(",")? vResult.ToString().Replace(",","."): vResult.ToString();
                            if (row["IDTipoFactura"].ToString() == "1")
                                Session["LIQ_HOSPEDAJE"] = Convert.ToDecimal(Session["LIQ_HOSPEDAJE"]) - Convert.ToDecimal(vMontoRestar);
                            if (row["IDTipoFactura"].ToString() == "3")
                                Session["LIQ_DESAYUNO"] = Convert.ToDecimal(Session["LIQ_DESAYUNO"]) - Convert.ToDecimal(vMontoRestar);
                            if (row["IDTipoFactura"].ToString() == "4")
                                Session["LIQ_ALMUERZO"] = Convert.ToDecimal(Session["LIQ_ALMUERZO"]) - Convert.ToDecimal(vMontoRestar);
                            if (row["IDTipoFactura"].ToString() == "5")
                                Session["LIQ_CENA"] = Convert.ToDecimal(Session["LIQ_CENA"]) - Convert.ToDecimal(vMontoRestar);
                            if (row["IDTipoFactura"].ToString() == "2")
                                Session["LIQ_TRANSPORTE"] = Convert.ToDecimal(Session["LIQ_TRANSPORTE"]) - Convert.ToDecimal(vMontoRestar);
                            if (row["IDTipoFactura"].ToString() == "6")
                                Session["LIQ_PEAJE"] = Convert.ToDecimal(Session["LIQ_PEAJE"]) - Convert.ToDecimal(vMontoRestar);
                            if (row["IDTipoFactura"].ToString() == "7")
                                Session["LIQ_CIRCULACION"] = Convert.ToDecimal(Session["LIQ_CIRCULACION"]) - Convert.ToDecimal(vMontoRestar);

                          
                            LB1.Text = Session["LIQ_CIRCULACION"].ToString();
                            LB2.Text = Session["LIQ_HOSPEDAJE"].ToString();
                            LB3.Text = Session["LIQ_DESAYUNO"].ToString();
                            LB4.Text = Session["LIQ_ALMUERZO"].ToString();
                            LB5.Text = Session["LIQ_CENA"].ToString();
                            LB6.Text = Session["LIQ_TRANSPORTE"].ToString();
                            LB7.Text = Session["LIQ_PEAJE"].ToString();


                            if (Convert.ToString(Session["VIATICOS_LIQ_IDTIPOVIAJE"]) == "1")
                            {
                               
                                decimal vTotalLiquidar = Convert.ToDecimal(Session["VIATICOS_LIQ_TOTAL"]);
                                decimal vDepreciacion = Convert.ToDecimal(Session["VIATICOS_COSTODEPRE"]);
                                Decimal vTotalReal = Convert.ToDecimal(LBMontoReal.Text);
                                Decimal vResTotal = vTotalLiquidar - vTotalReal;
                                Decimal vResTotalRecibir = vTotalReal - vTotalLiquidar;

                                String vResTotalF = string.Format("{0:N2}", vResTotal);
                                String vResTotalRecibirF = string.Format("{0:N2}", vResTotalRecibir);

                              
                                if (vTotalReal<= vTotalLiquidar)
                                   txtAlerta.Text = "Viajes nacionales son liquidables, " + Session["VIATICOS_LIQ_EMPLEADO"] + " devolverá L. " + vResTotalF.ToString() + " a Infatlan.";
                                if (vTotalReal > vTotalLiquidar)
                                    txtAlerta.Text = "Viajes nacionales son liquidables, Infatlan devolverá L. " + vResTotalRecibirF.ToString() + " a " + Session["VIATICOS_LIQ_EMPLEADO"] ;
                               

                            }

                            vDatos.Rows.Remove(row);
                        }
                    }
                }
            }
            GVLiquidaciones.DataSource = vDatos;
            GVLiquidaciones.DataBind();
            Session["VIATICOS_LIQUIDAR"] = vDatos;
            UpdatePanel2.Update();
        }

        protected void CBFactura_CheckedChanged(object sender, EventArgs e)
        {
            txtNoFactura.Text = "";
            if (CBFactura.Checked == true)
                txtNoFactura.Enabled = true;
            else
                txtNoFactura.Enabled = false;
        }

        void validar()
        {
                   
            if (CBFactura.Checked == true)
            {
                if (DDLTipoFactura.SelectedValue == "0")
                    throw new Exception("Seleccione tipo de factura");
                 if(txtFechaFactura.Text == "")
                    throw new Exception("Ingrese fecha de factura");
                  if(txtcantidad.Text == "" )
                    throw new Exception("Ingrese monto de factura");
                   if(txtNoFactura.Text == "")
                    throw new Exception("Ingrese número de factura");
            }
            if (CBFactura.Checked == false)
            {
                if (DDLTipoFactura.SelectedValue == "0")
                    throw new Exception("Seleccione tipo de factura");
                if (txtFechaFactura.Text == "")
                    throw new Exception("Ingrese fecha de factura");
                if (txtcantidad.Text == "")
                    throw new Exception("Ingrese monto de factura");
               
            }
            
        }
        void sumarCostosViaticos()
        {
            Decimal vMonto = Convert.ToDecimal(txtcantidad.Text.Contains(".") ? Convert.ToDecimal(txtcantidad.Text.Replace(".", ",")) : Convert.ToDecimal(txtcantidad.Text));
            if (DDLTipoFactura.SelectedValue == "1")
            {               
                Session["LIQ_HOSPEDAJE"] = Convert.ToDecimal(Session["LIQ_HOSPEDAJE"].ToString()) + vMonto;
            }
            if (DDLTipoFactura.SelectedValue == "3")
            {               
                Session["LIQ_DESAYUNO"] = Convert.ToDecimal(Session["LIQ_DESAYUNO"].ToString()) + vMonto;
            }
            if (DDLTipoFactura.SelectedValue == "4")
            {              
                Session["LIQ_ALMUERZO"] = Convert.ToDecimal(Session["LIQ_ALMUERZO"].ToString()) + vMonto;
            }
            if (DDLTipoFactura.SelectedValue == "5")
            {               
                Session["LIQ_CENA"] = Convert.ToDecimal(Session["LIQ_CENA"].ToString()) + vMonto;
            }
            if (DDLTipoFactura.SelectedValue == "2")
            {              
                Session["LIQ_TRANSPORTE"] = Convert.ToDecimal(Session["LIQ_TRANSPORTE"].ToString()) + vMonto;
            }
            if (DDLTipoFactura.SelectedValue == "6")
            {              
                Session["LIQ_PEAJE"] = Convert.ToDecimal(Session["LIQ_PEAJE"].ToString()) + vMonto;
            }
            if (DDLTipoFactura.SelectedValue == "7")
            {               
                Session["LIQ_CIRCULACION"] = Convert.ToDecimal(Session["LIQ_CIRCULACION"].ToString()) + vMonto;
            }           

            LB1.Text = Session["LIQ_CIRCULACION"].ToString();
            LB2.Text = Session["LIQ_HOSPEDAJE"].ToString();
            LB3.Text = Session["LIQ_DESAYUNO"].ToString();
            LB4.Text = Session["LIQ_ALMUERZO"].ToString();
            LB5.Text = Session["LIQ_CENA"].ToString();
            LB6.Text = Session["LIQ_TRANSPORTE"].ToString();
            LB7.Text = Session["LIQ_PEAJE"].ToString();
        }
        

        protected void btnAceptarF_Click(object sender, EventArgs e)
        {

            try
            {
                validar();

                DataTable vData = new DataTable();
                DataTable vDatos = (DataTable)Session["VIATICOS_LIQUIDAR"];
                int vIdViaticos = Convert.ToInt32(Session["VIATICOS_LIQ_CODIGO"]);
                int vFacturado = 0;
                if (CBFactura.Checked == true)
                    vFacturado = 1;
                String vIdTipoFactura = DDLTipoFactura.SelectedValue;
                String vTipoFactura = DDLTipoFactura.SelectedItem.Text;
                DateTime vFecha = Convert.ToDateTime(txtFechaFactura.Text);
                Decimal vMonto = Convert.ToDecimal(txtcantidad.Text);
                //Decimal vMonto = Convert.ToDecimal(txtcantidad.Text.Replace('.', ','));
                string vNumFactura = txtNoFactura.Text;
                string vHora= DateTime.Now.ToString("hh:mm:ss");
                Decimal vMontoReal = 0;

                vData.Columns.Add("idViaticos");
                vData.Columns.Add("Factura");
                vData.Columns.Add("IDTipoFactura");
                vData.Columns.Add("TipoFactura");
                vData.Columns.Add("Fecha");
                vData.Columns.Add("Monto");
                vData.Columns.Add("NoFac");
                vData.Columns.Add("Hora");

                if (vDatos == null)
                    vDatos = vData.Clone();

                if (vDatos != null)
                {
                    if (vDatos.Rows.Count < 1)
                    {
                        vDatos.Rows.Add(vIdViaticos, vFacturado, vIdTipoFactura, vTipoFactura, vFecha, string.Format("{0:N2}", vMonto), vNumFactura, vHora);
                        vMontoReal = Convert.ToDecimal(LBMontoReal.Text) + vMonto;
                        LBMontoReal.Text = string.Format("{0:N2}", vMontoReal);
                        sumarCostosViaticos();
                    }
                    else
                    {
                        //string vTotalCantidad = Session["STOCK_CANTIDAD_ATM"].ToString();
                        Boolean vRegistered = false;
                        for (int i = 0; i < vDatos.Rows.Count; i++)
                        {
                        string vNumExistente = vDatos.Rows[i]["NoFac"].ToString();
                            if (vNumFactura == vNumExistente && vNumFactura!="")
                            {
                                
                                vRegistered = true;
                               
                                throw new Exception("Ya ingresó este número de factura");
                                

                            }
                           
                        }

                        if (!vRegistered)
                        vDatos.Rows.Add(vIdViaticos, vFacturado, vIdTipoFactura, vTipoFactura, vFecha, string.Format("{0:N2}", vMonto), vNumFactura,vHora);
                        vMontoReal = Convert.ToDecimal(LBMontoReal.Text) + vMonto;
                        LBMontoReal.Text = string.Format("{0:N2}", vMontoReal);
                        txtNoFactura.Enabled = true;
                        sumarCostosViaticos();
                    }
                    if (Convert.ToString(Session["VIATICOS_LIQ_IDTIPOVIAJE"]) == "1")
                    {
                      
                        decimal vTotalLiquidar = Convert.ToDecimal(Session["VIATICOS_LIQ_TOTAL"]);
                        decimal vDepreciacion = Convert.ToDecimal(Session["VIATICOS_COSTODEPRE"]);

                        Decimal vTotalReal = Convert.ToDecimal(LBMontoReal.Text);
                        Decimal vResTotal = vTotalLiquidar - vTotalReal;
                        Decimal vResTotalRecibir = vTotalReal - vTotalLiquidar;
                        String vResTotalF = string.Format("{0:N2}", vResTotal);
                        String vResTotalRecibirF = string.Format("{0:N2}", vResTotalRecibir);

                        if (vTotalReal <= vTotalLiquidar)
                            txtAlerta.Text = "Viajes nacionales son liquidables, " + Session["VIATICOS_LIQ_EMPLEADO"] + " devolverá L. " + vResTotalF.ToString() + " a Infatlan.";
                        if (vTotalReal > vTotalLiquidar)
                            txtAlerta.Text = "Viajes nacionales son liquidables, Infatlan devolverá L. " + vResTotalRecibirF.ToString() + " a " + Session["VIATICOS_LIQ_EMPLEADO"];

                    }

                    GVLiquidaciones.DataSource = vDatos;
                    GVLiquidaciones.DataBind();
                    Session["VIATICOS_LIQUIDAR"] = vDatos;
                    UPMateriales.Update();

                    DDLTipoFactura.SelectedIndex = -1;
                    txtcantidad.Text = "";
                    txtFechaFactura.Text = "";
                    CBFactura.Checked = true;
                    txtNoFactura.Enabled = true;
                    txtNoFactura.Text = "";


                }
            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, WarningType.Danger);
            }
            
        }

        protected void txtFechaFactura_TextChanged(object sender, EventArgs e)
        {
            String vFormato = "yyyy-MM-dd";
            string vFechaIngresa= Convert.ToDateTime(txtFechaFactura.Text).ToString(vFormato);
            string vFechaViajeI= Convert.ToDateTime(txtFechaInicioReal.Text).ToString(vFormato);
            string vFechaViajeF = Convert.ToDateTime(txtFechaRegresaReal.Text).ToString(vFormato);
            if (Convert.ToDateTime(vFechaIngresa) >= Convert.ToDateTime(vFechaViajeI) && Convert.ToDateTime(vFechaIngresa) <= Convert.ToDateTime(vFechaViajeF))
                vFormato = "yyyy-MM-dd";
            else
            {
                Mensaje("No se permite ingresar esta fecha", WarningType.Warning);
                txtFechaFactura.Text = "";
            }
        }

        protected void BtnCrearLiquidacion_Click(object sender, EventArgs e)
        {
            if(GVLiquidaciones.Rows.Count==0)
                Mensaje("Debe ingresar facturas a la liquidación", WarningType.Warning);
            else if (HFValidarRecibos.Value != "si")
                Mensaje("Debe agregar facturas en archivo formato PDF", WarningType.Warning);
            else
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openModal();", true);
        }

        protected void BtnDevolverLiquidacion_Click(object sender, EventArgs e)
        {
            if (txtComentarioLiq.Text == "")
                Mensaje("Ingrese motivo por el que devuelve la solicitud.",WarningType.Warning);
            else
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openModal2();", true);
        }

        private string GetExtension(string Extension)
        {
            switch (Extension)
            {
                case ".doc":
                    return "application/ms-word";
                case ".xls":
                    return "application/vnd.ms-excel";
                case ".ppt":
                    return "application/mspowerpoint";
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".zip":
                    return "application/zip";
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case ".wav":
                    return "audio/wav";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                    return "application/xml";
                default:
                    return "application/octet-stream";
            }
        }

        void estadosViaticos()
        {
            string vSolicitante = Session["VIATICOS_LIQ_IDEMPLEADO"].ToString();
            string vAprueba = Session["USUARIO"].ToString();
            string vTipoViaje = Session["VIATICOS_LIQ_IDTIPOVIAJE"].ToString();
            string vEstadoLiquidacion = "";
            string vEstado = "";
            string vCategoria = "";
            Decimal vTotalSolicitud=0;
            Decimal vTotalLiquidar=0;
            string vSubGerenciaA = "";
            string vCategoriaA = "";

            String vQuery = "VIATICOS_Solicitud 9,'" + Session["VIATICOS_CODIGO"].ToString() + "'";
            DataTable vDatos = vConexion.obtenerDataTable(vQuery);
            foreach (DataRow item in vDatos.Rows)
            {
                vEstado = item["estado"].ToString();
                //vTipoViaje = item["TipoViaje"].ToString();
                //vTransporte = item["Transporte"].ToString();
                //vSolicitante = item["Solicitante"].ToString();
                //vSubGerencia = item["SubGerencia"].ToString();
                vCategoria = item["Categoria"].ToString();
            }

            String vQuery2 = "VIATICOS_ObtenerGeneralesViaticos 9,'" + Session["VIATICOS_CODIGO"].ToString() + "'";
            DataTable vDatos2 = vConexion.obtenerDataTable(vQuery2);
            foreach (DataRow item in vDatos2.Rows)
            {
                vEstadoLiquidacion = item["Estado"].ToString();
                vTotalLiquidar = Convert.ToDecimal(item["TotalLiquidar"].ToString());
                vTotalSolicitud = Convert.ToDecimal(item["TotalSolicitud"].ToString());
            }

            String vQueryA = "VIATICOS_Solicitud 12,'" + vAprueba + "'";
            DataTable vDatosA = vConexion.obtenerDataTable(vQueryA);
            foreach (DataRow item in vDatosA.Rows)
            {
                vSubGerenciaA = item["SubGerencia"].ToString();
                vCategoriaA = item["Categoria"].ToString();
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

            if ((vCategoria=="2" && vAprueba != "389") || (vCategoria == "1" && vAprueba != "389") || ((vCategoriaA=="2" || vCategoriaA=="1") && vAprueba != "389"))
            {
                String vNewEstado = "9";
                string vQueryNE = "VIATICOS_ObtenerGeneralesViaticos 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A ADMINISTRACION
                vService.EnviarMensaje(ConfigurationManager.AppSettings["SmtpFromAdmin"],
                                typeBody.Viaticos,
                                "DINA ZEPEDA",
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha enviado una solicitud de liquidación de viáticos que requiere de su aprobación."
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
                                "Se ha aprobado solicitud de liquidación de viáticos."
                                );
                        }
                    }
                }

            }
            if (vAprueba == "3627" && vEstado == "8")
            {
                String vNewEstado = "9";
                string vQueryNE = "VIATICOS_ObtenerGeneralesViaticos 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A ADMINISTRACION
                vService.EnviarMensaje(ConfigurationManager.AppSettings["SmtpFromAdmin"],
                                typeBody.Viaticos,
                                "DINA ZEPEDA",
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha enviado una solicitud de liquidación de viáticos que requiere de su aprobación."
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
                                "Se ha aprobado solicitud de liquidación de viáticos."
                                );
                        }
                    }
                }

            }
            if (vCategoria != "2" && vCategoria != "1" && vAprueba != "389" && vAprueba != "3627" && vEstado!="10" && vCategoriaA != "2" && vCategoriaA != "1")
            {
                String vNewEstado = "10";
                string vQueryNE = "VIATICOS_ObtenerGeneralesViaticos 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "'";
                vConexion.ejecutarSql(vQueryNE);

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
                                "Se ha aprobado solicitud de liquidación de viáticos."
                                );
                        }
                    }
                }
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
                                "Se ha enviado solicitud de liquidación de viáticos que requiere su aprobación."
                                );
                        }
                    }
                }

            }
            if (vCategoria != "2" && vCategoria != "1" && vAprueba != "389" && vEstado == "10")
            {
                String vNewEstado = "9";
                string vQueryNE = "VIATICOS_ObtenerGeneralesViaticos 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "'";
                vConexion.ejecutarSql(vQueryNE);

                //ENVIAR A ADMINISTRACION
                vService.EnviarMensaje(ConfigurationManager.AppSettings["SmtpFromAdmin"],
                                typeBody.Viaticos,
                                "DINA ZEPEDA",
                                "/pages/viaticos/aprobarViaticos.aspx",
                                "Se ha enviado una solicitud de liquidación de viáticos que requiere de su aprobación."
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
                                "Se ha aprobado solicitud de liquidación de viáticos."
                                );
                        }
                    }
                }

            }
            //if (vEstadoLiquidacion == "2")
            //{
            //    String vNewEstado = "11";
            //    string vQueryNE = "VIATICOS_ObtenerGeneralesViaticos 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "'";
            //    vConexion.ejecutarSql(vQueryNE);
            //}
            if (vAprueba == "389")
            {
                if(vTipoViaje=="1")
                {
                    if (vEstadoLiquidacion == "1")
                    {
                        if (vTotalSolicitud > vTotalLiquidar)
                        {
                            String vNewEstado = "11";
                            string vQueryNE = "VIATICOS_ObtenerGeneralesViaticos 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "'";
                            vConexion.ejecutarSql(vQueryNE);

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
                                            "Se ha aprobado solicitud de liquidación de viáticos."
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
                                            "/pages/viaticos/buscarRecibo.aspx",
                                            "Se ha aprobado solicitud de liquidación de viáticos, favor subir el voucher de pago.<br>¡RECORDATORIO! Tiene 3 días habiles para subir voucher como evidencia de pago por liquidación, si no cumple, se aplicaran las politícas vigentes y se le deducirá de su quincena el valor a pagar.<br>Recuerde que por motivos de auditoría debe devolver todas las facturas originales a SubGerencia Administrativa que envió en su liquidación."
                                            );
                                    }
                                }
                            }

                        }
                        if (vTotalSolicitud <= vTotalLiquidar)
                        {
                            String vNewEstado = "13";
                            string vQueryNE = "VIATICOS_ObtenerGeneralesViaticos 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "'";
                            vConexion.ejecutarSql(vQueryNE);

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
                                            "Se ha aprobado solicitud de liquidación de viáticos."
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
                                            "/pages/viaticos/solicitarViaticos.aspx",
                                            "Se ha aprobado solicitud de liquidación de viáticos, Infatlan le hará depósito a su favor en caso de que su gasto fuera mayor al solicitado."
                                            );
                                    }
                                }
                            }

                        }
                    }
                    if (vEstadoLiquidacion == "2")
                    {
                        String vNewEstado = "11";
                        string vQueryNE = "VIATICOS_ObtenerGeneralesViaticos 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "'";
                        vConexion.ejecutarSql(vQueryNE);

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
                                        "Se ha aprobado solicitud de liquidación de viáticos."
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
                                        "/pages/viaticos/buscarRecibo.aspx",
                                        "Se ha aprobado solicitud de liquidación de viáticos, favor subir el voucher de pago.<br>¡RECORDATORIO! Tiene 3 días habiles para subir voucher como evidencia de pago por liquidación, si no cumple, se aplicaran las politícas vigentes y se le deducirá de su quincena el valor a pagar."
                                        );
                                }
                            }
                        }

                    }
                }
                if (vTipoViaje == "2")
                {
                    if (vEstadoLiquidacion == "1")
                    {
                        String vNewEstado = "13";
                        string vQueryNE = "VIATICOS_ObtenerGeneralesViaticos 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "'";
                        vConexion.ejecutarSql(vQueryNE);

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
                                        "Se ha aprobado solicitud de liquidación de viáticos."
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
                                        "/pages/viaticos/solicitarViaticos.aspx",
                                        "Se ha aprobado solicitud de liquidación de viáticos."
                                        );
                                }
                            }
                        }

                    }
                    if (vEstadoLiquidacion == "2")
                    {
                        String vNewEstado = "11";
                        string vQueryNE = "VIATICOS_ObtenerGeneralesViaticos 8, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vNewEstado + "'";
                        vConexion.ejecutarSql(vQueryNE);

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
                                        "Se ha aprobado solicitud de liquidación de viáticos."
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
                                        "/pages/viaticos/buscarRecibo.aspx",
                                        "Se ha aprobado solicitud de liquidación de viáticos, favor subir el voucher de pago.<br>¡RECORDATORIO! Tiene 3 días habiles para subir voucher como evidencia de pago por liquidación, si no cumple, se aplicaran las politícas vigentes y se le deducirá de su quincena el valor a pagar."
                                        );
                                }
                            }
                        }

                    }
                }
            }
        }

        protected void btnModarEnviar_Click(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            string tipo = Request.QueryString["tipo"];

            //ARCHIVO
            String vNombreDepot1 = String.Empty;
            HttpPostedFile bufferDeposito1T = FULiquidacion.PostedFile;
            byte[] vFileDeposito1 = null;
            string vExtension = string.Empty;

            if (bufferDeposito1T != null)
            {
                vNombreDepot1 = FULiquidacion.FileName;
                Stream vStream = bufferDeposito1T.InputStream;
                BinaryReader vReader = new BinaryReader(vStream);
                vFileDeposito1 = vReader.ReadBytes((int)vStream.Length);
                vExtension = System.IO.Path.GetExtension(FULiquidacion.FileName);
            }
            String vArchivo = String.Empty;
            if (vFileDeposito1 != null)
                vArchivo = Convert.ToBase64String(vFileDeposito1);

            
            String vFormato = "yyyy-MM-dd HH:mm:ss"; //"dd/MM/yyyy HH:mm:ss"
            //String vFormato = "dd/MM/yyyy HH:mm:ss"; //LOCAL

            String vfechaI = Convert.ToDateTime(txtFechaInicioReal.Text).ToString(vFormato);
            string vfechaF = Convert.ToDateTime(txtFechaRegresaReal.Text).ToString(vFormato);

            Decimal vsubTotal = Convert.ToDecimal(LBMontoReal.Text);
            String vSubTotalL = vsubTotal.ToString();
            Decimal vTotal = Convert.ToDecimal(LBMontoReal.Text);

            if (tipo == "1")
            {
                //string vQuery3 = "VIATICOS_ObtenerGeneralesViaticos 5, '" + Session["VIATICOS_CODIGO"].ToString() + "','"+ Session["USUARIO"].ToString() + "','"+ Session["VIATICOS_LIQ_IDTIPOVIAJE"].ToString() + "','"+ Session["VIATICOS_LIQ_IDEMPLEADO"] + "'";
                string vQuery3 = "VIATICOS_ObtenerGeneralesViaticos 7, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + Session["USUARIO"].ToString() + "'";
                Int32 vInfo= vConexion.ejecutarSql(vQuery3);
                DataTable vDatosSiguiente = vConexion.obtenerDataTable(vQuery3);
                if (vInfo == 1)
                {
                    estadosViaticos();


                    string vEstadoViaticos = "";
                    string vEstadoLiquidacion = "";
                    string vTransporte = "";
                    string vTipoViaje = "";
                    String vCorreoSolicitante = "";
                    String vQuery4 = "VIATICOS_ObtenerGenerales 50,'" + Session["VIATICOS_CODIGO"] + "'";
                    DataTable vDatos4 = vConexion.obtenerDataTable(vQuery4);
                    foreach (DataRow item in vDatos4.Rows)
                    {
                        vEstadoViaticos = item["estado"].ToString();
                        vEstadoLiquidacion = item["EstadoLiquidacion"].ToString();
                        vTransporte= item["Transporte"].ToString();
                        vTipoViaje= item["TipoViaje"].ToString();
                        vCorreoSolicitante = item["Correo"].ToString();
                    }

                    if (vEstadoViaticos == "13" && vEstadoLiquidacion == "2" && vTipoViaje == "2" || vEstadoViaticos == "11" && vEstadoLiquidacion == "1" && vTipoViaje == "1")
                    {
                        string vReporteViaticos = "Recibo Liquidacion";
                        string vCorreoAdministrativo = "dzepeda@bancatlan.hn";
                        //string vCorreoAdministrativo = "acedillo@bancatlan.hn";
                        string vAsuntoRV = "Recibo de liquidación";
                        string vBody = "Aprobación de liquidación";
                        int vEstadoSuscripcion = 0;
                        string vQueryRep = "VIATICOS_ObtenerGenerales 51, '" + vReporteViaticos + "','" + vCorreoSolicitante + "','" + vCorreoAdministrativo + "','" + vAsuntoRV + "','" + vBody + "','" + vEstadoSuscripcion + "','" + Session["VIATICOS_CODIGO"] + "'";
                        vConexion.ejecutarSql(vQueryRep);

                        if (vEstadoViaticos == "11" && vEstadoLiquidacion == "1" && vTipoViaje == "1")
                        {
                            string vReporteDeduccion = "Recibo Deduccion";
                            string vCorreoRRHH = "gcruz@bancatlan.hn";
                            //string vCorreoRRHH = "acedillo@bancatlan.hn";
                            string vAsuntoRD = "Recibo de Deduccion";
                            string vBodyRD = "Deduccion de planilla";
                            //int vEstadoSuscripcion = 0;
                            string vQueryRep2 = "VIATICOS_ObtenerGenerales 51, '" + vReporteDeduccion + "','" + vCorreoSolicitante + "','" + vCorreoRRHH + "','" + vAsuntoRD + "','" + vBodyRD + "','" + vEstadoSuscripcion + "','" + Session["VIATICOS_CODIGO"] + "'";
                            vConexion.ejecutarSql(vQueryRep2);
                        }

                    }


                    }
                HFVerRecibo.Value = null;
                Response.Redirect("aprobarViaticos.aspx");
            }
            else if (tipo == "2")
            {
                Decimal vHospedaje = Convert.ToDecimal(Session["LIQ_HOSPEDAJE"]);
                Decimal vDesayuno = Convert.ToDecimal(Session["LIQ_DESAYUNO"]);
                Decimal vAlmuerzo = Convert.ToDecimal(Session["LIQ_ALMUERZO"]);
                Decimal vCena = Convert.ToDecimal(Session["LIQ_CENA"]);
                Decimal vTransporte = Convert.ToDecimal(Session["LIQ_TRANSPORTE"]);
                Decimal vPeaje = Convert.ToDecimal(Session["LIQ_PEAJE"]);
                Decimal vCirculacion = Convert.ToDecimal(Session["LIQ_CIRCULACION"]);

                //CAMBIAR ESTADO A SOLICITADO
                string vQuery4 = "VIATICOS_Liquidaciones 3, '" + Session["VIATICOS_CODIGO"].ToString() + "', '" + Session["USUARIO"].ToString() + "'";
                Int32 vInfo1 = vConexion.ejecutarSql(vQuery4);

                if (FULiquidacion.HasFile != false)
                {
                    //ACTUALIZA ARCHIVO SI ES NUEVO
                    string vQueryA = "VIATICOS_Liquidaciones 6, '" + Session["VIATICOS_CODIGO"].ToString() + "', '" + vArchivo + "'";
                    vConexion.ejecutarSql(vQueryA);
                }

                //ELIMINAR LIQUIDACION A CAMBIAR SOLICITADO
                string vQuery3 = "VIATICOS_Liquidaciones 5, '" + Session["VIATICOS_CODIGO"].ToString() + "', '" + Session["USUARIO"].ToString() + "'";
                vConexion.ejecutarSql(vQuery3);

                //GUARDAR NUEVA LIQUIDACION DE COSTOS
                String vFormato2 = "yyyy-MM-dd HH:mm:ss"; //"dd/MM/yyyy HH:mm:ss"
                //String vFormato2 = "dd/MM/yyyy HH:mm:ss"; //LOCAL
                DataTable vDatos = (DataTable)Session["VIATICOS_LIQUIDAR"];
                for (int i = 0; i < vDatos.Rows.Count; i++)
                {
                    string vIdViaticos = vDatos.Rows[i]["idViaticos"].ToString();
                    string vFacturado = vDatos.Rows[i]["Factura"].ToString();
                    string vTipoFactura = vDatos.Rows[i]["IDTipoFactura"].ToString();
                    string vFecha = Convert.ToDateTime(vDatos.Rows[i]["Fecha"]).ToString(vFormato2);
                    String vMonto = vDatos.Rows[i]["Monto"].ToString();
                    string vNumFactura = vDatos.Rows[i]["NoFac"].ToString();
                    string vHora = vDatos.Rows[i]["Hora"].ToString();

                    //vMonto = vMonto.Replace(',', '.');
                    string vQuery2 = "VIATICOS_Liquidaciones 2, '" + vIdViaticos + "','" + vFacturado + "','" + vTipoFactura + "'," +
                        "'" + vFecha + "','" + Convert.ToDecimal(vMonto) + "','" + vNumFactura + "','" + vHora + "'";
                    vConexion.ejecutarSql(vQuery2);
                }

                //MODIFICAR LIQUIDACION
                int vEstado = 1;

                string vQuery5 = "VIATICOS_Liquidaciones 4, '" + Session["VIATICOS_CODIGO"].ToString() + "','" + vEstado + "','" + vfechaI + "'," +
                                "'" + vfechaF + "','" + vHospedaje + "'," +
                                "'" + vDesayuno + "','" + vAlmuerzo + "'," +
                                "'" + vCena + "', '" + vTransporte + "'," +
                                "'" + vPeaje + "','" + vCirculacion + "'," +
                                "'" + vSubTotalL + "','" + vTotal + "', " +
                                "'" + Session["USUARIO"].ToString() + "'";
                Int32 vInfo = vConexion.ejecutarSql(vQuery5);               
                //DataTable vDatosSiguiente = vConexion.obtenerDataTable(vQuery3);
                if (vInfo == 1)
                {

                    enviarCorreo();

                }
                HFVerRecibo.Value = null;
                Response.Redirect("devolverViaticos.aspx");
            }
            else
            {

                Decimal vHospedaje =  Convert.ToDecimal(Session["LIQ_HOSPEDAJE"]);
                Decimal vDesayuno = Convert.ToDecimal(Session["LIQ_DESAYUNO"]);
                Decimal vAlmuerzo =  Convert.ToDecimal(Session["LIQ_ALMUERZO"]);
                Decimal vCena =  Convert.ToDecimal(Session["LIQ_CENA"]);
                Decimal vTransporte = Convert.ToDecimal(Session["LIQ_TRANSPORTE"]);
                Decimal vPeaje = Convert.ToDecimal(Session["LIQ_PEAJE"]);
                Decimal vCirculacion =  Convert.ToDecimal(Session["LIQ_CIRCULACION"]);

                int vEstado = 1;
                string vQueryLiqui = "VIATICOS_Liquidaciones 1, '" + Session["VIATICOS_LIQ_CODIGO"].ToString() + "','" + vEstado + "','" + vfechaI + "'," +
                                "'" + vfechaF + "','" + vHospedaje + "'," +
                                "'" + vDesayuno + "','" + vAlmuerzo + "'," +
                                "'" + vCena + "', '" + vTransporte + "'," +
                                "'" + vPeaje + "','" + vCirculacion + "'," +
                                "'" + vSubTotalL + "','" + vTotal + "', " +
                "'" + vArchivo + "','" + Session["USUARIO"].ToString() + "'";
                vConexion.ejecutarSql(vQueryLiqui);

                String vFormato2 = "yyyy-MM-dd HH:mm:ss"; //"dd/MM/yyyy HH:mm:ss"
                //String vFormato2 = "dd/MM/yyyy HH:mm:ss"; //LOCAL
                DataTable vDatos = (DataTable)Session["VIATICOS_LIQUIDAR"];
                for (int i = 0; i < vDatos.Rows.Count; i++)
                {
                    string vIdViaticos = vDatos.Rows[i]["idViaticos"].ToString();
                    string vFacturado = vDatos.Rows[i]["Factura"].ToString();
                    string vTipoFactura = vDatos.Rows[i]["IDTipoFactura"].ToString();
                    string vFecha = Convert.ToDateTime(vDatos.Rows[i]["Fecha"]).ToString(vFormato2);
                    String vMonto = vDatos.Rows[i]["Monto"].ToString();
                    string vNumFactura = vDatos.Rows[i]["NoFac"].ToString();
                    string vHora = vDatos.Rows[i]["Hora"].ToString();

                    //vMonto = vMonto.Replace(',', '.');
                    string vQuery2 = "VIATICOS_Liquidaciones 2, '" + vIdViaticos + "','" + vFacturado + "','" + vTipoFactura + "'," +
                        "'" + vFecha + "','" + Convert.ToDecimal(vMonto) + "','" + vNumFactura + "','" + vHora + "'";
                    vConexion.ejecutarSql(vQuery2);
                }

                string vQuery3 = "VIATICOS_Liquidaciones 3, '" + Session["VIATICOS_LIQ_CODIGO"].ToString() + "', '"+ Session["USUARIO"].ToString() + "'";
                Int32 vInfo2= vConexion.ejecutarSql(vQuery3);
                if (vInfo2 == 1)
                {

                    enviarCorreo();

                }

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal();", true);
                LimpiarForm();
                HFVerRecibo.Value = null;
                Response.Redirect("liquidaciones.aspx");
            }
        }

        void enviarCorreo()
        {
            string vEstadoViaticos = "";
            string vEstadoLiquidacion = "";
            string vTipoViaje = "";
            string vTransporte = "";
            String vQuery4 = "VIATICOS_ObtenerGenerales 50,'" + Session["VIATICOS_LIQ_CODIGO"] + "'";
            DataTable vDatos4 = vConexion.obtenerDataTable(vQuery4);
            foreach (DataRow item in vDatos4.Rows)
            {
                vEstadoViaticos = item["estado"].ToString();
                vEstadoLiquidacion = item["EstadoLiquidacion"].ToString();
                vTipoViaje= item["TipoViaje"].ToString();
                vTransporte= item["Transporte"].ToString();
            }


            //OBTENER CORREOS DE PARTICIPANTES
            string vQueryD = "VIATICOS_ObtenerGenerales 48," + Session["VIATICOS_LIQ_CODIGO"];
            DataTable vDatosEmpleado = vConexion.obtenerDataTable(vQueryD);
            string vQueryJefe = "VIATICOS_ObtenerGenerales 53," + Session["VIATICOS_LIQ_CODIGO"];
            DataTable vDatosJefe = vConexion.obtenerDataTable(vQueryJefe);
            string vQuerySG = "VIATICOS_ObtenerGenerales 54," + Session["VIATICOS_LIQ_CODIGO"];
            DataTable vDatosSubGerencte = vConexion.obtenerDataTable(vQuerySG);
            DataTable vDatosUsuario = (DataTable)Session["AUTHCLASS"];
            //OBTENER CORREOS DE PARTICIPANTES

            if (vEstadoViaticos == "8")
            {
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
                                "Se ha enviado su liquidación de viaticos."
                                );
                        }
                    }
                }
                //ENVIAR A JEFE APRUEBA
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
                                "Se ha enviado liquidación de viáticos que requiere de su aprobación"
                                );
                        }
                    }
                }
            }
            if (vEstadoViaticos == "22")
            {
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
                                "Se ha enviado su liquidación de viaticos."
                                );
                        }
                    }
                }
                //ENVIAR A GERENTE
                vService.EnviarMensaje(ConfigurationManager.AppSettings["SmtpFromGerencia"],
                               typeBody.Viaticos,
                               "RIGOBERTO PINEDA ZELAYA",
                               "/pages/viaticos/aprobarViaticos.aspx",
                               "Se ha enviado solicitud de liquidación de viáticos para su aprobación."
                               );
            }
            if (vEstadoViaticos == "21")
            {
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
                                "Se ha enviado su liquidación de viaticos."
                                );
                        }
                    }
                }
                //ENVIAR A RRHH
                vService.EnviarMensaje(ConfigurationManager.AppSettings["SmtpFromDev"],
                                typeBody.Viaticos,
                                "GLADYS YOLANDA CRUZ",
                                "/pages/viaticos/aprobarViaticos.aspx",
                               "Se ha enviado solicitud de liquidación de viáticos que requiere de su aprobación."
                                );
            }
            if (vEstadoViaticos == "10")
            {
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
                                "Se ha enviado su liquidación de viaticos."
                                );
                        }
                    }
                }
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
                                "Se ha enviado solicitud de liquidación de viáticos que requiere su aprobación."
                                );
                        }
                    }
                }
            }

          
        }

        void LimpiarForm()
        {
            GVLiquidaciones.DataSource = null;
            GVLiquidaciones.DataBind();
            Session["LIQ_HOSPEDAJE"] = null;
            Session["LIQ_DESAYUNO"] = null;
            Session["LIQ_ALMUERZO"] = null;
            Session["LIQ_CENA"] = null;
            Session["LIQ_TRANSPORTE"] = null;
            Session["LIQ_PEAJE"] = null;
            Session["LIQ_CIRCULACION"] = null;
        }
        protected void btnModalCerrar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal();", true);
        }

        protected void btnModalDevolver_Click(object sender, EventArgs e)
        {
            string vQuery3 = "VIATICOS_ObtenerGeneralesViaticos 4, '" + Session["VIATICOS_CODIGO"].ToString() + "','"+ Session["USUARIO"].ToString() + "', '"+txtComentarioLiq.Text+"'";
            Int32 vInfo= vConexion.ejecutarSql(vQuery3);
            //DataTable vDatosSiguiente = vConexion.obtenerDataTable(vQuery);
            if (vInfo >= 1)
            {

                //OBTENER CORREOS DE PARTICIPANTES
                string vQueryD = "VIATICOS_ObtenerGenerales 48," + Session["VIATICOS_CODIGO"];
                DataTable vDatosEmpleado = vConexion.obtenerDataTable(vQueryD);
                string vQueryJefe = "VIATICOS_ObtenerGenerales 53," + Session["VIATICOS_CODIGO"];
                DataTable vDatosJefe = vConexion.obtenerDataTable(vQueryJefe);
                string vQuerySG = "VIATICOS_ObtenerGenerales 54," + Session["VIATICOS_CODIGO"];
                DataTable vDatosSubGerencte = vConexion.obtenerDataTable(vQuerySG);
                DataTable vDatosUsuario = (DataTable)Session["AUTHCLASS"];
                //OBTENER CORREOS DE PARTICIPANTES

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
                                "Se ha regresado su liquidación de viáticos.<br>Motivo: "+txtComentarioLiq.Text
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
                                "Se ha regresado liquidación de viáticos.<br>Motivo: " + txtComentarioLiq.Text
                                );
                        }
                    }
                }
            }
            HFVerRecibo.Value = null;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal2();", true);
            Response.Redirect("aprobarViaticos.aspx");
        }

        protected void btnModalCerrarDevolver_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal2();", true);
        }

        protected void GVLiquidaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVLiquidaciones.PageIndex = e.NewPageIndex;
                GVLiquidaciones.DataSource = (DataTable)Session["VIATICOS_LIQUIDAR"];
                GVLiquidaciones.DataBind();
            }
            catch (Exception Ex)
            {

            }
        }

        protected void btnArchivo_Click(object sender, EventArgs e)
        {
            //Response.Write("<script> window.open('" +"verRecibos.aspx" +"'); </script>");
              Response.Write("<script> window.open('" + "liquidar.aspx?id=1&tipo=Rep" + "'); </script>");
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            txtcomentario.Text = "";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openModal3();", true);
        }

        protected void btnModalCancelarLiq_Click(object sender, EventArgs e)
        {
            if (txtcomentario.Text == "")
                H5Alerta.Visible = true;
            else
            {
                String vFormato = "yyyy-MM-dd HH:mm:ss"; //"dd/MM/yyyy HH:mm:ss"
                //String vFormato = "dd/MM/yyyy HH:mm:ss"; //LOCAL
                string vfechaI = Convert.ToDateTime(txtFechaInicioReal.Text).ToString(vFormato);
                string vfechaF = Convert.ToDateTime(txtFechaRegresaReal.Text).ToString(vFormato);
                String vHospedaje = "0";
                String vDesayuno = "0";
                String vAlmuerzo = "0";
                String vCena = "0";
                String vTransporte = "0";
                String vPeaje = "0";
                String vCirculacion = "0";
                String vTotal = "0";
                String vArchivo = "";

                //vHospedaje = vHospedaje.Replace(',', '.');
                //vDesayuno = vDesayuno.Replace(',', '.');
                //vAlmuerzo = vAlmuerzo.Replace(',', '.');
                //vCena = vCena.Replace(',', '.');
                //vTransporte = vTransporte.Replace(',', '.');
                //vPeaje = vPeaje.Replace(',', '.');
                //vCirculacion = vCirculacion.Replace(',', '.');

                

                int vEstado = 2;
                string vQuery = "VIATICOS_Liquidaciones 1, '" + Session["VIATICOS_LIQ_CODIGO"].ToString() + "','" + vEstado + "','" + vfechaI + "'," +
                                "'" + vfechaI + "'," + vHospedaje + "," +
                                "" + vDesayuno + "," + vAlmuerzo + "," +
                                "" + vCena + ", " + vTransporte + "," +
                                "" + vPeaje + "," + vCirculacion + "," +
                                "" + vTotal + "," + vTotal + ", " +
                "'" + vArchivo + "','" + Session["USUARIO"].ToString() + "'";
                Int32 vInfo = vConexion.ejecutarSql(vQuery);

                String vFormato2 = "dd/MM/yyyy"; //"dd/MM/yyyy HH:mm:ss"       
                DataTable vDatos = (DataTable)Session["VIATICOS_LIQUIDAR"];
                for (int i = 0; i < vDatos.Rows.Count; i++)
                {
                    string vIdViaticos = vDatos.Rows[i]["idViaticos"].ToString();
                    string vFacturado = vDatos.Rows[i]["Factura"].ToString();
                    string vTipoFactura = vDatos.Rows[i]["IDTipoFactura"].ToString();
                    string vFecha = Convert.ToDateTime(vDatos.Rows[i]["Fecha"]).ToString(vFormato2);
                    String vMonto = vDatos.Rows[i]["Monto"].ToString();
                    string vNumFactura = vDatos.Rows[i]["NoFac"].ToString();
                    string vHora = vDatos.Rows[i]["Hora"].ToString();

                    vMonto = vMonto.Replace(',', '.');
                    string vQuery2 = "VIATICOS_Liquidaciones 2, '" + vIdViaticos + "','" + vFacturado + "','" + vTipoFactura + "'," +
                        "'" + vFecha + "','" + Convert.ToDecimal(vMonto) + "','" + vNumFactura + "','" + vHora + "'";
                    vConexion.ejecutarSql(vQuery2);
                }


                string vQuery3 = "VIATICOS_Liquidaciones 10, '" + Session["VIATICOS_LIQ_CODIGO"].ToString() + "', '" + Session["USUARIO"].ToString() + "','"+txtcomentario.Text+"'";
               Int32 vInfo1= vConexion.ejecutarSql(vQuery3);
                //DataTable vDatosSiguiente = vConexion.obtenerDataTable(vQuery);
                if (vInfo1 == 1)
                {

                    //OBTENER CORREOS DE PARTICIPANTES
                    string vQueryD = "VIATICOS_ObtenerGenerales 48," + Session["VIATICOS_LIQ_CODIGO"];
                    DataTable vDatosEmpleado = vConexion.obtenerDataTable(vQueryD);
                    string vQueryJefe = "VIATICOS_ObtenerGenerales 53," + Session["VIATICOS_LIQ_CODIGO"];
                    DataTable vDatosJefe = vConexion.obtenerDataTable(vQueryJefe);
                    string vQuerySG = "VIATICOS_ObtenerGenerales 54," + Session["VIATICOS_LIQ_CODIGO"];
                    DataTable vDatosSubGerencte = vConexion.obtenerDataTable(vQuerySG);
                    DataTable vDatosUsuario = (DataTable)Session["AUTHCLASS"];
                    //OBTENER CORREOS DE PARTICIPANTES

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
                                    "Se ha cancelado su liquidación de viáticos, su jefe deberá aprobarlo.<br>Motivo: "+txtMotivoCanceloLiquidacion.Text
                                    );
                            }
                        }
                    }
                    //ENVIAR A JEFE APRUEBA
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
                                    "Se ha cancelado solicitud de viáticos.<br>Motivo: " + txtMotivoCanceloLiquidacion.Text
                                    );
                            }
                        }
                    }

                }
                LimpiarForm();
                HFVerRecibo.Value = null;
                txtcomentario.Text = "";
                Response.Redirect("liquidaciones.aspx");               
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal3();", true);
            }
        }

        protected void btnModalCerrarLiq_Click(object sender, EventArgs e)
        {
            txtcomentario.Text = "";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal3();", true);
        }

        protected void DDLTipoFactura_TextChanged(object sender, EventArgs e)
        {
            bloquarFactura();
        }

        void bloquarFactura()
        {
            if (DDLTipoFactura.SelectedValue == "3" || DDLTipoFactura.SelectedValue == "4" || DDLTipoFactura.SelectedValue == "5")
            {
                CBFactura.Enabled = true;
                CBFactura.Checked = true;
                txtNoFactura.Enabled = true;
            }
            else
            {
                CBFactura.Enabled = false;
                CBFactura.Checked = true;
                txtNoFactura.Enabled = true;
            }
        }
    }
}