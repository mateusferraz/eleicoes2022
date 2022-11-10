using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProcessarBu
{
    class Program
    {
        static void Main(string[] args)
        {
            DownloadBUs();
            //importarUrnas();
            /*
            DadosBU dadosBU = new DadosBU();
            dadosBU.url = Settings.GetSetting("urlEleicao");
            dadosBU.estado = "al";
            dadosBU.municipio = 27030;
            dadosBU.zona = "0048";
            dadosBU.sessao = "0090";
            ObterBUs.getBU(dadosBU).Wait();
            */
            string path = Settings.GetSetting("pathCSVFiles");
            //var rowsBu = lerBu(path);
            /*
            List<Boletim> dadosBoletim = GetListBoletimFromBu(rowsBu);
            foreach (var bu in dadosBoletim)
            {
                Data.InsertBu(bu).Wait();
            }
            */

        }
        static void importarUrnas()
        {
            List<Urna> urnas = new List<Urna>();
            string path = Settings.GetSetting("pathFileUrnas");
            string[] file = Directory.GetFiles(path, "csv_Municipios_e_Zonas_e_sessao_COMPLETO.csv");            
            var reader = new StreamReader(File.OpenRead(file[0]));
            //List<Object> row = new List<Object>();            
            bool lineZero = false;
            while (!reader.EndOfStream)
            {
                //row = new List<Object>();
                var line = reader.ReadLine();
                
                if (!lineZero)
                {
                    lineZero = true;
                    continue;
                }
                try
                {

                    var row = line.Split(';');
                    urnas.Add(new Urna()
                    {
                        SG_UF = row[0].ToString()
                        ,SG_UE = row[1].ToString()
                        ,NM_UE = row[2].ToString()
                        ,CD_MUNICIPIO = row[3].ToString()
                        ,NM_MUNICIPIO = row[4].ToString()
                        ,NM_ZONA = row[5].ToString()
                        ,NR_SECAO = row[6].ToString()
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao Inserir arquivo {file} - Erro :{ex.Message}");
                }
            }
            Data.InsertUrnas(urnas).Wait();

        }
        static List<List<Object>> lerBu(string path)
        {
            
            string[] files = Directory.GetFiles(path,"*.csv");
            List<List<Object>> rows = new List<List<Object>>();
            foreach (var file in files)
            {
                try
                {
                    rows = new List<List<Object>>();
                    var reader = new StreamReader(File.OpenRead(file));

                    bool lineZero = false;

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        //Console.WriteLine(line);
                        if (!lineZero)
                        {
                            lineZero = true;
                            continue;
                        }

                        rows.Add(line.Split(';').ToList<Object>());
                    }
                    //fazer inser DB
                    var bu = GetListBoletimFromBu(rows);
                    Data.InsertBu(bu).Wait();
                    
                    reader.Close();
                    string fileName = file.Replace(path, "");
                    System.IO.File.Move(file, path + "/Processado/" + fileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao Inserir arquivo {file} - Erro :{ex.Message}");
                }
                //Directory.Move(file, );
                
            }
            return rows;
        }
        static Boletim GetListBoletimFromBu(List<List<Object>> rows)
        {
            var row = rows[0]; //pegar dados primeira linha
            Boletim boletim = new Boletim
            {
                dataGeracao =row[0].ToString()
                ,idEleitoral =row[1].ToString()
                ,chaveAssinaturaVotosVotavel =row[2].ToString()
                ,dadosSecaoSA =row[3].ToString()
                ,dataHoraEmissao =row[4].ToString()
                ,fase =row[5].ToString()
                ,codigoCarga =row[6].ToString()
                ,dataHoraCarga =row[7].ToString()
                ,numeroInternoUrna =row[8].ToString()
                ,numeroSerieFC =row[9].ToString()
                ,identificacao =row[10].ToString()
                ,numeroSerieFV =row[11].ToString()
                ,tipoArquivo =row[12].ToString()
                ,tipoUrna =row[13].ToString()
                ,versaoVotacao =row[14].ToString()
                ,local =Convert.ToInt32( row[15])
                ,municipio = Convert.ToInt32(row[16])
                , zona = Convert.ToInt32( row[17])
                , secao = Convert.ToInt32( row[18])
                , qtdEleitoresCompBiometrico = (row[19] ==null || row[19].ToString() == "null") ? null : Convert.ToInt32( row[19])
                , qtdEleitoresLibCodigo = (row[20] == null || row[20].ToString() == "null") ? null : Convert.ToInt32( row[20])
                , idEleicao1 = Convert.ToInt32( row[21])
                , qtdEleitoresAptos1 = Convert.ToInt32( row[22])
                , qtdComparecimento1 = Convert.ToInt32( row[23])
                , tipoCargo1 =row[24].ToString()
                ,idEleicao2 = Convert.ToInt32( row[25])
                ,qtdEleitoresAptos2 = Convert.ToInt32( row[26])
                , qtdComparecimento2 = Convert.ToInt32( row[27])
                , tipoCargo2 =row[28].ToString()
                ,codigoCargo =row[29].ToString()
                ,ordemImpressao = Convert.ToInt32(row[30])
                , assinatura =row[31].ToString()
                //,partido =row[32].ToString()
                //,quantidadeVotos = Convert.ToInt32( row[33])
                //,tipoVoto =row[34].ToString()
            };
            

            foreach (var line in rows)
            {
                string partido = line[32].ToString().ToUpper();
                switch (partido)
                {
                    case "15": boletim.candidato15 = Convert.ToInt32(line[33]); break;
                    case "27": boletim.candidato27 = Convert.ToInt32(line[33]); break;
                    case "80": boletim.candidato80 = Convert.ToInt32(line[33]); break;
                    case "13": boletim.candidato13 = Convert.ToInt32(line[33]); break;
                    case "16": boletim.candidato16 = Convert.ToInt32(line[33]); break;
                    case "44": boletim.candidato44 = Convert.ToInt32(line[33]); break;
                    case "22": boletim.candidato22 = Convert.ToInt32(line[33]); break;
                    case "12": boletim.candidato12 = Convert.ToInt32(line[33]); break;
                    case "14": boletim.candidato14 = Convert.ToInt32(line[33]); break;
                    case "90": boletim.candidato90 = Convert.ToInt32(line[33]); break;
                    case "21": boletim.candidato21 = Convert.ToInt32(line[33]); break;
                    case "30": boletim.candidato30 = Convert.ToInt32(line[33]); break;
                    case "BRANCO": boletim.Branco = Convert.ToInt32(line[33]); break;
                    case "NULO": boletim.Nulo = Convert.ToInt32(line[33]); break;
                    default: boletim.Outro = Convert.ToInt32(line[33]); break;
                }
            }
            return boletim;
        }
        
        static void DownloadBUs()
        {
            //var urnas = Data.ConsultarUrnas().Wait();
            //ObterBUs.getBU(dadosBU).Wait();
            DadosBU dadosBU = new DadosBU();
            dadosBU.url = Settings.GetSetting("urlEleicao2turno");
            dadosBU.idEleicao = "407";
            dadosBU.estado = "zz";
            dadosBU.municipio = 29351;
            dadosBU.zona = "0001";
            dadosBU.sessao = "1557";
            ObterBUs.getBU(dadosBU).Wait();
            //foreach (var urna in urnas)
            //{

            //    dadosBU.estado = urna.SG_UF;
            //    dadosBU.municipio = urna.CD_MUNICIPIO;
            //    dadosBU.zona = urna.NM_ZONA;
            //    dadosBU.sessao = urna.NR_SECAO;
            //    ObterBUs.getBU(dadosBU).Wait();
            //}

        }
    }
}
