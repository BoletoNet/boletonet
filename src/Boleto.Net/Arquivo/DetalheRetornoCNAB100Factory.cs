using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BoletoNet
{
    public enum TipoDeDeposito
    {
        ChequeCompensacaoNacional = 505,
        ChequeInferiorLimite = 513,
        ChequeSuperiorLimite = 521,
        ChequePraca = 612,
        ChequeItau = 539,
        ChequeAposHorarioCompensacao = 646,
        ChequeAcolhidoDinheiro = 547,
        Dinheiro = 604,
        Estorno = 1,
        Estorno2 = 540
    }

    public class DetalheRetornoCNAB100Factory
    {
        public static AbstractDetalheSegmento Create(string linha)
        {
            if (linha.Length == 100)
            {
                if (Regex.IsMatch(linha, "^.{13}10"))
                {
                    return new DetalheSegmento10RetornoCNAB100(linha);
                }
                else if (Regex.IsMatch(linha, "^.{13}20"))
                {
                    return new DetalheSegmento20RetornoCNAB100(linha);
                }
            }
            throw new NotImplementedException("Layout não implementado!");
        }
    }


}
