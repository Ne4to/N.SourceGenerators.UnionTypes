﻿//HintName: Result.g.cs
// <auto-generated/>
#pragma warning disable
#nullable enable
namespace MyApp
{
    partial class Result
    {
        private readonly MyApp.Success? _Success;
        public bool IsSuccess => _Success != null;
        public MyApp.Success AsSuccess => _Success ?? throw new System.InvalidOperationException("This is not a Success");
        public Result(MyApp.Success Success)
        {
            System.ArgumentNullException.ThrowIfNull(Success);
            _Success = Success;
        }

        public static implicit operator Result(MyApp.Success Success) => new Result(Success);
        public static explicit operator MyApp.Success(Result value) => value.AsSuccess;
        private readonly MyApp.Error? _Error;
        public bool IsError => _Error != null;
        public MyApp.Error AsError => _Error ?? throw new System.InvalidOperationException("This is not a Error");
        public Result(MyApp.Error Error)
        {
            System.ArgumentNullException.ThrowIfNull(Error);
            _Error = Error;
        }

        public static implicit operator Result(MyApp.Error Error) => new Result(Error);
        public static explicit operator MyApp.Error(Result value) => value.AsError;
    }
}