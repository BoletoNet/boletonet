using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoNet.Testes
{
    [TestClass]
    public class BancoBrasilTeste
    {
        #region Carteira 17-019

        private BoletoBancario GerarBoletoCarteira17019()
        {
            DateTime vencimento = new DateTime(2012, 6, 14);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0131", "7", "00059127", "0");

            Boleto boleto = new Boleto(vencimento, 1700, "17-019", "18204", cedente);

            boleto.NumeroDocumento = "18204";

            var boletoBancario = new BoletoBancario();

            boletoBancario.CodigoBanco = 1;

            boletoBancario.Boleto = boleto;

            return boletoBancario;
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_17019_NossoNumero_ComCodigoConvenio_4Posicoes()
        {
            var boletoBancario = GerarBoletoCarteira17019();

            boletoBancario.Cedente.Convenio = 2379;

            boletoBancario.Boleto.Valida();

            string nossoNumeroValido = "17/23790018204";

            Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido para 4 posições");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_17019_NossoNumero_ComCodigoConvenio_6Posicoes()
        {
            var boletoBancario = GerarBoletoCarteira17019();

            boletoBancario.Cedente.Convenio = 237966;

            boletoBancario.Boleto.Valida();

            string nossoNumeroValido = "17/23796618204";

            Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido para 6 posições");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_17019_NossoNumero_ComCodigoConvenio_7Posicoes()
        {
            var boletoBancario = GerarBoletoCarteira17019();

            boletoBancario.Cedente.Convenio = 2379661;

            boletoBancario.Boleto.Valida();

            string nossoNumeroValido = "17/23796610000018204";

            Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido para 7 posições");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_17019_LinhaDigitavel()
        {
            var boletoBancario = GerarBoletoCarteira17019();

            boletoBancario.Cedente.Convenio = 2379661;

            boletoBancario.Boleto.Valida();

            string linhaDigitavelValida = "00190.00009 02379.661008 00018.204172 9 53640000170000";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_17019_CodigoBarra()
        {
            var boletoBancario = GerarBoletoCarteira17019();

            boletoBancario.Cedente.Convenio = 2379661;

            boletoBancario.Boleto.Valida();

            string codigoBarraValida = "00199536400001700000000002379661000001820417";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido");
        }

        #endregion  Carteira 17-019

        #region Carteira 18-019

        private BoletoBancario GerarBoletoCarteira18019()
        {
            DateTime vencimento = new DateTime(2012, 12, 3);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0131", "7", "00045110", "X");

            var boleto = new Boleto(vencimento, 8420, "18-019", "10028528", cedente);

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
