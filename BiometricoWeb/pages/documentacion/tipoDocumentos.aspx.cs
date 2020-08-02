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
    public partial class tipoDocumentos : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            try{
                if (!Page.IsPostBack){
                    if (Convert.ToBoolean(Session["AUTH"])){
                        string vId = Request.QueryString["id"];
                        if (vId == "1")
                            TxTitulo.Text = "Boletines";
                        else if(vId == "2")
                            TxTitulo.Text = "Formatos";
                        else if(vId == "3")
                            TxTitulo.Text = "Manuales";
                        else if(vId == "4")
                            TxTitulo.Text = "Politicas";
                        else if(vId == "5")
                            TxTitulo.Text = "Procesos";

                        DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                        cargarDatos(vId);
                    }
                }
			}catch (Exception ex){
				throw new Exception(ex.Message);
			}
        }
        
        private void cargarDatos(String vId) {
            try{
                String vQuery = "[RSP_Documentacion] 3," + vId;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                GVBusqueda.DataSource = vDatos;
                GVBusqueda.DataBind();
                Session["DOCUMENTOS"] = vDatos;
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void TxBuscar_TextChanged(object sender, EventArgs e){

        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                String vId = e.CommandArgument.ToString();
                Session["DOCUMENTO_ID"] = vId;
                if (e.CommandName == "verDocumento"){
                    Response.Redirect("archivo.aspx?id=" + vId + "&ti=" + TxTitulo.Text);
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){

        }
    }
}