using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BiometricoWeb.clases;

namespace BiometricoWeb.pages.servicios
{
    public partial class descriptorDetalle : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        void CargarDatos()
        {
            string vIdPuesto = Request.QueryString["i"];

            try
            {
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_DescriptorPuestos 3,'" + vIdPuesto + "'");

                // IFramePDF.Src ="../plantilla/"+vDatos.Rows[0]["nombre"].ToString();

                LbTitulo.Text = vDatos.Rows[0]["nombre"].ToString();

                String vFUPlano = vDatos.Rows[0]["documento"].ToString();
                string srcPlano = "data:application/pdf;base64," + vFUPlano;
                IFramePDF.Src = srcPlano;
                IFramePDF.Visible = true;
                //fuPlano.Visible = false;

            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, WarningType.Danger);
            }

        }
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
    }
}