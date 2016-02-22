using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCarteiras_Santander
    {
        //101-Cobrança Simples Rápida COM Registro
        CobrancaSimplesComRegistro = 101,
        //102- Cobrança simples – SEM Registro
        CobrancaSimplesSemRegistro = 102,
        //201- Penhor Rápida com Registro
        PenhorRapida = 201

  //CC - Cobrança Caucionada
  //CD - Cobrança Descontada
  //CSR - Cobrança Simples Sem Registro
  //ECR - Cobrança Simples Com Registro
  //ECR2 - Cobrança Simples Com Registro - Emissão Banco
  //PENHOR - Penhor Rápida com Registro
  //PENHOR-Eletron - Penhor Eletrônica com Registro
    }
    #endregion Enumerado

    public class Carteira_Santander: AbstractCarteira, ICarteira
    {

        #region Construtores 

		public Carteira_Santander()
		{
			try
			{
                this.Banco = new Banco(33);
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

        public Carteira_Santander(int carteira)
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
                this.Banco = new Banco_Santander();

                switch ((EnumCarteiras_Santander)carteira)
                {
                    case EnumCarteiras_Santander.CobrancaSimplesComRegistro:
                        this.NumeroCarteira = (int)EnumCarteiras_Santander.CobrancaSimplesComRegistro;
                        this.Codigo = "ECR";
                        this.Tipo = "";
                        this.Descricao = "Cobrança Simples Com Registro";
                        break;
                    case EnumCarteiras_Santander.CobrancaSimplesSemRegistro:
                        this.NumeroCarteira = (int)EnumCarteiras_Santander.CobrancaSimplesSemRegistro;
                        this.Codigo = "CSR";
                        this.Tipo = "";
                        this.Descricao = "Cobrança Simples Sem Registro";
                        break;
                    case EnumCarteiras_Santander.PenhorRapida:
                        this.NumeroCarteira = (int)EnumCarteiras_Santander.PenhorRapida;
                        this.Codigo = "CSR";
                        this.Tipo = "";
                        this.Descricao = "Penhor Rápida com Registro";
                        break;
                    default:
                        this.NumeroCarteira = 0;
                        this.Codigo = " ";
                        this.Tipo = " ";
                        this.Descricao = "";
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

                Carteira_Santander obj;

                obj = new Carteira_Santander((int)EnumCarteiras_Santander.CobrancaSimplesComRegistro);
                alCarteiras.Add(obj);

                obj = new Carteira_Santander((int)EnumCarteiras_Santander.CobrancaSimplesSemRegistro);
                alCarteiras.Add(obj);

                obj = new Carteira_Santander((int)EnumCarteiras_Santander.PenhorRapida);
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