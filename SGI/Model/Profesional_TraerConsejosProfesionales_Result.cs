

namespace SGI.Model
{
    using System;
    
    public partial class Profesional_TraerConsejosProfesionales_Result
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Profesion { get; set; }
        public string Calle { get; set; }
        public string NroPuerta { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Contacto { get; set; }
        public Nullable<int> id_consejo_habilitaciones { get; set; }
        public int id_grupoconsejo { get; set; }
    }
}
