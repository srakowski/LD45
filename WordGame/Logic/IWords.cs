using System.Collections.Generic;

namespace WordGame.Logic
{
    public interface IWords
    {
        IEnumerable<Word> GetWordsThatStartWith(StartsWith startsWith);
        StartsWith GetNextStartsWith(int seed, Maybe<Word> foundWord);
    }
}