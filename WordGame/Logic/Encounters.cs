namespace WordGame.Logic
{
    public class Encounters
    {
        public static Encounter EnterDungeon()
        {
            return new Encounter(
                new Enemy("Bat", 3).ToMaybe<Enemy>(),
                new[]
                {
                    new Enemy("Racoon", 5),
                    new Enemy("Donkey", 8),
                }
            );
        }
    }
}
