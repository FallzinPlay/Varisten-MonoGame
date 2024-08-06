using MonoGame.Extended;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Dynamics;
using mVector2 = Microsoft.Xna.Framework.Vector2;
using aVector2 = nkast.Aether.Physics2D.Common.Vector2;
using Varisten.Objects.Atributes;
using Varisten.Objects.Characters;

namespace Varisten.Objects
{
    public abstract class Instance
    {
        public mVector2 Position
        {
            get => new mVector2(Body.Position.X, Body.Position.Y);
            set => Body.Position = new aVector2(value.X, value.Y);
        }
        public float X { get => Position.X; set => Position = new mVector2(value, Position.Y); }
        public float Y { get => Position.Y; set => Position = new mVector2(Position.X, value); }
        public Texture2D Sprite { get; set; }
        public Body Body { get; private set; }
        public mVector2 OriginPoint { get; private set; }
        public Hitbox Hitbox { get; private set; }

        public void SetBody(World world, aVector2 size, float density, aVector2 position)
        {
            Body = world.CreateRectangle(size.X, size.Y, density, position);
        }

        public void SetOriginPoint(Texture2D shape)
        {
            OriginPoint = new mVector2(X + shape.Width / 2, Y + shape.Height);
            Hitbox = new Hitbox(
                new RectangleF(X + 5f, Y, Sprite.Width /2 + 5f, 1f),
                new RectangleF(X + 5f, Y + Sprite.Height, Sprite.Width /2 + 5f, 1f),
                new RectangleF(X + Sprite.Width - 5f, Y, 1f, Sprite.Height),
                new RectangleF(X + 5f, Y, 1f, Sprite.Height));
        }
    }
}
