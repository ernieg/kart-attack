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
    class Rocket : Projectile
    {
        public Rocket(Vector2 position, Color source, float damage, Kart k, float angle = 0)
          : base(position, source, damage, 32, 16, k, angle, 1, Utilities.blasts, 350, 10, 46, 46)
        {
            AnimationTextures = Utilities.rockets;
            PlayFireSound();
        }

        public override void Update(float timeDelta)
        {
            base.Move(0.5f * Velocity * timeDelta);
        }

        public void PlayFireSound()
        {
            Utilities.RocketSound.Play();
        }
    }
}
