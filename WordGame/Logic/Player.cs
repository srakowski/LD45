using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WordGame.Logic
{
    public class Player
    {
        private Player(
            string name,
            int hp,
            int maxHP,
            int xp,
            int level,
            int str,
            int cons,
            Weapon weapon,
            Maybe<Armor> armor,
            IEnumerable<Item> inventory)
        {
            Name = name;
            HP = hp;
            MaxHP = maxHP;
            XP = xp;
            Level = level;
            Off = str;
            Def = cons;
            Weapon = weapon;
            Armor = armor;
            Inventory = inventory;
        }

        private Player(
            Player player,
            int? hp = null,
            int? xp = null,
            IEnumerable<Item> inventory = null)
        {
            Name = player.Name;
            HP = hp ?? player.HP;
            MaxHP = player.MaxHP;
            XP = xp ?? player.XP;
            Level = player.Level;
            Off = player.Off;
            Def = player.Def;
            Weapon = player.Weapon;
            Armor = player.Armor;
            Inventory = inventory ?? player.Inventory;
        }

        public string Name { get; }

        public int HP { get; }

        public int MaxHP { get; }

        public int XP { get; }

        public int Level { get; }

        public int Off { get; }

        public int AttackDamage(Random rand, int bonus)
        {
            var weaponDmg = Weapon.RollDmg(rand);
            var mult = (Constants.DmgMod + Off + bonus) / Constants.DmgMod;
            var result = (int)(weaponDmg * mult);
            Debug.WriteLine($"{weaponDmg}*{mult}={result}");
            return result;
        }

        public int Def { get; }

        public int AttackDefense => Def + Armor.Select(a => a.AC).ValueOr(() => 0);

        public Weapon Weapon { get; }

        public Maybe<Armor> Armor { get; }

        public IEnumerable<Item> Inventory { get; }

        public bool IsAlive => HP > 0;

        public static Player New(string playerName, Random random)
        {
            var dice = new Dice(Die.D(6), Die.D(6), Die.D(6));
            return new Player(
                playerName,
                Constants.StartingHP, 
                Constants.StartingHP,
                0,
                1,
                dice.RollAndSum(random),
                dice.RollAndSum(random),
                Weapon.Fists(),
                Maybe.None<Armor>(),
                Enumerable.Empty<Item>());
        }

        public Player TakeDamage(int points)
        {
            return new Player(this, hp: Math.Clamp(HP - points, 0, int.MaxValue));
        }

        public Player TakeMitigatedDamage(int points)
        {
            var modifier = Constants.DmgMod / (Constants.DmgMod + AttackDefense);
            var effectivePoints = (int)(points * modifier);
            return TakeDamage(effectivePoints);
        }

        internal Player Progress(int xp, IEnumerable<Item> lootEscrow)
        {
            return new Player(this,
                xp: XP + xp,
                inventory: Inventory.Concat(lootEscrow).ToArray());
        }
    }
}
