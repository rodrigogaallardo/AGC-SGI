

namespace SGI.Model
{
    using System;
    
    public partial class Ubicacion_TraerLocalidades_Result
    {
        public int Id { get; set; }
        public Nullable<int> IdProvincia { get; set; }
        public Nullable<int> IdDepto { get; set; }
        public string CodDepto { get; set; }
        public string Depto { get; set; }
        public string Cabecera { get; set; }
        public Nullable<double> Area { get; set; }
        public Nullable<double> Perimetro { get; set; }
        public Nullable<double> Clave { get; set; }
        public Nullable<bool> Excluir { get; set; }
        public int Id1 { get; set; }
        public string Nombre { get; set; }
        public Nullable<int> IdProvincia1 { get; set; }
        public string Provincia { get; set; }
    }
}
