using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CP.FastConsig.DAL
{

    public static class Entidades
    {

        public static FastConsigEntities Conexao
        {
            get
            {
                return new FastConsigEntities();
            }
        }

    }

}