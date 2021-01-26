using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Configuration;
using System.IO;
using Excel;

namespace BiometricoWeb.pages
{
    public partial class UpdateEmployees : System.Web.UI.Page
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
                    
                    CargarEmpleados();
                    CargarTurnos();
                    CargarPuesto();
                    CargarJefes();
                    CargarAreas();
                }
            }
        }
        
        void CargarEmpleados(){
            try{
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerEmpleados 6");

                GVBusqueda.DataSource = vDatos;
                GVBusqueda.DataBind();
                Session["DATAEMPLEADOS"] = vDatos;

            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }
        
        void CargarTurnos(){
            try{
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerEmpleados 3");

                DDLModTurnos.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLModTurnos.Items.Add(new ListItem { Value = item["idTurno"].ToString(), Text = item["nombre"].ToString() });
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        void CargarPuesto(){
            try{
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerEmpleados 4");

                DDLModPuestos.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLModPuestos.Items.Add(new ListItem { Value = item["idPuesto"].ToString(), Text = item["nombre"].ToString() });
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        
        void CargarJefes(){
            try{
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerGenerales 16");

                DDLModJefatura.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLModJefatura.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text =  item["nombre"].ToString() });
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        void CargarAreas(){
            try{
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerGenerales 12");

                DDLModArea.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLModArea.Items.Add(new ListItem { Value = item["idDepartamento"].ToString(), Text = item["nombre"].ToString() });
                }
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GVBusqueda.PageIndex = e.NewPageIndex;
                GVBusqueda.DataSource = (DataTable)Session["DATAEMPLEADOS"];
                GVBusqueda.DataBind();
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                string vIdEmpleado = e.CommandArgument.ToString();
                if (e.CommandName == "UsuarioModificar"){
                    LbModNoEmpleado.Text = vIdEmpleado;

                    DataTable vDatos = new DataTable();
                    vDatos = vConexion.obtenerDataTable("RSP_ObtenerEmpleados 2," + vIdEmpleado);

                    foreach (DataRow item in vDatos.Rows){
                        TxModIdentidad.Text = item["identidad"].ToString();
                        TxModCodigoSAP.Text = item["codigoSAP"].ToString();
                        TxModNombre.Text = item["nombre"].ToString();
                        TxModNacimiento.Text = Convert.ToDateTime((item["fechaNacimiento"].ToString() == "" ? "1900-01-01" : item["fechaNacimiento"].ToString())).ToString("yyyy-MM-dd");
                        TxModTelefono.Text = item["telefono"].ToString();
                        TxModEmailEmpresa.Text = item["emailEmpresa"].ToString();
                        TxModEmailPersonal.Text = item["emailPersonal"].ToString();
                        DDLModCiudad.SelectedIndex = CargarInformacionDDL(DDLModCiudad, item["ciudad"].ToString());
                        DDLModArea.SelectedValue = item["area"].ToString();
                        DDLEstado.SelectedIndex = CargarInformacionDDL(DDLEstado, item["estado"].ToString());
                        DDLModTurnos.SelectedIndex = CargarInformacionDDL(DDLModTurnos, item["idTurno"].ToString());
                        DDLModPuestos.SelectedIndex = CargarInformacionDDL(DDLModPuestos, item["idPuesto"].ToString());
                        //DDLModJefatura.SelectedIndex = CargarInformacionDDL(DDLModJefatura, item["idJefe"].ToString().PadLeft(8, '0'));
                        DDLModJefatura.SelectedIndex = CargarInformacionDDL(DDLModJefatura, item["idJefe"].ToString());
                        TxModADUser.Text = item["adUser"].ToString();
                        string vPermiso = item["PermisosCGS"].ToString();

                        CBxPermisos.Checked = vPermiso == "" ? false : Convert.ToBoolean(vPermiso);
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openEmpleadosModal();", true);
                }

                if (e.CommandName == "UsuarioPassword"){
                    LbEmpleadoPassword.Text = vIdEmpleado;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openPasswordModal();", true);
                }


            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        protected void TxBuscarEmpleado_TextChanged(object sender, EventArgs e){
            try{
                CargarEmpleados();
                String vBusqueda = TxBuscarEmpleado.Text;
                DataTable vDatos = (DataTable)Session["DATAEMPLEADOS"];

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
                                Convert.ToInt32(r["idEmpleado"]) == Convert.ToInt32(vBusqueda));
                        }
                    }

                    DataTable vDatosFiltrados = new DataTable();
                    vDatosFiltrados.Columns.Add("idEmpleado");
                    vDatosFiltrados.Columns.Add("nombre");
                    vDatosFiltrados.Columns.Add("area");
                    vDatosFiltrados.Columns.Add("ciudad");
                    vDatosFiltrados.Columns.Add("identidad");
                    vDatosFiltrados.Columns.Add("telefono");
                    vDatosFiltrados.Columns.Add("estado");
                    vDatosFiltrados.Columns.Add("nombreArea");
                    foreach (DataRow item in filtered){
                        vDatosFiltrados.Rows.Add(
                            item["idEmpleado"].ToString(),
                            item["nombre"].ToString(),
                            item["area"].ToString(),
                            item["ciudad"].ToString(),
                            item["identidad"].ToString(),
                            item["telefono"].ToString(),
                            item["estado"].ToString(),
                            item["nombreArea"].ToString()
                            );
                    }

                    GVBusqueda.DataSource = vDatosFiltrados;
                    GVBusqueda.DataBind();
                    Session["DATAEMPLEADOS"] = vDatosFiltrados;
                    UpdateGridView.Update();
                }
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        public void CerrarModal(String vModal){
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#" + vModal + "').modal('hide');", true);
        }

        int CargarInformacionDDL(DropDownList vList, String vValue){
            int vIndex = 0;
            try{
                int vContador = 0;
                foreach (ListItem item in vList.Items){
                    if (item.Value.Equals(vValue)){
                        vIndex = vContador;
                    }
                    vContador++;
                }
            }
            catch { throw; }
            return vIndex;
        }

        protected void BtnModEmpleados_Click(object sender, EventArgs e){
            try{
                String vQuery = "RSP_IngresarEmpleados 2," + LbModNoEmpleado.Text + "," +
                    "'" + TxModNombre.Text + "'," +
                    DDLModArea.SelectedValue + "," +
                    "'" + DDLModCiudad.SelectedValue + "'," +
                    "'" + null + "'," +
                    "'" + TxModIdentidad.Text + "'," +
                    "'" + TxModNacimiento.Text + "'," +
                    "'" + null + "'," +
                    "'" + TxModEmailEmpresa.Text + "'," +
                    "'" + TxModEmailPersonal.Text + "'," +
                    "'" + TxModTelefono.Text + "'," +
                    "'" + TxModCodigoSAP.Text + "'," +
                    "'" + DDLEstado.SelectedValue + "'," +
                    "'" + DDLModTurnos.SelectedValue + "'," +
                    "'" + DDLModPuestos.SelectedValue + "','" +
                    DDLModJefatura.SelectedValue + "'," +
                    "'" + TxModADUser.Text + "'";

                Int32 vInformacion = vConexion.ejecutarSql(vQuery);
                if (vInformacion == 1){
                    String vActivo = CBxPermisos.Checked ? "1" : "0";

                    vQuery = "[RSP_IngresarEmpleados] 4," + LbModNoEmpleado.Text + ",'" + vActivo + "'";
                    vInformacion = vConexion.ejecutarSql(vQuery);

                    if (vInformacion == 1){
                        Mensaje("Actualizado con Exito!", WarningType.Success);
                        CerrarModal("EmpleadoModal");
                        CargarEmpleados();
                        UpdateGridView.Update();
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnCambiarPassword_Click(object sender, EventArgs e){
            try{
                generales vGenerales = new generales();
                if (TxModPassword.Text.Equals(TxModPasswordConfirmar.Text)){
                    String vPasswordMD5 = vGenerales.MD5Hash(TxModPassword.Text);
                    String vQuery = "RSP_IngresarEmpleados 3," + LbEmpleadoPassword.Text + ",'" + vPasswordMD5 + "'";
                    Int32 vInformacion = vConexion.ejecutarSql(vQuery);
                    if (vInformacion == 1){
                        Mensaje("Actualizado con Exito!", WarningType.Success);
                        CerrarModal("PasswordModal");
                    }else{
                        Mensaje("No se pudo actualizar la contraseña!", WarningType.Danger);
                        CerrarModal("PasswordModal");
                    }

                }else{
                    throw new Exception("Las contraseñas ingresadas no coinciden.");
                }
            }catch (Exception Ex) { 
                LbUsuarioMensaje.Text = Ex.Message; UpdateUsuarioMensaje.Update(); 
            }
        }

        protected void BtnSubirCompensatorio_Click(object sender, EventArgs e){
            String archivoLog = string.Format("{0}_{1}", Convert.ToString(Session["usuario"]), DateTime.Now.ToString("yyyyMMddHHmmss")); //CEMC 26.12.2017 Identificar el archivo con un nombre unico

            try{
                //String vDireccionCarga = @"C:\Carga\";
                
                String vDireccionCarga = ConfigurationManager.AppSettings["RUTA_SERVER"].ToString();
                if (FUCompensatorio.HasFile){
                    String vNombreArchivo = FUCompensatorio.FileName;
                    vDireccionCarga += vNombreArchivo;

                    FUCompensatorio.SaveAs(vDireccionCarga);

                    Boolean vCargado = false;
                    int vSuccess = 0, vError = 0;
                    if (File.Exists(vDireccionCarga)){
                        //cargar vCargarDatos = new cargar();
                        vCargado = cargarArchivo(vDireccionCarga, ref vSuccess, ref vError, Convert.ToString(Session["usuario"]), archivoLog);
                    }

                    if (vCargado){
                        LabelMensaje.Text = "Archivo cargado con exito, favor revise logs" + "<br>" +
                            "<b style='color:green;'>Success:</b> " + vSuccess.ToString() + "&emsp;" + (vSuccess.ToString() == "0" ? "" : "<a href=\"" + ConfigurationManager.AppSettings["log"] + @"/" + archivoLog + "_SUCCESS_log.csv \" download>Log</a>") + "<br>" +
                            "<b style='color:red;'>Error:</b> " + vError.ToString() + "&emsp;" + (vError.ToString() == "0" ? "" : "<a href=\"" + ConfigurationManager.AppSettings["log"] + @"/" + archivoLog + "_ERROR_log.csv \" download>Log</a>");
                    }
                }else{
                    LabelMensaje.Text = "Fallo la carga del archivo, contacte a sistemas.";
                }

            }catch (Exception Ex){
                LabelMensaje.Text = Ex.Message;
            }
        }

        public Boolean cargarArchivo(String DireccionCarga, ref int vSuccess, ref int vError, String vUsuario, String archivoLog){
            Boolean vResultado = false;
            try{
                FileStream stream = File.Open(DireccionCarga, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader;
                if (DireccionCarga.Contains("xlsx"))
                    //2007
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                else
                    //97-2003
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);

                excelReader.IsFirstRowAsColumnNames = true;
                DataSet vDatosExcel = excelReader.AsDataSet();
                excelReader.Close();

                DataSet vDatosVerificacion = vDatosExcel.Copy();
                for (int i = 0; i < vDatosVerificacion.Tables[0].Rows.Count; i++){
                    if (verificarRow(vDatosVerificacion.Tables[0].Rows[i]))
                        vDatosExcel.Tables[0].Rows[i].Delete();
                }
                vDatosExcel.Tables[0].AcceptChanges();
                
                //procesarArchivo(vDatosExcel, ref vSuccess, ref vError, vUsuario, archivoLog);

                vResultado = true;

            }catch (Exception Ex){
                throw  new Exception(Ex.ToString());
            }
            return vResultado;
        }

        private bool verificarRow(DataRow dr){
            int contador = 0;
            foreach (var value in dr.ItemArray){
                if (value.ToString() != ""){
                    contador++;
                }
            }

            if (contador > 0)
                return false;
            else
                return true;
        }

        protected void Parse(string DireccionCarga){
            try{
                FileStream stream = File.Open(DireccionCarga, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader;
                if (DireccionCarga.Contains("xlsx"))
                    //2007
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                else
                    //97-2003
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);

                excelReader.IsFirstRowAsColumnNames = true;
                DataSet vDatosExcel = excelReader.AsDataSet();
                excelReader.Close();

                if (vDatosExcel.Tables.Contains("CAMBIOS") && vDatosExcel.Tables[0].Rows.Count > 0){
                    string vQuery = "[SP_CM_InsertaCambios] '" + Session["USUARIO"] + "','" + DireccionCarga + "',0,0,0,0,1";
                    vQuery = String.Format(vQuery);
                    DataTable vDatosPQR = vConexion.obtenerDataTable(vQuery);
                }

                foreach (DataRow vData in vDatosExcel.Tables[0].Rows){
                    string vMensaje = string.Empty;
                    try{
                        if (vData["OS"].ToString() != string.Empty){
                            string vOS = vData["OS"].ToString();
                            string[] vArray = vOS.Split('-');
                            string vInsertaOS = vArray[0].Trim();

                            if (vInsertaOS != string.Empty)
                                InsertaCompensatorio(vData["OS"].ToString(), vData["CLAVE"].ToString(), vData["LECTURA"].ToString(), vData["MES ACTUAL"].ToString(), ref DireccionCarga);
                        }
                    }catch (Exception ex){
                        if (ex.Message.Contains("OS")){
                            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "text", "eeh.showNotification('top','center','" + "Favor Utilizar Plantilla PlantillaCambiosMedidor.xlsx" + "')", true);
                        }
                    }
                }
            }catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }
        
        protected void InsertaCompensatorio(string Os, string Clave, string lectura, string mes, ref string ruta){
            try{
                string vQuery = "[SP_CM_InsertaCambios] '" + Session["USUARIO"] + "',0," + Os + "," + Clave + ",'" + lectura + "','" + mes + "',2";
                vQuery = String.Format(vQuery);
                DataTable vDatosPQR = vConexion.obtenerDataTable(vQuery);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "eeh.showNotification('top','center','" + "Se ha cargado el archivo correctamente" + "')", true);
            }catch (Exception ex){
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "eeh.showNotification('top','center','" + "Ha habido un error al cargar el archivo, favor revise los datos" + "')", true);
                throw new Exception(ex.Message);
            }
        }
    }
}