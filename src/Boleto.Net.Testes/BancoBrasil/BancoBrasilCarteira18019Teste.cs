using System;
using BoletoNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoNet.Testes.BancoBrasil
{
    [TestClass]
    public class BancoBrasilCarteira18019Teste
    {
        #region Carteira 18-019

        private Boleto GerarBoletoCarteira18019()
        {
            DateTime vencimento = new DateTime(2012, 12, 3);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0131", "7", "00045110", "X");

            var boleto = new Boleto(ListaBancos.BancodoBrasil, vencimento, 8420, "18-019", "10028528", cedente);

            boleto.NumeroDocumento = "10028528";

            return boleto;
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_18019_NossoNumero_ComCodigoConvenio_4Posicoes()
        {
            var boleto = GerarBoletoCarteira18019();

            boleto.Cedente.Convenio = 1653;
            boleto.NossoNumero = "1002852";

            boleto.Valida();

            string nossoNumeroValido = "18/16531002852";

            Assert.AreEqual(boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_18019_NossoNumero_ComCodigoConvenio_6Posicoes()
        {
            var boleto = GerarBoletoCarteira18019();

            boleto.Cedente.Convenio = 1653205;

            boleto.Valida();

            string nossoNumeroValido = "18/16532050010028528";

            Assert.AreEqual(boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_18019_NossoNumero_ComCodigoConvenio_7Posicoes()
        {
            var boleto = GerarBoletoCarteira18019();

            boleto.Cedente.Convenio = 1653205;

            boleto.Valida();

            string nossoNumeroValido = "18/16532050010028528";

            Assert.AreEqual(boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_18019_LinhaDigitavel()
        {
            var boleto = GerarBoletoCarteira18019();

            boleto.Cedente.Convenio = 1653205;

            boleto.Valida();

            string linhaDigitavelValida = "00190.00009 01653.205003 10028.528189 2 55360000842000";

            Assert.AreEqual(boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_18019_CodigoBarra()
        {
            var boleto = GerarBoletoCarteira18019();

            boleto.Cedente.Convenio = 1653205;

            boleto.Valida();

            string codigoBarraValida = "00192553600008420000000001653205001002852818";

            Assert.AreEqual(boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido");
        }

        #endregion Carteira 18-019
    }
}