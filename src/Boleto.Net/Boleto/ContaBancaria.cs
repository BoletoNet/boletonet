namespace BoletoNet
{
    public class ContaBancaria
    {
        #region Variables
        private string _agencia = string.Empty;
        private string _digitoAgencia = string.Empty;
        private string _conta = string.Empty;
        private string _digitoConta = string.Empty;
        private string _operacaoConta = string.Empty;
        #endregion Variables

        public ContaBancaria()
        {
        }

        public ContaBancaria(string agencia, string conta)
        {
            _agencia = agencia;
            _conta = conta;
        }

        public ContaBancaria(string agencia, string digitoAgencia, string conta, string digitoConta)
        {
            _agencia = agencia;
            _digitoAgencia = digitoAgencia;
            _conta = conta;
            _digitoConta = digitoConta;
        }

        public ContaBancaria(string agencia, string digitoAgencia, string conta, string digitoConta, string operacaoConta)
        {
            _agencia = agencia;
            _digitoAgencia = digitoAgencia;
            _conta = conta;
            _digitoConta = digitoConta;
            _operacaoConta = operacaoConta;
        }

        #region Properties
        /// <summary>
        /// Retorna o numero da agência.
        /// Completar com zeros a esquerda quando necessario
        /// </summary>
        public string Agencia
        {
            get
            {
                return _agencia;
            }

            set
            {
                _agencia = value;
            }
        }

        /// <summary>
        /// Digito da Agência
        /// </summary>
        public string DigitoAgencia
        {
            get
            {
                return _digitoAgencia;
            }
            set
            {
                _digitoAgencia = value;
            }
        }

        /// <summary>
        /// Número da Conta Corrente
        /// </summary>
        public string Conta
        {
            get
            {
                return _conta;
            }
            set
            {
                _conta = value;
            }
        }

        /// <summary>
        /// Digito da Conta Corrente
        /// </summary>
        public string DigitoConta
        {
            get
            {
                return _digitoConta;
            }
            set
            {
                _digitoConta = value;
            }
        }

        /// <summary>
        /// get/set Opreração da Conta Corrente
        /// </summary>
        public string OperacaConta
        {
            get
            {
                return _operacaoConta;
            }
            set
            {
                _operacaoConta = value;
            }
        }
        #endregion Properties
    }
}
