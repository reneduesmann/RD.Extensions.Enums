using RD.Extensions.Enums.Enums;

namespace RD.Extensions.Enums.Cache;

/// <summary>
/// Options to configure the enum cache.
/// </summary>
public sealed class EnumCacheOptions
{
    /// <summary>
    /// Method that will be used to cache the enum/values.
    /// </summary>
    public CachingMethod CachingMethod { get; set; } = CachingMethod.CacheExplicitly;
}
