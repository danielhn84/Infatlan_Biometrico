﻿using BiometricoWeb.clases;
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
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            try{
                if (!IsPostBack){
                    if (Convert.ToBoolean(Session["AUTH"])) { 
                    }
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
                                String vFechaCompensacion = vDatos.Rows[i]["FECHA"].ToString();
                                String vCantidadHoras = vDatos.Rows[i]["CANTIDAD_HORAS"].ToString();
                                String vHoras2 = vCantidadHoras.Contains(",") ? vCantidadHoras.ToString().Replace(',', '.') : vCantidadHoras.ToString();

                                string[] varr = DireccionCarga.Split('/');
                            
                                vQuery = "RSP_Compensatorio 1,'" + vEmpleado + "', 1,'" + varr[4].ToString() + "','" + Session["USUARIO"].ToString() + "','" + vFechaCompensacion + "'," + vHoras2;
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

            }
        }

        private String Cargar(){
            String vNombre = "";
            try{
                String vQuery = "[RSP_Compensatorio] 2,'" + TxBusqueda.Text + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                if (vDatos.Rows.Count > 0){
                    vDatos.Columns.Add("Detalle");

                    for (int i = 0; i < vDatos.Rows.Count; i++){
                        int vHoras = Convert.ToInt32(vDatos.Rows[i]["cantidadHoras"].ToString());
                        Decimal vMinutos = Convert.ToDecimal(vDatos.Rows[i]["cantidadHoras"].ToString());
                        vMinutos = vMinutos - vHoras;
                        vDatos.Rows[i]["Detalle"] = vHoras + " Horas " + vMinutos + " Minutos";
                    }

                    Session["COMPENSATORIO"] = vDatos;
                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                    vNombre = vDatos.Rows[0]["idEmpleado"].ToString();
                }

            }catch (Exception ex){ }

            return vNombre;
        }

        void limpiarGrid() {
            GVBusqueda.DataSource = null;
            GVBusqueda.DataBind();
            LbTotal.Text = string.Empty;
        }
    }
}