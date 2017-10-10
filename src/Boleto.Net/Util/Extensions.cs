using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Threading;
using System.Globalization;

namespace BoletoNet.Util
{
    public static class Extensions
    {
        public static string Modulo11(this string str, int @base)
        {
            var fats = Enumerable.Repeat(Enumerable.Range(2, @base - 1), 10).SelectMany(x => x).Take(str.Length);
            var mod = 11 - str.Reverse().Zip(fats, (x, a) => (char.GetNumericValue(x) * a)).Sum() % 11;
            return mod > 9 || mod <= 1 ? "1" : mod.ToString().Substring(0, 1);
        }

        public static T GetFirstAttribute<T>(this Type type)
        {
            return (T)type.GetCustomAttributes(typeof(T), false).FirstOrDefault();
        }
        public static T GetFirstAttribute<T>(this MemberInfo memberInfo)
        {
            return (T)memberInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault();
        }


        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");
            return ZipIterator(first, second, resultSelector);
        }

        static IEnumerable<TResult> ZipIterator<TFirst, TSecond, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            using (IEnumerator<TFirst> e1 = first.GetEnumerator())
            using (IEnumerator<TSecond> e2 = second.GetEnumerator())
                while (e1.MoveNext() && e2.MoveNext())
                    yield return resultSelector(e1.Current, e2.Current);
        }

        /// <summary>
        /// Retorna o valor atual removendo a vírgula
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static string ApenasNumeros(this decimal valor)
        {
            return valor.ToString("0.00", CultureInfo.GetCultureInfo("pt-BR")).Replace(",", "");
        }

        /// <summary>
        /// Retorna o valor atual removendo a vírgula
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static string ApenasNumeros(this decimal? valor)
        {
            if (valor != null)
            {
                return valor.Value.ToString("0.00", CultureInfo.GetCultureInfo("pt-BR")).Replace(",", "");
            }

            return string.Empty;
        }
        
    }

    /// <summary>
    /// Classe com Métodos de Extensão para strings atendendo o padrão do Visual Basic (Substituindo a necessidade de importar a biblioteca Microsoft.VisualBasic)
    /// </summary>
    public static class Strings
    {
        public static string Mid(this string str, int start, int? length = null)
        {
            if (!length.HasValue)
                return str.Substring(start - 1);
            else
                return str.Substring(start -1, length.Value);
        }

        public static string Left(this string s, int length)
        {
            return s.Substring(0, length);
        }

        public static string Right(this string str, int length)
        {
            return str.Substring(length >= str.Length ? 0 : str.Length - length);
        }

    }
}
