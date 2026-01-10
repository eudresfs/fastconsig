using System.Linq;

namespace CP.FastConsig.DAL
{
    public partial class Usuario
    {

        public string Perfil
        {
            get
            {

                if (UsuarioPerfil.Count > 1) return "Múltiplos";

                UsuarioPerfil usuarioPerfil = UsuarioPerfil.FirstOrDefault();

                return usuarioPerfil == null ? string.Empty : usuarioPerfil.Perfil.Nome;

            }
        }

        public string Nome { get { return this.NomeCompleto; } }

    }

}