using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CP.FastConsig.DAL
{
    public class ConciliacaoResultadoConciliacao
    {
        public string NomeConsignataria { get; set; }
        public string Competencia { get; set; }
        public string Descricao { get; set; }
        public decimal? Valor { get; set; }
    }
}
