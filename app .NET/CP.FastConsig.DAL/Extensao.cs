using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Data.Entity;
using System.Data.Objects;

namespace CP.FastConsig.DAL
{
    public static class Extensao
    {
        public static IQueryable<T> Include<T>(this IQueryable<T> obj, string path)
        {
            if (obj is ObjectQuery<T>)
                return (obj as ObjectQuery<T>).Include(path);
            return obj;
        }

        
        public static IQueryable<T> ListarDaPagina<T>(this IQueryable<T> obj, int pagina, int qtdeporpagina)
        {
            //if (!(obj is IOrderedQueryable<T>))
            //    obj = obj.OrderBy(obj.ChavePrimaria<T>());
            return obj.Skip(pagina).Take(qtdeporpagina);
           
        }

        public static Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>> Pivot<TSource, TFirstKey, TSecondKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TFirstKey> firstKeySelector, Func<TSource, TSecondKey> secondKeySelector, Func<IEnumerable<TSource>, TValue> aggregate)
        {
            var retVal = new Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>>();

            var l = source.ToLookup(firstKeySelector);
            foreach (var item in l)
            {
                var dict = new Dictionary<TSecondKey, TValue>();
                retVal.Add(item.Key, dict);
                var subdict = item.ToLookup(secondKeySelector);
                foreach (var subitem in subdict)
                {
                    dict.Add(subitem.Key, aggregate(subitem));
                }
            }
            return retVal;
        }
    }
}
