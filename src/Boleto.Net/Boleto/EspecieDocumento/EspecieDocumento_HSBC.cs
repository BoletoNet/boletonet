using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_HSBC
    {
        Cheque = 1, //CH – CHEQUE
        DuplicataMercantil = 2, //DM – DUPLICATA MERCANTIL
        DuplicataMercantilIndicacao = 3, //DMI – DUPLICATA MERCANTIL P/ INDICAÇÃO
        DuplicataServico = 4, //DS –  DUPLICATA DE SERVIÇO
        DuplicataServicoIndicacao = 5, //DSI –  DUPLICATA DE SERVIÇO P/ INDICAÇÃO
        DuplicataRural = 6, //DR – DUPLICATA RURAL
        LetraCambio = 7, //LC – LETRA DE CAMBIO
        NotaCreditoComercial = 8, //NCC – NOTA DE CRÉDITO COMERCIAL
        NotaCreditoExportacao = 9, //NCE – NOTA DE CRÉDITO A EXPORTAÇÃO
        NotaCreditoIndustrial = 10, //NCI – NOTA DE CRÉDITO INDUSTRIAL
        NotaCreditoRural = 11, //NCR – NOTA DE CRÉDITO RURAL
        NotaPromissoria = 12, //NP – NOTA PROMISSÓRIA
        NotaPromissoriaRural = 13, //NPR –NOTA PROMISSÓRIA RURAL
        TriplicataMercantil = 14, //TM – TRIPLICATA MERCANTIL
        TriplicataServico = 15, //TS –  TRIPLICATA DE SERVIÇO
        NotaSeguro = 16, //NS – NOTA DE SEGURO
        Recibo = 17, //RC – RECIBO
        Fatura = 18, //FAT – FATURA
        NotaDebito = 19, //ND –  NOTA DE DÉBITO
        ApoliceSeguro = 20, //AP –  APÓLICE DE SEGURO
        MensalidadeEscolar = 21, //ME – MENSALIDADE ESCOLAR
        ParcelaConsorcio = 22, //PC –  PARCELA DE CONSÓRCIO
        NotaFiscal = 23, //NF-Nota Fiscal
        DocumentoDivida = 24, //DD-Documento de Dívida
        CobrancaEmissaoCliente = 98,
        Outros = 99, //Outros
        PD = 0
    }

    #endregion

    public class EspecieDocumento_HSBC : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_HSBC()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_HSBC(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_HSBC.Cheque: return "1";
                case EnumEspecieDocumento_HSBC.DuplicataMercantil: return "2";
                case EnumEspecieDocumento_HSBC.DuplicataMercantilIndicacao: return "3";
                case EnumEspecieDocumento_HSBC.DuplicataServico: return "4";
                case EnumEspecieDocumento_HSBC.DuplicataServicoIndicacao: return "5";
                case EnumEspecieDocumento_HSBC.DuplicataRural: return "6";
                case EnumEspecieDocumento_HSBC.LetraCambio: return "7";
                case EnumEspecieDocumento_HSBC.NotaCreditoComercial: return "8";
                case EnumEspecieDocumento_HSBC.NotaCreditoExportacao: return "9";
                case EnumEspecieDocumento_HSBC.NotaCreditoIndustrial: return "10";
                case EnumEspecieDocumento_HSBC.NotaCreditoRural: return "11";
                case EnumEspecieDocumento_HSBC.NotaPromissoria: return "12";
                case EnumEspecieDocumento_HSBC.NotaPromissoriaRural: return "13";
                case EnumEspecieDocumento_HSBC.TriplicataMercantil: return "14";
                case EnumEspecieDocumento_HSBC.TriplicataServico: return "15";
                case EnumEspecieDocumento_HSBC.NotaSeguro: return "16";
                case EnumEspecieDocumento_HSBC.Recibo: return "17";
                case EnumEspecieDocumento_HSBC.Fatura: return "18";
                case EnumEspecieDocumento_HSBC.NotaDebito: return "19";
                case EnumEspecieDocumento_HSBC.ApoliceSeguro: return "20";
                case EnumEspecieDocumento_HSBC.MensalidadeEscolar: return "21";
                case EnumEspecieDocumento_HSBC.ParcelaConsorcio: return "22";
                case EnumEspecieDocumento_HSBC.NotaFiscal: return "23";
                case EnumEspecieDocumento_HSBC.DocumentoDivida: return "24";
                case EnumEspecieDocumento_HSBC.CobrancaEmissaoCliente: return "98";
                case EnumEspecieDocumento_HSBC.Outros: return "99";
                case EnumEspecieDocumento_HSBC.PD: return "0";
                default: return "99";

            }
        }

        public EnumEspecieDocumento_HSBC getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1": return EnumEspecieDocumento_HSBC.Cheque;
                case "2": return EnumEspecieDocumento_HSBC.DuplicataMercantil;
                case "3": return EnumEspecieDocumento_HSBC.DuplicataMercantilIndicacao;
                case "4": return EnumEspecieDocumento_HSBC.DuplicataServico;
                case "5": return EnumEspecieDocumento_HSBC.DuplicataServicoIndicacao;
                case "6": return EnumEspecieDocumento_HSBC.DuplicataRural;
                case "7": return EnumEspecieDocumento_HSBC.LetraCambio;
                case "8": return EnumEspecieDocumento_HSBC.NotaCreditoComercial;
                case "9": return EnumEspecieDocumento_HSBC.NotaCreditoExportacao;
                case "10": return EnumEspecieDocumento_HSBC.NotaCreditoIndustrial;
                case "11": return EnumEspecieDocumento_HSBC.NotaCreditoRural;
                case "12": return EnumEspecieDocumento_HSBC.NotaPromissoria;
                case "13": return EnumEspecieDocumento_HSBC.NotaPromissoriaRural;
                case "14": return EnumEspecieDocumento_HSBC.TriplicataMercantil;
                case "15": return EnumEspecieDocumento_HSBC.TriplicataServico;
                case "16": return EnumEspecieDocumento_HSBC.NotaSeguro;
                case "17": return EnumEspecieDocumento_HSBC.Recibo;
                case "18": return EnumEspecieDocumento_HSBC.Fatura;
                case "19": return EnumEspecieDocumento_HSBC.NotaDebito;
                case "20": return EnumEspecieDocumento_HSBC.ApoliceSeguro;
                case "21": return EnumEspecieDocumento_HSBC.MensalidadeEscolar;
                case "22": return EnumEspecieDocumento_HSBC.ParcelaConsorcio;
                case "23": return EnumEspecieDocumento_HSBC.NotaFiscal;
                case "24": return EnumEspecieDocumento_HSBC.DocumentoDivida;
                case "98": return EnumEspecieDocumento_HSBC.CobrancaEmissaoCliente;
                case "99": return EnumEspecieDocumento_HSBC.Outros;
                case "0": return EnumEspecieDocumento_HSBC.PD;
                default: return EnumEspecieDocumento_HSBC.Outros;
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
                case "NF": return "23";
                case "DD": return "24";
                case "CEC": return "98";
                case "OUTROS": return "99";
                case "PD": return "0";
                default: return "99";
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_HSBC();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_HSBC.Cheque:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.Cheque);
                        this.Especie = "CHEQUE";
                        this.Sigla = "CH";
                        break;
                    case EnumEspecieDocumento_HSBC.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.DuplicataMercantil);
                        this.Especie = "DUPLICATA MERCANTIL";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_HSBC.DuplicataMercantilIndicacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.DuplicataMercantilIndicacao);
                        this.Especie = "DUPLICATA MERCANTIL P/ INDICAÇÃO";
                        this.Sigla = "DMI";
                        break;
                    case EnumEspecieDocumento_HSBC.DuplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.DuplicataServico);
                        this.Especie = "DUPLICATA DE SERVIÇO";
                        this.Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_HSBC.DuplicataServicoIndicacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.DuplicataServicoIndicacao);
                        this.Especie = "DUPLICATA DE SERVIÇO P/ INDICAÇÃO";
                        this.Sigla = "DSI";
                        break;
                    case EnumEspecieDocumento_HSBC.DuplicataRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.DuplicataRural);
                        this.Especie = "DUPLICATA RURAL";
                        this.Sigla = "DR";
                        break;
                    case EnumEspecieDocumento_HSBC.LetraCambio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.LetraCambio);
                        this.Especie = "LETRA DE CAMBIO";
                        this.Sigla = "LC";
                        break;
                    case EnumEspecieDocumento_HSBC.NotaCreditoComercial:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.NotaCreditoComercial);
                        this.Especie = "NOTA DE CRÉDITO COMERCIAL";
                        this.Sigla = "NCC";
                        break;
                    case EnumEspecieDocumento_HSBC.NotaCreditoExportacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.NotaCreditoExportacao);
                        this.Especie = "NOTA DE CRÉDITO A EXPORTAÇÃO";
                        this.Sigla = "NCE";
                        break;
                    case EnumEspecieDocumento_HSBC.NotaCreditoIndustrial:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.NotaCreditoIndustrial);
                        this.Especie = "NOTA DE CRÉDITO INDUSTRIAL";
                        this.Sigla = "NCI";
                        break;
                    case EnumEspecieDocumento_HSBC.NotaCreditoRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.NotaCreditoRural);
                        this.Especie = "NOTA DE CRÉDITO RURAL";
                        this.Sigla = "NCR";
                        break;
                    case EnumEspecieDocumento_HSBC.NotaPromissoria:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.NotaPromissoria);
                        this.Especie = "NOTA PROMISSÓRIA";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_HSBC.NotaPromissoriaRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.NotaPromissoriaRural);
                        this.Especie = "NOTA PROMISSÓRIA RURAL";
                        this.Sigla = "NPR";
                        break;
                    case EnumEspecieDocumento_HSBC.TriplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.TriplicataMercantil);
                        this.Especie = "TRIPLICATA MERCANTIL";
                        this.Sigla = "TM";
                        break;
                    case EnumEspecieDocumento_HSBC.TriplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.TriplicataServico);
                        this.Especie = "TRIPLICATA DE SERVIÇO";
                        this.Sigla = "TS";
                        break;
                    case EnumEspecieDocumento_HSBC.NotaSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.NotaSeguro);
                        this.Especie = "NOTA DE SEGURO";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_HSBC.Recibo:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.Recibo);
                        this.Especie = "RECIBO";
                        this.Sigla = "RC";
                        break;
                    case EnumEspecieDocumento_HSBC.Fatura:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.Fatura);
                        this.Especie = "FATURA";
                        this.Sigla = "FAT";
                        break;
                    case EnumEspecieDocumento_HSBC.NotaDebito:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.NotaDebito);
                        this.Especie = "NOTA DE DÉBITO";
                        this.Sigla = "ND";
                        break;
                    case EnumEspecieDocumento_HSBC.ApoliceSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.ApoliceSeguro);
                        this.Especie = "APÓLICE DE SEGURO";
                        this.Sigla = "AP";
                        break;
                    case EnumEspecieDocumento_HSBC.MensalidadeEscolar:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.MensalidadeEscolar);
                        this.Especie = "MENSALIDADE ESCOLAR";
                        this.Sigla = "ME";
                        break;
                    case EnumEspecieDocumento_HSBC.ParcelaConsorcio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.ParcelaConsorcio);
                        this.Especie = "PARCELA DE CONSÓRCIO";
                        this.Sigla = "PC";
                        break;
                    case EnumEspecieDocumento_HSBC.NotaFiscal:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.NotaFiscal);
                        this.Especie = "NOTA FISCAL";
                        this.Sigla = "NF";
                        break;
                    case EnumEspecieDocumento_HSBC.DocumentoDivida:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.DocumentoDivida);
                        this.Especie = "DOCUMENTO DE DIVIDA";
                        this.Sigla = "DD";
                        break;
                    case EnumEspecieDocumento_HSBC.Outros:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.Outros);
                        this.Especie = "OUTROS";
                        this.Sigla = "OUTROS";
                        break;
                    case EnumEspecieDocumento_HSBC.CobrancaEmissaoCliente:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.CobrancaEmissaoCliente);
                        this.Especie = "COBRANÇA COM EMISSÃO TOTAL DO BLOQUETO PELO CLIENTE";
                        this.Sigla = "PD";
                        break;
                    case EnumEspecieDocumento_HSBC.PD:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.PD);
                        this.Especie = "PD";
                        this.Sigla = "PD";
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
            try
            {
                EspeciesDocumento alEspeciesDocumento = new EspeciesDocumento();
                EspecieDocumento_HSBC ed = new EspecieDocumento_HSBC();

                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.Cheque)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.DuplicataMercantil)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.DuplicataMercantilIndicacao)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.DuplicataServico)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.DuplicataServicoIndicacao)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.DuplicataRural)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.LetraCambio)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.NotaCreditoComercial)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.NotaCreditoExportacao)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.NotaCreditoIndustrial)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.NotaCreditoRural)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.NotaPromissoria)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.NotaPromissoriaRural)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.TriplicataMercantil)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.TriplicataServico)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.NotaSeguro)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.Recibo)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.Fatura)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.NotaDebito)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.ApoliceSeguro)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.MensalidadeEscolar)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.ParcelaConsorcio)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.NotaFiscal)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.DocumentoDivida)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.Outros)));

                return alEspeciesDocumento;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar objetos", ex);
            }
        }

        public override IEspecieDocumento DuplicataMercantil()
        {
            return new EspecieDocumento_HSBC(getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC.DuplicataMercantil));
        }

        #endregion
    }
}
