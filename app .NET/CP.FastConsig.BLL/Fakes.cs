using System.Linq;
using CP.FastConsig.DAL;

namespace CP.FastConsig.BLL
{

    public static class Fakes
    {

        public static IQueryable<TmpSimulacaoCompraDivida> ObtemSimulacoesCompraDivida()
        {
            return new Repositorio<TmpSimulacaoCompraDivida>().Listar();
        }

    }

}