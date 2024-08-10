using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using EngineFP;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Varisten.Objects.Characters
{
    public class Player : Character
    {
        public Player(Texture2D hitboxSprite, string name, Texture2D sprite) : base(hitboxSprite)
        {
            Name = name;
            Sprite = sprite;
            Speed = 200f;
            Jump = 30f;
            Life = MaxLife;
            MaxLife = 15;
            Damage = 1.50d;
            Defense = 2.10d;
            CriticDamage = 1.30d;
            DodgeChance = 2.20d;
            CriticChance = 2.15d;
            CounterattackChance = 1.25d;
            MaxBagWeight = 20;
            Coins = 5.00d;
        }

        public void Update(float dt, KeyboardState kState, List<Instance> collisors)
        {
            // Update my origin point of the player and player's hitbox
            UpdatePhysics();

            // keyboard
            RightSpeed = kState.IsKeyDown(Keys.D) ? 1 : 0;
            LeftSpeed = kState.IsKeyDown(Keys.A) ? 1 : 0;
            UpSpeed = kState.IsKeyDown(Keys.W) || kState.IsKeyDown(Keys.Space) ? 1 : 0;
            DownSpeed = kState.IsKeyDown(Keys.S) ? 1 : 0;

            // Deninindo que o player não poderá sair do mapa
            if (Position.X + HitboxSprite.Width >= 3000)
                X = 3000;
            if (Position.X <= 0)
                X = 0;
            if (Position.Y >= 2000)
                Y = 2000;
            if (Position.Y <= 0)
                Y = 0;
            
            // ---- Collision
            foreach (Instance obj in collisors)
            {
                switch (Engine.HitboxMeeting(Hitbox, obj.Hitbox))
                {
                    // Colisão Horizontal
                    case "Right":
                        RightSpeed = 0;
                        break;

                    case "Left":
                        LeftSpeed = 0;
                        break;

                    // Colisão Vertical
                    case "Bottom":
                        DownSpeed = 0;
                        break;

                    case "Top":
                        UpSpeed = 0;
                        break;

                    default:
                        break;
                }
            }

            // ---------- Player movement
            HorizontalSpeed = Speed * dt * (RightSpeed - LeftSpeed);
            VerticalSpeed = Speed * dt * (DownSpeed - UpSpeed);
            X += HorizontalSpeed;
            Y += VerticalSpeed;
            Position = new Vector2(X, Y);
        }

        public bool PlaceMeeting(float x, float y, List<Instance> objects)
        {
            // Salva as coordenadas atuais do objeto
            float originalX = X;
            float originalY = Y;

            // Move a hitbox temporariamente para a nova posição
            X = x;
            Y = y;
            UpdatePhysics();  // Atualiza a hitbox para a nova posição

            // Verifica colisão com todos os objetos na lista
            foreach (Instance obj in objects)
            {
                if (Engine.HitboxMeeting(Hitbox, obj.Hitbox) != null)
                {
                    // Restaura a posição original antes de retornar
                    X = originalX;
                    Y = originalY;
                    UpdatePhysics();  // Restaura a hitbox para a posição original
                    return true;  // Colisão detectada
                }
            }

            // Restaura a posição original antes de retornar
            X = originalX;
            Y = originalY;
            UpdatePhysics();  // Restaura a hitbox para a posição original

            return false;  // Nenhuma colisão detectada
        }


        public void Draw(SpriteBatch _spriteBatch)
        {

            // Draw the player
            _spriteBatch.Draw(Sprite, Position, Color.White);

            // Player's origin point
            _spriteBatch.DrawPoint(OriginPoint.X, OriginPoint.Y, Color.Blue, 5f);

            // Player's hitbox
            _spriteBatch.DrawPoint(Hitbox.Top, Color.White, 5f);
            _spriteBatch.DrawPoint(Hitbox.Bottom, Color.White, 5f);
            _spriteBatch.DrawPoint(Hitbox.Right, Color.White, 5f);
            _spriteBatch.DrawPoint(Hitbox.Left, Color.White, 5f);
        }
    }
}
