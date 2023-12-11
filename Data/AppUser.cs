using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace UserManagementPanel.Data;

public class AppUser : IdentityUser
{
    public string FullName { get; set; } = null!;

    public DateTime LastLoginTime { get; set; }

    public DateTime RegistrationTime { get; set; }

    public string BlockedStatus => LockoutEnabled ? "Blocked" : "Active";
}