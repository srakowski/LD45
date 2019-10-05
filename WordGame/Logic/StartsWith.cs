namespace WordGame.Logic
{
    public class StartsWith
    {
        public StartsWith(char first, char second)
        {
            Value = $"{first}{second}";
        }

        public string Value { get; }

        public char First => Value[2];

        public char Second => Value[1];
    }
}
