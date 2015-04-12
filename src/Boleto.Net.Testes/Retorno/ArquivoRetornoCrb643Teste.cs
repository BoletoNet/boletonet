using System.IO;
using System.Linq;
using System.Text;
using BoletoNet;
using BoletoNet.Arquivo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Boleto.Net.Testes.Retorno
{
    [TestClass]
    public class ArquivoRetornoCrb643Teste
    {
        const string ARQUIVO_RETORNO =
@"02RETORNO01COBRANCA       34746000203289000000MINHA EMPRESA DE EXEMPLO      001BANCO DO BRASIL2807140002172                      000003925032479234  1234567                                                                                                                                                                                                                                              000001
70000000000000000123450001234561234567                         1234567000000123450000001   01900000000000 1806280714                              000000000000001108100127936002907140000185000000000000000000000000000000000000000000000000000000000000000000000000011421000000000034000000000000000000000000000000000001123620000000000000          0000000000000000000000000000000000000000000000001002000002";
       
        [TestMethod]
        public void PossoLerALinhaDeCabecalho()
        {
            var arquivoRetorno = new ArquivoRetornoCrb643();
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(ARQUIVO_RETORNO));
            ms.Seek(0, SeekOrigin.Begin);
            using (ms)
            {
                var banco = new Banco(001);
                arquivoRetorno.LerArquivoRetorno(banco, ms);
            }
            var linhaHeader = arquivoRetorno.Linhas.OfType<HeaderCbr643>().FirstOrDefault();
            Assert.IsNotNull(linhaHeader);
            Assert.AreEqual("MINHA EMPRESA DE EXEMPLO", linhaHeader.Cedente);
            Assert.AreEqual("1234567", linhaHeader.NumeroConvenio);
        }

        [TestMethod]
        public void PossoLerALinhaDeDetalhe()
        {
            var arquivoRetorno = new ArquivoRetornoCrb643();
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(ARQUIVO_RETORNO));
            ms.Seek(0, SeekOrigin.Begin);
            using (ms)
            {
                var banco = new Banco(001);
                arquivoRetorno.LerArquivoRetorno(banco, ms);
            }
            var linhaDetalhe = arquivoRetorno.Linhas.OfType<DetalheCbr643>().FirstOrDefault();
            Assert.IsNotNull(linhaDetalhe);
            Assert.AreEqual(1234, linhaDetalhe.Agencia);
            Assert.AreEqual(1234567, linhaDetalhe.NumeroConvenio);
            Assert.AreEqual(110.81m, linhaDetalhe.ValorTitulo);
        }
    }
}
