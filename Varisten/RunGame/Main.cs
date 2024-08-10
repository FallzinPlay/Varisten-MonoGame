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

        // Physics
        List<Instance> collisors;
        private float gravity;

        Player player;

        Color color;

        Texture2D spriteTest;
        SpriteFont font;
        Texture2D wallSprite;

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

            player = new Player(spriteTest, "Fallzin", spriteTest);

            // Invisible walls
            // Create 10 walls
            for (int i = 0; i < 1; i++)
            {
                Instance _inst = new Collisor(wallSprite);
                _inst.Position = new Vector2((wallSprite.Width - 100f * i) + 5f, 700f);
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
                #region Keyboard
                var kState = Keyboard.GetState();
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
                    inst.Position = new Vector2(inst.Position.X, inst.Position.Y);
                }
                player.Update(dt, kState, collisors);

                #region Camera
                _cam.Follow(player.Position);
                _cam.ResetPosition(3000, 2000);
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
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // Text
            _spriteBatch.DrawString(font, player.Position.ToString(), new Vector2(10f, 10f), Color.White);

            _spriteBatch.End();

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _cam.Transform);

            player.Draw(_spriteBatch);

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