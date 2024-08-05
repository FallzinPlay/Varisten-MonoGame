using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Varisten.Objects;
using Varisten.Objects.Characters;

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
            player = new Player("Fallzin");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp; // Faz com que o sprite seja desenhado limpo

            player.CurrentSprite = Content.Load<Texture2D>("Marine");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Keyboard
            keyRight = 0;
            keyLeft = 0;
            keyUp = 0;
            keyDown = 0;
            var kState = Keyboard.GetState();
            if (kState.IsKeyDown(Keys.D))
                keyRight = 1;
            if (kState.IsKeyDown(Keys.A))
                keyLeft = 1;
            if (kState.IsKeyDown(Keys.W))
                keyUp = 1;
            if (kState.IsKeyDown(Keys.S))
                keyDown = 1;

            #region Player
            
            player.HorizontalSpeed = player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds * (keyRight - keyLeft);
            player.X += player.HorizontalSpeed;
            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp); // Inicia o desenho com a configuração de Pixel perfect
            _spriteBatch.Draw(player.CurrentSprite, new Vector2(player.X, player.Y), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
