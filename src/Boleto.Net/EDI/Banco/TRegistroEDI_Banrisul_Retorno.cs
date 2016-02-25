using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet.EDI.Banco
{
    /// <summary>
	/// Classe de Integração Banrisul
	/// </summary>
    public class TRegistroEDI_Banrisul_Retorno : TRegistroEDI
    {

		#region Atributos e Propriedades
        private string _Constante1 = String.Empty;
        public string Constante1
        {
            get { return _Constante1; }
            set { _Constante1 = value; }
        }
        private string _TipoInscricao = String.Empty;
        public string TipoInscricao
        {
            get { return _TipoInscricao; }
            set { _TipoInscricao = value; }
        }
        private string _CpfCnpj = String.Empty;
        public string CpfCnpj
        {
            get { return _CpfCnpj; }
            set { _CpfCnpj = value; }
        }
        private string _CodigoCedente = String.Empty;
        public string CodigoCedente
        {
            get { return _CodigoCedente; }
            set { _CodigoCedente = value; }
        }
        private string _EspecieCobrancaRegistrada = String.Empty;
        public string EspecieCobrancaRegistrada
        {
            get { return _EspecieCobrancaRegistrada; }
            set { _EspecieCobrancaRegistrada = value; }
        }
        private string _Branco1 = String.Empty;
        public string Branco1
        {
            get { return _Branco1; }
            set { _Branco1 = value; }
        }
        private string _IdentificacaoTituloCedente = String.Empty;
        public string IdentificacaoTituloCedente
        {
            get { return _IdentificacaoTituloCedente; }
            set { _IdentificacaoTituloCedente = value; }
        }
        private string _IdentificacaoTituloBanco_NossoNumero = String.Empty;
        public string IdentificacaoTituloBanco_NossoNumero
        {
            get { return _IdentificacaoTituloBanco_NossoNumero; }
            set { _IdentificacaoTituloBanco_NossoNumero = value; }
        }
        private string _IdentificacaoTituloBanco_NossoNumeroOpcional = String.Empty;
        public string IdentificacaoTituloBanco_NossoNumeroOpcional
        {
            get { return _IdentificacaoTituloBanco_NossoNumeroOpcional; }
            set { _IdentificacaoTituloBanco_NossoNumeroOpcional = value; }
        }
        private string _NumeroContratoBLU = String.Empty;
        public string NumeroContratoBLU
        {
            get { return _NumeroContratoBLU; }
            set { _NumeroContratoBLU = value; }
        }
        private string _Brancos2 = String.Empty;
        public string Brancos2
        {
            get { return _Brancos2; }
            set { _Brancos2 = value; }
        }
        private string _TipoCarteira = String.Empty;
        public string TipoCarteira
        {
            get { return _TipoCarteira; }
            set { _TipoCarteira = value; }
        }
        private string _CodigoOcorrencia = String.Empty;
        public string CodigoOcorrencia
        {
            get { return _CodigoOcorrencia; }
            set { _CodigoOcorrencia = value; }
        }
        private string _DataOcorrenciaBanco = String.Empty;
        public string DataOcorrenciaBanco
        {
            get { return _DataOcorrenciaBanco; }
            set { _DataOcorrenciaBanco = value; }
        }
        private string _SeuNumero = String.Empty;
        public string SeuNumero
        {
            get { return _SeuNumero; }
            set { _SeuNumero = value; }
        }
        private string _NossoNumero = String.Empty;
        public string NossoNumero
        {
            get { return _NossoNumero; }
            set { _NossoNumero = value; }
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
        private string _TipoDocumento = String.Empty;
        public string TipoDocumento
        {
            get { return _TipoDocumento; }
            set { _TipoDocumento = value; }
        }
        private string _ValorDespesasCobranca = String.Empty;
        public string ValorDespesasCobranca
        {
            get { return _ValorDespesasCobranca; }
            set { _ValorDespesasCobranca = value; }
        }
        private string _OutrasDespesas = String.Empty;
        public string OutrasDespesas
        {
            get { return _OutrasDespesas; }
            set { _OutrasDespesas = value; }
        }
        private string _Zeros1 = String.Empty;
        public string Zeros1
        {
            get { return _Zeros1; }
            set { _Zeros1 = value; }
        }
        //
        //private string _ValorAvista = String.Empty;
        //private string _SituacaoIOF = String.Empty;
        //private string _Zeros2 = String.Empty;
        //
        private string _ValorAbatimento_DeflacaoConcedido = String.Empty;
        public string ValorAbatimento_DeflacaoConcedido
        {
            get { return _ValorAbatimento_DeflacaoConcedido; }
            set { _ValorAbatimento_DeflacaoConcedido = value; }
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
        private string _ValorOutrosRecebimentos = String.Empty;
        public string ValorOutrosRecebimentos
        {
            get { return _ValorOutrosRecebimentos; }
            set { _ValorOutrosRecebimentos = value; }
        }
        private string _Brancos3 = String.Empty;
        public string Brancos3
        {
            get { return _Brancos3; }
            set { _Brancos3 = value; }
        }
        private string _DataCreditoConta = String.Empty;
        public string DataCreditoConta
        {
            get { return _DataCreditoConta; }
            set { _DataCreditoConta = value; }
        }
        private string _Brancos4 = String.Empty;
        public string Brancos4
        {
            get { return _Brancos4; }
            set { _Brancos4 = value; }
        }
        private string _PagamentoDinheiro_Cheque = String.Empty;
        public string PagamentoDinheiro_Cheque
        {
            get { return _PagamentoDinheiro_Cheque; }
            set { _PagamentoDinheiro_Cheque = value; }
        }
        private string _Brancos5 = String.Empty;
        public string Brancos5
        {
            get { return _Brancos5; }
            set { _Brancos5 = value; }
        }
        private string _MotivoOcorrencia = String.Empty;
        public string MotivoOcorrencia
        {
            get { return _MotivoOcorrencia; }
            set { _MotivoOcorrencia = value; }
        }
        private string _Brancos6 = String.Empty;
        public string Brancos6
        {
            get { return _Brancos6; }
            set { _Brancos6 = value; }
        }
        private string _NumeroSequenciaRegistro = String.Empty;
        public string NumeroSequenciaRegistro
        {
            get { return _NumeroSequenciaRegistro; }
            set { _NumeroSequenciaRegistro = value; }
        }
        #endregion

    
        public TRegistroEDI_Banrisul_Retorno()
        {
            /*
             * Aqui é que iremos informar as características de cada campo do arquivo
             * Na classe base, TCampoRegistroEDI, temos a propriedade CamposEDI, que é uma coleção de objetos
             * TCampoRegistroEDI.
             */
            #region TODOS os Campos
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, string.Empty, ' ')); //001-001
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 001, 0, string.Empty, ' ')); //002-003
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0004, 014, 0, string.Empty, ' ')); //004-017
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0018, 013, 0, string.Empty, ' ')); //018-030
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0031, 006, 0, string.Empty, ' ')); //031-036
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0037, 001, 0, string.Empty, ' ')); //037-037
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0038, 025, 0, string.Empty, ' ')); //038-062
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0063, 010, 0, string.Empty, ' ')); //063-072
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0073, 010, 0, string.Empty, ' ')); //073-082
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0083, 022, 0, string.Empty, ' ')); //083-104
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0105, 003, 0, string.Empty, ' ')); //105-107
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0108, 001, 0, string.Empty, ' ')); //108-108
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0109, 002, 0, string.Empty, ' ')); //109-110
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0111, 006, 0, string.Empty, ' ')); //111-116
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0117, 010, 0, string.Empty, ' ')); //117-126
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0127, 020, 0, string.Empty, ' ')); //127-146
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0147, 006, 0, string.Empty, ' ')); //147-152
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0153, 013, 2, string.Empty, ' ')); //153-165
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0166, 003, 0, string.Empty, ' ')); //166-168
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0169, 005, 0, string.Empty, ' ')); //169-173
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0174, 002, 0, string.Empty, ' ')); //174-175
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0176, 013, 2, string.Empty, ' ')); //176-188
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0189, 013, 2, string.Empty, ' ')); //189-201
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0202, 026, 0, string.Empty, ' ')); //202-227
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0228, 013, 2, string.Empty, ' ')); //228-240
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0241, 013, 2, string.Empty, ' ')); //241-253
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0254, 013, 2, string.Empty, ' ')); //254-266
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0267, 013, 2, string.Empty, ' ')); //267-279
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0280, 013, 2, string.Empty, ' ')); //280-292
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0293, 003, 0, string.Empty, ' ')); //293-295
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0296, 006, 0, string.Empty, ' ')); //296-301
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0302, 041, 0, string.Empty, ' ')); //302-342
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0343, 001, 0, string.Empty, ' ')); //343-343
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0344, 039, 0, string.Empty, ' ')); //344-382
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0383, 010, 0, string.Empty, ' ')); //383-392
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0393, 002, 0, string.Empty, ' ')); //393-394
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
            this._Constante1 = (string)this._CamposEDI[0].ValorNatural;
            this._TipoInscricao = (string)this._CamposEDI[1].ValorNatural;
            this._CpfCnpj = (string)this._CamposEDI[2].ValorNatural;
            this._CodigoCedente = (string)this._CamposEDI[3].ValorNatural;
            this._EspecieCobrancaRegistrada = (string)this._CamposEDI[4].ValorNatural;
            this._Branco1 = (string)this._CamposEDI[5].ValorNatural;
            this._IdentificacaoTituloCedente = (string)this._CamposEDI[6].ValorNatural;
            this._IdentificacaoTituloBanco_NossoNumero = (string)this._CamposEDI[7].ValorNatural;
            this._IdentificacaoTituloBanco_NossoNumeroOpcional = (string)this._CamposEDI[8].ValorNatural;
            this._NumeroContratoBLU = (string)this._CamposEDI[9].ValorNatural;
            this._Brancos2 = (string)this._CamposEDI[10].ValorNatural;
            this._TipoCarteira = (string)this._CamposEDI[11].ValorNatural;
            this._CodigoOcorrencia = (string)this._CamposEDI[12].ValorNatural;
            this._DataOcorrenciaBanco = (string)this._CamposEDI[13].ValorNatural;
            this._SeuNumero = (string)this._CamposEDI[14].ValorNatural;
            this._NossoNumero = (string)this._CamposEDI[15].ValorNatural;
            this._DataVencimentoTitulo = (string)this._CamposEDI[16].ValorNatural;
            this._ValorTitulo = (string)this._CamposEDI[17].ValorNatural;
            this._CodigoBancoCobrador = (string)this._CamposEDI[18].ValorNatural;
            this._CodigoAgenciaCobradora = (string)this._CamposEDI[19].ValorNatural;
            this._TipoDocumento = (string)this._CamposEDI[20].ValorNatural;
            this._ValorDespesasCobranca = (string)this._CamposEDI[21].ValorNatural;
            this._OutrasDespesas = (string)this._CamposEDI[22].ValorNatural;
            this._Zeros1 = (string)this._CamposEDI[23].ValorNatural;
            //this._ValorAvista = (string)this._CamposEDI[0].ValorNatural;
            //this._SituacaoIOF = (string)this._CamposEDI[0].ValorNatural;
            //this._Zeros2 = (string)this._CamposEDI[0].ValorNatural;
            this._ValorAbatimento_DeflacaoConcedido = (string)this._CamposEDI[24].ValorNatural;
            this._ValorDescontoConcedido = (string)this._CamposEDI[25].ValorNatural;
            this._ValorPago = (string)this._CamposEDI[26].ValorNatural;
            this._ValorJuros = (string)this._CamposEDI[27].ValorNatural;
            this._ValorOutrosRecebimentos = (string)this._CamposEDI[28].ValorNatural;
            this._Brancos3 = (string)this._CamposEDI[29].ValorNatural;
            this._DataCreditoConta = (string)this._CamposEDI[30].ValorNatural;
            this._Brancos4 = (string)this._CamposEDI[31].ValorNatural;
            this._PagamentoDinheiro_Cheque = (string)this._CamposEDI[32].ValorNatural;
            this._Brancos5 = (string)this._CamposEDI[33].ValorNatural;
            this._MotivoOcorrencia = (string)this._CamposEDI[34].ValorNatural;
            this._Brancos6 = (string)this._CamposEDI[35].ValorNatural;
            this._NumeroSequenciaRegistro = (string)this._CamposEDI[36].ValorNatural;   
            //
		}
	}

	/// <summary>
	/// Classe que irá representar o arquivo EDI em si
	/// </summary>
    public class TArquivoBanrisulRetorno_EDI : TEDIFile
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
            Lines.Add(new TRegistroEDI_Banrisul_Retorno()); //Adiciono a linha a ser decodificada
			Lines[Lines.Count - 1].LinhaRegistro = Line; //Atribuo a linha que vem do arquivo
            Lines[Lines.Count - 1].DecodificarLinha(); //Finalmente, a separação das substrings na linha do arquivo.
		}
	}
	

}
