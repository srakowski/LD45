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
            Weapon weapon,
            IEnumerable<Item> inventory)
        {
            Name = name;
            HP = hp;
            XP = xp;
            Level = level;
            Str = str;
            Def = def;
            Weapon = weapon;
            Inventory = inventory;
        }

        private Player(Player player)
        {
            Name = player.Name;
            HP = player.HP;
            XP = player.XP;
            Level = player.Level;
            Str = player.Str;
            Def = player.Def;
            Weapon = player.Weapon;
            Inventory = player.Inventory;
        }

        public string Name { get; }

        public int HP { get; }

        public int XP { get; }

        public int Level { get; }

        public int Str { get; }

        public int Def { get; }

        public Weapon Weapon { get; }

        public IEnumerable<Item> Inventory { get; }

        public static Player New(string playerName)
        {
            return new Player(playerName, 10, 0, 1, 1, 1, new Weapons.None(), Enumerable.Empty<Item>());
        }

        public Player TakeDamage(int combatValue)
        {
            return new Player(this);
        }
    }
}
