using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoNet.Testes
{
    [TestClass]
    public class BancoBradescoTeste
    {
        #region Carteira 16

        private Boleto GerarBoletoCarteira16()
        {
            var vencimento = new DateTime(2015, 7, 20);
            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0413", "8", "0002916", "5");
            var boleto = new Boleto(ListaBancos.Bradesco, vencimento, 123, "16", "00970171092", cedente);
            boleto.NumeroDocumento = "970171092";

            return boleto;
        }

        private Boleto GerarBoletoCarteira16_Boleto()
        {
            var vencimento = new DateTime(2015, 7, 20);
            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0413", "8", "0002916", "5");
            var boleto = new Boleto(ListaBancos.Bradesco, vencimento, 123, "16", "00970171092", cedente);
            boleto.NumeroDocumento = "970171092";

            return boleto;
        }

        [TestMethod]
        public void Bradesco_Carteira_16_NossoNumero()
        {
            var boleto = GerarBoletoCarteira16();

            boleto.Valida();

            string nossoNumeroValido = "16/00970171092-1";

            Assert.AreEqual(boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido");
        }

        [TestMethod]
        public void Bradesco_Carteira_16_LinhaDigitavel()
        {
            var boleto = GerarBoletoCarteira16();

            boleto.Valida();

            string linhaDigitavelValida = "23790.41318 60097.017101 92000.291606 2 64950000012300";

            Assert.AreEqual(boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
        }

        [TestMethod]
        public void Bradesco_Carteira_16_CodigoBarra()
        {
            var boleto = GerarBoletoCarteira16();

            boleto.Valida();

            string codigoBarraValida = "23792649500000123000413160097017109200029160";

            Assert.AreEqual(boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido");
        }

        #endregion

        #region Carteira 09

        private Boleto GerarBoletoCarteira09()
        {
            DateTime vencimento = new DateTime(2012, 6, 8);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0539", "8", "0032463", "9");

            Boleto boleto = new Boleto(ListaBancos.Bradesco, vencimento, 7620, "09", "18194", cedente);

            boleto.NumeroDocumento = "18194";

            return boleto;
        }

        [TestMethod]
        public void Bradesco_Carteira_09_NossoNumero()
        {
            var boletoBancario = GerarBoletoCarteira09();

            boletoBancario.Valida();

            string nossoNumeroValido = "09/00000018194-6";

            Assert.AreEqual(boletoBancario.NossoNumero, nossoNumeroValido, "Nosso número inválido");
        }

        [TestMethod]
        public void Bradesco_Carteira_09_LinhaDigitavel()
        {
            var boletoBancario = GerarBoletoCarteira09();

            boletoBancario.Valida();

            string linhaDigitavelValida = "23790.53909 90000.001819 94003.246306 3 53580000762000";

            Assert.AreEqual(boletoBancario.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
        }

        [TestMethod]
        public void Bradesco_Carteira_09_CodigoBarra()
        {
            var boletoBancario = GerarBoletoCarteira09();

            boletoBancario.Valida();

            string codigoBarraValida = "23793535800007620000539090000001819400324630";

            Assert.AreEqual(boletoBancario.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido");
        }
        #endregion

        #region Carteira 25

        private Boleto GerarBoletoCarteira25()
        {
            DateTime vencimento = new DateTime( 2015, 10, 21 );

            var cedente = new Cedente( "00.000.000/0000-00", "Empresa Teste", "054", "0", "148870", "8" );

            Boleto boleto = new Boleto(ListaBancos.Bradesco, vencimento,(decimal)469.4, "25", "97000005287", cedente );

            boleto.NumeroDocumento = "5..287";

            return boleto;
        }

        [TestMethod]
        public void Bradesco_Carteira_25_NossoNumero()
        {
            var boleto = GerarBoletoCarteira25();

            boleto.Valida();

            string nossoNumeroValido = "25/97000005287-P";

            Assert.AreEqual( boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido" );
        }

        [TestMethod]
        public void Bradesco_Carteira_25_LinhaDigitavel()
        {
            var boleto = GerarBoletoCarteira25();

            boleto.Valida();

            string linhaDigitavelValida = "23790.05420 59700.000520 87014.887001 1 65880000046940";

            Assert.AreEqual( boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida" );
        }

        [TestMethod]
        public void Bradesco_Carteira_25_CodigoBarra()
        {
            var boleto = GerarBoletoCarteira25();

            boleto.Valida();

            string codigoBarraValida = "23791658800000469400054259700000528701488700";

            Assert.AreEqual( boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido" );
        }
        #endregion

        #region Carteira 26

        private Boleto GerarBoletoCarteira26()
        {
            DateTime vencimento = new DateTime(2015, 10, 21);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "054", "0", "148870", "8");

            Boleto boleto = new Boleto(ListaBancos.Bradesco, vencimento, (decimal)469.4, "26", "97000005287", cedente);

            boleto.NumeroDocumento = "5..287";

            return boleto;
        }

        [TestMethod]
        public void Bradesco_Carteira_26_NossoNumero()
        {
            var boleto = GerarBoletoCarteira26();

            boleto.Valida();

            string nossoNumeroValido = "26/97000005287-3";

            Assert.AreEqual(boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido");
        }

        [TestMethod]
        public void Bradesco_Carteira_26_LinhaDigitavel()
        {
            var boleto = GerarBoletoCarteira26();

            boleto.Valida();

            string linhaDigitavelValida = "23790.05420 69700.000529 87014.887001 5 65880000046940";

            Assert.AreEqual(boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
        }

        [TestMethod]
        public void Bradesco_Carteira_26_CodigoBarra()
        {
            var boleto = GerarBoletoCarteira26();

            boleto.Valida();

            string codigoBarraValida = "23795658800000469400054269700000528701488700";

            Assert.AreEqual(boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido");
        }
        #endregion
    }
}