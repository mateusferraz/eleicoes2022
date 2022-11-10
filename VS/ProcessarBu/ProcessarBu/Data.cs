using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ProcessarBu
{


    public class Data
    {
        

        public Data()
        {

        }
        public static string Conexao()
        {
            return Settings.GetSetting("ConnectionStrings:DefaultConnection");
        }

        public static async Task<bool>  InsertBu(Boletim bu)
        {
            string conexao = Conexao();
            bool Inserted = false;
            using (var db = new SqlConnection(conexao))
            {
                try
                {
                    await db.OpenAsync();
                    var query = @"Insert into Boletim
                               (
                                dataGeracao,idEleitoral,chaveAssinaturaVotosVotavel,dadosSecaoSA,dataHoraEmissao,fase,codigoCarga,dataHoraCarga,numeroInternoUrna,numeroSerieFC,identificacao,numeroSerieFV,tipoArquivo,tipoUrna,versaoVotacao,local,municipio,zona,secao,qtdEleitoresCompBiometrico
                                ,qtdEleitoresLibCodigo,idEleicao1,qtdEleitoresAptos1,qtdComparecimento1,tipoCargo1,idEleicao2,qtdEleitoresAptos2,qtdComparecimento2,tipoCargo2,codigoCargo,ordemImpressao,assinatura,partido,quantidadeVotos,tipoVoto
                                ,candidato15 ,candidato27 ,candidato80 ,candidato13 ,candidato16 ,candidato44 ,candidato22 ,candidato12 ,candidato14 ,candidato90 ,candidato21 ,candidato30 ,Branco ,Nulo ,Outro
                                )
                                values
                                (@dataGeracao ,@idEleitoral ,@chaveAssinaturaVotosVotavel ,@dadosSecaoSA ,@dataHoraEmissao ,@fase ,@codigoCarga ,@dataHoraCarga ,@numeroInternoUrna ,@numeroSerieFC ,@identificacao ,@numeroSerieFV ,@tipoArquivo ,@tipoUrna ,@versaoVotacao ,@local ,@municipio ,@zona ,@secao ,@qtdEleitoresCompBiometrico ,@qtdEleitoresLibCodigo ,@idEleicao1 ,@qtdEleitoresAptos1 ,@qtdComparecimento1 ,@tipoCargo1 ,@idEleicao2 ,@qtdEleitoresAptos2 ,@qtdComparecimento2 ,@tipoCargo2 ,@codigoCargo ,@ordemImpressao ,@assinatura ,@partido ,@quantidadeVotos ,@tipoVoto
                                 ,@candidato15 ,@candidato27 ,@candidato80 ,@candidato13 ,@candidato16 ,@candidato44 ,@candidato22 ,@candidato12 ,@candidato14 ,@candidato90 ,@candidato21 ,@candidato30 ,@Branco ,@Nulo ,@Outro)
                                ";
                    await db.ExecuteAsync(query, bu);
                    Inserted = true;
                    Console.WriteLine($"bu {bu.identificacao} incluido com sucesso");
                    return Inserted;
                }catch(Exception ex)
                {
                    Console.WriteLine($"Erro ao inserir BU - { bu.identificacao}, Erro -  {ex.Message}");
                }
                return Inserted;
            }
        }



        public static async Task<IEnumerable<Boletim>> ConsultarBoletins()
        {
            string conexao = Conexao();
            using (var db = new SqlConnection(conexao))
            {
                IEnumerable<Boletim> boletins;
                await db.OpenAsync();
                var query = "Select * from Boletim";
                boletins = await db.QueryAsync<Boletim>(query);
                return boletins;
            }
        }

        public static async Task<bool> InsertUrnas(List<Urna> urnas)
        {
            string conexao = Conexao();
            bool Inserted = false;
            using (var db = new SqlConnection(conexao))
            {
                try
                {
                    await db.OpenAsync();
                    foreach (var urna in urnas)
                    {



                        var query = @"Insert into Urnas
                               (
                                SG_UF,SG_UE,NM_UE,CD_MUNICIPIO,NM_MUNICIPIO,NM_ZONA,NR_SECAO
                                )
                                values
                                (@SG_UF,@SG_UE,@NM_UE,@CD_MUNICIPIO,@NM_MUNICIPIO,@NM_ZONA,@NR_SECAO)
                                ";
                        await db.ExecuteAsync(query, urna);

                        Inserted = true;
                        Console.WriteLine($"Urna {urna.CD_MUNICIPIO} incluido com sucesso");
                    }
                    return Inserted;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao inserir urnas, Erro -  {ex.Message}");
                }
                return Inserted;
            }
        }

        public static async Task<IEnumerable<Urna>> ConsultarUrnas()
        {
            string conexao = Conexao();
            using (var db = new SqlConnection(conexao))
            {
                IEnumerable<Urna> urnas;
                await db.OpenAsync();
                var query = "Select * from urnas";
                urnas = await db.QueryAsync<Urna>(query);
                return urnas;
            }
        }

    }
}
