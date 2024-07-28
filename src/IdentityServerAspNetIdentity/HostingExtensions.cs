using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using IdentityServerAspNetIdentity.Data;
using IdentityServerAspNetIdentity.Data.Migrations;
using IdentityServerAspNetIdentity.MassTransit;
using IdentityServerAspNetIdentity.Models;
using IdentityServerAspNetIdentity.Profile;
using IdentityServerAspNetIdentity.RabbitMQ;
using InnoClinic.SharedModels.MQMessages.Profiles;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityServerAspNetIdentity;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        builder.Services.AddSingleton<IRabbitMqConnection>(new RabbitMqConnection(builder.Configuration));
        builder.Services.AddScoped<IMessageProducer, MessageProducer>();

        builder.Services.AddAutoMapper(typeof(MapperProfile));

        //Nessessary for Api endpoints
        builder.Services.AddLocalApiAuthentication();

        builder.Services.AddDbContext<ApplicationUserDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("UserContext")));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;

                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationUserDbContext>()
            .AddDefaultTokenProviders();

        var migrationsAssembly = typeof(Program).Assembly.GetName().Name;
        var serverConfigurationConnectionString = builder.Configuration.GetConnectionString("ServerConfigurationContext");

        builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                //Because this shit don't want to work in production without money. Default = true
                options.KeyManagement.Enabled = false;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;

            })
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlServer(serverConfigurationConnectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlServer(serverConfigurationConnectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddAspNetIdentity<ApplicationUser>();
        
        builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                // register your IdentityServer with Google at https://console.developers.google.com
                // enable the Google+ API
                // set the redirect URI to https://localhost:5001/signin-google

                options.ClientId = builder.Configuration["GoogleClientId"];
                options.ClientSecret = builder.Configuration["GoogleClientSecret"];
            });


        builder.Services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.SetInMemorySagaRepositoryProvider();

            var assembly = typeof(Program).Assembly;

            x.AddConsumer<WorkerProfileCreatedConsumer>();

            x.AddSagaStateMachines(assembly);
            x.AddSagas(assembly);
            x.AddActivities(assembly);

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("worker-profile-created", queueConfigurator =>
                {
                    queueConfigurator.Consumer<WorkerProfileCreatedConsumer>(context);
                });
            });
        });

        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    { 
        app.UseSerilogRequestLogging();
    
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();

        app.MapRazorPages()
            .RequireAuthorization();

        app.MigrateDatabaseAsync();

        return app;
    }
}