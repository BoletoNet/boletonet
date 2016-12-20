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
    public class TRegistroEDI_Banco_Nordeste_Retorno : TRegistroEDI
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

        private string _NossoNumeroDV;
        public string NossoNumeroDV
        {
            get { return _NossoNumeroDV; }
            set { _NossoNumeroDV = value; }
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
        private string _DataOcorrencia = String.Empty;

        public string DataOcorrencia
        {
            get { return _DataOcorrencia; }
            set { _DataOcorrencia = value; }
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


        public TRegistroEDI_Banco_Nordeste_Retorno()
        {
            /*
             * Aqui é que iremos informar as características de cada campo do arquivo
             * Na classe base, TCampoRegistroEDI, temos a propriedade CamposEDI, que é uma coleção de objetos
             * TCampoRegistroEDI.
             */
            #region TODOS os Campos
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, string.Empty, ' ')); //001-001 Preenchido com o número “1”
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 002, 0, string.Empty, ' ')); //002-003 Preenchido com o tipo de inscrição do Cedente
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0004, 014, 0, string.Empty, ' ')); //004-017 Preenchido com o CGC ou CPF
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0018, 004, 0, string.Empty, ' ')); //018-021 Preenchido com o código da Agência na qual o Cliente opera
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0022, 002, 0, string.Empty, ' ')); //022-023 Zeros
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0024, 007, 0, string.Empty, ' ')); //024-030 Preenchido com o número da Conta Corrente do Cliente cadastrado na cobrança como cedente
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0031, 001, 0, string.Empty, ' ')); //031-031 Preenchido com o Dígito da Conta do Cliente
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0032, 006, 0, string.Empty, ' ')); //032-037 Brancos
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0038, 025, 0, string.Empty, ' ')); //038-062 Número Controle
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0063, 007, 0, string.Empty, ' ')); //063-069 Nosso Número
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0070, 001, 0, string.Empty, ' ')); //070-070 Dígito do Nosso Número
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0071, 010, 0, string.Empty, ' ')); //071-080 Número do Contrato
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0081, 027, 0, string.Empty, ' ')); //081-107 Brancos 
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0108, 001, 0, string.Empty, ' ')); //108-108 Carteira
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0109, 002, 0, string.Empty, ' ')); //109-010 Código de Serviço. 
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0111, 006, 0, string.Empty, ' ')); //111-116 Data de Ocorrência
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0117, 010, 0, string.Empty, ' ')); //117-126 Seu Número
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0127, 007, 0, string.Empty, ' ')); //127-133 Confirmação do Nosso Número
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0134, 001, 0, string.Empty, ' ')); //134-134 Confirmação do Dígito do Nosso Número
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0135, 012, 0, string.Empty, ' ')); //135-146 Brancos
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0147, 006, 0, string.Empty, ' ')); //147-152 Data de Vencimento
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0153, 013, 0, string.Empty, ' ')); //153-165 Valor Nominal do Título
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0166, 003, 0, string.Empty, ' ')); //166-168 Número do Banco.
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0169, 004, 0, string.Empty, ' ')); //169-172 Agência Cobradora. 
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0173, 001, 0, string.Empty, ' ')); //173-173 Branco
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0174, 002, 0, string.Empty, ' ')); //174-175 Espécie
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0176, 013, 0, string.Empty, ' ')); //176-188 Tarifa de Cobrança
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0189, 013, 0, string.Empty, ' ')); //189-201 Outras 
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0202, 011, 0, string.Empty, ' ')); //202-214 Juros
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0215, 013, 0, string.Empty, ' ')); //215-227 IOC de Operações de Seguro
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0228, 013, 0, string.Empty, ' ')); //228-240 Abatimento Concedido
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0241, 013, 0, string.Empty, ' ')); //241-253 Desconto Concedido
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0254, 013, 0, string.Empty, ' ')); //254-266 Valor Recebido
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0267, 013, 0, string.Empty, ' ')); //267-279 Juros de Mora
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0280, 115, 0, string.Empty, ' ')); //280-394 Tabela de Erros
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0395, 006, 0, string.Empty, ' ')); //395-400 Sequencial do Registro
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
            this._PrefixoAgencia = (string)this._CamposEDI[3].ValorNatural;
            this._ContaCorrente = (string)this._CamposEDI[5].ValorNatural;
            this._DVContaCorrente = (string)this._CamposEDI[6].ValorNatural;
            this._NumeroControleParticipante = (string)this._CamposEDI[8].ValorNatural;
            this._NossoNumero = (string)this._CamposEDI[9].ValorNatural;
            this._NossoNumeroDV = (string)this._CamposEDI[10].ValorNatural;
            this._Carteira = (string)this._CamposEDI[13].ValorNatural;

            this._Comando = (string)this._CamposEDI[14].ValorNatural;            
            this._DataOcorrencia = (string)this._CamposEDI[15].ValorNatural;            
            this._NumeroTituloCedente = (string)this._CamposEDI[16].ValorNatural;
            this._DataVencimento = (string)this._CamposEDI[20].ValorNatural;
            this._ValorTitulo = (string)this._CamposEDI[21].ValorNatural;
            this._EspecieTitulo = (string)this._CamposEDI[24].ValorNatural;
            this._ValorTarifa = (string)this._CamposEDI[26].ValorNatural;
            this._OutrasDespesas = (string)this._CamposEDI[27].ValorNatural;
            this._JurosDesconto = (string)this._CamposEDI[28].ValorNatural;
            this._ValorAbatimento = (string)this._CamposEDI[30].ValorNatural;
            this._DescontoConcedido = (string)this._CamposEDI[31].ValorNatural;
            this._ValorRecebido = (string)this._CamposEDI[32].ValorNatural;
            this._JurosMora = (string)this._CamposEDI[33].ValorNatural;
            this._NumeroSequenciaRegistro = (string)this._CamposEDI[35].ValorNatural;

            /*
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
            this._NumeroSequenciaRegistro = (string)this._CamposEDI[56].ValorNatural;*/
            //
		}
	}

	/// <summary>
	/// Classe que irá representar o arquivo EDI em si
	/// </summary>
    public class TArquivoBanco_Nordeste_Retorno_EDI : TEDIFile
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
            Lines.Add(new TRegistroEDI_Banco_Nordeste_Retorno()); //Adiciono a linha a ser decodificada
			Lines[Lines.Count - 1].LinhaRegistro = Line; //Atribuo a linha que vem do arquivo
            Lines[Lines.Count - 1].DecodificarLinha(); //Finalmente, a separação das substrings na linha do arquivo.
		}
	}
	

}
