using BiometricoWeb.clases;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BiometricoWeb.pages.activos
{
    public partial class visitaDCPersonal : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            if (!IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    String vId = Request.QueryString["id"];
                    LbIdSolicitud.Text = vId;
                    cargarData(vId);
                }
            }
        }

        private void cargarData(String vId) {
            try{
                String vQuery = "[RSP_ActivosDC] 5," + vId;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    GvInternos.DataSource = vDatos;
                    GvInternos.DataBind();
                    Session["ACTIVOS_DC_INTERNOS"] = vDatos;
                    UPInternos.Visible = true;
                }

                vQuery = "[RSP_ActivosDC] 25," + vId;
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    vDatos.Columns.Add("cantEquipos");
                    for (int i = 0; i < vDatos.Rows.Count; i++){
                        vDatos.Rows[i]["cantEquipos"] = 0;
                    }

                    GvExterno.DataSource = vDatos;
                    Session["ACTIVOS_DC_EXTERNOS"] = vDatos;
                    GvExterno.DataBind();
                    UPExternos.Visible = true;
                }

                vQuery = "[RSP_ActivosDC] 26";
                vDatos = vConexion.obtenerDataTable(vQuery);
                DDLTipo.Items.Clear();
                DDLTipo.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                if (vDatos.Rows.Count > 0){
                    foreach (DataRow item in vDatos.Rows){
                        DDLTipo.Items.Add(new ListItem { Value = item["idTipoEquipo"].ToString(), Text = item["nombre"].ToString() });
                    }
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void GvInternos_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GvInternos.PageIndex = e.NewPageIndex;
                GvInternos.DataSource = (DataTable)Session["ACTIVOS_DC_INTERNOS"];
                GvInternos.DataBind();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GvInternos_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                String vId = e.CommandArgument.ToString();
                if (e.CommandName == "addEquipo"){
                    DDLTipo.SelectedValue = "0";
                    TxSerie.Text = "";
                    LbIdPersona.Text = vId;
                    Session["ACTIVOS_PROCESO"] = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GvExterno_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GvExterno.PageIndex = e.NewPageIndex;
                GvExterno.DataSource = (DataTable)Session["ACTIVOS_DC_EXTERNOS"];
                GvExterno.DataBind();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GvExterno_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                String vId = e.CommandArgument.ToString();
                if (e.CommandName == "addEquipo"){
                    DDLTipo.SelectedValue = "0";
                    TxSerie.Text = "";
                    LbIdPersona.Text = vId;
                    Session["ACTIVOS_PROCESO"] = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnAceptar_Click(object sender, EventArgs e){
            try{
                String vQuery = "[RSP_ActivosDC] 10" +
                    "," + LbIdSolicitud.Text +
                    "," + LbIdPersona.Text +
                    "," + DDLTipo.SelectedValue +
                    ",'" + TxSerie.Text + "'";
                int vInfo = vConexion.ejecutarSql(vQuery);
                if (vInfo == 1){
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);
                    Mensaje("Equipo Agregado", WarningType.Success);
                }else 
                    throw new Exception ("No se pudo agregar el equipo");
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e){
            try{
                String vQuery = "[RSP_ActivosDC] 27" +
                    "," + LbIdSolicitud.Text;
                int vInfo = vConexion.ejecutarSql(vQuery);
                if (vInfo == 2){
                    Response.Redirect("registroVisitaSeguridad.aspx");
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
    }
}