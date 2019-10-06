using System;
using System.Collections.Generic;
using System.Linq;

namespace Wordgeon.Logic
{
    public struct Maybe<T>
    {
        private IEnumerable<T> _value;

        public Maybe(T value)
        {
            _value = value == null
                ? Enumerable.Empty<T>()
                : new[] { value }.AsEnumerable();
        }

        public bool HasValue => _value?.Any() ?? false;

        public T Value => _value == null ? default : _value.SingleOrDefault();

        public T ValueOr(Func<T> value) => HasValue ? Value : value();

        public Maybe<TResult> Select<TResult>(Func<T, TResult> map) =>
            HasValue ? map(Value).ToMaybe() : Maybe.None<TResult>();

        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> bind) =>
            HasValue ? bind(Value) : Maybe.None<TResult>();

        public static implicit operator Maybe<T>(T value) => new Maybe<T>(value);
    }

    public static class Maybe
    {
        public static Maybe<T> None<T>() => new Maybe<T>();

        public static Maybe<T> Some<T>(T value) => new Maybe<T>(value);

        public static Maybe<T> ToMaybe<T>(this T value) => value == null ? None<T>() : Some(value);
    }
}
