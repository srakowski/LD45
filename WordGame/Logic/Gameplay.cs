using System;

namespace WordGame.Logic
{
    public class Gameplay
    {
        public Gameplay()
        {
            Words = new Words();
            Words.Initialize();
            CurrentState = GameState.New(Words, "Noob");
        }

        public Words Words { get; }

        public GameState CurrentState { get; private set; }

        internal bool MakeAutoLetterSelection(char c)
        {
            var result = CurrentState.MakeAutoLetterSelection(c);
            return HandleUpdateResult(result);
        }

        internal bool CompleteWord()
        {
            var result = CurrentState.CompleteWord();
            return HandleUpdateResult(result);
        }

        internal bool UndoLastSelection()
        {
            var result = CurrentState.UndoLastSelection();
            return HandleUpdateResult(result);
        }

        private bool HandleUpdateResult(Maybe<GameState> result)
        {
            if (!result.HasValue)
            {
                return false;
            }

            CurrentState = result.Value;
            return true;
        }
    }
}
