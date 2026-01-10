using System;
using System.Linq;
using CP.FastConsig.BLL;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Util;

namespace CP.FastConsig.Facade
{

    public static class FachadaInformarQuitacao
    {

        public static int InformarQuitacao(int averbacao, int idempresa, int idresponsavel, string obs)
        {
            return Averbacoes.SolicitacaoAtualizar(averbacao, idempresa, (int)Enums.SolicitacaoTipo.InformarQuitacao, idresponsavel, false, obs);
        }

        public static void SalvaInformacaoQuitacao(DateTime dataQuitacao, decimal valor, int idTipoFormaPagamento, string observacao, int idAverbacao, string comprovante)
        {
            Averbacoes.SalvaInformacaoQuitacao(dataQuitacao, valor, idTipoFormaPagamento, observacao, idAverbacao, comprovante);
        }

        public static IQueryable<TipoPagamento> ObtemFormasPagamento()
        {
            return FachadaConsignatarias.ListaTiposPagamentos();
        }


        public static EmpresaSolicitacao ObtemSolicitacaoOrigem(int idaverbacao)
        {
            return Solicitacoes.ObtemUltimaSolicitacaoProcessadaTipo(idaverbacao, (int)Enums.SolicitacaoTipo.InformarQuitacao);
        }
     
        public static decimal CalculaSaldoRestante(int idaverbacao)
        {
            return Averbacoes.CalculaSaldoRestante(idaverbacao);
        }
    }
}