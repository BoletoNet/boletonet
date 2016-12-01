using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoletoNet;

namespace Boleto.Net.Testes
{
    [TestClass]
    public class BancoBanestesTeste
    {
        private const int BancoBanestesCodigo = 21;

        private Banco _banco;
        private BoletoNet.Boleto _boleto;

        [TestInitialize]
        public void Initialize()
        {
            _banco = new Banco(BancoBanestesCodigo);

            _boleto = new BoletoNet.Boleto(new DateTime(2014, 1, 18), 2952.95m, "00", "21487805", "123", "1.222.333")
            {
                Banco = _banco,
                NumeroDocumento = "18.030299.01"
            };
        }

        [TestMethod]
        public void NovoBanco_DadoNumeroBanco21_DeveRetornarInstanciaBanco()
        {
            Assert.IsInstanceOfType(_banco, typeof(Banco));
        }

        [TestMethod]
        public void NovoBanco_DadoNumeroBanco21_DeveRetornarNomeBanestes()
        {
            Assert.AreEqual(_banco.Nome, "Banestes");
        }

        [TestMethod]
        public void NovoBanco_DadoNumeroBanco21_DeveRetornarDigito3()
        {
            Assert.AreEqual(_banco.Digito, "3");
        }

        [TestMethod]
        public void Valida_QuandoCriarBoleto_DeveRetornarNossoNumeroEsperado()
        {
            _boleto.Valida();

            Assert.AreEqual(_boleto.NossoNumero, "21487805.81");
        }

        [TestMethod]
        public void Valida_QuandoCriarBoleto_DeveRetornarBancoChaveASBACEEsperado()
        {
            _boleto.Valida();

            Assert.AreEqual(_boleto.Banco.ChaveASBACE, "2148780500001222333202107");
        }

        [TestMethod]
        public void Valida_QuandoCriarBoleto_DeveRetornarCodigoBarraCodigoEsperado()
        {
            _boleto.Valida();

            Assert.AreEqual(_boleto.CodigoBarra.Codigo, "02191594700002952952148780500001222333202107");
        }

        [TestMethod]
        public void Valida_QuandoCriarBoleto_DeveRetornarCodigoBarraLinhaDigitavelEsperado()
        {
            _boleto.Valida();

            Assert.AreEqual(_boleto.CodigoBarra.LinhaDigitavel, "02192.14871 80500.001229 23332.021072 1 59470000295295");
        }
    }
}