using System;
using System.Collections.Generic;
using System.Linq;

namespace WordGame.Logic
{
    public class Player
    {
        private Player(
            string name,
            int hp,
            int xp,
            int level,
            int str,
            int def,
            Maybe<Weapon> weapon,
            Maybe<Armor> armor,
            IEnumerable<Item> inventory)
        {
            Name = name;
            HP = hp;
            XP = xp;
            Level = level;
            Str = str;
            Def = def;
            Weapon = weapon;
            Armor = armor;
            Inventory = inventory;
        }

        private Player(string playerName, Player player, int? hp = null)
        {
            Name = player.Name;
            HP = hp ?? player.HP;
            XP = player.XP;
            Level = player.Level;
            Str = player.Str;
            Def = player.Def;
            Weapon = player.Weapon;
            Armor = player.Armor;
            Inventory = player.Inventory;
        }

        public string Name { get; }

        public int HP { get; }

        public int XP { get; }

        public int Level { get; }

        public int Str { get; }

        public int AttackDamage => Str;

        public int Def { get; }

        public int AttackDefense => Def;

        public Maybe<Weapon> Weapon { get; }

        public Maybe<Armor> Armor { get; }

        public IEnumerable<Item> Inventory { get; }

        public bool IsAlive => HP > 0;

        public static Player New(string playerName)
        {
            return new Player(
                playerName,
                Constants.StartingHP, 
                0,
                1,
                1,
                1,
                Maybe.None<Weapon>(),
                Maybe.None<Armor>(),
                Enumerable.Empty<Item>());
        }

        public Player TakeDamage(int points)
        {
            return new Player(Name, this, hp: Math.Clamp(HP - points, 0, int.MaxValue));
        }

        public Player TakePartialDamage(int points)
        {
            var effectivePoints = Math.Clamp(points - AttackDefense, 0, int.MaxValue);
            return TakeDamage(effectivePoints);
        }
    }
}
