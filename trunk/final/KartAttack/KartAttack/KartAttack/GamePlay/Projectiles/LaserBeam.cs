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
    class LaserBeam : Projectile
    {
        public LaserBeam(Vector2 position, Color source, float damage, Kart k, float angle = 0)
          : base(position, source, damage, 32, 8, k, angle)
        {
            AnimationTextures = Utilities.laserTextures;

            // need to play the firing sound
            Utilities.LaserFireSound.Play();
        }

        public override void Update(float timeDelta)
        {
            base.Move(3.0f * Velocity * timeDelta);
        }

        public void PlayFireSound()
        {
            
        }

    }
}
