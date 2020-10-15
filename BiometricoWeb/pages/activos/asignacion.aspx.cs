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
    public partial class asignacion : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            if (!Page.IsPostBack) {
                if (Convert.ToBoolean(Session["AUTH"])) {
                    generales vGenerales = new generales();
                    DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                    if (!vGenerales.PermisosRecursosHumanos(vDatos))
                        Response.Redirect("/default.aspx");

                    cargar();
                }
            }
        }

        private void cargar() {
            try{
                String vQuery = "[RSP_SeguridadActivos] 1";
                DataTable vData = vConexion.obtenerDataTable(vQuery);
                DDLEmpleado.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vData.Rows) {
                    DDLEmpleado.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
                }

                vQuery = "[RSP_SeguridadActivos] 4";
                vData = vConexion.obtenerDataTable(vQuery);
                DDLTipoEquipo.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vData.Rows) {
                    DDLTipoEquipo.Items.Add(new ListItem { Value = item["idDetalle"].ToString(), Text = item["descripcion"].ToString() });
                }
            }catch (Exception ex) {
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type) {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void DDLTipoEquipo_SelectedIndexChanged(object sender, EventArgs e){
            try{
                String vQuery = "[RSP_SeguridadActivos] 3," + DDLTipoEquipo.SelectedValue;
                DataTable vData = vConexion.obtenerDataTable(vQuery);
                DDLEquipo.Items.Clear();
                DDLEquipo.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vData.Rows) {
                    DDLEquipo.Items.Add(new ListItem { Value = item["idEquipo"].ToString(), Text = item["marca"].ToString() + " - " + item["serie"].ToString() });
                }
                DDLEquipo.CssClass = "fstdropdown-select form-control";

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnAsignar_Click(object sender, EventArgs e){
            try{
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnConfirmar_Click(object sender, EventArgs e){
            try{
                String vQuery = "[RSP_SeguridadActivos] 2" +
                    "," + DDLEquipo.SelectedValue +
                    "," + DDLEmpleado.SelectedValue + 
                    "," + Session["USUARIO"].ToString();
                int vInfo = vConexion.ejecutarSql(vQuery);

                if (vInfo == 2){
                    Mensaje("Asignación realizada con éxito.", WarningType.Success);
                }

                cargar();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
    }
}