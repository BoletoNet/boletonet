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
        NaoCobrarJurosDeMora = 08,

        Multa = 80,
        ProtestarAposNDiasCorridos = 81,
        ProtestarAposNDiasUteis = 82,
    }

    #endregion

    public sealed class Instrucao_Santander : AbstractInstrucao, IInstrucao
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
            this.Carregar(codigo, 0);
        }

        public Instrucao_Santander(int codigo, int nrDias)
        {
            this.Carregar(codigo, nrDias);
        }

        public Instrucao_Santander(int codigo, decimal valorJurosMulta)
        {
            this.Carregar(codigo, valorJurosMulta);
        }
        #endregion

        #region Metodos Privados

        public override bool Carregar(int idInstrucao, decimal valor) {
            try {
                this.Banco = new Banco_Santander();

                switch ((EnumInstrucoes_Santander)idInstrucao) {
                    case EnumInstrucoes_Santander.Multa:
                        this.Codigo = (int)EnumInstrucoes_Santander.Multa;
                        this.Descricao = "Ap�s vencimento cobrar Multa de " + valor + "%";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "";
                        break;
                }

                return Codigo > 0;
            } catch (Exception ex) {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public override bool Carregar(int idInstrucao, int nrDias) {
            try {
                this.Banco = new Banco_Santander();

                switch ((EnumInstrucoes_Santander)idInstrucao) {
                    case EnumInstrucoes_Santander.BaixarApos15Dias:
                        this.Codigo = (int)EnumInstrucoes_Santander.BaixarApos15Dias;
                        this.Descricao = "Baixar ap�s quinze dias do vencimento";
                        break;
                    case EnumInstrucoes_Santander.BaixarApos30Dias:
                        this.Codigo = (int)EnumInstrucoes_Santander.BaixarApos30Dias;
                        this.Descricao = "Baixar ap�s 30 dias do vencimento";
                        break;
                    case EnumInstrucoes_Santander.NaoBaixar:
                        this.Codigo = (int)EnumInstrucoes_Santander.NaoBaixar;
                        this.Descricao = "N�o baixar";
                        break;
                    case EnumInstrucoes_Santander.Protestar:
                        this.Codigo = (int)EnumInstrucoes_Santander.Protestar;
                        this.Descricao = "Protestar ap�s " + nrDias + " do vencimento";
                        this.QuantidadeDias = nrDias;
                        break;
                    case EnumInstrucoes_Santander.NaoProtestar:
                        this.Codigo = (int)EnumInstrucoes_Santander.NaoProtestar;
                        this.Descricao = "N�o protestar";
                        break;
                    case EnumInstrucoes_Santander.NaoCobrarJurosDeMora:
                        this.Codigo = (int)EnumInstrucoes_Santander.NaoCobrarJurosDeMora;
                        this.Descricao = "N�o cobrar juros de mora";
                        break;
                    case EnumInstrucoes_Santander.ProtestarAposNDiasCorridos:
                        Codigo = (int)EnumInstrucoes_Santander.ProtestarAposNDiasCorridos;
                        Descricao = "Protestar no " + nrDias + "� dia corrido ap�s vencimento";
                        break;
                    case EnumInstrucoes_Santander.ProtestarAposNDiasUteis:
                        Codigo = (int)EnumInstrucoes_Santander.ProtestarAposNDiasUteis;
                        Descricao = "Protestar no " + nrDias + "� dia �til ap�s vencimento";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "";
                        break;
                }

                this.QuantidadeDias = nrDias;

                return Codigo > 0;
            } catch (Exception ex) {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        #endregion

    }
}
