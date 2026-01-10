using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;

namespace CP.FastConsig.BLL
{
    public static class Consignantes
    {
        public static IQueryable<TmpEnviadosVDescontados> listaEnviadosVDescontados()
        {
            return new Repositorio<TmpEnviadosVDescontados>().Listar();
        }

        public static IQueryable<TmpOcorrencias> obtemOcorrencias(int tipo)
        {
            return new Repositorio<TmpOcorrencias>().Listar().Where( x => x.Tipo == tipo );
        }

        public static IQueryable<EmpresaSolicitacaoTipo> listaSolicitacoesTipo()
        {
            return new Repositorio<EmpresaSolicitacaoTipo>().Listar();
        }

        public static IQueryable<EmpresaSolicitacao> listaSolicitacoes()
        {
            return new Repositorio<EmpresaSolicitacao>().Listar();
        }

        public static IQueryable<EmpresaSolicitacaoTipo> listaPrazosExecucao(string idmodulo)
        {
            return new Repositorio<EmpresaSolicitacaoTipo>().Listar().Where(x => x.Modulo.Contains(idmodulo)).OrderBy( x => x.Nome );
        }

        public static decimal? ADescontar(string competencia, int idempresa = 0)
        {
            List<ConciliacaoMovimento> dados;
            if (idempresa == 0)
                dados = new Repositorio<ConciliacaoMovimento>().Listar().Where(x => x.Competencia == competencia).ToList();
            else
                dados = new Repositorio<ConciliacaoMovimento>().Listar().Where(x => x.Competencia == competencia && x.IDConsignataria == idempresa).ToList();


            decimal? aDescontar = 0;
            
            if (dados.Count > 0)
            {
                aDescontar = dados.Sum(z => z.Valor);
            }
            else
            {
                List<AverbacaoParcela> parcelas;
                if (idempresa == 0)
                    parcelas = new Repositorio<AverbacaoParcela>().Listar().Where(x => x.Competencia == competencia).ToList();
                else
                    parcelas = new Repositorio<AverbacaoParcela>().Listar().Where(x => x.Competencia == competencia && x.Averbacao.IDConsignataria == idempresa).ToList();
                    

                if (parcelas.Count > 0)
                    aDescontar = parcelas.Sum(z => z.Valor);

            }
            return aDescontar;
        }

        public static decimal? Descontados(string competencia, int idempresa = 0)
        {
            List<ConciliacaoMovimento> dados;
            
            if (idempresa == 0)
                dados = new Repositorio<ConciliacaoMovimento>().Listar().Where(x => x.Competencia == competencia).ToList();
            else
                dados = new Repositorio<ConciliacaoMovimento>().Listar().Where(x => x.Competencia == competencia && x.IDConsignataria == idempresa).ToList();

            decimal? Descontados = 0;

            if (dados.Count > 0)
            {
                Descontados = dados.Sum(z => z.ValorDescontado);
            }
            else
            {
                List<AverbacaoParcela> parcelas;
                if (idempresa == 0)
                    parcelas = new Repositorio<AverbacaoParcela>().Listar().Where(x => x.Competencia == competencia).ToList();
                else
                    parcelas = new Repositorio<AverbacaoParcela>().Listar().Where(x => x.Competencia == competencia && x.Averbacao.IDConsignataria == idempresa).ToList();

                if (parcelas.Count > 0)
                    Descontados = parcelas.Sum(z => z.ValorDescontado);

            }
            return Descontados;

        }
    }
}
