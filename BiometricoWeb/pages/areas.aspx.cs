using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BiometricoWeb.pages
{
    public partial class areas : System.Web.UI.Page
    {
        db vConexion;
        protected void Page_Load(object sender, EventArgs e){
            vConexion = new db();
            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    generales vGenerales = new generales();
                    DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                    if (!vGenerales.PermisosRecursosHumanos(vDatos))
                        Response.Redirect("/default.aspx");

                    CargarDepartamento();
                }
            }
        }

        void CargarDepartamento(){
            try{
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerGenerales 12");

                GVBusqueda.DataSource = vDatos;
                GVBusqueda.DataBind();
                Session["DATADEPARTAMENTOS"] = vDatos;

            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        void limpiarModal() {
            TxArea.Text = string.Empty;
            TxIdArea.Text = string.Empty;
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void TxBuscarArea_TextChanged(object sender, EventArgs e){
            try{
                CargarDepartamento();

                String vBusqueda = TxBuscarArea.Text;
                DataTable vDatos = (DataTable)Session["DATADEPARTAMENTOS"];

                if (vBusqueda.Equals("")){
                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                    UpdateGridView.Update();
                }else{
                    EnumerableRowCollection<DataRow> filtered = vDatos.AsEnumerable()
                        .Where(r => r.Field<String>("nombre").Contains(vBusqueda.ToUpper()));

                    Boolean isNumeric = int.TryParse(vBusqueda, out int n);

                    if (isNumeric){
                        if (filtered.Count() == 0){
                            filtered = vDatos.AsEnumerable().Where(r =>
                                Convert.ToInt32(r["idPuesto"]) == Convert.ToInt32(vBusqueda));
                        }
                    }

                    DataTable vDatosFiltrados = new DataTable();
                    vDatosFiltrados.Columns.Add("idDepartamento");
                    vDatosFiltrados.Columns.Add("nombre");

                    foreach (DataRow item in filtered){
                        vDatosFiltrados.Rows.Add(
                            item["idDepartamento"].ToString(),
                            item["nombre"].ToString()
                            );
                    }

                    GVBusqueda.DataSource = vDatosFiltrados;
                    GVBusqueda.DataBind();
                    Session["DATADEPARTAMENTOS"] = vDatosFiltrados;
                    UpdateGridView.Update();
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GVBusqueda.PageIndex = e.NewPageIndex;
                GVBusqueda.DataSource = (DataTable)Session["DATADEPARTAMENTOS"];
                GVBusqueda.DataBind();
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                string vIdArea = e.CommandArgument.ToString();
                if (e.CommandName == "AreaModificar"){
                    LbModArea.Text = vIdArea;
                    Session["ACCION"] = "1";
                    DivEstado.Visible = true;
                    TxIdArea.ReadOnly = true;
                    DataTable vDatos = new DataTable();
                    vDatos = vConexion.obtenerDataTable("RSP_ObtenerGenerales 14," + vIdArea );

                    foreach (DataRow item in vDatos.Rows){
                        TxArea.Text = item["nombre"].ToString();
                        TxIdArea.Text = item["idDepartamento"].ToString();
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void BtnCrear_Click(object sender, EventArgs e){
            try{
                if (Session["ACCION"].ToString() == "1") { //MODIFICAR
                    String vQuery = "[RSP_IngresaMantenimientos] 4, '" + TxIdArea.Text + "','" + TxArea.Text.ToUpper() + "'," + DDLEstado.SelectedValue;
                    Int32 vInformacion = vConexion.ejecutarSql(vQuery);
                    if (vInformacion == 1){
                        Mensaje("Actualizado con Exito!", WarningType.Success);
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "$('#AreaModal').modal('hide');", true);
                    }
                }else if (Session["ACCION"].ToString() == "2") { //CREAR 
                    validaDepto();
                    String vQuery = "[RSP_IngresaMantenimientos] 2, '" + TxArea.Text.ToUpper() + "', '" + TxIdArea.Text + "'";
                    Int32 vInformacion = vConexion.ejecutarSql(vQuery);

                    if (vInformacion == 1){
                        Mensaje("Creado con Exito!", WarningType.Success);
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "$('#AreaModal').modal('hide');", true);
                    }
                    
                }
                CargarDepartamento();
                limpiarModal();

            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        void validaDepto() {
            String vQuery = "[RSP_ObtenerGenerales] 13, '" + TxArea.Text + "'";
            DataTable vData = vConexion.obtenerDataTable(vQuery);

            if (vData.Rows.Count > 0)
                throw new Exception("El nombre de área ya existe. Ingrese otro");
            if (TxArea.Text == "" || TxArea.Text == String.Empty)
                throw new Exception("Ingrese el nombre del área.");
            if (TxIdArea.Text == "" || TxIdArea.Text == String.Empty)
                throw new Exception("Ingrese el Id.");
        }

        protected void btnNuevo_Click(object sender, EventArgs e){
            limpiarModal();
            DivEstado.Visible = false;
            TxIdArea.ReadOnly = false;
            Session["ACCION"] = "2";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "$('#AreaModal').modal('show');", true);
        }
    }
}