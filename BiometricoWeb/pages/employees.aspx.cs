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
    public partial class employees : System.Web.UI.Page
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
                    CargarRelojes();
                    CargarTurnos();
                    CargarPuesto();
                    CargarJefes();
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

        void CargarEmpleados()
        {
            try
            {
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerEmpleados 1");

                GVBusqueda.DataSource = vDatos;
                GVBusqueda.DataBind();
                Session["DATAEMPLEADOS"] = vDatos;

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
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

        void CargarTurnos()
        {
            try
            {
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerEmpleados 3");

                DDLTurnos.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                DDLModTurnos.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows)
                {
                    DDLTurnos.Items.Add(new ListItem { Value = item["idTurno"].ToString(), Text = item["nombre"].ToString() });
                    DDLModTurnos.Items.Add(new ListItem { Value = item["idTurno"].ToString(), Text = item["nombre"].ToString() });
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        void CargarPuesto()
        {
            try
            {
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerEmpleados 4");

                DDLPuestos.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                DDLModPuestos.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows)
                {
                    DDLPuestos.Items.Add(new ListItem { Value = item["idPuesto"].ToString(), Text = item["nombre"].ToString() });
                    DDLModPuestos.Items.Add(new ListItem { Value = item["idPuesto"].ToString(), Text = item["nombre"].ToString() });
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        void CargarJefes()
        {
            try
            {
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerGenerales 1");

                DDLJefatura.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                DDLModJefatura.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows)
                {
                    DDLJefatura.Items.Add(new ListItem { Value = item["codigoSAP"].ToString(), Text = item["codigoSAP"].ToString() + " - " + item["nombre"].ToString() });
                    DDLModJefatura.Items.Add(new ListItem { Value = item["codigoSAP"].ToString(), Text = item["codigoSAP"].ToString() + " - " + item["nombre"].ToString() });
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string vIdEmpleado = e.CommandArgument.ToString();
                if (e.CommandName == "UsuarioModificar")
                {
                    LbModNoEmpleado.Text = vIdEmpleado;


                    DataTable vDatos = new DataTable();
                    vDatos = vConexion.obtenerDataTable("RSP_ObtenerEmpleados 2," + vIdEmpleado);


                    foreach (DataRow item in vDatos.Rows)
                    {
                        TxModIdentidad.Text = item["identidad"].ToString();
                        TxModCodigoSAP.Text = item["codigoSAP"].ToString();
                        TxModNombre.Text = item["nombre"].ToString();
                        TxModNacimiento.Text = Convert.ToDateTime((item["fechaNacimiento"].ToString() == "" ? "1900-01-01" : item["fechaNacimiento"].ToString())).ToString("yyyy-MM-dd");
                        TxModTelefono.Text = item["telefono"].ToString();
                        TxModEmailEmpresa.Text = item["emailEmpresa"].ToString();
                        TxModEmailPersonal.Text = item["emailPersonal"].ToString();
                        DDLModCiudad.SelectedIndex = CargarInformacionDDL(DDLModCiudad, item["ciudad"].ToString());
                        DDLModArea.SelectedIndex = CargarInformacionDDL(DDLModArea, item["area"].ToString());
                        DDLEstado.SelectedIndex = CargarInformacionDDL(DDLEstado, item["estado"].ToString());
                        DDLModTurnos.SelectedIndex = CargarInformacionDDL(DDLModTurnos, item["idTurno"].ToString());
                        DDLModPuestos.SelectedIndex = CargarInformacionDDL(DDLModPuestos, item["idPuesto"].ToString());
                        DDLModJefatura.SelectedIndex = CargarInformacionDDL(DDLModJefatura, item["idJefe"].ToString().PadLeft(8, '0'));
                        TxModADUser.Text = item["adUser"].ToString();
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openEmpleadosModal();", true);
                }
                if (e.CommandName == "UsuarioPassword")
                {
                    LbEmpleadoPassword.Text = vIdEmpleado;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openPasswordModal();", true);
                }


            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        int CargarInformacionDDL(DropDownList vList, String vValue)
        {
            int vIndex = 0;
            try
            {
                int vContador = 0;
                foreach (ListItem item in vList.Items)
                {
                    if (item.Value.Equals(vValue))
                    {
                        vIndex = vContador;
                    }
                    vContador++;
                }
            }
            catch { throw; }
            return vIndex;
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVBusqueda.PageIndex = e.NewPageIndex;
                GVBusqueda.DataSource = (DataTable)Session["DATAEMPLEADOS"];
                GVBusqueda.DataBind();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void TxBuscarEquipo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CargarEmpleados();

                String vBusqueda = TxBuscarEmpleado.Text;
                DataTable vDatos = (DataTable)Session["DATAEMPLEADOS"];

                if (vBusqueda.Equals(""))
                {
                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                    UpdateGridView.Update();
                }
                else
                {
                    EnumerableRowCollection<DataRow> filtered = vDatos.AsEnumerable()
                        .Where(r => r.Field<String>("nombre").Contains(vBusqueda.ToUpper()));

                    Boolean isNumeric = int.TryParse(vBusqueda, out int n);

                    if (isNumeric)
                    {
                        if (filtered.Count() == 0)
                        {
                            filtered = vDatos.AsEnumerable().Where(r =>
                                Convert.ToInt32(r["idEmpleado"]) == Convert.ToInt32(vBusqueda));
                        }
                    }


                    DataTable vDatosFiltrados = new DataTable();
                    vDatosFiltrados.Columns.Add("idEmpleado");
                    vDatosFiltrados.Columns.Add("nombre");
                    vDatosFiltrados.Columns.Add("area");
                    vDatosFiltrados.Columns.Add("ciudad");
                    vDatosFiltrados.Columns.Add("identidad");
                    vDatosFiltrados.Columns.Add("telefono");
                    vDatosFiltrados.Columns.Add("estado");
                    foreach (DataRow item in filtered)
                    {
                        vDatosFiltrados.Rows.Add(
                            item["idEmpleado"].ToString(),
                            item["nombre"].ToString(),
                            item["area"].ToString(),
                            item["ciudad"].ToString(),
                            item["identidad"].ToString(),
                            item["telefono"].ToString(),
                            item["estado"].ToString()
                            );
                    }

                    GVBusqueda.DataSource = vDatosFiltrados;
                    GVBusqueda.DataBind();
                    Session["DATAEMPLEADOS"] = vDatosFiltrados;
                    UpdateGridView.Update();
                }


            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnGuardarCambio_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacionesEmpleado();

                String vQuery = "RSP_IngresarEmpleados 1," + TxCrearNoEmpleado.Text + "," +
                    "'" + TxCrearNombre.Text.ToUpper() + "'," +
                    "'" + DDLCrearArea.SelectedValue + "'," +
                    "'" + DDLCrearCiudad.SelectedValue + "'," +
                    "'" + TxCrearComentarios.Text + "'," +
                    "'" + TxCrearIdentidad.Text + "'," +
                    "'" + TxCrearFechaNacimiento.Text + "'," +
                    "'" + TxCrearDireccion.Text + "'," +
                    "'" + TxCrearEmailEmpresa.Text + "'," +
                    "'" + TxCrearEmailPersonal.Text + "'," +
                    "'" + TxCrearTelefono.Text + "'," +
                    "'" + TxCrearCodigoSAP.Text + "',''," +
                    "'" + DDLTurnos.SelectedValue + "'," +
                    "'" + DDLPuestos.SelectedValue + "'," +
                    DDLJefatura.SelectedValue + "," +
                    "'" + TxAdUser.Text + "'";

                Int32 vInformacion = vConexion.ejecutarSql(vQuery);

                if (vInformacion == 1)
                {
                    if (!DDLCrearRelojes.SelectedValue.Equals("0"))
                    {
                        String vErrorSuccess = "";
                        biometricos vRelojes = new biometricos(DDLCrearRelojes.SelectedValue);
                        Int32 vRelojReturn = vRelojes.CrearUsuarioBiometrico(TxCrearNoEmpleado.Text, TxCrearNombre.Text, Convert.ToInt32(DDLCrearRole.SelectedValue), ref vErrorSuccess);

                        if (vRelojReturn == 1)
                        {
                            Mensaje("Usuario ingresado con exito", WarningType.Success);
                        }
                        else
                        {
                            Mensaje("Usuario ingresado con exito, fallo ingreso en Reloj", WarningType.Warning);
                            Mensaje(vErrorSuccess, WarningType.Danger);
                        }                       
                    }
                    else
                    {
                        Mensaje("Usuario ingresado con exito, falta ingresar en Reloj", WarningType.Warning);
                    }
                    LimpiarIngresoEmpleados();
                }

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        void LimpiarIngresoEmpleados()
        {
            try
            {
                TxCrearNoEmpleado.Text = "";
                TxCrearNombre.Text = "";
                TxCrearIdentidad.Text = "";
                TxCrearFechaNacimiento.Text = "";
                TxCrearDireccion.Text = "";
                TxCrearEmailEmpresa.Text = "";
                TxCrearEmailPersonal.Text = "";
                TxCrearTelefono.Text = "";
                TxCrearComentarios.Text = "";
                TxCrearCodigoSAP.Text = "";
                DDLCrearArea.SelectedIndex = -1;
                DDLCrearCiudad.SelectedIndex = -1;
                DDLCrearRelojes.SelectedIndex = -1;
                DDLCrearRole.SelectedIndex = -1;
                DDLTurnos.SelectedIndex = -1;
                DDLPuestos.SelectedIndex = -1;
                DDLJefatura.SelectedIndex = -1;
                DDLModJefatura.SelectedIndex = -1;
                TxAdUser.Text = "";
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        private void ValidacionesEmpleado()
        {
            if (TxCrearNoEmpleado.Text.Equals(""))
                throw new Exception("Por favor ingrese un codigo de empleado");
            if (TxCrearNombre.Text.Equals(""))
                throw new Exception("Por favor ingrese el nombre del empleado");
            if (TxCrearIdentidad.Text.Equals(""))
                throw new Exception("Por favor ingrese un numero de identidad");
            if (TxCrearFechaNacimiento.Text.Equals(""))
                throw new Exception("Por favor ingrese una fecha de nacimiento");
            if (TxCrearDireccion.Text.Equals(""))
                throw new Exception("Por favor ingrese una dirección");
            if (TxCrearEmailPersonal.Text.Equals(""))
                throw new Exception("Por favor ingrese un email personal");
            if (TxCrearTelefono.Text.Equals(""))
                throw new Exception("Por favor ingrese un telefono");
            if (TxCrearCodigoSAP.Text.Equals(""))
                throw new Exception("Por favor ingrese el codigo SAP");
            if (DDLCrearArea.SelectedValue.Equals("0"))
                throw new Exception("Por favor seleccione un area para el nuevo empleado");
            if (DDLCrearCiudad.SelectedValue.Equals("0"))
                throw new Exception("Por favor seleccione una ciudad para el nuevo empleado");
            if (DDLTurnos.SelectedValue.Equals("0"))
                throw new Exception("Por favor seleccione un turno para el nuevo empleado");
            if (DDLPuestos.SelectedValue.Equals("0"))
                throw new Exception("Por favor seleccione un puesto para el nuevo empleado");
        }

        protected void BtnCancelarCambio_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarIngresoEmpleados();
                Mensaje("Se ha cancelado la inserción", WarningType.Info);
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnModEmpleados_Click(object sender, EventArgs e)
        {
            try
            {
                String vQuery = "RSP_IngresarEmpleados 2," + LbModNoEmpleado.Text + "," +
                    "'" + TxModNombre.Text + "'," +
                    "'" + DDLModArea.SelectedValue + "'," +
                    "'" + DDLModCiudad.SelectedValue + "'," +
                    "'" + null + "'," +
                    "'" + TxModIdentidad.Text + "'," +
                    "'" + TxModNacimiento.Text + "'," +
                    "'" + null + "'," +
                    "'" + TxModEmailEmpresa.Text + "'," +
                    "'" + TxModEmailPersonal.Text + "'," +
                    "'" + TxModTelefono.Text + "'," +
                    "'" + TxModCodigoSAP.Text + "'," +
                    "'" + DDLEstado.SelectedValue + "'," +
                    "'" + DDLModTurnos.SelectedValue + "'," +
                    "'" + DDLModPuestos.SelectedValue + "'," +
                    DDLModJefatura.SelectedValue + "," +
                    "'" + TxModADUser.Text + "'";

                Int32 vInformacion = vConexion.ejecutarSql(vQuery);
                if (vInformacion == 1)
                {
                    Mensaje("Actualizado con Exito!", WarningType.Success);
                    CerrarModal("EmpleadoModal");
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnCambiarPassword_Click(object sender, EventArgs e)
        {
            try
            {
                generales vGenerales = new generales();
                if (TxModPassword.Text.Equals(TxModPasswordConfirmar.Text))
                {
                    String vPasswordMD5 = vGenerales.MD5Hash(TxModPassword.Text);
                    String vQuery = "RSP_IngresarEmpleados 3," + LbEmpleadoPassword.Text + ",'" + vPasswordMD5 + "'";
                    Int32 vInformacion = vConexion.ejecutarSql(vQuery);
                    if (vInformacion == 1)
                    {
                        Mensaje("Actualizado con Exito!", WarningType.Success);
                        CerrarModal("PasswordModal");
                    }
                    else
                    {
                        Mensaje("No se pudo actualizar la contraseña!", WarningType.Danger);
                        CerrarModal("PasswordModal");
                    }

                }
                else
                {
                    throw new Exception("Las contraseñas ingresadas no coinciden.");
                }
            }
            catch (Exception Ex) { LbUsuarioMensaje.Text = Ex.Message; UpdateUsuarioMensaje.Update(); }
        }
    }
}

