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
    public partial class docReportes : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                    cargarDatos();
                }
            }
        }

        private void cargarDatos() {
            try{
                String vQuery = "[RSP_Documentacion] 1";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                DDLTipoPDoc.Items.Clear();
                DDLTipoPDoc.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLTipoPDoc.Items.Add(new ListItem { Value = item["idTipoDoc"].ToString(), Text = item["nombre"].ToString() });
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void DDLTipoPDoc_SelectedIndexChanged(object sender, EventArgs e){
            try{
                String vQuery = "[RSP_Documentacion] 11," + DDLTipoPDoc.SelectedValue;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                DDLDocumento.Items.Clear();
                DDLDocumento.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLDocumento.Items.Add(new ListItem { Value = item["idDocumento"].ToString(), Text = item["nombre"].ToString() });
                }
            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void BtnCrearReporte_Click(object sender, EventArgs e){
            try{
                validarDatos();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        private void validarDatos() {
            if (DDLTipoPDoc.SelectedValue == "0")
                throw new Exception("Favor seleccione un tipo de documento.");            
            if (DDLDocumento.SelectedValue == "0")
                throw new Exception("Favor seleccione un documento.");
        }

        protected void BtnConfirmar_Click(object sender, EventArgs e)
        {

        }
    }
}