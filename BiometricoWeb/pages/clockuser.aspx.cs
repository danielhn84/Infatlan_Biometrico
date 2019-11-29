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
    public partial class clockuser : System.Web.UI.Page
    {
        db vConexion;
        protected void Page_Load(object sender, EventArgs e)
        {
            vConexion = new db();
            if (!Page.IsPostBack)
            {
                if (Convert.ToBoolean(Session["AUTH"]))
                {
                    generales vGenerales = new generales();
                    DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                    if (!vGenerales.PermisosRecursosHumanos(vDatos))
                        Response.Redirect("/default.aspx");

                    CargarRelojes();
                }
            }
        }
        void CargarRelojes()
        {
            try
            {
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerRelojes 1");

                DDLCrearRelojes.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows)
                {
                    DDLCrearRelojes.Items.Add(new ListItem { Value = item["duo"].ToString(), Text = item["nombre"].ToString() });
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        public void CerrarModal(String vModal)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#" + vModal + "').modal('hide');", true);
        }
        protected void BtnGuardarCambio_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxCrearNoEmpleado.Text.Equals(""))
                    throw new Exception("Por favor ingrese un codigo de empleado");
                if (TxCrearNombre.Text.Equals(""))
                    throw new Exception("Por favor ingrese el nombre del empleado");

                if (DDLCrearRelojes.SelectedValue.Equals("0"))
                    throw new Exception("Seleccione un Biometrico para la insercion.");

                String vErrorSuccess = "";
                biometricos vRelojes = new biometricos(DDLCrearRelojes.SelectedValue);
                Int32 vRelojReturn = vRelojes.CrearUsuarioBiometrico(TxCrearNoEmpleado.Text, TxCrearNombre.Text, Convert.ToInt32(DDLCrearRole.SelectedValue), ref vErrorSuccess);

                if (vRelojReturn == 1)
                {
                    Mensaje("Usuario ingresado con exito", WarningType.Success);
                }
                else
                {
                    Mensaje("Fallo ingreso en Biometrico", WarningType.Danger);
                }
                LimpiarBiometrico();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnCancelarCambio_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarBiometrico();

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        private void LimpiarBiometrico()
        {
            TxCrearNoEmpleado.Text = "";
            TxCrearNombre.Text = "";
            DDLCrearRelojes.SelectedIndex = -1;
            DDLCrearRole.SelectedIndex = -1;
        }
    }
}