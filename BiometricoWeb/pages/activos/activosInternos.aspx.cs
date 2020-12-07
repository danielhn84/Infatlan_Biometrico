using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace BiometricoWeb.pages.activos
{
    public partial class activosInternos : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    generales vGenerales = new generales();
                    DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                    if (!vGenerales.PermisosPersonalSeguridad(vDatos))
                        Response.Redirect("/login.aspx");

                    TxBusqueda.Focus();
                    cargarDatos();
                }
            }
        }

        private void cargarDatos(){
            try{
                String vQuery = "[RSP_ActivosPI] 7";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    GvAsignaciones.DataSource = vDatos;
                    GvAsignaciones.DataBind();
                    Session["ACTIVOS_PI_ASIGNACIONES"] = vDatos;
                }

                vQuery = "[RSP_ActivosPI] 8";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    GvHistorico.DataSource = vDatos;
                    GvHistorico.DataBind();
                    Session["ACTIVOS_PI_HISTORICO"] = vDatos;
                }

                vQuery = "[RSP_ActivosPI] 10";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLEmpleado.Items.Clear();
                    DDLEmpleado.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLEmpleado.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
                    }
                }

                vQuery = "[RSP_ActivosPI] 12";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLCategoria.Items.Clear();
                    DDLCategoria.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLCategoria.Items.Add(new ListItem { Value = item["idCategoria"].ToString(), Text = item["nombre"].ToString() });
                    }
                }

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        
        protected void TxBusqueda_TextChanged(object sender, EventArgs e){
            try{
                if (TxBusqueda.Text != "" || TxBusqueda.Text != string.Empty){
                    String vQuery = "[RSP_ActivosPI] 5,'" + TxBusqueda.Text + "'";
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                    DivResultado.Visible = false;
                    DivEquipoPersonal.Visible = false;
                    DivRegistrar.Visible = false;
                    DivInfoIN.Visible = false;
                    Session["ACTIVOS_PI_PERSONAL"] = vDatos.Rows.Count < 1 ? true : false;
                    if (vDatos.Rows.Count < 1){
                        vQuery = "[RSP_ActivosPI] 15,'" + TxBusqueda.Text + "'";
                        vDatos = vConexion.obtenerDataTable(vQuery);
                        DivResultado.Visible = true;
                        if (vDatos.Rows.Count > 0){
                            LbMensaje.Text = "El equipo no está asignado a un empleado!";
                        }else{ 
                            TxBusqueda.Focus();
                            LbMensaje.Text = "El equipo con serie " + TxBusqueda.Text +  " no está registrado! Si es equipo personal regístrelo.";
                            DivEquipoPersonal.Visible = true;
                            DivRegistrar.Visible = true;
                            TxSerie.Text = TxBusqueda.Text;
                            limpiarFormulario();
                        }
                    }else{
                        Session["ACTIVOS_ID"] = vDatos.Rows[0]["idEquipo"].ToString();
                        if (vDatos.Rows[0]["nombre"].ToString() == ""){
                            DivResultado.Visible = true;
                            LbMensaje.Text = "El equipo no está asignado a un empleado!";
                        }else if (vDatos.Rows[0]["autorizado"].ToString() == ""){
                            DivResultado.Visible = true;
                            LbMensaje.Text = "El equipo no está autorizado para salir!";
                        }else {
                            DivRegistrar.Visible = true;
                            DivInfoIN.Visible = true;
                            LbIdEquipoEnt.Text = vDatos.Rows[0]["idEquipo"].ToString();
                            LbNombre.Text = vDatos.Rows[0]["nombre"].ToString();
                            LbSerieSalida.Text = vDatos.Rows[0]["serie"].ToString();
                            LbMarca.Text = vDatos.Rows[0]["marca"].ToString();
                            LbTipo.Text = vDatos.Rows[0]["TipoEquipo"].ToString();
                            LbCodInventario.Text = vDatos.Rows[0]["inventario"].ToString();
                            DivBody.Visible = true;
                        }
                    }
                    UpdatePanel1.Update();
                }else
                    limpiarFormulario();

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e){
            try{
                String vQuery = "";
                if (Convert.ToBoolean(Session["ACTIVOS_PI_PERSONAL"])){
                    validarPersonal();
                    vQuery = "[RSP_ActivosPI] 11" +
                        "," + DDLCategoria.SelectedValue +
                        "," + DDLTipo.SelectedValue +
                        ",'" + TxMarca.Text + "'" +
                        ",'" + TxSerie.Text + "'" +
                        ",'" + TxModelo.Text + "'" +
                        "," + Session["USUARIO"].ToString() +
                        "," + DDLEmpleado.SelectedValue  + 
                        ",1";
                }else{ 
                    vQuery = "[RSP_ActivosPI] 6" +
                        "," + LbIdEquipoEnt.Text +
                        ",1," + Session["USUARIO"].ToString() + "";
                }

                int vInfo = vConexion.ejecutarSql(vQuery);
                if (vInfo > 0){
                    cargarDatos();
                    UpdateDivBusquedas.Update();
                    Mensaje("Entrada guardada con éxito", WarningType.Success);
                }else
                    Mensaje("Hubo un error al guardar la entrada.", WarningType.Danger);
                
                limpiarFormulario();
                LbMensaje.Text = "";
                DivResultado.Visible = false;
                DivRegistrar.Visible = false;
                DivEquipoPersonal.Visible = false;
                UpdatePanel7.Update();
                TxBusqueda.Focus();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        private void validarPersonal(){
            if (DDLEmpleado.SelectedValue == "0")
                throw new Exception("Por favor seleccione el empleado.");
            if (TxSerie.Text == "" || TxSerie.Text == string.Empty)
                throw new Exception("Por favor ingrese la serie del equipo.");
            if (DDLCategoria.SelectedValue == "0")
                throw new Exception("Por favor seleccione la categoría.");
            if (DDLTipo.SelectedValue == "0")
                throw new Exception("Por favor seleccione el Tipo de Equipo.");
            if (TxMarca.Text == "" || TxMarca.Text == string.Empty)
                throw new Exception("Por favor ingrese la marca del equipo.");
            if (TxModelo.Text == "" || TxModelo.Text == string.Empty)
                throw new Exception("Por favor ingrese el modelo del equipo.");
        }

        private void limpiarFormulario(){
            DDLEmpleado.SelectedValue = "0";
            DDLCategoria.SelectedValue = "0";
            DDLTipo.SelectedValue = "0";
            TxModelo.Text = string.Empty;
            TxMarca.Text = string.Empty;
            
            TxBusqueda.Text = string.Empty;
            DivInfoIN.Visible = false;
            LbIdEquipoEnt.Text = string.Empty;
            LbNombre.Text = string.Empty;
            LbMarca.Text = string.Empty;
            LbSerieSalida.Text = string.Empty;
            LbCodInventario.Text = string.Empty;

            UpdatePanel3.Update();
            UpdatePanel1.Update();
        }

        protected void BtnCancelar_Click(object sender, EventArgs e){
            limpiarFormulario();
            DivRegistrar.Visible = false;
        }

        private void enviaCorreo(DataTable vDatos) {
            //ENVIAR CORREO
            //String vQuery = "RSP_ObtenerEmpleados 2," + DDLAutorizado.SelectedValue;
            DataTable vDatosEmpleado = vConexion.obtenerDataTable("");

            SmtpService vService = new SmtpService();
            foreach (DataRow item in vDatosEmpleado.Rows){
                if (!item["emailEmpresa"].ToString().Trim().Equals("")){
                    vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                        typeBody.Seguridad,
                        item["nombre"].ToString(),
                        "ENTRADA-" + vDatos.Rows[0]["id"].ToString() + "-" + vDatos.Rows[0]["tabla"].ToString()
                        );
                }
            }

        }

        protected void TxBuscarSalida_TextChanged(object sender, EventArgs e){
            try{
                if (TxBuscarSalida.Text != "" || TxBuscarSalida.Text != string.Empty){
                    String vQuery = "[RSP_ActivosPI] 5,'" + TxBuscarSalida.Text + "'";
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                    DivInfoOUT.Visible = vDatos.Rows.Count > 0 ? true : false;
                    DivRegistroSalida.Visible = vDatos.Rows.Count > 0 ? true : false;
                    if (vDatos.Rows.Count > 0){
                        LbNombreOut.Text = vDatos.Rows[0]["nombre"].ToString();
                        LbIdEquipoOut.Text = vDatos.Rows[0]["idEquipo"].ToString();
                        LbTipoOut.Text = vDatos.Rows[0]["TipoEquipo"].ToString();
                        LbMarcaOut.Text = vDatos.Rows[0]["marca"].ToString();
                        LbSerieOut.Text = vDatos.Rows[0]["serie"].ToString();
                        LbInventarioOut.Text = vDatos.Rows[0]["inventario"].ToString();
                        
                        vQuery = "[RSP_ActivosPI] 14," + vDatos.Rows[0]["idEquipo"].ToString();
                        vDatos = vConexion.obtenerDataTable(vQuery);
                        if (vDatos.Rows.Count > 0)
                            LbFechaIn.Text = vDatos.Rows[0]["fechaRegistro"].ToString();

                    }else {
                        Mensaje("Este equipo no ha sido registrado.", WarningType.Danger);
                    }
                    UpdatePanel4.Update();


                }else 
                    cargarDatos();

                UpdateDivBusquedas.Update();
            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void TxBuscaHistorico_TextChanged(object sender, EventArgs e){

        }

        protected void GvHistorico_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GvHistorico.PageIndex = e.NewPageIndex;
                GvHistorico.DataSource = (DataTable)Session["ACTIVOS_PI_HISTORICO"];
                GvHistorico.DataBind();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void TxBuscaAsignacion_TextChanged(object sender, EventArgs e){
            try{
                if (TxBuscaAsignacion.Text != "" || TxBuscaAsignacion.Text != string.Empty){
                    String vQuery = "[RSP_ActivosPI] 9,'" + TxBuscaAsignacion.Text + "'";
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                    GvAsignaciones.DataSource = vDatos;
                    GvAsignaciones.DataBind();
                    Session["ACTIVOS_PI_ASIGNACIONES"] = vDatos;
                }else 
                    cargarDatos();

                UpdateDivBusquedas.Update();
            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        protected void GvAsignaciones_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GvAsignaciones.PageIndex = e.NewPageIndex;
                GvAsignaciones.DataSource = (DataTable)Session["ACTIVOS_PI_ASIGNACIONES"];
                GvAsignaciones.DataBind();

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void DDLCategoria_SelectedIndexChanged(object sender, EventArgs e){
            try{
                String vQuery = "[RSP_ActivosPI] 13," + DDLCategoria.SelectedValue;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLTipo.Items.Clear();
                    DDLTipo.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLTipo.Items.Add(new ListItem { Value = item["idTipoEquipo"].ToString(), Text = item["nombre"].ToString() });
                    }
                }

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnGuardarOut_Click(object sender, EventArgs e){
            try{
                //validarPersonal();
                String vQuery = "[RSP_ActivosPI] 6" +
                    "," + LbIdEquipoOut.Text +
                    ",2," + Session["USUARIO"].ToString() + "";

                int vInfo = vConexion.ejecutarSql(vQuery);
                if (vInfo > 0){
                    cargarDatos();
                    Mensaje("Salida guardada con éxito", WarningType.Success);
                }else
                    Mensaje("Hubo un error al guardar la salida. Comuníquese con sistemas", WarningType.Danger);
                
                limpiarFormularioOut();
                
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnCancelarOut_Click(object sender, EventArgs e){
            limpiarFormularioOut();
            DivRegistrar.Visible = false;
        }

        private void limpiarFormularioOut(){
            TxBuscarSalida.Text = string.Empty;
            DivInfoOUT.Visible = false;
            DivRegistroSalida.Visible = false;

            LbIdEquipoOut.Text = string.Empty;
            LbNombreOut.Text = string.Empty;
            LbMarcaOut.Text = string.Empty;
            LbSerieOut.Text = string.Empty;
            LbInventarioOut.Text = string.Empty;
            LbMensajeOut.Text = "";

            UpdatePanel2.Update();
            UpdatePanel7.Update();
            UpdatePanel4.Update();
            TxBuscarSalida.Focus();
        }
    }
}