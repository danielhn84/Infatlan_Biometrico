using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BiometricoWeb.pages
{
    public partial class puestos : System.Web.UI.Page
    {
        db vConexion;
        Boolean vArchivoDescrip = false;
        protected void Page_Load(object sender, EventArgs e){
            vConexion = new db();
            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    generales vGenerales = new generales();
                    DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                    if (!vGenerales.PermisosRecursosHumanos(vDatos))
                        Response.Redirect("/default.aspx");

                    HFSubirPDF.Value = string.Empty;

                    CargarPuesto();
                    
                }
            }
        }

        void CargarPuesto() {
            try{
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerGenerales 9");

                GVBusqueda.DataSource = vDatos;
                GVBusqueda.DataBind();
                Session["DATAPUESTOS"] = vDatos;

                vDatos = vConexion.obtenerDataTable("RSP_ObtenerGenerales 12");
                DDLDepto.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLDepto.Items.Add(new ListItem { Value = item["idDepartamento"].ToString(), Text = item["nombre"].ToString() });
                }

            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GVBusqueda.PageIndex = e.NewPageIndex;
                GVBusqueda.DataSource = (DataTable)Session["DATAPUESTOS"];
                GVBusqueda.DataBind();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                string vIdPuesto = e.CommandArgument.ToString();
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerGenerales 11,'" + vIdPuesto + "'");

                if (e.CommandName == "PuestoModificar") {
                    LbModPuesto.Text = vIdPuesto;
                    Session["ACCION"] = "1";
                    DivEstado.Visible = true;
                    TxIdPuesto.ReadOnly = true;

                    foreach (DataRow item in vDatos.Rows) {
                        TxIdPuesto.Text = item["idPuesto"].ToString();
                        TxPuesto.Text = item["nombre"].ToString();
                        DDLDepto.SelectedValue = (item["idDepartamento"].ToString() == "" ? "0" : item["idDepartamento"].ToString());
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }else if(e.CommandName == "DescriptorPuesto") {

                    DataTable vDatosDescrip = new DataTable();
                    vDatosDescrip = vConexion.obtenerDataTable("RSP_DescriptorPuestos 5,'" + vIdPuesto + "'");
                    
                    LbSubir.Text = vIdPuesto;

                    if (vDatosDescrip.Rows.Count.ToString()!="0")
                    {
                        DivAlertaDescriptor.Visible = true;
                        LbAlertaDescriptor.Text = "Puesto " + vDatos.Rows[0]["nombre"].ToString() + " ya contiene archivo. " +
                            "<br>Si desea cambiarlo, ingrese uno nuevo.";
                        

                    }
                    else
                    {
                        DivAlertaDescriptor.Visible = false;
                    }
                   
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDescriptor();", true);
                    
                }

            }
            catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void TxBuscarPuesto_TextChanged(object sender, EventArgs e){
            try{
                CargarPuesto();

                String vBusqueda = TxBuscarPuesto.Text;
                DataTable vDatos = (DataTable)Session["DATAPUESTOS"];

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
                    vDatosFiltrados.Columns.Add("idPuesto");
                    vDatosFiltrados.Columns.Add("nombre");
                    vDatosFiltrados.Columns.Add("Departamento");

                    foreach (DataRow item in filtered){
                        vDatosFiltrados.Rows.Add(
                            item["idPuesto"].ToString(),
                            item["nombre"].ToString(),
                            item["Departamento"].ToString()
                            );
                    }

                    GVBusqueda.DataSource = vDatosFiltrados;
                    GVBusqueda.DataBind();
                    Session["DATAPUESTOS"] = vDatosFiltrados;
                    //UpdateGridView.Update();
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnCrear_Click(object sender, EventArgs e){
            try{
                if (Session["ACCION"].ToString() == "1"){ //MODIFICAR
                    String vQuery = "[RSP_IngresaMantenimientos] 3, '" + TxIdPuesto.Text + "','" + TxPuesto.Text.ToUpper() + "','" + DDLDepto.SelectedValue + "'," + DDLEstado.SelectedValue;
                    Int32 vInformacion = vConexion.ejecutarSql(vQuery);
                    if (vInformacion == 1){
                        Mensaje("Actualizado con Exito!", WarningType.Success);
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "$('#PuestosModal').modal('hide');", true);
                    }
                }else if (Session["ACCION"].ToString() == "2"){ //CREAR 
                    validaPuesto();
                    String vQuery = "[RSP_IngresaMantenimientos] 1, '" + TxIdPuesto.Text + "','" + TxPuesto.Text.ToUpper() + "','" + DDLDepto.SelectedValue + "',0";
                    Int32 vInformacion = vConexion.ejecutarSql(vQuery);

                    if (vInformacion == 1){
                        Mensaje("Creado con Exito!", WarningType.Success);
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "$('#PuestosModal').modal('hide');", true);
                    }
                }
                CargarPuesto();
                limpiarModal();
            }
            catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        void validaPuesto() {
            String vQuery = "[RSP_ObtenerGenerales] 11, '" + TxIdPuesto.Text + "'";
            DataTable vData = vConexion.obtenerDataTable(vQuery);

            if (vData.Rows.Count > 0)
                throw new Exception("El Id del puesto ya existe. Ingrese otro");
            if (TxIdPuesto.Text == "" || TxIdPuesto.Text == String.Empty)
                throw new Exception("Ingrese un Id de puesto");
            if (DDLDepto.SelectedValue == "")
                throw new Exception("Seleccione un Sub Departamento");
            if (TxPuesto.Text == "" || TxPuesto.Text == String.Empty)
                throw new Exception("Ingrese el Nombre del puesto");
        }

        void limpiarModal() {
            TxIdPuesto.Text = string.Empty;
            TxPuesto.Text = string.Empty;
            DDLDepto.SelectedValue = "0";
        }

        protected void btnNuevo_Click(object sender, EventArgs e){
            limpiarModal();
            DivEstado.Visible = false;
            TxIdPuesto.ReadOnly = false;
            Session["ACCION"] = "2";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Pop", "openModal();", true);
        }

        protected void BtnSubirDescriptor_Click(object sender, EventArgs e){

            try
            {
                //IMAGENES1
                String vNombreDepot = String.Empty;
                HttpPostedFile bufferDepositoT = FUSubirPDF.PostedFile;
                if (!FUSubirPDF.HasFile)
                {
                    //DivAlertaDescriptor.Visible = true;
                    //LbAlertaDescriptor.Text = "Favor ingresar archivo.";
                   // ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','Favor ingresar archivo.','" + WarningType.Danger.ToString().ToLower() + "')", true);
                }
                else
                {

                    byte[] vFileDeposito = null;
                    string vExtension = string.Empty;

                    if (bufferDepositoT != null)
                    {
                        vNombreDepot = FUSubirPDF.FileName;

                        Stream vStream = bufferDepositoT.InputStream;
                        BinaryReader vReader = new BinaryReader(vStream);
                        vFileDeposito = vReader.ReadBytes((int)vStream.Length);
                        vExtension = System.IO.Path.GetExtension(FUSubirPDF.FileName);
                    }
                    String vArchivo = String.Empty;
                    if (vFileDeposito != null)
                        vArchivo = Convert.ToBase64String(vFileDeposito);

                    DataTable vDatosDescrip = new DataTable();
                    vDatosDescrip = vConexion.obtenerDataTable("RSP_DescriptorPuestos 5,'" + LbSubir.Text + "'");

                    String vQuery;
                    if (vDatosDescrip.Rows.Count.ToString() != "0")
                    {
                        vQuery = "RSP_DescriptorPuestos 6," + Session["USUARIO"].ToString() + "," +
                                                            "'" + LbSubir.Text + "'," +
                                                           "'" + vArchivo + "'";
                    }
                    else
                    {
                        vQuery = "RSP_DescriptorPuestos 2," + Session["USUARIO"].ToString() + "," +
                                                            "'" + LbSubir.Text + "'," +
                                                            "'" + vNombreDepot + "'," +
                                                            "'" + vArchivo + "'";
                    }



                    Int32 vInformacion = vConexion.ejecutarSql(vQuery);
               

                }

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }

        }
    }
}