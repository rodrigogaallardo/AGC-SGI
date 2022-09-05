

namespace SGI.Model
{
    using System;
    
    public partial class AVH_Cons_PartidasPorCalle_Result
    {
        public int id_partidamatriz { get; set; }
        public Nullable<int> id_subtipoubicacion { get; set; }
        public Nullable<int> pdamatriz { get; set; }
        public Nullable<int> seccion { get; set; }
        public string manzana { get; set; }
        public string parcela { get; set; }
        public string Calle { get; set; }
        public string PartidasHorizontales { get; set; }
    }
}
