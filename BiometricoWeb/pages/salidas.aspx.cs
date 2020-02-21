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

namespace BiometricoWeb.pages
{
    public partial class salidas : System.Web.UI.Page
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
                String vQuery = "RSP_Seguridad 4";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                if (vDatos.Rows.Count > 0){
                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                    Session["SEG_SALIDAS"] = vDatos;
                }

                vQuery = "RSP_Seguridad 19";
                vDatos = vConexion.obtenerDataTable(vQuery);

                if (vDatos.Rows.Count > 0){
                    GVAprobaciones.DataSource = vDatos;
                    GVAprobaciones.DataBind();
                    Session["SEG_APR_SALIDAS"] = vDatos;
                }


                vQuery = "[RSP_Seguridad] 9";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLMotivo.Items.Clear();
                    DDLMotivo.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLMotivo.Items.Add(new ListItem { Value = item["idDetalle"].ToString(), Text = item["descripcion"].ToString() });
                    }
                }

                vQuery = "RSP_Seguridad 5";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    DDLArticulo.Items.Clear();
                    DDLArticulo.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                    foreach (DataRow item in vDatos.Rows){
                        DDLArticulo.Items.Add(new ListItem { Value = item["idDetalle"].ToString(), Text = item["descripcion"].ToString() });
                    }
                }

                //vQuery = "[RSP_Seguridad] 18";
                //vDatos = vConexion.obtenerDataTable(vQuery);
                //if (vDatos.Rows.Count > 0){
                //    DDLAutorizado.Items.Clear();
                //    DDLAutorizado.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                //    foreach (DataRow item in vDatos.Rows){
                //        DDLAutorizado.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["nombre"].ToString() });
                //    }
                //}
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
                GVBusqueda.DataSource = (DataTable)Session["SEG_SALIDAS"];
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
                DataTable vAprob = (DataTable)Session["SEC_APROBACION_SALIDA"];

                if (Session["ID_ENTRADA"] != null){
                    String vId = Session["ID_ENTRADA"].ToString();
                    vQuery = "[RSP_Seguridad] 10," + vId +
                        ",'" + TxNombre.Text + "'" +
                        "," + DDLMotivo.SelectedValue +
                        ",'" + TxObservaciones.Text + "'" +
                        "," + vAprob.Rows[0]["id"].ToString();

                    vInfo = vConexion.obtenerDataTable(vQuery);
                    if (vInfo.Rows.Count > 0){
                        if (vAprob != null){
                            String vIdAprob = vAprob.Rows[0]["id"].ToString();
                            vQuery = "[RSP_Seguridad] 20," + vIdAprob;
                            vConexion.ejecutarSql(vQuery);
                        }
                        //else
                        //    enviaCorreo(vInfo);
                        
                        cargarDatos();
                        UpdateDivBusquedas.Update();
                        Mensaje("Salida guardada con éxito", WarningType.Success);
                    }else
                        Mensaje("Hubo un error al guardar la salida.", WarningType.Danger);
                }else{
                    String vRSP = DDLMotivo.SelectedValue == "9" ? "[RSP_Seguridad] 13" : "[RSP_Seguridad] 2";
                    
                    vQuery = vRSP +
                        ",'" + TxNombre.Text + "'" +
                        "," + DDLArticulo.SelectedValue +
                        ",'" + TxSerie.Text + "'" +
                        ",'" + TxInventario.Text + "'" +
                        "," + DDLMotivo.SelectedValue +
                        ",'" + TxObservaciones.Text + "'" +
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
                TxBusqueda.Focus();

            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        private void validaciones() {
            if (TxNombre.Text == "" || TxNombre.Text == string.Empty)
                throw new Exception("Favor ingrese el Nombre.");
            if (DDLArticulo.SelectedValue.Equals("0"))
                throw new Exception("Favor seleccione el Articulo.");
            if (TxSerie.Text == "" || TxSerie.Text == string.Empty)
                throw new Exception("Favor ingrese la serie.");
            if (TxInventario.Text == "" || TxInventario.Text == string.Empty)
                throw new Exception("Favor ingrese el número de inventario.");
            if (DDLMotivo.SelectedValue.Equals("0"))
                throw new Exception("Favor seleccione el Motivo de salida.");
            if (LbAprobacion.Text != "Aprobado!")
                throw new Exception("El articulo no ha sido aprobado para salir.");

            //if (LbAprobacion.Text == "No Aprobado!" && DDLAutorizado.SelectedValue.Equals("0"))
            //    throw new Exception("Favor ingrese la persona que autorizó la salida de equipo.");
        }

        private void limpiarFormulario(){
            TxMensaje.Text = string.Empty;
            TxBusqueda.Text = string.Empty;
            LbAprobacion.Text = string.Empty;

            LbIdEntrada.Text = string.Empty;
            LbNombreEntrada.Text = string.Empty;
            LbArticuloEntrada.Text = string.Empty;
            LbSerieEntrada.Text = string.Empty;
            LbInventarioEntrada.Text = string.Empty;
            LbFechaEntrada.Text = string.Empty;
            DivEntradas.Visible = false;

            TxNombre.Text = string.Empty;
            DDLArticulo.SelectedIndex = -1;
            TxSerie.Text = string.Empty;
            TxInventario.Text = string.Empty;
            DDLMotivo.SelectedIndex = -1;
            TxObservaciones.Text = string.Empty;
            //DDLAutorizado.SelectedIndex = -1;

            UpdatePanel3.Update();
            UpdatePanel1.Update();
            UpdatePanel2.Update();
            UpdatePanel6.Update();
        }

        protected void TxBusqueda_TextChanged(object sender, EventArgs e){
            try{
                if (TxBusqueda.Text != "" || TxBusqueda.Text != string.Empty){
                    String vQuery = "[RSP_Seguridad] 16,'" + TxBusqueda.Text + "'";
                    DataTable vVerificacion = vConexion.obtenerDataTable(vQuery);
                    if (vVerificacion.Rows.Count > 0){
                        TxBusqueda.Focus();
                        limpiarFormulario();
                        Mensaje("El número de serie tiene una entrada pendiente. Favor ingrese otro.", WarningType.Warning);
                    }else{
                        vQuery = "[RSP_Seguridad] 21,'" + TxBusqueda.Text + "'";
                        DataTable vDatos1 = vConexion.obtenerDataTable(vQuery);
                        if (vDatos1.Rows.Count > 0){
                            LbAprobacion.Text = "Aprobado!";
                            LbAprobacion.Attributes.CssStyle.Value = "color:Green; margin-top:100px; margin-left:20px;";
                            
                            //divAutorizado.Visible = false;
                            Session["SEC_APROBACION_SALIDA"] = vDatos1;
                        }else{
                            LbAprobacion.Text = "No Aprobado!";
                            LbAprobacion.Attributes.CssStyle.Value = "color:Tomato; margin-bottom:10px; margin-left:20px;";
                            //divAutorizado.Visible = true;
                        }
                        LbAprobacion.Visible = true;

                        vQuery = "[RSP_Seguridad] 6,'" + TxBusqueda.Text + "'";
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

                            TxInventario.Text = vDatos.Rows[0]["inventario"].ToString();
                            DDLArticulo.SelectedValue = vDatos.Rows[0]["idArticulo"].ToString();

                            DivBody.Visible = true;
                            TxMensaje.Text = "";
                            TxMensaje.Visible = false;
                            UpdatePanel1.Update();
                        }else{
                            TxInventario.Text = string.Empty;
                            DDLArticulo.SelectedIndex = -1;
                            Session["ID_ENTRADA"] = null;
                            DivEntradas.Visible = false;
                            DivBody.Visible = false;
                            TxMensaje.Visible = true;
                            TxMensaje.Text = LbAprobacion.Text == "Aprobado!" ? "Cree un nuevo registro." : string.Empty ;
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
                    String vQuery = "[RSP_Seguridad] 8,'" + TxBuscaSerie.Text + "'";
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                    Session["SEG_SALIDAS"] = vDatos;
                }else
                    cargarDatos();

                UpdateDivBusquedas.Update();
            }catch (Exception Ex){
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }

        private void enviaCorreo(DataTable vDatos){
            //String vQuery = "RSP_ObtenerEmpleados 2," + DDLAutorizado.SelectedValue;
            DataTable vDatosEmpleado = vConexion.obtenerDataTable("");

            SmtpService vService = new SmtpService();
            foreach (DataRow item in vDatosEmpleado.Rows){
                if (!item["emailEmpresa"].ToString().Trim().Equals("")){
                    vService.EnviarMensaje(item["emailEmpresa"].ToString(),
                        typeBody.Seguridad,
                        item["nombre"].ToString(),
                        "SALIDA-" + vDatos.Rows[0]["id"].ToString() + "-" + vDatos.Rows[0]["tabla"].ToString()
                        );
                }
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

        protected void GVAprobaciones_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                if (e.CommandName == "SalidaEquipo"){
                    string vIdPermiso = e.CommandArgument.ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
            }catch (Exception ex){

                throw;
            }
        }
    }
}