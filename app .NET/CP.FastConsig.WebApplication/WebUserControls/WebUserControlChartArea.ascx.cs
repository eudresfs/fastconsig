using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlChartArea : CustomUserControl
    {

        #region Contantes

        private const string TagTitulo = "#titulo";
        private const string TagSubTitulo = "#subTitulo";
        private const string TagDados = "#dados";
        private const string FormatoDados = "{{name:'{0}', data:[{1}]}}";
        private const string SeparadorDados = ",";
        private const string TagTituloEixoY = "#eixoY";
        private const string TagCategorias = "#categorias";

        #endregion

        public void ConfiguraGrafico(string nomeArquivoScriptChartBarra, string titulo, string subTitulo, string tituloEixoY, string[] categorias, Dictionary<string, decimal[]> dados)
        {

            Dictionary<string, string> tagsValores = new Dictionary<string, string>();

            tagsValores.Add(TagTitulo, titulo);
            tagsValores.Add(TagSubTitulo, subTitulo);
            tagsValores.Add(TagTituloEixoY, tituloEixoY);
            tagsValores.Add(TagCategorias, string.Join(SeparadorDados, categorias.Select(x => string.Format("'{0}'", x)).ToArray()));
            tagsValores.Add(TagDados, string.Join(SeparadorDados, string.Join(SeparadorDados, dados.Select(x => string.Format(FormatoDados, x.Key, string.Join(SeparadorDados, x.Value.Select(y => y.ToString().Replace(",", "."))))).ToArray())));

            LimpaScripts();
            AdicionaArquivoScriptParaExecucao(nomeArquivoScriptChartBarra, tagsValores);

        }
        
    }

}