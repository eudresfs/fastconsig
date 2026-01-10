using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CP.FastConsig.DAL
{
    public class IndiceNegocio
    {
            public int QtdFuncionarios { get; set; }
            public double Valor { get; set; }
            public int QtdAverbacoes { get; set; }
            public string MargemTotalDisp { get; set; }
            public string Tipo { get; set; }
            public double Lucro { get; set; }
            public string DicaValor { get; set; }
            public string DicaLucro { get; set; }

            public IndiceNegocio(int qtdFuncionarios, string margemDisp, double valor, double lucro)
            {
                QtdFuncionarios = qtdFuncionarios;
                MargemTotalDisp = margemDisp;
                Valor = valor;
                Lucro = lucro;
            }

            public IndiceNegocio(string tipo, int qtdAverbacoes, double valor, double lucro, string dicaValor, string dicaLucro)
            {
                Tipo = tipo;
                QtdAverbacoes = qtdAverbacoes;
                Valor = valor;
                Lucro = lucro;
                DicaValor = dicaValor;
                DicaLucro = dicaLucro;
            }        
    }
}
