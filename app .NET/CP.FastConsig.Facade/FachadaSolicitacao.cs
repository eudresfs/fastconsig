using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.DAL;
using CP.FastConsig.BLL;

namespace CP.FastConsig.Facade
{
    public static class FachadaSolicitacao
    {

        public static EmpresaSolicitacaoQuitacao ObtemSolicitacaoQuitacao(int idAverbacao)
        {
            return Solicitacoes.ObtemSolicitacaoQuitacao(idAverbacao);
        }

        public static EmpresaSolicitacaoSaldoDevedor ObtemSolicitacaoSaldoDevedor(int idAverbacao)
        {
            return Solicitacoes.ObtemSolicitacaoSaldoDevedor(idAverbacao);
        }

        public static EmpresaSolicitacao ObtemSolicitacaoPendente(int idAverbacao, int tipoSolicitacao)
        {
            return Solicitacoes.ObtemSolicitacaoPendente(idAverbacao, tipoSolicitacao);
        }

        public static IQueryable<EmpresaSolicitacaoTipo> listaSolicitacaoTipo()
        {
            return Solicitacoes.ListaSolicitacaoTipo();
        }

    }
}
