namespace RD.Extensions.Enums.Attributes;

/// <summary>
/// Base class for all value attributes to identify the attributes
/// in the enum cache.
/// </summary>
public abstract class ValueBaseAttribute<TType> : Attribute
{
    public abstract TType Value { get; }

    /// <summary>
    /// Define whether the attribute can used multiple times on the same enum value.
    /// </summary>
    public abstract bool AllowMultiple { get; }
}
