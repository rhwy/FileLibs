namespace Rhwy.RailwayResult
{
	//author : Rui Carvalho <@rhwy>
	//source : http://github.com/rhwy/filelibs/cs/rhwy.railwayresult/railwayresult.cs
	//updated: 2014-11-13
	//version: 0.1.0
	using System;
	using System.Collections.Generic;

	public abstract class Result<T>
	{
		public bool IsSuccess { get; private set;}
		public T Value { get; private set;}
		public ResultContext Context { get; private set;}

		public Result (bool isSuccess, ResultContext context, T value)
		{
			IsSuccess = isSuccess;
			Context = context;
			Value = value;
		}

		public Result<U> then<U>(Func<T,Result<U>> success,Func<Result<T>,Result<U>> error)
		{
			if (IsSuccess) {
				return success (Value);
			} else {
				return error (this);
			}
		}

		public Result<U> then<U>(Func<T,Result<U>> success)
		{
			if (IsSuccess) {
				return success (Value);
			} else {
				return new Failure<U>(this.Context);
			}
		}

		public Result<Unit> then(Action<T> success,Action<Result<T>> error)
		{
			if (IsSuccess) {
				success (Value);
				return Result.FromSuccess<Unit> ();
			} else {
				error (this);
				return Result.FromFailure<Unit> ();
			}
		}

		public Result<Unit> then(Action<T> success)
		{
			if (IsSuccess) {
				success (Value);
				return Result.FromSuccess<Unit> ();
			} else {
				return Result.FromFailure<Unit> ();
			}
		}
	}


	public static class Result
	{
		public static Failure<T> FromFailure<T>(ResultContext context, T value)
		{
			return new Failure<T> (context, value);
		}
		public static Failure<T> FromFailure<T>(ResultContext context)
		{
			return new Failure<T> (context);
		}
		public static Failure<T> FromFailure<T>(string message)
		{
			return new Failure<T> (new ResultContext(message));
		}
		public static Failure<T> FromFailure<T>()
		{
			return new Failure<T> ();
		}
		public static Success<T> FromSuccess<T>(ResultContext context, T value)
		{
			return new Success<T> (context, value);
		}
		public static Success<T> FromSuccess<T>(T value)
		{
			return new Success<T> (value);
		}
		public static Success<T> FromSuccess<T>()
		{
			return new Success<T> ();
		}
	}


	public class Failure<T> : Result<T>
	{
		public Failure (ResultContext context, T value)
			:base(false,context,value)
		{
		}

		public Failure (ResultContext context)
			:base(false,context,default(T))
		{
		}
		public Failure ()
			:base(false,ResultContext.Empty,default(T))
		{
		}
	}
	public class Success<T> : Result<T>
	{
		public Success (ResultContext context, T value)
			:base(true,context,value)
		{
		}

		public Success (T value)
			:base(true,ResultContext.Empty,value)
		{
		}
		public Success ()
			:base(true,ResultContext.Empty,default(T))
		{
		}
	}


	public class ResultContext
	{
		public string Message  {
			get;
			private set;
		}

		public Dictionary<string,object> Params { get; private set; }

		public ResultContext (string message=null, Dictionary<string, object> parameters=null)
		{
			Message = message ?? string.Empty;
			Params = parameters??new Dictionary<string, object>();

		}

		public static ResultContext Empty {
			get{ return new ResultContext (); }
		}
	}


	public class Unit{
		private static Unit voidResult = new Unit();
		private Unit ()
		{
			
		}
		public static Unit FromVoid {
			get{ return voidResult; }
		}
	}
}


