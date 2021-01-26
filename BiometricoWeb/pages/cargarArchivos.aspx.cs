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
using System.Drawing;
using System.Globalization;

namespace BiometricoWeb.pages
{
    public partial class cargaConsolidado : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            try{
                if (!IsPostBack){
                    if (Convert.ToBoolean(Session["AUTH"])) { 
                    }
                }
            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void BtnSubirCompensatorio_Click(object sender, EventArgs e){
            String archivoLog = string.Format("{0}_{1}", Convert.ToString(Session["usuario"]), DateTime.Now.ToString("yyyyMMddHHmmss"));

            try{
                //String vDireccionCarga = @"C:\Carga\";

                String vDireccionCarga = ConfigurationManager.AppSettings["RUTA_SERVER"].ToString();
                if (FUCompensatorio.HasFile){
                    String vNombreArchivo = FUCompensatorio.FileName;
                    vDireccionCarga += "/" + archivoLog + "_" + vNombreArchivo;

                    FUCompensatorio.SaveAs(vDireccionCarga);
                    String vTipoProceso = "COMPENSATORIO";
                    Boolean vCargado = false;
                    int vSuccess = 0, vError = 0;
                    if (File.Exists(vDireccionCarga))
                        vCargado = cargarArchivo(vDireccionCarga, ref vSuccess, ref vError, Convert.ToString(Session["usuario"]), vTipoProceso);
                    
                    if (vCargado)
                        LabelMensaje.Text = "Archivo cargado con exito." + "<br>" + "<b style='color:green;'>Success:</b> " + vSuccess.ToString() + "&emsp;";

                }else
                    LabelMensaje.Text = "No se encontró ningún archivo a cargar.";
                
            }catch (Exception Ex){
                LabelMensaje.Text = Ex.Message;
            }
        }

        public Boolean cargarArchivo(String DireccionCarga, ref int vSuccess, ref int vError, String vUsuario, String TipoProceso){
            Boolean vResultado = false;
            try{
                FileStream stream = File.Open(DireccionCarga, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader;
                if (DireccionCarga.Contains("xlsx"))
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);   //2007
                else
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);    //97-2003

                excelReader.IsFirstRowAsColumnNames = true;
                DataSet vDatosExcel = excelReader.AsDataSet();
                excelReader.Close();

                DataSet vDatosVerificacion = vDatosExcel.Copy();
                for (int i = 0; i < vDatosVerificacion.Tables[0].Rows.Count; i++){
                    if (verificarRow(vDatosVerificacion.Tables[0].Rows[i]))
                        vDatosExcel.Tables[0].Rows[i].Delete();
                }
                vDatosExcel.Tables[0].AcceptChanges();

                procesarArchivo(vDatosExcel, ref vSuccess, DireccionCarga, TipoProceso);

                vResultado = true;

            }catch (Exception Ex){
                throw new Exception(Ex.ToString());
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

        public void procesarArchivo(DataSet vArchivo, ref int vSuccess, string DireccionCarga, string TipoProceso){
            try{
                db vConexion = new db();
                if (vArchivo.Tables[0].Rows.Count > 0){
                    DataTable vDatos = vArchivo.Tables[0];
                    string vQuery = "";
                    Boolean idEmpleado = false;

                    //COMPENSATORIOS
                    if (TipoProceso == "COMPENSATORIO"){
                        Boolean fechainicio = false, fechafin = false;
                        foreach (DataColumn item in vDatos.Columns){
                            if (item.ColumnName.ToString() == "ID_EMPLEADO_SAP")
                                idEmpleado = true;
                            if (item.ColumnName.ToString() == "FECHA")
                                fechainicio = true;
                            if (item.ColumnName.ToString() == "CANTIDAD_HORAS")
                                fechafin = true;
                        }

                        if (idEmpleado && fechainicio && fechafin){
                            for (int i = 0; i < vDatos.Rows.Count; i++){
                                String vEmpleado = vDatos.Rows[i]["ID_EMPLEADO_SAP"].ToString();
                                String vFechaCompensacion = Convert.ToDateTime(vDatos.Rows[i]["FECHA"].ToString()).ToString("MM-dd-yyyy HH:mm:ss");
                                String vCantidadHoras = vDatos.Rows[i]["CANTIDAD_HORAS"].ToString();
                                String vHoras2 = vCantidadHoras.Contains(",") ? vCantidadHoras.ToString().Replace(',', '.') : vCantidadHoras.ToString();

                                string[] varr = DireccionCarga.Split('/');
                                
                                vQuery = "RSP_Compensatorio 1,'" + vEmpleado + "', 1,'" + varr[4].ToString() + "','" + Session["USUARIO"].ToString() + "','" + vFechaCompensacion + "'," + vHoras2 + ",null";
                                int vRespuesta = vConexion.ejecutarSql(vQuery);
                                if (vRespuesta == 2)
                                    vSuccess++;
                            }
                        }
                    }

                    // PERMISOS
                    if (TipoProceso == "PERMISOS"){
                        Boolean idJefe = false, tipoPermiso = false, motivo = false, inicio = false, fin = false;

                        foreach (DataColumn item in vDatos.Columns){
                            if (item.ColumnName.ToString() == "ID_EMPLEADO")
                                idEmpleado = true;
                            if (item.ColumnName.ToString() == "ID_JEFE")
                                idJefe = true;
                            if (item.ColumnName.ToString() == "COD_TIPO_PERMISO")
                                tipoPermiso = true;
                            if (item.ColumnName.ToString() == "MOTIVO")
                                motivo = true;
                            if (item.ColumnName.ToString() == "INICIO")
                                inicio = true;
                            if (item.ColumnName.ToString() == "FIN")
                                fin = true;
                        }

                        if (idEmpleado && idJefe && tipoPermiso && motivo && inicio && fin){
                            for (int i = 0; i < vDatos.Rows.Count; i++){
                                String vEmpleado = vDatos.Rows[i]["ID_EMPLEADO"].ToString();
                                String vJefe = vDatos.Rows[i]["ID_JEFE"].ToString();
                                String vTipoPermiso = vDatos.Rows[i]["COD_TIPO_PERMISO"].ToString();
                                String vMotivo = vDatos.Rows[i]["MOTIVO"].ToString();
                                DateTime vInicio = Convert.ToDateTime(vDatos.Rows[i]["INICIO"].ToString());
                                DateTime vFin = Convert.ToDateTime(vDatos.Rows[i]["FIN"].ToString());

                                vQuery = "RSP_IngresarPermisos 2," + vEmpleado + "," +
                                    "" + vJefe + "," +
                                    "" + vTipoPermiso + "," +
                                    "'" + vMotivo + "'," +
                                    "'" + vInicio.ToString("MM/dd/yyyy HH:mm:ss") + "'," +
                                    "'" + vFin.ToString("MM/dd/yyyy HH:mm:ss") + "'," +
                                    "'" + Session["USUARIO"].ToString() + "'";
                                    
                                int vRespuesta = vConexion.ejecutarSql(vQuery);
                                if (vRespuesta == 1)
                                    vSuccess++;
                            }
                        }
                    }
                }else
                    throw new Exception("No contiene ninguna hoja de excel.");
            }catch (Exception){
                throw;
            }
        }

        protected void BtnSubirPermisos_Click(object sender, EventArgs e){
            String archivoLog = string.Format("{0}_{1}", Convert.ToString(Session["usuario"]), DateTime.Now.ToString("yyyyMMddHHmmss"));

            try{
                String vDireccionCarga = ConfigurationManager.AppSettings["RUTA_SERVER"].ToString();
                //String vDireccionCarga = ConfigurationManager.AppSettings["RUTA_SERVER_LOCAL"].ToString();
                if (FUPermisos.HasFile){
                    String vNombreArchivo = FUPermisos.FileName;
                    vDireccionCarga += "/" + archivoLog + "_" + vNombreArchivo;
                    FUPermisos.SaveAs(vDireccionCarga);
                    String vTipoPermiso = "PERMISOS";
                    Boolean vCargado = false;
                    int vSuccess = 0, vError = 0;
                    if (File.Exists(vDireccionCarga))
                        vCargado = cargarArchivo(vDireccionCarga, ref vSuccess, ref vError, Convert.ToString(Session["usuario"]), vTipoPermiso);

                    if (vCargado)
                        LabelPermisos.Text = "Archivo cargado con exito." + "<br>" + "<b style='color:green;'>Success:</b> " + vSuccess.ToString() + "&emsp;";

                    
                }else
                    LabelPermisos.Text = "No se encontró ningún archivo a cargar.";
            }catch (Exception Ex){
                LabelPermisos.Text = Ex.Message;
            }
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){
            GVBusqueda.PageIndex = e.NewPageIndex;
            GVBusqueda.DataSource = (DataTable)Session["COMPENSATORIO"];
            GVBusqueda.DataBind();
        }

        protected void TxBusqueda_TextChanged(object sender, EventArgs e){
            try{
                limpiarGrid();
                if (TxBusqueda.Text != ""){
                    String vEmpleado = Cargar();
                    if (vEmpleado != ""){
                        String vQuery = "[RSP_ObtenerGenerales] 18, " + vEmpleado;
                        DataTable vData = vConexion.obtenerDataTable(vQuery);
                        LbTotal.Text = vData.Rows.Count > 0 ? "Tiempo Comensatorio Actual: <b> " + vData.Rows[0]["compensatorio"].ToString() + "</b>": "";
                    }else
                        LbTotal.Text = "No se han encontrado registros en la búsqueda.";
                }
            }catch (Exception Ex){
                LbTotal.Text = Ex.Message.ToString();
            } 
        }

        private String Cargar(){
            String vNombre = "";
            try{
                String vQuery = "[RSP_Compensatorio] 2,'" + TxBusqueda.Text + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                if (vDatos.Rows.Count > 0){
                    vDatos.Columns.Add("Detalle");
                    vDatos.Columns.Add("Horas");

                    for (int i = 0; i < vDatos.Rows.Count; i++){
                        String vCantHoras = vDatos.Rows[i]["cantidadHoras"].ToString();
                        string[] vArray = null;
                        
                        if (vCantHoras.Contains(',')) 
                            vArray = vCantHoras.Split(',');
                        else if (vCantHoras.Contains('.'))
                            vArray = vCantHoras.Split('.');
                        else{
                            vArray[0] = vCantHoras;
                            vArray[1] = "0";
                        }

                        String vM = vArray[1];
                        double vMin = Math.Round(float.Parse(vM) * float.Parse("0,6"));
                        String vQMin = vMin.ToString();
                        
                        vQMin = vQMin.Length > 2 ? vQMin.Substring(0,2) : vQMin;
                        vDatos.Rows[i]["Detalle"] = vArray[0].ToString() + " Horas " + vQMin + " Minutos";

                        if (vDatos.Rows[i]["tipoMovimiento"].ToString() == "0"){
                            String vPermiso = vDatos.Rows[i]["idPermiso"].ToString();
                            String vConsulta = "RSP_Compensatorio 3," + vPermiso;
                            DataTable vInfo = vConexion.obtenerDataTable(vConsulta);

                            vDatos.Rows[i]["Estado"] = vInfo.Rows[0][0].ToString() == "1" ? "APROBADO" : "PENDIENTE";
                            vDatos.Rows[i]["Horas"] = "-" + vDatos.Rows[i]["cantidadHoras"].ToString();
                        }else
                            vDatos.Rows[i]["Horas"] = vDatos.Rows[i]["cantidadHoras"].ToString();

                        
                    }

                    for (int i = 0; i < vDatos.Rows.Count; i++){
                        if (vDatos.Rows[i]["tipoMovimiento"].ToString() == "2"){
                            vDatos.Rows[i]["Horas"] = "0.00";

                            foreach (DataRow item in vDatos.Rows){
                                String vPermisoRow = item["idPermiso"].ToString();
                                String vPermisoData = vDatos.Rows[i]["idPermiso"].ToString();

                                if (vPermisoData == vPermisoRow){
                                    vDatos.Rows.Remove(item);
                                    break;
                                }
                            }
                        }
                    }


                    Session["COMPENSATORIO"] = vDatos;
                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                    vNombre = vDatos.Rows[0]["idEmpleado"].ToString();
                }
            }catch (Exception ex){
                LbTotal.Text = ex.Message.ToString();
            }

            return vNombre;
        }

        void limpiarGrid() {
            GVBusqueda.DataSource = null;
            GVBusqueda.DataBind();
            LbTotal.Text = string.Empty;
        }

        protected void GVBusqueda_RowDataBound(object sender, GridViewRowEventArgs e){
            try{
                if (e.Row.RowType == DataControlRowType.DataRow){
                    DataRowView drv = e.Row.DataItem as DataRowView;

                    TableCell vCel = new TableCell();
                    if (drv["tipoMovimiento"].ToString().Equals("0"))
                        e.Row.Cells[6].Attributes.CssStyle.Value = "color : red;"; //System.Drawing.Color.LimeGreen;
                }
            }catch (Exception Ex){
                throw new Exception(Ex.Message);
            }
            
        }

    }
}