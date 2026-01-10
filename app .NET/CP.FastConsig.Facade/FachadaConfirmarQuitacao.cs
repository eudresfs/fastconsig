using CP.FastConsig.BLL;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{

    public static class FachadaConfirmarQuitacao
    {

        public static void ConfirmarQuitacao(int averbacao, int idBanco, string observacao, int idResponsavel)
        {
            Averbacoes.ConfirmarQuitacao(averbacao, idBanco, observacao, idResponsavel);
        }

        public static void RejeitarQuitacao(int averbacao, int idBanco, int idResponsavel)
        {
            Averbacoes.SolicitacaoAtualizar(averbacao, idBanco, (int)Enums.SolicitacaoTipo.ConfirmarRejeitarQuitacao, idResponsavel, true);
        }

        public static EmpresaSolicitacaoQuitacao ObtemSolicitacaoQuitacao(int idAverbacao)
        {
            return Solicitacoes.ObtemSolicitacaoQuitacao(idAverbacao);
        }

    }

}