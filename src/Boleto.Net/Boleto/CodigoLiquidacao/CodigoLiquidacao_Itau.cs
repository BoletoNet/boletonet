using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCodigoLiquidacao_Itau
    {
        CaixaEletronicoBancoItau = 1,
        PagamentoCartorioAutomatizado = 2,
        BancosCorrespondentes = 4,
        ItauBankFone = 5,
        ItauBankLine = 6,
        OB_RecebimentoOffline = 7,
        OB_PeloCodigoBarras = 8,
        OB_PelaLinhaDigitavel = 9,
        OB_PeloAutoAtendimento = 10,
        OB_RecebimentoCasaLoterica = 11,
        ComChequeOutroBanco = 12,
        Sispag = 13,
        DebitoContaCorrente = 14,
        CapturadoOffline = 15,
        PagamentoCartorioProtestoComCheque = 16,
        PagamentoAgendadoViaBankLine = 17,
    }

    #endregion 

    public class CodigoLiquidacao_Itau: AbstractCodigoLiquidacao, ICodigoLiquidacao
    {
        #region Construtores 

		public CodigoLiquidacao_Itau()
		{
			try
			{
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

        public CodigoLiquidacao_Itau(int codigo)
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

        private void carregar(int idCodigo)
        {
            try
            {
                this.Banco = new Banco_Itau();

                switch ((EnumCodigoLiquidacao_Itau)idCodigo)
                {
                    case  EnumCodigoLiquidacao_Itau.CaixaEletronicoBancoItau:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.CaixaEletronicoBancoItau;
                        this.Codigo = "AA";
                        this.Descricao = "Caixa eletrônico do Banco Itaú.";
                        this.Recurso = "Disponível";
                        break;
                    case EnumCodigoLiquidacao_Itau.PagamentoCartorioAutomatizado:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoCartorioAutomatizado;
                        this.Codigo = "AC";
                        this.Descricao = "Pagamento em cartório automatizado.";
                        this.Recurso = "A compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.BancosCorrespondentes:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.BancosCorrespondentes;
                        this.Codigo = "BC";
                        this.Descricao = "Bancos correspondentes.";
                        this.Recurso = "Disponível";
                        break;
                    case EnumCodigoLiquidacao_Itau.ItauBankFone:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.ItauBankFone;
                        this.Codigo = "BF";
                        this.Descricao = "Itaú BankFone.";
                        this.Recurso = "Disponível";
                        break;
                    case EnumCodigoLiquidacao_Itau.ItauBankLine:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.ItauBankLine;
                        this.Codigo = "BL";
                        this.Descricao = "Itaú BankLine.";
                        this.Recurso = "Disponível";
                        break;
                    case EnumCodigoLiquidacao_Itau.OB_RecebimentoOffline:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_RecebimentoOffline;
                        this.Codigo = "B0";
                        this.Descricao = "Outros bancos - Recebimento offline.";
                        this.Recurso = "A compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.OB_PeloCodigoBarras:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_PeloCodigoBarras;
                        this.Codigo = "B1";
                        this.Descricao = "Outros bancos - Pelo código de barras.";
                        this.Recurso = "A compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.OB_PelaLinhaDigitavel:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_PelaLinhaDigitavel;
                        this.Codigo = "B2";
                        this.Descricao = "Outros bancos - Pelo linha digitável.";
                        this.Recurso = "A compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.OB_PeloAutoAtendimento:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_PeloAutoAtendimento;
                        this.Codigo = "B3";
                        this.Descricao = "Outros bancos - Pelo auto-atendimento.";
                        this.Recurso = "A compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.OB_RecebimentoCasaLoterica:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_RecebimentoCasaLoterica;
                        this.Codigo = "B4";
                        this.Descricao = "Outros bancos - Recebimento em casa lotérica.";
                        this.Recurso = "A compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.ComChequeOutroBanco:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.ComChequeOutroBanco;
                        this.Codigo = "CC";
                        this.Descricao = "Agência Itaú - Com cheque de outro banco.";
                        this.Recurso = "A compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.Sispag:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.Sispag;
                        this.Codigo = "CK";
                        this.Descricao = "SISPAG - Sistema de contas a pagar Itaú.";
                        this.Recurso = "Disponível";
                        break;
                    case EnumCodigoLiquidacao_Itau.DebitoContaCorrente:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.DebitoContaCorrente;
                        this.Codigo = "CP";
                        this.Descricao = "Agência Itaú - Por débito em conta corrente, cheque ou dinheiro.";
                        this.Recurso = "Disponível";
                        break;
                    case EnumCodigoLiquidacao_Itau.CapturadoOffline:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.CapturadoOffline;
                        this.Codigo = "DG";
                        this.Descricao = "Agência Itaú - Capturado offline.";
                        this.Recurso = "Disponível";
                        break;
                    case EnumCodigoLiquidacao_Itau.PagamentoCartorioProtestoComCheque:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoCartorioProtestoComCheque;
                        this.Codigo = "LC";
                        this.Descricao = "Pagamento em cartório de protesto com cheque.";
                        this.Recurso = "A Compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.PagamentoAgendadoViaBankLine:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoAgendadoViaBankLine;
                        this.Codigo = "Q0";
                        this.Descricao = "Agendamento - Pagamento agendado via BankLine ou outro canal eletrônico.";
                        this.Recurso = "Disponível";
                        break;
                    default:
                        this.Enumerado = 0;
                        this.Codigo = " ";
                        this.Descricao = "( Selecione )";
                        this.Recurso = "Sem Recurso";
                        break;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        private void Ler(string Code)
        {
            try
            {
                switch (Code)
                {
                    case "AA":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.CaixaEletronicoBancoItau;
                        this.Descricao = "Caixa eletrônico do banco Itaú";
                        this.Codigo = "AA";
                        this.Recurso = "Disponível";
                        break;
                    case "AC":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoCartorioAutomatizado;
                        this.Descricao = "Pagamento em cartório automatizado";
                        this.Codigo = "AC";
                        this.Recurso = "A compensar";
                        break;
                    case "BC":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.BancosCorrespondentes;
                        this.Descricao = "Bancos correspondentes";
                        this.Codigo = "BC";
                        this.Recurso = "Disponível";
                        break;
                    case "BF":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.ItauBankFone;
                        this.Descricao = "Itaú Bankfone";
                        this.Codigo = "BF";
                        this.Recurso = "Disponível";
                        break;
                    case "BL":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.ItauBankLine;
                        this.Descricao = "Itaú Bankline";
                        this.Codigo = "BL";
                        this.Recurso = "Disponível";
                        break;
                    case "B0":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_RecebimentoOffline;
                        this.Descricao = "Outros bancos - recebimento offline";
                        this.Codigo = "B0";
                        this.Recurso = "A compensar";
                        break;
                    case "B1":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_PeloCodigoBarras;
                        this.Descricao = "Outros bancos - pelo código de barras";
                        this.Codigo = "B1";
                        this.Recurso = "A compensar";
                        break;
                    case "B2":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_PelaLinhaDigitavel;
                        this.Descricao = "Outros bancos - pelo linha digitável";
                        this.Codigo = "B2";
                        this.Recurso = "A compensar";
                        break;
                    case "B3":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_PeloAutoAtendimento;
                        this.Descricao = "Outros bancos - pelo auto-atendimento";
                        this.Codigo = "B3";
                        this.Recurso = "A compensar";
                        break;
                    case "B4":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_RecebimentoCasaLoterica;
                        this.Descricao = "Outros bancos - recebimento em casa lotérica";
                        this.Codigo = "B4";
                        this.Recurso = "A compensar";
                        break;
                    case "CC":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.ComChequeOutroBanco;
                        this.Descricao = "Agêcnia Itaú - com cheque de outro banco";
                        this.Codigo = "CC";
                        this.Recurso = "A compensar";
                        break;
                    case "CK":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.Sispag;
                        this.Descricao = "SISPAG - Sistema de contas a pagar Itaú";
                        this.Codigo = "CK";
                        this.Recurso = "Disponível";
                        break;
                    case "CP":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.DebitoContaCorrente;
                        this.Descricao = "Agência Itaú - por débito em conta-corrente, cheque ou dinheiro";
                        this.Codigo = "CP";
                        this.Recurso = "Disponível";
                        break;
                    case "DG":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.CapturadoOffline;
                        this.Descricao = "Agência Itaú - capturado em offline";
                        this.Codigo = "DG";
                        this.Recurso = "Disponível";
                        break;
                    case "LC":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoCartorioProtestoComCheque;
                        this.Descricao = "Pagamento em cartório de protesto com cheque";
                        this.Codigo = "LC";
                        this.Recurso = "A compensar";
                        break;
                    case "Q0":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoAgendadoViaBankLine;
                        this.Descricao = "Agendamento - Pagamento agendado via BankLine ou outro canal eletrônico.";
                        this.Codigo = "Q0";
                        this.Recurso = "Disponível";
                        break;
                    default:
                        this.Enumerado = 0;
                        this.Descricao = " (Selecione) ";
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
