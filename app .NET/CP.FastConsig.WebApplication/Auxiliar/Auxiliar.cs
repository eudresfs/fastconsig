using System.IO;
using System.Web.UI;

namespace CP.FastConsig.WebApplication.Auxiliar
{

	public class Auxiliar : Page
	{

		#region Constantes

		private const string CaminhoFisicoImagensPerfis = @"{0}Imagens\WebCam\{1}\{2}.jpg";
		private const string CaminhoVirtualImagensPerfis = "~/Imagens/WebCam/{0}/{1}.jpg";

		#endregion

		public Auxiliar(Page pagina)
		{
			Page = pagina;
		}

		public string ObtemPathImagemPerfil(int idUsuario, string pasta)
		{

			string pathImagemPerfil = string.Format(CaminhoFisicoImagensPerfis, Page.Request.PhysicalApplicationPath, pasta, idUsuario);

			if (File.Exists(pathImagemPerfil)) return string.Format(CaminhoVirtualImagensPerfis, pasta, idUsuario);

			return null;

		}

	}

}