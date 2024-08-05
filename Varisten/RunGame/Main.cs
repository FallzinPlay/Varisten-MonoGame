using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using nkast.Aether.Physics2D.Dynamics;
using Varisten.Objects.Characters;
using mVector2 = Microsoft.Xna.Framework.Vector2;
using aVector2 = nkast.Aether.Physics2D.Common.Vector2;
using System;
using nkast.Aether.Physics2D.Dynamics.Contacts;
using Varisten.Objects;

// dotnet add "C:\Users\aprendiz.informatica\Desktop\Varisten-Official-Game-\Varisten\Varisten.csproj" package VelcroPhysics --prerelease
// dotnet add "C:\Users\vitor\OneDrive\Área de Trabalho\Varisten-Official-Game-\Varisten\Varisten.csproj" package VelcroPhysics --prerelease
// Para publicar:
// dotnet publish -c Release -r win-x64 /p:PublishReadyToRun=false /p:TieredCompilation=false --self-contained
namespace Varisten.RunGame
{
    public class Main : Engine
    {
        // Physics
        World world;

        // Keyboard
        int keyRight;
        int keyLeft;
        int keyUp;
        int keyDown;

        Player player;
        Player player2;

        Color color;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            world = new World(new aVector2(0f, 1f));

            player = new Player("Fallzin");
            player2 = new Player("Mob");

            player.SetBody(world, new aVector2(15f, 25f), 2f, new aVector2(100f, 200f));
            player2.SetBody(world, new aVector2(15f, 25f), 2f, new aVector2(250f, 250f));

            player.Body.BodyType = BodyType.Dynamic;
            player2.Body.BodyType = BodyType.Static;

            color = Color.White;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp; // Faz com que o sprite seja desenhado limpo

            player.Sprite = Content.Load<Texture2D>("Marine");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            world.Step(dt);

            // Movimentação do jogador
            var kState = Keyboard.GetState();
            int keyRight = kState.IsKeyDown(Keys.D) ? 1 : 0;
            int keyLeft = kState.IsKeyDown(Keys.A) ? 1 : 0;
            int keyUp = kState.IsKeyDown(Keys.W) ? 1 : 0;
            int keyDown = kState.IsKeyDown(Keys.S) ? 1 : 0;

            player.HorizontalSpeed = player.Speed * dt * (keyRight - keyLeft);
            player.VerticalSpeed = player.Speed * dt * (keyDown - keyUp);
            player.X += player.HorizontalSpeed;
            player.Y += player.VerticalSpeed;
            player.Position = new mVector2(player.X, player.Y);

            // Verificar se o jogador está colidindo
            if (player.IsColliding != null)
            {
                color = Color.Blue;
                player.ClearCollision(); // Limpar a bandeira de colisão após o tratamento
            }
            else
            {
                color = Color.White;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // Desenhar o jogador e o segundo jogador
            _spriteBatch.Draw(player.Sprite, player.Position, Color.White);
            _spriteBatch.Draw(player.Sprite, player2.Position, color);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return true;
        }
    }
}
