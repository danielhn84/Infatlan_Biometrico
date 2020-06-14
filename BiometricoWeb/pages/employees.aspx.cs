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
            if (!Page.IsPostBack){
                if (Convert.ToBoolean(Session["AUTH"])){
                    generales vGenerales = new generales();
                    DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                    if (!vGenerales.PermisosRecursosHumanos(vDatos))
                        Response.Redirect("/default.aspx");

                    CargarRelojes();
                    CargarTurnos();
                    CargarPuesto();
                    CargarJefes();
                    CargarAreas();
                }
            }
        }
        
        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        
        public void CerrarModal(String vModal){
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#" + vModal + "').modal('hide');", true);
        }

        void CargarRelojes(){
            try{
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerRelojes 1");

                DDLCrearRelojes.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLCrearRelojes.Items.Add(new ListItem { Value = item["duo"].ToString(), Text = item["nombre"].ToString() });
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        
        void CargarTurnos(){
            try{
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerEmpleados 3");

                DDLTurnos.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLTurnos.Items.Add(new ListItem { Value = item["idTurno"].ToString(), Text = item["nombre"].ToString() });
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        
        void CargarPuesto(){
            try{
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerEmpleados 4");

                DDLPuestos.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLPuestos.Items.Add(new ListItem { Value = item["idPuesto"].ToString(), Text = item["nombre"].ToString() });
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        
        void CargarJefes(){
            try{
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerGenerales 20");

                DDLJefatura2.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLJefatura2.Items.Add(new ListItem { Value = item["idEmpleado"].ToString(), Text = item["codigoSAP"].ToString() + " - " + item["nombre"].ToString() });
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        
        void CargarAreas() {
            try{
                DataTable vDatos = new DataTable();
                vDatos = vConexion.obtenerDataTable("RSP_ObtenerGenerales 12");

                DDLCrearArea.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows){
                    DDLCrearArea.Items.Add(new ListItem { Value = item["idDepartamento"].ToString(), Text = item["nombre"].ToString() });
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        int CargarInformacionDDL(DropDownList vList, String vValue){
            int vIndex = 0;
            try{
                int vContador = 0;
                foreach (ListItem item in vList.Items){
                    if (item.Value.Equals(vValue)){
                        vIndex = vContador;
                    }
                    vContador++;
                }
            }catch { throw; }
            return vIndex;
        }

        protected void BtnGuardarCambio_Click(object sender, EventArgs e){
            try{
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
                    DDLJefatura2.SelectedValue + "," +
                    "'" + TxAdUser.Text + "'";

                Int32 vInformacion = vConexion.ejecutarSql(vQuery);

                if (vInformacion == 1){
                    if (!DDLCrearRelojes.SelectedValue.Equals("0")){
                        String vErrorSuccess = "";
                        biometricos vRelojes = new biometricos(DDLCrearRelojes.SelectedValue);
                        Int32 vRelojReturn = vRelojes.CrearUsuarioBiometrico(TxCrearNoEmpleado.Text, TxCrearNombre.Text, Convert.ToInt32(DDLCrearRole.SelectedValue), ref vErrorSuccess);

                        if (vRelojReturn == 1){
                            Mensaje("Usuario ingresado con exito", WarningType.Success);
                        }else{
                            Mensaje("Usuario ingresado con exito, fallo ingreso en Reloj", WarningType.Warning);
                            Mensaje(vErrorSuccess, WarningType.Danger);
                        }                       
                    }else{
                        Mensaje("Usuario ingresado con exito, falta ingresar en Reloj", WarningType.Warning);
                    }
                    LimpiarIngresoEmpleados();
                }
            }catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        void LimpiarIngresoEmpleados(){
            try{
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
                DDLJefatura2.SelectedIndex = -1;
                TxAdUser.Text = "";
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        private void ValidacionesEmpleado(){
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

        protected void BtnCancelarCambio_Click(object sender, EventArgs e){
            try{
                LimpiarIngresoEmpleados();
                Mensaje("Se ha cancelado la inserción", WarningType.Info);
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
    }
}

