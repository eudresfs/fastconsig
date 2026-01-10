using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace CP.FastConsig.DAL
{
    public static class Contexto
    {
        public static ObjectContext ObtemContexto()
        {
            DbContext contexto = new FastConsigEntities();
            ObjectContext objContext = ((IObjectContextAdapter)contexto).ObjectContext;

            return objContext;
        }
    }
}
