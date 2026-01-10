using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.DAL;
using CP.FastConsig.BLL;

namespace CP.FastConsig.Facade
{
    public static class FachadaFluxoAprovacao
    {

        public static IQueryable<FluxoAprovacao> ListaFluxoAprovacao(int idprodutogrupo)
        {
            return FluxoAprovacoes.ListaFluxoAprovacao(idprodutogrupo);
        }

        public static FluxoAprovacao ObtemFluxoAprovacao(int idprodutogrupo)
        {
            return FluxoAprovacoes.ObtemFluxoAprovacao(idprodutogrupo);
        }

        public static FluxoAprovacaoEmpresa ObtemFluxoAprovacaoEmpresa(int idprodutogrupo, int idempresa)
        {
            return FluxoAprovacoes.ObtemFluxoAprovacaoEmpresa(idprodutogrupo, idempresa);
        }

        public static void SalvarFluxoAprovacao(int idprodutogrupo, bool bconsignante, bool bfuncionario, bool bconsignataria)
        {
            FluxoAprovacoes.SalvarFluxoAprovacao(idprodutogrupo, bconsignante, bfuncionario, bconsignataria);
        }

        public static void SalvarFluxoAprovacaoEmpresa(int idprodutogrupo, int idempresa, bool habilitado)
        {
            FluxoAprovacoes.SalvarFluxoAprovacaoEmpresa(idprodutogrupo, idempresa, habilitado);    
        }

    }
}
