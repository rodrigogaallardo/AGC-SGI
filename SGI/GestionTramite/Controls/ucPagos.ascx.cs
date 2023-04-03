using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Web;
using iTextSharp.text.pdf;
using SGI.Model;
using SGI.Webservices.ws_interface_AGC;


namespace SGI.GestionTramite.Controls
{
    public partial class ucPagos : System.Web.UI.UserControl
    {
        private int id_solicitud;

        public int tipo_tramite
        {
            get
            {
                int ret = 0;
                int.TryParse(hid_tipo_tramite.Value, out ret);
                return ret;
            }
            set
            {
                hid_tipo_tramite.Value = value.ToString();
            }

        }

        public void LoadData(int id_solicitud)
        {
            this.id_solicitud = id_solicitud;

            if (id_solicitud > 0)
            {
                CargarPagosAGC(id_solicitud);
                CargarPagosAPRA(id_solicitud);
            }
        }

        public string ConsultarEstadoPago(Constants.PagosTipoTramite tipo_tramite, int id_pago)
        {
            string strEstadoPago = "";

            string WSPagos_Usuario = "";
            string WSPagos_Password = "";

            if (id_pago <= 0)
                return strEstadoPago;


            try
            {

                if (tipo_tramite == Constants.PagosTipoTramite.CAA)
                {

                    SGI.Webservices.ws_interface_AGC.ws_Interface_AGC servicio = new SGI.Webservices.ws_interface_AGC.ws_Interface_AGC();
                    SGI.Webservices.ws_interface_AGC.wsResultado ws_resultado_BUI = new SGI.Webservices.ws_interface_AGC.wsResultado();
                    servicio.Url = this.Url_Interface_AGC;
                    var lstBUIsCAA = servicio.Get_BUIs_Pagos(this.User_Interface_AGC, this.Password_Interface_AGC, new int[] { id_pago }, ref ws_resultado_BUI).ToList();
                    servicio.Dispose();

                    if (ws_resultado_BUI.ErrorCode != 0)
                    {
                        throw new Exception("CAA - " + ws_resultado_BUI.ErrorDescription);
                    }

                    strEstadoPago = (from bui in lstBUIsCAA
                                     where bui.IdPago == id_pago
                                     select bui.EstadoNombre).FirstOrDefault();


                }
                else if (tipo_tramite == Constants.PagosTipoTramite.HAB)
                {
                    SGI.Webservices.Pagos.ws_pagos servicePagos = new SGI.Webservices.Pagos.ws_pagos();
                    SGI.Webservices.Pagos.wsResultado wsPagos_resultado = new SGI.Webservices.Pagos.wsResultado();
                    servicePagos.Url = Functions.GetParametroChar("Pagos.Url");

                    WSPagos_Usuario = Functions.GetParametroChar("SSIT.WebService.Pagos.User");
                    WSPagos_Password = Functions.GetParametroChar("SSIT.WebService.Pagos.Password");
                    strEstadoPago = servicePagos.GetEstadoPago(WSPagos_Usuario, WSPagos_Password, id_pago, ref wsPagos_resultado);
                    servicePagos.Dispose();

                    if (wsPagos_resultado.ErrorCode != 0)
                    {
                        throw new Exception("HAB - " + wsPagos_resultado.ErrorDescription);
                    }

                }

            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Error en ws GetEstadoPago");
                throw new Exception("El servicio de generación de boletas no esta disponible. Intente en otro momento.");
            }


            return strEstadoPago;

        }

        public void SetEstadoPago(string valor, int id_pago)
        {
            string estado_actual = hid_estado_pago.Value;
            if (string.IsNullOrEmpty(estado_actual))
            {
                hid_estado_pago.Value = valor;
                hid_id_pago.Value = id_pago.ToString();
            }
            else
            {
                if (estado_actual != valor && estado_actual != Constants.BUI_EstadoPago.Pagado.ToString())
                {
                    //me pasan un valor diferente al º y el actual no es pagado.
                    //porque el estado pago no se cambia porque tiene mas prioridad que los otros

                    if (valor != Constants.BUI_EstadoPago.Vencido.ToString())
                    {
                        hid_estado_pago.Value = valor;
                        hid_id_pago.Value = id_pago.ToString();
                    }
                }
            }
        }

        private void setImprimirBoleta(int isApra, GridViewRowEventArgs e)
        {
            clsItemGrillaPagos rowItem = (clsItemGrillaPagos)e.Row.DataItem;

            HyperLink lnkImprimirBoletaUnica = (HyperLink)e.Row.FindControl("lnkImprimirBoletaUnica");
            lnkImprimirBoletaUnica.NavigateUrl = string.Format("~/Reportes/ImprimirBoletaUnica.aspx?id={0}&is_apra={1}", rowItem.id_pago, isApra);

            if (rowItem.estado == Constants.BUI_EstadoPago.Vencido.ToString())
                lnkImprimirBoletaUnica.Visible = false;
        }

        protected void grdPagosGeneradosBUI_AGC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                setImprimirBoleta(0, e);
            }
        }

        protected void grdPagosGeneradosBUI_APRA_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                setImprimirBoleta(1, e);
            }
        }

        private string Url_Interface_AGC
        {
            get
            {
                return Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC");
            }
        }
        private string User_Interface_AGC
        {
            get
            {
                return Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC.User");
            }
        }
        private string Password_Interface_AGC
        {
            get
            {
                return Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC.Password");
            }
        }
        public string GetEstadoPagoAGC(int id_solicitud)
        {
            Constants.BUI_EstadoPago ret = Constants.BUI_EstadoPago.SinPagar;

            //SGI.Webservices.Pagos.ws_pagos servicePagos = new SGI.Webservices.Pagos.ws_pagos();
            //SGI.Webservices.Pagos.wsResultado wsPagos_resultado = new SGI.Webservices.Pagos.wsResultado();
            //servicePagos.Url = Functions.GetParametroChar("Pagos.Url");

            //string WSPagos_Usuario = Functions.GetParametroChar("SSIT.WebService.Pagos.User");
            //string WSPagos_Password = Functions.GetParametroChar("SSIT.WebService.Pagos.Password");

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            int id_grupotramite;
            Engine.getIdGrupoTrabajo(id_solicitud, out id_grupotramite);

            List<int> list_id_pagos = new List<int>(); 

            if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
            {
                list_id_pagos = db.SSIT_Solicitudes_Pagos.Where(x => x.id_solicitud == id_solicitud).Select(s => s.id_pago).ToList();
            }
            else if (id_grupotramite == (int)Constants.GruposDeTramite.TR) //ampliacion
            {
                list_id_pagos = db.Transf_Solicitudes_Pagos.Where(x => x.id_solicitud == id_solicitud).Select(s => s.id_pago).ToList();
            }
            int[] arr_id_pagos = list_id_pagos.ToArray();

            //var lstBUIsHAB = servicePagos.GetBoletas(WSPagos_Usuario, WSPagos_Password, arr_id_pagos, ref wsPagos_resultado).ToList();
            //servicePagos.Dispose();

            //if (wsPagos_resultado.ErrorCode != 0)
            //{
            //    throw new Exception("HAB - " + wsPagos_resultado.ErrorDescription);
            //}
            //else
            //{
            //    if (lstBUIsHAB.Count() > 0)
            //    {
            //        if (lstBUIsHAB.Count(x => x.EstadoId == (int)Constants.BUI_EstadoPago.Pagado) > 0)
            //            ret = Constants.BUI_EstadoPago.Pagado;
            //        else
            //            ret = (Constants.BUI_EstadoPago)lstBUIsHAB.LastOrDefault().EstadoId;
            //    }
            //}

            List<wsPagos_BoletaUnica> lstBUIsHAB = new List<wsPagos_BoletaUnica>();
            foreach (var item in arr_id_pagos)
            {
                var BUIsHAB = db.wsPagos_BoletaUnica.Where(x => x.id_pago == item).FirstOrDefault();
                lstBUIsHAB.Add(BUIsHAB);
            }

            if (lstBUIsHAB.Count() > 0)
            {
                if (lstBUIsHAB.Count(x => x.EstadoPago_BU == (int)Constants.BUI_EstadoPago.Pagado) > 0)
                    ret = Constants.BUI_EstadoPago.Pagado;
                else
                    ret = (Constants.BUI_EstadoPago)lstBUIsHAB.LastOrDefault().EstadoPago_BU;
            }
            return db.wsPagos_BoletaUnica_Estados.Where(x => x.id_estadopago == (int)ret).FirstOrDefault().nom_estadopago;
        }
        public class clsItemGrillaPagos
        {
            public int id_sol_pago { get; set; }
            public int id_solicitud { get; set; }
            public int id_pago { get; set; }
            public int id_medio_pago { get; set; }
            public decimal monto_pago { get; set; }
            public DateTime CreateDate { get; set; }
            public string desc_medio_pago { get; set; }
            public string estado { get; set; }
        }

        public void CargarPagosAGC(int id_solicitud)
        {
            List<clsItemGrillaPagos> lstPagos = new List<clsItemGrillaPagos>();

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            int id_grupotramite;
            Engine.getIdGrupoTrabajo(id_solicitud, out id_grupotramite);

            if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
            {
                lstPagos = (from solpag in db.SSIT_Solicitudes_Pagos
                            where solpag.id_solicitud == id_solicitud
                            join ws_Pagos in db.wsPagos_BoletaUnica on solpag.id_pago equals ws_Pagos.id_pago
                            join bue in db.wsPagos_BoletaUnica_Estados on ws_Pagos.EstadoPago_BU equals bue.id_estadopago
                            select new clsItemGrillaPagos
                            {
                                id_sol_pago = solpag.id_sol_pago,
                                id_solicitud = solpag.id_solicitud,
                                id_pago = solpag.id_pago,
                                id_medio_pago = 0,
                                monto_pago = solpag.monto_pago,
                                CreateDate = solpag.CreateDate,
                                estado = bue.nom_estadopago,
                                desc_medio_pago = "Boleta única"
                            }).ToList();
            } else if (id_grupotramite == (int)Constants.GruposDeTramite.TR) //ampliacion
            {
                lstPagos = (from solpag in db.Transf_Solicitudes_Pagos
                            where solpag.id_solicitud == id_solicitud
                            select new clsItemGrillaPagos
                            {
                                id_sol_pago = solpag.id_sol_pago,
                                id_solicitud = solpag.id_solicitud,
                                id_pago = solpag.id_pago,
                                id_medio_pago = 0,
                                monto_pago = solpag.monto_pago,
                                CreateDate = solpag.CreateDate,
                                desc_medio_pago = "Boleta única"
                            }).ToList();

                foreach (clsItemGrillaPagos pago in lstPagos)
                {
                    pago.estado = GetEstadoPagoAGC(id_solicitud);
                }

                //cargar Pagos Transf_Solicitudes_Pagos
                var PagosTransf = (from tt in db.SGI_Tramites_Tareas
                                   join tt_tr in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_tr.id_tramitetarea
                                   join sp in db.SGI_Solicitudes_Pagos on tt.id_tramitetarea equals sp.id_tramitetarea
                                   join ws_Pagos in db.wsPagos_BoletaUnica on sp.id_pago equals ws_Pagos.id_pago
                                   join bue in db.wsPagos_BoletaUnica_Estados on ws_Pagos.EstadoPago_BU equals bue.id_estadopago
                                   where tt_tr.id_solicitud == id_solicitud
                                   select new clsItemGrillaPagos
                                   {
                                       id_sol_pago = sp.id_sol_pago,
                                       id_solicitud = id_solicitud,
                                       id_pago = sp.id_pago,
                                       id_medio_pago = 0,
                                       monto_pago = sp.monto_pago,
                                       CreateDate = sp.CreateDate,
                                       estado = bue.nom_estadopago,
                                       desc_medio_pago = "Boleta única",
                                   });
                lstPagos.AddRange(PagosTransf);
            }          


            //pnlPagosGeneradosBUI.Visible = (lstPagos.Count > 0);
            grdPagosGeneradosBUI_AGC.DataSource = lstPagos;
            grdPagosGeneradosBUI_AGC.DataBind();

        }

        public void CargarPagosAPRA(int id_solicitud)
        {
            List<clsItemGrillaPagos> lstPagos = new List<clsItemGrillaPagos>();
            List<int> lstEncomiendasRelacionadas = new List<int>();
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            int id_grupotramite;
            Engine.getIdGrupoTrabajo(id_solicitud, out id_grupotramite);

            if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
            {
                var sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                if (sol != null)
                {
                    lstEncomiendasRelacionadas = (from rel in db.Encomienda_SSIT_Solicitudes
                                                  join enc in db.Encomienda on rel.id_encomienda equals enc.id_encomienda
                                                  where rel.id_solicitud == id_solicitud
                                                    && enc.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo
                                                  select enc.id_encomienda).ToList();
                   
                }
            }
            else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
            {
                var sol = db.Transf_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                if (sol != null)
                {
                    lstEncomiendasRelacionadas = (from rel in db.Encomienda_Transf_Solicitudes
                                                  join enc in db.Encomienda on rel.id_encomienda equals enc.id_encomienda
                                                  where rel.id_solicitud == id_solicitud
                                                    && enc.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo
                                                  select enc.id_encomienda).ToList();

                }

            }

            ws_Interface_AGC servicio = new ws_Interface_AGC();
            SGI.Webservices.ws_interface_AGC.wsResultado ws_resultado_CAA = new SGI.Webservices.ws_interface_AGC.wsResultado();

            servicio.Url = Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC");
            string username_servicio = Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC.User");
            string password_servicio = Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC.Password");
            int[] l = servicio.Get_IdPagosCAA_by_Encomiendas(username_servicio, password_servicio, lstEncomiendasRelacionadas.ToArray(), ref ws_resultado_CAA);

            lstPagos = (from bu in db.wsPagos_BoletaUnica
                        join pag in db.wsPagos on bu.id_pago equals pag.id_pago
                        join est in db.wsPagos_BoletaUnica_Estados on bu.EstadoPago_BU equals est.id_estadopago
                        where l.Contains(bu.id_pago)
                        select new clsItemGrillaPagos
                        {
                            //.id_solicitud = caa.id_caa,
                            id_pago = bu.id_pago,
                            id_medio_pago = 0,
                            monto_pago = bu.Monto_BU,
                            CreateDate = pag.CreateDate,
                            estado = est.nom_estadopago,
                            desc_medio_pago = "Boleta única" // (p.id_medio_pago == 0 ? "Boleta única" : "Pago electrónico")
                        }).ToList();

            //pnlPagosGeneradosBUI.Visible = (lstPagos.Count > 0);
            grdPagosGeneradosBUI_APRA.DataSource = lstPagos;
            grdPagosGeneradosBUI_APRA.DataBind();
        }

        public void CargarPagosAGCVisibility( bool Visibility)
        {
          // grdPagosGeneradosBUI_AGC.Visible = Visibility;  
            pnlAGC.Visible = Visibility;
        }
        public void CargarPagosAPRAVisibility(bool Visibility)
        {
            //grdPagosGeneradosBUI_APRA.Visible = Visibility;
            pnlAPRA.Visible = Visibility;
        }

        public List<clsItemGrillaPagos> PagosAGCList(int id_solicitud)
        {
            List<clsItemGrillaPagos> lstPagos = new List<clsItemGrillaPagos>();

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            int id_grupotramite;
            Engine.getIdGrupoTrabajo(id_solicitud, out id_grupotramite);

            if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
            {
                lstPagos = (from solpag in db.SSIT_Solicitudes_Pagos
                            where solpag.id_solicitud == id_solicitud
                            join ws_Pagos in db.wsPagos_BoletaUnica on solpag.id_pago equals ws_Pagos.id_pago
                            join bue in db.wsPagos_BoletaUnica_Estados on ws_Pagos.EstadoPago_BU equals bue.id_estadopago
                            select new clsItemGrillaPagos
                            {
                                id_sol_pago = solpag.id_sol_pago,
                                id_solicitud = solpag.id_solicitud,
                                id_pago = solpag.id_pago,
                                id_medio_pago = 0,
                                monto_pago = solpag.monto_pago,
                                CreateDate = solpag.CreateDate,
                                estado = bue.nom_estadopago,
                                desc_medio_pago = "Boleta única"
                            }).ToList();
            }
            else if (id_grupotramite == (int)Constants.GruposDeTramite.TR) //ampliacion
            {
                lstPagos = (from solpag in db.Transf_Solicitudes_Pagos
                            where solpag.id_solicitud == id_solicitud
                            select new clsItemGrillaPagos
                            {
                                id_sol_pago = solpag.id_sol_pago,
                                id_solicitud = solpag.id_solicitud,
                                id_pago = solpag.id_pago,
                                id_medio_pago = 0,
                                monto_pago = solpag.monto_pago,
                                CreateDate = solpag.CreateDate,
                                desc_medio_pago = "Boleta única"
                            }).ToList();

                foreach (clsItemGrillaPagos pago in lstPagos)
                {
                    pago.estado = GetEstadoPagoAGC(id_solicitud);
                }

                //cargar Pagos Transf_Solicitudes_Pagos
                var PagosTransf = (from tt in db.SGI_Tramites_Tareas
                                   join tt_tr in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_tr.id_tramitetarea
                                   join sp in db.SGI_Solicitudes_Pagos on tt.id_tramitetarea equals sp.id_tramitetarea
                                   join ws_Pagos in db.wsPagos_BoletaUnica on sp.id_pago equals ws_Pagos.id_pago
                                   join bue in db.wsPagos_BoletaUnica_Estados on ws_Pagos.EstadoPago_BU equals bue.id_estadopago
                                   where tt_tr.id_solicitud == id_solicitud
                                   select new clsItemGrillaPagos
                                   {
                                       id_sol_pago = sp.id_sol_pago,
                                       id_solicitud = id_solicitud,
                                       id_pago = sp.id_pago,
                                       id_medio_pago = 0,
                                       monto_pago = sp.monto_pago,
                                       CreateDate = sp.CreateDate,
                                       estado = bue.nom_estadopago,
                                       desc_medio_pago = "Boleta única",
                                   });
                lstPagos.AddRange(PagosTransf);
            }

            return lstPagos;

        }

        public List<clsItemGrillaPagos> PagosAPRAList(int id_solicitud)
        {
            List<clsItemGrillaPagos> lstPagos = new List<clsItemGrillaPagos>();
            List<int> lstEncomiendasRelacionadas = new List<int>();
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            int id_grupotramite;
            Engine.getIdGrupoTrabajo(id_solicitud, out id_grupotramite);

            if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
            {
                var sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                if (sol != null)
                {
                    lstEncomiendasRelacionadas = (from rel in db.Encomienda_SSIT_Solicitudes
                                                  join enc in db.Encomienda on rel.id_encomienda equals enc.id_encomienda
                                                  where rel.id_solicitud == id_solicitud
                                                    && enc.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo
                                                  select enc.id_encomienda).ToList();

                }
            }
            else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
            {
                var sol = db.Transf_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                if (sol != null)
                {
                    lstEncomiendasRelacionadas = (from rel in db.Encomienda_Transf_Solicitudes
                                                  join enc in db.Encomienda on rel.id_encomienda equals enc.id_encomienda
                                                  where rel.id_solicitud == id_solicitud
                                                    && enc.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo
                                                  select enc.id_encomienda).ToList();

                }

            }

            ws_Interface_AGC servicio = new ws_Interface_AGC();
            SGI.Webservices.ws_interface_AGC.wsResultado ws_resultado_CAA = new SGI.Webservices.ws_interface_AGC.wsResultado();

            servicio.Url = Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC");
            string username_servicio = Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC.User");
            string password_servicio = Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC.Password");
            int[] l = servicio.Get_IdPagosCAA_by_Encomiendas(username_servicio, password_servicio, lstEncomiendasRelacionadas.ToArray(), ref ws_resultado_CAA);

            lstPagos = (from bu in db.wsPagos_BoletaUnica
                        join pag in db.wsPagos on bu.id_pago equals pag.id_pago
                        join est in db.wsPagos_BoletaUnica_Estados on bu.EstadoPago_BU equals est.id_estadopago
                        where l.Contains(bu.id_pago)
                        select new clsItemGrillaPagos
                        {
                            //.id_solicitud = caa.id_caa,
                            id_pago = bu.id_pago,
                            id_medio_pago = 0,
                            monto_pago = bu.Monto_BU,
                            CreateDate = pag.CreateDate,
                            estado = est.nom_estadopago,
                            desc_medio_pago = "Boleta única" // (p.id_medio_pago == 0 ? "Boleta única" : "Pago electrónico")
                        }).ToList();
            return lstPagos;
        }
    }

}