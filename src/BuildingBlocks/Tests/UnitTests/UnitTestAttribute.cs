using NUnit.Framework;

namespace App.BuildingBlocks.Tests.UnitTests;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true)]
public class UnitTestAttribute : CategoryAttribute;
