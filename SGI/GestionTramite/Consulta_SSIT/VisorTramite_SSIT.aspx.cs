using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Consulta_SSIT
{
    public partial class VisorTramite_SSIT : System.Web.UI.Page
    {
        #region entity

        private DGHP_Entities db = null;
        private AGC_FilesEntities dbFiles = null;

        private void IniciarEntity()
        {
            if (this.db == null)
                this.db = new DGHP_Entities();
        }

        private void FinalizarEntity()
        {
            if (this.db != null)
            {
                this.db.Dispose();
                this.db = null;
            }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hid_return_url.Value = HttpContext.Current.Request.UrlReferrer.AbsolutePath.ToString().Split('/').Last(); ;
                int id_solicitud = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);
                int id_tipotramite = (Request.QueryString["idTipo"] != null ? Convert.ToInt32(Request.QueryString["idTipo"]) : 0);
                ComprobarSolicitud(id_solicitud);
                if (id_solicitud > 0)
                    CargarDatosTramite(id_solicitud, id_tipotramite);
            }

        }

        private void ComprobarSolicitud(int id_solicitud)
        {
            if (id_solicitud <= 0)
            {
                Server.Transfer("~/Errores/error3020.aspx");
            }
        }

        private void CargarDatosTramite(int id_solicitud, int id_tipotramite)
        {
            IniciarEntity();
            //int id_grupotramite;
            try
            {
                //Engine.getIdGrupoTrabajo(id_solicitud, out id_grupotramite);               


                if (id_tipotramite == (int)Constants.TipoDeTramite.Consulta_Padron)
                {
                    
                    upDatos.Visible = true;
                    tabTitulares.Visible = true;
                    tabTitularesSol.Visible = true;
                    tabTitularesTr.Visible = false;
                    upSolicitud.Visible = false;
                    CabeceraV2.LoadData((int)Constants.GruposDeTramite.CP, id_solicitud);
                    Historial.LoadData((int)Constants.GruposDeTramite.CP, id_solicitud);
                    visUbicaciones.CargarDatos(id_solicitud);
                    Tab_DatosLocal.CargarDatos(id_solicitud, 1, false);
                    Tab_Rubros.CargarDatos(id_solicitud,0, 1, false);
                    Tab_Titulares.CargarDatos(id_solicitud, 1, false);
                    Tab_TitularesSol.CargarDatos(id_solicitud, false);
                    DocumentoAdjunto.LoadData((int)Constants.GruposDeTramite.CP, id_solicitud);
                }
                else if (id_tipotramite == (int)Constants.TipoDeTramite.Transferencia)
                {
                   
                    upDatos.Visible = true;
                    tabTitulares.Visible = false;
                    tabTitularesSol.Visible = false;
                    tabTitularesTr.Visible = true;
                    upSolicitud.Visible = false;
                    int idCpadron = db.Transf_Solicitudes.Where(x => x.id_solicitud == id_solicitud).Select(y => y.id_cpadron).FirstOrDefault();
                    CabeceraV2.LoadData((int)Constants.TipoDeTramite.Transferencia, id_solicitud);
                    Historial.LoadData((int)Constants.TipoDeTramite.Transferencia, id_solicitud);
                    visUbicaciones.CargarDatos(idCpadron);
                    Tab_DatosLocal.CargarDatos(idCpadron, 1, false);
                    Tab_Rubros.CargarDatos(idCpadron,0, 1, false);
                    Tab_TitularesTr.LoadData(id_solicitud);
                    DocumentoAdjunto.LoadData((int)Constants.GruposDeTramite.TR, id_solicitud);
                }
                else
                {
                   
                    upDatos.Visible = false;
                    CabeceraV2.LoadData(id_solicitud);
                    Historial.LoadData(id_solicitud);
                    Ubicacion_hab.CargarDatos(id_solicitud);
                    DatosLocal_hab.CargarDatos(id_solicitud);
                    Titulares_hab.CargarDatos(id_solicitud);
                    Encomienda_sol.CargarDatos(id_solicitud);
                    Rubros_hab.CargarDatos(id_solicitud);
                    AnexoNotarial_sol.CargarDatos(id_solicitud);
                    TramiteCAA.CargarDatos(id_solicitud);
                    DocumentoAdjunto.LoadData(id_solicitud);
                    upSolicitud.Visible = true;
                    
                }
            }
            catch(Exception ex)
            {
                LogError.Write(ex, "Procedimiento CargarDatosTramite en VisorTramite_SSIT.aspx");
                LogError.Write(ex.InnerException, "INNER EXCEPTION en Procedimiento CargarDatosTramite en VisorTramite_SSIT.aspx");
                Server.Transfer(string.Format("~/Errores/error3020.aspx?m={0}", ex.Message + Environment.NewLine + ex.InnerException.Message));
            }
            FinalizarEntity();
            
        }
        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("~/GestionTramite/Consulta_SSIT/BusquedaTramiteSSIT" + "/" + "{0}", hid_return_url.Value), false);
        }
    }

    
}