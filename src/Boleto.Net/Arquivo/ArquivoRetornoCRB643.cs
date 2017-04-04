using System.IO;
using BoletoNet.Arquivo.Reader;

namespace BoletoNet.Arquivo
{
    public class ArquivoRetornoCrb643 : AbstractArquivoRetorno<LinhaCbr643>, IArquivoRetorno
    {
        public override void LerArquivoRetorno(IBanco banco, Stream arquivo)
        {
            var streamReader = new StreamReader(arquivo);
            var linha = streamReader.ReadLine();
            var textPosReader = new TextPosReader();
            while (!string.IsNullOrEmpty(linha))
            {
                var linhaCbr643 = textPosReader.Read(linha);
                OnLinhaLida(linhaCbr643);
                linha = streamReader.ReadLine();
            }
        }
    }
}
