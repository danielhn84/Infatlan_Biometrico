using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace BiometricoWeb.pages.servicios
{
    public partial class parkingList : System.Web.UI.Page
    {
        db vConexion = new db();
        generales vPermiso = new generales();
        protected void Page_Load(object sender, EventArgs e){
            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    Boolean vPerfil = vPermiso.Permisos((DataTable)Session["AUTHCLASS"], "16");
                    navCategoria1.Visible = vPerfil;
                    cargarDatos(vPerfil);
                }
            }
        }

        private void cargarDatos(Boolean vPerfil){
            try{
                String vQuery = "[RSP_Parqueos] 8,1,1,2";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                GvListaA.PageIndex = 0;
                GvListaA.DataSource = null;
                GvListaA.DataBind();
                if (vDatos.Rows.Count > 0){
                    GvListaA.DataSource = vDatos;
                    GvListaA.DataBind();
                    verLista(vDatos, 0, GvListaA);
                    Session["PARKING_LISTA_A"] = vDatos;
                }

                vQuery = "[RSP_Parqueos] 8,2,1,2";
                vDatos = vConexion.obtenerDataTable(vQuery);
                GvListaB.PageIndex = 0;
                GvListaB.DataSource = null;
                GvListaB.DataBind();
                if (vDatos.Rows.Count > 0){
                    GvListaB.DataSource = vDatos;
                    GvListaB.Columns[6].Visible = vPerfil;
                    GvListaB.DataBind();
                    verLista(vDatos, 0, GvListaB);
                    Session["PARKING_LISTA_B"] = vDatos;
                }

                vQuery = "[RSP_Parqueos] 9,1,2";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    GvParqueos.DataSource = vDatos;
                    GvParqueos.DataBind();
                    int vCuenta = 0;
                    foreach (GridViewRow row in GvParqueos.Rows){
                        LinkButton button = row.FindControl("BtnParqueo") as LinkButton;
                        button.Text = vDatos.Rows[vCuenta]["parqueo"].ToString();
                        vCuenta++;
                    }
                    Session["PARKING"] = vDatos;
                }

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        private void verLista(DataTable vDatos, int vCuenta, GridView vGid) {
            foreach (GridViewRow row in vGid.Rows){
                LinkButton button = row.FindControl("BtnParqueo") as LinkButton;
                button.Text = vDatos.Rows[vCuenta]["idLista"].ToString();
                vCuenta++;
            }
        }

        protected void TxBuscarNombreA_TextChanged(object sender, EventArgs e){
            try{
                Boolean vPerfil = vPermiso.Permisos((DataTable)Session["AUTHCLASS"], "16");
                cargarDatos(vPerfil);
                String vBusqueda = TxBuscarNombreA.Text;
                DataTable vDatos = (DataTable)Session["PARKING_LISTA_A"];

                if (vBusqueda.Equals("")){
                    GvListaA.DataSource = vDatos;
                    GvListaA.DataBind();
                    verLista(vDatos, 0, GvListaA);
                }else{
                    EnumerableRowCollection<DataRow> filtered = vDatos.AsEnumerable()
                        .Where(r => r.Field<String>("nombreEmpleado").ToUpper().Contains(vBusqueda.ToUpper()));

                    Boolean isNumeric = int.TryParse(vBusqueda, out int n);

                    if (isNumeric){
                        if (filtered.Count() == 0){
                            filtered = vDatos.AsEnumerable().Where(r =>
                                Convert.ToInt32(r["idLista"]) == Convert.ToInt32(vBusqueda));
                        }
                    }

                    DataTable vDatosFiltrados = new DataTable();
                    vDatosFiltrados.Columns.Add("id");
                    vDatosFiltrados.Columns.Add("idLista");
                    vDatosFiltrados.Columns.Add("nombreEmpleado");
                    vDatosFiltrados.Columns.Add("fechaModifico");
                    vDatosFiltrados.Columns.Add("nombreZona");
                    foreach (DataRow item in filtered){
                        vDatosFiltrados.Rows.Add(
                            item["id"].ToString(),
                            item["idLista"].ToString(),
                            item["nombreEmpleado"].ToString(),
                            item["fechaModifico"].ToString(),
                            item["nombreZona"].ToString()
                            );
                    }

                    GvListaA.DataSource = vDatosFiltrados;
                    GvListaA.DataBind();
                    verLista(vDatosFiltrados, 0, GvListaA);
                    Session["PARKING_LISTA_A"] = vDatosFiltrados;
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void TxBuscarNombreB_TextChanged(object sender, EventArgs e){
            try{
                Boolean vPerfil = vPermiso.Permisos((DataTable)Session["AUTHCLASS"], "16");
                cargarDatos(vPerfil);
                String vBusqueda = TxBuscarNombreB.Text;
                DataTable vDatos = (DataTable)Session["PARKING_LISTA_B"];

                if (vBusqueda.Equals("")){
                    GvListaB.DataSource = vDatos;
                    GvListaB.DataBind();
                    verLista(vDatos, 0, GvListaB);
                }else{
                    EnumerableRowCollection<DataRow> filtered = vDatos.AsEnumerable()
                        .Where(r => r.Field<String>("nombreEmpleado").ToUpper().Contains(vBusqueda.ToUpper()));

                    Boolean isNumeric = int.TryParse(vBusqueda, out int n);
                    if (isNumeric){
                        if (filtered.Count() == 0){
                            filtered = vDatos.AsEnumerable().Where(r =>
                                Convert.ToInt32(r["idLista"]) == Convert.ToInt32(vBusqueda));
                        }
                    }

                    DataTable vDatosFiltrados = new DataTable();
                    vDatosFiltrados.Columns.Add("id");
                    vDatosFiltrados.Columns.Add("idLista");
                    vDatosFiltrados.Columns.Add("nombreEmpleado");
                    vDatosFiltrados.Columns.Add("nombreZona");
                    vDatosFiltrados.Columns.Add("fechaCreacion");
                    foreach (DataRow item in filtered){
                        vDatosFiltrados.Rows.Add(
                            item["id"].ToString(),
                            item["idLista"].ToString(),
                            item["nombreEmpleado"].ToString(),
                            item["nombreZona"].ToString(),
                            item["fechaCreacion"].ToString()
                            );
                    }

                    GvListaB.DataSource = vDatosFiltrados;
                    GvListaB.DataBind();
                    verLista(vDatosFiltrados, 0, GvListaB);
                    Session["PARKING_LISTA_B"] = vDatosFiltrados;
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
        
        protected void TxBuscarParqueo_TextChanged(object sender, EventArgs e){
            try{
                Boolean vPerfil = vPermiso.Permisos((DataTable)Session["AUTHCLASS"], "16");
                cargarDatos(vPerfil);
                String vBusqueda = TxBuscarParqueo.Text;
                DataTable vDatos = (DataTable)Session["PARKING"];

                if (vBusqueda.Equals("")){
                    GvParqueos.DataSource = vDatos;
                    GvParqueos.DataBind();
                }else{
                    EnumerableRowCollection<DataRow> filtered = vDatos.AsEnumerable()
                        .Where(r => r.Field<String>("nombreEmpleado").ToUpper().Contains(vBusqueda.ToUpper()));

                    Boolean isNumeric = int.TryParse(vBusqueda, out int n);
                    if (isNumeric){
                        if (filtered.Count() == 0){
                            filtered = vDatos.AsEnumerable().Where(r =>
                                Convert.ToInt32(r["parqueo"]) == Convert.ToInt32(vBusqueda));
                        }
                    }

                    DataTable vDatosFiltrados = new DataTable();
                    vDatosFiltrados.Columns.Add("id");
                    vDatosFiltrados.Columns.Add("parqueo");
                    vDatosFiltrados.Columns.Add("nombreEmpleado");
                    vDatosFiltrados.Columns.Add("nombreZona");
                    foreach (DataRow item in filtered){
                        vDatosFiltrados.Rows.Add(
                            item["id"].ToString(),
                            item["parqueo"].ToString(),
                            item["nombreEmpleado"].ToString(),
                            item["nombreZona"].ToString()
                            );
                    }

                    GvParqueos.DataSource = vDatosFiltrados;
                    GvParqueos.DataBind();
                    Session["PARKING"] = vDatosFiltrados;
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void DDLZonaA_SelectedIndexChanged(object sender, EventArgs e){

        }

        protected void DDLZonaB_SelectedIndexChanged(object sender, EventArgs e){

        }

        protected void GvListaA_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GvListaA.PageIndex = e.NewPageIndex;
                GvListaA.DataSource = (DataTable)Session["PARKING_LISTA_A"];
                GvListaA.DataBind();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GvListaB_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                DataTable vDatos = (DataTable)Session["PARKING_LISTA_B"];
                GvListaB.PageIndex = e.NewPageIndex;
                GvListaB.DataSource = vDatos;
                GvListaB.DataBind();
                verLista(vDatos, e.NewPageIndex * 10, GvListaB);
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GvParqueos_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                DataTable vDatos = (DataTable)Session["PARKING"];
                GvParqueos.PageIndex = e.NewPageIndex;
                GvParqueos.DataSource = vDatos;
                GvParqueos.DataBind();
                int vCuenta = e.NewPageIndex * 10;
                foreach (GridViewRow row in GvParqueos.Rows){
                    LinkButton button = row.FindControl("BtnParqueo") as LinkButton;
                    button.Text = vDatos.Rows[vCuenta]["parqueo"].ToString();
                    vCuenta++;
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GvListaA_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                String vId = e.CommandArgument.ToString();
                if (e.CommandName == "editarLista"){
                    LbListId.Text = vId;

                    DDLCategoria.Items.Clear();
                    DDLCategoria.Items.Add(new ListItem { Value = "2", Text = "Categoria B" });
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#ModalEditList').modal('show');", true);
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GvListaB_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                String vId = e.CommandArgument.ToString();
                if (e.CommandName == "editarLista"){
                    LbListId.Text = vId;
                    DDLCategoria.Items.Clear();
                    DDLCategoria.Items.Add(new ListItem { Value = "1", Text = "Categoria A" });

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#ModalEditList').modal('show');", true);
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e){
            try{
                DivAlerta.Visible = false;
                LbAlerta.Text = string.Empty;

                String vCambio = DDLCategoria.SelectedValue == "1" ? "10" : "11";
                String vQuery = "[RSP_Parqueos] " + vCambio + "," + LbListId.Text;
                int vInfo = vConexion.ejecutarSql(vQuery);
                if (vInfo > 0){
                    Mensaje("Actualización realizada con éxito.", WarningType.Success);
                    Boolean vPerfil = vPermiso.Permisos((DataTable)Session["AUTHCLASS"], "16");
                    cargarDatos(vPerfil);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#ModalEditList').modal('hide');", true);
                }
            }catch (Exception ex){
                DivAlerta.Visible = true;
                LbAlerta.Text = ex.Message;
            }
        }
    }
}