using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerador

    public enum EnumEspecieDocumento_Banrisul
    {
        Cheque,
        DuplicataMercantil,
        DuplicataMercantilIndicacao ,
        DuplicataServico ,
        DuplicataServicoIndicacao ,
        DuplicataRural ,
        LetraCambio ,
        NotaCreditoComercial ,
        NotaCreditoExportacao ,
        NotaCreditoIndustrial ,
        NotaCreditoRural ,
        NotaPromissoria,
        NotaPromissoriaRural,
        TriplicataMercantil,
        TriplicataServico,
        NotaSeguro,
        Recibo,
        Fatura,
        NotaDebito,
        ApoliceSeguro,
        MensalidadeEscolar,
        ParcelaConsorcio,
        Outros,
    }

    #endregion

    public class EspecieDocumento_Banrisul : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Banrisul()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Banrisul(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Banrisul.Cheque: return "1";
                case EnumEspecieDocumento_Banrisul.DuplicataMercantil: return "2";
                case EnumEspecieDocumento_Banrisul.DuplicataMercantilIndicacao: return "3";
                case EnumEspecieDocumento_Banrisul.DuplicataServico: return "4";
                case EnumEspecieDocumento_Banrisul.DuplicataServicoIndicacao: return "5";
                case EnumEspecieDocumento_Banrisul.DuplicataRural: return "6";
                case EnumEspecieDocumento_Banrisul.LetraCambio : return "7";
                case EnumEspecieDocumento_Banrisul.NotaCreditoComercial: return "8";
                case EnumEspecieDocumento_Banrisul.NotaCreditoExportacao : return "9";
                case EnumEspecieDocumento_Banrisul.NotaCreditoIndustrial: return "10";
                case EnumEspecieDocumento_Banrisul.NotaCreditoRural : return "11";
                case EnumEspecieDocumento_Banrisul.NotaPromissoria : return "12";
                case EnumEspecieDocumento_Banrisul.NotaPromissoriaRural: return "13";
                case EnumEspecieDocumento_Banrisul.TriplicataMercantil: return "14";
                case EnumEspecieDocumento_Banrisul.TriplicataServico : return "15";
                case EnumEspecieDocumento_Banrisul.NotaSeguro : return "16";
                case EnumEspecieDocumento_Banrisul.Recibo: return "17";
                case EnumEspecieDocumento_Banrisul.Fatura: return "18";
                case EnumEspecieDocumento_Banrisul.NotaDebito: return "19";
                case EnumEspecieDocumento_Banrisul.ApoliceSeguro: return "20";
                case EnumEspecieDocumento_Banrisul.MensalidadeEscolar: return "21";
                case EnumEspecieDocumento_Banrisul.ParcelaConsorcio: return "22";
                case EnumEspecieDocumento_Banrisul.Outros: return "23";
                default: return "23";

            }
        }

        public EnumEspecieDocumento_Banrisul getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1": return EnumEspecieDocumento_Banrisul.Cheque;
                case "2": return EnumEspecieDocumento_Banrisul.DuplicataMercantil;
                case "3": return EnumEspecieDocumento_Banrisul.DuplicataMercantilIndicacao;
                case "4": return EnumEspecieDocumento_Banrisul.DuplicataServico;
                case "5": return EnumEspecieDocumento_Banrisul.DuplicataServicoIndicacao;
                case "6": return EnumEspecieDocumento_Banrisul.DuplicataRural;
                case "7": return EnumEspecieDocumento_Banrisul.LetraCambio;
                case "8": return EnumEspecieDocumento_Banrisul.NotaCreditoComercial;
                case "9": return EnumEspecieDocumento_Banrisul.NotaCreditoExportacao;
                case "10": return EnumEspecieDocumento_Banrisul.NotaCreditoIndustrial;
                case "11": return EnumEspecieDocumento_Banrisul.NotaCreditoRural;
                case "12": return EnumEspecieDocumento_Banrisul.NotaPromissoria;
                case "13": return EnumEspecieDocumento_Banrisul.NotaPromissoriaRural;
                case "14": return EnumEspecieDocumento_Banrisul.TriplicataMercantil;
                case "15": return EnumEspecieDocumento_Banrisul.TriplicataServico;
                case "16": return EnumEspecieDocumento_Banrisul.NotaSeguro;
                case "17": return EnumEspecieDocumento_Banrisul.Recibo;
                case "18": return EnumEspecieDocumento_Banrisul.Fatura;
                case "19": return EnumEspecieDocumento_Banrisul.NotaDebito;
                case "20": return EnumEspecieDocumento_Banrisul.ApoliceSeguro;
                case "21": return EnumEspecieDocumento_Banrisul.MensalidadeEscolar;
                case "22": return EnumEspecieDocumento_Banrisul.ParcelaConsorcio;
                case "23": return EnumEspecieDocumento_Banrisul.Outros;
                default: return EnumEspecieDocumento_Banrisul.DuplicataMercantil;
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Banrisul();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Banrisul.Cheque:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.Cheque);
                        this.Especie = "CHEQUE";
                        this.Sigla = "CH";
                        break;
                    case EnumEspecieDocumento_Banrisul.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.DuplicataMercantil);
                        this.Especie = "DUPLICATA MERCANTIL";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Banrisul.DuplicataMercantilIndicacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.DuplicataMercantilIndicacao);
                        this.Especie = "DUPLICATA MERCANTIL P/ INDICAÇÃO";
                        this.Sigla = "DMI";
                        break;
                    case EnumEspecieDocumento_Banrisul.DuplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.DuplicataServico);
                        this.Especie = "DUPLICATA DE SERVIÇO";
                        this.Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_Banrisul.DuplicataServicoIndicacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.DuplicataServicoIndicacao);
                        this.Especie = "DUPLICATA DE SERVIÇO P/ INDICAÇÃO";
                        this.Sigla = "DSI";
                        break;
                    case EnumEspecieDocumento_Banrisul.DuplicataRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.DuplicataRural);
                        this.Especie = "DUPLICATA RURAL";
                        this.Sigla = "DR";
                        break;
                    case EnumEspecieDocumento_Banrisul.LetraCambio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.LetraCambio);
                        this.Especie = "LETRA DE CAMBIO";
                        this.Sigla = "LC";
                        break;
                    case EnumEspecieDocumento_Banrisul.NotaCreditoComercial:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.NotaCreditoComercial);
                        this.Especie = "NOTA DE CRÉDITO COMERCIAL";
                        this.Sigla = "NCC";
                        break;
                    case EnumEspecieDocumento_Banrisul.NotaCreditoExportacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.NotaCreditoExportacao);
                        this.Especie = "NOTA DE CRÉDITO A EXPORTAÇÃO";
                        this.Sigla = "NCE";
                        break;
                    case EnumEspecieDocumento_Banrisul.NotaCreditoIndustrial:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.NotaCreditoIndustrial);
                        this.Especie = "NOTA DE CRÉDITO INDUSTRIAL";
                        this.Sigla = "NCI";
                        break;
                    case EnumEspecieDocumento_Banrisul.NotaCreditoRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.NotaCreditoRural);
                        this.Especie = "NOTA DE CRÉDITO RURAL";
                        this.Sigla = "NCR";
                        break;
                    case EnumEspecieDocumento_Banrisul.NotaPromissoria:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.NotaPromissoria);
                        this.Especie = "NOTA PROMISSÓRIA";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_Banrisul.NotaPromissoriaRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.NotaPromissoriaRural);
                        this.Especie = "NOTA PROMISSÓRIA RURAL";
                        this.Sigla = "NPR";
                        break;
                    case EnumEspecieDocumento_Banrisul.TriplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.TriplicataMercantil);
                        this.Especie = "TRIPLICATA MERCANTIL";
                        this.Sigla = "TM";
                        break;
                    case EnumEspecieDocumento_Banrisul.TriplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.TriplicataServico);
                        this.Especie = "TRIPLICATA DE SERVIÇO";
                        this.Sigla = "TS";
                        break;
                    case EnumEspecieDocumento_Banrisul.NotaSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.NotaSeguro);
                        this.Especie = "NOTA DE SEGURO";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Banrisul.Recibo:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.Recibo);
                        this.Especie = "RECIBO";
                        this.Sigla = "RC";
                        break;
                    case EnumEspecieDocumento_Banrisul.Fatura:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.Fatura);
                        this.Especie = "FATURA";
                        this.Sigla = "FAT";
                        break;
                    case EnumEspecieDocumento_Banrisul.NotaDebito:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.NotaDebito);
                        this.Especie = "NOTA DE DÉBITO";
                        this.Sigla = "ND";
                        break;
                    case EnumEspecieDocumento_Banrisul.ApoliceSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.ApoliceSeguro);
                        this.Especie = "APÓLICE DE SEGURO";
                        this.Sigla = "AP";
                        break;
                    case EnumEspecieDocumento_Banrisul.MensalidadeEscolar:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.MensalidadeEscolar);
                        this.Especie = "MENSALIDADE ESCOLAR";
                        this.Sigla = "ME";
                        break;
                    case EnumEspecieDocumento_Banrisul.ParcelaConsorcio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.ParcelaConsorcio);
                        this.Especie = "PARCELA DE CONSÓRCIO";
                        this.Sigla = "PC";
                        break;
                    case EnumEspecieDocumento_Banrisul.Outros:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.Outros);
                        this.Especie = "OUTROS";
                        this.Sigla = "OUTROS";
                        break;
                    default:
                        this.Codigo = "0";
                        this.Especie = "( Selecione )";
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
            EspeciesDocumento especiesDocumento = new EspeciesDocumento();
            EspecieDocumento_Banrisul ed = new EspecieDocumento_Banrisul();

            foreach (EnumEspecieDocumento_Banrisul item in Enum.GetValues(typeof(EnumEspecieDocumento_Banrisul)))
                especiesDocumento.Add(new EspecieDocumento_Banrisul(ed.getCodigoEspecieByEnum(item)));

            return especiesDocumento;
        }

        #endregion
    }
}
