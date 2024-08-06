using Microsoft.Xna.Framework.Graphics;

namespace Varisten.Objects.Atributes
{
    public class Sprite
    {
        public Texture2D Idle { get; private set; }
        public Texture2D Walking { get; private set; }

        public Sprite(Texture2D idle, Texture2D walking)
        {
            Idle = idle;
            Walking = walking;
        }
    }
}
