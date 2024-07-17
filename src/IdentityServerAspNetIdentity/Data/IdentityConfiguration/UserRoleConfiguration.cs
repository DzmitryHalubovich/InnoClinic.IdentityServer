using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServerAspNetIdentity.Data.IdentityConfiguration;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData(
            new IdentityUserRole<string>
            {               
                UserId = "C4AB4EEC-4CDF-438E-961A-6AB0C2C3DD6D",
                RoleId = "C500EDA0-6BEA-4222-8689-B3924C070CA2"
            }); 
    }
}
