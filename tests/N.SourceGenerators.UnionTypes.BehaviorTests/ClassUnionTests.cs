namespace N.SourceGenerators.UnionTypes.BehaviorTests;

[UnionType(typeof(ValueNotSet))]
[UnionType(typeof(string))]
[UnionType(typeof(UserStatus))]
partial class ClassUnion
{
}

public class ClassUnionTests
{
    [Fact]
    public void AllOperations()
    {
        ClassUnion unionValue = "42";

        // IsProperty
        Assert.False(unionValue.IsValueNotSet);
        Assert.True(unionValue.IsString);
        Assert.False(unionValue.IsUserStatus);

        // AsProperty
        Assert.Throws<InvalidOperationException>(() => _ = unionValue.AsValueNotSet);
        Assert.Equal("42", unionValue.AsString);
        Assert.Throws<InvalidOperationException>(() => _ = unionValue.AsUserStatus);

        // .ctor
        ClassUnion ctorValueNotSet = new ClassUnion(new ValueNotSet());
        Assert.Equal(new ValueNotSet(), ctorValueNotSet);
        
        ClassUnion ctorString = new ClassUnion("42");
        Assert.Equal("42", ctorString);
        
        ClassUnion ctorUserStatus = new ClassUnion(UserStatus.Active);
        Assert.Equal(UserStatus.Active, ctorUserStatus);
        
        // implicit operator
        ClassUnion implicitValueNotSet = new ValueNotSet();
        Assert.Equal(new ValueNotSet(), implicitValueNotSet);
        
        ClassUnion implicitUserStatus = UserStatus.Active;
        Assert.Equal(UserStatus.Active, implicitUserStatus);
        
        // explicit operator
        Assert.Throws<InvalidOperationException>(() => _ = (ValueNotSet)unionValue);
        Assert.Equal("42", (string)unionValue);
        Assert.Throws<InvalidOperationException>(() => _ = (UserStatus)unionValue);
        
        // TryGet
        Assert.False(unionValue.TryGetValueNotSet(out var tryGetValueNotSetValue));
        Assert.Equal(default, tryGetValueNotSetValue);
        
        Assert.True(unionValue.TryGetString(out var tryGetStringValue));
        Assert.Equal("42", tryGetStringValue);

        Assert.False(unionValue.TryGetUserStatus(out var tryGetUserStatusValue));
        Assert.Equal(default, tryGetUserStatusValue);

        // Match
        var matchResult = unionValue.Match<string>(
            matchValueNotSet: _ =>
            {
                Assert.Fail("should not be called");
                return "ValueNotSet";
            },
            matchString: s =>
            {
                Assert.Equal("42", s);
                return "String";
            },
            matchUserStatus: _ =>
            {
                Assert.Fail("should not be called");
                return "UserStatus";
            });
        Assert.Equal("String", matchResult);

        // Switch
        bool switchWasExecuted = false;
        unionValue.Switch(
            switchValueNotSet: _ => Assert.Fail("should not be called"),
            switchString: x =>
            {
                Assert.Equal("42", x);
                switchWasExecuted = true;
            },
            switchUserStatus: _ => Assert.Fail("should not be called"));
        Assert.True(switchWasExecuted);

        // ValueType
        Assert.Equal(typeof(string), unionValue.ValueType);
        
        // operator ==
        Assert.False(unionValue == new ValueNotSet());
        Assert.True(unionValue == "42");
        Assert.False(unionValue == UserStatus.Active);
        
        // operator !=
        Assert.True(unionValue != new ValueNotSet());
        Assert.False(unionValue != "42");
        Assert.True(unionValue != UserStatus.Active);
        
        // Equals
        Assert.False(unionValue.Equals(new ValueNotSet()));
        Assert.True(unionValue.Equals( "42"));
        Assert.False(unionValue.Equals( UserStatus.Active));
        
        // ToString
        Assert.Equal("42", unionValue.ToString());
        
        // GetHashCode
        Assert.Equal("42".GetHashCode(), unionValue.GetHashCode());
    }
}