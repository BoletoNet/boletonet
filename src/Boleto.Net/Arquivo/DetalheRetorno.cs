using System;

namespace BoletoNet
{
    public class DetalheRetorno
    {

        #region Variáveis

        private string _numeroInscricao = string.Empty;
        private string _usoEmpresa = string.Empty;
        private string _dacNossoNumero = string.Empty;
        private string _carteira = string.Empty;
        private string _descOcorrencia = string.Empty;
        private DateTime _dataCredito = new DateTime(1, 1, 1);
        private string _erros = string.Empty;
        private string _codigoLiquidacao = string.Empty;
        private readonly string _registro = string.Empty;
        private string _origemPagamento = string.Empty;
        private string _identificacaoTitulo = string.Empty;
        private string _numeroControle = string.Empty;

        #region Propriedades BRB

        private string _cgcCpf = string.Empty;
        private string _nossoNumero = string.Empty;
        private string _seuNumero = string.Empty;
        private DateTime _dataOcorrencia = new DateTime(1, 1, 1);
        private string _numeroDocumento = string.Empty;
        private DateTime _dataVencimento = new DateTime(1, 1, 1);
        private string _especieTitulo = string.Empty;
        private DateTime _dataLiquidacao = new DateTime(1, 1, 1);

        #endregion

        #endregion

        #region Construtores

        public DetalheRetorno()
        {
            Sequencial = 0;
            Abatimentos = 0;
            Juros = 0;
            OutrasDespesas = 0;
            DespeasaDeCobranca = 0;
            BancoCobrador = 0;
            CodigoRateio = 0;
            Instrucao = 0;
            ContaCorrente = 0;
            TipoInscricao = 0;
            IdentificacaoDoRegistro = 0;
            NumeroCartorio = 0;
            ValorPago = 0;
            ValorOutrasDespesas = 0;
            ValorDespesa = 0;
            NumeroSequencial = 0;
            InstrucaoCancelada = 0;
            OutrosDebitos = 0;
            OutrosCreditos = 0;
            JurosMora = 0;
            ValorPrincipal = 0;
            Descontos = 0;
            ValorAbatimento = 0;
            IOF = 0;
            TarifaCobranca = 0;
            Especie = 0;
            DACAgenciaCobradora = 0;
            AgenciaCobradora = 0;
            CodigoBanco = 0;
            ValorTitulo = 0;
            ConfirmacaoNossoNumero = 0;
            CodigoOcorrencia = 0;
            DACConta = 0;
            Conta = 0;
            Agencia = 0;
            MotivoCodigoOcorrencia = string.Empty;
            MotivosRejeicao = string.Empty;
            NumeroProtocolo = string.Empty;
            NomeSacado = string.Empty;
        }

        public DetalheRetorno(string registro)
        {
            Sequencial = 0;
            Abatimentos = 0;
            Juros = 0;
            OutrasDespesas = 0;
            DespeasaDeCobranca = 0;
            BancoCobrador = 0;
            CodigoRateio = 0;
            Instrucao = 0;
            ContaCorrente = 0;
            TipoInscricao = 0;
            IdentificacaoDoRegistro = 0;
            NumeroCartorio = 0;
            ValorPago = 0;
            ValorOutrasDespesas = 0;
            ValorDespesa = 0;
            NumeroSequencial = 0;
            InstrucaoCancelada = 0;
            OutrosDebitos = 0;
            OutrosCreditos = 0;
            JurosMora = 0;
            ValorPrincipal = 0;
            Descontos = 0;
            ValorAbatimento = 0;
            IOF = 0;
            TarifaCobranca = 0;
            Especie = 0;
            DACAgenciaCobradora = 0;
            AgenciaCobradora = 0;
            CodigoBanco = 0;
            ValorTitulo = 0;
            ConfirmacaoNossoNumero = 0;
            CodigoOcorrencia = 0;
            DACConta = 0;
            Conta = 0;
            Agencia = 0;
            _registro = registro;
            MotivoCodigoOcorrencia = string.Empty;
            MotivosRejeicao = string.Empty;
            NumeroProtocolo = string.Empty;
            NomeSacado = string.Empty;
        }

        #endregion

        #region Propriedades

        public int CodigoInscricao { get; set; }

        public string NumeroInscricao
        {
            get { return _numeroInscricao; }
            set { _numeroInscricao = value; }
        }
        /// <summary>
        /// Agência com o Dígito Verificador, quando houver
        /// </summary>
        public int Agencia { get; set; }

        public int Conta { get; set; }

        public int DACConta { get; set; }

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
        public string NossoNumeroComDV { get; set; }

        public string Carteira
        {
            get { return _carteira; }
            set { _carteira = value; }
        }

        public int CodigoOcorrencia { get; set; }

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

        public int ConfirmacaoNossoNumero { get; set; }

        public DateTime DataVencimento
        {
            get { return _dataVencimento; }
            set { _dataVencimento = value; }
        }

        public decimal ValorTitulo { get; set; }

        public int CodigoBanco { get; set; }

        public int AgenciaCobradora { get; set; }

        public int DACAgenciaCobradora { get; set; }

        public int Especie { get; set; }

        public decimal TarifaCobranca { get; set; }

        public decimal IOF { get; set; }

        public decimal ValorAbatimento { get; set; }

        public decimal Descontos { get; set; }

        public decimal ValorPrincipal { get; set; }

        public decimal JurosMora { get; set; }

        public decimal OutrosCreditos { get; set; }

        public decimal OutrosDebitos { get; set; }

        public DateTime DataCredito
        {
            get { return _dataCredito; }
            set { _dataCredito = value; }
        }

        public int InstrucaoCancelada { get; set; }

        public string NomeSacado { get; set; }

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

        public int NumeroSequencial { get; set; }

        public string Registro
        {
            get { return _registro; }
        }
        public decimal ValorDespesa { get; set; }

        public decimal ValorOutrasDespesas { get; set; }

        public decimal ValorPago { get; set; }

        public string MotivoCodigoOcorrencia { get; set; }

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
        public string MotivosRejeicao { get; set; }

        public string NumeroProtocolo { get; set; }

        public int NumeroCartorio { get; set; }

        public string NumeroControle
        {
            get { return _numeroControle; }
            set { _numeroControle = value; }
        }

        public int IdentificacaoDoRegistro { get; set; }

        public int TipoInscricao { get; set; }

        public string CgcCpf
        {
            get { return _cgcCpf; }
            set { _cgcCpf = value; }
        }

        public int ContaCorrente { get; set; }

        public string SeuNumero
        {
            get { return _seuNumero; }
            set { _seuNumero = value; }
        }

        public int Instrucao { get; set; }

        public int CodigoRateio { get; set; }

        public int BancoCobrador { get; set; }

        public string EspecieTitulo
        {
            get { return _especieTitulo; }
            set { _especieTitulo = value; }
        }

        public decimal DespeasaDeCobranca { get; set; }

        public decimal OutrasDespesas { get; set; }

        public decimal Juros { get; set; }

        public decimal Abatimentos { get; set; }

        public DateTime DataLiquidacao
        {
            get { return _dataLiquidacao; }
            set { _dataLiquidacao = value; }
        }

        public int Sequencial { get; set; }

        public decimal ValorMulta { get; set; }

        #endregion

        #region Métodos de Instância

        public void LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                int dataOcorrencia = Utils.ToInt32(registro.Substring(110, 6));
                int dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
                int dataCredito = Utils.ToInt32(registro.Substring(295, 6));

                CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2));
                _numeroInscricao = registro.Substring(3, 14);
                Agencia = Utils.ToInt32(registro.Substring(17, 4));
                Conta = Utils.ToInt32(registro.Substring(23, 5));
                DACConta = Utils.ToInt32(registro.Substring(28, 1));
                _usoEmpresa = registro.Substring(37, 25);
                _nossoNumero = Utils.ToString(registro.Substring(85, 8));
                _dacNossoNumero = registro.Substring(93, 1);
                _carteira = registro.Substring(107, 1);
                CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));
                _dataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                _numeroDocumento = registro.Substring(116, 10);
                _nossoNumero = Utils.ToString(registro.Substring(126, 9));
                _dataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                decimal valorTitulo = Convert.ToInt64(registro.Substring(152, 13));
                ValorTitulo = valorTitulo / 100;
                CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));
                BancoCobrador = Utils.ToInt32(registro.Substring(165, 3));
                AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 4));
                Especie = Utils.ToInt32(registro.Substring(173, 2));
                decimal tarifaCobranca = Convert.ToUInt64(registro.Substring(175, 13));
                TarifaCobranca = tarifaCobranca / 100;
                // 26 brancos
                decimal iof = Convert.ToUInt64(registro.Substring(214, 13));
                IOF = iof / 100;
                decimal valorAbatimento = Convert.ToUInt64(registro.Substring(227, 13));
                ValorAbatimento = valorAbatimento / 100;
                decimal valorPrincipal = Convert.ToUInt64(registro.Substring(253, 13));
                ValorPrincipal = valorPrincipal / 100;
                decimal jurosMora = Convert.ToUInt64(registro.Substring(266, 13));
                JurosMora = jurosMora / 100;
                _dataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                _dataCredito = Utils.ToDateTime(dataCredito.ToString("##-##-##"));
                InstrucaoCancelada = Utils.ToInt32(registro.Substring(301, 4));
                NomeSacado = registro.Substring(324, 30);
                _erros = registro.Substring(377, 8);

                _codigoLiquidacao = registro.Substring(392, 2);
                NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));

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
