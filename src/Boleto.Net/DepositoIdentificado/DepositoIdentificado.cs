using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoletoNet
{
    public abstract class DepositoIdentificado
    {
        public IBanco Banco { get; private set; }
        public Cedente Cedente { get; set; }
        public Sacado Sacado { get; set; }
        public string Logo { get; set; }
        public abstract string NumeroDocumento { get; set; }
        public List<String> InstrucoesPagamento { get; set; }
        public List<String> InstrucoesCaixa { get; set; }

        public DepositoIdentificado(IBanco banco)
        {
            this.Banco = banco;
        }


    }
}
