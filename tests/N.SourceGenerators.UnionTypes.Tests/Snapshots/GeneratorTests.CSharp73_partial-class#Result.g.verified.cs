﻿//HintName: Result.g.cs
// <auto-generated>
//   This code was generated by https://github.com/Ne4to/N.SourceGenerators.UnionTypes
//   Feel free to open an issue
// </auto-generated>
#pragma warning disable
namespace MyApp
{
    partial class Result : System.IEquatable<Result>
    {
        private readonly int _variantId;
        private const int SuccessId = 1;
        private readonly global::MyApp.Success _success;
        public bool IsSuccess => _variantId == SuccessId;
        public global::MyApp.Success AsSuccess
        {
            get
            {
                if (_variantId == SuccessId)
                    return _success;
                throw new System.InvalidOperationException($"Unable convert to Success. Inner value is {ValueAlias} not Success.");
            }
        }

        public Result(global::MyApp.Success success)
        {
            System.ArgumentNullException.ThrowIfNull(success);
            _variantId = SuccessId;
            _success = success;
        }

        public static implicit operator Result(global::MyApp.Success success) => new Result(success);
        public static explicit operator global::MyApp.Success(Result value)
        {
            if (value._variantId == SuccessId)
                return value._success;
            throw new System.InvalidOperationException($"Unable convert to Success. Inner value is {value.ValueAlias} not Success.");
        }

        public bool TryGetSuccess([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::MyApp.Success value)
        {
            if (_variantId == SuccessId)
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

        private const int ErrorId = 2;
        private readonly global::MyApp.Error _error;
        public bool IsError => _variantId == ErrorId;
        public global::MyApp.Error AsError
        {
            get
            {
                if (_variantId == ErrorId)
                    return _error;
                throw new System.InvalidOperationException($"Unable convert to Error. Inner value is {ValueAlias} not Error.");
            }
        }

        public Result(global::MyApp.Error @error)
        {
            System.ArgumentNullException.ThrowIfNull(@error);
            _variantId = ErrorId;
            _error = @error;
        }

        public static implicit operator Result(global::MyApp.Error @error) => new Result(@error);
        public static explicit operator global::MyApp.Error(Result value)
        {
            if (value._variantId == ErrorId)
                return value._error;
            throw new System.InvalidOperationException($"Unable convert to Error. Inner value is {value.ValueAlias} not Error.");
        }

        public bool TryGetError([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::MyApp.Error value)
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

        private const int IReadOnlyListOfInt32Id = 3;
        private readonly global::System.Collections.Generic.IReadOnlyList<int> _iReadOnlyListOfInt32;
        public bool IsIReadOnlyListOfInt32 => _variantId == IReadOnlyListOfInt32Id;
        public global::System.Collections.Generic.IReadOnlyList<int> AsIReadOnlyListOfInt32
        {
            get
            {
                if (_variantId == IReadOnlyListOfInt32Id)
                    return _iReadOnlyListOfInt32;
                throw new System.InvalidOperationException($"Unable convert to IReadOnlyListOfInt32. Inner value is {ValueAlias} not IReadOnlyListOfInt32.");
            }
        }

        public Result(global::System.Collections.Generic.IReadOnlyList<int> iReadOnlyListOfInt32)
        {
            System.ArgumentNullException.ThrowIfNull(iReadOnlyListOfInt32);
            _variantId = IReadOnlyListOfInt32Id;
            _iReadOnlyListOfInt32 = iReadOnlyListOfInt32;
        }

        public bool TryGetIReadOnlyListOfInt32([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::System.Collections.Generic.IReadOnlyList<int> value)
        {
            if (_variantId == IReadOnlyListOfInt32Id)
            {
                value = _iReadOnlyListOfInt32;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        private const int ArrayOfStringId = 4;
        private readonly string[] _arrayOfString;
        public bool IsArrayOfString => _variantId == ArrayOfStringId;
        public string[] AsArrayOfString
        {
            get
            {
                if (_variantId == ArrayOfStringId)
                    return _arrayOfString;
                throw new System.InvalidOperationException($"Unable convert to ArrayOfString. Inner value is {ValueAlias} not ArrayOfString.");
            }
        }

        public Result(string[] arrayOfString)
        {
            System.ArgumentNullException.ThrowIfNull(arrayOfString);
            _variantId = ArrayOfStringId;
            _arrayOfString = arrayOfString;
        }

        public static implicit operator Result(string[] arrayOfString) => new Result(arrayOfString);
        public static explicit operator string[](Result value)
        {
            if (value._variantId == ArrayOfStringId)
                return value._arrayOfString;
            throw new System.InvalidOperationException($"Unable convert to ArrayOfString. Inner value is {value.ValueAlias} not ArrayOfString.");
        }

        public bool TryGetArrayOfString([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out string[] value)
        {
            if (_variantId == ArrayOfStringId)
            {
                value = _arrayOfString;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        private const int TupleOfInt32AndStringId = 5;
        private readonly global::System.Tuple<int, string> _tupleOfInt32AndString;
        public bool IsTupleOfInt32AndString => _variantId == TupleOfInt32AndStringId;
        public global::System.Tuple<int, string> AsTupleOfInt32AndString
        {
            get
            {
                if (_variantId == TupleOfInt32AndStringId)
                    return _tupleOfInt32AndString;
                throw new System.InvalidOperationException($"Unable convert to TupleOfInt32AndString. Inner value is {ValueAlias} not TupleOfInt32AndString.");
            }
        }

        public Result(global::System.Tuple<int, string> tupleOfInt32AndString)
        {
            System.ArgumentNullException.ThrowIfNull(tupleOfInt32AndString);
            _variantId = TupleOfInt32AndStringId;
            _tupleOfInt32AndString = tupleOfInt32AndString;
        }

        public static implicit operator Result(global::System.Tuple<int, string> tupleOfInt32AndString) => new Result(tupleOfInt32AndString);
        public static explicit operator global::System.Tuple<int, string>(Result value)
        {
            if (value._variantId == TupleOfInt32AndStringId)
                return value._tupleOfInt32AndString;
            throw new System.InvalidOperationException($"Unable convert to TupleOfInt32AndString. Inner value is {value.ValueAlias} not TupleOfInt32AndString.");
        }

        public bool TryGetTupleOfInt32AndString([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::System.Tuple<int, string> value)
        {
            if (_variantId == TupleOfInt32AndStringId)
            {
                value = _tupleOfInt32AndString;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public TOut Match<TOut>(global::System.Func<global::MyApp.Success, TOut> matchSuccess, global::System.Func<global::MyApp.Error, TOut> matchError, global::System.Func<global::System.Collections.Generic.IReadOnlyList<int>, TOut> matchIReadOnlyListOfInt32, global::System.Func<string[], TOut> matchArrayOfString, global::System.Func<global::System.Tuple<int, string>, TOut> matchTupleOfInt32AndString)
        {
            if (_variantId == SuccessId)
                return matchSuccess(_success);
            if (_variantId == ErrorId)
                return matchError(_error);
            if (_variantId == IReadOnlyListOfInt32Id)
                return matchIReadOnlyListOfInt32(_iReadOnlyListOfInt32);
            if (_variantId == ArrayOfStringId)
                return matchArrayOfString(_arrayOfString);
            if (_variantId == TupleOfInt32AndStringId)
                return matchTupleOfInt32AndString(_tupleOfInt32AndString);
            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public async global::System.Threading.Tasks.Task<TOut> MatchAsync<TOut>(global::System.Func<global::MyApp.Success, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TOut>> matchSuccess, global::System.Func<global::MyApp.Error, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TOut>> matchError, global::System.Func<global::System.Collections.Generic.IReadOnlyList<int>, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TOut>> matchIReadOnlyListOfInt32, global::System.Func<string[], global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TOut>> matchArrayOfString, global::System.Func<global::System.Tuple<int, string>, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TOut>> matchTupleOfInt32AndString, global::System.Threading.CancellationToken ct)
        {
            if (_variantId == SuccessId)
                return await matchSuccess(_success, ct).ConfigureAwait(false);
            if (_variantId == ErrorId)
                return await matchError(_error, ct).ConfigureAwait(false);
            if (_variantId == IReadOnlyListOfInt32Id)
                return await matchIReadOnlyListOfInt32(_iReadOnlyListOfInt32, ct).ConfigureAwait(false);
            if (_variantId == ArrayOfStringId)
                return await matchArrayOfString(_arrayOfString, ct).ConfigureAwait(false);
            if (_variantId == TupleOfInt32AndStringId)
                return await matchTupleOfInt32AndString(_tupleOfInt32AndString, ct).ConfigureAwait(false);
            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public void Switch(global::System.Action<global::MyApp.Success> switchSuccess, global::System.Action<global::MyApp.Error> switchError, global::System.Action<global::System.Collections.Generic.IReadOnlyList<int>> switchIReadOnlyListOfInt32, global::System.Action<string[]> switchArrayOfString, global::System.Action<global::System.Tuple<int, string>> switchTupleOfInt32AndString)
        {
            if (_variantId == SuccessId)
            {
                switchSuccess(_success);
                return;
            }

            if (_variantId == ErrorId)
            {
                switchError(_error);
                return;
            }

            if (_variantId == IReadOnlyListOfInt32Id)
            {
                switchIReadOnlyListOfInt32(_iReadOnlyListOfInt32);
                return;
            }

            if (_variantId == ArrayOfStringId)
            {
                switchArrayOfString(_arrayOfString);
                return;
            }

            if (_variantId == TupleOfInt32AndStringId)
            {
                switchTupleOfInt32AndString(_tupleOfInt32AndString);
                return;
            }

            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public async global::System.Threading.Tasks.Task SwitchAsync(global::System.Func<global::MyApp.Success, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> switchSuccess, global::System.Func<global::MyApp.Error, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> switchError, global::System.Func<global::System.Collections.Generic.IReadOnlyList<int>, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> switchIReadOnlyListOfInt32, global::System.Func<string[], global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> switchArrayOfString, global::System.Func<global::System.Tuple<int, string>, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> switchTupleOfInt32AndString, global::System.Threading.CancellationToken ct)
        {
            if (_variantId == SuccessId)
            {
                await switchSuccess(_success, ct).ConfigureAwait(false);
                return;
            }

            if (_variantId == ErrorId)
            {
                await switchError(_error, ct).ConfigureAwait(false);
                return;
            }

            if (_variantId == IReadOnlyListOfInt32Id)
            {
                await switchIReadOnlyListOfInt32(_iReadOnlyListOfInt32, ct).ConfigureAwait(false);
                return;
            }

            if (_variantId == ArrayOfStringId)
            {
                await switchArrayOfString(_arrayOfString, ct).ConfigureAwait(false);
                return;
            }

            if (_variantId == TupleOfInt32AndStringId)
            {
                await switchTupleOfInt32AndString(_tupleOfInt32AndString, ct).ConfigureAwait(false);
                return;
            }

            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public global::System.Type ValueType
        {
            get
            {
                if (_variantId == SuccessId)
                    return typeof(global::MyApp.Success);
                if (_variantId == ErrorId)
                    return typeof(global::MyApp.Error);
                if (_variantId == IReadOnlyListOfInt32Id)
                    return typeof(global::System.Collections.Generic.IReadOnlyList<int>);
                if (_variantId == ArrayOfStringId)
                    return typeof(string[]);
                if (_variantId == TupleOfInt32AndStringId)
                    return typeof(global::System.Tuple<int, string>);
                throw new System.InvalidOperationException("Inner type is unknown");
            }
        }

        private string ValueAlias
        {
            get
            {
                if (_variantId == SuccessId)
                    return "Success";
                if (_variantId == ErrorId)
                    return "Error";
                if (_variantId == IReadOnlyListOfInt32Id)
                    return "IReadOnlyListOfInt32";
                if (_variantId == ArrayOfStringId)
                    return "ArrayOfString";
                if (_variantId == TupleOfInt32AndStringId)
                    return "TupleOfInt32AndString";
                throw new System.InvalidOperationException("Inner type is unknown");
            }
        }

        public override int GetHashCode()
        {
            if (_variantId == SuccessId)
                return _success.GetHashCode();
            if (_variantId == ErrorId)
                return _error.GetHashCode();
            if (_variantId == IReadOnlyListOfInt32Id)
                return _iReadOnlyListOfInt32.GetHashCode();
            if (_variantId == ArrayOfStringId)
                return _arrayOfString.GetHashCode();
            if (_variantId == TupleOfInt32AndStringId)
                return _tupleOfInt32AndString.GetHashCode();
            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public static bool operator ==(Result left, Result right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Result left, Result right)
        {
            return !Equals(left, right);
        }

        public bool Equals(Result other)
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

            if (_variantId == SuccessId)
                return System.Collections.Generic.EqualityComparer<global::MyApp.Success>.Default.Equals(_success, other._success);
            if (_variantId == ErrorId)
                return System.Collections.Generic.EqualityComparer<global::MyApp.Error>.Default.Equals(_error, other._error);
            if (_variantId == IReadOnlyListOfInt32Id)
                return System.Collections.Generic.EqualityComparer<global::System.Collections.Generic.IReadOnlyList<int>>.Default.Equals(_iReadOnlyListOfInt32, other._iReadOnlyListOfInt32);
            if (_variantId == ArrayOfStringId)
                return System.Collections.Generic.EqualityComparer<string[]>.Default.Equals(_arrayOfString, other._arrayOfString);
            if (_variantId == TupleOfInt32AndStringId)
                return System.Collections.Generic.EqualityComparer<global::System.Tuple<int, string>>.Default.Equals(_tupleOfInt32AndString, other._tupleOfInt32AndString);
            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public override string ToString()
        {
            if (_variantId == SuccessId)
                return _success.ToString();
            if (_variantId == ErrorId)
                return _error.ToString();
            if (_variantId == IReadOnlyListOfInt32Id)
                return _iReadOnlyListOfInt32.ToString();
            if (_variantId == ArrayOfStringId)
                return _arrayOfString.ToString();
            if (_variantId == TupleOfInt32AndStringId)
                return _tupleOfInt32AndString.ToString();
            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other.GetType() != typeof(Result))
            {
                return false;
            }

            return Equals((Result)other);
        }
    }
}