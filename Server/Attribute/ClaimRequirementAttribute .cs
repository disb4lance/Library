using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

public class ClaimRequirementAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    private readonly string _claimType;
    private readonly string _claimValue;

    public ClaimRequirementAttribute(string claimType, string claimValue)
    {
        _claimType = claimType;
        _claimValue = claimValue;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        // Проверка наличия нужного клеймаа
        var claim = user.Claims.FirstOrDefault(c => c.Type == _claimType && c.Value == _claimValue);
        if (claim == null)
        {
            context.HttpContext.Response.StatusCode = 403;
            return;
        }
    }
}
