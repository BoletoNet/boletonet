using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_BankBoston
    {
        DuplicataMercantil,
        DuplicataServico,
        NotaSeguro,
        ReciboEscolar,
        ReciboAssociacao,
        ReciboCondominio,
        NotaDebito,
        Outros,
    }

    #endregion 

    public class EspecieDocumento_BankBoston: AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores 

		public EspecieDocumento_BankBoston()
		{
			try
			{
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

        public EspecieDocumento_BankBoston(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_BankBoston especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_BankBoston.DuplicataMercantil: return "1";
                case EnumEspecieDocumento_BankBoston.DuplicataServico: return "2";
                case EnumEspecieDocumento_BankBoston.NotaSeguro: return "3";
                case EnumEspecieDocumento_BankBoston.ReciboEscolar: return "4";
                case EnumEspecieDocumento_BankBoston.ReciboAssociacao: return "5";
                case EnumEspecieDocumento_BankBoston.ReciboCondominio: return "6";
                case EnumEspecieDocumento_BankBoston.NotaDebito: return "7";
                case EnumEspecieDocumento_BankBoston.Outros: return "8";
                default: return"0";
                
            }
        }

        public EnumEspecieDocumento_BankBoston getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1": return EnumEspecieDocumento_BankBoston.DuplicataMercantil;
                case "2": return EnumEspecieDocumento_BankBoston.DuplicataServico;
                case "3": return EnumEspecieDocumento_BankBoston.NotaSeguro;
                case "4": return EnumEspecieDocumento_BankBoston.ReciboEscolar;
                case "5": return EnumEspecieDocumento_BankBoston.ReciboAssociacao;
                case "6": return EnumEspecieDocumento_BankBoston.ReciboCondominio;
                case "7": return EnumEspecieDocumento_BankBoston.NotaDebito;
                case "8": return EnumEspecieDocumento_BankBoston.Outros;
                default: return EnumEspecieDocumento_BankBoston.Outros;
            }
        }

        private void carregar(string Codigo)
        {
            try
            {
                this.Banco = new Banco_BankBoston();

                switch (getEnumEspecieByCodigo(Codigo))
                {
                    case  EnumEspecieDocumento_BankBoston.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BankBoston.DuplicataMercantil);
                        this.Sigla = "DM";
                        this.Especie = "Duplicata mercantil";
                        break;
                    case EnumEspecieDocumento_BankBoston.DuplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BankBoston.DuplicataServico);
                        this.Sigla = "DS";
                        this.Especie = "Duplicata de serviço";
                        break;
                    case EnumEspecieDocumento_BankBoston.NotaSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BankBoston.NotaSeguro);
                        this.Sigla = "NS";
                        this.Especie = "Nota de seguro";
                        break;
                    case EnumEspecieDocumento_BankBoston.ReciboEscolar:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BankBoston.ReciboEscolar);
                        this.Sigla = "RE";
                        this.Especie = "Reciboescolar";
                        break;
                    case EnumEspecieDocumento_BankBoston.ReciboAssociacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BankBoston.ReciboAssociacao);
                        this.Sigla = "RS";
                        this.Especie = "Recibo Associação";
                        break;
                    case EnumEspecieDocumento_BankBoston.ReciboCondominio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BankBoston.ReciboCondominio);
                        this.Sigla = "RC";
                        this.Especie = "Contrato";
                        break;
                    case EnumEspecieDocumento_BankBoston.NotaDebito:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BankBoston.NotaDebito);
                        this.Sigla = "ND";
                        this.Especie = "Nota débito";
                        break;
                    case EnumEspecieDocumento_BankBoston.Outros:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BankBoston.Outros);
                        this.Sigla = "OT";
                        this.Especie = "Outros";
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
            EspecieDocumento_BankBoston ed = new EspecieDocumento_BankBoston();
            foreach (EnumEspecieDocumento_BankBoston item in Enum.GetValues(typeof(EnumEspecieDocumento_BankBoston)))
                especiesDocumento.Add(new EspecieDocumento_BankBoston(ed.getCodigoEspecieByEnum(item)));

            return especiesDocumento;
        }

        #endregion
    }
}
