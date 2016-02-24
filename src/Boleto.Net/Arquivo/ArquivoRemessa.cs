using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class ArquivoRemessa : AbstractArquivoRemessa, IArquivoRemessa
    {
        public ArquivoRemessa(TipoArquivo tipoarquivo)
            : base(tipoarquivo)
        {
        }
    }
}
