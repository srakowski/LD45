using System.Collections.Generic;
using System.Linq;

namespace WordGame.Logic
{
    public class GameState
    {
        private GameState(
            IWords words,
            StartsWith startsWith,
            CharBoard charBoard,
            IEnumerable<AttemptResult> attemptResults)
        {
            Words = words;
            StartsWith = startsWith;
            CharBoard = charBoard;
            AttemptResults = attemptResults;
        }

        public IWords Words { get; }

        public StartsWith StartsWith { get; }

        public CharBoard CharBoard { get; }

        public IEnumerable<AttemptResult> AttemptResults { get; }

        public static GameState New(IWords words)
        {
            var startsWith = new StartsWith('n', 'o');
            var charBoard = CharBoard.New(words, startsWith, "nothing");
            return new GameState(
                words,
                startsWith,
                charBoard,
                Enumerable.Empty<AttemptResult>()
            );
        }

        public Maybe<GameState> MakeAutoLetterSelection(char letter)
        {
            var maybeCharBoard = CharBoard.MakeAutoLetterSelection(letter);

            if (!maybeCharBoard.HasValue)
            {
                return Maybe.None<GameState>();
            }

            return new GameState(
                Words,
                StartsWith,
                maybeCharBoard.Value,
                AttemptResults
                ).ToMaybe();
        }

        public Maybe<GameState> UndoLastSelection()
        {
            var maybeCharBoard = CharBoard.UndoLastSelection();

            if (!maybeCharBoard.HasValue)
            {
                return Maybe.None<GameState>();
            }

            return new GameState(
                Words,
                StartsWith,
                maybeCharBoard.Value,
                AttemptResults
                ).ToMaybe();
        }

        public Maybe<GameState> CompleteWord()
        {
            if (!CharBoard.HasSelectedCharCells)
            {
                return Maybe.None<GameState>();
            }

            var word = new Word(StartsWith.Value + CharBoard.EndsWith);

            var seed = word.Value.Aggregate(0, (c, a) => (int)c + a);

            var attemptResult = CharBoard.HasWord(word)
                ? AttemptResult.Success(word.Value)
                : AttemptResult.Failure(word.Value);

            var nextStartsWith = Words.GetNextStartsWith(seed, attemptResult is AttemptResult.SuccessResult ? word.ToMaybe() : Maybe.None<Word>());

            return new GameState(
                Words,
                nextStartsWith,
                CharBoard.New(Words, nextStartsWith),
                AttemptResults.Prepend(attemptResult)
            ).ToMaybe();
        }
    }
}
