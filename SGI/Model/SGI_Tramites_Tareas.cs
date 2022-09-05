

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Tramites_Tareas
    {
        public SGI_Tramites_Tareas()
        {
            this.SGI_SADE_Procesos = new HashSet<SGI_SADE_Procesos>();
            this.SGI_Solicitudes_Pagos = new HashSet<SGI_Solicitudes_Pagos>();
            this.SGI_Tarea_Aprobado = new HashSet<SGI_Tarea_Aprobado>();
            this.SGI_Tarea_Asignar_Calificador = new HashSet<SGI_Tarea_Asignar_Calificador>();
            this.SGI_Tarea_Asignar_Inspector = new HashSet<SGI_Tarea_Asignar_Inspector>();
            this.SGI_Tarea_Calificacion_Tecnica_Legal = new HashSet<SGI_Tarea_Calificacion_Tecnica_Legal>();
            this.SGI_Tarea_Carga_Tramite = new HashSet<SGI_Tarea_Carga_Tramite>();
            this.SGI_Tarea_Dictamen_Asignar_Profesional = new HashSet<SGI_Tarea_Dictamen_Asignar_Profesional>();
            this.SGI_Tarea_Dictamen_GEDO = new HashSet<SGI_Tarea_Dictamen_GEDO>();
            this.SGI_Tarea_Dictamen_Revisar_Tramite = new HashSet<SGI_Tarea_Dictamen_Revisar_Tramite>();
            this.SGI_Tarea_Dictamen_Revision_Gerente = new HashSet<SGI_Tarea_Dictamen_Revision_Gerente>();
            this.SGI_Tarea_Dictamen_Revision_SubGerente = new HashSet<SGI_Tarea_Dictamen_Revision_SubGerente>();
            this.SGI_Tarea_Enviar_PVH = new HashSet<SGI_Tarea_Enviar_PVH>();
            this.SGI_Tarea_Fin_Tramite = new HashSet<SGI_Tarea_Fin_Tramite>();
            this.SGI_Tarea_Generar_Expediente = new HashSet<SGI_Tarea_Generar_Expediente>();
            this.SGI_Tarea_Generar_Expediente_Procesos = new HashSet<SGI_Tarea_Generar_Expediente_Procesos>();
            this.SGI_Tarea_Pagos_log = new HashSet<SGI_Tarea_Pagos_log>();
            this.SGI_Tarea_Rechazo_En_SADE = new HashSet<SGI_Tarea_Rechazo_En_SADE>();
            this.SGI_Tarea_Resultado_Inspector = new HashSet<SGI_Tarea_Resultado_Inspector>();
            this.SGI_Tarea_Revision_Dictamenes = new HashSet<SGI_Tarea_Revision_Dictamenes>();
            this.SGI_Tarea_Revision_Director = new HashSet<SGI_Tarea_Revision_Director>();
            this.SGI_Tarea_Revision_Gerente = new HashSet<SGI_Tarea_Revision_Gerente>();
            this.SGI_Tarea_Revision_Pagos = new HashSet<SGI_Tarea_Revision_Pagos>();
            this.SGI_Tarea_Revision_SubGerente = new HashSet<SGI_Tarea_Revision_SubGerente>();
            this.SGI_Tarea_Revision_Tecnica_Legal = new HashSet<SGI_Tarea_Revision_Tecnica_Legal>();
            this.SGI_Tarea_Validar_Zonificacion = new HashSet<SGI_Tarea_Validar_Zonificacion>();
            this.SGI_Tarea_Verificacion_AVH = new HashSet<SGI_Tarea_Verificacion_AVH>();
            this.SGI_Tramites_Tareas_CPADRON = new HashSet<SGI_Tramites_Tareas_CPADRON>();
            this.SGI_Tramites_Tareas_HAB = new HashSet<SGI_Tramites_Tareas_HAB>();
            this.SGI_Tramites_Tareas_TRANSF = new HashSet<SGI_Tramites_Tareas_TRANSF>();
            this.SGI_Tarea_Enviar_AVH = new HashSet<SGI_Tarea_Enviar_AVH>();
            this.SGI_Tarea_Revision_DGHP = new HashSet<SGI_Tarea_Revision_DGHP>();
            this.SGI_LIZA_Procesos = new HashSet<SGI_LIZA_Procesos>();
            this.SGI_LIZA_Ticket = new HashSet<SGI_LIZA_Ticket>();
            this.SGI_Tarea_Generar_Ticket_Liza = new HashSet<SGI_Tarea_Generar_Ticket_Liza>();
            this.SGI_Tarea_Obtener_Ticket_Liza = new HashSet<SGI_Tarea_Obtener_Ticket_Liza>();
            this.SGI_Tarea_Calificar_ObsGrupo = new HashSet<SGI_Tarea_Calificar_ObsGrupo>();
            this.SGI_Tarea_Dictamen_Realizar_Dictamen = new HashSet<SGI_Tarea_Dictamen_Realizar_Dictamen>();
            this.SGI_Tarea_Dictamen_Revision = new HashSet<SGI_Tarea_Dictamen_Revision>();
            this.SGI_Tarea_Enviar_DGFC = new HashSet<SGI_Tarea_Enviar_DGFC>();
            this.SGI_Tarea_Documentos_Adjuntos = new HashSet<SGI_Tarea_Documentos_Adjuntos>();
            this.SGI_Tarea_Calificar = new HashSet<SGI_Tarea_Calificar>();
            this.SGI_Tarea_Entregar_Tramite = new HashSet<SGI_Tarea_Entregar_Tramite>();
            this.SGI_Tarea_Informar_Doc_Sade = new HashSet<SGI_Tarea_Informar_Doc_Sade>();
            this.SGI_Tarea_Visado = new HashSet<SGI_Tarea_Visado>();
            this.SGI_Tarea_Ejecutiva = new HashSet<SGI_Tarea_Ejecutiva>();
            this.SGI_Tarea_Ejecutiva_NumeroGedo = new HashSet<SGI_Tarea_Ejecutiva_NumeroGedo>();
            this.SGI_Tramites_Tareas_Dispo_Considerando = new HashSet<SGI_Tramites_Tareas_Dispo_Considerando>();
            this.SGI_Tarea_Gestion_Documental = new HashSet<SGI_Tarea_Gestion_Documental>();
            this.SGI_Tarea_Enviar_Procuracion = new HashSet<SGI_Tarea_Enviar_Procuracion>();
        }
    
        public int id_tramitetarea { get; set; }
        public int id_tarea { get; set; }
        public int id_resultado { get; set; }
        public System.DateTime FechaInicio_tramitetarea { get; set; }
        public Nullable<System.DateTime> FechaCierre_tramitetarea { get; set; }
        public Nullable<System.Guid> UsuarioAsignado_tramitetarea { get; set; }
        public Nullable<System.DateTime> FechaAsignacion_tramtietarea { get; set; }
        public Nullable<System.Guid> CreateUser { get; set; }
        public Nullable<int> id_proxima_tarea { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual ENG_Resultados ENG_Resultados { get; set; }
        public virtual ENG_Tareas ENG_Tareas { get; set; }
        public virtual ENG_Tareas ENG_Tareas1 { get; set; }
        public virtual ICollection<SGI_SADE_Procesos> SGI_SADE_Procesos { get; set; }
        public virtual ICollection<SGI_Solicitudes_Pagos> SGI_Solicitudes_Pagos { get; set; }
        public virtual ICollection<SGI_Tarea_Aprobado> SGI_Tarea_Aprobado { get; set; }
        public virtual ICollection<SGI_Tarea_Asignar_Calificador> SGI_Tarea_Asignar_Calificador { get; set; }
        public virtual ICollection<SGI_Tarea_Asignar_Inspector> SGI_Tarea_Asignar_Inspector { get; set; }
        public virtual ICollection<SGI_Tarea_Calificacion_Tecnica_Legal> SGI_Tarea_Calificacion_Tecnica_Legal { get; set; }
        public virtual ICollection<SGI_Tarea_Carga_Tramite> SGI_Tarea_Carga_Tramite { get; set; }
        public virtual ICollection<SGI_Tarea_Dictamen_Asignar_Profesional> SGI_Tarea_Dictamen_Asignar_Profesional { get; set; }
        public virtual ICollection<SGI_Tarea_Dictamen_GEDO> SGI_Tarea_Dictamen_GEDO { get; set; }
        public virtual ICollection<SGI_Tarea_Dictamen_Revisar_Tramite> SGI_Tarea_Dictamen_Revisar_Tramite { get; set; }
        public virtual ICollection<SGI_Tarea_Dictamen_Revision_Gerente> SGI_Tarea_Dictamen_Revision_Gerente { get; set; }
        public virtual ICollection<SGI_Tarea_Dictamen_Revision_SubGerente> SGI_Tarea_Dictamen_Revision_SubGerente { get; set; }
        public virtual ICollection<SGI_Tarea_Enviar_PVH> SGI_Tarea_Enviar_PVH { get; set; }
        public virtual ICollection<SGI_Tarea_Fin_Tramite> SGI_Tarea_Fin_Tramite { get; set; }
        public virtual ICollection<SGI_Tarea_Generar_Expediente> SGI_Tarea_Generar_Expediente { get; set; }
        public virtual ICollection<SGI_Tarea_Generar_Expediente_Procesos> SGI_Tarea_Generar_Expediente_Procesos { get; set; }
        public virtual ICollection<SGI_Tarea_Pagos_log> SGI_Tarea_Pagos_log { get; set; }
        public virtual ICollection<SGI_Tarea_Rechazo_En_SADE> SGI_Tarea_Rechazo_En_SADE { get; set; }
        public virtual ICollection<SGI_Tarea_Resultado_Inspector> SGI_Tarea_Resultado_Inspector { get; set; }
        public virtual ICollection<SGI_Tarea_Revision_Dictamenes> SGI_Tarea_Revision_Dictamenes { get; set; }
        public virtual ICollection<SGI_Tarea_Revision_Director> SGI_Tarea_Revision_Director { get; set; }
        public virtual ICollection<SGI_Tarea_Revision_Gerente> SGI_Tarea_Revision_Gerente { get; set; }
        public virtual ICollection<SGI_Tarea_Revision_Pagos> SGI_Tarea_Revision_Pagos { get; set; }
        public virtual ICollection<SGI_Tarea_Revision_SubGerente> SGI_Tarea_Revision_SubGerente { get; set; }
        public virtual ICollection<SGI_Tarea_Revision_Tecnica_Legal> SGI_Tarea_Revision_Tecnica_Legal { get; set; }
        public virtual ICollection<SGI_Tarea_Validar_Zonificacion> SGI_Tarea_Validar_Zonificacion { get; set; }
        public virtual ICollection<SGI_Tarea_Verificacion_AVH> SGI_Tarea_Verificacion_AVH { get; set; }
        public virtual ICollection<SGI_Tramites_Tareas_CPADRON> SGI_Tramites_Tareas_CPADRON { get; set; }
        public virtual ICollection<SGI_Tramites_Tareas_HAB> SGI_Tramites_Tareas_HAB { get; set; }
        public virtual ICollection<SGI_Tramites_Tareas_TRANSF> SGI_Tramites_Tareas_TRANSF { get; set; }
        public virtual ICollection<SGI_Tarea_Enviar_AVH> SGI_Tarea_Enviar_AVH { get; set; }
        public virtual ICollection<SGI_Tarea_Revision_DGHP> SGI_Tarea_Revision_DGHP { get; set; }
        public virtual ICollection<SGI_LIZA_Procesos> SGI_LIZA_Procesos { get; set; }
        public virtual ICollection<SGI_LIZA_Ticket> SGI_LIZA_Ticket { get; set; }
        public virtual ICollection<SGI_Tarea_Generar_Ticket_Liza> SGI_Tarea_Generar_Ticket_Liza { get; set; }
        public virtual ICollection<SGI_Tarea_Obtener_Ticket_Liza> SGI_Tarea_Obtener_Ticket_Liza { get; set; }
        public virtual ICollection<SGI_Tarea_Calificar_ObsGrupo> SGI_Tarea_Calificar_ObsGrupo { get; set; }
        public virtual ICollection<SGI_Tarea_Dictamen_Realizar_Dictamen> SGI_Tarea_Dictamen_Realizar_Dictamen { get; set; }
        public virtual ICollection<SGI_Tarea_Dictamen_Revision> SGI_Tarea_Dictamen_Revision { get; set; }
        public virtual ICollection<SGI_Tarea_Enviar_DGFC> SGI_Tarea_Enviar_DGFC { get; set; }
        public virtual ICollection<SGI_Tarea_Documentos_Adjuntos> SGI_Tarea_Documentos_Adjuntos { get; set; }
        public virtual ICollection<SGI_Tarea_Calificar> SGI_Tarea_Calificar { get; set; }
        public virtual ICollection<SGI_Tarea_Entregar_Tramite> SGI_Tarea_Entregar_Tramite { get; set; }
        public virtual ICollection<SGI_Tarea_Informar_Doc_Sade> SGI_Tarea_Informar_Doc_Sade { get; set; }
        public virtual ICollection<SGI_Tarea_Visado> SGI_Tarea_Visado { get; set; }
        public virtual ICollection<SGI_Tarea_Ejecutiva> SGI_Tarea_Ejecutiva { get; set; }
        public virtual ICollection<SGI_Tarea_Ejecutiva_NumeroGedo> SGI_Tarea_Ejecutiva_NumeroGedo { get; set; }
        public virtual ICollection<SGI_Tramites_Tareas_Dispo_Considerando> SGI_Tramites_Tareas_Dispo_Considerando { get; set; }
        public virtual ICollection<SGI_Tarea_Gestion_Documental> SGI_Tarea_Gestion_Documental { get; set; }
        public virtual ICollection<SGI_Tarea_Enviar_Procuracion> SGI_Tarea_Enviar_Procuracion { get; set; }
    }
}
