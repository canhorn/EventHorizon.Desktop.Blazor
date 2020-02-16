namespace EventHorizon.Desktop.Blazor.Client.Configuration
{
    public struct ApplicationConfigurationOptions
    {
        public string ContentRootPath { get; }
        public string EnvironmentName { get; }

        public ApplicationConfigurationOptions(
            string contentRootPath,
            string environmentName
        )
        {
            ContentRootPath = contentRootPath;
            EnvironmentName = environmentName;
        }
    }
}
