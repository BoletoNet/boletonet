using System;
using System.Text.RegularExpressions;

namespace BoletoNet
{
    [TipoCarteira(typeof(Banco_Brasil), "18", 1)]
    public class Carteira18Brasil : CarteiraBrasil
    {
        public override void FormataCodigoBarra(Boleto boleto)
        {
            boleto.CodigoBarra.PreencheValores(boleto.Banco.Codigo,
                boleto.Moeda,
                AbstractBanco.FatorVencimento(boleto),
                Regex.Replace(boleto.ValorBoleto.ToString("f"), @"[,.]", "").PadLeft(10, '0'),
                "000000" + boleto.Cedente.Codigo + boleto.NossoNumero + Utils.FormatCode(LimparCarteira(boleto.Carteira), 2));
        }


        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            boleto.CodigoBarra.LinhaDigitavel = boleto.CodigoBarra.LinhaDigitavelFormatada;
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 10);
        }

        public override void ValidaBoleto(Boleto boleto)
        {
            boleto.NossoNumero = boleto.NossoNumero.PadLeft(10, '0');
            if (boleto.Cedente.Codigo.Length != 7)
                throw new Exception("Para a carteira 18, o código do cedente deve ter no mínimo 7 dígitos, ou seja, deve ser maior que 1 milhão");
        }
    }
}
