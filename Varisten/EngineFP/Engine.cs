using Microsoft.Xna.Framework;

namespace EngineFP
{
    public class Engine : Game
    {
        public sbyte HorizontalMeeting(Instance inst1, Instance inst2)
        {
            sbyte _dir = 0;
            // Verify if the inst1's hortizontal position is not taller or smaller than inst2's horizontal position
            if (inst1.Hitbox.Bottom.Y > inst2.Hitbox.Top.Y + 1
                    && inst1.Hitbox.Top.Y < inst2.Hitbox.Bottom.Y - 1)
            {
                // collision on the right
                if (inst1.Hitbox.Right.X >= inst2.Hitbox.Left.X
                    && inst1.Hitbox.Right.X < inst2.Hitbox.Right.X)
                    _dir = 1;
                // collision on the left
                if (inst1.Hitbox.Left.X <= inst2.Hitbox.Right.X
                    && inst1.Hitbox.Left.X > inst2.Hitbox.Left.X)
                    _dir = -1;
            }
            return _dir;
        }

        public sbyte VerticalMeeting(Instance inst1, Instance inst2)
        {
            sbyte _dir = 0;
            // Verify if the inst1's vertical position is not taller or smaller than inst2's vertical position
            if (inst1.Hitbox.Right.X > inst2.Hitbox.Left.X + 1
                    && inst1.Hitbox.Left.X < inst2.Hitbox.Right.X - 1)
            {
                // collision on the top
                if (inst1.Hitbox.Top.Y <= inst2.Hitbox.Bottom.Y
                    && inst1.Hitbox.Top.Y > inst2.Hitbox.Top.Y)
                    _dir = -1;
                // collision on the bottom
                if (inst1.Hitbox.Bottom.Y >= inst2.Hitbox.Top.Y
                    && inst1.Hitbox.Bottom.Y < inst2.Hitbox.Bottom.Y)
                    _dir = 1;
            }
            return _dir;
        }
    }
}
