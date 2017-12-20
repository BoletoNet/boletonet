//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Banco_Daycoval.cs">
//    Boleto.Net
//  </copyright>
//  <summary>
//    Defines the Banco_Daycoval.cs type.
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------
using System.Web.UI;

[assembly: WebResource("BoletoNet.Imagens.707.jpg", "image/jpg")]

namespace BoletoNet
{
	using System;
	using System.IO;
	using System.Text;

	using global::BoletoNet.Excecoes;
	using global::BoletoNet.Util;

	internal class Banco_Daycoval : AbstractBanco, IBanco
	{
		#region Fields

		private IBanco _banco;

		#endregion

		#region Constructors and Destructors

		internal Banco_Daycoval()
		{
			this.Codigo = 707;
			this.Digito = "0";
			this.Nome = "Daycoval";
		}

		#endregion

		#region Public Methods and Operators

		public override void FormataNossoNumero(Boleto boleto)
		{
			if (boleto.NossoNumero.Length > 12)
			{
				throw new TamanhoNossoNumeroInvalidoException(12);
			}

			base.FormataNossoNumero(boleto);
		}

		public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
		{
			try
			{
				var detalhe = string.Empty;

				switch (tipoArquivo)
				{
					case TipoArquivo.CNAB240:
						break;
					case TipoArquivo.CNAB400:
						detalhe = this.GerarDetalheRemessaCNAB400(boleto, numeroRegistro);
						break;
					case TipoArquivo.CBR643:
						break;
					case TipoArquivo.Outro:
						break;
					default:
						throw new ArgumentOutOfRangeException("tipoArquivo", tipoArquivo, null);
				}

				return detalhe;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public override string GerarHeaderRemessa(
			string numeroConvenio,
			Cedente cedente,
			TipoArquivo tipoArquivo,
			int numeroArquivoRemessa)
		{
			try
			{
				this.Cedente = cedente;

				var header = string.Empty;

				base.GerarHeaderRemessa(numeroConvenio, cedente, tipoArquivo, numeroArquivoRemessa);

				switch (tipoArquivo)
				{
					case TipoArquivo.CNAB240:
						break;
					case TipoArquivo.CNAB400:
						header = this.GerarHeaderRemessaCNAB400(numeroConvenio);
						break;
					case TipoArquivo.CBR643:
						break;
					case TipoArquivo.Outro:
						break;
					default:
						throw new ArgumentOutOfRangeException("tipoArquivo", tipoArquivo, null);
				}

				return header;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public override string GerarTrailerRemessa(
			int numeroRegistro,
			TipoArquivo tipoArquivo,
			Cedente cedente,
			decimal vltitulostotal)
		{
			try
			{
				var trailer = string.Empty;

				base.GerarTrailerRemessa(numeroRegistro, tipoArquivo, cedente, vltitulostotal);

				switch (tipoArquivo)
				{
					case TipoArquivo.CNAB240:
						break;
					case TipoArquivo.CNAB400:
						trailer = this.GerarTrailerRemessa400(numeroRegistro);
						break;
					case TipoArquivo.Outro:
						break;
					default:
						throw new ArgumentOutOfRangeException("tipoArquivo", tipoArquivo, null);
				}

				return trailer;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
		{
			var detalheRetorno = new DetalheRetorno();

			detalheRetorno.IdentificacaoDoRegistro = Utils.ToInt32(registro.Substring(0, 1));
			detalheRetorno.CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2));
			detalheRetorno.NumeroInscricao = registro.Substring(3, 14);
			detalheRetorno.UsoEmpresa = registro.Substring(37, 25);

            var nossoNumeroLido = registro.Substring(94, 13).Trim();
            detalheRetorno.NossoNumero = nossoNumeroLido.Substring(0, nossoNumeroLido.Length - 1);
			detalheRetorno.NossoNumeroComDV = detalheRetorno.NossoNumero + nossoNumeroLido.Substring(nossoNumeroLido.Length -1, 1);
			detalheRetorno.Carteira = registro.Substring(107, 1);
			detalheRetorno.CodigoOcorrencia = int.Parse(registro.Substring(108, 2));

			var dataOcorrencia = Utils.ToInt32(registro.Substring(110, 6));
			detalheRetorno.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));

			detalheRetorno.SeuNumero = registro.Substring(116, 10);

			var dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
			detalheRetorno.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));

			decimal valorTitulo = Convert.ToInt64(registro.Substring(152, 13));
			detalheRetorno.ValorTitulo = valorTitulo / 100;

			detalheRetorno.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));
			detalheRetorno.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 5));
			detalheRetorno.Especie = Utils.ToInt32(registro.Substring(173, 2));

			decimal valorDespesa = Convert.ToUInt64(registro.Substring(175, 13));
			detalheRetorno.ValorDespesa = valorDespesa / 100;

			decimal valorIof = Convert.ToUInt64(registro.Substring(214, 11));
			detalheRetorno.IOF = valorIof / 100;

			decimal valorAbatimento = Convert.ToUInt64(registro.Substring(227, 11));
			detalheRetorno.ValorAbatimento = valorAbatimento / 100;

			decimal valorDesconto = Convert.ToUInt64(registro.Substring(240, 11));
			detalheRetorno.Descontos = valorDesconto / 100;

			decimal valorPago = Convert.ToUInt64(registro.Substring(253, 13));
			detalheRetorno.ValorPago = valorPago / 100;

			decimal jurosMora = Convert.ToUInt64(registro.Substring(266, 13));
			detalheRetorno.JurosMora = jurosMora / 100;

			return detalheRetorno;
		}

		public override void ValidaBoleto(Boleto boleto)
		{
			if (this._banco == null && this.Codigo != boleto.Banco.Codigo)
			{
				this._banco = boleto.Banco;
			}
			else
			{
				// Por padrão o Daycoval utiliza o layout do Bradesco,
				// porém é apto a trabalhar com outros bancos, 
				// por isso a validação acima.
				this._banco = new Banco_Bradesco();
			}

			this._banco.ValidaBoleto(boleto);
		}

		#endregion

		#region Methods

		private string GerarDetalheRemessaCNAB400(Boleto boleto, int numeroRegistro)
		{
			var detalhe = new StringBuilder();

			detalhe.Append("1"); // Identificação do registro, sempre 1

			// Identificação do tipo de inscrição da empresa
			// 01 - CPF do cedente
			// 02 - CNPJ do cedente
			// 03 - CPF do Sacador
			// 04 - CNPJ Sacador
			detalhe.Append(boleto.Cedente.CPFCNPJ.Length == 11 ? "01" : "02");

			// CPF/CNPJ da empresa ou sacador
			detalhe.Append(Utils.FitStringLength(boleto.Cedente.CPFCNPJ, 14, 14, '0', 0, true, true, true));

			detalhe.Append(Utils.FitStringLength(boleto.Cedente.Convenio.ToString(), 20, 20, ' ', 0, true, true, false));

				// Código da empresea, fornecido pelo banco
			detalhe.Append(Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, true));

            detalhe.Append(Utils.FitStringLength(boleto.NossoNumero, 8, 8, '0', 0, true, true, true)); // Nosso número
            detalhe.Append(Utils.FitStringLength(boleto.NossoNumero, 13, 13, '0', 0, true, true, true)); 

            // Nosso número do correspondente, mesmo do boleto
            if (this._banco == null)
			{
				this._banco = boleto.Banco;
			}

			this._banco.ValidaBoleto(boleto);

			detalhe.Append(Utils.FitStringLength(string.Empty, 24, 24, ' ', 0, true, true, false)); // Uso do banco
			detalhe.Append("4"); // TODO: Código de remessa
			detalhe.Append("01"); // TODO: Código de ocorrência
			detalhe.Append(Utils.FitStringLength(boleto.NumeroDocumento, 10, 10, ' ', 0, true, true, false)); // Seu número
			detalhe.Append(boleto.DataVencimento.ToString("ddMMyy"));
			detalhe.Append(Utils.FitStringLength(boleto.ValorBoleto.ApenasNumeros(), 13, 13, '0', 0, true, true, true));
			detalhe.Append("707"); // Código do banco
			detalhe.Append(Utils.FitStringLength(string.Empty, 4, 4, '0', 0, true, true, true)); // Agência cobradora
			detalhe.Append("0"); // DAC da agência cobradora
			detalhe.Append(Utils.FitStringLength(boleto.EspecieDocumento.Codigo, 2, 2, '0', 0, true, true, true));
			detalhe.Append("N"); // Indicação de aceite do título, sempre N
			detalhe.Append(boleto.DataDocumento.ToString("ddMMyy"));
			detalhe.Append(Utils.FitStringLength(string.Empty, 4, 4, '0', 0, true, true, true)); // Zeros
            // detalhe.Append(Utils.FitStringLength(boleto.JurosMora.ApenasNumeros(), 13, 13, '0', 0, true, true, true));
            detalhe.Append(Utils.FitStringLength("0", 13, 13, '0', 0, true, true, true));
            detalhe.Append(boleto.DataDesconto == DateTime.MinValue ? "000000" : boleto.DataDesconto.ToString("ddMMyy"));
			detalhe.Append(Utils.FitStringLength(boleto.ValorDesconto.ApenasNumeros(), 13, 13, '0', 0, true, true, true));
			detalhe.Append(Utils.FitStringLength(boleto.IOF.ApenasNumeros(), 26, 26, '0', 0, true, true, true));
			detalhe.Append(boleto.Sacado.CPFCNPJ.Length == 11 ? "01" : "02");
			detalhe.Append(Utils.FitStringLength(boleto.Sacado.CPFCNPJ, 14, 14, '0', 0, true, true, true));
			detalhe.Append(
				Utils.SubstituiCaracteresEspeciais(Utils.FitStringLength(boleto.Sacado.Nome, 30, 30, ' ', 0, true, true, false)));
			detalhe.Append(Utils.FitStringLength(string.Empty, 10, 10, ' ', 0, true, true, false));
			detalhe.Append(
				Utils.SubstituiCaracteresEspeciais(
					Utils.FitStringLength(boleto.Sacado.Endereco.EndComNumeroEComplemento, 40, 40, ' ', 0, true, true, false)));
			detalhe.Append(
				Utils.SubstituiCaracteresEspeciais(
					Utils.FitStringLength(boleto.Sacado.Endereco.Bairro, 12, 12, ' ', 0, true, true, false)));
			detalhe.Append(Utils.FitStringLength(boleto.Sacado.Endereco.CEP, 8, 8, '0', 0, true, true, true));
			detalhe.Append(
				Utils.SubstituiCaracteresEspeciais(
					Utils.FitStringLength(boleto.Sacado.Endereco.Cidade, 15, 15, ' ', 0, true, true, false)));
			detalhe.Append(Utils.FitStringLength(boleto.Sacado.Endereco.UF, 2, 2, ' ', 0, true, true, false));
			if (boleto.Avalista != null)
			{
				detalhe.Append(Utils.FitStringLength(boleto.Avalista.Nome, 30, 30, ' ', 0, true, true, false));
			}
			else
			{
				detalhe.Append(Utils.FitStringLength(string.Empty, 30, 30, ' ', 0, true, true, false));
			}

			detalhe.Append(Utils.FitStringLength(string.Empty, 10, 10, ' ', 0, true, true, false)); // Brancos
			detalhe.Append("00"); // Dias para início do protesto
			detalhe.Append(boleto.Moeda == 9 ? 0 : 2);
			detalhe.Append(Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true));

			var detalheFormatado = detalhe.ToString();

			if (detalheFormatado.Length != 400)
			{
				throw new Exception("Tamanho do registro inválido.");
			}

			return detalheFormatado;
		}

		private string GerarHeaderRemessaCNAB400(string convenio)
		{
			var header = new StringBuilder();

			header.Append("0"); // Identificação do registro header, sempre 0
			header.Append("1"); // Código da remessa, sempre 1
			header.Append("REMESSA"); // Literal de remessa
			header.Append("01"); // Código do serviço, sempre 01
			header.Append(Utils.FitStringLength("COBRANCA", 15, 15, ' ', 0, true, true, false));

				// Identificação por extenso do tipo de serviço
			header.Append(Utils.FitStringLength(convenio, 12, 12, ' ', 0, true, true, false));

				// Código da empresa, fornecido pelo banco
			header.Append(new string(' ', 8)); // BRANCOS
			header.Append(Utils.FitStringLength(this.Cedente.Nome, 30, 30, ' ', 0, true, true, false)); // Nome do cedente
			header.Append("707"); // Código do banco
			header.Append(Utils.FitStringLength("BANCO DAYCOVAL", 15, 15, ' ', 0, true, true, false)); // Nome do banco
			header.Append(DateTime.Now.ToString("ddMMyy")); // Data
			header.Append(Utils.FitStringLength(string.Empty, 294, 294, ' ', 0, true, true, false));
			header.Append("000001"); // Sequencial, sempre 1

			var headerFormatado = Utils.SubstituiCaracteresEspeciais(header.ToString());

			return headerFormatado;
		}

		private string GerarTrailerRemessa400(int numeroRegistro)
		{
			try
			{
				var complemento = new string(' ', 393);
				var trailer = new StringBuilder();

				trailer.Append("9");
				trailer.Append(complemento);
				trailer.Append(Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true));

				// Número sequencial do registro no arquivo.
				var trailerFormatado = Utils.SubstituiCaracteresEspeciais(trailer.ToString());

				return trailerFormatado;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
			}
		}

		#endregion
	}
}