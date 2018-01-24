using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Banestes
    {
        Protestar = 6,
        NaoProtestar = 7
    }

    #endregion 

    public class Instrucao_Banestes: AbstractInstrucao, IInstrucao
    {

        #region Construtores 

        public Instrucao_Banestes()
        {
            try
            {
                Banco = new Banco(21);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_Banestes(int codigo, int nrDias)
        {
            carregar(codigo, nrDias);
        }


        #endregion

        #region Metodos Privados

        private void carregar (int idInstrucao, int nrDias)
        {
            try
            {
                Banco = new Banco_Banestes();
                Valida();

                switch ((EnumInstrucoes_Banestes)idInstrucao)
                {
                    case EnumInstrucoes_Banestes.Protestar:
                        Codigo = 6;
                        Descricao = $"Protestar após {nrDias} dias do vencimento";
                        break;
                    case EnumInstrucoes_Banestes.NaoProtestar:
                        Codigo = 7;
                        Descricao = "Não protestar";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = " (Selecione) ";
                        break;
                }

                QuantidadeDias = nrDias;
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
