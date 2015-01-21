using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    // Códigos de tarifas de 1 a 11 associados ao código de movimento 28

    #region Enumerado

    public enum EnumCodigoTarifas_BancoBrasil
    {
        TarifaDeExtratoDePosicao = 1,
        TarifaDeManutencaoTituloVencido = 2,
        TarifaDeSustacao = 3,
        TarifaDeProtesto = 4,
        TarifaDeOutrasInstrucoes = 5,
        TarifaDeOutrasOcorrencias = 6,
        TarifaDeEnvioDeDuplicataAoSacado = 7,
        CustasDeProtesto = 8,
        CustasDeSustacaoDeProtesto = 9,
        CustasDoCartorioDistribuidor = 10,
        CustasDeEdital = 11,
    }

    #endregion 

    public class CodigoTarifas_BancoBrasil: AbstractCodigoTarifas, ICodigoTarifas
    {
        #region Construtores 

		public CodigoTarifas_BancoBrasil()
		{
			try
			{
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

        public CodigoTarifas_BancoBrasil(int codigo)
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

        private void carregar(int codigo)
        {
            try
            {
                this.Banco = new Banco_Brasil();

                switch ((EnumCodigoTarifas_BancoBrasil)codigo)
                {
                    case  EnumCodigoTarifas_BancoBrasil.TarifaDeExtratoDePosicao:
                        this.Codigo = (int)EnumCodigoTarifas_BancoBrasil.TarifaDeExtratoDePosicao;
                        this.Descricao = "Tarifa de extrato de posição";
                        break;
                    case EnumCodigoTarifas_BancoBrasil.TarifaDeManutencaoTituloVencido:
                        this.Codigo = (int)EnumCodigoTarifas_BancoBrasil.TarifaDeManutencaoTituloVencido;
                        this.Descricao = "Tarifa de manutenção de título vencido";
                        break;
                    case EnumCodigoTarifas_BancoBrasil.TarifaDeSustacao:
                        this.Codigo = (int)EnumCodigoTarifas_BancoBrasil.TarifaDeSustacao;
                        this.Descricao = "Tarifa de sustação";
                        break;
                    case EnumCodigoTarifas_BancoBrasil.TarifaDeProtesto:
                        this.Codigo = (int)EnumCodigoTarifas_BancoBrasil.TarifaDeProtesto;
                        this.Descricao = "Tarifa de protesto";
                        break;
                    case EnumCodigoTarifas_BancoBrasil.TarifaDeOutrasInstrucoes:
                        this.Codigo = (int)EnumCodigoTarifas_BancoBrasil.TarifaDeOutrasInstrucoes;
                        this.Descricao = "Tarifa de outras instruções";
                        break;
                    case EnumCodigoTarifas_BancoBrasil.TarifaDeOutrasOcorrencias:
                        this.Codigo = (int)EnumCodigoTarifas_BancoBrasil.TarifaDeOutrasOcorrencias;
                        this.Descricao = "Tarifa de outras ocorrências";
                        break;
                    case EnumCodigoTarifas_BancoBrasil.TarifaDeEnvioDeDuplicataAoSacado:
                        this.Codigo = (int)EnumCodigoTarifas_BancoBrasil.TarifaDeEnvioDeDuplicataAoSacado;
                        this.Descricao = "Tarifa de envio de duplicata ao sacado";
                        break;
                    case EnumCodigoTarifas_BancoBrasil.CustasDeProtesto:
                        this.Codigo = (int)EnumCodigoTarifas_BancoBrasil.CustasDeProtesto;
                        this.Descricao = "Custas de protesto";
                        break;
                    case EnumCodigoTarifas_BancoBrasil.CustasDeSustacaoDeProtesto:
                        this.Codigo = (int)EnumCodigoTarifas_BancoBrasil.CustasDeSustacaoDeProtesto;
                        this.Descricao = "Custas de sustação de protesto";
                        break;
                    case EnumCodigoTarifas_BancoBrasil.CustasDoCartorioDistribuidor:
                        this.Codigo = (int)EnumCodigoTarifas_BancoBrasil.CustasDoCartorioDistribuidor;
                        this.Descricao = "Custas do cartório distribuidor";
                        break;
                    case EnumCodigoTarifas_BancoBrasil.CustasDeEdital:
                        this.Codigo = (int)EnumCodigoTarifas_BancoBrasil.CustasDeEdital;
                        this.Descricao = "Custas de edital";
                        break;
                    default:
                        this.Codigo = 0;
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
