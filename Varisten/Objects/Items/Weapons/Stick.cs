using Microsoft.Xna.Framework.Graphics;

namespace Varisten.Objects.Items.Weapons
{
   public class Stick : Weapon
    {
        public Stick()
        {
            Name = "Stick";
            Weight = 1.50d;
            Price = 2.52d;
            Damage = 1.25d;
            NecessaryLevel = 1;
        }
    }
}
