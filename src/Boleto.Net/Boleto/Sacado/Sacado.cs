using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace BoletoNet
{
    [Serializable(), Browsable(false)]
    public class Sacado
    {
        #region Variaveis

        private string _cpfcnpj = string.Empty;
        private string _nome = string.Empty;
        private Endereco _endereco = new Endereco();
        private InformacoesSacado _info = new InformacoesSacado();//Flavio(fhlviana@hotmail.com) - lista de todas as informações para serem apresentadas abaixo do nome do sacado
        private IList<IInstrucao> _instrucoes = new List<IInstrucao>();

        #endregion

        # region Construtores

        public Sacado()
        {
        }

        public Sacado(string nome)//Flavio(fhlviana@hotmail.com) - tem boleto que o sacado nao apresenta o CPF, sendo assim, adicionei a possibilidade
        {
            _nome = nome;
        }

        public Sacado(string cpfcnpj, string nome)
        {
            CPFCNPJ = cpfcnpj;
            _nome = nome;
        }

        public Sacado(string cpfcnpj, string nome, Endereco endereco)
        {
            CPFCNPJ = cpfcnpj;
            _nome = nome;
            Endereco = endereco;
        }

        # endregion

        #region Properties
        /// <summary>
        /// Retorna o endereco do sacado.
        /// </summary>
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

        /// <summary>
        /// Retorna as Informações do sacado
        /// </summary>
        public InformacoesSacado InformacoesSacado
        {
            get { return _info; }
        }

        /// <summary>
        /// Retorna CPF ou CNPJ
        /// </summary>
        public string CPFCNPJ
        {
            get
            {                   
                //return _cpfcnpj.Replace(".", "").Replace("-", "").Replace("/", "");
                return _cpfcnpj;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Erro. O CPF/CNPJ do cliente não pode ser nulo.");
                }
                string o = value.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
                
                //if (o == null || (o.Length != 11 && o.Length != 14))
                //    throw new ArgumentException("O CPF/CNPJ inválido. Utilize 11 dígitos para CPF ou 14 para CPNJ.");
                if (o == null || o == string.Empty)//Flavio(fhlviana@hotmail.com) - em razao da adiçao da possibilidade do boleto nao apresentar CPF ou CNPJ na renderização
                    _cpfcnpj = string.Empty;
                else if (o.Length != 11 && o.Length != 14)
                    throw new ArgumentException("O CPF/CNPJ inválido. Utilize 11 dígitos para CPF ou 14 para CPNJ.");

                //this._cpfcnpj = value;
                _cpfcnpj = o;//Flavio(fhlviana@hotmail.com) - se existe um conjunto de funções na classe "Utils" para gerar o CPF
                                  //e o CNPJ, e as mesma já são utilizadas na renderização, não há necessidade de armazenar o ".", "-"
                                  //e "/" dos mesmos e toda vez que o metodo "Get" da propriedade for requisitado esses mesmos terem que
                                  //ser retirados pelo método "Replace". Dessa forma os tres "Replace" sequencias só são executados uma vez.
            }
        }

        /// <summary>
        /// Nome do Sacado
        /// </summary>
        public string Nome
        {
            get
            {
                return _nome;
            }
            set
            {
                this._nome = value;
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
        #endregion Properties
    }
}
