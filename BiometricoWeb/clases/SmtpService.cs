using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Data;

namespace BiometricoWeb.clases
{
    public enum typeBody{
        Supervisor,
        RecursosHumanos,
        Solicitante,
        Aprobado,
        Rechazado,
        Token,
        Seguridad,
        Sugerencias,
        TiempoExtraordinario,
        Reporte,
        Viaticos
    }

    public class SmtpService : Page{

        public SmtpService() { }

        public Boolean EnviarMensaje(String To, typeBody Body, String Usuario, String Nombre, String vMessage = null, String vCopia = null, DataTable vDatos = null){
            Boolean vRespuesta = false;
            try{
                MailMessage mail = new MailMessage("Recursos Humanos<" + ConfigurationManager.AppSettings["SmtpFrom"] + ">", To);
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);

                if (!String.IsNullOrEmpty(vCopia)){
                    if (vCopia.Contains("@")){
                        mail.CC.Add(vCopia);
                    }
                }
                String vTable = "", vHead = "", vHead2 = "";
                if (vDatos != null){
                    if (vDatos.Rows.Count > 0){
                        vHead = "<table width='100%' style='border:1px ridge #333'>" +
                                    "<tr style='background-color:#DCDCDC; font-weight: bold;'>{0}";
                        foreach (DataColumn column in vDatos.Columns){
                            vHead2 += "<td style='border:ridge'>" + column.ColumnName + " </td>";
                        }
                        vHead = string.Format(vHead, vHead2 + "</tr>");
                        vTable = vHead + "{0}";
                            
                        String vFilas = "", vColumnas = "";
                        for (int i = 0; i < vDatos.Rows.Count; i++){
                            vFilas += "<tr>{0}</tr>"; 
                            for (int j = 0; j < vDatos.Columns.Count; j++){
                                vColumnas += "<td style='border:ridge'>" + vDatos.Rows[i][j] + " </td> ";
                            }
                            vFilas = string.Format(vFilas, vColumnas);
                            vColumnas = "";
                        }
                        vTable = string.Format(vTable, vFilas + "</table>");
                    }
                }

                //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = ConfigurationManager.AppSettings["SmtpServer"];
                mail.Subject = "Recursos Humanos - Información de empleado";
                mail.IsBodyHtml = true;

                switch (Body){
                    case typeBody.Supervisor:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBody(
                            Usuario,
                            "El empleado " + Nombre + " ha creado una petición de permiso.",
                            ConfigurationManager.AppSettings["Host"] + "/pages/authorizations.aspx",
                            "Te informamos que el permiso tiene que ser autorizado, para que sea procesado por Recursos Humanos."
                            ), Server.MapPath("/images/logo.png")));
                        break;
                    case typeBody.RecursosHumanos:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBody(
                            Usuario,
                            "El empleado " + Nombre + " ha creado una petición de permiso.",
                            ConfigurationManager.AppSettings["Host"] + "/pages/authorizations.aspx",
                            "Te informamos que el permiso ha sido autorizado por el jefe del area, ingresa al link para poder ver mas detalles."
                            ), Server.MapPath("/images/logo.png")));
                        break;
                    case typeBody.Solicitante:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBody(
                            Usuario,
                            "Has creado una petición de permiso.",
                            ConfigurationManager.AppSettings["Host"] + "/pages/permissions.aspx",
                            "Te informamos que el permiso ha sido enviado a tu jefe para la debida autorización."
                            ), Server.MapPath("/images/logo.png")));
                        break;
                    case typeBody.Aprobado:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBody(
                            Usuario,
                            "Se ha aprobado tu solicitud de permiso",
                            ConfigurationManager.AppSettings["Host"] + "/pages/permissions.aspx",
                            "Te informamos que el permiso que has solicitado ha sido aprobado."
                            ), Server.MapPath("/images/logo.png")));
                        break;
                    case typeBody.Rechazado:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBody(
                            Usuario,
                            "Se ha rechazado tu solicitud de permiso",
                            ConfigurationManager.AppSettings["Host"] + "/pages/permissions.aspx",
                            Nombre
                            ), Server.MapPath("/images/logo.png")));
                        break;
                    case typeBody.Token:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBody(
                            Usuario,
                            "Se ha creado un Token para emergencias",
                            ConfigurationManager.AppSettings["Host"] + "/pages/permissions.aspx",
                            "Token: <b>" + Nombre + @"</b> <br \> Ingresa este token en la pagina de permisos despues de haber seleccionado la opción de emergencias, este token solo puede ser utilizado una sola vez."
                            ), Server.MapPath("/images/logo.png")));
                        break;
                    case typeBody.Seguridad:
                        string[] vInfo = Nombre.Split('-');

                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBodyES(
                            Usuario,
                            "Se ha creado un registro de <b>" + vInfo[0].ToString() + "</b> con su autorización. " +
                            "Id: <b>" + vInfo[1].ToString() + "</b><br> Si no ha autorizado la salida del artículo. Favor comuníquese con el personal de seguridad."
                            ), Server.MapPath("/images/logo.png")));
                        break;
                    case typeBody.Sugerencias:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBodyBuzon(
                            Usuario,
                            Nombre,
                            vMessage,
                            ""
                            ), Server.MapPath("/images/logo.png")));
                        break;
                    case typeBody.TiempoExtraordinario:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBodyBuzon(
                            Usuario,
                            Nombre,
                            vMessage,
                            ""
                            ), Server.MapPath("/images/logo.png")));
                        break;
                    case typeBody.Reporte:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBodyReport(
                            Usuario,
                            Nombre,
                            vTable
                            ), Server.MapPath("/images/logo.png")));
                        break;
                    case typeBody.Viaticos:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBodyBuzon(
                            Usuario,
                            Nombre,
                            vMessage,
                            ConfigurationManager.AppSettings["Host"] + vCopia
                            ), Server.MapPath("/images/logo.png")));
                        break;
                }
                client.Send(mail);
                vRespuesta = true;
            }catch (System.Net.Mail.SmtpException Ex){
                String vError = Ex.Message;
                throw;
            }catch (Exception Ex){
                throw;
            }
            return vRespuesta;
        }
        
        private AlternateView CreateHtmlMessage(string message, string logoPath){
            var inline = new LinkedResource(logoPath, "image/png");
            inline.ContentId = "companyLogo";

            var alternateView = AlternateView.CreateAlternateViewFromString(
                                    message,
                                    Encoding.UTF8,
                                    MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(inline);

            return alternateView;
        }

        public string PopulateBody(string vNombre, string vTitulo, string vUrl, string vDescripcion){
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("/pages/mail/TemplateMail.html"))){
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Host}", ConfigurationManager.AppSettings["Host"]);
            body = body.Replace("{Nombre}", vNombre);
            body = body.Replace("{Titulo}", vTitulo);
            body = body.Replace("{Url}", vUrl);
            body = body.Replace("{Descripcion}", vDescripcion);
            return body;
        }

        public string PopulateBodyES(string vNombre, string vDescripcion){
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("/pages/mail/TemplateMail.html"))){
                body = reader.ReadToEnd();
            }

            body = body.Replace("{Host}", ConfigurationManager.AppSettings["Host"]);
            body = body.Replace("{Nombre}", vNombre);
            body = body.Replace("{Titulo}", "");
            body = body.Replace("{Descripcion}", vDescripcion);
            return body;
        }

        public string PopulateBodyBuzon(string vNombre, string vTitulo, string vDescripcion, string URL){
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("/pages/mail/TemplateMailSugerencias.html"))){
                body = reader.ReadToEnd();
            }

            body = body.Replace("{Host}", ConfigurationManager.AppSettings["Host"]);
            body = body.Replace("{Nombre}", vNombre);
            body = body.Replace("{Titulo}", vTitulo);
            body = body.Replace("{Descripcion}", vDescripcion);
            //body = body.Replace("{Url}", URL);
            //body = body.Replace("{Direccion}", URL);
            return body;
        }
        
        public string PopulateBodyReport(string vNombre, string vTitulo, string vTabla){
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("/pages/mail/TemplateReport.html"))){
                body = reader.ReadToEnd();
            }

            body = body.Replace("{Host}", ConfigurationManager.AppSettings["Host"]);
            body = body.Replace("{Nombre}", vNombre);
            body = body.Replace("{Titulo}", vTitulo);
            body = body.Replace("{Tabla}", vTabla);
            return body;
        }
    }
}