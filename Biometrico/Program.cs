using Biometrico.clases;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zkemkeeper;

namespace Biometrico
{
    class Program 
    {
        static zkemkeeper.CZKEMClass vKeeper = new zkemkeeper.CZKEMClass();
        static db vConexion = new db();
        private static int idwErrorCode = 0;
        private static int iMachineNumber = 1;
        static Boolean vConnect = false;

        static void Main(string[] args)
        {

            DataTable vDatos = vConexion.obtenerDataTable("RSP_ObtenerRelojes 1");
            foreach (DataRow item in vDatos.Rows)
            {
                vKeeper.SetCommPassword(Convert.ToInt32(item["compass"].ToString()));
                vConnect = vKeeper.Connect_Net(item["ip"].ToString(), Convert.ToInt32(item["puerto"].ToString()));
                ObtenerData(item["ciudad"].ToString());
                vKeeper.Disconnect();
            }

            while (true) { };
        }

        public static bool GetConnectState()
        {
            return vConnect;
        }
        public static int GetMachineNumber()
        {
            return iMachineNumber;
        }

        public static void ObtenerData(String vCiudad)
        {
            try
            {
                if (vConnect)
                {
                    DataTable vDatos = ObtenerHorarios();

                    bool vRevisionInsercion = true;

                    foreach (DataRow item in vDatos.Rows)
                    {
                    //    if (vConexion.ejecutarSql("RSP_IngresarMarcajes 1," +
                    //        "'" + item["User ID"].ToString() + "'," +
                    //        "'" + item["Verify Date"].ToString() + "'," +
                    //        item["Verify Type"].ToString() + "," +
                    //        item["Verify State"].ToString() + "," +
                    //        vCiudad + ""
                    //        ) == 0)
                    //    {
                    //        vRevisionInsercion = false;
                    //    }
                    }

                    if (vRevisionInsercion)
                    {
                        //if (BorrarHorarios() == 0)
                        //{
                        //    //LOG
                        //}
                    }
                }
                else
                    throw new Exception("No se pudo conectar");
            }
            catch (Exception Ex){ Console.WriteLine(Ex.Message); }
        }

        public static DataTable ObtenerHorarios()
        {
            DataTable vDatos = new DataTable("dt_period");
            vDatos = new DataTable("dt_period");
            vDatos.Columns.Add("User ID", System.Type.GetType("System.String"));
            vDatos.Columns.Add("Verify Date", System.Type.GetType("System.String"));
            vDatos.Columns.Add("Verify Type", System.Type.GetType("System.Int32"));
            vDatos.Columns.Add("Verify State", System.Type.GetType("System.Int32"));
            vDatos.Columns.Add("WorkCode", System.Type.GetType("System.Int32"));
            String vError = String.Empty;
            try
            {
                if (GetConnectState() == false)
                {
                    throw new Exception("No estas conectado");
                }
                vKeeper.EnableDevice(GetMachineNumber(), false);

                string sdwEnrollNumber = "";
                int idwVerifyMode = 0;
                int idwInOutMode = 0;
                int idwYear = 0;
                int idwMonth = 0;
                int idwDay = 0;
                int idwHour = 0;
                int idwMinute = 0;
                int idwSecond = 0;
                int idwWorkcode = 0;

                if (vKeeper.ReadGeneralLogData(GetMachineNumber()))
                {
                    while (vKeeper.SSR_GetGeneralLogData(GetMachineNumber(), out sdwEnrollNumber, out idwVerifyMode, out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))//get records from the memory
                    {
                        DataRow dr = vDatos.NewRow();
                        dr["User ID"] = sdwEnrollNumber;
                        dr["Verify Date"] = idwYear + "-" + idwMonth.ToString().PadLeft(2, '0') + "-" + idwDay.ToString().PadLeft(2, '0') + " " + idwHour.ToString().PadLeft(2, '0') + ":" + idwMinute.ToString().PadLeft(2, '0') + ":" + idwSecond.ToString().PadLeft(2, '0');
                        dr["Verify Type"] = idwVerifyMode;
                        dr["Verify State"] = idwInOutMode;
                        dr["WorkCode"] = idwWorkcode;
                        vDatos.Rows.Add(dr);
                    }
                }
                else
                {
                    vKeeper.GetLastError(ref idwErrorCode);
                    if (idwErrorCode != 0)
                    {
                        vError = "*Read attlog failed,ErrorCode: " + idwErrorCode.ToString();
                    }
                    else
                    {
                        vError = "No data from terminal returns!";
                    }
                }
                vKeeper.EnableDevice(GetMachineNumber(), true);
            }
            catch (Exception Ex)
            {
                vError = Ex.Message;
            }
            return vDatos;
        }

        public static int BorrarHorarios()
        {
            if (GetConnectState() == false)
            {
                throw new Exception("No estas conectado");
            }

            int ret = 0;
            vKeeper.EnableDevice(GetMachineNumber(), false);


            if (vKeeper.ClearGLog(GetMachineNumber()))
            {
                vKeeper.RefreshData(GetMachineNumber());
                ret = 1;
            }

            vKeeper.EnableDevice(GetMachineNumber(), true);

            return ret;
        }

    }
}
