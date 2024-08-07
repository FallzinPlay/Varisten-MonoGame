using Microsoft.Xna.Framework;
using System;

namespace EngineFP
{
    public class Engine : Game
    {

        public sbyte HorizontalMeeting(Instance inst1, Instance inst2)
        {
            if (inst1.Hitbox.Bottom.Y <= inst2.Hitbox.Top.Y + 10 ||
            inst1.Hitbox.Top.Y >= inst2.Hitbox.Bottom.Y - 10)
            {
                return 0;
            }

            if (inst1.Hitbox.Right.X >= inst2.Hitbox.Left.X && inst1.Hitbox.Right.X < inst2.Hitbox.Right.X)
            {
                return 1;
            }

            if (inst1.Hitbox.Left.X <= inst2.Hitbox.Right.X && inst1.Hitbox.Left.X > inst2.Hitbox.Left.X)
            {
                return -1;
            }

            return 0;
        }

        public sbyte VerticalMeeting(Instance inst1, Instance inst2)
        {
            if (inst1.Hitbox.Right.X <= inst2.Hitbox.Left.X + 10 ||
            inst1.Hitbox.Left.X >= inst2.Hitbox.Right.X - 10)
            {
                return 0;
            }

            if (inst1.Hitbox.Top.Y <= inst2.Hitbox.Bottom.Y && inst1.Hitbox.Top.Y > inst2.Hitbox.Top.Y)
            {
                return -1;
            }

            if (inst1.Hitbox.Bottom.Y >= inst2.Hitbox.Top.Y && inst1.Hitbox.Bottom.Y < inst2.Hitbox.Bottom.Y)
            {
                return 1;
            }

            return 0;
        }
    }
}
