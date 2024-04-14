using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Authentication.Methods;
public interface ITokenContents
{
    long GetUserId(ClaimsPrincipal User);

    ICollection<int> GetUserBaseRoles(ClaimsPrincipal User);
}

public class TokenContents : ITokenContents
{
    public long GetUserId(ClaimsPrincipal User)
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;
        long userId = long.Parse(claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return userId;
    }

    public ICollection<int> GetUserBaseRoles(ClaimsPrincipal User)
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;
        var roles = claimsIdentity!.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

        return roles.Select(int.Parse).ToList();
    }
}

