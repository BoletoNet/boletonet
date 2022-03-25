using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerador

    public enum EnumEspecieDocumento_Safra
    {
        Cheque,
        DuplicataMercantil,
        DuplicataMercantilIndicacao,
        DuplicataServico,
        DuplicataServicoIndicacao,
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
        Outros,
    }

    #endregion

    public class EspecieDocumento_Safra : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Safra()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Safra(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Safra especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Safra.DuplicataMercantil: return "1";
                case EnumEspecieDocumento_Safra.NotaPromissoria: return "2";
                case EnumEspecieDocumento_Safra.NotaSeguro: return "3";
                case EnumEspecieDocumento_Safra.Recibo: return "5";
                case EnumEspecieDocumento_Safra.DuplicataServico: return "9";

                default: return "1";

            }
        }

        public EnumEspecieDocumento_Safra getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1": return EnumEspecieDocumento_Safra.DuplicataMercantil;
                case "2": return EnumEspecieDocumento_Safra.NotaPromissoria;
                case "3": return EnumEspecieDocumento_Safra.NotaSeguro;
                case "5": return EnumEspecieDocumento_Safra.Recibo;
                case "9": return EnumEspecieDocumento_Safra.DuplicataServico;
                default: return EnumEspecieDocumento_Safra.DuplicataMercantil;
            }
        }

        public override string getCodigoEspecieBySigla(string sigla)
        {
            switch (sigla)
            {
              
                case "DM": return "1";
                case "NP": return "2";
                case "NS": return "2";
                case "REC": return "3";
                case "DS": return "9";
                
             
                default: return "1";
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Safra();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    
                    case EnumEspecieDocumento_Safra.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.DuplicataMercantil);
                        this.Especie = "DUPLICATA MERCANTIL";
                        this.Sigla = "DM";
                        break;
                 
                    case EnumEspecieDocumento_Safra.DuplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.DuplicataServico);
                        this.Especie = "DUPLICATA DE SERVIÇO";
                        this.Sigla = "DS";
                        break;
            

                    case EnumEspecieDocumento_Safra.NotaSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.NotaSeguro);
                        this.Especie = "NOTA DE SEGURO";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Safra.Recibo:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.Recibo);
                        this.Especie = "RECIBO";
                        this.Sigla = "RC";
                        break;
                       
                    case EnumEspecieDocumento_Safra.Outros:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.Outros);
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
            EspeciesDocumento especiesDocumento = new EspeciesDocumento();
            EspecieDocumento_Safra ed = new EspecieDocumento_Safra();

            foreach (EnumEspecieDocumento_Safra item in Enum.GetValues(typeof(EnumEspecieDocumento_Safra)))
                especiesDocumento.Add(new EspecieDocumento_Safra(ed.getCodigoEspecieByEnum(item)));

            return especiesDocumento;
        }

        public override IEspecieDocumento DuplicataMercantil()
        {
            return new EspecieDocumento_Safra(getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.DuplicataMercantil));
        }

        #endregion
    }
}
