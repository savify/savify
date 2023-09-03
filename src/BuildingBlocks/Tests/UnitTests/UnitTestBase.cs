using App.BuildingBlocks.Domain;
using NUnit.Framework;

namespace App.BuildingBlocks.Tests.UnitTests;

public abstract class UnitTestBase
{
    public static T AssertPublishedDomainEvent<T>(Entity aggregate) where T : IDomainEvent
    {
        var domainEvent = DomainEventsAccessor.GetAllDomainEvents(aggregate).OfType<T>().SingleOrDefault();

        Assert.That(domainEvent, Is.Not.Null, $"{typeof(T).Name} event not published");

        return domainEvent;
    }

    public void AssertBrokenRule<TRule>(TestDelegate testDelegate) where TRule : class, IBusinessRule
    {
        var message = $"Expected {typeof(TRule).Name} broken rule";
        var businessRuleValidationException = Assert.Catch<BusinessRuleValidationException>(testDelegate, message);

        if (businessRuleValidationException != null)
        {
            Assert.That(businessRuleValidationException.BrokenRule, Is.TypeOf<TRule>(), message);
        }
    }

    public void AssertBrokenRuleAsync<TRule>(AsyncTestDelegate testDelegate) where TRule : class, IBusinessRule
    {
        var message = $"Expected {typeof(TRule).Name} broken rule";
        var businessRuleValidationException = Assert.CatchAsync<BusinessRuleValidationException>(testDelegate, message);

        if (businessRuleValidationException != null)
        {
            Assert.That(businessRuleValidationException.BrokenRule, Is.TypeOf<TRule>(), message);
        }
    }
}
