//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TipoTramiteCertificados
    {
        public TipoTramiteCertificados()
        {
            this.Rel_TipoTramite_Roles = new HashSet<Rel_TipoTramite_Roles>();
            this.Certificados = new HashSet<Certificados>();
        }
    
        public int TipoTramite { get; set; }
        public string Descripcion { get; set; }
        public int id_agrupamiento { get; set; }
    
        public virtual ICollection<Rel_TipoTramite_Roles> Rel_TipoTramite_Roles { get; set; }
        public virtual ICollection<Certificados> Certificados { get; set; }
        public virtual NivelesDeAgrupamiento NivelesDeAgrupamiento { get; set; }
    }
}
