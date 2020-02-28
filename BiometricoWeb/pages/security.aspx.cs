using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace BiometricoWeb.pages
{
    public partial class security : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    generales vGenerales = new generales();
                    DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                    if (!vGenerales.PermisosPersonalSeguridad(vDatos))
                        Response.Redirect("/login.aspx");

                    TxBusqueda.Focus();
                    cargarDatos();
                }
            }
        }

        private void cargarDatos(){
            try{
                String vQuery = "RSP_Seguridad 3";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                    Session["SEG_ENTRADAS"] = vDatos;
                }

                vQuery = "RSP_Seguridad 5";
                vDatos = vConexion.obtenerDataTable(vQuery);
                DDLArticulos.Items.Clear();
                DDLArticulos.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLArticulos.Items.Add(new ListItem { Value = item["idDetalle"].ToString(), Text = item["descripcion"].ToString() });
                }

                vQuery = "RSP_ObtenerGenerales 12";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLDepartamento.Items.Clear();
                    DDLDepartamento.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLDepartamento.Items.Add(new ListItem { Value = item["idDepartamento"].ToString(), Text = item["nombre"].ToString() });
                    }
                }

                vQuery = "RSP_Seguridad 9";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLMotivo.Items.Clear();
                    DDLMotivo.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLMotivo.Items.Add(new ListItem { Value = item["idDetalle"].ToString(), Text = item["descripcion"].ToString() });
                    }
                }

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GVBusqueda.PageIndex = e.NewPageIndex;
                GVBusqueda.DataSource = (DataTable)Session["SEG_ENTRADAS"];
                GVBusqueda.DataBind();

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e){
            try{
                validaciones();
                String vQuery = "";
                DataTable vInfo = new DataTable();

                if (Session["ID_SALIDA"] != null) {
                    vQuery = "[RSP_Seguridad] 7" +
                        "," + Session["ID_SALIDA"].ToString() +
                        ",'" + TxNombre.Text + "'" +
                        ",'" + TxDestinatario.Text + "'" +
                        "," + DDLDepartamento.SelectedValue +
                        "," + DDLMotivo.SelectedValue +
                        ",'" + TxObservaciones.Text + "'";
                    vInfo = vConexion.obtenerDataTable(vQuery);
                    if (vInfo.Rows.Count > 0){
                        //enviaCorreo(vInfo);
                        cargarDatos();
                        UpdateDivBusquedas.Update();
                        Mensaje("Entrada guardada con éxito", WarningType.Success);
                    }else
                        Mensaje("Hubo un error al guardar la entrada.", WarningType.Danger);
                }else{
                    String vRSP = DDLMotivo.SelectedValue == "9" ? "[RSP_Seguridad] 12," : "[RSP_Seguridad] 1,";

                    if (DDLMotivo.SelectedValue == "13"){
                        vQuery = "[RSP_Seguridad] 17" +
                        ",'" + Session["USUARIO"].ToString() + "'" +
                        "," + DDLArticulos.SelectedValue +
                        "," + 1 +
                        ",'" + TxSerie.Text + "'" +
                        ",'" + TxObservaciones.Text + "'";
                        vConexion.ejecutarSql(vQuery);
                    }

                    vQuery = vRSP + "'" + TxNombre.Text + "'" +
                    "," + DDLArticulos.SelectedValue +
                    ",'" + TxSerie.Text + "'" +
                    ",'" + TxInventario.Text + "'" +
                    ",'" + TxDestinatario.Text + "'" +
                    ",'" + Session["USUARIO"].ToString() + "'" +
                    "," + DDLMotivo.SelectedValue +
                    ",'" + TxObservaciones.Text + "'" +
                    "," + DDLDepartamento.SelectedValue;
                    vInfo = vConexion.obtenerDataTable(vQuery);
                    if (vInfo.Rows.Count > 0){
                        cargarDatos();
                        UpdateDivBusquedas.Update();
                        Mensaje("Entrada guardada con éxito", WarningType.Success);
                    }else
                        Mensaje("Hubo un error al guardar la entrada.", WarningType.Danger);
                }
                limpiarFormulario();
                TxBusqueda.Focus();

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        private void validaciones() {
            if (TxNombre.Text == "" || TxNombre.Text == string.Empty)
                throw new Exception("Favor ingrese el nombre.");
            if (DDLArticulos.SelectedValue.Equals("0"))
                throw new Exception("Favor seleccione el tipo de articulo.");
            if (TxSerie.Text == "" || TxSerie.Text == string.Empty)
                throw new Exception("Favor ingrese la serie del articulo.");
            if (TxInventario.Text == "" || TxInventario.Text == string.Empty)
                throw new Exception("Favor ingrese el número de inventario.");
            if (DDLDepartamento.SelectedValue.Equals("0"))
                throw new Exception("Favor seleccione el departamento de destino.");
            if (TxDestinatario.Text == "" || TxDestinatario.Text == string.Empty)
                throw new Exception("Favor ingrese el destinatario.");
            if (DDLMotivo.SelectedValue.Equals("0"))
                throw new Exception("Favor seleccione un motivo de entrada.");

        }

        private void limpiarFormulario(){
            TxMensaje.Text = string.Empty;
            TxBusqueda.Text = string.Empty;

            DivSalidas.Visible = false;
            LbIdSalida.Text = string.Empty;
            LbNombreSalida.Text = string.Empty;
            LbArticuloSalida.Text = string.Empty;
            LbSerieSalida.Text = string.Empty;
            LbInventarioSalida.Text = string.Empty;
            LbFechaSalida.Text = string.Empty;

            TxNombre.Text = string.Empty;
            TxSerie.Text = string.Empty;
            TxDestinatario.Text = string.Empty;
            TxInventario.Text = string.Empty;
            DDLArticulos.SelectedIndex = -1;
            DDLMotivo.SelectedIndex = -1;
            TxObservaciones.Text = string.Empty;
            DDLDepartamento.SelectedIndex = -1;

            UpdatePanel3.Update();
            UpdatePanel1.Update();
            UpdatePanel2.Update();
        }

        protected void TxBusqueda_TextChanged(object sender, EventArgs e){
            try{
                if (TxBusqueda.Text != "" || TxBusqueda.Text != string.Empty){
                    String vQuery = "[RSP_Seguridad] 15,'" + TxBusqueda.Text + "'";
                    DataTable vVerificacion = vConexion.obtenerDataTable(vQuery);
                    if (vVerificacion.Rows.Count > 0){
                        TxBusqueda.Focus();
                        limpiarFormulario();
                        Mensaje("El número de serie tiene una salida pendiente. Favor ingrese otro.", WarningType.Warning);
                    }else{
                        vQuery = "[RSP_Seguridad] 8,'" + TxBusqueda.Text + "'";
                        DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                        if (vDatos.Rows.Count > 0){
                            Session["ID_SALIDA"] = vDatos.Rows[0]["id"].ToString();

                            DivSalidas.Visible = true;
                            LbIdSalida.Text = vDatos.Rows[0]["id"].ToString();
                            LbNombreSalida.Text = vDatos.Rows[0]["nombreSalida"].ToString();
                            LbSerieSalida.Text = vDatos.Rows[0]["serie"].ToString();
                            LbArticuloSalida.Text = vDatos.Rows[0]["articulo"].ToString();
                            LbInventarioSalida.Text = vDatos.Rows[0]["inventario"].ToString();
                            LbFechaSalida.Text = vDatos.Rows[0]["fechaSalida"].ToString();

                            TxInventario.Text = vDatos.Rows[0]["inventario"].ToString();
                            DDLArticulos.SelectedValue = vDatos.Rows[0]["idArticulo"].ToString();

                            DivBody.Visible = true;
                            TxMensaje.Text = "";
                            TxMensaje.Visible = false;
                            UpdatePanel1.Update();
                        }else{
                            TxInventario.Text = string.Empty;
                            DDLArticulos.SelectedIndex = -1;
                            Session["ID_SALIDA"] = null;
                            DivSalidas.Visible = false;
                            DivBody.Visible = false;
                            TxMensaje.Visible = true;
                            TxMensaje.Text = "Cree un nuevo registro.";
                            UpdatePanel1.Update();
                        }
                        TxNombre.Focus();
                        TxSerie.Text = TxBusqueda.Text;
                    }
                    UpdatePanel2.Update();
                }else
                    limpiarFormulario();

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e){
            limpiarFormulario();
        }

        protected void TxBuscaSerie_TextChanged(object sender, EventArgs e){
            try{
                if (TxBuscaSerie.Text != "" || TxBuscaSerie.Text != string.Empty){
                    String vQuery = "[RSP_Seguridad] 6,'" + TxBuscaSerie.Text + "'";
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                    Session["SEG_ENTRADAS"] = vDatos;

                }else 
                    cargarDatos();

                UpdateDivBusquedas.Update();
            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        private void enviaCorreo(DataTable vDatos) {
            //ENVIAR CORREO
            //String vQuery = "RSP_ObtenerEmpleados 2," + DDLAutorizado.SelectedValue;
            DataTable vDatosEmpleado = vConexion.obtenerDataTable("");

            SmtpService vService = new SmtpService();
            foreach (DataRow item in vDatosEmpleado.Rows){
                if (!item["emailEmpresa"].ToString().Trim().Equals("")){
                    vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                        typeBody.Seguridad,
                        item["nombre"].ToString(),
                        "ENTRADA-" + vDatos.Rows[0]["id"].ToString() + "-" + vDatos.Rows[0]["tabla"].ToString()
                        );
                }
            }

        }
    }
}