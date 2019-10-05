namespace WordGame.Logic
{
    public class Encounters
    {
        public static Encounter EnterDungeon()
        {
            return new Encounter(
                Maybe.None<Enemy>(),
                new[]
                {
                    new Enemy("Bat", 4, 1, 1),
                    new Enemy("Racoon", 5, 1, 1),
                    new Enemy("Donkey", 6, 2, 2),
                }
            );
        }
    }
}
