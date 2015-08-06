using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BoletoNet
{
    public class DetalheSegmento10RetornoCNAB100 : AbstractDetalheSegmento
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
        /// Data da efetivação do crédito em conta (compensação)
        /// </summary>
        public DateTime DataCredito { get; set; }
        /// <summary>
        /// Agência de acolheu o crétido (onde o depósito foi feito)
        /// </summary>
        public string AgenciaOrigem { get; set; }
        public string NumeroLote { get; set; }
        /// <summary>
        /// Data do recebimento
        /// </summary>
        public DateTime DataRemessa { get; set; }
        /// <summary>
        /// Número do documento gerado no formulário de depósito identificado
        /// </summary>
        public string Identificacao { get; set; }
        public decimal Valor { get; set; }
        public TipoDeDeposito Especie { get; set; }
        /// <summary>
        /// Número de sequencia do registro no arquivo
        /// </summary>
        public int Sequencia { get; set; }

        private static Regex _regex = new Regex(
                @"^(?<NCL>.{3})(?<AG>\d{4})(?<CC>\d{5})(?<DAC>\d{1})10(?<DC>\d{6})(?<AGO>\d{4})(?<LT>\d{3})(?<DR\d{6})(?<ID>\d{16})" +
                @"(?<RES>\s{4})(?<VLR>\d{17})(?<RES>\d{6})(?<TD>.{3})(?<RES>.{4})(?<SEQ>\d{6})(?<RES>.{3})06(?<RES>.{5})$"
            );

        public DetalheSegmento10RetornoCNAB100(string linha)
            : base(linha, _regex)
        {
            this.NumeroCliente = GetStr("NCL");
            this.Agencia = GetStr("AG");
            this.Conta = GetStr("CC");
            this.DAC = GetStr("DAC");
            this.DataCredito = GetDate("DC");
            this.AgenciaOrigem = GetStr("AGO");
            this.NumeroLote = GetStr("LT");
            this.DataRemessa = GetDate("DR");
            this.Identificacao = GetStr("ID");
            this.Valor = GetDec("VLR");
            this.Especie = (TipoDeDeposito)GetInt("TD");
            this.Sequencia = GetInt("SEQ");
        }
    }
}
