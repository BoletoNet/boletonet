using System;
using BoletoNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Boleto.Net.Testes.BancoBrasil
{
    [TestClass]
    public class BancoBrasil17027Teste
    {
        #region Carteira 17-027

        private BoletoBancario GerarBoletoCarteira17027()
        {
            DateTime vencimento = new DateTime(2012, 6, 14);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0131", "7", "00059127", "0");

            BoletoNet.Boleto boleto = new BoletoNet.Boleto(vencimento, 1700, "17-027", "18204", cedente);

            boleto.NumeroDocumento = "18204";

            var boletoBancario = new BoletoBancario();

            boletoBancario.CodigoBanco = 1;

            boletoBancario.Boleto = boleto;

            return boletoBancario;
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_17027_NossoNumero_ComCodigoConvenio_4Posicoes()
        {
            var boletoBancario = GerarBoletoCarteira17027();

            boletoBancario.Cedente.Convenio = 2379;

            boletoBancario.Boleto.Valida();

            string nossoNumeroValido = "17/23790018204";

            Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido para 4 posições");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_17027_NossoNumero_ComCodigoConvenio_6Posicoes()
        {
            var boletoBancario = GerarBoletoCarteira17027();

            boletoBancario.Cedente.Convenio = 237966;

            boletoBancario.Boleto.Valida();

            string nossoNumeroValido = "17/23796618204";

            Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido para 6 posições");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_17027_NossoNumero_ComCodigoConvenio_7Posicoes()
        {
            var boletoBancario = GerarBoletoCarteira17027();

            boletoBancario.Cedente.Convenio = 2379661;

            boletoBancario.Boleto.Valida();

            string nossoNumeroValido = "17/23796610000018204";

            Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido para 7 posições");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_17027_LinhaDigitavel()
        {
            var boletoBancario = GerarBoletoCarteira17027();

            boletoBancario.Cedente.Convenio = 2379661;

            boletoBancario.Boleto.Valida();

            string linhaDigitavelValida = "00190.00009 02379.661008 00018.204172 9 53640000170000";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_17027_CodigoBarra()
        {
            var boletoBancario = GerarBoletoCarteira17027();

            boletoBancario.Cedente.Convenio = 2379661;

            boletoBancario.Boleto.Valida();

            string codigoBarraValida = "00199536400001700000000002379661000001820417";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido");
        }

        #endregion  Carteira 17-027
    }
}
