using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalService.Class
{
    public class GetBUIsCAAResponse
    {
        public int idBoletaUnica { get; set; }
        public int idPago { get; set; }
        public string codBarras { get; set; }
        public int nroBoletaUnica { get; set; }
        public int dependencia { get; set; }
        public string montoTotal { get; set; }
        public int estadoId { get; set; }
        public string estadoNombre { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? fechaPago { get; set; }
        public DateTime? fechaAnulada { get; set; }
        public DateTime? fechaCancelada { get; set; }
        public string trazaPago { get; set; }
        public string codigoVerificador { get; set; }
        public string nroBUI { get; set; }
        public string buI_ID { get; set; }
        public string updateUser { get; set; }
        public string medioDePagoRectificado { get; set; }
        public string numeroComprobante { get; set; }
        public string numeroVoucher { get; set; }
        public string cantidadCuotas { get; set; }
        public string codigoAutorizacion { get; set; }
        public string lugarDePago { get; set; }
        public DateTime? fechaPago2 { get; set; }
        public string ultimoEstado { get; set; }
        public string ultimoEstadoActualizadoFecha { get; set; }
        public Contribuyente contribuyente { get; set; }
        public Estado[] estados { get; set; }
    }

    public class Contribuyente
    {
        public int tipoPersona { get; set; }
        public string apellidoyNombre { get; set; }
        public int tipoDoc { get; set; }
        public string documento { get; set; }
        public string direccion { get; set; }
        public string piso { get; set; }
        public string departamento { get; set; }
        public string localidad { get; set; }
        public string codPost { get; set; }
        public string email { get; set; }
    }

    public class Estado
    {
        public string estado { get; set; }
        public DateTime actualizadoFecha { get; set; }
    }
}