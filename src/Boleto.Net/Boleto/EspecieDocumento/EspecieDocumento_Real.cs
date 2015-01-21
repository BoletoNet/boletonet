using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_Real
    {
        Cheque,
        DuplicataMercantil,
        DuplicataMercantilIndicacao,
        DuplicataServico,
        DuplicataServiçoIndicacao,
        DuplicataRural,
        LetraCambio,
        NotaCreditoComercial,
        NotaCreditoExportacao,
        NotaCreditoIndustrial,
        NotaCreditoRural,
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
        IdentificacaoTituloAceitoNaoAceito,
    }

    #endregion

    public class EspecieDocumento_Real : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Real()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Real(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Real especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Real.Cheque:return"1";
                case EnumEspecieDocumento_Real.DuplicataMercantil:return"2";
                case EnumEspecieDocumento_Real.DuplicataMercantilIndicacao:return"3";
                case EnumEspecieDocumento_Real.DuplicataServico:return"4";
                case EnumEspecieDocumento_Real.DuplicataServiçoIndicacao:return"5";
                case EnumEspecieDocumento_Real.DuplicataRural:return"6";
                case EnumEspecieDocumento_Real.LetraCambio:return"7";
                case EnumEspecieDocumento_Real.NotaCreditoComercial:return"8";
                case EnumEspecieDocumento_Real.NotaCreditoExportacao:return"9";
                case EnumEspecieDocumento_Real.NotaCreditoIndustrial :return"10";
                case EnumEspecieDocumento_Real.NotaCreditoRural :return"11";
                case EnumEspecieDocumento_Real.NotaPromissoria:return"12";
                case EnumEspecieDocumento_Real.NotaPromissoriaRural:return"13";
                case EnumEspecieDocumento_Real.TriplicataMercantil :return"14";
                case EnumEspecieDocumento_Real.TriplicataServico:return"15";
                case EnumEspecieDocumento_Real.NotaSeguro :return"16";
                case EnumEspecieDocumento_Real.Recibo :return"17";
                case EnumEspecieDocumento_Real.Fatura :return"18";
                case EnumEspecieDocumento_Real.NotaDebito :return"19";
                case EnumEspecieDocumento_Real.ApoliceSeguro :return"20";
                case EnumEspecieDocumento_Real.MensalidadeEscolar :return"21";
                case EnumEspecieDocumento_Real.ParcelaConsorcio:return"22";
                case EnumEspecieDocumento_Real.IdentificacaoTituloAceitoNaoAceito: return "23";
                default: return "2";

            }
        }

        public EnumEspecieDocumento_Real getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1": return EnumEspecieDocumento_Real.Cheque;
                case "2": return EnumEspecieDocumento_Real.DuplicataMercantil;
                case "3": return EnumEspecieDocumento_Real.DuplicataMercantilIndicacao;
                case "4": return EnumEspecieDocumento_Real.DuplicataServico;
                case "5": return EnumEspecieDocumento_Real.DuplicataServiçoIndicacao;
                case "6": return EnumEspecieDocumento_Real.DuplicataRural;
                case "7": return EnumEspecieDocumento_Real.LetraCambio;
                case "8": return EnumEspecieDocumento_Real.NotaCreditoComercial;
                case "9": return EnumEspecieDocumento_Real.NotaCreditoExportacao;
                case "10": return EnumEspecieDocumento_Real.NotaCreditoIndustrial;
                case "11": return EnumEspecieDocumento_Real.NotaCreditoRural;
                case "12": return EnumEspecieDocumento_Real.NotaPromissoria;
                case "13": return EnumEspecieDocumento_Real.NotaPromissoriaRural;
                case "14": return EnumEspecieDocumento_Real.TriplicataMercantil;
                case "15": return EnumEspecieDocumento_Real.TriplicataServico;
                case "16": return EnumEspecieDocumento_Real.NotaSeguro;
                case "17": return EnumEspecieDocumento_Real.Recibo;
                case "18": return EnumEspecieDocumento_Real.Fatura;
                case "19": return EnumEspecieDocumento_Real.NotaDebito;
                case "20": return EnumEspecieDocumento_Real.ApoliceSeguro;
                case "21": return EnumEspecieDocumento_Real.MensalidadeEscolar;
                case "22": return EnumEspecieDocumento_Real.ParcelaConsorcio;
                case "23": return EnumEspecieDocumento_Real.IdentificacaoTituloAceitoNaoAceito;

                default: return EnumEspecieDocumento_Real.DuplicataMercantil;
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Real();

                  switch (getEnumEspecieByCodigo(idCodigo))
                {
                   case EnumEspecieDocumento_Real.IdentificacaoTituloAceitoNaoAceito:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.IdentificacaoTituloAceitoNaoAceito);
                        this.Especie = "Identificação de Título Aceito / Não Aceito";
                        this.Sigla = "C016";
                        break;
                    case EnumEspecieDocumento_Real.ParcelaConsorcio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.ParcelaConsorcio);
                        this.Especie = "Parcela de Consórcio";
                        this.Sigla = "PC";
                        break;
                    case EnumEspecieDocumento_Real.MensalidadeEscolar:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.MensalidadeEscolar);
                        this.Especie = "Mensalidade Escolar";
                        this.Sigla = "ME";
                        break;
                    case EnumEspecieDocumento_Real.ApoliceSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.ApoliceSeguro);
                        this.Especie = "Apólice de Seguro";
                        this.Sigla = "AP";
                        break;
                    case EnumEspecieDocumento_Real.NotaDebito:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.NotaDebito);
                        this.Especie = "Nota de Débito";
                        this.Sigla = "ND";
                        break;
                    case EnumEspecieDocumento_Real.Fatura:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.Fatura);
                        this.Especie = "Fatura";
                        this.Sigla = "FAT";
                        break;
                    case EnumEspecieDocumento_Real.NotaSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.NotaSeguro);
                        this.Especie = "Nota de Seguro";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Real.TriplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.TriplicataMercantil);
                        this.Especie = "Triplicata Mercantil";
                        this.Sigla = "TM";
                        break;
                    case EnumEspecieDocumento_Real.TriplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.TriplicataServico);
                        this.Especie = "Triplicata de Serviço";
                        this.Sigla = "TS";
                        break;
                    case EnumEspecieDocumento_Real.NotaPromissoriaRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.NotaPromissoriaRural);
                        this.Especie = "Nota Promissória Rural";
                        this.Sigla = "NPR";
                        break;
                    case EnumEspecieDocumento_Real.NotaCreditoRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.NotaCreditoRural);
                        this.Especie = "Nota de Crédito Rural";
                        this.Sigla = "NCR";
                        break;
                    case EnumEspecieDocumento_Real.NotaCreditoExportacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.NotaCreditoExportacao);
                        this.Especie = "Nota de Crédito a Exportação";
                        this.Sigla = "NCE";
                        break;
                    case EnumEspecieDocumento_Real.NotaCreditoIndustrial:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.NotaCreditoIndustrial);
                        this.Especie = "Nota de Crédito Industrial";
                        this.Sigla = "NCI";
                        break;
                    case EnumEspecieDocumento_Real.NotaCreditoComercial:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.NotaCreditoComercial);
                        this.Especie = "Nota de Crédito Comercial";
                        this.Sigla = "NCC";
                        break;
                    case EnumEspecieDocumento_Real.Cheque:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.Cheque);
                        this.Especie = "Cheque";
                        this.Sigla = "CH";
                        break;
                    case EnumEspecieDocumento_Real.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.DuplicataMercantil);
                        this.Especie = "Duplicata mercantil";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Real.DuplicataMercantilIndicacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.DuplicataMercantilIndicacao);
                        this.Especie = "Duplicata Mercantil p/ Indicação";
                        this.Sigla = "DMI";
                        break;
                    case EnumEspecieDocumento_Real.DuplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.DuplicataServico);
                        this.Especie = "Duplicata Serviço";
                        this.Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_Real.DuplicataServiçoIndicacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.DuplicataServiçoIndicacao);
                        this.Especie = "Duplicata Serviços p/ Indicação";
                        this.Sigla = "DSI";
                        break;
                    case EnumEspecieDocumento_Real.DuplicataRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.DuplicataRural);
                        this.Especie = "Duplicata Rural";
                        this.Sigla = "DR";
                        break;
                    case EnumEspecieDocumento_Real.NotaPromissoria:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.NotaPromissoria);
                        this.Especie = "Nota promissória";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_Real.Recibo:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.Recibo);
                        this.Especie = "Recibo";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Real.LetraCambio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Real.LetraCambio);
                        this.Sigla = "LC";
                        this.Especie = "Letra de câmbio";
                        break;
                    default:
                        this.Codigo = "0";
                        this.Especie = "";
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
            EspeciesDocumento especiesDocumento = new EspeciesDocumento();
            EspecieDocumento_Real ed = new EspecieDocumento_Real();

            foreach (EnumEspecieDocumento_Real item in Enum.GetValues(typeof(EnumEspecieDocumento_Real)))
                especiesDocumento.Add(new EspecieDocumento_Real(ed.getCodigoEspecieByEnum(item)));

            return especiesDocumento;
        }

        #endregion
    }
}
