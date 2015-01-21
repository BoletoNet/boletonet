using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class DetalheSegmentoURetornoCNAB240
    {

        #region Variáveis
        Decimal _Servico_Codigo_Movimento_Retorno;
        decimal _jurosMultaEncargos;
        decimal _valorDescontoConcedido;
        decimal _valorAbatimentoConcedido;
        decimal _valorIOFRecolhido;
        decimal _valorPagoPeloSacado;
        decimal _valorLiquidoASerCreditado;
        decimal _valorOutrasDespesas;
        decimal _valorOutrosCreditos;
        DateTime _dataOcorrencia;
        DateTime _dataCredito;
        string _codigoOcorrenciaSacado;
        DateTime _dataOcorrenciaSacado;
        decimal _valorOcorrenciaSacado;
        string _registro;

        private List<DetalheSegmentoURetornoCNAB240> _listaDetalhe = new List<DetalheSegmentoURetornoCNAB240>();

        #endregion

        #region Construtores

        public DetalheSegmentoURetornoCNAB240(string registro)
		{
            _registro = registro;
        }

        public DetalheSegmentoURetornoCNAB240()
        {
        }

        #endregion

        #region Propriedades
        public Decimal Servico_Codigo_Movimento_Retorno
        {
            get { return _Servico_Codigo_Movimento_Retorno; }
            set { _Servico_Codigo_Movimento_Retorno = value; }
        }

        public decimal JurosMultaEncargos
        {
            get { return _jurosMultaEncargos; }
            set { _jurosMultaEncargos = value; }
        }

        public decimal ValorDescontoConcedido
        {
            get { return _valorDescontoConcedido; }
            set { _valorDescontoConcedido = value; }
        }

        public decimal ValorAbatimentoConcedido
        {
            get { return _valorAbatimentoConcedido; }
            set { _valorAbatimentoConcedido = value; }
        }

        public decimal ValorIOFRecolhido
        {
            get { return _valorIOFRecolhido; }
            set { _valorIOFRecolhido = value; }
        }

        public decimal ValorPagoPeloSacado
        {
            get { return _valorPagoPeloSacado; }
            set { _valorPagoPeloSacado = value; }
        }

        public decimal ValorLiquidoASerCreditado
        {
            get { return _valorLiquidoASerCreditado; }
            set { _valorLiquidoASerCreditado = value; }
        }

        public decimal ValorOutrasDespesas
        {
            get { return _valorOutrasDespesas; }
            set { _valorOutrasDespesas = value; }
        }

        public decimal ValorOutrosCreditos
        {
            get { return _valorOutrosCreditos; }
            set { _valorOutrosCreditos = value; }
        }

        public DateTime DataOcorrencia
        {
            get { return _dataOcorrencia; }
            set { _dataOcorrencia = value; }
        }

        public DateTime DataCredito
        {
            get { return _dataCredito; }
            set { _dataCredito = value; }
        }

        public string CodigoOcorrenciaSacado
        {
            get { return _codigoOcorrenciaSacado; }
            set { _codigoOcorrenciaSacado = value; }
        }

        public DateTime DataOcorrenciaSacado
        {
            get { return _dataOcorrenciaSacado; }
            set { _dataOcorrenciaSacado = value; }
        }

        public decimal ValorOcorrenciaSacado

        {
            get { return _valorOcorrenciaSacado; }
            set { _valorOcorrenciaSacado = value; }
        }

        public List<DetalheSegmentoURetornoCNAB240> ListaDetalhe
        {
            get { return _listaDetalhe; }
            set { _listaDetalhe = value; }
        }

        public string Registro
        {
            get { return _registro; }
        }

        #endregion

        #region Métodos de Instância

        public void LerDetalheSegmentoURetornoCNAB240(string registro)
        {
            try
            {
                _registro = Registro;

                if (registro.Substring(13, 1) != "U")
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento U.");

                int dataOcorrenciaSacado = 0;
                if (registro.Substring(153, 4) != "    ")
                    dataOcorrenciaSacado = Convert.ToInt32(registro.Substring(157, 8));

                decimal jurosMultaEncargos = Convert.ToInt64(registro.Substring(17, 15));
                JurosMultaEncargos = jurosMultaEncargos / 100;
                decimal valorDescontoConcedido = Convert.ToInt64(registro.Substring(32, 15));
                ValorDescontoConcedido = valorDescontoConcedido / 100;
                decimal valorAbatimentoConcedido = Convert.ToInt64(registro.Substring(47, 15));
                ValorAbatimentoConcedido = valorAbatimentoConcedido / 100;
                decimal valorIOFRecolhido = Convert.ToInt64(registro.Substring(62, 15));
                ValorIOFRecolhido = valorIOFRecolhido / 100;
                decimal valorPagoPeloSacado = Convert.ToInt64(registro.Substring(77, 15));
                ValorPagoPeloSacado = valorPagoPeloSacado / 100;
                decimal valorLiquidoASerCreditado = Convert.ToInt64(registro.Substring(92, 15));
                ValorLiquidoASerCreditado = valorLiquidoASerCreditado / 100;
                decimal valorOutrasDespesas = Convert.ToInt64(registro.Substring(107, 15));
                ValorOutrasDespesas = valorOutrasDespesas / 100;
                decimal valorOutrosCreditos = Convert.ToInt64(registro.Substring(122, 15));
                ValorOutrosCreditos = valorOutrosCreditos / 100;
                int dataOcorrencia = Convert.ToInt32(registro.Substring(137, 8));
                DataOcorrencia = Convert.ToDateTime(dataOcorrencia.ToString("##-##-####"));
                int dataCredito = Convert.ToInt32(registro.Substring(145, 8));
                if (dataCredito != 0)
                    DataCredito = Convert.ToDateTime(dataCredito.ToString("##-##-####"));
                CodigoOcorrenciaSacado = registro.Substring(153, 4);
                if (dataOcorrenciaSacado != 0)
                    DataOcorrenciaSacado = Convert.ToDateTime(dataOcorrenciaSacado.ToString("##-##-####"));
                decimal valorOcorrenciaSacado = Convert.ToInt64(registro.Substring(165, 15));
                ValorOcorrenciaSacado = valorOcorrenciaSacado / 100;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO U.", ex);
            }
        }

        #endregion
    }
}
