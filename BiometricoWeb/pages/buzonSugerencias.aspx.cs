using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BiometricoWeb.clases;
using System.Data;

namespace BiometricoWeb.pages
{
    public partial class buzonSugerencias : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            if (!Convert.ToBoolean(Session["AUTH"]))
                Response.Redirect("/login.aspx");
            
            if (!Page.IsPostBack){
                DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                if (vDatos.Rows[0]["tipoEmpleado"].ToString().Equals("1"))
                    BuzonGenerales.Visible = true;

                cargarDatos();
            }
        }

        private void cargarDatos() {
            try{
                String vQuery = "RSP_Sugerencias 2";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                DDLMotivo.Items.Clear();
                DDLMotivo.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLMotivo.Items.Add(new ListItem { Value = item["idTipoSugerencia"].ToString(), Text = item["nombre"].ToString() });
                }

                // MI BUZON
                vQuery = "RSP_Sugerencias 3," + Session["USUARIO"].ToString();
                vDatos = vConexion.obtenerDataTable(vQuery);
                GvMensajes.DataSource = vDatos;
                GvMensajes.DataBind();
                Session["DATA_BUZON_EMPLEADO"] = vDatos;

                // BUZON GENERAL
                vQuery = "RSP_Sugerencias 4";
                vDatos = vConexion.obtenerDataTable(vQuery);
                GVBusqueda.DataSource = vDatos;
                GVBusqueda.DataBind();
                Session["DATA_BUZON_GENERAL"] = vDatos;

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void BtnEnviar_Click(object sender, EventArgs e){
            try{
                validar();
                String vUrge = DDLUrgencia.SelectedValue == "0" ? "" : DDLUrgencia.SelectedItem.Text;
                String vUser = Session["USUARIO"].ToString();

                String vQuery = "RSP_Sugerencias 1," + DDLMotivo.SelectedValue + 
                    "," + vUser + 
                    ",'" + TxMensaje.Text + 
                    "','" + vUrge + "'";
                int vInfo = vConexion.ejecutarSql(vQuery);
                if (vInfo == 1){
                    enviarCorreo();
                    Mensaje(DDLMotivo.SelectedItem.Text + " enviada con éxito!", WarningType.Success);
                }else
                    Mensaje("Hubo un error al enviar su mensaje.", WarningType.Danger);

                limpiarForm();
                cargarDatos();
                UpdatePanel1.Update();
                UPBuzonGeneral.Update();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void limpiarForm() {
            DDLMotivo.SelectedValue = "0";
            DDLUrgencia.SelectedValue = "0";
            TxMensaje.Text = string.Empty;
        }

        public void validar() {
            if (DDLMotivo.SelectedValue == "0")
                throw new Exception("Favor seleccione el motivo.");
            if (TxMensaje.Text == "" || TxMensaje.Text == string.Empty)
                throw new Exception("Favor ingrese un mensaje.");
        }

        protected void GvMensajes_PageIndexChanging(object sender, GridViewPageEventArgs e){
            GvMensajes.PageIndex = e.NewPageIndex;
            GvMensajes.DataSource = (DataTable)Session["DATA_BUZON_EMPLEADO"];
            GvMensajes.DataBind();
        }

        protected void GvMensajes_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                if (e.CommandName== "VerMensaje"){
                    DataTable vDatos = (DataTable)Session["DATA_BUZON_EMPLEADO"];
                    Int32 vId = Convert.ToInt32(e.CommandArgument.ToString());
                    EnumerableRowCollection<DataRow> filtered = vDatos.AsEnumerable().
                        Where(r => r.Field<Int32>("idSugerencia") == vId);

                    foreach (DataRow item in filtered){
                        LbMensaje.Text = item["sugerencia"].ToString();
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Pop", "openModal();", true);

                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                if (e.CommandName == "VerMensaje"){
                    DataTable vDatos = (DataTable)Session["DATA_BUZON_GENERAL"];
                    Int32 vId = Convert.ToInt32(e.CommandArgument.ToString());
                    EnumerableRowCollection<DataRow> filtered = vDatos.AsEnumerable().Where(r => r.Field<Int32>("idSugerencia") == vId);

                    foreach (DataRow item in filtered){
                        LbMensaje.Text = item["sugerencia"].ToString();
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Pop", "openModal();", true);
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){
            GVBusqueda.PageIndex = e.NewPageIndex;
            GVBusqueda.DataSource = (DataTable)Session["DATA_BUZON_GENERAL"];
            GVBusqueda.DataBind();
        }

        private void enviarCorreo(){
            //ENVIAR CORREO
            String vCorreo = "wpadilla@bancatlan.hn";
            SmtpService vService = new SmtpService();
            vService.EnviarMensaje(vCorreo,
                typeBody.Sugerencias,
                "Te informamos que se ha recibido una nueva sugerencia.",
                "Se ha creado una nueva sugerencia en el buzón de mensajes del módulo de sugerencias.",
                TxMensaje.Text
                );
        }

    }
}