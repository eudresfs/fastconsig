using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace CP.FastConsig.DAL
{
    public interface IRepositorio<T> where T : class
    {
        int Incluir(T objeto);
        void Alterar(T objeto);
        void Excluir(T objeto);
        void Excluir(int id);
        string DadosString(T objeto);

        T ObterPorId(int id);
        IQueryable<T> Listar();

        void Atachar(T entity);
    }
}
