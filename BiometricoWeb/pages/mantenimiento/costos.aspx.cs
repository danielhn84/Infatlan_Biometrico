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
    public partial class costos : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["CARGAR_DATA_COSTOS"] = null;
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
            if (HttpContext.Current.Session["CARGAR_DATA_COSTOS"] == null)
            {
                DDLTipoViaje.Items.Clear();
                DDLCategoria.Items.Clear();
                //CARGAR CATEGORIA
                String vQuery = "VIATICOS_Costos 1";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                DDLCategoria.Items.Add(new ListItem { Value = "0", Text = "Seleccione categoría..." });
                foreach (DataRow item in vDatos.Rows)
                {
                    DDLCategoria.Items.Add(new ListItem { Value = item["idCategoria"].ToString(), Text = "("+ item["idCategoria"].ToString() + ")"+item["nombre"].ToString() });                    
                }

                //CARGAR TIPO TRANSPORTE
                String vQuery2 = "VIATICOS_Costos 2";
                DataTable vDatos2 = vConexion.obtenerDataTable(vQuery2);
                DDLTipoViaje.Items.Add(new ListItem { Value = "0", Text = "Seleccione tipo de viaje..." });
                foreach (DataRow item in vDatos2.Rows)
                {
                    DDLTipoViaje.Items.Add(new ListItem { Value = item["idTipoViaje"].ToString(), Text = item["nombre"].ToString() });
                }
                Session["CARGAR_DATA_COSTOS"] = 1;
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
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        void cargarCostos()
        {
            if (DDLCategoria.SelectedValue != "0" && DDLTipoViaje.SelectedValue != "0")
            {
                //CARGAR COSTOS
                string vQuery = "VIATICOS_Costos 3, '" + DDLCategoria.SelectedValue + "','" + DDLTipoViaje.SelectedValue + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                foreach (DataRow item in vDatos.Rows)
                {
                    txtCirculacion.Text = item["circulacion"].ToString();
                    txtTransporte.Text = item["transporte"].ToString();
                    txtPeaje.Text = item["peaje"].ToString();
                    txtHospedaje.Text = item["hospedaje"].ToString();
                    txtDepre.Text = item["depreciacion"].ToString();
                    txtAlimento.Text = item["alimento"].ToString();
                    txtCABelice.Text = item["CA_Belice"].ToString();
                    txtAreaDolar.Text = item["pais_dolar"].ToString();
                    txtAreaNoDolar.Text = item["pais_Nodolar"].ToString();
                }
                UPCostoTotal.Update();
            }
            else
            {
                limpiar();
                limpiarMoneda();
                DIVInternacional.Visible = false;
                UPCostoTotal.Update();
            }
        }

        void validar()
        {
            if (DDLCategoria.SelectedValue == "0")
                throw new Exception("Favor seleccione categoría.");
            if (DDLTipoViaje.SelectedValue == "0")
                throw new Exception("Favor seleccione tipo de viaje.");
            if (txtCirculacion.Text == "" || txtCirculacion.Text == string.Empty)
                throw new Exception("Favor ingrese costo de circulación.");
            if (txtTransporte.Text == "" || txtTransporte.Text == string.Empty)
                throw new Exception("Favor ingrese costo de transporte.");           
            if (txtPeaje.Text == "" || txtPeaje.Text == string.Empty)
                throw new Exception("Favor ingrese costo peaje.");
            if (txtDepre.Text == "" || txtDepre.Text == string.Empty)
                throw new Exception("Favor ingrese costo de depreciación.");
            if (txtHospedaje.Text == "" || txtHospedaje.Text == string.Empty)
                throw new Exception("Favor ingrese costo de hospedaje.");
            if (txtAlimento.Text == "" || txtAlimento.Text == string.Empty)
                throw new Exception("Favor ingrese costo de alimento.");
            if (txtCABelice.Text == "" || txtCABelice.Text == string.Empty)
                throw new Exception("Favor ingrese monto establecido para viajes en Centro America y Belice.");
            if (txtAreaDolar.Text == "" || txtAreaDolar.Text == string.Empty)
                throw new Exception("Favor ingrese monto establecido para viajes en países área de dolar.");
            if (txtAreaNoDolar.Text == "" || txtAreaNoDolar.Text == string.Empty)
                throw new Exception("Favor ingrese monto establecido para viajes en países área no dolar.");
        }

        protected void btnModCostos_Click(object sender, EventArgs e)
        {
            try
            {
                validar();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openModal();", true);
            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void DDLCategoria_TextChanged(object sender, EventArgs e)
        {
            cargarCostos();
        }

        protected void DDLTipoViaje_TextChanged(object sender, EventArgs e)
        {
            cargarCostos();
            if (DDLTipoViaje.SelectedValue == "2")
            {
                LBMonedaCirculacion.InnerText = "Circulación $";
                LBMonedaTransporte.InnerText = "Transporte $";
                LBMonedaPeaje.InnerText = "Peaje $";
                LBMonedaHospedaje.InnerText = "Hospedaje $";
                LBMonedaDepre.InnerText = "Depreciación $";
                LBMonedaAlimento.InnerText = "Alimento $";
                LBMonedaCAB.InnerText = "Centro América y Belice $";
                LBMonedaPaisDolar.InnerText = "Países área dolar $";
                LBPaisNoDolar.InnerText = "Países área no dolar $";
                DIVInternacional.Visible = true;
            }
            else if (DDLTipoViaje.SelectedValue == "1")
            {
                LBMonedaCirculacion.InnerText = "Circulación L.";
                LBMonedaTransporte.InnerText = "Transporte L.";
                LBMonedaPeaje.InnerText = "Peaje L.";
                LBMonedaHospedaje.InnerText = "Hospedaje L.";
                LBMonedaDepre.InnerText = "Depreciación L.";
                LBMonedaAlimento.InnerText = "Alimento L.";
                LBMonedaCAB.InnerText = "Centro América y Belice L.";
                LBMonedaPaisDolar.InnerText = "Países área dolar L.";
                LBPaisNoDolar.InnerText = "Países área no dolar L.";
                DIVInternacional.Visible = false;
            }
            else
            {
                DIVInternacional.Visible = false;
                limpiarMoneda();

            }
            UPCostoTotal.Update();
        }
        void limpiar()
        {           
            txtCirculacion.Text = "";
            txtTransporte.Text = "";
            txtPeaje.Text = "";
            txtHospedaje.Text = "";
            txtDepre.Text = "";
            txtAlimento.Text = "";
            txtCABelice.Text = "";
            txtAreaDolar.Text = "";
            txtAreaNoDolar.Text = "";
            DIVInternacional.Visible = true;
        }
        void limpiarMoneda()
        {
            LBMonedaCirculacion.InnerText = "Circulación";
            LBMonedaTransporte.InnerText = "Transporte";
            LBMonedaPeaje.InnerText = "Peaje";
            LBMonedaHospedaje.InnerText = "Hospedaje";
            LBMonedaDepre.InnerText = "Depreciación";
            LBMonedaAlimento.InnerText = "Alimento";
            LBMonedaCAB.InnerText = "Centro América y Belice";
            LBMonedaPaisDolar.InnerText = "Países área dolar";
            LBPaisNoDolar.InnerText = "Países área no dolar";
        }

        protected void btnModalCrear_Click(object sender, EventArgs e)
        {
            string vQuery = "VIATICOS_Costos 4, '" + DDLCategoria.SelectedValue + "','" + DDLTipoViaje.SelectedValue + "','"+txtCirculacion.Text+"'," +
                "'"+txtTransporte.Text+"','"+txtPeaje.Text+"','"+txtAlimento.Text+"','"+txtDepre.Text+"','"+txtCABelice.Text+"','"+txtAreaDolar.Text+"'," +
                "'"+txtAreaNoDolar.Text+"','"+txtHospedaje.Text+"'";
            DataTable vDatos = vConexion.obtenerDataTable(vQuery);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal();", true);
            limpiar();
            limpiarMoneda();
            DDLCategoria.SelectedValue = "0";
            DDLTipoViaje.SelectedValue = "0";
            UPCatTipoV.Update();
            UPCostoTotal.Update();
            Mensaje("Costos modificados con éxito.",WarningType.Success);
        }

        protected void btnModalCerrar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "closeModal();", true);
        }
    }
}