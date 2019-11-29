using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Configuration;

namespace BiometricoWeb.clases
{
    public class SapConnector
    {
        public SapConnector()
        {

        }

        public String getDiasVacaciones(String vEmpleado)
        {
            String vResultado = String.Empty;
            try
            {
                SapServicesH.ZFM_CONSULTA_VACACIONES vConsulta = new SapServicesH.ZFM_CONSULTA_VACACIONES();
                vConsulta.P_BEGDA = ConfigurationManager.AppSettings["SapDateInicio"];
                vConsulta.P_ENDDA = ConfigurationManager.AppSettings["SapDateFinal"];
                vConsulta.P_BUKRS = ConfigurationManager.AppSettings["SapCodigoEmpresa"];
                vConsulta.P_PERNR = vEmpleado;
                SapServicesH.ZWS_HR_VACACIONES vRequest = new SapServicesH.ZWS_HR_VACACIONES();
                vRequest.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SapUsername"], ConfigurationManager.AppSettings["SapPassword"]);

                SapServicesH.ZFM_CONSULTA_VACACIONESResponse vResponse = vRequest.ZFM_CONSULTA_VACACIONES(vConsulta);
                vResultado = Convert.ToString(vResponse.P_DIAS);
            }
            catch (Exception Ex)
            {
                String vError = Ex.Message;
                throw;
            }
            return vResultado;
        }

        public String postPermiso(DateTime vInicio, DateTime vFinal, String vEmpleado, String vTipoPermiso, String vSubTipoPermiso, String vMensajeEnvio, ref String vMensaje)
        {
            String vResultado = String.Empty;
            try
            {
                SapServicesP.ZST_GETDATA_INFA[] vItem = new SapServicesP.ZST_GETDATA_INFA[1];
                vItem[0] = new SapServicesP.ZST_GETDATA_INFA()
                {
                    MANDT = "101",
                    BEGDA = vInicio.ToString("yyyy-MM-dd"),
                    ENDDA = vFinal.ToString("yyyy-MM-dd"),
                    BEGUZ = vInicio.ToString("HH:mm:ss"),
                    ENDUZ = vFinal.ToString("HH:mm:ss"),
                    PERNR = vEmpleado,
                    INFTY = vTipoPermiso,
                    SUBTY = vSubTipoPermiso,
                    UMSCH = vMensajeEnvio
                };

                //vItem[0] = new SapServicesP.ZST_GETDATA_INFA()
                //{
                //    MANDT = "101",
                //    BEGDA = "2019-10-23",
                //    ENDDA = "2019-10-23",
                //    BEGUZ = "12:22:00",
                //    ENDUZ = "12:46:00",
                //    PERNR = "63",
                //    INFTY = "2001",
                //    SUBTY = "1004"
                //};

                SapServicesP.ZFM_SAVE_INFTP vPermiso = new SapServicesP.ZFM_SAVE_INFTP();
                vPermiso.ST_GETDATAS = vItem;

                SapServicesP.ZWS_HR_SETDATA vRequest = new SapServicesP.ZWS_HR_SETDATA();
                vRequest.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SapUsername"], ConfigurationManager.AppSettings["SapPassword"]);
                SapServicesP.ZFM_SAVE_INFTPResponse vResponse = vRequest.ZFM_SAVE_INFTP(vPermiso);

                SapServicesP.ZST_GETDATA_INFA[] vResponseInfo = vResponse.ST_GETDATAS;
                if (vResponseInfo.Length > 0)
                {
                    vResultado = vResponseInfo[0].ESTADO;
                    vMensaje = vResponseInfo[0].MESSAGE;
                }

            }
            catch (Exception Ex)
            {
                String vError = Ex.Message;
                throw;
            }
            return vResultado;
        }
    }
}