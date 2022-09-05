using SGI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls
{
    public class ucCargaDocumentosEventsArgs : EventArgs
    {
        public int id_tdocreq { get; set; }
        public string nombre_tdocreq { get; set; }
        public string detalle_tdocreq { get; set; }
        public string nombre_archivo { get; set; }
        public byte[] Documento { get; set; }
    }

    public partial class ucCargaDocumentos : System.Web.UI.UserControl
    {
        private DGHP_Entities db = null;

        public delegate void EventHandlerSubirDocumento(object sender, ucCargaDocumentosEventsArgs e);
        public event EventHandlerSubirDocumento SubirDocumentoClick;
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(pnlucCargaDocumento, pnlucCargaDocumento.GetType(), "init_Js_ucCargaDocumento", "init_Js_ucCargaDocumento();", true);
        }

        public void LoadData()
        {
            CargarCombo();
        }
        public void CargarCombo()
        {
            this.db = new DGHP_Entities();
            var q = (
                    from tdoc in db.TiposDeDocumentosRequeridos
                    join rel in db.Rel_TipoTramite_TiposDeDocumentosRequeridos on tdoc.id_tdocreq equals rel.id_tdocreq
                    where tdoc.visible_en_SGI == true
                    orderby tdoc.RequiereDetalle, tdoc.nombre_tdocreq
                    select tdoc
                );
            List<TiposDeDocumentosRequeridos> list_doc_adj = q.ToList();

            if (list_doc_adj.Count > 1)
                list_doc_adj.Insert(0, (new TiposDeDocumentosRequeridos
                {
                    id_tdocreq = 0,
                    nombre_tdocreq = "Seleccione",
                    observaciones_tdocreq = "Seleccione",
                    baja_tdocreq = false,
                    RequiereDetalle = false,
                    visible_en_SSIT = false,
                    visible_en_SGI = true
                }));


            ddlTiposDeDocumentosRequeridos.DataSource = list_doc_adj;
            ddlTiposDeDocumentosRequeridos.DataValueField = "id_tdocreq";
            ddlTiposDeDocumentosRequeridos.DataTextField = "nombre_tdocreq";
            ddlTiposDeDocumentosRequeridos.DataBind();
            ddlTiposDeDocumentosRequeridos.Items.Insert(0, "");
            //updpnlAgregarDocumentos.Update();
            this.db.Dispose();
        }

        protected virtual void OnSubirDocumentoClick(EventArgs e)
        {
            if (SubirDocumentoClick != null)
            {
                //Guid userid = (Guid)Membership.GetUser().ProviderUserKey;

                pnlErrorUcCD.Style["display"] = "none";
                string savedFileName = hid_filename_documentoUcCD.Value;

                //Elimina las fotos de firmas con mas de 1 dÃ­a para mantener el directorio limpio.
                string[] lstArchs = Directory.GetFiles("C:\\Temporal");
                foreach (string arch in lstArchs)
                {
                    DateTime fechaCreacion = File.GetCreationTime(arch);
                    if (fechaCreacion < DateTime.Now.AddDays(-3))
                        File.Delete(arch);
                }
                lblErrorUcCD.Text = "";
                byte[] Documento = new byte[0];
                try
                {

                    if (hid_filename_documentoUcCD.Value.Length > 0)
                    {
                        Documento = File.ReadAllBytes(savedFileName);
                        if (Documento.Length > Convert.ToInt32(hid_size_max.Value))
                            throw new Exception("El tamaño máximo permitido para los documentos es de " + hid_size_max_MB + " MB");
                        File.Delete(savedFileName);
                    }
                }
                catch (Exception ex)
                {
                    lblErrorUcCD.Text = ex.Message;
                    ScriptManager.RegisterClientScriptBlock(pnlucCargaDocumento, pnlucCargaDocumento.GetType(), "mostrarError", "showfrmErrorDocumentos(); ", true);
                }

                if (lblErrorUcCD.Text == "")
                {
                    int id_tdocreq = int.Parse(ddlTiposDeDocumentosRequeridos.SelectedValue);
                    ucCargaDocumentosEventsArgs args = new ucCargaDocumentosEventsArgs();
                    args.nombre_tdocreq = ddlTiposDeDocumentosRequeridos.SelectedItem.Text;
                    args.id_tdocreq = id_tdocreq;
                    args.detalle_tdocreq = txtDetalle.Text;
                    args.nombre_archivo = hid_filenamereal_documentoUcCD.Value;
                    args.Documento = Documento;
                    SubirDocumentoClick(this, args);
                }
            }
        }
        protected void btnSubirDocumentoUcCD_Click(object sender, EventArgs e)
        {
            OnSubirDocumentoClick(e);
        }

        protected void ddlTiposDeDocumentosRequeridos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id_tdocreq = 0;
            if (int.TryParse(ddlTiposDeDocumentosRequeridos.SelectedValue, out id_tdocreq))
            {
                DGHP_Entities db = new DGHP_Entities();
                var TipoDoc = db.TiposDeDocumentosRequeridos.FirstOrDefault(x => x.id_tdocreq == id_tdocreq);
                if (TipoDoc.RequiereDetalle)
                    ScriptManager.RegisterStartupScript(updTiposDeDocumentosRequeridosCD, updTiposDeDocumentosRequeridosCD.GetType(), "showDetalleDocumentoCD", "showDetalleDocumentoCD();", true);
                else
                    ScriptManager.RegisterStartupScript(updTiposDeDocumentosRequeridosCD, updTiposDeDocumentosRequeridosCD.GetType(), "hideDetalleDocumentoCD", "hideDetalleDocumentoCD();", true);

                hid_requiere_detalle.Value = TipoDoc.RequiereDetalle.ToString();
                hid_formato_archivo.Value = TipoDoc.formato_archivo;
                hid_size_max.Value = Convert.ToString(TipoDoc.tamanio_maximo_mb * 1024 * 1024);
                hid_size_max_MB.Value = Convert.ToString(TipoDoc.tamanio_maximo_mb);
            }
            else
            {
                ScriptManager.RegisterStartupScript(updTiposDeDocumentosRequeridosCD, updTiposDeDocumentosRequeridosCD.GetType(), "hideDetalleDocumentoCD", "hideDetalleDocumentoCD();", true);
                hid_requiere_detalle.Value = "False";
                hid_formato_archivo.Value = "pdf";
                hid_size_max.Value = "2097152";
                hid_size_max_MB.Value = "2";
            }
        }
    }
}