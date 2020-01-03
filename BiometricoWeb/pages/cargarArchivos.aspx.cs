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
    public partial class cargaConsolidado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e){
            try{
                if (!IsPostBack){

                }
            }catch (Exception Ex){

            }
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
                        Boolean cantidadDias = false;
                        foreach (DataColumn item in vDatos.Columns){
                            if (item.ColumnName.ToString() == "ID_EMPLEADO")
                                idEmpleado = true;
                            if (item.ColumnName.ToString() == "CANTIDAD_DIAS")
                                cantidadDias = true;
                        }

                        if (idEmpleado && cantidadDias){
                            for (int i = 0; i < vDatos.Rows.Count; i++){
                                String vEmpleado = vDatos.Rows[i]["ID_EMPLEADO"].ToString();
                                String vCantidad = vDatos.Rows[i]["CANTIDAD_DIAS"].ToString();
                                string[] varr = DireccionCarga.Split('/');
                            
                                vQuery = "RSP_Compensatorio 1," + vEmpleado + "," + vCantidad + ", 1,'" + varr[4].ToString() + "','" + Session["USUARIO"].ToString() + "'";
                                int vRespuesta = vConexion.ejecutarSql(vQuery);
                                if (vRespuesta == 2)
                                    vSuccess++;
                            }
                        }
                    }

                    // PERMISOS
                    if (TipoProceso == "PERMISOS"){
                        Boolean idJefe = false;
                        Boolean tipoPermiso = false;
                        Boolean motivo = false;
                        Boolean inicio = false;
                        Boolean fin = false;

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
                                String vInicio = vDatos.Rows[i]["INICIO"].ToString();
                                String vFin = vDatos.Rows[i]["FIN"].ToString();

                                vQuery = "RSP_IngresarPermisos 2," + vEmpleado + "," +
                                    "" + vJefe + "," +
                                    "" + vTipoPermiso + "," +
                                    "'" + vMotivo + "'," +
                                    "'" + vInicio + "'," +
                                    "'" + vFin + "'," +
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
    }
}