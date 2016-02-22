using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_BankBoston
    {
        SemInstrucao = 0,
        Protestar = 1,
        NaoProtestar = 2,
        Devolver = 3,
    }

    #endregion 

    public class Instrucao_BankBoston: AbstractInstrucao, IInstrucao
    {

        #region Construtores 

		public Instrucao_BankBoston()
		{
			try
			{
                this.Banco = new Banco(479);
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

        public Instrucao_BankBoston(int codigo)
        {
            this.carregar(codigo, 0);
        }

        public Instrucao_BankBoston(int codigo, int nrDias)
        {
            this.carregar(codigo, nrDias);
        }

		#endregion 

        #region Metodos Privados

        private void carregar(int idInstrucao, int nrDias)
        {
            try
            {
                this.Banco = new Banco_BankBoston();
                this.Valida();

                switch ((EnumInstrucoes_BankBoston)idInstrucao)
                {
                    case EnumInstrucoes_BankBoston.SemInstrucao:
                        this.Codigo = (int)EnumInstrucoes_BankBoston.SemInstrucao;
                        this.Descricao = "Sem instrução";
                        break;
                    case EnumInstrucoes_BankBoston.Protestar:
                        this.Codigo = (int)EnumInstrucoes_BankBoston.Protestar;
                        this.Descricao = "Protestar após 5 dias úteis";
                        break;
                    case EnumInstrucoes_BankBoston.NaoProtestar:
                        this.Codigo = (int)EnumInstrucoes_BankBoston.NaoProtestar;
                        this.Descricao = "Não protestar";
                        break;
                    case EnumInstrucoes_BankBoston.Devolver:
                        this.Codigo = (int)EnumInstrucoes_BankBoston.Devolver;
                        this.Descricao = "Devolver após 5 dias";
                        break;
                    default:
                        this.Codigo = 99;
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
