﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BiometricoWeb.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.4.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://10.128.2.22:8000/sap/bc/srt/rfc/sap/zws_hr_vacaciones/300/zws_hr_vacacione" +
            "s/zws_hr_vacaciones")]
        public string BiometricoWeb_SapServicesH_ZWS_HR_VACACIONES {
            get {
                return ((string)(this["BiometricoWeb_SapServicesH_ZWS_HR_VACACIONES"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://10.128.2.22:8000/sap/bc/srt/rfc/sap/zws_hr_setdata/300/zws_hr_setdata/zws_" +
            "hr_setdata")]
        public string BiometricoWeb_SapServicesP_ZWS_HR_SetData {
            get {
                return ((string)(this["BiometricoWeb_SapServicesP_ZWS_HR_SetData"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://10.128.0.52:80/ReportServer/ReportExecution2005.asmx")]
        public string BiometricoWeb_ReportExecutionService_ReportExecutionService {
            get {
                return ((string)(this["BiometricoWeb_ReportExecutionService_ReportExecutionService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://150.150.6.142:8000/sap/bc/srt/rfc/sap/zws_hr_cons_capac_inf2/101/zws_rh_co" +
            "ns_capac_inf2/zws_rh_cons_capac_inf2")]
        public string BiometricoWeb_SapServiceConstancias_ZWS_RH_CONS_CAPAC_INF2 {
            get {
                return ((string)(this["BiometricoWeb_SapServiceConstancias_ZWS_RH_CONS_CAPAC_INF2"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://s4hqas-mgmt.bancatlan.hn:8000/sap/bc/srt/rfc/sap/zws_hr_ser_inf/210/zws_hr" +
            "_ser_inf/zws_hr_ser_inf")]
        public string BiometricoWeb_SapServiceEmployees_ZWS_HR_SER_INF {
            get {
                return ((string)(this["BiometricoWeb_SapServiceEmployees_ZWS_HR_SER_INF"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://s4hqas-mgmt.bancatlan.hn:8000/sap/bc/srt/rfc/sap/zws_hr_cons_capac_inf2/21" +
            "0/zws_hr_cons_capac_inf2/zws_hr_cons_capac_inf2")]
        public string BiometricoWeb_SapServiceC_ZWS_HR_CONS_CAPAC_INF2 {
            get {
                return ((string)(this["BiometricoWeb_SapServiceC_ZWS_HR_CONS_CAPAC_INF2"]));
            }
        }
    }
}
