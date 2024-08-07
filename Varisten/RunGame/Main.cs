using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Varisten.Objects.Characters;
using EngineFP;
using aVector2 = nkast.Aether.Physics2D.Common.Vector2;
using mVector2 = Microsoft.Xna.Framework.Vector2;
using System.Collections.Generic;
using EngineFP;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;

// Para publicar:
// dotnet publish -c Release -r win-x64 /p:PublishReadyToRun=false /p:TieredCompilation=false --self-contained
namespace Varisten.RunGame
{
    public class Main : Engine
    {
        #region My Game
        /*
        // Keyboard
        int keyRight;
        int keyLeft;
        int keyUp;
        int keyDown;

        // Physics
        List<Instance> collisors;
        private float gravity;
        float _vspd = 0;

        Player player;

        Color colorRight;
        Color colorLeft;
        Color colorTop;
        Color colorBottom;

        Texture2D spriteTest;
        SpriteFont font;
        Texture2D wallSprite;

        string exception;
        /*/
        #endregion

        #region Aether test
        private PhysicsWorld _physicsWorld;
        private Body _playerBody;
        private Body _platformBody;
        private Texture2D _playerTexture;
        private Texture2D _platformTexture;
        private float _moveSpeed = 10000f;
        private float _jumpImpulse = 1.4f;
        private int _dir;
        private float _hspd;
        private float _vspd;
        private float _gravity;
        private bool _canJump;
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
            /*
            #region Screen
            _graphics.IsFullScreen = false;
            #endregion

            #region Camera
            _cam = new Camera(GraphicsDevice.Viewport);
            #endregion

            #region Physics
            collisors = new List<Instance>();
            gravity = 2f;
            _vspd = 0;
            #endregion
            */
            #endregion

            #region Aether test
            _physicsWorld = new PhysicsWorld();
            _playerBody = _physicsWorld.CreateBody(new aVector2(100, 100), new aVector2(50, 50), 1f);
            _platformBody = _physicsWorld.CreateBody(new aVector2(100, 200), new aVector2(300, 20), 1f, true); // Plataforma mais longa
            _dir = 0;
            _hspd = 0;
            _vspd = 0;
            _gravity = 100f;
            base.Initialize();
            #endregion
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp; // Faz com que o sprite seja desenhado limpo

            #region My game
            /*
            font = Content.Load<SpriteFont>(@"Fonts\File");
            spriteTest = Content.Load<Texture2D>(@"Sprites\Halek");
            wallSprite = Content.Load<Texture2D>(@"Sprites\Hitbox\Wall");

            player = new Player(spriteTest, "Fallzin");
            player.Sprite = spriteTest;

            // Invisible walls
            // Create 10 walls
            for (int i = 0; i < 10; i++)
            {
                Instance _inst = new Collisor(wallSprite);
                _inst.X = (wallSprite.Width - 100f * i) + 5f;
                _inst.Y = 700f;
                collisors.Add(_inst);
            }
            /*/
            #endregion

            #region Aether test
            // Criar texturas básicas para o player e a plataforma
            _playerTexture = new Texture2D(GraphicsDevice, 50, 50);
            Color[] playerData = new Color[50 * 50];
            for (int i = 0; i < playerData.Length; ++i) playerData[i] = Color.White;
            _playerTexture.SetData(playerData);

            _platformTexture = new Texture2D(GraphicsDevice, 300, 20); // Plataforma mais longa
            Color[] platformData = new Color[300 * 20];
            for (int i = 0; i < platformData.Length; ++i) platformData[i] = Color.Gray;
            _platformTexture.SetData(platformData);
            #endregion
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            #region My game
            /*
            try
            {

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
                    inst.Position = new mVector2(inst.X, inst.Y);
                }

                #region Player
                // Update my origin point of the player and player's hitbox
                player.UpdatePhysics();

                // ---------- Player movement
                player.RightSpeed = player.Speed * dt * keyRight;
                player.LeftSpeed = player.Speed * dt * keyLeft;
                player.UpSpeed = player.Speed * dt * keyUp;
                player.DownSpeed = player.Speed * dt * keyDown;
                _vspd += gravity;

                // ---- Collision
                // Change the player's hitbox color if he's colliding and stop the player if he's colliding
                colorRight = Color.Red;
                colorLeft = Color.Red;
                colorTop = Color.Red;
                colorBottom = Color.Red;

                bool isOnGround = false;

                foreach (Instance obj in collisors)
                {
                    // Colisão Horizontal
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

                    // Colisão Vertical
                    if (VerticalMeeting(player, obj) < 0)
                    {
                        colorTop = Color.White;
                        _vspd = 0;
                    }
                    if (VerticalMeeting(player, obj) > 0)
                    {
                        colorBottom = Color.White;
                        _vspd = 0;
                        isOnGround = true;
                        player.Y = obj.Hitbox.Top.Y - player.HitboxSprite.Height;
                    }
                }

                // Jump
                if (isOnGround && keyUp != 0)
                {
                    _vspd -= player.Jump;
                }

                player.X += player.RightSpeed - player.LeftSpeed;
                player.Y += _vspd;
                player.Position = new mVector2(player.X, player.Y);
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
            /*/
            #endregion

            #region Aether test
            KeyboardState state = Keyboard.GetState();

            // Movimentação horizontal
            aVector2 velocity = _playerBody.Position;

            _dir = 0;
            if (state.IsKeyDown(Keys.A)) _dir = -1;
            else if (state.IsKeyDown(Keys.D)) _dir = 1;
            _hspd = velocity.X = _moveSpeed * dt * _dir;

            // Pulo
            if (_canJump)
            {
                _vspd = 0;
                if (state.IsKeyDown(Keys.Space))
                {
                    _vspd -= _jumpImpulse * _moveSpeed * dt;
                    _canJump = false;
                }
            }
            else
            {
                _vspd += _gravity * dt;
            }

            velocity.X += _hspd;
            velocity.Y += _vspd;
            _playerBody.LinearVelocity = new aVector2(velocity.X, velocity.Y);

            // Step physics simulation
            _physicsWorld.Step(dt);

            // Verifica a colisão entre o player e a plataforma
            _canJump = false;
            ContactEdge contactEdge = _playerBody.ContactList;
            while (contactEdge != null)
            {
                if (contactEdge.Contact.IsTouching)
                {
                    _canJump = true;
                    break;
                }
                contactEdge = contactEdge.Next;
            }
            #endregion
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            #region My Game
            /*
            _spriteBatch.Begin();

            // Text
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
            */
            #endregion

            #region Aether test
            _spriteBatch.Begin();

            _spriteBatch.DrawString(Content.Load<SpriteFont>(@"Fonts\File"), $"vspd: {_vspd}", new mVector2(10f, 10f), Color.Blue);

            // Convertendo de Aether Vector2 para XNA Vector2
            mVector2 playerPosition = ConvertToXNAVector(_playerBody.Position);
            mVector2 platformPosition = ConvertToXNAVector(_platformBody.Position);

            // Ajustando a posição para o centro das texturas
            playerPosition -= new mVector2(_playerTexture.Width / 2, _playerTexture.Height / 2);
            platformPosition -= new mVector2(_platformTexture.Width / 2, _platformTexture.Height / 2);

            _spriteBatch.Draw(_playerTexture, playerPosition, Color.White);
            _spriteBatch.Draw(_platformTexture, platformPosition, Color.Gray);

            _spriteBatch.End();
            #endregion
            base.Draw(gameTime);
        }

        private mVector2 ConvertToXNAVector(nkast.Aether.Physics2D.Common.Vector2 aetherVector)
        {
            return new mVector2(aetherVector.X, aetherVector.Y);
        }
    }
}
