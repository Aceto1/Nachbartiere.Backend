using System;
using System.IdentityModel.Tokens.Jwt;

namespace Nachbartiere.Backend.Helper
{
    public static class JWTHelper
    {
        private static readonly JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        public static bool IsTokenExpired(string accessToken)
        {
            var token = tokenHandler.ReadJwtToken(accessToken);

            return token.ValidTo < DateTime.UtcNow;
        }
    }
}
