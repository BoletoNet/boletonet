using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BoletoNet.Arquivo
{
    public class ArquivoRetornoCRB643 : AbstractArquivoRetorno, IArquivoRetorno
    {
        public override void LerArquivoRetorno(IBanco banco, Stream arquivo)
        {
            base.LerArquivoRetorno(banco, arquivo);
        }
    }
}
