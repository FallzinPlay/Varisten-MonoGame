using Microsoft.Xna.Framework;
using System;

namespace EngineFP
{
    public class Hitbox
    {
        public Vector2 Top { get; private set; }
        public Vector2 Bottom { get; private set; }
        public Vector2 Left { get; private set; }
        public Vector2 Right { get; private set; }

        // Make a rectangle arround the inst using it's sprite (the origin point must be at the top-left)
        public Hitbox(Instance inst)
        {
            Top = new Vector2(inst.Position.X, inst.Position.Y);
            Right = new Vector2(inst.Position.X + inst.HitboxSprite.Width, inst.Position.Y);
            Bottom = new Vector2(inst.Position.X + inst.HitboxSprite.Width, inst.Position.Y + inst.HitboxSprite.Height);
            Left = new Vector2(inst.Position.X, inst.Position.Y + inst.HitboxSprite.Height);
        }

        // customized hitbox
        public Hitbox(Vector2 top, Vector2 bottom, Vector2 right, Vector2 left)
        {
            Top = top;
            Bottom = bottom;
            Right = right;
            Left = left;
        }
    }
}
