using System.Linq;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{

    public static class FachadaContatosEdicao
    {

        public static void SalvarContato(EmpresaContato contato)
        {
            Empresas.SalvarContato(contato);
        }

        public static IQueryable<EmpresaContatoPerfil> ListaContatoPerfis()
        {
            return new Repositorio<EmpresaContatoPerfil>().Listar();
        }

        public static IQueryable<ContatoTipo> ListaContatoTipos()
        {
            return new Repositorio<ContatoTipo>().Listar();
        }

        public static EmpresaContato ObtemContato(int idContato)
        {
            return Empresas.ObtemContato(idContato);
        }

    }

}