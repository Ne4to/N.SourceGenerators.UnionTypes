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
        private readonly int _variantId;
        private const int NotFoundErrorId = 1;
        private readonly global::MyApp.NotFoundError _notFoundError;
        public bool IsNotFoundError => _variantId == NotFoundErrorId;
        public global::MyApp.NotFoundError AsNotFoundError
        {
            get
            {
                if (_variantId == NotFoundErrorId)
                    return _notFoundError;
                throw new System.InvalidOperationException("Inner value is not NotFoundError");
            }
        }

        public BusinessLogicResult(global::MyApp.NotFoundError notFoundError)
        {
            System.ArgumentNullException.ThrowIfNull(notFoundError);
            _variantId = NotFoundErrorId;
            _notFoundError = notFoundError;
        }

        public static implicit operator BusinessLogicResult(global::MyApp.NotFoundError notFoundError) => new BusinessLogicResult(notFoundError);
        public static explicit operator global::MyApp.NotFoundError(BusinessLogicResult value)
        {
            if (value._variantId == NotFoundErrorId)
                return value._notFoundError;
            throw new System.InvalidOperationException("Inner value is not NotFoundError");
        }

        public bool TryGetNotFoundError([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::MyApp.NotFoundError value)
        {
            if (_variantId == NotFoundErrorId)
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

        private const int ValidationErrorId = 2;
        private readonly global::MyApp.ValidationError _validationError;
        public bool IsValidationError => _variantId == ValidationErrorId;
        public global::MyApp.ValidationError AsValidationError
        {
            get
            {
                if (_variantId == ValidationErrorId)
                    return _validationError;
                throw new System.InvalidOperationException("Inner value is not ValidationError");
            }
        }

        public BusinessLogicResult(global::MyApp.ValidationError validationError)
        {
            System.ArgumentNullException.ThrowIfNull(validationError);
            _variantId = ValidationErrorId;
            _validationError = validationError;
        }

        public static implicit operator BusinessLogicResult(global::MyApp.ValidationError validationError) => new BusinessLogicResult(validationError);
        public static explicit operator global::MyApp.ValidationError(BusinessLogicResult value)
        {
            if (value._variantId == ValidationErrorId)
                return value._validationError;
            throw new System.InvalidOperationException("Inner value is not ValidationError");
        }

        public bool TryGetValidationError([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::MyApp.ValidationError value)
        {
            if (_variantId == ValidationErrorId)
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

        public TOut Match<TOut>(global::System.Func<global::MyApp.NotFoundError, TOut> matchNotFoundError, global::System.Func<global::MyApp.ValidationError, TOut> matchValidationError)
        {
            if (_variantId == NotFoundErrorId)
                return matchNotFoundError(_notFoundError);
            if (_variantId == ValidationErrorId)
                return matchValidationError(_validationError);
            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public async global::System.Threading.Tasks.Task<TOut> MatchAsync<TOut>(global::System.Func<global::MyApp.NotFoundError, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TOut>> matchNotFoundError, global::System.Func<global::MyApp.ValidationError, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TOut>> matchValidationError, global::System.Threading.CancellationToken ct)
        {
            if (_variantId == NotFoundErrorId)
                return await matchNotFoundError(_notFoundError, ct).ConfigureAwait(false);
            if (_variantId == ValidationErrorId)
                return await matchValidationError(_validationError, ct).ConfigureAwait(false);
            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public void Switch(global::System.Action<global::MyApp.NotFoundError> switchNotFoundError, global::System.Action<global::MyApp.ValidationError> switchValidationError)
        {
            if (_variantId == NotFoundErrorId)
            {
                switchNotFoundError(_notFoundError);
                return;
            }

            if (_variantId == ValidationErrorId)
            {
                switchValidationError(_validationError);
                return;
            }

            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public async global::System.Threading.Tasks.Task SwitchAsync(global::System.Func<global::MyApp.NotFoundError, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> switchNotFoundError, global::System.Func<global::MyApp.ValidationError, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> switchValidationError, global::System.Threading.CancellationToken ct)
        {
            if (_variantId == NotFoundErrorId)
            {
                await switchNotFoundError(_notFoundError, ct).ConfigureAwait(false);
                return;
            }

            if (_variantId == ValidationErrorId)
            {
                await switchValidationError(_validationError, ct).ConfigureAwait(false);
                return;
            }

            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public global::System.Type ValueType
        {
            get
            {
                if (_variantId == NotFoundErrorId)
                    return typeof(global::MyApp.NotFoundError);
                if (_variantId == ValidationErrorId)
                    return typeof(global::MyApp.ValidationError);
                throw new System.InvalidOperationException("Inner type is unknown");
            }
        }

        public override int GetHashCode()
        {
            if (_variantId == NotFoundErrorId)
                return _notFoundError.GetHashCode();
            if (_variantId == ValidationErrorId)
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

            if (_variantId == NotFoundErrorId)
                return System.Collections.Generic.EqualityComparer<global::MyApp.NotFoundError>.Default.Equals(_notFoundError, other._notFoundError);
            if (_variantId == ValidationErrorId)
                return System.Collections.Generic.EqualityComparer<global::MyApp.ValidationError>.Default.Equals(_validationError, other._validationError);
            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public override string ToString()
        {
            if (_variantId == NotFoundErrorId)
                return _notFoundError.ToString();
            if (_variantId == ValidationErrorId)
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