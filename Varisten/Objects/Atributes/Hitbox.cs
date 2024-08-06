using MonoGame.Extended;

namespace Varisten.Objects.Atributes
{
    public class Hitbox
    {
        public RectangleF Top {  get; set; }
        public RectangleF Bottom { get; set; }
        public RectangleF Left { get; set; }
        public RectangleF Right { get; set; }

        // Make a rectangle arround the inst using it's sprite (the origin point must be at the top-left)
        public Hitbox(Instance inst)
        {
            Top = new RectangleF(inst.X, inst.Y, inst.Sprite.Width, 1f);
            Bottom = new RectangleF(inst.X, inst.Y + inst.Sprite.Height, inst.Sprite.Width, 1f);
            Right = new RectangleF(inst.X + inst.Sprite.Width, inst.Y, 1f, inst.Sprite.Height);
            Left = new RectangleF(inst.X, inst.Y, 1f, inst.Sprite.Height);
        }

        // customized hitbox
        public Hitbox(RectangleF top, RectangleF bottom, RectangleF right, RectangleF left)
        {
            Top = top;
            Bottom = bottom;
            Right = right;
            Left = left;
        }
    }
}
