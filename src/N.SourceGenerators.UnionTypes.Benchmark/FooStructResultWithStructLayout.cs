using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
public struct FooStructResultWithStructLayout : IEquatable<FooStructResultWithStructLayout>
{
    private const int SuccessStructVariant = 1;
    private const int ValidationErrorStructVariant = 2;
    private const int NotFoundErrorStructVariant = 3;

    [FieldOffset(0)] private readonly int _variant;

    [FieldOffset(sizeof(int))] private readonly SuccessStruct _successStruct;

    [FieldOffset(sizeof(int))] private readonly ValidationErrorStruct _validationErrorStruct;

    [FieldOffset(sizeof(int))] private readonly NotFoundErrorStruct _notFoundErrorStruct;


    public bool IsSuccessStruct => _variant == SuccessStructVariant;

    public SuccessStruct AsSuccessStruct
    {
        get
        {
            if (_variant == SuccessStructVariant)
                return _successStruct;

            throw new InvalidOperationException("This is not a global::SuccessStruct");
        }
    }

    public FooStructResultWithStructLayout(SuccessStruct successStruct)
    {
        _variant = SuccessStructVariant;
        _successStruct = successStruct;
    }

    public static implicit operator FooStructResultWithStructLayout(SuccessStruct successStruct) =>
        new FooStructResultWithStructLayout(successStruct);

    public static explicit operator SuccessStruct(FooStructResultWithStructLayout value) => value.AsSuccessStruct;

    public bool TryGetSuccessStruct([NotNullWhen(true)] out SuccessStruct? value)
    {
        if (_variant == SuccessStructVariant)
        {
            value = _successStruct;
            return true;
        }

        value = default;
        return false;
    }

    public bool IsValidationErrorStruct => _variant == ValidationErrorStructVariant;

    public ValidationErrorStruct AsValidationErrorStruct
    {
        get
        {
            if (_variant == ValidationErrorStructVariant)
                return _validationErrorStruct;

            throw new InvalidOperationException("This is not a global::ValidationErrorStruct");
        }
    }

    public FooStructResultWithStructLayout(ValidationErrorStruct validationErrorStruct)
    {
        _variant = ValidationErrorStructVariant;
        _validationErrorStruct = validationErrorStruct;
    }

    public static implicit operator FooStructResultWithStructLayout(ValidationErrorStruct validationErrorStruct) =>
        new FooStructResultWithStructLayout(validationErrorStruct);

    public static explicit operator ValidationErrorStruct(FooStructResultWithStructLayout value) =>
        value.AsValidationErrorStruct;

    public bool TryGetValidationErrorStruct([NotNullWhen(true)] out ValidationErrorStruct? value)
    {
        if (_variant == ValidationErrorStructVariant)
        {
            value = _validationErrorStruct;
            return true;
        }

        value = default;
        return false;
    }

    public bool IsNotFoundErrorStruct => _variant == NotFoundErrorStructVariant;

    public NotFoundErrorStruct AsNotFoundErrorStruct
    {
        get
        {
            if (_variant == NotFoundErrorStructVariant)
                return _notFoundErrorStruct;

            throw new InvalidOperationException(
                "This is not a global::NotFoundErrorStruct");
        }
    }

    public FooStructResultWithStructLayout(NotFoundErrorStruct notFoundErrorStruct)
    {
        _variant = NotFoundErrorStructVariant;
        _notFoundErrorStruct = notFoundErrorStruct;
    }

    public static implicit operator FooStructResultWithStructLayout(NotFoundErrorStruct notFoundErrorStruct) =>
        new FooStructResultWithStructLayout(notFoundErrorStruct);

    public static explicit operator NotFoundErrorStruct(FooStructResultWithStructLayout value) =>
        value.AsNotFoundErrorStruct;

    public bool TryGetNotFoundErrorStruct([NotNullWhen(true)] out NotFoundErrorStruct? value)
    {
        if (_variant == NotFoundErrorStructVariant)
        {
            value = _notFoundErrorStruct;
            return true;
        }

        value = default;
        return false;
    }

    public TOut Match<TOut>(Func<SuccessStruct, TOut> matchSuccessStruct,
        Func<ValidationErrorStruct, TOut> matchValidationErrorStruct,
        Func<NotFoundErrorStruct, TOut> matchNotFoundErrorStruct)
    {
        if (IsSuccessStruct)
            return matchSuccessStruct(AsSuccessStruct);
        if (IsValidationErrorStruct)
            return matchValidationErrorStruct(AsValidationErrorStruct);
        if (IsNotFoundErrorStruct)
            return matchNotFoundErrorStruct(AsNotFoundErrorStruct);
        throw new InvalidOperationException("Unknown type");
    }

    public async Task<TOut> MatchAsync<TOut>(Func<SuccessStruct, CancellationToken, Task<TOut>> matchSuccessStruct,
        Func<ValidationErrorStruct, CancellationToken, Task<TOut>> matchValidationErrorStruct,
        Func<NotFoundErrorStruct, CancellationToken, Task<TOut>> matchNotFoundErrorStruct, CancellationToken ct)
    {
        if (IsSuccessStruct)
            return await matchSuccessStruct(AsSuccessStruct, ct).ConfigureAwait(false);
        if (IsValidationErrorStruct)
            return await matchValidationErrorStruct(AsValidationErrorStruct, ct).ConfigureAwait(false);
        if (IsNotFoundErrorStruct)
            return await matchNotFoundErrorStruct(AsNotFoundErrorStruct, ct).ConfigureAwait(false);
        throw new InvalidOperationException("Unknown type");
    }

    public void Switch(Action<SuccessStruct> switchSuccessStruct,
        Action<ValidationErrorStruct> switchValidationErrorStruct,
        Action<NotFoundErrorStruct> switchNotFoundErrorStruct)
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

    public async Task SwitchAsync(Func<SuccessStruct, CancellationToken, Task> switchSuccessStruct,
        Func<ValidationErrorStruct, CancellationToken, Task> switchValidationErrorStruct,
        Func<NotFoundErrorStruct, CancellationToken, Task> switchNotFoundErrorStruct, CancellationToken ct)
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

    public static bool operator ==(FooStructResultWithStructLayout left, FooStructResultWithStructLayout right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(FooStructResultWithStructLayout left, FooStructResultWithStructLayout right)
    {
        return !left.Equals(right);
    }

    public bool Equals(FooStructResultWithStructLayout other)
    {
        if (ValueType != other.ValueType)
        {
            return false;
        }

        if (IsSuccessStruct)
            return EqualityComparer<SuccessStruct>.Default.Equals(AsSuccessStruct, other.AsSuccessStruct);
        if (IsValidationErrorStruct)
            return EqualityComparer<ValidationErrorStruct>.Default.Equals(AsValidationErrorStruct,
                other.AsValidationErrorStruct);
        if (IsNotFoundErrorStruct)
            return EqualityComparer<NotFoundErrorStruct>.Default.Equals(AsNotFoundErrorStruct,
                other.AsNotFoundErrorStruct);
        throw new InvalidOperationException("Unknown type");
    }

    public override string ToString()
    {
        return $"{InnerValueAlias} - {InnerValue}";
    }

    public override bool Equals(object? obj)
    {
        return obj is FooStructResultWithStructLayout other && Equals(other);
    }
}