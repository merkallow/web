using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Merkallow.Web.ViewModels
{
    public class Token
    {
        public string AccessToken { get; set; }
    }

    public class TokenPayload
    {
        public long Id { get; set; }
        public string PublicAddress { get; set; }
    }

    public class TokenData
    {
        public TokenPayload Payload { get; set; }
        public DateTime IssuedAt { get; set; }
    }

    public static class TokenExtensions
    {
        public static TokenData GetTokenFromJwt(this string cookie)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(cookie);
            var claim = jwt.Claims.First(o => o.Type == "payload").Value;
            var payload = JsonConvert.DeserializeObject<TokenPayload>(claim);

            return new TokenData()
            {
                IssuedAt = jwt.IssuedAt,
                Payload = payload
            };
        }
    }
}
