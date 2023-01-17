﻿//HintName: Result.g.cs
// <auto-generated/>
#pragma warning disable
#nullable enable
partial class Result
{
    private readonly global::Success? _Success;
    public bool IsSuccess => _Success != null;
    public global::Success AsSuccess => _Success ?? throw new InvalidOperationException("This is not a Success");
    public Result(global::Success Success)
    {
        System.ArgumentNullException.ThrowIfNull(Success);
        _Success = Success;
    }

    public static implicit operator Result(global::Success Success) => new Result(Success);
    public static explicit operator global::Success(Result value) => value.AsSuccess;
    private readonly global::Error? _Error;
    public bool IsError => _Error != null;
    public global::Error AsError => _Error ?? throw new InvalidOperationException("This is not a Error");
    public Result(global::Error Error)
    {
        System.ArgumentNullException.ThrowIfNull(Error);
        _Error = Error;
    }

    public static implicit operator Result(global::Error Error) => new Result(Error);
    public static explicit operator global::Error(Result value) => value.AsError;
    public TOut Match<TOut>(global::System.Func<global::Success, TOut> matchSuccess, global::System.Func<global::Error, TOut> matchError)
    {
        if (IsSuccess)
            return matchSuccess(AsSuccess);
        if (IsError)
            return matchError(AsError);
        throw new InvalidOperationException("Unknown type");
    }

    public async global::System.Threading.Tasks.Task<TOut> MatchAsync<TOut>(global::System.Func<global::Success, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TOut>> matchSuccess, global::System.Func<global::Error, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TOut>> matchError, global::System.Threading.CancellationToken ct)
    {
        if (IsSuccess)
            return await matchSuccess(AsSuccess, ct).ConfigureAwait(false);
        if (IsError)
            return await matchError(AsError, ct).ConfigureAwait(false);
        throw new InvalidOperationException("Unknown type");
    }

    public void Switch(global::System.Action<global::Success> switchSuccess, global::System.Action<global::Error> switchError)
    {
        if (IsSuccess)
        {
            switchSuccess(AsSuccess);
            return;
        }

        if (IsError)
        {
            switchError(AsError);
            return;
        }

        throw new InvalidOperationException("Unknown type");
    }

    public async global::System.Threading.Tasks.Task SwitchAsync(global::System.Func<global::Success, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> switchSuccess, global::System.Func<global::Error, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> switchError, global::System.Threading.CancellationToken ct)
    {
        if (IsSuccess)
        {
            await switchSuccess(AsSuccess, ct).ConfigureAwait(false);
            return;
        }

        if (IsError)
        {
            await switchError(AsError, ct).ConfigureAwait(false);
            return;
        }

        throw new InvalidOperationException("Unknown type");
    }
}