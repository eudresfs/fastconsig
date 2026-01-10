using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;

namespace CP.FastConsig.BLL
{

    public static class FluxoAprovacoes
    {
        public static IQueryable<FluxoAprovacao> ListaFluxoAprovacao(int idprodutogrupo)
        {
            return Geral.ListaFluxoAprovacao(idprodutogrupo);
        }

        public static FluxoAprovacao ObtemFluxoAprovacao(int idprodutogrupo)
        {
            return new Repositorio<FluxoAprovacao>().Listar().FirstOrDefault(x => x.IDProdutoGrupo == idprodutogrupo);
        }

        public static FluxoAprovacaoEmpresa ObtemFluxoAprovacaoEmpresa(int idprodutogrupo, int idempresa)
        {
            return new Repositorio<FluxoAprovacaoEmpresa>().Listar().FirstOrDefault(x => x.IDProdutoGrupo == idprodutogrupo && x.IDEmpresa == idempresa);
        }

        public static void SalvarFluxoAprovacao(int idprodutogrupo, bool bconsignante, bool bfuncionario, bool bconsignataria)
        {
            var rep = new Repositorio<FluxoAprovacao>();
            var dados = rep.Listar().Where(x => x.IDProdutoGrupo == idprodutogrupo);

            var fluxoaprovacao = dados.FirstOrDefault();

            if (fluxoaprovacao == null)
            {
                fluxoaprovacao = new FluxoAprovacao();
                fluxoaprovacao.IDProdutoGrupo = idprodutogrupo;
                rep.Incluir(fluxoaprovacao);
            }
            fluxoaprovacao.RequerAprovacaoConsignante = bconsignante;
            fluxoaprovacao.RequerAprovacaoConsignataria = bconsignataria;
            fluxoaprovacao.RequerAprovacaoFuncionario = bfuncionario;
            rep.Alterar(fluxoaprovacao);
        }

        public static void SalvarFluxoAprovacaoEmpresa(int idprodutogrupo, int idempresa, bool habilitado)
        {
            var rep = new Repositorio<FluxoAprovacaoEmpresa>();
            var dados = rep.Listar().Where(x => x.IDProdutoGrupo == idprodutogrupo && x.IDEmpresa == idempresa);

            var fluxoaprovacao = dados.FirstOrDefault();

            if (fluxoaprovacao == null)
            {
                fluxoaprovacao = new FluxoAprovacaoEmpresa();
                fluxoaprovacao.IDProdutoGrupo = idprodutogrupo;
                fluxoaprovacao.IDEmpresa = idempresa;
                rep.Incluir(fluxoaprovacao);
            }
            fluxoaprovacao.RequerAprovacao = habilitado;
            rep.Alterar(fluxoaprovacao);
        }
    }

}