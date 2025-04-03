using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using System.Reflection;

namespace EzStocks.Api.Infrastructure.UnitTests.Helpers
{
    public static class FileHelpers
    {
        public static async Task<string> GetFileContentAsync(string filePath, CancellationToken cancellationToken = default)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var path = Path.Join(Path.GetDirectoryName(assembly.GetAssemblyLocation()));
            var txt = await File.ReadAllTextAsync(Path.Join(path, filePath));
            return txt;
        }
    }
}
