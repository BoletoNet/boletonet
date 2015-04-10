using System;
using BoletoNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Boleto.Net.Testes.BancoBrasil
{
    [TestClass]
    public class BancoBrasilCarteira18019Teste
    {
        #region Carteira 18-019

        private BoletoBancario GerarBoletoCarteira18019()
        {
            DateTime vencimento = new DateTime(2012, 12, 3);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0131", "7", "00045110", "X");

            var boleto = new BoletoNet.Boleto(vencimento, 8420, "18-019", "10028528", cedente);

            boleto.NumeroDocumento = "10028528";

            var boletoBancario = new BoletoBancario();

            boletoBancario.CodigoBanco = 1;

            boletoBancario.Boleto = boleto;

            return boletoBancario;
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_18019_NossoNumero_ComCodigoConvenio_4Posicoes()
        {
            var boletoBancario = GerarBoletoCarteira18019();

            boletoBancario.Cedente.Convenio = 1653;
            boletoBancario.Boleto.NossoNumero = "1002852";

            boletoBancario.Boleto.Valida();

            string nossoNumeroValido = "18/16531002852";

            Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_18019_NossoNumero_ComCodigoConvenio_6Posicoes()
        {
            var boletoBancario = GerarBoletoCarteira18019();

            boletoBancario.Cedente.Convenio = 1653205;

            boletoBancario.Boleto.Valida();

            string nossoNumeroValido = "18/16532050010028528";

            Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_18019_NossoNumero_ComCodigoConvenio_7Posicoes()
        {
            var boletoBancario = GerarBoletoCarteira18019();

            boletoBancario.Cedente.Convenio = 1653205;

            boletoBancario.Boleto.Valida();

            string nossoNumeroValido = "18/16532050010028528";

            Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_18019_LinhaDigitavel()
        {
            var boletoBancario = GerarBoletoCarteira18019();

            boletoBancario.Cedente.Convenio = 1653205;

            boletoBancario.Boleto.Valida();

            string linhaDigitavelValida = "00190.00009 01653.205003 10028.528189 2 55360000842000";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_18019_CodigoBarra()
        {
            var boletoBancario = GerarBoletoCarteira18019();

            boletoBancario.Cedente.Convenio = 1653205;

            boletoBancario.Boleto.Valida();

            string codigoBarraValida = "00192553600008420000000001653205001002852818";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido");
        }

        #endregion Carteira 18-019
    }
}