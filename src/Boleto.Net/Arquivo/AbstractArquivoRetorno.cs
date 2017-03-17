using System;
using System.IO;
using BoletoNet.Arquivo;

namespace BoletoNet
{
    public abstract class AbstractArquivoRetorno
    {
        public event EventHandler<LinhaDeArquivoLidaArgs> LinhaDeArquivoLida;
 
        #region Variáveis

        private IBanco _banco;
        private TipoArquivo _tipoArquivo;
        private DetalheRetorno _detalheRetorno;
        private IArquivoRetorno _arquivoRetorno;

        #endregion

        #region Construtores

        protected AbstractArquivoRetorno()
        {
        }

        public AbstractArquivoRetorno(TipoArquivo tipoArquivo)
        {
            switch (tipoArquivo)
            {
                case TipoArquivo.CNAB240:
                    _arquivoRetorno = new ArquivoRetornoCNAB240();
                    _arquivoRetorno.LinhaDeArquivoLida += ArquivoRemessa_LinhaDeArquivoLidaCNAB240;
                    break;
                case TipoArquivo.CNAB400:
                    _arquivoRetorno = new ArquivoRetornoCNAB400();
                    _arquivoRetorno.LinhaDeArquivoLida += ArquivoRemessa_LinhaDeArquivoLidaCNAB400;
                    break;
                case TipoArquivo.CBR643:
                    _arquivoRetorno = new ArquivoRetornoCrb643();
                    break;
                default:
                    throw new NotImplementedException("Arquivo não implementado.");
            }
        }

        void ArquivoRemessa_LinhaDeArquivoLidaCNAB240(object sender, LinhaDeArquivoLidaArgs e)
        {
            OnLinhaLida(e.Detalhe as DetalheRetornoCNAB240, e.Linha, e.TipoLinha);
        }

        void ArquivoRemessa_LinhaDeArquivoLidaCNAB400(object sender, LinhaDeArquivoLidaArgs e)
        {
            OnLinhaLida(e.Detalhe as DetalheRetorno, e.Linha);
        }

        #endregion

        #region Propriedades

        /// <summary>
        /// Banco
        /// </summary>
        public virtual IBanco Banco
        {
            get { return _banco; }
            protected set { _banco = value; }
        }

        /// <summary>
        /// TipoArquivo
        /// </summary>
        public virtual TipoArquivo TipoArquivo
        {
            get { return _tipoArquivo; }
            protected set { _tipoArquivo = value; }
        }

        /// <summary>
        /// Detalhe do arquivo retorno
        /// </summary>
        public virtual DetalheRetorno DetalheRetorno
        {
            get { return _detalheRetorno; }
            protected set { _detalheRetorno = value; }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Gera o arquivo de remessa
        /// </summary>
        public virtual void LerArquivoRetorno(IBanco banco, Stream arquivo)
        {
            _banco = banco;
            _arquivoRetorno.LerArquivoRetorno(banco, arquivo);
        }

        #endregion

        #region Disparadores de Eventos

        public virtual void OnLinhaLida(DetalheRetornoCNAB240 detalheRetornoCNAB240, string linha, EnumTipodeLinhaLida tipoLinha)
        {
            try
            {
                if (this.LinhaDeArquivoLida != null)
                    this.LinhaDeArquivoLida(this, new LinhaDeArquivoLidaArgs(detalheRetornoCNAB240, linha, tipoLinha));
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar evento.", ex);
            }
        }

        public virtual void OnLinhaLida(DetalheRetorno detalheRetorno, string linha)
        {
            try
            {
                if (this.LinhaDeArquivoLida != null)
                    this.LinhaDeArquivoLida(this, new LinhaDeArquivoLidaArgs(detalheRetorno, linha));
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar evento.", ex);
            }
        }
        #endregion
    }
}
