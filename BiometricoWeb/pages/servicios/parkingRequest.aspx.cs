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

namespace BiometricoWeb.pages.servicios
{
    public partial class parkingRequest : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    cargarDatos();
                }
            }
        }

        private void cargarDatos() {
            try{
                String vQuery = "[RSP_Parqueos] 1," + Session["USUARIO"].ToString();
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                DivSolicitud.Visible = vDatos.Rows.Count > 0 ? true : false;
                GvCars.DataSource = null;
                GvCars.DataBind();
                if (vDatos.Rows.Count > 0){
                    GvCars.DataSource = vDatos;
                    GvCars.DataBind();
                    Session["PARKING_CARS"] = vDatos;

                    DDLVehiculo.Items.Clear();
                    DDLVehiculo.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLVehiculo.Items.Add(new ListItem { Value = item["id"].ToString(), Text = item["modelo"].ToString() + " - " + item["placa"].ToString() });
                    }

                    vQuery = "[RSP_Parqueos] 6," + Session["USUARIO"].ToString();
                    vDatos = vConexion.obtenerDataTable(vQuery);
                    DivSolicitud.Visible = vDatos.Rows.Count > 0 ? false : true;
                    DivInformación.Visible = DivSolicitud.Visible ? false : true;
                }
                vDatos = (DataTable)Session["AUTHCLASS"];
                LbUser.Text = vDatos.Rows[0]["nombre"].ToString();
                LbArea.Text = vDatos.Rows[0]["departamento"].ToString();


                vQuery = "[RSP_Parqueos] 6," + Session["USUARIO"].ToString();
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    GvSolicitud.DataSource = vDatos;
                    GvSolicitud.DataBind();
                }

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        public void MensajeLoad(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", " document.addEventListener(\"DOMContentLoaded\", function (event) { infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "'); });", true);
        }

        protected void GvCars_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                limpiarModal();
                String vId = e.CommandArgument.ToString();
                if (e.CommandName == "editarVehiculo"){
                    DivEstado.Visible = true;
                    LbCarId.Text = vId;
                    LbTitle.Text = "Editar Vehículo";

                    String vQuery = "[RSP_Parqueos] 3," + vId;
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                    if (vDatos.Rows.Count > 0){
                        TxMarca.Text = vDatos.Rows[0]["modelo"].ToString();
                        TxColor.Text = vDatos.Rows[0]["color"].ToString();
                        TxPlaca.Text = vDatos.Rows[0]["placa"].ToString();
                        DDLEstado.SelectedValue = vDatos.Rows[0]["estado"].ToString();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#ModalVehiculos').modal('show');", true);
                    }
                }

                if (e.CommandName == "eliminarVehiculo"){
                    LbCarIdDelete.Text = vId;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#ModalConfirmar').modal('show');", true);
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GvCars_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void BtnEnviar_Click(object sender, EventArgs e){
            try{
                if (DDLVehiculo.SelectedValue == "0")
                    throw new Exception("Por favor seleccione un vehículo.");
                if (DDLZona.SelectedValue == "0")
                    throw new Exception("Por favor seleccione una zona.");

                String vUser = Session["USUARIO"].ToString();
                String vQuery = "[RSP_Parqueos] 7" +
                    "," + vUser +
                    ",2,0,1" +
                    "," + DDLZona.SelectedValue +
                    "," + vUser; 
                int vInfo = vConexion.ejecutarSql(vQuery);
                if (vInfo == 1){
                    Mensaje("Solicitud enviada con éxito.", WarningType.Success);
                    cargarDatos();
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnNewCar_Click(object sender, EventArgs e){
            try{
                limpiarModal();
                LbTitle.Text = "Crear Vehículo";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#ModalVehiculos').modal('show');", true);
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        private void limpiarModal() {
            DivMensaje.Visible = false; LbMensaje.Text = ""; LbTitle.Text = "";
            TxColor.Text = ""; TxMarca.Text = ""; TxPlaca.Text = ""; DivEstado.Visible = false;
        }

        protected void BtnAceptar_Click(object sender, EventArgs e){
            try{
                if (TxMarca.Text == "" || TxMarca.Text == string.Empty)
                    throw new Exception("Por favor ingrese la marca del vehículo.");
                if (TxColor.Text == "" || TxColor.Text == string.Empty)
                    throw new Exception("Por favor ingrese el color del vehículo.");
                if (TxPlaca.Text == "" || TxPlaca.Text == string.Empty)
                    throw new Exception("Por favor ingrese la placa del vehículo.");

                String vMensaje = "";
                String vQuery = "[RSP_Parqueos] {0}" +
                    ",'" + TxMarca.Text + "'" +
                    ",'" + TxColor.Text + "'" +
                    ",'" + TxPlaca.Text + "'" +
                    ",'" + DDLEstado.SelectedValue + "'" +
                    "," + Session["USUARIO"].ToString();
                if (LbTitle.Text == "Crear Vehículo") {
                    vQuery = string.Format(vQuery, "2," + Session["UsUARIO"].ToString());
                    vMensaje = "Vehículo agregado con éxito.";
                } else if (LbTitle.Text == "Editar Vehículo") { 
                    vQuery = string.Format(vQuery,"4," + LbCarId.Text);
                    vMensaje = "Vehículo actualizado con éxito.";
                }

                int vInfo = vConexion.ejecutarSql(vQuery);
                if (vInfo == 1){
                    Mensaje(vMensaje, WarningType.Success);
                    cargarDatos();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#ModalVehiculos').modal('hide');", true);
                }
            }catch (Exception ex){
                DivMensaje.Visible = true;
                LbMensaje.Text = ex.Message;
            }
        }

        protected void BtnConfirmar_Click(object sender, EventArgs e){
            try{
                String vQuery = "[RSP_Parqueos] 5," + LbCarIdDelete.Text;
                int vInfo = vConexion.ejecutarSql(vQuery);
                if (vInfo == 1){
                    Mensaje("Vehículo eliminado con éxito", WarningType.Success);
                    cargarDatos();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#ModalConfirmar').modal('hide');", true);
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnEditarReq_Click(object sender, EventArgs e){
            try{

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GvSolicitud_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                String vId = e.CommandArgument.ToString();
                if (e.CommandName == "editRequest"){
                    LbIdReq.Text = vId;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#ModalEditReq').modal('show');", true);
                }

                if (e.CommandName == "eliminarSolicitud"){
                    LbIdRequest.Text = vId;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#ModalDeleteReq').modal('show');", true);
                }

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnDeleteReq_Click(object sender, EventArgs e)
        {

        }
    }
}