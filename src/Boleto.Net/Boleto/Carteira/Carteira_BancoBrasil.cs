using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCarteiras_BancoBrasil
    {
        CobrancaSimples = 1,
        CobrancaVinculada = 2,
        CobrancaCaucionada = 3,
        CobrancaDescontada = 4,
        CobrançaDiretaEspecialCarteira17 = 7,
    }

    #endregion 

    public class Carteira_BancoBrasil: AbstractCarteira, ICarteira
    {

        #region Construtores 

		public Carteira_BancoBrasil()
		{
			try
			{
                this.Banco = new Banco(1);
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

        public Carteira_BancoBrasil(int carteira)
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
                this.Banco = new Banco_Brasil();

                switch ((EnumCarteiras_BancoBrasil)carteira)
                {
                    case EnumCarteiras_BancoBrasil.CobrancaSimples:
                        this.NumeroCarteira = (int)EnumCarteiras_BancoBrasil.CobrancaSimples;
                        this.Codigo = "S";
                        this.Tipo = "S";
                        this.Descricao = "Cobrança simples";
                        break;
                    case EnumCarteiras_BancoBrasil.CobrancaVinculada:
                        this.NumeroCarteira = (int)EnumCarteiras_BancoBrasil.CobrancaVinculada;
                        this.Codigo = "S";
                        this.Tipo = "S";
                        this.Descricao = "Cobrança vincluada";
                        break;
                    case EnumCarteiras_BancoBrasil.CobrancaCaucionada:
                        this.NumeroCarteira = (int)EnumCarteiras_BancoBrasil.CobrancaCaucionada;
                        this.Codigo = "S";
                        this.Tipo = "S";
                        this.Descricao = "Cobrança caucionada";
                        break;
                    case EnumCarteiras_BancoBrasil.CobrancaDescontada:
                        this.NumeroCarteira = (int)EnumCarteiras_BancoBrasil.CobrancaDescontada;
                        this.Codigo = "S";
                        this.Tipo = "S";
                        this.Descricao = "Cobrança descontada";
                        break;
                    case EnumCarteiras_BancoBrasil.CobrançaDiretaEspecialCarteira17:
                        this.NumeroCarteira = (int)EnumCarteiras_BancoBrasil.CobrançaDiretaEspecialCarteira17;
                        this.Codigo = "S";
                        this.Tipo = "S";
                        this.Descricao = "Cobrança direta especial - Carteira 17";
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

                Carteira_BancoBrasil obj;

                obj = new Carteira_BancoBrasil((int)EnumCarteiras_BancoBrasil.CobrancaSimples);
                alCarteiras.Add(obj);

                obj = new Carteira_BancoBrasil((int)EnumCarteiras_BancoBrasil.CobrancaVinculada);
                alCarteiras.Add(obj);

                obj = new Carteira_BancoBrasil((int)EnumCarteiras_BancoBrasil.CobrancaCaucionada);
                alCarteiras.Add(obj);

                obj = new Carteira_BancoBrasil((int)EnumCarteiras_BancoBrasil.CobrancaDescontada);
                alCarteiras.Add(obj);

                obj = new Carteira_BancoBrasil((int)EnumCarteiras_BancoBrasil.CobrançaDiretaEspecialCarteira17);
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
