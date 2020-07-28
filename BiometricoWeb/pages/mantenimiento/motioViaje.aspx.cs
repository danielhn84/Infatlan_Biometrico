using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace BiometricoWeb.pages.mantenimiento
{
    public partial class motioViaje : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["CARGAR_DATA_MOTIVOVIAJE"] = null;
            if (!Page.IsPostBack)
            {
                if (Convert.ToBoolean(Session["AUTH"])){
                    cargarData();
                }else{
                    Response.Redirect("/login.aspx");
                }
            }
        }

        void cargarData()
        {
            if (HttpContext.Current.Session["CARGAR_DATA_MOTIVOVIAJE"] == null)
            {
                //CARGAR MOTIVOS
                DataTable vDato = new DataTable();
                vDato = vConexion.obtenerDataTable("VIATICOS_ObtenerGenerales 38");
                GVSolicitud.DataSource = vDato;
                GVSolicitud.DataBind();
                Session["MOTIVOSVIAJES"] = vDato;

                Session["CARGAR_DATA_MOTIVOVIAJE"] = 1;
            }
        }
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        void limpiar()
        {
            txtModalModificarMotivo.Text = "";
            txtbuscarMotivos.Text = "";
            txtbuscarMotivos.Text = "";
            DDLModalEstado.SelectedValue = "1";
            UPBuscarTra.Update();
            GVSolicitud.DataSource = null;
            GVSolicitud.DataBind();
            UpdateGridView.Update();
            UPBuscarTra.Update();

            //CARGAR MOTIVOS
            DataTable vDato = new DataTable();
            vDato = vConexion.obtenerDataTable("VIATICOS_ObtenerGenerales 38");
            GVSolicitud.DataSource = vDato;
            GVSolicitud.DataBind();
            Session["MOTIVOSVIAJES"] = vDato;

        }
        protected void btnNewMotivo_Click(object sender, EventArgs e)
        {
            limpiar();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openModal();", true);
        }

        protected void txtbuscarMotivos_TextChanged(object sender, EventArgs e)
        {
            try
            {
                cargarData();

                String vBusqueda = txtbuscarMotivos.Text;
                DataTable vDatos = (DataTable)Session["MOTIVOSVIAJES"];

                if (vBusqueda.Equals(""))
                {
                    GVSolicitud.DataSource = vDatos;
                    GVSolicitud.DataBind();
                    UpdateGridView.Update();
                    //cargarData();
                }
                else
                {
                    EnumerableRowCollection<DataRow> filtered = vDatos.AsEnumerable()
                        .Where(r => r.Field<String>("Motivo").Contains(vBusqueda));

                    DataTable vDatosFiltrados = new DataTable();
                    vDatosFiltrados.Columns.Add("ID");
                    vDatosFiltrados.Columns.Add("Motivo");
                    vDatosFiltrados.Columns.Add("Estado");
                    //vDatosFiltrados.Columns.Add("Habitacion");
                    //vDatosFiltrados.Columns.Add("Costo");

                    foreach (DataRow item in filtered)
                    {
                        vDatosFiltrados.Rows.Add(
                            item["ID"].ToString(),
                            item["Motivo"].ToString(),
                            item["Estado"].ToString()


                            );
                    }

                    GVSolicitud.DataSource = vDatosFiltrados;
                    GVSolicitud.DataBind();
                    Session["MOTIVOSVIAJES"] = vDatosFiltrados;
                    UpdateGridView.Update();
                }


            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void GVSolicitud_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVSolicitud.PageIndex = e.NewPageIndex;
                GVSolicitud.DataSource = (DataTable)Session["MOTIVOSVIAJES"];
                GVSolicitud.DataBind();
            }
            catch (Exception Ex)
            {

            }
        }

        int CargarInformacionDDL(DropDownList vList, String vValue)
        {
            int vIndex = 0;
            try
            {
                int vContador = 0;
                foreach (ListItem item in vList.Items)
                {
                    if (item.Value.Equals(vValue))
                    {
                        vIndex = vContador;
                    }
                    vContador++;
                }
            }
            catch { throw; }
            return vIndex;
        }

        protected void GVSolicitud_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            limpiar();
            DataTable vDataaaa = (DataTable)Session["MOTIVOSVIAJES"];
            string codMotivo = e.CommandArgument.ToString();
            if (e.CommandName == "Crear")
            {

                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("VIATICOS_ObtenerGenerales 41,'" + codMotivo + "'");
                //vDatos = vConexion.ObtenerTabla(vQuery);
                foreach (DataRow item in vDatos.Rows)
                {
                    Session["VIATICOS_MANT_CODMOTIVO"] = codMotivo;
                    txtModalModificarMotivo.Text = item["Motivo"].ToString();
                    DDLModalEstado.SelectedIndex = CargarInformacionDDL(DDLModalEstado, item["Estado"].ToString());
                }
                UpdatePanel1.Update();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openModal2();", true);
            }
        }

        protected void btnCrearMotivo_Click(object sender, EventArgs e)
        {
            if (txtModalMotivo.Text == "")
            {
                Mensaje("Ingrese nuevo motivo.", WarningType.Success);
            }
            else
            {
                string vQuery = "VIATICOS_ObtenerGenerales 39, '" + txtModalMotivo.Text + "','" + Session["USUARIO"].ToString() + "'";
                Int32 vInfo = vConexion.ejecutarSql(vQuery);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal();", true);
                Mensaje("Nuevo motivo de viaje guardado con éxito.", WarningType.Success);
                limpiar();
            }
        }

        protected void btnCerrarMotivo_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal();", true);
        }

        protected void btnModificarMotivo_Click(object sender, EventArgs e)
        {
            if (txtModalModificarMotivo.Text == "")
            {
                Mensaje("Ingrese nuevo motivo de transporte.", WarningType.Success);
            }
            else
            {
                string vQuery = "VIATICOS_ObtenerGenerales 40, '" + Session["VIATICOS_MANT_CODMOTIVO"] + "', '" + txtModalModificarMotivo.Text + "','" + Session["USUARIO"].ToString() + "','"+DDLModalEstado.SelectedValue+"'";
                Int32 vInfo = vConexion.ejecutarSql(vQuery);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal2();", true);
                Mensaje("Motivo de viaje modificado con éxito.", WarningType.Success);
                limpiar();
            }
        }

        protected void btnCerrarModMotivo_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal2();", true);
        }
    }
}