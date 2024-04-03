using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Banestes
    {
        Protestar = 6,
        NaoProtestar = 7,
        CobrarTaxaDeMulta = 9999,
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

        public Instrucao_Banestes(int codigo, double valorDaMulta)
        {
            carregar(codigo, 0, valorDaMulta);
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
                        Descricao = string.Format("Protestar após {0} dias do vencimento", nrDias);
                        break;
                    case EnumInstrucoes_Banestes.NaoProtestar:
                        Codigo = 7;
                        Descricao = "Não protestar";
                        break;
                    default:
                        Codigo = 0;
                        Descricao = " (Selecione) ";
                        break;
                }

                QuantidadeDias = nrDias;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        private void carregar(int idInstrucao, int nrDias, double valorDaMulta)
        {
            try
            {
                Banco = new Banco_Banestes();
                switch ((EnumInstrucoes_Banestes)idInstrucao)
                {
                    case EnumInstrucoes_Banestes.Protestar:
                        Codigo = 6;
                        Descricao = string.Format("Protestar após {0} dias do vencimento", nrDias);
                        break;
                    case EnumInstrucoes_Banestes.NaoProtestar:
                        Codigo = 7;
                        Descricao = "Não protestar";
                        break;
                    case EnumInstrucoes_Banestes.CobrarTaxaDeMulta:
                        Codigo = (int)EnumInstrucoes_Banestes.CobrarTaxaDeMulta;
                        Descricao = string.Format("Após vencimento cobrar multa de {0}.", valorDaMulta.ToString("C"));
                        break;
                    default:
                        Codigo = 0;
                        Descricao = " (Selecione) ";
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
