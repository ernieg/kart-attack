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
    class ShotgunPellet : Projectile
    {
        public ShotgunPellet(Vector2 position, Color source, float damage, Kart k, float angle = 0, bool first_shot = false)
          : base(position, source, damage, 4, 4, k, angle)
        {
            AnimationTextures = Utilities.shotgunPellets;
            if (first_shot)
                PlayFireSound();
        }

        public override void Update(float timeDelta)
        {
            base.Move(1.0f * Velocity * timeDelta);
        }

        public void PlayFireSound()
        {
            Utilities.ShotgunPelletSound.Play();
        }

    }
}
