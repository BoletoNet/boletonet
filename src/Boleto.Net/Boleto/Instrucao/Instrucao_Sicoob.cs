namespace BoletoNet
{
	using System;

	public enum EnumInstrucoes_Sicoob
	{
		Protestar = 9, // Emite aviso ao sacado ap�s N dias do vencto, e envia ao cart�rio ap�s 5 dias �teis

		NaoProtestar = 10, // Inibe protesto, quando houver instru��o permanente na conta corrente

		ImportanciaporDiaDesconto = 30, 

		ProtestoFinsFalimentares = 42, 

		ProtestarAposNDiasCorridos = 81, 

		ProtestarAposNDiasUteis = 82, 

		NaoReceberAposNDias = 91, 

		DevolverAposNDias = 92, 

		JurosdeMora = 998, 

		DescontoporDia = 999
	}

	public class Instrucao_Sicoob : AbstractInstrucao
	{
		#region Metodos Privados

		private void carregar(int idInstrucao, int nrDias, double valorMultaDesconto)
		{
			try
			{
				switch ((EnumInstrucoes_Sicoob)idInstrucao)
				{
					case EnumInstrucoes_Sicoob.Protestar:
						this.Codigo = (int)EnumInstrucoes_Sicoob.Protestar;
						this.Descricao = "Protestar ap�s " + nrDias + " dias �teis.";
						break;
					case EnumInstrucoes_Sicoob.NaoProtestar:
						this.Codigo = (int)EnumInstrucoes_Sicoob.NaoProtestar;
						this.Descricao = "N�o protestar";
						break;
					case EnumInstrucoes_Sicoob.ImportanciaporDiaDesconto:
						this.Codigo = (int)EnumInstrucoes_Sicoob.ImportanciaporDiaDesconto;
						this.Descricao = "Import�ncia por dia de desconto.";
						break;
					case EnumInstrucoes_Sicoob.ProtestoFinsFalimentares:
						this.Codigo = (int)EnumInstrucoes_Sicoob.ProtestoFinsFalimentares;
						this.Descricao = "Protesto para fins falimentares";
						break;
					case EnumInstrucoes_Sicoob.ProtestarAposNDiasCorridos:
						this.Codigo = (int)EnumInstrucoes_Sicoob.ProtestarAposNDiasCorridos;
						this.Descricao = "Protestar ap�s " + nrDias + " dias corridos do vencimento";
						break;
					case EnumInstrucoes_Sicoob.ProtestarAposNDiasUteis:
						this.Codigo = (int)EnumInstrucoes_Sicoob.ProtestarAposNDiasUteis;
						this.Descricao = "Protestar ap�s " + nrDias + " dias �teis do vencimento";
						break;
					case EnumInstrucoes_Sicoob.NaoReceberAposNDias:
						this.Codigo = (int)EnumInstrucoes_Sicoob.NaoReceberAposNDias;
						this.Descricao = "N�o receber ap�s " + nrDias + " dias do vencimento";
						break;
					case EnumInstrucoes_Sicoob.DevolverAposNDias:
						this.Codigo = (int)EnumInstrucoes_Sicoob.DevolverAposNDias;
						this.Descricao = "Devolver ap�s " + nrDias + " dias do vencimento";
						break;
					case EnumInstrucoes_Sicoob.JurosdeMora:
						this.Codigo = (int)EnumInstrucoes_Sicoob.JurosdeMora;
						this.Descricao = "Ap�s vencimento cobrar R$ " + valorMultaDesconto + " por dia de atraso";
						break;
					case EnumInstrucoes_Sicoob.DescontoporDia:
						this.Codigo = (int)EnumInstrucoes_Sicoob.DescontoporDia;
						this.Descricao = "Conceder desconto de R$ " + valorMultaDesconto; // por dia de antecipa��o
						break;
					default:
						this.Codigo = 0;
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

		#endregion

		#region Construtores

		public Instrucao_Sicoob()
		{
			try
			{
				this.Banco = new Banco(756);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao carregar objeto", ex);
			}
		}

		public Instrucao_Sicoob(int codigo)
		{
			this.carregar(codigo, 0, 0);
		}

		public Instrucao_Sicoob(int codigo, int nrDias)
		{
			this.carregar(codigo, nrDias, 0.0);
		}

		public Instrucao_Sicoob(int codigo, double percentualMultaDia)
		{
			this.carregar(codigo, 0, percentualMultaDia);
		}

		public Instrucao_Sicoob(int codigo, int nrDias, double percentualMultaDia)
		{
			this.carregar(codigo, nrDias, percentualMultaDia);
		}

		#endregion
	}
}