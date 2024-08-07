using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using nkast.Aether.Physics2D.Dynamics;
using Varisten.Objects.Characters;
using EngineFP;
using aVector2 = nkast.Aether.Physics2D.Common.Vector2;
using mVector2 = Microsoft.Xna.Framework.Vector2;

// Para publicar:
// dotnet publish -c Release -r win-x64 /p:PublishReadyToRun=false /p:TieredCompilation=false --self-contained
namespace Varisten.RunGame
{
    public class Main : Engine
    {

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

        Texture2D spriteTest;
        SpriteFont font;

        private Camera _cam;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        string exception;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            #region Screen
            _graphics.IsFullScreen = false;
            #endregion

            #region Camera
            _cam = new Camera(GraphicsDevice.Viewport);
            #endregion
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp; // Faz com que o sprite seja desenhado limpo

            font = Content.Load<SpriteFont>(@"Fonts\File");
            spriteTest = Content.Load<Texture2D>(@"Sprites\Halek");

            player = new Player(spriteTest, "Fallzin");
            player2 = new Player(spriteTest, "Mob");

            player.Sprite = spriteTest;
            player2.Sprite = spriteTest;

            player2.X = 200f;
            player2.Y = 200f;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            try
            {
                #region Keyboard
                var kState = Keyboard.GetState();
                keyRight = kState.IsKeyDown(Keys.D) ? 1 : 0;
                keyLeft = kState.IsKeyDown(Keys.A) ? 1 : 0;
                keyUp = kState.IsKeyDown(Keys.W) ? 1 : 0;
                keyDown = kState.IsKeyDown(Keys.S) ? 1 : 0;
                #endregion

                #region Screen
                // Full screen
                if (kState.IsKeyDown(Keys.F11)) _graphics.IsFullScreen = !_graphics.IsFullScreen;

                // Resolution
                _graphics.PreferredBackBufferWidth = 1280;
                _graphics.PreferredBackBufferHeight = 720;
                _graphics.ApplyChanges();
                #endregion

                #region Player
                // Update my origin point of the player and player's hitbox
                player.UpdatePhysics();

                // ---------- Player movement
                player.RightSpeed = player.Speed * dt * keyRight;
                player.LeftSpeed = player.Speed * dt * keyLeft;
                player.UpSpeed = player.Speed * dt * keyUp;
                player.DownSpeed = player.Speed * dt * keyDown;

                // ---- Collision
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
                // ----------------------------------
                #endregion

                #region Camera
                _cam.Follow(player.Position);
                #endregion


                player2.UpdatePhysics();
                player2.Position = new mVector2(player2.X, player2.Y);
            }
            catch (FPException ex)
            {
                exception = $"Error: {ex.Message}";
            }
            finally { }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _cam.Transform);

            #region Player
            // Draw the player
            _spriteBatch.Draw(player.Sprite, player.Position, Color.White);
            // Draw my origin point of the player
            _spriteBatch.DrawPoint(player.OriginPoint.X, player.OriginPoint.Y, Color.Blue, 5f);

            // Player's hitbox
            _spriteBatch.DrawRectangle(player.Hitbox.Top, colorTop);
            _spriteBatch.DrawRectangle(player.Hitbox.Bottom, colorBottom);
            _spriteBatch.DrawRectangle(player.Hitbox.Right, colorRight);
            _spriteBatch.DrawRectangle(player.Hitbox.Left, colorLeft);
            #endregion

            #region Object to tests
            // Draw the object
            _spriteBatch.Draw(player2.Sprite, player2.Position, Color.Blue);

            // object's hitbox
            _spriteBatch.DrawRectangle(player2.Hitbox.Top, Color.Blue);
            _spriteBatch.DrawRectangle(player2.Hitbox.Bottom, Color.Blue);
            _spriteBatch.DrawRectangle(player2.Hitbox.Right, Color.Blue);
            _spriteBatch.DrawRectangle(player2.Hitbox.Left, Color.Blue);
            #endregion

            // Error
            if (exception != null)
                _spriteBatch.DrawString(font, exception, new mVector2(_graphics.PreferredBackBufferWidth / 3, _graphics.PreferredBackBufferHeight / 2), Color.Black);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
