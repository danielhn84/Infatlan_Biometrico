using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Text;


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

        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable vData = new DataTable();
                DataTable vDatos = (DataTable)Session["PERSONAL_VISITA"];
                int vVisita = 1;
                string vEmpresa = DdlEmpresa.SelectedItem.Text;
                string vNombre = TxNombre.Text;
                string Videntidad = TxIdentidad.Text;
                //string vCantidad = txtcantidad.Text;
                //int vUbic = Convert.ToInt32(Session["ATM_INVUBI_MATERIAL"]);

                vData.Columns.Add("idVisita");
                vData.Columns.Add("nombre");
                vData.Columns.Add("identidad");
                vData.Columns.Add("empresa");


                if (vDatos == null)
                    vDatos = vData.Clone();

                if (vDatos != null)
                {
                    if (vDatos.Rows.Count < 1)
                        vDatos.Rows.Add(Session["PERSONAL_VISITA"].ToString(), vVisita, TxNombre.Text, TxIdentidad.Text, DdlEmpresa.SelectedItem.Text);
                    else
                    {
                        

                            vDatos.Rows.Add(Session["PERSONAL_VISITA"].ToString(), vVisita, TxNombre.Text, TxIdentidad.Text, DdlEmpresa.SelectedItem.Text);
                    }
                }

                GvVisitas.DataSource = vDatos;
                GvVisitas.DataBind();
                Session["PERSONAL_VISITA"] = vDatos;
                UPVisitas.Update();

                DdlEmpresa.SelectedIndex = -1;
                TxNombre.Text = "";
                TxIdentidad.Text = "";


            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, WarningType.Success);
            }
        }
    }
}