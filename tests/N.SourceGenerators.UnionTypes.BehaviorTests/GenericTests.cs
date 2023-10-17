namespace N.SourceGenerators.UnionTypes.BehaviorTests;

record struct ValueNotSet;

enum UserStatus
{
    Active = 1,
    Deactivated
}

[UnionType(typeof(ValueNotSet))]
partial class GenericUnion<TValue>
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
        Assert.False(unionValue.IsValue);

        // AsProperty
        Assert.Equal(new ValueNotSet(), unionValue.AsValueNotSet);
        Assert.Throws<InvalidOperationException>(() => _ = unionValue.AsValue);

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
        Assert.False(unionValue.TryGetValue(out var tryGetValue));
        Assert.Equal(default(UserStatus), tryGetValue);

        Assert.True(unionValue.TryGetValueNotSet(out var tryGetValueNotSet));
        Assert.Equal(new ValueNotSet(), tryGetValueNotSet);

        // Match
        var matchResult = unionValue.Match<string>(
            matchValue: _ => "Value",
            matchValueNotSet: _ => "ValueNotSet");
        Assert.Equal("ValueNotSet", matchResult);

        // Switch
        bool switchWasExecuted = false;
        unionValue.Switch(
            switchValue: _ => Assert.Fail("switchValue should not be executed"),
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