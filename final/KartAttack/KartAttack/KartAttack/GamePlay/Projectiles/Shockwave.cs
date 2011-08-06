﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace KartAttack
{
    class Shockwave : Projectile
    {
        public int LengthOfLife { get; private set; }
        public int TimeLiving { get; private set; }

        public Shockwave(Vector2 position, Color source, float damage, Kart k, float angle = 0, bool first_shot = false)
          : base(position, source, damage, 16, 16, k, angle)
        {
            AnimationTextures = Utilities.shockwaves;
            LengthOfLife = 140;
            TimeLiving = 0;
        }

        public override void Update(float timeDelta)
        {
            base.Move(1.5f * Velocity * timeDelta);
            TimeLiving += (int)timeDelta;
        }

        public void PlayFireSound()
        {
            return;
        }

    }
}
