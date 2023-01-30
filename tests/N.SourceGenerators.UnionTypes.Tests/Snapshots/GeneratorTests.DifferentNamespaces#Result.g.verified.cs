﻿//HintName: Result.g.cs
// <auto-generated>
//   This code was generated by https://github.com/Ne4to/N.SourceGenerators.UnionTypes
//   Feel free to open an issue
// </auto-generated>
#pragma warning disable
#nullable enable
namespace MyApp.Domain.Child
{
    partial class Result : IEquatable<Result>
    {
        private readonly global::MyApp.Models.S.Child.Success? _success;
        public bool IsSuccess => _success != null;
        public global::MyApp.Models.S.Child.Success AsSuccess => _success ?? throw new InvalidOperationException("Inner value is not Success");
        public Result(global::MyApp.Models.S.Child.Success Success)
        {
            System.ArgumentNullException.ThrowIfNull(Success);
            _success = Success;
        }

        public static implicit operator Result(global::MyApp.Models.S.Child.Success Success) => new Result(Success);
        public static explicit operator global::MyApp.Models.S.Child.Success(Result value) => value._success;
        public bool TryGetSuccess([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::MyApp.Models.S.Child.Success? value)
        {
            if (_success != null)
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

        private readonly global::MyApp.Models.E.Child.Error? _error;
        public bool IsError => _error != null;
        public global::MyApp.Models.E.Child.Error AsError => _error ?? throw new InvalidOperationException("Inner value is not Error");
        public Result(global::MyApp.Models.E.Child.Error Error)
        {
            System.ArgumentNullException.ThrowIfNull(Error);
            _error = Error;
        }

        public static implicit operator Result(global::MyApp.Models.E.Child.Error Error) => new Result(Error);
        public static explicit operator global::MyApp.Models.E.Child.Error(Result value) => value._error;
        public bool TryGetError([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::MyApp.Models.E.Child.Error? value)
        {
            if (_error != null)
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

        public TOut Match<TOut>(global::System.Func<global::MyApp.Models.S.Child.Success, TOut> matchSuccess, global::System.Func<global::MyApp.Models.E.Child.Error, TOut> matchError)
        {
            if (_success != null)
                return matchSuccess(_success);
            if (_error != null)
                return matchError(_error);
            throw new InvalidOperationException("Inner type is unknown");
        }

        public async global::System.Threading.Tasks.Task<TOut> MatchAsync<TOut>(global::System.Func<global::MyApp.Models.S.Child.Success, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TOut>> matchSuccess, global::System.Func<global::MyApp.Models.E.Child.Error, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TOut>> matchError, global::System.Threading.CancellationToken ct)
        {
            if (_success != null)
                return await matchSuccess(_success, ct).ConfigureAwait(false);
            if (_error != null)
                return await matchError(_error, ct).ConfigureAwait(false);
            throw new InvalidOperationException("Inner type is unknown");
        }

        public void Switch(global::System.Action<global::MyApp.Models.S.Child.Success> switchSuccess, global::System.Action<global::MyApp.Models.E.Child.Error> switchError)
        {
            if (_success != null)
            {
                switchSuccess(_success);
                return;
            }

            if (_error != null)
            {
                switchError(_error);
                return;
            }

            throw new InvalidOperationException("Inner type is unknown");
        }

        public async global::System.Threading.Tasks.Task SwitchAsync(global::System.Func<global::MyApp.Models.S.Child.Success, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> switchSuccess, global::System.Func<global::MyApp.Models.E.Child.Error, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> switchError, global::System.Threading.CancellationToken ct)
        {
            if (_success != null)
            {
                await switchSuccess(_success, ct).ConfigureAwait(false);
                return;
            }

            if (_error != null)
            {
                await switchError(_error, ct).ConfigureAwait(false);
                return;
            }

            throw new InvalidOperationException("Inner type is unknown");
        }

        public global::System.Type ValueType
        {
            get
            {
                if (_success != null)
                    return typeof(global::MyApp.Models.S.Child.Success);
                if (_error != null)
                    return typeof(global::MyApp.Models.E.Child.Error);
                throw new InvalidOperationException("Inner type is unknown");
            }
        }

        public override int GetHashCode()
        {
            if (_success != null)
                return _success.GetHashCode();
            if (_error != null)
                return _error.GetHashCode();
            throw new InvalidOperationException("Inner type is unknown");
        }

        public static bool operator ==(Result? left, Result? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Result? left, Result? right)
        {
            return !Equals(left, right);
        }

        public bool Equals(Result? other)
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

            if (_success != null)
                return EqualityComparer<global::MyApp.Models.S.Child.Success>.Default.Equals(_success, other._success);
            if (_error != null)
                return EqualityComparer<global::MyApp.Models.E.Child.Error>.Default.Equals(_error, other._error);
            throw new InvalidOperationException("Inner type is unknown");
        }

        public override string ToString()
        {
            if (_success != null)
                return _success.ToString();
            if (_error != null)
                return _error.ToString();
            throw new InvalidOperationException("Inner type is unknown");
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

            if (other.GetType() != typeof(Result))
            {
                return false;
            }

            return Equals((Result)other);
        }
    }
}