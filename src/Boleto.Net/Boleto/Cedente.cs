using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BoletoNet
{
	[Serializable, Browsable(false)]
    public class Cedente
    {
        #region Variaveis

        private string _codigo = "0";
        private string _cpfcnpj;
        private string _nome;
        private ContaBancaria _contaBancaria;
        private int _convenio = 0;
        private int _numeroSequencial;
        private int _numeroBordero;
        private int _digitoCedente = -1;
        private string _carteira;
        private Endereco _endereco;
        private IList<IInstrucao> _instrucoes = new List<IInstrucao>();
        private bool _mostrarCNPJnoBoleto = false;

        #endregion Variaveis

        public Cedente()
        {
        }

        public Cedente(ContaBancaria contaBancaria)
        {
            _contaBancaria = contaBancaria;
        }

        public Cedente(string cpfcnpj, string nome)
        {
            CPFCNPJ = cpfcnpj;
            _nome = nome;
        }

		public Cedente(string cpfcnpj, string nome, string agencia, string digitoAgencia, string conta, string digitoConta, string operacaoConta) :
			this(cpfcnpj, nome, agencia, digitoAgencia, conta, digitoConta)
        {
			_contaBancaria = new ContaBancaria
			{
				Agencia = agencia,
				DigitoAgencia = digitoAgencia,
				Conta = conta,
				DigitoConta = digitoConta,
				OperacaConta = operacaoConta
			};
        }

        public Cedente(string cpfcnpj, string nome, string agencia, string digitoAgencia, string conta, string digitoConta)
			: this(cpfcnpj, nome)
		{
			_contaBancaria = new ContaBancaria
        {
				Agencia = agencia,
				DigitoAgencia = digitoAgencia,
				Conta = conta,
				DigitoConta = digitoConta
			};
        }

		public Cedente(string cpfcnpj, string nome, string agencia, string conta, string digitoConta) :
			this(cpfcnpj, nome)
        {
            _contaBancaria = new ContaBancaria();
            _contaBancaria.Agencia = agencia;
            _contaBancaria.Conta = conta;
            _contaBancaria.DigitoConta = digitoConta;
        }

        public Cedente(string cpfcnpj, string nome, string agencia, string conta)
			: this(cpfcnpj, nome)
        {
            _contaBancaria = new ContaBancaria();
            _contaBancaria.Agencia = agencia;
            _contaBancaria.Conta = conta;
        }

        #region Propriedades

        /// <summary>
        /// C�digo do Cedente
        /// </summary>
        public string Codigo
        {
            get
            {
                return _codigo;
            }
            set
            {
                _codigo = value;
            }
        }

        public int DigitoCedente
        {
            get
            {
                return _digitoCedente;
            }
            set
            {
                _digitoCedente = value;
            }
            
        }

        /// <summary>
        /// Retona o CPF ou CNPJ do Cedente
        /// </summary>
        public string CPFCNPJ
        {
            get
            {
                return _cpfcnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            }
            set
            {
                string o = value.Replace(".", "").Replace("-", "").Replace("/", "");
                if (o == null || (o.Length != 11 && o.Length != 14))
                    throw new ArgumentException("O CPF/CNPJ inv�lido. Utilize 11 d�gitos para CPF ou 14 para CPNJ.");

                _cpfcnpj = value;
            }
        }
		
        /// <summary>
        /// Retona o CPF ou CNPJ do Cedente (com m�scara)
        /// </summary>
        public string CPFCNPJcomMascara
        {
            get
            {
                return _cpfcnpj;
            }
        }
		
        /// <summary>
        /// Nome do Cedente
        /// </summary>
        public String Nome
        {
            get
            {
                return _nome;
            }
            set
            {
                _nome = value;
            }
        }

        /// <summary>
        /// Conta Correnta do Cedente
        /// </summary>
        public ContaBancaria ContaBancaria
        {
            get
            {
                return _contaBancaria;
            }
            set
            {
                _contaBancaria = value;
            }
        }

        /// <summary>
        /// N�mero do Conv�nio
        /// </summary>
        public int Convenio
        {
            get
            {
                return _convenio;
            }
            set
            {
                _convenio = value;
            }
        }

        /// <summary>
        /// N�mero sequencial para gera��o de remessa
        /// </summary>
        public int NumeroSequencial
        {
            get
            {
                return _numeroSequencial;
            }
            set
            {
                _numeroSequencial = value;
            }
        }

        /// <summary>
        /// N�mero bordero do cliente
        /// </summary>
        public int NumeroBordero
        {
            get
            {
                return _numeroBordero;
            }
            set
            {
                _numeroBordero = value;
            }
        }

        /// <summary>
        /// N�mero da Carteira
        /// </summary>
        public string Carteira
        {
            get
            {
                return _carteira;
            }
            set
            {
                _carteira = value;
            }
        }

        public Endereco Endereco
        {
            get
            {
                return _endereco;
            }
            set
            {
                _endereco = value;
            }
        }

        public IList<IInstrucao> Instrucoes
        {
            get
            {
                return _instrucoes;
            }
            set
            {
                _instrucoes = value;
            }
        }


        public bool MostrarCNPJnoBoleto
        {
            get
            {
                return _mostrarCNPJnoBoleto;
            }
            set
            {
                _mostrarCNPJnoBoleto = value;
            }
        }


        
        #endregion Propriedades
    }
}
