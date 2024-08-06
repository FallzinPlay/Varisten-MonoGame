using Microsoft.Xna.Framework.Graphics;
using Varisten.Objects.Items.Weapons;
using Varisten.Objects.Items;

namespace Varisten.Objects.Characters
{
    public class Player : Character
    {
        public Player(Texture2D hitboxSprite, string name) : base(hitboxSprite)
        {
            Name = name;
            Speed = 100f;
            Life = MaxLife;
            MaxLife = 15;
            Damage = 1.50d;
            Defense = 2.10d;
            CriticDamage = 1.30d;
            DodgeChance = 2.20d;
            CriticChance = 2.15d;
            CounterattackChance = 1.25d;
            MaxBagWeight = 20;
            Coins = 5.00d;
        }
    }
}
