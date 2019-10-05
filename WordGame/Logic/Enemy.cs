using System;

namespace WordGame.Logic
{
    public class Enemy
    {
        public Enemy(string name, int hp)
        {
            Name = name;
            HP = hp;
        }

        public string Name { get; }

        public int HP { get; }

        public Maybe<Enemy> TakeDamage(int points)
        {
            if (points >= HP) return Maybe.None<Enemy>();
            return new Enemy(Name, hp: HP - points).ToMaybe();
        }
    }
}