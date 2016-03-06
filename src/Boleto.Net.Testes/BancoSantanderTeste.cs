using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoNet.Testes
{
    [TestClass]
    public class BancoSantanderTeste
    {
        #region Carteira 101

        private Boleto GerarBoletoCarteira101()
        {
            DateTime vencimento = new DateTime(2012, 12, 4);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "3265", "3309894", "4");

            cedente.Codigo = "3309894";

            Boleto boleto = new Boleto(ListaBancos.Santander,vencimento, 2701.40m, "101", "000000020061", cedente);

            boleto.NumeroDocumento = "20061";

            return boleto;
        }

        [TestMethod]
        public void Santander_Carteira_101_NossoNumero()
        {
            var boleto = GerarBoletoCarteira101();

            boleto.Valida();

            string nossoNumeroValido = "000000020061-1";

            Assert.AreEqual(boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido");
        }

        [TestMethod]
        public void Santander_Carteira_101_LinhaDigitavel()
        {
            var boleto = GerarBoletoCarteira101();

            boleto.Valida();

            string linhaDigitavelValida = "03399.33095 89400.000009 20061.101018 1 55370000270140";

            Assert.AreEqual(boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
        }

        //[TestMethod]
        public void Santander_Carteira_101_CodigoBarra()
        {
            var boleto = GerarBoletoCarteira101();

            boleto.Valida();

            string codigoBarraValida = "03391553700002701409330989400000002006110101";

            Assert.AreEqual(boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido");
        }
        #endregion

        #region Carteira 102
        private Boleto GerarBoletoCarteira101_Com_IOS_8()
        {
            DateTime vencimento = new DateTime(2013, 4, 23);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "3719", "", "") { Codigo = "3780457" };

            Boleto boleto = new Boleto(ListaBancos.Santander, vencimento, 1052.50m, "101", "000000252084", cedente);

            boleto.PercentualIOS = 8;

            boleto.DataProcessamento = new DateTime(2013, 4, 15);

            //boleto.NumeroDocumento = "20061";
            return boleto;
        }

        [TestMethod]
        public void Santander_Carteira_101_LinhaDigitavel_Com_IOS_8()
        {
            var boleto = GerarBoletoCarteira101_Com_IOS_8();

            boleto.Valida();

            string linhaDigitavelValida = "03399.37807 45700.000024 52084.281014 8 56770000105250";

            Assert.AreEqual(boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
        }

        [TestMethod]
        public void Santander_Carteira_101_CodigoBarra_Com_IOS_8()
        {
            var boleto = GerarBoletoCarteira101_Com_IOS_8();

            boleto.Valida();

            string codigoBarra = "03398567700001052509378045700000025208428101";

            Assert.AreEqual(boleto.CodigoBarra.Codigo, codigoBarra, "Código de Barra inválido");
        }

        [TestMethod]
        public void Santander_Mod11_Com_IOS_6()
        {
            int dv = AbstractBanco.Mod11("0339204600000273719028203356661245780020102", 9, 0);

            Assert.AreEqual(dv, 6, "Linha digitável inválida");
        }

        [TestMethod]
        public void Santander_Mod11_Com_IOS_8()
        {
            int dv = AbstractBanco.Mod11("0339567700001052509378045700000025208428101", 9, 0);

            Assert.AreEqual(dv, 8, "Linha digitável inválida");
        }

        #endregion
    }

    
}
