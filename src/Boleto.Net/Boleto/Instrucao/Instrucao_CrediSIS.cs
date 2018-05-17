using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_CrediSIS
    {
        ProtestarAposNDiasUteis = 1,
        ProtestarAposNDiasCorridos = 2
        
    }

    #endregion

    public class Instrucao_CrediSIS : AbstractInstrucao, IInstrucao
    {

        #region Construtores

        public Instrucao_CrediSIS()
        {
            try
            {
                this.Banco = new Banco(097);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_CrediSIS(int codigo)
        {
            this.carregar(codigo, 0);
        }

        public Instrucao_CrediSIS(int codigo, int nrDias)
        {
            this.carregar(codigo, nrDias);
        }

        public Instrucao_CrediSIS(int codigo, double valor)
        {
            this.carregar(codigo, valor);
        }
        #endregion

        #region Metodos Privados

        private void carregar(int idInstrucao, double valor)
        {
            try
            {
                this.Banco = new Banco_CrediSis();
                this.Valida();

                switch ((EnumInstrucoes_CrediSIS)idInstrucao)
                {

                    //case EnumInstrucoes_CrediSIS.Percentual_Multa:
                    //    this.Codigo = (int)EnumInstrucoes_CrediSIS.Percentual_Multa;
                    //    this.Descricao = "Após vencimento cobrar multa de " + valor + " %";
                    //    break;
                    //case EnumInstrucoes_CrediSIS.JurosdeMora:
                    //    this.Codigo = (int)EnumInstrucoes_CrediSIS.JurosdeMora;
                    //    this.Descricao = "Após vencimento cobrar R$ " + valor + " por dia de atraso";
                    //    break;
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
                this.Banco = new Banco_CrediSis();
                this.Valida();

                switch ((EnumInstrucoes_CrediSIS)idInstrucao)
                {
                    case EnumInstrucoes_CrediSIS.ProtestarAposNDiasCorridos:
                        this.Codigo = (int)EnumInstrucoes_CrediSIS.ProtestarAposNDiasCorridos;
                        this.Descricao = "Protestar no " + nrDias + "º dia corrido após vencimento";
                        break;
                    case EnumInstrucoes_CrediSIS.ProtestarAposNDiasUteis:
                        this.Codigo = (int)EnumInstrucoes_CrediSIS.ProtestarAposNDiasUteis;
                        this.Descricao = "Protestar no " + nrDias + "º dia útil após vencimento";/*Jéferson (jefhtavares) em 02/12/2013 a pedido do setor de homologação do BB*/
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "( Selecione )";
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
