using NUnit.Framework;

namespace App.BuildingBlocks.Tests.ArchTests;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true)]
public class ArchTestAttribute : CategoryAttribute;
