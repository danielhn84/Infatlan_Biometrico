using BiometricoWeb.clases;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;

namespace BiometricoWeb.pages.activos
{
    public partial class registroVisitas : System.Web.UI.Page
    {
        db vConexion = new db();
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarInformacionGeneral();
            }
        }

        void CargarInformacionGeneral()
        {
            try
            {
                String vQuery = "RSP_SeguridadActivosDataCenter 1,'" + Convert.ToString(Session["USUARIO"]) + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                TxResponsable.Text= vDatos.Rows[0]["nombre"].ToString();
                TxIdentidadResponsable.Text = vDatos.Rows[0]["identidad"].ToString();
                TxSubgerencia.Text = vDatos.Rows[0]["area"].ToString();
                TxJefe.Text = vDatos.Rows[0]["jefeNombre"].ToString();
                //TxFechaSolicitud.Text= DateTime.Today.ToString("dd/MM/yyyy");
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnAddTrabajo_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "abrirModal();", true);
        }
    }
}