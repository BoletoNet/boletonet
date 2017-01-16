//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TamanhoNossoNumeroInvalidoException.cs">
//    Boleto.Net
//  </copyright>
//  <summary>
//    Defines the TamanhoNossoNumeroInvalidoException.cs type.
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------
namespace BoletoNet.Excecoes
{
	using System;

	public class TamanhoNossoNumeroInvalidoException : Exception
	{
		#region Constructors and Destructors

		public TamanhoNossoNumeroInvalidoException(int tamanhoMaximo)
			: base(string.Format("O tamanho máximo para o campo 'nosso número' é de {0} caracteres.", tamanhoMaximo))
		{
		}

		#endregion
	}
}