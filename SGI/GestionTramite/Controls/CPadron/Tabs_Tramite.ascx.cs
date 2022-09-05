using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using System.ComponentModel;

namespace SGI.GestionTramite.Controls.CPadron
{
    public class Tabs_TramiteEventsArgs : EventArgs
    {
        public string nroexpediente { get; set; }
        public int IdCPadron { get; set; }
    }

    public partial class Tabs_Tramite : System.Web.UI.UserControl
    {
        public int Validar_Estado;


        private int id_cpadron
        {
            get
            {
                int ret = 0;
                int.TryParse(hid_cpadron_id.Value, out ret);
                return ret;
            }
            set
            {
                hid_cpadron_id.Value = value.ToString();
            }

        }

       
        public bool editar
        {
            get
            {
                bool ret = false;
                ret = hid_editar.Value.Equals("True") ? true : false;
                return ret;
            }
            set
            {
                hid_editar.Value = value.ToString();
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void LoadData(int id_cpadron, int id_encomienda, int Validar_Estado)
        {
            this.id_cpadron = id_cpadron;
            this.Validar_Estado = Validar_Estado;

            DGHP_Entities db = new DGHP_Entities();

            string nro_expediente = db.CPadron_Solicitudes.Where(x => x.id_cpadron == this.id_cpadron).Select(x => x.nro_expediente_anterior).FirstOrDefault();
            lblNumeroExpediente.Text = nro_expediente;
            txtNumeroExpediente.Text = nro_expediente;
            updDatoExpediente.Update();

            Tab_Ubicaciones.editar = this.editar;
            Tab_Ubicaciones.CargarDatos(this.id_cpadron, Validar_Estado);
            Tab_DatosLocal.CargarDatos(this.id_cpadron, Validar_Estado, true);
            Tab_Rubros.CargarDatos(this.id_cpadron, id_encomienda, Validar_Estado, true);
            Tab_Titulares.CargarDatos(this.id_cpadron, Validar_Estado, true);
            Tab_TitularesSol.CargarDatos(this.id_cpadron, true);
            Tab_ConformacionesLocal.CargarDatos(this.id_cpadron, Validar_Estado);
            updDatoTramiteCP.Update();
        }

        public string scriptCarga(bool habilitarControl, bool visibleConformacionLocal)
        {
            string script = "cargaTabs();" + (habilitarControl ? "habilitarControles(true);" : "habilitarControles(false);")
                +(visibleConformacionLocal? "visibleConformacionLocal(true);": "visibleConformacionLocal(false);");
            return script;
        }

        protected void Tab_Ubicaciones_UbicacionActualizada(object sender, EventArgs e)
        {
            Tab_Rubros.CargarDatos(this.id_cpadron,0, this.Validar_Estado, true);
            Tab_ConformacionesLocal.CargarDatos(this.id_cpadron, Validar_Estado);
        }

        protected void Tab_DatosLocal_DatosLocalActualizada(object sender, EventArgs e)
        {
            Tab_Rubros.CargarDatos(this.id_cpadron,0, this.Validar_Estado, true);
        }

        protected void Tab_Rubros_RubrosActualizado(object sender, EventArgs e)
        {
        }

        public delegate void EventHandlerTabsTramite(object sender, Tabs_TramiteEventsArgs e);
        public event EventHandlerTabsTramite NroExpedienteSaveClick;

        protected virtual void OnNroExpedienteSaveClick(EventArgs e)
        {
            if (NroExpedienteSaveClick != null)
            {

                int IdCPadron = 0;
                string edit_nroexpediente = txtNumeroExpediente.Text.Trim();
                int.TryParse(hid_cpadron_id.Value, out IdCPadron);

                DGHP_Entities db = new DGHP_Entities();
                try
                {


                    using (TransactionScope Tran = new TransactionScope())
                    {
                        try
                        {

                            CPadron_Solicitudes original = db.CPadron_Solicitudes.Where(x => x.id_cpadron == IdCPadron).FirstOrDefault();

                            if (original != null)
                            {
                                original.nro_expediente_anterior = edit_nroexpediente;
                                db.SaveChanges();
                            }
                            db.Entry(original).Reload();
                            lblNumeroExpediente.Text = edit_nroexpediente;
                            Tran.Complete();
                        }
                        catch (Exception ex)
                        {
                            Tran.Dispose();
                            LogError.Write(ex, ex.Message);
                            //lblError.Text = Functions.GetErrorMessage(ex);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmError_Carga", "$('#frmError_Carga').modal('show');", true);
                        }
                    }
                    updDatoExpediente.Update();
                    //CargarDatosTramite(this.id_tramitetarea);
                    Tabs_TramiteEventsArgs args = new Tabs_TramiteEventsArgs();
                    args.IdCPadron = IdCPadron;
                    args.nroexpediente = edit_nroexpediente;

                    NroExpedienteSaveClick(this, args);
                }
                catch (Exception ex)
                {
                    //
                    LogError.Write(ex, ex.Message);
                    //lblError.Text = Functions.GetErrorMessage(ex);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmError_Carga", "$('#frmError_Carga').modal('show');", true);

                }
                finally
                {
                    db.Dispose();
                }

            }
        }

        protected void lnkNroExpSave_Click(object sender, EventArgs e)
        {
            OnNroExpedienteSaveClick(e);
        }
        

    }
}