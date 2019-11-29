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
    public partial class access : System.Web.UI.Page
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

                    CargarEmpleados();
                    CargarGrupos();
                    CargarRelojes();
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
        void CargarEmpleados()
        {
            try
            {
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerGenerales 1");

                DDLEmpleado.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows)
                {
                    DDLEmpleado.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["idEmpleado"].ToString() + " - " + item["nombre"].ToString() });
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        void CargarGrupos()
        {
            try
            {
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerGenerales 2");

                DDLGrupo.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows)
                {
                    DDLGrupo.Items.Add(new ListItem { Value = item["idAcceso"].ToString(), Text = item["idAcceso"].ToString() + " - " + item["descripcion"].ToString() });
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void DDLGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerGenerales 3," + DDLGrupo.SelectedValue);

                foreach (DataRow item in vDatos.Rows)
                {
                    TxDescripcion.Text = item["descripcion"].ToString();
                    TxHorario.Text = item["horario"].ToString();
                    TxDias.Text = item["dias"].ToString();
                    TxObservaciones.Text = item["observaciones"].ToString();
                }

                if (vDatos.Rows.Count == 0)
                {
                    TxDescripcion.Text = "N/A";
                    TxHorario.Text = "N/A";
                    TxDias.Text = "N/A";
                    TxObservaciones.Text = "N/A";
                }

              

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtModificarEmpleado_Click(object sender, EventArgs e)
        {
            try
            {
                if (DDLEmpleado.SelectedValue.Equals("0"))
                    throw new Exception("Seleccione un empleado");
                if (DDLGrupo.SelectedValue.Equals("0"))
                    throw new Exception("Seleccione un grupo");
                if (DDLCrearRelojes.SelectedValue.Equals("0"))
                    throw new Exception("Seleccione un biometrico");


                String vErrorSuccess = "";
                biometricos vBiometrico = new biometricos(DDLCrearRelojes.SelectedValue);
                Int32 vRelojReturn = vBiometrico.ModificarGrupoUsuarioBiometrico(Convert.ToInt32(DDLEmpleado.SelectedValue), Convert.ToInt32(DDLGrupo.SelectedValue), ref vErrorSuccess);

                if (vRelojReturn == 1)
                {
                    Mensaje("Usuario modificado con exito", WarningType.Success);
                }
                else
                {
                    Mensaje("Fallo la modificacion en el Biometrico", WarningType.Danger);
                }
                LimpiarModificacionBiometrico();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarModificacionBiometrico();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        private void LimpiarModificacionBiometrico()
        {
            TxDescripcion.Text = String.Empty;
            TxHorario.Text = String.Empty;
            TxDias.Text = String.Empty;
            TxObservaciones.Text = String.Empty;
            DDLGrupo.SelectedIndex = -1;
            DDLEmpleado.SelectedIndex = -1;
            DDLCrearRelojes.SelectedIndex = -1;
        }
    }
}