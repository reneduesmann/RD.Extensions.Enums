namespace RD.Extensions.Enums.Attributes;

/// <summary>
/// Long value attribute for enums.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class LongValueAttribute : ValueBaseAttribute<long>
{
    /// <summary>
    /// Gets the long value for this attribute.
    /// </summary>
    public override long Value { get; }

    /// <summary>
    /// Define whether the attribute can used multiple.
    /// </summary>
    public override bool AllowMultiple { get; }

    /// <summary>
    /// Constructor for the <see cref="LongValueAttribute"/>.
    /// </summary>
    /// <param name="value">Long value to save.</param>
    public LongValueAttribute(long value)
    {
        this.Value = value;
    }
}
