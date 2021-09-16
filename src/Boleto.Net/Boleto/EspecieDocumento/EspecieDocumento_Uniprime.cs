using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_Uniprime
    {
        DuplicataMercantil = 1,
        NotaPromissoria = 2,
        NotaSeguro = 3,
        CobrancaSeriada = 4,
        Recibo = 5,
        LetraCambio = 10,
        NotaDebito = 11,
        DuplicataServico = 12,
        BoletoProposta = 30,
        Outros = 99
    }

    #endregion

    public class EspecieDocumento_Uniprime : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Uniprime()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Uniprime(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Uniprime especie)
        {
            return Convert.ToInt32(especie).ToString("00");
        }

        public EnumEspecieDocumento_Uniprime getEnumEspecieByCodigo(string codigo)
        {
            return (EnumEspecieDocumento_Uniprime) Convert.ToInt32(codigo);
        }

        public override string getCodigoEspecieBySigla(string sigla)
        {
            switch (sigla)
            {
                case "DM": return "01";
                case "NP": return "02";
                case "NS": return "03";
                case "CS": return "04";
                case "RC": return "05";
                case "LC": return "10";
                case "ND": return "11";
                case "DS": return "12";
                case "BP": return "30";

                default: return "99";
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Uniprime();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Uniprime.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Uniprime.DuplicataMercantil);
                        this.Especie = "Duplicata mercantil";
                        this.Sigla = "DM";
                        break;

                    case EnumEspecieDocumento_Uniprime.NotaPromissoria:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Uniprime.NotaPromissoria);
                        this.Especie = "Nota promissória";
                        this.Sigla = "NP";
                        break;

                    case EnumEspecieDocumento_Uniprime.NotaSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Uniprime.NotaSeguro);
                        this.Especie = "Nota de seguro";
                        this.Sigla = "NS";
                        break;

                    case EnumEspecieDocumento_Uniprime.Recibo:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Uniprime.Recibo);
                        this.Especie = "Recibo";
                        this.Sigla = "RC";
                        break;

                    case EnumEspecieDocumento_Uniprime.LetraCambio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Uniprime.LetraCambio);
                        this.Sigla = "LC";
                        this.Especie = "Letra de Câmbio";
                        break;

                    case EnumEspecieDocumento_Uniprime.DuplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Uniprime.DuplicataServico);
                        this.Sigla = "DS";
                        this.Especie = "Duplicata de serviço";
                        break;

                    case EnumEspecieDocumento_Uniprime.NotaDebito:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Uniprime.NotaDebito);
                        this.Sigla = "ND";
                        this.Especie = "Nota de débito";
                        break;

                    case EnumEspecieDocumento_Uniprime.Outros:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Uniprime.Outros);
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

                var obj = new EspecieDocumento_Uniprime();

                foreach (var item in Enum.GetValues(typeof (EnumEspecieDocumento_Uniprime)))
                {
                    obj = new EspecieDocumento_Uniprime(obj.getCodigoEspecieByEnum((EnumEspecieDocumento_Uniprime)item));
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
            return new EspecieDocumento_Uniprime(getCodigoEspecieByEnum(EnumEspecieDocumento_Uniprime.DuplicataMercantil));
        }

        #endregion
    }
}