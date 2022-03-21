using System;
using Chilly.Clients;
using Chilly.Models;
using Chilly;

namespace ChillyCgi
{
    class Program
    {
        static void Main(string[] args)
        {
            IWeatherClient client = ClientForge.ConfigureWeatherClient();

            ILocaleClient localeClient = new FreeIpApiClient();

            string ip = Environment.GetEnvironmentVariable("REMOTE_ADDR") ?? "";

            GeoLocale locale = localeClient.GetIPLocale(ip);

            var forecast = client.GetForecast(locale);
            
            Console.Write("20 text/gemini\r\n");

            Renderer renderer = new Renderer(Console.Out);
            renderer.Render(forecast);

            int x = 4;
        }
    }
}
