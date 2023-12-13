using System.Globalization;
using System.Text;
using App.BuildingBlocks.Infrastructure.Data.NamingConventions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace App.BuildingBlocks.Infrastructure.Data.NamingConventions;

public class NamingConventionsOptionsExtension : IDbContextOptionsExtension
{
    private DbContextOptionsExtensionInfo? _info;

    private NamingConvention _namingConvention;

    private CultureInfo? _culture;

    public NamingConventionsOptionsExtension() {}

    public DbContextOptionsExtensionInfo Info => _info ??= new ExtensionInfo(this);

    internal NamingConvention NamingConvention => _namingConvention;

    internal CultureInfo? Culture => _culture;

    private NamingConventionsOptionsExtension Clone() => new(this);

    private NamingConventionsOptionsExtension(NamingConventionsOptionsExtension copyFrom)
    {
        _namingConvention = copyFrom._namingConvention;
        _culture = copyFrom._culture;
    }

    public NamingConventionsOptionsExtension WithSnakeCaseNamingConvention(CultureInfo? culture = null)
    {
        var clone = Clone();

        clone._namingConvention = NamingConvention.SnakeCase;
        clone._culture = culture;

        return clone;
    }

    public void Validate(IDbContextOptions options) {}

    public void ApplyServices(IServiceCollection services) => services.AddEntityFrameworkNamingConventions();

    private sealed class ExtensionInfo(IDbContextOptionsExtension extension) : DbContextOptionsExtensionInfo(extension)
    {
        private string? _logFragment;

        private new NamingConventionsOptionsExtension Extension => (NamingConventionsOptionsExtension)base.Extension;

        public override bool IsDatabaseProvider => false;

        public override string LogFragment
        {
            get
            {
                if (_logFragment == null)
                {
                    var builder = new StringBuilder();

                    builder.Append(Extension._namingConvention switch
                    {
                        NamingConvention.SnakeCase => "using snake-case naming ",
                        _ => throw new ArgumentOutOfRangeException("Unhandled enum value: " + Extension._namingConvention)
                    });

                    if (Extension._culture is null)
                    {
                        builder
                            .Append(" (culture=")
                            .Append(Extension._culture)
                            .Append(')');
                    }

                    _logFragment = builder.ToString();
                }

                return _logFragment;
            }
        }

        public override int GetServiceProviderHashCode()
        {
            var hashCode = Extension._namingConvention.GetHashCode();

            hashCode = (hashCode * 3) ^ (Extension._culture?.GetHashCode() ?? 0);

            return hashCode;
        }

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) => other is ExtensionInfo;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
            debugInfo["Naming:UseNamingConvention"] = Extension._namingConvention.GetHashCode().ToString(CultureInfo.InvariantCulture);

            if (Extension._culture is not null)
            {
                debugInfo["Naming:Culture"] = Extension._culture.GetHashCode().ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
