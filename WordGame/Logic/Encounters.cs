namespace WordGame.Logic
{
    using System;

    public class Encounters
    {
        public static Encounter EnterDungeon(Random rand)
        {
            var hpDice = new Dice(Die.D(6), Die.D(6), Die.D(6), Die.D(6));
            var stDice = new Dice(Die.D(6), Die.D(6), Die.D(6));
            var dmgDice = new Dice(Die.D(2), Die.D(2));

            return new Encounter(
                Maybe.None<Enemy>(),
                new[]
                {
                    RollEnenmy("E1", rand, hpDice, stDice, dmgDice),
                    RollEnenmy("E2", rand, hpDice, stDice, dmgDice),
                    RollEnenmy("E3", rand, hpDice, stDice, dmgDice),
                }
            );
        }

        private static Enemy RollEnenmy(string name, Random rand, Dice hpDice, Dice stDice, Dice weapDice)
        {
            var hp = hpDice.RollAndSum(rand);
            return new Enemy(name, hp, hp, stDice.RollAndSum(rand), stDice.RollAndSum(rand), weapDice);
        }
    }
}
