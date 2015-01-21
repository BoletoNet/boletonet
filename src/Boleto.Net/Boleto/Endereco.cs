namespace BoletoNet
{
    /// <summary>
    /// Representa o endereço do Cedente ou Sacado.
    /// </summary>
    public class Endereco
    {
        private string _logradouro;
        private string _endereco;
        private string _numero;
        private string _complemento;
        private string _bairro;
        private string _cidade;
        private string _uf;
        private string _cep;
        private string _email;

        /// <summary>
        /// Retorna o Logradouro
        /// Exemplo : Rua, Av., Travessa...
        /// </summary>        
        public string Logradouro
        {
            get
            {
                return _logradouro;
            }
            set
            {
                this._logradouro = value;
            }
        }

        /// <summary>
        /// Retorna o endereço
        /// </summary>
        public string End
        {
            get
            {
                return _endereco;
            }
            set
            {
                this._endereco = value;
            }
        }

        /// <summary>
        /// Retorna o Número do endereço
        /// </summary>
        public string Numero
        {
            get
            {
                return _numero;
            }
            set
            {
                this._numero = value;
            }
        }

        /// <summary>
        /// Retorna o complemento
        /// </summary>
        public string Complemento
        {
            get
            {
                return _complemento;
            }
            set
            {
                this._complemento = value;
            }
        }

        /// <summary>
        /// Retorna o bairro
        /// </summary>
        public string Bairro
        {
            get
            {
                return _bairro;
            }
            set
            {
                this._bairro = value;
            }
        }

        /// <summary>
        /// Retona o nome da Cidade
        /// </summary>
        public string Cidade
        {
            get
            {
                return _cidade;
            }
            set
            {
                this._cidade = value;
            }
        }

        /// <summary>
        /// Retorna o UF
        /// Exemplo :
        /// SP - São Paulo
        /// SC - Santa Catarina
        /// </summary>
        public string UF
        {
            get
            {
                return _uf;
            }
            set
            {
                this._uf = value;
            }
        }

        /// <summary>
        /// Retorna o número do CEP
        /// </summary>
        public string CEP
        {
            get
            {
                //return _cep.Replace(".", "").Replace("-", "");
                return _cep;
            }
            //Flavio(fhlviana@hotmail.com) - o metodo "Set" acontece menos vezes do que o get, por estimativa. Sendo assim, armazenar
            //sem o "." e o "-" faz com que o código tenda a executar os dois Replace uma vez só.
            set
            {
                //this._cep = value;
                this._cep = value.Replace(".", "").Replace("-", "");
            }
        }

        /// <summary>
        /// Retorna o E-Mail
        /// </summary>
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                this._email = value;
            }
        }
    }
}
