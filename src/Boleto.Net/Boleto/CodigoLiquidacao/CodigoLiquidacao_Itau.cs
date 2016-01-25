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
                        this.Descricao = "Caixa eletr�nico do Banco Ita�.";
                        this.Recurso = "Dispon�vel";
                        break;
                    case EnumCodigoLiquidacao_Itau.PagamentoCartorioAutomatizado:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoCartorioAutomatizado;
                        this.Codigo = "AC";
                        this.Descricao = "Pagamento em cart�rio automatizado.";
                        this.Recurso = "A compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.BancosCorrespondentes:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.BancosCorrespondentes;
                        this.Codigo = "BC";
                        this.Descricao = "Bancos correspondentes.";
                        this.Recurso = "Dispon�vel";
                        break;
                    case EnumCodigoLiquidacao_Itau.ItauBankFone:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.ItauBankFone;
                        this.Codigo = "BF";
                        this.Descricao = "Ita� BankFone.";
                        this.Recurso = "Dispon�vel";
                        break;
                    case EnumCodigoLiquidacao_Itau.ItauBankLine:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.ItauBankLine;
                        this.Codigo = "BL";
                        this.Descricao = "Ita� BankLine.";
                        this.Recurso = "Dispon�vel";
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
                        this.Descricao = "Outros bancos - Pelo c�digo de barras.";
                        this.Recurso = "A compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.OB_PelaLinhaDigitavel:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_PelaLinhaDigitavel;
                        this.Codigo = "B2";
                        this.Descricao = "Outros bancos - Pelo linha digit�vel.";
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
                        this.Descricao = "Outros bancos - Recebimento em casa lot�rica.";
                        this.Recurso = "A compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.ComChequeOutroBanco:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.ComChequeOutroBanco;
                        this.Codigo = "CC";
                        this.Descricao = "Ag�ncia Ita� - Com cheque de outro banco.";
                        this.Recurso = "A compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.Sispag:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.Sispag;
                        this.Codigo = "CK";
                        this.Descricao = "SISPAG - Sistema de contas a pagar Ita�.";
                        this.Recurso = "Dispon�vel";
                        break;
                    case EnumCodigoLiquidacao_Itau.DebitoContaCorrente:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.DebitoContaCorrente;
                        this.Codigo = "CP";
                        this.Descricao = "Ag�ncia Ita� - Por d�bito em conta corrente, cheque ou dinheiro.";
                        this.Recurso = "Dispon�vel";
                        break;
                    case EnumCodigoLiquidacao_Itau.CapturadoOffline:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.CapturadoOffline;
                        this.Codigo = "DG";
                        this.Descricao = "Ag�ncia Ita� - Capturado offline.";
                        this.Recurso = "Dispon�vel";
                        break;
                    case EnumCodigoLiquidacao_Itau.PagamentoCartorioProtestoComCheque:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoCartorioProtestoComCheque;
                        this.Codigo = "LC";
                        this.Descricao = "Pagamento em cart�rio de protesto com cheque.";
                        this.Recurso = "A Compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.PagamentoAgendadoViaBankLine:
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoAgendadoViaBankLine;
                        this.Codigo = "Q0";
                        this.Descricao = "Agendamento - Pagamento agendado via BankLine ou outro canal eletr�nico.";
                        this.Recurso = "Dispon�vel";
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
                        this.Descricao = "Caixa eletr�nico do banco Ita�";
                        this.Codigo = "AA";
                        this.Recurso = "Dispon�vel";
                        break;
                    case "AC":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoCartorioAutomatizado;
                        this.Descricao = "Pagamento em cart�rio automatizado";
                        this.Codigo = "AC";
                        this.Recurso = "A compensar";
                        break;
                    case "BC":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.BancosCorrespondentes;
                        this.Descricao = "Bancos correspondentes";
                        this.Codigo = "BC";
                        this.Recurso = "Dispon�vel";
                        break;
                    case "BF":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.ItauBankFone;
                        this.Descricao = "Ita� Bankfone";
                        this.Codigo = "BF";
                        this.Recurso = "Dispon�vel";
                        break;
                    case "BL":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.ItauBankLine;
                        this.Descricao = "Ita� Bankline";
                        this.Codigo = "BL";
                        this.Recurso = "Dispon�vel";
                        break;
                    case "B0":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_RecebimentoOffline;
                        this.Descricao = "Outros bancos - recebimento offline";
                        this.Codigo = "B0";
                        this.Recurso = "A compensar";
                        break;
                    case "B1":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_PeloCodigoBarras;
                        this.Descricao = "Outros bancos - pelo c�digo de barras";
                        this.Codigo = "B1";
                        this.Recurso = "A compensar";
                        break;
                    case "B2":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_PelaLinhaDigitavel;
                        this.Descricao = "Outros bancos - pelo linha digit�vel";
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
                        this.Descricao = "Outros bancos - recebimento em casa lot�rica";
                        this.Codigo = "B4";
                        this.Recurso = "A compensar";
                        break;
                    case "CC":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.ComChequeOutroBanco;
                        this.Descricao = "Ag�cnia Ita� - com cheque de outro banco";
                        this.Codigo = "CC";
                        this.Recurso = "A compensar";
                        break;
                    case "CK":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.Sispag;
                        this.Descricao = "SISPAG - Sistema de contas a pagar Ita�";
                        this.Codigo = "CK";
                        this.Recurso = "Dispon�vel";
                        break;
                    case "CP":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.DebitoContaCorrente;
                        this.Descricao = "Ag�ncia Ita� - por d�bito em conta-corrente, cheque ou dinheiro";
                        this.Codigo = "CP";
                        this.Recurso = "Dispon�vel";
                        break;
                    case "DG":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.CapturadoOffline;
                        this.Descricao = "Ag�ncia Ita� - capturado em offline";
                        this.Codigo = "DG";
                        this.Recurso = "Dispon�vel";
                        break;
                    case "LC":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoCartorioProtestoComCheque;
                        this.Descricao = "Pagamento em cart�rio de protesto com cheque";
                        this.Codigo = "LC";
                        this.Recurso = "A compensar";
                        break;
                    case "Q0":
                        this.Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoAgendadoViaBankLine;
                        this.Descricao = "Agendamento - Pagamento agendado via BankLine ou outro canal eletr�nico.";
                        this.Codigo = "Q0";
                        this.Recurso = "Dispon�vel";
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
