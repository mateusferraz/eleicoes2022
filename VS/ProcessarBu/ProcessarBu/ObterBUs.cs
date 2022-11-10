using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProcessarBu
{
    public class ObterBUs
    {

        public static async Task getBU(DadosBU dadosBU)
        {
            using var client = new HttpClient();
            //string _url = "https://resultados.tse.jus.br/oficial/ele2022/arquivo-urna/406/dados/al/27030/0048/0090/p000406-al-m27030-z0048-s0090-aux.json";
            string _url = $"{dadosBU.url}/{dadosBU.idEleicao}/dados/{dadosBU.estado}/{dadosBU.municipio}/{dadosBU.zona}/{dadosBU.sessao}/p000{dadosBU.idEleicao}-{dadosBU.estado}-m{dadosBU.municipio}-z{dadosBU.zona}-s{dadosBU.sessao}-aux.json";
            //var jSonResult = await client.GetAsync();
            //client.BaseAddress = new Uri(_url);
            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));            
            HttpResponseMessage response = await client.GetAsync(_url);
            response.EnsureSuccessStatusCode();
            var resp = await response.Content.ReadAsStringAsync();
            //var jsonResult = JsonConvert.DeserializeObject(resp);
            var data = (JObject.Parse(resp)["hashes"].First()).ToString();
            dadosBU.hash = JObject.Parse(data)["hash"].ToString();
            dadosBU.buName = JObject.Parse(data)["nmarq"][3].ToString();
            await DownloadFileBu(dadosBU);

        }
        public static async Task DownloadFileBu(DadosBU dadosBU)
        {
            string _url = $"{dadosBU.url}/{dadosBU.estado}/{dadosBU.municipio}/{dadosBU.zona}/{dadosBU.sessao}/{dadosBU.hash}/{dadosBU.buName}";
            var uri = new Uri(_url);
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(uri);
            string fileName = Settings.GetSetting("pathBUFiles2Turno") + dadosBU.buName;
            //HostingEnvironment.MapPath(string.Format("~/Downloads/{0}.pdf", pdfGuid)),
            using (var fs = new FileStream(
                fileName,
                FileMode.CreateNew))
            {
                await response.Content.CopyToAsync(fs);
            }
        }
    }

}
