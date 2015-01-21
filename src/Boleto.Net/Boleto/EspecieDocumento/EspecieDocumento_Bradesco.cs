using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_Bradesco
    {
        DuplicataMercantil,
        NotaPromissoria,
        NotaSeguro,
        CobrancaSeriada,
        Recibo,
        LetraCambio,
        NotaDebito,
        DuplicataServico,
        Outros,
    }

    #endregion

    public class EspecieDocumento_Bradesco : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Bradesco()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Bradesco(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Bradesco.DuplicataMercantil: return "1";
                case EnumEspecieDocumento_Bradesco.NotaPromissoria: return "2";
                case EnumEspecieDocumento_Bradesco.NotaSeguro: return "3";
                case EnumEspecieDocumento_Bradesco.CobrancaSeriada: return "4";
                case EnumEspecieDocumento_Bradesco.Recibo: return "5";
                case EnumEspecieDocumento_Bradesco.LetraCambio: return "10";
                case EnumEspecieDocumento_Bradesco.NotaDebito: return "11";
                case EnumEspecieDocumento_Bradesco.DuplicataServico: return "12";
                case EnumEspecieDocumento_Bradesco.Outros: return "99";
                default: return "99";

            }
        }

        public EnumEspecieDocumento_Bradesco getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1": return EnumEspecieDocumento_Bradesco.DuplicataMercantil;
                case "2": return EnumEspecieDocumento_Bradesco.NotaPromissoria;
                case "3": return EnumEspecieDocumento_Bradesco.NotaSeguro;
                case "4": return EnumEspecieDocumento_Bradesco.CobrancaSeriada;
                case "5": return EnumEspecieDocumento_Bradesco.Recibo;
                case "10": return EnumEspecieDocumento_Bradesco.LetraCambio;
                case "11": return EnumEspecieDocumento_Bradesco.NotaDebito;
                case "12": return EnumEspecieDocumento_Bradesco.DuplicataServico;
                case "99": return EnumEspecieDocumento_Bradesco.Outros;
                default: return EnumEspecieDocumento_Bradesco.Outros;
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Bradesco();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Bradesco.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.DuplicataMercantil);
                        this.Especie = "Duplicata mercantil";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Bradesco.NotaPromissoria:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.NotaPromissoria);
                        this.Especie = "Nota promissória";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_Bradesco.NotaSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.NotaSeguro);
                        this.Especie = "Nota de seguro";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Bradesco.CobrancaSeriada:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.CobrancaSeriada);
                        this.Especie = "Cobrança seriada";
                        this.Sigla = "CS";
                        break;
                    case EnumEspecieDocumento_Bradesco.Recibo:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.Recibo);
                        this.Especie = "Recibo";
                        this.Sigla = "RC";
                        break;
                    case EnumEspecieDocumento_Bradesco.LetraCambio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.LetraCambio);
                        this.Sigla = "LC";
                        this.Especie = "Letra de câmbio";
                        break;
                    case EnumEspecieDocumento_Bradesco.NotaDebito:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.NotaDebito);
                        this.Sigla = "ND";
                        this.Especie = "Nota de débito";
                        break;
                    case EnumEspecieDocumento_Bradesco.DuplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.DuplicataServico);
                        this.Sigla = "DS";
                        this.Especie = "Duplicata de serviço";
                        break;
                    case EnumEspecieDocumento_Bradesco.Outros:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.Outros);
                        this.Especie = "Outros";
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
                EspeciesDocumento alEspeciesDocumento = new EspeciesDocumento();

                EspecieDocumento_Bradesco obj = new EspecieDocumento_Bradesco();

                obj = new EspecieDocumento_Bradesco(obj.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.DuplicataMercantil));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Bradesco(obj.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.NotaPromissoria));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Bradesco(obj.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.NotaSeguro));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Bradesco(obj.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.CobrancaSeriada));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Bradesco(obj.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.Recibo));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Bradesco(obj.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.LetraCambio));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Bradesco(obj.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.NotaDebito));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Bradesco(obj.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.DuplicataServico));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Bradesco(obj.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.Outros));
                alEspeciesDocumento.Add(obj);

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