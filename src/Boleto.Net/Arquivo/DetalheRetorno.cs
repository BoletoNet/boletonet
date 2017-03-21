using System;

namespace BoletoNet
{
    public class DetalheRetorno
    {

        #region Variáveis

        private int _codigoInscricao = 0;
        private string _numeroInscricao = string.Empty;
        private int _conta = 0;
        private int _codigoBanco = 0;
        private int _dacConta = 0;
        private string _usoEmpresa = string.Empty;
        private string _dacNossoNumero = string.Empty;
        private string _carteira = string.Empty;
        private int _codigoOcorrencia = 0;
        private string _descOcorrencia = string.Empty;
        private int _confirmacaoNossoNumero = 0;
        private decimal _valorTitulo = 0;
        private int _agenciaCobradora = 0;
        private int _dacAgenciaCobradora = 0;
        private int _especie = 0;
        private decimal _tarifaCobranca = 0;
        private decimal _valorAbatimento = 0;
        private decimal _valorPrincipal = 0;
        private decimal _jurosMora = 0;
        private DateTime _dataCredito = new DateTime(1, 1, 1);
        private int _instrucaoCancelada = 0;
        private string _nomeSacado = string.Empty;
        private string _erros = string.Empty;
        private string _codigoLiquidacao = string.Empty;
        private int _numeroSequencial = 0;
        private string _registro = string.Empty;
        private decimal _valorDespesa = 0;
        private decimal _valorOutrasDespesas = 0;
        private string _origemPagamento = string.Empty;
        private string _motivoCodigoOcorrencia = string.Empty;
        private string _identificacaoTitulo = string.Empty;
        private string _motivosRejeicao = string.Empty;
        private int _numeroCartorio = 0;
        private string _numeroProtocolo = string.Empty;
        private string _numeroControle = string.Empty;

        #region Propriedades BRB

        private int _identificacaoDoRegistro = 0;
        private int _tipoInscricao = 0;
        private string _cgcCpf = string.Empty;
        private int _contaCorrente = 0;
        private string _nossoNumero = string.Empty;
        private string _seuNumero = string.Empty;
        private int _instrucao = 0;
        private DateTime _dataOcorrencia = new DateTime(1, 1, 1);
        private string _numeroDocumento = string.Empty;
        private int _codigoRateio = 0;
        private DateTime _dataVencimento = new DateTime(1, 1, 1);
        private int _bancoCobrador = 0;
        private int _agencia = 0;
        private string _especieTitulo = string.Empty;
        private decimal _despeasaDeCobranca = 0;
        private decimal _outrasDespesas = 0;
        private decimal _juros = 0;
        private decimal _iof = 0;
        private decimal _abatimentos = 0;
        private decimal _descontos = 0;
        private decimal _valorPago = 0;
        private decimal _outrosDebitos = 0;
        private decimal _outrosCreditos = 0;
        private DateTime _dataLiquidacao = new DateTime(1, 1, 1);
        private int _sequencial = 0;
        private string _NossoNumeroComDV;
        private decimal _valorMulta;

        #endregion

        #endregion

        #region Construtores

        public DetalheRetorno()
        {
        }

        public DetalheRetorno(string registro)
        {
            _registro = registro;
        }

        #endregion

        #region Propriedades

        public int CodigoInscricao
        {
            get { return _codigoInscricao; }
            set { _codigoInscricao = value; }
        }

        public string NumeroInscricao
        {
            get { return _numeroInscricao; }
            set { _numeroInscricao = value; }
        }
        /// <summary>
        /// Agência com o Dígito Verificador, quando houver
        /// </summary>
        public int Agencia
        {
            get { return _agencia; }
            set { _agencia = value; }
        }

        public int Conta
        {
            get { return _conta; }
            set { _conta = value; }
        }

        public int DACConta
        {
            get { return _dacConta; }
            set { _dacConta = value; }
        }

        public string UsoEmpresa
        {
            get { return _usoEmpresa; }
            set { _usoEmpresa = value; }
        }
        /// <summary>
        /// Nosso Numero Sem o DV
        /// </summary>
        public string NossoNumero
        {
            get { return _nossoNumero; }
            set { _nossoNumero = value; }
        }
        /// <summary>
        /// DV do Nosso Numero
        /// </summary>
        public string DACNossoNumero
        {
            get { return _dacNossoNumero; }
            set { _dacNossoNumero = value; }
        }
        /// <summary>
        /// Nosso Numero Completo Com o Dígito Verificador
        /// </summary>
        public string NossoNumeroComDV
        {
            get { return _NossoNumeroComDV; }
            set { _NossoNumeroComDV = value; }
        }

        public string Carteira
        {
            get { return _carteira; }
            set { _carteira = value; }
        }

        public int CodigoOcorrencia
        {
            get { return _codigoOcorrencia; }
            set { _codigoOcorrencia = value; }
        }

        public string DescricaoOcorrencia
        {
            get { return _descOcorrencia; }
            set { _descOcorrencia = value; }
        }

        public DateTime DataOcorrencia
        {
            get { return _dataOcorrencia; }
            set { _dataOcorrencia = value; }
        }

        public string NumeroDocumento
        {
            get { return _numeroDocumento; }
            set { _numeroDocumento = value; }
        }

        public int ConfirmacaoNossoNumero
        {
            get { return _confirmacaoNossoNumero; }
            set { _confirmacaoNossoNumero = value; }
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

        public int CodigoBanco
        {
            get { return _codigoBanco; }
            set { _codigoBanco = value; }
        }

        public int AgenciaCobradora
        {
            get { return _agenciaCobradora; }
            set { _agenciaCobradora = value; }
        }

        public int DACAgenciaCobradora
        {
            get { return _dacAgenciaCobradora; }
            set { _dacAgenciaCobradora = value; }
        }

        public int Especie
        {
            get { return _especie; }
            set { _especie = value; }
        }

        public decimal TarifaCobranca
        {
            get { return _tarifaCobranca; }
            set { _tarifaCobranca = value; }
        }

        public decimal IOF
        {
            get { return _iof; }
            set { _iof = value; }
        }

        public decimal ValorAbatimento
        {
            get { return _valorAbatimento; }
            set { _valorAbatimento = value; }
        }

        public decimal Descontos
        {
            get { return _descontos; }
            set { _descontos = value; }
        }

        public decimal ValorPrincipal
        {
            get { return _valorPrincipal; }
            set { _valorPrincipal = value; }
        }

        public decimal JurosMora
        {
            get { return _jurosMora; }
            set { _jurosMora = value; }
        }

        public decimal OutrosCreditos
        {
            get { return _outrosCreditos; }
            set { _outrosCreditos = value; }
        }

        public decimal OutrosDebitos
        {
            get { return _outrosDebitos; }
            set { _outrosDebitos = value; }
        }

        public DateTime DataCredito
        {
            get { return _dataCredito; }
            set { _dataCredito = value; }
        }

        public int InstrucaoCancelada
        {
            get { return _instrucaoCancelada; }
            set { _instrucaoCancelada = value; }
        }

        public string NomeSacado
        {
            get { return _nomeSacado; }
            set { _nomeSacado = value; }
        }

        public string Erros
        {
            get { return _erros; }
            set { _erros = value; }
        }

        public string CodigoLiquidacao
        {
            get { return _codigoLiquidacao; }
            set { _codigoLiquidacao = value; }
        }

        public int NumeroSequencial
        {
            get { return _numeroSequencial; }
            set { _numeroSequencial = value; }
        }

        public string Registro
        {
            get { return _registro; }
        }
        public decimal ValorDespesa
        {
            get { return _valorDespesa; }
            set { _valorDespesa = value; }
        }
        public decimal ValorOutrasDespesas
        {
            get { return _valorOutrasDespesas; }
            set { _valorOutrasDespesas = value; }
        }
        public decimal ValorPago
        {
            get { return _valorPago; }
            set { _valorPago = value; }
        }
        public string MotivoCodigoOcorrencia
        {
            get { return _motivoCodigoOcorrencia; }
            set { _motivoCodigoOcorrencia = value; }
        }
        public string OrigemPagamento
        {
            get { return _origemPagamento; }
            set { _origemPagamento = value; }
        }
        public string IdentificacaoTitulo
        {
            get { return _identificacaoTitulo; }
            set { _identificacaoTitulo = value; }
        }
        public string MotivosRejeicao
        {
            get { return _motivosRejeicao; }
            set { _motivosRejeicao = value; }
        }
        public string NumeroProtocolo
        {
            get { return _numeroProtocolo; }
            set { _numeroProtocolo = value; }
        }
        public int NumeroCartorio
        {
            get { return _numeroCartorio; }
            set { _numeroCartorio = value; }
        }
        public string NumeroControle
        {
            get { return _numeroControle; }
            set { _numeroControle = value; }
        }

        public int IdentificacaoDoRegistro
        {
            get { return _identificacaoDoRegistro; }
            set { _identificacaoDoRegistro = value; }
        }

        public int TipoInscricao
        {
            get { return _tipoInscricao; }
            set { _tipoInscricao = value; }
        }

        public string CgcCpf
        {
            get { return _cgcCpf; }
            set { _cgcCpf = value; }
        }

        public int ContaCorrente
        {
            get { return _contaCorrente; }
            set { _contaCorrente = value; }
        }

        public string SeuNumero
        {
            get { return _seuNumero; }
            set { _seuNumero = value; }
        }

        public int Instrucao
        {
            get { return _instrucao; }
            set { _instrucao = value; }
        }

        public int CodigoRateio
        {
            get { return _codigoRateio; }
            set { _codigoRateio = value; }
        }

        public int BancoCobrador
        {
            get { return _bancoCobrador; }
            set { _bancoCobrador = value; }
        }

        public string EspecieTitulo
        {
            get { return _especieTitulo; }
            set { _especieTitulo = value; }
        }

        public decimal DespeasaDeCobranca
        {
            get { return _despeasaDeCobranca; }
            set { _despeasaDeCobranca = value; }
        }

        public decimal OutrasDespesas
        {
            get { return _outrasDespesas; }
            set { _outrasDespesas = value; }
        }

        public decimal Juros
        {
            get { return _juros; }
            set { _juros = value; }
        }

        public decimal Abatimentos
        {
            get { return _abatimentos; }
            set { _abatimentos = value; }
        }

        public DateTime DataLiquidacao
        {
            get { return _dataLiquidacao; }
            set { _dataLiquidacao = value; }
        }

        public int Sequencial
        {
            get { return _sequencial; }
            set { _sequencial = value; }
        }

        public decimal ValorMulta
        {
            get
            {
                return _valorMulta;
            }
            set
            {
                _valorMulta = value;
            }
        }

        #endregion

        #region Métodos de Instância

        public void LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                int dataOcorrencia = Utils.ToInt32(registro.Substring(110, 6));
                int dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
                int dataCredito = Utils.ToInt32(registro.Substring(295, 6));

                _codigoInscricao = Utils.ToInt32(registro.Substring(1, 2));
                _numeroInscricao = registro.Substring(3, 14);
                _agencia = Utils.ToInt32(registro.Substring(17, 4));
                _conta = Utils.ToInt32(registro.Substring(23, 5));
                _dacConta = Utils.ToInt32(registro.Substring(28, 1));
                _usoEmpresa = registro.Substring(37, 25);
                _nossoNumero = Utils.ToString(registro.Substring(85, 8));
                _dacNossoNumero = registro.Substring(93, 1);
                _carteira = registro.Substring(107, 1);
                _codigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));
                _dataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                _numeroDocumento = registro.Substring(116, 10);
                _nossoNumero = Utils.ToString(registro.Substring(126, 9));
                _dataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                decimal valorTitulo = Convert.ToInt64(registro.Substring(152, 13));
                _valorTitulo = valorTitulo / 100;
                _codigoBanco = Utils.ToInt32(registro.Substring(165, 3));
                _bancoCobrador = Utils.ToInt32(registro.Substring(165, 3));
                _agenciaCobradora = Utils.ToInt32(registro.Substring(168, 4));
                _especie = Utils.ToInt32(registro.Substring(173, 2));
                decimal tarifaCobranca = Convert.ToUInt64(registro.Substring(175, 13));
                _tarifaCobranca = tarifaCobranca / 100;
                // 26 brancos
                decimal iof = Convert.ToUInt64(registro.Substring(214, 13));
                _iof = iof / 100;
                decimal valorAbatimento = Convert.ToUInt64(registro.Substring(227, 13));
                _valorAbatimento = valorAbatimento / 100;
                decimal valorPrincipal = Convert.ToUInt64(registro.Substring(253, 13));
                _valorPrincipal = valorPrincipal / 100;
                decimal jurosMora = Convert.ToUInt64(registro.Substring(266, 13));
                _jurosMora = jurosMora / 100;
                _dataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                // 293 - 3 brancos
                _dataCredito = Utils.ToDateTime(dataCredito.ToString("##-##-##"));
                _instrucaoCancelada = Utils.ToInt32(registro.Substring(301, 4));
                // 306 - 6 brancos
                // 311 - 13 zeros
                _nomeSacado = registro.Substring(324, 30);
                // 354 - 23 brancos
                _erros = registro.Substring(377, 8);
                // 377 - Registros rejeitados ou alegação do sacado
                // 386 - 7 brancos

                _codigoLiquidacao = registro.Substring(392, 2);
                _numeroSequencial = Utils.ToInt32(registro.Substring(394, 6));

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        public static string PrimeiroCaracter(string retorno)
        {
            try
            {
                return retorno.Substring(0, 1);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao desmembrar registro.", ex);
            }
        }

        #endregion

    }
}
