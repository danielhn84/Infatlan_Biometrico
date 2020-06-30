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
    public partial class transporte : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["CARGAR_DATA_TRANSPORTE"] = null;
            if (!Page.IsPostBack)
            {
                cargarData();
            }
        }

        void cargarData()
        {
            if (HttpContext.Current.Session["CARGAR_DATA_TRANSPORTE"] == null)
            {
                //CARGAR TRANSPORTE
                DataTable vDato = new DataTable();
                vDato = vConexion.obtenerDataTable("VIATICOS_ObtenerGenerales 34");
                GVSolicitud.DataSource = vDato;
                GVSolicitud.DataBind();
                Session["TRANSPORTES"] = vDato;

                Session["CARGAR_DATA_TRANSPORTE"] = 1;
            }
        }
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        protected void GVSolicitud_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVSolicitud.PageIndex = e.NewPageIndex;
                GVSolicitud.DataSource = (DataTable)Session["TRANSPORTES"];
                GVSolicitud.DataBind();
            }
            catch (Exception Ex)
            {

            }
        }

        void limpiar()
        {
            txtModalModificarTransporte.Text = "";
            txtModalTransporte.Text = "";
            txtbuscarTransporte.Text = "";
            UPBuscarTra.Update();
            GVSolicitud.DataSource = null;
            GVSolicitud.DataBind();
            UpdateGridView.Update();
            UPBuscarTra.Update();

            //CARGAR TRANPORTE
            DataTable vDato = new DataTable();
            vDato = vConexion.obtenerDataTable("VIATICOS_ObtenerGenerales 34");
            GVSolicitud.DataSource = vDato;
            GVSolicitud.DataBind();
            Session["TRANSPORTES"] = vDato;

        }

        protected void GVSolicitud_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            limpiar();
            DataTable vDataaaa = (DataTable)Session["TRANSPORTES"];
            string codTransporte = e.CommandArgument.ToString();
            if (e.CommandName == "Crear")
            {

                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("VIATICOS_ObtenerGenerales 37,'" + codTransporte + "'");
                //vDatos = vConexion.ObtenerTabla(vQuery);
                foreach (DataRow item in vDatos.Rows)
                {
                    Session["VIATICOS_MANT_CODTRANSPORTE"] = codTransporte;
                    txtModalModificarTransporte.Text = item["Transporte"].ToString();                    
                }
                UpdatePanel1.Update();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openModal2();", true);
            }
        }

        protected void btnCrearTransporte_Click(object sender, EventArgs e)
        {
            if (txtModalTransporte.Text == "")
            {
                Mensaje("Inngrese nuevo transporte.", WarningType.Success);
            }
            else
            {
                string vQuery = "VIATICOS_ObtenerGenerales 35, '" + txtModalTransporte.Text + "','" + Session["USUARIO"].ToString() + "'";
                Int32 vInfo = vConexion.ejecutarSql(vQuery);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal();", true);
                Mensaje("Nuevo transporte guardado con éxito.", WarningType.Success);
                limpiar();
            }
        }

        protected void btnCerrarTransporte_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal();", true);
        }

        protected void btnModificarTransporte_Click(object sender, EventArgs e)
        {
            if (txtModalModificarTransporte.Text == "")
            {
                Mensaje("Inngrese nuevo transporte.", WarningType.Success);
            }
            else
            {
                string vQuery = "VIATICOS_ObtenerGenerales 36, '"+ Session["VIATICOS_MANT_CODTRANSPORTE"] + "', '" + txtModalModificarTransporte.Text + "','" + Session["USUARIO"].ToString() + "'";
                Int32 vInfo = vConexion.ejecutarSql(vQuery);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal2();", true);
                Mensaje("Transporte modificado con éxito.", WarningType.Success);
                limpiar();
            }
        }

        protected void btnCerrarModTransporte_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal2();", true);
        }

        protected void txtbuscarTransporte_TextChanged(object sender, EventArgs e)
        {
            try
            {
                cargarData();

                String vBusqueda = txtbuscarTransporte.Text;
                DataTable vDatos = (DataTable)Session["TRANSPORTES"];

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
                        .Where(r => r.Field<String>("Transporte").Contains(vBusqueda));

                    DataTable vDatosFiltrados = new DataTable();
                    vDatosFiltrados.Columns.Add("ID");
                    vDatosFiltrados.Columns.Add("Transporte");
                    //vDatosFiltrados.Columns.Add("Hotel");
                    //vDatosFiltrados.Columns.Add("Habitacion");
                    //vDatosFiltrados.Columns.Add("Costo");

                    foreach (DataRow item in filtered)
                    {
                        vDatosFiltrados.Rows.Add(
                            item["ID"].ToString(),
                            item["Transporte"].ToString()
                           

                            );
                    }

                    GVSolicitud.DataSource = vDatosFiltrados;
                    GVSolicitud.DataBind();
                    Session["TRANSPORTES"] = vDatosFiltrados;
                    UpdateGridView.Update();
                }


            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void btnNewTransporte_Click(object sender, EventArgs e)
        {
            limpiar();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openModal();", true);
        }
    }
}