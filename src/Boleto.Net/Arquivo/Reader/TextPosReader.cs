using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using BoletoNet.Util;

namespace BoletoNet.Arquivo.Reader
{
    public class TextPosReader
    {
        private readonly NumberFormatInfo numberFormat = CultureInfo.GetCultureInfo("pt-BR").NumberFormat;
        public LinhaCbr643 Read(string line)
        {
            var type = typeof(LinhaCbr643);

            var lineObject = GetType().Assembly
                                    .GetTypes()
                                    .Where(t => !t.IsAbstract && type.IsAssignableFrom(t))
                                    .Select(t => (LinhaCbr643)Activator.CreateInstance(t))
                                    .FirstOrDefault(t => t.LinhaCorresponde(line));

            var tuples = lineObject.GetType()
                .GetProperties()
                .Where(p => p.CanWrite)
                .Select(p => new Tuple<PropertyInfo, TextPosAttribute>(p, p.GetFirstAttribute<TextPosAttribute>()))
                .Where(t => t.Item2 != null)
                .ToList();

            foreach (var tuple in tuples)
            {
                var str = line.Substring(tuple.Item2.Start, tuple.Item2.Lenght).Trim();

                if (typeof(DateTime).IsAssignableFrom(tuple.Item1.PropertyType))
                {
                    DateTime data;
                    if (DateTime.TryParseExact(str, tuple.Item2.Format, CultureInfo.CurrentCulture, DateTimeStyles.None, out data))
                        tuple.Item1.SetValue(lineObject, data, null);
                }
                else if (typeof(int).IsAssignableFrom(tuple.Item1.PropertyType))
                    tuple.Item1.SetValue(lineObject, int.Parse(str, numberFormat), null);
                else if (typeof(decimal).IsAssignableFrom(tuple.Item1.PropertyType))
                {
                    str = line.Substring(tuple.Item2.Start, tuple.Item2.Lenght + 2).Trim();
                    str = str.Insert(tuple.Item2.Lenght, ",");
                    tuple.Item1.SetValue(lineObject, decimal.Parse(str, numberFormat), null);
                }
                else
                    tuple.Item1.SetValue(lineObject, str, null);
            }
            return lineObject;
        }
    }
}