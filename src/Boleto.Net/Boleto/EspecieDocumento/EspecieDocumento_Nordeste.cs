using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_Nordeste
    {
        DuplicataMercantil = 1, //DM – DUPLICATA MERCANTIL
        NotaPromissoria = 2, //NP – NOTA PROMISSÓRIA
        Cheque = 3, //CH – CHEQUE
        Carne = 4, // – Carne
        Recibo = 5, //RC – RECIBO
        DuplicataServico = 6, //DS –  DUPLICATA DE SERVIÇO
        Outros = 19 //OUTROS
    }

    #endregion

    public class EspecieDocumento_Nordeste : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Nordeste()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Nordeste(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Nordeste especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Nordeste.Cheque: return "03";
                case EnumEspecieDocumento_Nordeste.DuplicataMercantil: return "01";
                case EnumEspecieDocumento_Nordeste.DuplicataServico: return "06";
                case EnumEspecieDocumento_Nordeste.NotaPromissoria: return "02";
                case EnumEspecieDocumento_Nordeste.Recibo: return "05";
                case EnumEspecieDocumento_Nordeste.Outros: return "19";
                case EnumEspecieDocumento_Nordeste.Carne: return "04";
                default: return "01";

            }
        }

        public EnumEspecieDocumento_Nordeste getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "01": case "1": return EnumEspecieDocumento_Nordeste.DuplicataMercantil;
                case "02": case "2": return EnumEspecieDocumento_Nordeste.NotaPromissoria;
                case "03": case "3": return EnumEspecieDocumento_Nordeste.Cheque;                
                case "04": case "4": return EnumEspecieDocumento_Nordeste.Carne;
                case "05": case "5": return EnumEspecieDocumento_Nordeste.Recibo;
                case "06": case "6": return EnumEspecieDocumento_Nordeste.DuplicataServico;
                case "19": return EnumEspecieDocumento_Nordeste.Outros;
                default: return EnumEspecieDocumento_Nordeste.DuplicataMercantil;
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Nordeste();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Nordeste.Cheque:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Nordeste.Cheque);
                        this.Especie = "CHEQUE";
                        this.Sigla = "CH";
                        break;
                    case EnumEspecieDocumento_Nordeste.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Nordeste.DuplicataMercantil);
                        this.Especie = "DUPLICATA MERCANTIL";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Nordeste.DuplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Nordeste.DuplicataServico);
                        this.Especie = "DUPLICATA DE SERVIÇO";
                        this.Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_Nordeste.NotaPromissoria:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Nordeste.NotaPromissoria);
                        this.Especie = "NOTA PROMISSÓRIA";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_Nordeste.Recibo:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Nordeste.Recibo);
                        this.Especie = "RECIBO";
                        this.Sigla = "RC";
                        break;                    
                    case EnumEspecieDocumento_Nordeste.Outros:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Nordeste.Outros);
                        this.Especie = "OUTROS";
                        this.Sigla = "OUTROS";
                        break;
                    case EnumEspecieDocumento_Nordeste.Carne:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Nordeste.Carne);
                        this.Especie = "CARNE";
                        this.Sigla = "";
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
                EspecieDocumento_Nordeste ed = new EspecieDocumento_Nordeste();
                alEspeciesDocumento.Add(new EspecieDocumento_Nordeste(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Nordeste.Cheque)));
                alEspeciesDocumento.Add(new EspecieDocumento_Nordeste(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Nordeste.DuplicataMercantil)));
                alEspeciesDocumento.Add(new EspecieDocumento_Nordeste(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Nordeste.NotaPromissoria)));
                alEspeciesDocumento.Add(new EspecieDocumento_Nordeste(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Nordeste.DuplicataServico)));
                alEspeciesDocumento.Add(new EspecieDocumento_Nordeste(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Nordeste.Recibo)));
                alEspeciesDocumento.Add(new EspecieDocumento_Nordeste(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Nordeste.Outros)));
                alEspeciesDocumento.Add(new EspecieDocumento_Nordeste(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Nordeste.Carne)));

                return alEspeciesDocumento;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar objetos", ex);
            }
        }

        public override IEspecieDocumento DuplicataMercantil()
        {
            return new EspecieDocumento_Nordeste(getCodigoEspecieByEnum(EnumEspecieDocumento_Nordeste.DuplicataMercantil));
        }

        #endregion
    }
}
