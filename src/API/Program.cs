using App.API.Configuration.Authorization;
using App.API.Configuration.ExecutionContext;
using App.API.Configuration.Localization;
using App.API.Configuration.Validation;
using App.API.Modules.UserAccess.Authentication;
using App.BuildingBlocks.Application;
using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Domain;
using App.BuildingBlocks.Infrastructure.Authentication;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.BuildingBlocks.Infrastructure.Localization;
using App.Modules.UserAccess.Application.Authentication.Exceptions;
using App.Modules.UserAccess.Infrastructure.Configuration;
using App.Modules.UserAccess.Infrastructure.IdentityServer;
using Destructurama;
using Hellang.Middleware.ProblemDetails;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Enrichers.Sensitive;

using ILogger = Serilog.ILogger;

namespace App.API;

public class Program
{
    private static ILogger _logger;
    
    private static ILogger _loggerForApi;
    
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<IServiceProvider>(provider => provider);
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();
        
        builder.Services.AddLocalization();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSingleton<ILocalizerFactory, JsonStringLocalizerFactory>();
        
        ConfigureLogger();
        ConfigureIdentityServer(builder);
        
        builder.Services.AddProblemDetails(x =>
        {
            x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
            x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
            x.Map<RepositoryException>(ex => new RepositoryExceptionProblemDetails(ex));
            x.Map<AuthenticationException>(ex => new AuthenticationExceptionProblemDetails(ex));
            x.Map<UserContextIsNotAvailableException>(ex => new UserContextIsNotAvailableProblemDetails(ex));
        });
        
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(HasPermissionAttribute.HasPermissionPolicyName, policyBuilder =>
            {
                policyBuilder.Requirements.Add(new HasPermissionAuthorizationRequirement());
                policyBuilder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            });
        });
        
        builder.Services.AddScoped<IAuthorizationHandler, HasPermissionAuthorizationHandler>();

        builder.Services.AddUserAccessModule(builder.Configuration, _logger);

        var app = builder.Build();

        app.UseCors(corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        
        var supportedCultures = new[] { "en", "ua" };
        var localizationOptions = new RequestLocalizationOptions()
            {
                ApplyCurrentCultureToResponseHeaders = true
            }
            .SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);

        app.UseRequestLocalization(localizationOptions);
        app.UseMiddleware<CorrelationMiddleware>();
        app.UseIdentityServer();
        app.UseProblemDetails();
        
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        if (!app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        
        app.Run();
    }
    
    private static void ConfigureLogger()
    {
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;

        _logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithSensitiveDataMasking()
            .Destructure.UsingAttributes()
            .Enrich.WithProperty("Environment", environment!)
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
        
        _loggerForApi = _logger.ForContext("Module", "API");
        _loggerForApi.Information("Logger configured");
    }
    
    private static void ConfigureIdentityServer(WebApplicationBuilder builder)
    {
        var authenticationConfiguration = builder.Configuration.GetSection("Authentication").Get<AuthenticationConfiguration>();
        
        builder.Services.AddIdentityServer()
            .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
            .AddInMemoryApiResources(IdentityServerConfig.GetApis(authenticationConfiguration))
            .AddInMemoryClients(IdentityServerConfig.GetClients(authenticationConfiguration))
            .AddInMemoryPersistedGrants()
            .AddProfileService<ProfileService>()
            .AddDeveloperSigningCredential();

        builder.Services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = authenticationConfiguration.Authority;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = authenticationConfiguration.Authority,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                };
            });

        builder.Services.AddSingleton<IAuthenticationConfigurationProvider>(
            _ => new AuthenticationConfigurationProvider(authenticationConfiguration));
    }
}
