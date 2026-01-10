using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CP.FastConsig.DAL
{
    public partial class Funcionario
    {
        public Decimal MargemDisponivelReal (int idprodutogrupo) 
        {
            if (idprodutogrupo > 0)
            {
                ProdutoGrupo pg = new Repositorio<ProdutoGrupo>().ObterPorId(idprodutogrupo);
                decimal margembruta = this.FuncionarioMargem.Where(x => x.ProdutoGrupo.IDProdutoGrupoCompartilha == pg.IDProdutoGrupoCompartilha).Sum(x => x.MargemFolha);
                decimal utilizado = this.Averbacao.Where(w => w.Ativo == 1 && w.Produto.ProdutoGrupo.IDProdutoGrupoCompartilha == pg.IDProdutoGrupoCompartilha && w.AverbacaoSituacao.DeduzMargem).Sum(y => y.ValorDeducaoMargem);
                return margembruta - utilizado;
            }
            else
                return -999999;
        }

        public Decimal MargemDisponivel(int idprodutogrupo)
        {
            decimal margemdisp = MargemDisponivelReal(idprodutogrupo);
            //if (margemdisp < 0)
            //    margemdisp = 0;
            return margemdisp;
        }

        public Decimal MargemFolha(int idprodutogrupo)
        {
            if (idprodutogrupo > 0)
            {
                ProdutoGrupo pg = new Repositorio<ProdutoGrupo>().ObterPorId(idprodutogrupo);
                decimal margembruta = this.FuncionarioMargem.Where(x => x.ProdutoGrupo.IDProdutoGrupoCompartilha == pg.IDProdutoGrupoCompartilha).Sum(x => x.MargemFolha);
                return margembruta;
            }
            else
                return 0;
        }

        public Decimal MargemDisponivel1 { get { return MargemDisponivel(1); } }
        public Decimal MargemDisponivel2 { get { return MargemDisponivel(2); } }
        public Decimal MargemDisponivel3 { get { return MargemDisponivel(3); } }
        public Decimal MargemDisponivel4 { get { return MargemDisponivel(4); } }
        public Decimal MargemDisponivel5 { get { return MargemDisponivel(5); } }
        public Decimal MargemDisponivel6 { get { return MargemDisponivel(6); } }
        public Decimal MargemDisponivel7 { get { return MargemDisponivel(7); } }
        public Decimal MargemDisponivel8 { get { return MargemDisponivel(8); } }
        public Decimal MargemDisponivel9 { get { return MargemDisponivel(9); } }

        public string NomeSituacao
        {
            get
            {
                if (this.FuncionarioSituacao != null)
                    return this.NomeSituacaoFolha + " (" + this.FuncionarioSituacao.Nome + ")";
                else
                    return this.NomeSituacaoFolha + " (Não Definido)";
            }
        }
    }
}
