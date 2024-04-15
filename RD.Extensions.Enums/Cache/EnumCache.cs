﻿using RD.Extensions.Enums.Attributes;
using RD.Extensions.Enums.Contracts;
using System.Collections.Concurrent;
using System.Reflection;

namespace RD.Extensions.Enums.Cache;

/// <summary>
/// Caching system for enum that operates with the underlying attributes of <see cref="ValueBaseAttribute{TDataType}"/>.
/// </summary>
public class EnumCache : IEnumCache
{
    private readonly ConcurrentDictionary<Type, Dictionary<Enum, List<EnumValue>>> _cache;

    public EnumCache()
    {
        this._cache = new();
    }

    /// <summary>
    /// Get the key value pairs for the <paramref name="enumInput"/>.
    /// </summary>
    /// <param name="enumInput">Value that will be used for searching the enum value.</param>
    /// <returns>Key value paris that are stored in the attributes.</returns>
    public List<KeyValuePair<string, object>> GetKeyValuePairs(Enum enumInput)
        => this.GetValues<KeyValuePair<string, object>>(enumInput);

    /// <summary>
    /// Get the string value for the <paramref name="enumInput"/>
    /// </summary>
    /// <param name="enumInput">Value that will be used for searching the enum value.</param>
    /// <returns>String value that is stored in the attribute.</returns>
    public string? GetStringValue(Enum enumInput)
        => this.GetValue<string>(enumInput);

    /// <summary>
    /// Get the boolean value for the <paramref name="enumInput"/>.
    /// </summary>
    /// <param name="enumInput">Value that will be used for searching the enum value.</param>
    /// <returns>Boolean value that is stored in the attribute.</returns>
    public bool GetBooleanValue(Enum enumInput)
        => this.GetValue<bool>(enumInput);

    /// <summary>
    /// Get the double value for the <paramref name="enumInput"/>.
    /// </summary>
    /// <param name="enumInput">Value that will be used for searching the enum value.</param>
    /// <returns>Double value that is stored in the attribute.</returns>
    public double GetDoubleValue(Enum enumInput)
        => this.GetValue<double>(enumInput);

    /// <summary>
    /// Get the integer value for the <paramref name="enumInput"/>.
    /// </summary>
    /// <param name="enumInput">Value that will be used for searching the enum value.</param>
    /// <returns>Integer value that is stored in the attribute.</returns>
    public int GetIntegerValue(Enum enumInput)
        => this.GetValue<int>(enumInput);

    /// <summary>
    /// Get the long value for the <paramref name="enumInput"/>.
    /// </summary>
    /// <param name="enumInput">Value that will be used for searching the enum value.</param>
    /// <returns>Long value that is stored in the attribute.</returns>
    public long GetLongValue(Enum enumInput)
        => this.GetValue<long>(enumInput);

    /// <summary>
    /// Get the derived attribute value from the <paramref name="enumInput"/> that are of type <typeparamref name="TDataType"/>
    /// and are not allowed to have multiple values.
    /// </summary>
    /// <typeparam name="TDataType">Type of the attribute to retrieve.</typeparam>
    /// <param name="enumInput">Value that will be used to get the attribute values.</param>
    /// <returns>Value of type <typeparamref name="TDataType"/> that is stored in the attribute.</returns>
    public TDataType? GetValue<TDataType>(Enum enumInput)
    {
        ArgumentNullException.ThrowIfNull(enumInput);

        List<EnumValue> enumValues = this.CacheValue(enumInput);

        if (enumValues is null || enumValues.Count == 0)
        {
            return default;
        }

        EnumValue? enumValue = enumValues
            .FirstOrDefault(x => x.Type == typeof(TDataType) && x.AllowMultiple == false);

        return enumValue is null || enumValue.Value is null
            ? default
            : (TDataType)enumValue.Value;
    }

    /// <summary>
    /// Get the derived attribute values from the <paramref name="enumInput"/> that are of type <typeparamref name="TDataType"/>
    /// and are allowed to have multiple values.
    /// </summary>
    /// <typeparam name="TDataType">Type of the attribute to retrieve.</typeparam>
    /// <param name="enumInput">Value that will be used to get the attribute values.</param>
    /// <returns>Values of type <typeparamref name="TDataType"/> that are stored in the attributes.</returns>
    public List<TDataType> GetValues<TDataType>(Enum enumInput)
    {
        ArgumentNullException.ThrowIfNull(enumInput);

        List<EnumValue> enumValues = this.CacheValue(enumInput);

        if (enumValues is null || enumValues.Count == 0)
        {
            return [];
        }

        EnumValue? enumValue = enumValues
            .FirstOrDefault(x => x.Type == typeof(TDataType) && x.AllowMultiple == true);

        return enumValue is null || enumValue.Value is null
            ? []
            : ((List<object>)enumValue.Value)
                .Where(x => x is not null)
                .Select(x => (TDataType)x)
                .ToList();
    }

    /// <summary>
    /// Cache the derived attribute values from the <typeparamref name="TEnum"/> enum.
    /// </summary>
    /// <typeparam name="TEnum">Type of the enum.</typeparam>
    /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not a valid enum.</exception>
    public void CacheEnum<TEnum>()
        where TEnum : Enum
    {
        Type enumType = typeof(TEnum);

        if (!enumType.IsEnum)
        {
            throw new ArgumentException("Type is not a valid enum.", nameof(TEnum));
        }

        if (this._cache.TryGetValue(enumType, out Dictionary<Enum, List<EnumValue>>? enumDictionary))
        {
            return;
        }

        IEnumerable<FieldInfo> fieldInfos = enumType
            .GetTypeInfo()
            .DeclaredMembers
            .OfType<FieldInfo>()
            .Where(x => x.IsStatic);

        Dictionary<Enum, List<EnumValue>> tmpCache = [];

        foreach (Enum enumValue in Enum.GetValues(enumType))
        {
            string? enumValueName = Enum.GetName(enumType, enumValue);
            FieldInfo? fieldInfo = fieldInfos
                .FirstOrDefault(x => x.Name == enumValueName);

            if (fieldInfo is null)
            {
                continue;
            }

            SetCacheValue(enumValue, fieldInfo, tmpCache);
        }

        this._cache[enumType] = tmpCache;
    }

    /// <summary>
    /// Get the cached derived attribute values from the <paramref name="enumValue"/>, is it cached;
    /// Otherwise it will cache the value and return them.
    /// </summary>
    /// <param name="enumValue">Enum value to get the attribute values and/or cache it.</param>
    /// <returns>Derived attribute values.</returns>
    /// <exception cref="ArgumentException"><paramref name="enumValue"/> is not a valid enum.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="enumValue"/> is null.</exception>
    public List<EnumValue> CacheValue(Enum enumValue)
    {
        ArgumentNullException.ThrowIfNull(enumValue);

        Type enumType = enumValue.GetType();

        if (!enumType.IsEnum)
        {
            throw new ArgumentException("Type is not a valid enum.", nameof(enumValue));
        }

        if (this._cache.TryGetValue(enumType, out Dictionary<Enum, List<EnumValue>>? enumDictionary) &&
            enumDictionary.TryGetValue(enumValue, out List<EnumValue>? enumValues))
        {
            return enumValues;
        }

        string? enumValueName = Enum.GetName(enumType, enumValue);

        FieldInfo? fieldInfo = enumType
            .GetTypeInfo()
            .DeclaredMembers
            .OfType<FieldInfo>()
            .FirstOrDefault(x => x.IsStatic &&
                x.Name == enumValueName);

        if (fieldInfo is null)
        {
            return [];
        }

        Dictionary<Enum, List<EnumValue>> tmpCache = [];

        SetCacheValue(enumValue, fieldInfo, tmpCache);

        this._cache[enumType] = tmpCache;

        return this._cache[enumType][enumValue];
    }

    private static void SetCacheValue(Enum enumValue, FieldInfo fieldInfo, Dictionary<Enum, List<EnumValue>> tmpCache)
    {
        IEnumerable<IGrouping<Type, Attribute>> groupedAttributes = fieldInfo
            .GetCustomAttributes(typeof(ValueBaseAttribute<>).GetGenericTypeDefinition())
            .GroupBy(x => x.GetType());

        List<EnumValue> enumValues = [];

        foreach (IGrouping<Type, Attribute> groupedValue in groupedAttributes)
        {
            Attribute attribute = groupedValue.First();

            bool attributeAllowMultiple = (bool)GetAttributePropertyValue(attribute, "AllowMultiple")!;

            if (attributeAllowMultiple)
            {
                List<object> values = [];

                foreach (Attribute multipleAttribute in groupedValue)
                {
                    object multipleAttributeValue = GetAttributePropertyValue(multipleAttribute, "Value")!;

                    values.Add(multipleAttributeValue);
                }

                enumValues.Add(new()
                {
                    AllowMultiple = attributeAllowMultiple,
                    Type = attribute.GetType().BaseType.GenericTypeArguments[0],
                    Value = values
                });

                continue;
            }

            object attributeValue = GetAttributePropertyValue(attribute, "Value")!;

            enumValues.Add(new()
            {
                AllowMultiple = attributeAllowMultiple,
                Type = attribute.GetType().BaseType.GenericTypeArguments[0],
                Value = attributeValue
            });
        }

        tmpCache[enumValue] = enumValues;
    }

    private static object? GetAttributePropertyValue(Attribute attribute, string propertyName)
    {
        return attribute
                .GetType()
                .GetProperty(propertyName)!
                .GetValue(attribute);
    }
}
