using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public interface ICarteira
    {
        IBanco Banco { get; set; }
        int NumeroCarteira { get; set; }
        string Codigo { get; set;}
        string Tipo { get; set; }
        string Descricao { get; set; }
    }
}
