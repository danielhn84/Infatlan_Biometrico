using BiometricoService.clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Configuration;


namespace BiometricoService
{
    public partial class BioService : ServiceBase
    {
        Timer vTimer = new Timer();
        zkemkeeper.CZKEMClass vKeeper = new zkemkeeper.CZKEMClass();
        db vConexion = new db();
        private static int idwErrorCode = 0;
        private static int iMachineNumber = 1;
        static Boolean vConnect = false;

        public BioService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.WriteToFile("Starting Service {0}");

            vTimer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            vTimer.Interval = 300000; 
            vTimer.Enabled = true;



        }

        protected override void OnStop()
        {
            vTimer.Enabled = false;
            this.WriteToFile("Stopping Service {0}");
        }


        private void OnElapsedTime(object source, ElapsedEventArgs e){
            if (DateTime.Now.Hour == 8 && DateTime.Now.Minute < 6){
                SapConnector vSapConnection = new SapConnector();
                int vResult = vSapConnection.updateEmployees(DateTime.Now);
            }


            if (Convert.ToBoolean(ConfigurationManager.AppSettings["TestServicio"]))
            {
                this.WriteToFile("Test de servicio {0}");
            }
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["ProcesoNormal"]))
            {
                DataTable vDatos = vConexion.obtenerDataTable("RSP_ObtenerRelojes 1");
                foreach (DataRow item in vDatos.Rows)
                {
                    vKeeper.SetCommPassword(Convert.ToInt32(item["compass"].ToString()));
                    vConnect = vKeeper.Connect_Net(item["ip"].ToString(), Convert.ToInt32(item["puerto"].ToString()));
                    if (vConnect)
                    {
                        ObtenerData(item["ciudad"].ToString());
                    }
                    vKeeper.Disconnect();
                }
            }
            else
                this.WriteToFile("Test de servicio apagado {0}");
        }

        private void WriteToFile(string text)
        {
            string path = "C:\\Services\\WServiceLog" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(string.Format("{0} " + text , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                writer.Close();
            }
        }

        public bool GetConnectState()
        {
            return vConnect;
        }
        public int GetMachineNumber()
        {
            return iMachineNumber;
        }

        public void ObtenerData(String vCiudad)
        {
            try
            {
                if (vConnect)
                {
                    DataTable vDatos = ObtenerHorarios();

                    bool vRevisionInsercion = true;

                    foreach (DataRow item in vDatos.Rows)
                    {
                        if (vConexion.ejecutarSql("RSP_IngresarMarcajes 1," +
                            "'" + item["User ID"].ToString() + "'," +
                            "'" + item["Verify Date"].ToString() + "'," +
                            item["Verify Type"].ToString() + "," +
                            item["Verify State"].ToString() + "," +
                            vCiudad + ""
                            ) == 0)
                        {
                            vRevisionInsercion = false;
                            this.WriteToFile("ERROR INSERT User: " + item["User ID"].ToString() +
                                ", Fecha: " + item["Verify Date"].ToString() + 
                                ", Type: " + item["Verify Type"].ToString() + 
                                ", State: " + item["Verify State"].ToString() + 
                                ", Ciudad: " + vCiudad);
                        }
                    }

                    if (vRevisionInsercion)
                    {
                        if (Convert.ToBoolean(ConfigurationManager.AppSettings["EliminarBiometrico"]))
                        {
                            if (BorrarHorarios() == 0)
                            {
                                this.WriteToFile("Error al borrar los datos, Biometrico: " + vCiudad);
                            }
                            else
                                this.WriteToFile("Insertados y borrados con exito con exito, Biometrico: " + vCiudad);
                        }
                    }
                    else
                        this.WriteToFile("No se borraron los datos, Verifique Insert, Biometrico: " + vCiudad);
                }
                else
                    throw new Exception("No se pudo conectar, Biometrico: " + vCiudad);
            }
            catch (Exception Ex) { WriteToFile(Ex.Message); }
        }

        public DataTable ObtenerHorarios()
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
                WriteToFile(Ex.Message);
            }
            return vDatos;
        }

        public int BorrarHorarios()
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
