using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls.Transferencias
{
    public partial class ucTitulares : System.Web.UI.UserControl
    {
        public void LoadData(int id_solicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            var lstTitulares = (from pf in db.Transf_Titulares_PersonasFisicas
                                where pf.id_solicitud == id_solicitud
                                select new
                                {
                                    id_persona = pf.id_personafisica,
                                    TipoPersona = "PF",
                                    TipoPersonaDesc = "Persona Física",
                                    ApellidoNomRazon = pf.Apellido + " " + pf.Nombres,
                                    CUIT = pf.Cuit,
                                    Domicilio = pf.Calle + " " + //pf.Nro_Puerta.ToString() +
                                                (pf.Piso.Length > 0 ? " Piso: " + pf.Piso : "") +
                                                (pf.Depto.Length > 0 ? " Depto: " + pf.Depto : "")
                                }).Union(
                                    (from pj in db.Transf_Titulares_PersonasJuridicas
                                     where pj.id_solicitud == id_solicitud
                                     select new
                                     {
                                         id_persona = pj.id_personajuridica,
                                         TipoPersona = "PJ",
                                         TipoPersonaDesc = "Persona Jurídica",
                                         ApellidoNomRazon = pj.Razon_Social,
                                         CUIT = pj.CUIT,
                                         Domicilio = pj.Calle + " " + //(pj.NroPuerta.HasValue ? pj.NroPuerta.Value.ToString() : "") +
                                                    (pj.Piso.Length > 0 ? " Piso: " + pj.Piso : "") +
                                                    (pj.Depto.Length > 0 ? " Depto: " + pj.Depto : "")
                                     })).ToList();


            grdTitulares.DataSource = lstTitulares;
            grdTitulares.DataBind();

            var lstfirmantes =
                (
                    from pj in db.Transf_Firmantes_PersonasJuridicas
                    join titpj in db.Transf_Titulares_PersonasJuridicas on pj.id_personajuridica equals titpj.id_personajuridica
                    join tcl in db.TiposDeCaracterLegal on pj.id_tipocaracter equals tcl.id_tipocaracter
                    join tdoc in db.TipoDocumentoPersonal on pj.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                    where pj.id_solicitud == id_solicitud

                    select new
                    {
                        id_firmante = pj.id_firmante_pj,
                        pj.id_solicitud,
                        TipoPersona = "PJ",
                        ApellidoNombres = pj.Apellido + " " + pj.Nombres,
                        TipoDoc = tdoc.Nombre,
                        NroDoc = pj.Nro_Documento,
                        CaracterLegal = tcl.nom_tipocaracter,
                        pj.cargo_firmante_pj,
                        FirmanteDe = titpj.Razon_Social
                    }).Union(
                    from pf in db.Transf_Firmantes_PersonasFisicas
                    join titpf in db.Transf_Titulares_PersonasFisicas on pf.id_personafisica equals titpf.id_personafisica
                    join tcl in db.TiposDeCaracterLegal on pf.id_tipocaracter equals tcl.id_tipocaracter
                    join tdoc in db.TipoDocumentoPersonal on pf.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                    where pf.id_solicitud == id_solicitud
                    select new
                    {
                        id_firmante = pf.id_firmante_pf,
                        pf.id_solicitud,
                        TipoPersona = "PF",
                        ApellidoNombres = pf.Apellido + " " + pf.Nombres,
                        TipoDoc = tdoc.Nombre,
                        NroDoc = pf.Nro_Documento,
                        CaracterLegal = tcl.nom_tipocaracter,
                        cargo_firmante_pj = "",
                        FirmanteDe = titpf.Apellido + ", " + titpf.Nombres
                    }
                ).ToList();
            grdFirmantes.DataSource = lstfirmantes;
            grdFirmantes.DataBind();

            updGrillaTitulares.Update();
            db.Dispose();
        }
    }
}