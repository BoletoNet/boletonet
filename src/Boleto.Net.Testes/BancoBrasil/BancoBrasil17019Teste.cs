using System;
using BoletoNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoNet.Testes.BancoBrasil
{
    [TestClass]
    public class BancoBrasil17019Teste
    {
        #region Carteira 17-019

        private Boleto GerarBoletoCarteira17019()
        {
            DateTime vencimento = new DateTime(2012, 6, 14);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0131", "7", "00059127", "0");

            Boleto boleto = new Boleto(ListaBancos.BancodoBrasil, vencimento, 1700, "17-019", "18204", cedente);

            boleto.NumeroDocumento = "18204";

            return boleto;
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_17019_NossoNumero_ComCodigoConvenio_4Posicoes()
        {
            var boleto = GerarBoletoCarteira17019();

            boleto.Cedente.Convenio = 2379;

            string nossoNumeroValido = "17/23790018204";

            var resultado = boleto.Montar();

            Assert.IsTrue(resultado.Contains(nossoNumeroValido), "Nosso número inválido para 4 posições");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_17019_NossoNumero_ComCodigoConvenio_6Posicoes()
        {
            var boleto = GerarBoletoCarteira17019();

            boleto.Cedente.Convenio = 237966;

            boleto.Valida();

            string nossoNumeroValido = "17/23796618204";

            Assert.AreEqual(boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido para 6 posições");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_17019_NossoNumero_ComCodigoConvenio_7Posicoes()
        {
            var boleto = GerarBoletoCarteira17019();

            boleto.Cedente.Convenio = 2379661;

            boleto.Valida();

            string nossoNumeroValido = "17/23796610000018204";

            Assert.AreEqual(boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido para 7 posições");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_17019_LinhaDigitavel()
        {
            var boleto = GerarBoletoCarteira17019();

            boleto.Cedente.Convenio = 2379661;

            boleto.Valida();

            string linhaDigitavelValida = "00190.00009 02379.661008 00018.204172 9 53640000170000";

            Assert.AreEqual(boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_17019_CodigoBarra()
        {
            var boleto = GerarBoletoCarteira17019();

            boleto.Cedente.Convenio = 2379661;

            string codigoBarraValida = "00199536400001700000000002379661000001820417";

            var resultado = boleto.Montar();

            Assert.AreEqual( codigoBarraValida, boleto.CodigoBarra.Codigo, "Código de Barra inválido");
        }

        #endregion  Carteira 17-019
    }
}
