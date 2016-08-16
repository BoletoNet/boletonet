using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public interface ICodigoMovimento
    {
        IBanco Banco { get; }
        int Codigo { get; set;}
        string Descricao { get; }
    }
}
