using System;

namespace BoletoNet
{
    public class HeaderRetorno
    {

        #region Vari�veis

        private string _literalRetorno = string.Empty;
        private string _literalServico = string.Empty;
        private string _complementoRegistro2 = string.Empty;
        private string _complementoRegistro3 = string.Empty;
        private string _nomeEmpresa = string.Empty;
        private string _codigoEmpresa = string.Empty;
        private DateTime _dataGeracao = new DateTime(1, 1, 1);
        private string _unidadeDensidade = string.Empty;
        private DateTime _dataCredito = new DateTime(1, 1, 1);
        private string _nomeBanco = string.Empty;

        #endregion

        #region Construtores

        public HeaderRetorno()
        {
            NumeroSequencial = 0;
            CodigoBanco = 0;
            Conta = 0;
            Agencia = 0;
            NumeroSequencialArquivoRetorno = 0;
            Densidade = 0;
            ComplementoRegistro1 = 0;
            CodigoServico = 0;
            CodigoRetorno = 0;
            TipoRegistro = 0;
            DACConta = 0;
        }

        public HeaderRetorno(string registro)
        {
            NumeroSequencial = 0;
            CodigoBanco = 0;
            Conta = 0;
            Agencia = 0;
            NumeroSequencialArquivoRetorno = 0;
            Densidade = 0;
            ComplementoRegistro1 = 0;
            CodigoServico = 0;
            CodigoRetorno = 0;
            TipoRegistro = 0;
            DACConta = 0;
            Registro = registro;
            this.LerHeaderRetornoCNAB400(registro);
        }

        #endregion

        #region Propriedades


        public int TipoRegistro { get; set; }

        public int CodigoRetorno { get; set; }

        public int CodigoServico { get; set; }

        public int ComplementoRegistro1 { get; set; }

        public string ComplementoRegistro2
        {
            get { return _complementoRegistro2; }
            set { _complementoRegistro2 = value; }
        }

        public string ComplementoRegistro3
        {
            get { return _complementoRegistro3; }
            set { _complementoRegistro3 = value; }
        }

        public int Densidade { get; set; }

        public int NumeroSequencialArquivoRetorno { get; set; }


        /// <summary>
        /// Ag�ncia com o D�gito Verificador, quando houver
        /// </summary>
        public int Agencia { get; set; }

        public int Conta { get; set; }

        public int DACConta { get; set; }

        public int CodigoBanco { get; set; }

        public DateTime DataCredito
        {
            get { return _dataCredito; }
            set { _dataCredito = value; }
        }

        public int NumeroSequencial { get; set; }

        public string Registro { get; private set; }

        public string LiteralRetorno
        {
            get { return _literalRetorno; }
            set { _literalRetorno = value; }
        }

        public string LiteralServico
        {
            get { return _literalServico; }
            set { _literalServico = value; }
        }

        public string NomeEmpresa
        {
            get { return _nomeEmpresa; }
            set { _nomeEmpresa = value; }
        }

        public string CodigoEmpresa
        {
            get { return _codigoEmpresa; }
            set { _codigoEmpresa = value; }
        }

        public DateTime DataGeracao
        {
            get { return _dataGeracao; }
            set { _dataGeracao = value; }
        }

        public string UnidadeDensidade
        {
            get { return _unidadeDensidade; }
            set { _unidadeDensidade = value; }
        }

        public string NomeBanco
        {
            get { return _nomeBanco; }
            set { _nomeBanco = value; }
        }

        public string Versao { get; set; }

        public string Mensagem { get; set; }

        #endregion

        #region M�todos de Inst�ncia

        public void LerHeaderRetornoCNAB400(string registro)
        {
            try
            {
                this.TipoRegistro = Utils.ToInt32(registro.Substring(000, 1));
                this.CodigoRetorno = Utils.ToInt32(registro.Substring(001, 1));
                this.LiteralRetorno = registro.Substring(002, 7);
                this.CodigoServico = Utils.ToInt32(registro.Substring(009, 2));
                this.LiteralServico = registro.Substring(011, 15);
                this.Agencia = Utils.ToInt32(registro.Substring(026, 4));
                this.ComplementoRegistro1 = Utils.ToInt32(registro.Substring(030, 2));
                this.Conta = Utils.ToInt32(registro.Substring(032, 5));
                this.DACConta = Utils.ToInt32(registro.Substring(037, 1));
                this.ComplementoRegistro2 = registro.Substring(038, 8);
                this.NomeEmpresa = registro.Substring(046, 30);
                this.CodigoBanco = Utils.ToInt32(registro.Substring(076, 3));
                this.NomeBanco = registro.Substring(079, 15);
                this.DataGeracao = Utils.ToDateTime(Utils.ToInt32(registro.Substring(094, 6)).ToString("##-##-##"));
                this.Densidade = Utils.ToInt32(registro.Substring(100, 5));
                this.UnidadeDensidade = registro.Substring(105, 3);
                this.NumeroSequencialArquivoRetorno = Utils.ToInt32(registro.Substring(108, 5));
                this.DataCredito = Utils.ToDateTime(Utils.ToInt32(registro.Substring(113, 6)).ToString("##-##-##"));
                this.ComplementoRegistro3 = registro.Substring(119, 275);
                this.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler header do arquivo de RETORNO / CNAB 400.", ex);
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
