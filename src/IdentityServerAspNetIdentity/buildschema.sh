#!/bin/bash

rm -rf "Data/Migrations"

dotnet ef migrations add Users -c ApplicationUserDbContext -o Data/Migrations/IdentityServer/UsersDb

dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb

Update-Database -Context ConfigurationDbContext