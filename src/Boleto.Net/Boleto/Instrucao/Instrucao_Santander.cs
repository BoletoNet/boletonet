using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Santander
    {
        BaixarApos15Dias = 02,
        BaixarApos30Dias = 03,
        NaoBaixar = 04,
        Protestar = 06,
        NaoProtestar = 07,
        NaoCobrarJurosDeMora = 08
    }

    #endregion

    public class Instrucao_Santander : AbstractInstrucao, IInstrucao
    {

        #region Construtores
        public Instrucao_Santander()
        {
            try
            {
                this.Banco = new Banco(33);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_Santander(int codigo)
        {
            this.carregar(codigo, 0);
        }

        public Instrucao_Santander(int codigo, int nrDias)
        {
            this.carregar(codigo, nrDias);
        }
        #endregion

        #region Metodos Privados

        private void carregar(int idInstrucao, int nrDias)
        {
            try
            {
                this.Banco = new Banco_Santander();

                switch ((EnumInstrucoes_Santander)idInstrucao)
                {
                    case EnumInstrucoes_Santander.BaixarApos15Dias:
                        this.Codigo = (int)EnumInstrucoes_Santander.BaixarApos15Dias;
                        this.Descricao = "Baixar após quinze dias do vencimento";
                        break;
                    case EnumInstrucoes_Santander.BaixarApos30Dias:
                        this.Codigo = (int)EnumInstrucoes_Santander.BaixarApos30Dias;
                        this.Descricao = "Baixar após 30 dias do vencimento";
                        break;
                    case EnumInstrucoes_Santander.NaoBaixar:
                        this.Codigo = (int)EnumInstrucoes_Santander.NaoBaixar;
                        this.Descricao = "Não baixar";
                        break;
                    case EnumInstrucoes_Santander.Protestar:
                        this.Codigo = (int)EnumInstrucoes_Santander.Protestar;
                        this.Descricao = "Protestar após "+nrDias+" do vencimento";
                        this.QuantidadeDias = nrDias;
                        break;
                    case EnumInstrucoes_Santander.NaoProtestar:
                        this.Codigo = (int)EnumInstrucoes_Santander.NaoProtestar;
                        this.Descricao = "Não protestar";
                        break;
                    case EnumInstrucoes_Santander.NaoCobrarJurosDeMora:
                        this.Codigo = (int)EnumInstrucoes_Santander.NaoCobrarJurosDeMora;
                        this.Descricao = "Não cobrar juros de mora";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "";
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
