using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_CrediSIS
    {
        DuplicataMercantilIndicacao = 3, //DMI – DUPLICATA MERCANTIL P/ INDICAÇÃO
        DuplicataServicoIndicacao = 5, //DSI –  DUPLICATA DE SERVIÇO P/ INDICAÇÃO
        NotaPromissoria = 12, //NP – NOTA PROMISSÓRIA
        Recibo = 17, //RC – RECIBO
        MensalidadeEscolar = 21, //ME – MENSALIDADE ESCOLAR
        NotaFiscal = 23 //Nota Fiscal
    }

    #endregion

    public class EspecieDocumento_CrediSIS : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_CrediSIS()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_CrediSIS(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_CrediSIS especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_CrediSIS.DuplicataMercantilIndicacao: return "3";
                case EnumEspecieDocumento_CrediSIS.DuplicataServicoIndicacao: return "5";
                case EnumEspecieDocumento_CrediSIS.NotaPromissoria: return "12";
                case EnumEspecieDocumento_CrediSIS.Recibo: return "17";
                case EnumEspecieDocumento_CrediSIS.MensalidadeEscolar: return "21";
                case EnumEspecieDocumento_CrediSIS.NotaFiscal: return "23";
                default: return "5";

            }
        }

        public EnumEspecieDocumento_CrediSIS getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "3": return EnumEspecieDocumento_CrediSIS.DuplicataMercantilIndicacao;
                case "5": return EnumEspecieDocumento_CrediSIS.DuplicataServicoIndicacao;
                case "12": return EnumEspecieDocumento_CrediSIS.NotaPromissoria;
                case "17": return EnumEspecieDocumento_CrediSIS.Recibo;
                case "21": return EnumEspecieDocumento_CrediSIS.MensalidadeEscolar;
                case "23": return EnumEspecieDocumento_CrediSIS.NotaFiscal;
                default: return EnumEspecieDocumento_CrediSIS.DuplicataServicoIndicacao;
            }
        }

        public override string getCodigoEspecieBySigla(string sigla)
        {
            switch (sigla)
            {
                case "DMI": return "3";
                case "DSI": return "5";
                case "NP": return "12";
                case "RC": return "17";
                case "ME": return "21";
                case "NF": return "23";
                default: return "5";
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_CrediSis();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_CrediSIS.DuplicataMercantilIndicacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_CrediSIS.DuplicataMercantilIndicacao);
                        this.Especie = "DUPLICATA MERCANTIL P/ INDICAÇÃO";
                        this.Sigla = "DMI";
                        break;
                    case EnumEspecieDocumento_CrediSIS.DuplicataServicoIndicacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_CrediSIS.DuplicataServicoIndicacao);
                        this.Especie = "DUPLICATA DE SERVIÇO P/ INDICAÇÃO";
                        this.Sigla = "DSI";
                        break;
                    case EnumEspecieDocumento_CrediSIS.NotaPromissoria:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_CrediSIS.NotaPromissoria);
                        this.Especie = "NOTA PROMISSÓRIA";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_CrediSIS.Recibo:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_CrediSIS.Recibo);
                        this.Especie = "RECIBO";
                        this.Sigla = "RC";
                        break;
                    case EnumEspecieDocumento_CrediSIS.MensalidadeEscolar:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_CrediSIS.MensalidadeEscolar);
                        this.Especie = "MENSALIDADE ESCOLAR";
                        this.Sigla = "ME";
                        break;
                    case EnumEspecieDocumento_CrediSIS.NotaFiscal:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_CrediSIS.NotaFiscal);
                        this.Especie = "Nota Fiscal";
                        this.Sigla = "NF";
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
                var alEspeciesDocumento = new EspeciesDocumento();

                var obj = new EspecieDocumento_CrediSIS();

                foreach (var item in Enum.GetValues(typeof(EnumEspecieDocumento_CrediSIS)))
                {
                    obj = new EspecieDocumento_CrediSIS(obj.getCodigoEspecieByEnum((EnumEspecieDocumento_CrediSIS)item));
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
            throw new NotImplementedException();
        }

        #endregion
    }
}
