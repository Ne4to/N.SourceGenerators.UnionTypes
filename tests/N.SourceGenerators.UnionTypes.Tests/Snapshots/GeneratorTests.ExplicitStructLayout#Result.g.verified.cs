﻿//HintName: Result.g.cs
// <auto-generated>
//   This code was generated by https://github.com/Ne4to/N.SourceGenerators.UnionTypes
//   Feel free to open an issue
// </auto-generated>
#pragma warning disable
#nullable enable
namespace MyApp
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    partial struct Result : System.IEquatable<Result>
    {
        [System.Runtime.InteropServices.FieldOffset(0)]
        private readonly int _variantId;
        private const int SuccessStructId = 1;
        [System.Runtime.InteropServices.FieldOffset(4)]
        private readonly global::MyApp.SuccessStruct _successStruct;
        public bool IsSuccessStruct => _variantId == SuccessStructId;
        public global::MyApp.SuccessStruct AsSuccessStruct
        {
            get
            {
                if (_variantId == SuccessStructId)
                    return _successStruct;
                throw new System.InvalidOperationException("Inner value is not SuccessStruct");
            }
        }

        public Result(global::MyApp.SuccessStruct successStruct)
        {
            _variantId = SuccessStructId;
            _successStruct = successStruct;
        }

        public static implicit operator Result(global::MyApp.SuccessStruct successStruct) => new Result(successStruct);
        public static explicit operator global::MyApp.SuccessStruct(Result value)
        {
            if (value._variantId == SuccessStructId)
                return value._successStruct;
            throw new System.InvalidOperationException("Inner value is not SuccessStruct");
        }

        public bool TryGetSuccessStruct([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::MyApp.SuccessStruct value)
        {
            if (_variantId == SuccessStructId)
            {
                value = _successStruct;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        private const int ValidationErrorStructId = 2;
        [System.Runtime.InteropServices.FieldOffset(4)]
        private readonly global::MyApp.ValidationErrorStruct _validationErrorStruct;
        public bool IsValidationErrorStruct => _variantId == ValidationErrorStructId;
        public global::MyApp.ValidationErrorStruct AsValidationErrorStruct
        {
            get
            {
                if (_variantId == ValidationErrorStructId)
                    return _validationErrorStruct;
                throw new System.InvalidOperationException("Inner value is not ValidationErrorStruct");
            }
        }

        public Result(global::MyApp.ValidationErrorStruct validationErrorStruct)
        {
            _variantId = ValidationErrorStructId;
            _validationErrorStruct = validationErrorStruct;
        }

        public static implicit operator Result(global::MyApp.ValidationErrorStruct validationErrorStruct) => new Result(validationErrorStruct);
        public static explicit operator global::MyApp.ValidationErrorStruct(Result value)
        {
            if (value._variantId == ValidationErrorStructId)
                return value._validationErrorStruct;
            throw new System.InvalidOperationException("Inner value is not ValidationErrorStruct");
        }

        public bool TryGetValidationErrorStruct([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::MyApp.ValidationErrorStruct value)
        {
            if (_variantId == ValidationErrorStructId)
            {
                value = _validationErrorStruct;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        private const int NotFoundErrorStructId = 3;
        [System.Runtime.InteropServices.FieldOffset(4)]
        private readonly global::MyApp.NotFoundErrorStruct _notFoundErrorStruct;
        public bool IsNotFoundErrorStruct => _variantId == NotFoundErrorStructId;
        public global::MyApp.NotFoundErrorStruct AsNotFoundErrorStruct
        {
            get
            {
                if (_variantId == NotFoundErrorStructId)
                    return _notFoundErrorStruct;
                throw new System.InvalidOperationException("Inner value is not NotFoundErrorStruct");
            }
        }

        public Result(global::MyApp.NotFoundErrorStruct notFoundErrorStruct)
        {
            _variantId = NotFoundErrorStructId;
            _notFoundErrorStruct = notFoundErrorStruct;
        }

        public static implicit operator Result(global::MyApp.NotFoundErrorStruct notFoundErrorStruct) => new Result(notFoundErrorStruct);
        public static explicit operator global::MyApp.NotFoundErrorStruct(Result value)
        {
            if (value._variantId == NotFoundErrorStructId)
                return value._notFoundErrorStruct;
            throw new System.InvalidOperationException("Inner value is not NotFoundErrorStruct");
        }

        public bool TryGetNotFoundErrorStruct([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::MyApp.NotFoundErrorStruct value)
        {
            if (_variantId == NotFoundErrorStructId)
            {
                value = _notFoundErrorStruct;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public TOut Match<TOut>(global::System.Func<global::MyApp.SuccessStruct, TOut> matchSuccessStruct, global::System.Func<global::MyApp.ValidationErrorStruct, TOut> matchValidationErrorStruct, global::System.Func<global::MyApp.NotFoundErrorStruct, TOut> matchNotFoundErrorStruct)
        {
            if (_variantId == SuccessStructId)
                return matchSuccessStruct(_successStruct);
            if (_variantId == ValidationErrorStructId)
                return matchValidationErrorStruct(_validationErrorStruct);
            if (_variantId == NotFoundErrorStructId)
                return matchNotFoundErrorStruct(_notFoundErrorStruct);
            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public async global::System.Threading.Tasks.Task<TOut> MatchAsync<TOut>(global::System.Func<global::MyApp.SuccessStruct, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TOut>> matchSuccessStruct, global::System.Func<global::MyApp.ValidationErrorStruct, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TOut>> matchValidationErrorStruct, global::System.Func<global::MyApp.NotFoundErrorStruct, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TOut>> matchNotFoundErrorStruct, global::System.Threading.CancellationToken ct)
        {
            if (_variantId == SuccessStructId)
                return await matchSuccessStruct(_successStruct, ct).ConfigureAwait(false);
            if (_variantId == ValidationErrorStructId)
                return await matchValidationErrorStruct(_validationErrorStruct, ct).ConfigureAwait(false);
            if (_variantId == NotFoundErrorStructId)
                return await matchNotFoundErrorStruct(_notFoundErrorStruct, ct).ConfigureAwait(false);
            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public void Switch(global::System.Action<global::MyApp.SuccessStruct> switchSuccessStruct, global::System.Action<global::MyApp.ValidationErrorStruct> switchValidationErrorStruct, global::System.Action<global::MyApp.NotFoundErrorStruct> switchNotFoundErrorStruct)
        {
            if (_variantId == SuccessStructId)
            {
                switchSuccessStruct(_successStruct);
                return;
            }

            if (_variantId == ValidationErrorStructId)
            {
                switchValidationErrorStruct(_validationErrorStruct);
                return;
            }

            if (_variantId == NotFoundErrorStructId)
            {
                switchNotFoundErrorStruct(_notFoundErrorStruct);
                return;
            }

            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public async global::System.Threading.Tasks.Task SwitchAsync(global::System.Func<global::MyApp.SuccessStruct, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> switchSuccessStruct, global::System.Func<global::MyApp.ValidationErrorStruct, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> switchValidationErrorStruct, global::System.Func<global::MyApp.NotFoundErrorStruct, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> switchNotFoundErrorStruct, global::System.Threading.CancellationToken ct)
        {
            if (_variantId == SuccessStructId)
            {
                await switchSuccessStruct(_successStruct, ct).ConfigureAwait(false);
                return;
            }

            if (_variantId == ValidationErrorStructId)
            {
                await switchValidationErrorStruct(_validationErrorStruct, ct).ConfigureAwait(false);
                return;
            }

            if (_variantId == NotFoundErrorStructId)
            {
                await switchNotFoundErrorStruct(_notFoundErrorStruct, ct).ConfigureAwait(false);
                return;
            }

            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public global::System.Type ValueType
        {
            get
            {
                if (_variantId == SuccessStructId)
                    return typeof(global::MyApp.SuccessStruct);
                if (_variantId == ValidationErrorStructId)
                    return typeof(global::MyApp.ValidationErrorStruct);
                if (_variantId == NotFoundErrorStructId)
                    return typeof(global::MyApp.NotFoundErrorStruct);
                throw new System.InvalidOperationException("Inner type is unknown");
            }
        }

        public override int GetHashCode()
        {
            if (_variantId == SuccessStructId)
                return _successStruct.GetHashCode();
            if (_variantId == ValidationErrorStructId)
                return _validationErrorStruct.GetHashCode();
            if (_variantId == NotFoundErrorStructId)
                return _notFoundErrorStruct.GetHashCode();
            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public static bool operator ==(Result left, Result right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Result left, Result right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Result other)
        {
            if (ValueType != other.ValueType)
            {
                return false;
            }

            if (_variantId == SuccessStructId)
                return System.Collections.Generic.EqualityComparer<global::MyApp.SuccessStruct>.Default.Equals(_successStruct, other._successStruct);
            if (_variantId == ValidationErrorStructId)
                return System.Collections.Generic.EqualityComparer<global::MyApp.ValidationErrorStruct>.Default.Equals(_validationErrorStruct, other._validationErrorStruct);
            if (_variantId == NotFoundErrorStructId)
                return System.Collections.Generic.EqualityComparer<global::MyApp.NotFoundErrorStruct>.Default.Equals(_notFoundErrorStruct, other._notFoundErrorStruct);
            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public override string ToString()
        {
            if (_variantId == SuccessStructId)
                return _successStruct.ToString();
            if (_variantId == ValidationErrorStructId)
                return _validationErrorStruct.ToString();
            if (_variantId == NotFoundErrorStructId)
                return _notFoundErrorStruct.ToString();
            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public override bool Equals(object? obj)
        {
            return obj is Result other && Equals(other);
        }
    }
}