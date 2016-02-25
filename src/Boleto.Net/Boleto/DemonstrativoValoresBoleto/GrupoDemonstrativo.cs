namespace BoletoNet.DemonstrativoValoresBoleto
{
	using System.Collections.ObjectModel;

	using global::BoletoNet.RelatorioValoresBoleto;

	public class GrupoDemonstrativo
	{
		#region Fields

		private ObservableCollection<ItemDemonstrativo> _itens;

		#endregion

		#region Public Properties

		public string Descricao { get; set; }

		public ObservableCollection<ItemDemonstrativo> Itens
		{
			get
			{
				return this._itens ?? (this._itens = new ObservableCollection<ItemDemonstrativo>());
			}
		}

		#endregion
	}
}