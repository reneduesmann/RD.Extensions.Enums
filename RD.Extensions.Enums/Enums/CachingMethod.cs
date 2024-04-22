namespace RD.Extensions.Enums.Enums;

/// <summary>
/// Method that will be used to cache the enum/values.
/// </summary>
public enum CachingMethod
{
    /// <summary>
    /// Enum values must be cached explicitly.
    /// </summary>
    CacheExplicitly,

    /// <summary>
    /// Enum values will be cached when they are used.
    /// </summary>
    CacheValueIfUsed,

    /// <summary>
    /// Entire enum will be cached when it is used for the first time.
    /// </summary>
    CacheEntireEnumWhenFirstUsed
}