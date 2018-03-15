using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCarteiras_Safra
    {
        CobrancaRegistrada = 1,
        CobrancaSemRegistro = 3,
    }

    #endregion
    public class Carteira_Safra : AbstractCarteira, ICarteira
    {

        #region Construtores 

        public Carteira_Safra()
        {
            try
            {
                this.Banco = new Banco(422);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Carteira_Safra(int carteira)
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
                this.Banco = new Banco_Safra();

                switch ((EnumCarteiras_Safra)carteira)
                {
                    case EnumCarteiras_Safra.CobrancaRegistrada:
                        this.NumeroCarteira = (int)EnumCarteiras_Safra.CobrancaRegistrada;
                        this.Codigo = "1";
                        this.Tipo = "1";
                        this.Descricao = "Cobrança registrada";
                        break;
                    case EnumCarteiras_Safra.CobrancaSemRegistro:
                        this.NumeroCarteira = (int)EnumCarteiras_Safra.CobrancaSemRegistro;
                        this.Codigo = "3";
                        this.Tipo = "3";
                        this.Descricao = "Cobrança sem registro";
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

                Carteira_Safra obj;

                obj = new Carteira_Safra((int)EnumCarteiras_Safra.CobrancaRegistrada);
                alCarteiras.Add(obj);

                obj = new Carteira_Safra((int)EnumCarteiras_Safra.CobrancaSemRegistro);
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
