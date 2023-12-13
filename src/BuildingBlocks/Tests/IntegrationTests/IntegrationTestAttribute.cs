using NUnit.Framework;

namespace App.BuildingBlocks.Tests.IntegrationTests;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true)]
public class IntegrationTestAttribute : CategoryAttribute;
