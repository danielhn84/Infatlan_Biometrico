using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

namespace BiometricoWeb.pages.activos
{
    public partial class visitas : System.Web.UI.Page
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
                String vQuery = "[RSP_ActivosPE] 1";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                    Session["ACTIVOS_PE_VISITAS"] = vDatos;
                }

                vQuery = "[RSP_ActivosPE] 3";
                vDatos = vConexion.obtenerDataTable(vQuery);

                DDLArea.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLArea.Items.Add(new ListItem { Value = item["idPuesto"].ToString(), Text = item["nombre"].ToString() });
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

        protected void BtnGuardar_Click(object sender, EventArgs e){
            try{
                validar();
                String vQuery = "[RSP_ActivosPE] 2" +
                    "," + DDLArea.SelectedValue + 
                    ",'" + TxNombre.Text + 
                    "','" +  TxApellido.Text +
                    "','" + TxIdentidad.Text + "'";
                int vInfo = vConexion.ejecutarSql(vQuery);
                if (vInfo == 1)
                    Mensaje("Visita registrada con éxito", WarningType.Success);
                else
                    Mensaje("Hubo un error al ingresar la visita, comuníquese con sistemas.", WarningType.Success);

                limpiarFormulario();
                UpdatePanel1.Update();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e){
            limpiarFormulario();
        }

        private void limpiarFormulario(){
            DDLArea.SelectedValue = "0";
            TxNombre.Text = string.Empty;
            TxApellido.Text = string.Empty;
            TxIdentidad.Text = string.Empty;

            UpdatePanel1.Update();
        }

        private void validar() {
            if (DDLArea.SelectedValue == "0")
                throw new Exception("Por favor seleccione un área de destino.");
            if (TxNombre.Text == string.Empty || TxNombre.Text == "")
                throw new Exception("Por favor ingrese el nombre.");
            if (TxApellido.Text == string.Empty || TxApellido.Text == "")
                throw new Exception("Por favor ingrese los apellidos.");
            if (TxIdentidad.Text == string.Empty || TxIdentidad.Text == "")
                throw new Exception("Por favor ingrese la identidad.");
            if (TxDescripcion.Text == string.Empty || TxDescripcion.Text == "")
                throw new Exception("Por favor ingrese el motivo de la visita.");
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void TxBusqueda_TextChanged(object sender, EventArgs e){
            try{
                DataTable vDatos = (DataTable)Session["ACTIVOS_PE_VISITAS"];
                if (TxBusqueda.Text != "" || TxBusqueda.Text != string.Empty){
                    cargarDatos();

                    String vBusqueda = TxBusqueda.Text;
                    EnumerableRowCollection<DataRow> filtered = vDatos.AsEnumerable()
                        .Where(r => r.Field<String>("nombre").Contains(vBusqueda.ToUpper()));

                    if (filtered.Count() < 1){
                        filtered = vDatos.AsEnumerable()
                        .Where(r => r.Field<String>("apellidos").Contains(vBusqueda.ToUpper()));
                    }


                    DataTable vDatosFiltrados = new DataTable();
                    vDatosFiltrados.Columns.Add("nombre");
                    vDatosFiltrados.Columns.Add("apellidos");
                    vDatosFiltrados.Columns.Add("identidad");
                    vDatosFiltrados.Columns.Add("descripcion");
                    vDatosFiltrados.Columns.Add("area");

                    foreach (DataRow item in filtered){
                        vDatosFiltrados.Rows.Add(
                            item["nombre"].ToString(),
                            item["apellidos"].ToString(),
                            item["identidad"].ToString(),
                            item["descripcion"].ToString(),
                            item["area"].ToString()
                            );
                    }

                    GVBusqueda.DataSource = vDatosFiltrados;
                    GVBusqueda.DataBind();
                    Session["ACTIVOS_PE_VISITAS"] = vDatosFiltrados;
                }else {
                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                }
                
                UpdateDivBusquedas.Update();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
    }
}