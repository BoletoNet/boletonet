using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoletoNet;

namespace Boleto.Net.Testes
{
    [TestClass]
    public class Banco_BanestesTeste
    {
        [TestMethod]
        public void Valida_Numero_Banco_Banestes()
        {
            var banco = new Banco(21);

            Assert.IsInstanceOfType(banco, typeof(Banco));
        }

        [TestMethod]
        public void Valida_Boleto_Banco_Banestes()
        {
            var boleto = new BoletoNet.Boleto(new DateTime(2014, 1, 18), 2952.95m, "00", "21487805", "123", "1.222.333");

            boleto.Banco = new Banco(21);
            boleto.NumeroDocumento = "18.030299.01";

            boleto.Valida();

            Assert.AreEqual(boleto.NossoNumero, "21487805.81");

            Assert.AreEqual(boleto.Banco.ChaveASBACE, "2148780500001222333202107");

            Assert.AreEqual(boleto.CodigoBarra.Codigo, "02191594700002952952148780500001222333202107");

            Assert.AreEqual(boleto.CodigoBarra.LinhaDigitavel, "02192.14871 80500.001229 23332.021072 1 59470000295295");

        }

    }
}
