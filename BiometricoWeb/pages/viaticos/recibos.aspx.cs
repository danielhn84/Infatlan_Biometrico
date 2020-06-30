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
using System.IO;

namespace BiometricoWeb.pages.viaticos
{
    public partial class recibos : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //HFRecibo.Value = null;
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
                vTotalLiquidar = Convert.ToDecimal(Session["VIATICOS_LIQ_TOTAL"].ToString().Replace('.', ','));
            }
            else
            {
                 vTotalLiquidar = Convert.ToDecimal(Session["VIATICOS_LIQ_TOTAL"].ToString().Replace('.', ',')) - Convert.ToDecimal(Session["VIATICOS_COSTODEPRE"]);            
            }

            if (Session["VIATICOS_LIQ_TIPOVIAJE"].ToString() == "Internacional")
            {
                txtMontoSolicitado.Text = "0";
                txtMontoLiquidado.Text = "0";
                txtMontoSolicitado.Text = "$ " + vTotalLiquidar.ToString().Replace(',', '.');
                txtTipoViaje.Text = Convert.ToString(Session["VIATICOS_LIQ_TIPOVIAJE"]);
                txtSolicitante.Text = Convert.ToString(Session["VIATICOS_LIQ_EMPLEADO"]);
                txtSAP.Text = Convert.ToString(Session["VIATICOS_LIQ_SAP"]);
                txtPuesto.Text = Convert.ToString(Session["VIATICOS_LIQ_PUESTO"]);

                LBComentario.Text = "Comentario= " + Session["COMENTARIO_RECIBO"];
                txtMontoLiquidado.Text = "$ " + Convert.ToString(Session["MONTOLIQUIDADOR"]).Replace(',', '.');
                Decimal vDiff = Convert.ToDecimal(vTotalLiquidar) - Convert.ToDecimal(Session["MONTOLIQUIDADOR"]);
                txtfechaDiferencia.Text = "$ "+ vDiff.ToString().Replace(',', '.');

                txtAlerta.Text = txtSolicitante.Text + " DEBE DEPOSITAR " + txtfechaDiferencia.Text.Replace(',', '.');
            }
            else
            {
                txtMontoSolicitado.Text = "0";
                txtMontoLiquidado.Text = "0";
                txtMontoSolicitado.Text = "L. " + vTotalLiquidar.ToString().Replace(',', '.');
                txtTipoViaje.Text = Convert.ToString(Session["VIATICOS_LIQ_TIPOVIAJE"]);
                txtSolicitante.Text = Convert.ToString(Session["VIATICOS_LIQ_EMPLEADO"]);
                txtSAP.Text = Convert.ToString(Session["VIATICOS_LIQ_SAP"]);
                txtPuesto.Text = Convert.ToString(Session["VIATICOS_LIQ_PUESTO"]);

                LBComentario.Text = "Comentario= " + Session["COMENTARIO_RECIBO"];
                txtMontoLiquidado.Text = "L. " + Convert.ToString(Session["MONTOLIQUIDADOR"]).Replace(',', '.');
                Decimal vDiff = Convert.ToDecimal(vTotalLiquidar) - Convert.ToDecimal(Session["MONTOLIQUIDADOR"]);
                txtfechaDiferencia.Text ="L. "+ vDiff.ToString().Replace(',', '.');

                txtAlerta.Text = txtSolicitante.Text + " DEBE DEPOSITAR " + txtfechaDiferencia.Text.Replace(',', '.');
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

        protected void btnModalEnviar_Click(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            string tipo = Request.QueryString["tipo"];

            if (tipo == "1")
            {
                string vQuery3 = "VIATICOS_Liquidaciones 9, '" + Session["VIATICOS_LIQ_CODIGO"].ToString() + "','" + Session["USUARIO"].ToString() + "'";
                vConexion.ejecutarSql(vQuery3);
                HFRecibo.Value = null;
                limpiarForm();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal();", true);
                Response.Redirect("buscarRecibo.aspx");
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
                vConexion.ejecutarSql(vQuery3);
                HFRecibo.Value = null;
                limpiarForm();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal();", true);
                Response.Redirect("buscarRecibo.aspx");

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
                vConexion.ejecutarSql(vQuery3);
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