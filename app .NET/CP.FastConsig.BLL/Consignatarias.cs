using System;
using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;

namespace CP.FastConsig.BLL
{

    public static class Consignatarias
    {

        public static IQueryable<Empresa> ObtemConsignatarias()
        {
            return new Repositorio<Empresa>().Listar().Where(x => x.IDEmpresaTipo >= 4).OrderBy(x => x.Fantasia);
        }

        public static Empresa ObtemConsignataria(int idconsignataria)
        {
            return new Repositorio<Empresa>().Listar().Where(x => x.IDEmpresa == idconsignataria).FirstOrDefault();
        }

        public static IQueryable<ProdutoGrupo> ListaProdutoGrupos()
        {
            return new Repositorio<ProdutoGrupo>().Listar();            
        }

        public static IEnumerable<MargemFuncionarioHistorico> ListaMargensHistorico(DateTime datai, DateTime dataf)
        {
            var dados = new Repositorio<FuncionarioHistorico>().Listar().Where( x => x.CreatedOn >= datai && x.CreatedOn <= dataf);

            var consulta = from d in dados
                           group d by new { d.IDFuncionario, d.CreatedOn.Value.Year, d.CreatedOn.Value.Month} into g                           
                           select new {func = g.Key.IDFuncionario, mes = g.Key.Month, ano = g.Key.Year, id = g.Max( x => x.IDFuncionarioHistorico)};

            var soma = from s in consulta
                       from d in dados
                       where s.id == d.IDFuncionarioHistorico && s.func== d.IDFuncionario
                       select new { ano = s.ano, mes = s.mes, soma = d.MargemFolhaBruta };

            var grupo = from gg in soma
                        group gg by new { gg.ano, gg.mes } into g
                        select new MargemFuncionarioHistorico { ano = g.Key.ano, mes = g.Key.mes, soma = g.Sum(x => x.soma) };

            return grupo;

        }

        public static IQueryable<TmpGrupoBoasNoticias> ListaGrupoBoasNoticias(string competencia)
        {
            return new Repositorio<TmpGrupoBoasNoticias>().Listar().Where( x => x.Competencia == competencia );
        }

        public static IQueryable<TmpBoasNoticias> obtemBoasNoticias(int id, string competencia, int IDConsignataria)
        {
            if (IDConsignataria == 0)
                return new Repositorio<TmpBoasNoticias>().Listar().Where(x => x.IdGrupoBoaNoticia == id && x.Competencia == competencia && x.IDConsignataria.Equals(null));
            else
                return new Repositorio<TmpBoasNoticias>().Listar().Where( x => x.IdGrupoBoaNoticia == id && x.Competencia == competencia && x.IDConsignataria == IDConsignataria);
        }

        public static IQueryable<TmpMasNoticias> obtemMasNoticias(string competencia, int IDConsignataria)
        {
            if (IDConsignataria == 0)
                return new Repositorio<TmpMasNoticias>().Listar().Where(x => x.Competencia == competencia && x.IDConsignataria.Equals(null));
            else
                return new Repositorio<TmpMasNoticias>().Listar().Where(x => x.Competencia == competencia && x.IDConsignataria == IDConsignataria);
        }

        public static IQueryable<TmpMasNoticiasDetalhe> obtemMasNoticiasDetalhe(int id)
        {
            return new Repositorio<TmpMasNoticiasDetalhe>().Listar().Where(x => x.IDMasNoticia == id);
        }

        public static IQueryable<TmpBoasNoticiasDetalhe> obtemBoasNoticiasDetalhe(int id)
        {
            return new Repositorio<TmpBoasNoticiasDetalhe>().Listar().Where(x => x.IdBoaNoticia == id);
        }
                                                             
        public static IQueryable<TmpMasNoticiasInadiplentes> obtemMasNoticiasInadiplentes(string competencia, int IDConsignataria)
        {
            if (IDConsignataria == 0)
            {
                return new Repositorio<TmpMasNoticiasInadiplentes>().Listar().Where(x => x.Competencia == competencia );
            }
            else
            {
                return new Repositorio<TmpMasNoticiasInadiplentes>().Listar().Where(x => x.Competencia == competencia && x.IDEmpresa == IDConsignataria);
            }
        }

        public static IQueryable<TmpMasNoticiasInadiplentesDetalhe> obtemMasNoticiasInadiplentesDetalhe(int id)
        {
            return new Repositorio<TmpMasNoticiasInadiplentesDetalhe>().Listar().Where(x => x.IDMasNoticiaInadiplente == id);
        }

        public static IQueryable<TmpInadimplenciaGeral> listaInadimplenciaGeral(string competencia, int idempresa)
        {
            if (idempresa == 0)
                return new Repositorio<TmpInadimplenciaGeral>().Listar().Where(x => x.Competencia == competencia);
            else
                return new Repositorio<TmpInadimplenciaGeral>().Listar().Where( x => x.Competencia == competencia && x.IDEmpresa == idempresa );
        }

        public static IQueryable<TmpVolumeInadimplencia> listaVolumeInadimplencia(int idempresa)
        {
            if (idempresa == 0)
                return new Repositorio<TmpVolumeInadimplencia>().Listar();
            else
                return new Repositorio<TmpVolumeInadimplencia>().Listar().Where(x => x.IDEmpresa == idempresa);
        }

        public static IQueryable<TmpInadimplenciaPadraoTrabalho> listaInadimplenciaPadraoTrabalho(string competencia, int idempresa)
        {
            if (idempresa == 0)
                return new Repositorio<TmpInadimplenciaPadraoTrabalho>().Listar().Where(x => x.Competencia == competencia);
            else
                return new Repositorio<TmpInadimplenciaPadraoTrabalho>().Listar().Where( x => x.Competencia == competencia && x.IDEmpresa == idempresa );
        }

        public static IQueryable<TmpInadimplenciaPadraoMargem> listaInadimplenciaPadraoMargem(string competencia, int idempresa)
        {
            if (idempresa == 0)
                return new Repositorio<TmpInadimplenciaPadraoMargem>().Listar().Where(x => x.Competencia == competencia);
            else
                return new Repositorio<TmpInadimplenciaPadraoMargem>().Listar().Where(x => x.Competencia == competencia && x.IDEmpresa == idempresa);
        }

        public static IQueryable<TmpInadimplenciaTempo> listaInadimplenciaTempo()
        {
            return new Repositorio<TmpInadimplenciaTempo>().Listar();
        }

        public static IQueryable<TmpInadimplenciaTempoDetalhe> listaInadimplenciaTempoDetalhe()
        {
            return new Repositorio<TmpInadimplenciaTempoDetalhe>().Listar();
        }

        public static IQueryable<TmpInadimplenciaGeralDetalhe> listaInadimplenciaGeralDetalhe(int id)
        {
            return new Repositorio<TmpInadimplenciaGeralDetalhe>().Listar().Where(x => x.IDInadimplenciaGeral == id);
        }

        public static IQueryable<TmpEnviadosVDescontados> listaEnviadosVDescontados()
        {
            return new Repositorio<TmpEnviadosVDescontados>().Listar();
        }


        public static IQueryable<TmpIndicesNegocios> obtemIndicesNegocios(int tipo)
        {
            return new Repositorio<TmpIndicesNegocios>().Listar().Where( x => x.Tipo == tipo );
        }

        public static IQueryable<TipoPagamento> ListaTiposPagamentos()
        {
            return new Repositorio<TipoPagamento>().Listar();
        }

        public static bool EmpresaBloqueada(int idEmpresa)
        {

            Empresa empresa = new Repositorio<Empresa>().ObterPorId(idEmpresa);

            bool existeSuspensao = ObtemSuspensoes(idEmpresa).ToList().FirstOrDefault() != null;
            bool empresaBloqueada = empresa != null && empresa.IDEmpresaSituacao > (int)Enums.EmpresaSituacao.Normal;

            return existeSuspensao || empresaBloqueada;

        }

        public static IQueryable<EmpresaSuspensao> ObtemSuspensoes(int idEmpresa)
        {
            return new Repositorio<EmpresaSuspensao>().Listar().Where(x => x.IDEmpresa.Equals(idEmpresa)).OrderByDescending(x => x.IDEmpresaSuspensao);
        }


        public static void VolumeValorAverbacoes(string competencia, string primeiracompetencia, int idempresa, int idprodutogrupo, out decimal? valorBruto, out decimal? valorAdicionado)
        {
            var conciliacao = new Repositorio<ConciliacaoCorteFolha>().Listar().Where(x => x.Competencia.CompareTo(primeiracompetencia) >= 0 && x.Competencia.CompareTo(competencia) <= 0 && x.IDConsignataria == idempresa && x.IDProdutoGrupo == idprodutogrupo);

            valorBruto = conciliacao.Sum(x => x.AverbacaoParcela.Averbacao.ValorDevidoTotal);

            valorAdicionado = conciliacao.Sum(x => x.AverbacaoParcela.Averbacao.ValorDevidoTotal - x.AverbacaoParcela.Averbacao.ValorRefinanciado);
            /*
            var dados = from v in conciliacao
                        group v by new { v.Competencia } into g
                        select new { ValorBruto = g.Sum(x => x.AverbacaoParcela.Averbacao.ValorDevidoTotal), ValorAdicionado = g.Sum(x => x.AverbacaoParcela.Averbacao.ValorDevidoTotal - x.AverbacaoParcela.Averbacao.ValorRefinanciado) };
            */
        }

        public static decimal? QtdeParcelasAverbacoes(string competencia, int idempresa, int idprodutogrupo)
        {
            var qtdeparcelas = new Repositorio<ConciliacaoCorteFolha>().Listar().Where(x => x.Competencia.CompareTo(competencia) == 0 && x.IDConsignataria == idempresa && x.IDProdutoGrupo == idprodutogrupo).Select( x => x.ValorParcela ?? 0 ).ToList();
            return qtdeparcelas.Sum();
        }

        public static IQueryable<TmpRecuperavelPorFolha> listaRecuperavelPelaFolha(string competencia, int idempresa)
        {
            if (idempresa == 0)
                return new Repositorio<TmpRecuperavelPorFolha>().Listar().Where(x => x.Competencia == competencia);
            else
                return new Repositorio<TmpRecuperavelPorFolha>().Listar().Where(x => x.Competencia == competencia && x.IDEmpresa == idempresa);
        }

        public static IQueryable<TmpNaoRecuperavel> listaNaoRecuperavel(string competencia, int idempresa)
        {
            if (idempresa == 0)
                return new Repositorio<TmpNaoRecuperavel>().Listar().Where(x => x.Competencia == competencia);
            else
                return new Repositorio<TmpNaoRecuperavel>().Listar().Where(x => x.Competencia == competencia && x.IDEmpresa == idempresa);
        }

        public static string ultimaCompetenciaConciliada(int idempresa)
        {
            bool bExiste = new Repositorio<Conciliacao>().Listar().Any(x => x.IDConsignataria == idempresa);
            if (bExiste)
            {
                return new Repositorio<Conciliacao>().Listar().Where(x => x.IDConsignataria == idempresa).Max(x => x.Competencia).ToString();
            }
            else
            {
                return "";
            }
        }

        public static decimal ValorInadimplencia(string competencia, int idempresa, string descricao)
        {
            return new Repositorio<TmpInadimplenciaGeral>().Listar().Where(x => x.Competencia == competencia && x.IDEmpresa == idempresa && x.Descricao == descricao).Max( x => x.Percentual ).Value;
        }
    }

}