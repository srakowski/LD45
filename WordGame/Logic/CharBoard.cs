using System;
using System.Collections.Generic;
using System.Linq;

namespace WordGame.Logic
{
    public class CharBoard
    {
        private CharBoard(IEnumerable<CharCell> charCells, IEnumerable<Word> possibleWords, int nextSelectionIndex)
        {
            CharCells = charCells.ToArray();
            PossibleWords = possibleWords;
            NextSelectionIndex = nextSelectionIndex;
        }

        public IEnumerable<CharCell> CharCells { get; }

        public IEnumerable<Word> PossibleWords { get; }

        public int NextSelectionIndex { get; }

        public string EndsWith => new string(
            CharCells
                .Where(c => c.IsSelected)
                .OrderBy(c => c.SelectionIndex.Value)
                .Select(c => c.Value)
                .ToArray()
            );

        public bool HasSelectedCharCells => CharCells.Any(c => c.IsSelected);

        public Maybe<CharBoard> MakeAutoLetterSelection(char letter)
        {
            var maybeOpenCell = CharCells.FirstOrDefault(c => !c.IsSelected && c.Value == letter).ToMaybe();

            if (maybeOpenCell.HasValue)
            {
                var newCell = maybeOpenCell.Value.Select(NextSelectionIndex);
                var newCells = CharCells.Select(c => c == maybeOpenCell.Value ? newCell : c);
                return new CharBoard(newCells, PossibleWords, NextSelectionIndex + 1).ToMaybe();
            }

            return Maybe.None<CharBoard>();
        }

        public static CharBoard New(IWords words, StartsWith startsWith, Word forceInclude = null)
        {
            var wordSet = words.GetWordsThatStartWith(startsWith);
            return ConstructBoard(wordSet, new Random(23), forceInclude);
        }

        public bool HasWord(Word word)
        {
            return PossibleWords.Any(w => w.Value == word.Value);
        }

        public static CharBoard ConstructBoard(IEnumerable<Word> wordSet, Random random, Word forceInclude = null)
        {
            var characters = wordSet
                .OrderBy(w => w.Value == forceInclude?.Value ? 0 : w.Value.Length)
                .Aggregate(Enumerable.Empty<char>(), AddWord)
                .Take(16)
                .OrderBy(_ => random?.Next() ?? 0)
                .ToArray();

            var possibleWords = GetPossibleWords(characters, wordSet);

            return new CharBoard(
                characters.Select(c => new CharCell(c, Maybe.None<int>())).ToArray(),
                possibleWords,
                0
                );
        }

        private static IEnumerable<char> AddWord(IEnumerable<char> collection, Word word)
        {
            var lettersToAdd = new List<char>();
            var existingLetters = collection.ToList();
            foreach (var letter in word.EndsWithValue)
            {
                if (existingLetters.Contains(letter))
                {
                    existingLetters.Remove(letter);
                    continue;
                }
                lettersToAdd.Add(letter);
            }
            return collection.Concat(lettersToAdd).ToArray();
        }

        private static IEnumerable<Word> GetPossibleWords(IEnumerable<char> characters, IEnumerable<Word> wordSet)
        {
            var charCellFreqs = GetCharFreqs(characters);

            return wordSet
                .Select(w => new { Word = w, CharFreqs = GetCharFreqs(w.EndsWithValue) })
                .Where(w => w.CharFreqs.Keys.All(c => charCellFreqs.ContainsKey(c) && w.CharFreqs[c] <= charCellFreqs[c]))
                .Select(w => w.Word)
                .ToArray();
        }

        private static Dictionary<char, int> GetCharFreqs(IEnumerable<char> characters)
        {
            return characters
                .GroupBy(c => c)
                .ToDictionary(
                    k => k.Key,
                    v => v.Count()
                );
        }
    }
}