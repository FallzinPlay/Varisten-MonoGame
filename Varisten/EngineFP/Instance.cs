using MonoGame.Extended;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace EngineFP
{
    public abstract class Instance
    {
        // Position
        public Vector2 Position {  get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        // Movement
        public float RightSpeed { get; set; }
        public float LeftSpeed { get; set; }
        public float UpSpeed { get; set; }
        public float DownSpeed { get; set; }
        public float HorizontalSpeed {  get; set; }
        public float VerticalSpeed { get; set; }
        public float Speed { get; protected set; }
        public float Jump { get; protected set; }
        // Sprite
        public Texture2D Sprite { get; set; }
        // Collision
        public Hitbox Hitbox { get; private set; }
        public Texture2D HitboxSprite { get; private set; }

        public Instance(Texture2D hitboxSprite)
        {
            HitboxSprite = hitboxSprite;
        }

        public void UpdatePhysics()
        {
            SetHitbox();
        }

        private void SetHitbox()
        {
            IsHitboxSprite();
            Hitbox = new Hitbox(this);
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
