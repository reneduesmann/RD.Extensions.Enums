namespace RD.Extensions.Enums.Attributes;

/// <summary>
/// Integer value attribute for enums.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class IntegerValueAttribute : ValueBaseAttribute<int>
{
    /// <summary>
    /// Gets the integer value for this attribute.
    /// </summary>
    public override int Value { get; }

    /// <summary>
    /// Define whether the attribute can used multiple.
    /// </summary>
    public override bool AllowMultiple { get; }

    /// <summary>
    /// Constructor for the <see cref="IntegerValueAttribute"/>.
    /// </summary>
    /// <param name="value">Integer value to save.</param>
    public IntegerValueAttribute(int value)
    {
        this.Value = value;
    }
}
