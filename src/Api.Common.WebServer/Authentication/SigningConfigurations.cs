using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Api.Common.WebServer.Authentication
{
    public class SigningConfigurations
    {
        public SigningConfigurations()
        {
            var keyBytes = Encoding.ASCII.GetBytes("Api.Template.Authentication");
            var signingKey = new SymmetricSecurityKey(keyBytes);

            Key = signingKey;

            SigningCredentials = new SigningCredentials(
                Key, SecurityAlgorithms.HmacSha256);
        }

        public SecurityKey Key { get; }
        public SigningCredentials SigningCredentials { get; }
    }
}