using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGI.Model
{

    public class MailSolicitudNuevaPuerta
    {
        public string Username { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public int? Seccion { get; set; }
        public string Manzana { get; set; }
        public string Parcela { get; set; }
        public int? NroPartidaMatriz { get; set; }
        public string Calle { get; set; }
        public string NroPuerta { get; set; }
        public string urlFoto { get; set; }
        public string UrlMapa { get; set; }
    }
    public class MailWelcome
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Renglon1 { get; set; }
        public string Renglon2 { get; set; }
        public string Renglon3 { get; set; }
        public string Urlactivacion { get; set; }
        public string UrlPage { get; set; }
        public string ApplicationName { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }

    public class MailPassRecovery
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Renglon1 { get; set; }
        public string Renglon2 { get; set; }
        public string Renglon3 { get; set; }
        public string UrlLogin { get; set; }
        public string UrlPage { get; set; }
        public string ApplicationName { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }

    public class MailUsuario
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Renglon1 { get; set; }
        public string Renglon2 { get; set; }
        public string Renglon3 { get; set; }
        public string UrlLogin { get; set; }
    }

    public class MailCaratula
    {
        public string Nombre { get; set; }
        public string NumeroSolicitud { get; set; }
        public string UrlLogin { get; set; }
    }

    public class MailPagoPendiente
    {
        public string Nombre { get; set; }
        public string Renglon1 { get; set; }
        public string NumeroSolicitud { get; set; }
        public string UrlLogin { get; set; }
    }

    public class MailCorreccionSolicitud
    {
        public string Nombre { get; set; }
        public string Renglon1 { get; set; }
        public string NumeroSolicitud { get; set; }
        public string UrlLogin { get; set; }
    }

    public class MailAsignarCalificador
    {
        public string Nombre { get; set; }
        public string Renglon1 { get; set; }
        public string NumeroSolicitud { get; set; }
        public string UrlLogin { get; set; }
    }

    public class MailAprobacionDG
    {
        public string Nombre { get; set; }
        public string Renglon1 { get; set; }
        public string NumeroSolicitud { get; set; }
        public string UrlLogin { get; set; }
    }

    public class MailWelcomeECADatos
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Renglon1 { get; set; }
        public string Renglon2 { get; set; }
        public string Renglon3 { get; set; }
        public string Urlactivacion { get; set; }
        public string UrlPage { get; set; }
        public string ApplicationName { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }

}