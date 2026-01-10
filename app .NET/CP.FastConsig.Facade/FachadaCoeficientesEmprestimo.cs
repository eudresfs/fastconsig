using System;
using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.BLL;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{

    public static class FachadaCoeficientesEmprestimo
    {

        public static bool ExisteCoeficienteValido(int idBanco, DateTime dataInicioVigencia, int validade)
        {
            return new Repositorio<EmpresaCoeficiente>().Listar().Where(x => x.IDEmpresa.Equals(idBanco) && x.IDProdutoGrupo.Equals((int)Enums.ProdutoGrupo.Emprestimos)).ToList().Any(x => ValidadeDentroPrazo(x, dataInicioVigencia, validade));
        }

        private static bool ValidadeDentroPrazo(EmpresaCoeficiente empresaCoeficiente, DateTime dataInicioVigencia, int validade)
        {

            DateTime novoPrazoInicial = dataInicioVigencia;
            DateTime novoPrazoFinal = dataInicioVigencia.AddDays(validade);

            DateTime antigoPrazoInicial = empresaCoeficiente.InicioVigencia;
            DateTime antigoPrazoFinal = empresaCoeficiente.InicioVigencia.AddDays(empresaCoeficiente.ValidadeDias.Value);

            return (novoPrazoFinal >= antigoPrazoInicial && novoPrazoFinal <= antigoPrazoFinal) || (novoPrazoInicial >= antigoPrazoInicial && novoPrazoInicial <= antigoPrazoFinal);

        }

        public static int Salva(int carencia, int validade, int idBanco, DateTime dataInicioVigencia)
        {
            
            Repositorio<EmpresaCoeficiente> repositorio = new Repositorio<EmpresaCoeficiente>();

            EmpresaCoeficiente empresaCoeficiente = repositorio.Listar().Where(x => x.IDEmpresa.Equals(idBanco) && x.IDProdutoGrupo.Equals((int)Enums.ProdutoGrupo.Emprestimos)).ToList().FirstOrDefault(x => ValidadeDentroPrazo(x, dataInicioVigencia, validade));

            bool novo = empresaCoeficiente == null;

            if (novo)
            {
                empresaCoeficiente = new EmpresaCoeficiente();
            }
            else
            {

                Repositorio<EmpresaCoeficienteDetalhe> repositorioCoeficienteDetalhes = new Repositorio<EmpresaCoeficienteDetalhe>();
                List<EmpresaCoeficienteDetalhe> detalhesParaExcluir = repositorioCoeficienteDetalhes.Listar().Where(x => x.IDEmpresaCoeficiente.Equals(empresaCoeficiente.IDEmpresaCoeficiente)).ToList();

                foreach (EmpresaCoeficienteDetalhe detalhe in detalhesParaExcluir) repositorioCoeficienteDetalhes.Excluir(detalhe.IDEmpresaCoeficienteDetalhe);

            }

            empresaCoeficiente.Data = DateTime.Today;
            empresaCoeficiente.Carencia = Convert.ToInt32(carencia);
            empresaCoeficiente.ValidadeDias = Convert.ToInt32(validade);
            empresaCoeficiente.IDEmpresa = idBanco;
            empresaCoeficiente.IDProdutoGrupo = (int)Enums.ProdutoGrupo.Emprestimos;
            empresaCoeficiente.InicioVigencia = dataInicioVigencia;

            if (novo) repositorio.Incluir(empresaCoeficiente);
            else repositorio.Alterar(empresaCoeficiente);

            return empresaCoeficiente.IDEmpresaCoeficiente;

        }

        public static IQueryable<EmpresaCoeficiente> ObtemCoeficientesEmpresa(int idBanco)
        {
            return Empresas.ObtemCoeficientes(idBanco);
        }

        public static void RemoveCoeficiente(int idCoeficiente)
        {
            Empresas.RemoveCoeficiente(idCoeficiente);
        }

        public static IEnumerable<EmpresaCoeficienteDetalhe> ObtemCoeficientesDetalhesCoeficiente(int idCoeficiente)
        {
            return Empresas.ObtemCoeficientesDetalhesCoeficiente(idCoeficiente);
        }

    }

}