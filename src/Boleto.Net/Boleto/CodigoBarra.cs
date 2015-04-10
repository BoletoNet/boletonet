using System;
using System.Drawing;
using System.Globalization;
using BoletoNet.Util;
using Microsoft.VisualBasic;

namespace BoletoNet
{
    public class CodigoBarra
    {
        public CodigoBarra()
        {
            Chave = "";
            LinhaDigitavel = "";
            Imagem = null;
            Codigo = "";
            Moeda = 9;
        }

        /// <summary>
        /// Código de Barra
        /// </summary>
        public string Codigo { get; set; }

        public Image Imagem { get; private set; }

        /// <summary>
        /// Retorna a representação numérica do código de barra
        /// </summary>
        public string LinhaDigitavel { get; set; }

        /// <summary>
        /// Chave para montar Codigo de Barra
        /// </summary>
        public string Chave { get; set; }

        public string CodigoBanco { get; set; }

        public int Moeda { get; set; }
        
        public string CampoLivre { get; set; }
        
        public long FatorVencimento { get; set; }
        
        public string ValorDocumento { get; set; }

        public string DigitoVerificador
        {
            get { return (CodigoBanco + Moeda + FatorVencimento + ValorDocumento + CampoLivre).Modulo11(9); }
        }

        public string LinhaDigitavelFormatada
        {
            get
            {
                var pt1 = (CodigoBanco + Moeda).PadRight(9, '0');
                var mod10 = AbstractBanco.Mod10(pt1);
                pt1 = (pt1 + mod10).Insert(5, ".");

                var substring = CampoLivre.Substring(5);

                var pt2 = substring.Substring(0, 10);
                mod10 = AbstractBanco.Mod10(pt2);
                pt2 = (pt2 + mod10).Insert(5, ".");

                var pt3 = substring.Substring(10);
                mod10 = AbstractBanco.Mod10(pt3);
                pt3 = (pt3 + mod10).Insert(5, ".");

                var pt5 = FatorVencimento + ValorDocumento;
                return string.Join(" ", new[] { pt1, pt2, pt3, DigitoVerificador, pt5 });
            }
        }

        public void PreencheValores(int codigoBanco, int moeda, long fatorVencimento, string valorDocumento, string campoLivre)
        {
            CodigoBanco = Utils.FormatCode(codigoBanco.ToString(), 3);
            Moeda = moeda;
            FatorVencimento = fatorVencimento;
            ValorDocumento = valorDocumento;
            CampoLivre = campoLivre;

            Codigo = string.Format("{0}{1}{2}{3}{4}",
                CodigoBanco,
                Moeda,
                FatorVencimento,
                ValorDocumento,
                CampoLivre);
        }
    }
}
