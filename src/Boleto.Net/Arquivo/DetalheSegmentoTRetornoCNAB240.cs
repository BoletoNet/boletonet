using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class DetalheSegmentoTRetornoCNAB240
    {

        #region Variáveis

        private int _codigoBanco;
        private int _idCodigoMovimento;
        private CodigoMovimento _codigoMovimento;
        private int _agencia;
        private string _digitoAgencia;
        private long _conta;
        private string _digitoConta;
        private int _dacAgConta;
        private string _nossoNumero; //identificação do título no banco
        private int _codigoCarteira;
        private string _numeroDocumento; //número utilizado pelo cliente para a identificação do título
        private DateTime _dataVencimento;
        private decimal _valorTitulo;
        private string _identificacaoTituloEmpresa;
        private int _tipoInscricao;
        private string _numeroInscricao;
        private string _nomeSacado;
        private decimal _valorTarifas;
        private string _codigoRejeicao;
        private string _registro;
        private string _usoFebraban;

        private List<DetalheSegmentoTRetornoCNAB240> _listaDetalhe = new List<DetalheSegmentoTRetornoCNAB240>();

        #endregion

        #region Construtores

        public DetalheSegmentoTRetornoCNAB240()
		{
        }

        public DetalheSegmentoTRetornoCNAB240(string registro)
        {
            _registro = registro;
        }

        #endregion

        #region Propriedades

        public int idCodigoMovimento
        {
            get { return _idCodigoMovimento; }
            set { _idCodigoMovimento = value; }
        }

        public int CodigoBanco
        {
            get { return _codigoBanco; }
            set { _codigoBanco = value; }
        }

        public string Registro
        {
            get { return _registro; }            
        }

        public CodigoMovimento CodigoMovimento
        {
            get 
            {
                _codigoMovimento = new CodigoMovimento(_codigoBanco, _idCodigoMovimento); 
                return _codigoMovimento;
            }
            set 
            { 
                _codigoMovimento = value;
                _idCodigoMovimento = _codigoMovimento.Codigo;
            }
        }

        public int Agencia
        {
            get { return _agencia; }
            set { _agencia = value; }
        }

        public string DigitoAgencia
        {
            get { return _digitoAgencia; }
            set { _digitoAgencia = value; }
        }

        public long Conta
        {
            get { return _conta; }
            set { _conta = value; }
        }

        public string DigitoConta
        {
            get { return _digitoConta; }
            set { _digitoConta = value; }
        }

        public int DACAgenciaConta
        {
            get { return _dacAgConta; }
            set { _dacAgConta = value; }
        }

        public string NossoNumero
        {
            get { return _nossoNumero; }
            set { _nossoNumero = value; }
        }

        public int CodigoCarteira
        {
            get { return _codigoCarteira; }
            set { _codigoCarteira = value; }
        }

        public string NumeroDocumento
        {
            get { return _numeroDocumento; }
            set { _numeroDocumento = value; }
        }

        public DateTime DataVencimento
        {
            get { return _dataVencimento; }
            set { _dataVencimento = value; }
        }

        public decimal ValorTitulo
        {
            get { return _valorTitulo; }
            set { _valorTitulo = value; }
        }

        public string IdentificacaoTituloEmpresa
        {
            get { return _identificacaoTituloEmpresa; }
            set { _identificacaoTituloEmpresa = value; }
        }

        public int TipoInscricao
        {
            get { return _tipoInscricao; }
            set { _tipoInscricao = value; }
        }

        public string NumeroInscricao
        {
            get { return _numeroInscricao; }
            set { _numeroInscricao = value; }
        }

        public string NomeSacado
        {
            get { return _nomeSacado; }
            set { _nomeSacado = value; }
        }

        public decimal ValorTarifas
        {
            get { return _valorTarifas; }
            set { _valorTarifas = value; }
        }

        public string CodigoRejeicao
        {
            get { return _codigoRejeicao; }
            set { _codigoRejeicao = value; }
        }

        public List<DetalheSegmentoTRetornoCNAB240> ListaDetalhe
        {
            get { return _listaDetalhe; }
            set { _listaDetalhe = value; }
        }

        public String UsoFebraban
        {
            get { return _usoFebraban; }
            set { _usoFebraban = value; }
        }

        #endregion

        #region Métodos de Instância

        public void LerDetalheSegmentoTRetornoCNAB240(string registro)
        {
            try
            {
                _registro = registro;

                if (registro.Substring(13, 1) != "T")
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento T.");

                CodigoBanco = Convert.ToInt32(registro.Substring(0, 3));
                idCodigoMovimento = Convert.ToInt32(registro.Substring(15, 2));
                Agencia = Convert.ToInt32(registro.Substring(17, 5));
                DigitoAgencia = registro.Substring(22, 1);
                Conta = Convert.ToInt32(registro.Substring(23, 12));
                DigitoConta = registro.Substring(35, 1);

                NossoNumero = registro.Substring(37, 20);
                CodigoCarteira = Convert.ToInt32(registro.Substring(57, 1));
                NumeroDocumento = registro.Substring(58, 15);
                int dataVencimento = Convert.ToInt32(registro.Substring(73, 8));
                DataVencimento = Convert.ToDateTime(dataVencimento.ToString("##-##-####"));
                decimal valorTitulo = Convert.ToInt64(registro.Substring(81, 15));
                ValorTitulo = valorTitulo / 100;
                IdentificacaoTituloEmpresa = registro.Substring(105, 25);
                TipoInscricao = Convert.ToInt32(registro.Substring(132, 1));
                NumeroInscricao = registro.Substring(133, 15);
                NomeSacado = registro.Substring(148, 40);
                decimal valorTarifas = Convert.ToUInt64(registro.Substring(198, 15));
                ValorTarifas = valorTarifas / 100;
                CodigoRejeicao = registro.Substring(213, 10);
                UsoFebraban = registro.Substring(224, 17);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO T.", ex);
            }
        }

        #endregion
    }
}
