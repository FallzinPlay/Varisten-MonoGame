﻿using MonoGame.Extended;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Dynamics;
using mVector2 = Microsoft.Xna.Framework.Vector2;
using aVector2 = nkast.Aether.Physics2D.Common.Vector2;

namespace EngineFP
{
    public abstract class Instance
    {
        public mVector2 Position {  get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public Texture2D Sprite { get; set; }
        public Body Body { get; private set; }
        public mVector2 OriginPoint { get; private set; }
        public Hitbox Hitbox { get; private set; }
        public Texture2D HitboxSprite { get; private set; }

        public Instance(Texture2D hitboxSprite)
        {
            HitboxSprite = hitboxSprite;
            SetOriginPoint();
            SetHitbox();
        }

        public void SetBody(World world, aVector2 size, float density, aVector2 position)
        {
            Body = world.CreateRectangle(size.X, size.Y, density, position);
        }

        public void UpdatePhysics()
        {
            SetOriginPoint();
            SetHitbox();
        }

        private void SetOriginPoint()
        {
            IsHitboxSprite();
            OriginPoint = new mVector2(X + HitboxSprite.Width / 2, Y + HitboxSprite.Height);
        }

        private void SetHitbox()
        {
            IsHitboxSprite();
            Hitbox = new Hitbox(
                new RectangleF(X + 5f, Y, HitboxSprite.Width / 2 + 5f, 1f),
                new RectangleF(X + 5f, Y + HitboxSprite.Height, HitboxSprite.Width / 2 + 5f, 1f),
                new RectangleF(X + HitboxSprite.Width - 5f, Y, 1f, HitboxSprite.Height),
                new RectangleF(X + 5f, Y, 1f, HitboxSprite.Height));
        }

        public void SetHitbox(RectangleF top, RectangleF bottom, RectangleF right, RectangleF left)
        {
            Hitbox = new Hitbox(top, bottom, right, left);
        }

        #region Varify systems
        private void IsSprite()
        {
            if (Sprite == null)
                throw new FPException("Instance's Sprite is null.");
        }
        private void IsHitboxSprite()
        {
            if (HitboxSprite == null)
                throw new FPException("Instance's HitboxSprite is null.");
        }
        #endregion
    }
}