using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_Sicoob
    {
        Cheque = 1,
        DuplicataMercantil = 2,
        DuplicataMercantilIndicacao = 3,
        DuplicataServico = 4,
        DuplicataServicoIndicacao = 5,
        DuplicataRural = 6,
        LetraCambio = 7,
        NotaCreditoComercial = 8,
        NotaCreditoExportacao = 9,
        NotaCreditoIndustrial = 10,
        NotaCreditoRural = 11,
        NotaPromissoria = 12,
        NotaPromissoriaRural = 13,
        TriplicataMercantil = 14,
        TriplicataServico = 15,
        NotaSeguro = 16,
        Recibo = 17,
        Fatura = 18,
        NotaDebito = 19,
        ApoliceSeguro = 20,
        MensalidadeEscolar = 21,
        ParcelaConsorcio = 22,
        NotaFiscal = 23,
        DocumentoDivida = 24,
        CedulaProdutoRural = 25,
        BoletoProposta = 32,
        Outros = 99
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

        public override string getCodigoEspecieBySigla(string sigla)
        {
            switch (sigla)
            {
                case "CH": return "1";
                case "DM": return "2";
                case "DS": return "4";
                case "DR": return "6";
                case "LC": return "7";
                case "NP": return "12";
                case "TP": return "14";
                case "TS": return "15";
                case "NS": return "16";
                case "RC": return "17";
                case "FT": return "18";
                case "ND": return "19";
                case "AP": return "20";
                case "ME": return "21";
                case "PC": return "22";
                case "OU": return "23";              
                default: return "2";
            }
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