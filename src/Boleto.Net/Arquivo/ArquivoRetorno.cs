using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class ArquivoRetorno : AbstractArquivoRetorno, IArquivoRetorno
    {

        public ArquivoRetorno(TipoArquivo tipoarquivo)
            : base(tipoarquivo)
        {
        }
    }
}
