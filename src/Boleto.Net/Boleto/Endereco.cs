namespace BoletoNet
{
    /// <summary>
    /// Representa o endere�o do Cedente ou Sacado com todas as informa��es necess�rias.
    /// </summary>
    public class Endereco
    {        
        private string _cep;

        /// <summary>
        /// Define o Logradouro
        /// <remarks>
        /// Exemplo: Rua, Av., Travessa...
        /// </remarks>
        /// </summary>        
        public string Logradouro { get; set; }

        /// <summary>
        /// Define o endere�o completo
        /// <remarks>
        /// Exemplo: Bar�o do Amazonas
        /// </remarks>
        /// </summary>
        public string End { get; set; }

        /// <summary>
        /// Define o N�mero do endere�o
        /// <remarks>
        /// Exemplo: 1025.
        /// </remarks>
        /// </summary>
        public string Numero { get; set; }

        /// <summary>
        /// Define o complemento
        /// <remarks>
        /// Exemplo: Ap, Apartamento, Bloco, Casa, etc.
        /// </remarks>
        /// </summary>
        public string Complemento { get; set; }

        /// <summary>
        /// Define o bairro
        /// <remarks>
        /// Exemplo: Centro.
        /// </remarks>
        /// </summary>
        public string Bairro { get; set; }

        /// <summary>
        /// Define o nome da Cidade
        /// <remarks>
        /// Exemplo: S�o Paulo
        /// </remarks>
        /// </summary>
        public string Cidade { get; set;}

        /// <summary>
        /// Define o Estado (UF)
        /// <remarks>
        /// Exemplo:
        /// SP - S�o Paulo
        /// SC - Santa Catarina
        /// *Utilizar apenas a sigla (UF)
        /// </remarks>
        /// </summary>
        public string UF { get; set;}

        /// <summary>
        /// Define o n�mero do CEP
        /// <remarks>
        /// O n�mero do CEP ser� formatado automaticamente para remover pontos e tra�os
        /// </remarks>
        /// </summary>
        public string CEP
        {
            get
            {
                return _cep;
            }
            //Flavio(fhlviana@hotmail.com) - o metodo "Set" acontece menos vezes do que o get, por estimativa. Sendo assim, armazenar
            //sem o "." e o "-" faz com que o c�digo tenda a executar os dois Replace uma vez s�.
            //Consist�ncia para evitar NullPointerException. (MarcielTorres)
            set
            {
                this._cep = !string.IsNullOrEmpty(value) ? value.Replace(".", "").Replace("-", "") : string.Empty;
            }
        }

        /// <summary>
        /// Define o E-Mail
        /// <remarks>
        /// Campo opcional, por�m se informado n�o h� consist�ncias para edere�os de e-mails v�lidos
        /// </remarks>
        /// </summary>
        public string Email { get; set; }
    }
}
