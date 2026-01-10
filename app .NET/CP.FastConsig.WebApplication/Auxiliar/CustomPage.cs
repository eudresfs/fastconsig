namespace CP.FastConsig.WebApplication.Auxiliar
{

    public class CustomPage : System.Web.UI.Page
    {

        public NewMasterPrincipal PageMaster
        {
            get { return (NewMasterPrincipal)Page.Master; }
        }

        protected GerenciadorSessao Sessao
        {
            get
            {
                return new GerenciadorSessao(Page);
            }
        }

    }

}