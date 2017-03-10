using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoletoNet
{
	public class EspecieDocumento_Sofisa : AbstractEspecieDocumento, IEspecieDocumento
	{
        #region Constructors and Destructors

        public EspecieDocumento_Sofisa()
        {
        }

		public EspecieDocumento_Sofisa(string codigo)
		{
			try
			{
				this.carregar(codigo);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao carregar objecto", ex);
			}
		}

		#endregion

		#region Public Enums

		public enum EnumEspecieDocumento_Sofisa
		{
			DuplicataMercantil = 1,

			NotaPromissoria = 2,

			Cheque = 3,

			LetraCambio = 4,

			Recibo = 5,

			ApoliceSeguro = 8,

			DuplicataServico = 12,

			Outros = 99
		}

		#endregion

		#region Methods

		private void carregar(string idCodigo)
		{
			try
			{
				this.Banco = new Banco_Daycoval();

				switch (this.RetornaEnumPorCodigo(idCodigo))
				{
					case EnumEspecieDocumento_Sofisa.DuplicataMercantil:
						this.Codigo = this.RetornaCodigoEspecie(EnumEspecieDocumento_Sofisa.DuplicataMercantil);
						this.Especie = "Duplicata Mercantil";
						this.Sigla = "DM";
						break;
					case EnumEspecieDocumento_Sofisa.Recibo:
						this.Codigo = this.RetornaCodigoEspecie(EnumEspecieDocumento_Sofisa.Recibo);
						this.Especie = "Recibo";
						this.Sigla = "RE";
						break;
					case EnumEspecieDocumento_Sofisa.DuplicataServico:
						this.Codigo = this.RetornaCodigoEspecie(EnumEspecieDocumento_Sofisa.DuplicataServico);
						this.Especie = "Duplicata de Serviço";
						this.Sigla = "DS";
						break;
					case EnumEspecieDocumento_Sofisa.Outros:
						this.Codigo = this.RetornaCodigoEspecie(EnumEspecieDocumento_Sofisa.Outros);
						this.Especie = "Outros";
						this.Sigla = "OU";
						break;
					case EnumEspecieDocumento_Sofisa.NotaPromissoria:
						this.Codigo = this.RetornaCodigoEspecie(EnumEspecieDocumento_Sofisa.NotaPromissoria);
						this.Especie = "Nota promissória";
						this.Sigla = "NP";
						break;
					case EnumEspecieDocumento_Sofisa.Cheque:
						this.Codigo = this.RetornaCodigoEspecie(EnumEspecieDocumento_Sofisa.Cheque);
						this.Especie = "Cheque";
						this.Sigla = "CH";
						break;
					case EnumEspecieDocumento_Sofisa.LetraCambio:
						this.Codigo = this.RetornaCodigoEspecie(EnumEspecieDocumento_Sofisa.LetraCambio);
						this.Especie = "Letra de câmbio";
						this.Sigla = "LC";
						break;
					case EnumEspecieDocumento_Sofisa.ApoliceSeguro:
						this.Codigo = this.RetornaCodigoEspecie(EnumEspecieDocumento_Sofisa.ApoliceSeguro);
						this.Especie = "Apólice de seguro";
						this.Sigla = "AS";
						break;
					default:
						this.Codigo = "0";
						this.Especie = "( Selecione )";
						break;
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao carregar objeto", ex);
			}
		}

		private string RetornaCodigoEspecie(EnumEspecieDocumento_Sofisa especie)
		{
			return ((int)especie).ToString().PadLeft(2, '0');
		}

		private EnumEspecieDocumento_Sofisa RetornaEnumPorCodigo(string codigo)
		{
			switch (codigo)
			{
				case "01":
					return EnumEspecieDocumento_Sofisa.DuplicataMercantil;
				case "02":
					return EnumEspecieDocumento_Sofisa.NotaPromissoria;
				case "03":
					return EnumEspecieDocumento_Sofisa.Cheque;
				case "04":
					return EnumEspecieDocumento_Sofisa.LetraCambio;
				case "05":
					return EnumEspecieDocumento_Sofisa.Recibo;
				case "08":
					return EnumEspecieDocumento_Sofisa.ApoliceSeguro;
				case "12":
					return EnumEspecieDocumento_Sofisa.DuplicataServico;
				case "99":
					return EnumEspecieDocumento_Sofisa.Outros;

				default:
					return EnumEspecieDocumento_Sofisa.DuplicataMercantil;
			}
		}

        public override IEspecieDocumento DuplicataMercantil()
        {
            return new EspecieDocumento_Sofisa(RetornaCodigoEspecie(EnumEspecieDocumento_Sofisa.DuplicataMercantil));
        }

        #endregion
    }
}
