using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public interface IArquivoRetorno
    {

        /// <summary>
        /// Ler arquivo de Retorno
        /// </summary>
        void LerArquivoRetorno(IBanco banco, Stream arquivo);

        IBanco Banco { get; }
        TipoArquivo TipoArquivo { get; }

        event EventHandler<LinhaDeArquivoLidaArgs> LinhaDeArquivoLida;
    }
}
