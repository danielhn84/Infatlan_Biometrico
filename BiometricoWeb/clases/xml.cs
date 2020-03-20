using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Xml;

namespace BiometricoWeb.clases
{
    public class xml
    {

        public String ObtenerMaestroString(Object[] vDatos){
            String vResultado = "";
            try{
                using (StringWriter sw = new StringWriter()){
                    XmlTextWriter vXmlTW = new XmlTextWriter(sw);
                    vXmlTW.Formatting = Formatting.None;

                    vXmlTW.WriteStartDocument();
                    vXmlTW.WriteStartElement("MAESTRO");

                    vXmlTW.WriteStartElement("Destino1");
                    vXmlTW.WriteString(Convert.ToString(vDatos[0]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("Monto1");
                    vXmlTW.WriteString(Convert.ToString(vDatos[1]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("Destino2");
                    vXmlTW.WriteString(Convert.ToString(vDatos[2]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("Monto2");
                    vXmlTW.WriteString(Convert.ToString(vDatos[3]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("Destino3");
                    vXmlTW.WriteString(Convert.ToString(vDatos[4]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("Monto3");
                    vXmlTW.WriteString(Convert.ToString(vDatos[5]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("Destino4");
                    vXmlTW.WriteString(Convert.ToString(vDatos[6]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("Monto4");
                    vXmlTW.WriteString(Convert.ToString(vDatos[7]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteEndElement();
                    vXmlTW.WriteEndDocument();

                    vResultado = sw.ToString();
                }
            }catch{
                throw;
            }
            return vResultado;
        }
    }
}