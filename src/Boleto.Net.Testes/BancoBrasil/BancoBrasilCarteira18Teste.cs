using System;
using System.Text.RegularExpressions;
using BoletoNet;
using BoletoNet.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Boleto.Net.Testes.BancoBrasil
{
    [TestClass]
    public class BancoBrasilCarteira18Teste
    {
        private BoletoBancario boletoBancario;

        [TestInitialize]
        public void TestInitialize()
        {
            DateTime vencimento = new DateTime(2012, 12, 3);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0131", "7", "00045110", "X")
            {
                Codigo = "1220950"
            };

            var boleto = new BoletoNet.Boleto(vencimento, 8420, "18", "10028528", cedente);

            boleto.NumeroDocumento = "10028528";

            var boletoBancario = new BoletoBancario();

            this.boletoBancario = boletoBancario;
            this.boletoBancario.CodigoBanco = 1;

            this.boletoBancario.Boleto = boleto;
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_18_NossoNumero()
        {
            boletoBancario.Cedente.Convenio = 1653;
            boletoBancario.Boleto.NossoNumero = "1002852";

            boletoBancario.Boleto.Valida();

            const string NOSSO_NUMERO_VALIDO = "0001002852";

            Assert.AreEqual(NOSSO_NUMERO_VALIDO, boletoBancario.Boleto.NossoNumero, "Nosso número inválido");
        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public void BancoDoBrasil_Carteira_18_Cedente_Com_Codigo_Menor_Que_Um_Milhao()
        {
            boletoBancario.Cedente.Codigo = "122095";

            boletoBancario.Boleto.Valida();
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_18_LinhaDigitavel()
        {
            boletoBancario.Cedente.Convenio = 1653205;

            boletoBancario.Boleto.Valida();

            string linhaDigitavelValida = boletoBancario.Boleto.CodigoBarra.LinhaDigitavelFormatada;
            Assert.AreEqual(linhaDigitavelValida, boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, "Linha digitável inválida");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_18_CodigoBarra()
        {
            boletoBancario.Cedente.Convenio = 1653205;

            boletoBancario.Boleto.Valida();

            var codigoBanco = "001";
            var moeda = "9";
            var fatorVencimento = "5536";
            var valor = Regex.Replace(boletoBancario.Boleto.ValorBoleto.ToString("f"), @"[,.]", "").PadLeft(10, '0');
            var cedente = "1220950";
            var nn = "0010028528";
            var carteira = "18";

            var codigo = string.Format("{0}{1}{2}{3}000000{4}{5}{6}", codigoBanco, moeda, fatorVencimento, valor, cedente, nn, carteira);
            var modulo11 = codigo.Modulo11(9);
            string codigoBarraValida = string.Format("{0}{1}{2}", codigo.Substring(0, 4), modulo11, codigo.Substring(4));

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido");
        }
    }
}