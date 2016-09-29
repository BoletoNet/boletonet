using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    /// <summary>
    /// Contém informações que são pertinentes a um boleto, mas para geração da Remessa. Não são necessárias para Impressão do Boleto
    /// Autor: sidneiklein Data: 09/08/2013
    /// </summary>
    public class Remessa
    {
        public enum TipoAmbiemte
        {
            Homologacao,
            Producao
        }
        //
        #region Atributos e Propriedades
        private TipoAmbiemte _Ambiente;
        /// <summary>
        /// Variável que define se a Remessa é para Testes ou Produção
        /// </summary>
        public TipoAmbiemte Ambiente
        {
            get { return _Ambiente; }
            set { _Ambiente = value; }
        }
        
        private string _TipoDocumento;
        /// <summary>
        /// Tipo Documento Utilizado na geração da remessa. |Identificado no Banrisul by sidneiklein|
        /// Tipo Cobranca Utilizado na geração da remessa.  |Identificado no Sicredi by sidneiklein|
        /// </summary>
        public string TipoDocumento
        {
            get { return _TipoDocumento; }
            set { _TipoDocumento = value; }
        }

        private string _CodigoOcorrencia;
        /// <summary>
        /// Código de Ocorrência Utilizado na geração da Remessa.
        /// |Identificado no Banrisul        como "CODIGO OCORRENCIA" by sidneiklein|
        /// |Identificado no Banco do Brasil como "COMANDO"           by sidneiklein|
        /// |Identificado no Santander como "CÓDIGO DE MOVIMENTO REMESSA"           by Leandro Morais
        /// </summary>
        public string CodigoOcorrencia
        {
            get { return _CodigoOcorrencia; }
            set { _CodigoOcorrencia = value; }
        }

        private int _NumeroLote;
        /// <summary>
        /// Numero do lote de remessa
        /// </summary>
        public int NumeroLote
        {
            get { return _NumeroLote; }
            set { _NumeroLote = value; }
        }


        #endregion

    }
}
