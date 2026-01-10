using CP.FastConsig.BLL;
using System.Linq;

namespace CP.FastConsig.Facade
{

    public static class FachadaFuncionariosAutorizacoes
    {

        public static void RemoveAutorizacao(int id)
        {
            Funcionarios.RemoveAutorizacao(id);
        }


        public static bool ExisteAutorizacoes(int idfunc)
        {
            return Funcionarios.ListaAutorizacoesDoFunc(idfunc).Count() > 0;
        }
    }

}