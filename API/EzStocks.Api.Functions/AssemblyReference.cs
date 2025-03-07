using System.Reflection;

namespace EzStocks.Api.Functions
{
    public static class AssemblyReference
    {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}
