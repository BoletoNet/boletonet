using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BoletoNet
{
    public class DI_Itau : DepositoIdentificado
    {
        private string _numeroDocumento;
        public override string NumeroDocumento
        {
            get
            {
                return _numeroDocumento;
            }
            set
            {
                string n = Regex.Replace(value, "\\D", "");
                switch(n.Length)
                {
                    case 11: //Se for CPF
                    case 14: //Ou CNPJ simplesmente atribui à variável
                        _numeroDocumento = n;
                        break;
                    case 15:
                        _numeroDocumento = n + AbstractBanco.Mod10(n).ToString();
                        break;
                    default:
                        throw new Exception("Tamanho inválido para o número do documento!");
                }
            }
        }

        public DI_Itau() : base(new Banco_Itau()) { }
    }
}
