using System;

namespace BoletoNet.Arquivo
{
    internal class TextPosAttribute : Attribute
    {
        public int Start { get; private set; }

        public int Lenght { get; private set; }

        public string Format { get; private set; }

        public TextPosAttribute(int start, int lenght, string format = null)
        {
            Start = start;
            Lenght = lenght;
            Format = format;
        }
    }
}