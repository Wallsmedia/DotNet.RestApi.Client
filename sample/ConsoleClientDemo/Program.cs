using System;
using System.Net.Http;
using DotNet.RestApi.Client;
using WebRestApiAbstractions.Market;

namespace ConsoleClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test DotNet.RespApi.Client === ");

            Uri baseUri = new Uri("http://localhost:5000/");
            RestApiClient client = new RestApiClient(baseUri);

            Uri relUri = new Uri("rest-api/WeatherForecast", UriKind.Relative);
            HttpResponseMessage response = client.SendJsonRequest(HttpMethod.Get, relUri, null).Result;
            var respStr = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine($" ===> {relUri} ====>");
            Console.WriteLine(respStr);


            relUri = new Uri("rest-api/market/ok", UriKind.Relative);
            response = client.SendJsonRequest(HttpMethod.Get, relUri, null).Result;
            respStr = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine($" ===> {relUri} ====>");
            Console.WriteLine(respStr);


            relUri = new Uri("rest-api/market/fault", UriKind.Relative);
            response = client.SendJsonRequest(HttpMethod.Get, relUri, null).Result;
            respStr = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine($" ===> {relUri} ====>");
            Console.WriteLine(respStr);

            var marketRequest = new TickerBitcoinRequest();

            relUri = new Uri(RequestPathAttribute.GetRestApiPath(marketRequest), UriKind.Relative);
            response = client.SendJsonRequest(HttpMethod.Post, relUri, marketRequest).Result;
            respStr = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine($" ===> {relUri} ====>");
            Console.WriteLine(respStr);


            relUri = new Uri(RequestPathAttribute.GetRestApiPath(marketRequest)+"/fault", UriKind.Relative);
            response = client.SendJsonRequest(HttpMethod.Post, relUri, marketRequest).Result;
            respStr = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine($" ===> {relUri} ====>");
            Console.WriteLine(respStr);


            Console.ReadKey();

        }
    }
}
