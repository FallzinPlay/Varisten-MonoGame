using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Varisten.Objects.Characters;
using EngineFP;
using aVector2 = nkast.Aether.Physics2D.Common.Vector2;
using mVector2 = Microsoft.Xna.Framework.Vector2;
using System.Collections.Generic;

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

        // Physics
        List<Instance> collisors;
        private float gravity;

        Player player;
        Player player2;
        Collisor wall;

        Color colorRight;
        Color colorLeft;
        Color colorTop;
        Color colorBottom;

        Texture2D spriteTest;
        SpriteFont font;
        Texture2D wallSprite;

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

            #region Physics
            collisors = new List<Instance>();
            gravity = 350f;
            #endregion
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp; // Faz com que o sprite seja desenhado limpo

            font = Content.Load<SpriteFont>(@"Fonts\File");
            spriteTest = Content.Load<Texture2D>(@"Sprites\Halek");
            wallSprite = Content.Load<Texture2D>(@"Sprites\Hitbox\Wall");

            player = new Player(spriteTest, "Fallzin");
            player2 = new Player(spriteTest, "Mob");

            player.Sprite = spriteTest;
            player2.Sprite = spriteTest;

            player2.X = 200f;
            player2.Y = 100f;

            collisors.Add(player2);

            // Invisible walls
            // Create 10 walls
            for (int i = 0; i < 10; i++)
            {
                Instance _inst = new Collisor(wallSprite);
                _inst.X = (wallSprite.Width - 100f * i) + 5f;
                _inst.Y = 600f;
                collisors.Add(_inst);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            try
            {
                OnFloor = false;
                #region Keyboard
                var kState = Keyboard.GetState();
                keyRight = kState.IsKeyDown(Keys.D) ? 1 : 0;
                keyLeft = kState.IsKeyDown(Keys.A) ? 1 : 0;
                keyUp = kState.IsKeyDown(Keys.W) || kState.IsKeyDown(Keys.Space) ? 1 : 0;
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
                float _vspd = 0;

                // ---- Collision
                // Change the player's hitbox color if he's colliding and stop the player if he's colliding
                colorRight = Color.Red;
                colorLeft = Color.Red;
                colorTop = Color.Red;
                colorBottom = Color.Red;
                foreach (Instance obj in collisors)
                {
                    if (HorizontalMeeting(player, obj) < 0)
                    {
                        colorLeft = Color.White;
                        player.LeftSpeed = 0;
                    }
                    if (HorizontalMeeting(player, obj) > 0)
                    {
                        colorRight = Color.White;
                        player.RightSpeed = 0;
                    }
                    if (VerticalMeeting(player, obj) < 0)
                    {
                        colorTop = Color.White;
                        player.UpSpeed = 0;
                    }
                    if (VerticalMeeting(player, obj) > 0)
                    {
                        colorBottom = Color.White;
                        _vspd = 0;
                    }
                }
                player.X += player.RightSpeed - player.LeftSpeed;
                player.Y += _vspd;

                if (OnFloor)
                {
                    if (keyUp != 0)
                    {
                        _vspd -= player.Jump;
                    }
                }
                else
                {
                    _vspd += gravity;
                }


                player.Position = new mVector2(player.X, player.Y);
                // ----------------------------------
                #endregion

                #region Camera
                _cam.Follow(player.Position);
                #endregion

                foreach (Instance inst in collisors)
                {
                    inst.UpdatePhysics();
                    inst.Position = new mVector2(inst.X, inst.Y);
                }
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

            _spriteBatch.Begin();
            // Text
            _spriteBatch.DrawString(font, OnFloor.ToString(), new mVector2(10f, 10f), Color.Black);
            _spriteBatch.End();

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

            #region Invisible walls
            foreach (Instance wall in collisors)
            {
                _spriteBatch.Draw(wall.HitboxSprite, wall.Position, Color.White);
            }
            #endregion

            // Error
            if (exception != null)
                _spriteBatch.DrawString(font, exception, new mVector2(_graphics.PreferredBackBufferWidth / 3, _graphics.PreferredBackBufferHeight / 2), Color.Black);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
