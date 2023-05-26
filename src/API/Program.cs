using App.API.Configuration.ExecutionContext;
using App.API.Configuration.Localization;
using App.API.Configuration.Validation;
using App.BuildingBlocks.Application;
using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Domain;
using App.BuildingBlocks.Infrastructure.Authentication;
using App.BuildingBlocks.Infrastructure.Data;
using App.BuildingBlocks.Infrastructure.Emails;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.BuildingBlocks.Infrastructure.Localization;
using App.Modules.UserAccess.Infrastructure;
using App.Modules.UserAccess.Infrastructure.Configuration;
using Destructurama;
using Hellang.Middleware.ProblemDetails;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Serilog;
using Serilog.Enrichers.Sensitive;
using Serilog.Sinks.Elasticsearch;

using ILogger = Serilog.ILogger;

namespace App.API;

public class Program
{
    private static ILogger _logger;
    
    private static ILogger _loggerForApi;
    
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureLogger(builder.Configuration);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();
        
        // ConfigureIdentityServer(builder);
        
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
            // x.Map<AuthenticationException>(ex => new AuthenticationExceptionProblemDetails(ex));
            x.Map<UserContextIsNotAvailableException>(ex => new UserContextIsNotAvailableProblemDetails(ex));
        });
        
        // builder.Services.AddAuthorization(options =>
        // {
        //     options.AddPolicy(HasPermissionAttribute.HasPermissionPolicyName, policyBuilder =>
        //     {
        //         policyBuilder.Requirements.Add(new HasPermissionAuthorizationRequirement());
        //         policyBuilder.AddAuthenticationSchemes(IdentityServerAuthenticationDefaults.AuthenticationScheme);
        //     });
        // });
        
        // builder.Services.AddScoped<IAuthorizationHandler, HasPermissionAuthorizationHandler>();

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
        
        // var container = app.Services.GetAutofacRoot();
        // InitializeModules(container, app.Configuration);
        
        app.UseMiddleware<CorrelationMiddleware>();
        
        // app.UseIdentityServer();
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
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        
        app.Run();
    }
    
    private static void ConfigureLogger(IConfigurationRoot configuration)
    {
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;

        _logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithSensitiveDataMasking()
            .Destructure.UsingAttributes()
            .Enrich.WithProperty("Environment", environment!)
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
            // .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"])){
            //     AutoRegisterTemplate = true,
            //     AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
            //     IndexFormat = $"{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            // })
            .CreateLogger();
        
        _loggerForApi = _logger.ForContext("Module", "API");
        _loggerForApi.Information("Logger configured");
    }
}
