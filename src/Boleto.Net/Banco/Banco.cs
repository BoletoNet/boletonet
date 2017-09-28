using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
	public class Banco : IBanco
	{
		#region Variaveis

		private IBanco _IBanco;

		#endregion Variaveis

		#region Construtores

		internal Banco()
		{
		}

		public Banco(int CodigoBanco)
		{
			try
			{
				InstanciaBanco(CodigoBanco);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao instanciar objeto.", ex);
			}
		}

		#endregion

		#region Propriedades da Interface

		public int Codigo
		{
			get { return _IBanco.Codigo; }
			set { _IBanco.Codigo = value; }
		}

		public string Digito
		{
			get { return _IBanco.Digito; }
		}

		public string Nome
		{
			get { return _IBanco.Nome; }
		}

		public Cedente Cedente
		{
			get { return _IBanco.Cedente; }
		}

		public string ChaveASBACE
		{
			get { return _IBanco.ChaveASBACE; }
			set { _IBanco.ChaveASBACE = value; }
		}

		#endregion

		#region Métodos Privados

		private void InstanciaBanco(int codigoBanco)
		{
			try
			{
				switch (codigoBanco)
				{
					//104 - Caixa
					case 104:
						_IBanco = new Banco_Caixa();
						break;
					//341 - Itaú
					case 341:
						_IBanco = new Banco_Itau();
						break;
					//356 - Real
					case 275:
					case 356:
						_IBanco = new Banco_Real();
						break;
					//422 - Safra
					case 422:
						_IBanco = new Banco_Safra();
						break;
					//237 - Bradesco
					case 237:
						_IBanco = new Banco_Bradesco();
						break;
					//347 - Sudameris
					case 347:
						_IBanco = new Banco_Sudameris();
						break;
					//353 - Santander
					case 353:
						_IBanco = new Banco_Santander();
						break;
					//070 - BRB
					case 70:
						_IBanco = new Banco_BRB();
						break;
					//479 - BankBoston
					case 479:
						_IBanco = new Banco_BankBoston();
						break;
					//001 - Banco do Brasil
					case 1:
						_IBanco = new Banco_Brasil();
						break;
					//399 - HSBC
					case 399:
						_IBanco = new Banco_HSBC();
						break;
					//003 - HSBC
					case 3:
						_IBanco = new Banco_Basa();
						break;
					//409 - Unibanco
					case 409:
						_IBanco = new Banco_Unibanco();
						break;
					//33 - Unibanco
					case 33:
						_IBanco = new Banco_Santander();
						break;
					//41 - Banrisul
					case 41:
						_IBanco = new Banco_Banrisul();
						break;
					//756 - Sicoob (Bancoob)
					case 756:
						_IBanco = new Banco_Sicoob();
						break;
					//748 - Sicredi
					case 748:
						_IBanco = new Banco_Sicredi();
						break;
					//21 - Banestes
					case 21:
						_IBanco = new Banco_Banestes();
						break;
					//004 - Nordeste
					case 4:
						_IBanco = new Banco_Nordeste();
						break;
					//85 - CECRED
					case 85:
						_IBanco = new Banco_Cecred();
						break;
                    //655 - Votorantim
                    case 655:
                        _IBanco = new Banco_Votorantim();
                        break;
                    case 707:
						_IBanco = new Banco_Daycoval();
						break;
					case 637:
						_IBanco = new Banco_Sofisa();
						break;
					default:
						throw new Exception("Código do banco não implementando: " + codigoBanco);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a execução da transação.", ex);
			}
		}

		#endregion

		#region Métodos de Interface

		public void FormataCodigoBarra(Boleto boleto)
		{
			try
			{
				_IBanco.FormataCodigoBarra(boleto);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a formatação do código de barra.", ex);
			}
		}

		public void FormataLinhaDigitavel(Boleto boleto)
		{
			try
			{
				_IBanco.FormataLinhaDigitavel(boleto);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a formatação da linha digitável.", ex);
			}
		}

		public void FormataNossoNumero(Boleto boleto)
		{
			try
			{
				_IBanco.FormataNossoNumero(boleto);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a formatação do nosso número.", ex);
			}
		}

		public void FormataNumeroDocumento(Boleto boleto)
		{
			try
			{
				_IBanco.FormataNumeroDocumento(boleto);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a formatação do número do documento.", ex);
			}
		}

		public void ValidaBoleto(Boleto boleto)
		{
			//try
			//{
			_IBanco.ValidaBoleto(boleto);
			//}
			//catch (Exception ex)
			//{
			//    throw new Exception("Erro durante a validação do banco.", ex);
			//}
		}

		#endregion

		#region Métodos de Validação de geração de arquivo
		public bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
		{
			try
			{
				return _IBanco.ValidarRemessa(tipoArquivo, numeroConvenio, _IBanco, cedente, boletos, numeroArquivoRemessa, out mensagem);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a validação do arquivo de REMESSA.", ex);
			}
		}
		#endregion

		#region Métodos de geração de arquivo

		public string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
		{
			try
			{
				return _IBanco.GerarHeaderRemessa(numeroConvenio, cedente, tipoArquivo, numeroArquivoRemessa);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração do registro HEADER do arquivo de REMESSA.", ex);
			}
		}

		public string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
		{
			try
			{
				return _IBanco.GerarHeaderRemessa(numeroConvenio, cedente, tipoArquivo, numeroArquivoRemessa, boletos);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração do registro HEADER do arquivo de REMESSA.", ex);
			}
		}

		public string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
		{
			try
			{
				return _IBanco.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração dos registros de DETALHE do arquivo de REMESSA.", ex);
			}
		}

		public string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
		{
			try
			{
				return _IBanco.GerarTrailerRemessa(numeroRegistro, tipoArquivo, cedente, vltitulostotal);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
			}
		}

		public string GerarTrailerRemessaComDetalhes(int numeroRegistro, int numeroRegistroDetalhes, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
		{
			try
			{
				return _IBanco.GerarTrailerRemessaComDetalhes(numeroRegistro, numeroRegistroDetalhes, tipoArquivo, cedente, vltitulostotal);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
			}
		}

		public string GerarHeaderRemessa(Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
		{
			try
			{
				return _IBanco.GerarHeaderRemessa(cedente, tipoArquivo, numeroArquivoRemessa);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração do registro HEADER do arquivo de REMESSA.", ex);
			}
		}

		public string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
		{
			try
			{
				return _IBanco.GerarHeaderLoteRemessa(numeroConvenio, cedente, numeroArquivoRemessa);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração do registro HEADER do arquivo de REMESSA.", ex);
			}
		}

		public string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
		{
			try
			{
				return _IBanco.GerarHeaderLoteRemessa(numeroConvenio, cedente, numeroArquivoRemessa, tipoArquivo);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração do registro HEADER do arquivo de REMESSA.", ex);
			}
		}

		public string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo, Boleto boletos)
		{
			try
			{
				return _IBanco.GerarHeaderLoteRemessa(numeroConvenio, cedente, numeroArquivoRemessa, tipoArquivo, boletos);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração do registro HEADER do arquivo de REMESSA.", ex);
			}
		}

		public string GerarDetalheSegmentoARemessa(Boleto boleto, int numeroRegistro)
		{
			try
			{
				return _IBanco.GerarDetalheSegmentoARemessa(boleto, numeroRegistro);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração dos registros de DETALHE do arquivo de REMESSA.", ex);
			}
		}

		public string GerarDetalheSegmentoBRemessa(Boleto boleto, int numeroRegistro)
		{
			try
			{
				return _IBanco.GerarDetalheSegmentoBRemessa(boleto, numeroRegistro);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração dos registros de DETALHE do arquivo de REMESSA.", ex);
			}
		}

		public string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio)
		{
			try
			{
				return _IBanco.GerarDetalheSegmentoPRemessa(boleto, numeroRegistro, numeroConvenio);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração dos registros de DETALHE do arquivo de REMESSA.", ex);
			}
		}

		public string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio, Cedente cedente)
		{
			try
			{
				return _IBanco.GerarDetalheSegmentoPRemessa(boleto, numeroRegistro, numeroConvenio, cedente);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração dos registros de DETALHE do arquivo de REMESSA.", ex);
			}
		}

		public string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio, Cedente cedente, Boleto boletos)
		{
			try
			{
				return _IBanco.GerarDetalheSegmentoPRemessa(boleto, numeroRegistro, numeroConvenio, cedente, boletos);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração dos registros de DETALHE do arquivo de REMESSA.", ex);
			}
		}

		public string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
		{
			try
			{
				return _IBanco.GerarDetalheSegmentoQRemessa(boleto, numeroRegistro, tipoArquivo);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração dos registros de DETALHE do arquivo de REMESSA.", ex);
			}
		}

		public string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, Sacado sacado)
		{
			try
			{
				return _IBanco.GerarDetalheSegmentoQRemessa(boleto, numeroRegistro, sacado);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração dos registros de DETALHE do arquivo de REMESSA.", ex);
			}
		}

		public string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
		{
			try
			{
				return _IBanco.GerarDetalheSegmentoRRemessa(boleto, numeroRegistro, tipoArquivo);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração dos registros de DETALHE do arquivo de REMESSA.", ex);
			}
		}

		public string GerarDetalheSegmentoSRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
		{
			try
			{
				return _IBanco.GerarDetalheSegmentoSRemessa(boleto, numeroRegistro, tipoArquivo);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração dos registros de DETALHE do arquivo de REMESSA.", ex);
			}
		}

		public string GerarTrailerArquivoRemessa(int numeroRegistro)
		{
			try
			{
				return _IBanco.GerarTrailerArquivoRemessa(numeroRegistro);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
			}
		}

		public string GerarTrailerArquivoRemessa(int numeroRegistro, Boleto boletos)
		{
			try
			{
				return _IBanco.GerarTrailerArquivoRemessa(numeroRegistro, boletos);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
			}
		}

		public string GerarTrailerLoteRemessa(int numeroRegistro)
		{
			try
			{
				return _IBanco.GerarTrailerLoteRemessa(numeroRegistro);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
			}
		}

		public string GerarTrailerLoteRemessa(int numeroRegistro, Boleto boletos)
		{
			try
			{
				return _IBanco.GerarTrailerLoteRemessa(numeroRegistro, boletos);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
			}
		}

		public string GerarMensagemVariavelRemessa(Boleto boleto, ref int numeroRegistro, TipoArquivo tipoArquivo)
		{
			try
			{
				return _IBanco.GerarMensagemVariavelRemessa(boleto, ref numeroRegistro, tipoArquivo);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração do registro MENSAGEM VARIAVEL do arquivo de REMESSA.", ex);
			}
		}

		#endregion

		#region Métodos de Leitura do arquivo de Retorno

		public DetalheSegmentoTRetornoCNAB240 LerDetalheSegmentoTRetornoCNAB240(string registro)
		{
			return _IBanco.LerDetalheSegmentoTRetornoCNAB240(registro);
		}

		public DetalheSegmentoURetornoCNAB240 LerDetalheSegmentoURetornoCNAB240(string registro)
		{
			return _IBanco.LerDetalheSegmentoURetornoCNAB240(registro);
		}

		public DetalheSegmentoYRetornoCNAB240 LerDetalheSegmentoYRetornoCNAB240(string registro)
		{
			return _IBanco.LerDetalheSegmentoYRetornoCNAB240(registro);
		}

		public DetalheSegmentoWRetornoCNAB240 LerDetalheSegmentoWRetornoCNAB240(string registro)
		{
			return _IBanco.LerDetalheSegmentoWRetornoCNAB240(registro);
		}

		public DetalheRetorno LerDetalheRetornoCNAB400(string registro)
		{
			return _IBanco.LerDetalheRetornoCNAB400(registro);
		}

        public HeaderRetorno LerHeaderRetornoCNAB400(string registro)
        {
            return _IBanco.LerHeaderRetornoCNAB400(registro);
        }

        public long ObterNossoNumeroSemConvenioOuDigitoVerificador(long convenio, string nossoNumero)
        {
            return _IBanco.ObterNossoNumeroSemConvenioOuDigitoVerificador(convenio, nossoNumero);
        }

        #endregion Métodos de Leitura do arquivo de Retorno
    }
}
