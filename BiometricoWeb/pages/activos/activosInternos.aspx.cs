using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace BiometricoWeb.pages.activos
{
    public partial class activosInternos : System.Web.UI.Page
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
                    //cargarDatos();
                }
            }
        }

        private void cargarDatos(){
            try{
                String vQuery = "[RSP_SeguridadActivos] 5";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                    Session["SEG_ENTRADAS"] = vDatos;
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        
        protected void TxBusqueda_TextChanged(object sender, EventArgs e){
            try{
                if (TxBusqueda.Text != "" || TxBusqueda.Text != string.Empty){
                    String vQuery = "[RSP_SeguridadActivos] 5,'" + TxBusqueda.Text + "'";
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                    if (vDatos.Rows.Count < 1){
                        TxBusqueda.Focus();
                        limpiarFormulario();
                        Mensaje("El número de serie no esta en inventario o no está asignado. Favor ingrese otro.", WarningType.Warning);
                    }else{
                        Session["ACTIVOS_ID"] = vDatos.Rows[0]["idEquipo"].ToString();

                        DivSalidas.Visible = true;
                        LbIdSalida.Text = vDatos.Rows[0]["idEquipo"].ToString();
                        LbNombre.Text = vDatos.Rows[0]["nombre"].ToString();
                        LbSerieSalida.Text = vDatos.Rows[0]["serie"].ToString();
                        LbMarca.Text = vDatos.Rows[0]["marca"].ToString();
                        LbTipo.Text = vDatos.Rows[0]["TipoEquipo"].ToString();
                        LbCodInventario.Text = vDatos.Rows[0]["CodInventario"].ToString();

                        DivBody.Visible = true;
                        UpdatePanel1.Update();
                        
                    }
                }else
                    limpiarFormulario();

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
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
                        "," + Session["ID_SALIDA"].ToString();
                    vInfo = vConexion.obtenerDataTable(vQuery);
                    if (vInfo.Rows.Count > 0){
                        //enviaCorreo(vInfo);
                        cargarDatos();
                        UpdateDivBusquedas.Update();
                        Mensaje("Entrada guardada con éxito", WarningType.Success);
                    }else
                        Mensaje("Hubo un error al guardar la entrada.", WarningType.Danger);
                }else{
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
        }

        private void limpiarFormulario(){
            TxBusqueda.Text = string.Empty;

            DivSalidas.Visible = false;
            LbIdSalida.Text = string.Empty;
            LbNombre.Text = string.Empty;
            LbMarca.Text = string.Empty;
            LbSerieSalida.Text = string.Empty;
            LbCodInventario.Text = string.Empty;

            UpdatePanel3.Update();
            UpdatePanel1.Update();
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