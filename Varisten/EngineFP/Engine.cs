using Microsoft.Xna.Framework;
using System;

namespace EngineFP
{
    public class Engine : Game
    {

        public static string HitboxMeeting(Hitbox hb1, Hitbox hb2)
        {
            if (VerticalAlign(hb1, hb2))
            {
                if ((hb1.Right.X + hb1.Right.X) >= hb2.Left.X &&
                    hb1.Right.X < hb2.Right.X)
                    return "Right";
                if ((hb1.Left.X + hb1.Left.X) <= hb2.Right.X &&
                    hb1.Left.X > hb2.Left.X)
                    return "Left";
            }
            if (HorizontalMeeting(hb1, hb2))
            {
                if ((hb1.Bottom.Y + hb1.Bottom.Y) >= hb2.Top.Y &&
                    hb1.Bottom.Y < hb2.Bottom.Y)
                    return "Bottom";
                if ((hb1.Top.Y + hb1.Top.Y) <= hb2.Bottom.Y &&
                    hb1.Top.Y > hb2.Top.Y)
                    return "Left";
            }
            return null;
        }

        public static bool VerticalAlign(Hitbox hb1, Hitbox hb2)
        {
            if (hb1.Bottom.Y >= hb2.Top.Y + 3 &&
            hb1.Top.Y <= hb2.Bottom.Y - 3)
                return true;

            return false;
        }

        public static bool HorizontalMeeting(Hitbox hb1, Hitbox hb2)
        {
            if (hb1.Right.X >= hb2.Left.X + 3 &&
            hb1.Left.X <= hb2.Right.X - 3)
                return true;

            return false;
        }
    }
}
