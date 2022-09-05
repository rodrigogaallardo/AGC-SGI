using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Security;
using System.Web.UI.WebControls;
using SGI.Webservices.Pagos;
using System.Data.Entity.Core.Objects;

namespace SGI.GestionTramite.Controls
{
    // http://localhost:15625/GestionTramite/Tareas/GenerarBoleta.aspx?id=2172

    public class ucMediosPagos_EventArgs : EventArgs
    {
        public int id_medio_pago { get; set; }
        public int id_tramite_tarea { get; set; }
        public bool cancel { get; set; }

    }

    public class ucMediosPagos_Encomienda_Concepto_a_cobrar
    {
        public string codigo_concepto { get; set; }
        public int cantidad { get; set; }
    }
    
    public class ucMediosPagos_Encomienda_Titular
    {
        public string TipoPersona { get; set; }
        public string TipoPersonaDesc { get; set; }
        public int id_persona { get; set; }
        public string ApellidoNomRazon{ get; set; }
        public string cuit{ get; set; }
        public string Calle{ get; set; }
        public string Nro_Puerta { get; set; }
        public string Domicilio{ get; set; }
        public string piso{ get; set; }
        public string Depto{ get; set; }
        public string email{ get; set; }
        public string Codigo_Postal{ get; set; }
        public string localidad{ get; set; }
        public int tipo_doc{ get; set; }
        public string desc_tipo_doc{ get; set; }
        public string nro_doc { get; set; }

        public string getDomicilio()
        {
            string calle =string.IsNullOrEmpty(this.Calle) ? "":this.Calle;
            string puerta = string.IsNullOrEmpty( this.Nro_Puerta ) ? "": " " + this.Nro_Puerta.ToString() ;

            return calle + puerta;
        }

    }


    public class ucMediosPagos_conceptos
    {
        public int cantidad { get; set; }
        public string concepto { get; set; }
        public string descripcion { get; set; }
        public int concepto1 { get; set; }
        public int concepto2 { get; set; }
        public int concepto3 { get; set; }
        public bool admite_reglas { get; set; }

        public ucMediosPagos_conceptos(string concepto_completo)
        {
            this.concepto = concepto_completo;
            string[] separar = concepto_completo.Split('.');

            int index = 1;
            int concepto = 0;
            foreach (string item in separar)
	        {

                if (!int.TryParse(item, out concepto))
                {
                    throw new Exception("Concepto inválido: " + item + " / " + concepto_completo);
                }

                switch (index)
                {
                    case 1:
                        this.concepto1 = concepto;
                        break;

                    case 2:
                        this.concepto2 = concepto;
                        break;

                    case 3:
                        this.concepto3 = concepto;
                        break;
                }

                index++;
	        }

        }

        public ucMediosPagos_conceptos(int concepto1, int concepto2, int concepto3)
        {
            this.concepto1 = concepto1;
            this.concepto2 = concepto2;
            this.concepto3 = concepto2;
            this.concepto = string.Format("{0:00}.{1:00}.{2:00}", concepto1, concepto2, concepto3);
        }

        public ucMediosPagos_conceptos(string concepto1, string concepto2, string concepto3)
        {
            int concepto = 0;

            if (!int.TryParse(concepto1, out concepto))
                throw new Exception("Concepto inválido: " + concepto1);

            this.concepto1 = concepto;
                 
            if (!int.TryParse(concepto2, out concepto))
                throw new Exception("Concepto inválido: " + concepto1);

            this.concepto2 = concepto;

            if (!int.TryParse(concepto3, out concepto))
                throw new Exception("Concepto inválido: " + concepto1);

            this.concepto3 = concepto;

            this.concepto = string.Format("{0:00}.{1:00}.{2:00}", this.concepto1, this.concepto2, this.concepto3);
        }


        public override bool Equals(object obj)
        {
            if (obj != null && obj is ucMediosPagos_conceptos)
            {
                ucMediosPagos_conceptos obj2 = (ucMediosPagos_conceptos)obj;
                if (this.concepto1 == obj2.concepto1 && this.concepto2 == obj2.concepto2 && this.concepto3 == obj2.concepto3)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }



    }



    public partial class ucMediosPagos : System.Web.UI.UserControl
    {

        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region "Propiedades"

        private string _ValidationGroup = "";
        public string ValidationGroup
        {
            get
            {
                return _ValidationGroup;
            }
            set
            {
                _ValidationGroup = value;
                lnkGenerarBoletaUnica.ValidationGroup = _ValidationGroup;
            }
        }

        private bool _generarBoleta_Visible ;
        public bool GenerarBoleta_Visible
        {
            get
            {
                return _generarBoleta_Visible;
            }
            set
            {
                _generarBoleta_Visible = value;
                pnlGenerarBoletaUnica.Visible = _generarBoleta_Visible;
            }
        }

        int _hid_id_tramite_tarea = 0;
        public int id_tramite_tarea
        {
            get
            {
                if (!int.TryParse(hid_id_tramite_tarea.Value, out _hid_id_tramite_tarea))
                {
                    _hid_id_tramite_tarea = 0;
                }
                return _hid_id_tramite_tarea;
            }
            set
            {
                _hid_id_tramite_tarea = value;
                hid_id_tramite_tarea.Value = _hid_id_tramite_tarea.ToString();
            }
        }

        int _hid_id_grupotramite = 0;
        public int id_grupotramite
        {
            get
            {
                if (!int.TryParse(hid_id_grupotramite.Value, out _hid_id_grupotramite))
                {
                    _hid_id_grupotramite = 0;
                }
                return _hid_id_grupotramite;
            }
            set
            {
                _hid_id_grupotramite = value;
                hid_id_grupotramite.Value = _hid_id_grupotramite.ToString();
            }
        }
        private bool _existenPagosPendientes = true;
        public bool ExistenPagosPendientes
        {
            get { return _existenPagosPendientes; }
        }


        public bool HabilitarGeneracionPago()
        {
            string estado_pago = GetEstadoPago();
            if (string.IsNullOrEmpty(estado_pago) ||
                estado_pago == Constants.Pago_EstadoPago.Vencido.ToString())
                return true;
            else
                return false;
        }

        int _hid_id_pago = 0;
        string _hid_estado_pago = "";
        public string GetEstadoPago()
        {
            _hid_estado_pago = hid_estado_pago.Value;
            return _hid_estado_pago;
        }

        public int GetIdPago()
        {
            _hid_id_pago = Convert.ToInt32(hid_id_pago.Value);
            return _hid_id_pago;
        }

        public void SetEstadoPago(string valor, int id_pago)
        {
            string estado_actual = hid_estado_pago.Value;
            if (string.IsNullOrEmpty(estado_actual))
            {
                _hid_estado_pago = valor; // no har estado cargado le asigno lo primero que venga
                _hid_id_pago = id_pago;
                hid_estado_pago.Value = _hid_estado_pago;
                hid_id_pago.Value = _hid_id_pago.ToString();
            }
            else
            {
                if (estado_actual != valor && estado_actual != Constants.Pago_EstadoPago.Pagado.ToString())
                {
                    //me pasan un valor diferente al actual y el actual no es pagado.
                    //porque el estado pago no se cambia porque tiene mas prioridad que los otros

                    if (valor != Constants.Pago_EstadoPago.Vencido.ToString())
                    {
                        _hid_estado_pago = valor;
                        hid_estado_pago.Value = _hid_estado_pago;
                        _hid_id_pago = id_pago;
                        hid_id_pago.Value = _hid_id_pago.ToString();
                    }
                }
            }
        }

        #endregion


        #region Eventos

        public delegate void EventHandlerGenerarBoletaUnica(object sender, ucMediosPagos_EventArgs e);
        public event EventHandlerGenerarBoletaUnica Click_GenerarBoletaUnica;
        public delegate void EventHandlerGenerarPagoElectronico(object sender, ucMediosPagos_EventArgs e);
        public event EventHandlerGenerarPagoElectronico Click_GenerarPagoElectronico;

        protected string OnClientClick_GenerarBoletaUnica
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    lnkGenerarBoletaUnica.Attributes.Add("onClick", value);
                }
            }
        }

        //protected string OnClientClick_GenerarPagoOnLine
        //{
        //    set
        //    {
        //        if (Constants.HabilitarPagoElectronico && !string.IsNullOrEmpty(value))
        //        {
        //            lnkGenerarPagoElectronico.Attributes.Add("onClick", value);
        //        }


        //    }
        //}

        #endregion


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
                this.db.Dispose();
        }

        private void IniciarEntityFiles()
        {
            if (this.dbFiles == null)
                this.dbFiles = new AGC_FilesEntities();
        }

        private void FinalizarEntityFiles()
        {
            if (this.dbFiles != null)
                this.dbFiles.Dispose();
        }

        #endregion

        private List<SGI_Solicitudes_Pagos> lstPagos = null;
        private SGI_Solicitudes_Pagos pagoVigente = null;

        #region Acceso a datos

        private List<SGI_Solicitudes_Pagos> Buscar_SGI_Solicitudes_Pagos(int id_tramite_tarea)
        {
            List<SGI_Solicitudes_Pagos> lstPagos =
                (
                    from p in db.SGI_Solicitudes_Pagos
                    where p.id_tramitetarea == id_tramite_tarea
                    // orderby p.id_sol_pago descending
                    select p
                ).ToList();
            var list=(from p in db.SSIT_Solicitudes_Pagos
                        join t in db.SGI_Tramites_Tareas_HAB on p.id_solicitud equals t.id_solicitud
                        where t.id_tramitetarea == id_tramite_tarea
                        select new 
                        {
                            nro_boleta_unica = p.id_pago,
                            CreateDate = p.CreateDate,
                            monto_pago = p.monto_pago,
                            id_pago = p.id_pago,
                            CreateUser = p.CreateUser,
                            id_sol_pago = p.id_sol_pago,
                            id_tramitetarea = id_tramite_tarea,
                        }
                ).ToList();
            foreach(var p in list)
            {
                lstPagos.Add(new SGI_Solicitudes_Pagos
                {
                    nro_boleta_unica = p.id_pago,
                    CreateDate = p.CreateDate,
                    monto_pago = p.monto_pago,
                    id_pago = p.id_pago,
                    codigo_barras = "",
                    codigo_verificador = "",
                    CreateUser = p.CreateUser,
                    id_medio_pago = 0,
                    id_sol_pago = p.id_sol_pago,
                    id_tramitetarea = id_tramite_tarea,
                    nro_dependencia = 0,
                    UpdateDate = null,
                    UpdateUser = null,
                    url_pago = "",
                    SGI_Tramites_Tareas = null,
                    wsPagos = null
                });
            }

            return lstPagos;
        }

        private List<ucMediosPagos_Encomienda_Titular> TraerTitularesSolicitud_paraWsPagos(int id_solicitud, int id_grupotramite)
        {
            List<ucMediosPagos_Encomienda_Titular> titular = new List<ucMediosPagos_Encomienda_Titular>();
            if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
            {

                var titular_juridico =
                    (
                        from sol in db.SSIT_Solicitudes
                        join pj in db.SSIT_Solicitudes_Titulares_PersonasJuridicas on sol.id_solicitud equals pj.id_solicitud
                        join loc in db.Localidad on pj.id_localidad equals loc.Id
                        where sol.id_solicitud == id_solicitud
                        orderby pj.id_personajuridica
                        select new
                        {
                            TipoPersona = "PJ",
                            TipoPersonaDesc = "Persona Jurídica",
                            id_persona = pj.id_personajuridica,
                            ApellidoNomRazon = pj.Razon_Social,
                            cuit = pj.CUIT,
                            Calle = pj.Calle,
                            Nro_Puerta = pj.NroPuerta,
                            piso = pj.Piso,
                            Depto = pj.Depto,
                            email = pj.Email,
                            Codigo_Postal = pj.Codigo_Postal,
                            localidad = loc.Depto,
                            tipo_doc = 0,
                            desc_tipo_doc = "CUIT",
                            nro_doc = pj.CUIT
                        }

                    ).ToList();

                var titular_fisico =
                    (
                        from sol in db.SSIT_Solicitudes
                        join pf in db.SSIT_Solicitudes_Titulares_PersonasFisicas on sol.id_solicitud equals pf.id_solicitud
                        join loc in db.Localidad on pf.Id_Localidad equals loc.Id
                        join tipo_doc in db.TipoDocumentoPersonal on pf.id_tipodoc_personal equals tipo_doc.TipoDocumentoPersonalId
                        where sol.id_solicitud == id_solicitud
                        orderby pf.id_personafisica
                        select new
                        {
                            TipoPersona = "PF",
                            TipoPersonaDesc = "Persona Física",
                            id_persona = pf.id_personafisica,
                            ApellidoNomRazon = pf.Apellido + ", " + pf.Nombres,
                            cuit = pf.Cuit,
                            Calle = pf.Calle,
                            Nro_Puerta = pf.Nro_Puerta,
                            piso = pf.Piso,
                            Depto = pf.Depto,
                            email = pf.Email,
                            Codigo_Postal = pf.Codigo_Postal,
                            localidad = loc.Depto,
                            tipo_doc = pf.id_tipodoc_personal,
                            desc_tipo_doc = (pf.id_tipodoc_personal == 0) ? "DNI" : tipo_doc.Nombre,
                            nro_doc = pf.Nro_Documento
                        }

                    ).ToList();

                foreach (var item in titular_juridico)
                {
                    ucMediosPagos_Encomienda_Titular tit = new ucMediosPagos_Encomienda_Titular();

                    tit.TipoPersona = item.TipoPersona;
                    tit.TipoPersonaDesc = item.TipoPersonaDesc;
                    tit.id_persona = item.id_persona;
                    tit.ApellidoNomRazon = item.ApellidoNomRazon;
                    tit.cuit = item.cuit;
                    tit.Calle = item.Calle;
                    tit.Nro_Puerta = item.Nro_Puerta.HasValue ? item.Nro_Puerta.ToString() : "";
                    tit.piso = item.piso;
                    tit.Depto = item.Depto;
                    tit.email = item.email;
                    tit.Codigo_Postal = item.Codigo_Postal;
                    tit.localidad = item.localidad;
                    tit.tipo_doc = item.tipo_doc;
                    tit.desc_tipo_doc = item.desc_tipo_doc;
                    tit.nro_doc = item.nro_doc;

                    tit.Domicilio = tit.getDomicilio();

                    titular.Add(tit);
                }

                foreach (var item in titular_fisico)
                {
                    ucMediosPagos_Encomienda_Titular tit = new ucMediosPagos_Encomienda_Titular();

                    tit.TipoPersona = item.TipoPersona;
                    tit.TipoPersonaDesc = item.TipoPersonaDesc;
                    tit.id_persona = item.id_persona;
                    tit.ApellidoNomRazon = item.ApellidoNomRazon;
                    tit.cuit = item.cuit;
                    tit.Calle = item.Calle;
                    tit.Nro_Puerta = item.Nro_Puerta.ToString();
                    tit.piso = item.piso;
                    tit.Depto = item.Depto;
                    tit.email = item.email;
                    tit.Codigo_Postal = item.Codigo_Postal;
                    tit.localidad = item.localidad;
                    tit.tipo_doc = item.tipo_doc;
                    tit.desc_tipo_doc = item.desc_tipo_doc;
                    tit.nro_doc = item.nro_doc.ToString();

                    tit.Domicilio = tit.getDomicilio();

                    titular.Add(tit);
                }
            }
            else if (id_grupotramite == (int)Constants.GruposDeTramite.CP)
            {

                var titular_juridico =
                    (
                        from sol in db.CPadron_Solicitudes
                        join pj in db.CPadron_Titulares_PersonasJuridicas on sol.id_cpadron equals pj.id_cpadron
                        join loc in db.Localidad on pj.id_localidad equals loc.Id
                        where sol.id_cpadron == id_solicitud
                        orderby pj.id_personajuridica
                        select new
                        {
                            TipoPersona = "PJ",
                            TipoPersonaDesc = "Persona Jurídica",
                            id_persona = pj.id_personajuridica,
                            ApellidoNomRazon = pj.Razon_Social,
                            cuit = pj.CUIT,
                            Calle = pj.Calle,
                            Nro_Puerta = pj.NroPuerta,
                            piso = pj.Piso,
                            Depto = pj.Depto,
                            email = pj.Email,
                            Codigo_Postal = pj.Codigo_Postal,
                            localidad = loc.Depto,
                            tipo_doc = 0,
                            desc_tipo_doc = "CUIT",
                            nro_doc = pj.CUIT
                        }

                    ).ToList();

                var titular_fisico =
                    (
                        from sol in db.CPadron_Solicitudes
                        join pf in db.CPadron_Titulares_PersonasFisicas on sol.id_cpadron equals pf.id_cpadron
                        join loc in db.Localidad on pf.Id_Localidad equals loc.Id
                        join tipo_doc in db.TipoDocumentoPersonal on pf.id_tipodoc_personal equals tipo_doc.TipoDocumentoPersonalId
                        where sol.id_cpadron == id_solicitud
                        orderby pf.id_personafisica
                        select new
                        {
                            TipoPersona = "PF",
                            TipoPersonaDesc = "Persona Física",
                            id_persona = pf.id_personafisica,
                            ApellidoNomRazon = pf.Apellido + ", " + pf.Nombres,
                            cuit = pf.Cuit,
                            Calle = pf.Calle,
                            Nro_Puerta = pf.Nro_Puerta,
                            piso = pf.Piso,
                            Depto = pf.Depto,
                            email = pf.Email,
                            Codigo_Postal = pf.Codigo_Postal,
                            localidad = loc.Depto,
                            tipo_doc = pf.id_tipodoc_personal,
                            desc_tipo_doc = (pf.id_tipodoc_personal == 0) ? "DNI" : tipo_doc.Nombre,
                            nro_doc = pf.Nro_Documento
                        }

                    ).ToList();

                foreach (var item in titular_juridico)
                {
                    ucMediosPagos_Encomienda_Titular tit = new ucMediosPagos_Encomienda_Titular();

                    tit.TipoPersona = item.TipoPersona;
                    tit.TipoPersonaDesc = item.TipoPersonaDesc;
                    tit.id_persona = item.id_persona;
                    tit.ApellidoNomRazon = item.ApellidoNomRazon;
                    tit.cuit = item.cuit;
                    tit.Calle = item.Calle;
                    tit.Nro_Puerta = item.Nro_Puerta.HasValue ? item.Nro_Puerta.ToString() : "";
                    tit.piso = item.piso;
                    tit.Depto = item.Depto;
                    tit.email = item.email;
                    tit.Codigo_Postal = item.Codigo_Postal;
                    tit.localidad = item.localidad;
                    tit.tipo_doc = item.tipo_doc;
                    tit.desc_tipo_doc = item.desc_tipo_doc;
                    tit.nro_doc = item.nro_doc;

                    tit.Domicilio = tit.getDomicilio();

                    titular.Add(tit);
                }

                foreach (var item in titular_fisico)
                {
                    ucMediosPagos_Encomienda_Titular tit = new ucMediosPagos_Encomienda_Titular();

                    tit.TipoPersona = item.TipoPersona;
                    tit.TipoPersonaDesc = item.TipoPersonaDesc;
                    tit.id_persona = item.id_persona;
                    tit.ApellidoNomRazon = item.ApellidoNomRazon;
                    tit.cuit = item.cuit;
                    tit.Calle = item.Calle;
                    tit.Nro_Puerta = item.Nro_Puerta.ToString();
                    tit.piso = item.piso;
                    tit.Depto = item.Depto;
                    tit.email = item.email;
                    tit.Codigo_Postal = item.Codigo_Postal;
                    tit.localidad = item.localidad;
                    tit.tipo_doc = item.tipo_doc;
                    tit.desc_tipo_doc = item.desc_tipo_doc;
                    tit.nro_doc = item.nro_doc.ToString();

                    tit.Domicilio = tit.getDomicilio();

                    titular.Add(tit);
                }
            }
            else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
            {

                var titular_juridico =
                    (
                        from sol in db.Transf_Solicitudes
                        join pj in db.Transf_Titulares_PersonasJuridicas on sol.id_solicitud equals pj.id_solicitud
                        join loc in db.Localidad on pj.id_localidad equals loc.Id
                        where sol.id_solicitud == id_solicitud
                        orderby pj.id_personajuridica
                        select new
                        {
                            TipoPersona = "PJ",
                            TipoPersonaDesc = "Persona Jurídica",
                            id_persona = pj.id_personajuridica,
                            ApellidoNomRazon = pj.Razon_Social,
                            cuit = pj.CUIT,
                            Calle = pj.Calle,
                            Nro_Puerta = pj.NroPuerta,
                            piso = pj.Piso,
                            Depto = pj.Depto,
                            email = pj.Email,
                            Codigo_Postal = pj.Codigo_Postal,
                            localidad = loc.Depto,
                            tipo_doc = 0,
                            desc_tipo_doc = "CUIT",
                            nro_doc = pj.CUIT
                        }

                    ).ToList();

                var titular_fisico =
                    (
                        from sol in db.Transf_Solicitudes
                        join pf in db.Transf_Titulares_PersonasFisicas on sol.id_solicitud equals pf.id_solicitud
                        join loc in db.Localidad on pf.id_Localidad equals loc.Id
                        join tipo_doc in db.TipoDocumentoPersonal on pf.id_tipodoc_personal equals tipo_doc.TipoDocumentoPersonalId
                        where sol.id_solicitud == id_solicitud
                        orderby pf.id_personafisica
                        select new
                        {
                            TipoPersona = "PF",
                            TipoPersonaDesc = "Persona Física",
                            id_persona = pf.id_personafisica,
                            ApellidoNomRazon = pf.Apellido + ", " + pf.Nombres,
                            cuit = pf.Cuit,
                            Calle = pf.Calle,
                            Nro_Puerta = pf.Nro_Puerta,
                            piso = pf.Piso,
                            Depto = pf.Depto,
                            email = pf.Email,
                            Codigo_Postal = pf.Codigo_Postal,
                            localidad = loc.Depto,
                            tipo_doc = pf.id_tipodoc_personal,
                            desc_tipo_doc = (pf.id_tipodoc_personal == 0) ? "DNI" : tipo_doc.Nombre,
                            nro_doc = pf.Nro_Documento
                        }

                    ).ToList();

                foreach (var item in titular_juridico)
                {
                    ucMediosPagos_Encomienda_Titular tit = new ucMediosPagos_Encomienda_Titular();

                    tit.TipoPersona = item.TipoPersona;
                    tit.TipoPersonaDesc = item.TipoPersonaDesc;
                    tit.id_persona = item.id_persona;
                    tit.ApellidoNomRazon = item.ApellidoNomRazon;
                    tit.cuit = item.cuit;
                    tit.Calle = item.Calle;
                    tit.Nro_Puerta = item.Nro_Puerta.HasValue ? item.Nro_Puerta.ToString() : "";
                    tit.piso = item.piso;
                    tit.Depto = item.Depto;
                    tit.email = item.email;
                    tit.Codigo_Postal = item.Codigo_Postal;
                    tit.localidad = item.localidad;
                    tit.tipo_doc = item.tipo_doc;
                    tit.desc_tipo_doc = item.desc_tipo_doc;
                    tit.nro_doc = item.nro_doc;

                    tit.Domicilio = tit.getDomicilio();

                    titular.Add(tit);
                }

                foreach (var item in titular_fisico)
                {
                    ucMediosPagos_Encomienda_Titular tit = new ucMediosPagos_Encomienda_Titular();

                    tit.TipoPersona = item.TipoPersona;
                    tit.TipoPersonaDesc = item.TipoPersonaDesc;
                    tit.id_persona = item.id_persona;
                    tit.ApellidoNomRazon = item.ApellidoNomRazon;
                    tit.cuit = item.cuit;
                    tit.Calle = item.Calle;
                    tit.Nro_Puerta = item.Nro_Puerta.ToString();
                    tit.piso = item.piso;
                    tit.Depto = item.Depto;
                    tit.email = item.email;
                    tit.Codigo_Postal = item.Codigo_Postal;
                    tit.localidad = item.localidad;
                    tit.tipo_doc = item.tipo_doc;
                    tit.desc_tipo_doc = item.desc_tipo_doc;
                    tit.nro_doc = item.nro_doc.ToString();

                    tit.Domicilio = tit.getDomicilio();

                    titular.Add(tit);
                }
            }
            return titular;
        }

        #endregion

        public void LoadData(int id_tramite_tarea, bool verificarPagos)
        {
            LoadData(id_tramite_tarea, (int)Constants.GruposDeTramite.HAB, verificarPagos);
        }
        public void LoadData(int id_tramite_tarea, int id_grupotramite, bool verificarPagos)
        {
           
            try
            {
                IniciarEntity();

                this.id_tramite_tarea = id_tramite_tarea;
                this.id_grupotramite = id_grupotramite;
                SetEstadoPago(Constants.Pago_EstadoPago.SinPagar.ToString(), 0);

                cargarGrillaPagos_Boleta();

                FinalizarEntity();
            }
            catch (Exception ex)
            {
                FinalizarEntity();    
                throw ex;
            }
            
        }

       

        private void cargarGrillaPagos_Boleta()
        {
            pnlGenerarBoletaUnica.Visible = false;
            pnlPagosGeneradosBU.Visible = false;

            this.lstPagos = Buscar_SGI_Solicitudes_Pagos(this.id_tramite_tarea);

            if (lstPagos.Count > 0)
                pagoVigente = this.lstPagos[lstPagos.Count - 1];

            grdPagosGeneradosBU.DataSource = null;
            this._existenPagosPendientes = false;

            if (this.lstPagos.Count > 0)
            {
                pnlPagosGeneradosBU.Visible = true;
                grdPagosGeneradosBU.DataSource = this.lstPagos;
            }

            grdPagosGeneradosBU.DataBind();

            pnlGenerarBoletaUnica.Visible = !this._existenPagosPendientes;
       
        }

        protected void grdPagosGeneradosBU_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int id_pago = int.Parse(grdPagosGeneradosBU.DataKeys[e.Row.RowIndex].Values["id_pago"].ToString());

                Label lblDescripicionEstadoPago = (Label)e.Row.FindControl("lblDescripicionEstadoPago");
                HyperLink lnkImprimirboleta = (HyperLink)e.Row.FindControl("lnkImprimirboleta");
                string estado = ConsultarEstadoPago(id_pago);

                lblDescripicionEstadoPago.Text = estado;
                SetEstadoPago(estado, id_pago);

                lnkImprimirboleta.Visible = false;
                if (estado != Constants.Pago_EstadoPago.Vencido.ToString())
                {
                    lnkImprimirboleta.Visible = true;
                    //analizar si el pago se vencio para activar los botones de generacion de pago con boleta o on line
                    this._existenPagosPendientes = true;
                }
            }
        }


        #region Generar pago

        protected void lnkGenerarBoletaUnica_Click(object sender, EventArgs e)
        {
            if (Click_GenerarBoletaUnica != null)
            {
                ucMediosPagos_EventArgs args = new ucMediosPagos_EventArgs();

                args.cancel = false;
                args.id_tramite_tarea = this.id_tramite_tarea;
                args.id_medio_pago = 0;

                Click_GenerarBoletaUnica(sender, args);
            }
        }

        protected void lnkGenerarPagoElectronico_Click(object sender, EventArgs e)
        {

            if (Click_GenerarPagoElectronico != null)
            {

                ucMediosPagos_EventArgs args = new ucMediosPagos_EventArgs();

                args.cancel = false;
                args.id_tramite_tarea = this.id_tramite_tarea;
                args.id_medio_pago = 1;

                Click_GenerarPagoElectronico(sender, args);
            }
        }

        public void GenerarBoletaUnica(int id_tramite_tarea, int id_grupotramite)
        {

            IniciarEntityFiles();
            IniciarEntity();
            ws_pagos servicePagos = null;
            byte[] PdfBoletaUnica = null;

            try
            {
                this.id_tramite_tarea = id_tramite_tarea;
                int id_solicitud = 0;
                if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
                {
                    SGI_Tramites_Tareas_HAB tt = db.SGI_Tramites_Tareas_HAB.Where(x => x.id_tramitetarea == id_tramite_tarea).FirstOrDefault();
                    id_solicitud = tt.id_solicitud;
                }
                else if (id_grupotramite == (int)Constants.GruposDeTramite.CP)
                {
                    SGI_Tramites_Tareas_CPADRON tt = db.SGI_Tramites_Tareas_CPADRON.Where(x => x.id_tramitetarea == id_tramite_tarea).FirstOrDefault();
                    id_solicitud = tt.id_cpadron;
                }
                else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
                {
                    SGI_Tramites_Tareas_TRANSF tt = db.SGI_Tramites_Tareas_TRANSF.Where(x => x.id_tramitetarea == id_tramite_tarea).FirstOrDefault();
                    id_solicitud = tt.id_solicitud;
                }
                string WSPagos_url = Parametros.GetParam_ValorChar("Pagos.Url");
                string WSPagos_Usuario = Parametros.GetParam_ValorChar("SGI.Pagos.User");
                string WSPagos_Password = Parametros.GetParam_ValorChar("SGI.Pagos.Password");

                servicePagos = new ws_pagos();
                servicePagos.Url = WSPagos_url;

                BUDatosContribuyente datosContribuyente = CargarDatosContribuyente(id_solicitud, id_grupotramite);
                List<BUConcepto> lstConceptos = CargarConceptos(id_solicitud, id_grupotramite, ref servicePagos, WSPagos_Usuario, WSPagos_Password);

                SGI.Webservices.Pagos.wsResultado wsPagos_resultado = null; 

                BUBoletaUnica boletaGenerada = null;

                try
                {
                    wsPagos_resultado = new SGI.Webservices.Pagos.wsResultado();
                    boletaGenerada = servicePagos.GenerarBoletaUnica(WSPagos_Usuario, WSPagos_Password, datosContribuyente, lstConceptos.ToArray(), ref wsPagos_resultado);
                }
                catch (Exception ex)
                {
                    LogError.Write(ex, "Error en ws GenerarBoletaUnica de ws_pagos");
                    throw new Exception("El servicio de generación de boletas no esta disponible. Intente en otro momento.");
                }


                if (!wsPagos_resultado.ErrorCode.Equals(0))
                {
                    LogError.Write(new Exception("Error en ws GenerarBoletaUnica de ws_pagos"), wsPagos_resultado.ErrorDescription + " - Código Error: " + wsPagos_resultado.ErrorCode);
                    throw new Exception("El servicio de generación de boletas no esta disponible. Intente en otro momento.");
                }

                try
                {
                    wsPagos_resultado = new SGI.Webservices.Pagos.wsResultado();
                    PdfBoletaUnica = servicePagos.GetPDFBoletaUnica(WSPagos_Usuario, WSPagos_Password, boletaGenerada.IdPago, ref wsPagos_resultado);

                }
                catch (Exception ex)
                {
                    LogError.Write(ex, "Error en ws GetPDFBoletaUnica de ws_pagos");
                    throw new Exception("El servicio de generación de boletas GetPDFBoletaUnica no esta disponible. Intente en otro momento.");
                }

                servicePagos.Dispose();

                if (!wsPagos_resultado.ErrorCode.Equals(0))
                {
                    LogError.Write(new Exception("Error en ws GenerarBoletaUnica de ws_pagos"), wsPagos_resultado.ErrorDescription + " - Código Error: " + wsPagos_resultado.ErrorCode);
                    throw new Exception("El servicio de generación de boletas GetPDFBoletaUnica no esta disponible. Intente en otro momento.");
                }

                Guid userId = (Guid)Membership.GetUser().ProviderUserKey;

                try
                {
                    int id_sol_pago = 0;

                    using (TransactionScope Tran = new TransactionScope())
                    {

                        try
                        {
                            id_sol_pago = (int)db.SGI_Solicitudes_Pagos_insert(id_tramite_tarea, boletaGenerada.IdPago,
                                    (int)Constants.Pago_MedioPago.BoletaUnica, boletaGenerada.MontoTotal,
                                    boletaGenerada.CodBarras, boletaGenerada.NroBoletaUnica, boletaGenerada.Dependencia,
                                    boletaGenerada.CodigoVerificador, "", userId).FirstOrDefault();

                            Tran.Complete();
                            Tran.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Tran.Dispose();
                            LogError.Write(ex, "error en transaccion. ucMediospagos-GenerarBoletaUnica 1");
                            throw ex;
                        }

                    }
                }
                catch (Exception ex)
                {
                     throw ex;
                }

                cargarGrillaPagos_Boleta();

            }
            catch (Exception ex)
            {
                if (servicePagos != null)
                    servicePagos.Dispose();

                FinalizarEntity();
                FinalizarEntityFiles();
                throw ex;

            }

            FinalizarEntity();
            FinalizarEntityFiles();

        }

        public string GenerarPagoElectronico(int id_caa, int id_grupotramite)
        {

            return ""; //deshabilitado

            string url_pago_electronico = "";

            string WSPagos_url = Parametros.GetParam_ValorChar("Pagos.Url");
            string WSPagos_Usuario = Parametros.GetParam_ValorChar("SGI.Pagos.User");
            string WSPagos_Password = Parametros.GetParam_ValorChar("SGI.Pagos.Password");

            ws_pagos servicePagos = new ws_pagos();
            servicePagos.Url = WSPagos_url;

            BUDatosContribuyente datosContribuyente = CargarDatosContribuyente(id_caa, id_grupotramite);
            List<BUConcepto> lstConceptos = CargarConceptos(id_caa, id_grupotramite, ref servicePagos, WSPagos_Usuario, WSPagos_Password);

            SGI.Webservices.Pagos.wsResultado wsPagos_resultado = new SGI.Webservices.Pagos.wsResultado();

            BUPagoElectronico pagoElectronico = null;

            try
            {
                pagoElectronico = servicePagos.GenerarPagoElectronico(WSPagos_Usuario, WSPagos_Password, datosContribuyente, lstConceptos.ToArray(), ref wsPagos_resultado);
            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Error en ws GenerarPagoElectronico de ws_pagos");
                throw new Exception("El servicio de generación de boletas no esta disponible. Intente en otro momento.");
            }

            servicePagos.Dispose();

            if (!wsPagos_resultado.ErrorCode.Equals(0))
            {
                LogError.Write(new Exception("Error en ws GenerarPagoElectronico de ws_pagos"), wsPagos_resultado.ErrorDescription + " - Código Error: " + wsPagos_resultado.ErrorCode);
                throw new Exception("El servicio de generación de boletas no esta disponible. Intente en otro momento.");
            }

            url_pago_electronico = pagoElectronico.UrlPagoElectronico;

            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;

            try
            {
                //this.Conexion.BeginTrans();

                //int id_sol_pago = Logic.CAA_Solicitudes_Pagos_insert(id_caa, pagoElectronico.IdPago,
                //        (int)Constants.CAA_MedioPago.PagoElectronico, pagoElectronico.MontoTotal,
                //        "", 0, 0,
                //        "", pagoElectronico.UrlPagoElectronico, userId);

                //this.Conexion.CommitTrans();

                //AjaxControlToolkit.ToolkitScriptManager.RegisterClientScriptBlock(
                //    updPnlGenerarPagoElectronico, updPnlGenerarPagoElectronico.GetType(),
                //    "pagarOnLine", "cargarPaginaPagoElectronico('" + pagoElectronico.UrlPagoElectronico + "');", true);

            }
            catch (Exception ex)
            {
                //this.Conexion.RollbackTrans();
                throw ex;
            }

            return url_pago_electronico;

        }

        public string ConsultarEstadoPago(int id_pago)
        {
            string strEstadoPago = "";

            if (id_pago <= 0)
                return strEstadoPago;

            string WSPagos_url = Parametros.GetParam_ValorChar("Pagos.Url");
            string WSPagos_Usuario = Parametros.GetParam_ValorChar("SGI.Pagos.User");
            string WSPagos_Password = Parametros.GetParam_ValorChar("SGI.Pagos.Password");

            ws_pagos servicePagos = new ws_pagos();
            servicePagos.Url = WSPagos_url;

            SGI.Webservices.Pagos.wsResultado wsPagos_resultado = new SGI.Webservices.Pagos.wsResultado();


            try
            {
                strEstadoPago = servicePagos.GetEstadoPago(WSPagos_Usuario, WSPagos_Password, id_pago, ref wsPagos_resultado);
            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Error en ws GetEstadoPago de ws_pagos");
                throw new Exception("El servicio de generación de boletas no esta disponible. Intente en otro momento.");
            }

            if (wsPagos_resultado.ErrorCode != 0)
            {
                LogError.Write(new Exception("Error en ws GetEstadoPago de ws_pagos"), wsPagos_resultado.ErrorDescription + " - Código Error: " + wsPagos_resultado.ErrorCode);
                throw new Exception("El servicio de generación de boletas no esta disponible. Intente en otro momento.");
            }

            return strEstadoPago;

        }

        private List<BUConcepto> CargarConceptos(int id_solicitud, int id_grupotramite, ref ws_pagos servicePagos, string WSPagos_Usuario, string WSPagos_Password)
        {
            
            List<BUConcepto> wsPago_lstConcepto = new List<BUConcepto>();

            int id_subtipoexpediente = 0;
            decimal superficie_cubierta_dl = 0;
            decimal superficie_descubierta_dl = 0;

            if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
            {

                var query_solicitud =
                     (
                        from sol in db.SSIT_Solicitudes
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            sol.id_tipotramite,
                            sol.id_tipoexpediente,
                            sol.id_subtipoexpediente
                        }
                     ).FirstOrDefault();
                var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();
                var query_local=
                     (
                        from local in db.Encomienda_DatosLocal
                        where local.id_encomienda == enc.id_encomienda
                        select new
                        {
                            local.superficie_cubierta_dl,
                            local.superficie_descubierta_dl
                        }
                     ).FirstOrDefault();

                id_subtipoexpediente = (int)query_solicitud.id_subtipoexpediente;
                superficie_cubierta_dl = query_local.superficie_cubierta_dl.HasValue ? (decimal)query_local.superficie_cubierta_dl : 0;
                superficie_descubierta_dl = query_local.superficie_descubierta_dl.HasValue ? (decimal)query_local.superficie_descubierta_dl : 0;

            }
            else if (id_grupotramite == (int)Constants.GruposDeTramite.CP)
            {
                var query_solicitud =
                     (
                        from sol in db.CPadron_Solicitudes
                        join eDatos in db.CPadron_DatosLocal on sol.id_cpadron equals eDatos.id_cpadron into zr
                        from ed in zr.DefaultIfEmpty()
                        where sol.id_cpadron == id_solicitud
                        select new
                        {
                            sol.id_tipotramite,
                            sol.id_tipoexpediente,
                            sol.id_subtipoexpediente,
                            superficie_cubierta_dl = ed != null ? ed.superficie_cubierta_dl : 0,
                            superficie_descubierta_dl = ed != null ? ed.superficie_descubierta_dl : 0
                        }
                     ).FirstOrDefault();
                id_subtipoexpediente = (int)query_solicitud.id_subtipoexpediente;
                superficie_cubierta_dl = query_solicitud.superficie_cubierta_dl.HasValue ? (decimal)query_solicitud.superficie_cubierta_dl : 0;
                superficie_descubierta_dl = query_solicitud.superficie_descubierta_dl.HasValue ? (decimal)query_solicitud.superficie_descubierta_dl : 0;
            }
            else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
            {
                var query_solicitud =
                     (
                        from sol in db.Transf_Solicitudes
                        join eDatos in db.CPadron_DatosLocal on sol.id_cpadron equals eDatos.id_cpadron into zr
                        from ed in zr.DefaultIfEmpty()
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            sol.id_tipotramite,
                            sol.id_tipoexpediente,
                            sol.id_subtipoexpediente,
                            superficie_cubierta_dl = ed != null ? ed.superficie_cubierta_dl : 0,
                            superficie_descubierta_dl = ed != null ? ed.superficie_descubierta_dl : 0
                        }
                     ).FirstOrDefault();
                //TODO aca iria el de transferencia
                id_subtipoexpediente = (int)query_solicitud.id_subtipoexpediente;
                superficie_cubierta_dl = query_solicitud.superficie_cubierta_dl.HasValue ? (decimal)query_solicitud.superficie_cubierta_dl : 0;
                superficie_descubierta_dl = query_solicitud.superficie_descubierta_dl.HasValue ? (decimal)query_solicitud.superficie_descubierta_dl : 0;
            }
            string str_subtipo = Enum.GetName( typeof( Constants.SubtipoDeExpediente ),  id_subtipoexpediente);
            Constants.SubtipoDeExpediente enum_SubtipoDeExpediente;
            Enum.TryParse(str_subtipo, true, out enum_SubtipoDeExpediente);

            if (enum_SubtipoDeExpediente != Constants.SubtipoDeExpediente.SinPlanos &&
                 enum_SubtipoDeExpediente != Constants.SubtipoDeExpediente.ConPlanos &&
                 enum_SubtipoDeExpediente != Constants.SubtipoDeExpediente.InspeccionPrevia &&
                 enum_SubtipoDeExpediente != Constants.SubtipoDeExpediente.HabilitacionPrevia
                )
            {
                throw new Exception("Tipo de trámite " + enum_SubtipoDeExpediente.ToString() + " inválido para generar boleta.");
            }


            // buscar los conceptos a cobrar
            List<ucMediosPagos_conceptos> cobrar_conceptos = new List<ucMediosPagos_conceptos>();
            decimal superficieTotal = superficie_cubierta_dl + superficie_descubierta_dl;

            List<ucMediosPagos_Encomienda_Concepto_a_cobrar> lst_conceptos_a_buscar = new List<ucMediosPagos_Encomienda_Concepto_a_cobrar>();
            

            if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
            {
                
                lst_conceptos_a_buscar.Add(new ucMediosPagos_Encomienda_Concepto_a_cobrar
                                                {
                                                    cantidad = 1,
                                                    codigo_concepto = "SGI.Pagos.Concepto.UsoConforme"
                                                });

                if (id_subtipoexpediente == (int)Constants.SubtipoDeExpediente.SinPlanos)
                {

                    lst_conceptos_a_buscar.Add(new ucMediosPagos_Encomienda_Concepto_a_cobrar
                                                {
                                                    cantidad = 1,
                                                    codigo_concepto = "SGI.Pagos.Concepto.SSP"
                                                });
                }
                else if (id_subtipoexpediente == (int)Constants.SubtipoDeExpediente.ConPlanos)
                {
                    lst_conceptos_a_buscar.Add(new ucMediosPagos_Encomienda_Concepto_a_cobrar
                                                {
                                                    cantidad = 1,
                                                    codigo_concepto = "SGI.Pagos.Concepto.SCP"
                                                });

                    if (superficieTotal > 500)
                    {
                        decimal cantidad = (superficieTotal/ Convert.ToDecimal(500));
                        int cantidad_concepto = Convert.ToInt32(Math.Ceiling(cantidad -1 ));
                        lst_conceptos_a_buscar.Add(new ucMediosPagos_Encomienda_Concepto_a_cobrar
                                                {
                                                    cantidad = cantidad_concepto,
                                                    codigo_concepto = "SGI.Pagos.Concepto.SCP.Exc.500"
                                                });
                    }
                }
                else if (id_subtipoexpediente == (int)Constants.SubtipoDeExpediente.InspeccionPrevia )
                {
                    lst_conceptos_a_buscar.Add(new ucMediosPagos_Encomienda_Concepto_a_cobrar
                    {
                        cantidad = 1,
                        codigo_concepto = "SGI.Pagos.Concepto.IP"
                    });

                    if (superficieTotal > 500)
                    {
                        decimal cantidad = (superficieTotal / Convert.ToDecimal(500));
                        int cantidad_concepto = Convert.ToInt32(Math.Ceiling(cantidad - 1));

                        lst_conceptos_a_buscar.Add(new ucMediosPagos_Encomienda_Concepto_a_cobrar
                        {
                            cantidad = cantidad_concepto,
                            codigo_concepto = "SGI.Pagos.Concepto.IP.Exc.500"
                        });
                    }

                }
                else if (id_subtipoexpediente == (int)Constants.SubtipoDeExpediente.HabilitacionPrevia)
                {
                    lst_conceptos_a_buscar.Add(new ucMediosPagos_Encomienda_Concepto_a_cobrar
                    {
                        cantidad = 1,
                        codigo_concepto = "SGI.Pagos.Concepto.HP"
                    });

                    if (superficieTotal > 500)
                    {
                        decimal cantidad = (superficieTotal / Convert.ToDecimal(500));
                        int cantidad_concepto = Convert.ToInt32(Math.Ceiling(cantidad - 1));

                        lst_conceptos_a_buscar.Add(new ucMediosPagos_Encomienda_Concepto_a_cobrar
                        {
                            cantidad = cantidad_concepto,
                            codigo_concepto = "SGI.Pagos.Concepto.HP.Exc.500"
                        });
                    }

                }

            }
            else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
            {
                //if (id_subtipoexpediente == (int)Constants.SubtipoDeExpediente.SinPlanos ||
                //    id_subtipoexpediente == (int)Constants.SubtipoDeExpediente.ConPlanos)
                //{

                //    lst_conceptos_a_buscar.Add(new ucMediosPagos_Encomienda_Concepto_a_cobrar
                //    {
                //        cantidad = 1,
                //        codigo_concepto = "SGI.Pagos.Concepto.TRS"
                //    });
                //}
                //else //if (id_subtipoexpediente == (int)Constants.SubtipoDeExpediente.InspeccionPrevia)
                //{
                //    lst_conceptos_a_buscar.Add(new ucMediosPagos_Encomienda_Concepto_a_cobrar
                //    {
                //        cantidad = 1,
                //        codigo_concepto = "SGI.Pagos.Concepto.TRE"
                //    });
                //}
                lst_conceptos_a_buscar.Add(new ucMediosPagos_Encomienda_Concepto_a_cobrar
                {
                    cantidad = 1,
                    codigo_concepto = "SGI.Pagos.Concepto.TRANSTR"
                });
            }

            // busca en la base los conceptos a cobrar
            // ---------------------------------------
            string[] arrCodConceptosCobrar = lst_conceptos_a_buscar.Select(s => s.codigo_concepto).ToArray();

            var qConceptosCobrar = (from con_bui in db.Conceptos_BUI_Independientes
                                    where arrCodConceptosCobrar.Contains(con_bui.keycode)
                                    select con_bui);
                         
            foreach (var item in  qConceptosCobrar)
            {
                string cod_concepto_completo =  item.cod_concepto_1.ToString().PadLeft(2,Convert.ToChar("0")) + "." + 
                                                item.cod_concepto_2.ToString().PadLeft(2,Convert.ToChar("0")) + "." +
                                                item.cod_concepto_3.ToString().PadLeft(2,Convert.ToChar("0"));
                
                ucMediosPagos_conceptos concepto_a_cobrar = new ucMediosPagos_conceptos(cod_concepto_completo);
                concepto_a_cobrar.descripcion = item.descripcion_concepto;
                concepto_a_cobrar.admite_reglas = item.admite_reglas;
                concepto_a_cobrar.cantidad = lst_conceptos_a_buscar.FirstOrDefault(x=> x.codigo_concepto == item.keycode).cantidad;

                cobrar_conceptos.Add(concepto_a_cobrar);
            }

            // pedir conceptos vigentes al ws para enviar la informacion correcta
            SGI.Webservices.Pagos.wsResultado wsPagos_resultado = new SGI.Webservices.Pagos.wsResultado();

            BUConcepto[] arrayConceptos = null;
            try
            {
                arrayConceptos = servicePagos.GetConceptos(WSPagos_Usuario, WSPagos_Password, ref wsPagos_resultado);
            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Error en ws GetConceptos de ws_pagos");
                throw new Exception("El servicio de generación de boletas no esta disponible. Intente en otro momento.");
            }


            if (wsPagos_resultado.ErrorCode != 0)
            {
                LogError.Write(new Exception("Error en ws GetConceptos de ws_pagos"), wsPagos_resultado.ErrorDescription + " - Código Error: " + wsPagos_resultado.ErrorCode);
                throw new Exception("El servicio de generación de boletas no esta disponible. Intente en otro momento.");
            }


            foreach (ucMediosPagos_conceptos item in cobrar_conceptos)
            {

               BUConcepto ws_concepto  = arrayConceptos.Where(x => x.CodConcepto1 == item.concepto1 && x.CodConcepto2 == item.concepto2 && x.CodConcepto3 == item.concepto3).FirstOrDefault();

               if (ws_concepto != null)
               {
                   ws_concepto.Cantidad = item.cantidad;
                   // Los conceptos de habilitaciones simples sin plano y con plano establecen su valor dependiendo de la superficie.
                   if (item.admite_reglas)
                        ws_concepto.ValorDetalle = superficie_cubierta_dl + superficie_descubierta_dl;

                   wsPago_lstConcepto.Add(ws_concepto);
               }

            }

            if (wsPago_lstConcepto == null || wsPago_lstConcepto.Count == 0)
                throw new Exception("No se encontraron los conceptos parametrizados correspondientes para la solicitud.");

            if ( wsPago_lstConcepto.Count != cobrar_conceptos.Count )
                throw new Exception("No se encontraron los conceptos a cobrar.");

            return wsPago_lstConcepto;

        }

        private BUDatosContribuyente CargarDatosContribuyente(int id_solicitud, int id_grupotramite)
        {
            BUDatosContribuyente datosContribuyente = null;

            List<ucMediosPagos_Encomienda_Titular> lstTitular = TraerTitularesSolicitud_paraWsPagos(id_solicitud, id_grupotramite);

            if (lstTitular.Count == 0)
                throw new Exception(string.Format("No se encontraron titulares para la solicitud Nº {0}.", id_solicitud));

            ucMediosPagos_Encomienda_Titular titular= lstTitular[0];
            string tipoPersona = titular.TipoPersona;

            datosContribuyente = new BUDatosContribuyente();
            string strNroDoc = titular.cuit.Replace("-", "");
            
            datosContribuyente.TipoDoc = BUTipodocumento.CUIT;
            datosContribuyente.Documento = strNroDoc;
            datosContribuyente.TipoPersona =  (titular.TipoPersona.Equals("PF")) ? BUTipoPersona.Fisica : BUTipoPersona.Juridica;
            datosContribuyente.ApellidoyNombre = titular.ApellidoNomRazon;
            datosContribuyente.Direccion = id_solicitud.ToString() + " - " + titular.Domicilio;
            datosContribuyente.CodPost = titular.Codigo_Postal;
            datosContribuyente.Piso = titular.piso;
            datosContribuyente.Departamento = titular.Depto;
            datosContribuyente.Email = titular.email;
            datosContribuyente.Localidad = titular.localidad;

            return datosContribuyente;
        }

        private string SoloDigitos(string cadena)
        {
            string cadenaSalida = "";

            foreach (char item in cadena.ToCharArray())
            {
                if (Char.IsDigit(item))
                {
                    cadenaSalida = cadenaSalida + item;
                }
            }

            return cadenaSalida;
        }



        #endregion


     }

}