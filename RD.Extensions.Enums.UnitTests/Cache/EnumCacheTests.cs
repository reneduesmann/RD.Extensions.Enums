using FluentAssertions;
using RD.Extensions.Enums.Attributes;
using RD.Extensions.Enums.Cache;
using RD.Extensions.Enums.Contracts;

namespace RD.Extensions.Enums.UnitTests.Cache;

public class EnumCacheTests
{
    enum TestEnum
    {
        Undefined = 0,

        [BooleanValue(true)]
        BooleanValue,

        [DoubleValue(5.5)]
        DoubleValue,

        [IntegerValue(10)]
        IntegerValue,

        [KeyValuePair("firstKey", "firstValue")]
        [KeyValuePair("secondKey", "secondValue")]
        KeyValuePairValues,

        [LongValue(100_000_000_000_000_000)]
        LongValue,

        [StringValue("Value of the string")]
        StringValue
    }

    private readonly IEnumCache _enumCache;

    public EnumCacheTests()
    {
        this._enumCache = new EnumCache();
    }

    [Fact]
    public void GetKeyValuePairs_EnumValueWithMultipleAttributes_ReturnsKeyValuePairs()
    {
        // Arrange
        TestEnum enumValue = TestEnum.KeyValuePairValues;
        
        // Act
        List<KeyValuePair<string, object>> keyValuePairs = this._enumCache.GetKeyValuePairs(enumValue);

        // Assert
        keyValuePairs.Should().HaveCount(2);
        keyValuePairs.Should().Contain(new KeyValuePair<string, object>("firstKey", "firstValue"));
        keyValuePairs.Should().Contain(new KeyValuePair<string, object>("secondKey", "secondValue"));
    }

    [Fact]
    public void GetKeyValuePairs_EnumValueWithNoAttributes_ReturnsEmptyList()
    {
        // Arrange
        TestEnum enumValue = TestEnum.Undefined;

        // Act
        List<KeyValuePair<string, object>> keyValuePairs = this._enumCache.GetKeyValuePairs(enumValue);

        // Assert
        keyValuePairs.Should().BeEmpty();
    }

    [Fact]
    public void GetKeyValuePairs_EnumValueWithNoKeyValuePairAttributes_ReturnsEmptyList()
    {
        // Arrange
        TestEnum enumValue = TestEnum.BooleanValue;

        // Act
        List<KeyValuePair<string, object>> keyValuePairs = this._enumCache.GetKeyValuePairs(enumValue);

        // Assert
        keyValuePairs.Should().BeEmpty();
    }

    [Fact]
    public void GetKeyValuePairs_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        TestEnum? enumValue = null;

        // Act
        Action act = () => this._enumCache.GetKeyValuePairs(enumValue);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetStringValue_EnumValueWithStringValueAttribute_ReturnsStringValue()
    {
        // Arrange
        TestEnum enumValue = TestEnum.StringValue;

        // Act
        string? stringValue = this._enumCache.GetStringValue(enumValue);

        // Assert
        stringValue.Should().Be("Value of the string");
    }

    [Fact]
    public void GetStringValue_EnumValueWithNoAttributes_ReturnsDefault()
    {
        // Arrange
        TestEnum enumValue = TestEnum.Undefined;

        // Act
        string? stringValue = this._enumCache.GetStringValue(enumValue);

        // Assert
        stringValue.Should().BeNull();
    }

    [Fact]
    public void GetStringValue_EnumValueWithNoStringValueAttribute_ReturnsDefault()
    {
        // Arrange
        TestEnum enumValue = TestEnum.BooleanValue;

        // Act
        string? stringValue = this._enumCache.GetStringValue(enumValue);

        // Assert
        stringValue.Should().BeNull();
    }

    [Fact]
    public void GetStringValue_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        TestEnum? enumValue = null;

        // Act
        Action act = () => this._enumCache.GetStringValue(enumValue);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetBooleanValue_EnumValueWithBooleanValueAttribute_ReturnsBooleanValue()
    {
        // Arrange
        TestEnum enumValue = TestEnum.BooleanValue;

        // Act
        bool booleanValue = this._enumCache.GetBooleanValue(enumValue);

        // Assert
        booleanValue.Should().BeTrue();
    }

    [Fact]
    public void GetBooleanValue_EnumValueWithNoAttributes_ReturnsDefault()
    {
        // Arrange
        TestEnum enumValue = TestEnum.Undefined;

        // Act
        bool booleanValue = this._enumCache.GetBooleanValue(enumValue);

        // Assert
        booleanValue.Should().BeFalse();
    }

    [Fact]
    public void GetBooleanValue_EnumValueWithNoBooleanValueAttribute_ReturnsDefault()
    {
        // Arrange
        TestEnum enumValue = TestEnum.StringValue;

        // Act
        bool booleanValue = this._enumCache.GetBooleanValue(enumValue);

        // Assert
        booleanValue.Should().BeFalse();
    }

    [Fact]
    public void GetBooleanValue_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        TestEnum? enumValue = null;

        // Act
        Action act = () => this._enumCache.GetBooleanValue(enumValue);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetDoubleValue_EnumValueWithDoubleValueAttribute_ReturnsDoubleValue()
    {
        // Arrange
        TestEnum enumValue = TestEnum.DoubleValue;

        // Act
        double doubleValue = this._enumCache.GetDoubleValue(enumValue);

        // Assert
        doubleValue.Should().Be(5.5);
    }

    [Fact]
    public void GetDoubleValue_EnumValueWithNoAttributes_ReturnsDefault()
    {
        // Arrange
        TestEnum enumValue = TestEnum.Undefined;

        // Act
        double doubleValue = this._enumCache.GetDoubleValue(enumValue);

        // Assert
        doubleValue.Should().Be(0);
    }

    [Fact]
    public void GetDoubleValue_EnumValueWithNoDoubleValueAttribute_ReturnsDefault()
    {
        // Arrange
        TestEnum enumValue = TestEnum.StringValue;

        // Act
        double doubleValue = this._enumCache.GetDoubleValue(enumValue);

        // Assert
        doubleValue.Should().Be(0);
    }

    [Fact]
    public void GetDoubleValue_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        TestEnum? enumValue = null;

        // Act
        Action act = () => this._enumCache.GetDoubleValue(enumValue);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetIntegerValue_EnumValueWithIntegerValueAttribute_ReturnsIntegerValue()
    {
        // Arrange
        TestEnum enumValue = TestEnum.IntegerValue;

        // Act
        int integerValue = this._enumCache.GetIntegerValue(enumValue);

        // Assert
        integerValue.Should().Be(10);
    }

    [Fact]
    public void GetIntegerValue_EnumValueWithNoAttributes_ReturnsDefault()
    {
        // Arrange
        TestEnum enumValue = TestEnum.Undefined;

        // Act
        int integerValue = this._enumCache.GetIntegerValue(enumValue);

        // Assert
        integerValue.Should().Be(0);
    }

    [Fact]
    public void GetIntegerValue_EnumValueWithNoIntegerValueAttribute_ReturnsDefault()
    {
        // Arrange
        TestEnum enumValue = TestEnum.StringValue;

        // Act
        int integerValue = this._enumCache.GetIntegerValue(enumValue);

        // Assert
        integerValue.Should().Be(0);
    }

    [Fact]
    public void GetIntegerValue_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        TestEnum? enumValue = null;

        // Act
        Action act = () => this._enumCache.GetIntegerValue(enumValue);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetLongValue_EnumValueWithLongValueAttribute_ReturnsLongValue()
    {
        // Arrange
        TestEnum enumValue = TestEnum.LongValue;

        // Act
        long longValue = this._enumCache.GetLongValue(enumValue);

        // Assert
        longValue.Should().Be(100_000_000_000_000_000);
    }

    [Fact]
    public void GetLongValue_EnumValueWithNoAttributes_ReturnsDefault()
    {
        // Arrange
        TestEnum enumValue = TestEnum.Undefined;

        // Act
        long longValue = this._enumCache.GetLongValue(enumValue);

        // Assert
        longValue.Should().Be(0);
    }

    [Fact]
    public void GetLongValue_EnumValueWithNoLongValueAttribute_ReturnsDefault()
    {
        // Arrange
        TestEnum enumValue = TestEnum.StringValue;

        // Act
        long longValue = this._enumCache.GetLongValue(enumValue);

        // Assert
        longValue.Should().Be(0);
    }

    [Fact]
    public void GetLongValue_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        TestEnum? enumValue = null;

        // Act
        Action act = () => this._enumCache.GetLongValue(enumValue);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetValue_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        TestEnum? enumValue = null;

        // Act
        Action act = () => this._enumCache.GetValue<bool>(enumValue);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetValue_UnsupportedType_ReturnsDefault()
    {
        // Arrange
        TestEnum enumValue = TestEnum.BooleanValue;

        // Act
        ulong value = this._enumCache.GetValue<ulong>(enumValue);

        // Assert
        value.Should().Be(0);
    }

    [Fact]
    public void GetValue_StringTypeWithStringValueAttribute_ReturnsStringValue()
    {
        // Arrange
        TestEnum enumValue = TestEnum.StringValue;

        // Act
        string? value = this._enumCache.GetValue<string>(enumValue);

        // Assert
        value.Should().Be("Value of the string");
    }

    [Fact]
    public void GetValue_StringTypeWithNoStringValueAttribute_ReturnsDefault()
    {
        // Arrange
        TestEnum enumValue = TestEnum.BooleanValue;

        // Act
        string? value = this._enumCache.GetValue<string>(enumValue);

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetValues_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        TestEnum? enumValue = null;

        // Act
        Action act = () => this._enumCache.GetValues<bool>(enumValue);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetValues_UnsupportedType_ReturnsDefault()
    {
        // Arrange
        TestEnum enumValue = TestEnum.BooleanValue;

        // Act
        ulong value = this._enumCache.GetValue<ulong>(enumValue);

        // Assert
        value.Should().Be(0);
    }

    [Fact]
    public void GetValue_KeyValuePairTypeWithKeyValuePairValueAttribute_ReturnsKeyValuePairs()
    {
        // Arrange
        TestEnum enumValue = TestEnum.KeyValuePairValues;

        // Act
        List<KeyValuePair<string, object>> keyValuePairs = this._enumCache.GetValues<KeyValuePair<string, object>>(enumValue);

        // Assert
        keyValuePairs.Should().HaveCount(2);
        keyValuePairs.Should().Contain(new KeyValuePair<string, object>("firstKey", "firstValue"));
        keyValuePairs.Should().Contain(new KeyValuePair<string, object>("secondKey", "secondValue"));
    }

    [Fact]
    public void GetValues_KeyValuePairTypeWithNoKeyValuePairAttribute_ReturnsEmptyList()
    {
        // Arrange
        TestEnum enumValue = TestEnum.BooleanValue;

        // Act
        List<KeyValuePair<string, object>> keyValuePairs = this._enumCache.GetValues<KeyValuePair<string, object>>(enumValue);

        // Assert
        keyValuePairs.Should().BeEmpty();
    }

    [Fact]
    public void CacheValue_EnumValueWithBooleanValueAttribute_ReturnsSingleBooleanEnumValue()
    {
        // Arrange
        TestEnum enumValue = TestEnum.BooleanValue;

        // Act
        List<EnumValue> enumValues = this._enumCache.CacheValue(enumValue);

        // Assert
        enumValues.Should().HaveCount(1);
        enumValues.First().Value.Should().Be(true);
        enumValues.First().Type.Should().Be(typeof(bool));
        enumValues.First().AllowMultiple.Should().BeFalse();
    }

    [Fact]
    public void CacheValue_EnumValueWithMultipleKeyValuePairAttributes_ReturnsSingleEnumValue()
    {
        // Arrange
        TestEnum enumValue = TestEnum.KeyValuePairValues;

        // Act
        List<EnumValue> enumValues = this._enumCache.CacheValue(enumValue);

        // Assert
        enumValues.Should().HaveCount(1);
        enumValues.First().Value.Should().BeOfType<List<object>>();
        enumValues.First().Type.Should().Be(typeof(KeyValuePair<string, object>));
        enumValues.First().AllowMultiple.Should().BeTrue();
    }

    [Fact]
    public void CacheValue_EnumValueWithNoAttributes_ReturnsEmptyList()
    {
        // Arrange
        TestEnum enumValue = TestEnum.Undefined;

        // Act
        List<EnumValue> enumValues = this._enumCache.CacheValue(enumValue);

        // Assert
        enumValues.Should().BeEmpty();
    }

    [Fact]
    public void CacheValue_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        TestEnum? enumValue = null;

        // Act
        Action act = () => this._enumCache.CacheValue(enumValue);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}
