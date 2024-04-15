using RD.Extensions.Enums.Cache;

namespace RD.Extensions.Enums.Contracts;

/// <summary>
/// Caching system for enum that operates with the underlying attributes of <see cref="ValueBaseAttribute{TDataType}"/>.
/// </summary>
public interface IEnumCache
{
    /// <summary>
    /// Get the boolean value for the <paramref name="enumInput"/>.
    /// </summary>
    /// <param name="enumInput">Value that will be used for searching the enum value.</param>
    /// <returns>Boolean value that is stored in the attribute.</returns>
    bool GetBooleanValue(Enum enumInput);

    /// <summary>
    /// Get the double value for the <paramref name="enumInput"/>.
    /// </summary>
    /// <param name="enumInput">Value that will be used for searching the enum value.</param>
    /// <returns>Double value that is stored in the attribute.</returns>
    double GetDoubleValue(Enum enumInput);

    /// <summary>
    /// Get the integer value for the <paramref name="enumInput"/>.
    /// </summary>
    /// <param name="enumInput">Value that will be used for searching the enum value.</param>
    /// <returns>Integer value that is stored in the attribute.</returns>
    int GetIntegerValue(Enum enumInput);

    /// <summary>
    /// Get the key value pairs for the <paramref name="enumInput"/>.
    /// </summary>
    /// <param name="enumInput">Value that will be used for searching the enum value.</param>
    /// <returns>Key value paris that are stored in the attributes.</returns>
    List<KeyValuePair<string, object>> GetKeyValuePairs(Enum enumInput);

    /// <summary>
    /// Get the long value for the <paramref name="enumInput"/>.
    /// </summary>
    /// <param name="enumInput">Value that will be used for searching the enum value.</param>
    /// <returns>Long value that is stored in the attribute.</returns>
    long GetLongValue(Enum enumInput);

    /// <summary>
    /// Get the string value for the <paramref name="enumInput"/>
    /// </summary>
    /// <param name="enumInput">Value that will be used for searching the enum value.</param>
    /// <returns>String value that is stored in the attribute.</returns>
    string? GetStringValue(Enum enumInput);

    /// <summary>
    /// Get the derived attribute value from the <paramref name="enumInput"/> that are of type <typeparamref name="TDataType"/>
    /// and are not allowed to have multiple values.
    /// </summary>
    /// <typeparam name="TDataType">Type of the attribute to retrieve.</typeparam>
    /// <param name="enumInput">Value that will be used to get the attribute values.</param>
    /// <returns>Value of type <typeparamref name="TDataType"/> that is stored in the attribute.</returns>
    TDataType? GetValue<TDataType>(Enum enumInput);

    /// <summary>
    /// Get the derived attribute values from the <paramref name="enumInput"/> that are of type <typeparamref name="TDataType"/>
    /// and are allowed to have multiple values.
    /// </summary>
    /// <typeparam name="TDataType">Type of the attribute to retrieve.</typeparam>
    /// <param name="enumInput">Value that will be used to get the attribute values.</param>
    /// <returns>Values of type <typeparamref name="TDataType"/> that are stored in the attributes.</returns>
    List<TDataType> GetValues<TDataType>(Enum enumInput);

    /// <summary>
    /// Cache the derived attribute values from the <typeparamref name="TEnum"/> enum.
    /// </summary>
    /// <typeparam name="TEnum">Type of the enum.</typeparam>
    /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not a valid enum.</exception>
    void CacheEnum<TEnum>() where TEnum : Enum;

    /// <summary>
    /// Get the cached derived attribute values from the <paramref name="enumValue"/>, is it cached;
    /// Otherwise it will cache the value and return them.
    /// </summary>
    /// <param name="enumValue">Enum value to get the attribute values and/or cache it.</param>
    /// <returns>Derived attribute values.</returns>
    /// <exception cref="ArgumentException"><paramref name="enumValue"/> is not a valid enum.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="enumValue"/> is null.</exception>
    List<EnumValue> CacheValue(Enum enumValue);
}