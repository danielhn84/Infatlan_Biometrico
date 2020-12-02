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
using System.Text;
using Newtonsoft.Json;

namespace BiometricoWeb.pages.documentacion
{
    public partial class crearDocumentos : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
			try{
                select2();
                if (!Page.IsPostBack){
                    if (Convert.ToBoolean(Session["AUTH"])){
                        String vEx = Request.QueryString["ex"];
                        
                        DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                        cargarDatos();
                    }
                }
			}catch (Exception ex){
				throw new Exception(ex.Message);
			}
        }

		private void cargarDatos() {
            try{
                String vQuery = "[RSP_Documentacion] 15," + Session["USUARIO"].ToString() ;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    GvTipos.DataSource = vDatos;
                    GvTipos.DataBind();
                    Session["DOCUMENTOS_TIPO"] = vDatos;
                    
                    vQuery = "[RSP_Perfiles] 2," + Session["USUARIO"].ToString() + ", 5";
                    DataTable vDataInfo = vConexion.obtenerDataTable(vQuery);
                    if (vDataInfo.Rows[0][0].ToString() == "0"){
                        foreach (GridViewRow row in GvTipos.Rows){
                            var vLinkButton = row.FindControl("BtnEditar") as LinkButton;
                            vLinkButton.Visible = false;
                        }
                    }
                }
                
                vQuery = "[RSP_Documentacion] 2";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLCategoria.Items.Clear();
                    DDLCategoria.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLCategoria.Items.Add(new ListItem { Value = item["idCategoria"].ToString(), Text = item["nombre"].ToString() });
                    }
                }
                
                vQuery = "[RSP_Documentacion] 6";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLEmpleados.Items.Clear();
                    DDLEmpleados.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLEmpleados.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString()  });
                    }
                }
                
                vQuery = "[RSP_Documentacion] 14";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLNivelConfidencialidad.Items.Clear();
                    DDLNivelConfidencialidad.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLNivelConfidencialidad.Items.Add(new ListItem { Value = item["idConfidencialidad"].ToString(), Text = item["nombre"].ToString()});
                    }
                }

                vQuery = "[RSP_Documentacion] 19";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLReferencia.Items.Clear();
                    //DDLReferencia.Items.Add(new ListItem { Value = "0", Text = "" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLReferencia.Items.Add(new ListItem { Value = item["idDocumento"].ToString(), Text = item["codigo"].ToString() + " - " + item["nombre"].ToString()});
                    }
                }

                vQuery = "[RSP_Documentacion] 6";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLPropietario.Items.Clear();
                    DDLPropietario.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLPropietario.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
                    }
                }

                vQuery = "[RSP_Documentacion] 20";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLGrupos.Items.Clear();
                    DDLGrupos.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLGrupos.Items.Add(new ListItem { Value = item["idGrupo"].ToString(), Text = item["nombre"].ToString() });
                    }
                }

                vQuery = "[RSP_Documentacion] 16";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLDepto.Items.Clear();
                    DDLDepto.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
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

        public void MensajeLoad(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", " document.addEventListener(\"DOMContentLoaded\", function (event) { infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "'); });", true);
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

        protected void GvTipos_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                limpiarModal();
                String vId = e.CommandArgument.ToString();
                Session["DOCUMENTOS_TIPO_ID"] = vId;
                if (e.CommandName == "NuevoDoc"){
                    DivEmpleados.Visible = DDLCategoria.SelectedValue == "2" ? true : false;
                    DivGrupos.Visible = DDLCategoria.SelectedValue == "3" ? true : false;
                    DivAreas.Visible = DDLCategoria.SelectedValue == "4" ? true : false;

                    Session["DOCUMENTOS_CORREOS"] = null;
                    GvCorreos.DataSource = null;
                    GvCorreos.DataBind();

                    if (vId == "1")
                        LitTitulo.Text = "Boletines";
                    else if (vId == "2")
                        LitTitulo.Text = "Formatos";
                    else if (vId == "3")
                        LitTitulo.Text = "Manuales";
                    else if (vId == "4")
                        LitTitulo.Text = "Politicas";
                    else if (vId == "5")
                        LitTitulo.Text = "Procesos";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }else if (e.CommandName == "EntrarDoc"){
                    Response.Redirect("tipoDocumentos.aspx");
                }
            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void GvTipos_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GvTipos.PageIndex = e.NewPageIndex;
                GvTipos.DataSource = (DataTable)Session["DOCUMENTOS_TIPO"];
                GvTipos.DataBind();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnCargar_Click(object sender, EventArgs e){
            try{
                validarDatos();
                String vExtension = "", vBody = "";
                vExtension = Path.GetExtension(FUArchivo.FileName);

                String archivoLog = string.Format("{0}_{1}", Convert.ToString(Session["usuario"]), DateTime.Now.ToString("yyyyMMddHHmmss"));
                //String vDireccionCarga = ConfigurationManager.AppSettings["RUTA_SERVER_DOCS"].ToString() + LitTitulo.Text.ToLower();
                String vDireccionCarga = ConfigurationManager.AppSettings["RUTA_SERVER_DOCS_LOCAL"].ToString() + LitTitulo.Text.ToLower();

                String vNombreArchivo = FUArchivo.FileName;
                vDireccionCarga += "/" + archivoLog + "_" + vNombreArchivo;
                FUArchivo.SaveAs(vDireccionCarga);
                Boolean vCargado = File.Exists(vDireccionCarga) ? true : false;
                if (vCargado){
                    xml vDatos = new xml();
                    String vIdArchivo = Session["DOCUMENTOS_TIPO_ID"].ToString();

                    Object[] vDatosMaestro = new object[16];
                    vDatosMaestro[0] = vIdArchivo;
                    vDatosMaestro[1] = DDLCategoria.SelectedValue;
                    vDatosMaestro[2] = TxNombre.Text;
                    vDatosMaestro[3] = FUArchivo.FileName;
                    vDatosMaestro[4] = vExtension;
                    vDatosMaestro[5] = TxCodigo.Text;
                    vDatosMaestro[6] = vDireccionCarga;
                    vDatosMaestro[7] = DDLConfirmacion.SelectedValue;
                    vDatosMaestro[8] = DDLCorreo.SelectedValue;
                    vDatosMaestro[9] = TxFecha.Text != "" ? Convert.ToDateTime(TxFecha.Text).ToString("yyyy-MM-dd HH:mm:ss") : "1900-01-01 00:00:00";
                    vDatosMaestro[10] = DDLRecordatorios.SelectedValue;
                    vDatosMaestro[11] = DDLEstado.SelectedValue;
                    vDatosMaestro[12] = Session["USUARIO"].ToString();
                    vDatosMaestro[13] = CBxConfidencial.Checked;
                    vDatosMaestro[14] = DDLNivelConfidencialidad.SelectedValue;
                    vDatosMaestro[15] = DDLPropietario.SelectedValue != "0" ? DDLPropietario.SelectedValue : "NULL";
                    String vXML = vDatos.ObtenerXMLDocumentos(vDatosMaestro);
                    vXML = vXML.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
                
                    String vQuery = "[RSP_Documentacion] 4,0" +
                                    ",'" + vXML + "'";
                    int vInfo = vConexion.obtenerId(vQuery);
                    if (vInfo > 0){
                        if (DDLReferencia.SelectedValue != "0"){

                        }




                        DataTable vDTConfidenciales = (DataTable)Session["DOCUMENTOS_CORREOS"];
                        if (vIdArchivo == "1")
                            vBody = "Se ha creado un nuevo Boletín, por favor revisar el documento.";
                        else if (vIdArchivo == "2")
                            vBody = "Se ha creado un nuevo Formato, por favor revisar el documento.";
                        else if (vIdArchivo == "3")
                            vBody = "Se ha creado un nuevo Manual, por favor revisar el documento.";
                        else if (vIdArchivo == "4")
                            vBody = "Se ha creado una nueva Política, por favor revisar el documento.";
                        else if (vIdArchivo == "5")
                            vBody = "Se ha creado un nuevo Proceso, por favor revisar el documento.";

                        if (DDLCategoria.SelectedValue == "1"){
                            if (DDLCorreo.SelectedValue == "1") {
                                String vConsulta = "[RSP_Documentacion] 6";
                                DataTable vData = vConexion.obtenerDataTable(vConsulta);
                                for (int i = 0; i < vData.Rows.Count; i++){
                                    String vTokenString = "";

                                    CryptoToken.CryptoToken vToken = new CryptoToken.CryptoToken();
                                    tokenClass vClassToken = new tokenClass(){
                                        usuario = Convert.ToInt32(vData.Rows[i]["idEmpleado"].ToString())
                                    };
                                    vTokenString = vToken.Encrypt(JsonConvert.SerializeObject(vClassToken), ConfigurationManager.AppSettings["TOKEN_DOC"].ToString());
                                    
                                    vQuery = "[RSP_Documentacion] 8" +
                                    "," + vData.Rows[i]["idEmpleado"].ToString() +
                                    ",null," + vInfo +
                                    ",'" + vBody + "'" +
                                    ",0,'" + vTokenString + "'";
                                    vConexion.ejecutarSql(vQuery);
                                }
                            }
                        }else if (DDLCategoria.SelectedValue == "2"){
                            for (int i = 0; i < vDTConfidenciales.Rows.Count; i++){
                                String vTokenString = "";
                                if (DDLCorreo.SelectedValue == "1") { 
                                    CryptoToken.CryptoToken vToken = new CryptoToken.CryptoToken();
                                    tokenClass vClassToken = new tokenClass(){
                                        usuario = Convert.ToInt32(vDTConfidenciales.Rows[i]["idEmpleado"].ToString()),
                                        parametro1 = vInfo.ToString()
                                    };
                                    vTokenString = vToken.Encrypt(JsonConvert.SerializeObject(vClassToken), ConfigurationManager.AppSettings["TOKEN_DOC"].ToString());
                                }

                                vQuery = "[RSP_Documentacion] 8" +
                                    "," + vDTConfidenciales.Rows[i]["idEmpleado"].ToString() +
                                    ",null," + vInfo +
                                    ",'" + vBody + "'" +
                                    ",0,'" + vTokenString + "'";
                                vConexion.ejecutarSql(vQuery);
                            }
                        }else if (DDLCategoria.SelectedValue == "3"){
                            vQuery = "[RSP_Documentacion] 23," + DDLGrupos.SelectedValue;
                            DataTable vDataGrupo = vConexion.obtenerDataTable(vQuery);
                            if (vDataGrupo.Rows.Count > 0){
                                for (int i = 0; i < vDataGrupo.Rows.Count; i++){
                                    String vTokenString = "";
                                    if (DDLCorreo.SelectedValue == "1") { 
                                        CryptoToken.CryptoToken vToken = new CryptoToken.CryptoToken();
                                        tokenClass vClassToken = new tokenClass(){
                                            usuario = Convert.ToInt32(vDataGrupo.Rows[i]["idEmpleado"].ToString()),
                                            parametro1 = vInfo.ToString()
                                        };
                                        vTokenString = vToken.Encrypt(JsonConvert.SerializeObject(vClassToken), ConfigurationManager.AppSettings["TOKEN_DOC"].ToString());
                                    }

                                    vQuery = "[RSP_Documentacion] 8" +
                                        "," + vDataGrupo.Rows[i]["idEmpleado"].ToString() +
                                        ",null," + vInfo +
                                        ",'" + vBody + "'" +
                                        ",0,'" + vTokenString + "'";
                                    vConexion.ejecutarSql(vQuery);
                                }
                            }
                        }
                        MensajeLoad("Documento cargado con éxito.", WarningType.Success);
                    }else
                        MensajeLoad("Solicitud no completada, favor comuníquese con sistemas.", WarningType.Danger);
                }else
                    MensajeLoad("Solicitud no completada, favor comuníquese con sistemas.", WarningType.Danger);
                    
                limpiarModal();
                cargarDatos();
                UpdatePanel1.Update();
            }catch (Exception ex){
                MensajeLoad(ex.Message, WarningType.Danger);
            }
        }

        protected void DDLCorreo_SelectedIndexChanged(object sender, EventArgs e){
            try{
                if (DDLCorreo.SelectedValue == "1") 
                    DivCorreos.Visible = true;
                else if (DDLCorreo.SelectedValue == "0") { 
                    DivCorreos.Visible = false;
                    DDLRecordatorios.SelectedValue = "0";
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        private void limpiarModal() {
            TxNombre.Text = string.Empty;
            TxFecha.Text = string.Empty;
            DDLCategoria.SelectedValue = "0";
            DDLConfirmacion.SelectedValue = "0";
            DDLCorreo.SelectedValue = "0";
            DDLRecordatorios.SelectedValue = "0";
            DDLGrupos.SelectedValue = "0";
            DDLReferencia.SelectedIndex = -1;
            DDLPropietario.SelectedValue = "0";
            DivMensaje.Visible = false;
            DivCorreos.Visible = false;
            LbAdvertencia.Text = string.Empty;
            DDLEstado.SelectedValue = "1";
            TxCodigo.Text = string.Empty;
            DDLNivelConfidencialidad.SelectedValue = "0";
        }

        private void validarDatos() {
            if (TxCodigo.Text == string.Empty || TxCodigo.Text == "")
                throw new Exception("Por favor ingrese el código del documento.");
            if (TxNombre.Text == string.Empty || TxNombre.Text == "")
                throw new Exception("Por favor ingrese el nombre del documento.");
            if (DDLCategoria.SelectedValue == "0")
                throw new Exception("Por favor seleccione una categoría del documento.");
            if (DDLCategoria.SelectedValue == "2"){
                DataTable vDatos = (DataTable)Session["DOCUMENTOS_CORREOS"];
                if (vDatos == null || vDatos.Rows.Count < 1)
                    throw new Exception("Por favor ingrese los usuarios que verán el documento.");
            }
            if (DDLCategoria.SelectedValue == "3"){
                if (DDLGrupos.SelectedValue == "0")
                    throw new Exception("Por favor seleccione el grupo de empleados.");
            }
            
            if (DDLCorreo.SelectedValue == "1"){
                if (TxFecha.Text == string.Empty || TxFecha.Text == "")
                    throw new Exception("Por favor ingrese la fecha que se enviará el correo.");
            }

            if (DDLPropietario.Text == "0")
                throw new Exception("Por favor seleccione un propietario.");
            if (!FUArchivo.HasFile)
                throw new Exception("Por favor ingrese un documento.");
        }

        protected void DDLCategoria_SelectedIndexChanged(object sender, EventArgs e){
            try{
                DivEmpleados.Visible = DDLCategoria.SelectedValue == "2" ? true : false;
                DivGrupos.Visible = DDLCategoria.SelectedValue == "3" ? true : false;
                DivAreas.Visible = DDLCategoria.SelectedValue == "4" ? true : false;
                if (DDLCategoria.SelectedValue == "1") {
                    Session["DOCUMENTOS_CORREOS"] = null;
                    DDLGrupos.SelectedValue = "0";
                    DDLDepto.SelectedValue = "0";
                    DDLArea.SelectedValue = "0";
                }else if (DDLCategoria.SelectedValue == "2") {
                    DDLGrupos.SelectedValue = "0";
                    DDLDepto.SelectedValue = "0";
                    DDLArea.SelectedValue = "0";
                }

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void LBAgregarCorreos_Click(object sender, EventArgs e){
            try{
                DivMensajeCorreo.Visible = false;
                LbMensajeCorreo.Text = "";
                DDLEmpleados.SelectedValue = "0";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalCorreos();", true);
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnAgregar_Click(object sender, EventArgs e){
            try{
                DataTable vDatosCorreos = (DataTable)Session["DOCUMENTOS_CORREOS"];
                if (vDatosCorreos == null || vDatosCorreos.Rows.Count < 1)
                    throw new Exception("Favor ingrese al menos un empleado");

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModalCorreos();", true);
            }catch (Exception ex){
                DivMensajeCorreo.Visible = true;
                LbMensajeCorreo.Text = ex.Message;
            }
        }

        protected void BtnAgregarCorreo_Click(object sender, EventArgs e){
            try{
                if (DDLEmpleados.SelectedValue == "0")
                    throw new Exception("Favor seleccione el empleado.");

                DivMensajeCorreo.Visible = false;
                String vQuery = "[RSP_Documentacion] 7," + DDLEmpleados.SelectedValue;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                DataTable vNewDatos = new DataTable();
                DataTable vData = (DataTable)Session["DOCUMENTOS_CORREOS"];
                
                vNewDatos.Columns.Add("idEmpleado");
                vNewDatos.Columns.Add("nombre");
                vNewDatos.Columns.Add("emailEmpresa");
                vNewDatos.Columns.Add("emailPersonal");
                
                if (vData == null)
                    vData = vNewDatos.Clone();
                
                Boolean vFlag = true;
                if (vData.Rows.Count > 0){
                    for (int i = 0; i < vData.Rows.Count; i++){
                        if (vData.Rows[i]["idEmpleado"].ToString() == DDLEmpleados.SelectedValue) { 
                            vFlag = false;
                            break;
                        }
                    }
                    if (vFlag)
                        vData.Rows.Add(DDLEmpleados.SelectedValue, DDLEmpleados.SelectedItem, vDatos.Rows[0]["emailEmpresa"].ToString(), vDatos.Rows[0]["emailPersonal"].ToString());
                }else{
                    vData.Rows.Add(DDLEmpleados.SelectedValue, DDLEmpleados.SelectedItem, vDatos.Rows[0]["emailEmpresa"].ToString(), vDatos.Rows[0]["emailPersonal"].ToString());
                }
                
                if (vData.Rows.Count > 0 && vFlag){
                    Session["DOCUMENTOS_CORREOS"] = vData;
                    GvCorreos.DataSource = vData;
                    GvCorreos.DataBind();
                }
            }catch (Exception ex){
                DivMensajeCorreo.Visible = true;
                LbMensajeCorreo.Text = ex.Message;
            }
        }

        protected void GvCorreos_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GvCorreos.PageIndex = e.NewPageIndex;
                GvCorreos.DataSource = (DataTable)Session["DOCUMENTOS_CORREOS"];
                GvCorreos.DataBind();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GvCorreos_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                DataTable vDatos = (DataTable)Session["DOCUMENTOS_CORREOS"];
                if (e.CommandName == "BorrarCorreo"){
                    String vID = e.CommandArgument.ToString();
                    if (Session["DOCUMENTOS_CORREOS"] != null){
                        DataRow[] result = vDatos.Select("idEmpleado = '" + vID + "'");
                        foreach (DataRow row in result){
                            if (row["idEmpleado"].ToString().Contains(vID))
                                vDatos.Rows.Remove(row);
                        }
                    }
                }
                GvCorreos.DataSource = vDatos;
                GvCorreos.DataBind();
                Session["DOCUMENTOS_CORREOS"] = vDatos;
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void DDLDepto_SelectedIndexChanged(object sender, EventArgs e){
            try{
                String vQuery = "[RSP_Documentacion] 17," + DDLDepto.SelectedValue;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                DDLArea.Items.Clear();
                if (vDatos.Rows.Count > 0){
                    DDLArea.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLArea.Items.Add(new ListItem { Value = item["idSubDepartamento"].ToString(), Text = item["nombre"].ToString() });
                    }
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
    }
}