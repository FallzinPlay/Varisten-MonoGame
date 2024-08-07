using nkast.Aether.Physics2D.Dynamics;
using Vector2 = nkast.Aether.Physics2D.Common.Vector2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

public class PhysicsWorld
{
    private World _world;
    public List<Body> Bodies { get; private set; }

    public PhysicsWorld()
    {
        _world = new World(new Vector2(0, 0)); // Gravidade
        Bodies = new List<Body>();
    }

    public Body CreateBody(Vector2 position, Vector2 size, float density, bool isStatic = false)
    {
        Body body = _world.CreateRectangle(size.X, size.Y, density, position);
        body.BodyType = isStatic ? BodyType.Static : BodyType.Dynamic;
        body.SetRestitution(0.0f); // Sem quique
        body.SetFriction(0.5f); // Algum atrito
        Bodies.Add(body);
        return body;
    }

    public void Step(float dt)
    {
        _world.Step(dt);
    }
}