using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGI.Model
{
    public class SmallDay
    {
        // genera la fecha sin minutos, segundos, etc
        private DateTime _fecha;
        public DateTime fecha
        {
            get
            {
                return _fecha;
            }
            set
            {
                _fecha = new DateTime(value.Year, value.Month, value.Day);
            }
        }

        public SmallDay()
        {

        }

        public SmallDay(DateTime fecha)
        {
            this.fecha = new DateTime(fecha.Year, fecha.Month, fecha.Day);
        }

        public override bool Equals(object obj)
        {
            try
            {
                if (!(obj is DateTime))
                    return false;

                if (obj != null)
                {
                    DateTime fecha = (DateTime)obj;
                    if (fecha.Year == this.fecha.Year &&
                        fecha.Month == this.fecha.Month &&
                        fecha.Day == this.fecha.Day)

                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static DateTime PrimerDiaMes(DateTime fecha)
        {
            SmallDay dia;

            dia = new SmallDay(fecha.AddDays(-fecha.Day + 1));

            return dia.fecha;
        }
        public static DateTime UltimoDiaMes(DateTime fecha)
        {
            SmallDay dia;

            dia = new SmallDay(PrimerDiaMes(fecha.AddMonths(1)).AddDays(-1));

            return dia.fecha;
        }

        public static int CalcularMesesDeDiferencia(DateTime fechaDesde, DateTime fechaHasta)
        {
            return Math.Abs((fechaDesde.Month - fechaHasta.Month) + 12 * (fechaDesde.Year - fechaHasta.Year));
        }
        public static string NombreMes(int nro_mes)
        {
            string mes_destino = "";
            switch (nro_mes)
            {
                case 1:
                    mes_destino = "Enero";
                    break;
                case 2:
                    mes_destino = "Febrero";
                    break;
                case 3:
                    mes_destino = "Marzo";
                    break;
                case 4:
                    mes_destino = "Abril";
                    break;
                case 5:
                    mes_destino = "Mayo";
                    break;
                case 6:
                    mes_destino = "Junio";
                    break;
                case 7:
                    mes_destino = "Julio";
                    break;
                case 8:
                    mes_destino = "Agosto";
                    break;
                case 9:
                    mes_destino = "Septiembre";
                    break;
                case 10:
                    mes_destino = "Octubre";
                    break;
                case 11:
                    mes_destino = "Noviembre";
                    break;
                case 12:
                    mes_destino = "Diciembre";
                    break;
                default:
                    break;
            }
            return mes_destino;
        }
    }
}