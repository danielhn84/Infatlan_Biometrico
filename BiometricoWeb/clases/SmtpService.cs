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

namespace BiometricoWeb.clases
{
    public enum typeBody
    {
        Supervisor,
        RecursosHumanos,
        Solicitante,
        Aprobado,
        Rechazado
    }
    public class SmtpService : Page
    {
        public SmtpService() { }

        public Boolean EnviarMensaje(String To, typeBody Body, String Usuario, String Nombre)
        {
            Boolean vRespuesta = false;
            try
            {
                MailMessage mail = new MailMessage("Recursos Humanos<" + ConfigurationManager.AppSettings["SmtpFrom"] + ">", To);
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
                //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = ConfigurationManager.AppSettings["SmtpServer"];
                mail.Subject = "Recursos Humanos - Información de empleado";
                mail.IsBodyHtml = true;

                switch (Body)
                {
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
                            "Te informamos que el permiso que has solicitado ha sido rechazado."
                            ), Server.MapPath("/images/logo.png")));
                        break;
                }
                client.Send(mail);
                vRespuesta = true;
            }
            catch (System.Net.Mail.SmtpException Ex)
            {
                String vError = Ex.Message;
                throw;
            }
            catch (Exception Ex)
            {
                throw;
            }
            return vRespuesta;
        }
        private AlternateView CreateHtmlMessage(string message, string logoPath)
        {
            var inline = new LinkedResource(logoPath, "image/png");
            inline.ContentId = "companyLogo";

            var alternateView = AlternateView.CreateAlternateViewFromString(
                                    message,
                                    Encoding.UTF8,
                                    MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(inline);

            return alternateView;
        }

        public string PopulateBody(string vNombre, string vTitulo, string vUrl, string vDescripcion)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("/pages/mail/TemplateMail.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Host}", ConfigurationManager.AppSettings["Host"]);
            body = body.Replace("{Nombre}", vNombre);
            body = body.Replace("{Titulo}", vTitulo);
            body = body.Replace("{Url}", vUrl);
            body = body.Replace("{Descripcion}", vDescripcion);
            return body;
        }
    }
}