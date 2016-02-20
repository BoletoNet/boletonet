using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoNet.Testes
{
    [TestClass]
    public class BancoItauTeste
    {
        #region Carteira 109

        private Boleto GerarBoletoCarteira109()
        {
            DateTime vencimento = new DateTime(2012, 12, 6);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0320", "0", "03200", "6");

            Boleto boleto = new Boleto(ListaBancos.Itau, vencimento, 2701.40m, "109", "20063", cedente);

            boleto.NumeroDocumento = "20063";

            return boleto;
        }

        [TestMethod]
        public void Itau_Carteira_109_NossoNumero()
        {
            var boleto = GerarBoletoCarteira109();

            boleto.Valida();

            string nossoNumeroValido = "109/00020063-7";

            Assert.AreEqual(boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido");
        }

        [TestMethod]
        public void Itau_Carteira_109_LinhaDigitavel()
        {
            var boleto = GerarBoletoCarteira109();

            boleto.Valida();

            string linhaDigitavelValida = "34191.09008 02006.370320 00320.060007 2 55390000270140";

            Assert.AreEqual(boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
        }

        [TestMethod]
        public void Itau_Carteira_109_CodigoBarra()
        {
            var boleto = GerarBoletoCarteira109();

            boleto.Valida();

            string codigoBarraValida = "34192553900002701401090002006370320032006000";

            Assert.AreEqual(boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido");
        }
        #endregion
    }
}
