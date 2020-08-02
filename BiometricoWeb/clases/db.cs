using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace BiometricoWeb.clases
{
    public enum WarningType
    {
        Success,
        Info,
        Warning,
        Danger
    }
    public class db
    {
        SqlConnection vConexion, vConexionSysAid, vConexionLocalidad;
        public db(){
            vConexion = new SqlConnection(ConfigurationManager.AppSettings["SQLServer"]);
            vConexionSysAid = new SqlConnection(ConfigurationManager.AppSettings["SQLServerSysAid"]);
            vConexionLocalidad = new SqlConnection(ConfigurationManager.AppSettings["SQLServerLocalidad"]);
        }

        public DataTable obtenerDataTable(String vQuery){
            DataTable vDatos = new DataTable();
            try{
                SqlDataAdapter vDataAdapter = new SqlDataAdapter(vQuery, vConexion);
                SqlCommand vSqlCommand = new SqlCommand(vQuery, vConexion);


                vDataAdapter.Fill(vDatos);
            }catch{
                throw;
            }
            return vDatos;
        }

        public DataSet obtenerDataSet(String vQuery){
            DataSet vDatos = new DataSet();
            try{
                SqlDataAdapter vDataAdapter = new SqlDataAdapter(vQuery, vConexion);
                vDataAdapter.Fill(vDatos);
            }catch{
                throw;
            }
            return vDatos;
        }

        public int ejecutarSql(String vQuery){
            int vResultado = 0;
            try{
                SqlCommand vSqlCommand = new SqlCommand(vQuery, vConexion);
                vSqlCommand.CommandType = CommandType.Text;

                vConexion.Open();
                vResultado = vSqlCommand.ExecuteNonQuery();
                vConexion.Close();

            }catch (Exception Ex){
                String vError = Ex.Message;
                vConexion.Close();
                throw;
            }
            return vResultado;
        }

        public int obtenerId(String vQuery){
            DataTable vDatos = new DataTable();
            int vId = 0;
            try{
                SqlDataAdapter vDataAdapter = new SqlDataAdapter(vQuery, vConexion);
                SqlCommand vSqlCommand = new SqlCommand(vQuery, vConexion);
                vDataAdapter.Fill(vDatos);
                vId = Convert.ToInt32(vDatos.Rows[0][0].ToString());
            }catch{
                throw;
            }
            return vId;
        }

        public int ejecutarSqlSysAid(String vQuery){
            int vResultado = 0;
            try{
                SqlCommand vSqlCommand = new SqlCommand(vQuery, vConexionSysAid);
                vSqlCommand.CommandType = CommandType.Text;
                vConexionSysAid.Open();
                vResultado = vSqlCommand.ExecuteNonQuery();
                vConexionSysAid.Close();

            }catch (Exception Ex){
                String vError = Ex.Message;
                vConexionSysAid.Close();
                throw;
            }
            return vResultado;
        }

        public DataTable obtenerDataTableSysAid(String vQuery){
            DataTable vDatos = new DataTable();
            try{
                SqlDataAdapter vDataAdapter = new SqlDataAdapter(vQuery, vConexionSysAid);
                vDataAdapter.Fill(vDatos);
            }catch{
                throw;
            }
            return vDatos;
        }

        public DataTable obtenerDataTableSTEI(string vQuery){
            DataTable vDatos = new DataTable();
            try{
                SqlDataAdapter vDataAdapter = new SqlDataAdapter(vQuery, vConexionLocalidad);
                vDataAdapter.Fill(vDatos);
            }catch (Exception){
                throw;
            }
            return vDatos;
        }

        public DataTable obtenerDataTableDashboard(string vQuery){
            DataTable vDatos = new DataTable();
            try{
                SqlCommand vSqlCommand = new SqlCommand(vQuery, vConexion);
                vSqlCommand.CommandTimeout = 60000;

                SqlDataAdapter vDataAdapter = new SqlDataAdapter(vQuery, vConexion);
                vConexion.Open();
                vDataAdapter.SelectCommand = new SqlCommand(vQuery, vConexion);
                vDataAdapter.Fill(vDatos);
                vConexion.Close();
            }catch (Exception){
                throw;
            }
            return vDatos;
        }
    }
}