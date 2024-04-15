namespace RD.Extensions.Enums.Attributes;

/// <summary>
/// Double value attribute for enums.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class DoubleValueAttribute : ValueBaseAttribute<double>
{
    /// <summary>
    /// Gets the double value for this attribute.
    /// </summary>
    public override double Value { get; }

    // <summary>
    /// Define whether the attribute can used multiple.
    /// </summary>
    public override bool AllowMultiple { get; }

    /// <summary>
    /// Constructor for the <see cref="DoubleValueAttribute"/>.
    /// </summary>
    /// <param name="value">Double value to save.</param>
    public DoubleValueAttribute(double value)
    {
        this.Value = value;
    }
}
