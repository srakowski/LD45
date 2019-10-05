namespace WordGame
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using global::WordGame.Logic;

    public class Words : IWords
    {
        private Dictionary<string, List<string>> _wordsByStartsWith;

        public void Initialize()
        {
            var words = File.ReadAllLines(@".\Content\words.txt");

            _wordsByStartsWith = words
                .Where(w => w.Length > 2)
                .Select(w => new { BeginsWith = w.Substring(0, 2), Word = w.ToLower() })
                .GroupBy(w => w.BeginsWith)
                .Where(w => w.SelectMany(s => s.Word).Count() > 16)
                .Select(s => new { Set = s, Count = CharBoard.ConstructBoard(s.Select(r => new Word(r.Word)), null).PossibleWords.Count() })
                .Where(s => s.Count > 3)
                .Select(s => s.Set)
                .ToDictionary(
                    k => k.Key,
                    v => v.Select(r => r.Word).ToList()
                );

            Debug.WriteLine(_wordsByStartsWith.Keys.Count());
            Debug.WriteLine(_wordsByStartsWith.Values.SelectMany(v => v).Count());
        }

        public IEnumerable<Word> GetWordsThatStartWith(StartsWith startsWith)
        {
            return _wordsByStartsWith[startsWith.Value].Select(v => new Word(v));
        }

        public StartsWith GetNextStartsWith(int seed, Maybe<Word> foundWord)
        {
            if (foundWord.HasValue)
            {
                var fw = foundWord.Value;
                var set = _wordsByStartsWith[fw.StartsWithValue];
                set.Remove(fw.Value);
                if (set.Count < 3)
                {
                    _wordsByStartsWith.Remove(fw.StartsWithValue);
                }
            }

            var r = new Random(seed);
            var next = _wordsByStartsWith.Keys
                .OrderByDescending(s =>
                {
                    var set = _wordsByStartsWith[s];
                    return CharBoard.ConstructBoard(set.Select(x => new Word(x)), r).PossibleWords.Count();
                })
                .Take(100)
                .OrderByDescending(_ => r.Next())
                .First();

            return new StartsWith(next[0], next[1]);
        }
    }
}
