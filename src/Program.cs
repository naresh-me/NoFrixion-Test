using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Linq;

namespace NoFrixion
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static async Task<ApiResponse> GetApiRespomseAsync(string path)
        {
            ApiResponse apiResponse = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                //string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                //File.WriteAllText(Path.Combine(docPath, "WriteFile.txt"), response.Content.ReadAsStringAsync().Result);
                apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            }
            return apiResponse;
        }

        static void ShowApiResponse(ApiResponse apiResponse)
        {
            Console.WriteLine("BTC Price EUR " + apiResponse.bpi.EUR.rate);
        }

        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
            //    Console.WriteLine("BTC Price EUR 39,589.5087");
        }

        static async Task RunAsync()
        {
            var url = "https://api.coindesk.com/v1/bpi/currentprice.json";
            client.BaseAddress = new Uri(url);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Get the Api response from rest api.
                //var apiResponse = await GetApiRespomseAsync("/");

                //As the actual api page wasn't found so, I used sample json response to write code.
                string apiJsonResponse = new WebClient().DownloadString(url);
                var jsonResponse = JsonConvert.DeserializeObject<ApiResponse>(apiJsonResponse);

                //List<ApiResponse> jsonListResponse = JsonConvert.DeserializeObject<List<ApiResponse>>(apiJsonResponse);
                //var latestBTCPrice = jsonListResponse.OrderBy(j => j.time.updated).FirstOrDefault();

                //Show the response.
                ShowApiResponse(jsonResponse);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }

    #region APIRESPONSE
    public class ApiResponse
    {
        public Time time { get; set; }
        public string disclaimer { get; set; }
        public string chartName { get; set; }
        public Bpi bpi { get; set; }
    }

    public class Time
    {
        public string updated { get; set; }
        public DateTime updatedISO { get; set; }
        public string updateduk { get; set; }
    }

    public class Bpi
    {
        public USD USD { get; set; }
        public GBP GBP { get; set; }
        public EUR EUR { get; set; }
    }

    public class USD
    {
        public string code { get; set; }
        public string symbol { get; set; }
        public string rate { get; set; }
        public string description { get; set; }
        public float rate_float { get; set; }
    }

    public class GBP
    {
        public string code { get; set; }
        public string symbol { get; set; }
        public string rate { get; set; }
        public string description { get; set; }
        public float rate_float { get; set; }
    }

    public class EUR
    {
        public string code { get; set; }
        public string symbol { get; set; }
        public string rate { get; set; }
        public string description { get; set; }
        public float rate_float { get; set; }
    }
    #endregion
}
