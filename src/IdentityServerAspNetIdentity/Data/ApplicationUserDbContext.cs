using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityServerAspNetIdentity.Models;
using IdentityServerAspNetIdentity.Data.IdentityConfiguration;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;

namespace IdentityServerAspNetIdentity.Data;

public class ApplicationUserDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationUserDbContext(DbContextOptions<ApplicationUserDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.ApplyConfiguration(new RoleConfiguration());
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new UserRoleConfiguration());

        base.OnModelCreating(builder);
    }
}
