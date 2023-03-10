//HintName: DataAccessResult.g.cs
// <auto-generated>
//   This code was generated by https://github.com/Ne4to/N.SourceGenerators.UnionTypes
//   Feel free to open an issue
// </auto-generated>
#pragma warning disable
#nullable enable
namespace MyApp
{
    partial class DataAccessResult : System.IEquatable<DataAccessResult>
    {
        private readonly global::MyApp.Success? _success;
        public bool IsSuccess => _success != null;
        public global::MyApp.Success AsSuccess => _success ?? throw new System.InvalidOperationException("Inner value is not Success");
        public DataAccessResult(global::MyApp.Success success)
        {
            System.ArgumentNullException.ThrowIfNull(success);
            _success = success;
        }

        public static implicit operator DataAccessResult(global::MyApp.Success success) => new DataAccessResult(success);
        public static explicit operator global::MyApp.Success(DataAccessResult value) => value._success ?? throw new System.InvalidOperationException("Inner value is not Success");
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
        public DataAccessResult(global::MyApp.NotFoundError notFoundError)
        {
            System.ArgumentNullException.ThrowIfNull(notFoundError);
            _notFoundError = notFoundError;
        }

        public static implicit operator DataAccessResult(global::MyApp.NotFoundError notFoundError) => new DataAccessResult(notFoundError);
        public static explicit operator global::MyApp.NotFoundError(DataAccessResult value) => value._notFoundError ?? throw new System.InvalidOperationException("Inner value is not NotFoundError");
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

        public TOut Match<TOut>(global::System.Func<global::MyApp.Success, TOut> matchSuccess, global::System.Func<global::MyApp.NotFoundError, TOut> matchNotFoundError)
        {
            if (_success != null)
                return matchSuccess(_success!);
            if (_notFoundError != null)
                return matchNotFoundError(_notFoundError!);
            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public async global::System.Threading.Tasks.Task<TOut> MatchAsync<TOut>(global::System.Func<global::MyApp.Success, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TOut>> matchSuccess, global::System.Func<global::MyApp.NotFoundError, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TOut>> matchNotFoundError, global::System.Threading.CancellationToken ct)
        {
            if (_success != null)
                return await matchSuccess(_success!, ct).ConfigureAwait(false);
            if (_notFoundError != null)
                return await matchNotFoundError(_notFoundError!, ct).ConfigureAwait(false);
            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public void Switch(global::System.Action<global::MyApp.Success> switchSuccess, global::System.Action<global::MyApp.NotFoundError> switchNotFoundError)
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

            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public async global::System.Threading.Tasks.Task SwitchAsync(global::System.Func<global::MyApp.Success, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> switchSuccess, global::System.Func<global::MyApp.NotFoundError, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> switchNotFoundError, global::System.Threading.CancellationToken ct)
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
                throw new System.InvalidOperationException("Inner type is unknown");
            }
        }

        public override int GetHashCode()
        {
            if (_success != null)
                return _success.GetHashCode();
            if (_notFoundError != null)
                return _notFoundError.GetHashCode();
            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public static bool operator ==(DataAccessResult? left, DataAccessResult? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DataAccessResult? left, DataAccessResult? right)
        {
            return !Equals(left, right);
        }

        public bool Equals(DataAccessResult? other)
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
            throw new System.InvalidOperationException("Inner type is unknown");
        }

        public override string ToString()
        {
            if (_success != null)
                return _success.ToString();
            if (_notFoundError != null)
                return _notFoundError.ToString();
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

            if (other.GetType() != typeof(DataAccessResult))
            {
                return false;
            }

            return Equals((DataAccessResult)other);
        }
    }
}