using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace BiometricoWeb.pages
{
    public partial class constancias : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    cargarDatos();
                }
            }
        }

        private void cargarDatos() {
            try{
                String vQuery = "RSP_Constancias 1, 1";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                DDLTipoConstancia.Items.Clear();
                DDLTipoConstancia.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLTipoConstancia.Items.Add(new ListItem { Value = item["idTipoConstancia"].ToString(), Text = item["nombre"].ToString() });
                }
                vQuery = "RSP_Constancias 7, 1";
                vDatos = vConexion.obtenerDataTable(vQuery);

                DDLCategoria.Items.Clear();
                DDLCategoria.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLCategoria.Items.Add(new ListItem { Value = item["idAgrupacion"].ToString(), Text = item["nombre"].ToString() });
                }

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void DDLTipoConstancia_SelectedIndexChanged(object sender, EventArgs e){
            try{
                if (DDLTipoConstancia.SelectedValue == "2"){
                    limpiarDivs();
                    DDLCategoria.SelectedValue = "0";
                    DDLCategoria.Enabled = false;
                    DivFinanciar.Visible = true;
                    DivDestino.Visible = false;
                }else if (DDLTipoConstancia.SelectedValue == "1"){
                    limpiarDivs();
                    DDLCategoria.Enabled = true;
                }else{
                    DDLCategoria.SelectedValue = "0";
                    DDLCategoria.Enabled = false;
                    DivDestino.Visible = false;
                    limpiarDivs();
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void DDLCategoria_SelectedIndexChanged(object sender, EventArgs e){
            try{
                limpiarDivs();
                DivDestino.Visible = DDLCategoria.SelectedValue != "0" ? true : false;

                if (DDLCategoria.SelectedValue != "" || DDLCategoria.SelectedValue != "0"){
                    String vQuery = "RSP_Constancias 1," + DDLCategoria.SelectedValue;
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                    DDLDestinoCL.Items.Clear();
                    DDLDestinoCL.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLDestinoCL.Items.Add(new ListItem { Value = item["idTipoConstancia"].ToString(), Text = item["idTipoConstancia"].ToString() + " - " + item["nombre"].ToString() });
                    }
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnEnviar_Click(object sender, EventArgs e){
            try{
                limpiarFormulario();

                if (DDLTipoConstancia.SelectedValue == "0")
                    throw new Exception("Favor seleccione el tipo de constancia.");

                String vTipo = DDLTipoConstancia.SelectedValue;
                String vQuery = "";
                DataTable vDatos = new DataTable();
                String vCat = DDLCategoria.SelectedValue;
                String vDest = DDLDestinoCL.SelectedValue;


                if (vTipo == "2"){
                    validarFinanciamiento();
                    vQuery = "[RSP_Constancias] 2" +
                        "," + Session["USUARIO"].ToString() +
                        "," + vTipo + 
                        "," + vCat + 
                        "," + vDest ;
                    vDatos = vConexion.obtenerDataTable(vQuery);

                    if (vDatos.Rows.Count > 0){
                        Mensaje("Solicitud creada con éxito.", WarningType.Success);
                        limpiarFormulario();
                    }

                }else{
                    validarConstancia();
                    String vSPTipo = "";
                    vQuery = "[RSP_Constancias] 2" +
                        "," + Session["USUARIO"].ToString() +
                        "," + vTipo + 
                        "," + vCat + 
                        "," + vDest ;
                    vDatos = vConexion.obtenerDataTable(vQuery);
                    if (vCat == "")
                        vSPTipo = "";

                    if (vDatos.Rows.Count > 0){
                        vQuery = "[RSP_Constancias] ";
                    }

                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        private void validarFinanciamiento() {
            if (TxMonto.Text == "" || TxMonto.Text == string.Empty)
                throw new Exception("Favor ingrese el monto.");
            if (TxPlazo.Text == "" || TxPlazo.Text == string.Empty)
                throw new Exception("Favor ingrese el plazo.");
            if (TxDestino.Text == "" || TxDestino.Text == string.Empty)
                throw new Exception("Favor ingrese el destino.");
        }
        
        private void validarConstancia() {
            if (DDLCategoria.SelectedValue == "0")
                throw new Exception("Favor seleccione el tipo de constancia laboral.");
            if (DDLDestinoCL.SelectedValue == "0")
                throw new Exception("Favor seleccione el destino de la constancia.");

            if (DDLDestinoCL.SelectedValue == "6") {
                if (TxAval.Text == "" || TxAval.Text == string.Empty)
                    throw new Exception("Favor ingrese el nombre del aval.");
                if (DDLParentezco.SelectedValue == "0")
                    throw new Exception("Favor seleccione el parentezco del aval.");
            }

            if (DDLDestinoCL.SelectedValue == "11") {
                if (TxEmbajada.Text == "" || TxEmbajada.Text == string.Empty)
                    throw new Exception("Favor ingrese el nombre de la embajada.");
            }

            if (DDLDestinoCL.SelectedValue == "12") {
                if (TxRepresentante.Text == "" || TxRepresentante.Text == string.Empty)
                    throw new Exception("Favor ingrese el representante.");
                if (TxFecha.Text == "" || TxFecha.Text == string.Empty)
                    throw new Exception("Favor ingrese la Fecha de emisión.");
                if (TxPasaporte.Text == "" || TxPasaporte.Text == string.Empty)
                    throw new Exception("Favor ingrese el pasaporte.");
                if (TxRTN.Text == "" || TxRTN.Text == string.Empty)
                    throw new Exception("Favor ingrese el RTN.");
                if (TxDomicilio1.Text == "" || TxDomicilio1.Text == string.Empty)
                    throw new Exception("Favor ingrese el domicilio 1.");
                if (TxContacto.Text == "" || TxContacto.Text == string.Empty)
                    throw new Exception("Favor ingrese el contacto.");
                if (TxDomicilio2.Text == "" || TxDomicilio2.Text == string.Empty)
                    throw new Exception("Favor ingrese el domicilio 2.");
                if (TxLugar.Text == "" || TxLugar.Text == string.Empty)
                    throw new Exception("Favor ingrese el Lugar.");
                if (TxCiudad.Text == "" || TxCiudad.Text == string.Empty)
                    throw new Exception("Favor ingrese la Ciudad.");
                if (TxTelefono.Text == "" || TxTelefono.Text == string.Empty)
                    throw new Exception("Favor ingrese el teléfono.");
                if (TxFechaInicio.Text == "" || TxFechaInicio.Text == string.Empty)
                    throw new Exception("Favor ingrese la Fecha Inicial.");
                if (TxEvento.Text == "" || TxEvento.Text == string.Empty)
                    throw new Exception("Favor ingrese el evento.");
                if (TxFechaFin.Text == "" || TxFechaFin.Text == string.Empty)
                    throw new Exception("Favor ingrese la Fecha Final.");
                if (TxConsulado.Text == "" || TxConsulado.Text == string.Empty)
                    throw new Exception("Favor ingrese el consulado.");
                if (TxDirConsul.Text == "" || TxDirConsul.Text == string.Empty)
                    throw new Exception("Favor ingrese la dirección del consulado.");
            }
        }

        protected void GvMisConstancias_RowCommand(object sender, GridViewCommandEventArgs e){

        }

        protected void GvMisConstancias_PageIndexChanging(object sender, GridViewPageEventArgs e){

        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e){

        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){

        }

        protected void DDLDestinoCL_SelectedIndexChanged(object sender, EventArgs e){
            try{
                DivAval.Visible = DDLDestinoCL.SelectedValue == "6" ? true : false;
                DivVisa.Visible = DDLDestinoCL.SelectedValue == "12" ? true : false;
                DivEmbajada.Visible = DDLDestinoCL.SelectedValue == "11" ? true : false;
            }
            catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        private void limpiarDivs() {
            DivEmbajada.Visible = false;
            DivFinanciar.Visible = false;
            DivAval.Visible = false;
            DivVisa.Visible = false;
        }
        
        private void limpiarFormulario(){
            DivEmbajada.Visible = false;
            DivFinanciar.Visible = false;
            DivAval.Visible = false;
            DivVisa.Visible = false;
            DivDestino.Visible = false;

            DDLTipoConstancia.SelectedValue = "0";
            DDLCategoria.SelectedValue = "0";
            DDLDestinoCL.SelectedValue = "0";
            DDLParentezco.SelectedValue = "0";

            foreach (TextBox item in Controls){
                if (item is TextBox){
                    item.Controls.Clear();
                }
            }
            /*

            TxMonto.Text = string.Empty;
            TxAval.Text = string.Empty;
            TxCiudad.Text = string.Empty;
            TxConsulado.Text = string.Empty;
            TxContacto.Text = string.Empty;
            TxDestino.Text = string.Empty;
            TxDirConsul.Text = string.Empty;
            TxDomicilio1.Text = string.Empty;
            TxDomicilio2.Text = string.Empty;
            TxEmbajada.Text = string.Empty;
            TxEvento.Text = string.Empty;
            TxFecha.Text = string.Empty;
            TxFechaFin.Text = string.Empty;
            TxFechaInicio.Text = string.Empty;
            TxLugar.Text = string.Empty;
            TxMonto.Text = string.Empty;
            TxPasaporte.Text = string.Empty;
            TxPlazo.Text = string.Empty;
            TxFechaFin.Text = string.Empty;
            */
        }
    }
}