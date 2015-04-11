using System.IO;
using BoletoNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Boleto.Net.Testes.Retorno
{
    [TestClass]
    public class Teste
    {
        [TestMethod]
        public void TesteMetodo()
        {
            var arquivoRetorno = new ArquivoRetorno(TipoArquivo.CNAB400);
            using (var fileStream = File.OpenRead(@"C:\Temp\Arquivos Retorno\CBR64340452010201422730.ret"))
            {
                var banco = new Banco(001);
                arquivoRetorno.LinhaDeArquivoLida += (sender, args) =>
                {
                    Assert.IsNotNull(args.Linha);
                };
                arquivoRetorno.LerArquivoRetorno(banco, fileStream);
            }
        }
    }
}
