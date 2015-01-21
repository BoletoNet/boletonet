using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    // Códigos de liquidação de 1 a 13 associados aos códigos de movimento 06, 09 e 17

    #region Enumerado

    public enum EnumCodigoLiquidacao_BancoBrasil
    {
        PorSaldo = 1,
        PorConta = 2,
        NoProprioBanco = 3,
        CompensacaoEletronica = 4,
        CompensacaoConvencional = 5,
        PorMeioEletronico = 6,
        AposFeriadoLocal = 7,
        EmCartorio = 8,
        ComandadaBanco = 9,
        ComandadaClienteArquivo = 10,
        ComandadaClienteOnline = 11,
        DecursoDePrazoCliente = 12,
        DecursoDePrazoBanco = 13,        
    }

    #endregion 

    public class CodigoLiquidacao_BancoBrasil: AbstractCodigoLiquidacao, ICodigoLiquidacao
    {
        #region Construtores 

		public CodigoLiquidacao_BancoBrasil()
		{
			try
			{
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

        public CodigoLiquidacao_BancoBrasil(int codigo)
        {
            try
            {
                this.carregar(codigo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

		#endregion 

        #region Metodos Privados

        private void carregar(int idCodigo)
        {
            try
            {
                this.Banco = new Banco_Brasil();

                switch ((EnumCodigoLiquidacao_BancoBrasil)idCodigo)
                {
                    case EnumCodigoLiquidacao_BancoBrasil.PorSaldo:
                        this.Enumerado = (int)EnumCodigoLiquidacao_BancoBrasil.PorSaldo;
                        this.Codigo = "";
                        this.Descricao = "Por saldo";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacao_BancoBrasil.PorConta:
                        this.Enumerado = (int)EnumCodigoLiquidacao_BancoBrasil.PorConta;
                        this.Codigo = "";
                        this.Descricao = "Por conta";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacao_BancoBrasil.NoProprioBanco:
                        this.Enumerado = (int)EnumCodigoLiquidacao_BancoBrasil.NoProprioBanco;
                        this.Codigo = "";
                        this.Descricao = "No próprio banco";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacao_BancoBrasil.CompensacaoEletronica:
                        this.Enumerado = (int)EnumCodigoLiquidacao_BancoBrasil.CompensacaoEletronica;
                        this.Codigo = "";
                        this.Descricao = "Compensação eletrônica";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacao_BancoBrasil.CompensacaoConvencional:
                        this.Enumerado = (int)EnumCodigoLiquidacao_BancoBrasil.CompensacaoConvencional;
                        this.Codigo = "";
                        this.Descricao = "Compensação convencional";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacao_BancoBrasil.PorMeioEletronico:
                        this.Enumerado = (int)EnumCodigoLiquidacao_BancoBrasil.PorMeioEletronico;
                        this.Codigo = "";
                        this.Descricao = "Por meio eletrônico";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacao_BancoBrasil.AposFeriadoLocal:
                        this.Enumerado = (int)EnumCodigoLiquidacao_BancoBrasil.AposFeriadoLocal;
                        this.Codigo = "";
                        this.Descricao = "Após feriado nacional";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacao_BancoBrasil.EmCartorio:
                        this.Enumerado = (int)EnumCodigoLiquidacao_BancoBrasil.EmCartorio;
                        this.Codigo = "";
                        this.Descricao = "Em cartório";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacao_BancoBrasil.ComandadaBanco:
                        this.Enumerado = (int)EnumCodigoLiquidacao_BancoBrasil.ComandadaBanco;
                        this.Codigo = "";
                        this.Descricao = "Comandada banco";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacao_BancoBrasil.ComandadaClienteArquivo:
                        this.Enumerado = (int)EnumCodigoLiquidacao_BancoBrasil.ComandadaClienteArquivo;
                        this.Codigo = "";
                        this.Descricao = "Comandada cliente - arquivo";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacao_BancoBrasil.ComandadaClienteOnline:
                        this.Enumerado = (int)EnumCodigoLiquidacao_BancoBrasil.ComandadaClienteOnline;
                        this.Codigo = "";
                        this.Descricao = "Comandada cliente - online";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacao_BancoBrasil.DecursoDePrazoCliente:
                        this.Enumerado = (int)EnumCodigoLiquidacao_BancoBrasil.DecursoDePrazoCliente;
                        this.Codigo = "";
                        this.Descricao = "Decurso de prazo - cliente";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacao_BancoBrasil.DecursoDePrazoBanco:
                        this.Enumerado = (int)EnumCodigoLiquidacao_BancoBrasil.DecursoDePrazoBanco;
                        this.Codigo = "";
                        this.Descricao = "Decurso de prazo - banco";
                        this.Recurso = "";
                        break;
                    default:
                        this.Enumerado = 0;
                        this.Codigo = "";
                        this.Descricao = "( Selecione )";
                        this.Recurso = "";
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
