using System.Data;
using App.API;
using App.BuildingBlocks.Domain;
using App.BuildingBlocks.Infrastructure.Configuration;
using App.BuildingBlocks.Tests.IntegrationTests.Probing;
using App.Database.Scripts.Clear;
using App.IntegrationTests.SeedData;
using App.Modules.Banks.Application.Contracts;
using App.Modules.Banks.Infrastructure;
using App.Modules.Categories.Application.Contracts;
using App.Modules.Categories.Infrastructure;
using App.Modules.FinanceTracking.Application.Contracts;
using App.Modules.FinanceTracking.Infrastructure;
using App.Modules.Notifications.Application.Contracts;
using App.Modules.Notifications.Application.Emails;
using App.Modules.Notifications.Infrastructure;
using App.Modules.UserAccess.Application.Contracts;
using App.Modules.UserAccess.Infrastructure;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace App.IntegrationTests.SeedWork;

public class TestBase
{
    protected const int TestExecutionTimeout = 30000;

    protected CustomWebApplicationFactory<Program> WebApplicationFactory { get; private set; }

    protected IBanksModule BanksModule { get; private set; }

    protected IUserAccessModule UserAccessModule { get; private set; }

    protected INotificationsModule NotificationsModule { get; private set; }

    protected ICategoriesModule CategoriesModule { get; private set; }

    protected IFinanceTrackingModule FinanceTrackingModule { get; private set; }

    protected SaltEdgeHttpClientMocker SaltEdgeHttpClientMocker { get; private set; }

    protected IEmailSender EmailSender { get; private set; }

    protected string ConnectionString { get; private set; }

    [OneTimeSetUp]
    public async Task Init()
    {
        SaltEdgeHttpClientMocker = new SaltEdgeHttpClientMocker();
        EmailSender = new EmailSenderMock();

        WebApplicationFactory = await CustomWebApplicationFactory<Program>.Create(EmailSender, SaltEdgeHttpClientMocker.BaseUrl);
        CompositionRoot.SetServiceProvider(WebApplicationFactory.Services);

        ConnectionString = WebApplicationFactory.GetConnectionString();

        using var scope = WebApplicationFactory.Services.CreateScope();

        BanksModule = scope.ServiceProvider.GetRequiredService<IBanksModule>();
        UserAccessModule = scope.ServiceProvider.GetRequiredService<IUserAccessModule>();
        NotificationsModule = scope.ServiceProvider.GetRequiredService<INotificationsModule>();
        CategoriesModule = scope.ServiceProvider.GetRequiredService<ICategoriesModule>();
        FinanceTrackingModule = scope.ServiceProvider.GetRequiredService<IFinanceTrackingModule>();

        await MigrateDb<BanksContext>(scope);
        await MigrateDb<CategoriesContext>(scope);
        await MigrateDb<FinanceTrackingContext>(scope);
        await MigrateDb<NotificationsContext>(scope);
        await MigrateDb<UserAccessContext>(scope);
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        SaltEdgeHttpClientMocker.StopWireMockServer();
        await WebApplicationFactory.DisposeAsync();
    }

    [SetUp]
    public async Task BeforeEachTest()
    {
        await using var sqlConnection = new NpgsqlConnection(ConnectionString);
        await ClearDatabase(sqlConnection);
    }

    protected static async Task AssertEventually(IProbe probe, int timeout = TestExecutionTimeout)
    {
        await new Poller(timeout).CheckAsync(probe);
    }

    protected static void AssertBrokenRule<TRule>(AsyncTestDelegate testDelegate) where TRule : class, IBusinessRule
    {
        var message = $"Expected {typeof(TRule).Name} broken rule";
        var businessRuleValidationException =
            Assert.CatchAsync<BusinessRuleValidationException>(testDelegate, message);

        if (businessRuleValidationException != null)
        {
            Assert.That(businessRuleValidationException.BrokenRule, Is.TypeOf<TRule>(), message);
        }
    }

    private static async Task ClearDatabase(IDbConnection connection)
    {
        var sql = await ClearDatabaseCommandProvider.GetAsync();

        await connection.ExecuteScalarAsync(sql);
    }

    private async Task MigrateDb<TContext>(IServiceScope scope) where TContext : DbContext
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        await dbContext.Database.MigrateAsync();
    }
}
