using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using zkemkeeper;
using System.Data;

namespace BiometricoWeb.clases
{
    public class biometricos
    {
        zkemkeeper.CZKEMClass vKeeper = new zkemkeeper.CZKEMClass();
        db vConexion = new db();
        private int iMachineNumber = 1;
        Boolean vConnect = false;
        String vDuo = String.Empty;

        public biometricos(String vDuo)
        {
            this.vDuo = vDuo;
        }

        public bool GetConnectState()
        {
            return vConnect;
        }
        public int GetMachineNumber()
        {
            return iMachineNumber;
        }

        public int CrearUsuarioBiometrico(String UserID, String Nombre, int Privilegio, ref String vErrorSuccess)
        {
            DataTable vDatos = vConexion.obtenerDataTable("RSP_ObtenerRelojes 2," + vDuo);
            foreach (DataRow item in vDatos.Rows)
            {
                vKeeper.SetCommPassword(Convert.ToInt32(item["compass"].ToString()));
                vConnect = vKeeper.Connect_Net(item["ip"].ToString(), Convert.ToInt32(item["puerto"].ToString()));

                if (GetConnectState() == false)
                {
                    vErrorSuccess = "No estas conectado";
                    return 0;
                }

                int iPrivilege = Privilegio;
                if (UserID.Length > 5)
                {
                    vErrorSuccess = "El codigo de usuario es muy grande";
                    return 0;
                }

                if (UserID.Substring(0, 1) == "0")
                {
                    vErrorSuccess = "El primer numero no puede ser cero";
                    return 0;
                }

                int idwErrorCode = 0;
                string sdwEnrollNumber = UserID.Trim();
                string sName = Nombre.Trim();
                string sCardnumber = "";
                string sPassword = "";

                bool bEnabled = true;

                vKeeper.EnableDevice(iMachineNumber, false);
                vKeeper.SetStrCardNumber(sCardnumber);
                if (vKeeper.SSR_SetUserInfo(iMachineNumber, sdwEnrollNumber, sName, sPassword, iPrivilege, bEnabled))
                {
                    vErrorSuccess = "El usuario se ha creado con exito";
                }
                else
                {
                    vKeeper.GetLastError(ref idwErrorCode);
                    vErrorSuccess = ("ErrorCode=" + idwErrorCode.ToString());
                }
                vKeeper.RefreshData(iMachineNumber);
                vKeeper.EnableDevice(iMachineNumber, true);
                vKeeper.Disconnect();
            }
            return 1;
        }

        public int ModificarGrupoUsuarioBiometrico( int UserID, int Group, ref String vErrorSuccess)
        {

            DataTable vDatos = vConexion.obtenerDataTable("RSP_ObtenerRelojes 2," + vDuo);
            foreach (DataRow item in vDatos.Rows)
            {
                vKeeper.SetCommPassword(Convert.ToInt32(item["compass"].ToString()));
                vConnect = vKeeper.Connect_Net(item["ip"].ToString(), Convert.ToInt32(item["puerto"].ToString()));


                if (GetConnectState() == false)
                {
                    vErrorSuccess = "No estas conectado";
                    return 0;
                }

                int idwErrorCode = 0;
                if (vKeeper.SetUserGroup(iMachineNumber, UserID, Group))
                {
                    vKeeper.RefreshData(iMachineNumber);
                }
                else
                {
                    vKeeper.GetLastError(ref idwErrorCode);
                    vErrorSuccess = ("ErrorCode=" + idwErrorCode.ToString());
                }
                vKeeper.Disconnect();
            }
            return 1;
        }

    }
}