using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoletoNet {

    public enum EnumInstrucoes_Cecred {
        /*
        * Código de Movimento Remessa 
        * 01 - Registro de títulos;  
        * 02 - Solicitação de baixa; 
        * 04 - Concessão de abatimento;  
        * 05 - Cancelamento de abatimento;  
        * 06 - Alteração de vencimento de título;  
        * 09 - Instruções para protestar (Nota 09);   
        * 10 - Instrução para sustar protesto;  
        * 12 - Alteração de nome e endereço do Pagador;  
        * 17 – Liquidação de título não registro ou pagamento em duplicidade; 
        * 31 - Conceder desconto; 
        * 32 - Não conceder desconto. 
        */
        CadastroDeTitulo = 1,
        PedidoBaixa = 2,
        ConcessaoAbatimento = 4,
        CancelamentoAbatimentoConcedido = 5,
        AlteracaoVencimento = 6,
        PedidoProtesto = 9,
        SustarProtestoBaixarTitulo = 10,
        AlteracaoNomeEnderecoPagador = 12,
        LiquidacaoDeTituloNaoRegristroOuPagamentoEmDuplicidade = 17,
        ConcederDesconto = 31,
        NaoConcederDesconto = 32

    }

    public class Instrucao_Cecred : AbstractInstrucao, IInstrucao {

        public Instrucao_Cecred() {
            try {
                this.Banco = new Banco_Cecred();
            } catch (Exception ex) {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_Cecred(int codigo) {
            try {

                this.Banco = new Banco_Cecred();

                this.Codigo = codigo;
                // TODO Implementar descricao do código de remessa para cecred

            } catch (Exception ex) {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }
    }
}
