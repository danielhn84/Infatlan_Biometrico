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
    public partial class proyectos : System.Web.UI.Page
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
                CargarProyectos();
            }
        }
        void CargarProyectos()
        {
            try
            {
                DataTable vDatos = new DataTable();
                String vQuery = "RSP_TiempoExtraordinarioGenerales 51";
                vDatos = vConexion.obtenerDataTable(vQuery);

                if (vDatos.Rows.Count > 0)
                {
                    GVBusquedaProyecto.DataSource = vDatos;
                    GVBusquedaProyecto.DataBind();
                    UpdateDivProyecto.Update();
                    Session["STEPROYECTOS"] = vDatos;
                }

            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, WarningType.Danger);
            }
        }
        private void validacionesCrear()
        {
            if (TxProyecto.Text.Equals(""))
                throw new Exception("Falta que ingrese proyecto.");

            if (TxDescripcion.Text.Equals(""))
                throw new Exception("Falta que ingrese descripcion de proyecto.");

            if (TxHrs.Text.Equals(""))
                throw new Exception("Falta que ingrese Hrs aprobadas segun proyecto.");

            if (TxPago.Text.Equals(""))
                throw new Exception("Falta que ingrese pago por hora de la proyecto.");
        }
        private void limpiarCreacion()
        {
            TxProyecto.Text = string.Empty;
            TxDescripcion.Text = string.Empty;
            TxHrs.Text = string.Empty;
            TxPago.Text = string.Empty;
        }
        protected void BtnCrear_Click(object sender, EventArgs e)
        {
            try
            {
                validacionesCrear();
                LbInformacion.Text = "Proyecto: <b>" + TxProyecto.Text + "</b><br /><br />" +
                "Detalle proyecto: <b>" + TxDescripcion.Text + "</b><br /><br />" +
                "Total Hrs: <b>" + TxHrs.Text + "</b><br /><br />" +
                "Pago Hr: <b>" + TxPago.Text + "</b><br /><br />";
                LbInformacionPregunta.Text = "<b>¿Está seguro que desea enviar la creacion del proyecto?</b>";
                UpdateAutorizarMensaje.Update();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable vDatos = new DataTable();
                String vQuery = "RSP_TiempoExtraordinarioGenerales 50,'1',"
                    + "'" + TxProyecto.Text
                    + "','" + TxDescripcion.Text
                    + "','" + TxHrs.Text
                    + "','" + TxPago.Text
                    + "','" + Session["USUARIO"]
                    + "',1";

                Int32 vRespuesta = vConexion.ejecutarSql(vQuery);

                if (vRespuesta == 1)
                {
                    Mensaje("Proyecto agregado con exito", WarningType.Success);

                }
                else
                {
                    Mensaje("Ha ocurrido un error. Favor comunicarse con sistemas.", WarningType.Danger);

                }
                limpiarCreacion();
                UpdatePanel21.Update();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                limpiarCreacion();
                UpdatePanel21.Update();
                Mensaje("Creación cancelada con exito.", WarningType.Success);
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        protected void GVBusquedaProyecto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVBusquedaProyecto.PageIndex = e.NewPageIndex;
                GVBusquedaProyecto.DataSource = (DataTable)Session["STEPROYECTOS"];
                GVBusquedaProyecto.DataBind();
                UpdateDivProyecto.Update();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        protected void GVBusquedaProyecto_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vId = e.CommandArgument.ToString();
            Session["STEIDPROYECTO"] = vId;
            if (e.CommandName == "Modificar")
            {
                DataTable vDatos = new DataTable();
                string vQuery = "RSP_TiempoExtraordinarioGenerales 48," + vId;
                vDatos = vConexion.obtenerDataTable(vQuery);
                Titulo.Text = "Modificar proyecto No.: " + vId;
                string estado = vDatos.Rows[0]["estado"].ToString() == "True" ? "1" : "0";
                TxProyectoModal.Text = vDatos.Rows[0]["nombreTrabajo"].ToString();
                TxDescripcionModal.Text = vDatos.Rows[0]["descripcionTrabajo"].ToString();
                TxHrsModal.Text = vDatos.Rows[0]["totalHrs"].ToString();
                TxPagoModal.Text = vDatos.Rows[0]["pagoHr"].ToString();
                DDLEstado.SelectedValue = estado;

                DivAlerta.Visible = false;
                UpdateModal.Update();

                if (Session["USUARIO"].ToString() == "80037" || Session["USUARIO"].ToString() == "305")
                {
                    TxHrsModal.ReadOnly = false;
                }
                else
                {
                    TxHrsModal.ReadOnly = true;
                }
                UpdatePanel1.Update();
                UpdatePanel3.Update();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModificarModal();", true);
            }
        }
        protected void BtnEnviarModificacion_Click(object sender, EventArgs e)
        {
            try
            {
                validacionesModalModificar();

                DataTable vDatos = new DataTable();
                String vQuery = "RSP_TiempoExtraordinarioGenerales 49,'"
                    + Session["STEIDPROYECTO"].ToString()
                    + "','" + TxProyectoModal.Text
                    + "','" + TxDescripcionModal.Text
                    + "','" + TxHrsModal.Text
                    + "','" + TxPagoModal.Text
                    + "'," + DDLEstado.SelectedValue
                    + "," + Session["USUARIO"]
                    + ",'" + TxComentario.Text + "'";
                Int32 vRespuesta = vConexion.ejecutarSql(vQuery);

                if (vRespuesta == 1)
                {
                    Mensaje("Proyecto modificado con exito", WarningType.Success);

                }
                else
                {
                    Mensaje("Ha ocurrido un error. Favor comunicarse con sistemas.", WarningType.Danger);

                }

                TxComentario.Text = string.Empty;
                CargarProyectos();
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
        private void validacionesModalModificar()
        {
            if (TxProyectoModal.Text.Equals(""))
                throw new Exception("Falta que ingrese proyecto.");

            if (TxDescripcionModal.Text.Equals(""))
                throw new Exception("Falta que ingrese descripcion de proyecto.");

            if (TxHrsModal.Text.Equals(""))
                throw new Exception("Falta que ingrese Hrs aprobadas segun proyecto.");

            if (TxPagoModal.Text.Equals(""))
                throw new Exception("Falta que ingrese pago por hora de la proyecto.");

            if (DDLEstado.SelectedValue.Equals(""))
                throw new Exception("Falta que seleccione un motivo.");
        }
        protected void TxBuscar_TextChanged(object sender, EventArgs e)
        {
            CargarProyectos();
            String vBusqueda = TxBuscar.Text;
            DataTable vDatos = (DataTable)Session["STEPROYECTOS"];

            if (vBusqueda.Equals(""))
            {
                GVBusquedaProyecto.DataSource = vDatos;
                GVBusquedaProyecto.DataBind();
                UpdateDivProyecto.Update();
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
                            Convert.ToInt32(r["idTipoTrabajoDescripcion"]) == Convert.ToInt32(vBusqueda));
                    }
                }

                DataTable vDatosFiltrados = new DataTable();
                vDatosFiltrados.Columns.Add("idTipoTrabajoDescripcion");
                vDatosFiltrados.Columns.Add("nombreTrabajo");
                vDatosFiltrados.Columns.Add("descripcionTrabajo");
                vDatosFiltrados.Columns.Add("totalHrs");
                vDatosFiltrados.Columns.Add("pagoHr");

                foreach (DataRow item in filtered)
                {
                    vDatosFiltrados.Rows.Add(
                        item["idTipoTrabajoDescripcion"].ToString(),
                        item["nombreTrabajo"].ToString(),
                        item["descripcionTrabajo"].ToString(),
                        item["totalHrs"].ToString(),
                        item["pagoHr"].ToString()

                        );
                }
                GVBusquedaProyecto.DataSource = vDatosFiltrados;
                GVBusquedaProyecto.DataBind();
                Session["STEPROYECTOS"] = vDatosFiltrados;
                UpdateDivProyecto.Update();
            }
        }
    }
}