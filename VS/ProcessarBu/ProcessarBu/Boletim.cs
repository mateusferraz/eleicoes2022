using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessarBu
{
    public class Boletim
    {
        public int Id { get; set; }
        public int local { get; set; }
        public int municipio { get; set; }
        public int zona { get; set; }
        public int secao { get; set; }
        public int? qtdEleitoresCompBiometrico { get; set; }
        public int? qtdComparecimento2 { get; set; }
        public int ordemImpressao { get; set; }
        public int quantidadeVotos { get; set; }
        public int? qtdEleitoresLibCodigo { get; set; }
        public int idEleicao1 { get; set; }
        public int qtdEleitoresAptos1 { get; set; }
        public int qtdComparecimento1 { get; set; }
        public int idEleicao2 { get; set; }
        public int qtdEleitoresAptos2 { get; set; }
        public string dataGeracao { get; set; }
        public string idEleitoral { get; set; }
        public string chaveAssinaturaVotosVotavel { get; set; }
        public string dadosSecaoSA { get; set; }
        public string dataHoraEmissao { get; set; }
        public string fase { get; set; }
        public string assinatura { get; set; }
        public string partido { get; set; }
        public string tipoVoto { get; set; }
        public string tipoArquivo { get; set; }
        public string tipoUrna { get; set; }
        public string versaoVotacao { get; set; }
        public string tipoCargo1 { get; set; }
        public string tipoCargo2 { get; set; }
        public string codigoCargo { get; set; }
        public string codigoCarga { get; set; }
        public string dataHoraCarga { get; set; }
        public string numeroInternoUrna { get; set; }
        public string numeroSerieFC { get; set; }
        public string identificacao { get; set; }
        public string numeroSerieFV { get; set; }

        public int? candidato15 { get; set; }
        public int? candidato27 { get; set; }
        public int? candidato80 { get; set; }
        public int? candidato13 { get; set; }
        public int? candidato16 { get; set; }
        public int? candidato44 { get; set; }
        public int? candidato22 { get; set; }
        public int? candidato12 { get; set; }
        public int? candidato14 { get; set; }        
        public int? candidato90 { get; set; }
        public int? candidato21 { get; set; }        
        public int? candidato30 { get; set; }
        public int? Branco { get; set; }
        public int? Nulo { get; set; }
        public int? Outro { get; set; }

    }
}
