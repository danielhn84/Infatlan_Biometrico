﻿using BiometricoWeb.clases;
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
        String vId = "";
        protected void Page_Load(object sender, EventArgs e){
            try{
                if (!Page.IsPostBack){
                    if (Convert.ToBoolean(Session["AUTH"])){
                        if (Session["DOCUMENTOS_TIPO_ID"] == null)
                            Response.Redirect("crearDocumentos.aspx");
                        
                        string vIdTipo = Session["DOCUMENTOS_TIPO_ID"].ToString();
                        vId = vIdTipo;
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
                        else if(vIdTipo == "6")
                            TxTitulo.Text = "Externos";

                        DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                        cargarDatos(vIdTipo);
                    }
                }
			}catch (Exception ex){
				String vError = ex.Message;
			}
        }
        
        private void cargarDatos(String vId) {
            try{
                String vQuery = "[RSP_Documentacion] 3," + vId + ",NULL," + Session["USUARIO"].ToString();
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                    mostrarOcultar(vDatos);
                    Session["DOCUMENTOS"] = vDatos;
                }

                vQuery = "[RSP_Documentacion] 16";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLDepartamento.Items.Clear();
                    DDLDepartamento.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLDepartamento.Items.Add(new ListItem { Value = item["idDepartamento"].ToString(), Text = item["nombre"].ToString() });
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

                vQuery = "[RSP_Documentacion] 14";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLNivelConfidencialidad.Items.Clear();
                    DDLNivelConfidencialidad.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLNivelConfidencialidad.Items.Add(new ListItem { Value = item["idConfidencialidad"].ToString(), Text = item["nombre"].ToString()});
                    }
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        private void mostrarOcultar(DataTable vDatos) {
            try{
                String vQuery = "[RSP_Perfiles] 2," + Session["USUARIO"].ToString() + ",5";
                DataTable vDataUser = vConexion.obtenerDataTable(vQuery);
                if (vDataUser.Rows.Count > 0){
                    if (vDataUser.Rows[0][0].ToString() == "1"){
                      GVBusqueda.Columns[10].Visible = true;
                    }
                }
                
                int vCuenta = 0;
                foreach (GridViewRow row in GVBusqueda.Rows){
                    if (row.Cells[4].Text.Equals("2")){
                        LinkButton button = row.FindControl("BtnNiveles") as LinkButton;
                        button.Text = "Uso Interno";
                        button.Style.Value = "background-color:#5bc0de";
                        button.ToolTip = vDatos.Rows[vCuenta]["descripcion"].ToString();
                    }
                    if (row.Cells[4].Text.Equals("3")){
                        LinkButton button = row.FindControl("BtnNiveles") as LinkButton;
                        button.Text = "Restringido";
                        button.Style.Value = "background-color:#f0ad4e";
                        button.ToolTip = vDatos.Rows[vCuenta]["descripcion"].ToString();
                    }
                    if (row.Cells[4].Text.Equals("4")){
                        LinkButton button = row.FindControl("BtnNiveles") as LinkButton;
                        button.Text = "Confidencial";
                        button.Style.Value = "background-color:#d9534f";
                        button.ToolTip = vDatos.Rows[vCuenta]["descripcion"].ToString();
                    }

                    vQuery = "[RSP_Documentacion] 24," + vDatos.Rows[vCuenta]["idDocumento"].ToString();
                    DataTable vDataRef = vConexion.obtenerDataTable(vQuery);
                    if (vDataRef.Rows.Count < 1){
                        LinkButton button = row.FindControl("BtnReferencias") as LinkButton;
                        button.CssClass = "btn btn default";
                        button.Style.Value = "background-color:#8b8b8c";
                        button.Enabled = false;
                    }
                    vCuenta++;
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void TxBuscar_TextChanged(object sender, EventArgs e){

        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                String vId = e.CommandArgument.ToString();
                String vQuery = "[RSP_Documentacion] 5," + vId;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                Session["DOCUMENTOS_ARCHIVO_ID"] = vId;
                if (e.CommandName == "verDocumento"){
                    Response.Redirect("archivo.aspx");
                }

                if (e.CommandName == "editarDoc"){
                    LiEditarDoc.Text = "Editar documento <b>" + vDatos.Rows[0]["codigo"].ToString() + "</b>";
                    TxNombre.Text = vDatos.Rows[0]["nombre"].ToString();
                    DDLCategoria.SelectedValue = vDatos.Rows[0]["idCategoria"].ToString();
                    DDLConfirmacion.SelectedValue = Convert.ToBoolean(vDatos.Rows[0]["flagLectura"]) ? "1" : "0";
                    DDLNivelConfidencialidad.SelectedValue = vDatos.Rows[0]["nivelConfidencialidad"].ToString() != "" ? vDatos.Rows[0]["nivelConfidencialidad"].ToString() : "0";
                    DDLEstado.SelectedValue = vDatos.Rows[0]["estado"].ToString() == "1" ? "1" : "0";
                    CBxConfidencial.Checked = Convert.ToBoolean(vDatos.Rows[0]["flagConfidencial"]) ? true : false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }

                if (e.CommandName == "docReferencias"){
                    Literal1.Text = "Referencias del documento <b>" + vDatos.Rows[0]["codigo"].ToString() + "</b>";
                    vQuery = "[RSP_Documentacion] 24," + vId;
                    vDatos = vConexion.obtenerDataTable(vQuery);
                    if (vDatos.Rows.Count > 0){
                        GvReferencias.DataSource = vDatos;
                        GvReferencias.DataBind();
                        Session["DOCUMENTOS_DOC_REF"] = vDatos;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalRef();", true);
                    }
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GVBusqueda.PageIndex = e.NewPageIndex;
                GVBusqueda.DataSource = (DataTable)Session["DOCUMENTOS"];
                GVBusqueda.DataBind();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnEditarDoc_Click(object sender, EventArgs e){
            try{
                xml vDatos = new xml();
                Object[] vDatosMaestro = new object[16];
                vDatosMaestro[0] = Session["DOCUMENTOS_ARCHIVO_ID"].ToString();
                vDatosMaestro[1] = DDLCategoria.SelectedValue;
                vDatosMaestro[2] = TxNombre.Text;
                vDatosMaestro[3] = "";
                vDatosMaestro[4] = "";
                vDatosMaestro[5] = "";
                vDatosMaestro[6] = "";
                vDatosMaestro[7] = DDLConfirmacion.SelectedValue;
                vDatosMaestro[8] = "";
                vDatosMaestro[9] = TxFecha.Text != "" ? Convert.ToDateTime(TxFecha.Text).ToString("yyyy-MM-dd HH:mm:ss") : "1900-01-01 00:00:00";
                vDatosMaestro[10] = DDLRecordatorios.SelectedValue;
                vDatosMaestro[11] = DDLEstado.SelectedValue;
                vDatosMaestro[12] = Session["USUARIO"].ToString();
                vDatosMaestro[13] = CBxConfidencial.Checked;
                vDatosMaestro[14] = DDLNivelConfidencialidad.SelectedValue;
                vDatosMaestro[15] = "";
                String vXML = vDatos.ObtenerXMLDocumentos(vDatosMaestro);
                vXML = vXML.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
                
                String vQuery = "[RSP_Documentacion] 26,0,'" + vXML + "'";  
                int vInfo = vConexion.ejecutarSql(vQuery);
                if (vInfo > 0){
                    Mensaje("Documento actualizado con éxito.", WarningType.Success);
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);
                cargarDatos(Session["DOCUMENTOS_TIPO_ID"].ToString());
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void DDLDepartamento_SelectedIndexChanged(object sender, EventArgs e){
            try{
                String vQuery = "[RSP_Documentacion] 17," + DDLDepartamento.SelectedValue;
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

        protected void BtnBuscar_Click(object sender, EventArgs e){
            try{
                String vQuery = "[RSP_Documentacion] 18," + Session["DOCUMENTOS_TIPO_ID"].ToString() + ",NULL," + Session["USUARIO"].ToString() + "," + DDLDepartamento.SelectedValue;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                    mostrarOcultar(vDatos);
                    Session["DOCUMENTOS"] = vDatos;
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GvReferencias_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                String vId = e.CommandArgument.ToString();
                Session["DOCUMENTOS_ARCHIVO_ID"] = vId;
                if (e.CommandName == "verDocumento"){
                    Response.Redirect("archivo.aspx");
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
    }
}