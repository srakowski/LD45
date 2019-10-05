using System;

namespace WordGame.Logic
{
    public abstract class Item
    {
        public static Maybe<Item> CreateLoot(Random random, int tier)
        {
            return Maybe.None<Item>();
        }
    }
}