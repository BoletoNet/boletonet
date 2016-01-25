using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCodigoMovimento_BancoBrasil
    {
        EntradaConfirmada = 2,
        EntradaRejeitada = 3,
        TransferenciaCarteiraEntrada = 4,
        TransferenciaCarteiraBaixa = 5,
        Liquidacao = 6,
        Baixa = 9,
        TitulosCarteiraEmSer = 11,
        ConfirmacaoRecebimentoInstrucaoAbatimento = 12,
        ConfirmacaoRecebimentoInstrucaoCancelamentoAbatimento = 13,
        ConfirmacaoRecebimentoInstrucaoAlteracaoVencimento = 14,
        FrancoPagamento = 15,
        LiquidacaoAposBaixa = 17,
        ConfirmacaoRecebimentoInstrucaoProtesto = 19,
        ConfirmacaoRecebimentoInstrucaoSustacaoProtesto = 20,
        RemessaCartorio = 23,
        RetiradaCartorioManutencaoCarteira = 24,
        ProtestadoBaixado = 25,
        InstrucaoRejeitada = 26,
        Confirma��oPedidoAlteracaoOutrosDados = 27,
        DebitoTarifas = 28,
        OcorrenciaSacado = 29,
        AlteracaoDadosRejeitada = 30,

    }

    #endregion 

    public class CodigoMovimento_BancoBrasil: AbstractCodigoMovimento, ICodigoMovimento
    {
        #region Construtores 

		public CodigoMovimento_BancoBrasil()
		{
			try
			{
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

        public CodigoMovimento_BancoBrasil(int codigo)
        {
            try
            {
                this.carregar(codigo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

		#endregion 

        #region Metodos Privados

        private void carregar(int codigo)
        {
            try
            {
                this.Banco = new Banco_Brasil();

                switch ((EnumCodigoMovimento_BancoBrasil)codigo)
                {
                    case  EnumCodigoMovimento_BancoBrasil.EntradaConfirmada:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.EntradaConfirmada;
                        this.Descricao = "Entrada confirmada";
                        break;
                    case EnumCodigoMovimento_BancoBrasil.EntradaRejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.EntradaRejeitada;
                        this.Descricao = "Entrada rejeitada";
                        break;
                    case EnumCodigoMovimento_BancoBrasil.TransferenciaCarteiraEntrada:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.TransferenciaCarteiraEntrada;
                        this.Descricao = "Transfer�ncia de carteira/entrada";
                        break;
                    case EnumCodigoMovimento_BancoBrasil.TransferenciaCarteiraBaixa:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.TransferenciaCarteiraBaixa;
                        this.Descricao = "Transfer�ncia de carteira/baixa";
                        break;
                    case EnumCodigoMovimento_BancoBrasil.Liquidacao:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.Liquidacao;
                        this.Descricao = "Liquida��o normal";
                        break;
                    case EnumCodigoMovimento_BancoBrasil.Baixa:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.Baixa;
                        this.Descricao = "Baixa";
                        break;
                    case EnumCodigoMovimento_BancoBrasil.TitulosCarteiraEmSer:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.TitulosCarteiraEmSer;
                        this.Descricao = "T�tulos em carteira em ser";
                        break;
                    case EnumCodigoMovimento_BancoBrasil.ConfirmacaoRecebimentoInstrucaoAbatimento:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.ConfirmacaoRecebimentoInstrucaoAbatimento;
                        this.Descricao = "Confirma��o recebimento instru��o de abatimento";
                        break;
                    case EnumCodigoMovimento_BancoBrasil.ConfirmacaoRecebimentoInstrucaoCancelamentoAbatimento:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.ConfirmacaoRecebimentoInstrucaoCancelamentoAbatimento;
                        this.Descricao = "Confirma��o recebimento instru��o de cancelamento de abatimento";
                        break;
                    case EnumCodigoMovimento_BancoBrasil.ConfirmacaoRecebimentoInstrucaoAlteracaoVencimento:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.ConfirmacaoRecebimentoInstrucaoAlteracaoVencimento;
                        this.Descricao = "Confirma��o recebimento instru��o altera��o de vencimento";
                        break;
                    case EnumCodigoMovimento_BancoBrasil.FrancoPagamento:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.FrancoPagamento;
                        this.Descricao = "Franco pagamento";
                        break;
                    case EnumCodigoMovimento_BancoBrasil.LiquidacaoAposBaixa:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.LiquidacaoAposBaixa;
                        this.Descricao = "Liquida��o ap�s baixa";
                        break;
                    case EnumCodigoMovimento_BancoBrasil.ConfirmacaoRecebimentoInstrucaoProtesto:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.ConfirmacaoRecebimentoInstrucaoProtesto;
                        this.Descricao = "Confirma��o de recebimento de instru��o de protesto";
                        break;
                    case EnumCodigoMovimento_BancoBrasil.ConfirmacaoRecebimentoInstrucaoSustacaoProtesto:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.ConfirmacaoRecebimentoInstrucaoSustacaoProtesto;
                        this.Descricao = "Confirma��o de recebimento de instru��o de susta��o de protesto";
                        break;
                    case EnumCodigoMovimento_BancoBrasil.RemessaCartorio:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.RemessaCartorio;
                        this.Descricao = "Remessa a cart�rio/aponte em cart�rio";
                        break;
                    case EnumCodigoMovimento_BancoBrasil.RetiradaCartorioManutencaoCarteira:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.RetiradaCartorioManutencaoCarteira;
                        this.Descricao = "Retirada de cart�rio e manuten��o em carteira";
                        break;
                    case EnumCodigoMovimento_BancoBrasil.ProtestadoBaixado:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.ProtestadoBaixado;
                        this.Descricao = "Protestado e baixado/baixa por ter sido protestado";
                        break;
                    case EnumCodigoMovimento_BancoBrasil.InstrucaoRejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.InstrucaoRejeitada;
                        this.Descricao = "Instru��o rejeitada";
                        break;
                    case EnumCodigoMovimento_BancoBrasil.Confirma��oPedidoAlteracaoOutrosDados:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.Confirma��oPedidoAlteracaoOutrosDados;
                        this.Descricao = "Confirma��o do pedido de altera��o de outros dados";
                        break;
                    case EnumCodigoMovimento_BancoBrasil.DebitoTarifas:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.DebitoTarifas;
                        this.Descricao = "Debito de tarifas/custas";
                        break;
                    case EnumCodigoMovimento_BancoBrasil.OcorrenciaSacado:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.OcorrenciaSacado;
                        this.Descricao = "Ocorrencias do sacado";
                        break;
                    case EnumCodigoMovimento_BancoBrasil.AlteracaoDadosRejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.AlteracaoDadosRejeitada;
                        this.Descricao = "Altera��o de dados rejeitada";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "( Selecione )";
                        break;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        private void Ler(int codigo)
        {
            try
            {
                switch (codigo)
                {
                    case 2:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.EntradaConfirmada;
                        this.Descricao = "Entrada confirmada";
                        break;
                    case 3:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.EntradaRejeitada;
                        this.Descricao = "Entrada rejeitada";
                        break;
                    case 4:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.TransferenciaCarteiraEntrada;
                        this.Descricao = "Transfer�ncia de carteira/entrada";
                        break;
                    case 5:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.TransferenciaCarteiraBaixa;
                        this.Descricao = "Transfer�ncia de carteira/baixa";
                        break;
                    case 6:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.Liquidacao;
                        this.Descricao = "Liquida��o";
                        break;
                    case 9:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.Baixa;
                        this.Descricao = "Baixa";
                        break;
                    case 11:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.TitulosCarteiraEmSer;
                        this.Descricao = "T�tulos em carteira em ser";
                        break;
                    case 12:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.ConfirmacaoRecebimentoInstrucaoAbatimento;
                        this.Descricao = "Confirma��o recebimento instru��o de abatimento";
                        break;
                    case 13:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.ConfirmacaoRecebimentoInstrucaoCancelamentoAbatimento;
                        this.Descricao = "Confirma��o recebimento instru��o de cancelamento de abatimento";
                        break;
                    case 14:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.ConfirmacaoRecebimentoInstrucaoAlteracaoVencimento;
                        this.Descricao = "Confirma��o recebimento instru��o altera��o de vencimento";
                        break;
                    case 15:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.FrancoPagamento;
                        this.Descricao = "Franco pagamento";
                        break;
                    case 17:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.LiquidacaoAposBaixa;
                        this.Descricao = "Liquida��o ap�s baixa";
                        break;
                    case 19:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.ConfirmacaoRecebimentoInstrucaoProtesto;
                        this.Descricao = "Confirma��o de recebimento de instru��o de protesto";
                        break;
                    case 20:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.ConfirmacaoRecebimentoInstrucaoSustacaoProtesto;
                        this.Descricao = "Confirma��o de recebimento de instru��o de susta��o de protesto";
                        break;
                    case 23:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.RemessaCartorio;
                        this.Descricao = "Remessa a cart�rio/aponte em cart�rio";
                        break;
                    case 24:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.RetiradaCartorioManutencaoCarteira;
                        this.Descricao = "Retirada de cart�rio e manuten��o em carteira";
                        break;
                    case 25:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.ProtestadoBaixado;
                        this.Descricao = "Protestado e baixado/baixa por ter sido protestado";
                        break;
                    case 26:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.InstrucaoRejeitada;
                        this.Descricao = "Instru��o rejeitada";
                        break;
                    case 27:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.Confirma��oPedidoAlteracaoOutrosDados;
                        this.Descricao = "Confirma��o do pedido de altera��o de outros dados";
                        break;
                    case 28:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.DebitoTarifas;
                        this.Descricao = "Debito de tarifas/custas";
                        break;
                    case 29:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.OcorrenciaSacado;
                        this.Descricao = "Ocorrencias do sacado";
                        break;
                    case 30:
                        this.Codigo = (int)EnumCodigoMovimento_BancoBrasil.AlteracaoDadosRejeitada;
                        this.Descricao = "Altera��o de dados rejeitada";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "( Selecione )";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }


        #endregion
    }
}
