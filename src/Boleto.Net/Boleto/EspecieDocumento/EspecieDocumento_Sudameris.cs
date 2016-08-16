using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_Sudameris
    {
        DuplicataMercantil,
        NotaPromissoria,
        NotaSeguro,
        MensalidadeEscolar,
        Recibo,
        Contrato,
        Cosseguros,
        DuplicataServico,
        LetraCambio,
        NotaDebito,
        DocumentoDivida,
        EncargosCondominais,
        Diversos,
    }

    #endregion 

    public class EspecieDocumento_Sudameris: AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores 

		public EspecieDocumento_Sudameris()
		{
			try
			{
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

        public EspecieDocumento_Sudameris(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Sudameris especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Sudameris.DuplicataMercantil: return "1";
                case EnumEspecieDocumento_Sudameris.NotaPromissoria: return "2";
                case EnumEspecieDocumento_Sudameris.NotaSeguro: return "3";
                case EnumEspecieDocumento_Sudameris.MensalidadeEscolar: return "4";
                case EnumEspecieDocumento_Sudameris.Recibo: return "5";
                case EnumEspecieDocumento_Sudameris.Contrato: return "6";
                case EnumEspecieDocumento_Sudameris.Cosseguros: return "7";
                case EnumEspecieDocumento_Sudameris.DuplicataServico: return "8";
                case EnumEspecieDocumento_Sudameris.LetraCambio: return "9";
                case EnumEspecieDocumento_Sudameris.NotaDebito: return "13";
                case EnumEspecieDocumento_Sudameris.DocumentoDivida: return "15";
                case EnumEspecieDocumento_Sudameris.EncargosCondominais: return "16";
                case EnumEspecieDocumento_Sudameris.Diversos: return "99";
                default: return "99";

            }
        }

        public EnumEspecieDocumento_Sudameris getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1": return EnumEspecieDocumento_Sudameris.DuplicataMercantil;
                case "2": return EnumEspecieDocumento_Sudameris.NotaPromissoria;
                case "3": return EnumEspecieDocumento_Sudameris.NotaSeguro;
                case "4": return EnumEspecieDocumento_Sudameris.MensalidadeEscolar;
                case "5": return EnumEspecieDocumento_Sudameris.Recibo;
                case "6": return EnumEspecieDocumento_Sudameris.Contrato;
                case "7": return EnumEspecieDocumento_Sudameris.Cosseguros;
                case "8": return EnumEspecieDocumento_Sudameris.DuplicataServico;
                case "9": return EnumEspecieDocumento_Sudameris.LetraCambio;
                case "13":return EnumEspecieDocumento_Sudameris.NotaDebito;
                case "15":return EnumEspecieDocumento_Sudameris.DocumentoDivida;
                case "16":return EnumEspecieDocumento_Sudameris.EncargosCondominais;
                case "99": return EnumEspecieDocumento_Sudameris.Diversos;
                default: return EnumEspecieDocumento_Sudameris.Diversos;
            }
        }
        
        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Sudameris();
                EspecieDocumento_Sudameris ed = new EspecieDocumento_Sudameris();

                switch (ed.getEnumEspecieByCodigo(idCodigo))
                {
                    case  EnumEspecieDocumento_Sudameris.DuplicataMercantil:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sudameris.DuplicataMercantil);
                        this.Especie = "Duplicata mercantil";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Sudameris.NotaPromissoria:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sudameris.NotaPromissoria);
                        this.Especie = "Nota promissória";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_Sudameris.NotaSeguro:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sudameris.NotaSeguro);
                        this.Especie = "Nota de seguro";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Sudameris.MensalidadeEscolar:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sudameris.MensalidadeEscolar);
                        this.Especie = "Mensalidade escolar";
                        this.Sigla = "ME";
                        break;
                    case EnumEspecieDocumento_Sudameris.Recibo:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sudameris.Recibo);
                        this.Especie = "Recibo";
                        this.Sigla = "R";
                        break;
                    case EnumEspecieDocumento_Sudameris.Contrato:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sudameris.Contrato);
                        this.Especie = "Contrato";
                        this.Sigla = "C";
                        break;
                    case EnumEspecieDocumento_Sudameris.Cosseguros:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sudameris.Cosseguros);
                        this.Especie = "Cosseguros";
                        this.Sigla = "CS";
                        break;
                    case EnumEspecieDocumento_Sudameris.DuplicataServico:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sudameris.DuplicataServico);
                        this.Especie = "Duplicata de serviço";
                        this.Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_Sudameris.LetraCambio:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sudameris.LetraCambio);
                        this.Especie = "Letra de câmbio";
                        this.Sigla = "LC";
                        break;
                    case EnumEspecieDocumento_Sudameris.NotaDebito:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sudameris.NotaDebito);
                        this.Especie = "Nota de débito";
                        this.Sigla = "ND";
                        break;
                    case EnumEspecieDocumento_Sudameris.DocumentoDivida:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sudameris.DocumentoDivida);
                        this.Especie = "Documento de dívida";
                        this.Sigla = "DD";
                        break;
                    case EnumEspecieDocumento_Sudameris.EncargosCondominais:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sudameris.EncargosCondominais);
                        this.Especie = "Encargos condominais";
                        this.Sigla = "EC";
                        break;
                    case EnumEspecieDocumento_Sudameris.Diversos:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sudameris.Diversos);
                        this.Especie = "Diversos";
                        this.Sigla = "D";
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
            EspecieDocumento_Sudameris ed = new EspecieDocumento_Sudameris();
            foreach (EnumEspecieDocumento_Sudameris item in Enum.GetValues(typeof(EnumEspecieDocumento_Sudameris)))
                especiesDocumento.Add(new EspecieDocumento_Sudameris(ed.getCodigoEspecieByEnum(item)));

            return especiesDocumento;
        }

        #endregion
    }
}
