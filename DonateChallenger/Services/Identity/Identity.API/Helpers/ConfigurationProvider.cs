using System.IO;

namespace Identity.API.Helpers
{
    public static class ConfigurationProvider
    {
        public static IConfiguration GetConfiguration(string directoryPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(Path.Combine(directoryPath))
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
    }   
}