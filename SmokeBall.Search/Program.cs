using System;
using Microsoft.Extensions.DependencyInjection;
using SmokeBall.Search.Service;
using SmokeBall.Search.Service.Interfaces;

namespace SmokeBall.Search.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            const string smokeballUrl = "wwww.smokeball.com";
            try
            {
                //setup our DI
                var serviceProvider = new ServiceCollection()
                    .AddScoped<IHttpWebProxy, HttpWebProxy>()
                    .AddScoped<IHtmlParser, HtmlParser>()
                    .AddScoped<ISearchService, SearchService>()
                    .AddScoped<IHttpHandler, HttpClientHandler>()
                    .BuildServiceProvider();

                System.Console.WriteLine("Searching ...");
                var service = serviceProvider.GetService<ISearchService>();
                var page = service.FindRankingsAsync("conveyancing software", smokeballUrl).Result;
                System.Console.WriteLine($"SEO Rankings for {smokeballUrl}");
                System.Console.WriteLine(page);
                
            }
            catch (Exception exception)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine(exception.Message);
            }
            finally
            {
                System.Console.ReadLine();
            }
        }
    }
}
