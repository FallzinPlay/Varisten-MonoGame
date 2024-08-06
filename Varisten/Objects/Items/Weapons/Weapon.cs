using Microsoft.Xna.Framework.Graphics;
using Varisten.Objects.Items;

namespace Varisten.Objects.Items.Weapons
{
    public abstract class Weapon : Item
    {
        // Combat
        public double Damage { get; protected set; }
        // Level
        public int NecessaryLevel { get; protected set; }

        public Weapon(Texture2D hitboxSprite) : base(hitboxSprite) { }
    }
}
