using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    // C�digos de rejeicoes de 1 a 64 associados ao c�digo de movimento 3, 26 e 30

    #region Enumerado

    public enum EnumCodigoRejeicao_BancoBrasil
    {
        CodigoBancoInvalido = 1,
        CodigoRegistroDetalheInvalido = 2,
        CodigoSegmentoInvalido = 3,
        CodigoMovimentoNaoPermitidoParaCarteira = 4,
        CodigoMovimentoInvalido = 5,
        TipoNumeroInscricaoCedenteInvalidos = 6,
        AgenciaContaDVInvalido = 7,
        NossoNumeroInvalido = 8,
        NossoNumeroDuplicado = 9,
        CarteiraInvalida = 10,
        FormaCadastramentoTituloInvalido = 11,
        TipoDocumentoInvalido = 12,
        IdentificacaoEmissaoBloquetoInvalida = 13,
        IdentificacaoDistribuicaoBloquetoInvalida = 14,
        CaracteristicasCobrancaIncompativeis = 15,
        DataVencimentoInvalida = 16,
        DataVencimentoAnteriorDataEmissao = 17,
        VencimentoForadoPrazodeOperacao = 18,
        TituloCargoBancosCorrespondentesVencimentoInferiorXXDias = 19,
        ValorTituloInvalido = 20,
        EspecieTituloInvalida = 21,
        EspecieNaoPermitidaParaCarteira = 22,
        AceiteInvalido = 23,
        DataEmissaoInvalida = 24,
        DataEmissaoPosteriorData = 25,
        CodigoJurosMoraInvalido = 26,
        ValorJurosMoraInvalido = 27,
        CodigoDescontoInvalido = 28,
        ValorDescontoMaiorIgualValorTitulo = 29,
        DescontoConcederNaoConfere = 30,
        ConcessaoDescontoJaExiste = 31,
        ValorIOFInvalido = 32,
        ValorAbatimentoInvalido = 33,
        ValorAbatimentoMaiorIgualValorTitulo = 34,
        AbatimentoConcederNaoConfere = 35,
        ConcessaoAbatimentoJaExiste = 36,
        CodigoProtestoInvalido = 37,
        PrazoProtestoInvalido = 38,
        PedidoProtestoNaoPermitidoParaTitulo = 39,
        TituloComOrdemProtestoEmitida = 40,
        PedidoCancelamentoParaTitulosSemInstrucaoProtesto = 41,
        CodigoParaBaixaDevolucaoInvalido = 42,
        PrazoParaBaixaDevolucaoInvalido = 43,
        CodigoMoedaInvalido = 44,
        NomeSacadoNaoInformado = 45,
        TipoNumeroInscricaoSacadoInvalidos = 46,
        EnderecoSacadoNaoInformado = 47,
        CEPInvalido = 48,
        CEPSemPracaCobranca = 49,
        CEPReferenteBancoCorrespondente = 50,
        CEPIncompativelComUnidadeFederacao = 51,
        UnidadeFederacaoInvalida = 52,
        TipoNumeroInscricaoSacadorAvalistaInvalido = 53,
        SacadorAvalistaNaoInformado = 54,
        NossoNumeroBancoCorrespondenteNaoInformado = 55,
        CodigoBancoCorrespondenteNaoInformado = 56,
        CodigoMultaInvalido = 57,
        DataMultaInvalida = 58,
        ValorPercentualMultaInvalido = 59,
        MovimentoParaTituloNaoCadastrado = 60,
        AlteracaoAgenciaCobradoraInvalida = 61,
        TipoImpressaoInvalido = 62,
        EntradaParaTituloJaCadastrado = 63,
        NumeroLinhaInvalido = 64,
        CodigoBancoDebitoInvalido = 65,
        AgenciaContaDVParaDebitoInvalido = 66,
        DadosParaDebitoIncompativel = 67,
        ArquivoEmDuplicidade = 88,
        ContratoInexistente = 99,
    }

    #endregion 

    public class CodigoRejeicao_BancoBrasil: AbstractCodigoRejeicao, ICodigoRejeicao
    {
        #region Construtores 

		public CodigoRejeicao_BancoBrasil()
		{
			try
			{
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

        public CodigoRejeicao_BancoBrasil(int codigo)
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

        private void carregar(int codigo)
        {
            try
            {
                this.Banco = new Banco_Brasil();

                switch ((EnumCodigoRejeicao_BancoBrasil)codigo)
                {
                    case  EnumCodigoRejeicao_BancoBrasil.CodigoBancoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoBancoInvalido;
                        this.Descricao = "C�digo do banco inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CodigoRegistroDetalheInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoRegistroDetalheInvalido;
                        this.Descricao = "C�digo do registro detalhe inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CodigoSegmentoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoSegmentoInvalido;
                        this.Descricao = "C�digo do segmento inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CodigoMovimentoNaoPermitidoParaCarteira:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoMovimentoNaoPermitidoParaCarteira;
                        this.Descricao = "C�digo do movimento n�o permitido para a carteira";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CodigoMovimentoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoMovimentoInvalido;
                        this.Descricao = "C�digo do movimento inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.TipoNumeroInscricaoCedenteInvalidos:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TipoNumeroInscricaoCedenteInvalidos;
                        this.Descricao = "Tipo/N�mero de inscri��o do cendente inv�lidos";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.AgenciaContaDVInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.AgenciaContaDVInvalido;
                        this.Descricao = "Ag�ncia/Conta/DV inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.NossoNumeroInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.NossoNumeroInvalido;
                        this.Descricao = "Nosso n�mero inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.NossoNumeroDuplicado:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.NossoNumeroDuplicado;
                        this.Descricao = "Nosso n�mero duplicado";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CarteiraInvalida:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CarteiraInvalida;
                        this.Descricao = "Carteira inv�lida";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.FormaCadastramentoTituloInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.FormaCadastramentoTituloInvalido;
                        this.Descricao = "Forma de cadastramento do t�tulo inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.TipoDocumentoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TipoDocumentoInvalido;
                        this.Descricao = "Tipo de documento inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.IdentificacaoEmissaoBloquetoInvalida:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.IdentificacaoEmissaoBloquetoInvalida;
                        this.Descricao = "Identifica��o da emiss�o do bloqueto inv�lida";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.IdentificacaoDistribuicaoBloquetoInvalida:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.IdentificacaoDistribuicaoBloquetoInvalida;
                        this.Descricao = "Identifica��o da distribui��o do bloqueto inv�lida";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CaracteristicasCobrancaIncompativeis:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CaracteristicasCobrancaIncompativeis;
                        this.Descricao = "Caracter�sticas da cobran�a incompat�veis";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.DataVencimentoInvalida:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DataVencimentoInvalida;
                        this.Descricao = "Data de vencimento inv�lida";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.DataVencimentoAnteriorDataEmissao:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DataVencimentoAnteriorDataEmissao;
                        this.Descricao = "Data de vencimento anterior a data de emiss�o";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.VencimentoForadoPrazodeOperacao:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.VencimentoForadoPrazodeOperacao;
                        this.Descricao = "Vencimento fora do prazo de emiss�o";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.TituloCargoBancosCorrespondentesVencimentoInferiorXXDias:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TituloCargoBancosCorrespondentesVencimentoInferiorXXDias;
                        this.Descricao = "Titulo a cargo de bancos correspondentes com vencimento inferior a XX dias";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.ValorTituloInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorTituloInvalido;
                        this.Descricao = "Valor do t�tulo inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.EspecieTituloInvalida:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.EspecieTituloInvalida;
                        this.Descricao = "Esp�cie do t�tulo inv�lida";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.EspecieNaoPermitidaParaCarteira:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.EspecieNaoPermitidaParaCarteira;
                        this.Descricao = "Esp�cie n�o permitida para a carteira";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.AceiteInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.AceiteInvalido;
                        this.Descricao = "Aceite inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.DataEmissaoInvalida:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DataEmissaoInvalida;
                        this.Descricao = "Data de emiss�o inv�lida";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.DataEmissaoPosteriorData:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DataEmissaoPosteriorData;
                        this.Descricao = "Data de emiss�o posterior a data";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CodigoJurosMoraInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoJurosMoraInvalido;
                        this.Descricao = "C�digo de juros de mora inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.ValorJurosMoraInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorJurosMoraInvalido;
                        this.Descricao = "Valor/Taxa de juros de mora inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CodigoDescontoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoDescontoInvalido;
                        this.Descricao = "C�digo do desconto inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.ValorDescontoMaiorIgualValorTitulo:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorDescontoMaiorIgualValorTitulo;
                        this.Descricao = "Valor do desconto maior ou igual ao valor do t�tulo";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.DescontoConcederNaoConfere:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DescontoConcederNaoConfere;
                        this.Descricao = "Desconto a conceder n�o confere";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.ConcessaoDescontoJaExiste:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ConcessaoDescontoJaExiste;
                        this.Descricao = "Concess�o de desconto - j� existe desconto anterior";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.ValorIOFInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorIOFInvalido;
                        this.Descricao = "Valor do IOF inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.ValorAbatimentoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorAbatimentoInvalido;
                        this.Descricao = "Valor do abatimento inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.ValorAbatimentoMaiorIgualValorTitulo:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorAbatimentoMaiorIgualValorTitulo;
                        this.Descricao = "Valor do abatimento maior ou igual ao valor do t�tulo";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.AbatimentoConcederNaoConfere:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.AbatimentoConcederNaoConfere;
                        this.Descricao = "Abatimento a conceder n�o confere";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.ConcessaoAbatimentoJaExiste:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ConcessaoAbatimentoJaExiste;
                        this.Descricao = "Concess�o de abatimento - j� existe abatimendo anterior";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CodigoProtestoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoProtestoInvalido;
                        this.Descricao = "C�digo para protesto inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.PrazoProtestoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.PrazoProtestoInvalido;
                        this.Descricao = "Prazo para protesto inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.PedidoProtestoNaoPermitidoParaTitulo:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.PedidoProtestoNaoPermitidoParaTitulo;
                        this.Descricao = "Pedido de protesto n�o permitido para o t�tulo";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.TituloComOrdemProtestoEmitida:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TituloComOrdemProtestoEmitida;
                        this.Descricao = "T�tulo com ordem de protesto emitida";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.PedidoCancelamentoParaTitulosSemInstrucaoProtesto:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.PedidoCancelamentoParaTitulosSemInstrucaoProtesto;
                        this.Descricao = "Pedido de cancelamento para t�tulos sem instru��o de protesto";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CodigoParaBaixaDevolucaoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoParaBaixaDevolucaoInvalido;
                        this.Descricao = "C�digo para baixa/devolu��o inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.PrazoParaBaixaDevolucaoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.PrazoParaBaixaDevolucaoInvalido;
                        this.Descricao = "Prazo para baixa/devolu��o inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CodigoMoedaInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoMoedaInvalido;
                        this.Descricao = "C�digo da moeda inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.NomeSacadoNaoInformado:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.NomeSacadoNaoInformado;
                        this.Descricao = "Nome do sacado n�o informado";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.TipoNumeroInscricaoSacadoInvalidos:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TipoNumeroInscricaoSacadoInvalidos;
                        this.Descricao = "Tipo/N�mero de inscri��o do sacado inv�lidos";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.EnderecoSacadoNaoInformado:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.EnderecoSacadoNaoInformado;
                        this.Descricao = "Endere�o do sacado n�o informado";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CEPInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CEPInvalido;
                        this.Descricao = "CEP inv�lido";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.CEPSemPracaCobranca:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CEPSemPracaCobranca;
                        this.Descricao = "CEP sem pra�a de cobran�a";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.CEPReferenteBancoCorrespondente:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CEPReferenteBancoCorrespondente;
                        this.Descricao = "CEP referente a um banco correspondente";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.CEPIncompativelComUnidadeFederacao:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CEPIncompativelComUnidadeFederacao;
                        this.Descricao = "CEP incompat�vel com a UF";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.UnidadeFederacaoInvalida:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.UnidadeFederacaoInvalida;
                        this.Descricao = "UF inv�lida";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.TipoNumeroInscricaoSacadorAvalistaInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TipoNumeroInscricaoSacadorAvalistaInvalido;
                        this.Descricao = "Tipo/N�mero de inscri��o do sacador/avalista inv�lido";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.SacadorAvalistaNaoInformado:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.SacadorAvalistaNaoInformado;
                        this.Descricao = "Sacador/Avalista n�o informado";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.NossoNumeroBancoCorrespondenteNaoInformado:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.NossoNumeroBancoCorrespondenteNaoInformado;
                        this.Descricao = "Nosso n�mero no banco correspondente n�o informado";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.CodigoBancoCorrespondenteNaoInformado:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoBancoCorrespondenteNaoInformado;
                        this.Descricao = "C�digo do banco correspondente n�o informado";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.CodigoMultaInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoMultaInvalido;
                        this.Descricao = "C�digo da multa inv�lido";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.DataMultaInvalida:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DataMultaInvalida;
                        this.Descricao = "Data da multa inv�lida";
                        break;                        
                        case EnumCodigoRejeicao_BancoBrasil.ValorPercentualMultaInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorPercentualMultaInvalido;
                        this.Descricao = "Valor/Percentual da multa inv�lida";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.MovimentoParaTituloNaoCadastrado:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.MovimentoParaTituloNaoCadastrado;
                        this.Descricao = "Movimento para t�tulo n�o cadastrado";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.AlteracaoAgenciaCobradoraInvalida:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.AlteracaoAgenciaCobradoraInvalida;
                        this.Descricao = "Altera��o da ag�ncia cobradora/dv inv�lida";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.TipoImpressaoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TipoImpressaoInvalido;
                        this.Descricao = "Tipo de impress�o inv�lido";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.EntradaParaTituloJaCadastrado:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.EntradaParaTituloJaCadastrado;
                        this.Descricao = "Entrada para t�tulo j� cadastrado";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.NumeroLinhaInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.NumeroLinhaInvalido;
                        this.Descricao = "N�mero da linha inv�lido";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.CodigoBancoDebitoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoBancoDebitoInvalido;
                        this.Descricao = "C�digo do banco para d�bito inv�lido";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.AgenciaContaDVParaDebitoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.AgenciaContaDVParaDebitoInvalido;
                        this.Descricao = "Ag�ncia/Conta/DV para d�bito inv�lido";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.DadosParaDebitoIncompativel:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DadosParaDebitoIncompativel;
                        this.Descricao = "Dados para d�bito incompat�vel com a identifica��o da emiss�o do boleto";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.ArquivoEmDuplicidade:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ArquivoEmDuplicidade;
                        this.Descricao = "Arquivo em duplicidade";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.ContratoInexistente:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ContratoInexistente;
                        this.Descricao = "Contrato inexistente";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "( Selecione )";
                        break;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        private void Ler(int codigo)
        {
            try
            {
                switch (codigo)
                {
                    case 1:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoBancoInvalido;
                        this.Descricao = "C�digo do banco inv�lido";
                        break;
                    case 2:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoRegistroDetalheInvalido;
                        this.Descricao = "C�digo do registro detalhe inv�lido";
                        break;
                    case 3:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoSegmentoInvalido;
                        this.Descricao = "C�digo do segmento inv�lido";
                        break;
                    case 4:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoMovimentoNaoPermitidoParaCarteira;
                        this.Descricao = "C�digo do movimento n�o permitido para a carteira";
                        break;
                    case 5:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoMovimentoInvalido;
                        this.Descricao = "C�digo do movimento inv�lido";
                        break;
                    case 6:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TipoNumeroInscricaoCedenteInvalidos;
                        this.Descricao = "Tipo/N�mero de inscri��o do cendente inv�lidos";
                        break;
                    case 7:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.AgenciaContaDVInvalido;
                        this.Descricao = "Ag�ncia/Conta/DV inv�lido";
                        break;
                    case 8:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.NossoNumeroInvalido;
                        this.Descricao = "Nosso n�mero inv�lido";
                        break;
                    case 9:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.NossoNumeroDuplicado;
                        this.Descricao = "Nosso n�mero duplicado";
                        break;
                    case 10:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CarteiraInvalida;
                        this.Descricao = "Carteira inv�lida";
                        break;
                    case 11:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.FormaCadastramentoTituloInvalido;
                        this.Descricao = "Forma de cadastramento do t�tulo inv�lido";
                        break;
                    case 12:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TipoDocumentoInvalido;
                        this.Descricao = "Tipo de documento inv�lido";
                        break;
                    case 13:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.IdentificacaoEmissaoBloquetoInvalida;
                        this.Descricao = "Identifica��o da emiss�o do bloqueto inv�lida";
                        break;
                    case 14:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.IdentificacaoDistribuicaoBloquetoInvalida;
                        this.Descricao = "Identifica��o da distribui��o do bloqueto inv�lida";
                        break;
                    case 15:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CaracteristicasCobrancaIncompativeis;
                        this.Descricao = "Caracter�sticas da cobran�a incompat�veis";
                        break;
                    case 16:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DataVencimentoInvalida;
                        this.Descricao = "Data de vencimento inv�lida";
                        break;
                    case 17:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DataVencimentoAnteriorDataEmissao;
                        this.Descricao = "Data de vencimento anterior a data de emiss�o";
                        break;
                    case 18:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.VencimentoForadoPrazodeOperacao;
                        this.Descricao = "Vencimento fora do prazo de emiss�o";
                        break;
                    case 19:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TituloCargoBancosCorrespondentesVencimentoInferiorXXDias;
                        this.Descricao = "Titulo a cargo de bancos correspondentes com vencimento inferior a XX dias";
                        break;
                    case 20:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorTituloInvalido;
                        this.Descricao = "Valor do t�tulo inv�lido";
                        break;
                    case 21:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.EspecieTituloInvalida;
                        this.Descricao = "Esp�cie do t�tulo inv�lida";
                        break;
                    case 22:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.EspecieNaoPermitidaParaCarteira;
                        this.Descricao = "Esp�cie n�o permitida para a carteira";
                        break;
                    case 23:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.AceiteInvalido;
                        this.Descricao = "Aceite inv�lido";
                        break;
                    case 24:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DataEmissaoInvalida;
                        this.Descricao = "Data de emiss�o inv�lida";
                        break;
                    case 25:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DataEmissaoPosteriorData;
                        this.Descricao = "Data de emiss�o posterior a data";
                        break;
                    case 26:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoJurosMoraInvalido;
                        this.Descricao = "C�digo de juros de mora inv�lido";
                        break;
                    case 27:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorJurosMoraInvalido;
                        this.Descricao = "Valor/Taxa de juros de mora inv�lido";
                        break;
                    case 28:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoDescontoInvalido;
                        this.Descricao = "C�digo do desconto inv�lido";
                        break;
                    case 29:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorDescontoMaiorIgualValorTitulo;
                        this.Descricao = "Valor do desconto maior ou igual ao valor do t�tulo";
                        break;
                    case 30:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DescontoConcederNaoConfere;
                        this.Descricao = "Desconto a conceder n�o confere";
                        break;
                    case 31:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ConcessaoDescontoJaExiste;
                        this.Descricao = "Concess�o de desconto - j� existe desconto anterior";
                        break;
                    case 32:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorIOFInvalido;
                        this.Descricao = "Valor do IOF inv�lido";
                        break;
                    case 33:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorAbatimentoInvalido;
                        this.Descricao = "Valor do abatimento inv�lido";
                        break;
                    case 34:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorAbatimentoMaiorIgualValorTitulo;
                        this.Descricao = "Valor do abatimento maior ou igual ao valor do t�tulo";
                        break;
                    case 35:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.AbatimentoConcederNaoConfere;
                        this.Descricao = "Abatimento a conceder n�o confere";
                        break;
                    case 36:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ConcessaoAbatimentoJaExiste;
                        this.Descricao = "Concess�o de abatimento - j� existe abatimendo anterior";
                        break;
                    case 37:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoProtestoInvalido;
                        this.Descricao = "C�digo para protesto inv�lido";
                        break;
                    case 38:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.PrazoProtestoInvalido;
                        this.Descricao = "Prazo para protesto inv�lido";
                        break;
                    case 39:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.PedidoProtestoNaoPermitidoParaTitulo;
                        this.Descricao = "Pedido de protesto n�o permitido para o t�tulo";
                        break;
                    case 40:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TituloComOrdemProtestoEmitida;
                        this.Descricao = "T�tulo com ordem de protesto emitida";
                        break;
                    case 41:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.PedidoCancelamentoParaTitulosSemInstrucaoProtesto;
                        this.Descricao = "Pedido de cancelamento para t�tulos sem instru��o de protesto";
                        break;
                    case 42:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoParaBaixaDevolucaoInvalido;
                        this.Descricao = "C�digo para baixa/devolu��o inv�lido";
                        break;
                    case 43:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.PrazoParaBaixaDevolucaoInvalido;
                        this.Descricao = "Prazo para baixa/devolu��o inv�lido";
                        break;
                    case 44:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoMoedaInvalido;
                        this.Descricao = "C�digo da moeda inv�lido";
                        break;
                    case 45:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.NomeSacadoNaoInformado;
                        this.Descricao = "Nome do sacado n�o informado";
                        break;
                    case 46:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.AbatimentoConcederNaoConfere;
                        this.Descricao = "Tipo/N�mero de inscri��o do sacado inv�lidos";
                        break;
                    case 47:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.EnderecoSacadoNaoInformado;
                        this.Descricao = "Endere�o do sacado n�o informado";
                        break;
                    case 48:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CEPInvalido;
                        this.Descricao = "CEP inv�lido";
                        break;
                        case 49:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CEPSemPracaCobranca;
                        this.Descricao = "CEP sem pra�a de cobran�a";
                        break;
                        case 50:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CEPReferenteBancoCorrespondente;
                        this.Descricao = "CEP referente a um banco correspondente";
                        break;
                        case 51:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CEPIncompativelComUnidadeFederacao;
                        this.Descricao = "CEP incompat�vel com a UF";
                        break;
                        case 52:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.UnidadeFederacaoInvalida;
                        this.Descricao = "UF inv�lida";
                        break;
                        case 53:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TipoNumeroInscricaoSacadorAvalistaInvalido;
                        this.Descricao = "Tipo/N�mero de inscri��o do sacador/avalista inv�lido";
                        break;
                        case 54:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.SacadorAvalistaNaoInformado;
                        this.Descricao = "Sacador/Avalista n�o informado";
                        break;
                        case 55:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.NossoNumeroBancoCorrespondenteNaoInformado;
                        this.Descricao = "Nosso n�mero no banco correspondente n�o informado";
                        break;
                        case 56:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoBancoCorrespondenteNaoInformado;
                        this.Descricao = "C�digo do banco correspondente n�o informado";
                        break;
                        case 57:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoMultaInvalido;
                        this.Descricao = "C�digo da multa inv�lido";
                        break;
                        case 58:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DataMultaInvalida;
                        this.Descricao = "Data da multa inv�lida";
                        break;                        
                        case 59:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorPercentualMultaInvalido;
                        this.Descricao = "Valor/Percentual da multa inv�lida";
                        break;
                        case 60:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.MovimentoParaTituloNaoCadastrado;
                        this.Descricao = "Movimento para t�tulo n�o cadastrado";
                        break;
                        case 61:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.AlteracaoAgenciaCobradoraInvalida;
                        this.Descricao = "Altera��o da ag�ncia cobradora/dv inv�lida";
                        break;
                        case 62:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TipoImpressaoInvalido;
                        this.Descricao = "Tipo de impress�o inv�lido";
                        break;
                        case 63:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.EntradaParaTituloJaCadastrado;
                        this.Descricao = "Entrada para t�tulo j� cadastrado";
                        break;
                        case 64:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.NumeroLinhaInvalido;
                        this.Descricao = "N�mero da linha inv�lido";
                        break;
                        case 65:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoBancoDebitoInvalido;
                        this.Descricao = "C�digo do banco para d�bito inv�lido";
                        break;
                        case 66:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.AgenciaContaDVParaDebitoInvalido;
                        this.Descricao = "Ag�ncia/Conta/DV para d�bito inv�lido";
                        break;
                        case 67:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DadosParaDebitoIncompativel;
                        this.Descricao = "Dados para d�bito incompat�vel com a identifica��o da emiss�o do boleto";
                        break;
                        case 88:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ArquivoEmDuplicidade;
                        this.Descricao = "Arquivo em duplicidade";
                        break;
                        case 99:
                        this.Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ContratoInexistente;
                        this.Descricao = "Contrato inexistente";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "( Selecione )";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }


        #endregion
    }
}
