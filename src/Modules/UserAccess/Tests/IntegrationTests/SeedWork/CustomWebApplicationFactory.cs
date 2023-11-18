using App.BuildingBlocks.Application;
using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using App.Modules.UserAccess.Application.PasswordResetRequests.RequestPasswordReset;
using App.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration;
using App.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;
using App.Modules.UserAccess.Application.UserRegistrations.RenewUserRegistration;
using App.Modules.UserAccess.Application.Users.CreateNewUser;
using App.Modules.UserAccess.Domain.PasswordResetRequest.Events;
using App.Modules.UserAccess.Domain.UserRegistrations.Events;
using App.Modules.UserAccess.Domain.Users.Events;
using App.Modules.UserAccess.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace App.Modules.UserAccess.IntegrationTests.SeedWork;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.Replace(ServiceDescriptor.Scoped<IExecutionContextAccessor>(_ => new ExecutionContextMock(Guid.NewGuid())));

            // TODO: find some solution to work with domain notifications maps without duplication in tests!
            var domainNotificationsMap = new BiDictionary<string, Type>();

            domainNotificationsMap.Add(nameof(UserCreatedDomainEvent), typeof(UserCreatedNotification));
            domainNotificationsMap.Add(nameof(NewUserRegisteredDomainEvent), typeof(NewUserRegisteredNotification));
            domainNotificationsMap.Add(nameof(UserRegistrationConfirmedDomainEvent), typeof(UserRegistrationConfirmedNotification));
            domainNotificationsMap.Add(nameof(UserRegistrationRenewedDomainEvent), typeof(UserRegistrationRenewedNotification));
            domainNotificationsMap.Add(nameof(PasswordResetRequestedDomainEvent), typeof(PasswordResetRequestedNotification));

            services.Replace(ServiceDescriptor.Scoped<IDomainNotificationsMapper<UserAccessContext>>(_ => new DomainNotificationsMapper<UserAccessContext>(domainNotificationsMap)));
        });
    }
}
