using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BiometricoWeb.clases;
using System.Data;

namespace BiometricoWeb.pages.configuracion
{
    public partial class feriados : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    generales vGenerales = new generales();
                    DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                    if (!vGenerales.PermisosRecursosHumanos(vDatos))
                        Response.Redirect("/default.aspx");

                    cargarData();
                }
            }
        }

        private void cargarData() {
            try{
                DataTable vDatos = vConexion.obtenerDataTable("RSP_Feriados 1");
                if (vDatos.Rows.Count > 0){
                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                    Session["FERIADOS"] = vDatos;
                }

                DDLDia.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción..." });
                for (int i = 1; i < 31; i++){
                    DDLDia.Items.Add(new ListItem { Value = i.ToString(), Text = i.ToString() });
                }
                
                vDatos = vConexion.obtenerDataTable("RSP_Feriados 3");
                if (vDatos.Rows.Count > 0) { 
                    DDLDepto.Items.Add(new ListItem { Value = "0", Text = "Todos" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLDepto.Items.Add(new ListItem { Value = item["idDepartamento"].ToString(), Text = item["nombre"].ToString() });
                    }
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void BtnBorrar_Click(object sender, EventArgs e){

        }

        protected void BtnAceptar_Click(object sender, EventArgs e){

        }

        protected void DDLDuracion_SelectedIndexChanged(object sender, EventArgs e){
            try{
                DivParcial.Visible = DDLDuracion.SelectedValue == "1" ? false : true;
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnNuevo_Click(object sender, EventArgs e){
            try{
                limpiarModal();
                LbTitulo.Text = "Crear Fecha";
                LbDiaId.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "$('#modalDia').modal('show');", true);
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        private void limpiarModal() {
            DivDepto.Visible = false;
            DivMunicipio.Visible = false;
            DivParcial.Visible = false;
            TxNombre.Text = string.Empty;
            DDLTipo.SelectedValue = "0";
            DDLMes.SelectedValue = "0";
            DDLDia.SelectedValue = "0";
            DDLDepto.SelectedValue = "0";
            DDLmunicipio.SelectedIndex = -1 ;
            DDLDuracion.SelectedValue = "1";
        }

        protected void DDLDepto_SelectedIndexChanged(object sender, EventArgs e){
            try{
                DivMunicipio.Visible = DDLDepto.SelectedValue != "0" ? true : false;
                if (DDLDepto.SelectedValue != "0"){
                    DataTable vDatos = vConexion.obtenerDataTable("RSP_Feriados 2," + DDLDepto.SelectedValue);
                    DDLmunicipio.Items.Clear();
                    DDLmunicipio.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    if (vDatos.Rows.Count > 0){
                        foreach (DataRow item in vDatos.Rows){
                            DDLmunicipio.Items.Add(new ListItem { Value = item["idMunicipio"].ToString(), Text = item["nombre"].ToString() });
                        }
                    }
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void DDLTipo_SelectedIndexChanged(object sender, EventArgs e){
            try{
                DivDepto.Visible = DDLTipo.SelectedValue == "2" ? true : false;
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
    }
}