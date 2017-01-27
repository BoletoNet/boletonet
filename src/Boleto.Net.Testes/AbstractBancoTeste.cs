using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoNet.Testes
{
    [TestClass]
    public class AbstractBancoTeste
    {
        /// <summary>
        /// Extende AbstractBanco para expor métodos internos para teste.
        /// </summary>
        private class BancoTeste : AbstractBanco
        {
            public string ObterCodigoOcorrenciaBoleto(Boleto boleto)
            {
                return ObterCodigoDaOcorrencia(boleto);
            }
        }

        [TestMethod]
        public void Codigo_De_Ocorrencia()
        {
            var boleto = new Boleto();

            var expected = "02"; //Pedido de baixa - código 02
            boleto.Remessa = new Remessa(TipoOcorrenciaRemessa.PedidoDeBaixa);

            var banco = new BancoTeste();
            var actual = banco.ObterCodigoOcorrenciaBoleto(boleto);

            Assert.AreEqual(expected, actual);
        }
    }
}
