using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.DAL;
using System;
using System.Web;
using CP.FastConsig.Common;
using CP.FastConsig.Util;

namespace CP.FastConsig.BLL
{

    public static class Solicitacoes
    {

        public static void AdicionaSolicitacao(int idEmpresaSolicitante, int idTipoSolicitacao, int idSituacaoSolicitacao, int? idEmpresa, int? idAverbacao, int? idFuncionario, int? idSolicitante, int? idResponsavel, string descricao, string motivo)
        {

            EmpresaSolicitacao empresaSolicitacao = new EmpresaSolicitacao();

            empresaSolicitacao.DataSolicitacao = DateTime.Now;
            empresaSolicitacao.IDEmpresaSolicitante = idEmpresaSolicitante;
            empresaSolicitacao.IDEmpresaSolicitacaoTipo = idTipoSolicitacao;
            empresaSolicitacao.IDEmpresaSolicitacaoSituacao = idSituacaoSolicitacao;
            empresaSolicitacao.IDEmpresaSolicitacaoTipo = idTipoSolicitacao;
            empresaSolicitacao.IDEmpresa = idEmpresa;
            empresaSolicitacao.IDAverbacao = idAverbacao;
            empresaSolicitacao.IDFuncionario = idFuncionario;
            empresaSolicitacao.IDSolicitante = idSolicitante;
            empresaSolicitacao.IDResponsavel = idResponsavel;
            empresaSolicitacao.Descricao = descricao;
            empresaSolicitacao.Motivo = motivo;

            new Repositorio<EmpresaSolicitacao>().Incluir(empresaSolicitacao);

        }

        public static IQueryable<EmpresaSolicitacao> ObtemSolicitacoesPendentes()
        {
            return new Repositorio<EmpresaSolicitacao>().Listar();
        }

        public static IQueryable<EmpresaSolicitacao> ObtemSolicitacoesPendentes(string modulo, int empresasolicitacaotipo)
        {
            return new Repositorio<EmpresaSolicitacao>().Listar().Where(x => x.Empresa.EmpresaTipo.Consignataria.Equals(false) && x.EmpresaSolicitacaoTipo.Modulo.Contains(modulo) && x.IDEmpresaSolicitacaoSituacao == (int)Enums.SolicitacaoSituacao.Pendente);
        }

        public static string ObtemDescricao(int idSolicitacaoTipo)
        {
            return new Repositorio<EmpresaSolicitacaoTipo>().ObterPorId(idSolicitacaoTipo).Nome;
        }

        public static IQueryable<EmpresaSolicitacao> ObtemSolicitacoesPendentes(int idEntidade, string modulo = "", bool funcionario = false, int idBanco = 0, int idUsuario = 0)
        {

            IQueryable<EmpresaSolicitacao> solicitacoes;

            if (funcionario)
            {
                solicitacoes = new Repositorio<EmpresaSolicitacao>().Listar().Where(x => x.IDFuncionario != null && x.IDFuncionario.Value.Equals(idEntidade) && x.IDEmpresaSolicitacaoSituacao == (int)Enums.SolicitacaoSituacao.Pendente);
            }
            else if (modulo != "")
            {
                if (modulo == "1")
                    solicitacoes = new Repositorio<EmpresaSolicitacao>().Listar().Where(x => (x.IDEmpresaSolicitante == idEntidade || x.IDEmpresa == idEntidade) && x.EmpresaSolicitacaoTipo.Modulo.Contains(modulo) && x.IDEmpresaSolicitacaoSituacao == (int)Enums.SolicitacaoSituacao.Pendente);
                else if (idBanco == 0)
                    solicitacoes = new Repositorio<EmpresaSolicitacao>().Listar().Where(x => x.EmpresaSolicitacaoTipo.Modulo.Contains(modulo) && x.IDEmpresaSolicitacaoSituacao == (int)Enums.SolicitacaoSituacao.Pendente);
                else
                    solicitacoes = new Repositorio<EmpresaSolicitacao>().Listar().Where(x => x.IDEmpresa != null && x.IDEmpresa.Value.Equals(idEntidade) && x.EmpresaSolicitacaoTipo.Modulo.Contains(modulo) && x.IDEmpresaSolicitacaoSituacao == (int)Enums.SolicitacaoSituacao.Pendente);
            }
            else
            {
                solicitacoes = new Repositorio<EmpresaSolicitacao>().Listar().Where(x => x.IDEmpresa != null && (x.IDEmpresa.Value.Equals(idEntidade)) && x.IDEmpresaSolicitacaoSituacao == (int)Enums.SolicitacaoSituacao.Pendente); //  || x.IDEmpresaSolicitante.Value.Equals(idEntidade)
            }

            int idmodulo;
            if (HttpContext.Current.Session["IdModulo"] != null)
            {
                idmodulo = (int)HttpContext.Current.Session["IdModulo"];

                if (idmodulo == (int)Enums.Modulos.Consignante || idmodulo == (int)Enums.Modulos.Consignataria)
                {
                    //IQueryable<UsuarioResponsabilidade> resp = Solicitacoes.ObtemSolicitacoesPendentesUsuario(idUsuario, idBanco);
                    var listasolic = solicitacoes.ToList();
                    List<UsuarioResponsabilidade> resp = Solicitacoes.ObtemSolicitacoesPendentesUsuario(idUsuario, idBanco).ToList();

                    if (resp.Count() > 0)
                    {
                        var dados = from r in resp
                                       join s in listasolic on r.IDEmpresaSolicitacaoTipo equals s.IDEmpresaSolicitacaoTipo
                                       select s;

                        solicitacoes = dados.AsQueryable();
                    }

                }
            }

            return solicitacoes;
        }

        public static IQueryable<EmpresaSolicitacao> ObtemSolicitacoesPendentesSolicitadasPelaEmpresa(int idEntidade)
        {

            IQueryable<EmpresaSolicitacao> solicitacoes;

            solicitacoes = new Repositorio<EmpresaSolicitacao>().Listar().Where(x => x.IDEmpresaSolicitacao != null && x.IDEmpresaSolicitante == idEntidade && x.IDEmpresaSolicitacaoSituacao == (int)Enums.SolicitacaoSituacao.Pendente);

            return solicitacoes;
        }

        
        public static IQueryable<UsuarioResponsabilidade> ObtemSolicitacoesPendentesUsuario(int idUsuario, int idEmpresa)
        {
            return new Repositorio<UsuarioResponsabilidade>().Listar().Where(x => x.IDUsuario == idUsuario && x.IDEmpresa == idEmpresa);

        }

        public static EmpresaSolicitacao ObtemUltimaSolicitacao(int idaverbacao, int idsolicitacaotipo, int idsolicitacaosituacao)
        {
            return new Repositorio<EmpresaSolicitacao>().Listar().Where(x => x.IDAverbacao == idaverbacao && x.IDEmpresaSolicitacaoTipo == idsolicitacaotipo && x.IDEmpresaSolicitacaoSituacao == idsolicitacaosituacao).OrderByDescending(x => x.IDEmpresaSolicitacao).FirstOrDefault();
        }

        public static EmpresaSolicitacao ObtemUltimaSolicitacaoProcessadaFuncionario(int idaverbacao, int idfuncionario, int idsolicitacaotipo)
        {
            return new Repositorio<EmpresaSolicitacao>().Listar().Where(x => x.IDAverbacao == idaverbacao && x.IDEmpresaSolicitacaoTipo == idsolicitacaotipo && x.IDFuncionario == idfuncionario && x.IDEmpresaSolicitacaoSituacao == (int)Enums.SolicitacaoSituacao.Processada).OrderByDescending(x => x.IDEmpresaSolicitacao).FirstOrDefault();
        }

        public static EmpresaSolicitacao ObtemUltimaSolicitacaoPendente(int idaverbacao, int idempresa, int idsolicitacaotipo)
        {
            if (idsolicitacaotipo == (int)Enums.SolicitacaoTipo.InformarQuitacao)
                return new Repositorio<EmpresaSolicitacao>().Listar().Where(x => x.IDAverbacao == idaverbacao && (x.IDEmpresaSolicitacaoTipo == idsolicitacaotipo || x.IDEmpresaSolicitacaoTipo == (int)Enums.SolicitacaoTipo.RegularizarQuitaçãoRejeitada) && x.IDEmpresa == idempresa && x.IDEmpresaSolicitacaoSituacao == (int)Enums.SolicitacaoSituacao.Pendente).OrderByDescending(x => x.IDEmpresaSolicitacao).FirstOrDefault();
            else
                return new Repositorio<EmpresaSolicitacao>().Listar().Where(x => x.IDAverbacao == idaverbacao && x.IDEmpresaSolicitacaoTipo == idsolicitacaotipo && x.IDEmpresa == idempresa && x.IDEmpresaSolicitacaoSituacao == (int)Enums.SolicitacaoSituacao.Pendente).OrderByDescending(x => x.IDEmpresaSolicitacao).FirstOrDefault();
        }

        public static EmpresaSolicitacao ObtemUltimaSolicitacaoFuncPendente(int idaverbacao, int idfunc, int idsolicitacaotipo)
        {
            if (idsolicitacaotipo == (int)Enums.SolicitacaoTipo.InformarQuitacao)
                return new Repositorio<EmpresaSolicitacao>().Listar().Where(x => x.IDAverbacao == idaverbacao && (x.IDEmpresaSolicitacaoTipo == idsolicitacaotipo || x.IDEmpresaSolicitacaoTipo == (int)Enums.SolicitacaoTipo.RegularizarQuitaçãoRejeitada) && x.IDFuncionario == idfunc && x.IDEmpresaSolicitacaoSituacao == (int)Enums.SolicitacaoSituacao.Pendente).OrderByDescending(x => x.IDEmpresaSolicitacao).FirstOrDefault();
            else
                return new Repositorio<EmpresaSolicitacao>().Listar().Where(x => x.IDAverbacao == idaverbacao && x.IDEmpresaSolicitacaoTipo == idsolicitacaotipo && x.IDFuncionario == idfunc && x.IDEmpresaSolicitacaoSituacao == (int)Enums.SolicitacaoSituacao.Pendente).OrderByDescending(x => x.IDEmpresaSolicitacao).FirstOrDefault();
        }

        public static EmpresaSolicitacao ObtemUltimaSolicitacaoProcessadaEmpresa(int idaverbacao, int idempresa, int idsolicitacaotipo)
        {
            return new Repositorio<EmpresaSolicitacao>().Listar().Where(x => x.IDAverbacao == idaverbacao && x.IDEmpresaSolicitacaoTipo == idsolicitacaotipo && x.IDEmpresa == idempresa && x.IDEmpresaSolicitacaoSituacao == (int)Enums.SolicitacaoSituacao.Processada).OrderByDescending(x => x.IDEmpresaSolicitacao).FirstOrDefault();
        }

        public static EmpresaSolicitacao ObtemUltimaSolicitacaoProcessadaTipo(int idaverbacao, int idsolicitacaotipo)
        {
            return new Repositorio<EmpresaSolicitacao>().Listar().Where(x => x.IDAverbacao == idaverbacao && (x.IDEmpresaSolicitacaoTipo == idsolicitacaotipo && x.IDEmpresaSolicitacaoSituacao == (int)Enums.SolicitacaoSituacao.Processada)).OrderByDescending(x => x.IDEmpresaSolicitacao).FirstOrDefault();
        }

        public static EmpresaSolicitacao ObtemUltimaSolicitacaoProcessada(int idaverbacao)
        {
            return new Repositorio<EmpresaSolicitacao>().Listar().Where(x => x.IDAverbacao == idaverbacao && (x.IDEmpresaSolicitacaoSituacao == (int)Enums.SolicitacaoSituacao.Processada || x.IDEmpresaSolicitacaoSituacao == (int)Enums.SolicitacaoSituacao.Rejeitada)).OrderByDescending(x => x.IDEmpresaSolicitacao).FirstOrDefault();
        }

        internal static void AtualizaSolicitacao(int idempresasolicitacao, int idsolicitacaosituacao, int idresponsavel, string obs)
        {
            Repositorio<EmpresaSolicitacao> repes = new Repositorio<EmpresaSolicitacao>();
            EmpresaSolicitacao es = repes.ObterPorId(idempresasolicitacao);
            es.IDEmpresaSolicitacaoSituacao = idsolicitacaosituacao;
            es.DataAtendimento = DateTime.Now;
            es.IDResponsavel = idresponsavel;
            es.Motivo = obs;
            repes.Alterar(es);
        }

        public static IQueryable<EmpresaSolicitacao> ObtemSolicitacoesDoSolicitantePendentes(int IDEmpresa, int idsolicitacaotipo)
        {
            return new Repositorio<EmpresaSolicitacao>().Listar().Where(x => x.IDEmpresaSolicitante == IDEmpresa && x.IDEmpresaSolicitacaoTipo == idsolicitacaotipo && x.IDEmpresaSolicitacaoSituacao == (int)Enums.SolicitacaoSituacao.Pendente);
        }

        public static IQueryable<EmpresaSolicitacaoTipo> ListaSolicitacaoTipo()
        {
            return new Repositorio<EmpresaSolicitacaoTipo>().Listar();
        }

        public static EmpresaSolicitacaoQuitacao ObtemSolicitacaoQuitacao(int idAverbacao)
        {
            EmpresaSolicitacao empresaSolicitacao = ObtemUltimaSolicitacao(idAverbacao, (int)Enums.SolicitacaoTipo.InformarQuitacao, (int)Enums.SolicitacaoSituacao.Processada);
            if (empresaSolicitacao != null)
                return new Repositorio<EmpresaSolicitacaoQuitacao>().Listar().Where(x => x.IDEmpresaSolicitacao.Equals(empresaSolicitacao.IDEmpresaSolicitacao) && x.IDAverbacao.Equals(idAverbacao)).ToList().LastOrDefault();
            else
                return null;
        }

        public static EmpresaSolicitacaoSaldoDevedor ObtemSolicitacaoSaldoDevedor(int idAverbacao)
        {
            EmpresaSolicitacao empresaSolicitacao = ObtemUltimaSolicitacao(idAverbacao, (int)Enums.SolicitacaoTipo.InformarSaldoDevedordeContratos, (int)Enums.SolicitacaoSituacao.Processada);
            if (empresaSolicitacao != null)
            {
                int idempresasolicitacao = empresaSolicitacao.IDEmpresaSolicitacao;
                var dados = new Repositorio<EmpresaSolicitacaoSaldoDevedor>().Listar().Where(x => x.IDEmpresaSolicitacao == idempresasolicitacao && x.IDAverbacao == idAverbacao);
                if (dados.Count() > 0)
                    return dados.ToList().LastOrDefault();
                else
                    return null;
            }
            else
                return null;
        }

        public static EmpresaSolicitacao ObtemSolicitacaoPendente(int idAverbacao, int tipoSolicitacao)
        {
            return ObtemUltimaSolicitacao(idAverbacao, tipoSolicitacao, (int)Enums.SolicitacaoSituacao.Pendente);
        }

        public static EmpresaSolicitacaoSaldoDevedor ObtemSaldoDevedor(int idsolicitacao)
        {
            return new Repositorio<EmpresaSolicitacaoSaldoDevedor>().Listar().Where(x => x.IDEmpresaSolicitacao == idsolicitacao).FirstOrDefault();
        }

        public static IEnumerable<EmpresaSolicitacaoTipo> ObterTiposSolicitacoes(int idModulo)
        {
            return ListaSolicitacaoTipo().ToList().Where(x => !string.IsNullOrEmpty(x.Modulo) &&  x.Modulo.Split(new []{';'},StringSplitOptions.RemoveEmptyEntries).ToList().Contains(idModulo.ToString())).ToList();
        }

        public static void AdicionaResponsabilidade(int idUsuario, int idBanco, int idSolicitacaoTipo)
        {

            UsuarioResponsabilidade usuarioResponsabilidade = new UsuarioResponsabilidade();
            
            usuarioResponsabilidade.IDEmpresa = idBanco;
            usuarioResponsabilidade.IDUsuario = idUsuario;
            usuarioResponsabilidade.IDEmpresaSolicitacaoTipo = idSolicitacaoTipo;

            new Repositorio<UsuarioResponsabilidade>().Incluir(usuarioResponsabilidade);

        }

        public static void RemoveResponsabilidade(int idUsuario, int idBanco, int idSolicitacaoTipo)
        {

            Repositorio<UsuarioResponsabilidade> repositorio = new Repositorio<UsuarioResponsabilidade>();

            UsuarioResponsabilidade usuarioResponsabilidade = repositorio.Listar().FirstOrDefault(x => x.IDUsuario.Equals(idUsuario) && x.IDEmpresa.Equals(idBanco) && x.IDEmpresaSolicitacaoTipo.Equals(idSolicitacaoTipo));

            if(usuarioResponsabilidade != null) repositorio.Excluir(usuarioResponsabilidade);

        }

        public static IQueryable<UsuarioResponsabilidade> ListaResponsabilidades(int idUsuario, int idBanco)
        {
            return new Repositorio<UsuarioResponsabilidade>().Listar().Where(x => x.IDUsuario.Equals(idUsuario) && x.IDEmpresa.Equals(idBanco));
        }


        public static IQueryable<EmpresaSolicitacaoTipo> ListaSolicitacaoTipo(int idmodulo)
        {
            string sModulo = idmodulo.ToString();
            return new Repositorio<EmpresaSolicitacaoTipo>().Listar().Where(x => x.Modulo.Contains(sModulo));
        }

        public static IEnumerable<Averbacao> ListaContratosSolicitacoes(int idEmpresa)
        {
            return new Repositorio<EmpresaSolicitacao>().Listar().Where(x => x.IDEmpresaSolicitante != null && x.IDEmpresaSolicitante.Value.Equals(idEmpresa) && x.IDAverbacao != null && x.IDEmpresaSolicitacaoSituacao.Equals((int)Enums.SolicitacaoSituacao.Pendente)).ToList().Select(x => Averbacoes.ObtemAverbacao(x.IDAverbacao.Value));
        }

        public static bool VerificaSolicitacoesCompraProcessadas(int idaverbacaopai, int idaverbacao)
        {
            return new Repositorio<EmpresaSolicitacao>().Listar().Where(x => x.IDAverbacao == idaverbacao && (x.IDEmpresaSolicitacaoTipo == (int)Enums.SolicitacaoTipo.InformarSaldoDevedordeContratos || x.IDEmpresaSolicitacaoTipo == (int)Enums.SolicitacaoTipo.InformarQuitacao) && x.IDEmpresaSolicitacaoSituacao == (int)Enums.SolicitacaoSituacao.Processada).OrderByDescending(x => x.IDEmpresaSolicitacao).FirstOrDefault() != null;
        }

        public static bool ContratoEmProcessoDeQuitacaoOuCompra(int idAverbacao)
        {
            return new Repositorio<EmpresaSolicitacao>().Listar().Any(x => x.IDAverbacao == idAverbacao && x.IDEmpresaSolicitacaoSituacao.Equals((int)Enums.SolicitacaoSituacao.Pendente) && (x.IDEmpresaSolicitacaoTipo == (int)Enums.SolicitacaoTipo.InformarQuitacao || x.IDEmpresaSolicitacaoTipo == (int)Enums.SolicitacaoTipo.InformarSaldoDevedordeContratos || x.IDEmpresaSolicitacaoTipo == (int)Enums.SolicitacaoTipo.RegularizarQuitaçãoRejeitada || x.IDEmpresaSolicitacaoTipo == (int)Enums.SolicitacaoTipo.ConcluirCompradeDivida));
        }


        public static IQueryable<EmpresaSolicitacao> SolicitacoesPrazoExpiradoParaLiquidacao()
        {
            Repositorio<EmpresaSolicitacao> rep = new Repositorio<EmpresaSolicitacao>();
            int dias = ObtemSolicitacaoTipo((int)Enums.SolicitacaoTipo.ConfirmarRejeitarQuitacao).Prazo;
            dias = Utilidades.SomaDiasUteis(dias);

            DateTime dataexpiracao = DateTime.Today.AddDays(-dias);
            return rep.Listar().Where(x => x.IDEmpresaSolicitacaoTipo == (int)Enums.SolicitacaoTipo.ConfirmarRejeitarQuitacao && x.IDEmpresaSolicitacaoSituacao == (int)Enums.SolicitacaoSituacao.Pendente && x.DataSolicitacao.CompareTo(dataexpiracao) < 0);
        }

        public static EmpresaSolicitacaoTipo ObtemSolicitacaoTipo(int idsolicitacaotipo)
        {
            return new Repositorio<EmpresaSolicitacaoTipo>().ObterPorId(idsolicitacaotipo);
        }
    }

}