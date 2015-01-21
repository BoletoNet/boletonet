using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoNet.Testes
{
    [TestClass]
    public class BancoBradescoTeste
    {
        #region Carteira 09

        private BoletoBancario GerarBoletoCarteira09()
        {
            DateTime vencimento = new DateTime(2012, 6, 8);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0539", "8", "0032463", "9");

            Boleto boleto = new Boleto(vencimento, 7620, "09", "18194", cedente);

            boleto.NumeroDocumento = "18194";

            var boletoBancario = new BoletoBancario();

            boletoBancario.CodigoBanco = 237;

            boletoBancario.Boleto = boleto;

            return boletoBancario;
        }

        [TestMethod]
        public void Bradesco_Carteira_09_NossoNumero()
        {
            var boletoBancario = GerarBoletoCarteira09();

            boletoBancario.Boleto.Valida();

            string nossoNumeroValido = "09/00000018194-6";

            Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido");
        }

        [TestMethod]
        public void Bradesco_Carteira_09_LinhaDigitavel()
        {
            var boletoBancario = GerarBoletoCarteira09();

            boletoBancario.Boleto.Valida();

            string linhaDigitavelValida = "23790.53909 90000.001819 94003.246306 3 53580000762000";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
        }

        [TestMethod]
        public void Bradesco_Carteira_09_CodigoBarra()
        {
            var boletoBancario = GerarBoletoCarteira09();

            boletoBancario.Boleto.Valida();

            string codigoBarraValida = "23793535800007620000539090000001819400324630";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido");
        }
        #endregion
    }
}
