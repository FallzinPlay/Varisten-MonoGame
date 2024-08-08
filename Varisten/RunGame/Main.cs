using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Varisten.Objects.Characters;
using EngineFP;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using System.Collections.Generic;

// Para publicar:
// dotnet publish -c Release -r win-x64 /p:PublishReadyToRun=false /p:TieredCompilation=false --self-contained
namespace Varisten.RunGame
{
    public class Main : Engine
    {
        #region My Game
        //*
        // Keyboard
        public int keyRight;
        public int keyLeft;
        public int keyUp;
        public int keyDown;

        // Physics
        List<Instance> collisors;
        private float gravity;

        Player player;

        Color color;

        Texture2D spriteTest;
        SpriteFont font;
        Texture2D wallSprite;

        bool collision;

        string exception;
        //*/
        #endregion

        private Camera _cam;
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
            #region My game
            //*
            #region Screen
            _graphics.IsFullScreen = false;
            #endregion

            #region Camera
            _cam = new Camera(GraphicsDevice.Viewport);
            #endregion

            #region Physics
            collisors = new List<Instance>();
            gravity = 2f;
            collision = false;
            #endregion

            color = Color.Black;
            //*/
            #endregion

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp; // Faz com que o sprite seja desenhado limpo

            #region My game
            //*
            font = Content.Load<SpriteFont>(@"Fonts\File");
            spriteTest = Content.Load<Texture2D>(@"Sprites\Halek");
            wallSprite = Content.Load<Texture2D>(@"Sprites\Hitbox\Wall");

            player = new Player(spriteTest, "Fallzin");
            player.Sprite = spriteTest;

            // Invisible walls
            // Create 10 walls
            for (int i = 0; i < 1; i++)
            {
                Instance _inst = new Collisor(wallSprite);
                _inst.X = (wallSprite.Width - 100f * i) + 5f;
                _inst.Y = 700f;
                collisors.Add(_inst);
            }
            //*/
            #endregion

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            #region My game
            //*
            try
            {
                collision = false;
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

                // Objects
                foreach (Instance inst in collisors)
                {
                    inst.UpdatePhysics();
                    inst.Position = new Vector2(inst.X, inst.Y);
                }

                #region Player
                // Update my origin point of the player and player's hitbox
                player.UpdatePhysics();

                // ---------- Player movement
                player.RightSpeed = player.Speed * dt * keyRight;
                player.LeftSpeed = player.Speed * dt * keyLeft;
                player.UpSpeed = player.Speed * dt * keyUp;
                player.DownSpeed = player.Speed * dt * keyDown;

                // ---- Collision

                foreach (Instance obj in collisors)
                {
                    // Colisão Horizontal
                    string _dir = HitboxMeeting(player.Hitbox, obj.Hitbox);
                    if (_dir == "Right")
                    {
                        collision = true;
                        player.RightSpeed = 0;
                    }
                    if (_dir == "Left")
                    {
                        collision = true;
                        player.LeftSpeed = 0;
                    }
                    if (_dir == "Bottom")
                    {
                        collision = true;
                        player.DownSpeed = 0;
                    }
                    if (_dir == "Top")
                    {
                        collision = true;
                        player.UpSpeed = 0;
                    }
                }

                /*/ Jump
                if (isOnGround && keyUp != 0)
                {
                    _vspd -= player.Jump;
                }
                //*/

                player.HorizontalSpeed = player.RightSpeed - player.LeftSpeed;
                player.VerticalSpeed = player.DownSpeed - player.UpSpeed;//gravity * dt;
                player.X += player.HorizontalSpeed;
                player.Y += player.VerticalSpeed;
                player.Position = new Vector2(player.X, player.Y);
                // ----------------------------------
                #endregion

                #region Camera
                _cam.Follow(player.Position);
                #endregion
            }
            catch (FPException ex)
            {
                exception = $"Error: {ex.Message}";
            }
            finally { }
            //*/
            #endregion

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            #region My Game
            //*
            _spriteBatch.Begin();

            // Text
            _spriteBatch.DrawString(font, collision.ToString(), new Vector2(10f, 10f), Color.White);
            _spriteBatch.End();

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _cam.Transform);

            #region Player
            // Draw the player
            _spriteBatch.Draw(player.Sprite, player.Position, Color.White);

            // Player's hitbox
            _spriteBatch.DrawPoint(player.Hitbox.Top, color, 5f);
            _spriteBatch.DrawPoint(player.Hitbox.Bottom, color, 5f);
            _spriteBatch.DrawPoint(player.Hitbox.Right, color, 5f);
            _spriteBatch.DrawPoint(player.Hitbox.Left, color, 5f);
            #endregion

            #region Invisible walls
            foreach (Instance wall in collisors)
            {
                _spriteBatch.Draw(wall.HitboxSprite, wall.Position, Color.White);
                _spriteBatch.DrawPoint(wall.Hitbox.Top, color, 5f);
                _spriteBatch.DrawPoint(wall.Hitbox.Bottom, color, 5f);
                _spriteBatch.DrawPoint(wall.Hitbox.Right, color, 5f);
                _spriteBatch.DrawPoint(wall.Hitbox.Left, color, 5f);
            }
            #endregion

            // Error
            if (exception != null)
                _spriteBatch.DrawString(font, exception, new Vector2(_graphics.PreferredBackBufferWidth / 3, _graphics.PreferredBackBufferHeight / 2), Color.Black);

            _spriteBatch.End();
            //*/
            #endregion

            base.Draw(gameTime);
        }
    }
}
