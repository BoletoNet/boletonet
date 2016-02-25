using System;

namespace BoletoNet
{
    public class DetalheSegmentoERetornoCNAB240
    {
        /// <summary>
        /// Código fornecido pelo Banco Central para identificação do Banco que está recebendo ou enviando o arquivo, com o qual se firmou o contrato de prestação de serviços.
        /// </summary>
        public int CodigoBanco { get; private set; }

        /// <summary>
        /// Número seqüencial para identificar univocamente um lote de serviço. Criado e controlado pelo responsável pela geração magnética dos dados contidos no arquivo.
        /// Preencher com '0001' para o primeiro lote do arquivo. Para os demais: número do lote anterior acrescido de 1. O número não poderá ser repetido dentro do arquivo.
        /// Se registro for Header do Arquivo preencher com '0000'
        /// Se registro for Trailer do Arquivo preencher com '9999'
        /// </summary>
        public int LoteServico { get; private set; }

        /// <summary>
        /// Código adotado pela FEBRABAN para identificar o tipo de registro.
        /// Domínio:
        /// '0' = Header de Arquivo
        /// '1' = Header de Lote
        /// '2' = Registros Iniciais do Lote
        /// '3' = Detalhe
        /// '4' = Registros Finais do Lote
        /// '5' = Trailer de Lote
        /// '9' = Trailer de Arquivo
        /// </summary>
        public string TipoRegistro { get; private set; }

        /// <summary>
        /// Número adotado e controlado pelo responsável pela geração magnética dos dados contidos no arquivo, para identificar a seqüência de registros encaminhados no lote.
        /// Deve ser inicializado sempre em '1', em cada novo lote.
        /// </summary>
        public int NumeroRegistro { get; private set; }

        /// <summary>
        /// Código adotado pela FEBRABAN para identificar o segmento do registro.
        /// </summary>
        public string Segmento { get; private set; }

        /// <summary>
        /// Texto de observações destinado para uso exclusivo da FEBRABAN.
        /// Preencher com Brancos.
        /// </summary>
        public string UsoExclusivoFebrabanCnab { get; private set; }

        /// <summary>
        /// Código que identifica o tipo de inscrição da Empresa ou Pessoa Física perante uma Instituição governamental.Domínio:
        /// '0' = Isento / Não Informado
        /// '1' = CPF
        /// '2' = CGC / CNPJ
        /// '3' = PIS / PASEP
        /// '9' = Outros
        /// Preenchimento deste campo é obrigatório para DOC e TED (Forma de Lançamento = 03, 41, 43)
        /// Para pagamento para o SIAPE com crédito em conta, o CPF deverá ser do 1º titular.
        /// </summary>
        public TipoInscricao TipoInscricaoCliente { get; private set; }

        /// <summary>
        /// Número de inscrição da Empresa ou Pessoa Física perante uma Instituição governamental.
        /// </summary>
        public long NumeroInscricaoCliente { get; private set; }

        /// <summary>
        /// Código adotado pelo Banco para identificar o Contrato entre este e a Empresa Cliente.
        /// </summary>
        public string CodigoConvenioBanco { get; private set; }

        /// <summary>
        /// Código adotado pelo Banco responsável pela conta, para identificar a qual unidade está vinculada a conta corrente.
        /// </summary>
        public int AgenciaMantenedoraConta { get; private set; }

        /// <summary>
        /// Código adotado pelo Banco responsável pela conta corrente, para verificação da autenticidade do Código da Agência.
        /// </summary>
        public string DigitoVerificadorAgencia { get; private set; }

        /// <summary>
        /// Número adotado pelo Banco, para identificar univocamente a conta corrente utilizada pelo Cliente.
        /// </summary>
        public long NumeroContaCorrente { get; private set; }

        /// <summary>
        /// Código adotado pelo responsável pela conta corrente, para verificação da autenticidade do Número da Conta Corrente.
        /// Para os Bancos que se utilizam de duas posições para o Dígito Verificador do Número da Conta Corrente, preencher este campo com a 1ª posição deste dígito.
        /// Exemplo :
        /// Número C/C = 45981-36
        /// Neste caso -> Dígito Verificador da Conta = 3
        /// </summary>
        public string DigitoVerificadorConta { get; private set; }

        /// <summary>
        /// Código adotado pelo Banco responsável pela conta corrente, para verificação da autenticidade do par Código da Agência / Número da Conta Corrente.
        /// Para os Bancos que se utilizam de duas posições para o Dígito Verificador do Número da Conta Corrente, preencher este campo com a 2ª posição deste dígito.
        /// Exemplo :
        /// Número C/C = 45981-36
        /// Neste caso -> Dígito Verificador da Ag/Conta = 6
        /// </summary>
        public string DigitoVerificadorAgenciaConta { get; private set; }

        /// <summary>
        /// Nome que identifica a pessoa, física ou jurídica, a qual se quer fazer referência.
        /// </summary>
        public string NomeEmpresa { get; private set; }

        /// <summary>
        /// Texto de observações destinado para uso exclusivo da FEBRABAN.
        /// Preencher com Brancos.
        /// </summary>
        public string   UsoExclusivoFebrabanCnab2 { get; private set; }

        /// <summary>
        /// Identifica se o Lançamento incide sobre valores disponíveis ou bloqueados, possibilitando a recomposição das posições dos saldos.
        /// Domínio:
        /// 'DPV' = TIPO DISPONÍVEL
        /// Lançamento ocorrido em Saldo Disponível
        /// 'SCR' = TIPO VINCULADO
        /// Lançamento ocorrido em Saldo Disponível ou Vinculado (a critério de cada banco), porém pendente de liberação por regras internas do banco
        /// 'SSR' = TIPO BLOQUEADO
        /// Lançamento ocorrido em Saldo Bloqueado
        /// 'CDS' = COMPOSIÇÃO DE DIVERSOS SALDOS
        /// Lançamento ocorrido em diversos saldos
        /// A condição de recurso Disponível, Vinculado ou Bloqueado para os códigos, SCR, SSR e CDS é critério de cada banco.
        /// </summary>
        public string NaturezaLancamento { get; private set; }

        /// <summary>
        /// Código adotado pela FEBRABAN para identificar a padronização a ser utilizada no complemento.
        /// Domínio:
        /// '00' = Sem Informação do Complemento do Lançamento
        /// '01' = Identificação da Origem do Lançamento
        /// </summary>
        public TipoComplementoLancamento TipoComplementoLancamento { get; private set; }

        /// <summary>
        /// Texto de informações complementares ao Lançamento.
        /// Para Tipo do Complemento = 01, o campo complemento terá o seguinte formato:
        /// Banco Origem Lançamento 114 116 3 Num
        /// Agência Origem Lançamento 117 121 5 Num
        /// Uso Exclusivo FEBRABAN/ CNAB 122 133 12 Alfa preencher com brancos
        /// </summary>
        public string ComplementoLancamento { get; private set; }

        /// <summary>
        /// Código adotado pela FEBRABAN para identificação de Lançamentos desobrigados de recolhimento do CPMF.
        /// Domínio:
        /// 'S' = Isento
        /// 'N' = Não Isento
        /// </summary>
        public IsencaoCpmf IdentificacaoIsencaoCpmf { get; private set; }

        /// <summary>
        /// Data de efetivação do Lançamento.
        /// Utilizar o formato DDMMAAAA, onde:
        /// DD = dia
        /// MM = mês
        /// AAAA = ano
        /// </summary>
        public DateTime? DataContabil { get; private set; }

        /// <summary>
        /// Data de ocorrência dos fatos, itens, componentes do extrato bancário.
        /// Utilizar o formato DDMMAAAA, onde:
        /// DD = dia
        /// MM = mês
        /// AAAA = ano
        /// </summary>
        public DateTime DataLancamento { get; private set; }

        /// <summary>
        /// Valor do Lançamento efetuado, expresso em moeda corrente.
        /// </summary>
        public decimal ValorLancamento { get; private set; }

        /// <summary>
        /// Código adotado pela FEBRABAN para caracterizar o item que está sendo representado no extrato bancário.
        /// Domínio:
        /// 'D' = Débito
        /// 'C' = Crédito
        /// </summary>
        public TipoLancamento TipoLancamento { get; private set; }

        /// <summary>
        /// Código adotado pela FEBRABAN, para identificar a categoria padrão do Lançamento, para conciliação entre Bancos.
        /// Domínio:
        /// Débitos:
        /// '101' = Cheques
        /// '102' = Encargos
        /// '103' = Estornos
        /// '104' = Lançamento Avisado
        /// '105' = Tarifas
        /// '106' = Aplicação
        /// '107' = Empréstimo / Financiamento
        /// '108' = Câmbio
        /// '109' = CPMF
        /// '110' = IOF
        /// '111' = Imposto de Renda
        /// '112' = Pagamento Fornecedores
        /// '113' = Pagamentos Salário
        /// '114' = Saque Eletrônico
        /// '115' = Ações
        /// '117' = Transferência entre Contas
        /// '118' = Devolução da Compensação
        /// '119' = Devolução de Cheque Depositado
        /// '120' = Transferência Interbancária (DOC, TED)
        /// '121' = Antecipação a Fornecedores
        /// '122' = OC / AEROPS
        /// Créditos:
        /// '201' = Depósitos
        /// '202' = Líquido de Cobrança
        /// '203' = Devolução de Cheques
        /// '204' = Estornos
        /// '205' = Lançamento Avisado
        /// '206' = Resgate de Aplicação
        /// '207' = Empréstimo / Financiamento
        /// '208' = Câmbio
        /// '209' = Transferência Interbancária (DOC, TED)
        /// '210' = Ações
        /// '211' = Dividendos
        /// '212' = Seguro
        /// '213' = Transferência entre Contas
        /// '214' = Depósitos Especiais
        /// '215' = Devolução da Compensação
        /// '216' = OCT
        /// '217' = Pagamentos Fornecedores
        /// '218' = Pagamentos Diversos
        /// '219' = Pagamentos Salários
        /// </summary>
        public CategoriaLancamento CategoriaLancamento { get; private set; }

        /// <summary>
        /// Código adotado por cada Banco para identificar o descritivo do Lançamento. Observar que no Extrato de Conta Corrente para Conciliação Bancária este campo possui 4 caracteres, enquanto no Extrato para Gestão de Caixa ele possui 5 caracteres.
        /// </summary>
        public string CodigoHistorico { get; private set; }

        /// <summary>
        /// Texto descritivo do histórico do Lançamento do extrato bancário.
        /// </summary>
        public string HistoricoLancamento { get; private set; }

        /// <summary>
        /// Número que identifica o documento que gerou o Lançamento. Para uso na conciliação automática de Conta Corrente, o número do documento não pode ser maior que 6 posições numéricas. O complemento está limitado de acordo com as restrições de cada banco.
        /// </summary>
        public string NumeroDocumentoComplemento { get; private set; }

        public void LerDetalheSegmentoERetornoCNAB240(string registro)
        {
            try
            {
                if (registro.Substring(13, 1) != "E")
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento E.");

                CodigoBanco = LeitorLinhaPosicao.ExtrairInt32DaPosicao(registro, 1, 3);
                LoteServico = LeitorLinhaPosicao.ExtrairInt32DaPosicao(registro, 4, 7);
                TipoRegistro = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 8, 8);
                NumeroRegistro = LeitorLinhaPosicao.ExtrairInt32DaPosicao(registro, 9, 13);
                Segmento = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 14, 14);
                UsoExclusivoFebrabanCnab = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 15, 17);
                TipoInscricaoCliente = (TipoInscricao) LeitorLinhaPosicao.ExtrairInt32DaPosicao(registro, 18, 18);
                NumeroInscricaoCliente = LeitorLinhaPosicao.ExtrairInt64DaPosicao(registro, 19, 32);
                CodigoConvenioBanco = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 33, 52);
                AgenciaMantenedoraConta = LeitorLinhaPosicao.ExtrairInt32DaPosicao(registro, 53, 57);
                DigitoVerificadorAgencia = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 58, 58);
                NumeroContaCorrente = LeitorLinhaPosicao.ExtrairInt64DaPosicao(registro, 59, 70);
                DigitoVerificadorConta = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 71, 71);
                DigitoVerificadorAgenciaConta = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 72, 72);
                NomeEmpresa = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 73, 102);
                UsoExclusivoFebrabanCnab2 = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 103, 108);
                NaturezaLancamento = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 109, 111);
                TipoComplementoLancamento = (TipoComplementoLancamento) LeitorLinhaPosicao.ExtrairInt32OpcionalDaPosicao(registro, 112, 113).GetValueOrDefault();
                ComplementoLancamento = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 114, 133);
                IdentificacaoIsencaoCpmf = (IsencaoCpmf)LeitorLinhaPosicao.ExtrairDaPosicao(registro, 134, 134)[0];
                DataContabil = LeitorLinhaPosicao.ExtrairDataOpcionalDaPosicao(registro, 135, 142);
                DataLancamento = LeitorLinhaPosicao.ExtrairDataDaPosicao(registro, 143, 150);
                ValorLancamento = decimal.Parse(LeitorLinhaPosicao.ExtrairDaPosicao(registro, 151, 168))/100m;
                TipoLancamento = (TipoLancamento) LeitorLinhaPosicao.ExtrairDaPosicao(registro, 169, 169)[0];
                CategoriaLancamento = (CategoriaLancamento) LeitorLinhaPosicao.ExtrairInt32DaPosicao(registro, 170, 172);
                CodigoHistorico = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 173, 176);
                HistoricoLancamento = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 177, 201);
                NumeroDocumentoComplemento = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 202, 240);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO E.", ex);
            }
        }
    }
}
