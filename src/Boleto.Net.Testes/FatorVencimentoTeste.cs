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
                DataVencimento = new DateTime(2023, 1, 18, 0, 0, 0)
            });

            Assert.AreEqual(382, fatorVencimento);
        }

        [TestMethod]
        public void FatorVencimento_12_03_2014()
        {
            var fatorVencimento = AbstractBanco.FatorVencimento(new BoletoNet.Boleto
            {
                DataVencimento = new DateTime(2025, 3, 12, 0, 0, 0)
            });

            Assert.AreEqual(1166, fatorVencimento);
        }

        [TestMethod]
        public void FatorVencimento_21_02_2025()
        {
            var fatorVencimento = AbstractBanco.FatorVencimento(new BoletoNet.Boleto
            {
                DataVencimento = new DateTime(2025, 2, 21, 0, 0, 0)
            });

            Assert.AreEqual(1147, fatorVencimento);
        }

        [TestMethod]
        public void FatorVencimento_22_02_2025()
        {
            var fatorVencimento = AbstractBanco.FatorVencimento(new BoletoNet.Boleto
            {
                DataVencimento = new DateTime(2025, 2, 22, 0, 0, 0)
            });

            Assert.AreEqual(1148, fatorVencimento);
        }

        [TestMethod]
        public void FatorVencimento_23_02_2025()
        {
            var fatorVencimento = AbstractBanco.FatorVencimento(new BoletoNet.Boleto
            {
                DataVencimento = new DateTime(2025, 2, 23, 0, 0, 0)
            });

            Assert.AreEqual(1149, fatorVencimento);
        }
    }
}
