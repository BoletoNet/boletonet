namespace BoletoNet.DemonstrativoValoresBoleto
{
    using System.Collections.ObjectModel;

    using global::BoletoNet.RelatorioValoresBoleto;
    using System.Collections.Generic;
    public class GrupoDemonstrativo
	{
		#region Fields

		private List<ItemDemonstrativo> _itens;

		#endregion

		#region Public Properties

		public string Descricao { get; set; }

		public List<ItemDemonstrativo> Itens
		{
			get
			{
				return this._itens ?? (this._itens = new List<ItemDemonstrativo>());
			}
		}

		#endregion
	}
}