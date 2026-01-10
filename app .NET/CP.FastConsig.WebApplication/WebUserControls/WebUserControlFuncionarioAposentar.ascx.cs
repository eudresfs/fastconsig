using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.DAL;
using DevExpress.Web.ASPxGridView;
using CP.FastConsig.Common;
using CP.FastConsig.Facade;

namespace CP.FastConsig.WebApplication.WebUserControls
{
    public partial class WebUserControlFuncionarioAposentar : CustomUserControl
    {
        const string ParametroIdFuncAposenta = "IdFuncAposenta";
        const string ParametroIdFunc = "IdFunc";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ControleCarregado) return;

            if (!EhPostBack)
            {
                EhPostBack = true;
                PopulaDados();
            }
        }

        private int IdFuncAposenta
        {
            get
            {
                if (ViewState[ParametroIdFuncAposenta] == null) ViewState[ParametroIdFuncAposenta] = 0;
                return (int)ViewState[ParametroIdFuncAposenta];
            }
            set
            {
                ViewState[ParametroIdFuncAposenta] = value;
            }
        }

        private int IdFunc
        {
            get
            {
                if (ViewState[ParametroIdFunc] == null) ViewState[ParametroIdFunc] = 0;
                return (int)ViewState[ParametroIdFunc];
            }
            set
            {
                ViewState[ParametroIdFunc] = value;
            }
        }

        public void PopulaDados()
        {
            var dados = new Repositorio<FuncionarioAposenta>().Listar().Where(x => !x.Importado.HasValue || !x.Importado.Value).Select(y => new {y.Data, y.IDFuncionario, y.Matricula, y.Nome, y.CPF}).Distinct();
            gridAposentar.DataSource = dados.ToList();
            gridAposentar.DataBind();
        }

        protected void gridAposentar_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.KeyValue);

            var dados = new Repositorio<FuncionarioAposenta>().Listar().Where(x => x.IDFuncionario == id);

            // verificando se o funcionario existe na base da previdencia
            var funcprev = FachadaFuncionariosConsulta.ObtemFuncionario(dados.FirstOrDefault().CPF);

            if (funcprev == null)
            {
                PageMaster.ExibeMensagem("Este funcionário ainda não possui cadastro no portal de consignações FastConsig! É necessário atualização por arquivo.");
                return;
            }

            IdFunc = funcprev.IDFuncionario;

            gridAposentar.Visible = false;
            panelFunc.Visible = true;

            FuncionarioAposenta fun = dados.FirstOrDefault();

            if (fun != null)
            {
                ASPxRoundPanelDadosFunc.Visible = true;
                LabelMatriculaFuncionario.Text = fun.Matricula;
                LabelNomeFuncionario.Text = fun.Nome;
                LabelCpfFuncionario.Text = fun.CPF;
            }

            gridAverbacoes.DataSource = dados.ToList();
            gridAverbacoes.DataBind();
        }

        protected void gridAverbacoes_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.KeyValue);
            IdFuncAposenta = id;

            var dados = new Repositorio<FuncionarioAposenta>().Listar().Where(x => x.IDFuncionarioAposenta == id);
            FuncionarioAposenta fun = dados.FirstOrDefault();

            if (fun != null)
            {
                ASPxRoundPanelDadosFunc.Visible = true;
                ButtonCancelar.Visible = true;
                ButtonSalvar.Visible = true;

                LabelNumero.Text = fun.Numero;
                LabelConsignataria.Text = fun.NomeConsignataria;
                LabelValorParcela.Text = fun.ValorParcela.HasValue ? fun.ValorParcela.Value.ToString() : "";
                LabelPrazo.Text = fun.Prazo.HasValue ? fun.Prazo.Value.ToString() : "";

                cmbConsignataria.DataSource = FachadaConsignatarias.ListaConsignatarias().ToList();
                cmbConsignataria.DataBind();
            }

            panelFunc.Visible = false;
            panelAverbacao.Visible = true;
        }

        protected void SelecionouConsignataria(object sender, EventArgs e)
        {
            if (cmbConsignataria.SelectedItem != null)
            {
                cmbProduto.DataSource = FachadaConsignatarias.Produtos(Convert.ToInt32(cmbConsignataria.SelectedValue)).ToList();
                cmbProduto.DataBind();
            }
        }

        protected void ButtonSalvarClick(object sender, EventArgs e)
        {
            if (cmbConsignataria.SelectedItem == null || cmbProduto.SelectedItem == null)
            {
                PageMaster.ExibeMensagem("Consignatária ou Verba não foi definido!");
                return;
            }


            Repositorio<FuncionarioAposenta> repaposenta = new Repositorio<FuncionarioAposenta>();
            var dados = repaposenta.Listar().Where(x => x.IDFuncionarioAposenta == IdFuncAposenta);
            FuncionarioAposenta aposenta = dados.FirstOrDefault();

            Averbacao a = new Averbacao();
            a.IDAverbacaoTipo = (int)Enums.AverbacaoTipo.Normal;
            a.IDAverbacaoSituacao = (int)Enums.AverbacaoSituacao.Reservado;
            a.IDFuncionario = IdFunc;
            a.IDConsignataria = Convert.ToInt32(cmbConsignataria.SelectedValue);
            a.IDProduto = Convert.ToInt32(cmbProduto.SelectedValue);
            a.ValorParcela = aposenta.ValorParcela.Value;
            a.ValorContratado = aposenta.ValorContrato.Value;
            a.Prazo = aposenta.Prazo.Value;
            a.Numero = aposenta.Numero;
            a.Identificador = aposenta.Identificador;
            a.CompetenciaInicial = aposenta.CompetenciaInicial;
            a.CompetenciaFinal = aposenta.CompetenciaFinal;
            a.Data = aposenta.Data.Value;
            a.IDUsuario = Sessao.IdUsuario;

            FachadaAverbacoes.IncluirAverbacao(a);

            aposenta.Importado = true;
            repaposenta.Alterar(aposenta);

            EstadoInicial();
            PageMaster.ExibeMensagem("Importação realizada com Sucesso!");
        }

        protected void ButtonCancelarClick(object sender, EventArgs e)
        {
            EstadoInicial();
        }

        private void EstadoInicial()
        {
            panelAverbacao.Visible = false;
            panelFunc.Visible = false;
            ASPxRoundPanelDadosFunc.Visible = false;
            ButtonCancelar.Visible = false;
            ButtonSalvar.Visible = false;
            gridAposentar.Visible = true;
            PopulaDados();
        }
    }
}