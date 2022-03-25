using System;
using System.Linq;

namespace BoletoNet
{
    public enum EnumEspecieDocumento_C6
    {
        DuplicataMercantil = 1,
        DuplicataServico = 2,
        NotaPromissoria = 3,
        NotaSeguro = 4,
        Recibo = 5,
        LetraCambio = 6,
        FichaCompensacao = 7,
        Carne = 08,
        Contrato = 09,
        Cheque = 10,
        CobrancaSeriada = 11,
        MensalidadeEscolar = 12,
        NotaDebito = 13,
        DocumentoDivida = 15,
        EncargosCondominiais = 16,
        ContaPrestacaoServicos = 17,
        FaturaCartaoCredito = 31,
        Outros = 99
    }

    public class EspecieDocumento_C6 : AbstractEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_C6()
        {
        }

        public EspecieDocumento_C6(EnumEspecieDocumento_C6 especieDocumento)
        {
            try
            {
                this.Carregar(especieDocumento);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_C6(string codigo) : this ((EnumEspecieDocumento_C6) int.Parse(codigo))
        {
        }

        #endregion Construtores

        #region Metodos Privados

        public override string getCodigoEspecieBySigla(string sigla)
        {
            var especie = CarregaTodas().FirstOrDefault(c => c.Sigla == sigla) ?? new EspecieDocumento_C6(EnumEspecieDocumento_C6.DuplicataMercantil);
            return especie.Codigo; 
        }

        private void Carregar(EnumEspecieDocumento_C6 idCodigo)
        {
            try
            {
                this.Banco = new Banco_C6();
                this.Codigo = ((int) idCodigo).ToString().PadLeft(2, '0');

                switch (idCodigo)
                {
                    case EnumEspecieDocumento_C6.DuplicataMercantil:
                        this.Especie = "Duplicata Mercantil";
                        this.Sigla = "DM";
                        break;

                    case EnumEspecieDocumento_C6.DuplicataServico:
                        this.Especie = "Duplicata de Serviço";
                        this.Sigla = "DS";
                        break;

                    case EnumEspecieDocumento_C6.NotaPromissoria:
                        this.Especie = "Nota Promissória";
                        this.Sigla = string.Empty;
                        break;

                    case EnumEspecieDocumento_C6.NotaSeguro:
                        this.Especie = "Nota de Seguro";
                        this.Sigla = string.Empty;
                        break;

                    case EnumEspecieDocumento_C6.Recibo:
                        this.Especie = "Recibo";
                        this.Sigla = string.Empty;
                        break;

                    case EnumEspecieDocumento_C6.LetraCambio:
                        this.Especie = "Letra de Câmbio";
                        this.Sigla = string.Empty;
                        break;

                    case EnumEspecieDocumento_C6.FichaCompensacao:
                        this.Especie = "Ficha de Compensação";
                        this.Sigla = string.Empty;
                        break;

                    case EnumEspecieDocumento_C6.Carne:
                        this.Especie = "Carnê";
                        this.Sigla = string.Empty;
                        break;

                    case EnumEspecieDocumento_C6.Contrato:
                        this.Especie = "Contrato";
                        this.Sigla = string.Empty;
                        break;

                    case EnumEspecieDocumento_C6.Cheque:
                        this.Especie = "Cheque";
                        this.Sigla = string.Empty;
                        break;

                    case EnumEspecieDocumento_C6.CobrancaSeriada:
                        this.Especie = "Cobrança Seriada";
                        this.Sigla = string.Empty;
                        break;

                    case EnumEspecieDocumento_C6.MensalidadeEscolar:
                        this.Especie = "Mensalidade Escolar";
                        this.Sigla = string.Empty;
                        break;

                    case EnumEspecieDocumento_C6.NotaDebito:
                        this.Especie = "Nota de Débito";
                        this.Sigla = string.Empty;
                        break;

                    case EnumEspecieDocumento_C6.DocumentoDivida:
                        this.Especie = "Documento de Dívida";
                        this.Sigla = string.Empty;
                        break;

                    case EnumEspecieDocumento_C6.EncargosCondominiais:
                        this.Especie = "Encargos Condominiais";
                        this.Sigla = string.Empty;
                        break;

                    case EnumEspecieDocumento_C6.ContaPrestacaoServicos:
                        this.Especie = "Conta de Prestação de Serviços";
                        this.Sigla = string.Empty;
                        break;

                    case EnumEspecieDocumento_C6.FaturaCartaoCredito:
                        this.Especie = "Fatura de Cartão de Crédito (FT)";
                        this.Sigla = "FT";
                        break;

                    case EnumEspecieDocumento_C6.Outros:
                        this.Especie = "Outros";
                        this.Sigla = string.Empty;
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
                throw new ArgumentException("Erro ao carregar objeto", ex);
            }
        }

        public static EspeciesDocumento CarregaTodas()
        {
            try
            {
                EspeciesDocumento alEspeciesDocumento = new EspeciesDocumento();

                foreach (EnumEspecieDocumento_C6 val in Enum.GetValues(typeof(EnumEspecieDocumento_C6)))
                {
                    alEspeciesDocumento.Add(new EspecieDocumento_C6(val));
                }

                return alEspeciesDocumento;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Erro ao listar objetos", ex);
            }
        }

        public override IEspecieDocumento DuplicataMercantil()
        {
            return new EspecieDocumento_C6(((int)EnumEspecieDocumento_C6.DuplicataMercantil).ToString());
        }

        #endregion Metodos Privados
    }
}