
namespace RD.Extensions.Enums.Attributes;

/// <summary>
/// Key value pair attribute for enums.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class KeyValuePairAttribute : ValueBaseAttribute<KeyValuePair<string, object>>
{
    public override KeyValuePair<string, object> Value { get; }

    /// <summary>
    /// Define whether the attribute can used multiple.
    /// </summary>
    public override bool AllowMultiple { get; } = true;

    /// <summary>
    /// Constructor for the <see cref="KeyValuePairAttribute"/>.
    /// </summary>
    /// <param name="key">Key that identify the value.</param>
    /// <param name="value">Value to save.</param>
    public KeyValuePairAttribute(string key, object value)
    {
        this.Value = new(key, value);
    }
}
