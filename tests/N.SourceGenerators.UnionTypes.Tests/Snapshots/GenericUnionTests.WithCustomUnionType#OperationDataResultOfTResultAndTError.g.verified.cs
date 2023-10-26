﻿//HintName: OperationDataResultOfTResultAndTError.g.cs
// <auto-generated>
//   This code was generated by https://github.com/Ne4to/N.SourceGenerators.UnionTypes
//   Feel free to open an issue
// </auto-generated>
#pragma warning disable
#nullable enable
partial class OperationDataResult<TResult, TError> : System.IEquatable<OperationDataResult<TResult, TError>>
{
    private readonly int _variantId;
    private const int ResultId = 1;
    private readonly TResult _result;
    public bool IsResult => _variantId == ResultId;
    public TResult AsResult
    {
        get
        {
            if (_variantId == ResultId)
                return _result;
            throw new System.InvalidOperationException($"Unable convert to Result. Inner value is {ValueAlias} not Result.");
        }
    }

    public OperationDataResult(TResult result)
    {
        System.ArgumentNullException.ThrowIfNull(result);
        _variantId = ResultId;
        _result = result;
    }

    public static implicit operator OperationDataResult<TResult, TError>(TResult result) => new OperationDataResult<TResult, TError>(result);
    public static explicit operator TResult(OperationDataResult<TResult, TError> value)
    {
        if (value._variantId == ResultId)
            return value._result;
        throw new System.InvalidOperationException($"Unable convert to Result. Inner value is {value.ValueAlias} not Result.");
    }

    public bool TryGetResult([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TResult value)
    {
        if (_variantId == ResultId)
        {
            value = _result;
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }

    private const int ErrorId = 2;
    private readonly TError _error;
    public bool IsError => _variantId == ErrorId;
    public TError AsError
    {
        get
        {
            if (_variantId == ErrorId)
                return _error;
            throw new System.InvalidOperationException($"Unable convert to Error. Inner value is {ValueAlias} not Error.");
        }
    }

    public OperationDataResult(TError @error)
    {
        System.ArgumentNullException.ThrowIfNull(@error);
        _variantId = ErrorId;
        _error = @error;
    }

    public static implicit operator OperationDataResult<TResult, TError>(TError @error) => new OperationDataResult<TResult, TError>(@error);
    public static explicit operator TError(OperationDataResult<TResult, TError> value)
    {
        if (value._variantId == ErrorId)
            return value._error;
        throw new System.InvalidOperationException($"Unable convert to Error. Inner value is {value.ValueAlias} not Error.");
    }

    public bool TryGetError([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TError value)
    {
        if (_variantId == ErrorId)
        {
            value = _error;
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }

    private const int Int32Id = 3;
    private readonly int _int32;
    public bool IsInt32 => _variantId == Int32Id;
    public int AsInt32
    {
        get
        {
            if (_variantId == Int32Id)
                return _int32;
            throw new System.InvalidOperationException($"Unable convert to Int32. Inner value is {ValueAlias} not Int32.");
        }
    }

    public OperationDataResult(int int32)
    {
        System.ArgumentNullException.ThrowIfNull(int32);
        _variantId = Int32Id;
        _int32 = int32;
    }

    public static implicit operator OperationDataResult<TResult, TError>(int int32) => new OperationDataResult<TResult, TError>(int32);
    public static explicit operator int(OperationDataResult<TResult, TError> value)
    {
        if (value._variantId == Int32Id)
            return value._int32;
        throw new System.InvalidOperationException($"Unable convert to Int32. Inner value is {value.ValueAlias} not Int32.");
    }

    public bool TryGetInt32([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out int value)
    {
        if (_variantId == Int32Id)
        {
            value = _int32;
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }

    public TOut Match<TOut>(global::System.Func<TResult, TOut> matchResult, global::System.Func<TError, TOut> matchError, global::System.Func<int, TOut> matchInt32)
    {
        if (_variantId == ResultId)
            return matchResult(_result);
        if (_variantId == ErrorId)
            return matchError(_error);
        if (_variantId == Int32Id)
            return matchInt32(_int32);
        throw new System.InvalidOperationException("Inner type is unknown");
    }

    public async global::System.Threading.Tasks.Task<TOut> MatchAsync<TOut>(global::System.Func<TResult, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TOut>> matchResult, global::System.Func<TError, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TOut>> matchError, global::System.Func<int, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TOut>> matchInt32, global::System.Threading.CancellationToken ct)
    {
        if (_variantId == ResultId)
            return await matchResult(_result, ct).ConfigureAwait(false);
        if (_variantId == ErrorId)
            return await matchError(_error, ct).ConfigureAwait(false);
        if (_variantId == Int32Id)
            return await matchInt32(_int32, ct).ConfigureAwait(false);
        throw new System.InvalidOperationException("Inner type is unknown");
    }

    public void Switch(global::System.Action<TResult> switchResult, global::System.Action<TError> switchError, global::System.Action<int> switchInt32)
    {
        if (_variantId == ResultId)
        {
            switchResult(_result);
            return;
        }

        if (_variantId == ErrorId)
        {
            switchError(_error);
            return;
        }

        if (_variantId == Int32Id)
        {
            switchInt32(_int32);
            return;
        }

        throw new System.InvalidOperationException("Inner type is unknown");
    }

    public async global::System.Threading.Tasks.Task SwitchAsync(global::System.Func<TResult, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> switchResult, global::System.Func<TError, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> switchError, global::System.Func<int, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> switchInt32, global::System.Threading.CancellationToken ct)
    {
        if (_variantId == ResultId)
        {
            await switchResult(_result, ct).ConfigureAwait(false);
            return;
        }

        if (_variantId == ErrorId)
        {
            await switchError(_error, ct).ConfigureAwait(false);
            return;
        }

        if (_variantId == Int32Id)
        {
            await switchInt32(_int32, ct).ConfigureAwait(false);
            return;
        }

        throw new System.InvalidOperationException("Inner type is unknown");
    }

    public global::System.Type ValueType
    {
        get
        {
            if (_variantId == ResultId)
                return typeof(TResult);
            if (_variantId == ErrorId)
                return typeof(TError);
            if (_variantId == Int32Id)
                return typeof(int);
            throw new System.InvalidOperationException("Inner type is unknown");
        }
    }

    private string ValueAlias
    {
        get
        {
            if (_variantId == ResultId)
                return "Result";
            if (_variantId == ErrorId)
                return "Error";
            if (_variantId == Int32Id)
                return "Int32";
            throw new System.InvalidOperationException("Inner type is unknown");
        }
    }

    public override int GetHashCode()
    {
        if (_variantId == ResultId)
            return _result.GetHashCode();
        if (_variantId == ErrorId)
            return _error.GetHashCode();
        if (_variantId == Int32Id)
            return _int32.GetHashCode();
        throw new System.InvalidOperationException("Inner type is unknown");
    }

    public static bool operator ==(OperationDataResult<TResult, TError>? left, OperationDataResult<TResult, TError>? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(OperationDataResult<TResult, TError>? left, OperationDataResult<TResult, TError>? right)
    {
        return !Equals(left, right);
    }

    public bool Equals(OperationDataResult<TResult, TError>? other)
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

        if (_variantId == ResultId)
            return System.Collections.Generic.EqualityComparer<TResult>.Default.Equals(_result, other._result);
        if (_variantId == ErrorId)
            return System.Collections.Generic.EqualityComparer<TError>.Default.Equals(_error, other._error);
        if (_variantId == Int32Id)
            return System.Collections.Generic.EqualityComparer<int>.Default.Equals(_int32, other._int32);
        throw new System.InvalidOperationException("Inner type is unknown");
    }

    public override string ToString()
    {
        if (_variantId == ResultId)
            return _result.ToString();
        if (_variantId == ErrorId)
            return _error.ToString();
        if (_variantId == Int32Id)
            return _int32.ToString();
        throw new System.InvalidOperationException("Inner type is unknown");
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

        if (other.GetType() != typeof(OperationDataResult<TResult, TError>))
        {
            return false;
        }

        return Equals((OperationDataResult<TResult, TError>)other);
    }
}