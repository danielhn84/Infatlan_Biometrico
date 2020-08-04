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
                        if (Session["DOCUMENTOS_TIPO_ID"] == null)
                            Response.Redirect("crearDocumentos.aspx");
                        
                        
                        string vIdTipo = Session["DOCUMENTOS_TIPO_ID"].ToString();
                        if (vIdTipo == "1")
                            TxTitulo.Text = "Boletines";
                        else if(vIdTipo == "2")
                            TxTitulo.Text = "Formatos";
                        else if(vIdTipo == "3")
                            TxTitulo.Text = "Manuales";
                        else if(vIdTipo == "4")
                            TxTitulo.Text = "Politicas";
                        else if(vIdTipo == "5")
                            TxTitulo.Text = "Procesos";

                        DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                        cargarDatos(vIdTipo);
                    }
                }
			}catch (Exception ex){
				throw;
			}
        }
        
        private void cargarDatos(String vId) {
            try{
                String vQuery = "[RSP_Documentacion] 3," + vId + ",NULL," + Session["USUARIO"].ToString();
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                    Session["DOCUMENTOS"] = vDatos;
                }
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
                Session["DOCUMENTO_ARCHIVO_ID"] = vId;
                if (e.CommandName == "verDocumento"){
                    Response.Redirect("archivo.aspx?ti=" + TxTitulo.Text);
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){

        }
    }
}