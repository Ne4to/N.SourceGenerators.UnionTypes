using System.Diagnostics.CodeAnalysis;

public struct FooStructResult : IEquatable<FooStructResult>
{
    private readonly SuccessStruct? _successStruct;
    public bool IsSuccessStruct => _successStruct != null;
    public SuccessStruct AsSuccessStruct => _successStruct ?? throw new InvalidOperationException("This is not a global::SuccessStruct");
    public FooStructResult(SuccessStruct SuccessStruct)
    {
        ArgumentNullException.ThrowIfNull(SuccessStruct);
        _successStruct = SuccessStruct;
    }

    public static implicit operator FooStructResult(SuccessStruct SuccessStruct) => new FooStructResult(SuccessStruct);
    public static explicit operator SuccessStruct(FooStructResult value) => value.AsSuccessStruct;
    public bool TryGetSuccessStruct([NotNullWhen(true)] out SuccessStruct? value)
    {
        if (_successStruct != null)
        {
            value = _successStruct;
            return true;
        }

        value = default;
        return false;
    }

    private readonly ValidationErrorStruct? _validationErrorStruct;
    public bool IsValidationErrorStruct => _validationErrorStruct != null;
    public ValidationErrorStruct AsValidationErrorStruct => _validationErrorStruct ?? throw new InvalidOperationException("This is not a global::ValidationErrorStruct");
    public FooStructResult(ValidationErrorStruct ValidationErrorStruct)
    {
        ArgumentNullException.ThrowIfNull(ValidationErrorStruct);
        _validationErrorStruct = ValidationErrorStruct;
    }

    public static implicit operator FooStructResult(ValidationErrorStruct ValidationErrorStruct) => new FooStructResult(ValidationErrorStruct);
    public static explicit operator ValidationErrorStruct(FooStructResult value) => value.AsValidationErrorStruct;
    public bool TryGetValidationErrorStruct([NotNullWhen(true)] out ValidationErrorStruct? value)
    {
        if (_validationErrorStruct != null)
        {
            value = _validationErrorStruct;
            return true;
        }

        value = default;
        return false;
    }

    private readonly NotFoundErrorStruct? _notFoundErrorStruct;
    public bool IsNotFoundErrorStruct => _notFoundErrorStruct != null;
    public NotFoundErrorStruct AsNotFoundErrorStruct => _notFoundErrorStruct ?? throw new InvalidOperationException("This is not a global::NotFoundErrorStruct");
    public FooStructResult(NotFoundErrorStruct NotFoundErrorStruct)
    {
        ArgumentNullException.ThrowIfNull(NotFoundErrorStruct);
        _notFoundErrorStruct = NotFoundErrorStruct;
    }

    public static implicit operator FooStructResult(NotFoundErrorStruct NotFoundErrorStruct) => new FooStructResult(NotFoundErrorStruct);
    public static explicit operator NotFoundErrorStruct(FooStructResult value) => value.AsNotFoundErrorStruct;
    public bool TryGetNotFoundErrorStruct([NotNullWhen(true)] out NotFoundErrorStruct? value)
    {
        if (_notFoundErrorStruct != null)
        {
            value = _notFoundErrorStruct;
            return true;
        }

        value = default;
        return false;
    }

    public TOut Match<TOut>(Func<SuccessStruct, TOut> matchSuccessStruct, Func<ValidationErrorStruct, TOut> matchValidationErrorStruct, Func<NotFoundErrorStruct, TOut> matchNotFoundErrorStruct)
    {
        if (IsSuccessStruct)
            return matchSuccessStruct(AsSuccessStruct);
        if (IsValidationErrorStruct)
            return matchValidationErrorStruct(AsValidationErrorStruct);
        if (IsNotFoundErrorStruct)
            return matchNotFoundErrorStruct(AsNotFoundErrorStruct);
        throw new InvalidOperationException("Unknown type");
    }

    public async Task<TOut> MatchAsync<TOut>(Func<SuccessStruct, CancellationToken, Task<TOut>> matchSuccessStruct, Func<ValidationErrorStruct, CancellationToken, Task<TOut>> matchValidationErrorStruct, Func<NotFoundErrorStruct, CancellationToken, Task<TOut>> matchNotFoundErrorStruct, CancellationToken ct)
    {
        if (IsSuccessStruct)
            return await matchSuccessStruct(AsSuccessStruct, ct).ConfigureAwait(false);
        if (IsValidationErrorStruct)
            return await matchValidationErrorStruct(AsValidationErrorStruct, ct).ConfigureAwait(false);
        if (IsNotFoundErrorStruct)
            return await matchNotFoundErrorStruct(AsNotFoundErrorStruct, ct).ConfigureAwait(false);
        throw new InvalidOperationException("Unknown type");
    }

    public void Switch(Action<SuccessStruct> switchSuccessStruct, Action<ValidationErrorStruct> switchValidationErrorStruct, Action<NotFoundErrorStruct> switchNotFoundErrorStruct)
    {
        if (IsSuccessStruct)
        {
            switchSuccessStruct(AsSuccessStruct);
            return;
        }

        if (IsValidationErrorStruct)
        {
            switchValidationErrorStruct(AsValidationErrorStruct);
            return;
        }

        if (IsNotFoundErrorStruct)
        {
            switchNotFoundErrorStruct(AsNotFoundErrorStruct);
            return;
        }

        throw new InvalidOperationException("Unknown type");
    }

    public async Task SwitchAsync(Func<SuccessStruct, CancellationToken, Task> switchSuccessStruct, Func<ValidationErrorStruct, CancellationToken, Task> switchValidationErrorStruct, Func<NotFoundErrorStruct, CancellationToken, Task> switchNotFoundErrorStruct, CancellationToken ct)
    {
        if (IsSuccessStruct)
        {
            await switchSuccessStruct(AsSuccessStruct, ct).ConfigureAwait(false);
            return;
        }

        if (IsValidationErrorStruct)
        {
            await switchValidationErrorStruct(AsValidationErrorStruct, ct).ConfigureAwait(false);
            return;
        }

        if (IsNotFoundErrorStruct)
        {
            await switchNotFoundErrorStruct(AsNotFoundErrorStruct, ct).ConfigureAwait(false);
            return;
        }

        throw new InvalidOperationException("Unknown type");
    }

    public Type ValueType
    {
        get
        {
            if (IsSuccessStruct)
                return typeof(SuccessStruct);
            if (IsValidationErrorStruct)
                return typeof(ValidationErrorStruct);
            if (IsNotFoundErrorStruct)
                return typeof(NotFoundErrorStruct);
            throw new InvalidOperationException("Unknown type");
        }
    }

    private Object InnerValue
    {
        get
        {
            if (IsSuccessStruct)
                return AsSuccessStruct;
            if (IsValidationErrorStruct)
                return AsValidationErrorStruct;
            if (IsNotFoundErrorStruct)
                return AsNotFoundErrorStruct;
            throw new InvalidOperationException("Unknown type");
        }
    }

    private string InnerValueAlias
    {
        get
        {
            if (IsSuccessStruct)
                return "SuccessStruct";
            if (IsValidationErrorStruct)
                return "ValidationErrorStruct";
            if (IsNotFoundErrorStruct)
                return "NotFoundErrorStruct";
            throw new InvalidOperationException("Unknown type");
        }
    }

    public override int GetHashCode()
    {
        return InnerValue.GetHashCode();
    }

    public static bool operator ==(FooStructResult left, FooStructResult right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(FooStructResult left, FooStructResult right)
    {
        return !left.Equals(right);
    }

    public bool Equals(FooStructResult other)
    {
        if (ValueType != other.ValueType)
        {
            return false;
        }

        if (IsSuccessStruct)
            return EqualityComparer<SuccessStruct>.Default.Equals(AsSuccessStruct, other.AsSuccessStruct);
        if (IsValidationErrorStruct)
            return EqualityComparer<ValidationErrorStruct>.Default.Equals(AsValidationErrorStruct, other.AsValidationErrorStruct);
        if (IsNotFoundErrorStruct)
            return EqualityComparer<NotFoundErrorStruct>.Default.Equals(AsNotFoundErrorStruct, other.AsNotFoundErrorStruct);
        throw new InvalidOperationException("Unknown type");
    }

    public override string ToString()
    {
        return $"{InnerValueAlias} - {InnerValue}";
    }

    public override bool Equals(object? obj)
    {
        return obj is FooStructResult other && Equals(other);
    }
}