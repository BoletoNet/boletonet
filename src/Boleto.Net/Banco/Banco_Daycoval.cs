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
	using System.Text;

	using global::BoletoNet.Excecoes;
	using global::BoletoNet.Util;

	internal class Banco_Daycoval : AbstractBanco, IBanco
	{
		private IBanco _banco;

		internal Banco_Daycoval()
		{
			this.Codigo = 707;
			this.Digito = "0";
			this.Nome = "Daycoval";
		}

		public override void FormataNossoNumero(Boleto boleto)
		{
			if (boleto.NossoNumero.Length > 12)
				throw new TamanhoNossoNumeroInvalidoException(12);
			base.FormataNossoNumero(boleto);
		}

		public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
		{
			try
			{
				string str = string.Empty;
				switch (tipoArquivo)
				{
					case TipoArquivo.CNAB240:
					case TipoArquivo.CBR643:
					case TipoArquivo.Outro:
						return str;
					case TipoArquivo.CNAB400:
						str = this.GerarDetalheRemessaCNAB400(boleto, numeroRegistro);
						goto case TipoArquivo.CNAB240;
					default:
						throw new ArgumentOutOfRangeException(nameof(tipoArquivo), (object)tipoArquivo, (string)null);
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
		{
			try
			{
				this.Cedente = cedente;
				string str = string.Empty;
				base.GerarHeaderRemessa(numeroConvenio, cedente, tipoArquivo, numeroArquivoRemessa);
				switch (tipoArquivo)
				{
					case TipoArquivo.CNAB240:
					case TipoArquivo.CBR643:
					case TipoArquivo.Outro:
						return str;
					case TipoArquivo.CNAB400:
						str = this.GerarHeaderRemessaCNAB400(numeroConvenio);
						goto case TipoArquivo.CNAB240;
					default:
						throw new ArgumentOutOfRangeException(nameof(tipoArquivo), (object)tipoArquivo, (string)null);
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, Decimal vltitulostotal)
		{
			try
			{
				string str = string.Empty;
				base.GerarTrailerRemessa(numeroRegistro, tipoArquivo, cedente, vltitulostotal);
				switch (tipoArquivo)
				{
					case TipoArquivo.CNAB240:
					case TipoArquivo.Outro:
						return str;
					case TipoArquivo.CNAB400:
						str = this.GerarTrailerRemessa400(numeroRegistro);
						goto case TipoArquivo.CNAB240;
					default:
						throw new ArgumentOutOfRangeException(nameof(tipoArquivo), (object)tipoArquivo, (string)null);
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
		{
			DetalheRetorno detalheRetorno = new DetalheRetorno();
			detalheRetorno.IdentificacaoDoRegistro = Utils.ToInt32(registro.Substring(0, 1));
			detalheRetorno.CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2));
			detalheRetorno.NumeroInscricao = registro.Substring(3, 14);
			detalheRetorno.UsoEmpresa = registro.Substring(37, 25);
			string str = registro.Substring(94, 13).Trim();
			detalheRetorno.NossoNumero = str.Substring(0, str.Length - 1);
			detalheRetorno.NossoNumeroComDV = detalheRetorno.NossoNumero + str.Substring(str.Length - 1, 1);
			detalheRetorno.Carteira = registro.Substring(107, 1);
			detalheRetorno.CodigoOcorrencia = int.Parse(registro.Substring(108, 2));
			int int32_1 = Utils.ToInt32(registro.Substring(110, 6));
			detalheRetorno.DataOcorrencia = Utils.ToDateTime((object)int32_1.ToString("##-##-##"));
			detalheRetorno.SeuNumero = registro.Substring(116, 10);
			int int32_2 = Utils.ToInt32(registro.Substring(146, 6));
			detalheRetorno.DataVencimento = Utils.ToDateTime((object)int32_2.ToString("##-##-##"));
			Decimal int64 = (Decimal)Convert.ToInt64(registro.Substring(152, 13));
			detalheRetorno.ValorTitulo = int64 / new Decimal(100);
			detalheRetorno.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));
			detalheRetorno.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 5));
			detalheRetorno.Especie = Utils.ToInt32(registro.Substring(173, 2));
			Decimal uint64_1 = (Decimal)Convert.ToUInt64(registro.Substring(175, 13));
			detalheRetorno.ValorDespesa = uint64_1 / new Decimal(100);
			Decimal uint64_2 = (Decimal)Convert.ToUInt64(registro.Substring(214, 11));
			detalheRetorno.IOF = uint64_2 / new Decimal(100);
			Decimal uint64_3 = (Decimal)Convert.ToUInt64(registro.Substring(227, 11));
			detalheRetorno.ValorAbatimento = uint64_3 / new Decimal(100);
			Decimal uint64_4 = (Decimal)Convert.ToUInt64(registro.Substring(240, 11));
			detalheRetorno.Descontos = uint64_4 / new Decimal(100);
			Decimal uint64_5 = (Decimal)Convert.ToUInt64(registro.Substring(253, 13));
			detalheRetorno.ValorPago = uint64_5 / new Decimal(100);
			Decimal uint64_6 = (Decimal)Convert.ToUInt64(registro.Substring(266, 13));
			detalheRetorno.JurosMora = uint64_6 / new Decimal(100);
			return detalheRetorno;
		}

		public override void ValidaBoleto(Boleto boleto)
		{
			this._banco = this._banco != null || this.Codigo == boleto.Banco.Codigo ? (IBanco)new Banco_Bradesco() : boleto.Banco;
			this._banco.ValidaBoleto(boleto);
		}

		private string GerarDetalheRemessaCNAB400(Boleto boleto, int numeroRegistro)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("1");
			stringBuilder.Append(boleto.Cedente.CPFCNPJ.Length == 11 ? "01" : "02");
			stringBuilder.Append(Utils.FitStringLength(boleto.Cedente.CPFCNPJ, 14, 14, '0', 0, true, true, true));
			stringBuilder.Append(Utils.FitStringLength(boleto.Cedente.Convenio.ToString(), 20, 20, ' ', 0, true, true, false));
			stringBuilder.Append(Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, true));
			stringBuilder.Append(Utils.FitStringLength("0", 8, 8, '0', 0, true, true, true));
			stringBuilder.Append(Utils.FitStringLength(boleto.NossoNumero, 12, 12, '0', 0, true, true, true));
			if (this._banco == null)
				this._banco = boleto.Banco;
			this._banco.ValidaBoleto(boleto);
			stringBuilder.Append(boleto.DigitoNossoNumero);
			stringBuilder.Append(Utils.FitStringLength(string.Empty, 24, 24, ' ', 0, true, true, false));
			stringBuilder.Append("4");
			stringBuilder.Append("01");
			stringBuilder.Append(Utils.FitStringLength(boleto.NumeroDocumento, 10, 10, ' ', 0, true, true, false));
			stringBuilder.Append(boleto.DataVencimento.ToString("ddMMyy"));
			stringBuilder.Append(Utils.FitStringLength(boleto.ValorBoleto.ApenasNumeros(), 13, 13, '0', 0, true, true, true));
			stringBuilder.Append("707");
			stringBuilder.Append(Utils.FitStringLength(string.Empty, 4, 4, '0', 0, true, true, true));
			stringBuilder.Append("0");
			stringBuilder.Append(Utils.FitStringLength(boleto.EspecieDocumento.Codigo, 2, 2, '0', 0, true, true, true));
			stringBuilder.Append("N");
			stringBuilder.Append(boleto.DataDocumento.ToString("ddMMyy"));
			stringBuilder.Append(Utils.FitStringLength(string.Empty, 4, 4, '0', 0, true, true, true));
			stringBuilder.Append(Utils.FitStringLength("0", 13, 13, '0', 0, true, true, true));
			stringBuilder.Append(boleto.DataDesconto == DateTime.MinValue ? "000000" : boleto.DataDesconto.ToString("ddMMyy"));
			stringBuilder.Append(Utils.FitStringLength(boleto.ValorDesconto.ApenasNumeros(), 13, 13, '0', 0, true, true, true));
			stringBuilder.Append(Utils.FitStringLength(boleto.IOF.ApenasNumeros(), 26, 26, '0', 0, true, true, true));
			stringBuilder.Append(boleto.Sacado.CPFCNPJ.Length == 11 ? "01" : "02");
			stringBuilder.Append(Utils.FitStringLength(boleto.Sacado.CPFCNPJ, 14, 14, '0', 0, true, true, true));
			stringBuilder.Append(Utils.SubstituiCaracteresEspeciais(Utils.FitStringLength(boleto.Sacado.Nome, 30, 30, ' ', 0, true, true, false)));
			stringBuilder.Append(Utils.FitStringLength(string.Empty, 10, 10, ' ', 0, true, true, false));
			stringBuilder.Append(Utils.SubstituiCaracteresEspeciais(Utils.FitStringLength(boleto.Sacado.Endereco.EndComNumeroEComplemento, 40, 40, ' ', 0, true, true, false)));
			stringBuilder.Append(Utils.SubstituiCaracteresEspeciais(Utils.FitStringLength(boleto.Sacado.Endereco.Bairro, 12, 12, ' ', 0, true, true, false)));
			stringBuilder.Append(Utils.FitStringLength(boleto.Sacado.Endereco.CEP, 8, 8, '0', 0, true, true, true));
			stringBuilder.Append(Utils.SubstituiCaracteresEspeciais(Utils.FitStringLength(boleto.Sacado.Endereco.Cidade, 15, 15, ' ', 0, true, true, false)));
			stringBuilder.Append(Utils.FitStringLength(boleto.Sacado.Endereco.UF, 2, 2, ' ', 0, true, true, false));
			if (boleto.Avalista != null)
				stringBuilder.Append(Utils.FitStringLength(boleto.Avalista.Nome, 30, 30, ' ', 0, true, true, false));
			else
				stringBuilder.Append(Utils.FitStringLength(string.Empty, 30, 30, ' ', 0, true, true, false));
			stringBuilder.Append(Utils.FitStringLength(string.Empty, 10, 10, ' ', 0, true, true, false));
			stringBuilder.Append("00");
			stringBuilder.Append(boleto.Moeda == 9 ? 0 : 2);
			stringBuilder.Append(Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true));
			string str = stringBuilder.ToString();
			if (str.Length != 400)
				throw new Exception("Tamanho do registro inválido.");
			return str;
		}

		private string GerarHeaderRemessaCNAB400(string convenio)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("0");
			stringBuilder.Append("1");
			stringBuilder.Append("REMESSA");
			stringBuilder.Append("01");
			stringBuilder.Append(Utils.FitStringLength("COBRANCA", 15, 15, ' ', 0, true, true, false));
			stringBuilder.Append(Utils.FitStringLength(convenio, 12, 12, ' ', 0, true, true, false));
			stringBuilder.Append(new string(' ', 8));
			stringBuilder.Append(Utils.FitStringLength(this.Cedente.Nome, 30, 30, ' ', 0, true, true, false));
			stringBuilder.Append("707");
			stringBuilder.Append(Utils.FitStringLength("BANCO DAYCOVAL", 15, 15, ' ', 0, true, true, false));
			stringBuilder.Append(DateTime.Now.ToString("ddMMyy"));
			stringBuilder.Append(Utils.FitStringLength(string.Empty, 294, 294, ' ', 0, true, true, false));
			stringBuilder.Append("000001");
			return Utils.SubstituiCaracteresEspeciais(stringBuilder.ToString());
		}

		private string GerarTrailerRemessa400(int numeroRegistro)
		{
			try
			{
				string str = new string(' ', 393);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("9");
				stringBuilder.Append(str);
				stringBuilder.Append(Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true));
				return Utils.SubstituiCaracteresEspeciais(stringBuilder.ToString());
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
			}
		}
	}
}
