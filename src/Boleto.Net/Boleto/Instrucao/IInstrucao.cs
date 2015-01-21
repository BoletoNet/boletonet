using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public interface IInstrucao
    {
        /// <summary>
        /// Valida os dados referentes à instrução
        /// </summary>
        void Valida();

        IBanco Banco { get; set; }
        int Codigo { get; set; }
        string Descricao { get; set; }
        int QuantidadeDias { get; set; }
    }
}
