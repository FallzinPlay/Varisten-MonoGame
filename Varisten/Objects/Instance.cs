using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Dynamics;
using mVector2 = Microsoft.Xna.Framework.Vector2;
using aVector2 = nkast.Aether.Physics2D.Common.Vector2;
using nkast.Aether.Physics2D.Dynamics.Contacts;
using System;

namespace Varisten.Objects
{
    public abstract class Instance
    {
        public mVector2 Position
        {
            get => new mVector2(Body.Position.X, Body.Position.Y); // Use a posição do corpo
            set => Body.Position = new aVector2(value.X, value.Y); // Ajusta a posição do corpo
        }
        public float X { get => Position.X; set => Position = new mVector2(value, Position.Y); }
        public float Y { get => Position.Y; set => Position = new mVector2(Position.X, value); }
        public Texture2D Sprite { get; set; }
        public Body Body { get; private set; }
        public Rectangle Hitbox { get; set; }
        public Instance IsColliding { get; set; }

        public void SetBody(World world, aVector2 size, float density, aVector2 position)
        {
            Body = world.CreateRectangle(size.X, size.Y, density, position);
            Body.Tag = this;
            Body.OnCollision += OnCollision;
        }

        private bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            Instance inst1 = fixtureA.Body.Tag as Instance;
            Instance inst2 = fixtureB.Body.Tag as Instance;

            if (inst1 != null && inst2 != null)
            {
                inst1.IsColliding = inst2;
                Console.WriteLine($"Colisão detectada! Posição do bodyA: {inst1.Position}, Posição do bodyB: {inst2.Position}");
            }

            return true;
        }

        public void ClearCollision()
        {
            IsColliding = null;
        }
    }
}
