using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerador

    public enum EnumEspecieDocumento_Safra
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

    public class EspecieDocumento_Safra : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Safra()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Safra(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Safra especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Safra.Cheque: return "1";
                case EnumEspecieDocumento_Safra.DuplicataMercantil: return "2";
                case EnumEspecieDocumento_Safra.DuplicataMercantilIndicacao: return "3";
                case EnumEspecieDocumento_Safra.DuplicataServico: return "4";
                case EnumEspecieDocumento_Safra.DuplicataServicoIndicacao: return "5";
                case EnumEspecieDocumento_Safra.DuplicataRural: return "6";
                case EnumEspecieDocumento_Safra.LetraCambio : return "7";
                case EnumEspecieDocumento_Safra.NotaCreditoComercial: return "8";
                case EnumEspecieDocumento_Safra.NotaCreditoExportacao : return "9";
                case EnumEspecieDocumento_Safra.NotaCreditoIndustrial: return "10";
                case EnumEspecieDocumento_Safra.NotaCreditoRural : return "11";
                case EnumEspecieDocumento_Safra.NotaPromissoria : return "12";
                case EnumEspecieDocumento_Safra.NotaPromissoriaRural: return "13";
                case EnumEspecieDocumento_Safra.TriplicataMercantil: return "14";
                case EnumEspecieDocumento_Safra.TriplicataServico : return "15";
                case EnumEspecieDocumento_Safra.NotaSeguro : return "16";
                case EnumEspecieDocumento_Safra.Recibo: return "17";
                case EnumEspecieDocumento_Safra.Fatura: return "18";
                case EnumEspecieDocumento_Safra.NotaDebito: return "19";
                case EnumEspecieDocumento_Safra.ApoliceSeguro: return "20";
                case EnumEspecieDocumento_Safra.MensalidadeEscolar: return "21";
                case EnumEspecieDocumento_Safra.ParcelaConsorcio: return "22";
                case EnumEspecieDocumento_Safra.Outros: return "23";
                default: return "23";

            }
        }

        public EnumEspecieDocumento_Safra getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1": return EnumEspecieDocumento_Safra.Cheque;
                case "2": return EnumEspecieDocumento_Safra.DuplicataMercantil;
                case "3": return EnumEspecieDocumento_Safra.DuplicataMercantilIndicacao;
                case "4": return EnumEspecieDocumento_Safra.DuplicataServico;
                case "5": return EnumEspecieDocumento_Safra.DuplicataServicoIndicacao;
                case "6": return EnumEspecieDocumento_Safra.DuplicataRural;
                case "7": return EnumEspecieDocumento_Safra.LetraCambio;
                case "8": return EnumEspecieDocumento_Safra.NotaCreditoComercial;
                case "9": return EnumEspecieDocumento_Safra.NotaCreditoExportacao;
                case "10": return EnumEspecieDocumento_Safra.NotaCreditoIndustrial;
                case "11": return EnumEspecieDocumento_Safra.NotaCreditoRural;
                case "12": return EnumEspecieDocumento_Safra.NotaPromissoria;
                case "13": return EnumEspecieDocumento_Safra.NotaPromissoriaRural;
                case "14": return EnumEspecieDocumento_Safra.TriplicataMercantil;
                case "15": return EnumEspecieDocumento_Safra.TriplicataServico;
                case "16": return EnumEspecieDocumento_Safra.NotaSeguro;
                case "17": return EnumEspecieDocumento_Safra.Recibo;
                case "18": return EnumEspecieDocumento_Safra.Fatura;
                case "19": return EnumEspecieDocumento_Safra.NotaDebito;
                case "20": return EnumEspecieDocumento_Safra.ApoliceSeguro;
                case "21": return EnumEspecieDocumento_Safra.MensalidadeEscolar;
                case "22": return EnumEspecieDocumento_Safra.ParcelaConsorcio;
                case "23": return EnumEspecieDocumento_Safra.Outros;
                default: return EnumEspecieDocumento_Safra.DuplicataMercantil;
            }
        }

        public override string getCodigoEspecieBySigla(string sigla)
        {
            switch (sigla)
            {
                case "CH": return "1";
                case "DM": return "2";
                case "DMI": return "3";
                case "DS": return "4";
                case "DSI": return "5";
                case "DR": return "6";
                case "LC": return "7";
                case "NCC": return "8";
                case "NCE": return "9";
                case "NCI": return "10";
                case "NCR": return "11";
                case "NP": return "12";
                case "NPR": return "13";
                case "TM": return "14";
                case "TS": return "15";
                case "NS": return "16";
                case "RC": return "17";
                case "FAT": return "18";
                case "ND": return "19";
                case "AP": return "20";
                case "ME": return "21";
                case "PC": return "22";
                case "OUTROS": return "23";
                default: return "2";
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Safra();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Safra.Cheque:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.Cheque);
                        this.Especie = "CHEQUE";
                        this.Sigla = "CH";
                        break;
                    case EnumEspecieDocumento_Safra.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.DuplicataMercantil);
                        this.Especie = "DUPLICATA MERCANTIL";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Safra.DuplicataMercantilIndicacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.DuplicataMercantilIndicacao);
                        this.Especie = "DUPLICATA MERCANTIL P/ INDICAÇÃO";
                        this.Sigla = "DMI";
                        break;
                    case EnumEspecieDocumento_Safra.DuplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.DuplicataServico);
                        this.Especie = "DUPLICATA DE SERVIÇO";
                        this.Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_Safra.DuplicataServicoIndicacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.DuplicataServicoIndicacao);
                        this.Especie = "DUPLICATA DE SERVIÇO P/ INDICAÇÃO";
                        this.Sigla = "DSI";
                        break;
                    case EnumEspecieDocumento_Safra.DuplicataRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.DuplicataRural);
                        this.Especie = "DUPLICATA RURAL";
                        this.Sigla = "DR";
                        break;
                    case EnumEspecieDocumento_Safra.LetraCambio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.LetraCambio);
                        this.Especie = "LETRA DE CAMBIO";
                        this.Sigla = "LC";
                        break;
                    case EnumEspecieDocumento_Safra.NotaCreditoComercial:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.NotaCreditoComercial);
                        this.Especie = "NOTA DE CRÉDITO COMERCIAL";
                        this.Sigla = "NCC";
                        break;
                    case EnumEspecieDocumento_Safra.NotaCreditoExportacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.NotaCreditoExportacao);
                        this.Especie = "NOTA DE CRÉDITO A EXPORTAÇÃO";
                        this.Sigla = "NCE";
                        break;
                    case EnumEspecieDocumento_Safra.NotaCreditoIndustrial:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.NotaCreditoIndustrial);
                        this.Especie = "NOTA DE CRÉDITO INDUSTRIAL";
                        this.Sigla = "NCI";
                        break;
                    case EnumEspecieDocumento_Safra.NotaCreditoRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.NotaCreditoRural);
                        this.Especie = "NOTA DE CRÉDITO RURAL";
                        this.Sigla = "NCR";
                        break;
                    case EnumEspecieDocumento_Safra.NotaPromissoria:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.NotaPromissoria);
                        this.Especie = "NOTA PROMISSÓRIA";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_Safra.NotaPromissoriaRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.NotaPromissoriaRural);
                        this.Especie = "NOTA PROMISSÓRIA RURAL";
                        this.Sigla = "NPR";
                        break;
                    case EnumEspecieDocumento_Safra.TriplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.TriplicataMercantil);
                        this.Especie = "TRIPLICATA MERCANTIL";
                        this.Sigla = "TM";
                        break;
                    case EnumEspecieDocumento_Safra.TriplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.TriplicataServico);
                        this.Especie = "TRIPLICATA DE SERVIÇO";
                        this.Sigla = "TS";
                        break;
                    case EnumEspecieDocumento_Safra.NotaSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.NotaSeguro);
                        this.Especie = "NOTA DE SEGURO";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Safra.Recibo:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.Recibo);
                        this.Especie = "RECIBO";
                        this.Sigla = "RC";
                        break;
                    case EnumEspecieDocumento_Safra.Fatura:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.Fatura);
                        this.Especie = "FATURA";
                        this.Sigla = "FAT";
                        break;
                    case EnumEspecieDocumento_Safra.NotaDebito:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.NotaDebito);
                        this.Especie = "NOTA DE DÉBITO";
                        this.Sigla = "ND";
                        break;
                    case EnumEspecieDocumento_Safra.ApoliceSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.ApoliceSeguro);
                        this.Especie = "APÓLICE DE SEGURO";
                        this.Sigla = "AP";
                        break;
                    case EnumEspecieDocumento_Safra.MensalidadeEscolar:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.MensalidadeEscolar);
                        this.Especie = "MENSALIDADE ESCOLAR";
                        this.Sigla = "ME";
                        break;
                    case EnumEspecieDocumento_Safra.ParcelaConsorcio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.ParcelaConsorcio);
                        this.Especie = "PARCELA DE CONSÓRCIO";
                        this.Sigla = "PC";
                        break;
                    case EnumEspecieDocumento_Safra.Outros:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.Outros);
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
            EspecieDocumento_Safra ed = new EspecieDocumento_Safra();

            foreach (EnumEspecieDocumento_Safra item in Enum.GetValues(typeof(EnumEspecieDocumento_Safra)))
                especiesDocumento.Add(new EspecieDocumento_Safra(ed.getCodigoEspecieByEnum(item)));

            return especiesDocumento;
        }

        public override IEspecieDocumento DuplicataMercantil()
        {
            return new EspecieDocumento_Safra(getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.DuplicataMercantil));
        }

        #endregion
    }
}
