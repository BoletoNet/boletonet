using System;
using System.Text.RegularExpressions;
using BoletoNet;
using BoletoNet.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoNet.Testes.BancoBrasil
{
    [TestClass]
    public class BancoBrasilCarteira18Teste
    {
        private Boleto boleto;

        [TestInitialize]
        public void TestInitialize()
        {
            DateTime vencimento = new DateTime(2012, 12, 3);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0131", "7", "00045110", "X")
            {
                Codigo = "1220950"
            };

            var boleto = new Boleto(ListaBancos.BancodoBrasil, vencimento, 8420, "18", "10028528", cedente);

            boleto.NumeroDocumento = "10028528";

            this.boleto = boleto;
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_18_NossoNumero()
        {
            boleto.Cedente.Convenio = 1653;
            boleto.NossoNumero = "1002852";

            boleto.Valida();

            const string NOSSO_NUMERO_VALIDO = "0001002852";

            Assert.AreEqual(NOSSO_NUMERO_VALIDO, boleto.NossoNumero, "Nosso número inválido");
        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public void BancoDoBrasil_Carteira_18_Cedente_Com_Codigo_Menor_Que_Um_Milhao()
        {
            boleto.Cedente.Codigo = "122095";

            boleto.Valida();
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_18_LinhaDigitavel()
        {
            boleto.Cedente.Convenio = 1653205;

            boleto.Valida();

            string linhaDigitavelValida = boleto.CodigoBarra.LinhaDigitavelFormatada;
            Assert.AreEqual(linhaDigitavelValida, boleto.CodigoBarra.LinhaDigitavel, "Linha digitável inválida");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_18_CodigoBarra()
        {
            boleto.Cedente.Convenio = 1653205;

            boleto.Valida();

            var codigoBanco = "001";
            var moeda = "9";
            var fatorVencimento = "5536";
            var valor = Regex.Replace(boleto.ValorBoleto.ToString("f"), @"[,.]", "").PadLeft(10, '0');
            var cedente = "1220950";
            var nn = "0010028528";
            var carteira = "18";

            var codigo = string.Format("{0}{1}{2}{3}000000{4}{5}{6}", codigoBanco, moeda, fatorVencimento, valor, cedente, nn, carteira);
            var modulo11 = codigo.Modulo11(9);
            string codigoBarraValida = string.Format("{0}{1}{2}", codigo.Substring(0, 4), modulo11, codigo.Substring(4));

            Assert.AreEqual(boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido");
        }
    }
}