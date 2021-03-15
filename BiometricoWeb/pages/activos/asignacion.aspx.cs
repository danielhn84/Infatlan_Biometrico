using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;

namespace BiometricoWeb.pages.activos
{
    public partial class asignacion : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            select2();
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

        private void cargar(){
            try{
                String vQuery = "[RSP_ActivosPI] 1";
                DataTable vData = vConexion.obtenerDataTable(vQuery);
                DDLEmpleado.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vData.Rows) {
                    DDLEmpleado.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
                }

                vQuery = "[RSP_ActivosPI] 4";
                vData = vConexion.obtenerDataTable(vQuery);
                DDLTipoEquipo.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vData.Rows) {
                    DDLTipoEquipo.Items.Add(new ListItem { Value = item["idTipoEquipo"].ToString(), Text = item["nombre"].ToString() });
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);

            }catch (Exception ex) {
                Mensaje(ex.Message, WarningType.Danger);
            }
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

        public void Mensaje(string vMensaje, WarningType type) {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void DDLTipoEquipo_SelectedIndexChanged(object sender, EventArgs e){
            try{
                String vQuery = "[RSP_ActivosPI] 3," + DDLTipoEquipo.SelectedValue;
                DataTable vData = vConexion.obtenerDataTable(vQuery);
                DDLEquipo.Items.Clear();
                DDLEquipo.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vData.Rows) {
                    DDLEquipo.Items.Add(new ListItem { Value = item["idActivo"].ToString(), Text = item["marca"].ToString() + " - " + item["serie"].ToString() });
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnAsignar_Click(object sender, EventArgs e){
            try{
                validar();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnConfirmar_Click(object sender, EventArgs e){
            try{
                String vQuery = "[RSP_ActivosPI] 2" +
                    "," + DDLEquipo.SelectedValue +
                    "," + DDLEmpleado.SelectedValue + 
                    "," + DDLAutorizado.SelectedValue +
                    "," + Session["USUARIO"].ToString();
                int vInfo = vConexion.ejecutarSql(vQuery);

                if (vInfo == 2){
                    Mensaje("Asignación realizada con éxito.", WarningType.Success);
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);
                cargar();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        void validar() {
            if (DDLTipoEquipo.SelectedValue == "0")
                throw new Exception("Por favor seleccione el Tipo de equipo.");
            if (DDLEquipo.SelectedValue == "0")
                throw new Exception("Por favor seleccione el Equipo.");
            if (DDLEmpleado.SelectedValue == "0")
                throw new Exception("Por favor seleccione el Empleado.");

        }
    }
}