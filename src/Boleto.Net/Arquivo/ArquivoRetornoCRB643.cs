using System.IO;
using System.Text.RegularExpressions;
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
                if (Regex.Match(linha, "[0-9]{8}\\.[0-9]{3}\\.[0-9]{3}").Success)
                {
                    linha = streamReader.ReadLine();
                    continue;
                }
                var linhaCbr643 = textPosReader.Read(linha);
                OnLinhaLida(linhaCbr643);
                linha = streamReader.ReadLine();
            }
        }
    }
}
