using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGI.Model
{
    public class Ubicacion
    {
        public int id_ubicacion { get; set; }
        public Nullable<int> id_subtipoubicacion { get; set; }
        public Nullable<int> NroPartidaMatriz { get; set; }
        public Nullable<int> Seccion { get; set; }
        public string Manzana { get; set; }
        public string Parcela { get; set; }
        public Nullable<DateTime> VigenciaDesde { get; set; }
        public Nullable<DateTime> VigenciaHasta { get; set; }
        public string Observaciones { get; set; }

        public class Puerta
        {
            public int id_ubic_puerta { get; set; }
            public int id_ubicacion { get; set; }
            public string tipo_puerta { get; set; }
            public int codigo_calle { get; set; }
            public string Nombre_calle { get; set; }
            public int NroPuerta_ubic { get; set; }

        }


        public string Bus_NombreCalle(int codigo_calle, int NroPuerta)
        {
            string nombre_calle = "";

            DGHP_Entities db = new DGHP_Entities();

            List<Calles> lstCalles = db.Calles.Where(x => x.Codigo_calle == codigo_calle).ToList();

            int AlturaInicio = 0;
            int AlturaFin = 0;

            foreach (Calles item in lstCalles)
            {
                AlturaInicio = (int)item.AlturaDerechaInicio_calle;

                if (item.AlturaIzquierdaInicio_calle < item.AlturaDerechaInicio_calle)
                    AlturaInicio = (int)item.AlturaIzquierdaInicio_calle;

                AlturaFin = (int)item.AlturaDerechaFin_calle;

                if (item.AlturaIzquierdaFin_calle < item.AlturaDerechaFin_calle)
                    AlturaFin = (int)item.AlturaIzquierdaFin_calle;

                if (NroPuerta >= AlturaInicio && NroPuerta <= AlturaFin)
                    nombre_calle = item.NombreOficial_calle;
            }

            db.Dispose();
            return nombre_calle;
        }


        public List<Puerta> GetPuertas()
        {

            DGHP_Entities db = new DGHP_Entities();

            List<Puerta> lstPuertas = (from sel in db.Ubicaciones_Puertas
                                       where sel.id_ubicacion == this.id_ubicacion
                                       select new Puerta
                                       {
                                           id_ubicacion = sel.id_ubicacion,
                                           id_ubic_puerta = sel.id_ubic_puerta,
                                           tipo_puerta = sel.tipo_puerta,
                                           codigo_calle = sel.codigo_calle,
                                           Nombre_calle = "",
                                           NroPuerta_ubic = sel.NroPuerta_ubic
                                       }).Distinct().ToList(); ;

            foreach (Puerta item in lstPuertas)
            {
                item.Nombre_calle = Bus_NombreCalle(item.codigo_calle, item.NroPuerta_ubic);
            }

            return lstPuertas;

        }
        public static Puerta GetPuerta(int id_ubic_puerta)
        {
            DGHP_Entities db = new DGHP_Entities();
            Puerta ret = (from sel in db.Ubicaciones_Puertas
                          join calle in db.Calles on sel.codigo_calle equals calle.Codigo_calle
                          where sel.id_ubic_puerta == id_ubic_puerta
                          select new Puerta
                          {
                              id_ubicacion = sel.id_ubicacion,
                              id_ubic_puerta = sel.id_ubic_puerta,
                              tipo_puerta = sel.tipo_puerta,
                              codigo_calle = sel.codigo_calle,
                              Nombre_calle = calle.NombreOficial_calle,
                              NroPuerta_ubic = sel.NroPuerta_ubic
                          }).Distinct().First();

            return ret;

        }
        public string GetUrlFoto(int ancho, int alto)
        {

            string ret = "";
            string SMP = "";
            int tamaManzana = 3;
            int tamaParcela = 3;

            string seccion = this.Seccion.ToString().Trim();
            string manzana = this.Manzana.Trim();
            string parcela = this.Parcela.Trim();


            SMP += seccion.PadLeft(2, Convert.ToChar("0"));
            SMP += "-";

            if (manzana.Length > 0)
            {
                if (!Char.IsNumber(manzana, manzana.Length - 1))
                    tamaManzana = 6;
            }

            SMP += manzana.PadLeft(tamaManzana, Convert.ToChar("0"));
            SMP += "-";

            if (parcela.Length > 0)
            {
                if (!Char.IsNumber(parcela, parcela.Length - 1))
                    tamaParcela = 4;
            }

            SMP += parcela.PadLeft(tamaParcela, Convert.ToChar("0"));

            ret = string.Format("http://fotos.usig.buenosaires.gob.ar/getFoto?smp={0}&i=0&h={1}&w={2}", SMP, alto, ancho);

            return ret;

        }

        public string GetUrlMapa(int ancho, int alto)
        {


            string ret = "";
            string SMP = "";
            int tamaManzana = 3;
            int tamaParcela = 3;

            string seccion = this.Seccion.ToString().Trim();
            string manzana = this.Manzana.Trim();
            string parcela = this.Parcela.Trim();
            string Direccion = "";

            SMP += seccion.PadLeft(2, Convert.ToChar("0"));
            SMP += "-";

            if (manzana.Length > 0)
            {
                if (!Char.IsNumber(manzana, manzana.Length - 1))
                    tamaManzana = 6;
            }

            SMP += manzana.PadLeft(tamaManzana, Convert.ToChar("0"));
            SMP += "-";

            if (parcela.Length > 0)
            {
                if (!Char.IsNumber(parcela, parcela.Length - 1))
                    tamaParcela = 4;
            }

            SMP += parcela.PadLeft(tamaParcela, Convert.ToChar("0"));

            ret = string.Format("http://servicios.usig.buenosaires.gob.ar/LocDir/mapa.phtml?dir={0}&desc={0}&w={2}&h={3}&punto=5&r=200&smp={1}",
                        HttpUtility.UrlDecode(Direccion), SMP, alto, ancho);
            return ret;

        }
        public string GetUrlCroquis(int ancho, int alto)
        {

            string ret = "";
            string SMP = "";
            int tamaManzana = 3;
            int tamaParcela = 3;
            string seccion = this.Seccion.ToString().Trim();
            string manzana = this.Manzana.Trim();
            string parcela = this.Parcela.Trim();
            string Direccion = "";

            SMP += seccion.PadLeft(2, Convert.ToChar("0"));
            SMP += "-";

            if (manzana.Length > 0)
            {
                if (!Char.IsNumber(manzana, manzana.Length - 1))
                    tamaManzana = 6;
            }

            SMP += manzana.PadLeft(tamaManzana, Convert.ToChar("0"));
            SMP += "-";

            if (parcela.Length > 0)
            {
                if (!Char.IsNumber(parcela, parcela.Length - 1))
                    tamaParcela = 4;
            }

            SMP += parcela.PadLeft(tamaParcela, Convert.ToChar("0"));

            ret = string.Format("http://servicios.usig.buenosaires.gob.ar/LocDir/mapa.phtml?dir={0}&w={2}&h={3}&punto=5&r=50&smp={1}",
                     HttpUtility.UrlDecode(Direccion), SMP, alto, ancho);
            return ret;

        }

    }
}