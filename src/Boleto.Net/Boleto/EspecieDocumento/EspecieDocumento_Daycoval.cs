//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EspecieDocumento_Daycoval.cs">
//    Boleto.Net
//  </copyright>
//  <summary>
//    Defines the EspecieDocumento_Daycoval.cs type.
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------
namespace BoletoNet
{
	using System;

	public class EspecieDocumento_Daycoval : AbstractEspecieDocumento, IEspecieDocumento
	{
        #region Constructors and Destructors

        public EspecieDocumento_Daycoval()
        {
        }

		public EspecieDocumento_Daycoval(string codigo)
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

		public enum EnumEspecieDocumento_Daycoval
		{
			DuplicataMercantil = 1,
			
			Recibo = 5,

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
					case EnumEspecieDocumento_Daycoval.DuplicataMercantil:
						this.Codigo = this.RetornaCodigoEspecie(EnumEspecieDocumento_Daycoval.DuplicataMercantil);
						this.Especie = "Duplicata Mercantil";
						this.Sigla = "DM";
						break;
					case EnumEspecieDocumento_Daycoval.Recibo:
						this.Codigo = this.RetornaCodigoEspecie(EnumEspecieDocumento_Daycoval.Recibo);
						this.Especie = "Recibo";
						this.Sigla = "RE";
						break;
					case EnumEspecieDocumento_Daycoval.DuplicataServico:
						this.Codigo = this.RetornaCodigoEspecie(EnumEspecieDocumento_Daycoval.DuplicataServico);
						this.Especie = "Duplicata de Serviço";
						this.Sigla = "DS";
						break;
					case EnumEspecieDocumento_Daycoval.Outros:
						this.Codigo = this.RetornaCodigoEspecie(EnumEspecieDocumento_Daycoval.Outros);
						this.Especie = "Outros";
						this.Sigla = "OU";
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

		private string RetornaCodigoEspecie(EnumEspecieDocumento_Daycoval especie)
		{
			return ((int)especie).ToString().PadLeft(2, '0');
		}

		private EnumEspecieDocumento_Daycoval RetornaEnumPorCodigo(string codigo)
		{
			switch (codigo)
			{
				case "01":
					return EnumEspecieDocumento_Daycoval.DuplicataMercantil;
				case "05":
					return EnumEspecieDocumento_Daycoval.Recibo;
				case "12":
					return EnumEspecieDocumento_Daycoval.DuplicataServico;
				case "99":
					return EnumEspecieDocumento_Daycoval.Outros;

				default:
					return EnumEspecieDocumento_Daycoval.DuplicataMercantil;
			}
		}

        public override IEspecieDocumento DuplicataMercantil()
        {
            return new EspecieDocumento_Daycoval(RetornaCodigoEspecie(EnumEspecieDocumento_Daycoval.DuplicataMercantil));
        }

        #endregion
    }
}