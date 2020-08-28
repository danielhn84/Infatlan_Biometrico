using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiometricoService.clases;

namespace BiometricoService.clases
{
    class SapConnector{

        db vConexion = new db();
        public String updateEmployees(DateTime vInicio){
            String vResultado = String.Empty;
            try{
                SapServiceEmployees.ZMFRH_SER_INF vItem = new SapServiceEmployees.ZMFRH_SER_INF() { 
                    BEGDA = vInicio.ToString("yyyy-MM-dd")
                };

                //SapServiceEmployees.ZMFRH_SER_INF vPermiso = new SapServiceEmployees.ZMFRH_SER_INF();
                //vPermiso.BEGDA = vItem;

                //SapServiceEmployees.ZWS_HR_SETDATA vRequest = new SapServiceEmployees.ZWS_HR_SETDATA();
                //vRequest.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SapUsername"], ConfigurationManager.AppSettings["SapPassword"]);
                //SapServiceEmployees.ZMFRH_SER_INFResponse vResponse = vRequest.ZFM_SAVE_INFTP(vPermiso);

                //SapServiceEmployees.ZST_GETDATA_INFA[] vResponseInfo = vResponse.ST_GETDATAS;

                //if (vResponseInfo.Length > 0){
                //    vResultado = vResponseInfo[0].ESTADO;
                //    vMensaje = vResponseInfo[0].MESSAGE;
                //}

            }catch (Exception Ex){
                String vError = Ex.Message;
                throw;
            }
            return vResultado;
        }
    }


}
