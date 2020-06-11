using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BiometricoWeb.clases;

namespace BiometricoWeb.pages
{
    public partial class descriptorPuestos : System.Web.UI.Page
    {
        db vConexion;
        protected void Page_Load(object sender, EventArgs e)
        {
            vConexion = new db();
            if (!Page.IsPostBack)
            {
                if (Convert.ToBoolean(Session["AUTH"]))
                {
                    CargarPuesto();

                }

            }
        }

        void CargarPuesto(){
            try{
                //Cargar datos  GV de los Puestos encargados.
                String vQuery = "RSP_DescriptorPuestos 1,"+"'" + Session["USUARIO"].ToString() +"'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                GVDescriptor.DataSource = vDatos;
                GVDescriptor.DataBind();
                Session["DATADESCRIPTORES"] = vDatos;

                //Cargar Datos GV del Puesto Actual.
                String vQueryActual = "RSP_DescriptorPuestos 4," + "'" + Session["USUARIO"].ToString() + "'";
                DataTable vDatosActual = vConexion.obtenerDataTable(vQueryActual);
               
                GVDescriptorActual.DataSource = vDatosActual;
                GVDescriptorActual.DataBind();
                Session["DATADESCRIPTORESACTUAL"] = vDatosActual;

                if (vDatos.Rows.Count.ToString() =="0")
                    updGVAsignados.Visible = false;
                

                foreach (GridViewRow row in GVDescriptorActual.Rows){
                    vQuery = "RSP_DescriptorPuestos 5,'" + row.Cells[0].Text + "'";
                    DataTable vDatosBusqueda = vConexion.obtenerDataTable(vQuery);

                    Button button2 = row.FindControl("BtnEntrarActual") as Button;
                    button2.Text = "Pendiente";
                    button2.CssClass = "btn btn-inverse-secondary  mr-2";
                    button2.Enabled = false;
                    button2.CommandName = "EntrarDescriptorActual";

                    foreach (DataRow item in vDatosBusqueda.Rows){
                        if (item["estado"].ToString().Equals("True")){
                            Button button = row.FindControl("BtnEntrarActual") as Button;
                            button.Text = "Entrar";
                            button.CssClass = "btn btn-inverse-primary  mr-2";
                            button.Enabled = true;
                            button.CommandName = "EntrarDescriptorActual";
                        }
                    }
                }

                foreach (GridViewRow row in GVDescriptor.Rows){
                    vQuery = "RSP_DescriptorPuestos 5,'" + row.Cells[0].Text +"'";
                    DataTable vDatosBusqueda = vConexion.obtenerDataTable(vQuery);

                    Button button2 = row.FindControl("BtnEntrar") as Button;
                    button2.Text = "Pendiente";
                    button2.CssClass = "btn btn-inverse-secondary  mr-2";
                    button2.Enabled = false;
                    button2.CommandName = "EntrarDescriptor";

                    foreach (DataRow item in vDatosBusqueda.Rows){
                        if (item["estado"].ToString().Equals("True")){
                            Button button = row.FindControl("BtnEntrar") as Button;
                            button.Text = "Entrar";
                            button.CssClass = "btn btn-inverse-primary  mr-2";
                            button.Enabled = true;
                            button.CommandName = "EntrarDescriptor";
                        }
                    }
                }
            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        
        protected void GVDescriptor_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GVDescriptor.PageIndex = e.NewPageIndex;
                GVDescriptor.DataSource = (DataTable)Session["DATADESCRIPTORES"];
                GVDescriptor.DataBind();

                foreach (GridViewRow row in GVDescriptor.Rows){
                    String vQuery = "RSP_DescriptorPuestos 5,'" + row.Cells[0].Text +"'";
                    DataTable vDatosBusqueda = vConexion.obtenerDataTable(vQuery);

                    Button button2 = row.FindControl("BtnEntrar") as Button;
                    button2.Text = "Pendiente";
                    button2.CssClass = "btn btn-inverse-secondary  mr-2";
                    button2.Enabled = false;
                    button2.CommandName = "EntrarDescriptor";

                    foreach (DataRow item in vDatosBusqueda.Rows){
                        if (item["estado"].ToString().Equals("True")){
                            Button button = row.FindControl("BtnEntrar") as Button;
                            button.Text = "Entrar";
                            button.CssClass = "btn btn-inverse-primary  mr-2";
                            button.Enabled = true;
                            button.CommandName = "EntrarDescriptor";
                        }
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }

        }

        protected void GVDescriptor_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {
                string vIdPuesto = e.CommandArgument.ToString();
                if (e.CommandName == "EntrarDescriptor")
                {
                    Response.Redirect("descriptorDetalle.aspx?i=" + vIdPuesto);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
                

            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, WarningType.Danger);
            }

        }

        protected void GVDescriptorActual_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GVDescriptorActual.PageIndex = e.NewPageIndex;
                GVDescriptorActual.DataSource = (DataTable)Session["DATADESCRIPTORESACTUAL"];
                
                foreach (GridViewRow row in GVDescriptorActual.Rows){
                    String vQuery = "RSP_DescriptorPuestos 5,'" + row.Cells[0].Text + "'";
                    DataTable vDatosBusqueda = vConexion.obtenerDataTable(vQuery);

                    Button button2 = row.FindControl("BtnEntrarActual") as Button;
                    button2.Text = "Pendiente";
                    button2.CssClass = "btn btn-inverse-secondary  mr-2";
                    button2.Enabled = false;
                    button2.CommandName = "EntrarDescriptorActual";

                    foreach (DataRow item in vDatosBusqueda.Rows){
                        if (item["estado"].ToString().Equals("True")){
                            Button button = row.FindControl("BtnEntrarActual") as Button;
                            button.Text = "Entrar";
                            button.CssClass = "btn btn-inverse-primary  mr-2";
                            button.Enabled = true;
                            button.CommandName = "EntrarDescriptorActual";
                        }
                    }
                }
                GVDescriptorActual.DataBind();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVDescriptorActual_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {
                string vIdPuesto = e.CommandArgument.ToString();
                if (e.CommandName == "EntrarDescriptorActual")
                {
                    Response.Redirect("descriptorDetalle.aspx?i=" + vIdPuesto);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }


            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, WarningType.Danger);
            }

        }
    }
}