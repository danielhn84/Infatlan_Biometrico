using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

namespace BiometricoWeb.pages
{
    public partial class marcajes : System.Web.UI.Page
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

        private void cargarDatos(){
            try{
                String vQuery = "[RSP_ObtenerEmpleados] 5";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                DDLEmpleados.Items.Clear();
                DDLEmpleados.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLEmpleados.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void BtnGuardar_Click(object sender, EventArgs e){
            try{
                String vFecha = Convert.ToDateTime(TxFecha.Text).ToString("yyyy-MM-dd HH:mm:ss");
                String vQuery = "[RSP_Marcajes] 2," + DDLEmpleados.SelectedValue + ",'" + vFecha + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    Mensaje("Marcaje registrado con éxito.", WarningType.Success);
                }else { 
                    Mensaje("Hubo problemas al registrar el maraje. Comuníquese con sistemas.", WarningType.Danger);
                }

                limpiarDatos();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e){
            limpiarDatos();
        }

        private void limpiarDatos() {
            DDLEmpleados.SelectedValue = "0";
            TxFecha.Text = string.Empty;
        }

        protected void DDLEmpleados_SelectedIndexChanged(object sender, EventArgs e){
            try{
                String vQuery = "[RSP_Marcajes] 1," + DDLEmpleados.SelectedValue;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                    Session["MARCAJES_EMPLEADO"] = vDatos;
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GVBusqueda.PageIndex = e.NewPageIndex;
                GVBusqueda.DataSource = (DataTable)Session["MARCAJES_EMPLEADO"];
                GVBusqueda.DataBind();
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }
    }
}