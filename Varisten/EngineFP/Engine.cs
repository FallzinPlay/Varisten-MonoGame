using Microsoft.Xna.Framework;
using System;

namespace EngineFP
{
    public class Engine : Game
    {
        public static bool HitboxMeeting(Instance inst1, Instance inst2)
        {
            if (VerticalAlign(inst1.Hitbox, inst2.Hitbox))
            {
                if (inst1.Hitbox.Right.X >= inst2.Hitbox.Left.X &&
                    inst1.Hitbox.Right.X < inst2.Hitbox.Right.X)
                    return true;
                if (inst1.Hitbox.Left.X <= inst2.Hitbox.Right.X &&
                    inst1.Hitbox.Left.X > inst2.Hitbox.Left.X)
                    return true;
            }
            if (HorizontalAlign(inst1.Hitbox, inst2.Hitbox))
            {
                if (inst1.Hitbox.Bottom.Y >= inst2.Hitbox.Top.Y &&
                    inst1.Hitbox.Bottom.Y < inst2.Hitbox.Bottom.Y)
                    return true;
                if (inst1.Hitbox.Top.Y <= inst2.Hitbox.Bottom.Y &&
                    inst1.Hitbox.Top.Y > inst2.Hitbox.Top.Y)
                    return true;
            }
            return false;
        }

        public static string HitboxMeeting(Hitbox hb1, Hitbox hb2)
        {
            if (VerticalAlign(hb1, hb2))
            {
                if (hb1.Right.X >= hb2.Left.X &&
                    hb1.Right.X < hb2.Right.X)
                    return "Right";
                if (hb1.Left.X <= hb2.Right.X &&
                    hb1.Left.X > hb2.Left.X)
                    return "Left";
            }
            if (HorizontalAlign(hb1, hb2))
            {
                if (hb1.Bottom.Y >= hb2.Top.Y &&
                    hb1.Bottom.Y < hb2.Bottom.Y)
                    return "Bottom";
                if (hb1.Top.Y <= hb2.Bottom.Y &&
                    hb1.Top.Y > hb2.Top.Y)
                    return "Top";
            }
            return null;
        }

        public static bool VerticalAlign(Hitbox hb1, Hitbox hb2)
        {
            if (hb1.Bottom.Y >= hb2.Top.Y + 5 &&
            hb1.Top.Y <= hb2.Bottom.Y - 5)
                return true;

            return false;
        }

        public static bool HorizontalAlign(Hitbox hb1, Hitbox hb2)
        {
            if (hb1.Right.X >= hb2.Left.X + 5 &&
            hb1.Left.X <= hb2.Right.X - 5)
                return true;

            return false;
        }
    }
}
