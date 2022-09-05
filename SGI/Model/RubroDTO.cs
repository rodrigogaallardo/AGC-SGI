using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGI.Model
{
    /// <summary>
    /// Clase creada para exportar el listado de rubros
    /// </summary>
    public class RubroDTO
    {      
        public string CodigoRubro { get; set; }
        public string Descripcion { get; set; }
        public string PalabrasClaves { get; set; }
        public string InfoDescriptiva { get; set; }
        public string TipoActividad { get; set; }
        public string TipoTramite { get; set; }
        public string RegistroAlcohol { get; set; }
        public string GrupoCircuito { get; set; }
        public bool Historico { get; set; }
        public bool CircuitoHabAutomatico { get; set; }
        public bool Uso_Condicionado { get; set; }
        public bool ValidaCargaDescarga { get; set; }
        public bool OficinaComercial { get; set; }
        public bool TieneDeposito { get; set; }
        public decimal? SupMinCargaDescarga { get; set; }
        public decimal? SupMinCargaDescargaRefII { get; set; }
        public decimal? PorSupMinCargaDescargaRefV { get; set; }
        public bool Librar_Uso { get; set; }

    }
}