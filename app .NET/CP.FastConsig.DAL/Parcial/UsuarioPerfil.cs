using CP.FastConsig.Common;

namespace CP.FastConsig.DAL
{

    public partial class UsuarioPerfil
    {

        public string Modulo
        {
            get { return ((Enums.Modulos) Perfil.IDModulo).ToString(); }
        }

    }

}