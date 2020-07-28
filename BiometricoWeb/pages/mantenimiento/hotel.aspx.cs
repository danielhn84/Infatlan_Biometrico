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
    public partial class hotel : System.Web.UI.Page
    {
        db vConexion = new db();
        db vConexion2 = new db();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["CARGAR_DATA_HOTELES"] = null;
            if (!Page.IsPostBack)
            {
                if (Convert.ToBoolean(Session["AUTH"])){
                    cargarData();
                }else{
                    Response.Redirect("/login.aspx");
                }
            }
        }

        void validar()
        {
            if (txtModalHotel.Text == "" || txtModalHotel.Text == string.Empty)
                throw new Exception("Favor ingrese hotel.");
            if (DDLModalUbicacion.SelectedValue == "0")
                throw new Exception("Favor seleccione ubicación.");
            if (DDLDesayuno.SelectedValue == "0")
                throw new Exception("Favor seleccione si hotel cuenta con desayuno.");
            if (txtModalHabitacion.Text == "" || txtModalHabitacion.Text == string.Empty)
                throw new Exception("Favor ingrese habitación.");
            if (txtModalPrecio.Text == "" || txtModalPrecio.Text == string.Empty)
                throw new Exception("Favor ingrese precio.");
            if(Convert.ToDecimal(txtModalPrecio.Text)<=0)
                throw new Exception("Favor ingrese un monto válido.");

        }
        void limpiar()
        {
            txtbuscarHotel.Text = "";
            txtNewHotel.Text = "";
            DDLUbicacion.SelectedValue = "0";
            DDLDesayuno.SelectedValue = "1";
            DDLHoteleria.SelectedValue = "0";
            txtHabitacion.Text = "";
            txtPrecio.Text = "";
            txtModalHotel.Text = "";
            DDLModalUbicacion.SelectedValue = "0";
            DDLModalDesayuno.SelectedValue = "0";
            txtModalHabitacion.Text = "";
            txtModalPrecio.Text = "";
            Session["VIATICOS_MANT_CODHABITACION"] = null;
            Session["VIATICOS_MANT_CODHOTEL"] = null;
            GVSolicitud.DataSource = null;
            GVSolicitud.DataBind();
            UPBuscarHotel.Update();

            //CARGAR SOLICITUDES FINALIZADAS
            DataTable vDato = new DataTable();
            vDato = vConexion.obtenerDataTable("VIATICOS_ObtenerGenerales 27");
            GVSolicitud.DataSource = vDato;
            GVSolicitud.DataBind();
            Session["HOTELESyHABITACIONES"] = vDato;
        }
        void cargarData()
        {
            if (HttpContext.Current.Session["CARGAR_DATA_HOTELES"] == null)
            {               
                    //CARGAR SOLICITUDES FINALIZADAS
                    DataTable vDato = new DataTable();
                    vDato = vConexion.obtenerDataTable("VIATICOS_ObtenerGenerales 27");
                    GVSolicitud.DataSource = vDato;
                    GVSolicitud.DataBind();
                    Session["HOTELESyHABITACIONES"] = vDato;

                //CARGAR UBICACION
                String vQuery10 = "STEISP_ATM_Generales 12";
                DataTable vDatos10 = vConexion2.obtenerDataTableSTEI(vQuery10);
                DDLUbicacion.Items.Add(new ListItem { Value = "0", Text = "Seleccione destino..." });
                DDLModalUbicacion.Items.Add(new ListItem { Value = "0", Text = "Seleccione destino..." });
                foreach (DataRow item in vDatos10.Rows)
                {
                    DDLUbicacion.Items.Add(new ListItem { Value = item["idMunicipio"].ToString(), Text = item["nombre"].ToString() });
                    DDLModalUbicacion.Items.Add(new ListItem { Value = item["idMunicipio"].ToString(), Text = item["nombre"].ToString() });
                }

                //CARGAR HABITACION
                //String vQuery2 = "VIATICOS_ObtenerGenerales 28";
                //DataTable vDatos2 = vConexion.obtenerDataTable(vQuery2);
                //DDLHabitacion.Items.Add(new ListItem { Value = "0", Text = "Seleccione habitacion..." });
                //foreach (DataRow item in vDatos2.Rows)
                //{
                //    DDLHabitacion.Items.Add(new ListItem { Text = item["nombre"].ToString() });
                //}

                //CARGAR HOTELES
                String vQuery3 = "VIATICOS_ObtenerGenerales 29";
                DataTable vDatos3 = vConexion.obtenerDataTable(vQuery3);
                DDLHoteleria.Items.Add(new ListItem { Value = "0", Text = "Seleccione hotel..." });
               
                foreach (DataRow item in vDatos3.Rows)
                {
                    DDLHoteleria.Items.Add(new ListItem { Value = item["idHotel"].ToString(), Text = item["nombre"].ToString() });
                    
                }

                Session["CARGAR_DATA_HOTELES"] = 1;
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
        protected void GVSolicitud_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVSolicitud.PageIndex = e.NewPageIndex;
                GVSolicitud.DataSource = (DataTable)Session["HOTELESyHABITACIONES"];
                GVSolicitud.DataBind();
            }
            catch (Exception Ex)
            {

            }
        }

        protected void GVSolicitud_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            limpiar();
            DataTable vDataaaa = (DataTable)Session["HOTELESyHABITACIONES"];
            string codHabitacion = e.CommandArgument.ToString();
            if (e.CommandName == "Crear")
            {

                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("VIATICOS_ObtenerGenerales 26,'" + codHabitacion + "'");
                //vDatos = vConexion.ObtenerTabla(vQuery);
                foreach (DataRow item in vDatos.Rows)
                {
                    Session["VIATICOS_MANT_CODHABITACION"] = codHabitacion;
                    Session["VIATICOS_MANT_CODHOTEL"] = item["IDHotel"].ToString();
                    DDLModalUbicacion.SelectedIndex = CargarInformacionDDL(DDLModalUbicacion, item["Municipio"].ToString());
                    DDLModalDesayuno.SelectedIndex = CargarInformacionDDL(DDLModalDesayuno, item["Desayuno"].ToString());  
                   txtModalHotel.Text = item["Hotel"].ToString();
                    txtModalHabitacion.Text = item["Habitacion"].ToString();
                    txtModalPrecio.Text = item["Costo"].ToString();
                }
                UPModalHoteles.Update();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openModal3();", true);
            }
        }

        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void txtbuscarHotel_TextChanged(object sender, EventArgs e)
        {
            try
            {
                cargarData();

                String vBusqueda = txtbuscarHotel.Text;
                DataTable vDatos = (DataTable)Session["HOTELESyHABITACIONES"];

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
                        .Where(r => r.Field<String>("Hotel").Contains(vBusqueda));

                    DataTable vDatosFiltrados = new DataTable();
                    vDatosFiltrados.Columns.Add("IDHotel");
                    vDatosFiltrados.Columns.Add("IDHabitacion");
                    vDatosFiltrados.Columns.Add("Hotel");
                    vDatosFiltrados.Columns.Add("Habitacion");
                    vDatosFiltrados.Columns.Add("Costo");
                   
                    foreach (DataRow item in filtered)
                    {
                        vDatosFiltrados.Rows.Add(
                            item["IDHotel"].ToString(),
                            item["IDHabitacion"].ToString(),
                            item["Hotel"].ToString(),
                            item["Habitacion"].ToString(),
                            item["Costo"].ToString()
                           
                            );
                    }

                    GVSolicitud.DataSource = vDatosFiltrados;
                    GVSolicitud.DataBind();
                    Session["HOTELESyHABITACIONES"] = vDatosFiltrados;
                    UpdateGridView.Update();
                }


            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void btnEnviarHotel_Click(object sender, EventArgs e)
        {
            if (txtNewHotel.Text == "" || DDLUbicacion.SelectedValue == "0")
                Mensaje("No deje campos vacíos.", WarningType.Warning);
            else
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openModal();", true);
        }

        protected void btnModalCrearHotel_Click(object sender, EventArgs e)
        {
            string vQuery = "VIATICOS_ObtenerGenerales 30, '" + txtNewHotel.Text + "','" + DDLUbicacion.SelectedValue + "','" + DDLDesayuno.SelectedValue + "','" + Session["USUARIO"].ToString() + "'";
            Int32 vInfo = vConexion.ejecutarSql(vQuery);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal();", true);
            Mensaje("Hotel guardado con éxito.",WarningType.Success);
            limpiar();
            UpdatePanelHoteles.Update();


        }

        protected void btnModalCerrarHotel_Click(object sender, EventArgs e)
        {
            limpiar();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal();", true);
        }

        protected void btnCrearHabitacion_Click(object sender, EventArgs e)
        {
            if (DDLHoteleria.SelectedValue == "0" || txtHabitacion.Text == "" || txtPrecio.Text=="")
                Mensaje("No deje campos vacíos.", WarningType.Warning);
            else
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openModal2();", true);
        }

        protected void btnModalCrearHabitacion_Click(object sender, EventArgs e)
        {
            string vQuery = "VIATICOS_ObtenerGenerales 31, '" + txtHabitacion.Text + "','" + DDLHoteleria.SelectedValue + "','" + txtPrecio.Text + "'";
            Int32 vInfo = vConexion.ejecutarSql(vQuery);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal2();", true);
            Mensaje("Habitación guardado con éxito.", WarningType.Success);
            limpiar();
            UPHabitaciones.Update();
        }

        protected void btnModalCerrarHabitacion_Click(object sender, EventArgs e)
        {
            limpiar();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal2();", true);
        }

        protected void btnModalModificar_Click(object sender, EventArgs e)
        {
            try
            {
                validar();
            
            string vQuery = "VIATICOS_ObtenerGenerales 32, '"+ Session["VIATICOS_MANT_CODHOTEL"] + "', '" + txtModalHotel.Text + "','" + DDLModalUbicacion.SelectedValue + "','" + DDLModalDesayuno.SelectedValue + "','" + Session["USUARIO"].ToString() + "'";
            Int32 vInfo = vConexion.ejecutarSql(vQuery);
            string vQuery2 = "VIATICOS_ObtenerGenerales 33, '"+ Session["VIATICOS_MANT_CODHABITACION"] + "', '" + txtModalHabitacion.Text + "','" + Session["VIATICOS_MANT_CODHOTEL"] + "','" + txtModalPrecio.Text + "'";
            Int32 vInfo2 = vConexion.ejecutarSql(vQuery2);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal3();", true);
                limpiar();
                UpdateGridView.Update();
                Mensaje("Hotel modificado con éxito", WarningType.Success);
               
           
            }
            catch (Exception ex)
            {

                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void btnModalCerrarModificar_Click(object sender, EventArgs e)
        {
            limpiar();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal3();", true);
        }
    }
}