using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.IO;

namespace EventHorizon.Desktop.Blazor.Client.Configuration
{
    public static class ApplicationConfiguration
    {
        private static IConfigurationRoot configuration;

        public static IConfigurationRoot Build(
            ApplicationConfigurationOptions options
        )
        {
            if (configuration != null)
            {
                return configuration;
            }
            var builder = new ConfigurationBuilder()
                .SetBasePath(options.ContentRootPath)
                .AddJsonFile("desktopsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"desktopsettings.{options.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            configuration = builder.Build();
            return configuration;
        }

        public static IConfigurationRoot Configuration
        {
            get
            {
                return configuration ?? throw new ConfigurationNotInitialized();
            }
        }
    }
}
