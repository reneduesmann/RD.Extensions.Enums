namespace RD.Extensions.Enums.Attributes;

/// <summary>
/// Boolean value attribute for enums.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class BooleanValueAttribute : ValueBaseAttribute<bool>
{
    /// <summary>
    /// Gets the boolean value for this attribute.
    /// </summary>
    public override bool Value { get; }

    /// <summary>
    /// Define whether the attribute can used multiple.
    /// </summary>
    public override bool AllowMultiple { get; }

    /// <summary>
    /// Constructor for the <see cref="BooleanValueAttribute"/>.
    /// </summary>
    /// <param name="value">Boolean value to save.</param>
    public BooleanValueAttribute(bool value)
    {
        this.Value = value;
    }
}
