using System;

namespace WordGame.Logic
{
    public class Enemy
    {
        public Enemy(string name, int hp, int maxHP, int str, int def, Dice weaponDice)
        {
            Name = name;
            HP = hp;
            MaxHP = maxHP;
            Off = str;
            Def = def;
            WeaponDice = weaponDice;
        }

        public string Name { get; }

        public int Tier => (MaxHP + Off + Def) / 10;

        public int HP { get; }

        public int MaxHP { get; }

        public int Off { get; }

        public Dice WeaponDice { get;  }

        public int AttackDamage(Random random, int bonus)
        {
            var weaponDmg = WeaponDice.RollAndSum(random);
            var mult = Math.Clamp((Constants.DmgMod + Off + bonus), Constants.DmgMod, int.MaxValue) / Constants.DmgMod;
            return (int)(weaponDmg * mult);
        }

        public int Def { get; }

        public int AttackDefense => Def;

        public Maybe<Enemy> TakeDamage(int points)
        {
            if (points >= HP) return Maybe.None<Enemy>();
            return new Enemy(Name, HP - points, MaxHP, Off, Def, WeaponDice).ToMaybe();
        }
    }
}