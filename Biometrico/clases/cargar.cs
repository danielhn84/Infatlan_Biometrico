using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Excel;
using System.IO;
using System.Data;

namespace Gestiones
{
    public class cargar
    {
        public Boolean cargarArchivo(String DireccionCarga, ref int vSuccess, ref int vError, String vUsuario, String archivoLog)
        {
            Boolean vResultado = false;
            try
            {
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
                for (int i = 0; i < vDatosVerificacion.Tables[0].Rows.Count; i++)
                {
                    if (verificarRow(vDatosVerificacion.Tables[0].Rows[i]))
                        vDatosExcel.Tables[0].Rows[i].Delete();
                }
                vDatosExcel.Tables[0].AcceptChanges();

                procesarArchivo(vDatosExcel, ref vSuccess, ref vError, vUsuario, archivoLog);

                vResultado = true;

            }
            catch (Exception)
            {
                throw;
            }
            return vResultado;
        }

        public void procesarArchivo(DataSet vArchivo, ref int vSuccess, ref int vError, String vUsuario, String archivoLog)
        {
            try
            {
               

                db vConexion = new db();
                log vLog = new log();
                if (vArchivo.Tables[0].Rows.Count > 0)
                {
                    DataTable vDatos = vArchivo.Tables[0];


                    //CEMC 03.08.2018 Relizar reserva y programacion automatica
                    string vQuery = "";
                    Boolean empresa = false;
                    Boolean cuadrilla = false;
                    Boolean fecha = false;

                    DataTable dtR, dtP;

                    int cod_empresa = 0;
                    int cod_cuadrilla = 0;
                    string fecha_prog = "";

                    foreach (DataColumn item in vDatos.Columns)
                    {
                        if (item.ColumnName.ToString() == "CODIGO_EMPRESA") {
                            empresa = true;
                        }

                        if (item.ColumnName.ToString() == "CODIGO_CUADRILLA")
                        {
                            cuadrilla = true;
                        }

                        if (item.ColumnName.ToString() == "FECHA_PROGRAMADO")
                        {
                            fecha = true;
                        }

                    }


                    for (int i = 0; i < vDatos.Rows.Count; i++)
                    {
                        int vGestion = 0;
                        String vMensaje = "";
                        vConexion.obtenerDatosReferencia(Convert.ToInt32(vDatos.Rows[i][0].ToString()),
                            Convert.ToInt32(vDatos.Rows[i][1].ToString()),
                            vDatos.Rows[i][2].ToString(), vUsuario,
                            ref vGestion,
                            ref vMensaje);

                        if (vGestion == -1)
                        {
                            vError++;
                            vLog.ingresarLog(false, vGestion.ToString(), vDatos.Rows[i][0].ToString(), vMensaje, vDatos.Rows[i][1].ToString(), vUsuario, archivoLog);
                        }
                        else
                        {
                            

                            //CEMC 03.08.2018 Relizar reserva y programacion automatica

                            if (empresa) {
                                int.TryParse(vDatos.Rows[i]["CODIGO_EMPRESA"].ToString(), out cod_empresa);
                            }

                            if ((cuadrilla) && (fecha)) {
                                int.TryParse(vDatos.Rows[i]["CODIGO_CUADRILLA"].ToString(), out cod_cuadrilla);

                                fecha_prog = vDatos.Rows[i]["FECHA_PROGRAMADO"].ToString();
                            }

                            if (cod_empresa > 0) {
                                vQuery = $"EEHGestiones_ReservasIngresar 1, {vGestion.ToString()}, {cod_empresa.ToString()}, '{vUsuario}', null ";

                                dtR = vConexion.obtenerDataTable(vQuery);

                                if (dtR.Rows.Count > 0) {
                                    if ((dtR.Rows[0]["ID"].ToString() != "") && (int.Parse(dtR.Rows[0]["ID"].ToString()) > 0)) // Reserva exitosa
                                    {
                                        vMensaje = vMensaje + " - Reserva: " + dtR.Rows[0]["MENSAJE"].ToString();

                                        if (cod_cuadrilla > 0) {
                                            vQuery = $" EEHGestiones_ProgramacionesIngresar 1,  {cod_cuadrilla.ToString()}, {vGestion.ToString()}, '{fecha_prog.ToString()}', '{vUsuario}' ";

                                            dtP = vConexion.obtenerDataTable(vQuery);

                                            if (dtP.Rows.Count > 0)
                                            {
                                                if ((dtP.Rows[0]["ID"].ToString() != "") && (int.Parse(dtP.Rows[0]["ID"].ToString()) > 0)) // Programacion exitosa
                                                {
                                                    vMensaje = vMensaje + " - Programacion: " + dtP.Rows[0]["MENSAJE"].ToString();
                                                }
                                                else
                                                {// Error en programacion
                                                    vMensaje = vMensaje + " - Programacion: " + dtP.Rows[0]["MENSAJE"].ToString();
                                                }
                                            }
                                        }
                                        else {
                                            vMensaje = vMensaje + " - Sin programacion";
                                        }
                                    }
                                    else {// Error en reserva
                                        vMensaje = vMensaje + " - Reserva: " +  dtR.Rows[0]["MENSAJE"].ToString();
                                    }
                                }

                                dtR = null;
                                vQuery = "";
                                dtP = null;
                            }

                            vSuccess++;

                            vLog.ingresarLog(true, vGestion.ToString(), vDatos.Rows[i][0].ToString(), vMensaje, vDatos.Rows[i][1].ToString(), vUsuario, archivoLog);

                        }

                        cod_empresa = 0;
                        cod_cuadrilla = 0;
                        fecha_prog = "";
                    }
                }
                else
                    throw new Exception("No contiene ninguna hoja de excel.");
            }
            catch (Exception)
            {

                throw;
            }
        }
        private bool verificarRow(DataRow dr)
        {
            int contador = 0;
            foreach (var value in dr.ItemArray)
            {
                if (value.ToString() != "")
                {
                    contador++;
                }
            }
            if (contador > 0)
                return false;
            else
                return true;
        }
    }
}