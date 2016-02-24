using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCarteiras_Caixa
    {
        CobrancaSimples = 11,
        CobrancaRegistrada = 12,
        CobrancaSemRegistro = 14,
        CobrancaDescontada = 41,
    }

    #endregion 

    public class Carteira_Caixa: AbstractCarteira, ICarteira
    {

        #region Construtores 

		public Carteira_Caixa()
		{
			try
			{
                this.Banco = new Banco(104);
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

        public Carteira_Caixa(int carteira)
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
                this.Banco = new Banco_Caixa();

                switch ((EnumCarteiras_Caixa)carteira)
                {
                    case EnumCarteiras_Caixa.CobrancaSimples:
                        this.NumeroCarteira = (int)EnumCarteiras_Caixa.CobrancaSimples;
                        this.Codigo = "CS";
                        this.Tipo = "CS";
                        this.Descricao = "Cobrança simples";
                        break;
                    case EnumCarteiras_Caixa.CobrancaRegistrada:
                        this.NumeroCarteira = (int)EnumCarteiras_Caixa.CobrancaRegistrada;
                        this.Codigo = "RG";
                        this.Tipo = "RG";
                        this.Descricao = "Cobrança rápida registrada";
                        break;
                    case EnumCarteiras_Caixa.CobrancaSemRegistro:
                        this.NumeroCarteira = (int)EnumCarteiras_Caixa.CobrancaSemRegistro;
                        this.Codigo = "SR";
                        this.Tipo = "SR";
                        this.Descricao = "Cobrança rápida sem registro";
                        break;
                    case EnumCarteiras_Caixa.CobrancaDescontada:
                        this.NumeroCarteira = (int)EnumCarteiras_Caixa.CobrancaDescontada;
                        this.Codigo = "DE";
                        this.Tipo = "DE";
                        this.Descricao = "Cobrança descontada";
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

                Carteira_Caixa obj;

                obj = new Carteira_Caixa((int)EnumCarteiras_Caixa.CobrancaSimples);
                alCarteiras.Add(obj);

                obj = new Carteira_Caixa((int)EnumCarteiras_Caixa.CobrancaRegistrada);
                alCarteiras.Add(obj);

                obj = new Carteira_Caixa((int)EnumCarteiras_Caixa.CobrancaSemRegistro);
                alCarteiras.Add(obj);

                obj = new Carteira_Caixa((int)EnumCarteiras_Caixa.CobrancaDescontada);
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
