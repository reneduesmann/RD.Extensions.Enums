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
```

### Caching Mechanism

The `EnumCache` class uses a caching mechanism to improve performance when retrieving values
associated with an Enum. The first time a value is retrieved for a particular Enum, it is store in a cache.
Subsequent requests for the same value are then served from the cache.

When retrieving a single value from an enum, only the value will be cached. 
It is also possible to cache the entire enum by calling the `CacheEnum` method.

### Contributing

If you want to contribute to this project, please create a pull request with your changes.

### License
The project is licensed under the MIT license. You can find the license [here](LICENSE).