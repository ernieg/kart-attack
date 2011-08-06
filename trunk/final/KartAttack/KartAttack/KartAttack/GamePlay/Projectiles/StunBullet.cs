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
    class StunBullet : Projectile
    {
        public StunBullet(Vector2 position, Color source, float damage, Kart k, float angle = 0)
          : base(position, source, damage, 16, 16, k, angle, 1, Utilities.stuns, 200, 5, 32, 32)
        {
            AnimationTextures = Utilities.stunBullets; 
            PlayFireSound();      
        }

        public override void Update(float timeDelta)
        {
            base.Move(0.75f * Velocity * timeDelta);
        }

        public void PlayFireSound()
        {
            Utilities.StunSound.Play();
        }

    }
}
