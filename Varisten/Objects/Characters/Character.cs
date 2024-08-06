using System.Collections.Generic;
using EngineFP;
using Microsoft.Xna.Framework.Graphics;
using Varisten.Objects.Items;
using Varisten.Objects.Items.Weapons;

namespace Varisten.Objects.Characters
{
    public abstract class Character : Instance
    {
        // Geral
        public string Name { get; set; }
        // Movement
        public float RightSpeed { get; set; }
        public float LeftSpeed { get; set; }
        public float UpSpeed { get; set; }
        public float DownSpeed { get; set; }
        public float Speed { get; protected set; }
        // Level
        public int Level { get; protected set; } = 1;
        public int LevelPoints { get; protected set; }
        public double Xp { get; protected set; }
        public double NextLevelXp { get; protected set; } = 15d;
        // Combat
        public double Life { get; set; }
        public int MaxLife { get; protected set; }
        public double Damage { get; protected set; }
        public double Defense { get; protected set; }
        public double CriticDamage { get; protected set; }
        // Chances to do
        public double DodgeChance { get; protected set; }
        public double CriticChance { get; protected set; }
        public double CounterattackChance { get; protected set; }
        // Money
        public double Coins { get; set; }
        // Inventory
        public List<Item> Bag { get; protected set; } = new List<Item>();
        public double BagWeight { get; protected set; }
        public double MaxBagWeight { get; protected set; }
        // Equipment
        public Weapon Weapon { get; protected set; }

        public Character(Texture2D hitboxSprite) : base(hitboxSprite) { }

        // Chance to do
        private bool Dodge()
        {
            if (Tool.NextDouble((double)Constant.Action) < DodgeChance)
                return true;
            return false;
        }

        private double Critic(double damage)
        {
            if (Tool.NextDouble((double)Constant.Action) < CriticChance)
                return damage *= CriticDamage;
            return damage;
        }

        // Combat
        public void Attack(Character enemy)
        {
            double _damage = Critic(Damage);
            if (!enemy.Dodge())
                enemy.Life -= Tool.NextDouble(_damage, _damage / 1.35d) - enemy.Defense;
        }

        // Inventory
        public void ItemCollect(Item item)
        {
            if (item.Weight + BagWeight <= MaxBagWeight)
            {
                Bag.Add(item);
                BagWeight += item.Weight;
            }
        }

        public void ItemDrop(Item item)
        {
            Bag.Remove(item);
            BagWeight -= item.Weight;
        }

        // Level
        public void XpCollect(double xp)
        {
            Xp += xp;
            while (Xp >= NextLevelXp)
            {
                Xp -= NextLevelXp;
                Level++;
                LevelPoints++;
                NextLevelXp += Level * 28;
            }
        }

        // Weapon
        public bool WeaponEquip(Weapon weapon)
        {
            if (Level > weapon.NecessaryLevel)
            {
                Weapon = weapon;
                Damage += weapon.Damage;
                DodgeChance += weapon.Effect.DodgeChance;
                CriticChance += weapon.Effect.CriticChance;
                CriticDamage += weapon.Effect.CriticDamage;
                return true;
            }
            return false;
        }

        public void WeaponUnequip()
        {
            if (Weapon != null)
            {
                Damage -= Weapon.Damage;
                DodgeChance -= Weapon.Effect.DodgeChance;
                CriticChance -= Weapon.Effect.CriticChance;
                CriticDamage -= Weapon.Effect.CriticDamage;
                Weapon = null;
            }
        }

        // Money

    }
}
