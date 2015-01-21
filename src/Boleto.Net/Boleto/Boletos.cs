using System;
using System.Collections.Generic;

namespace BoletoNet
{
    public class Boletos : List<Boleto>
    {

        # region Variáveis

	    # endregion

        # region Propriedades

	    public Banco Banco { get; set; }

	    public ContaBancaria ContaBancaria { get; set; }

	    public Cedente Cedente { get; set; }

	    # endregion

        # region Métodos

        /// <summary>
        /// Verifica se já existe o arquivo relativo a remessa, caso não exista é criado um arquivo ".rem".
        /// 
        /// O padrão dos arquivos de Remessa e Retorno, obedece as regras estabelecidas pelo C.N.A.B. (Centro Nacional
        /// de Automação Bancária) e deverá ser gravado contendo:
        /// Registro Header : Primeiro registro do Arquivo contendo a identificação da empresa
        /// Registro Detalhe : Registro contendo as informações de Pagamentos :
        /// - Inclusão de compromissos
        /// - Alteração de Compromissos
        /// - Pagamentos Efetuados
        /// - Bloqueios / Desbloqueios
        /// Registro Trailer : Último registro indicando finalização do Arquivo
        /// Caracteres obrigatórios = 0D 0A (Final de Registro) 0D 0A 1A (Final de Arquivo)
        /// </summary>

        private new void Add(Boleto item)
        {
            if (item.Banco == null)
                throw new Exception("Boleto não possui Banco.");

            if (item.ContaBancaria == null)
                throw new Exception("Boleto não possui conta bancária.");

            if (item.Cedente == null)
                throw new Exception("Boleto não possui cedente.");

            item.Valida();
            this.Add(item);
        }

        # endregion

    }
}
