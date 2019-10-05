using System;
using System.Collections.Generic;
using System.Linq;

namespace WordGame.Logic
{
    public class Dice
    {
        private IEnumerable<Die> dice;

        public Dice(params Die[] dice)
        {
            this.dice = dice;
        }

        public int RollAndSum(Random rand) => dice.Select(d => d.Roll(rand)).Sum();
    }
    
    public class Die
    {
        public Die(int sides)
        {
            Sides = sides;
        }

        public int Sides { get; }

        public int Roll(Random rand)
        {
            return rand.Next(Sides) + 1;
        }

        public static Die D(int sides) => new Die(sides);
    }
}