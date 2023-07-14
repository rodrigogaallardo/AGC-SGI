using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;
using System.Data.Entity.Core.Objects;
using SGI.WebServices;

namespace SGI.GestionTramite.Controls
{
    public partial class ucDatosContacto : System.Web.UI.UserControl
    {
        public void LoadData(int id_grupotramite, int id_solicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            if(id_grupotramite == (int)Constants.GruposDeTramite.HAB)
            {
                var objsol = (from sol in db.SSIT_Solicitudes
                              where sol.id_solicitud == id_solicitud
                              select new
                              {
                                  sol.id_solicitud,
                                  sol.CodArea,
                                  sol.Prefijo,
                                  sol.Sufijo
                              }).FirstOrDefault();


                var personaJuridica = (from pj in db.SSIT_Solicitudes_Titulares_PersonasJuridicas
                            where pj.id_solicitud == id_solicitud
                            select pj).FirstOrDefault();


                var personaFisica = (from pf in db.SSIT_Solicitudes_Titulares_PersonasFisicas
                                     where pf.id_solicitud == id_solicitud
                                     select pf).FirstOrDefault();
                                     
                if (objsol != null)
                {
                    if (string.IsNullOrEmpty(objsol.CodArea))
                        codigoArea.Text = string.Empty;
                    else
                        codigoArea.Text = objsol.CodArea.ToString();
                    if (string.IsNullOrEmpty(objsol.Prefijo))
                        prefijo.Text = string.Empty;
                    else
                        prefijo.Text = objsol.Prefijo.ToString();
                    if (string.IsNullOrEmpty(objsol.Sufijo))
                        sufijo.Text = string.Empty;
                    else
                        sufijo.Text = objsol.Sufijo.ToString();
                }
                else
                {
                    codigoArea.Text = string.Empty;
                    prefijo.Text = string.Empty;
                    sufijo.Text = string.Empty;
                }

                if (personaJuridica != null)
                {
                    telJuridico.Text = personaJuridica.Telefono.ToString();
                    emailJuridico.Text = personaJuridica.Email.ToString();
                }
                else
                {
                    pnlDatosJuridico.Visible = false;
                }

                if(personaFisica != null)
                {
                    telefonoMovil.Text = personaFisica.TelefonoMovil.ToString();
                    telefonoFijo.Text = personaFisica.Telefono.ToString();
                    emailFisico.Text = personaFisica.Email.ToString();
                }
                else
                {
                    pnlDatosFisicos.Visible = false;
                }
            }
            else if(id_grupotramite == (int)Constants.GruposDeTramite.TR)
            {
                pnlDatosTramite.Visible = false;

                var personaJuridica = (from pj in db.Transf_Titulares_Solicitud_PersonasJuridicas
                                       where pj.id_solicitud == id_solicitud
                                       select pj).FirstOrDefault();


                var personaFisica = (from pf in db.Transf_Titulares_Solicitud_PersonasFisicas
                                     where pf.id_solicitud == id_solicitud
                                     select pf).FirstOrDefault();

                if (personaJuridica != null)
                {

                    telJuridico.Text = personaJuridica.Telefono.ToString();
                    emailJuridico.Text = personaJuridica.Email.ToString();

                    pnlDatosFisicos.Visible = false;
                }

                if (personaFisica != null)
                {

                    telefonoMovil.Text = personaFisica.TelefonoMovil.ToString();
                    telefonoFijo.Text = personaFisica.Telefono.ToString();
                    emailFisico.Text = personaFisica.Email.ToString();

                    pnlDatosJuridico.Visible = false;
                }
            }
            else
            {
                pnlDatosTramite.Visible = false;

                var personaJuridica = (from pj in db.CPadron_Titulares_Solicitud_PersonasJuridicas
                                       where pj.id_cpadron == id_solicitud
                                       select pj).FirstOrDefault();


                var personaFisica = (from pf in db.CPadron_Titulares_Solicitud_PersonasFisicas
                                     where pf.id_cpadron == id_solicitud
                                     select pf).FirstOrDefault();

                if (personaJuridica != null)
                {

                    telJuridico.Text = personaJuridica.Telefono.ToString();
                    emailJuridico.Text = personaJuridica.Email.ToString();

                    pnlDatosFisicos.Visible = false;
                }

                if (personaFisica != null)
                {

                    telefonoMovil.Text = personaFisica.TelefonoMovil.ToString();
                    telefonoFijo.Text = personaFisica.Telefono.ToString();
                    emailFisico.Text = personaFisica.Email.ToString();

                    pnlDatosJuridico.Visible = false;
                }
            }    
        }

    }
}