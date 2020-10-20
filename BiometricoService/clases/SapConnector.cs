using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiometricoService.clases;
using System.Data;

namespace BiometricoService.clases
{
    class SapConnector{

        db vConexion = new db();
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

        //public Int32 updateEmployees(DateTime vInicio){
        //    int vCounter = 0;
        //    try{
        //        SapServiceEmployees.ZMFRH_SER_INF vItem = new SapServiceEmployees.ZMFRH_SER_INF() { 
        //            BEGDA = vInicio.ToString("yyyy-MM-dd")
        //        };

        //        SapServiceEmployees.ZWS_HR_SER_INF vRequest = new SapServiceEmployees.ZWS_HR_SER_INF();
        //        SapServiceEmployees.ZMFRH_SER_INFResponse vResponse = vRequest.ZMFRH_SER_INF(vItem);
        //        for (int i = 0; i < vResponse.IT_SALIDA.Length; i++){
        //            String vEstado = vResponse.IT_SALIDA[i].STAT2 == "3" ? "1" : "0";
        //            String vQuery = "RSP_ActualizarEmpleado 1" +
        //                "," + vResponse.IT_SALIDA[i].PERNR.ToString() +
        //                "," + vEstado;
        //            int vInfo = vConexion.ejecutarSql(vQuery);
        //            vCounter = vCounter + vInfo;
        //        }
        //    }catch (Exception Ex){
        //        String vError = Ex.Message;
        //    }
        //    return vCounter;
        //}
    }
}
