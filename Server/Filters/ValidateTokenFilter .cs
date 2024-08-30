using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;

namespace Server.Filters
{
    public class ValidateTokenFilter : IActionFilter
    {
        private readonly SymmetricSecurityKey _signingKey;

        public ValidateTokenFilter(SymmetricSecurityKey signingKey)
        {
            _signingKey = signingKey;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                var principal = ValidateToken(token);

                if (principal == null)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                context.HttpContext.User = principal;
            }
            else
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
           
        }

        private ClaimsPrincipal ValidateToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
                var jwtToken = validatedToken as JwtSecurityToken;

                if (jwtToken == null || !jwtToken.Claims.Any(x => x.Type == "role" && x.Value == "Admin"))
                    return null;

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
