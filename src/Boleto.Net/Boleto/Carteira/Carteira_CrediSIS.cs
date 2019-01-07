using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCarteiras_CrediSIS
    {
        CobrancaSimples = 18
    }

    #endregion 

    public class Carteira_CrediSIS: AbstractCarteira, ICarteira
    {

        #region Construtores 

		public Carteira_CrediSIS()
		{
			try
			{
                this.Banco = new Banco(97);
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

        public Carteira_CrediSIS(int carteira)
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
                this.Banco = new Banco_CrediSis();

                switch ((EnumCarteiras_CrediSIS)carteira)
                {
                    case EnumCarteiras_CrediSIS.CobrancaSimples:
                        this.NumeroCarteira = (int)EnumCarteiras_CrediSIS.CobrancaSimples;
                        this.Codigo = "S";
                        this.Tipo = "S";
                        this.Descricao = "Cobrança simples";
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

        public static Carteiras CarregaTodas()
        {
            try
            {
                Carteiras alCarteiras = new Carteiras();

                Carteira_CrediSIS obj;

                obj = new Carteira_CrediSIS((int)EnumCarteiras_CrediSIS.CobrancaSimples);
                alCarteiras.Add(obj);

               return alCarteiras;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar objetos", ex);
            }
        }


        #endregion

    }
}
