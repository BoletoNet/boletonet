namespace BoletoNet
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;

	using System.Collections.ObjectModel;

	using global::BoletoNet.DemonstrativoValoresBoleto;

	[Serializable, Browsable(false)]
	public class Boleto
	{
		#region Variaveis

		private readonly CodigoBarra _codigoBarra = new CodigoBarra();

		private string _carteira = string.Empty;
		private string _variacaoCarteira = string.Empty;
		private string _nossoNumero = string.Empty;
		private string _digitoNossoNumero = string.Empty;
		private DateTime _dataVencimento;
		private DateTime _dataDocumento;
		private DateTime _dataProcessamento;
		private int _numeroParcela;
		private decimal _valorBoleto;
		private decimal _valorCobrado;
		private string _localPagamento = "At� o vencimento, preferencialmente no ";
		private int _quantidadeMoeda = 1;
		private string _valorMoeda = string.Empty;
		private IList<IInstrucao> _instrucoes = new List<IInstrucao>();
		private IEspecieDocumento _especieDocumento = new EspecieDocumento();
		private string _aceite = "N";
		private string _numeroDocumento = string.Empty;
		private string _especie = "R$";
		private int _moeda = 9;
		private string _usoBanco = string.Empty;

		private Cedente _cedente;
		private int _categoria;

		// private string _instrucoesHtml = string.Empty;
		private IBanco _banco;
		private ContaBancaria _contaBancaria = new ContaBancaria();
		private decimal _valorDesconto;
		private Sacado _sacado;
		private bool _jurosPermanente;

		private decimal _percJurosMora;
		private decimal _jurosMora;
		private decimal _iof;
		private decimal _abatimento;
		private decimal _percMulta;
		private decimal _valorMulta;
		private decimal _outrosAcrescimos;
		private decimal _outrosDescontos;
		private DateTime _dataJurosMora;
		private DateTime _dataMulta;
		private DateTime _dataDesconto;
		private DateTime _dataOutrosAcrescimos;
		private DateTime _dataOutrosDescontos;
		private short _percentualIOS;

		private string _tipoModalidade = string.Empty;
		private Remessa _remessa;

		private ObservableCollection<GrupoDemonstrativo> _demonstrativos;

		#endregion

		#region Construtor
		public Boleto()
		{
		}

		public Boleto(DateTime dataVencimento, decimal valorBoleto, string carteira, string nossoNumero, Cedente cedente, EspecieDocumento especieDocumento)
		{
			this._carteira = carteira;
			this._nossoNumero = nossoNumero;
			this._dataVencimento = dataVencimento;
			this._valorBoleto = valorBoleto;
			this._valorBoleto = valorBoleto;
			this._valorCobrado = this.ValorCobrado;
			this._cedente = cedente;

			this._especieDocumento = especieDocumento;
		}

		public Boleto(decimal valorBoleto, string carteira, string nossoNumero, Cedente cedente)
		{
			this._carteira = carteira;
			this._nossoNumero = nossoNumero;
			this._valorBoleto = valorBoleto;
			this._valorBoleto = valorBoleto;
			this._valorCobrado = this.ValorCobrado;
			this._cedente = cedente;
		}

		public Boleto(DateTime dataVencimento, decimal valorBoleto, string carteira, string nossoNumero, Cedente cedente)
		{
			this._carteira = carteira;
			this._nossoNumero = nossoNumero;
			this._dataVencimento = dataVencimento;
			this._valorBoleto = valorBoleto;
			this._valorBoleto = valorBoleto;
			this._valorCobrado = this.ValorCobrado;
			this._cedente = cedente;
		}

		public Boleto(DateTime dataVencimento, decimal valorBoleto, string carteira, string nossoNumero, string digitoNossoNumero, Cedente cedente)
		{
			this._carteira = carteira;
			this._nossoNumero = nossoNumero;
			this._digitoNossoNumero = digitoNossoNumero;
			this._dataVencimento = dataVencimento;
			this._valorBoleto = valorBoleto;
			this._valorBoleto = valorBoleto;
			this._valorCobrado = this.ValorCobrado;
			this._cedente = cedente;
		}

		public Boleto(DateTime dataVencimento, decimal valorBoleto, string carteira, string nossoNumero, string agencia, string conta)
		{
			this._carteira = carteira;
			this._nossoNumero = nossoNumero;
			this._dataVencimento = dataVencimento;
			this._valorBoleto = valorBoleto;
			this._valorBoleto = valorBoleto;
			this._valorCobrado = this.ValorCobrado;
			this._cedente = new Cedente(new ContaBancaria(agencia, conta));
		}
		#endregion Construtor

		#region Properties

		public ObservableCollection<GrupoDemonstrativo> Demonstrativos
		{
			get
			{
				return this._demonstrativos ?? (this._demonstrativos = new ObservableCollection<GrupoDemonstrativo>());
			}
		}

		/// <summary> 
		/// Retorna a Categoria do boleto
		/// </summary>
		public int Categoria
		{
			get { return this._categoria; }
			set { this._categoria = value; }
		}

		/// <summary> 
		/// Retorna o numero da carteira.
		/// </summary>
		public string Carteira
		{
			get { return this._carteira; }
			set { this._carteira = value; }
		}

		/// <summary> 
		/// Retorna a Varia��o da carteira.
		/// </summary>
		public string VariacaoCarteira
		{
			get { return this._variacaoCarteira; }
			set { this._variacaoCarteira = value; }
		}

		/// <summary> 
		/// Retorna a data do vencimento do titulo.
		/// </summary>
		public DateTime DataVencimento
		{
			get { return this._dataVencimento; }
			set { this._dataVencimento = value; }
		}

		/// <summary> 
		/// Retorna o valor do titulo.
		/// </summary>
		public decimal ValorBoleto
		{
			get { return this._valorBoleto; }
			set { this._valorBoleto = value; }
		}

		/// <summary> 
		/// Retorna o valor Cobrado.
		/// </summary>
		public decimal ValorCobrado
		{
			get { return this._valorCobrado; }
			set { this._valorCobrado = value; }
		}

		/// <summary> 
		/// Retorna o campo para a 1 linha da instrucao.
		/// </summary>
		public IList<IInstrucao> Instrucoes
		{
			get { return this._instrucoes; }
			set { this._instrucoes = value; }
		}

		/// <summary> 
		/// Retorna o local de pagamento.
		/// </summary>
		public string LocalPagamento
		{
			get { return this._localPagamento; }
			set { this._localPagamento = value; }
		}

		/// <summary> 
		/// Retorna a quantidade da moeda.
		/// </summary>
		public int QuantidadeMoeda
		{
			get { return this._quantidadeMoeda; }
			set { this._quantidadeMoeda = value; }
		}

		/// <summary> 
		/// Retorna o valor da moeda
		/// </summary>
		public string ValorMoeda
		{
			get { return this._valorMoeda; }
			set { this._valorMoeda = value; }
		}

		/// <summary> 
		/// Retorna o campo aceite que por padrao vem com N.
		/// </summary>
		public string Aceite
		{
			get { return this._aceite; }
			set { this._aceite = value; }
		}

		/// <summary> 
		/// Retorna o campo especie do documento que por padrao vem com DV
		/// </summary>
		public string Especie
		{
			get { return this._especie; }
			set { this._especie = value; }
		}

		/// <summary> 
		/// Retorna o campo especie do documento que por padrao vem com DV
		/// </summary>
		public IEspecieDocumento EspecieDocumento
		{
			get { return this._especieDocumento ?? (this._especieDocumento = new EspecieDocumento()); }
			set { this._especieDocumento = value; }
		}

		/// <summary> 
		/// Retorna a data do documento.
		/// </summary>        
		public DateTime DataDocumento
		{
			get { return this._dataDocumento; }
			set { this._dataDocumento = value; }
		}

		/// <summary> 
		/// Retorna a data do processamento
		/// </summary>        
		public DateTime DataProcessamento
		{
			get { return this._dataProcessamento; }
			set { this._dataProcessamento = value; }
		}

		/// <summary> 
		/// Retorna a numero de parcelas
		/// </summary>        
		public int NumeroParcela
		{
			get { return this._numeroParcela; }
			set { this._numeroParcela = value; }
		}

		/// <summary> 
		/// Recupara o n�mero do documento
		/// </summary>        
		public string NumeroDocumento
		{
			get { return this._numeroDocumento; }
			set { this._numeroDocumento = value; }
		}

		/// <summary> 
		/// Recupara o digito nosso n�mero 
		/// </summary>        
		public string DigitoNossoNumero
		{
			get { return this._digitoNossoNumero; }
			set { this._digitoNossoNumero = value; }
		}

		/// <summary> 
		/// Recupara o nosso n�mero 
		/// </summary>        
		public string NossoNumero
		{
			get { return this._nossoNumero; }
			set { this._nossoNumero = value; }
		}

		/// <summary> 
		/// Recupera o valor da moeda 
		/// </summary>  
		public int Moeda
		{
			get { return this._moeda; }
			set { this._moeda = value; }
		}

		public Cedente Cedente
		{
			get { return this._cedente; }
			set { this._cedente = value; }
		}

		public CodigoBarra CodigoBarra
		{
			get { return this._codigoBarra; }
		}

		public IBanco Banco
		{
			get { return this._banco; }
			set { this._banco = value; }
		}

		public ContaBancaria ContaBancaria
		{
			get { return this._contaBancaria; }
			set { this._contaBancaria = value; }
		}

		/// <summary> 
		/// Retorna o valor do desconto do titulo.
		/// </summary>
		public decimal ValorDesconto
		{
			get { return this._valorDesconto; }
			set { this._valorDesconto = value; }
		}

		/// <summary>
		/// Retorna do Sacado
		/// </summary>
		public Sacado Sacado
		{
			get { return this._sacado; }
			set { this._sacado = value; }
		}

		/// <summary> 
		/// Para uso do banco 
		/// </summary>        
		public string UsoBanco
		{
			get { return this._usoBanco; }
			set { this._usoBanco = value; }
		}

		/// <summary>
		/// Percentual de Juros de Mora (ao dia)
		/// </summary>
		public decimal PercJurosMora
		{
			get { return this._percJurosMora; }
			set { this._percJurosMora = value; }
		}

		/// <summary> 
		/// Juros de mora (ao dia)
		/// </summary>  
		public decimal JurosMora
		{
			get { return this._jurosMora; }
			set { this._jurosMora = value; }
		}

		/// <summary>
		/// Caso a empresa tenha no conv�nio Juros permanentes cadastrados
		/// </summary>
		public bool JurosPermanente
		{
			get { return this._jurosPermanente; }
			set { this._jurosPermanente = value; }
		}

		/// <summary> 
		/// IOF
		/// </summary>  
		public decimal IOF
		{
			get { return this._iof; }
			set { this._iof = value; }
		}

		/// <summary> 
		/// Abatimento
		/// </summary>  
		public decimal Abatimento
		{
			get { return this._abatimento; }
			set { this._abatimento = value; }
		}

		/// <summary> 
		/// Percentual da Multa
		/// </summary>  
		public decimal PercMulta
		{
			get { return this._percMulta; }
			set { this._percMulta = value; }
		}

		/// <summary> 
		/// Valor da Multa
		/// </summary>  
		public decimal ValorMulta
		{
			get { return this._valorMulta; }
			set { this._valorMulta = value; }
		}

		/// <summary> 
		/// Outros Acr�scimos
		/// </summary>  
		public decimal OutrosAcrescimos
		{
			get { return this._outrosAcrescimos; }
			set { this._outrosAcrescimos = value; }
		}

		/// <summary> 
		/// Outros descontos
		/// </summary>  
		public decimal OutrosDescontos
		{
			get { return this._outrosDescontos; }
			set { this._outrosDescontos = value; }
		}

		/// <summary> 
		/// Data do Juros de Mora
		/// </summary>  
		public DateTime DataJurosMora
		{
			get { return this._dataJurosMora; }
			set { this._dataJurosMora = value; }
		}

		/// <summary> 
		/// Data do Juros da Multa
		/// </summary>  
		public DateTime DataMulta
		{
			get { return this._dataMulta; }
			set { this._dataMulta = value; }
		}

		/// <summary> 
		/// Data do Juros do Desconto
		/// </summary>  
		public DateTime DataDesconto
		{
			get { return this._dataDesconto; }
			set { this._dataDesconto = value; }
		}

		/// <summary> 
		/// Data de Outros Acr�scimos
		/// </summary>  
		public DateTime DataOutrosAcrescimos
		{
			get { return this._dataOutrosAcrescimos; }
			set { this._dataOutrosAcrescimos = value; }
		}

		/// <summary> 
		/// Data de Outros Descontos
		/// </summary>  
		public DateTime DataOutrosDescontos
		{
			get { return this._dataOutrosDescontos; }
			set { this._dataOutrosDescontos = value; }
		}

		/// <summary> 
		/// Retorna o tipo da modalidade
		/// </summary>
		public string TipoModalidade
		{
			get { return this._tipoModalidade; }
			set { this._tipoModalidade = value; }
		}

		/// <summary> 
		/// Retorna o percentual IOS para Seguradoras no caso do Banco Santander
		/// </summary>
		public short PercentualIOS
		{
			get { return this._percentualIOS; }
			set { this._percentualIOS = value; }
		}

		/// <summary>
		/// Retorna os Par�metros utilizados na gera��o da Remessa para o Boleto
		/// </summary>
		public Remessa Remessa
		{
			get { return this._remessa; }
			set { this._remessa = value; }
		}

		public IBancoCarteira BancoCarteira { get; set; }

		#endregion Properties

		public void Valida()
		{
			// Valida��es b�sicas, caso ainda tenha implementada na classe do banco.ValidaBoleto()
			if (this.Cedente == null)
				throw new Exception("Cedente n�o cadastrado.");

			// Atribui o nome do banco ao local de pagamento
			// Comentada por duplicidade no nome do banco
			////this.LocalPagamento += this.Banco.Nome + string.Empty;

			// Verifica se data do processamento � valida
			// if (this.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
			if (this.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
				this.DataProcessamento = DateTime.Now;

			// Verifica se data do documento � valida
			////if (this.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
			if (this.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
				this.DataDocumento = DateTime.Now;

			this.Banco.ValidaBoleto(this);
		}

		public void FormataCampos()
		{
			try
			{
				this.QuantidadeMoeda = 0;
				this.Banco.FormataCodigoBarra(this);
				this.Banco.FormataLinhaDigitavel(this);
				this.Banco.FormataNossoNumero(this);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a formata��o dos campos.", ex);
			}
		}
	}
}
