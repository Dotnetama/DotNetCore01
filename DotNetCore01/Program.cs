using DotNetCore01.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace DotNetCore01
{
    internal class Program
    {
        // block body vs expression body
        // 블록 vs 식
        static Task Main(string[] args) =>
            CreateHostBuilder(args).Build().RunAsync();

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostBuilderContext, services) =>
                    services.AddHostedService<WorkerService>()
                            .AddScoped<IBookmark, BookmarkService>());
    }
}
