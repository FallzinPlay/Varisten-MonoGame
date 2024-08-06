using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using nkast.Aether.Physics2D.Dynamics;
using Varisten.Objects.Characters;
using aVector2 = nkast.Aether.Physics2D.Common.Vector2;
using mVector2 = Microsoft.Xna.Framework.Vector2;

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

        Color colorRight;
        Color colorLeft;
        Color colorTop;
        Color colorBottom;
        SpriteFont font;

        RectangleF r2;

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
            world = new World(new aVector2(0f, 0f));

            player = new Player("Fallzin");
            player2 = new Player("Mob");

            player.SetBody(world, new aVector2(15f, 25f), 2f, new aVector2(100f, 200f));
            player2.SetBody(world, new aVector2(15f, 25f), 2f, new aVector2(250f, 250f));

            player.Body.BodyType = BodyType.Dynamic;
            player2.Body.BodyType = BodyType.Static;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp; // Faz com que o sprite seja desenhado limpo

            font = Content.Load<SpriteFont>("File");
            player.Sprite = Content.Load<Texture2D>("Marine");
            player2.Sprite = player.Sprite;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            world.Step(dt);

            // Movimentação do jogador
            var kState = Keyboard.GetState();
            keyRight = kState.IsKeyDown(Keys.D) ? 1 : 0;
            keyLeft = kState.IsKeyDown(Keys.A) ? 1 : 0;
            keyUp = kState.IsKeyDown(Keys.W) ? 1 : 0;
            keyDown = kState.IsKeyDown(Keys.S) ? 1 : 0;

            #region Player
            player.RightSpeed = player.Speed * dt * keyRight;
            player.LeftSpeed = player.Speed * dt * keyLeft;
            player.UpSpeed = player.Speed * dt * keyUp;
            player.DownSpeed = player.Speed * dt * keyDown;

            player.SetOriginPoint(player.Sprite);
            player2.SetOriginPoint(player.Sprite);

            // Change the player's hitbox color if he's colliding and stop the player if he's colliding
            colorRight = Color.Red;
            colorLeft = Color.Red;
            colorTop = Color.Red;
            colorBottom = Color.Red;
            if (HorizontalMeeting(player, player2) < 0)
            {
                colorLeft = Color.White;
                player.LeftSpeed = 0;
            }
            if (HorizontalMeeting(player, player2) > 0)
            {
                colorRight = Color.White;
                player.RightSpeed = 0;
            }
            if (VerticalMeeting(player, player2) < 0)
            {
                colorTop = Color.White;
                player.UpSpeed = 0;
            }
            if (VerticalMeeting(player, player2) > 0)
            {
                colorBottom = Color.White;
                player.DownSpeed = 0;
            }

            player.X += player.RightSpeed - player.LeftSpeed;
            player.Y += player.DownSpeed - player.UpSpeed;
            player.Position = new mVector2(player.X, player.Y);
            #endregion
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // Desenhar o jogador e o segundo jogador
            _spriteBatch.Draw(player.Sprite, player.Position, Color.White);
            _spriteBatch.Draw(player.Sprite, player2.Position, Color.Blue);
            _spriteBatch.DrawPoint(player.OriginPoint.X, player.OriginPoint.Y, Color.Blue, 5f);
            _spriteBatch.DrawRectangle(r2, Color.Red);

            // Player's hitbox
            _spriteBatch.DrawRectangle(player.Hitbox.Top, colorTop);
            _spriteBatch.DrawRectangle(player.Hitbox.Bottom, colorBottom);
            _spriteBatch.DrawRectangle(player.Hitbox.Right, colorRight);
            _spriteBatch.DrawRectangle(player.Hitbox.Left, colorLeft);

            // object's hitbox
            _spriteBatch.DrawRectangle(player2.Hitbox.Top, Color.Blue);
            _spriteBatch.DrawRectangle(player2.Hitbox.Bottom, Color.Blue);
            _spriteBatch.DrawRectangle(player2.Hitbox.Right, Color.Blue);
            _spriteBatch.DrawRectangle(player2.Hitbox.Left, Color.Blue);

            // Text
            _spriteBatch.DrawString(font, $"Player hitbox X: {player.Hitbox.Right.X}, Player2 hitbox X: {player2.Hitbox.Right.X}", new mVector2(10f, 10f), Color.Black);
            _spriteBatch.DrawString(font, $"Player hitbox X > Player2 hitbox X: {player.Hitbox.Right.X >= player2.Hitbox.Left.X}", new mVector2(10f, 20f), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
