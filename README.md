## RD.Extensions.Enums

This project provides a set of utilities to work with enums in .NET.
It includes methods for retrieving various types of values (string, integer, double, etc.) from enum values.
Also it caches the values for better performance.

### Installation
Install the nuget package `RD.Extensions.Enums` from nuget.org.

### Usage
```csharp
enum MyEnum 
{
	[StringValue("Value1")]
	[IntegerValue(1)]
	[LongValue(1)]
	[DoubleValue(5.5)]
	[BooleanValue(true)]
	[KeyValuePair("keyOne", "valueOne")]
	[KeyValuePair("keyTwo", "valueTwo")]
	MyValue
}

IEnumCache enumCache = new EnumCache();

enumCache.GetStringValue(MyEnum.MyValue); // returns "Value1"
enumCache.GetIntegerValue(MyEnum.MyValue); // returns 1
enumCache.GetLongValue(MyEnum.MyValue); // returns 1
enumCache.GetDoubleValue(MyEnum.MyValue); // returns 5.5
enumCache.GetBooleanValue(MyEnum.MyValue); // returns true
enumCache.GetKeyValuePairs(MyEnum.MyValue); // returns a List<KeyValuePair<string, object>>
enumCache.GetEnumValueByAttributeValue<MyEnum, string>("Value1"); // returns MyEnum.MyValue
```

### Caching Mechanism

The `EnumCache` class uses a caching mechanism to improve performance when retrieving values
associated with an Enum. The `EnumCache` class provides provides multiple caching methods that allow you to cache values associated with an Enum value.
The caching method can be configured in the `EnumCacheOptions` class.
It exists the following caching methods:
  - `CacheExplicitly` - caches the values explicitly by calling the `CacheValue` or `CacheEnum` method.
  - `CacheValueIfUsed` - caches the value when it is retrieved for the first time.
  - `CacheEntireEnumWhenFirstUsed` - caches the entire enum when the first value is retrieved.


### Contributing

If you want to contribute to this project, please create a pull request with your changes.

### License
The project is licensed under the MIT license. You can find the license [here](LICENSE).