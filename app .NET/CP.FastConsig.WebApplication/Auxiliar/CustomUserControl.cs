using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;

namespace CP.FastConsig.WebApplication.Auxiliar
{

    public abstract class CustomUserControl : UserControl
    {

        #region Constantes

        private const string ParametroEhPostBack = "EhPostBack";
        private const string ParametroIndiceControleAnterior = "IndiceControleAnterior";
        private const string ParametroId = "Id";
        private const string ParametroIdRecurso = "IdRecurso";
        private const string ParametroIdPermissao = "IdPermissao";
        private const string ParametroScriptsParaExecutar = "ScriptsParaExecutar";
        private const string LabelJQueryTotal = "jQueryTotal";
        private const string ScriptAplicaJquery = "AplicaJquery();";
        private const string LabelScriptGeral = "ScriptGeral";
        private const string PathScripts = @"{0}\Scripts\{1}";
        private const string ArquivosTha = "{0}.tha";
        private const string TagsJavaScript = "<script language='javascript' type='text/javascript'>{0}</script>";

        #endregion

        protected List<object> ParametrosConfiguracao { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                Page.MaintainScrollPositionOnPostBack = false;
            }
            catch
            {
                //TODO: Magnum, verificar posteriormente.
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Page.MaintainScrollPositionOnPostBack = false;

            DadosControleCarregado = true;

            ScriptManager.RegisterStartupScript(this, GetType(), LabelJQueryTotal, ScriptAplicaJquery, true);

            int i = 0;

            foreach (string script in ScriptsParaExecutar) ScriptManager.RegisterStartupScript(this, GetType(), LabelScriptGeral + i++, script, false);

        }

        protected void LimpaScripts()
        {
            ScriptsParaExecutar.Clear();
        }

        /// <summary>
        /// Executa o script contido em um arquivo .THA.
        /// </summary>
        /// <param name="arquivoTha">Nome do arquivo .THA sem a extensão.</param>
        protected void AdicionaArquivoScriptParaExecucao(string arquivoTha)
        {
            AdicionaScriptParaExecucao(arquivoTha, true);
        }

        /// <summary>
        /// Exectu o script contido no parâmetro passado.
        /// </summary>
        /// <param name="script">Script para ser executado sem as tags javascript.</param>
        protected void AdicionaScriptParaExecucao(string script)
        {
            AdicionaScriptParaExecucao(script, false);
        }

        /// <summary>
        /// Executa o script contido em um arquivo .THA.
        /// </summary>
        /// <param name="arquivoTha">Nome do arquivo .THA sem a extensão.</param>
        /// <param name="tagsValores">Parâmetros para configuração do arquivo.</param>
        protected void AdicionaArquivoScriptParaExecucao(string arquivoTha, Dictionary<string, string> tagsValores)
        {
            AdicionaScriptParaExecucao(arquivoTha, true, tagsValores);
        }

        private void AdicionaScriptParaExecucao(string script, bool ehArquivo = false, Dictionary<string, string> tagsValores = null)
        {

            if(tagsValores == null) tagsValores = new Dictionary<string, string>();

            if (!ehArquivo)
            {
                ScriptsParaExecutar.Add(string.Format(TagsJavaScript, script));
                return;
            }

            TextReader textReader = new StreamReader(string.Format(PathScripts, Request.PhysicalApplicationPath, string.Format(ArquivosTha, script)));

            string scriptAux = textReader.ReadToEnd();

            foreach (KeyValuePair<string, string> tagValor in tagsValores) scriptAux = scriptAux.Replace(tagValor.Key, tagValor.Value);

            ScriptsParaExecutar.Add(scriptAux);

        }

        protected bool ControleCarregado
        {
            get
            {
                try
                {

                    if (Sessao.Voltou) return false;
                    if (!FachadaMaster.AtivaAbasSistema()) return false;

                    return (this == PageMaster.ControleAtual) && DadosControleCarregado;

                }
                catch
                {
                    // TODO - MAGNUM - VERIFICAR PORQUE ESTÁ ENTRANDO AQUI DE VEZ EM QUANDO. REMOVER ISSO E TRATAR O POSTBACK DE TODAS AS PÁGINAS.
                    return false;
                }
            }
        }

        public bool DadosControleCarregado
        {
            get
            {
                if (Session["DadosControleCarregado" + ID] == null) Session["DadosControleCarregado" + ID] = 0;
                return Convert.ToBoolean(Session["DadosControleCarregado" + ID]);
            }
            set
            {
                Session["DadosControleCarregado" + ID] = value;
            }
        }

        protected int? Id
        {
            get
            {
                if (Session[ParametroId + ID] == null) Session[ParametroId + ID] = 0;
                return (int)Session[ParametroId + ID];
            }
            set
            {
                Session[ParametroId + ID] = value;
            }
        }

        public int IdRecurso
        {
            get
            {
                if (Session[ParametroIdRecurso + ID] == null) Session[ParametroIdRecurso + ID] = 0;
                return (int)Session[ParametroIdRecurso + ID];
            }
            set
            {
                Session[ParametroIdRecurso + ID] = value;
            }
        }

        public int IdPermissao
        {
            get
            {
                if (Session[ParametroIdPermissao + ID] == null) Session[ParametroIdPermissao + ID] = 0;
                return (int)Session[ParametroIdPermissao + ID];
            }
            set
            {
                Session[ParametroIdPermissao + ID] = value;
            }
        }

        private List<string> ScriptsParaExecutar
        {
            get
            {
                if (Session[ParametroScriptsParaExecutar + ID] == null) Session[ParametroScriptsParaExecutar + ID] = new List<string>();
                return (List<string>) Session[ParametroScriptsParaExecutar + ID];
            }
            set
            {
                Session[ParametroScriptsParaExecutar + ID] = value;
            }
        }

        public int IndiceControleAnterior
        {
            get
            {
                if (Session[ParametroIndiceControleAnterior + ID] == null) Session[ParametroIndiceControleAnterior + ID] = 0;
                return (int)Session[ParametroIndiceControleAnterior + ID];
            }
            set
            {
                Session[ParametroIndiceControleAnterior + ID] = value;
            }
        }

        protected CustomUserControl ControleAnterior
        {
            get { return PageMaster.ControleAnterior; }
        }

        protected GerenciadorSessao Sessao
        {
            get
            {
                return new GerenciadorSessao(Page);
            }
        }

        protected Auxiliar Util
        {
            get
            {
                return new Auxiliar(Page);
            }
        }

        protected NewMasterPrincipal PageMaster
        {
            get { return (NewMasterPrincipal)Page.Master; }
        }

        public void AtualizaPagina()
        {
            PageMaster.AtualizaPagina();
        }

        protected UpdatePanel upPrincipal
        {
            get { return (UpdatePanel)Page.Master.FindControl("upPrincipal"); }
        }

        public void ConfiguraParametros(int indiceControleAnterior, params object[] parametros)
        {

            while (parametros.Length > 0 && parametros[0] is object[]) parametros = (object[]) parametros[0];

            Id = (parametros.Length > 0 ? Convert.ToInt32(parametros[0]) : (int?) null);

            ParametrosConfiguracao = parametros.ToList();
            IndiceControleAnterior = indiceControleAnterior;

        }

        public string Titulo
        {
            get
            {
                return PageMaster.Titulo;
            }
            set
            {
                PageMaster.Titulo = value;
            }
        }

        public string SubTitulo
        {
            get
            {
                return PageMaster.SubTitulo;
            }
            set
            {
                PageMaster.SubTitulo = value;
            }
        }

        public virtual void ExecutaAcoes()
        {
            
        }

        public virtual void ConfiguraTopo()
        {

            Recurso recurso = FachadaSubMenu.ObtemRecurso(IdRecurso);

            if (recurso == null) return;

            PageMaster.Titulo = recurso.Nome;
            PageMaster.Icone = recurso.Imagem;              

        }

        protected bool EhPostBack
        {
            get
            {
                if (Session[ParametroEhPostBack + ID] == null) Session[ParametroEhPostBack + ID] = false;
                return (bool)Session[ParametroEhPostBack + ID];
            }
            set
            {
                Session[ParametroEhPostBack + ID] = value;
            }
        }

        protected void RegistrarStartupScript(UserControl control, string script)
        {

            ScriptManager Smgr = ScriptManager.GetCurrent(control.Page);
            if (Smgr == null) throw new Exception("ScriptManager não encontrado!");

            bool registered = false; if (Smgr.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(control, control.GetType(), String.Format("ScriptStatic_{0}", control.ClientID), script, true);

                registered = true;
            }

            if (!registered)
            {
                if (!control.Page.ClientScript.IsStartupScriptRegistered(control.GetType(), String.Format("ScriptStatic_{0}", control.ClientID)))
                    control.Page.ClientScript.RegisterStartupScript(control.GetType(), String.Format("ScriptStatic_{0}", control.ClientID), script, true);
            }

        }

        protected void RegistrarBlockScript(UserControl control, string script, bool btagscript)
        {

            ScriptManager Smgr = ScriptManager.GetCurrent(control.Page);
            if (Smgr == null) throw new Exception("ScriptManager não encontrado!");

            bool registered = false; 
            if (Smgr.IsInAsyncPostBack)
            {

                ScriptManager.RegisterClientScriptBlock(control, control.GetType(), String.Format("ScriptStatic_{0}", control.ClientID), script, btagscript);

                registered = true;
            }

            if (!registered)
            {

                control.Page.ClientScript.RegisterClientScriptBlock(control.GetType(), String.Format("ScriptStatic_{0}", control.ClientID), script, btagscript);
            }

        }

    }

}