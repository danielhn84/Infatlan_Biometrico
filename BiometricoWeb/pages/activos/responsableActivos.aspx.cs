using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BiometricoWeb.clases;
using System.Data;


namespace BiometricoWeb.pages.activos
{
    public partial class responsableActivos : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            select2();
            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"]))
                    cargarDatos();
                else
                    Response.Redirect("/login.aspx");
            }
        }

        private void cargarDatos() {
            try{
                String vQuery = "[RPS_ActivosTI] 3," + Session["USUARIO"].ToString();
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                GvBusqueda.DataSource = null;
                if (vDatos.Rows.Count > 0)
                    GvBusqueda.DataSource = vDatos;
                
                Session["ACTIVOS_RESPONSABLE"] = vDatos;
                GvBusqueda.DataBind();
                
                vQuery = "[RPS_ActivosTI] 8," + Session["USUARIO"].ToString();
                vDatos = vConexion.obtenerDataTable(vQuery);
                GvActivos.DataSource = null;
                if (vDatos.Rows.Count > 0){
                    GvActivos.DataSource = vDatos;
                    GvActivos.DataBind();
                    Session["ACTIVOS_ASIGNADOS"] = vDatos;
                }

                vQuery = "[RPS_ActivosTI] 13," + Session["USUARIO"].ToString();
                vDatos = vConexion.obtenerDataTable(vQuery);
                GvMisActivos.DataSource = null;
                if (vDatos.Rows.Count > 0){
                    GvMisActivos.DataSource = vDatos;
                    GvMisActivos.DataBind();
                    Session["ACTIVOS_MIS_ACTIVOS"] = vDatos;
                }
                
                vQuery = "[RSP_ActivosPI] 1";
                DataTable vData = vConexion.obtenerDataTable(vQuery);
                DDLEmpleado.Items.Clear();
                DDLEmpleado.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vData.Rows) {
                    DDLEmpleado.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
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

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void GvBusqueda_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                limpiarModal();
                String vId = e.CommandArgument.ToString();
                if (e.CommandName == "VerActivo"){
                    LbIdActivo.Text = vId;
                    String vQuery = "[RPS_ActivosTI] 4," + vId;
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                    if (vDatos.Rows.Count > 0){
                        Session["ACTIVOS_CAT"] = vDatos.Rows[0]["idCategoria"].ToString();
                        DivHW.Visible = vDatos.Rows[0]["idCategoria"].ToString() == "1" ? true : false;
                        DivRED.Visible = vDatos.Rows[0]["idCategoria"].ToString() == "3" ? true : false;
                        DivUPS.Visible = vDatos.Rows[0]["idCategoria"].ToString() == "5" ? true : false;
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        void limpiarModal() {
            TxDisco.Text = string.Empty;
            TxMarca.Text = string.Empty;
            TxMemoria.Text = string.Empty;
            TxModelo.Text = string.Empty;
            TxProcesador.Text = string.Empty;
            TxUbicacion.Text = string.Empty;
            
            TxMarcaRED.Text = string.Empty;
            TxModeloRED.Text = string.Empty;
            TxUbicacionRED.Text = string.Empty;

            TxKvaUPS.Text = string.Empty;
            TxMarcaUPS.Text = string.Empty;
            TxModeloUPS.Text = string.Empty;
            TxUbicacionUPS.Text = string.Empty;
            TxWattsUPS.Text = string.Empty;
        }
        
        protected void GvBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GvBusqueda.PageIndex = e.NewPageIndex;
                GvBusqueda.DataSource = (DataTable)Session["ACTIVOS_RESPONSABLE"];
                GvBusqueda.DataBind();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnAceptar_Click(object sender, EventArgs e){
            try{
                xml vDatosXML = new xml();
                Object[] vDatosMaestro = new object[22];
                String vMarca = "", vModelo = "", vUbicacion = "", vCat = "";
                vCat = Session["ACTIVOS_CAT"].ToString();
                if (vCat == "1" || vCat == "4"){ 
                    vMarca = TxMarca.Text;
                    vModelo = TxModelo.Text;
                    vUbicacion = TxUbicacion.Text;
                }else if(vCat == "3"){ 
                    vMarca = TxMarcaRED.Text;
                    vModelo = TxModeloRED.Text;
                    vUbicacion = TxUbicacionRED.Text;
                }else if(vCat == "5"){
                    vMarca = TxMarcaUPS.Text;
                    vModelo = TxModeloUPS.Text;
                    vUbicacion = TxUbicacionUPS.Text;
                }

                for (int i = 0; i < 13; i++){
                    vDatosMaestro[i] = "";
                }
                vDatosMaestro[13] = Session["USUARIO"].ToString();
                vDatosMaestro[14] = vMarca;
                vDatosMaestro[15] = vModelo;
                vDatosMaestro[16] = TxMemoria.Text;
                vDatosMaestro[17] = TxDisco.Text;
                vDatosMaestro[18] = TxProcesador.Text;
                vDatosMaestro[19] = vUbicacion;
                vDatosMaestro[20] = TxKvaUPS.Text;
                vDatosMaestro[21] = TxWattsUPS.Text;
                String vXML = vDatosXML.ObtenerXMLActivos(vDatosMaestro);
                vXML = vXML.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");

                String vQuery = "[RPS_ActivosTI] {0}," + LbIdActivo.Text + ",'" + vXML + "'";
                if (vCat == "1" || vCat == "4")
                    vQuery = string.Format(vQuery,"5");
                if (vCat == "3")
                    vQuery = string.Format(vQuery,"10");
                if (vCat == "5")
                    vQuery = string.Format(vQuery,"11");

                int vInfo = vConexion.ejecutarSql(vQuery);
                if (vInfo == 1){
                    limpiarModal();
                    Mensaje("El activo ha sido actualizado.", WarningType.Success);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);
                    cargarDatos();
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GvActivos_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                limpiarModalActivos();
                String vId = e.CommandArgument.ToString();
                if (e.CommandName == "AsignarActivo"){
                    LbIdActivoAsignar.Text = vId;
                    String vQuery = "[RPS_ActivosTI] 12," + vId;
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                    Session["ACTIVOS_TI_ASIGNADO"] = vDatos.Rows.Count > 0 ? true : false;
                    if (vDatos.Rows.Count > 0){
                        DDLEmpleado.SelectedValue = vDatos.Rows[0]["idEmpleado"].ToString();
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openAsign();", true);
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnAsignar_Click(object sender, EventArgs e){
            try{
                validar();
                String vQuery = "";
                if (Convert.ToBoolean(Session["ACTIVOS_TI_ASIGNADO"]))
                    vQuery = "[RSP_ActivosPI] 17" +
                        "," + LbIdActivoAsignar.Text +
                        "," + DDLEmpleado.SelectedValue +
                        "," + DDLAutorizado.SelectedValue +
                        "," + Session["USUARIO"].ToString();
                else
                    vQuery = "[RSP_ActivosPI] 2" +
                        "," + LbIdActivoAsignar.Text + 
                        "," + DDLEmpleado.SelectedValue + 
                        "," + DDLAutorizado.SelectedValue + 
                        "," + Session["USUARIO"].ToString();

                int vInfo = vConexion.ejecutarSql(vQuery);

                if (vInfo == 2){
                    Mensaje("Asignación realizada con éxito.", WarningType.Success);
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeAsign();", true);
                cargarDatos();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GvActivos_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GvActivos.PageIndex = e.NewPageIndex;
                GvActivos.DataSource = (DataTable)Session["ACTIVOS_ASIGNADOS"];
                GvActivos.DataBind();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        void validar() {
            if (DDLEmpleado.SelectedValue == "0")
                throw new Exception("Por favor seleccione el Empleado.");
        }

        void limpiarModalActivos() {
            DDLEmpleado.SelectedValue = "0";
            DDLAutorizado.SelectedValue = "2";
        }

        protected void GvMisActivos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
    }
}