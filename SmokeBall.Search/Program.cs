using Microsoft.Extensions.DependencyInjection;
using SmokeBall.Search.Service;
using SmokeBall.Search.Service.Interfaces;

namespace SmokeBall.Search.Console
{
    class Program
    {
        static void Main(string[] args)
        {

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddScoped<IHttpWebProxy, HttpWebProxy>()
                .AddScoped<IHtmlParser, HtmlParser>()
                .AddScoped<ISearchService, SearchService>()
                .AddScoped<IHttpHandler, HttpClientHandler>()
                .BuildServiceProvider();
            

            System.Console.WriteLine("Hello World!");
            var service = serviceProvider.GetService<ISearchService>();
            var page = service.FindRankingsAsync("health+insurance", "www.hcf.com").Result;
            

            System.Console.ReadLine();
        }


    }
}
