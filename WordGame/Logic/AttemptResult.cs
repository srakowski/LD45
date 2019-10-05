using System;

namespace WordGame.Logic
{
    public abstract class AttemptResult
    {
        public AttemptResult(string attemptedWord)
        {
            AttemptedWord = attemptedWord;
        }

        public string AttemptedWord { get; }

        public class SuccessResult : AttemptResult
        {
            public SuccessResult(string attemptedWord) : base(attemptedWord)
            {
            }
        }

        public class FailureResult : AttemptResult
        {
            public FailureResult(string attemptedWord) : base(attemptedWord)
            {
            }
        }

        internal static AttemptResult Success(string value) => new SuccessResult(value);
        internal static AttemptResult Failure(string value) => new FailureResult(value);
    }
}
