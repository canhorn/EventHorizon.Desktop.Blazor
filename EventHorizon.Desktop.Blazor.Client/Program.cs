
using EventHorizon.Desktop.Blazor.Client.Configuration;
using EventHorizon.Desktop.Blazor.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using WebWindows;

namespace EventHorizon.Desktop.Blazor.Client
{
    class Program
    {
        static IHost host;
        static void Main(string[] args)
        {
            Console.WriteLine("Main Window Startup");
            // This builds the configuration 
            var configuration = ApplicationConfiguration.Build(
                new ApplicationConfigurationOptions(
                    Path.GetDirectoryName(
                        Process.GetCurrentProcess().MainModule.FileName
                    ),
                    "Development"
                )
            );

            var serverUrl = configuration["ServerUrl"];
            // Check ServerUrl in Configuration
            if (string.IsNullOrEmpty(serverUrl))
            {
                // No Configured ServerUrl, create one.
                serverUrl = "http://localhost:8080";
                StartServer(
                    args,
                    serverUrl,
                    configuration
                );
            }

            // Build WebWindow with Configuration Title
            var window = new WebWindow(
                configuration["Title"]
            );
            // Point the WebWindow at the ServerUrl
            window.NavigateToUrl(
                serverUrl
            );
            // Stop the current thread, waiting on Window to be closed/exit.
            window.WaitForExit();
            host?.Dispose();

            Console.WriteLine("Main Window Closed.");
        }

        private static void StartServer(
            string[] args,
            string serverUrl,
            IConfigurationRoot configuration
        )
        {
            // Start Server in background thread.
            // This way it does not stop the rest of the application from starting/showing window.
            Task.Run(() =>
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                Console.WriteLine(pathToExe);
                Console.WriteLine(pathToContentRoot);

                Array.Resize(ref args, args.Length + 1);
                // This takes the configuration Environment, can be used for debugging.
                args[args.GetUpperBound(0)] = $"--environment={configuration["Environment"]}";
                Array.Resize(ref args, args.Length + 1);
                // Start the backend server with this ServerUrl 
                args[args.GetUpperBound(0)] = $"URLS={serverUrl}";

                host = CreateHostBuilder(args, pathToContentRoot).Build();
                host.Start();
                host.WaitForShutdown();
            });
        }

        public static IHostBuilder CreateHostBuilder(string[] args, string contentRoot) =>
            Host.CreateDefaultBuilder(args)
                .UseContentRoot(contentRoot)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        // Make sure to set the content root to this startup location.
                        // Might not be needed when no static resources are provided by the server.
                        .UseWebRoot(Path.Combine(contentRoot, "wwwroot"))
                        .UseStaticWebAssets()
                    ;
                });
    }
}