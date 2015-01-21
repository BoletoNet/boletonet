using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet.EDI.Banco
{
    /// <summary>
	/// Classe de Integração Banrisul
    /// CBR643
    /// Convênio 6 posições -- Único com Identificação do Registro Detalhe = 1
	/// </summary>
    public class TRegistroEDI_BancoBrasil_Retorno : TRegistroEDI
    {

		#region Atributos e Propriedades
        private string _Identificacao = String.Empty;

        public string Identificacao
        {
            get { return _Identificacao; }
            set { _Identificacao = value; }
        }
        private string _Zeros1 = String.Empty;

        public string Zeros1
        {
            get { return _Zeros1; }
            set { _Zeros1 = value; }
        }
        private string _Zeros2 = String.Empty;

        public string Zeros2
        {
            get { return _Zeros2; }
            set { _Zeros2 = value; }
        }
        private string _PrefixoAgencia = String.Empty;

        public string PrefixoAgencia
        {
            get { return _PrefixoAgencia; }
            set { _PrefixoAgencia = value; }
        }
        private string _DVPrefixoAgencia = String.Empty;

        public string DVPrefixoAgencia
        {
            get { return _DVPrefixoAgencia; }
            set { _DVPrefixoAgencia = value; }
        }
        private string _ContaCorrente = String.Empty;

        public string ContaCorrente
        {
            get { return _ContaCorrente; }
            set { _ContaCorrente = value; }
        }
        private string _DVContaCorrente = String.Empty;

        public string DVContaCorrente
        {
            get { return _DVContaCorrente; }
            set { _DVContaCorrente = value; }
        }
        private string _NumeroConvenioCobranca = String.Empty;

        public string NumeroConvenioCobranca
        {
            get { return _NumeroConvenioCobranca; }
            set { _NumeroConvenioCobranca = value; }
        }
        private string _NumeroControleParticipante = String.Empty;

        public string NumeroControleParticipante
        {
            get { return _NumeroControleParticipante; }
            set { _NumeroControleParticipante = value; }
        }
        private string _NossoNumero = String.Empty;

        public string NossoNumero
        {
            get { return _NossoNumero; }
            set { _NossoNumero = value; }
        }
        private string _TipoCobranca = String.Empty;

        public string TipoCobranca
        {
            get { return _TipoCobranca; }
            set { _TipoCobranca = value; }
        }
        private string _TipoCobrancaEspecifico = String.Empty;

        public string TipoCobrancaEspecifico
        {
            get { return _TipoCobrancaEspecifico; }
            set { _TipoCobrancaEspecifico = value; }
        }
        private string _DiasCalculo = String.Empty;

        public string DiasCalculo
        {
            get { return _DiasCalculo; }
            set { _DiasCalculo = value; }
        }
        private string _NaturezaRecebimento = String.Empty;

        public string NaturezaRecebimento
        {
            get { return _NaturezaRecebimento; }
            set { _NaturezaRecebimento = value; }
        }
        private string _PrefixoTitulo = String.Empty;

        public string PrefixoTitulo
        {
            get { return _PrefixoTitulo; }
            set { _PrefixoTitulo = value; }
        }
        private string _VariacaoCarteira = String.Empty;

        public string VariacaoCarteira
        {
            get { return _VariacaoCarteira; }
            set { _VariacaoCarteira = value; }
        }
        private string _ContaCaucao = String.Empty;

        public string ContaCaucao
        {
            get { return _ContaCaucao; }
            set { _ContaCaucao = value; }
        }
        private string _TaxaDesconto = String.Empty;

        public string TaxaDesconto
        {
            get { return _TaxaDesconto; }
            set { _TaxaDesconto = value; }
        }
        private string _TaxaIOF = String.Empty;

        public string TaxaIOF
        {
            get { return _TaxaIOF; }
            set { _TaxaIOF = value; }
        }
        private string _Brancos1 = String.Empty;

        public string Brancos1
        {
            get { return _Brancos1; }
            set { _Brancos1 = value; }
        }
        private string _Carteira = String.Empty;

        public string Carteira
        {
            get { return _Carteira; }
            set { _Carteira = value; }
        }
        private string _Comando = String.Empty;

        public string Comando
        {
            get { return _Comando; }
            set { _Comando = value; }
        }
        private string _DataLiquidacao = String.Empty;

        public string DataLiquidacao
        {
            get { return _DataLiquidacao; }
            set { _DataLiquidacao = value; }
        }
        private string _NumeroTituloCedente = String.Empty;

        public string NumeroTituloCedente
        {
            get { return _NumeroTituloCedente; }
            set { _NumeroTituloCedente = value; }
        }
        private string _Brancos2 = String.Empty;

        public string Brancos2
        {
            get { return _Brancos2; }
            set { _Brancos2 = value; }
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
        private string _CodigoBancoRecebedor = String.Empty;

        public string CodigoBancoRecebedor
        {
            get { return _CodigoBancoRecebedor; }
            set { _CodigoBancoRecebedor = value; }
        }
        private string _PrefixoAgenciaRecebedora = String.Empty;

        public string PrefixoAgenciaRecebedora
        {
            get { return _PrefixoAgenciaRecebedora; }
            set { _PrefixoAgenciaRecebedora = value; }
        }
        private string _DVPrefixoRecebedora = String.Empty;

        public string DVPrefixoRecebedora
        {
            get { return _DVPrefixoRecebedora; }
            set { _DVPrefixoRecebedora = value; }
        }
        private string _EspecieTitulo = String.Empty;

        public string EspecieTitulo
        {
            get { return _EspecieTitulo; }
            set { _EspecieTitulo = value; }
        }
        private string _DataCredito = String.Empty;

        public string DataCredito
        {
            get { return _DataCredito; }
            set { _DataCredito = value; }
        }
        private string _ValorTarifa = String.Empty;

        public string ValorTarifa
        {
            get { return _ValorTarifa; }
            set { _ValorTarifa = value; }
        }
        private string _OutrasDespesas = String.Empty;

        public string OutrasDespesas
        {
            get { return _OutrasDespesas; }
            set { _OutrasDespesas = value; }
        }
        private string _JurosDesconto = String.Empty;

        public string JurosDesconto
        {
            get { return _JurosDesconto; }
            set { _JurosDesconto = value; }
        }
        private string _IOFDesconto = String.Empty;

        public string IOFDesconto
        {
            get { return _IOFDesconto; }
            set { _IOFDesconto = value; }
        }
        private string _ValorAbatimento = String.Empty;

        public string ValorAbatimento
        {
            get { return _ValorAbatimento; }
            set { _ValorAbatimento = value; }
        }
        private string _DescontoConcedido = String.Empty;

        public string DescontoConcedido
        {
            get { return _DescontoConcedido; }
            set { _DescontoConcedido = value; }
        }
        private string _ValorRecebido = String.Empty;

        public string ValorRecebido
        {
            get { return _ValorRecebido; }
            set { _ValorRecebido = value; }
        }
        private string _JurosMora = String.Empty;

        public string JurosMora
        {
            get { return _JurosMora; }
            set { _JurosMora = value; }
        }
        private string _OutrosRecebimentos = String.Empty;

        public string OutrosRecebimentos
        {
            get { return _OutrosRecebimentos; }
            set { _OutrosRecebimentos = value; }
        }
        private string _AbatimentoNaoAproveitado = String.Empty;

        public string AbatimentoNaoAproveitado
        {
            get { return _AbatimentoNaoAproveitado; }
            set { _AbatimentoNaoAproveitado = value; }
        }
        private string _ValorLancamento = String.Empty;

        public string ValorLancamento
        {
            get { return _ValorLancamento; }
            set { _ValorLancamento = value; }
        }
        private string _IndicativoDebitoCredito = String.Empty;

        public string IndicativoDebitoCredito
        {
            get { return _IndicativoDebitoCredito; }
            set { _IndicativoDebitoCredito = value; }
        }
        private string _IndicadorValor = String.Empty;

        public string IndicadorValor
        {
            get { return _IndicadorValor; }
            set { _IndicadorValor = value; }
        }
        private string _ValorAjuste = String.Empty;

        public string ValorAjuste
        {
            get { return _ValorAjuste; }
            set { _ValorAjuste = value; }
        }
        private string _Brancos3 = String.Empty;

        public string Brancos3
        {
            get { return _Brancos3; }
            set { _Brancos3 = value; }
        }
        private string _Brancos4 = String.Empty;

        public string Brancos4
        {
            get { return _Brancos4; }
            set { _Brancos4 = value; }
        }
        private string _Zeros3 = String.Empty;

        public string Zeros3
        {
            get { return _Zeros3; }
            set { _Zeros3 = value; }
        }
        private string _Zeros4 = String.Empty;

        public string Zeros4
        {
            get { return _Zeros4; }
            set { _Zeros4 = value; }
        }
        private string _Zeros5 = String.Empty;

        public string Zeros5
        {
            get { return _Zeros5; }
            set { _Zeros5 = value; }
        }
        private string _Zeros6 = String.Empty;

        public string Zeros6
        {
            get { return _Zeros6; }
            set { _Zeros6 = value; }
        }
        private string _Zeros7 = String.Empty;

        public string Zeros7
        {
            get { return _Zeros7; }
            set { _Zeros7 = value; }
        }
        private string _Zeros8 = String.Empty;

        public string Zeros8
        {
            get { return _Zeros8; }
            set { _Zeros8 = value; }
        }
        private string _Brancos5 = String.Empty;

        public string Brancos5
        {
            get { return _Brancos5; }
            set { _Brancos5 = value; }
        }
        private string _CanalPagamento = String.Empty;

        public string CanalPagamento
        {
            get { return _CanalPagamento; }
            set { _CanalPagamento = value; }
        }
        private string _NumeroSequenciaRegistro = String.Empty;

        public string NumeroSequenciaRegistro
        {
            get { return _NumeroSequenciaRegistro; }
            set { _NumeroSequenciaRegistro = value; }
        }
        #endregion


        public TRegistroEDI_BancoBrasil_Retorno()
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
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0022, 001, 0, string.Empty, ' ')); //022-022
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0023, 008, 0, string.Empty, ' ')); //023-030
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0031, 001, 0, string.Empty, ' ')); //031-031
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0032, 007, 0, string.Empty, ' ')); //032-038
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0039, 025, 0, string.Empty, ' ')); //039-063
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0064, 017, 0, string.Empty, ' ')); //064-080
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0081, 001, 0, string.Empty, ' ')); //081-081
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0082, 001, 0, string.Empty, ' ')); //082-082
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0083, 004, 0, string.Empty, ' ')); //083-086
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0087, 002, 0, string.Empty, ' ')); //087-088
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0089, 003, 0, string.Empty, ' ')); //089-091
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0092, 003, 0, string.Empty, ' ')); //092-094
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0095, 001, 0, string.Empty, ' ')); //095-095
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0096, 005, 0, string.Empty, ' ')); //096-100
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0101, 004, 0, string.Empty, ' ')); //101-105
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0106, 001, 0, string.Empty, ' ')); //106-106
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0107, 002, 0, string.Empty, ' ')); //107-108
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0109, 002, 0, string.Empty, ' ')); //109-110
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0111, 006, 0, string.Empty, ' ')); //111-116
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0117, 010, 0, string.Empty, ' ')); //117-126
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0127, 020, 0, string.Empty, ' ')); //127-146
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0147, 006, 0, string.Empty, ' ')); //147-152
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0153, 013, 0, string.Empty, ' ')); //153-165
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0166, 003, 0, string.Empty, ' ')); //166-168
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0169, 004, 0, string.Empty, ' ')); //169-172
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0173, 001, 0, string.Empty, ' ')); //173-173
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0174, 002, 0, string.Empty, ' ')); //174-175
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0176, 006, 0, string.Empty, ' ')); //176-181
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0182, 007, 0, string.Empty, ' ')); //182-188
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0189, 013, 0, string.Empty, ' ')); //189-201
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0202, 013, 0, string.Empty, ' ')); //202-214
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0215, 013, 0, string.Empty, ' ')); //215-227
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0228, 013, 0, string.Empty, ' ')); //228-240
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0241, 013, 0, string.Empty, ' ')); //241-253
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0254, 013, 0, string.Empty, ' ')); //254-266
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0267, 013, 0, string.Empty, ' ')); //267-279
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0280, 013, 0, string.Empty, ' ')); //280-292
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0293, 013, 0, string.Empty, ' ')); //293-305
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0306, 013, 0, string.Empty, ' ')); //306-318
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0319, 001, 0, string.Empty, ' ')); //319-319
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0320, 001, 0, string.Empty, ' ')); //320-320
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0321, 012, 0, string.Empty, ' ')); //321-332
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0333, 001, 0, string.Empty, ' ')); //333-333
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0334, 009, 0, string.Empty, ' ')); //334-342
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0343, 007, 0, string.Empty, ' ')); //343-349
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0350, 009, 0, string.Empty, ' ')); //350-358
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0359, 007, 0, string.Empty, ' ')); //359-365
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0366, 009, 0, string.Empty, ' ')); //366-374
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0375, 007, 0, string.Empty, ' ')); //375-381
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0382, 009, 0, string.Empty, ' ')); //382-390
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0391, 002, 0, string.Empty, ' ')); //391-392
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
            this._Identificacao = (string)this._CamposEDI[0].ValorNatural;
            this._Zeros1 = (string)this._CamposEDI[1].ValorNatural;
            this._Zeros2 = (string)this._CamposEDI[2].ValorNatural;
            this._PrefixoAgencia = (string)this._CamposEDI[3].ValorNatural;
            this._DVPrefixoAgencia = (string)this._CamposEDI[4].ValorNatural;
            this._ContaCorrente = (string)this._CamposEDI[5].ValorNatural;
            this._DVContaCorrente = (string)this._CamposEDI[6].ValorNatural;
            this._NumeroConvenioCobranca = (string)this._CamposEDI[7].ValorNatural;
            this._NumeroControleParticipante = (string)this._CamposEDI[8].ValorNatural;
            this._NossoNumero = (string)this._CamposEDI[9].ValorNatural;
            this._TipoCobranca = (string)this._CamposEDI[10].ValorNatural;
            this._TipoCobrancaEspecifico = (string)this._CamposEDI[11].ValorNatural;
            this._DiasCalculo = (string)this._CamposEDI[12].ValorNatural;
            this._NaturezaRecebimento = (string)this._CamposEDI[13].ValorNatural;
            this._PrefixoTitulo = (string)this._CamposEDI[14].ValorNatural;
            this._VariacaoCarteira = (string)this._CamposEDI[15].ValorNatural;
            this._ContaCaucao = (string)this._CamposEDI[16].ValorNatural;
            this._TaxaDesconto = (string)this._CamposEDI[17].ValorNatural;
            this._TaxaIOF = (string)this._CamposEDI[18].ValorNatural;
            this._Brancos1 = (string)this._CamposEDI[19].ValorNatural;
            this._Carteira = (string)this._CamposEDI[20].ValorNatural;
            this._Comando = (string)this._CamposEDI[21].ValorNatural;
            this._DataLiquidacao = (string)this._CamposEDI[22].ValorNatural;
            this._NumeroTituloCedente = (string)this._CamposEDI[23].ValorNatural;
            this._Brancos2 = (string)this._CamposEDI[24].ValorNatural;
            this._DataVencimento = (string)this._CamposEDI[25].ValorNatural;
            this._ValorTitulo = (string)this._CamposEDI[26].ValorNatural;
            this._CodigoBancoRecebedor = (string)this._CamposEDI[27].ValorNatural;
            this._PrefixoAgenciaRecebedora = (string)this._CamposEDI[28].ValorNatural;
            this._DVPrefixoRecebedora = (string)this._CamposEDI[29].ValorNatural;
            this._EspecieTitulo = (string)this._CamposEDI[30].ValorNatural;
            this._DataCredito = (string)this._CamposEDI[31].ValorNatural;
            this._ValorTarifa = (string)this._CamposEDI[32].ValorNatural;
            this._OutrasDespesas = (string)this._CamposEDI[33].ValorNatural;
            this._JurosDesconto = (string)this._CamposEDI[34].ValorNatural;
            this._IOFDesconto = (string)this._CamposEDI[35].ValorNatural;
            this._ValorAbatimento = (string)this._CamposEDI[36].ValorNatural;
            this._DescontoConcedido = (string)this._CamposEDI[37].ValorNatural;
            this._ValorRecebido = (string)this._CamposEDI[38].ValorNatural;
            this._JurosMora = (string)this._CamposEDI[39].ValorNatural;
            this._OutrosRecebimentos = (string)this._CamposEDI[40].ValorNatural;
            this._AbatimentoNaoAproveitado = (string)this._CamposEDI[41].ValorNatural;
            this._ValorLancamento = (string)this._CamposEDI[42].ValorNatural;
            this._IndicativoDebitoCredito = (string)this._CamposEDI[43].ValorNatural;
            this._IndicadorValor = (string)this._CamposEDI[44].ValorNatural;
            this._ValorAjuste = (string)this._CamposEDI[45].ValorNatural;
            this._Brancos3 = (string)this._CamposEDI[46].ValorNatural;
            this._Brancos4 = (string)this._CamposEDI[47].ValorNatural;
            this._Zeros3 = (string)this._CamposEDI[48].ValorNatural;
            this._Zeros4 = (string)this._CamposEDI[49].ValorNatural;
            this._Zeros5 = (string)this._CamposEDI[50].ValorNatural;
            this._Zeros6 = (string)this._CamposEDI[51].ValorNatural;
            this._Zeros7 = (string)this._CamposEDI[52].ValorNatural;
            this._Zeros8 = (string)this._CamposEDI[53].ValorNatural;
            this._Brancos5 = (string)this._CamposEDI[54].ValorNatural;
            this._CanalPagamento = (string)this._CamposEDI[55].ValorNatural;
            this._NumeroSequenciaRegistro = (string)this._CamposEDI[56].ValorNatural;
            //
		}
	}

	/// <summary>
	/// Classe que irá representar o arquivo EDI em si
	/// </summary>
    public class TArquivoBancoBrasilRetorno_EDI : TEDIFile
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
            Lines.Add(new TRegistroEDI_BancoBrasil_Retorno()); //Adiciono a linha a ser decodificada
			Lines[Lines.Count - 1].LinhaRegistro = Line; //Atribuo a linha que vem do arquivo
            Lines[Lines.Count - 1].DecodificarLinha(); //Finalmente, a separação das substrings na linha do arquivo.
		}
	}
	

}
