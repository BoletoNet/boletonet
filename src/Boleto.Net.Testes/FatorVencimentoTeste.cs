using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoletoNet;

namespace Boleto.Net.Testes
{
    [TestClass]
    public class FatorVencimentoTeste
    {
        [TestMethod]
        public void FatorVencimento_18_01_2014()
        {
            var fatorVencimento = AbstractBanco.FatorVencimento(new BoletoNet.Boleto
            {
                DataVencimento = new DateTime(2014, 1, 18, 0, 0, 0)
            });

            Assert.AreEqual(5947, fatorVencimento);
        }

        [TestMethod]
        public void FatorVencimento_12_03_2014()
        {
            var fatorVencimento = AbstractBanco.FatorVencimento(new BoletoNet.Boleto
            {
                DataVencimento = new DateTime(2014, 3, 12, 0, 0, 0)
            });

            Assert.AreEqual(6000, fatorVencimento);
        }

        [TestMethod]
        public void FatorVencimento_21_02_2025()
        {
            var fatorVencimento = AbstractBanco.FatorVencimento(new BoletoNet.Boleto
            {
                DataVencimento = new DateTime(2025, 2, 21, 0, 0, 0)
            });

            Assert.AreEqual(9999, fatorVencimento);
        }

        [TestMethod]
        public void FatorVencimento_22_02_2025()
        {
            var fatorVencimento = AbstractBanco.FatorVencimento(new BoletoNet.Boleto
            {
                DataVencimento = new DateTime(2025, 2, 22, 0, 0, 0)
            });

            Assert.AreEqual(1000, fatorVencimento);
        }

        [TestMethod]
        public void FatorVencimento_23_02_2025()
        {
            var fatorVencimento = AbstractBanco.FatorVencimento(new BoletoNet.Boleto
            {
                DataVencimento = new DateTime(2025, 2, 23, 0, 0, 0)
            });

            Assert.AreEqual(1001, fatorVencimento);
        }
    }
}
