using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace UserManagementPanel.Data;

public class ApplicationUserClaimsTransformation : IClaimsTransformation
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDbContext _dbContext;

    public ApplicationUserClaimsTransformation(
        UserManager<AppUser> userManager,
        ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }


    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = principal.Identities.FirstOrDefault(c => c.IsAuthenticated);
        if (identity == null) return principal;

        var user = await _userManager.GetUserAsync(principal);
        if (user == null) return principal;
        
        user.LastLoginTime = DateTime.UtcNow;
        _dbContext.Users.Update(user);
        _dbContext.SaveChanges();
        // Add or replace identity.Claims.
        identity.AddClaim(new Claim("FullName", user.FullName, ClaimValueTypes.String));

        return new ClaimsPrincipal(identity);
    }
}
