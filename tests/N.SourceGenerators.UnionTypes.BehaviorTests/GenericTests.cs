namespace N.SourceGenerators.UnionTypes.BehaviorTests;

record struct ValueNotSet;

enum UserStatus
{
    Active = 1,
    Deactivated
}

[UnionType(typeof(ValueNotSet))]
partial class GenericUnion<[GenericUnionType (Alias = "Result")] TValue>
{
}

public class GenericTests
{
    [Fact]
    public void AllOperations()
    {
        GenericUnion<UserStatus> unionValue = new ValueNotSet();

        // IsProperty
        Assert.True(unionValue.IsValueNotSet);
        Assert.False(unionValue.IsResult);

        // AsProperty
        Assert.Equal(new ValueNotSet(), unionValue.AsValueNotSet);
        Assert.Throws<InvalidOperationException>(() => _ = unionValue.AsResult);

        // .ctor
        GenericUnion<UserStatus> ctorValueNotSet = new GenericUnion<UserStatus>(new ValueNotSet());
        Assert.Equal(new ValueNotSet(), ctorValueNotSet);
        
        GenericUnion<UserStatus> ctorUserStatus = new GenericUnion<UserStatus>(UserStatus.Active);
        Assert.Equal(UserStatus.Active, ctorUserStatus);
        
        // implicit operator
        GenericUnion<UserStatus> implicitUserStatus = UserStatus.Active;
        Assert.Equal(UserStatus.Active, implicitUserStatus);
        
        // explicit operator
        Assert.Equal(new ValueNotSet(), (ValueNotSet)unionValue);
        Assert.Throws<InvalidOperationException>(() => _ = (UserStatus)unionValue);
        
        // TryGet
        Assert.False(unionValue.TryGetResult(out var tryGetResult));
        Assert.Equal(default(UserStatus), tryGetResult);

        Assert.True(unionValue.TryGetValueNotSet(out var tryGetValueNotSet));
        Assert.Equal(new ValueNotSet(), tryGetValueNotSet);

        // Match
        var matchResult = unionValue.Match<string>(
            matchResult: _ => "Result",
            matchValueNotSet: _ => "ValueNotSet");
        Assert.Equal("ValueNotSet", matchResult);

        // Switch
        bool switchWasExecuted = false;
        unionValue.Switch(
            switchResult: _ => Assert.Fail("switchResult should not be executed"),
            switchValueNotSet: x =>
            {
                Assert.Equal(new ValueNotSet(), x);
                switchWasExecuted = true;
            });
        Assert.True(switchWasExecuted);

        // ValueType
        Assert.Equal(typeof(ValueNotSet), unionValue.ValueType);
        
        // operator ==
        Assert.True(unionValue == new ValueNotSet());
        Assert.False(unionValue == UserStatus.Active);
        
        // operator !=
        Assert.False(unionValue != new ValueNotSet());
        Assert.True(unionValue != UserStatus.Active);
        
        // Equals
        Assert.True(unionValue.Equals(new ValueNotSet()));
        Assert.False(unionValue.Equals( UserStatus.Active));
        Assert.False(unionValue.Equals( 42));
        
        // ToString
        Assert.Equal((new ValueNotSet()).ToString(), unionValue.ToString());
        
        // GetHashCode
        Assert.Equal((new ValueNotSet()).GetHashCode(), unionValue.GetHashCode());
    }
}