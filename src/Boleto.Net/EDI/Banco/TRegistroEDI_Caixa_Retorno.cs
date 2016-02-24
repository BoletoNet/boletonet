using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet.EDI.Banco
{
    /// <summary>
	/// Classe de Integração Caixa
	/// </summary>
    public class TRegistroEDI_Caixa_Retorno : TRegistroEDI
    {

		#region Atributos e Propriedades
        private string _CodigoIdentificadorTipoRegistro = String.Empty;
        public string CodigoIdentificadorTipoRegistro
        {
            get { return _CodigoIdentificadorTipoRegistro; }
            set { _CodigoIdentificadorTipoRegistro = value; }
        }
        private string _TipoInscricaoEmpresa = String.Empty;
        public string TipoInscricaoEmpresa
        {
            get { return _TipoInscricaoEmpresa; }
            set { _TipoInscricaoEmpresa = value; }
        }
        private string _NumeroInscricaoEmpresa = String.Empty;
        public string NumeroInscricaoEmpresa
        {
            get { return _NumeroInscricaoEmpresa; }
            set { _NumeroInscricaoEmpresa = value; }
        }
        private string _CodigoEmpresa = String.Empty;
        public string CodigoEmpresa
        {
            get { return _CodigoEmpresa; }
            set { _CodigoEmpresa = value; }
        }
        private string _Branco1 = String.Empty;
        public string Branco1
        {
            get { return _Branco1; }
            set { _Branco1 = value; }
        }
        private string _IdentificacaoTituloEmpresa_NossoNumero_Modalidde = String.Empty;
        public string IdentificacaoTituloEmpresa_NossoNumero_Modalidde
        {
            get { return _IdentificacaoTituloEmpresa_NossoNumero_Modalidde; }
            set { _IdentificacaoTituloEmpresa_NossoNumero_Modalidde = value; }
        }

        private string _IdentificacaoTituloEmpresa_NossoNumero = String.Empty;
        public string IdentificacaoTituloEmpresa_NossoNumero
        {
            get { return _IdentificacaoTituloEmpresa_NossoNumero; }
            set { _IdentificacaoTituloEmpresa_NossoNumero = value; }
        }
        private string _IdentificacaoTituloCaixa_NossoNumero = String.Empty;
        public string IdentificacaoTituloCaixa_NossoNumero
        {
            get { return _IdentificacaoTituloCaixa_NossoNumero; }
            set { _IdentificacaoTituloCaixa_NossoNumero = value; }
        }
        private string _Brancos2 = String.Empty;
        public string Brancos2
        {
            get { return _Brancos2; }
            set { _Brancos2 = value; }
        }
        private string _CodigoMotivoRejeicao = String.Empty;
        public string CodigoMotivoRejeicao
        {
            get { return _CodigoMotivoRejeicao; }
            set { _CodigoMotivoRejeicao = value; }
        }
        private string _IdentificacaoOperacao = String.Empty;
        public string IdentificacaoOperacao
        {
            get { return _IdentificacaoOperacao; }
            set { _IdentificacaoOperacao = value; }
        }
        private string _CodigoCarteira = String.Empty;
        public string CodigoCarteira
        {
            get { return _CodigoCarteira; }
            set { _CodigoCarteira = value; }
        }
        private string _CodigoOcorrencia = String.Empty;
        public string CodigoOcorrencia
        {
            get { return _CodigoOcorrencia; }
            set { _CodigoOcorrencia = value; }
        }
        private string _DataOcorrencia = String.Empty;
        public string DataOcorrencia
        {
            get { return _DataOcorrencia; }
            set { _DataOcorrencia = value; }
        }
        private string _NumeroDocumento = String.Empty;
        public string NumeroDocumento
        {
            get { return _NumeroDocumento; }
            set { _NumeroDocumento = value; }
        }
        private string _Brancos3 = String.Empty;
        public string Brancos3
        {
            get { return _Brancos3; }
            set { _Brancos3 = value; }
        }
        private string _DataVencimentoTitulo = String.Empty;
        public string DataVencimentoTitulo
        {
            get { return _DataVencimentoTitulo; }
            set { _DataVencimentoTitulo = value; }
        }
        private string _ValorTitulo = String.Empty;
        public string ValorTitulo
        {
            get { return _ValorTitulo; }
            set { _ValorTitulo = value; }
        }
        private string _CodigoBancoCobrador = String.Empty;
        public string CodigoBancoCobrador
        {
            get { return _CodigoBancoCobrador; }
            set { _CodigoBancoCobrador = value; }
        }
        private string _CodigoAgenciaCobradora = String.Empty;
        public string CodigoAgenciaCobradora
        {
            get { return _CodigoAgenciaCobradora; }
            set { _CodigoAgenciaCobradora = value; }
        }
        private string _EspecieTitulo = String.Empty;
        public string EspecieTitulo
        {
            get { return _EspecieTitulo; }
            set { _EspecieTitulo = value; }
        }
        private string _ValorDespesasCobranca = String.Empty;
        public string ValorDespesasCobranca
        {
            get { return _ValorDespesasCobranca; }
            set { _ValorDespesasCobranca = value; }
        }
        private string _TipoLiquidacao = String.Empty;
        public string TipoLiquidacao
        {
            get { return _TipoLiquidacao; }
            set { _TipoLiquidacao = value; }
        }
        private string _FormaPagamentoUtilizada = String.Empty;
        public string FormaPagamentoUtilizada
        {
            get { return _FormaPagamentoUtilizada; }
            set { _FormaPagamentoUtilizada = value; }
        }
        private string _FloatNegociado = String.Empty;
        public string FloatNegociado
        {
            get { return _FloatNegociado; }
            set { _FloatNegociado = value; }
        }
        private string _DataDebitoTarifaLiquidacao = String.Empty;
        public string DataDebitoTarifaLiquidacao
        {
            get { return _DataDebitoTarifaLiquidacao; }
            set { _DataDebitoTarifaLiquidacao = value; }
        }
        private string _Brancos4 = String.Empty;
        public string Brancos4
        {
            get { return _Brancos4; }
            set { _Brancos4 = value; }
        }
        private string _ValorIOF = String.Empty;
        public string ValorIOF
        {
            get { return _ValorIOF; }
            set { _ValorIOF = value; }
        }
        private string _ValorAbatimentoConcedido = String.Empty;
        public string ValorAbatimentoConcedido
        {
            get { return _ValorAbatimentoConcedido; }
            set { _ValorAbatimentoConcedido = value; }
        }
        private string _ValorDescontoConcedido = String.Empty;
        public string ValorDescontoConcedido
        {
            get { return _ValorDescontoConcedido; }
            set { _ValorDescontoConcedido = value; }
        }
        private string _ValorPago = String.Empty;
        public string ValorPago
        {
            get { return _ValorPago; }
            set { _ValorPago = value; }
        }
        private string _ValorJuros = String.Empty;
        public string ValorJuros
        {
            get { return _ValorJuros; }
            set { _ValorJuros = value; }
        }
        private string _ValorMulta = String.Empty;
        public string ValorMulta
        {
            get { return _ValorMulta; }
            set { _ValorMulta = value; }
        }
        private string _CodigoMoeda = String.Empty;
        public string CodigoMoeda
        {
            get { return _CodigoMoeda; }
            set { _CodigoMoeda = value; }
        }
        private string _DataCreditoConta = String.Empty;
        public string DataCreditoConta
        {
            get { return _DataCreditoConta; }
            set { _DataCreditoConta = value; }
        }
        private string _Brancos5 = String.Empty;
        public string Brancos5
        {
            get { return _Brancos5; }
            set { _Brancos5 = value; }
        }
        private string _NumeroSequenciaRegistro = String.Empty;
        public string NumeroSequenciaRegistro
        {
            get { return _NumeroSequenciaRegistro; }
            set { _NumeroSequenciaRegistro = value; }
        }
        #endregion
    
        public TRegistroEDI_Caixa_Retorno()
        {
            /*
             * Aqui é que iremos informar as características de cada campo do arquivo
             * Na classe base, TCampoRegistroEDI, temos a propriedade CamposEDI, que é uma coleção de objetos
             * TCampoRegistroEDI.
             */
            #region TODOS os Campos
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, string.Empty, ' ')); //001-001
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 002, 0, string.Empty, ' ')); //002-003
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0004, 014, 0, string.Empty, ' ')); //004-017
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0018, 004, 0, string.Empty, ' ')); //018-021
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0022, 006, 0, string.Empty, ' ')); //0022-027
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0028, 001, 0, string.Empty, ' ')); //0028-0028
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0029, 001, 0, string.Empty, ' ')); //0029-0029
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0030, 002, 0, string.Empty, ' ')); //0030-0031
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0032, 025, 0, string.Empty, ' ')); //0032-0056
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0057, 002, 0, string.Empty, ' ')); //0057-0058
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0059, 015, 0, string.Empty, ' ')); //0059-073
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0074, 006, 0, string.Empty, ' ')); //074-079
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0080, 003, 0, string.Empty, ' ')); //080-082
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0083, 024, 0, string.Empty, ' ')); //083-106
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0107, 002, 0, string.Empty, ' ')); //107-108
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0109, 002, 0, string.Empty, ' ')); //109-110
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0111, 006, 0, string.Empty, ' ')); //111-116
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0117, 010, 0, string.Empty, ' ')); //117-126
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0127, 020, 0, string.Empty, ' ')); //127-146
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0147, 006, 0, string.Empty, ' ')); //147-152
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0153, 013, 0, string.Empty, ' ')); //153-165
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0166, 003, 0, string.Empty, ' ')); //166-168
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0169, 005, 0, string.Empty, ' ')); //169-173
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0174, 002, 0, string.Empty, ' ')); //174-175
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0176, 013, 0, string.Empty, ' ')); //176-188
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0189, 003, 0, string.Empty, ' ')); //189-191
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0192, 001, 0, string.Empty, ' ')); //192-192
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0193, 002, 0, string.Empty, ' ')); //193-194
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0195, 006, 0, string.Empty, ' ')); //195-200
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0201, 014, 0, string.Empty, ' ')); //201-214
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0215, 013, 0, string.Empty, ' ')); //215-227
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0228, 013, 0, string.Empty, ' ')); //228-240
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0241, 013, 0, string.Empty, ' ')); //241-253
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0254, 013, 0, string.Empty, ' ')); //254-266
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0267, 013, 0, string.Empty, ' ')); //267-279
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0280, 013, 0, string.Empty, ' ')); //280-292
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0293, 001, 0, string.Empty, ' ')); //293-293
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0294, 006, 0, string.Empty, ' ')); //294-299
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0300, 095, 0, string.Empty, ' ')); //300-394
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0395, 006, 0, string.Empty, ' ')); //395-400
            #endregion
        }
		
		/// <summary>
		/// Aqui iremos atribuir os valores das propriedades em cada campo correspondente do Registro EDI
		/// e codificaremos a linha para obter uma string formatada com o nosso layout.
		/// Repare que declarei as propriedades em uma ordem tal que a adição dos objetos TCampoRegistroEDI na propriedade
		/// _CamposEDI siga a mesma ordem. Portanto, utilizarei o índice na atribuição.
		/// </summary>
        public override void CodificarLinha()
        {
            #region Todos os Campos
            //PARA LEITURA DO RETORNO BANCÁRIO NÃO PRECISAMOS IMPLEMENTAR ESSE MÉTODO           
            #endregion
            //
            base.CodificarLinha(); //Aqui que eu chamo efetivamente a rotina de codificação; o resultado será exibido na propriedade LinhaRegistro.
        }
		
		/// <summary>
		/// Agora, faço o inverso da codificação. Decodifico o valor da propriedade LinhaRegistro e separo em cada campo.
		/// Cada campo é separado na propriedade ValorNatural de cada item da prop. _CamposEDI. Como esta é do tipo object, para atribuir
		/// nas propriedades do registro é necessário fazer um cast para o tipo de dado adequado. Caso ocorra algum erro na decodificação,
		/// uma exceção será disparada, provavelmente por causa de impossibilidade de fazer um cast na classe pai. Portanto, o layout deve estar
		/// correto!
		/// </summary>
		public override void DecodificarLinha()
		{
			base.DecodificarLinha();
            //
            this._CodigoIdentificadorTipoRegistro = (string)this._CamposEDI[0].ValorNatural;
            this._TipoInscricaoEmpresa = (string)this._CamposEDI[1].ValorNatural;        
            this._NumeroInscricaoEmpresa = (string)this._CamposEDI[2].ValorNatural;
            this._CodigoEmpresa = (string)this._CamposEDI[4].ValorNatural;

           // this._Branco1 = (string)this._CamposEDI[4].ValorNatural;
            this.IdentificacaoTituloEmpresa_NossoNumero_Modalidde  = (string)this._CamposEDI[9].ValorNatural;
            this._IdentificacaoTituloCaixa_NossoNumero = (string)this._CamposEDI[10].ValorNatural;
         //   this._IdentificacaoTituloEmpresa_NossoNumero = (string)this._CamposEDI[5].ValorNatural;
            
           // this._Brancos2 = (string)this._CamposEDI[7].ValorNatural;
            this._CodigoMotivoRejeicao = (string)this._CamposEDI[12].ValorNatural;
           // this._IdentificacaoOperacao = (string)this._CamposEDI[9].ValorNatural;
            this._CodigoCarteira = (string)this._CamposEDI[14].ValorNatural;
            this._CodigoOcorrencia = (string)this._CamposEDI[15].ValorNatural;
            this._DataOcorrencia = (string)this._CamposEDI[16].ValorNatural;
            this._NumeroDocumento = (string)this._CamposEDI[17].ValorNatural;
           // this._Brancos3 = (string)this._CamposEDI[18].ValorNatural;
            this._DataVencimentoTitulo = (string)this._CamposEDI[19].ValorNatural;
            this._ValorTitulo = (string)this._CamposEDI[20].ValorNatural;
            this._CodigoBancoCobrador = (string)this._CamposEDI[21].ValorNatural;
            this._CodigoAgenciaCobradora = (string)this._CamposEDI[22].ValorNatural;
            this._EspecieTitulo = (string)this._CamposEDI[23].ValorNatural;
            this._ValorDespesasCobranca = (string)this._CamposEDI[24].ValorNatural;

            this._TipoLiquidacao = (string)this._CamposEDI[25].ValorNatural;
            this._FormaPagamentoUtilizada = (string)this._CamposEDI[26].ValorNatural;
            this._FloatNegociado = (string)this._CamposEDI[27].ValorNatural;
            this._DataDebitoTarifaLiquidacao = (string)this._CamposEDI[28].ValorNatural;

           // this._Brancos4 = (string)this._CamposEDI[25].ValorNatural;
            this._ValorIOF = (string)this._CamposEDI[30].ValorNatural;
            this._ValorAbatimentoConcedido = (string)this._CamposEDI[31].ValorNatural;
            this._ValorDescontoConcedido = (string)this._CamposEDI[32].ValorNatural;
            this._ValorPago = (string)this._CamposEDI[33].ValorNatural;
            this._ValorJuros = (string)this._CamposEDI[34].ValorNatural;
            this._ValorMulta = (string)this._CamposEDI[35].ValorNatural;
            this._CodigoMoeda = (string)this._CamposEDI[36].ValorNatural;
            this._DataCreditoConta = (string)this._CamposEDI[37].ValorNatural;
            //this._Brancos5 = (string)this._CamposEDI[38].ValorNatural;
            this._NumeroSequenciaRegistro = (string)this._CamposEDI[39].ValorNatural; 
            //
		}
	}

	/// <summary>
	/// Classe que irá representar o arquivo EDI em si
	/// </summary>
    public class TArquivoCaixaRetorno_EDI : TEDIFile
	{
		/*
		 * De modo geral, apenas preciso sobreescrever o método de decodificação de linhas,
		 * pois preciso adicionar um objeto do tipo registro na coleção do arquivo, passar a linha que vem do arquivo
		 * neste objeto novo, e decodificá-lo para separar nos campos.
		 * O DecodeLine é chamado a partir do método LoadFromFile() (ou Stream) da classe base.
		 */
		protected override void DecodeLine(string Line)
		{
			base.DecodeLine(Line);
            Lines.Add(new TRegistroEDI_Caixa_Retorno()); //Adiciono a linha a ser decodificada
			Lines[Lines.Count - 1].LinhaRegistro = Line; //Atribuo a linha que vem do arquivo
            Lines[Lines.Count - 1].DecodificarLinha(); //Finalmente, a separação das substrings na linha do arquivo.
		}
	}
	

}
