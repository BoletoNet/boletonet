using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_BancoBrasil
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
        Outros = 23 //OUTROS
    }

    #endregion

    public class EspecieDocumento_BancoBrasil : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_BancoBrasil()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_BancoBrasil(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_BancoBrasil.Cheque: return "1";
                case EnumEspecieDocumento_BancoBrasil.DuplicataMercantil: return "2";
                case EnumEspecieDocumento_BancoBrasil.DuplicataMercantilIndicacao: return "3";
                case EnumEspecieDocumento_BancoBrasil.DuplicataServico: return "4";
                case EnumEspecieDocumento_BancoBrasil.DuplicataServicoIndicacao: return "5";
                case EnumEspecieDocumento_BancoBrasil.DuplicataRural: return "6";
                case EnumEspecieDocumento_BancoBrasil.LetraCambio: return "7";
                case EnumEspecieDocumento_BancoBrasil.NotaCreditoComercial: return "8";
                case EnumEspecieDocumento_BancoBrasil.NotaCreditoExportacao: return "9";
                case EnumEspecieDocumento_BancoBrasil.NotaCreditoIndustrial: return "10";
                case EnumEspecieDocumento_BancoBrasil.NotaCreditoRural: return "11";
                case EnumEspecieDocumento_BancoBrasil.NotaPromissoria: return "12";
                case EnumEspecieDocumento_BancoBrasil.NotaPromissoriaRural: return "13";
                case EnumEspecieDocumento_BancoBrasil.TriplicataMercantil: return "14";
                case EnumEspecieDocumento_BancoBrasil.TriplicataServico: return "15";
                case EnumEspecieDocumento_BancoBrasil.NotaSeguro: return "16";
                case EnumEspecieDocumento_BancoBrasil.Recibo: return "17";
                case EnumEspecieDocumento_BancoBrasil.Fatura: return "18";
                case EnumEspecieDocumento_BancoBrasil.NotaDebito: return "19";
                case EnumEspecieDocumento_BancoBrasil.ApoliceSeguro: return "20";
                case EnumEspecieDocumento_BancoBrasil.MensalidadeEscolar: return "21";
                case EnumEspecieDocumento_BancoBrasil.ParcelaConsorcio: return "22";
                case EnumEspecieDocumento_BancoBrasil.Outros: return "23";
                default: return "23";

            }
        }

        public EnumEspecieDocumento_BancoBrasil getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1": return EnumEspecieDocumento_BancoBrasil.Cheque;
                case "2": return EnumEspecieDocumento_BancoBrasil.DuplicataMercantil;
                case "3": return EnumEspecieDocumento_BancoBrasil.DuplicataMercantilIndicacao;
                case "4": return EnumEspecieDocumento_BancoBrasil.DuplicataServico;
                case "5": return EnumEspecieDocumento_BancoBrasil.DuplicataServicoIndicacao;
                case "6": return EnumEspecieDocumento_BancoBrasil.DuplicataRural;
                case "7": return EnumEspecieDocumento_BancoBrasil.LetraCambio;
                case "8": return EnumEspecieDocumento_BancoBrasil.NotaCreditoComercial;
                case "9": return EnumEspecieDocumento_BancoBrasil.NotaCreditoExportacao;
                case "10": return EnumEspecieDocumento_BancoBrasil.NotaCreditoIndustrial;
                case "11": return EnumEspecieDocumento_BancoBrasil.NotaCreditoRural;
                case "12": return EnumEspecieDocumento_BancoBrasil.NotaPromissoria;
                case "13": return EnumEspecieDocumento_BancoBrasil.NotaPromissoriaRural;
                case "14": return EnumEspecieDocumento_BancoBrasil.TriplicataMercantil;
                case "15": return EnumEspecieDocumento_BancoBrasil.TriplicataServico;
                case "16": return EnumEspecieDocumento_BancoBrasil.NotaSeguro;
                case "17": return EnumEspecieDocumento_BancoBrasil.Recibo;
                case "18": return EnumEspecieDocumento_BancoBrasil.Fatura;
                case "19": return EnumEspecieDocumento_BancoBrasil.NotaDebito;
                case "20": return EnumEspecieDocumento_BancoBrasil.ApoliceSeguro;
                case "21": return EnumEspecieDocumento_BancoBrasil.MensalidadeEscolar;
                case "22": return EnumEspecieDocumento_BancoBrasil.ParcelaConsorcio;
                case "23": return EnumEspecieDocumento_BancoBrasil.Outros;
                default: return EnumEspecieDocumento_BancoBrasil.DuplicataMercantil;
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Brasil();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_BancoBrasil.Cheque:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Cheque);
                        this.Especie = "CHEQUE";
                        this.Sigla = "CH";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataMercantil);
                        this.Especie = "DUPLICATA MERCANTIL";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.DuplicataMercantilIndicacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataMercantilIndicacao);
                        this.Especie = "DUPLICATA MERCANTIL P/ INDICAÇÃO";
                        this.Sigla = "DMI";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.DuplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataServico);
                        this.Especie = "DUPLICATA DE SERVIÇO";
                        this.Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.DuplicataServicoIndicacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataServicoIndicacao);
                        this.Especie = "DUPLICATA DE SERVIÇO P/ INDICAÇÃO";
                        this.Sigla = "DSI";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.DuplicataRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataRural);
                        this.Especie = "DUPLICATA RURAL";
                        this.Sigla = "DR";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.LetraCambio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.LetraCambio);
                        this.Especie = "LETRA DE CAMBIO";
                        this.Sigla = "LC";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.NotaCreditoComercial:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaCreditoComercial);
                        this.Especie = "NOTA DE CRÉDITO COMERCIAL";
                        this.Sigla = "NCC";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.NotaCreditoExportacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaCreditoExportacao);
                        this.Especie = "NOTA DE CRÉDITO A EXPORTAÇÃO";
                        this.Sigla = "NCE";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.NotaCreditoIndustrial:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaCreditoIndustrial);
                        this.Especie = "NOTA DE CRÉDITO INDUSTRIAL";
                        this.Sigla = "NCI";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.NotaCreditoRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaCreditoRural);
                        this.Especie = "NOTA DE CRÉDITO RURAL";
                        this.Sigla = "NCR";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.NotaPromissoria:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaPromissoria);
                        this.Especie = "NOTA PROMISSÓRIA";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.NotaPromissoriaRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaPromissoriaRural);
                        this.Especie = "NOTA PROMISSÓRIA RURAL";
                        this.Sigla = "NPR";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.TriplicataMercantil:
                        this.Codigo =getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.TriplicataMercantil);
                        this.Especie = "TRIPLICATA MERCANTIL";
                        this.Sigla = "TM";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.TriplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.TriplicataServico);
                        this.Especie = "TRIPLICATA DE SERVIÇO";
                        this.Sigla = "TS";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.NotaSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaSeguro);
                        this.Especie = "NOTA DE SEGURO";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.Recibo:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Recibo);
                        this.Especie = "RECIBO";
                        this.Sigla = "RC";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.Fatura:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Fatura);
                        this.Especie = "FATURA";
                        this.Sigla = "FAT";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.NotaDebito:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaDebito);
                        this.Especie = "NOTA DE DÉBITO";
                        this.Sigla = "ND";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.ApoliceSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.ApoliceSeguro);
                        this.Especie = "APÓLICE DE SEGURO";
                        this.Sigla = "AP";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.MensalidadeEscolar:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.MensalidadeEscolar);
                        this.Especie = "MENSALIDADE ESCOLAR";
                        this.Sigla = "ME";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.ParcelaConsorcio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.ParcelaConsorcio);
                        this.Especie = "PARCELA DE CONSÓRCIO";
                        this.Sigla = "PC";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.Outros:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Outros);
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
            try
            {
                EspeciesDocumento alEspeciesDocumento = new EspeciesDocumento();
                EspecieDocumento_BancoBrasil ed = new EspecieDocumento_BancoBrasil();

                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Cheque)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataMercantil)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataMercantilIndicacao)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataServico)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataServicoIndicacao)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataRural)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.LetraCambio)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaCreditoComercial)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaCreditoExportacao)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaCreditoIndustrial)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaCreditoRural)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaPromissoria)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaPromissoriaRural)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.TriplicataMercantil)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.TriplicataServico)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaSeguro)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Recibo)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Fatura)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaDebito)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.ApoliceSeguro)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.MensalidadeEscolar)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.ParcelaConsorcio)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Outros)));

                return alEspeciesDocumento;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar objetos", ex);
            }
        }

        #endregion
    }
}
