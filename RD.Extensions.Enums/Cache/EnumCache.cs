using RD.Extensions.Enums.Attributes;
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

    private readonly EnumCacheOptions _enumCacheOptions;

    /// <summary>
    /// Create an instance of <see cref="EnumCache"/>.
    /// </summary>
    /// <param name="enumCacheOptions">Options to configure the caching.</param>
    public EnumCache(EnumCacheOptions? enumCacheOptions = null)
    {
        this._cache = new();
        this._enumCacheOptions = enumCacheOptions ?? new();
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
    /// Get the enum value that has an attribute value of <paramref name="attributeValue"/>.
    /// </summary>
    /// <remarks>
    /// Enum valeus must be cached.
    /// </remarks>
    /// <typeparam name="TEnum">Enum to search. Enum values must be cached.</typeparam>
    /// <typeparam name="TDataType">Type to search.</typeparam>
    /// <param name="attributeValue">Value to search.</param>
    /// <returns>Found enum value. If a value is not found, it will returns the default value,
    /// what is the first value in the enum.</returns>
    /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not a valid enum type.</exception>
    public TEnum? GetEnumValueByAttributeValue<TEnum, TDataType>(TDataType attributeValue)
        where TEnum : Enum
    {
        Type enumType = typeof(TEnum);

        if (!enumType.IsEnum)
        {
            throw new ArgumentException("Type is not a valid enum.", nameof(TEnum));
        }

        if(!this.IsEnumCached(enumType))
        {
            switch (this._enumCacheOptions.CachingMethod)
            {
                case Enums.CachingMethod.CacheExplicitly:
                default:
                case Enums.CachingMethod.CacheValueIfUsed:
                    break;
                case Enums.CachingMethod.CacheEntireEnumWhenFirstUsed:
                    this.CacheEnum<TEnum>();
                    break;
            }
        }

        if(!this._cache.TryGetValue(enumType, out Dictionary<Enum, List<EnumValue>>? enumDictionary) ||
            enumDictionary is null)
        {
            return default;
        }

        foreach (KeyValuePair<Enum, List<EnumValue>> enumValues in enumDictionary)
        {
            foreach (EnumValue enumValue in enumValues.Value)
            {
                if (enumValue.Type != typeof(TDataType))
                {
                    continue;
                }

                if (enumValue.AllowMultiple)
                {
                    if(enumValue.Value is not List<object> objectValues)
                    {
                        continue;
                    }

                    foreach (object objectValue in objectValues)
                    {
                        if(objectValue.Equals(attributeValue))
                        {
                            return (TEnum)enumValues.Key;
                        }
                    }
                }

                if(enumValue.Value is not TDataType typedValue)
                {
                    continue;
                }

                if(EqualityComparer<TDataType>.Default.Equals(typedValue, attributeValue))
                {
                    return (TEnum)enumValues.Key;
                }
            }
        }

        return default;
    }

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

        List<EnumValue> enumValues = this.GetAndHandleCachingEnum(enumInput);

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

        List<EnumValue> enumValues = this.GetAndHandleCachingEnum(enumInput);

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

        this.CacheEnum(enumType);
    }

    /// <summary>
    /// Cache the derived attribute values from the <paramref name="enumType"/> enum.
    /// </summary>
    /// <param name="enumType">Type of the enum.</param>
    /// <exception cref="ArgumentException"><paramref name="enumType"/> is not a valid enum.</exception>
    public void CacheEnum(Type enumType)
    {
        if (!enumType.IsEnum)
        {
            throw new ArgumentException("Type is not a valid enum.", nameof(enumType));
        }

        if(this.IsEnumCached(enumType))
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

        if(this.IsEnumCached(enumType))
        {
            return this.GetCachedValues(enumValue);
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

    /// <summary>
    /// Check if the enum from the <paramref name="enumValue"/> is cached.
    /// </summary>
    /// <param name="enumValue">Value of the enum to check.</param>
    /// <returns>True when the enum is cached; Otherwise false.</returns>
    public bool IsEnumCached(Enum enumValue)
    {
        if(enumValue is null)
        {
            return false;
        }

        return this.IsEnumCached(enumValue.GetType());
    }

    /// <summary>
    /// Check if the enum type is cached.
    /// </summary>
    /// <param name="enumType">Type of the enum.</param>
    /// <returns>True when the enum is cached; Otherwise false.</returns>
    public bool IsEnumCached(Type enumType)
    {
        if(enumType is null || !enumType.IsEnum)
        {
            return false;
        }

        return this._cache.ContainsKey(enumType);
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

    private List<EnumValue> GetCachedValues(Enum enumValue)
    {
        Type enumType = enumValue.GetType();

        if (this._cache.TryGetValue(enumType, out Dictionary<Enum, List<EnumValue>>? enumDictionary) &&
            enumDictionary.TryGetValue(enumValue, out List<EnumValue>? enumValues))
        {
            return enumValues ?? [];
        }

        return [];
    }

    private List<EnumValue> GetAndHandleCachingEnum(Enum enumValue)
    {
        if(this.IsEnumCached(enumValue))
        {
            return this.GetCachedValues(enumValue);
        }

        switch (this._enumCacheOptions.CachingMethod)
        {
            case Enums.CachingMethod.CacheExplicitly:
                return [];
            case Enums.CachingMethod.CacheValueIfUsed:
                return this.CacheValue(enumValue);
            case Enums.CachingMethod.CacheEntireEnumWhenFirstUsed:
                this.CacheEnum(enumValue.GetType());
                return this.GetCachedValues(enumValue);
            default:
                return [];
        }
    }
}
