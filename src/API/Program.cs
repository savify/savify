using System.IdentityModel.Tokens.Jwt;
using App.API.Configuration.Authorization;
using App.API.Configuration.ExecutionContext;
using App.API.Configuration.Localization;
using App.API.Configuration.Validation;
using App.BuildingBlocks.Application;
using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Domain;
using App.BuildingBlocks.Infrastructure.Configuration.Extensions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.BuildingBlocks.Infrastructure.Localization;
using App.Integrations.SaltEdge;
using App.Modules.Banks.Infrastructure.Configuration;
using App.Modules.Categories.Infrastructure.Configuration;
using App.Modules.Notifications.Infrastructure.Configuration;
using App.Modules.Transactions.Infrastructure.Configuration;
using App.Modules.UserAccess.Application.Authentication.Exceptions;
using App.Modules.UserAccess.Infrastructure.Configuration;
using App.Modules.Wallets.Infrastructure.Configuration;
using App.Modules.Wallets.Infrastructure.Integrations.Exceptions;
using Destructurama;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Serilog.Enrichers.Sensitive;
using Serilog.Sinks.Elasticsearch;
using ILogger = Serilog.ILogger;

namespace App.API;

public class Program
{
    private static ILogger _logger;

    private static ILogger _loggerForApi;

    private static LoggerConfiguration _loggerConfiguration;

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureLogger(builder.Configuration);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<IServiceProvider>(provider => provider);
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();

        builder.Services.AddLocalization();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSingleton<ILocalizerFactory, JsonStringLocalizerFactory>();

        builder.Services.AddProblemDetails(x =>
        {
            x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
            x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
            x.Map<RepositoryException>(ex => new RepositoryExceptionProblemDetails(ex));
            x.Map<AuthenticationException>(ex => new AuthenticationExceptionProblemDetails(ex));
            x.Map<UserContextIsNotAvailableException>(ex => new UserContextIsNotAvailableProblemDetails(ex));
            x.Map<ExternalProviderException>(ex => new ExternalProviderExceptionProblemDetails(ex));
        });

        builder.Services.AddUserAuthentication(builder.Configuration);
        builder.Services.AddScoped<IAuthorizationHandler, HasPermissionAuthorizationHandler>();
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(HasPermissionAttribute.HasPermissionPolicyName, policyBuilder =>
            {
                policyBuilder.Requirements.Add(new HasPermissionAuthorizationRequirement());
                policyBuilder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            });
        });

        builder.Services.AddEventBus();
        builder.Services.AddLogger(_logger);
        builder.Services.AddInternalProcessingServices();
        builder.Services.AddSqlConnectionFactory(builder.Configuration);
        builder.Services.AddSaltEdgeIntegration(builder.Configuration);

        builder.Services.AddUserAccessModule(builder.Configuration, _logger);
        builder.Services.AddNotificationsModule(builder.Configuration, _logger);
        builder.Services.AddWalletsModule(builder.Configuration, _logger);
        builder.Services.AddBanksModule(builder.Configuration, _logger, builder.Environment.IsProduction());
        builder.Services.AddCategoriesModule(builder.Configuration, _logger);
        builder.Services.AddTransactionsModule(builder.Configuration, _logger);

        var app = builder.Build();

        // TODO: change for production
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

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        app.UseAuthentication();
        app.UseAuthorization();

#pragma warning disable ASP0014
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
#pragma warning restore ASP0014

        app.Run();
    }

    private static void ConfigureLogger(IConfiguration configuration)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;
        var logTemplate = "[{Environment}] [{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level:u3}] [CorrelationId:{CorrelationId}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}";

        _loggerConfiguration = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithSensitiveDataMasking()
            .Destructure.UsingAttributes()
            .Enrich.WithProperty("Environment", environment)
            .WriteTo.Console(outputTemplate: logTemplate)
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["ElasticSearch:Uri"]!))
            {
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                IndexFormat = $"savify-{environment.ToLower().Replace(".", "-")}"
            });

        _logger = _loggerConfiguration.CreateLogger();

        _loggerForApi = _logger.ForContext("Module", "API");
        _loggerForApi.Information("Logger configured");
    }
}
