

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class aspnet_token_usuario
    {
        public int Id { get; set; }
        public System.Guid UsuarioId { get; set; }
        public string Token { get; set; }
        public System.DateTime FechaCreacion { get; set; }
    }
}
