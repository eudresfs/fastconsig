using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.ComponentModel;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;

namespace CP.FastConsig.BLL
{

    public class ODS_Empresa
    {

        private int quantidadeConsignatarias;
        private int quantidadeAgentes;

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<Empresa> SelectGrid(string sortExpression, string nameSearchString, int startRowIndex, int maximumRows)
        {

            Repositorio<Empresa> emp = new Repositorio<Empresa>();

            if (String.IsNullOrWhiteSpace(sortExpression)) sortExpression = "Fantasia";
            if (String.IsNullOrWhiteSpace(nameSearchString)) nameSearchString = string.Empty;
            
            var dados = emp.Listar().Where((x => x.EmpresaTipo.Consignataria && x.Ativo.Equals(1) && (x.Fantasia.Contains(nameSearchString) || x.Sigla.Contains(nameSearchString)) ));
            
            dados = dados.OrderBy(!string.IsNullOrEmpty(sortExpression) ? sortExpression : emp.ChavePrimaria());

            quantidadeConsignatarias = dados.ToList().Count;

            return dados.ListarDaPagina(startRowIndex, maximumRows);

        }

        public int SelectGridCount(string nameSearchString)
        {
            return quantidadeConsignatarias;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<Empresa> SelectGridAgentes(string sortExpression, string nameSearchString, int startRowIndex, int maximumRows, int idConsignataria)
        {

            Repositorio<Empresa> emp = new Repositorio<Empresa>();
            Repositorio<EmpresaVinculo> vinculos = new Repositorio<EmpresaVinculo>();

            if (String.IsNullOrWhiteSpace(sortExpression)) sortExpression = string.Empty;
            if (String.IsNullOrWhiteSpace(nameSearchString)) nameSearchString = string.Empty;

            var dados = vinculos.Listar().Where(x => idConsignataria.Equals(0) ? x.Ativo.Value.Equals(1) : x.IDEmpresa.Equals(idConsignataria)).Select(x => x.Agente).Where(x => x.Ativo.Equals(1) && x.IDEmpresaTipo.Equals((int)Enums.EmpresaTipo.Agente) && (x.Fantasia.Contains(nameSearchString) || x.Sigla.Contains(nameSearchString)));
            
            dados = dados.OrderBy(!string.IsNullOrEmpty(sortExpression) ? sortExpression : emp.ChavePrimaria());

            quantidadeAgentes = dados.ToList().Count;

            return dados.ListarDaPagina(startRowIndex, maximumRows);

        }

        public int SelectGridCountAgentes(string nameSearchString, int idConsignataria)
        {
            return quantidadeAgentes;
        }

    }

}