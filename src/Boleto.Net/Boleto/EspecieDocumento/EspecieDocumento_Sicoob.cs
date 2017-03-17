using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_Sicoob
    {
        DuplicataMercantil = 1,
        NotaPromissoria = 2,
        NotaSeguro = 3,
        Recibo = 5,
        DuplicataRural = 6,
        LetraCambio = 8,
        Warrant = 9,
        Cheque = 10,
        DuplicataServico = 12,
        NotaDebito = 13,
        TriplicataMercantil = 14,
        TriplicataServico = 15,
        Fatura = 18,
        ApoliceSeguro = 20,
        MensalidadeEscolar = 21,
        ParcelaConsorcio = 22,
        Outros = 99,
    }

    #endregion

    public class EspecieDocumento_Sicoob : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Sicoob()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Sicoob(string codigo)
        {
            try
            {
                this.carregar(codigo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        #endregion

        #region Metodos Privados

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob especie)
        {
            return Convert.ToInt32(especie).ToString("00");
        }

        public EnumEspecieDocumento_Sicoob getEnumEspecieByCodigo(string codigo)
        {
            return (EnumEspecieDocumento_Sicoob) Convert.ToInt32(codigo);
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Sicoob();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Sicoob.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.DuplicataMercantil);
                        this.Especie = "Duplicata mercantil";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Sicoob.NotaPromissoria:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.NotaPromissoria);
                        this.Especie = "Nota promissória";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_Sicoob.NotaSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.NotaSeguro);
                        this.Especie = "Nota de seguro";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Sicoob.Recibo:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.Recibo);
                        this.Especie = "Recibo";
                        this.Sigla = "RC";
                        break;
                    case EnumEspecieDocumento_Sicoob.DuplicataRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.DuplicataRural);
                        this.Especie = "Duplicata Rural";
                        this.Sigla = "DR";
                        break;
                    case EnumEspecieDocumento_Sicoob.LetraCambio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.LetraCambio);
                        this.Sigla = "LC";
                        this.Especie = "Letra de Câmbio";
                        break;
                    case EnumEspecieDocumento_Sicoob.Warrant:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.Warrant);
                        this.Sigla = "WR";
                        this.Especie = "Warrant";
                        break;
                    case EnumEspecieDocumento_Sicoob.Cheque:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.Cheque);
                        this.Sigla = "CH";
                        this.Especie = "Cheque";
                        break;
                    case EnumEspecieDocumento_Sicoob.DuplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.DuplicataServico);
                        this.Sigla = "DS";
                        this.Especie = "Duplicata de serviço";
                        break;
                    case EnumEspecieDocumento_Sicoob.NotaDebito:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.NotaDebito);
                        this.Sigla = "ND";
                        this.Especie = "Nota de débito";
                        break;
                    case EnumEspecieDocumento_Sicoob.TriplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.TriplicataMercantil);
                        this.Sigla = "TP";
                        this.Especie = "Triplicata Mercantil";
                        break;
                    case EnumEspecieDocumento_Sicoob.TriplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.TriplicataServico);
                        this.Sigla = "TS";
                        this.Especie = "Triplicata de Serviço";
                        break;
                    case EnumEspecieDocumento_Sicoob.Fatura:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.Fatura);
                        this.Sigla = "FT";
                        this.Especie = "Fatura";
                        break;
                    case EnumEspecieDocumento_Sicoob.ApoliceSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.ApoliceSeguro);
                        this.Sigla = "AP";
                        this.Especie = "Apólice de Seguro";
                        break;
                    case EnumEspecieDocumento_Sicoob.MensalidadeEscolar:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.MensalidadeEscolar);
                        this.Sigla = "ME";
                        this.Especie = "Mensalidade Escolar";
                        break;
                    case EnumEspecieDocumento_Sicoob.ParcelaConsorcio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.ParcelaConsorcio);
                        this.Sigla = "PC";
                        this.Especie = "Parcela de Consórcio";
                        break;
                    case EnumEspecieDocumento_Sicoob.Outros:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.Outros);
                        this.Especie = "Outros";
                        this.Sigla = "OU";
                        break;
                    default:
                        this.Codigo = "0";
                        this.Especie = "( Selecione )";
                        this.Sigla = "";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public static EspeciesDocumento CarregaTodas()
        {
            try
            {
                var alEspeciesDocumento = new EspeciesDocumento();

                var obj = new EspecieDocumento_Sicoob();

                foreach (var item in Enum.GetValues(typeof (EnumEspecieDocumento_Sicoob)))
                {
                    obj = new EspecieDocumento_Sicoob(obj.getCodigoEspecieByEnum((EnumEspecieDocumento_Sicoob)item));
                    alEspeciesDocumento.Add(obj);
                }

                return alEspeciesDocumento;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar objetos", ex);
            }
        }

        public override IEspecieDocumento DuplicataMercantil()
        {
            return new EspecieDocumento_Sicoob(getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.DuplicataMercantil));
        }

        #endregion
    }
}