using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;

namespace BiometricoWeb.pages
{
    public partial class constancias : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                    if (vDatos.Rows[0]["tipoEmpleado"].ToString().Equals("1")) { 
                        ConstanciasGenerales.Visible = true;
                        Busquedas.Visible = true;
                    }
                    cargarDatos();
                    Session["CONTEO"] = null;
                    
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

                vQuery = "[RSP_Constancias] 16";
                vDatos = vConexion.obtenerDataTable(vQuery);

                DDLFirmante.Items.Clear();
                foreach (DataRow item in vDatos.Rows){
                    DDLFirmante.Items.Add(new ListItem { Value = item["codigoSAP"].ToString(), Text = item["nombre"].ToString() });
                }

                // MIS SOLICITUDES
                vQuery = "[RSP_Constancias] 8," + Session["USUARIO"].ToString();
                vDatos = vConexion.obtenerDataTable(vQuery);
                GvMisConstancias.DataSource = vDatos;
                GvMisConstancias.DataBind();
                Session["CONSTANCIAS_EMPLEADO"] = vDatos;

                // SOLICITUDES PENDIENTES
                vQuery = "[RSP_Constancias] 9";
                vDatos = vConexion.obtenerDataTable(vQuery);
                GVBusqueda.DataSource = vDatos;
                GVBusqueda.DataBind();
                Session["CONSTANCIAS_GENERAL"] = vDatos;

                //BUSQUEDA 
                vQuery = "[RSP_Constancias] 11";
                vDatos = vConexion.obtenerDataTable(vQuery);

                DDLEstadoConstancia.Items.Clear();
                DDLEstadoConstancia.Items.Add(new ListItem { Value = "-1", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLEstadoConstancia.Items.Add(new ListItem { Value = item["idEstado"].ToString(), Text = item["nombre"].ToString() });
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        public void MensajeLoad(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", " document.addEventListener(\"DOMContentLoaded\", function (event) { infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "'); });", true);
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
                validarDatos();

                String vTipo = DDLTipoConstancia.SelectedValue;
                String vQuery = "";
                DataTable vDatos = new DataTable();
                String vCat = DDLCategoria.SelectedValue == "0" ? "null" : DDLCategoria.SelectedValue;
                String vDest = DDLDestinoCL.SelectedValue == "" ? "null" : DDLDestinoCL.SelectedValue;
                int vInfo = 0;

                vQuery = "[RSP_Constancias] 2" +
                    "," + Session["USUARIO"].ToString() +
                    "," + vTipo +
                    "," + vCat +
                    "," + vDest;
                vDatos = vConexion.obtenerDataTable(vQuery);

                if (vTipo == "2"){
                    if (vDatos.Rows.Count > 0){
                        String vIdSolicitud = vDatos.Rows[0][0].ToString();
                        // xml
                        xml vMaestro = new xml();
                        Object[] vDatosMaestro = new object[8];
                        vDatosMaestro[0] = TxDest1.Text;
                        vDatosMaestro[1] = TxMont1.Text;
                        vDatosMaestro[2] = TxDest2.Text;
                        vDatosMaestro[3] = TxMont2.Text;
                        vDatosMaestro[4] = TxDest3.Text;
                        vDatosMaestro[5] = TxMont3.Text;
                        vDatosMaestro[6] = TxDest4.Text;
                        vDatosMaestro[7] = TxMont4.Text;
                        String vXML = vMaestro.ObtenerMaestroString(vDatosMaestro);
                        vXML = vXML.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");

                        vQuery = "[RSP_Constancias] 3" +
                                "," + vIdSolicitud +
                                ",'" + TxMonto.Text + "'" +
                                ",'" + TxPlazo.Text + "'" +
                                ",'" + vXML + "'";
                        vInfo = vConexion.ejecutarSql(vQuery);

                        if (vInfo == 1){
                            Mensaje("Solicitud creada con éxito.", WarningType.Success);
                        }else
                            Mensaje("Solicitud no completada, favor comuníquese con sistemas.", WarningType.Success);
                        limpiarFormulario();
                    }

                }else{
                    if (vDatos.Rows.Count > 0){
                        Boolean vDA = true;
                        String vIdSolicitud = vDatos.Rows[0][0].ToString();
                        if (vDest == "6") {
                            vQuery = "[RSP_Constancias] 4" +
                                "," + vIdSolicitud +
                                ",'" + TxAval.Text + "'" +
                                ",'" + DDLParentezco.SelectedItem + "'";
                            vInfo = vConexion.ejecutarSql(vQuery);
                        }else if (vDest == "11") {
                            vQuery = "[RSP_Constancias] 5" +
                                "," + vIdSolicitud +
                                ",'" + TxEmbajada.Text + "'" +
                                ",'" + TxFechaCita.Text + "'";
                            vInfo = vConexion.ejecutarSql(vQuery);
                        }else if (vDest == "12") {
                            

                            vQuery = "[RSP_Constancias] 6" +
                                "," + vIdSolicitud +
                                ",'" + DDLFirmante.SelectedValue + "'" +
                                ",'" + TxFecha.Text + "'" +
                                ",'" + TxPasaporte.Text + "'" +
                                ",'" + TxRTN.Text + "'" +
                                ",'" + TxDomicilio1.Text + "'" +
                                ",'" + TxDomicilio2.Text + "'" +
                                ",'" + TxContacto.Text + "'" +
                                ",'" + TxLugar.Text + "'" +
                                ",'" + TxTelefono.Text + "'" +
                                ",'" + TxEvento.Text + "'" +
                                ",'" + TxFechaInicio.Text + "'" +
                                ",'" + TxFechaFin.Text + "'" +
                                ",'" + TxConsulado.Text + "'" +
                                ",'" + TxDirConsul.Text + "'" +
                                ",'" + TxPais.Text + "'" +
                                ",'" + TxCiudad.Text + "'";
                            vInfo = vConexion.ejecutarSql(vQuery);
                            
                        }else
                            vDA = false;

                        if (vDA){
                            if (vInfo == 1){
                                MensajeLoad("Constancia solicitada con éxito.", WarningType.Success);
                            }else
                                MensajeLoad("Solicitud no completada, favor comuníquese con sistemas.", WarningType.Success);
                        }else
                            MensajeLoad("Constancia solicitada con éxito.", WarningType.Success);
                    }else
                        MensajeLoad("Solicitud no completada, favor comuníquese con sistemas.", WarningType.Warning);
                    limpiarFormulario();
                }
                cargarDatos();
                UPBuzonGeneral.Update();
                UpdatePanel1.Update();
            }catch (Exception ex){
                MensajeLoad(ex.Message, WarningType.Danger);
            }
        }
        
        private void validarDatos() {
            if (DDLTipoConstancia.SelectedValue == "0")
                throw new Exception("Favor seleccione el tipo de constancia.");

            if (DDLTipoConstancia.SelectedValue == "2"){
                if (TxMonto.Text == "" || TxMonto.Text == string.Empty)
                    throw new Exception("Favor ingrese el monto.");
                if (TxPlazo.Text == "" || TxPlazo.Text == string.Empty)
                    throw new Exception("Favor ingrese el plazo.");
                if (TxDest1.Text == "" || TxDest1.Text == string.Empty)
                    throw new Exception("Favor ingrese al menos un destino.");
                if (TxMont1.Text == "" || TxMont1.Text == string.Empty)
                    throw new Exception("Favor ingrese al menos un monto.");
            }
            else{
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
                    if (TxFechaCita.Text == "" || TxFechaCita.Text == string.Empty)
                        throw new Exception("Favor ingrese la fecha de la cita.");
                }

                if (DDLDestinoCL.SelectedValue == "12") {
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
                    if (TxPais.Text == "" || TxPais.Text == string.Empty)
                        throw new Exception("Favor ingrese el país.");
                    if (TxDirConsul.Text == "" || TxDirConsul.Text == string.Empty)
                        throw new Exception("Favor ingrese la dirección del consulado.");
                }
            }
        }

        protected void GvMisConstancias_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                String vId = e.CommandArgument.ToString();
                Session["CONSTANCIA_ID"] = vId;
                if (e.CommandName == "EliminarMensaje"){
                    LbTitulo.Text = "Eliminar Solicitud " + vId;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
                if (e.CommandName == "DescargarInfo"){
                    DataTable vDatos = (DataTable)Session["CONSTANCIAS_EMPLEADO"];
                    for (int i = 0; i < vDatos.Rows.Count; i++){
                        if (vDatos.Rows[i]["idSolicitud"].ToString() == vId && vDatos.Rows[i]["Tipo"].ToString() != "Laboral" && vDatos.Rows[i]["Estado"].ToString() == "Aprobado"){
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openDescarga();", true);
                        }
                    }
                }
            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void GvMisConstancias_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GvMisConstancias.PageIndex = e.NewPageIndex;
                GvMisConstancias.DataSource = (DataTable)Session["CONSTANCIAS_EMPLEADO"];
                GvMisConstancias.DataBind();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                String vId = e.CommandArgument.ToString();
                Session["SOLICITUD_RESPUESTA"] = vId;
                Session["CONSTANCIA_ID"] = null;
                if (e.CommandName == "ResponderSolicitud"){
                    Session["CONSTANCIA_ELIMINAR"] = null;
                    LbTitulo.Text = "Responder Solicitud " + vId;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }

                if (e.CommandName == "VerSolicitud"){
                    DataTable vDatos = (DataTable)Session["CONSTANCIAS_GENERAL"];
                    DataTable vDataFiltro = new DataTable();
                    vDataFiltro.Columns.Add("Tipo");
                    vDataFiltro.Columns.Add("Categoria");
                    vDataFiltro.Columns.Add("Destino");

                    DivModFinanc.Visible = false;
                    DivModAval.Visible = false;
                    DivModEmbajada.Visible = false;
                    DivModCapa.Visible = false;
                    EnumerableRowCollection<DataRow> filtered = vDatos.AsEnumerable()
                        .Where(r => r.Field<Int32>("idSolicitud").Equals(Convert.ToInt32(vId)));

                    foreach (DataRow item in filtered){
                        vDataFiltro.Rows.Add(
                            item["Tipo"].ToString(),
                            item["Categoria"].ToString(),
                            item["Destino"].ToString()
                        );
                    }

                    Boolean vFlag = true;
                    if (vDataFiltro.Rows[0]["Tipo"].ToString() == "Financiamiento")
                        DivModFinanc.Visible = true;
                    else if (vDataFiltro.Rows[0]["Destino"].ToString() == "Aval")
                        DivModAval.Visible = true;
                    else if (vDataFiltro.Rows[0]["Destino"].ToString() == "Visa Embajada")
                        DivModEmbajada.Visible = true;
                    else if (vDataFiltro.Rows[0]["Destino"].ToString() == "Visa para capacitacion")
                        DivModCapa.Visible = true;
                    else
                        vFlag = false;

                    if (vFlag){
                        verInfo(vId, vDatos);
                        LbTituloInfo.Text = "Información de Solicitud " + vId;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openInfo();", true);
                    }
                }

                if (e.CommandName == "EliminarSolicitud"){
                    LbTitulo.Text = "Eliminar Solicitud " + vId;
                    Session["CONSTANCIA_ELIMINAR"] = vId;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        private void verInfo(String vId, DataTable vDatos) {
            for (int i = 0; i < vDatos.Rows.Count; i++){
                if (vDatos.Rows[i]["idSolicitud"].ToString() == vId){
                    String vResultado = "";
                    if (vDatos.Rows[i]["Tipo"].ToString() == "Financiamiento"){
                        String vsDest = vDatos.Rows[i]["financDestino"].ToString();
                        if (vsDest != "" || vsDest != string.Empty){
                            DataTable vData = stam(vsDest);
                            for (int j = 0; j < vData.Rows.Count; j++){
                                String vDest1 = vData.Rows[j]["Destino1"].ToString();
                                String vMont1 = vData.Rows[j]["Monto1"].ToString();
                                String vDest2 = vData.Rows[j]["Destino2"].ToString();
                                String vMont2 = vData.Rows[j]["Monto2"].ToString();
                                String vDest3 = vData.Rows[j]["Destino3"].ToString();
                                String vMont3 = vData.Rows[j]["Monto3"].ToString();
                                String vDest4 = vData.Rows[j]["Destino4"].ToString();
                                String vMont4 = vData.Rows[j]["Monto4"].ToString();

                                vMont1 = vMont1.Substring(vMont1.Length -3 , 1) == "." ? vMont1 : vMont1 + ".00";
                                if (vMont2 != "")
                                    vMont2 = vMont2.Substring(vMont2.Length - 3, 1) == "." ? vMont2 : vMont2 + ".00";
                                if (vMont3 != "")
                                    vMont3 = vMont3.Substring(vMont3.Length - 3, 1) == "." ? vMont3 : vMont3 + ".00";
                                if (vMont4 != "")
                                    vMont4 = vMont4.Substring(vMont4.Length - 3, 1) == "." ? vMont4 : vMont4 + ".00";

                                vResultado = vDest1 + " " + vMont1 + ", " + vDest2 + " " + vMont2 + ", " + vDest3 + " " + vMont3 + ", " + vDest4 + " " + vMont4;
                                vResultado = vResultado.TrimEnd();
                                vResultado = vResultado.Replace(",  ,  ,", "");
                                vResultado = vResultado.Replace(",  ,", "");
                            }
                            String NewString = vResultado.Substring(vResultado.Length - 1, 1);
                            if (NewString == ",")
                                vResultado = vResultado.Remove(vResultado.Length - 1, 1);
                        }
                    }

                    LbMonto.Text = vDatos.Rows[i]["financMonto"].ToString();
                    LbPlazo.Text = vDatos.Rows[i]["financPlazo"].ToString() + " meses";
                    LbDetalle.Text = vResultado;
                    LbAval.Text = vDatos.Rows[i]["avalNombre"].ToString();
                    LbParentezco.Text = vDatos.Rows[i]["avalParentezco"].ToString();
                    LbEmbajada.Text = vDatos.Rows[i]["embajadaNombre"].ToString();
                    LbFechaCita.Text = vDatos.Rows[i]["embajadaFecha"].ToString();

                    LbRepresentante.Text = vDatos.Rows[i]["representante"].ToString();
                    LbEmision.Text = vDatos.Rows[i]["fechaEmision"].ToString();
                    LbPasaporte.Text = vDatos.Rows[i]["pasaporte"].ToString();
                    LbRTN.Text = vDatos.Rows[i]["rtn"].ToString();
                    LbDomicilio1.Text = vDatos.Rows[i]["domicilio1"].ToString();
                    LbDomicilio2.Text = vDatos.Rows[i]["domicilio2"].ToString();
                    LbContacto.Text = vDatos.Rows[i]["contacto"].ToString();
                    LbLugar.Text = vDatos.Rows[i]["lugar"].ToString();
                    LbTelefono.Text = vDatos.Rows[i]["telefono"].ToString();
                    LbEvento.Text = vDatos.Rows[i]["eventoParticipacion"].ToString();
                    LbInicio.Text = vDatos.Rows[i]["fechaInicio"].ToString();
                    LbFin.Text = vDatos.Rows[i]["fechaFin"].ToString();
                    LbConsulado.Text = vDatos.Rows[i]["consulado"].ToString();
                    LbDirConsul.Text = vDatos.Rows[i]["direccionConsulado"].ToString();
                    break;
                }
            }
        }

        public DataTable stam(String vXml){
            StringReader theReader = new StringReader(vXml);
            DataSet theDataSet = new DataSet();
            theDataSet.ReadXml(theReader);
            return theDataSet.Tables[0];
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GVBusqueda.PageIndex = e.NewPageIndex;
                GVBusqueda.DataSource = (DataTable)Session["CONSTANCIAS_GENERAL"];
                GVBusqueda.DataBind();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
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
            
            //financ
            TxDestino.Text = string.Empty;
            TxMonto.Text = string.Empty;
            TxPlazo.Text = string.Empty;
            
            //visa
            TxEmbajada.Text = string.Empty;
            TxFechaCita.Text = string.Empty;

            //aval
            TxAval.Text = string.Empty;
            DDLParentezco.SelectedValue = "0";

            //visa capacitacion
            TxFecha.Text = string.Empty;
            TxPasaporte.Text = string.Empty;
            TxRTN.Text = string.Empty;
            TxDomicilio1.Text = string.Empty;
            TxDomicilio2.Text = string.Empty;
            TxContacto.Text = string.Empty;
            TxLugar.Text = string.Empty;
            TxCiudad.Text = string.Empty;
            TxTelefono.Text = string.Empty;
            TxEvento.Text = string.Empty;
            TxFechaInicio.Text = string.Empty;
            TxFechaFin.Text = string.Empty;
            TxConsulado.Text = string.Empty;
            TxDirConsul.Text = string.Empty;
        }

        protected void GvMisConstancias_RowDataBound(object sender, GridViewRowEventArgs e){
            try{
                if (e.Row.RowType == DataControlRowType.DataRow){
                    DataRowView drv = e.Row.DataItem as DataRowView;

                    TableCell vCel = new TableCell();
                    if (drv["estado"].ToString().Equals("Aprobado")) {
                        if (drv["Tipo"].ToString().Equals("Financiamiento")){
                            LinkButton LbDescarga = e.Row.FindControl("BtnDownload") as LinkButton;
                            LbDescarga.CssClass = "btn btn-success";
                        }
                        e.Row.Attributes.CssStyle.Value = "color : LimeGreen; font-weight: bold;"; //System.Drawing.Color.LimeGreen;
                    }
                }
            }catch (Exception Ex){
                throw new Exception(Ex.Message);
            }
        }

        protected void BtnConfirmar_Click(object sender, EventArgs e){
            try{
                DataTable vDatos = (DataTable)Session["CONSTANCIAS_GENERAL"];

                if (HttpContext.Current.Session["CONSTANCIA_ELIMINAR"] == null){
                    String vId = HttpContext.Current.Session["CONSTANCIA_ID"] != null ? Session["CONSTANCIA_ID"].ToString() : Session["SOLICITUD_RESPUESTA"].ToString();
                    String vEstado = HttpContext.Current.Session["CONSTANCIA_ID"] != null ? "2" : "1";
                    String vMensaje = vEstado == "2" ? "Solicitud eliminada con éxito." : "Constancia aprobada con éxito.";
                    String vQuery = "[RSP_Constancias] 10" +
                                    "," + vId + "," + vEstado;
                    int vInfo = vConexion.ejecutarSql(vQuery);
                    if (vInfo == 1){
                        for (int i = 0; i < vDatos.Rows.Count; i++){
                            if (vDatos.Rows[i]["idSolicitud"].ToString() == vId && vDatos.Rows[i]["Destino"].ToString() == "Visa para capacitacion"){
                                CargarConstancia(vDatos, i);
                            }
                        }

                        Mensaje(vMensaje, WarningType.Success);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);
                    }else
                        Mensaje("Solicitud no fue eliminada, comuníquese con sistemas.", WarningType.Danger);
                }else{
                    String vId = Session["CONSTANCIA_ELIMINAR"].ToString();
                    String vQuery = "[RSP_Constancias] 10," + vId + ",4";
                    int vInfo = vConexion.ejecutarSql(vQuery);
                    if (vInfo == 1){
                        Mensaje("Solicitud eliminada con éxito.", WarningType.Success);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);
                    }else
                        Mensaje("Solicitud no fue eliminada, comuníquese con sistemas.", WarningType.Danger);
                }
                cargarDatos();
                UpdatePanel1.Update();
                UPBuzonGeneral.Update();
            }catch (Exception Ex){
                ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + Ex.Message + "','" + WarningType.Danger.ToString().ToLower() + "')", true);
            }finally { CerrarModal("ModalConfirmar"); }
        }

        protected void BtnAgregar_Click(object sender, EventArgs e){
            try{
                int vCuenta = 0;
                Line2.Visible = false;
                Line3.Visible = false;
                Line4.Visible = false;

                if (HttpContext.Current.Session["CONTEO"] != null){
                    Line2.Visible = true;
                    vCuenta = Convert.ToInt32(Session["CONTEO"].ToString());
                    if (vCuenta == 1)
                        Line3.Visible = true;
                    else if( vCuenta > 1) { 
                        Line3.Visible = true;
                        Line4.Visible = true;
                    }
                }else{
                    Line2.Visible = true;
                }
                
                Session["CONTEO"] = vCuenta + 1;
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GVHistorico_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GVHistorico.PageIndex = e.NewPageIndex;
                GVHistorico.DataSource = (DataTable)Session["CONSTANCIA_HISTORICO"];
                GVHistorico.DataBind();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void DDLEstadoConstancia_SelectedIndexChanged(object sender, EventArgs e){
            try{
                if (DDLEstadoConstancia.SelectedValue == "-1")
                    throw new Exception("Favor seleccione el estado de la constancia.");

                String vQuery = "[RSP_Constancias] 12," + DDLEstadoConstancia.SelectedValue;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                GVHistorico.DataSource = null;

                GVHistorico.DataSource = vDatos;
                GVHistorico.DataBind();
                Session["CONSTANCIA_HISTORICO"] = vDatos;
            }catch (Exception ex){

            }
        }

        protected void GVHistorico_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                String vId = e.CommandArgument.ToString();
                Session["SOLICITUD_RESPUESTA"] = vId;
                Session["CONSTANCIA_ID"] = null;

                if (e.CommandName == "VerSolicitudHistorial"){
                    DataTable vDatos = (DataTable)Session["CONSTANCIA_HISTORICO"];
                    if (vDatos.Rows.Count > 0){
                        DataTable vDataFiltro = new DataTable();
                        vDataFiltro.Columns.Add("Tipo");
                        vDataFiltro.Columns.Add("Categoria");
                        vDataFiltro.Columns.Add("Destino");

                        DivModFinanc.Visible = false;
                        DivModAval.Visible = false;
                        DivModEmbajada.Visible = false;
                        DivModCapa.Visible = false;
                        EnumerableRowCollection<DataRow> filtered = vDatos.AsEnumerable()
                            .Where(r => r.Field<Int32>("idSolicitud").Equals(Convert.ToInt32(vId)));

                        foreach (DataRow item in filtered){
                            vDataFiltro.Rows.Add(
                                item["Tipo"].ToString(),
                                item["Categoria"].ToString(),
                                item["Destino"].ToString()

                                );
                        }

                        Boolean vFlag = true;
                        if (vDataFiltro.Rows[0]["Tipo"].ToString() == "Financiamiento")
                            DivModFinanc.Visible = true;
                        else if (vDataFiltro.Rows[0]["Destino"].ToString() == "Aval")
                            DivModAval.Visible = true;
                        else if (vDataFiltro.Rows[0]["Destino"].ToString() == "Visa Embajada")
                            DivModEmbajada.Visible = true;
                        else if (vDataFiltro.Rows[0]["Destino"].ToString() == "Visa para capacitacion")
                            DivModCapa.Visible = true;
                        else
                            vFlag = false;

                        if (vFlag){
                            verInfo(vId, vDatos);
                            LbTituloInfo.Text = "Información de Solicitud " + vId;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openInfo();", true);
                        }
                    }
                }
            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void BtnDescargarArchivo_Click(object sender, EventArgs e){
            try{
                String vDocumento = "InformacionFinanciamiento.pdf";
                Response.ContentType = "application/pdf";  
                Response.AddHeader("Content-disposition", "attachment;filename=" + vDocumento);
                Response.TransmitFile(Server.MapPath("plantilla/" + vDocumento));
                Response.Flush();
                Response.Close();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeDescarga();", true);

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        void CargarConstancia(DataTable vDatos, int i){
            try{
                SapConnector vTest = new SapConnector();
                byte[] vResultado = null;
                String vInicio = Convert.ToDateTime(vDatos.Rows[i]["fechaInicio"]).ToString("yyyy-MM-dd");
                String vFin = Convert.ToDateTime(vDatos.Rows[i]["fechaFin"]).ToString("yyyy-MM-dd");
                String vFEmision = Convert.ToDateTime(vDatos.Rows[i]["fechaEmision"]).ToString("yyyy-MM-dd");
                String vPDF = vTest.getConstancias(vDatos.Rows[i]["ciudad"].ToString(),
                    vDatos.Rows[i]["consulado"].ToString(), 
                    vDatos.Rows[i]["contacto"].ToString(), 
                    vDatos.Rows[i]["direccionConsulado"].ToString(), 
                    vDatos.Rows[i]["domicilio1"].ToString(), 
                    vDatos.Rows[i]["domicilio2"].ToString(),
                    vFEmision, 
                    vInicio, 
                    vFin,
                    vDatos.Rows[i]["lugar"].ToString(),
                    vDatos.Rows[i]["pais"].ToString(),
                    vDatos.Rows[i]["pasaporte"].ToString(),
                    vDatos.Rows[i]["codigoSAP"].ToString(),
                    vDatos.Rows[i]["rtn"].ToString(),
                    vDatos.Rows[i]["eventoParticipacion"].ToString(),
                    vDatos.Rows[i]["telefono"].ToString(),
                    vDatos.Rows[i]["representante"].ToString(),
                    ref vResultado);

                if (vPDF.Equals("Código SAP incorrecto")){
                    MensajeLoad(vPDF, WarningType.Danger);
                }else {
                    byte[] fileData = vResultado;
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    GetExtension(".pdf");
                    byte[] bytFile = fileData;
                    Response.OutputStream.Write(bytFile, 0, bytFile.Length);
                    Response.AddHeader("Content-disposition", "attachment;filename=" + "Constancia.pdf");
                    Response.End();
                    //Response.Close();
                    //Response.SuppressContent = true;
                    //HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }finally { CerrarModal("ModalConfirmar"); }
        }

        public void CerrarModal(String vModal){
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#" + vModal + "').modal('hide');", true);
        }

        private string GetExtension(string Extension)
        {
            switch (Extension)
            {
                case ".doc":
                    return "application/ms-word";
                case ".xls":
                    return "application/vnd.ms-excel";
                case ".ppt":
                    return "application/mspowerpoint";
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".zip":
                    return "application/zip";
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case ".wav":
                    return "audio/wav";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                    return "application/xml";
                default:
                    return "application/octet-stream";
            }
        }
    }
}