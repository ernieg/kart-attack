using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace KartAttack
{
    class Coin : GameObject
    {
        private float timeWaiting;
        private float timeToSpawn;
        public bool Respawn
        {
            get;
            private set;
        }

        public int worth = 1;

        public bool isSpawned;

        public Coin(Vector2 position, bool willRespawn = true, float xVel = 0.0f, float yVel = 0.0f)
            : base(position, 22, 22, number_of_frames: 7)
        {
            Respawn = willRespawn;
            Velocity = new Vector2(xVel, yVel);
            timeWaiting = 0.0f;
            timeToSpawn = 20000.0f;
            isSpawned = true;
            AnimationTextures = Utilities.coinTextures;

        }

        public override void Update(float timeDelta)
        {
            if (!Respawn)
                return;
            if (!isSpawned)
            {
                timeWaiting += timeDelta;
                if (timeWaiting > timeToSpawn)
                {
                    timeWaiting = 0;
                    isSpawned = true;
                }
            }
        }

        public void Move(float timeDelta, List<Wall> walls)
        {
            if (Velocity.Length() < 0.1f)
            {
                Velocity = Vector2.Zero;
                return;
            }
            
            Velocity *= (float)Math.Pow(0.60, timeDelta / 1000.0);  // apply the friction
            
            Vector2 v = Velocity;
            v.Y = 0.0f;
            Move(timeDelta * 0.012f * v);                       // try moving coin in X direction
            foreach (Wall w in walls)
            {
                if (base.Collide(w))
                {
                    Velocity = new Vector2(-Velocity.X, Velocity.Y);
                    Move(timeDelta * 0.012f * -v); // move the car back
                    break;
                }
            }

            v = Velocity;
            v.X = 0.0f;
            Move(timeDelta * 0.012f * v);                       // try moving coin in Y direction
            foreach (Wall w in walls)
            {
                if (base.Collide(w))
                {
                    Velocity = new Vector2(Velocity.X, -Velocity.Y);
                    Move(timeDelta * 0.012f * -v); // move the car back
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, int millisElapsed)
        {
            if (isSpawned || !Respawn)
            {
                if (worth == 5)
                    AnimationTextures = Utilities.blueCoinTextures;
                else
                    AnimationTextures = Utilities.coinTextures;
                base.Draw(spriteBatch, millisElapsed);
            }
        }

        public override bool Collide(GameObject o)
        {
            return false;
        }

        public void PlayCollideSound()
        {
            Utilities.CoinSound.Play();
        }
    }
}
