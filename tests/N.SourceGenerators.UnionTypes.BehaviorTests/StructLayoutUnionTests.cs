namespace N.SourceGenerators.UnionTypes.BehaviorTests;

// TODO support nested type
[UnionType(typeof(ValueNotSet))]
[UnionType(typeof(int))]
[UnionType(typeof(UserStatus))]
partial struct StructLayoutUnion
{
}

public class StructLayoutUnionTests
{
    [Fact]
    public void AllOperations()
    {
        StructLayoutUnion unionValue = UserStatus.Active;

        // IsProperty
        Assert.False(unionValue.IsValueNotSet);
        Assert.False(unionValue.IsInt32);
        Assert.True(unionValue.IsUserStatus);

        // AsProperty
        Assert.Throws<InvalidOperationException>(() => _ = unionValue.AsValueNotSet);
        Assert.Throws<InvalidOperationException>(() => _ = unionValue.AsInt32);
        Assert.Equal(UserStatus.Active, unionValue.AsUserStatus);
        
        // .ctor
        StructLayoutUnion ctorValueNotSet = new StructLayoutUnion(new ValueNotSet());
        Assert.Equal(new ValueNotSet(), ctorValueNotSet);
        
        StructLayoutUnion ctorInt32 = new StructLayoutUnion(42);
        Assert.Equal(42, ctorInt32);
        
        StructLayoutUnion ctorUserStatus = new StructLayoutUnion(UserStatus.Active);
        Assert.Equal(UserStatus.Active, ctorUserStatus);
        
        // implicit operator
        StructLayoutUnion implicitValueNotSet = new ValueNotSet();
        Assert.Equal(new ValueNotSet(), implicitValueNotSet);
        
        StructLayoutUnion implicitUserStatus = UserStatus.Active;
        Assert.Equal(UserStatus.Active, implicitUserStatus);
        
        // explicit operator
        Assert.Throws<InvalidOperationException>(() => _ = (ValueNotSet)unionValue);
        Assert.Throws<InvalidOperationException>(() => _ = (int)unionValue);
        Assert.Equal(UserStatus.Active, (UserStatus)unionValue);
        
        // TryGet
        Assert.False(unionValue.TryGetValueNotSet(out var tryGetValueNotSetValue));
        Assert.Equal(default, tryGetValueNotSetValue);
        
        Assert.False(unionValue.TryGetInt32(out var tryGetInt32Value));
        Assert.Equal(default, tryGetInt32Value);

        Assert.True(unionValue.TryGetUserStatus(out var tryGetUserStatusValue));
        Assert.Equal(UserStatus.Active, tryGetUserStatusValue);

        // Match
        var matchResult = unionValue.Match<string>(
            matchValueNotSet: _ =>
            {
                Assert.Fail("should not be called");
                return "ValueNotSet";
            },
            matchInt32: _ =>
            {
                Assert.Fail("should not be called");
                return "Int32";
            },
            matchUserStatus: v =>
            {
                Assert.Equal(UserStatus.Active, v);
                return "UserStatus";
            });
        Assert.Equal("UserStatus", matchResult);

        // Switch
        bool switchWasExecuted = false;
        unionValue.Switch(
            switchValueNotSet: _ => Assert.Fail("should not be called"),
            switchInt32: _ => Assert.Fail("should not be called"),
            switchUserStatus: x =>
            {
                Assert.Equal(UserStatus.Active, x);
                switchWasExecuted = true;
            });
        Assert.True(switchWasExecuted);

        // ValueType
        Assert.Equal(typeof(UserStatus), unionValue.ValueType);
        
        // operator ==
        Assert.False(unionValue == new ValueNotSet());
        Assert.False(unionValue == 42);
        Assert.True(unionValue == UserStatus.Active);
        
        // operator !=
        Assert.True(unionValue != new ValueNotSet());
        Assert.True(unionValue != 42);
        Assert.False(unionValue != UserStatus.Active);
        
        // Equals
        Assert.False(unionValue.Equals(new ValueNotSet()));
        Assert.False(unionValue.Equals( 42));
        Assert.True(unionValue.Equals( UserStatus.Active));
        
        // ToString
        Assert.Equal(UserStatus.Active.ToString(), unionValue.ToString());
        
        // GetHashCode
        Assert.Equal(UserStatus.Active.GetHashCode(), unionValue.GetHashCode());
    }
}