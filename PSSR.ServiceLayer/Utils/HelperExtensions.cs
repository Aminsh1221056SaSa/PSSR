using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace PSSR.ServiceLayer.Utils
{
    public static class HelperExtensions
    {
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (action == null) throw new ArgumentNullException("action");
            foreach (var element in source)
            {
                action(element);
            }
        }

        public static T GetNext<T>(this IEnumerable<T> list, T current)
        {
            try
            {
                return list.SkipWhile(x => !x.Equals(current)).Skip(1).First();
            }
            catch
            {
                return default(T);
            }
        }

        public static T GetPrevious<T>(this IEnumerable<T> list, T current)
        {
            try
            {
                return list.TakeWhile(x => !x.Equals(current)).Last();
            }
            catch
            {
                return default(T);
            }
        }
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
         (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static string AbsoluteAction(
        this IUrlHelper url,
        string actionName,
        string controllerName,
        object routeValues = null)
        {
            string scheme = url.ActionContext.HttpContext.Request.Scheme;
            return url.Action(actionName, controllerName, routeValues, scheme);
        }
    }
}
