using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Unicred
    {
        CadastroDeTitulo = 1,                      
        PedidoBaixa = 2,                  
        ConcessaoAbatimento = 4,
        CancelamentoAbatimentoConcedido = 5,
        AlteracaoVencimento = 6,
        PedidoProtesto = 9,
        SustarProtestoBaixarTitulo = 18,
        SustarProtestoManterCarteira = 19,
        AlteracaoOutrosDados = 31,
        AlteracaoOutrosDados_Desconto = 311,
        AlteracaoOutrosDados_JuroDia = 312,
        AlteracaoOutrosDados_DescontoAntecipacao = 313,
        AlteracaoOutrosDados_DataLimiteDesconto = 314,
        AlteracaoOutrosDados_CancelamentoProtestoAutomatico = 315,
        //AlteracaoOutrosDados_CarteiraDeCobranca = 316,  n�o disponivel...

	
        OutrasInstrucoes_ExibeMensagem_MoraDiaria = 900,
        OutrasInstrucoes_ExibeMensagem_MultaVencimento = 901
    }

    #endregion 

    public class Instrucao_Unicred : AbstractInstrucao, IInstrucao
    {

        #region Construtores 

		public Instrucao_Unicred()
		{
			try
			{
                this.Banco = new Banco(748);
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

        public Instrucao_Unicred(int codigo)
        {
            this.carregar(codigo, 0);
        }

        public Instrucao_Unicred(int codigo, int nrDias)
        {
            this.carregar(codigo, nrDias);
        }
        public Instrucao_Unicred(int codigo, double valor)
        {
            this.carregar(codigo, valor);
        }

        public Instrucao_Unicred(int codigo, double valor, EnumTipoValor tipoValor)
        {
            this.carregar(codigo, valor, tipoValor);
        }
        #endregion

        #region Metodos Privados


        private void carregar(int idInstrucao, double valor, EnumTipoValor tipoValor = EnumTipoValor.Percentual)
        {
            try
            {
                this.Banco = new Banco_Unicred();
                this.Valida();

                switch ((EnumInstrucoes_Unicred)idInstrucao)
                {
                    case EnumInstrucoes_Unicred.OutrasInstrucoes_ExibeMensagem_MoraDiaria:
                        this.Codigo = (int)EnumInstrucoes_Unicred.AlteracaoOutrosDados;
                        this.Descricao = String.Format("  - AP�S VENCIMENTO COBRAR JUROS DE {0} {1} POR DIA DE ATRASO",
                            (tipoValor.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2")),
                            (tipoValor.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2")));
                        break;
                    case EnumInstrucoes_Unicred.OutrasInstrucoes_ExibeMensagem_MultaVencimento:
                        this.Codigo = (int)EnumInstrucoes_Unicred.AlteracaoOutrosDados;
                        this.Descricao = String.Format("  - AP�S VENCIMENTO COBRAR MULTA DE {0} {1}",
                            (tipoValor.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2")),
                            (tipoValor.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2")));
                        break;
                    case EnumInstrucoes_Unicred.AlteracaoOutrosDados_Desconto:
                        this.Codigo = (int)EnumInstrucoes_Unicred.AlteracaoOutrosDados;
                        this.Descricao = "  - CONCEDER DESCONTO DE R$ " + valor;
                        break;
                    case EnumInstrucoes_Unicred.AlteracaoOutrosDados_DescontoAntecipacao:
                        this.Codigo = (int)EnumInstrucoes_Unicred.AlteracaoOutrosDados;
                        this.Descricao = "  - CONCEDER DESCONTO DE R$ " + valor + "POR DIA DE ANTECIPA��O";
                        break;
                    case EnumInstrucoes_Unicred.AlteracaoOutrosDados_JuroDia:
                        this.Codigo = (int)EnumInstrucoes_Unicred.AlteracaoOutrosDados;
                        this.Descricao = "  - AP�S VENCIMENTO COBRAR JURO DE " + valor + "% POR DIA DE ATRASO";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = " (Selecione) ";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        private void carregar(int idInstrucao, int nrDias)
        {
            try
            {
                this.Banco = new Banco_Unicred();
                this.Valida();

                switch ((EnumInstrucoes_Unicred)idInstrucao)
                {
                    case EnumInstrucoes_Unicred.CadastroDeTitulo:
                        this.Codigo = (int)EnumInstrucoes_Unicred.CadastroDeTitulo;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Unicred.PedidoBaixa:
                        this.Codigo = (int)EnumInstrucoes_Unicred.PedidoBaixa;
                        this.Descricao = "";
                        break;   
                    case EnumInstrucoes_Unicred.ConcessaoAbatimento:
                        this.Codigo = (int)EnumInstrucoes_Unicred.ConcessaoAbatimento;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Unicred.CancelamentoAbatimentoConcedido:
                        this.Codigo = (int)EnumInstrucoes_Unicred.CancelamentoAbatimentoConcedido;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Unicred.AlteracaoVencimento:
                        this.Codigo = (int)EnumInstrucoes_Unicred.AlteracaoVencimento;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Unicred.PedidoProtesto:
                        this.Codigo = (int)EnumInstrucoes_Unicred.PedidoProtesto;
                        this.Descricao = "  - PROTESTAR AP�S " + nrDias + " DIAS �TEIS DO VENCIMENTO";
                        break;
                    case EnumInstrucoes_Unicred.SustarProtestoBaixarTitulo:
                        this.Codigo = (int)EnumInstrucoes_Unicred.SustarProtestoBaixarTitulo;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Unicred.SustarProtestoManterCarteira:
                        this.Codigo = (int)EnumInstrucoes_Unicred.SustarProtestoManterCarteira;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Unicred.AlteracaoOutrosDados:
                        this.Codigo = (int)EnumInstrucoes_Unicred.AlteracaoOutrosDados;
                        this.Descricao = "";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = " (Selecione) ";
                        break;
                }

                this.QuantidadeDias = nrDias;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }


        public override void Valida()
        {
            //base.Valida();
        }

        #endregion

    }
}
