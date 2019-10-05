using System;

namespace WordGame.Logic
{
    public class Enemy
    {
        public Enemy(string name, int hp, int str, int def)
        {
            Name = name;
            HP = hp;
            Str = str;
            Def = def;
        }

        public string Name { get; }

        public int Tier => (MaxHP + Str + Def) / 10;

        public int HP { get; }

        public int MaxHP { get; }

        public int Str { get; }

        public int AttackDamage => Str;

        public int Def { get; }

        public int AttackDefense => Def;

        public Maybe<Enemy> TakeDamage(int points)
        {
            if (points >= HP) return Maybe.None<Enemy>();
            return new Enemy(Name, hp: HP - points, Str, Def).ToMaybe();
        }
    }
}