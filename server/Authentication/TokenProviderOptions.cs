using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace InventoryManager.Authentication
{
    public class TokenProviderOptions
    {
        public static string Audience { get; } = "InventoryManagerAudience";
        public static string Issuer { get; } = "InventoryManager";
        public static SymmetricSecurityKey Key { get; } = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("InventoryManagerSecretSecurityKeyInventoryManager"));
        public static TimeSpan Expiration { get; } = TimeSpan.FromMinutes(10);
        public static SigningCredentials SigningCredentials { get; } = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
    }
}
