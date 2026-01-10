using System.Collections.Generic;
using CP.FastConsig.WebApplication.Auxiliar;
using DevExpress.XtraCharts;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlChartBarra3D : CustomUserControl
    {

        public void ConfiguraTituloSuperior(string titulo)
        {
            WebChartControlGrafico.Titles[(int) PosicaoTitulo.TituloSuperior].Text = titulo;
        }

        public void ConfiguraTituloInferior(string titulo)
        {
            WebChartControlGrafico.Titles[(int) PosicaoTitulo.TituloInferior].Text = titulo;
        }

        public void AdicionaSerie(string nomeSerie, Dictionary<string, decimal?> valores)
        {

            Series series = new Series(nomeSerie, ViewType.Bar3D);
            SideBySideBar3DSeriesView seriesView = new SideBySideBar3DSeriesView();

            seriesView.Model = Bar3DModel.Cylinder;

            series.View = seriesView;
            series.Label.Visible = true;

            foreach (KeyValuePair<string, decimal?> textoValor in valores) series.Points.Add(new SeriesPoint(textoValor.Key, textoValor.Value));
            
            WebChartControlGrafico.Series.Add(series);

        }

        private enum PosicaoTitulo
        {
            TituloSuperior,
            TituloInferior,
        }

    }

}