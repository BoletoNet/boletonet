using System;
using System.Globalization;

namespace BoletoNet
{
    internal class LeitorLinhaPosicao
    {
        public static string ExtrairDaPosicao(string linha, int de, int ate)
        {
            int inicio = de - 1;
            return linha.Substring(inicio, ate - inicio);
        }

        public static int ExtrairInt32DaPosicao(string linha, int de, int ate)
        {
            return int.Parse(ExtrairDaPosicao(linha, de, ate));
        }

        public static long ExtrairInt64DaPosicao(string linha, int de, int ate)
        {
            return long.Parse(ExtrairDaPosicao(linha, de, ate));
        }

        public static DateTime ExtrairDataDaPosicao(string linha, int de, int ate)
        {
            string valor = ExtrairDaPosicao(linha, de, ate);
            return DateTime.ParseExact(valor, "ddMMyyyy", null);
        }

        public static int? ExtrairInt32OpcionalDaPosicao(string linha, int de, int ate)
        {
            string valor = ExtrairDaPosicao(linha, de, ate);
            int aux;
            if (int.TryParse(valor, out aux))
            {
                return aux;
            }
            return null;
        }

        public static DateTime? ExtrairDataOpcionalDaPosicao(string linha, int de, int ate)
        {
            string valor = ExtrairDaPosicao(linha, de, ate);
            DateTime aux;
            if (DateTime.TryParseExact(valor, "ddMMyyyy", null, DateTimeStyles.None, out aux))
            {
                return aux;
            }
            return null;
        }
    }
}
