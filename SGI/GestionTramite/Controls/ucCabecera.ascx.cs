using SGI.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;

namespace SGI.GestionTramite.Controls
{
    public partial class ucCabecera : System.Web.UI.UserControl
    {
        private int id_solicitud
        {
            get
            {
                int ret = 0;
                int.TryParse(hid_id_solicitud.Value, out ret);
                return ret;
            }
            set
            {
                hid_id_solicitud.Value = value.ToString();
            }

        }

        public void LoadData(int id_solicitud)
        {
            LoadData((int)Constants.GruposDeTramite.HAB, id_solicitud);
        }

        private string GetDireccionPermiso(int id_solicitud)
        {
            string direccion = string.Empty;

            using (var db = new DGHP_Entities())
            {
                db.Database.CommandTimeout = 300;
                var encomiendaSSIT = db.Encomienda
                                    .Where(x => x.Encomienda_SSIT_Solicitudes.Any(y => y.id_solicitud == id_solicitud) &&
                                    x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo)
                                    .OrderByDescending(x => x.id_encomienda)
                                    .FirstOrDefault();
                if (encomiendaSSIT == null)
                {
                    var encomiendaTransf = db.Encomienda
                        .Where(x => x.Encomienda_Transf_Solicitudes.Any(y => y.id_solicitud == id_solicitud) &&
                                    x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo)
                        .OrderByDescending(x => x.id_encomienda)
                        .FirstOrDefault();
                }
                else
                {
                    string objResult = db.SGI_GetDireccionEncomienda(enc.id_encomienda).FirstOrDefault();
                    direccion = objResult + ". - Plantas a Habilitar: " + CargarPlantasHabilitar(enc.id_encomienda);
                }
            }

            return direccion;
        }

        public void LoadData(int id_grupotramite, int id_solicitud)
        {
            int nroSolReferencia = 0;
            int.TryParse(ConfigurationManager.AppSettings["NroSolicitudReferencia"], out nroSolReferencia);

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
            {
                lnkNroExpEdit.Visible = false;
                pnlExpediente.Visible = true;
                pnlUbicaciones.Visible = true;
                pnlCPadron.Visible = false;
                pnlPresentacionAgreagr.Visible = false;
                pnlTransmision.Visible = false;
                var objsol = db.SSIT_Solicitudes
                   .Where(sol => sol.id_solicitud == id_solicitud)
                   .Select(sol => new
                   {
                       sol.id_solicitud,
                       DescripcionEstadoSolicitud = sol.TipoEstadoSolicitud.Descripcion,
                       sol.id_tipotramite,
                       sol.id_tipoexpediente,
                       sol.id_subtipoexpediente,
                       sol.NroExpediente,
                       sol.NroExpedienteSade,
                       sol.telefono,
                       sol.CodArea,
                       sol.Prefijo,
                       sol.Sufijo,
                       sol.documentacionPA,
                       sol.TipoTramite.descripcion_tipotramite,
                       sol.TipoExpediente.descripcion_tipoexpediente,
                       sol.NroExpedienteSadeRelacionado,
                       sol.FechaLibrado
                   })
                   .FirstOrDefault();

                var enc = db.Encomienda_SSIT_Solicitudes
                         .Where(rel => rel.id_solicitud == id_solicitud)
                         .Join(
                             db.Encomienda,
                             rel => rel.id_encomienda,
                             enco => enco.id_encomienda,
                             (rel, enco) => new { rel, enco }
                         )
                         .Where(joinResult => joinResult.enco.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo)
                         .OrderByDescending(joinResult => joinResult.enco.id_encomienda)
                         .Select(joinResult => joinResult.enco)
                         .FirstOrDefault();


                if (enc != null)
                {
                    string encomienda_desc = enc.id_encomienda.ToString() + " - " + Functions.GetTipoDeTramiteDesc(objsol.id_tipotramite) + " " + Functions.GetTipoExpedienteDesc(objsol.id_tipoexpediente, objsol.id_subtipoexpediente);
                    string objResult = db.SGI_GetDireccionEncomienda(enc.id_encomienda).FirstOrDefault();

                    var encUbic = enc.Encomienda_Ubicaciones.OrderBy(o => o.id_encomiendaubicacion).FirstOrDefault();

                    if (id_solicitud > nroSolReferencia)
                    {
                        pnlZonaDeclarada.Visible = false;
                        pnlSubZonaDeclarada.Visible = false;
                        pnlDistritosEspeciales.Visible = false;
                        pnlMixturaDistrito.Visible = true;
                        lblMixturaDistrito.Text = GetMixDistritoZonaySubZonaBySolicitud(id_solicitud);
                    }
                    else
                    {
                        lblZona.Text = enc.ZonaDeclarada.ToString();
                        lblZonaParcela.Text = encUbic.Zonas_Planeamiento.CodZonaPla;
                        var ubicZonaCom = encUbic.Ubicaciones.Ubicaciones_ZonasComplementarias.Where(x => x.tipo_ubicacion == "Z3").FirstOrDefault();

                        string DistritoEspecial;

                        if (ubicZonaCom != null)
                            DistritoEspecial = ubicZonaCom.Zonas_Planeamiento.CodZonaPla;
                        else
                            DistritoEspecial = "-";

                        lblDistritosEspeciales.Text = DistritoEspecial;
                    }

                    lblTextEncomienda.Text = "Nro. Encomienda";
                    lblSuperficieTotal.Text = (enc.Encomienda_DatosLocal.First().superficie_cubierta_dl.Value + enc.Encomienda_DatosLocal.First().superficie_descubierta_dl.Value).ToString();
                    lblEncomienda.Text = encomienda_desc;
                    lblUbicacion.Text = objResult + ". - Plantas a Habilitar: " + CargarPlantasHabilitar(enc.id_encomienda);
                    if (objsol.FechaLibrado != null)
                    {
                        lblLibradoUso.Text = "<b>" + objsol.FechaLibrado.ToString() + "</b>";
                    }
                    else if (objsol.FechaLibrado == null && enc.AcogeBeneficios == true)
                    {
                        lblLibradoUso.Text = "<font color='red'><b>EL PRESENTE TRAMITE NO SE ENCUENTRA LIBRADO AL USO, YA QUE SE ACOGE A LOS BENEFICIOS DE LA DI-2023-2-GCABA-UERESGP.</b></font>";
                    }
                    else
                    {
                        lblLibradoUso.Text = "<font color='red'><b>EL PRESENTE TRAMITE NO SE ENCUENTRA LIBRADO AL USO.</b></font>";
                    }
                }
                else if (objsol.id_tipotramite == (int)Constants.TipoDeTramite.Permiso)
                {
                    //string Direccion = "";
                    //var lstDirecciones = Shared.GetDireccionesENC(new string[] { objsol.id_solicitud.ToString() });
                    //if (lstDirecciones.Count > 0)
                    //    Direccion = lstDirecciones.FirstOrDefault().direccion;

                    var datosLocal = db.SSIT_Solicitudes_DatosLocal.FirstOrDefault(x => x.IdSolicitud == objsol.id_solicitud);

                    pnlZonaDeclarada.Visible = false;
                    pnlSubZonaDeclarada.Visible = false;
                    pnlDistritosEspeciales.Visible = false;
                    pnlMixturaDistrito.Visible = true;
                    lblMixturaDistrito.Text = GetMixDistritoZonaySubZonaBySolicitud(id_solicitud);
                    lblSuperficieTotal.Text = (datosLocal != null ? datosLocal.superficie_cubierta_dl + datosLocal.superficie_descubierta_dl : 0).ToString();

                    lblTextEncomienda.Text = "Tipo de Trámite:";
                    lblEncomienda.Text = objsol.descripcion_tipotramite + " " + objsol.descripcion_tipoexpediente;
                    //lblUbicacion.Text = Direccion;

                    var solOrigen = db.SSIT_Solicitudes_Origen.FirstOrDefault(x => x.id_solicitud == objsol.id_solicitud);
                    if (solOrigen != null)
                    {
                        pnlSolicitudOrigen.Visible = true;
                        lblSoliictitudOrigen.Text = solOrigen.id_solicitud_origen.ToString();

                        if (solOrigen.id_solicitud_origen != null)
                        {
                            lblUbicacion.Text = GetDireccionPermiso(solOrigen.id_solicitud_origen.Value);
                        }
                        else
                        {
                            lblUbicacion.Text = GetDireccionPermiso(solOrigen.id_transf_origen.Value);
                        }
                    }
                }

                lblSolicitud.Text = objsol.id_solicitud.ToString();
                lblEstado.Text = objsol.DescripcionEstadoSolicitud;
                if (objsol.NroExpedienteSade != "")
                {
                    lblExpediente.Visible = true;
                    lblExpediente.Text = objsol.NroExpedienteSade;
                }

                if (objsol.NroExpedienteSadeRelacionado != null && objsol.NroExpedienteSadeRelacionado.Trim().Length > 0)
                {
                    pnlExpeSadeRelacionado.Visible = true;
                    lblExpeSadeRelacionado.Text = objsol.NroExpedienteSadeRelacionado.Trim().ToUpper();
                }

                if (!string.IsNullOrWhiteSpace(objsol.telefono)
                        || (!string.IsNullOrWhiteSpace(objsol.CodArea) && !string.IsNullOrWhiteSpace(objsol.Prefijo) && !string.IsNullOrWhiteSpace(objsol.Sufijo)))
                {
                    pnlHAB.Visible = true;
                    string tlf;

                    if (!string.IsNullOrWhiteSpace(objsol.CodArea) && !string.IsNullOrWhiteSpace(objsol.Prefijo) && !string.IsNullOrWhiteSpace(objsol.Sufijo))
                        tlf = string.Format("({0}) {1} - {2}", objsol.CodArea, objsol.Prefijo, objsol.Sufijo);
                    else
                        tlf = objsol.telefono;

                    lblTelefono.Text = tlf;
                }

                pnlTitulares.Visible = true;
                var titulares = (  from pf in db.SSIT_Solicitudes_Titulares_PersonasFisicas
                                   where pf.id_solicitud == id_solicitud
                                   select new
                                   {
                                       label = pf.Apellido + ", " + pf.Nombres
                                   }
                               ).Concat(
                                   from pj in db.SSIT_Solicitudes_Titulares_PersonasJuridicas
                                   where pj.id_solicitud == id_solicitud
                                   select new
                                   {
                                       label = pj.Razon_Social
                                   }
                               ).ToList();

                lblTitulares.Text = "";
                foreach (var tit in titulares)
                    lblTitulares.Text = lblTitulares.Text + tit.label + "; ";
                pnlPresentacionAgreagr.Visible = objsol.documentacionPA != null ? objsol.documentacionPA.Value : false;
                lblPresentacionAgreagr.Text = "Si";

            }
            else if (id_grupotramite == (int)Constants.GruposDeTramite.CP)
            {
                lnkNroExpEdit.Visible = true;
                pnlExpediente.Visible = false;
                pnlUbicaciones.Visible = false;
                pnlCPadron.Visible = true;
                pnlTransmision.Visible = false;
                var objsol = db.Transf_Solicitudes
                              .Where(sol => sol.id_solicitud == id_solicitud)
                              .Select(sol => new
                              {
                                  sol.id_solicitud,
                                  sol.id_tipotramite,
                                  sol.id_tipoexpediente,
                                  sol.id_subtipoexpediente,
                                  sol.id_cpadron,
                                  DescripcionEstadoSolicitud = sol.TipoEstadoSolicitud.Descripcion,
                                  sol.CPadron_Solicitudes.nro_expediente_anterior,
                                  sol.NroExpedienteSade,
                                  sol.CreateDate,
                                  sol.TiposdeTransmision.nom_tipotransmision
                              })
                              .FirstOrDefault();

                lblSolicitud.Text = objsol.id_cpadron.ToString();
                lblEstado.Text = objsol.DescripcionEstadoSolicitud;
                lblNroExpediente.Text = objsol.nro_expediente_anterior;
                txtNroExpediente.Text = objsol.nro_expediente_anterior;
                lblFecha.Text = objsol.CreateDate.ToString("dd/MM/yyyy");

            }
            else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
            {

                var nroTr = (int)db.Parametros.Where(p => p.cod_param == "NroTransmisionReferencia").Select(x => x.valornum_param).First();
                if (id_solicitud > nroTr)
                {
                    lnkNroExpEdit.Visible = false;
                    pnlExpediente.Visible = true;
                    pnlUbicaciones.Visible = true;
                    pnlCPadron.Visible = true;
                    lifecha.Visible = false;
                    var objsol = (from sol in db.Transf_Solicitudes
                                  where sol.id_solicitud.Equals(id_solicitud)
                                  select new
                                  {
                                      sol.id_solicitud,
                                      sol.id_tipotramite,
                                      sol.id_tipoexpediente,
                                      sol.id_subtipoexpediente,
                                      sol.id_cpadron,
                                      DescripcionEstadoSolicitud = sol.TipoEstadoSolicitud.Descripcion,
                                      sol.CPadron_Solicitudes.nro_expediente_anterior,
                                      sol.NroExpedienteSade,
                                      sol.CreateDate,
                                      sol.TiposdeTransmision.nom_tipotransmision
                                  }).FirstOrDefault();

                    var enc = (from en in db.Encomienda
                               join et in db.Encomienda_Transf_Solicitudes on en.id_encomienda equals et.id_encomienda
                               where en.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo
                               && et.id_solicitud == id_solicitud
                               orderby en.id_encomienda descending
                               select en).FirstOrDefault();

                    if (enc != null)
                    {
                        lblEncomienda.Text = enc.id_encomienda.ToString() + " - " + Functions.GetTipoDeTramiteDesc(objsol.id_tipotramite) + " " + Functions.GetTipoExpedienteDesc(objsol.id_tipoexpediente, objsol.id_subtipoexpediente);
                        lblUbicacion.Text = db.SGI_GetDireccionEncomienda(enc.id_encomienda).FirstOrDefault();
                        var encUbic = enc.Encomienda_Ubicaciones.OrderBy(o => o.id_encomiendaubicacion).FirstOrDefault();

                        if (id_solicitud > nroTr)
                        {
                            pnlZonaDeclarada.Visible = false;
                            pnlSubZonaDeclarada.Visible = false;
                            pnlDistritosEspeciales.Visible = false;
                            pnlMixturaDistrito.Visible = true;

                            lblMixturaDistrito.Text = GetMixDistritoZonaySubZonaByTR(id_solicitud);
                        }
                        else
                        {
                            lblZona.Text = enc.ZonaDeclarada.ToString();
                            lblZonaParcela.Text = encUbic.Zonas_Planeamiento.CodZonaPla;

                            var ubicZonaCom = encUbic.Ubicaciones.Ubicaciones_ZonasComplementarias.Where(x => x.tipo_ubicacion == "Z3").FirstOrDefault();

                            string DistritoEspecial;

                            if (ubicZonaCom != null)
                                DistritoEspecial = ubicZonaCom.Zonas_Planeamiento.CodZonaPla;
                            else
                                DistritoEspecial = "-";

                            lblDistritosEspeciales.Text = DistritoEspecial;
                        }

                        lblSuperficieTotal.Text = (enc.Encomienda_DatosLocal.First().superficie_cubierta_dl.Value + enc.Encomienda_DatosLocal.First().superficie_descubierta_dl.Value).ToString();
                    }
                    lblSolicitud.Text = objsol.id_solicitud.ToString();
                    lblEstado.Text = objsol.DescripcionEstadoSolicitud;
                    lblTextEncomienda.Text = "Nro. Encomienda";
                    lblNroExpediente.Text = objsol.nro_expediente_anterior;
                    txtNroExpediente.Text = objsol.nro_expediente_anterior;
                    lblTipoTransm.Text = objsol.nom_tipotransmision;

                    pnlTitulares.Visible = true;
                    var titulares = (from pf in db.Transf_Titulares_Solicitud_PersonasFisicas
                                     where pf.id_solicitud == id_solicitud
                                     select new
                                     {
                                         label = pf.Apellido + ", " + pf.Nombres
                                     }).Union(
                                     from pj in db.Transf_Titulares_Solicitud_PersonasJuridicas
                                     where pj.id_solicitud == id_solicitud
                                     select new
                                     {
                                         label = pj.Razon_Social
                                     }).ToList();
                    lblTitulares.Text = "";
                    foreach (var tit in titulares)
                        lblTitulares.Text = lblTitulares.Text + tit.label + "; ";
                    if (objsol.NroExpedienteSade != "")
                    {
                        lblExpediente.Visible = true;
                        lblExpediente.Text = objsol.NroExpedienteSade;
                    }
                }
                else
                {
                    lnkNroExpEdit.Visible = false;
                    pnlTransmision.Visible = false;
                    pnlExpediente.Visible = true;
                    pnlUbicaciones.Visible = false;
                    pnlCPadron.Visible = true;
                    var objsol = db.Transf_Solicitudes
                                  .Where(sol => sol.id_solicitud == id_solicitud)
                                  .Select(sol => new
                                  {
                                      sol.id_solicitud,
                                      sol.id_cpadron,
                                      DescripcionEstadoSolicitud = sol.TipoEstadoSolicitud.Descripcion,
                                      nro_expediente_anterior = sol.CPadron_Solicitudes.nro_expediente_anterior,
                                      sol.NroExpedienteSade,
                                      sol.CreateDate
                                  })
                                  .FirstOrDefault();

                    lblSolicitud.Text = objsol.id_solicitud.ToString();
                    lblEstado.Text = objsol.DescripcionEstadoSolicitud;
                    lblTextEncomienda.Text = "Nro. Consulta al Padrón";
                    lblEncomienda.Text = objsol.id_cpadron.ToString();
                    lblNroExpediente.Text = objsol.nro_expediente_anterior;
                    txtNroExpediente.Text = objsol.nro_expediente_anterior;
                    lblFecha.Text = objsol.CreateDate.ToString("dd/MM/yyyy");
                    if (objsol.NroExpedienteSade != "")
                    {
                        lblExpediente.Visible = true;
                        lblExpediente.Text = objsol.NroExpedienteSade;
                    }
                }
                db.Dispose();
            }
        }

        private string GetMixDistritoZonaySubZonaBySolicitud(int id_solicitud)
        {
            using (var db = new DGHP_Entities())
            {
                db.Database.CommandTimeout = 300;
                var parameter = new System.Data.Entity.Core.Objects.ObjectParameter("result", "varchar(1000)");
                db.GetMixDistritoZonaySubZonaBySolicitud(id_solicitud, parameter);
                return parameter.Value.ToString();
            }
        }

        private string GetMixDistritoZonaySubZonaByTR(int id_solicitud)
        {
            using (var db = new DGHP_Entities())
            {
                db.Database.CommandTimeout = 300;
                var parameter = new System.Data.Entity.Core.Objects.ObjectParameter("result", "varchar(1000)");
                db.GetMixDistritoZonaySubZonaByTR(id_solicitud, parameter);
                return parameter.Value.ToString();
            }
        }

        //private string GetMixDistritoZonaySubZonaBySolicitud(int id_solicitud)
        //{
        //    using (var db = new DGHP_Entities())
        //    {
        //        return db.GetMixDistritoZonaySubZonaBySolicitud(id_solicitud).FirstOrDefault();
        //    }
        //}

        protected void lnkNroExpSave_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                db.Database.CommandTimeout = 300;
                int id_cpadron = 0;
                string edit_nroexpediente = txtNroExpediente.Text.Trim();
                int.TryParse(lblSolicitud.Text.Trim(), out id_cpadron);

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {

                        CPadron_Solicitudes original = db.CPadron_Solicitudes
                            .AsNoTracking()
                            .SingleOrDefault(x => x.id_cpadron == id_cpadron);

                        if (original != null)
                        {
                            original.nro_expediente_anterior = edit_nroexpediente;
                            db.SaveChanges();
                        }

                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }
                LoadData((int)Constants.GruposDeTramite.CP, id_cpadron);
            }
            catch (Exception ex)
            {
                //

            }
            finally
            {
                db.Dispose();
            }

        }

        private List<string> ZonasDistritos(int id_encomiendaUbic)
        {
            DGHP_Entities db = new DGHP_Entities();
            List<String> lsta = new List<string>();
            db.Database.CommandTimeout = 300;
            try
            {
                lsta = (from encZona in db.Encomienda_Ubicaciones_Mixturas
                        join zona in db.Ubicaciones_ZonasMixtura on encZona.IdZonaMixtura equals zona.IdZonaMixtura
                        where encZona.id_encomiendaubicacion == id_encomiendaUbic
                        select zona.Codigo
                    )
                    .Concat(
                        from encDis in db.Encomienda_Ubicaciones_Distritos
                        join distri in db.Ubicaciones_CatalogoDistritos on encDis.IdDistrito equals distri.IdDistrito
                        where encDis.id_encomiendaubicacion == id_encomiendaUbic
                        select distri.Codigo
                    ).ToList();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                db.Dispose();
            }

            return lsta;
        }

        private List<string> SubZonas(int id_encomiendaUbic)
        {
            DGHP_Entities db = new DGHP_Entities();
            List<String> lsta = new List<string>();
            db.Database.CommandTimeout = 300;
            try
            {
                lsta = db.Encomienda_Ubicaciones_Distritos
                        .Where(encDis => encDis.id_encomiendaubicacion == id_encomiendaUbic)
                        .Join(db.Ubicaciones_CatalogoDistritos,
                              encDis => encDis.IdDistrito,
                              distri => distri.IdDistrito,
                              (encDis, distri) => distri)
                        .Join(db.Ubicaciones_CatalogoDistritos_Zonas,
                              distri => distri.IdDistrito,
                              zona => zona.IdDistrito,
                              (distri, zona) => zona)
                        .Join(db.Ubicaciones_CatalogoDistritos_Subzonas,
                              zona => zona.IdZona,
                              subzona => subzona.IdZona,
                              (zona, subzona) => subzona.CodigoSubZona)
                        .ToList();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                db.Dispose();
            }

            return lsta;
        }

        private string CargarPlantasHabilitar(int id_encomienda)
        {
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            string plantasHab = "";
            try
            {
                var plantasFiltradas = db.Encomienda_Plantas
                                      .Where(encPlan => encPlan.id_encomienda == id_encomienda)
                                      .Select(encPlan => new
                                      {
                                          encPlan.id_tiposector,
                                          encPlan.TipoSector,
                                          encPlan.detalle_encomiendatiposector
                                      }).ToList();

                var lstaPlantas = (from encPlan in plantasFiltradas
                                   join tipo in db.TipoSector on encPlan.id_tiposector equals tipo.Id
                                   select new
                                   {
                                       tipo.TamanoCampoAdicional,
                                       tipo.MuestraCampoAdicional,
                                       encPlan.TipoSector,
                                       encPlan.detalle_encomiendatiposector,
                                       tipo.Descripcion
                                   }).ToList();


                foreach (var item in lstaPlantas)
                {

                    int TamanoCampoAdicional = item.TamanoCampoAdicional != null ? item.TamanoCampoAdicional.Value : 0;
                    bool MuestraCampoAdicional = false;
                    string separador = "";

                    if (item.TipoSector != null && item.MuestraCampoAdicional != null)
                        MuestraCampoAdicional = item.MuestraCampoAdicional.Value;


                    if (plantasHab.Length == 0)
                        separador = "";
                    else
                        separador = ", ";

                    if (MuestraCampoAdicional)
                    {
                        if (TamanoCampoAdicional >= 10)
                            plantasHab += separador + item.Descripcion.Trim();
                        else
                            plantasHab += separador + item.Descripcion.Trim() + " " + item.detalle_encomiendatiposector.Trim();
                    }
                    else
                        plantasHab += separador + item.Descripcion.Trim();
                }
            }
            catch (Exception ex)
            {
                //

            }
            finally
            {
                db.Dispose();
            }
            return plantasHab;
        }
    }



}