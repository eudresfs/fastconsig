using System;
using System.Linq;
using CP.FastConsig.BLL;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{

    public static class FachadaInformarSaldoDevedor
    {

        public static EmpresaSolicitacao ObtemSolicitacaoOrigem(int idaverbacao)
        {
            return Solicitacoes.ObtemUltimaSolicitacaoPendente(idaverbacao, FachadaAverbacoes.ObtemAverbacao(idaverbacao).IDConsignataria, (int)Enums.SolicitacaoTipo.InformarSaldoDevedordeContratos);
        }

        public static EmpresaSolicitacao ObtemSolicitacaoFuncOrigem(int idaverbacao)
        {
            return Solicitacoes.ObtemUltimaSolicitacaoFuncPendente(idaverbacao, FachadaAverbacoes.ObtemAverbacao(idaverbacao).IDFuncionario, (int)Enums.SolicitacaoTipo.InformarSaldoDevedordeContratos);
        }

        public static EmpresaSolicitacao ObtemSolicitacaoProcessadaOrigem(int idaverbacao)
        {
            return Solicitacoes.ObtemUltimaSolicitacaoProcessadaTipo(idaverbacao, (int)Enums.SolicitacaoTipo.InformarSaldoDevedordeContratos);
        }

        public static EmpresaSolicitacaoSaldoDevedor ObtemSaldoDevedor(int idsolicitacao)
        {
            return Solicitacoes.ObtemSaldoDevedor(idsolicitacao);
        }
    }

}