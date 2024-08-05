using Microsoft.Xna.Framework;

namespace Varisten.Objects
{
    public abstract class Instance
    {
        public float X { get; set; }
        public float Y { get; set; }
        public Rectangle Hitbox { get; set; }
    }
}
