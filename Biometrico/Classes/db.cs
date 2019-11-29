using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Biometrico.Classes
{
    class db
    {
        SqlConnection vConexion;
        public db()
        {
            vConexion = new SqlConnection(ConfigurationManager.AppSettings["SQLServer"]);
        }

        public Boolean EjecutarSql(String vQuery, ref String vError)
        {
            Boolean vFlag = false;
            try
            {
                SqlCommand vCommand = new SqlCommand(vQuery, vConexion);
                if (vCommand.ExecuteNonQuery() == 1)
                    vFlag = true;
            }
            catch (Exception Ex)
            {
                vError = Ex.ToString();
            }
            return vFlag;
        }

        public DataTable ObtenerSql(String vQuery, ref String vError)
        {
            DataTable vDatos = new DataTable();
            try
            {
                SqlDataAdapter vAdapter = new SqlDataAdapter(vQuery, vConexion);
                vAdapter.Fill(vDatos);
            }
            catch (Exception Ex)
            {
                vError = Ex.ToString();
            }
            return vDatos;
        }
    }
}
