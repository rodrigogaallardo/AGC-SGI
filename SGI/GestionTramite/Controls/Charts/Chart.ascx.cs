using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Syncfusion.JavaScript.DataVisualization.Models;
using System.IO;
using Syncfusion.EJ.Export;
using Syncfusion.XlsIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;

namespace SGI.GestionTramite.Controls.Charts
{
    public class ResultadosPOA
    {
        public int Ingresados { get; set; }
        public int Resueltos { get; set; }
    }

    public partial class Chart : System.Web.UI.UserControl
    {
        public void CargarGraficoColumnas(string[] Columnas, ResultadosPOA Valores)
        {
            //Add Chart data
            List<ChartDatos> ChartList = new List<ChartDatos>();
                        
            ChartDatos ElevPendientes = new ChartDatos("Ingresados",Convert.ToDecimal(Valores.Ingresados), Valores.Ingresados.ToString());            
            ChartList.Add(ElevPendientes);
            ElevPendientes = new ChartDatos("Resueltos", Convert.ToDecimal(Valores.Resueltos), Valores.Resueltos.ToString());
            ChartList.Add(ElevPendientes);

            decimal Porcentaje = Valores.Resueltos * 100 / Valores.Ingresados;

            Chart_estadoECA.Text = string.Format("Porcentaje Resuelto {0} %", Porcentaje);

            //Binding DataSource to Chart
            this.Chart_estadoECA.DataSource = ChartList;
            this.Chart_estadoECA.DataBind();
        }

    }

    [Serializable]
    public class ChartDatos
    {
        public ChartDatos(string xval, decimal yvalue1, string name)
        {
            this.ValorTexto = xval;
            this.ValorPorcentaje = yvalue1;
            this.ValorDetalle = name;
        }

        public string ValorTexto { get; set; }
        public decimal ValorPorcentaje { get; set; }
        public string ValorDetalle { get; set; }

    }
}