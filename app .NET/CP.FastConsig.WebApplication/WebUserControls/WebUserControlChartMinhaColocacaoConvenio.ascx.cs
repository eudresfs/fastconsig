using System;
using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.Common;
using DevExpress.XtraCharts;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlChartMinhaColocacaoConvenio : CustomUserControl
    {

        private string[] aMeses = {"Jan","Fev","Mar","Abr","Mai","Jun","Jul","Ago","Set","Out","Nov","Dez"};

        private int anoi, anof, mesi, mesf, qtdeMeses;

        List<int> empresas = new List<int>(); 

        private enum tipoGrafico { Barra, Linha, Bolha, Ranking };

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (EhPostBack || ControleCarregado) return;

            PopulaGrupos();

            WebUserControlChartBarraColocacaoConvenio.Visible = true;
            WebUserControlChartBarraColocacaoConvenio.Visible = false;
            WebChartControlBolha.Visible = false;
            
            EhPostBack = true;

            ASPxSpinEditMeses.Number = 6;

            ASPxSpinEditTop.Number = 5;

            //DropDownListProdutoGrupo.SelectedValue = "1";

            MostraGrafico(tipoGrafico.Barra);

        }

        private void PopulaGrupos()
        {

            List<ProdutoGrupo> lista = FachadaProdutosEdicao.ListaProdutoGrupos().ToList();

            DropDownListProdutoGrupo.DataSource = lista;
            DropDownListProdutoGrupo.DataBind();

        }

        private List<TmpColocacaoConvenio> Calcula(int Produto, int top, int meses, string tipo)
        {

            DateTime data = new DateTime();
            data = DateTime.Now;

            int mesatual = data.Month;
            int anoatual = data.Year;

            qtdeMeses = meses;

            
            if (mesatual < meses)
            {
                mesi = 12 - (meses - mesatual) + 1;
                mesf = Math.Abs((mesi + meses) - 12) - 1;
                anoi = anoatual - 1;
                anof = anoatual;
            }
            else
            {
                mesi = mesatual - meses + 1;
                mesf = mesatual;
                anoi = anoatual;
                anof = anoatual;
            }

            List<TmpColocacaoConvenio> d = FachadaGerencial.ListarColocacaoConvenio().Where(x => x.IDServicoGrupo == Produto && x.Tipo == tipo && ((x.Ano == anoi && x.Mes >= mesi) || (x.Ano == anof && x.Mes <= mesf))).OrderByDescending(z => z.Total).ToList();

            empresas.Clear();

            foreach (var item in d)
	        {
		        if ((empresas.Count < top) && (empresas.IndexOf((int)item.IDEmpresa)<0))
                {
                    empresas.Add( (int)item.IDEmpresa );
                }
	        }

            return FachadaGerencial.ListarColocacaoConvenio().Where(x => x.IDServicoGrupo == Produto && x.Tipo == tipo && ((x.Ano == anoi && x.Mes >= mesi) || (x.Ano == anof && x.Mes <= mesf))).OrderByDescending(z => z.Total).OrderBy(z => z.Mes).OrderBy(z => z.Ano).OrderBy(z => z.IDEmpresa).ToList();
        
        }

        private void MostraGrafico(tipoGrafico tipo)
        {

            int top = (int)ASPxSpinEditTop.Number;

            List<TmpColocacaoConvenio> tmp = Calcula(Convert.ToInt32(DropDownListProdutoGrupo.SelectedValue), (int)ASPxSpinEditTop.Number, (int)ASPxSpinEditMeses.Number, DropDownListDados.SelectedValue);
        
            
            Dictionary<string, decimal[]> serieValores = new Dictionary<string, decimal[]>();

            List<decimal> dadosTotais = new List<decimal>();

            List<string> categorias = new List<string>();

            int mes = mesi;
            int ano = anoi;            
            
            if (tipo == tipoGrafico.Bolha)
            {
                WebChartControlBolha.Visible = true;
                WebUserControlChartBarraColocacaoConvenio.Visible = false;
                WebUserControlChartLinhaColocacaoConvenio.Visible = false;

                XYDiagram diagram = (XYDiagram)WebChartControlBolha.Diagram;
                diagram.AxisY.Visible = false;

                WebChartControlBolha.Titles.Clear();
                ChartTitle titulo = new ChartTitle();
                titulo.Alignment = System.Drawing.StringAlignment.Center;
                titulo.Text = ResourceMensagens.TituloMinhaPosicaoNoConvenio;

                ChartTitle subtitulo1 = new ChartTitle();
                subtitulo1.Alignment = System.Drawing.StringAlignment.Center;
                subtitulo1.Font = new System.Drawing.Font("Tahoma", 10);
                subtitulo1.Text = "Tamanho da Bolha => Quantidade de Averbações";

                ChartTitle subtitulo2 = new ChartTitle();
                subtitulo2.Alignment = System.Drawing.StringAlignment.Center;
                subtitulo2.Font = new System.Drawing.Font("Tahoma", 10);
                subtitulo2.Text = "Altura da Bolha => Valor das Averbações";

                WebChartControlBolha.Titles.Add(titulo);
                WebChartControlBolha.Titles.Add(subtitulo1);
                WebChartControlBolha.Titles.Add(subtitulo2);

                WebChartControlBolha.Series.Clear();
            }
            else if ((tipo == tipoGrafico.Linha) || (tipo == tipoGrafico.Ranking))
            {
                WebChartControlBolha.Visible = false;
                WebUserControlChartBarraColocacaoConvenio.Visible = false;
                WebUserControlChartLinhaColocacaoConvenio.Visible = true;
            }
            else
            {
                WebChartControlBolha.Visible = false;
                WebUserControlChartBarraColocacaoConvenio.Visible = true;
                WebUserControlChartLinhaColocacaoConvenio.Visible = false;            
            }

            if (tipo != tipoGrafico.Bolha)
            {
                for (int es = 1; es <= qtdeMeses; es++)
                {
                    categorias.Add(String.Format("{0}/{1}", aMeses[mes - 1], ano));
                    dadosTotais.Add((decimal)0.00);
                    mes++;
                    if (mes > 12)
                    {
                        mes = 1;                    
                        ano++;
                    }
                }
            }

            int i = 0;

            int letra = 65; // A

            TmpColocacaoConvenio e;

            while (i < tmp.Count)
            {
                e = tmp[i];

                int idempresa = (int)e.IDEmpresa;

                if (empresas.IndexOf(idempresa) < 0)
                {
                    i++;
                    continue;
                }

                string nomeEmpresa;                

                if (idempresa == (int)Sessao.IdBanco)
                {
                    nomeEmpresa = FachadaConsignatarias.ObtemEmpresa( Sessao.IdBanco ).Nome;
                }
                else
                {
                    nomeEmpresa = "Empresa " + (char)(letra);
                    letra++;
                }                

                Series series = new Series(nomeEmpresa, ViewType.Bubble);
                    
                if (tipo == tipoGrafico.Bolha)
                {
                    series.LegendText = nomeEmpresa;
                    series.Label.Visible = false;
                }

                mes = mesi;
                ano = anoi;

                double[] aValores = new double[2];

                dadosTotais.Clear();
                for (int es = 1; es <= qtdeMeses; es++)
                {
                    if (tipo == tipoGrafico.Bolha)
                    {
                        SeriesPoint p = new SeriesPoint(String.Format("{0}/{1}", aMeses[mes - 1], ano), aValores);
                        series.Points.Add(p);
                    }
                    dadosTotais.Add((decimal)0.00);
                    mes++;
                    if (mes > 12)
                    {
                        mes = 1;
                        ano++;
                    }
                }

                while ((idempresa == (int)e.IDEmpresa) && (i < tmp.Count))
                {

                    if (tipo == tipoGrafico.Bolha)
                    {
                        for (int j = 0; j < series.Points.Count; j++)
                        {
                            if (series.Points[j].Argument == String.Format("{0}/{1}", aMeses[(int)e.Mes - 1], e.Ano))
                            {
                                double[] aValor = new double[2];
                                aValor[0] = (double)e.Qtde;
                                aValor[1] = (double)e.Total;

                                series.Points[j].Values = aValor;
                            }

                        }
                    }
                    else
                    {
                        int index = categorias.IndexOf(String.Format("{0}/{1}", aMeses[(int)e.Mes - 1], e.Ano));
                        if (index != -1)
                            dadosTotais[index] = (decimal)e.Total;
                    }

                    i++;
                    if (i < tmp.Count)
                    {
                        e = tmp[i];
                    }
                }

                    if (tipo == tipoGrafico.Bolha)
                        WebChartControlBolha.Series.Add(series);
                    else
                    {
                        serieValores.Add(nomeEmpresa, dadosTotais.ToArray());
                    }
                
            }
        
         if (tipo == tipoGrafico.Ranking)
         {
            //Ordenando para pegar a ordem de cada mês
            List<decimal> ordem = new List<decimal>();
            List<decimal> semordem = new List<decimal>();
            List<int> indice = new List<int>();

            // Varrendo todos os meses, ordenando mês a mês
            for (int k = 0; k < qtdeMeses; k++)
            {
                ordem.Clear();
                semordem.Clear();
                indice.Clear();

                // Alimentando os arrays de cada mês
                foreach (var item in serieValores)
                {
                    ordem.Add(item.Value[k]);
                    semordem.Add(item.Value[k]);
                }
                
                // Ordenando os valores
                ordem.Sort();

                int rank = ordem.Count;

                int ind = 0;
                // Pegando o indice da empresa
                foreach (var item in serieValores)
                {
                    int a = ordem.IndexOf(semordem[ind]);
                    indice.Add( a );
                    ind++;
                }
                // Informando o rank de cada empresa
                for (int j = 0; j < ordem.Count; j++)
                {
                    ordem[j] = rank;
                    rank--;
                }

                int m = 0;
                // Definindo a ranking de cada mês de todas as empresas
                foreach (var item in serieValores)
                {
                    item.Value[k] = ordem[indice[m]];
                    m++;
                }

            }

         }

            
            if (tipo == tipoGrafico.Barra)
                WebUserControlChartBarraColocacaoConvenio.ConfiguraGrafico(ResourceAuxiliar.NomeArquivoScriptChartBarraColocacaoConvenio, ResourceMensagens.TituloMinhaPosicaoNoConvenio, "Últimos " + qtdeMeses.ToString() + " meses", "Total", categorias.ToArray(), serieValores);
            else if (tipo == tipoGrafico.Linha)
                WebUserControlChartLinhaColocacaoConvenio.ConfiguraGrafico(ResourceAuxiliar.NomeArquivoScriptChartLinhaColocacaoConvenio, ResourceMensagens.TituloMinhaPosicaoNoConvenio, "Últimos " + qtdeMeses.ToString() + " meses", "Total", categorias.ToArray(), serieValores);
            else if (tipo == tipoGrafico.Ranking)
                WebUserControlChartLinhaColocacaoConvenio.ConfiguraGrafico(ResourceAuxiliar.NomeArquivoScriptChartLinhaRanking, ResourceMensagens.TituloMinhaPosicaoNoConvenio, "Últimos " + qtdeMeses.ToString() + " meses", "Ranking", categorias.ToArray(), serieValores);
             
        }

        protected void ButtonBarra_Click(object sender, EventArgs e)
        {
            MostraGrafico(tipoGrafico.Barra);
        }

        protected void ButtonLinha_Click(object sender, EventArgs e)
        {
            MostraGrafico(tipoGrafico.Linha);
        }

        protected void ButtonBolha_Click(object sender, EventArgs e)
        {
            MostraGrafico(tipoGrafico.Bolha);
        }

        protected void ButtonRanking_Click(object sender, EventArgs e)
        {
            MostraGrafico(tipoGrafico.Ranking);
        }


    }

    
}