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
    public partial class sec_aprobaciones : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    generales vGenerales = new generales();
                    DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                    if (!vGenerales.PermisosPersonalSeguridad(vDatos))
                        Response.Redirect("/login.aspx");

                    cargarDatos();
                }
            }
        }

        private void cargarDatos() {
            try{
                String vQuery = "[RSP_Seguridad] 5";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLArticulo.Items.Clear();
                    DDLArticulo.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLArticulo.Items.Add(new ListItem { Value = item["idDetalle"].ToString(), Text = item["descripcion"].ToString() });
                    }
                }

                vQuery = "[RSP_Seguridad] 22," + Session["USUARIO"].ToString();
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                    Session["SEG_SALIDAS_EMPLEADO"] = vDatos;
                }

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void BtnEnviar_Click(object sender, EventArgs e){
            try{
                validarDatos();

                String vAccion = DDLAccion.SelectedValue == "0" ? "Entrada" : "Salida";
                String vUser = Session["USUARIO"].ToString();
                String vQuery = "[RSP_Seguridad] 17" +
                    "," + vUser +
                    "," + DDLArticulo.SelectedValue +
                    "," + DDLAccion.SelectedValue +
                    ",'" + TxSerie.Text + "'" +
                    ",'" + TxObservaciones.Text + "'";
                int vInfo = vConexion.ejecutarSql(vQuery);
                if (vInfo == 1)
                    Mensaje("Aprobacion de " + vAccion + " de equipo exitosa.", WarningType.Success);
                else
                    Mensaje("Hubo un problema al aprobar la " + vAccion + ". Comuníquese con Sistemas.", WarningType.Danger);

                limpiarFormulario();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        private void validarDatos() {
            if (DDLArticulo.SelectedValue == "0")
                throw new Exception("Favor seleccione un artículo.");
            if (TxSerie.Text == "" || TxSerie.Text == string.Empty)
                throw new Exception("Favor ingrese la serie del artículo.");
            if (DDLAccion.SelectedValue == "-1")
                throw new Exception("Favor seleccione una acción a realizar.");
        }

        protected void BtnCancelar_Click(object sender, EventArgs e){
            try{
                limpiarFormulario();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        private void limpiarFormulario() {
            DDLArticulo.SelectedIndex = -1;
            TxSerie.Text = string.Empty;
            DDLAccion.SelectedIndex = -1;
            TxObservaciones.Text = string.Empty;

            UpdatePanel1.Update();
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GVBusqueda.PageIndex = e.NewPageIndex;
                GVBusqueda.DataSource = (DataTable)Session["SEG_SALIDAS_EMPLEADO"];
                GVBusqueda.DataBind();

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
    }
}