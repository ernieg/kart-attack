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
    class Plasma : Projectile
    {
        public Plasma(Vector2 position, Color source, float damage, Kart k, float angle = 0)
          : base(position, source, damage, 16, 16, k, angle, 1, Utilities.shocks, 200, 4, 32, 32)
        {
            AnimationTextures = Utilities.plasma; 
            PlayFireSound();      
        }

        public override void Update(float timeDelta)
        {
            base.Move(0.75f * Velocity * timeDelta);
        }

        public void PlayFireSound()
        {
            Utilities.PlasmaSound.Play();
        }


    }
}
