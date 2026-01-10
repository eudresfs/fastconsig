using System.Linq;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{

    public static class FachadaPerfisEdicao
    {

        public static void Salvar(Perfil perfil)
        {
            Empresas.SalvarPerfil(perfil);
        }

        public static Perfil ObtemPerfil(int idPerfil)
        {
            return Empresas.ObtemPerfil(idPerfil);
        }


        public static IQueryable<Perfil> Listar(int idempresa, int idmodulo)
        {
            return Perfis.Listar(idempresa, idmodulo);
        }

        public static void CopiarPerfil(int IdEmpresa, int De, int Para)
        {
            Perfis.CopiarPerfil(IdEmpresa, De, Para);
        }
    }

}