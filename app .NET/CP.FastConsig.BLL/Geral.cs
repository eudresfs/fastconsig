using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.DAL;
using CP.FastConsig.Common;
using System.Web;
using CP.FastConsig.Util;

namespace CP.FastConsig.BLL
{
    public static class Geral
    {

        public static string IdEmpresaConsignante()
        {
            return new Repositorio<Empresa>().Listar().Where(x => x.IDEmpresaTipo == (int)Enums.EmpresaTipo.Consignante).First().IDEmpresa.ToString();
        }

        public static int IdEmpresaConsignanteCenter()
        {
            return Convert.ToInt32(new Repositorio<Parametro>().Listar().Where(x => x.Nome.Equals("IdConsignanteCenter")).Select(x => x.Valor).First());
        }

        public static void ProcessaInicializacao()
        {
            string dataatual = DateTime.Today.ToString("dd/MM/yyyy");
            if (ObtemParametro("DataProcessamento").Valor != dataatual)
            {
                Empresas.AplicaSuspensoes();
                Averbacoes.AplicaLiquidacoes();

                atualizaParametro("DataProcessamento", dataatual);
            }
        } 

        public static List<Estado> ObtemEstados()
        {
            return new Repositorio<Estado>().Listar().OrderBy(x => x.Nome).ToList();
        } 

        public static IQueryable<Recurso> ListaMenuOpcoes(int idpai, int idmodulo)
        {
            if (idpai == 0)
                return new Repositorio<Recurso>().Listar().Where(y => y.IDModulo == idmodulo && y.Visivel.HasValue && y.Visivel.Value && !y.Arquivo.Equals(null)).Where(x => x.IDRecursoPai.Equals(null));
            else
                return new Repositorio<Recurso>().Listar().Where(y => y.IDModulo == idmodulo && y.Visivel.HasValue && y.Visivel.Value && !y.Arquivo.Equals(null)).Where(x => x.IDRecursoPai == idpai);
        }

        public static Recurso ObtemRecurso(int idrecurso)
        {
            return new Repositorio<Recurso>().ObterPorId(idrecurso);
        }

        public static Parametro ObtemParametro(string nome)
        {
            Parametro param = new Repositorio<Parametro>().Listar().Where( x => x.Nome == nome ).FirstOrDefault();
            if (param == null)
                param = InsereParametro(nome);
            return param;
        }

        public static Parametro InsereParametro(string parametro)
        {
            Repositorio<Parametro> rep = new Repositorio<Parametro>();

            Parametro p = new Parametro();
            p.Nome = parametro;
            p.Dominio = "Geral";
            p.Valor = string.Empty;
            p.Tipo = "str";           

            rep.Incluir(p);
            return p;
        }

        public static void atualizaParametro(string parametro, string valor)
        {
            Repositorio<Parametro> rep = new Repositorio<Parametro>();

            Parametro objparam = rep.Listar().Where(x => x.Nome == parametro).FirstOrDefault();

            if (objparam != null)
            {
                objparam.Valor = valor;
                rep.Alterar(objparam);
            }
        }

        public static int ObtemRecursoPorNome(string nomerecurso, int idmodulo)
        {
            var recurso = new Repositorio<Recurso>().Listar().Where(x => x.Arquivo == nomerecurso && x.IDModulo == idmodulo).FirstOrDefault();
            if (recurso == null)
                return -1;
            else
                return recurso.IDRecurso;
        }

        public static IQueryable<FluxoAprovacao> ListaFluxoAprovacao(int idprodutogrupo)
        {
            return new Repositorio<FluxoAprovacao>().Listar().Where(x => x.IDProdutoGrupo == idprodutogrupo);
        }

        public static IQueryable<FluxoTipo> ListaFluxoTipo()
        {
            return new Repositorio<FluxoTipo>().Listar();
        }

        public static IQueryable<EmpresaSolicitacaoTipo> ListaSolicitacaoTipo()
        {
            return new Repositorio<EmpresaSolicitacaoTipo>().Listar();
        }

        public static IQueryable<Auditoria> localizaAuditoria(string busca)
        {
             return new Repositorio<Auditoria>().Listar().Where( x => x.Usuario.NomeCompleto.Contains( busca ) );
        }

        public static Permissao ObtemPermissao(int IdPermissao)
        {
            return new Repositorio<Permissao>().ObterPorId(IdPermissao);
        }

        public static void RegistrarErro(string browser, string ip, int idmodulo, int idempresa, int idperfil, int idusuario, int idrecurso, string nomerecurso, string erro)
        {
            new Repositorio<AuditoriaOcorrencia>().Incluir(new AuditoriaOcorrencia() { Navegador = browser, IP = ip, IDModulo = idmodulo, IDEmpresa = idempresa, IDPerfil = idperfil, IDUsuario = idusuario, IDRecurso = idrecurso, NomeRecurso = nomerecurso, Descricao = erro, Data = DateTime.Now});
        }

        public static void RegistrarAcesso(string browser, string ip, int idmodulo, int idempresa, int idperfil, int idusuario, int idrecurso, string nomerecurso, string erro)
        {
            new Repositorio<AuditoriaAcesso>().Incluir(new AuditoriaAcesso() { Navegador = browser, IP = ip, IDModulo = idmodulo, IDEmpresa = idempresa, IDPerfil = idperfil, IDUsuario = idusuario, IDRecurso = idrecurso, NomeRecurso = nomerecurso, Descricao = erro, Data = DateTime.Now });
        }

        public static string ObtemIP(HttpRequest request)
        {
            string ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ip != "")
            {
                ip = request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }

        public static string ObtemBrowser(HttpRequest request)
        {
            System.Web.HttpBrowserCapabilities browser = request.Browser;
            string s = browser.Type + "\n"
                + " " + browser.Browser + "\n"
                + " " + browser.Version + "\n"
                + "Major Version = " + browser.MajorVersion + "\n"
                + "Minor Version = " + browser.MinorVersion + "\n"
                + "Supports Tables = " + browser.Tables + "\n"
                + "Supports Cookies = " + browser.Cookies + "\n"
                + "Supports VBScript = " + browser.VBScript + "\n"
                + "Supports JavaScript = " + (browser.EcmaScriptVersion.ToString().CompareTo("1") >= 0 ? "Sim " : "Nao ") + browser.EcmaScriptVersion.ToString() + "\n"
                + "Supports JavaScript Version = " + browser["JavaScriptVersion"] + "\n";
            return s;
        }


        internal static IQueryable<AuditoriaAcesso> PesquisarAuditoriaAcesso(string texto)
        {
            if (!string.IsNullOrEmpty(texto))
                return new Repositorio<AuditoriaAcesso>().Listar().Where(x => x.Descricao.Contains(texto));
            else
                return new Repositorio<AuditoriaAcesso>().Listar();
        }

        public static void RegistrarErro(HttpRequest request, string erro)
        {
            string browser = ObtemBrowser(request);
            string ip = ObtemIP(request);

            new Repositorio<AuditoriaOcorrencia>().Incluir(new AuditoriaOcorrencia() { Navegador = browser, IP = ip, IDModulo = DadosSessao.IdModulo, IDEmpresa = DadosSessao.IdBanco, IDPerfil = DadosSessao.IdPerfil, IDUsuario = DadosSessao.IdUsuario, IDRecurso = DadosSessao.IdRecurso, NomeRecurso = DadosSessao.NomeRecurso, Descricao = erro, Data = DateTime.Now });
        }
    }
}
