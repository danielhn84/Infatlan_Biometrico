﻿using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;

namespace BiometricoWeb.pages.viaticos
{
    public partial class recibos : System.Web.UI.Page
    {
        db vConexion = new db();
        SmtpService vService = new SmtpService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Convert.ToBoolean(Session["AUTH"])){
                    cargarDataR();

                    string id = Request.QueryString["id"];
                    string tipo = Request.QueryString["tipo"];
                    switch (tipo)
                    {
                        case "1":
                            cargarImg();

                            BtnDevolver.Visible = true;
                            FURecibo.Visible = false;
                            LBComentario.Visible = false;
                            break;
                    }
                }else{
                    Response.Redirect("/login.aspx");
                }
            }
        }

        void cargarImg()
        {
            //IMAGEN1
            string vImagen1 = Session["IMG_RECIBO"].ToString();
            string srcImgen1 = "data:image;base64," + vImagen1;
            imgRecibo.Src = srcImgen1;
            HFRecibo.Value = "si";
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
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
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
        }
        void cargarDataR()
        {
            //Session["VIATICOS_LIQ_CODIGO"];
            //Session["VIATICOS_LIQ_IDTIPOVIAJE"] = null;
            //Session["VIATICOS_LIQ_CORREO"] = null;
            //Session["VIATICOS_LIQ_IDJEFE"] = null;

            String vQuery = "VIATICOS_ObtenerGeneralesViaticos 6, '" + Session["VIATICOS_LIQ_CODIGO"] + "'";
            DataTable vDatos = vConexion.obtenerDataTable(vQuery);
            foreach (DataRow item in vDatos.Rows)
            {
                txtFechaInicio.Text = item["fechaInicio"].ToString();
                txtFechaFin.Text = item["fechaFin"].ToString();
                Session["MONTOLIQUIDADOR"] = item["total"].ToString();
                Session["IMG_RECIBO"] = item["imgRecibo"].ToString();
                Session["COMENTARIO_RECIBO"] = item["comentarioRecibo"].ToString();
                Session["LIQ_ESTADO2"] = item["estado"].ToString();
            }
            Decimal vTotalLiquidar = 0;
            if (Session["LIQ_ESTADO2"].ToString() == "2")
            {
                //vTotalLiquidar = Convert.ToDecimal(Session["VIATICOS_LIQ_TOTAL"].ToString().Replace('.', ','));
                vTotalLiquidar = Convert.ToDecimal(Session["VIATICOS_LIQ_TOTAL"]);
            }
            else
            {
                vTotalLiquidar = Convert.ToDecimal(Session["VIATICOS_LIQ_TOTAL"]);
                // vTotalLiquidar = Convert.ToDecimal(Session["VIATICOS_LIQ_TOTAL"].ToString().Replace('.', ',')) - Convert.ToDecimal(Session["VIATICOS_COSTODEPRE"]);            
            }

            if (Session["VIATICOS_LIQ_TIPOVIAJE"].ToString() == "Internacional")
            {
                txtMontoSolicitado.Text = "0";
                txtMontoLiquidado.Text = "0";
                txtMontoSolicitado.Text = "$ " + string.Format("{0:N2}", vTotalLiquidar);
                txtTipoViaje.Text = Convert.ToString(Session["VIATICOS_LIQ_TIPOVIAJE"]);
                txtSolicitante.Text = Convert.ToString(Session["VIATICOS_LIQ_EMPLEADO"]);
                txtSAP.Text = Convert.ToString(Session["VIATICOS_LIQ_SAP"]);
                txtPuesto.Text = Convert.ToString(Session["VIATICOS_LIQ_PUESTO"]);

                LBComentario.Text = "Comentario= " + Session["COMENTARIO_RECIBO"];
                txtMontoLiquidado.Text = "$ " + string.Format("{0:N2}", Convert.ToDecimal(Session["MONTOLIQUIDADOR"])); 
                Decimal vDiff = vTotalLiquidar - Convert.ToDecimal(Session["MONTOLIQUIDADOR"]);
                txtfechaDiferencia.Text = "$ "+ string.Format("{0:N2}", vDiff); 

                txtAlerta.Text = txtSolicitante.Text + " DEBE DEPOSITAR " + txtfechaDiferencia.Text;
            }
            else
            {
                txtMontoSolicitado.Text = "0";
                txtMontoLiquidado.Text = "0";
                txtMontoSolicitado.Text = "L. " + string.Format("{0:N2}", vTotalLiquidar);
                txtTipoViaje.Text = Convert.ToString(Session["VIATICOS_LIQ_TIPOVIAJE"]);
                txtSolicitante.Text = Convert.ToString(Session["VIATICOS_LIQ_EMPLEADO"]);
                txtSAP.Text = Convert.ToString(Session["VIATICOS_LIQ_SAP"]);
                txtPuesto.Text = Convert.ToString(Session["VIATICOS_LIQ_PUESTO"]);

                LBComentario.Text = "Comentario= " + Session["COMENTARIO_RECIBO"];
                txtMontoLiquidado.Text = "L. " + string.Format("{0:N2}", Convert.ToDecimal(Session["MONTOLIQUIDADOR"])); 
                Decimal vDiff = vTotalLiquidar - Convert.ToDecimal(Session["MONTOLIQUIDADOR"]);
                txtfechaDiferencia.Text ="L. "+ string.Format("{0:N2}", vDiff);

                txtAlerta.Text = txtSolicitante.Text + " DEBE DEPOSITAR " + txtfechaDiferencia.Text;
            }
        }

        protected void BtnCrear_Click(object sender, EventArgs e)
        {
            if (HFRecibo.Value == "si")
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openModal();", true);
            else
                Mensaje("Debe ingresar foto de recibo", WarningType.Warning);
        }

        protected void BtnDevolver_Click(object sender, EventArgs e)
        {
            txtcomentario.Text = "";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openModal2();", true);
        }

        void enviarCorreo()
        {
            string id = Request.QueryString["id"];
            string tipo = Request.QueryString["tipo"];

            string vQueryD = "VIATICOS_ObtenerGenerales 48," + Session["VIATICOS_CODIGO"];
            DataTable vDatosEmpleado = vConexion.obtenerDataTable(vQueryD);
            string vQueryJefe = "VIATICOS_ObtenerGenerales 55," + Session["VIATICOS_CODIGO"];
            DataTable vDatosJefe = vConexion.obtenerDataTable(vQueryJefe);
            DataTable vDatos = (DataTable)Session["AUTHCLASS"];

            if (tipo == "1")
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
                                "/pages/viaticos/buscarRecibo.aspx",
                                "Se ha aprobado solicitud de voucher de liquidación."
                                );
                        }
                    }
                }
                //ENVIAR A SUBGERENCIA ADMINISTRATIVA
                if (vDatos.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatos.Rows)
                    {
                        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                                typeBody.Viaticos,
                                item["nombre"].ToString(),
                                "/pages/viaticos/buscarRecibo.aspx",
                                "Se ha aprobado solicitud de voucher de liquidación."
                                );
                        }
                    }
                }
            }
            else
            {
                //ENVIAR A SUBGERENCIA ADMINISTRATIVA
                if (vDatosJefe.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatosJefe.Rows)
                    {
                        if (!item["Email"].ToString().Trim().Equals(""))
                        {
                            vService.EnviarMensaje(item["Email"].ToString(),
                                typeBody.Viaticos,
                                item["Nombre"].ToString(),
                                "/pages/viaticos/buscarRecibo.aspx",
                                "Se ha ingresado una solicitud de voucher de pago de liquidación que requiere de su aprobación."
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
                                "/pages/viaticos/buscarRecibo.aspx",
                                "Se ha enviado solicitud de voucher de pago de liquidación que será aprobado por Sub Gerencia Administrativa."
                                );
                        }
                    }
                }
            }
        }

        void enviarSuscripcion()
        {
            String vCorreoSolicitante = "";
            String vQuery4 = "VIATICOS_ObtenerGenerales 50,'" + Session["VIATICOS_LIQ_CODIGO"] + "'";
            DataTable vDatos4 = vConexion.obtenerDataTable(vQuery4);
            foreach (DataRow item in vDatos4.Rows)
            {
                vCorreoSolicitante = item["Correo"].ToString();
            }

            string vReporteViaticos = "voucher";
            string vCorreoAdministrativo = "acedillo@bancatlan.hn;dzepeda@bancatlan.hn;gcruz@bancatlan.hn";
            //string vCorreoAdministrativo = "acedillo@bancatlan.hn";
            string vAsuntoRV = "Recibo de pago";
            string vBody = "Confirmación de pago";
            int vEstadoSuscripcion = 0;
            string vQueryRep = "VIATICOS_ObtenerGenerales 51, '" + vReporteViaticos + "','" + vCorreoSolicitante + "','" + vCorreoAdministrativo + "','" + vAsuntoRV + "','" + vBody + "','" + vEstadoSuscripcion + "','" + Session["VIATICOS_LIQ_CODIGO"] + "'";
            vConexion.ejecutarSql(vQueryRep);
        }

        protected void btnModalEnviar_Click(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            string tipo = Request.QueryString["tipo"];

            if (tipo == "1")
            {
                string vQuery3 = "VIATICOS_Liquidaciones 9, '" + Session["VIATICOS_LIQ_CODIGO"].ToString() + "','" + Session["USUARIO"].ToString() + "'";
                Int32 vInfo = vConexion.ejecutarSql(vQuery3);
                if (vInfo >= 1)
                {
                    enviarCorreo();
                    enviarSuscripcion();
                    HFRecibo.Value = null;
                    limpiarForm();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal();", true);
                    Response.Redirect("buscarRecibo.aspx");
                }
               
            }
            else
            {
                //ARCHIVO
                String vNombreDepot1 = String.Empty;
                HttpPostedFile bufferDeposito1T = FURecibo.PostedFile;
                byte[] vFileDeposito1 = null;
                string vExtension = string.Empty;

                if (bufferDeposito1T != null)
                {
                    vNombreDepot1 = FURecibo.FileName;
                    Stream vStream = bufferDeposito1T.InputStream;
                    BinaryReader vReader = new BinaryReader(vStream);
                    vFileDeposito1 = vReader.ReadBytes((int)vStream.Length);
                    vExtension = System.IO.Path.GetExtension(FURecibo.FileName);
                }
                String vArchivo = String.Empty;
                if (vFileDeposito1 != null)
                    vArchivo = Convert.ToBase64String(vFileDeposito1);

                string vQuery3 = "VIATICOS_Liquidaciones 7, '" + Session["VIATICOS_LIQ_CODIGO"].ToString() + "','" + vArchivo + "','" + Session["USUARIO"].ToString() + "'";
                Int32 vInfo = vConexion.ejecutarSql(vQuery3);
                if (vInfo >= 1)
                {
                    enviarCorreo();
                    HFRecibo.Value = null;
                    limpiarForm();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal();", true);
                    Response.Redirect("buscarRecibo.aspx");
                }
               

            }
            
        }

        protected void btnModalCerrar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal();", true);
        }

        protected void btnModalDevolver_Click(object sender, EventArgs e)
        {
            if (txtcomentario.Text == "")
            {
                H5Alerta.Visible = true;
            }
            else
            {
                H5Alerta.Visible = false;
                string vQuery3 = "VIATICOS_Liquidaciones 8, '" + Session["VIATICOS_LIQ_CODIGO"].ToString() + "','" + txtcomentario.Text + "','" + Session["USUARIO"].ToString() + "'";
                Int32 vInfo= vConexion.ejecutarSql(vQuery3);
                if (vInfo == 1)
                {
                    
                    //string vQueryD = "VIATICOS_ObtenerGenerales 48," + Session["VIATICOS_CODIGO"];
                    //DataTable vDatosEmpleado = vConexion.obtenerDataTable(vQueryD);

                    //Boolean vFlagEnvioSupervisor = false;
                    //DataTable vDatosJefatura = (DataTable)Session["AUTHCLASS"];
                    //if (vDatosJefatura.Rows.Count > 0)
                    //{
                    //    foreach (DataRow item in vDatosJefatura.Rows)
                    //    {
                    //        if (!item["emailEmpresa"].ToString().Trim().Equals(""))
                    //        {
                    //            vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                    //                typeBody.Viaticos,
                    //                item["nombre"].ToString(),
                    //                vDatosEmpleado.Rows[0]["Nombre"].ToString(),
                    //                 "Has devuelto baucher de pago de solicitud de viáticos solicitada el " + Convert.ToDateTime(txtFechaInicio.Text).ToString("dd-MM-yyyy"),
                    //                 "/pages/viaticos/buscarRecibo.aspx"
                    //                );
                    //            vFlagEnvioSupervisor = true;
                    //        }
                    //    }
                    //}

                    //if (vFlagEnvioSupervisor)
                    //{
                    //    foreach (DataRow item in vDatosJefatura.Rows)
                    //    {
                    //        //if (!item["emailEmpresa"].ToString().Trim().Equals(""))                        
                    //        vService.EnviarMensaje(item["Email"].ToString(),
                    //        typeBody.Viaticos,
                    //       item["Nombre"].ToString(),
                    //      "DINA ZEPEDA",
                    //       "Baucher de pago fue devuellto correspondiente a solicitud de viáticos solicitada el " + Convert.ToDateTime(txtFechaInicio.Text).ToString("dd-MM-yyyy"),
                    //        "/pages/viaticos/buscarRecibo.aspx"
                    //        );
                    //    }
                    //}
                }
                HFRecibo.Value = null;
                limpiarForm();
                txtcomentario.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal2();", true);
                Response.Redirect("buscarRecibo.aspx");
            }
        }

        protected void btnModalCerrarDevolver_Click(object sender, EventArgs e)
        {
            txtcomentario.Text = "";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal2();", true);
        }
    }
}