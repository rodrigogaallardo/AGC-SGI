
namespace SGI.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EE_Entities : DbContext
    {
        public EE_Entities()
            : base("name=EE_Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Parametros> Parametros { get; set; }
        public DbSet<wsEE_Caratulas> wsEE_Caratulas { get; set; }
        public DbSet<wsEE_TareasDocumentos> wsEE_TareasDocumentos { get; set; }
        public DbSet<wsEE_Documentos> wsEE_Documentos { get; set; }
        public DbSet<wsEE_DocumentosRelacionados> wsEE_DocumentosRelacionados { get; set; }
        public DbSet<wsEE_Paquetes> wsEE_Paquetes { get; set; }
    }
}
