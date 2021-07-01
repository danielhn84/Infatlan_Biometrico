using BiometricoWeb.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace BiometricoWeb.pages.activos
{
    public partial class soporte : System.Web.UI.Page
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
                String vQuery = "RSP_Seguridad 23";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                    Session["ACTIVOS_ENTRADAS"] = vDatos;
                }

                vQuery = "RSP_Seguridad 5";
                vDatos = vConexion.obtenerDataTable(vQuery);
                DDLArticulos.Items.Clear();
                DDLArticulos.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLArticulos.Items.Add(new ListItem { Value = item["idDetalle"].ToString(), Text = item["descripcion"].ToString() });
                }

                vQuery = "RSP_ObtenerGenerales 12";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLDepartamento.Items.Clear();
                    DDLDepartamento.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLDepartamento.Items.Add(new ListItem { Value = item["idDepartamento"].ToString(), Text = item["nombre"].ToString() });
                    }
                }

                vQuery = "RSP_Seguridad 9";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLMotivo.Items.Clear();
                    DDLMotivoSalida.Items.Clear();
                    DDLMotivo.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    DDLMotivoSalida.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLMotivoSalida.Items.Add(new ListItem { Value = item["idDetalle"].ToString(), Text = item["descripcion"].ToString() });
                        DDLMotivo.Items.Add(new ListItem { Value = item["idDetalle"].ToString(), Text = item["descripcion"].ToString() });
                    }
                }

                vQuery = "RSP_Seguridad 19";
                vDatos = vConexion.obtenerDataTable(vQuery);

                if (vDatos.Rows.Count > 0){
                    GVAprobaciones.DataSource = vDatos;
                    GVAprobaciones.DataBind();
                    Session["SEG_APR_SALIDAS"] = vDatos;
                }

                vQuery = "RSP_Seguridad 5";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLArticuloSalida.Items.Clear();
                    DDLArticuloSalida.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLArticuloSalida.Items.Add(new ListItem { Value = item["idDetalle"].ToString(), Text = item["descripcion"].ToString() });
                    }
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GVBusqueda.PageIndex = e.NewPageIndex;
                GVBusqueda.DataSource = (DataTable)Session["ACTIVOS_ENTRADAS"];
                GVBusqueda.DataBind();

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e){
            try{
                validaciones();
                String vQuery = "";
                DataTable vInfo = new DataTable();

                if (Session["ID_SALIDA"] != null) {
                    vQuery = "[RSP_Seguridad] 7" +
                        "," + Session["ID_SALIDA"].ToString() +
                        ",'" + TxNombre.Text + "'" +
                        ",'" + TxDestinatario.Text + "'" +
                        "," + DDLDepartamento.SelectedValue +
                        "," + DDLMotivo.SelectedValue +
                        ",'" + TxObservaciones.Text + "'";
                    vInfo = vConexion.obtenerDataTable(vQuery);
                    if (vInfo.Rows.Count > 0){
                        //enviaCorreo(vInfo);
                        cargarDatos();
                        UpdateDivBusquedas.Update();
                        Mensaje("Entrada guardada con éxito", WarningType.Success);
                    }else
                        Mensaje("Hubo un error al guardar la entrada.", WarningType.Danger);
                }else{
                    String vRSP = DDLMotivo.SelectedValue == "9" ? "[RSP_Seguridad] 12," : "[RSP_Seguridad] 1,";

                    if (DDLMotivo.SelectedValue == "13"){
                        vQuery = "[RSP_Seguridad] 17" +
                        ",'" + Session["USUARIO"].ToString() + "'" +
                        "," + DDLArticulos.SelectedValue +
                        "," + 1 +
                        ",'" + TxSerie.Text + "'" +
                        ",'" + TxObservaciones.Text + "'";
                        vConexion.ejecutarSql(vQuery);
                    }

                    vQuery = vRSP + "'" + TxNombre.Text + "'" +
                    "," + DDLArticulos.SelectedValue +
                    ",'" + TxSerie.Text + "'" +
                    ",'" + TxInventario.Text + "'" +
                    ",'" + TxDestinatario.Text + "'" +
                    ",'" + Session["USUARIO"].ToString() + "'" +
                    "," + DDLMotivo.SelectedValue +
                    ",'" + TxObservaciones.Text + "'" +
                    "," + DDLDepartamento.SelectedValue +
                    "," + TxSysAid.Text;
                    vInfo = vConexion.obtenerDataTable(vQuery);
                    if (vInfo.Rows.Count > 0){
                        cargarDatos();
                        UpdateDivBusquedas.Update();
                        Mensaje("Entrada guardada con éxito", WarningType.Success);
                    }else
                        Mensaje("Hubo un error al guardar la entrada.", WarningType.Danger);
                }
                limpiarFormulario();
                TxBusqueda.Focus();

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        private void validaciones() {
            if (TxNombre.Text == "" || TxNombre.Text == string.Empty)
                throw new Exception("Favor ingrese el nombre.");
            if (DDLArticulos.SelectedValue.Equals("0"))
                throw new Exception("Favor seleccione el tipo de articulo.");
            if (TxSerie.Text == "" || TxSerie.Text == string.Empty)
                throw new Exception("Favor ingrese la serie del articulo.");
            if (TxInventario.Text == "" || TxInventario.Text == string.Empty)
                throw new Exception("Favor ingrese el número de inventario.");
            if (DDLDepartamento.SelectedValue.Equals("0"))
                throw new Exception("Favor seleccione el departamento de destino.");
            if (TxDestinatario.Text == "" || TxDestinatario.Text == string.Empty)
                throw new Exception("Favor ingrese el destinatario.");
            if (DDLMotivo.SelectedValue.Equals("0"))
                throw new Exception("Favor seleccione un motivo de entrada.");

        }

        private void limpiarFormulario(){
            TxMensaje.Text = string.Empty;
            TxBusqueda.Text = string.Empty;

            DivSalidas.Visible = false;
            LbIdSalida.Text = string.Empty;
            LbNombreSalida.Text = string.Empty;
            LbArticuloSalida.Text = string.Empty;
            LbSerieSalida.Text = string.Empty;
            LbInventarioSalida.Text = string.Empty;
            LbFechaSalida.Text = string.Empty;

            TxNombre.Text = string.Empty;
            TxSerie.Text = string.Empty;
            TxDestinatario.Text = string.Empty;
            TxInventario.Text = string.Empty;
            DDLArticulos.SelectedIndex = -1;
            DDLMotivo.SelectedIndex = -1;
            TxObservaciones.Text = string.Empty;
            DDLDepartamento.SelectedIndex = -1;
            TxSysAid.Text = string.Empty;

            UpdatePanel3.Update();
            UpdatePanel1.Update();
            UpdatePanel2.Update();
        }

        protected void TxBusqueda_TextChanged(object sender, EventArgs e){
            try{
                if (TxBusqueda.Text != "" || TxBusqueda.Text != string.Empty){
                    String vQuery = "[RSP_Seguridad] 15,'" + TxBusqueda.Text + "'";
                    DataTable vVerificacion = vConexion.obtenerDataTable(vQuery);
                    if (vVerificacion.Rows.Count > 0){
                        TxBusqueda.Focus();
                        limpiarFormulario();
                        Mensaje("El número de serie tiene una salida pendiente. Favor ingrese otro.", WarningType.Warning);
                    }else{
                        vQuery = "[RSP_Seguridad] 8,'" + TxBusqueda.Text + "'";
                        DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                        if (vDatos.Rows.Count > 0){
                            Session["ID_SALIDA"] = vDatos.Rows[0]["id"].ToString();

                            DivSalidas.Visible = true;
                            LbIdSalida.Text = vDatos.Rows[0]["id"].ToString();
                            LbNombreSalida.Text = vDatos.Rows[0]["nombreSalida"].ToString();
                            LbSerieSalida.Text = vDatos.Rows[0]["serie"].ToString();
                            LbArticuloSalida.Text = vDatos.Rows[0]["articulo"].ToString();
                            LbInventarioSalida.Text = vDatos.Rows[0]["inventario"].ToString();
                            LbFechaSalida.Text = vDatos.Rows[0]["fechaSalida"].ToString();

                            TxInventario.Text = vDatos.Rows[0]["inventario"].ToString();
                            DDLArticulos.SelectedValue = vDatos.Rows[0]["idArticulo"].ToString();

                            DivBody.Visible = true;
                            TxMensaje.Text = "";
                            TxMensaje.Visible = false;
                            UpdatePanel1.Update();
                        }else{
                            TxInventario.Text = string.Empty;
                            DDLArticulos.SelectedIndex = -1;
                            Session["ID_SALIDA"] = null;
                            DivSalidas.Visible = false;
                            DivBody.Visible = false;
                            TxMensaje.Visible = true;
                            TxMensaje.Text = "Cree un nuevo registro.";
                            UpdatePanel1.Update();
                        }
                        TxNombre.Focus();
                        TxSerie.Text = TxBusqueda.Text;
                    }
                    UpdatePanel2.Update();
                }else
                    limpiarFormulario();

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e){
            limpiarFormulario();
        }

        protected void TxBuscaSerie_TextChanged(object sender, EventArgs e){
            try{
                if (TxBuscaSerie.Text != "" || TxBuscaSerie.Text != string.Empty){
                    String vQuery = "[RSP_Seguridad] 6,'" + TxBuscaSerie.Text + "'";
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                    Session["ACTIVOS_ENTRADAS"] = vDatos;

                }else 
                    cargarDatos();

                UpdateDivBusquedas.Update();
            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
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

        #region SALIDAS
        protected void TxBusquedaSalida_TextChanged(object sender, EventArgs e){
            try{
                if (TxBusquedaSalida.Text != "" || TxBusquedaSalida.Text != string.Empty){
                    String vQuery = "[RSP_Seguridad] 16,'" + TxBusquedaSalida.Text + "'";
                    DataTable vVerificacion = vConexion.obtenerDataTable(vQuery);
                    if (vVerificacion.Rows.Count > 0){
                        TxBusquedaSalida.Focus();
                        limpiarFormularioSalida();
                        Mensaje("El número de serie tiene una entrada pendiente. Favor ingrese otro.", WarningType.Warning);
                    }else{
                        vQuery = "[RSP_Seguridad] 21,'" + TxBusquedaSalida.Text + "'";
                        DataTable vDatos1 = vConexion.obtenerDataTable(vQuery);
                        if (vDatos1.Rows.Count > 0){
                            LbAprobacion.Text = "Aprobado!";
                            LbAprobacion.Attributes.CssStyle.Value = "color:Green; margin-top:100px; margin-left:20px;";
                            Session["SEC_APROBACION_SALIDA"] = vDatos1;
                        }else{
                            LbAprobacion.Text = "No Aprobado!";
                            LbAprobacion.Attributes.CssStyle.Value = "color:Tomato; margin-bottom:10px; margin-left:20px;";
                        }
                        LbAprobacion.Visible = true;

                        vQuery = "[RSP_Seguridad] 6,'" + TxBusquedaSalida.Text + "'";
                        DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                        if (vDatos.Rows.Count > 0){
                            Session["ID_ENTRADA"] = vDatos.Rows[0]["id"].ToString();

                            DivEntradas.Visible = true;
                            LbIdEntrada.Text = vDatos.Rows[0]["id"].ToString();
                            LbNombreEntrada.Text = vDatos.Rows[0]["nombre"].ToString();
                            LbArticuloEntrada.Text = vDatos.Rows[0]["articulo"].ToString();
                            LbSerieEntrada.Text = vDatos.Rows[0]["serie"].ToString();
                            LbInventarioEntrada.Text = vDatos.Rows[0]["inventario"].ToString();
                            LbFechaEntrada.Text = vDatos.Rows[0]["fechaEntrada"].ToString();

                            TxInventarioSalida.Text = vDatos.Rows[0]["inventario"].ToString();
                            DDLArticuloSalida.SelectedValue = vDatos.Rows[0]["idArticulo"].ToString();

                            DivBodySalida.Visible = true;
                            LbMensajeSalida.Text = "";
                            LbMensajeSalida.Visible = false;
                            UpdatePanel7.Update();
                        }else{
                            TxInventarioSalida.Text = string.Empty;
                            DDLArticuloSalida.SelectedIndex = -1;
                            Session["ID_ENTRADA"] = null;
                            DivEntradas.Visible = false;
                            DivBodySalida.Visible = false;
                            LbMensajeSalida.Visible = true;
                            LbMensajeSalida.Text = LbAprobacion.Text == "Aprobado!" ? "Cree un nuevo registro." : string.Empty ;
                            UpdatePanel7.Update();
                        }
                        TxNombreSalida.Focus();
                        TxSerieSalida.Text = TxBusquedaSalida.Text;
                    }
                    UpdatePanel8.Update();

                }else
                    limpiarFormulario();

            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        private void limpiarFormularioSalida(){
            LbMensajeSalida.Text = string.Empty;
            TxBusqueda.Text = string.Empty;
            LbAprobacion.Text = string.Empty;

            LbIdEntrada.Text = string.Empty;
            LbNombreEntrada.Text = string.Empty;
            LbArticuloEntrada.Text = string.Empty;
            LbSerieEntrada.Text = string.Empty;
            LbInventarioEntrada.Text = string.Empty;
            LbFechaEntrada.Text = string.Empty;
            DivEntradas.Visible = false;

            TxNombreSalida.Text = string.Empty;
            DDLArticuloSalida.SelectedIndex = -1;
            TxSerieSalida.Text = string.Empty;
            TxInventarioSalida.Text = string.Empty;
            DDLMotivoSalida.SelectedIndex = -1;
            TxObservacionesSalida.Text = string.Empty;
            //DDLAutorizado.SelectedIndex = -1;

            UpdatePanel6.Update();
            UpdatePanel7.Update();
            UpdatePanel8.Update();
            UpdatePanel10.Update();
        }

        protected void BtnGuardarSalida_Click(object sender, EventArgs e){
            try{
                validacionesSalida();
                String vQuery = "";
                DataTable vInfo = new DataTable();
                DataTable vAprob = (DataTable)Session["SEC_APROBACION_SALIDA"];

                if (Session["ID_ENTRADA"] != null){
                    String vId = Session["ID_ENTRADA"].ToString();
                    vQuery = "[RSP_Seguridad] 10," + vId +
                        ",'" + TxNombreSalida.Text + "'" +
                        "," + DDLMotivoSalida.SelectedValue +
                        ",'" + TxObservacionesSalida.Text + "'" +
                        "," + vAprob.Rows[0]["id"].ToString();

                    vInfo = vConexion.obtenerDataTable(vQuery);
                    if (vInfo.Rows.Count > 0){
                        if (vAprob != null){
                            String vIdAprob = vAprob.Rows[0]["id"].ToString();
                            vQuery = "[RSP_Seguridad] 20," + vIdAprob;
                            vConexion.ejecutarSql(vQuery);
                        }
                        
                        cargarDatos();
                        UpdateDivBusquedas.Update();
                        Mensaje("Salida guardada con éxito", WarningType.Success);
                    }else
                        Mensaje("Hubo un error al guardar la salida.", WarningType.Danger);
                }else{
                    String vRSP = DDLMotivoSalida.SelectedValue == "9" ? "[RSP_Seguridad] 13" : "[RSP_Seguridad] 2";
                    
                    vQuery = vRSP +
                        ",'" + TxNombreSalida.Text + "'" +
                        "," + DDLArticuloSalida.SelectedValue +
                        ",'" + TxSerieSalida.Text + "'" +
                        ",'" + TxInventarioSalida.Text + "'" +
                        "," + DDLMotivoSalida.SelectedValue +
                        ",'" + TxObservacionesSalida.Text + "'" +
                        ",'" + Session["USUARIO"].ToString() + "'";
                    vInfo = vConexion.obtenerDataTable(vQuery);
                    if (vInfo.Rows.Count > 0){
                        if (vAprob != null){
                            String vIdAprob = vAprob.Rows[0]["id"].ToString();
                            vQuery = "[RSP_Seguridad] 20," + vIdAprob;
                            vConexion.ejecutarSql(vQuery);
                        }//else
                            //enviaCorreo(vInfo);
                        
                        cargarDatos();
                        UpdateDivBusquedas.Update();
                        Mensaje("Salida guardada con éxito", WarningType.Success);
                    }else
                        Mensaje("Hubo un error al guardar la entrada.", WarningType.Danger);
                }

                limpiarFormulario();
                TxBusquedaSalida.Focus();

            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        private void validacionesSalida() {
            if (TxNombreSalida.Text == "" || TxNombreSalida.Text == string.Empty)
                throw new Exception("Favor ingrese el Nombre.");
            if (DDLArticuloSalida.SelectedValue.Equals("0"))
                throw new Exception("Favor seleccione el Articulo.");
            if (TxSerieSalida.Text == "" || TxSerieSalida.Text == string.Empty)
                throw new Exception("Favor ingrese la serie.");
            if (TxInventarioSalida.Text == "" || TxInventarioSalida.Text == string.Empty)
                throw new Exception("Favor ingrese el número de inventario.");
            if (DDLMotivoSalida.SelectedValue.Equals("0"))
                throw new Exception("Favor seleccione el Motivo de salida.");
            if (LbAprobacion.Text != "Aprobado!")
                throw new Exception("El articulo no ha sido aprobado para salir.");
        }

        protected void BtnCancelarSalida_Click(object sender, EventArgs e){
            limpiarFormularioSalida();
        }

        protected void GVAprobaciones_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                if (e.CommandName == "SalidaEquipo"){
                    string vIdPermiso = e.CommandArgument.ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void GVAprobaciones_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GVAprobaciones.PageIndex = e.NewPageIndex;
                GVAprobaciones.DataSource = (DataTable)Session["SEG_APR_SALIDAS"];
                GVAprobaciones.DataBind();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void DDLProceso_SelectedIndexChanged(object sender, EventArgs e){
            try{
                if (DDLProceso.SelectedValue == "1")
                    Response.Redirect("/pages/activos/activosInternos.aspx");
                if (DDLProceso.SelectedValue == "2")
                    Response.Redirect("/pages/activos/visitas.aspx");
                if (DDLProceso.SelectedValue == "3")
                    Response.Redirect("/pages/activos/registroVisitaSeguridad.aspx");
                if (DDLProceso.SelectedValue == "5")
                    Response.Redirect("/pages/activos/nuevoActivo.aspx");
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
        #endregion
    }
}