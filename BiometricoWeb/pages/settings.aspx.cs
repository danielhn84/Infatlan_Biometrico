using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BiometricoWeb.pages
{
    public partial class settings : System.Web.UI.Page
    {
        db vConexion;
        protected void Page_Load(object sender, EventArgs e)
        {
            vConexion = new db();
            if (!Page.IsPostBack)
            {
                if (Convert.ToBoolean(Session["AUTH"]))
                {
                    DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                    LbNombreUsuario.Text = ((DataRow)vDatos.Rows[0])["nombre"].ToString();
                }
            }
        }
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        public void CerrarModal(String vModal)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#" + vModal + "').modal('hide');", true);
        }

        protected void BtnGuardarCambios_Click(object sender, EventArgs e)
        {
            try
            {
                generales vGenerales = new generales();
                if (!TxPassword.Text.Equals(TxConfirmar.Text))
                    throw new Exception("Las contraseñas ingresadas no coinciden.");

                String vQuery = "RSP_LoginUpdate '" + Convert.ToString(Session["USUARIO"]) + "','" + vGenerales.MD5Hash(TxPassword.Text) + "'";
                Int32 vInformacion = vConexion.ejecutarSql(vQuery);


                if (vInformacion.Equals(1))
                {
                    Exitoso();
                }
                else
                    throw new Exception("Ha ocurrido un problema, contacte a sistemas");
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                TxPassword.Text = String.Empty;
                TxConfirmar.Text = String.Empty;
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        private void Exitoso()
        {
            TxPassword.Text = String.Empty;
            TxConfirmar.Text = String.Empty;
            Mensaje("Actualizado con Exito!", WarningType.Success);
        }

    }
}