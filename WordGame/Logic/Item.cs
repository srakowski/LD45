using System;
using System.Linq;

namespace WordGame.Logic
{
    using static Die;

    public abstract class Item
    {
        public Item(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public static Maybe<Item> CreateLoot(Random random)
        {
            var tier1 = new Func<Item>[]
            {
                Weapon.Dagger,
                Weapon.Spear,
                Armor.Leather,
                Armor.StuddedLeather,
                Potion.Healing,
            };

            var tier2 = (new Func<Item>[]
            {
                Weapon.Mace,
                Armor.ScaleMail,
            });

            var tier3 = (new Func<Item>[]
            {
                Potion.Poison,
                Weapon.LongSword,
                Armor.ChainMail,
            });


            var tier4 = (new Func<Item>[]
            {
                Weapon.TwoHandedSword,
                Armor.PlateMail,
                Potion.RaiseLevel
            });

            var value = random.Next(100);
            if (value < 20)
            {
                return tier1[random.Next(tier1.Length)]().ToMaybe();
            }
            else if (value < 13)
            {
                return tier2[random.Next(tier2.Length)]().ToMaybe();
            }
            else if (value < 5)
            {
                return tier3[random.Next(tier3.Length)]().ToMaybe();
            }
            else if (value < 3)
            {
                return tier4[random.Next(tier4.Length)]().ToMaybe();
            }
            else if (value < 20)
            {
               // maybe gold?
            }
            return Maybe.None<Item>();
        }
    }

    public class Weapon : Item
    {
        private Weapon(string name, Dice damage) : base(name)
        {
            Damage = damage;
        }

        public Dice Damage { get; }


        public static Weapon Fists() => new Weapon("Fists", new Dice(D(2), D(2)));

        public static Weapon Dagger() => new Weapon("Dagger", new Dice(D(3), D(3)));

        public static Weapon Spear() => new Weapon("Spear", new Dice(D(4), D(4)));

        public static Weapon Mace() => new Weapon("Mace", new Dice(D(4), D(4), D(4)));

        public static Weapon LongSword() => new Weapon("Long sword", new Dice(D(4), D(4), D(4), D(4)));

        public static Weapon TwoHandedSword() => new Weapon("Two handed sword", new Dice(D(4), D(4), D(4), D(4), D(4)));

        public int RollDmg(Random rand)
        {
            return Damage.RollAndSum(rand);
        }
    }


    public class Armor : Item
    {
        private Armor(string name, int ac) : base(name)
        {
            AC = ac;
        }

        public int AC { get; }

        public static Armor Leather() => new Armor("Leather", 3);

        public static Armor StuddedLeather() => new Armor("Studded leather", 4);

        public static Armor ScaleMail() => new Armor("Scale mail", 5);

        public static Armor ChainMail() => new Armor("Chain mail", 6);

        public static Armor PlateMail() => new Armor("Plate mail", 7);
    }

    public class Potion : Item
    {
        private Potion(string name) : base(name) { }

        public static Potion Healing() => new Potion("Healing");

        public static Potion Poison() => new Potion("Poison");

        public static Potion RaiseLevel() => new Potion("Raise level");
    }
}