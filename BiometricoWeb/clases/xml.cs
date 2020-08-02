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

        public String ObtenerXMLDocumentos(Object[] vDatos){
            String vResultado = "";
            try{
                using (StringWriter sw = new StringWriter()){
                    XmlTextWriter vXmlTW = new XmlTextWriter(sw);
                    vXmlTW.Formatting = Formatting.None;

                    vXmlTW.WriteStartDocument();
                    vXmlTW.WriteStartElement("DATOS");

                    vXmlTW.WriteStartElement("idTipoDoc");
                    vXmlTW.WriteString(Convert.ToString(vDatos[0]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("idCategoria");
                    vXmlTW.WriteString(Convert.ToString(vDatos[1]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("nombre");
                    vXmlTW.WriteString(Convert.ToString(vDatos[2]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("nombreArchivo");
                    vXmlTW.WriteString(Convert.ToString(vDatos[3]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("formato");
                    vXmlTW.WriteString(Convert.ToString(vDatos[4]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("archivo");
                    vXmlTW.WriteString(Convert.ToString(vDatos[5]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("direccionArchivo");
                    vXmlTW.WriteString(Convert.ToString(vDatos[6]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("flagLectura");
                    vXmlTW.WriteString(Convert.ToString(vDatos[7]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("flagCorreo");
                    vXmlTW.WriteString(Convert.ToString(vDatos[8]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("fechaCorreo");
                    vXmlTW.WriteString(Convert.ToString(vDatos[9]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("frecuenciaCorreo");
                    vXmlTW.WriteString(Convert.ToString(vDatos[10]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("formatoFrecuencia");
                    vXmlTW.WriteString(Convert.ToString(vDatos[11]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("duracionCorreo");
                    vXmlTW.WriteString(Convert.ToString(vDatos[12]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("formatoDuracion");
                    vXmlTW.WriteString(Convert.ToString(vDatos[13]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("estado");
                    vXmlTW.WriteString(Convert.ToString(vDatos[14]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("usuarioCreacion");
                    vXmlTW.WriteString(Convert.ToString(vDatos[15]));
                    vXmlTW.WriteEndElement();

                    vXmlTW.WriteStartElement("flagAdjunto");
                    vXmlTW.WriteString(Convert.ToString(vDatos[16]));
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