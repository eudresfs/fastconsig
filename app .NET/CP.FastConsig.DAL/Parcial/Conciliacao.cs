using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CP.FastConsig.DAL
{
    public partial class Conciliacao
    {
        public decimal ValorCalculo {
            get { if (ValorDescontado > 0) { return ValorDescontado; } else { return Valor; }; }
        }
    }
}
