using System;
using System.Collections.Generic;
using System.Linq;
using BoletoNet.Util;

namespace BoletoNet
{
    public static class BancoCarteiraFactory
    {
        private static readonly List<Tuple<TipoCarteiraAttribute, Type>> carteiras;

        static BancoCarteiraFactory()
        {
            var type = typeof(IBancoCarteira);
            carteiras = type.Assembly.GetTypes()
                .Where(t => !t.IsInterface && !t.IsAbstract)
                .Where(type.IsAssignableFrom)
                .Select(tipo => new Tuple<TipoCarteiraAttribute, Type>(tipo.GetFirstAttribute<TipoCarteiraAttribute>(), tipo))
                .ToList();

        }
        public static IBancoCarteira Fabrica(string carteira, int codBanco)
        {
            var tuple = carteiras.FirstOrDefault(t => t.Item1.Carteira == carteira && t.Item1.CodigoBanco == codBanco);
            return tuple != null ? (IBancoCarteira) Activator.CreateInstance(tuple.Item2) : null;
        }
    }
}