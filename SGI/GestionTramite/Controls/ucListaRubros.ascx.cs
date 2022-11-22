using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using SGI.Model;

namespace SGI.GestionTramite.Controls
{
    public partial class ucListaRubros : System.Web.UI.UserControl
    {
        public void LoadData(int id_solicitud)
        {
            using (var db = new DGHP_Entities())
            {
                db.Database.CommandTimeout = 120;
                var estadosSolPres = db.TipoEstadoSolicitud.Where(e =>
                        e.Id == (int)Constants.Solicitud_Estados.Pendiente_de_Ingreso ||
                        e.Id == (int)Constants.Solicitud_Estados.En_trámite)
                        .Select(e => e.Nombre).ToList();

                var sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                var trf = db.Transf_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);

                var ultimaSolicitudPresentada = sol?.SSIT_Solicitudes_HistorialEstados.Where(h =>
                    estadosSolPres.Contains(h.cod_estado_nuevo)).Select(h => h.fecha_modificacion).OrderByDescending(h => h).FirstOrDefault();
                var ultimaTransmisionPresentada = trf?.Transf_Solicitudes_HistorialEstados.Where(h =>
                    estadosSolPres.Contains(h.cod_estado_nuevo)).Select(h => h.fecha_modificacion).OrderByDescending(h => h).FirstOrDefault();

                if (sol != null)
                {
                    LoadData(sol, ultimaSolicitudPresentada);
                }
                else if (trf != null)
                {
                    LoadData(trf, ultimaTransmisionPresentada);
                }
            }
        }

        public void LoadData(SSIT_Solicitudes solicitud, DateTime? ultimaPresentacion)
        {
            Encomienda encomienda = null;

            using (var db = new DGHP_Entities())
            {
                db.Database.CommandTimeout = 120;
                if (ultimaPresentacion != null)
                {

                    encomienda = (from rel in db.Encomienda_SSIT_Solicitudes
                                  join enc in db.Encomienda on rel.id_encomienda equals enc.id_encomienda
                                  join hist in db.Encomienda_HistorialEstados on enc.id_encomienda equals hist.id_encomienda
                                  where rel.id_solicitud == solicitud.id_solicitud
                                    && enc.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo
                                     && hist.fecha_modificacion <= ultimaPresentacion
                                  orderby enc.id_encomienda descending
                                  select enc).FirstOrDefault();
                             
                }
            }

            grdRubros.DataSource = GetRubros(encomienda, null);
            grdRubros.DataBind();

            grdRubrosANT.DataSource = GetRubrosAnterior(encomienda);
            grdRubrosANT.DataBind();

            if (grdRubrosANT.Rows.Count == 0)
            {
                pnlRubrosAnterior.Visible = false;
            }
        }

        public void LoadData(Transf_Solicitudes solicitud, DateTime? ultimaPresentacion)
        {
            Encomienda encomienda = null;

            using(var db = new DGHP_Entities())
            {
                db.Database.CommandTimeout = 120;
                if (ultimaPresentacion != null)
                {
                    IOrderedQueryable<int> idEncsApro = null;

                    idEncsApro = db.Encomienda.Where(x => x.Encomienda_Transf_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == solicitud.id_solicitud
                                                && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).Select(x => x.id_encomienda).OrderByDescending(x => x);

                    if (idEncsApro != null)
                    {
                        var idEncomiendaPresentada = db.Encomienda_HistorialEstados.Where(h => idEncsApro.Contains(h.id_encomienda) && h.fecha_modificacion <= ultimaPresentacion).Select(h => h.id_encomienda).OrderByDescending(h => h).FirstOrDefault();

                        encomienda = db.Encomienda.Where(e => e.id_encomienda == idEncomiendaPresentada).FirstOrDefault();
                    }
                }
            }

            grdRubros.DataSource = GetRubros(encomienda, null);
            grdRubros.DataBind();

            grdRubrosANT.DataSource = GetRubrosAnterior(encomienda);
            grdRubrosANT.DataBind();

            if (grdRubrosANT.Rows.Count == 0)
            {
                pnlRubrosAnterior.Visible = false;
            }
        }

        protected List<RubrosClass> GetRubros(Encomienda encomienda, SSIT_Solicitudes solicitud)
        {
            using (var db = new DGHP_Entities())
            {
                db.Database.CommandTimeout = 120;
                if (encomienda != null)
                {
                    var q = (from encrub in db.Encomienda_Rubros
                             join tdocreq in db.Tipo_Documentacion_Req on encrub.id_tipodocreq equals tdocreq.Id
                             join tact in db.TipoActividad on encrub.id_tipoactividad equals tact.Id
                             where encrub.id_encomienda == encomienda.id_encomienda
                             orderby encrub.SuperficieHabilitar descending, encrub.cod_rubro
                             select new RubrosClass
                             {
                                 id_Encomienda = encrub.id_encomienda,
                                 id_EncomiendaRubro = encrub.id_encomiendarubro,
                                 cod_rubro = encrub.cod_rubro,
                                 desc_rubro = encrub.desc_rubro,
                                 nom_tipoactividad = tact.Nombre,
                                 EsAnterior = encrub.EsAnterior,
                                 Nomenclatura = tdocreq.Nomenclatura,
                                 SuperficieHabilitar = encrub.SuperficieHabilitar

                             }).Union(from encrub in db.Encomienda_RubrosCN
                                      join r in db.RubrosCN on encrub.CodigoRubro equals r.Codigo
                                      join tact in db.TipoActividad on encrub.IdTipoActividad equals tact.Id
                                      join gc in db.ENG_Grupos_Circuitos on r.IdGrupoCircuito equals gc.id_grupo_circuito
                                      where encrub.id_encomienda == encomienda.id_encomienda
                                      orderby encrub.SuperficieHabilitar descending, encrub.CodigoRubro
                                      select new RubrosClass
                                      {
                                          id_Encomienda = encrub.id_encomienda,
                                          id_EncomiendaRubro = encrub.id_encomiendarubro,
                                          cod_rubro = encrub.CodigoRubro,
                                          desc_rubro = encrub.NombreRubro,
                                          nom_tipoactividad = tact.Nombre,
                                          EsAnterior = false,
                                          Nomenclatura = gc.cod_grupo_circuito,
                                          SuperficieHabilitar = encrub.SuperficieHabilitar

                                      }).Union(from ents in db.Encomienda_Transf_Solicitudes
                                               join ts in db.Transf_Solicitudes on ents.id_solicitud equals ts.id_solicitud
                                               join cprub in db.CPadron_RubrosCN on ts.id_cpadron equals cprub.id_cpadron
                                               join r in db.RubrosCN on cprub.id_rubro equals r.IdRubro
                                               join tact in db.TipoActividad on cprub.id_tipoactividad equals tact.Id
                                               join gc in db.ENG_Grupos_Circuitos on r.IdGrupoCircuito equals gc.id_grupo_circuito
                                               where ents.id_encomienda == encomienda.id_encomienda
                                               orderby cprub.SuperficieHabilitar descending, cprub.cod_rubro
                                               select new RubrosClass
                                               {
                                                   id_Encomienda = ents.id_encomienda,
                                                   id_EncomiendaRubro = cprub.id_cpadron,
                                                   cod_rubro = cprub.cod_rubro,
                                                   desc_rubro = cprub.desc_rubro,
                                                   nom_tipoactividad = tact.Nombre,
                                                   EsAnterior = false,
                                                   Nomenclatura = gc.cod_grupo_circuito,
                                                   SuperficieHabilitar = cprub.SuperficieHabilitar

                                               });

                    return q.ToList();
                }
                else if (solicitud != null && solicitud.id_tipotramite == (int)Constants.TipoDeTramite.Permiso)
                {
                    var q = (from solrub in db.SSIT_Solicitudes_RubrosCN
                             join r in db.RubrosCN on solrub.CodigoRubro equals r.Codigo
                             join tact in db.TipoActividad on solrub.IdTipoActividad equals tact.Id
                             join gc in db.ENG_Grupos_Circuitos on r.IdGrupoCircuito equals gc.id_grupo_circuito
                             where solrub.IdSolicitud == solicitud.id_solicitud
                             orderby solrub.SuperficieHabilitar descending
                             select new RubrosClass
                             {
                                 id_Encomienda = solrub.IdSolicitud,
                                 id_EncomiendaRubro = solrub.IdSolicitudRubro,
                                 cod_rubro = solrub.CodigoRubro,
                                 desc_rubro = solrub.NombreRubro,
                                 nom_tipoactividad = tact.Nombre,
                                 EsAnterior = false,
                                 Nomenclatura = gc.cod_grupo_circuito,
                                 SuperficieHabilitar = solrub.SuperficieHabilitar

                             });

                    return q.ToList();
                }

                return new List<RubrosClass>();
            }
        }

        protected List<RubrosClass> GetRubrosAnterior(Encomienda encomienda)
        {
            using (var db = new DGHP_Entities())
            {
                db.Database.CommandTimeout = 120;
                if (encomienda != null)
                {
                    var qANT = (from encrub in db.Encomienda_Rubros_AT_Anterior
                                join tdocreq in db.Tipo_Documentacion_Req on encrub.id_tipodocreq equals tdocreq.Id
                                join tact in db.TipoActividad on encrub.id_tipoactividad equals tact.Id
                                where encrub.id_encomienda == encomienda.id_encomienda
                                orderby encrub.SuperficieHabilitar descending
                                select new RubrosClass
                                {
                                    id_Encomienda = encrub.id_encomienda,
                                    id_EncomiendaRubro = encrub.id_encomiendarubro,
                                    cod_rubro = encrub.cod_rubro,
                                    desc_rubro = encrub.desc_rubro,
                                    nom_tipoactividad = tact.Nombre,
                                    EsAnterior = encrub.EsAnterior,
                                    Nomenclatura = tdocreq.Nomenclatura,
                                    SuperficieHabilitar = encrub.SuperficieHabilitar

                                }).Union(from encrub in db.Encomienda_RubrosCN_AT_Anterior
                                         join r in db.RubrosCN on encrub.CodigoRubro equals r.Codigo
                                         join tact in db.TipoActividad on encrub.IdTipoActividad equals tact.Id
                                         join gc in db.ENG_Grupos_Circuitos on r.IdGrupoCircuito equals gc.id_grupo_circuito
                                         where encrub.id_encomienda == encomienda.id_encomienda
                                         orderby encrub.SuperficieHabilitar descending
                                         select new RubrosClass
                                         {
                                             id_Encomienda = encrub.id_encomienda,
                                             id_EncomiendaRubro = encrub.id_encomiendarubro,
                                             cod_rubro = encrub.CodigoRubro,
                                             desc_rubro = encrub.NombreRubro,
                                             nom_tipoactividad = tact.Nombre,
                                             EsAnterior = false,
                                             Nomenclatura = gc.cod_grupo_circuito,
                                             SuperficieHabilitar = encrub.SuperficieHabilitar

                                         });

                    return qANT.ToList();
                }

                return new List<RubrosClass>();
            }
        }

        protected void grdRubros_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Cargo la lista de SubRubros si tiene
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RubrosClass row = (RubrosClass)e.Row.DataItem;
                //Busco los subrubros
                DGHP_Entities db = new DGHP_Entities();
                db.Database.CommandTimeout = 120;
                var q = (from erc in db.Encomienda_RubrosCN
                         join ers in db.Encomienda_RubrosCN_Subrubros on erc.id_encomiendarubro equals ers.Id_EncRubro
                         join rsr in db.RubrosCN_Subrubros on ers.Id_rubrosubrubro equals rsr.Id_rubroCNsubrubro
                         where
                            erc.id_encomienda == row.id_Encomienda &&
                            ers.Id_EncRubro == row.id_EncomiendaRubro
                         select new SubRubrosClass
                         {
                             id_Encomienda = erc.id_encomienda,
                             id_EncomiendaRubro = erc.id_encomiendarubro,
                             Nombre = rsr.Nombre
                         });

                GridView gvSubRubros = e.Row.FindControl("gvSubRubros") as GridView;
                if (q.ToList().Count > 0)
                {
                    gvSubRubros.DataSource = q.ToList();
                    gvSubRubros.DataBind();

                    // Mostramos el boton que los despliega
                    HtmlImage ImgBtn = (HtmlImage)e.Row.FindControl("ImgBtn");
                    ImgBtn.Visible = true;
                }
            }

            //Cargo la lista de Depósitos si tiene
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RubrosClass row = (RubrosClass)e.Row.DataItem;
                //Busco los Depósitos
                DGHP_Entities db = new DGHP_Entities();
                db.Database.CommandTimeout = 120;
                var q = (from ercd in db.Encomienda_RubrosCN_Deposito
                         join erc in db.Encomienda_RubrosCN on new { ercd.id_encomienda, ercd.IdRubro } equals new { erc.id_encomienda, erc.IdRubro }
                         join rsr in db.RubrosDepositosCN on ercd.IdDeposito equals rsr.IdDeposito
                         where ercd.id_encomienda == row.id_Encomienda && erc.CodigoRubro == row.cod_rubro
                         select new Depositos
                         {
                             idDeposito = ercd.IdDeposito,
                             Codigo = rsr.Codigo,
                             Descripcion = rsr.Descripcion
                         });

                GridView gvRubrosDepositos = e.Row.FindControl("gvRubrosDepositos") as GridView;
                var depos = q.ToList();
                if (depos.Count > 0)
                {
                    gvRubrosDepositos.DataSource = depos;
                    gvRubrosDepositos.DataBind();

                    // Mostramos el boton que los despliega
                    HtmlImage ImgBtn = (HtmlImage)e.Row.FindControl("ImgBtn");
                    ImgBtn.Visible = true;
                }
            }

        }

    }
}