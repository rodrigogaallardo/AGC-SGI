

namespace SGI.Model
{
    using System;
    
    public partial class InspeccionDatosAdicionales_Result
    {
        public Nullable<int> id_ifci { get; set; }
        public int id_datos_adicionales { get; set; }
        public string plano_proyecto_registrado { get; set; }
        public string expediente_plano_proyecto_registrado { get; set; }
        public string plano_conforme_obra { get; set; }
        public string expediente_plano_conforme_obra { get; set; }
        public string certificado_final { get; set; }
        public string expediente_certificado_final { get; set; }
        public string medios_salida_protegidos { get; set; }
        public string accesibilidad_rampas { get; set; }
        public string cumple_cuadro_de_usos { get; set; }
        public string pisos_protegidos_por_instalacion { get; set; }
        public Nullable<decimal> superficie { get; set; }
        public string observaciones { get; set; }
        public string Tecnico { get; set; }
        public string Empresa { get; set; }
        public int id_dgubicacion { get; set; }
        public Nullable<int> añovigencia { get; set; }
        public Nullable<int> NroInstalacion { get; set; }
        public Nullable<int> Patente { get; set; }
        public string TipoDeInstalacion { get; set; }
        public Nullable<int> CantidadPisos { get; set; }
        public Nullable<int> CantidadSubsuelos { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public string senializacion_medios_salida { get; set; }
        public string iluminiacion_emergencia { get; set; }
        public string direccion { get; set; }
        public string Matricula { get; set; }
    }
}
