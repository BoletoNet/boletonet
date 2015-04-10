using System;
using System.Linq;

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
    }
}
