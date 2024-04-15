namespace RD.Extensions.Enums.Attributes;

/// <summary>
/// String value attribute for enums.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class StringValueAttribute : ValueBaseAttribute<string>
{
    /// <summary>
    /// Gets the string value for this attribute.
    /// </summary>
    public override string Value { get; }

    /// <summary>
    /// Define whether the attribute can used multiple times on the same enum value.
    /// </summary>
    public override bool AllowMultiple { get; }

    /// <summary>
    /// Constructor for the <see cref="StringValueAttribute"/>.
    /// </summary>
    /// <param name="value">String value to save.</param>
    public StringValueAttribute(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value));
        }

        this.Value = value;
    }
}
