﻿//HintName: BusinessLogicResult.g.cs
// <auto-generated>
//   This code was generated by https://github.com/Ne4to/N.SourceGenerators.UnionTypes
//   Feel free to open an issue
// </auto-generated>
#pragma warning disable
#nullable enable
namespace MyApp
{
    partial class BusinessLogicResult : System.IEquatable<BusinessLogicResult>
    {
        private readonly global::MyApp.Success? _success;
        public bool IsSuccess => _success != null;
        public global::MyApp.Success AsSuccess => _success ?? throw new System.InvalidOperationException("Inner value is not Success");
        public BusinessLogicResult(global::MyApp.Success success)
        {
            System.ArgumentNullException.ThrowIfNull(success);
            _success = success;
        }

        public static implicit operator BusinessLogicResult(global::MyApp.Success success) => new BusinessLogicResult(success);
        public static explicit operator global::MyApp.Success(BusinessLogicResult value) => value._success ?? throw new System.InvalidOperationException("Inner value is not Success");
        public bool TryGetSuccess([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::MyApp.Success? value)
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

        private readonly global::MyApp.NotFoundError? _notFoundError;
        public bool IsNotFoundError => _notFoundError != null;
        public global::MyApp.NotFoundError AsNotFoundError => _notFoundError ?? throw new System.InvalidOperationException("Inner value is not NotFoundError");
        public BusinessLogicResult(global::MyApp.NotFoundError notFoundError)
        {
            System.ArgumentNullException.ThrowIfNull(notFoundError);
            _notFoundError = notFoundError;
        }

        public static implicit operator BusinessLogicResult(global::MyApp.NotFoundError notFoundError) => new BusinessLogicResult(notFoundError);
        public static explicit operator global::MyApp.NotFoundError(BusinessLogicResult value) => value._notFoundError ?? throw new System.InvalidOperationException("Inner value is not NotFoundError");
        public bool TryGetNotFoundError([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::MyApp.NotFoundError? value)
        {
            if (_notFoundError != null)
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

        private readonly global::MyApp.ValidationError? _validationError;
        public bool IsValidationError => _validationError != null;
        public global::MyApp.ValidationError AsValidationError => _validationError ?? throw new System.InvalidOperationException("Inner value is not ValidationError");
        public BusinessLogicResult(global::MyApp.ValidationError validationError)
        {
            System.ArgumentNullException.ThrowIfNull(validationError);
            _validationError = validationError;
        }

        public static implicit operator BusinessLogicResult(global::MyApp.ValidationError validationError) => new BusinessLogicResult(validationError);
        public static explicit operator global::MyApp.ValidationError(BusinessLogicResult value) => value._validationError ?? throw new System.InvalidOperationException("Inner value is not ValidationError");
        public bool TryGetValidationError([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::MyApp.ValidationError? value)
        {
            if (_validationError != null)
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

        public TOut Match<TOut>(global::System.Func<global::MyApp.Success, TOut> matchSuccess, global::System.Func<global::MyApp.NotFoundError, TOut> matchNotFoundError, global::System.Func<global::MyApp.ValidationError, TOut> matchValidationError)
        {
            if (_success != null)
                return matchSuccess(_success!);
            if (_notFoundError != null)
                return matchNotFoundError(_notFoundError!);
            if (_validationError != null)
                return matchValidationError(_validationError!);
            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public async global::System.Threading.Tasks.Task<TOut> MatchAsync<TOut>(global::System.Func<global::MyApp.Success, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TOut>> matchSuccess, global::System.Func<global::MyApp.NotFoundError, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TOut>> matchNotFoundError, global::System.Func<global::MyApp.ValidationError, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TOut>> matchValidationError, global::System.Threading.CancellationToken ct)
        {
            if (_success != null)
                return await matchSuccess(_success!, ct).ConfigureAwait(false);
            if (_notFoundError != null)
                return await matchNotFoundError(_notFoundError!, ct).ConfigureAwait(false);
            if (_validationError != null)
                return await matchValidationError(_validationError!, ct).ConfigureAwait(false);
            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public void Switch(global::System.Action<global::MyApp.Success> switchSuccess, global::System.Action<global::MyApp.NotFoundError> switchNotFoundError, global::System.Action<global::MyApp.ValidationError> switchValidationError)
        {
            if (_success != null)
            {
                switchSuccess(_success!);
                return;
            }

            if (_notFoundError != null)
            {
                switchNotFoundError(_notFoundError!);
                return;
            }

            if (_validationError != null)
            {
                switchValidationError(_validationError!);
                return;
            }

            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public async global::System.Threading.Tasks.Task SwitchAsync(global::System.Func<global::MyApp.Success, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> switchSuccess, global::System.Func<global::MyApp.NotFoundError, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> switchNotFoundError, global::System.Func<global::MyApp.ValidationError, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> switchValidationError, global::System.Threading.CancellationToken ct)
        {
            if (_success != null)
            {
                await switchSuccess(_success!, ct).ConfigureAwait(false);
                return;
            }

            if (_notFoundError != null)
            {
                await switchNotFoundError(_notFoundError!, ct).ConfigureAwait(false);
                return;
            }

            if (_validationError != null)
            {
                await switchValidationError(_validationError!, ct).ConfigureAwait(false);
                return;
            }

            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public global::System.Type ValueType
        {
            get
            {
                if (_success != null)
                    return typeof(global::MyApp.Success);
                if (_notFoundError != null)
                    return typeof(global::MyApp.NotFoundError);
                if (_validationError != null)
                    return typeof(global::MyApp.ValidationError);
                throw new System.InvalidOperationException("Inner type is unknown");
            }
        }

        public override int GetHashCode()
        {
            if (_success != null)
                return _success.GetHashCode();
            if (_notFoundError != null)
                return _notFoundError.GetHashCode();
            if (_validationError != null)
                return _validationError.GetHashCode();
            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public static bool operator ==(BusinessLogicResult? left, BusinessLogicResult? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BusinessLogicResult? left, BusinessLogicResult? right)
        {
            return !Equals(left, right);
        }

        public bool Equals(BusinessLogicResult? other)
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
                return System.Collections.Generic.EqualityComparer<global::MyApp.Success>.Default.Equals(_success!, other._success);
            if (_notFoundError != null)
                return System.Collections.Generic.EqualityComparer<global::MyApp.NotFoundError>.Default.Equals(_notFoundError!, other._notFoundError);
            if (_validationError != null)
                return System.Collections.Generic.EqualityComparer<global::MyApp.ValidationError>.Default.Equals(_validationError!, other._validationError);
            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public override string ToString()
        {
            if (_success != null)
                return _success.ToString();
            if (_notFoundError != null)
                return _notFoundError.ToString();
            if (_validationError != null)
                return _validationError.ToString();
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

            if (other.GetType() != typeof(BusinessLogicResult))
            {
                return false;
            }

            return Equals((BusinessLogicResult)other);
        }
    }
}