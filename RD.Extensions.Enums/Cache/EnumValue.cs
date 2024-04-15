namespace RD.Extensions.Enums.Cache;

/// <summary>
/// Class to save a datatype and their value.
/// </summary>
public class EnumValue
{
    /// <summary>
    /// Type of the value that will be stored.
    /// </summary>
    public Type? Type { get; set; }

    /// <summary>
    /// Value that will be stored.
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    /// Whether the attribute could defined multiple times.
    /// </summary>
    public bool AllowMultiple { get; set; }
}
