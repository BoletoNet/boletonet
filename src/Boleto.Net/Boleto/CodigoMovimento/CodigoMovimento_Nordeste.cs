using System;
using System.Collections.Generic;
using System.Text;
using BoletoNet.Enums;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCodigoMovimento_Nordeste
    {
        EntradaConfirmada = 2,
        Alteracao = 4,
        Liquidacao = 6,
        PagamentoPorConta = 7,
        PagamentoPorCartorio = 8,
        Baixa = 9,
        DevolvidoProtestado = 10,
        EmSer = 11,
        AbatimentoConcedido = 12,
        AbatimentoCancelado = 13,
        VencimentoAlterado = 14,
        BaixaAutomatica = 15,
        AlteracaoDepositaria = 18,
        ConfirmacaoProtesto = 19,
        ConfirmacaoSustarProtesto = 20,
        AlteracaoInformacoesControleEmpresa = 21,
        AlteracaoSeuNumero = 22,
        EntradaRejeitada = 51,

    }

    #endregion

    public class CodigoMovimento_Nordeste : AbstractCodigoMovimento, ICodigoMovimento
    {
        #region Construtores

        public CodigoMovimento_Nordeste()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public CodigoMovimento_Nordeste(int codigo)
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
                this.Banco = new Banco_Nordeste();

                switch ((EnumCodigoMovimento_Nordeste)codigo)
                {
                    case EnumCodigoMovimento_Nordeste.EntradaConfirmada:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.EntradaConfirmada;
                        this.Descricao = "Entrada confirmada";
                        break;
                    case EnumCodigoMovimento_Nordeste.Alteracao:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.Alteracao;
                        this.Descricao = "Alteração";
                        break;
                    case EnumCodigoMovimento_Nordeste.EntradaRejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.EntradaRejeitada;
                        this.Descricao = "Entrada rejeitada";
                        break;
                    case EnumCodigoMovimento_Nordeste.Liquidacao:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.Liquidacao;
                        this.Descricao = "Liquidação normal";
                        break;
                    case EnumCodigoMovimento_Nordeste.Baixa:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.Baixa;
                        this.Descricao = "Baixa Simples";
                        break;
                    case EnumCodigoMovimento_Nordeste.PagamentoPorConta:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.PagamentoPorConta;
                        this.Descricao = "Pagamento por Conta";
                        break;
                    case EnumCodigoMovimento_Nordeste.PagamentoPorCartorio:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.PagamentoPorCartorio;
                        this.Descricao = "Pagamento por Cartório";
                        break;
                    case EnumCodigoMovimento_Nordeste.DevolvidoProtestado:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.DevolvidoProtestado;
                        this.Descricao = "Devolvido / Protestado";
                        break;
                    case EnumCodigoMovimento_Nordeste.EmSer:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.DevolvidoProtestado;
                        this.Descricao = "Devolvido / Protestado";
                        break;
                    case EnumCodigoMovimento_Nordeste.AbatimentoConcedido:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.AbatimentoConcedido;
                        this.Descricao = "Abatimento Concedido";
                        break;
                    case EnumCodigoMovimento_Nordeste.AbatimentoCancelado:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.AbatimentoCancelado;
                        this.Descricao = "Abatimento Cancelado";
                        break;
                    case EnumCodigoMovimento_Nordeste.VencimentoAlterado:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.VencimentoAlterado;
                        this.Descricao = "Vencimento Alterado";
                        break;
                    case EnumCodigoMovimento_Nordeste.BaixaAutomatica:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.BaixaAutomatica;
                        this.Descricao = "Baixa Automática";
                        break;
                    case EnumCodigoMovimento_Nordeste.AlteracaoDepositaria:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.AlteracaoDepositaria;
                        this.Descricao = "Alteração Depositária";
                        break;
                    case EnumCodigoMovimento_Nordeste.ConfirmacaoProtesto:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.ConfirmacaoProtesto;
                        this.Descricao = "Confirmação de Protesto";
                        break;
                    case EnumCodigoMovimento_Nordeste.ConfirmacaoSustarProtesto:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.ConfirmacaoSustarProtesto;
                        this.Descricao = "Confirmação de Sustar Protesto";
                        break;
                    case EnumCodigoMovimento_Nordeste.AlteracaoInformacoesControleEmpresa:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.AlteracaoInformacoesControleEmpresa;
                        this.Descricao = "Alteração Informações de Controle da Empresa";
                        break;
                    case EnumCodigoMovimento_Nordeste.AlteracaoSeuNumero:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.AlteracaoSeuNumero;
                        this.Descricao = "Alteração Seu Número";
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
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.EntradaConfirmada;
                        this.Descricao = "Entrada confirmada";
                        break;
                    case 4:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.Alteracao;
                        this.Descricao = "Alteração";
                        break;
                    case 6:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.Liquidacao;
                        this.Descricao = "Liquidação normal";
                        break;
                    case 7:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.PagamentoPorConta;
                        this.Descricao = "Pagamento por Conta";
                        break;
                    case 8:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.PagamentoPorCartorio;
                        this.Descricao = "Pagamento por Cartório";
                        break;
                    case 9:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.Baixa;
                        this.Descricao = "Baixa Simples";
                        break;
                    case 10:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.DevolvidoProtestado;
                        this.Descricao = "Devolvido / Protestado";
                        break;
                    case 11:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.EmSer;
                        this.Descricao = "Em Ser";
                        break;
                    case 12:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.AbatimentoConcedido;
                        this.Descricao = "Abatimento Concedido";
                        break;
                    case 13:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.AbatimentoCancelado;
                        this.Descricao = "Abatimento Cancelado";
                        break;
                    case 14:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.VencimentoAlterado;
                        this.Descricao = "Vencimento Alterado";
                        break;
                    case 15:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.BaixaAutomatica;
                        this.Descricao = "Baixa Automática";
                        break;
                    case 18:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.AlteracaoDepositaria;
                        this.Descricao = "Alteração Depositária";
                        break;
                    case 19:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.ConfirmacaoProtesto;
                        this.Descricao = "Confirmação de Protesto";
                        break;
                    case 20:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.ConfirmacaoSustarProtesto;
                        this.Descricao = "Confirmação de Sustar Protesto";
                        break;
                    case 21:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.AlteracaoInformacoesControleEmpresa;
                        this.Descricao = "Alteração Informações de Controle da Empresa";
                        break;
                    case 22:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.AlteracaoSeuNumero;
                        this.Descricao = "Alteração Seu Número";
                        break;
                    case 51:
                        this.Codigo = (int)EnumCodigoMovimento_Nordeste.EntradaRejeitada;
                        this.Descricao = "Entrada Rejeitada";
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

        public override TipoOcorrenciaRetorno ObterCorrespondenteFebraban()
        {
            return ObterCorrespondenteFebraban(correspondentesFebraban, (EnumCodigoMovimento_Nordeste)Codigo);
        }

        private Dictionary<EnumCodigoMovimento_Nordeste, TipoOcorrenciaRetorno> correspondentesFebraban = new Dictionary<EnumCodigoMovimento_Nordeste, TipoOcorrenciaRetorno>()
        {
            {EnumCodigoMovimento_Nordeste.EntradaConfirmada, TipoOcorrenciaRetorno.EntradaConfirmada},
            {EnumCodigoMovimento_Nordeste.EntradaRejeitada, TipoOcorrenciaRetorno.EntradaRejeitada},
            {EnumCodigoMovimento_Nordeste.Liquidacao, TipoOcorrenciaRetorno.Liquidacao},
            {EnumCodigoMovimento_Nordeste.Baixa, TipoOcorrenciaRetorno.Baixa},
        };
    }
}
