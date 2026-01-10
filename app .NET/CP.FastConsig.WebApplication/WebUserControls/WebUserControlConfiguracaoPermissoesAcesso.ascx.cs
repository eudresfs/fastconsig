using System;
using CP.FastConsig.Common;
using CP.FastConsig.WebApplication.Auxiliar;
using System.Web.UI.WebControls;
using System.Linq;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using DevExpress.Web.ASPxTreeList;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlConfiguracaoPermissoesAcesso : CustomUserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            //if (ControleCarregado) return;
            
            if (EhPostBack)
            {
                PopularDados();
                return;
            }

            PopularCombos();
            PopularDados();

            EhPostBack = true;
        }

        public override void ExecutaAcoes()
        {
            PopularDados();
        }

        private void PopularCombos()
        {

            DropDownListModulo.DataSource = FachadaPermissoesAcesso.ListarModulos().Where(x => x.IDModulo != (int)Enums.Modulos.Agente).ToList();
            DropDownListConsignataria.DataSource = FachadaConciliacao.ListarConsignatarias().ToList();
            DropDownListConsignataria.DataBind();
            DropDownListModulo.DataBind();

            //DropDownListConsignataria.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, "0"));
            DropDownListConsignataria.SelectedValue = Sessao.IdBanco.ToString();

            DropDownListModulo.SelectedValue = Sessao.IdModulo.ToString();
            DropDownListModulo.Visible = Sessao.IdModulo == (int)Enums.Modulos.Consignante;
            LabelModulo.Visible = DropDownListModulo.Visible;
            btnBuscar.Visible = DropDownListModulo.Visible;
        }

        protected void DropDownListModulo_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (DropDownListModulo.SelectedValue == ((int)Enums.Modulos.Consignataria).ToString())
            {
                tr_consignataria.Visible = true;
                DropDownListConsignataria.Visible = true;
            }
            else
            {
                DropDownListConsignataria.Visible = false;
                tr_consignataria.Visible = false;
            }

        }
        
        protected void Buscar_Click(object sender, EventArgs e)
        {
            PopularDados();            
        }

        private void PopularDados()
        {

            int idmodulo = Convert.ToInt32(DropDownListModulo.SelectedValue);
            int idempresa = (idmodulo == (int)Enums.Modulos.Consignante ? Sessao.IdBanco : Convert.ToInt32(DropDownListConsignataria.SelectedValue));

            var dados = FachadaPermissoesAcesso.MenuArvore(idempresa,idmodulo).Distinct().ToList();

            menu.DataSource = dados;
            menu.DataBind();

            MontarCabecalhoPerfis(idempresa, idmodulo);
        }

        private void MontarCabecalhoPerfis(int idempresa, int idmodulo)
        {

            var perfis = new Repositorio<Perfil>().Listar().Where(x => x.IDModulo == idmodulo && (x.IDEmpresa.Equals(null) || x.IDEmpresa == idempresa)).OrderBy(x => x.IDPerfil).ToList();

            int indice = 0;
            int limite = perfis.Count;

            foreach (TreeListDataColumn coluna in menu.Columns)
            {

                if (coluna.Name.StartsWith("coluna_"))
                {
                    coluna.Visible = true;
                    if (indice < limite)
                    {
                        coluna.Caption = perfis[indice].Nome;
                        coluna.ToolTip = perfis[indice].IDPerfil.ToString();
                    }
                    else
                    {
                        coluna.Visible = false;
                    }

                    indice++;

                }

            }

        }

        public bool GetCheckboxVisibility (object Container)
        {
            TreeListDataCellTemplateContainer cnt = (TreeListDataCellTemplateContainer)Container;
            if (cnt.Column.ToolTip == "1" || cnt.Column.ToolTip == "3")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private int ObtemPerfilDasColunas(string nome)
        {
            return (from TreeListDataColumn coluna in menu.Columns where coluna.Name == nome select Convert.ToInt16(coluna.ToolTip)).FirstOrDefault();
        }

        protected void treeList_CustomDataCallback(object sender, TreeListCustomDataCallbackEventArgs e)
        {
            string[] key = e.Argument.ToString().Split('|');

            string coluna = (key[0]).ToString();
            int idperfil = ObtemPerfilDasColunas("coluna_"+coluna.ToString());

            TreeListNode node = menu.FindNodeByKeyValue(key[2]);

            int idpermissaorecurso = Convert.ToInt32(node.GetValue("idpermissaorecurso"));
            int idrecurso = Convert.ToInt32(node.GetValue("idopcao"));
            int idpermissao = Convert.ToInt32(node.GetValue("idpermissao"));

            int idmodulo = Convert.ToInt32(DropDownListModulo.SelectedValue);
            int idempresa = (idmodulo == (int)Enums.Modulos.Consignante ? Sessao.IdBanco : Convert.ToInt32(DropDownListConsignataria.SelectedValue) > 0 ? Convert.ToInt32(DropDownListConsignataria.SelectedValue) : Sessao.IdBanco);

            bool habilitado = Convert.ToBoolean(key[3]);

            FachadaPermissoesAcesso.SalvarPermissaoAcesso(idpermissaorecurso,habilitado,idrecurso,idempresa,idperfil,idpermissao);
            
        }

        protected void ButtonPerfis_Click(object sender, EventArgs e)
        {
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlPerfis);
        }

    }

}