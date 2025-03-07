using EzStocks.Api.Application.Security;

namespace EzStocks.Api.Functions.Security
{
    public class UserContext : IUserContext
    {
        public Guid UserId => new Guid("b8343eaf-6c20-4a40-bada-168ff5ceec89");
    }
}
