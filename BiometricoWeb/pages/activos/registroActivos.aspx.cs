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
            select2();
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

        private void cargarDatos(){
            try{
                String vQuery = "[RSP_ActivosGenerales] 2";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLCategorias.Items.Clear();
                    DDLCategorias.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLCategorias.Items.Add(new ListItem { Value = item["idCategoria"].ToString(), Text = item["nombre"].ToString() });
                    }
                }

                vQuery = "[RSP_ActivosPI] 1";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLResponsable.Items.Clear();
                    DDLResponsableSW.Items.Clear();
                    DDLModResponsable.Items.Clear();
                    DDLResponsable.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    DDLResponsableSW.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    DDLModResponsable.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLResponsable.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
                        DDLResponsableSW.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
                        DDLModResponsable.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
                    }
                }

                vQuery = "[RPS_ActivosTI] 2";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    GvActivos.DataSource = vDatos;
                    GvActivos.DataBind();
                    Session["ACTIVOS_TI"] = vDatos;
                }


            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void TxBusqueda_TextChanged(object sender, EventArgs e){

        }

        protected void DDLCategorias_SelectedIndexChanged(object sender, EventArgs e){
            try{
                limpiarData();

                DivGenerales.Visible = DDLCategorias.SelectedValue != "2" && DDLCategorias.SelectedValue != "6" && DDLCategorias.SelectedValue != "0" ? true : false;
                DivSoftware.Visible = DDLCategorias.SelectedValue == "2" ? true : false;
                DivRegistrar.Visible = DDLCategorias.SelectedValue != "0" ? true : false;

                String vQuery = "[RSP_ActivosGenerales] 3," + DDLCategorias.SelectedValue;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                DDLTipo.Items.Clear();
                DDLTipoSW.Items.Clear();
                if (vDatos.Rows.Count > 0){
                    DDLTipo.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    DDLTipoSW.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLTipo.Items.Add(new ListItem { Value = item["idTipoEquipo"].ToString(), Text = item["nombre"].ToString() });
                        DDLTipoSW.Items.Add(new ListItem { Value = item["idTipoEquipo"].ToString(), Text = item["nombre"].ToString() });
                    }
                }
                DDLTipo.DataBind();
                DDLTipoSW.DataBind();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void TxGuardar_Click(object sender, EventArgs e){
            try{
                validaciones();
                LtMensaje.Text = "Creación de Equipo de " + DDLCategorias.SelectedItem.Text;
                Lb1.Text = DDLTipo.SelectedItem.Text;
                Lb2.Text = TxSerie.Text;
                Lb3.Text = TxInventario.Text;
                Lb4.Text = DDLResponsable.SelectedItem.Text;
                
                Lb5.Text = DDLTipoSW.SelectedItem.Text;
                Lb6.Text = TxNombreSW.Text;
                Lb7.Text = TxLicenciaSW.Text;
                Lb8.Text = TxUsuariosSW.Text;
                Lb9.Text = TxVersionSW.Text;
                Lb10.Text = TxLenguajeSW.Text;
                Lb11.Text = TxProveedorSW.Text;

                DivInfoSoftware.Visible = DDLCategorias.SelectedValue == "2" ? true : false;
                DivInfoEquipo.Visible = DDLCategorias.SelectedValue != "2" ? true : false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        private void validaciones() {
            if (DDLCategorias.SelectedValue != "2" && DDLCategorias.SelectedValue != "6" && DDLCategorias.SelectedValue != "0") {
                if (DDLTipo.SelectedValue == "0" || DDLTipo.SelectedIndex == -1)
                    throw new Exception("Por favor seleccione el tipo de equipo.");
                if (TxSerie.Text == "" || TxSerie.Text == string.Empty)
                    throw new Exception("Por favor ingrese el número de serie");
                if (DDLResponsable.SelectedValue == "0" || DDLResponsable.SelectedIndex == -1)
                    throw new Exception("Por favor seleccione el responsable.");
            }
            
            if (DDLCategorias.SelectedValue == "2") {
                if (DDLTipoSW.SelectedValue == "0" || DDLTipoSW.SelectedIndex == -1)
                    throw new Exception("Por favor seleccione el tipo de Software.");
                if (TxNombreSW.Text == "" || TxNombreSW.Text == string.Empty)
                    throw new Exception("Por favor ingrese el nombre del Software.");
                if (TxProveedorSW.Text == "" || TxProveedorSW.Text == string.Empty)
                    throw new Exception("Por favor ingrese el proveedor.");
                if (TxLicenciaSW.Text == "" || TxLicenciaSW.Text == string.Empty)
                    throw new Exception("Por favor ingrese la licencia del Software.");
                if (TxUsuariosSW.Text == "" || TxUsuariosSW.Text == string.Empty)
                    throw new Exception("Por favor agregue la cantidad de usuarios.");
                if (TxVersionSW.Text == "" || TxVersionSW.Text == string.Empty)
                    throw new Exception("Por favor agregue la versión del Software.");
            }

        }

        private void limpiarData() {
            DDLTipo.SelectedIndex = -1;
            TxSerie.Text = string.Empty;
            TxInventario.Text = string.Empty;
            DDLResponsable.SelectedIndex = -1;

            DDLTipoSW.SelectedIndex = -1;
            TxNombreSW.Text = string.Empty;
            TxLicenciaSW.Text = string.Empty;
            TxUsuariosSW.Text = string.Empty;
            TxVersionSW.Text = string.Empty;
            TxLenguajeSW.Text = string.Empty;
            TxProveedorSW.Text = string.Empty;
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnGuardarInfo_Click(object sender, EventArgs e){
            try{
                xml vDatosXML = new xml();

                Object[] vDatosMaestro = new object[22];
                vDatosMaestro[0] = DDLCategorias.SelectedValue;
                vDatosMaestro[1] = DDLCategorias.SelectedValue != "2" ? DDLTipo.SelectedValue : DDLTipoSW.SelectedValue;
                vDatosMaestro[2] = DDLCategorias.SelectedValue != "2" ? DDLResponsable.SelectedValue : DDLResponsableSW.SelectedValue;
                vDatosMaestro[3] = DDLCategorias.SelectedValue != "2" ? TxSerie.Text : "";
                vDatosMaestro[4] = DDLCategorias.SelectedValue != "2" ? TxInventario.Text : "";
                vDatosMaestro[5] = DDLCategorias.SelectedValue != "2" ? "" : TxNombreSW.Text;
                vDatosMaestro[6] = DDLCategorias.SelectedValue != "2" ? "" : TxProveedorSW.Text;
                vDatosMaestro[7] = DDLCategorias.SelectedValue != "2" ? "" : TxLicenciaSW.Text;
                vDatosMaestro[8] = DDLCategorias.SelectedValue != "2" ? "" : TxUsuariosSW.Text;
                vDatosMaestro[9] = DDLCategorias.SelectedValue != "2" ? "" : TxVersionSW.Text;
                vDatosMaestro[10] = DDLCategorias.SelectedValue != "2" ? "" : TxLenguajeSW.Text;
                vDatosMaestro[11] = 0;
                vDatosMaestro[12] = 1;
                vDatosMaestro[13] = Session["USUARIO"].ToString();
                for (int i = 14; i < 22; i++){
                    vDatosMaestro[i] = "";
                }

                String vXML = vDatosXML.ObtenerXMLActivos(vDatosMaestro);
                vXML = vXML.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");

                String vQuery = "[RPS_ActivosTI] 1,0,'" + vXML + "'";
                int vInfo = vConexion.obtenerId(vQuery);
                if (vInfo > 0){
                    Mensaje("Registro ingresado con éxito", WarningType.Success);
                    limpiarData();
                    DivGenerales.Visible = false;
                    DivSoftware.Visible = false;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);
                cargarDatos();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GvActivos_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                limpiarModal();
                String vId = e.CommandArgument.ToString();
                if (e.CommandName == "EditarActivo"){
                    TxIdModal.Text = vId;
                    String vQuery = "[RPS_ActivosTI] 6," + vId;
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                    if (vDatos.Rows.Count > 0){
                        TxModInventario.Text = vDatos.Rows[0]["inventario"].ToString();
                        TxModSerie.Text = vDatos.Rows[0]["serie"].ToString();
                        DDLModResponsable.SelectedValue = vDatos.Rows[0]["responsable"].ToString();
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "editarModal();", true);
                }
            }
            catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        void limpiarModal() {
            TxModInventario.Text = string.Empty;
            TxModSerie.Text = string.Empty;
            DDLModResponsable.SelectedValue = "0";
        }

        protected void GvActivos_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GvActivos.PageIndex = e.NewPageIndex;
                GvActivos.DataSource = (DataTable)Session["ACTIVOS_TI"];
                GvActivos.DataBind();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            try{
                String vQuery = "[RPS_ActivosTI] 7," + TxIdModal.Text + 
                    ",''" +
                    ",'" + TxModInventario.Text + "'" +
                    ",'" + TxModSerie.Text + "'" +
                    "," + DDLModResponsable.SelectedValue ;
                int vInfo = vConexion.ejecutarSql(vQuery);
                if (vInfo == 1){
                    Mensaje("Activo actualizado con éxito", WarningType.Success);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeEditar();", true);
                    cargarDatos();
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
    }
}