using System;

namespace BoletoNet
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TipoCarteiraAttribute : Attribute
    {
        public Type TipoBanco { get; private set; }
        public string Carteira { get; private set; }
        public int CodigoBanco { get; private set; }

        public TipoCarteiraAttribute(Type tipoBanco, string carteira, int codigoBanco)
        {
            TipoBanco = tipoBanco;
            Carteira = carteira;
            CodigoBanco = codigoBanco;
        }
    }
}