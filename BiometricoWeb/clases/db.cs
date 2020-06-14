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
        SqlConnection vConexion, vConexionSysAid;
        public db(){
            vConexion = new SqlConnection(ConfigurationManager.AppSettings["SQLServer"]);
            vConexionSysAid = new SqlConnection(ConfigurationManager.AppSettings["SQLServerSysAid"]);
        }

        public DataTable obtenerDataTable(String vQuery){
            DataTable vDatos = new DataTable();
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
    }
}