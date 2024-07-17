using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServerAspNetIdentity.Data.IdentityConfiguration;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = "FE2EBE31-6E9E-436F-A247-C431EC225231",
                Name = "Patient",
                NormalizedName = "PATIENT"
            },
            new IdentityRole
            {
                Id = "80719001-2BCD-4043-A9A7-4290D59ED73C",
                Name = "Doctor",
                NormalizedName = "DOCTOR"
            },
            new IdentityRole
            {
                Id = "C500EDA0-6BEA-4222-8689-B3924C070CA2",
                Name = "Receptionist",
                NormalizedName = "RECEPTIONIST"
            });
    }
}
