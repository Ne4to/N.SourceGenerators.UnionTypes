
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
public class FooResultLayout : IEquatable<FooResultLayout>
{
    private const int SuccessVariant = 1;
    private const int ValidationErrorVariant = 2;
    private const int NotFoundErrorVariant = 3;

    [FieldOffset(0)] 
    private readonly int _variant;
    
    [FieldOffset(sizeof(long))]
    private readonly Success _success;
    
    [FieldOffset(sizeof(long))]
    private readonly ValidationError _validationError;
    
    [FieldOffset(sizeof(long))]
    private readonly NotFoundError _notFoundError;
    
    public bool IsSuccess => _variant == SuccessVariant;
    public Success AsSuccess
    {
        get
        {
            if (_variant == SuccessVariant)
                return _success;
            
            throw new InvalidOperationException("This is not a global::Success");
        }
    }

    public FooResultLayout(Success success)
    {
        ArgumentNullException.ThrowIfNull(success);
        _variant = SuccessVariant;
        _success = success;
    }

    public static implicit operator FooResultLayout(Success success) => new FooResultLayout(success);
    public static explicit operator Success(FooResultLayout value) => value.AsSuccess;
    public bool TryGetSuccess([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Success? value)
    {
        if (_variant == SuccessVariant)
        {
            value = _success;
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }

    
    public bool IsValidationError => _variant == ValidationErrorVariant;
    public ValidationError AsValidationError
    {
        get
        {
            if (_variant == ValidationErrorVariant)
                return _validationError; 
            
            throw new InvalidOperationException("This is not a global::ValidationError");
        }
    }

    public FooResultLayout(ValidationError validationError)
    {
        ArgumentNullException.ThrowIfNull(validationError);
        _variant = ValidationErrorVariant;
        _validationError = validationError;
    }

    public static implicit operator FooResultLayout(ValidationError validationError) => new FooResultLayout(validationError);
    public static explicit operator ValidationError(FooResultLayout value) => value.AsValidationError;
    public bool TryGetValidationError([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out ValidationError? value)
    {
        if (_variant == ValidationErrorVariant)
        {
            value = _validationError;
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }

    public bool IsNotFoundError => _variant == NotFoundErrorVariant;
    public NotFoundError AsNotFoundError
    {
        get
        {
            if (_variant == NotFoundErrorVariant)
                return _notFoundError;
            
            throw new InvalidOperationException("This is not a global::NotFoundError");
        }
    }

    public FooResultLayout(NotFoundError notFoundError)
    {
        ArgumentNullException.ThrowIfNull(notFoundError);
        _variant = NotFoundErrorVariant;
        _notFoundError = notFoundError;
    }

    public static implicit operator FooResultLayout(NotFoundError notFoundError) => new FooResultLayout(notFoundError);
    public static explicit operator NotFoundError(FooResultLayout value) => value.AsNotFoundError;
    public bool TryGetNotFoundError([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out NotFoundError? value)
    {
        if (_variant == NotFoundErrorVariant)
        {
            value = _notFoundError;
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }

    public TOut Match<TOut>(Func<Success, TOut> matchSuccess, Func<ValidationError, TOut> matchValidationError, Func<NotFoundError, TOut> matchNotFoundError)
    {
        if (IsSuccess)
            return matchSuccess(AsSuccess);
        if (IsValidationError)
            return matchValidationError(AsValidationError);
        if (IsNotFoundError)
            return matchNotFoundError(AsNotFoundError);
        throw new InvalidOperationException("Unknown type");
    }

    public async Task<TOut> MatchAsync<TOut>(Func<Success, CancellationToken, Task<TOut>> matchSuccess, Func<ValidationError, CancellationToken, Task<TOut>> matchValidationError, Func<NotFoundError, CancellationToken, Task<TOut>> matchNotFoundError, CancellationToken ct)
    {
        if (IsSuccess)
            return await matchSuccess(AsSuccess, ct).ConfigureAwait(false);
        if (IsValidationError)
            return await matchValidationError(AsValidationError, ct).ConfigureAwait(false);
        if (IsNotFoundError)
            return await matchNotFoundError(AsNotFoundError, ct).ConfigureAwait(false);
        throw new InvalidOperationException("Unknown type");
    }

    public void Switch(Action<Success> switchSuccess, Action<ValidationError> switchValidationError, Action<NotFoundError> switchNotFoundError)
    {
        if (IsSuccess)
        {
            switchSuccess(AsSuccess);
            return;
        }

        if (IsValidationError)
        {
            switchValidationError(AsValidationError);
            return;
        }

        if (IsNotFoundError)
        {
            switchNotFoundError(AsNotFoundError);
            return;
        }

        throw new InvalidOperationException("Unknown type");
    }

    public async Task SwitchAsync(Func<Success, CancellationToken, Task> switchSuccess, Func<ValidationError, CancellationToken, Task> switchValidationError, Func<NotFoundError, CancellationToken, Task> switchNotFoundError, CancellationToken ct)
    {
        if (IsSuccess)
        {
            await switchSuccess(AsSuccess, ct).ConfigureAwait(false);
            return;
        }

        if (IsValidationError)
        {
            await switchValidationError(AsValidationError, ct).ConfigureAwait(false);
            return;
        }

        if (IsNotFoundError)
        {
            await switchNotFoundError(AsNotFoundError, ct).ConfigureAwait(false);
            return;
        }

        throw new InvalidOperationException("Unknown type");
    }

    public Type ValueType
    {
        get
        {
            if (IsSuccess)
                return typeof(Success);
            if (IsValidationError)
                return typeof(ValidationError);
            if (IsNotFoundError)
                return typeof(NotFoundError);
            throw new InvalidOperationException("Unknown type");
        }
    }

    private Object InnerValue
    {
        get
        {
            if (IsSuccess)
                return AsSuccess;
            if (IsValidationError)
                return AsValidationError;
            if (IsNotFoundError)
                return AsNotFoundError;
            throw new InvalidOperationException("Unknown type");
        }
    }

    private string InnerValueAlias
    {
        get
        {
            if (IsSuccess)
                return "Success";
            if (IsValidationError)
                return "ValidationError";
            if (IsNotFoundError)
                return "NotFoundError";
            throw new InvalidOperationException("Unknown type");
        }
    }

    public override int GetHashCode()
    {
        return InnerValue.GetHashCode();
    }

    public static bool operator ==(FooResultLayout? left, FooResultLayout? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(FooResultLayout? left, FooResultLayout? right)
    {
        return !Equals(left, right);
    }

    public bool Equals(FooResultLayout? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (ValueType != other.ValueType)
        {
            return false;
        }

        if (IsSuccess)
            return EqualityComparer<Success>.Default.Equals(AsSuccess, other.AsSuccess);
        if (IsValidationError)
            return EqualityComparer<ValidationError>.Default.Equals(AsValidationError, other.AsValidationError);
        if (IsNotFoundError)
            return EqualityComparer<NotFoundError>.Default.Equals(AsNotFoundError, other.AsNotFoundError);
        throw new InvalidOperationException("Unknown type");
    }

    public override string ToString()
    {
        return $"{InnerValueAlias} - {InnerValue}";
    }

    public override bool Equals(object? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (other.GetType() != typeof(FooResultLayout))
        {
            return false;
        }

        return Equals((FooResultLayout)other);
    }
}