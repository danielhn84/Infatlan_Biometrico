using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Configuration;
using System.Data;
using BiometricoWeb.clases;

namespace BiometricoWeb.clases
{
    public class SapConnector
    {
        db vConexion = new db();
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

        public String getConstancias(String vCiudad, String vConsul, String vConta, String vDicon, String vDom1, String vDom2, String vFcons, String vFIni, String vFFin, String vLueve, String vPais, String vPasap, String vEmpleado, String vRtn, String vSemin, String vTelef, String vFirma, ref byte[] pdf){
            String vResultado = String.Empty;
            try{
                SapServiceC.ZMF_HR_CONS_CAPAC_INF2 vConsulta = new SapServiceC.ZMF_HR_CONS_CAPAC_INF2();
                vConsulta.CIEVE = vCiudad;
                vConsulta.CONSU = vConsul;
                vConsulta.CONTA = vConta;
                vConsulta.DICON = vDicon;
                vConsulta.DOMI1 = vDom1;
                vConsulta.DOMI2 = vDom2;
                vConsulta.FCONS = vFcons;
                vConsulta.FEINI = vFIni;
                vConsulta.FEFIN = vFFin;
                vConsulta.LUEVE = vLueve;
                vConsulta.PAIS = vPais;
                vConsulta.PASAP = vPasap;
                vConsulta.PERNR = vEmpleado;
                vConsulta.RTN = vRtn;
                vConsulta.SEMIN = vSemin;
                vConsulta.TELEF = vTelef;
                vConsulta.CODRE = vFirma;
                
                SapServiceC.ZWS_HR_CONS_CAPAC_INF2 vRequest = new SapServiceC.ZWS_HR_CONS_CAPAC_INF2();
                SapServiceC.ZMF_HR_CONS_CAPAC_INF2Response vResponse = vRequest.ZMF_HR_CONS_CAPAC_INF2(vConsulta);

                if (vResponse.MSJ.Equals("Código SAP incorrecto") || vResponse.MSJ.Equals("Código de representate invalido"))
                    vResultado = vResponse.MSJ.ToString();
                else{
                    pdf = vResponse.PDF;
                    vResultado = Convert.ToString(vResponse.PDF);
                }
            }catch (Exception Ex){
                String vError = Ex.Message;
                throw;
            }
            return vResultado;
        }

        public Int32 updateEmployees(DateTime vInicio){
            int vCounter = 0;
            try{
                SapServiceEmployees.ZMFRH_SER_INF vItem = new SapServiceEmployees.ZMFRH_SER_INF() { 
                    BEGDA = vInicio.ToString("yyyy-MM-dd")
                };

                SapServiceEmployees.ZWS_HR_SER_INF vRequest = new SapServiceEmployees.ZWS_HR_SER_INF();
                SapServiceEmployees.ZMFRH_SER_INFResponse vResponse = vRequest.ZMFRH_SER_INF(vItem);
                for (int i = 0; i < vResponse.IT_SALIDA.Length; i++){
                    String vEstado = vResponse.IT_SALIDA[i].STAT2 == "3" ? "1" : "0";
                    String vQuery = "RSP_ActualizarEmpleado 1" +
                        "," + vResponse.IT_SALIDA[i].PERNR.ToString() +
                        "," + vEstado +
                        ",'" + vResponse.IT_SALIDA[i].GBDAT.ToString() + "'" +
                        "," + vResponse.IT_SALIDA[i].PERNR_J.ToString() +
                        ",'" + vResponse.IT_SALIDA[i].DAT01.ToString() + "'" ;
                    int vInfo = vConexion.ejecutarSql(vQuery);
                    vCounter = vCounter + vInfo;
                }
            }catch (Exception Ex){
                String vError = Ex.Message;
            }
            return vCounter;
        }
    }
}