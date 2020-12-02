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
    public partial class tipoEquipo : System.Web.UI.Page
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
                String vQuery = "[ActivosGenerales] 1";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                if (vDatos.Rows.Count > 0){
                    Session["ACTIVOS_TIPOEQUIPO"] = vDatos;
                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                }

                vQuery = "[ActivosGenerales] 2";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLCategorias.Items.Clear();
                    DDLCategorias.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLCategorias.Items.Add(new ListItem { Value = item["idCategoria"].ToString(), Text = item["nombre"].ToString() });
                    }
                }

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void TxBusqueda_TextChanged(object sender, EventArgs e){

        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){

        }

        protected void BtnCrear_Click(object sender, EventArgs e){
            try{
                limpiarmodal();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                LbTitle.Text = "Nuevo Tipo de Equipo";
            }catch (Exception ex) { 
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        void limpiarmodal() {
            DDLCategorias.SelectedValue = "0";
            TxNombre.Text = string.Empty;
            DivMensaje.Visible = false;
        }

        protected void BtnAceptar_Click(object sender, EventArgs e){
            try{
                if (DDLCategorias.SelectedValue == "0")
                    throw new Exception("Por favor seleccione la categoría.");
                if (TxNombre.Text == "" || TxNombre.Text == string.Empty)
                    throw new Exception("Por favor ingrese el nombre.");

                String vQuery = "[ActivosGenerales] 3" +
                    "," + DDLCategorias.SelectedValue +
                    ",'" + TxNombre.Text + "'" +
                    ",'" + Session["USUARIO"].ToString() + "'";
                int vInfo = vConexion.ejecutarSql(vQuery);
                if (vInfo == 1)
                    Mensaje("Registro creado con éxito.", WarningType.Success);
                else 
                    Mensaje("Hubo un problema al crear el registro. Comuníquese con sistemas.", WarningType.Danger);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);
                cargarDatos();
                UpdateDivBusquedas.Update();
            }catch (Exception ex) {
                LbMensaje.Text = ex.Message;
                DivMensaje.Visible = true;
            }
        }
    }
}