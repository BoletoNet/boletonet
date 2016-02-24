using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_Itau
    {
        DuplicataMercantil = 1,
        NotaPromissoria = 2,
        NotaSeguro = 3,
        MensalidadeEscolar = 4,
        Recibo = 5,
        Contrato = 6,
        Cosseguros = 7,
        DuplicataServico = 8,
        LetraCambio = 9,
        NotaDebito = 13,
        DocumentoDivida = 15,
        EncargosCondominais = 16,
        Diversos = 99,
    }

    #endregion

    public class EspecieDocumento_Itau : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Itau()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Itau(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Itau especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Itau.DuplicataMercantil: return "1";
                case EnumEspecieDocumento_Itau.NotaPromissoria: return "2";
                case EnumEspecieDocumento_Itau.NotaSeguro: return "3";
                case EnumEspecieDocumento_Itau.MensalidadeEscolar: return "4";
                case EnumEspecieDocumento_Itau.Recibo: return "5";
                case EnumEspecieDocumento_Itau.Contrato: return "6";
                case EnumEspecieDocumento_Itau.Cosseguros: return "7";
                case EnumEspecieDocumento_Itau.DuplicataServico: return "8";
                case EnumEspecieDocumento_Itau.LetraCambio: return "9";
                case EnumEspecieDocumento_Itau.NotaDebito: return "13";
                case EnumEspecieDocumento_Itau.DocumentoDivida: return "15";
                case EnumEspecieDocumento_Itau.EncargosCondominais: return "16";
                case EnumEspecieDocumento_Itau.Diversos: return "99";
                default: return "99";

            }
        }

        public EnumEspecieDocumento_Itau getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1":return EnumEspecieDocumento_Itau.DuplicataMercantil;
                case "2":return EnumEspecieDocumento_Itau.NotaPromissoria;
                case "3":return EnumEspecieDocumento_Itau.NotaSeguro;
                case "4":return EnumEspecieDocumento_Itau.MensalidadeEscolar;
                case "5":return EnumEspecieDocumento_Itau.Recibo;
                case "6":return EnumEspecieDocumento_Itau.Contrato;
                case "7":return EnumEspecieDocumento_Itau.Cosseguros;
                case "8":return EnumEspecieDocumento_Itau.DuplicataServico;
                case "9":return EnumEspecieDocumento_Itau.LetraCambio;
                case "13":return EnumEspecieDocumento_Itau.NotaDebito;
                case "15":return EnumEspecieDocumento_Itau.DocumentoDivida;
                case "16":return EnumEspecieDocumento_Itau.EncargosCondominais;
                case "99": return EnumEspecieDocumento_Itau.Diversos;
                default: return EnumEspecieDocumento_Itau.Diversos;
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Itau();
                EspecieDocumento_Itau ed = new EspecieDocumento_Itau();
                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Itau.DuplicataMercantil:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Itau.DuplicataMercantil);
                        this.Especie = "Duplicata mercantil";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Itau.NotaPromissoria:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Itau.NotaPromissoria);
                        this.Especie = "Nota promissória";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_Itau.NotaSeguro:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Itau.NotaSeguro);
                        this.Especie = "Nota de seguro";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Itau.MensalidadeEscolar:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Itau.MensalidadeEscolar);
                        this.Especie = "Mensalidade escolar";
                        this.Sigla = "ME";
                        break;
                    case EnumEspecieDocumento_Itau.Recibo:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Itau.Recibo);
                        this.Especie = "Recibo";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Itau.Contrato:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Itau.Contrato);
                        this.Sigla = "C";
                        this.Especie = "Contrato";
                        break;
                    case EnumEspecieDocumento_Itau.Cosseguros:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Itau.Cosseguros);
                        this.Sigla = "CS";
                        this.Especie = "Cosseguros";
                        break;
                    case EnumEspecieDocumento_Itau.DuplicataServico:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Itau.DuplicataServico);
                        this.Sigla = "DS";
                        this.Especie = "Duplicata de serviço";
                        break;
                    case EnumEspecieDocumento_Itau.LetraCambio:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Itau.LetraCambio);
                        this.Sigla = "LC";
                        this.Especie = "Letra de câmbio";
                        break;
                    case EnumEspecieDocumento_Itau.NotaDebito:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Itau.NotaDebito);
                        this.Sigla = "ND";
                        this.Especie = "Nota de débito";
                        break;
                    case EnumEspecieDocumento_Itau.DocumentoDivida:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Itau.DocumentoDivida);
                        this.Sigla = "DD";
                        this.Especie = "Documento de dívida";
                        break;
                    case EnumEspecieDocumento_Itau.EncargosCondominais:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Itau.EncargosCondominais);
                        this.Sigla = "EC";
                        this.Especie = "Encargos condominais";
                        break;
                    case EnumEspecieDocumento_Itau.Diversos:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Itau.Diversos);
                        this.Especie = "Diversos";
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
            EspeciesDocumento especiesDocumento = new EspeciesDocumento();
            EspecieDocumento_Itau ed = new EspecieDocumento_Itau();

            foreach (EnumEspecieDocumento_Itau item in Enum.GetValues(typeof(EnumEspecieDocumento_Itau)))
                especiesDocumento.Add(new EspecieDocumento_Itau(ed.getCodigoEspecieByEnum(item)));

            return especiesDocumento;
        }

        #endregion
    }
}
