using System.Linq;
using System.Collections.Generic;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlChartPizza : CustomUserControl
    {

        #region Contantes

        private const string TagTitulo = "#titulo";
        private const string TagSubTitulo = "#subTitulo";
        private const string TagDados = "#dados";
        private const string FormatoDados = "['{0}', {1}]";
        private const string SeparadorDados = ",";

        #endregion

        public void ConfiguraGrafico(string nomeArquivoScriptChartPizza, string titulo, string subTitulo, Dictionary<string, decimal> dados)
        {

            Dictionary<string, string> tagsValores = new Dictionary<string, string>();
            
            tagsValores.Add(TagTitulo, titulo);
            tagsValores.Add(TagSubTitulo, subTitulo);
            tagsValores.Add(TagDados, string.Join(SeparadorDados, dados.Select(x => string.Format(FormatoDados, x.Key, x.Value.ToString("0.00"))).ToArray()));

            LimpaScripts();
            AdicionaArquivoScriptParaExecucao(nomeArquivoScriptChartPizza, tagsValores);

        }
        
    }

}