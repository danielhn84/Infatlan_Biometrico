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
    public partial class security : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    generales vGenerales = new generales();
                    DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                    if (!vGenerales.PermisosPersonalSeguridad(vDatos))
                        Response.Redirect("/login.aspx");

                    TxSerie.Focus();
                    cargarDatos();
                }
            }
        }

        private void cargarDatos(){
            try{
                String vQuery = "RSP_Seguridad 3";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                if (vDatos.Rows.Count > 0){
                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                    Session["SEG_ENTRADAS"] = vDatos;
                }

                vQuery = "RSP_Seguridad 5";
                vDatos = vConexion.obtenerDataTable(vQuery);

                DDLArticulos.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLArticulos.Items.Add(new ListItem { Value = item["idArticulo"].ToString(), Text = item["descripcion"].ToString() });
                }


            }
            catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }

        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){

        }

        protected void BtnGuardar_Click(object sender, EventArgs e){
            try{
                validaciones();
                String vQuery = "[RSP_Seguridad] 1,'"+ TxNombre.Text + "'" +
                    "," + DDLArticulos.SelectedValue +
                    ",'" + TxSerie.Text + "'" +
                    ",'" + TxInventario.Text + "'" +
                    ",'" + TxDestinatario.Text + "'" +
                    ",'" + Session["USUARIO"].ToString() + "'";

                int vInfo = vConexion.ejecutarSql(vQuery);
                if (vInfo == 1)
                    Mensaje("Registro ingresado con éxito", WarningType.Success);
                else
                    Mensaje("Hubo un error al inresar la entrada.", WarningType.Danger);
                limpiarEntradas();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        private void validaciones() {
            if (TxNombre.Text == "" || TxNombre.Text == string.Empty)
                throw new Exception("Favor ingrese el nombre.");
            if (DDLArticulos.SelectedValue.Equals("0"))
                throw new Exception("Favor seleccione el tipo de articulo.");
            if (TxSerie.Text == "" || TxSerie.Text == string.Empty)
                throw new Exception("Favor ingrese la serie del articulo.");
            if (TxInventario.Text == "" || TxInventario.Text == string.Empty)
                throw new Exception("Favor ingrese el número de inventario.");
            if (TxDestinatario.Text == "" || TxDestinatario.Text == string.Empty)
                throw new Exception("Favor ingrese el destinatario.");
        }

        protected void BtnCancelar_Click(object sender, EventArgs e){
            limpiarEntradas();
        }

        void limpiarEntradas() {
            TxBusqueda.Text = string.Empty;
            TxNombre.Text = string.Empty;
            TxSerie.Text = string.Empty;
            TxDestinatario.Text = string.Empty;
            TxInventario.Text = string.Empty;
            DDLArticulos.SelectedIndex = -1;
        }
    }
}