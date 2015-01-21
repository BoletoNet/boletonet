using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet.EDI.Banco
{
    /// <summary>
	/// Classe de Integração Sicredi
	/// </summary>
    public class TRegistroEDI_Sicredi_Retorno : TRegistroEDI
    {

		#region Atributos e Propriedades
        private string _IdentificacaoRegDetalhe = String.Empty;

        public string IdentificacaoRegDetalhe
        {
            get { return _IdentificacaoRegDetalhe; }
            set { _IdentificacaoRegDetalhe = value; }
        }
        private string _Filler1 = String.Empty;

        public string Filler1
        {
            get { return _Filler1; }
            set { _Filler1 = value; }
        }
        private string _TipoCobranca = String.Empty;

        public string TipoCobranca
        {
            get { return _TipoCobranca; }
            set { _TipoCobranca = value; }
        }
        private string _CodigoPagadorAgenciaBeneficiario = String.Empty;

        public string CodigoPagadorAgenciaBeneficiario
        {
            get { return _CodigoPagadorAgenciaBeneficiario; }
            set { _CodigoPagadorAgenciaBeneficiario = value; }
        }
        private string _CodigoPagadorJuntoAssociado = String.Empty;

        public string CodigoPagadorJuntoAssociado
        {
            get { return _CodigoPagadorJuntoAssociado; }
            set { _CodigoPagadorJuntoAssociado = value; }
        }
        private string _BoletoDDA = String.Empty;

        public string BoletoDDA
        {
            get { return _BoletoDDA; }
            set { _BoletoDDA = value; }
        }
        private string _Filler2 = String.Empty;

        public string Filler2
        {
            get { return _Filler2; }
            set { _Filler2 = value; }
        }
        private string _NossoNumeroSicredi = String.Empty;

        public string NossoNumeroSicredi
        {
            get { return _NossoNumeroSicredi; }
            set { _NossoNumeroSicredi = value; }
        }
        private string _Filler3 = String.Empty;

        public string Filler3
        {
            get { return _Filler3; }
            set { _Filler3 = value; }
        }
        private string _Ocorrencia = String.Empty;

        public string Ocorrencia
        {
            get { return _Ocorrencia; }
            set { _Ocorrencia = value; }
        }
        private string _DataOcorrencia = String.Empty;

        public string DataOcorrencia
        {
            get { return _DataOcorrencia; }
            set { _DataOcorrencia = value; }
        }
        private string _SeuNumero = String.Empty;

        public string SeuNumero
        {
            get { return _SeuNumero; }
            set { _SeuNumero = value; }
        }
        private string _Filler4 = String.Empty;

        public string Filler4
        {
            get { return _Filler4; }
            set { _Filler4 = value; }
        }
        private string _DataVencimento = String.Empty;

        public string DataVencimento
        {
            get { return _DataVencimento; }
            set { _DataVencimento = value; }
        }
        private string _ValorTitulo = String.Empty;

        public string ValorTitulo
        {
            get { return _ValorTitulo; }
            set { _ValorTitulo = value; }
        }
        private string _Filler5 = String.Empty;

        public string Filler5
        {
            get { return _Filler5; }
            set { _Filler5 = value; }
        }
        private string _EspecieDocumento = String.Empty;

        public string EspecieDocumento
        {
            get { return _EspecieDocumento; }
            set { _EspecieDocumento = value; }
        }
        private string _DespesasCobranca = String.Empty;

        public string DespesasCobranca
        {
            get { return _DespesasCobranca; }
            set { _DespesasCobranca = value; }
        }
        private string _DespesasCustasProtesto = String.Empty;

        public string DespesasCustasProtesto
        {
            get { return _DespesasCustasProtesto; }
            set { _DespesasCustasProtesto = value; }
        }
        private string _Filler6 = String.Empty;

        public string Filler6
        {
            get { return _Filler6; }
            set { _Filler6 = value; }
        }
        private string _AbatimentoConcedido = String.Empty;

        public string AbatimentoConcedido
        {
            get { return _AbatimentoConcedido; }
            set { _AbatimentoConcedido = value; }
        }
        private string _DescontoConcedido = String.Empty;

        public string DescontoConcedido
        {
            get { return _DescontoConcedido; }
            set { _DescontoConcedido = value; }
        }
        private string _ValorEfetivamentePago = String.Empty;

        public string ValorEfetivamentePago
        {
            get { return _ValorEfetivamentePago; }
            set { _ValorEfetivamentePago = value; }
        }
        private string _JurosMora = String.Empty;

        public string JurosMora
        {
            get { return _JurosMora; }
            set { _JurosMora = value; }
        }
        private string _Multa = String.Empty;

        public string Multa
        {
            get { return _Multa; }
            set { _Multa = value; }
        }        
        private string _Filler7 = String.Empty;

        public string Filler7
        {
            get { return _Filler7; }
            set { _Filler7 = value; }
        }
        private string _SomenteOcorrencia19 = String.Empty;

        public string SomenteOcorrencia19
        {
            get { return _SomenteOcorrencia19; }
            set { _SomenteOcorrencia19 = value; }
        }
        private string _Filler8 = String.Empty;

        public string Filler8
        {
            get { return _Filler8; }
            set { _Filler8 = value; }
        }
        private string _MotivoOcorrencia = String.Empty;

        public string MotivoOcorrencia
        {
            get { return _MotivoOcorrencia; }
            set { _MotivoOcorrencia = value; }
        }
        private string _DataPrevistaLancamentoContaCorrente = String.Empty;

        public string DataPrevistaLancamentoContaCorrente
        {
            get { return _DataPrevistaLancamentoContaCorrente; }
            set { _DataPrevistaLancamentoContaCorrente = value; }
        }
        private string _Filler9 = String.Empty;

        public string Filler9
        {
            get { return _Filler9; }
            set { _Filler9 = value; }
        }
        private string _NumeroSequencialRegistro = String.Empty;

        public string NumeroSequencialRegistro
        {
            get { return _NumeroSequencialRegistro; }
            set { _NumeroSequencialRegistro = value; }
        }
        #endregion

        public TRegistroEDI_Sicredi_Retorno()
        {
            /*
             * Aqui é que iremos informar as características de cada campo do arquivo
             * Na classe base, TCampoRegistroEDI, temos a propriedade CamposEDI, que é uma coleção de objetos
             * TCampoRegistroEDI.
             */
            #region TODOS os Campos
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, string.Empty, ' ')); //001-001
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 012, 0, string.Empty, ' ')); //002-013
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0014, 001, 0, string.Empty, ' ')); //014-014
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0015, 005, 0, string.Empty, ' ')); //015-019
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0020, 005, 0, string.Empty, ' ')); //020-025
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0025, 001, 0, string.Empty, ' ')); //025-025
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0026, 022, 0, string.Empty, ' ')); //026-047
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0048, 015, 0, string.Empty, ' ')); //048-062
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0063, 046, 0, string.Empty, ' ')); //063-108
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0109, 002, 0, string.Empty, ' ')); //109-110
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0111, 006, 0, string.Empty, ' ')); //111-116
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0117, 010, 0, string.Empty, ' ')); //117-126
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0127, 020, 0, string.Empty, ' ')); //127-146
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0147, 006, 0, string.Empty, ' ')); //147-152
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0153, 013, 0, string.Empty, ' ')); //153-165
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0166, 009, 0, string.Empty, ' ')); //166-174
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0175, 001, 0, string.Empty, ' ')); //175-175
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0176, 013, 0, string.Empty, ' ')); //176-188
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0189, 013, 0, string.Empty, ' ')); //189-201
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0202, 026, 0, string.Empty, ' ')); //202-227
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0228, 013, 0, string.Empty, ' ')); //228-240
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0241, 013, 0, string.Empty, ' ')); //241-253
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0254, 013, 0, string.Empty, ' ')); //254-266
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0267, 013, 0, string.Empty, ' ')); //267-279
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0280, 013, 0, string.Empty, ' ')); //280-292
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0293, 002, 0, string.Empty, ' ')); //293-294
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0295, 001, 0, string.Empty, ' ')); //295-295
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0296, 023, 0, string.Empty, ' ')); //296-318
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0319, 010, 0, string.Empty, ' ')); //318-328
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0329, 008, 0, string.Empty, ' ')); //329-336
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0337, 058, 0, string.Empty, ' ')); //337-394
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
            this._IdentificacaoRegDetalhe = (string)this._CamposEDI[00].ValorNatural;
            this._Filler1 = (string)this._CamposEDI[01].ValorNatural;
            this._TipoCobranca = (string)this._CamposEDI[02].ValorNatural;
            this._CodigoPagadorAgenciaBeneficiario = (string)this._CamposEDI[03].ValorNatural;
            this._CodigoPagadorJuntoAssociado = (string)this._CamposEDI[04].ValorNatural;
            this._BoletoDDA = (string)this._CamposEDI[05].ValorNatural;
            this._Filler2 = (string)this._CamposEDI[06].ValorNatural;
            this._NossoNumeroSicredi = (string)this._CamposEDI[07].ValorNatural;
            this._Filler3 = (string)this._CamposEDI[08].ValorNatural;
            this._Ocorrencia = (string)this._CamposEDI[09].ValorNatural;
            this._DataOcorrencia = (string)this._CamposEDI[10].ValorNatural;
            this._SeuNumero = (string)this._CamposEDI[11].ValorNatural;
            this._Filler4 = (string)this._CamposEDI[12].ValorNatural;
            this._DataVencimento = (string)this._CamposEDI[13].ValorNatural;
            this._ValorTitulo = (string)this._CamposEDI[14].ValorNatural;
            this._Filler5 = (string)this._CamposEDI[15].ValorNatural;
            this._EspecieDocumento = (string)this._CamposEDI[16].ValorNatural;
            this._DespesasCobranca = (string)this._CamposEDI[17].ValorNatural;
            this._DespesasCustasProtesto = (string)this._CamposEDI[18].ValorNatural;
            this._Filler6 = (string)this._CamposEDI[19].ValorNatural;
            this._AbatimentoConcedido = (string)this._CamposEDI[20].ValorNatural;
            this._DescontoConcedido = (string)this._CamposEDI[21].ValorNatural;
            this._ValorEfetivamentePago = (string)this._CamposEDI[22].ValorNatural;
            this._JurosMora = (string)this._CamposEDI[23].ValorNatural;
            this._Multa = (string)this._CamposEDI[24].ValorNatural;
            this._Filler7 = (string)this._CamposEDI[25].ValorNatural;
            this._SomenteOcorrencia19 = (string)this._CamposEDI[26].ValorNatural;
            this._Filler8 = (string)this._CamposEDI[27].ValorNatural;
            this._MotivoOcorrencia = (string)this._CamposEDI[28].ValorNatural;
            this._DataPrevistaLancamentoContaCorrente = (string)this._CamposEDI[29].ValorNatural;
            this._Filler9 = (string)this._CamposEDI[30].ValorNatural;
            this._NumeroSequencialRegistro = (string)this._CamposEDI[31].ValorNatural;
            //
		}
	}

	/// <summary>
	/// Classe que irá representar o arquivo EDI em si
	/// </summary>
    public class TArquivoSicrediRetorno_EDI : TEDIFile
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
            Lines.Add(new TRegistroEDI_Sicredi_Retorno()); //Adiciono a linha a ser decodificada
			Lines[Lines.Count - 1].LinhaRegistro = Line; //Atribuo a linha que vem do arquivo
            Lines[Lines.Count - 1].DecodificarLinha(); //Finalmente, a separação das substrings na linha do arquivo.
		}
	}
}