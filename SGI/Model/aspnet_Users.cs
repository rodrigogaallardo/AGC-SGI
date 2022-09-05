

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class aspnet_Users
    {
        public aspnet_Users()
        {
            this.ENG_EquipoDeTrabajo = new HashSet<ENG_EquipoDeTrabajo>();
            this.ENG_EquipoDeTrabajo1 = new HashSet<ENG_EquipoDeTrabajo>();
            this.SGI_Tarea_Generar_Expediente_Procesos = new HashSet<SGI_Tarea_Generar_Expediente_Procesos>();
            this.SGI_Tarea_Generar_Expediente_Procesos1 = new HashSet<SGI_Tarea_Generar_Expediente_Procesos>();
            this.SGI_Perfiles = new HashSet<SGI_Perfiles>();
            this.SGI_Perfiles1 = new HashSet<SGI_Perfiles>();
            this.SGI_PerfilesUsuarios = new HashSet<SGI_Perfiles>();
            this.SGI_Menues = new HashSet<SGI_Menues>();
            this.SGI_Menues1 = new HashSet<SGI_Menues>();
            this.aspnet_Roles = new HashSet<aspnet_Roles>();
            this.Rubros_Historial_Cambios_UsuariosIntervinientes = new HashSet<Rubros_Historial_Cambios_UsuariosIntervinientes>();
            this.CPadron_HistorialEstados = new HashSet<CPadron_HistorialEstados>();
            this.CPadron_Normativas = new HashSet<CPadron_Normativas>();
            this.CPadron_Normativas1 = new HashSet<CPadron_Normativas>();
            this.CPadron_Ubicaciones = new HashSet<CPadron_Ubicaciones>();
            this.Ubicaciones_PropiedadHorizontal_Historial_Cambios_UsuariosIntervinientes = new HashSet<Ubicaciones_PropiedadHorizontal_Historial_Cambios_UsuariosIntervinientes>();
            this.Rel_UsuariosProf_Roles_Clasificacion = new HashSet<Rel_UsuariosProf_Roles_Clasificacion>();
            this.SGI_SADE_Procesos = new HashSet<SGI_SADE_Procesos>();
            this.SGI_SADE_Procesos1 = new HashSet<SGI_SADE_Procesos>();
            this.Instructivos = new HashSet<Instructivos>();
            this.Instructivos1 = new HashSet<Instructivos>();
            this.Transf_Titulares_PersonasFisicas = new HashSet<Transf_Titulares_PersonasFisicas>();
            this.Transf_Titulares_PersonasFisicas1 = new HashSet<Transf_Titulares_PersonasFisicas>();
            this.Transf_Titulares_PersonasJuridicas = new HashSet<Transf_Titulares_PersonasJuridicas>();
            this.Transf_Titulares_PersonasJuridicas1 = new HashSet<Transf_Titulares_PersonasJuridicas>();
            this.SGI_Tramites_Tareas = new HashSet<SGI_Tramites_Tareas>();
            this.SSIT_Solicitudes_Encomienda = new HashSet<SSIT_Solicitudes_Encomienda>();
            this.SGI_LIZA_Procesos = new HashSet<SGI_LIZA_Procesos>();
            this.SGI_LIZA_Procesos1 = new HashSet<SGI_LIZA_Procesos>();
            this.SSIT_Solicitudes_Pagos = new HashSet<SSIT_Solicitudes_Pagos>();
            this.Encomienda_DocumentosAdjuntos = new HashSet<Encomienda_DocumentosAdjuntos>();
            this.Encomienda_DocumentosAdjuntos1 = new HashSet<Encomienda_DocumentosAdjuntos>();
            this.Encomienda_HistorialEstados = new HashSet<Encomienda_HistorialEstados>();
            this.CPadron_Solicitudes_Observaciones = new HashSet<CPadron_Solicitudes_Observaciones>();
            this.Profesional = new HashSet<Profesional>();
            this.SGI_FiltrosBusqueda = new HashSet<SGI_FiltrosBusqueda>();
            this.Rel_UsuariosProf_Roles_Clasificacion1 = new HashSet<Rel_UsuariosProf_Roles_Clasificacion>();
            this.Sectores_SADE = new HashSet<Sectores_SADE>();
            this.Sectores_SADE1 = new HashSet<Sectores_SADE>();
            this.RAL_Licencias = new HashSet<RAL_Licencias>();
            this.SADE_Estados_Expedientes = new HashSet<SADE_Estados_Expedientes>();
            this.SADE_Estados_Expedientes1 = new HashSet<SADE_Estados_Expedientes>();
            this.SGI_Tareas_Pases_Sectores = new HashSet<SGI_Tareas_Pases_Sectores>();
            this.SGI_Tareas_Pases_Sectores1 = new HashSet<SGI_Tareas_Pases_Sectores>();
            this.SGI_Profiles1 = new HashSet<SGI_Profiles>();
            this.SGI_Profiles2 = new HashSet<SGI_Profiles>();
            this.CPadron_Solicitudes = new HashSet<CPadron_Solicitudes>();
            this.CPadron_Solicitudes1 = new HashSet<CPadron_Solicitudes>();
            this.CPadron_DatosLocal = new HashSet<CPadron_DatosLocal>();
            this.CPadron_DatosLocal1 = new HashSet<CPadron_DatosLocal>();
            this.Encomienda_RubrosCN = new HashSet<Encomienda_RubrosCN>();
            this.Encomienda_RubrosCN_AT_Anterior = new HashSet<Encomienda_RubrosCN_AT_Anterior>();
            this.Transf_DatosLocal = new HashSet<Transf_DatosLocal>();
            this.Transf_DatosLocal1 = new HashSet<Transf_DatosLocal>();
            this.Transf_Normativas = new HashSet<Transf_Normativas>();
            this.Transf_Normativas1 = new HashSet<Transf_Normativas>();
            this.Transf_Ubicaciones = new HashSet<Transf_Ubicaciones>();
            this.Encomienda = new HashSet<Encomienda>();
            this.Solicitud_planoVisado = new HashSet<Solicitud_planoVisado>();
            this.Ubicaciones_Historial_Cambios_UsuariosIntervinientes = new HashSet<Ubicaciones_Historial_Cambios_UsuariosIntervinientes>();
            this.Transf_Solicitudes_Pagos = new HashSet<Transf_Solicitudes_Pagos>();
            this.SSIT_Permisos_DatosAdicionales = new HashSet<SSIT_Permisos_DatosAdicionales>();
            this.SSIT_Solicitudes_DatosLocal = new HashSet<SSIT_Solicitudes_DatosLocal>();
            this.SSIT_Solicitudes_DatosLocal1 = new HashSet<SSIT_Solicitudes_DatosLocal>();
            this.SSIT_Solicitudes_RubrosCN = new HashSet<SSIT_Solicitudes_RubrosCN>();
            this.SSIT_DocumentosAdjuntos = new HashSet<SSIT_DocumentosAdjuntos>();
            this.SSIT_DocumentosAdjuntos1 = new HashSet<SSIT_DocumentosAdjuntos>();
        }
    
        public System.Guid ApplicationId { get; set; }
        public System.Guid UserId { get; set; }
        public string UserName { get; set; }
        public string LoweredUserName { get; set; }
        public string MobileAlias { get; set; }
        public bool IsAnonymous { get; set; }
        public System.DateTime LastActivityDate { get; set; }
    
        public virtual aspnet_Applications aspnet_Applications { get; set; }
        public virtual aspnet_Membership aspnet_Membership { get; set; }
        public virtual ICollection<ENG_EquipoDeTrabajo> ENG_EquipoDeTrabajo { get; set; }
        public virtual ICollection<ENG_EquipoDeTrabajo> ENG_EquipoDeTrabajo1 { get; set; }
        public virtual ICollection<SGI_Tarea_Generar_Expediente_Procesos> SGI_Tarea_Generar_Expediente_Procesos { get; set; }
        public virtual ICollection<SGI_Tarea_Generar_Expediente_Procesos> SGI_Tarea_Generar_Expediente_Procesos1 { get; set; }
        public virtual ICollection<SGI_Perfiles> SGI_Perfiles { get; set; }
        public virtual ICollection<SGI_Perfiles> SGI_Perfiles1 { get; set; }
        public virtual ICollection<SGI_Perfiles> SGI_PerfilesUsuarios { get; set; }
        public virtual ICollection<SGI_Menues> SGI_Menues { get; set; }
        public virtual ICollection<SGI_Menues> SGI_Menues1 { get; set; }
        public virtual ICollection<aspnet_Roles> aspnet_Roles { get; set; }
        public virtual ICollection<Rubros_Historial_Cambios_UsuariosIntervinientes> Rubros_Historial_Cambios_UsuariosIntervinientes { get; set; }
        public virtual ICollection<CPadron_HistorialEstados> CPadron_HistorialEstados { get; set; }
        public virtual ICollection<CPadron_Normativas> CPadron_Normativas { get; set; }
        public virtual ICollection<CPadron_Normativas> CPadron_Normativas1 { get; set; }
        public virtual ICollection<CPadron_Ubicaciones> CPadron_Ubicaciones { get; set; }
        public virtual ICollection<Ubicaciones_PropiedadHorizontal_Historial_Cambios_UsuariosIntervinientes> Ubicaciones_PropiedadHorizontal_Historial_Cambios_UsuariosIntervinientes { get; set; }
        public virtual ICollection<Rel_UsuariosProf_Roles_Clasificacion> Rel_UsuariosProf_Roles_Clasificacion { get; set; }
        public virtual ICollection<SGI_SADE_Procesos> SGI_SADE_Procesos { get; set; }
        public virtual ICollection<SGI_SADE_Procesos> SGI_SADE_Procesos1 { get; set; }
        public virtual ICollection<Instructivos> Instructivos { get; set; }
        public virtual ICollection<Instructivos> Instructivos1 { get; set; }
        public virtual ICollection<Transf_Titulares_PersonasFisicas> Transf_Titulares_PersonasFisicas { get; set; }
        public virtual ICollection<Transf_Titulares_PersonasFisicas> Transf_Titulares_PersonasFisicas1 { get; set; }
        public virtual ICollection<Transf_Titulares_PersonasJuridicas> Transf_Titulares_PersonasJuridicas { get; set; }
        public virtual ICollection<Transf_Titulares_PersonasJuridicas> Transf_Titulares_PersonasJuridicas1 { get; set; }
        public virtual ICollection<SGI_Tramites_Tareas> SGI_Tramites_Tareas { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Encomienda> SSIT_Solicitudes_Encomienda { get; set; }
        public virtual ICollection<SGI_LIZA_Procesos> SGI_LIZA_Procesos { get; set; }
        public virtual ICollection<SGI_LIZA_Procesos> SGI_LIZA_Procesos1 { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Pagos> SSIT_Solicitudes_Pagos { get; set; }
        public virtual ICollection<Encomienda_DocumentosAdjuntos> Encomienda_DocumentosAdjuntos { get; set; }
        public virtual ICollection<Encomienda_DocumentosAdjuntos> Encomienda_DocumentosAdjuntos1 { get; set; }
        public virtual ICollection<Encomienda_HistorialEstados> Encomienda_HistorialEstados { get; set; }
        public virtual ICollection<CPadron_Solicitudes_Observaciones> CPadron_Solicitudes_Observaciones { get; set; }
        public virtual ICollection<Profesional> Profesional { get; set; }
        public virtual ICollection<SGI_FiltrosBusqueda> SGI_FiltrosBusqueda { get; set; }
        public virtual ICollection<Rel_UsuariosProf_Roles_Clasificacion> Rel_UsuariosProf_Roles_Clasificacion1 { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Sectores_SADE> Sectores_SADE { get; set; }
        public virtual ICollection<Sectores_SADE> Sectores_SADE1 { get; set; }
        public virtual ICollection<RAL_Licencias> RAL_Licencias { get; set; }
        public virtual ICollection<SADE_Estados_Expedientes> SADE_Estados_Expedientes { get; set; }
        public virtual ICollection<SADE_Estados_Expedientes> SADE_Estados_Expedientes1 { get; set; }
        public virtual ICollection<SGI_Tareas_Pases_Sectores> SGI_Tareas_Pases_Sectores { get; set; }
        public virtual ICollection<SGI_Tareas_Pases_Sectores> SGI_Tareas_Pases_Sectores1 { get; set; }
        public virtual SGI_Profiles SGI_Profiles { get; set; }
        public virtual ICollection<SGI_Profiles> SGI_Profiles1 { get; set; }
        public virtual ICollection<SGI_Profiles> SGI_Profiles2 { get; set; }
        public virtual ICollection<CPadron_Solicitudes> CPadron_Solicitudes { get; set; }
        public virtual ICollection<CPadron_Solicitudes> CPadron_Solicitudes1 { get; set; }
        public virtual ICollection<CPadron_DatosLocal> CPadron_DatosLocal { get; set; }
        public virtual ICollection<CPadron_DatosLocal> CPadron_DatosLocal1 { get; set; }
        public virtual ICollection<Encomienda_RubrosCN> Encomienda_RubrosCN { get; set; }
        public virtual ICollection<Encomienda_RubrosCN_AT_Anterior> Encomienda_RubrosCN_AT_Anterior { get; set; }
        public virtual ICollection<Transf_DatosLocal> Transf_DatosLocal { get; set; }
        public virtual ICollection<Transf_DatosLocal> Transf_DatosLocal1 { get; set; }
        public virtual ICollection<Transf_Normativas> Transf_Normativas { get; set; }
        public virtual ICollection<Transf_Normativas> Transf_Normativas1 { get; set; }
        public virtual ICollection<Transf_Ubicaciones> Transf_Ubicaciones { get; set; }
        public virtual ICollection<Encomienda> Encomienda { get; set; }
        public virtual ICollection<Solicitud_planoVisado> Solicitud_planoVisado { get; set; }
        public virtual ICollection<Ubicaciones_Historial_Cambios_UsuariosIntervinientes> Ubicaciones_Historial_Cambios_UsuariosIntervinientes { get; set; }
        public virtual ICollection<Transf_Solicitudes_Pagos> Transf_Solicitudes_Pagos { get; set; }
        public virtual ICollection<SSIT_Permisos_DatosAdicionales> SSIT_Permisos_DatosAdicionales { get; set; }
        public virtual ICollection<SSIT_Solicitudes_DatosLocal> SSIT_Solicitudes_DatosLocal { get; set; }
        public virtual ICollection<SSIT_Solicitudes_DatosLocal> SSIT_Solicitudes_DatosLocal1 { get; set; }
        public virtual ICollection<SSIT_Solicitudes_RubrosCN> SSIT_Solicitudes_RubrosCN { get; set; }
        public virtual ICollection<SSIT_DocumentosAdjuntos> SSIT_DocumentosAdjuntos { get; set; }
        public virtual ICollection<SSIT_DocumentosAdjuntos> SSIT_DocumentosAdjuntos1 { get; set; }
    }
}
