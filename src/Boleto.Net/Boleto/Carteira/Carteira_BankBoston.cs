using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCarteiras_BankBoston
    {
        Simples = 1,
        Direta = 2,
        Escritural = 3,
        Caucao = 4,
    }

    #endregion 

    public class Carteira_BankBoston: AbstractCarteira, ICarteira
    {

        #region Construtores 

		public Carteira_BankBoston()
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

        public Carteira_BankBoston(int carteira)
        {
            try
            {
                this.carregar(carteira);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

		#endregion 

        #region Metodos Privados

        private void carregar(int carteira)
        {
            try
            {
                this.Banco = new Banco_BankBoston();

                switch ((EnumCarteiras_BankBoston)carteira)
                {
                    case EnumCarteiras_BankBoston.Simples:
                        this.NumeroCarteira = (int)EnumCarteiras_BankBoston.Simples;
                        this.Codigo = "1";
                        this.Tipo = "1";
                        this.Descricao = "Simples";
                        break;
                    case EnumCarteiras_BankBoston.Direta:
                        this.NumeroCarteira = (int)EnumCarteiras_BankBoston.Direta;
                        this.Codigo = "2";
                        this.Tipo = "2";
                        this.Descricao = "Direta";
                        break;
                    case EnumCarteiras_BankBoston.Escritural:
                        this.NumeroCarteira = (int)EnumCarteiras_BankBoston.Escritural;
                        this.Codigo = "3";
                        this.Tipo = "3";
                        this.Descricao = "Escritural";
                        break;
                    case EnumCarteiras_BankBoston.Caucao:
                        this.NumeroCarteira = (int)EnumCarteiras_BankBoston.Caucao;
                        this.Codigo = "4";
                        this.Tipo = "4";
                        this.Descricao = "Caução";
                        break;
                    default:
                        this.NumeroCarteira = 0;
                        this.Codigo = " ";
                        this.Tipo = " ";
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

    }
}
