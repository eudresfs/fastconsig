using System.Collections.Generic;
using CP.FastConsig.Common;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlAlerta : CustomUserControl
    {

        #region Constantes
        
        private const string TagMensagem = "#mensagem";
        private const string TagTipo = "#tipo";

        #endregion

        public void ExibeAlerta(string mensagem, string tipo = "message")
        {

            Dictionary<string, string> tagsValores = new Dictionary<string, string>();

            tagsValores.Add(TagMensagem, mensagem);
            tagsValores.Add(TagTipo, tipo);

            LimpaScripts();
            AdicionaArquivoScriptParaExecucao(ResourceAuxiliar.NomeArquivoScriptAlerta, tagsValores);
            
        }

    }

}