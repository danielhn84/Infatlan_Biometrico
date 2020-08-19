using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;

namespace BiometricoWeb.pages.documentacion
{
    public partial class archivo : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            String vToken = Request.QueryString["id"];
            if (vToken != null){
                try{
                    String vQuery = "[RSP_Documentacion] 12,'" + vToken + "'";
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                    if (vDatos.Rows.Count > 0){
                        token vTokenClass = new token();
                        CryptoToken.CryptoToken vTokenCrypto = new CryptoToken.CryptoToken();
                        vTokenClass = JsonConvert.DeserializeObject<token>(vTokenCrypto.Decrypt(vToken, ConfigurationManager.AppSettings["TOKEN_DOC"].ToString()));
                        Session["DOCUMENTOS_ARCHIVO_ID"] = Request.QueryString["d"];

                        Session["AUTHCLASS"] = vDatos;
                        Session["USUARIO"] = vDatos.Rows[0]["idEmpleado"].ToString();
                        Session["AUTH"] = true;
                        vQuery = "[RSP_Documentacion] 13,'" + vToken + "'";
                        vConexion.ejecutarSql(vQuery);
                        Response.Redirect("archivo.aspx", false);
                    }else 
                        throw new Exception();

                }catch(Exception ex){
                    Session["AUTH"] = false;
                    Response.Redirect("/login.aspx");
                }
            }

            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    string vId = Session["DOCUMENTOS_ARCHIVO_ID"].ToString();
                    String vQuery = "[RSP_Documentacion] 5" +
                        "," + Session["DOCUMENTOS_ARCHIVO_ID"].ToString();
                    DataTable vData = vConexion.obtenerDataTable(vQuery);
                    LbTitulo.Text = vData.Rows[0]["nombre"].ToString();

                    DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                    cargarDatos(vId);
                    //Response.Redirect("tipoDocumentos.aspx");
                }
            }
        }

        private void cargarDatos(String vId) {
            try{
                String vQuery = "[RSP_Documentacion] 5," + vId;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DivLectura.Visible = Convert.ToBoolean(vDatos.Rows[0]["flagLectura"].ToString()) == true ? true : false;
                    String vDireccion = vDatos.Rows[0]["direccionArchivo"].ToString();
                    vDireccion = vDireccion.StartsWith("E:/") ? vDireccion.Replace("E:/htdocs/Biometrico", "") : vDireccion.Replace("C:/Users/wpadilla/source/repos/danielhn84/Infatlan_Biometrico/BiometricoWeb", "");
                    
                    String vConfidencial = "";
                    if (Convert.ToBoolean(vDatos.Rows[0]["flagConfidencial"].ToString()))
                        vConfidencial = "#toolbar=0";
                    
                    IFramePDF.Attributes.Add("src", vDireccion + vConfidencial);
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
                String vQuery = "[RSP_Documentacion] 5," + Session["DOCUMENTOS_ARCHIVO_ID"].ToString();
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                String vDoc = "";
                if (vDatos.Rows[0]["idTipoDoc"].ToString() == "1")
                    vDoc = "el Boletín";
                else if(vDatos.Rows[0]["idTipoDoc"].ToString() == "2")
                    vDoc = "el Formato";
                else if(vDatos.Rows[0]["idTipoDoc"].ToString() == "3")
                    vDoc = "el Manual";
                else if(vDatos.Rows[0]["idTipoDoc"].ToString() == "4")
                    vDoc = "la Política";
                else if(vDatos.Rows[0]["idTipoDoc"].ToString() == "5")
                    vDoc = "el Proceso";


                LbMensaje.Text = "Declaro que he leído, entendido y doy por enterado(a) " + vDoc + " <b>" + LbTitulo.Text + "</b>,  <br><br>" +
                    "Me comprometo a dar cumplimiento a los lineamientos incluidos en " + vDoc + " y entiendo que el hecho de no hacerlo puede dar lugar a que INFATLAN proceda a aplicarme las acciones disciplinarias o legales correspondientes.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnVolver_Click(object sender, EventArgs e){
            try{
                Session["DOCUMENTOS_ARCHIVO_ID"] = null;
                Response.Redirect("tipoDocumentos.aspx");
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnConfirmar_Click(object sender, EventArgs e){
            try{
                String vQuery = "[RSP_Documentacion] 10" +
                    ",'" + Session["USUARIO"].ToString() + "'" +
                    ",null," + Session["DOCUMENTOS_ARCHIVO_ID"].ToString();
                DataTable vData = vConexion.obtenerDataTable(vQuery);
                if (vData.Rows.Count < 1){
                    vQuery = "[RSP_Documentacion] 9" +
                        ",'" + Session["USUARIO"].ToString() + "'" +
                        ",null," + Session["DOCUMENTOS_ARCHIVO_ID"].ToString();
                    int vInfo = vConexion.ejecutarSql(vQuery);
                    if (vInfo == 1)
                        Mensaje("Respuesta enviada con éxito!", WarningType.Success);
                }else
                    Mensaje("Ya se ha registrado su confirmación de lectura.", WarningType.Warning);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
    }
}