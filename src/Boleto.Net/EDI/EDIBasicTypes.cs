using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    //Classes básicas para manipulação de registros para geração/interpretação de EDI

    /// <summary>
    /// Classe para ordenação pela propriedade Posição no Registro EDI
    /// </summary>
    internal class OrdenacaoPorPosEDI : IComparer<TCampoRegistroEDI>
    {
        public int Compare(TCampoRegistroEDI x, TCampoRegistroEDI y)
        {
            return x.OrdemNoRegistroEDI.CompareTo(y.OrdemNoRegistroEDI);
        }
    }
    
    /// <summary>
    /// Representa cada tipo de dado possível em um arquivo EDI.
    /// </summary>
    public enum TTiposDadoEDI
    { 
        /// <summary>
        /// Representa um campo alfanumérico, alinhado à esquerda e com brancos à direita. A propriedade ValorNatural é do tipo String
        /// </summary>
        ediAlphaAliEsquerda_____,
        /// <summary>
        /// Representa um campo alfanumérico, alinhado à direita e com brancos à esquerda. A propriedade ValorNatural é do tipo String
        /// </summary>
        ediAlphaAliDireita______,
        /// <summary>
        /// Representa um campo numérico inteiro alinhado à direita com zeros à esquerda. A propriedade ValorNatural é do tipo Int ou derivados
        /// </summary>
        ediInteiro______________,
        /// <summary>
        /// Representa um campo numérico com decimais, sem o separador de decimal. A propriedade ValorNatural é do tipo Double
        /// </summary>
        ediNumericoSemSeparador_,
        /// <summary>
        /// Representa um campo numérico com decimais, com o caracter ponto (.) como separador decimal,
        /// alinhado à direita com zeros à esquerda. A propriedade ValorNatural é do tipo Double
        /// </summary>
        ediNumericoComPonto_____,
        /// <summary>
        /// Representa um campo numérico com decimais, com o caracter vírgula (,) como separador decimal,
        /// alinhado à direita com zeros à esquerda. A propriedade ValorNatural é do tipo Double
        /// </summary>
        ediNumericoComVirgula___,
        /// <summary>
        /// Representa um campo de data no formato ddm/mm/aaaa. A propriedade ValorNatural é do tipo DateTime
        /// </summary>
        ediDataDDMMAAAA_________,
        /// <summary>
        /// Representa um campo de data no formato aaaa/mm/dd. A propriedade ValorNatural é do tipo DateTime
        /// </summary>
        ediDataAAAAMMDD_________,
        /// <summary>
        /// Representa um campo de data no formato dd/mm. A propriedade ValorNatural é do tipo DateTime, com o ano igual a 1900
        /// </summary>
        ediDataDDMM_____________,
        /// <summary>
        /// Representa um campo de data no formato mm/aaaa. A propriedade ValorNatural é do tipo DateTime, com o dia igual a 01
        /// </summary>
        ediDataMMAAAA___________,
        /// <summary>
        /// Representa um campo de data no formato mm/dd. A propriedade ValorNatural é do tipo DateTime com o ano igual a 1900
        /// </summary>
        ediDataMMDD_____________,
        /// <summary>
        /// Representa um campo de hora no formato HH:MM. A propriedade ValorNatural é do tipo DateTime, com a data igual a 01/01/1900
        /// </summary>
        ediHoraHHMM_____________,
        /// <summary>
        /// Representa um campo de hora no formato HH:MM:SS. A propriedade ValorNatural é do tipo DateTime, com a data igual a 01/01/1900
        /// </summary>
        ediHoraHHMMSS___________,
        /// <summary>
        /// Representa um campo de data no formato DD/MM/AAAA. A propriedade ValorNatural é do tipo DateTime.
        /// </summary>
        ediDataDDMMAA___________,
        /// <summary>
        /// Representa um campo de data no formato DD/MM/AAAA, porém colocando zeros no lugar de espaços no ValorFormatado. A propriedade
        /// ValorNatural é do tipo DateTime, e este deve ser nulo caso queira que a data seja zero.
        /// </summary>
        ediDataDDMMAAAAWithZeros,
        /// <summary>
        /// Representa um campo de data no formato AAAA/MM/DD, porém colocando zeros no lugar de espaços no ValorFormatado. A propriedade
        /// ValorNatural é do tipo DateTime, e este deve ser nulo caso queira que a data seja zero.
        /// </summary>
        ediDataAAAAMMDDWithZeros
    }


    public class TCampoRegistroEDI
    {
        #region Variáveis Privadas
        private string _DescricaoCampo;
        private TTiposDadoEDI _TipoCampo;
        private int _TamanhoCampo;
        private int _QtdDecimais;
        private object _ValorNatural;
        private string _ValorFormatado;
        private int _OrdemNoRegistroEDI;
        private string _SeparadorDatas;
        private string _SeparadorHora;
        private int _PosicaoInicial;
        private int _PosicaoFinal;
        private char _Preenchimento = ' ';
        #endregion

        #region Propriedades
        /// <summary>
        /// Descrição do campo no registro EDI (meramente descritivo)
        /// </summary>
        public string DescricaoCampo
        {
            get { return _DescricaoCampo; }
            set { _DescricaoCampo = value; }
        }

        /// <summary>
        /// Tipo de dado de ORIGEM das informações do campo EDI.
        /// </summary>
        public TTiposDadoEDI TipoCampo
        {
            get { return _TipoCampo; }
            set { _TipoCampo = value; }
        }

        /// <summary>
        /// Tamanho em caracteres do campo no arquivo EDI (DESTINO)
        /// </summary>
        public int TamanhoCampo
        {
            get { return _TamanhoCampo; }
            set { _TamanhoCampo = value; }
        }

        /// <summary>
        /// Quantidade de casas decimais do campo, caso ele seja do tipo numérico sem decimais. Caso
        /// não se aplique ao tipo de dado, o valor da propriedade será ignorado nas funções de formatação.
        /// </summary>
        public int QtdDecimais
        {
            get { return _QtdDecimais; }
            set { _QtdDecimais = value; }
        }

        /// <summary>
        /// Valor de ORIGEM do campo, sem formatação, no tipo de dado adequado ao campo. O valor deve ser atribuido
        /// com o tipo de dado adequado ao seu proposto, por exemplo, Double para representar valor, DateTime para
        /// representar datas e/ou horas, etc.
        /// </summary>
        public object ValorNatural
        {
            get { return _ValorNatural; }
            set { _ValorNatural = value; }
        }

        /// <summary>
        /// Valor formatado do campo, pronto para ser utilizado no arquivo EDI. A formatação será de acordo
        /// com a especificada na propriedade TipoCampo, com numéricos alinhados à direita e zeros à esquerda
        /// e campos alfanuméricos alinhados à esquerda e com brancos à direita.
        /// Também pode receber o valor vindo do arquivo EDI, para ser decodificado e o resultado da decodificação na propriedade
        /// ValorNatural
        /// </summary>
        public string ValorFormatado
        {
            get { return _ValorFormatado; }
            set { _ValorFormatado = value; }
        }

        /// <summary>
        /// Número de ordem do campo no registro EDI
        /// </summary>
        public int OrdemNoRegistroEDI
        {
            get { return _OrdemNoRegistroEDI; }
            set { _OrdemNoRegistroEDI = value; }
        }

        /// <summary>
        /// Caractere separador dos elementos de campos com o tipo DATA. Colocar null caso esta propriedade
        /// não se aplique ao tipo de dado.
        /// </summary>
        public string SeparadorDatas
        {
            get { return _SeparadorDatas; }
            set { _SeparadorDatas = value; }
        }

        /// <summary>
        /// Caractere separador dos elementos de campos com o tipo HORA. Colocar null caso esta propriedade
        /// não se aplique ao tipo de dado.
        /// </summary>
        public string SeparadorHora
        {
            get { return _SeparadorHora; }
            set { _SeparadorHora = value; }
        }

        /// <summary>
        /// Posição do caracter inicial do campo no arquivo EDI
        /// </summary>
        public int PosicaoInicial
        {
            get { return _PosicaoInicial; }
            set { _PosicaoInicial = value; }
        }

        public int PosicaoFinal
        {
            get { return _PosicaoFinal; }
            set { _PosicaoFinal = value; }
        }
        /// <summary>
        /// Caractere de Preenchimento do campo da posição inicial até a posição final
        /// </summary>
        public char Preenchimento
        {
            get { return _Preenchimento; }
            set { _Preenchimento = value; }
        }
        #endregion

        #region Construtores
        /// <summary>
        /// Cria um objeto TCampoRegistroEDI
        /// </summary>
        public TCampoRegistroEDI()
        { 
        }

        /// <summary>
        /// Cria um objeto do tipo TCampoRegistroEDI inicializando as propriedades básicas.
        /// </summary>
        /// <param name="pTipoCampo">Tipo de dado de origem dos dados</param>
        /// <param name="pPosicaoInicial">Posição Inicial do Campo no Arquivo</param>
        /// <param name="pTamanho">Tamanho em caracteres do campo (destino)</param>
        /// <param name="pDecimais">Quantidade de decimais do campo (destino)</param>
        /// <param name="pValor">Valor do campo (Origem), no tipo de dado adequado ao propósito do campo</param>
        /// <param name="pPreenchimento">Caractere de Preenchimento do campo caso o valor não ocupe todo o tamanho</param>
        /// <param name="pSeparadorHora">Separador de hora padrão; null para sem separador</param>
        /// <param name="pSeparadorData">Separador de data padrão; null para sem separador</param>
        public TCampoRegistroEDI(TTiposDadoEDI pTipoCampo, int pPosicaoInicial, int pTamanho, int pDecimais, object pValor, char pPreenchimento, string pSeparadorHora, string pSeparadorData)
        {
            this._TipoCampo = pTipoCampo;
            this._TamanhoCampo = pTamanho;
            this._QtdDecimais = pDecimais;
            this._ValorNatural = pValor;
            this._SeparadorHora = pSeparadorHora;
            this._SeparadorDatas = pSeparadorData;
            this._OrdemNoRegistroEDI = 0;
            this._DescricaoCampo = "";
            this._PosicaoInicial = pPosicaoInicial - 1; //Compensa a indexação com base em zero
            this._PosicaoFinal = pPosicaoInicial + this._TamanhoCampo;
            this._Preenchimento = pPreenchimento;
        }
        /// <summary>
        /// Cria um objeto do tipo TCampoRegistroEDI inicializando as propriedades básicas.
        /// </summary>
        /// <param name="pTipoCampo">Tipo de dado de origem dos dados</param>
        /// <param name="pPosicaoInicial">Posição Inicial do Campo no Arquivo</param>
        /// <param name="pTamanho">Tamanho em caracteres do campo (destino)</param>
        /// <param name="pDecimais">Quantidade de decimais do campo (destino)</param>
        /// <param name="pValor">Valor do campo (Origem), no tipo de dado adequado ao propósito do campo</param>
        /// <param name="pPreenchimento">Caractere de Preenchimento do campo caso o valor não ocupe todo o tamanho</param>
        public TCampoRegistroEDI(TTiposDadoEDI pTipoCampo, int pPosicaoInicial, int pTamanho, int pDecimais, object pValor, char pPreenchimento)
        {
            this._TipoCampo = pTipoCampo;
            this._TamanhoCampo = pTamanho;
            this._QtdDecimais = pDecimais;
            this._ValorNatural = pValor;
            this._SeparadorHora = null;
            this._SeparadorDatas = null;
            this._OrdemNoRegistroEDI = 0;
            this._DescricaoCampo = "";
            this._PosicaoInicial = pPosicaoInicial - 1; //Compensa a indexação com base em zero
            this._PosicaoFinal = pPosicaoInicial + this._TamanhoCampo;
            this._Preenchimento = pPreenchimento;
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Aplica formatação ao valor do campo em ValorNatural, colocando o resultado na propriedade ValorFormatado
        /// </summary>
        public void CodificarNaturalParaEDI()
        {
            switch (this._TipoCampo)
            {
                case TTiposDadoEDI.ediAlphaAliEsquerda_____:
                    {
                        if (this._ValorNatural != null)
                        {
                            if (this._ValorNatural.ToString().Trim().Length >= this._TamanhoCampo)
                                this._ValorFormatado = this._ValorNatural.ToString().Trim().Substring(0, this._TamanhoCampo);
                            else
                                this._ValorFormatado = this._ValorNatural.ToString().Trim().PadRight(this._TamanhoCampo, this._Preenchimento); //' '
                        }
                        else
                            this._ValorFormatado = string.Empty.PadRight(this._TamanhoCampo, this._Preenchimento); //' '
                        break;
                    }
                case TTiposDadoEDI.ediAlphaAliDireita______:
                    {
                        if (this._ValorNatural != null)
                        {
                            if (this._ValorNatural.ToString().Trim().Length >= this._TamanhoCampo)
                                this._ValorFormatado = this._ValorNatural.ToString().Trim().Substring(0, this._TamanhoCampo);
                            else
                                this._ValorFormatado = this._ValorNatural.ToString().Trim().PadLeft(this._TamanhoCampo, this._Preenchimento); //' '
                        }
                        else
                            this._ValorFormatado = string.Empty.PadLeft(this._TamanhoCampo, this._Preenchimento); //' '
                        break;
                    }
                case TTiposDadoEDI.ediInteiro______________:
                    {
                        this._ValorFormatado = this._ValorNatural.ToString().Trim().PadLeft(this._TamanhoCampo, this._Preenchimento); //'0'
                        break;
                    }
                case TTiposDadoEDI.ediNumericoSemSeparador_:
                    {
                        if (this._ValorNatural == null)
                        {
                            string aux = "";
                            this._ValorFormatado = aux.Trim().PadLeft(this._TamanhoCampo, ' ');//Se o Número for NULL, preenche com espaços em branco
                        }
                        else
                        {
                            string Formatacao = "{0:f" + this._QtdDecimais.ToString() + "}";
                            this._ValorFormatado = String.Format(Formatacao, this._ValorNatural).Replace(",", "").Replace(".", "").Trim().PadLeft(this._TamanhoCampo, this._Preenchimento); //'0'
                        }
                        break;
                    }
                case TTiposDadoEDI.ediNumericoComPonto_____:
                    {
                        string Formatacao = "{0:f" + this._QtdDecimais.ToString() + "}";
                        this._ValorFormatado = String.Format(Formatacao, this._ValorNatural).Replace(",", ".").Trim().PadLeft(this._TamanhoCampo, this._Preenchimento); //'0'
                        break;
                    }
                case TTiposDadoEDI.ediNumericoComVirgula___:
                    {
                        string Formatacao = "{0:f" + this._QtdDecimais.ToString() + "}";
                        this._ValorFormatado = String.Format(Formatacao, this._ValorNatural).Replace(".", ",").Trim().PadLeft(this._TamanhoCampo, this._Preenchimento); //'0'
                        break;
                    }
                case TTiposDadoEDI.ediDataAAAAMMDD_________:
                    {
                        if ( (DateTime)this._ValorNatural != DateTime.MinValue)
                        {
                            string sep = (this._SeparadorDatas == null ? "" : this._SeparadorDatas.ToString());
                            string Formatacao = "{0:yyyy" + sep + "MM" + sep + "dd}";
                            this._ValorFormatado = String.Format(Formatacao, this._ValorNatural);
                        }
                        else
                        {
                            this._ValorNatural = "";
                            goto case TTiposDadoEDI.ediAlphaAliEsquerda_____;
                        }
                        break;
                    }
                case TTiposDadoEDI.ediDataDDMM_____________:
                    {
                        if ((DateTime)this._ValorNatural != DateTime.MinValue)
                        {
                            string sep = (this._SeparadorDatas == null ? "" : this._SeparadorDatas.ToString());
                            string Formatacao = "{0:dd" + sep + "MM}";
                            this._ValorFormatado = String.Format(Formatacao, this._ValorNatural);
                        }
                        else
                        {
                            this._ValorNatural = "";
                            goto case TTiposDadoEDI.ediAlphaAliEsquerda_____;
                        }
                        break;
                    }
                case TTiposDadoEDI.ediDataDDMMAAAA_________:
                    {
                        if ((DateTime)this._ValorNatural != DateTime.MinValue)
                        {
                            string sep = (this._SeparadorDatas == null ? "" : this._SeparadorDatas.ToString());
                            string Formatacao = "{0:dd" + sep + "MM" + sep + "yyyy}";
                            this._ValorFormatado = String.Format(Formatacao, this._ValorNatural);
                        }
                        else
                        {
                            this._ValorNatural = "";
                            goto case TTiposDadoEDI.ediAlphaAliEsquerda_____;
                        }
                        break;
                    }
                case TTiposDadoEDI.ediDataDDMMAA___________:
                    {
                        if ((DateTime)this._ValorNatural != DateTime.MinValue)
                        {
                            string sep = (this._SeparadorDatas == null ? "" : this._SeparadorDatas.ToString());
                            string Formatacao = "{0:dd" + sep + "MM" + sep + "yy}";
                            this._ValorFormatado = String.Format(Formatacao, this._ValorNatural);
                        }
                        else
                        {
                            this._ValorNatural = "";
                            goto case TTiposDadoEDI.ediAlphaAliEsquerda_____;
                        }
                        break;
                    }
                case TTiposDadoEDI.ediDataMMAAAA___________:
                    {
                        if ((DateTime)this._ValorNatural != DateTime.MinValue)
                        {
                            string sep = (this._SeparadorDatas == null ? "" : this._SeparadorDatas.ToString());
                            string Formatacao = "{0:MM" + sep + "yyyy}";
                            this._ValorFormatado = String.Format(Formatacao, this._ValorNatural);
                        }
                        else
                        {
                            this._ValorNatural = "";
                            goto case TTiposDadoEDI.ediAlphaAliEsquerda_____;
                        }
                        break;
                    }
                case TTiposDadoEDI.ediDataMMDD_____________:
                    {
                        if ((DateTime)this._ValorNatural != DateTime.MinValue)
                        {
                            string sep = (this._SeparadorDatas == null ? "" : this._SeparadorDatas.ToString());
                            string Formatacao = "{0:MM" + sep + "dd}";
                            this._ValorFormatado = String.Format(Formatacao, this._ValorNatural);
                        }
                        else
                        {
                            this._ValorNatural = "";
                            goto case TTiposDadoEDI.ediAlphaAliEsquerda_____;
                        }
                        break;
                    }
                case TTiposDadoEDI.ediHoraHHMM_____________:
                    {
                        string sep = (this._SeparadorHora == null ? "" : this._SeparadorHora.ToString());
                        string Formatacao = "{0:HH" + sep + "mm}";
                        this._ValorFormatado = String.Format(Formatacao, this._ValorNatural);
                        break;
                    }
                case TTiposDadoEDI.ediHoraHHMMSS___________:
                    {
                        string sep = (this._SeparadorHora == null ? "" : this._SeparadorHora.ToString());
                        string Formatacao = "{0:HH" + sep + "mm" + sep + "ss}";
                        this._ValorFormatado = String.Format(Formatacao, this._ValorNatural);
                        break;
                    }
                case TTiposDadoEDI.ediDataDDMMAAAAWithZeros:
                    {
                        string sep = (this._SeparadorDatas == null ? "" : this._SeparadorDatas.ToString());
                        if ( (this._ValorNatural != null) || (!this.ValorNatural.ToString().Trim().Equals("")))
                        {
                            string Formatacao = "{0:dd" + sep + "MM" + sep + "yyyy}";
                            this._ValorFormatado = String.Format(Formatacao, this._ValorNatural);
                        }
                        else
                        {
                            this._ValorFormatado = "00" + sep + "00" + sep + "0000";
                        }
                        break;
                    }
                case TTiposDadoEDI.ediDataAAAAMMDDWithZeros:
                    {
                        string sep = (this._SeparadorDatas == null ? "" : this._SeparadorDatas.ToString());
                        if (this._ValorNatural != null)
                        {
                            string Formatacao = "{0:yyyy" + sep + "MM" + sep + "dd}";
                            this._ValorFormatado = String.Format(Formatacao, this._ValorNatural);
                        }
                        else
                        {
                            this._ValorFormatado = "00" + sep + "00" + sep + "0000";
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// Transforma o valor vindo do campo do registro EDI da propriedade ValorFormatado para o valor natural (com o tipo
        /// de dado adequado) na propriedade ValorNatural
        /// </summary>
        public void DecodificarEDIParaNatural()
        {
            if (this._ValorFormatado.Trim().Equals(""))
            {
                this._ValorNatural = null;
            }
            else
            {
                switch (this._TipoCampo)
                {
                    case TTiposDadoEDI.ediAlphaAliEsquerda_____:
                        {
                            this._ValorNatural = this._ValorFormatado.ToString().Trim();
                            break;
                        }
                    case TTiposDadoEDI.ediAlphaAliDireita______:
                        {
                            this._ValorNatural = this._ValorFormatado.ToString().Trim();
                            break;
                        }
                    case TTiposDadoEDI.ediInteiro______________:
                        {
                            this._ValorNatural = long.Parse(this._ValorFormatado.ToString().Trim());
                            break;
                        }
                    case TTiposDadoEDI.ediNumericoSemSeparador_:
                        {
                            string s = this._ValorFormatado.Substring(0, this._ValorFormatado.Length - this._QtdDecimais) + "," + this._ValorFormatado.Substring(this._ValorFormatado.Length - this._QtdDecimais, this._QtdDecimais);
                            this._ValorNatural = Double.Parse(s.Trim());
                            break;
                        }
                    case TTiposDadoEDI.ediNumericoComPonto_____:
                        {
                            this._ValorNatural = Double.Parse(this._ValorFormatado.ToString().Replace(".", ",").Trim());
                            break;
                        }
                    case TTiposDadoEDI.ediNumericoComVirgula___:
                        {
                            this._ValorNatural = Double.Parse(this._ValorFormatado.ToString().Trim().Replace(".", ","));
                            break;
                        }
                    case TTiposDadoEDI.ediDataAAAAMMDD_________:
                        {
                            if (!this._ValorFormatado.Trim().Equals(""))
                            {
                                string cAno = "";
                                string cMes = "";
                                string cDia = "";
                                if (this._SeparadorDatas != null)
                                {
                                    string[] split = this._ValorFormatado.Split(this._SeparadorDatas.ToCharArray());
                                    cAno = split[0];
                                    cMes = split[1];
                                    cDia = split[2];
                                }
                                else
                                {
                                    cAno = this._ValorFormatado.Substring(0, 4);
                                    cMes = this._ValorFormatado.Substring(4, 2);
                                    cDia = this._ValorFormatado.Substring(6, 2);
                                }
                                if ((cDia.Equals("00") && cMes.Equals("00") && cAno.Equals("0000")))
                                {
                                    this._ValorNatural = null;
                                }
                                else
                                {
                                    this._ValorNatural = DateTime.Parse(cDia + "/" + cMes + "/" + cAno);
                                }
                            }
                            else
                            {
                                this._ValorNatural = null;
                            }
                            break;
                        }
                    case TTiposDadoEDI.ediDataDDMM_____________:
                        {
                            if (!this._ValorFormatado.Trim().Equals(""))
                            {
                                string cAno = "1900";
                                string cMes = "";
                                string cDia = "";
                                if (this._SeparadorDatas != null)
                                {
                                    string[] split = this._ValorFormatado.Split(this._SeparadorDatas.ToCharArray());
                                    cMes = split[1];
                                    cDia = split[0];
                                }
                                else
                                {
                                    cMes = this._ValorFormatado.Substring(2, 2);
                                    cDia = this._ValorFormatado.Substring(0, 2);
                                }
                                this._ValorNatural = DateTime.Parse(cDia + "/" + cMes + "/" + cAno);
                            }
                            else
                            {
                                this._ValorNatural = null;
                            }
                            break;
                        }
                    case TTiposDadoEDI.ediDataDDMMAAAA_________:
                        {
                            string cDia = "";
                            string cMes = "";
                            string cAno = "";
                            if (this._SeparadorDatas != null)
                            {
                                string[] split = this._ValorFormatado.Split(this._SeparadorDatas.ToCharArray());
                                cAno = split[2];
                                cMes = split[1];
                                cDia = split[0];
                            }
                            else
                            {
                                cDia = this._ValorFormatado.Substring(0, 2);
                                cMes = this._ValorFormatado.Substring(2, 2);
                                cAno = this._ValorFormatado.Substring(4, 4);
                            }
                            if ((cDia.Equals("00") && cMes.Equals("00") && cAno.Equals("0000")) || this._ValorFormatado.Trim().Equals(""))
                            {
                                this._ValorNatural = DateTime.Parse("01/01/1900"); //data start
                            }
                            else
                            {
                                this._ValorNatural = DateTime.Parse(cDia + "/" + cMes + "/" + cAno);
                            }
                            break;
                        }
                    case TTiposDadoEDI.ediDataDDMMAA___________:
                        {
                            string cDia = "";
                            string cMes = "";
                            string cAno = "";
                            if (this._SeparadorDatas != null)
                            {
                                string[] split = this._ValorFormatado.Split(this._SeparadorDatas.ToCharArray());
                                cAno = split[2];
                                cMes = split[1];
                                cDia = split[0];
                            }
                            else
                            {
                                cDia = this._ValorFormatado.Substring(0, 2);
                                cMes = this._ValorFormatado.Substring(2, 2);
                                cAno = this._ValorFormatado.Substring(4, 2);
                            }
                            this._ValorNatural = DateTime.Parse(cDia + "/" + cMes + "/" + cAno);
                            break;
                        }
                    case TTiposDadoEDI.ediDataMMAAAA___________:
                        {
                            string cDia = "01";
                            string cMes = "";
                            string cAno = "";
                            if (this._SeparadorDatas != null)
                            {
                                string[] split = this._ValorFormatado.Split(this._SeparadorDatas.ToCharArray());
                                cAno = split[1];
                                cMes = split[0];
                            }
                            else
                            {
                                cMes = this._ValorFormatado.Substring(0, 2);
                                cAno = this._ValorFormatado.Substring(2, 4);
                            }
                            this._ValorNatural = DateTime.Parse(cDia + "/" + cMes + "/" + cAno);
                            break;
                        }
                    case TTiposDadoEDI.ediDataMMDD_____________:
                        {
                            string cDia = "";
                            string cMes = "";
                            string cAno = "1900";
                            if (this._SeparadorDatas != null)
                            {
                                string[] split = this._ValorFormatado.Split(this._SeparadorDatas.ToCharArray());
                                cMes = split[0];
                                cDia = split[1];
                            }
                            else
                            {
                                cDia = this._ValorFormatado.Substring(2, 2);
                                cMes = this._ValorFormatado.Substring(0, 2);
                            }
                            this._ValorNatural = DateTime.Parse(cDia + "/" + cMes + "/" + cAno);
                            break;
                        }
                    case TTiposDadoEDI.ediHoraHHMM_____________:
                        {
                            string cHora = "";
                            string cMinuto = "";
                            if (this._SeparadorHora != null)
                            {
                                string[] split = this._ValorFormatado.Split(this._SeparadorHora.ToCharArray());
                                cHora = split[0];
                                cMinuto = split[1];
                            }
                            else
                            {
                                cHora = this._ValorFormatado.Substring(0, 2);
                                cMinuto = this._ValorFormatado.Substring(2, 2);
                            }
                            this._ValorNatural = DateTime.Parse(cHora + ":" + cMinuto + ":00");
                            break;
                        }
                    case TTiposDadoEDI.ediHoraHHMMSS___________:
                        {
                            string cHora = "";
                            string cMinuto = "";
                            string cSegundo = "";
                            if (this._SeparadorHora != null)
                            {
                                string[] split = this._ValorFormatado.Split(this._SeparadorHora.ToCharArray());
                                cHora = split[0];
                                cMinuto = split[1];
                                cSegundo = split[2];
                            }
                            else
                            {
                                cHora = this._ValorFormatado.Substring(0, 2);
                                cMinuto = this._ValorFormatado.Substring(2, 2);
                                cSegundo = this._ValorFormatado.Substring(4, 2);
                            }
                            this._ValorNatural = DateTime.Parse(cHora + ":" + cMinuto + ":00");
                            break;
                        }
                    case TTiposDadoEDI.ediDataDDMMAAAAWithZeros:
                        {
                            goto case TTiposDadoEDI.ediDataDDMMAAAA_________;
                        }
                    case TTiposDadoEDI.ediDataAAAAMMDDWithZeros:
                        {
                            goto case TTiposDadoEDI.ediDataAAAAMMDD_________;
                        }
                }
            }
        }

        #endregion

        #region Métodos Privados e Protegidos

        #endregion


    }

    /// <summary>
    /// Indica os tipos de registro possíveis em um arquivo EDI
    /// </summary>
    public enum TTipoRegistroEDI
    {
        /// <summary>
        /// Indicador de registro Header
        /// </summary>
        treHeader,
        /// <summary>
        /// Indica um registro detalhe
        /// </summary>
        treDetalhe,
        /// <summary>
        /// Indica um registro Trailler
        /// </summary>
        treTrailler,
        /// <summary>
        /// Indica um registro sem definições, utilizado para transmissão socket ou similar
        /// </summary>
        treLinhaUnica
    }

    /// <summary>
    /// Classe representativa de um registro (linha) de um arquivo EDI
    /// </summary>
    public class TRegistroEDI
    {
        #region Variáveis Privadas e Protegidas
        protected TTipoRegistroEDI _TipoRegistro;
        protected int _TamanhoMaximo = 0;
        protected char _CaracterPreenchimento = ' ';
        private string _LinhaRegistro;
        protected List<TCampoRegistroEDI> _CamposEDI = new List<TCampoRegistroEDI>();
        #endregion

        #region Propriedades
        /// <summary>
        /// Tipo de Registro da linha do arquivo EDI
        /// </summary>
        public TTipoRegistroEDI TipoRegistro
        {
            get { return _TipoRegistro; }
        }

        /// <summary>
        /// Seta a linha do registro para a decodificação nos campos;
        /// Obtém a linha decodificada a partir dos campos.
        /// </summary>
        public string LinhaRegistro
        {
            get { return _LinhaRegistro; }
            set { _LinhaRegistro = value; }
        }

        /// <summary>
        /// Coleção dos campos do registro EDI
        /// </summary>
        public List<TCampoRegistroEDI> CamposEDI
        {
            get { return _CamposEDI; }
            set { _CamposEDI = value; }
        }
        #endregion

        #region Métodos Privados e Protegidos
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Codifica uma linha a partir dos campos; o resultado irá na propriedade LinhaRegistro
        /// </summary>
        public virtual void CodificarLinha()
        {
            this._LinhaRegistro = "";
            foreach (TCampoRegistroEDI campos in this._CamposEDI)
            {
                campos.CodificarNaturalParaEDI();
                this._LinhaRegistro += campos.ValorFormatado; 
            }
        }

        /// <summary>
        /// Decodifica uma linha a partir da propriedade LinhaRegistro nos campos do registro
        /// </summary>
        public virtual void DecodificarLinha()
        {
            foreach (TCampoRegistroEDI campos in this._CamposEDI)
            {
                if (this._TamanhoMaximo > 0)
                {
                    this._LinhaRegistro = this._LinhaRegistro.PadRight(this._TamanhoMaximo, this._CaracterPreenchimento);
                }
                campos.ValorFormatado = this._LinhaRegistro.Substring(campos.PosicaoInicial, campos.TamanhoCampo);
                campos.DecodificarEDIParaNatural();
            }
        }
        #endregion
    }
}
