using System.Collections.Generic;
using System.Linq;

namespace WordGame.Logic
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
    }

    public static class Maybe
    {
        public static Maybe<T> None<T>() => new Maybe<T>();
        public static Maybe<T> Some<T>(T value) => new Maybe<T>(value);

        public static Maybe<T> ToMaybe<T>(this T value) => value == null ? None<T>() : Some(value);
    }
}
