using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServerAspNetIdentity.Data.IdentityConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();

        var admin = new ApplicationUser
        {
            Id = "C4AB4EEC-4CDF-438E-961A-6AB0C2C3DD6D",
            Email = "innoclinic.admin@mail.com",
            NormalizedEmail = "innoclinic.admin@mail.com".ToUpper(),
            EmailConfirmed = true,
            UserName = "innoclinic.admin@mail.com",
            NormalizedUserName = "innoclinic.admin@mail.com".ToUpper(),
        };

        admin.PasswordHash = ph.HashPassword(admin, "1234");

        builder.HasData(admin);
    }
}
