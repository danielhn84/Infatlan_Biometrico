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
    public partial class archivo : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            try{
                if (!Page.IsPostBack){
                    if (Convert.ToBoolean(Session["AUTH"])){
                        if (Session["DOCUMENTO_ARCHIVO_ID"] != null){
                            string vId = Session["DOCUMENTO_ARCHIVO_ID"].ToString();
                            LbTitulo.Text = Request.QueryString["ti"];

                            DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                            cargarDatos(vId);
                        }else
                            Response.Redirect("tipoDocumentos.aspx");
                    }
                }
			}catch (Exception ex){
				throw;
			}
        }

        private void cargarDatos(String vId) {
            try{
                String vQuery = "[RSP_Documentacion] 5," + vId;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DivLectura.Visible = Convert.ToBoolean(vDatos.Rows[0]["flagLectura"].ToString()) == true ? true : false;
                    //String vDireccion = vDatos.Rows[0]["direccionArchivo"].ToString().Replace("C:/Users/wpadilla/source/repos/danielhn84/Infatlan_Biometrico/BiometricoWeb", "");
                    String vDireccion = vDatos.Rows[0]["direccionArchivo"].ToString().Replace("E:/htdocs/BiometricoDev", "");
                    IFramePDF.Attributes.Add("src", vDireccion);
                    
                    //for (int i = 0; i < vDireccion.Length; i++){
                    //    if (vDireccion.){
                    //    }
                    //}
                    //string[] vSplit = vDatos.Rows[0]["direccionArchivo"].ToString().Split('/');
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void BtnLeido_Click(object sender, EventArgs e){
            try{
                String vQuery = "[RSP_Documentacion] 10" +
                    "," + Session["USUARIO"].ToString() +
                    ",null," + Session["DOCUMENTOS_ARCHIVO_ID"].ToString();
                DataTable vData = vConexion.obtenerDataTable(vQuery);
                if (vData.Rows.Count < 1){
                    if (!CBxLectura.Checked){
                        Mensaje("Falta confirmar en la casilla de lectura.", WarningType.Warning);
                    }else{
                        vQuery = "[RSP_Documentacion] 9" +
                            "," + Session["USUARIO"].ToString() +
                            ",null," + Session["DOCUMENTOS_ARCHIVO_ID"].ToString();
                        int vInfo = vConexion.ejecutarSql(vQuery);
                        if (vInfo == 1)
                            Mensaje("Respuesta enviada con éxito!", WarningType.Success);
                    }
                }else
                    Mensaje("Ya se ha registrado su confirmación de lectura.", WarningType.Warning);
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnVolver_Click(object sender, EventArgs e){
            try{
                Session["DOCUMENTO_ARCHIVO_ID"] = null;
                Response.Redirect("tipoDocumentos.aspx");
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
    }
}