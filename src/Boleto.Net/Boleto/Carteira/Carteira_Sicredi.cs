using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCarteiras_Sicredi
    {
        CobrancaRegistrada = 1,
        CobrancaSemRegistro = 3,
    }

    #endregion
    public class Carteira_Sicredi : AbstractCarteira, ICarteira
    {

        #region Construtores 

        public Carteira_Sicredi()
        {
            try
            {
                this.Banco = new Banco(748);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Carteira_Sicredi(int carteira)
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
                this.Banco = new Banco_Sicredi();

                switch ((EnumCarteiras_Sicredi)carteira)
                {
                    case EnumCarteiras_Sicredi.CobrancaRegistrada:
                        this.NumeroCarteira = (int)EnumCarteiras_Sicredi.CobrancaRegistrada;
                        this.Codigo = "1";
                        this.Tipo = "1";
                        this.Descricao = "Cobrança registrada";
                        break;
                    case EnumCarteiras_Sicredi.CobrancaSemRegistro:
                        this.NumeroCarteira = (int)EnumCarteiras_Sicredi.CobrancaSemRegistro;
                        this.Codigo = "3";
                        this.Tipo = "3";
                        this.Descricao = "Cobrança rápida sem registro";
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

                Carteira_Sicredi obj;

                obj = new Carteira_Sicredi((int)EnumCarteiras_Sicredi.CobrancaRegistrada);
                alCarteiras.Add(obj);

                obj = new Carteira_Sicredi((int)EnumCarteiras_Sicredi.CobrancaSemRegistro);
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
