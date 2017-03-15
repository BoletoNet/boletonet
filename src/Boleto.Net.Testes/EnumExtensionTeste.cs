using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoletoNet;
using BoletoNet.Enums;

namespace Boleto.Net.Testes
{
    [TestClass]
    public class EnumExtensionTeste
    {
        [TestMethod]
        public void Formatar_Codigo_Ocorrencia_Remessa()
        {
            var codigo = TipoOcorrenciaRemessa.EntradaDeTitulos.Format();
            Assert.AreEqual("01", codigo);
        }
    }
}
