using System.Reflection;

namespace EzStocks.Api.Domain
{
    public static class AssemblyReference
    {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}
