using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BoletoNet
{
    public class Cheque
    {
        public string Banco { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string Numero { get; set; }
        public decimal Valor { get; set; }
    }

    public class DetalheSegmento20RetornoCNAB100 : AbstractDetalheSegmento
    {
        /// <summary>
        /// Número do cliente fornecido pelo banco
        /// </summary>
        public string NumeroCliente { get; set; }
        /// <summary>
        /// Agência recebedora do crédito
        /// </summary>
        public string Agencia { get; set; }
        /// <summary>
        /// Número da conta corrente do cliente
        /// </summary>
        public string Conta { get; set; }
        /// <summary>
        /// Dac da agência/conta
        /// </summary>
        public string DAC { get; set; }
        /// <summary>
        /// Número do documento gerado no formulário de depósito identificado
        /// </summary>
        public string Identificacao { get; set; }
        /// <summary>
        /// Código da câmara de compensação
        /// </summary>
        public string Comp { get; set; }
        /// <summary>
        /// Informações do cheque
        /// </summary>
        public Cheque Cheque { get; set; }
        /// <summary>
        /// Quantidade de dias de bloqueio/prazo de compensação
        /// </summary>
        public byte Bloqueio { get; set; }

        private static Regex _regex = new Regex(
                @"^(?<NCL>.{3})(?<AG>\d{4})(?<CC>\d{5})(?<DAC>\d{1})20(?<RES>\d{13})(?<ID>\d{16})(?<RES>\s{4})(?<COMP>\d{3})" +
                @"(?<BANCO>\d{3})(?<CHAG>\d{4})(?<CHCC>\d{10})(?<CHN>\d{6})(?<VLR>\d{17})(?<BLOQ>{2})(?<RES>.{7})$"
            );

        public DetalheSegmento20RetornoCNAB100(string linha)
            : base(linha, _regex)
        {
            this.NumeroCliente = GetStr("NCL");
            this.Agencia = GetStr("AG");
            this.Conta = GetStr("CC");
            this.DAC = GetStr("DAC");
            this.Identificacao = GetStr("ID");
            this.Comp = GetStr("COMP");
            this.Bloqueio = (byte)GetInt("BLOQ");
            this.Cheque = new Cheque {
                Banco = GetStr("BANCO"),
                Agencia = GetStr("CHAG"),
                Conta = GetStr("CHCC"),
                Numero = GetStr("CHN"),
                Valor = GetDec("VLR")
            };
        }
    }
}
