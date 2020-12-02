using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;

namespace BiometricoWeb.pages.documentacion
{
    public partial class docGrupos : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            try{
                if (!Page.IsPostBack){
                    if (Convert.ToBoolean(Session["AUTH"])){
                        DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                        cargarDatos();
                    }
                }
			}catch (Exception ex){
				String vError = ex.Message;
			}
        }

        private void cargarDatos() {
            try{
                String vQuery = "[RSP_Documentacion] 20";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                    Session["DOCUMENTOS_GRUPOS"] = vDatos;
                }
                
                vQuery = "[RSP_Documentacion] 6";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLEmpleado.Items.Clear();
                    DDLEmpleado.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLEmpleado.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
                    }
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        public void MensajeLoad(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", " document.addEventListener(\"DOMContentLoaded\", function (event) { infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "'); });", true);
        }

        private void select2() {
            String vScript = @"
                    $(function test() {
                        $('.select2').select2();
                        $('.ajax').select2({
                            ajax: {
                                url: 'https://api.github.com/search/repositories',
                                dataType: 'json',
                                delay: 250,
                                data: function (params) {
                                    return {
                                        q: params.term, // search term
                                        page: params.page
                                    };
                                },
                                processResults: function (data, params) {
                                    params.page = params.page || 1;
                                    return {
                                        results: data.items,
                                        pagination: {
                                            more: (params.page * 30) < data.total_count
                                        }
                                    };
                                },
                                cache: true
                            },
                            escapeMarkup: function (markup) {
                                return markup;
                            },
                            minimumInputLength: 1,
                        });
                    });
                    ";

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "select2", vScript, true);

        }

        private void limpiarModal() {
            TxNombre.Text = string.Empty;
            DDLEmpleado.SelectedValue = "0";
            DivMensaje.Visible = false;
            LbMensaje.Text = string.Empty;
            GvEmpleados.DataSource = null;
            GvEmpleados.DataBind();
        }

        protected void BtnNuevo_Click(object sender, EventArgs e){
            try{
                select2();
                limpiarModal();
                LitTitulo.Text = "Crear Grupo";
                Session["DOCUMENTOS_GRUPO_ACCION"] = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void TxBuscar_TextChanged(object sender, EventArgs e){

        }

        protected void BtnEditar_Click(object sender, EventArgs e){
            try{
                if (TxNombre.Text == string.Empty || TxNombre.Text == "")
                    throw new Exception("Por favor ingrese el nombre.");
                if (HttpContext.Current.Session["DOCUMENTOS_GRUPOEMPLEADO"] == null)
                    throw new Exception("Por favor ingrese los empleados.");

                String vQuery = "[RSP_Documentacion] 21, '" + TxNombre.Text + "',NULL," + Session["USUARIO"].ToString();
                int vId = vConexion.obtenerId(vQuery);

                if (vId > 0){
                    DataTable vDatosEmpleados = (DataTable)Session["DOCUMENTOS_GRUPOEMPLEADO"];
                    if (vDatosEmpleados.Rows.Count > 0){
                        for (int i = 0; i < vDatosEmpleados.Rows.Count; i++){
                            vQuery = "[RSP_Documentacion] 22," + vId + ",NULL," + vDatosEmpleados.Rows[i]["idEmpleado"].ToString();
                            vConexion.ejecutarSql(vQuery);
                        }
                    }
                    Mensaje("Grupo creado con éxito", WarningType.Success);
                    cargarDatos();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);
                }

            }catch (Exception ex){
                DivMensaje.Visible = true;
                LbMensaje.Text = ex.Message;
            }
        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                String vId = e.CommandArgument.ToString();
                if (e.CommandName == "verGrupo"){
                    Literal1.Text = "Empleados del grupo";

                    String vQuery = "[RSP_Documentacion] 23," + vId;
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                    if (vDatos.Rows.Count > 0){
                        GvViewEmpleados.DataSource = vDatos;
                        GvViewEmpleados.DataBind();
                        Session["DOCUMENTOS_GRUPO_VER"] = vDatos;
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalVer();", true);
                }

                if (e.CommandName == "editarGrupo"){

                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){

        }

        protected void GvEmpleados_RowCommand(object sender, GridViewCommandEventArgs e){

        }

        protected void GvEmpleados_PageIndexChanging(object sender, GridViewPageEventArgs e){

        }

        protected void BtnAgregarEmpleado_Click(object sender, EventArgs e){
            try{
                DataTable vDatos = new DataTable();
                if (!Convert.ToBoolean(HttpContext.Current.Session["DOCUMENTOS_GRUPO_ACCION"])){
                    vDatos.Columns.Add("idEmpleado");
                    vDatos.Columns.Add("nombre");
                }else {
                    vDatos = (DataTable)Session["DOCUMENTOS_GRUPOEMPLEADO"];
                }
                
                vDatos.Rows.Add(DDLEmpleado.SelectedValue, DDLEmpleado.SelectedItem);

                Session["DOCUMENTOS_GRUPO_ACCION"] = true;

                GvEmpleados.DataSource = vDatos;
                GvEmpleados.DataBind();
                Session["DOCUMENTOS_GRUPOEMPLEADO"] = vDatos;
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GvViewEmpleados_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GvViewEmpleados.PageIndex = e.NewPageIndex;
                GvViewEmpleados.DataSource = (DataTable)Session["DOCUMENTOS_GRUPO_VER"];
                GvViewEmpleados.DataBind();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
    }
}