using BiometricoWeb.clases;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using System.Globalization;
using System.Text;

namespace BiometricoWeb.pages.tiempoExtraordinario
{
    public partial class mantenimiento : System.Web.UI.Page
    {
        db vConexion;
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            vConexion = new db();

            if (!IsPostBack)
            {
                CargarTiposTrabajo();
            }
        }

        void CargarTiposTrabajo()
        {
            try
            {
                DataTable vDatos = new DataTable();
                String vQuery = "RSP_TiempoExtraordinarioGenerales 53";
                vDatos = vConexion.obtenerDataTable(vQuery);

                if (vDatos.Rows.Count > 0)
                {
                    GVBusquedaTrabajos.DataSource = vDatos;
                    GVBusquedaTrabajos.DataBind();
                    UpdateDivTiposTrabajo.Update();
                    Session["STETIPOSTRABAJO"] = vDatos;
                }

  
                vQuery = "RSP_TiempoExtraordinarioGenerales 56";
                vDatos = vConexion.obtenerDataTable(vQuery);

                DDLTrabajos.Items.Add(new ListItem { Value = "0", Text = "Seleccione una opción" });
                foreach (DataRow item in vDatos.Rows)
                {
                    DDLTrabajos.Items.Add(new ListItem { Value = item["idTipoTrabajo"].ToString(), Text = item["nombreTrabajo"].ToString() });
                }


            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }
        protected void BtnCrear_Click(object sender, EventArgs e)
        {
            try
            {
                validacionesCrear();
                LbInformacion.Text = "Tipo Trabajo: <b>" + TxTrabajo.Text + "</b><br /><br />";                
                LbInformacionPregunta.Text = "<b>¿Está seguro que desea desea crear el tipo de trabajo?</b>";
                UpdateAutorizarMensaje.Update();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                limpirarCrear();
                UpdatePanel21.Update();
                Mensaje("Creacion cancelada con exito.", WarningType.Success);
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        private void validacionesCrear()
        {
            if (TxTrabajo.Text.Equals(""))
                throw new Exception("Falta que ingrese el tipo de trabajo.");
        }
        private void limpirarCrear()
        {
            TxTrabajo.Text = string.Empty;            
        }
        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable vDatos = new DataTable();
                String vQuery = "RSP_TiempoExtraordinarioGenerales 52,"
                    + "'" + TxTrabajo.Text
                    + "','" + Session["USUARIO"]
                    + "',1" ;

                Int32 vRespuesta = vConexion.ejecutarSql(vQuery);

                if (vRespuesta == 1)
                {
                    Mensaje("Tipo de trabajo agregado con exito", WarningType.Success);

                }
                else
                {
                    Mensaje("Ha ocurrido un error. Favor comunicarse con sistemas.", WarningType.Danger);

                }
                limpirarCrear();
                UpdatePanel21.Update();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        protected void GVBusquedaTrabajos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVBusquedaTrabajos.PageIndex = e.NewPageIndex;
                GVBusquedaTrabajos.DataSource = (DataTable)Session["STETIPOSTRABAJO"];
                GVBusquedaTrabajos.DataBind();
                UpdateDivTiposTrabajo.Update();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        protected void GVBusquedaTrabajos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vId = e.CommandArgument.ToString();
            Session["STEIDTIPOTRABAJO"] = vId;
            if (e.CommandName == "Modificar")
            {
                DataTable vDatos = new DataTable();
                string vQuery = "RSP_TiempoExtraordinarioGenerales 54," + vId;
                vDatos = vConexion.obtenerDataTable(vQuery);
                
                Titulo.Text = "Modificar tipo trabajo No.: " + vId;
                string estado = vDatos.Rows[0]["estado"].ToString() == "True" ? "1" : "0";
                TxTrabajoModal.Text = vDatos.Rows[0]["nombreTrabajo"].ToString();             
                DDLEstado.SelectedValue = estado;

                DivAlerta.Visible = false;
                UpdateModal.Update();
                UpdatePanel1.Update();
                UpdatePanel3.Update();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModificarModal();", true);

            }
        }
        protected void BtnEnviarModificacion_Click(object sender, EventArgs e)
        {
            try
            {
                validacionesModificar();

                DataTable vDatos = new DataTable();
                String vQuery = "RSP_TiempoExtraordinarioGenerales 55,'"
                    + Session["STEIDTIPOTRABAJO"].ToString()
                    + "','" + TxTrabajoModal.Text
                    + "'," + DDLEstado.SelectedValue
                    + "," + Session["USUARIO"];
                Int32 vRespuesta = vConexion.ejecutarSql(vQuery);

                if (vRespuesta == 1)
                {
                    Mensaje("Tipo de trabajo modificada con exito", WarningType.Success);

                }
                else
                {
                    Mensaje("Ha ocurrido un error. Favor comunicarse con sistemas.", WarningType.Danger);

                }


                CargarTiposTrabajo();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModificarModal();", true);

            }
            catch (Exception ex)
            {
                LbMensajeModalError.Text = ex.Message;
                DivAlerta.Visible = true;
                UpdateModal.Visible = true;
                UpdateModal.Update();
            }
        }
        private void validacionesModificar()
        {
            if (TxTrabajoModal.Text.Equals(""))
                throw new Exception("Falta que ingrese el tipo de trabajo.");
        }
        protected void TxBuscar_TextChanged(object sender, EventArgs e)
        {
            CargarTiposTrabajo();
            String vBusqueda = TxBuscar.Text;
            DataTable vDatos = (DataTable)Session["STETIPOSTRABAJO"];

            if (vBusqueda.Equals(""))
            {
                GVBusquedaTrabajos.DataSource = vDatos;
                GVBusquedaTrabajos.DataBind();
                UpdateDivTiposTrabajo.Update();
            }
            else
            {
                EnumerableRowCollection<DataRow> filtered = vDatos.AsEnumerable()
                    .Where(r => r.Field<String>("nombreTrabajo").Contains(vBusqueda.ToUpper()));

                Boolean isNumeric = int.TryParse(vBusqueda, out int n);

                if (isNumeric)
                {
                    if (filtered.Count() == 0)
                    {
                        filtered = vDatos.AsEnumerable().Where(r =>
                            Convert.ToInt32(r["idTipoTrabajo"]) == Convert.ToInt32(vBusqueda));
                    }
                }

                DataTable vDatosFiltrados = new DataTable();
                vDatosFiltrados.Columns.Add("idTipoTrabajo");
                vDatosFiltrados.Columns.Add("nombreTrabajo");
                

                foreach (DataRow item in filtered)
                {
                    vDatosFiltrados.Rows.Add(
                        item["idTipoTrabajo"].ToString(),
                        item["nombreTrabajo"].ToString()

                        );
                }
                GVBusquedaTrabajos.DataSource = vDatosFiltrados;
                GVBusquedaTrabajos.DataBind();
                Session["STETIPOSTRABAJO"] = vDatosFiltrados;
                UpdateDivTiposTrabajo.Update();
            }
        }
        private void validacionesCrearSubTarea()
        {
            if (DDLTrabajos.SelectedValue.Equals(""))
                throw new Exception("Falta que seleccione tipo de trabajo.");

            if (TxTarea.Text.Equals(""))
                throw new Exception("Falta que ingrese tarea.");
        }
        protected void BtnCrearTarea_Click(object sender, EventArgs e)
        {
            try
            {
                validacionesCrearSubTarea();
                Lb1.Text = "Tipo Trabajo: <b>" + DDLTrabajos.SelectedItem.Text + "</b><br /><br />"+
                    "Tarea: <b>" + TxTarea.Text + "</b><br /><br />";
                Lb2.Text = "<b>¿Está seguro que desea desea crear la subtarea del trabajo seleccionado?</b>";
                UpdatePanel7.Update();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalTarea();", true);
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        protected void BtnEnviarTarea_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable vDatos = new DataTable();
                String vQuery = "RSP_TiempoExtraordinarioGenerales 57,"
                    + DDLTrabajos.SelectedValue
                    + ",'" + TxTarea.Text
                    + "'," + Session["USUARIO"]
                    +",1";

                Int32 vRespuesta = vConexion.ejecutarSql(vQuery);

                if (vRespuesta == 1)
                {
                    Mensaje("Subtarea de trabajo agregado con exito", WarningType.Success);

                }
                else
                {
                    Mensaje("Ha ocurrido un error. Favor comunicarse con sistemas.", WarningType.Danger);

                }
                limpiarSubTarea();
                UpdatePanel4.Update();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModalTarea();", true);

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        private void limpiarSubTarea()
        {
            DDLTrabajos.SelectedIndex = -1;
            TxTarea.Text = string.Empty;
             
        }
        protected void BtnCancelarTarea_Click(object sender, EventArgs e)
        {
            try
            {
                limpiarSubTarea();
                UpdatePanel4.Update();
                Mensaje("Creacion cancelada con exito.", WarningType.Success);
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
    }
}