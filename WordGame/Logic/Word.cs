using System.Collections.Generic;

namespace WordGame.Logic
{
    public class Word
    {
        public Word(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public string EndsWithValue => Value.Substring(2);

        public string StartsWithValue => Value.Substring(0, 2);

        public static implicit operator Word(string value) => new Word(value);
    }
}
