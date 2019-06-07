using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Safra
    {
        PedidoBaixa = 2,
        ConcessaoAbatimento = 4,
        CancelamentoAbatimentoConcedido = 5,
        AlteracaoVencimento = 6,
        Protestar = 9,
        NaoProtestar = 10,
        NaoCobrarJurosDeMora = 11,
        JurosdeMora = 16,
        AlteracaoOutrosDados = 31
    }

    #endregion

    public class Instrucao_Safra : AbstractInstrucao, IInstrucao
    {
        #region Construtores

        public Instrucao_Safra()
        {
            try
            {
                this.Banco = new Banco(422);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_Safra(int codigo)
        {
            this.carregar(codigo, 0);
        }

        public Instrucao_Safra(int codigo, int nrDias)
        {
            this.carregar(codigo, nrDias);
        }
        #endregion Construtores

        #region Metodos Privados

        private void carregar(int idInstrucao, int nrDias)
        {
            try
            {
                this.Banco = new Banco_Safra();
                this.Valida();

                switch ((EnumInstrucoes_Safra)idInstrucao)
                {
                    case EnumInstrucoes_Safra.PedidoBaixa:
                        this.Codigo = (int)EnumInstrucoes_Safra.PedidoBaixa;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Safra.ConcessaoAbatimento:
                        this.Codigo = (int)EnumInstrucoes_Safra.ConcessaoAbatimento;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Safra.CancelamentoAbatimentoConcedido:
                        this.Codigo = (int)EnumInstrucoes_Safra.CancelamentoAbatimentoConcedido;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Safra.AlteracaoVencimento:
                        this.Codigo = (int)EnumInstrucoes_Safra.AlteracaoVencimento;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Safra.Protestar:
                        this.Codigo = (int)EnumInstrucoes_Safra.Protestar;
                        this.Descricao = "PROTESTAR APÓS " + nrDias + " DIAS ÚTEIS DO VENCIMENTO";
                        break;
                    case EnumInstrucoes_Safra.NaoProtestar:
                        this.Codigo = (int)EnumInstrucoes_Safra.NaoProtestar;
                        this.Descricao = "Não protestar";
                        break;
                    case EnumInstrucoes_Safra.NaoCobrarJurosDeMora:
                        this.Codigo = (int)EnumInstrucoes_Safra.NaoCobrarJurosDeMora;
                        this.Descricao = "Não cobrar juros de mora";
                        break;
                    case EnumInstrucoes_Safra.JurosdeMora:
                        this.Codigo = (int)EnumInstrucoes_Safra.JurosdeMora;
                        this.Descricao = "Após vencimento cobrar R$ "; // por dia de atraso
                        break;
                    case EnumInstrucoes_Safra.AlteracaoOutrosDados:
                        this.Codigo = (int)EnumInstrucoes_Safra.AlteracaoOutrosDados;
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
