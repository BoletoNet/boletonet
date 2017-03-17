using System;

namespace BoletoNet.Arquivo
{
    public class HeaderCbr643 : LinhaCbr643
    {
        public HeaderCbr643()
        {
            Id = 0;
        }

        /// <summary>
        /// Tipo de Operação: “2”   
        /// </summary>
        [TextPos(1, 1)]
        public string Operacao { get; set; }

        /// <summary>
        /// Identificação Tipo de Operação “RETORNO”  
        /// </summary>
        [TextPos(2, 7)]
        public string IdOperacao { get; set; }

        /// <summary>
        /// Identificação do Tipo de Serviço: “01”  
        /// </summary>
        [TextPos(9, 2)]
        public string Servico { get; set; }

        /// <summary>
        /// Identificação por Extenso do Tipo de Serviço: “COBRANCA”  
        /// </summary>
        [TextPos(11, 8)]
        public string ServicoPorExtenso { get; set; }

        /// <summary>
        /// Prefixo da Agência: Número da Agência onde está cadastrado o convênio líder do cedente  
        /// </summary>
        [TextPos(26, 4)]
        public string Agencia { get; set; }

        /// <summary>
        /// Dígito Verificador - D.V. - do Prefixo da Agência.  
        /// </summary>
        [TextPos(30, 1)]
        public string AgenciaDV { get; set; }

        /// <summary>
        /// Número da Conta Corrente: Número da conta onde está cadastrado o Convênio Líder do Cedente  
        /// </summary>
        [TextPos(31, 8)]
        public string ContaCorrente { get; set; }

        /// <summary>
        /// Dígito Verificador - D.V. - da Conta Corrente do Cedente  
        /// </summary>
        [TextPos(39, 1)]
        public string ContaCorrenteDV { get; set; }

        /// <summary>
        /// Nome do Cedente  
        /// </summary>
        [TextPos(46, 30)]
        public string Cedente { get; set; }

        /// <summary>
        /// Data da Gravação: Informe no formado “DDMMAA”  
        /// </summary>
        [TextPos(94, 6, "ddMMyy")]
        public DateTime DataGravacao { get; set; }

        /// <summary>
        /// Seqüencial do Retorno 01 
        /// </summary>
        [TextPos(100, 7)]
        public string SequenciaRetorno { get; set; }

        /// <summary>
        /// Número de convênio  
        /// </summary>
        [TextPos(149, 7)]
        public string NumeroConvenio { get; set; }
    }
}