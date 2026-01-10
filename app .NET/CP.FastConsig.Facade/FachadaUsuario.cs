using CP.FastConsig.BLL;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{

    public static class FachadaUsuario
    {

        public static Usuario ObtemUsuario(int idUsuario)
        {
            return Usuarios.ObtemUsuario(idUsuario);
        }

        public static void AtualizaSenha(int idUsuario, string senha)
        {
            Usuarios.AtualizaSenha(idUsuario, senha);
        }
    }

}