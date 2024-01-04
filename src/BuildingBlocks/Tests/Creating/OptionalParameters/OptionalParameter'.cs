namespace App.BuildingBlocks.Tests.Creating.OptionalParameters;

/// <summary>
/// Simplifies methods with optional parameters.
/// 'default' .Net keyword will return an optional parameter without a value. And will identify the optional parameter as is not set. The GetValueOr() method will return an alternative value.
/// Use OptionalParameter.Default to set the T default value. The optional parameter will be identified as set. The GetValueOr() method will return an optional parameter value.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct OptionalParameter<T>
{
    private readonly T _value;
    private readonly bool _hasValue;

    public OptionalParameter(T value)
    {
        _value = value;
        _hasValue = true;
    }

    public OptionalParameter()
    {
        _value = default!;
        _hasValue = false;
    }

    private OptionalParameter(DefaultValue _)
    {
        _value = default!;
        _hasValue = true;
    }

    public T GetValueOr(T alternativeValue) => _hasValue ? _value : alternativeValue;

    public static implicit operator OptionalParameter<T>(T value) => new(value);
    public static implicit operator OptionalParameter<T>(DefaultValue defaultValue) => new(defaultValue);
}
