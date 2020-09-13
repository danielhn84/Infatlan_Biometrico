using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BiometricoWeb.pages
{
    public partial class firmantes : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    generales vGenerales = new generales();
                    DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                    if (!vGenerales.PermisosRecursosHumanos(vDatos))
                        Response.Redirect("/default.aspx");

                    CargarPuesto();
                }
            }
        }

        void CargarPuesto() {
            try{
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("[RSP_Constancias] 13");

                GVBusqueda.DataSource = vDatos;
                GVBusqueda.DataBind();
                Session["DATAPUESTOS"] = vDatos;
            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                string vIdEmpleado = e.CommandArgument.ToString();
                String vQuery = "[RSP_Constancias] 14," + vIdEmpleado;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                if (e.CommandName == "ActualizarEstado") {
                    LbModPuesto.Text = vIdEmpleado;
                    Session["ACCION"] = "1";
                    DivEstado.Visible = true;

                    foreach (DataRow item in vDatos.Rows) {
                        TxNombre.Text = item["nombre"].ToString();
                        DDLEstado.SelectedValue = item["estado"].ToString();
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }

            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void BtnAceptar_Click(object sender, EventArgs e){
            try{
                String vQuery = "[RSP_Constancias] 15" +
                    "," + LbModPuesto.Text + 
                    "," + DDLEstado.SelectedValue + 
                    ",'" + Session["USUARIO"].ToString() + "'";
                int vInfo = vConexion.ejecutarSql(vQuery);
                if (vInfo == 1){
                    Mensaje("Registro actualizado con éxito.", WarningType.Success);
                }else
                    Mensaje("Hubo un problema, comuníquese con sistemas", WarningType.Success);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
                throw;
            }
        }
    }
}