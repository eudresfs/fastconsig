using System;
using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.WebApplication.Auxiliar;
using DevExpress.XtraCharts;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlChartPizza3D : CustomUserControl
    {

        private const string FormatoDataHoraGrafico = "dd/MM/yyyy hh:mm:ss";

        public void AdicionaSerie(List<GrupoMargem> gruposMargem)
        {

            decimal valorTotal = gruposMargem.Sum(x => x.MargemUtilizada ?? 0);

            ConfiguraTituloSuperior(ResourceMensagens.TituloUtilizacaoMargemProduto);
            ConfiguraTituloInferior(DateTime.Now.ToString(FormatoDataHoraGrafico));

            Series series = WebChartControlGraficoPorProduto.Series[0];
            Pie3DSeriesView seriesView = new Pie3DSeriesView();

            seriesView.ExplodedDistancePercentage = 15;
            seriesView.ExplodeMode = PieExplodeMode.All;

            series.View = seriesView;
            series.Label.Visible = true;

            foreach (GrupoMargem grupoMargem in gruposMargem) series.Points.Add(new SeriesPoint(grupoMargem.Nome, grupoMargem.MargemUtilizada / valorTotal));

        }

        public void ConfiguraTituloSuperior(string titulo)
        {
            WebChartControlGraficoPorProduto.Titles[(int)PosicaoTitulo.TituloSuperior].Text = titulo;
        }

        public void ConfiguraTituloInferior(string titulo)
        {
            WebChartControlGraficoPorProduto.Titles[(int)PosicaoTitulo.TituloInferior].Text = titulo;
        }

        private enum PosicaoTitulo
        {
            TituloSuperior,
            TituloInferior,
        }
        
    }

}