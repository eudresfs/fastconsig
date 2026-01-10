using System;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;
using CP.FastConsig.WebApplication.Auxiliar;
using System.Web.UI.WebControls;
using CP.FastConsig.Facade;
using System.Linq;
using CP.FastConsig.DAL;

namespace CP.FastConsig.WebApplication.WebUserControls
{

	public partial class WebUserControlSubMenu : CustomUserControl
	{

		#region Constantes
		
		private const string IdControleAsPxButtonMenu = "subMenu";

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{

			if (EhPostBack || ControleCarregado) return;

			ConfiguraComponentes();

			EhPostBack = true;

		}

		private void ConfiguraComponentes()
		{
			ASPxDataViewMenu.DataSource = FachadaSubMenu.ListaMenuOpcoes(IdRecurso, Sessao.IdModulo).ToList();
			ASPxDataViewMenu.DataBind();
		}

		protected void ASPxDataViewMenu_CustomCallback(object sender, CallbackEventArgsBase e)
		{

			int idrecurso = Convert.ToInt32(e.Parameter);

			Recurso recurso = FachadaSubMenu.ObtemRecurso(idrecurso);

			if (recurso.ParametrosAoIniciar != null) PageMaster.CarregaControle(recurso.IDRecurso, 1, recurso.ParametrosAoIniciar);
			else PageMaster.CarregaControle(recurso.IDRecurso);

		}

	}

}