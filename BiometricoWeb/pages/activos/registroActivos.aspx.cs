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
    public partial class registroActivos : System.Web.UI.Page
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
                String vQuery = "[RSP_ActivosGenerales] 1";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLProceso.Items.Clear();
                    DDLProceso.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLProceso.Items.Add(new ListItem { Value = item["idProceso"].ToString(), Text = item["nombre"].ToString() });
                    }
                }

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        
        protected void DDLProceso_SelectedIndexChanged(object sender, EventArgs e){

        }

        protected void TxBusqueda_TextChanged(object sender, EventArgs e){

        }

        protected void BtnSavePIEntrada_Click(object sender, EventArgs e){
            try{
                String vQuery = "";
                if (Convert.ToBoolean(Session["ACTIVOS_PI_PERSONAL"])){
                    validarPIEquipoPersonal();
                    vQuery = "[RSP_ActivosPI] 11" +
                        "," + DDLCategoria.SelectedValue +
                        "," + DDLTipo.SelectedValue +
                        ",'" + TxMarca.Text + "'" +
                        ",'" + TxSerie.Text + "'" +
                        ",'" + TxModelo.Text + "'" +
                        "," + Session["USUARIO"].ToString() +
                        "," + DDLEmpleado.SelectedValue  + 
                        ",1";
                }else{ 
                    vQuery = "[RSP_ActivosPI] 6" +
                        "," + LbIdEquipoEnt.Text +
                        ",1," + Session["USUARIO"].ToString() + "";
                }

                int vInfo = vConexion.ejecutarSql(vQuery);
                if (vInfo > 0){
                    cargarDatos();
                    Mensaje("Entrada guardada con éxito", WarningType.Success);
                }else
                    Mensaje("Hubo un error al guardar la entrada.", WarningType.Danger);

                limpiarFormularioPI();
                LbMensaje.Text = "";
                DivResultado.Visible = false;
                DivRegistrarPIEntrada.Visible = false;
                DivEquipoPersonal.Visible = false;
                TxBusqueda.Focus();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        private void validarPIEquipoPersonal(){
            if (DDLEmpleado.SelectedValue == "0")
                throw new Exception("Por favor seleccione el empleado.");
            if (TxSerie.Text == "" || TxSerie.Text == string.Empty)
                throw new Exception("Por favor ingrese la serie del equipo.");
            if (DDLCategoria.SelectedValue == "0")
                throw new Exception("Por favor seleccione la categoría.");
            if (DDLTipo.SelectedValue == "0")
                throw new Exception("Por favor seleccione el Tipo de Equipo.");
            if (TxMarca.Text == "" || TxMarca.Text == string.Empty)
                throw new Exception("Por favor ingrese la marca del equipo.");
            if (TxModelo.Text == "" || TxModelo.Text == string.Empty)
                throw new Exception("Por favor ingrese el modelo del equipo.");
        }

        private void limpiarFormularioPI(){
            DDLEmpleado.SelectedValue = "0";
            DDLCategoria.SelectedValue = "0";
            DDLTipo.SelectedValue = "0";
            TxModelo.Text = string.Empty;
            TxMarca.Text = string.Empty;
            
            TxBusqueda.Text = string.Empty;
            DivInfoIN.Visible = false;
            LbIdEquipoEnt.Text = string.Empty;
            LbNombre.Text = string.Empty;
            LbMarca.Text = string.Empty;
            LbSerieSalida.Text = string.Empty;
            LbCodInventario.Text = string.Empty;

            UpdatePanel3.Update();
        }

        protected void BtnCancelPIEntrada_Click(object sender, EventArgs e){

        }
    }
}