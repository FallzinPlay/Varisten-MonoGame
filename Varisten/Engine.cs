using Microsoft.Xna.Framework;
using System.Linq;
using Varisten.Objects;

namespace Varisten
{
    public class Engine : Game
    {
        public string CollisionVerify(Instance inst1, Instance inst2)
        {
            // Calcula a interseção dos retângulos
            Rectangle intersection = Rectangle.Intersect(inst1.Hitbox, inst2.Hitbox);

            // Verifica a direção da colisão
            if (inst1.Hitbox.Intersects(inst2.Hitbox))
            {
                if (intersection.Width >= intersection.Height)
                {
                    if (inst1.Hitbox.Top < inst2.Hitbox.Top)
                        return "Top";
                    else
                        return "Bottom";
                }
                else
                {
                    if (inst1.Hitbox.Left < inst2.Hitbox.Left)
                        return "Left";
                    else
                        return "Right";
                }
            }
            return null;
        }
    }
}
