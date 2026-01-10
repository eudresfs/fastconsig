using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CP.FastConsig.DAL
{
    public class AnaliseProducao
    {           
        public string NomeConsignataria { get; set; }
        public string TipoProduto { get; set; }
        public string Tipo { get; set; }
        public string Competencia { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorConsignado { get; set; }
        public decimal ValorMensal { get; set; }
        public decimal? Percentual { get; set; }
    }
}
