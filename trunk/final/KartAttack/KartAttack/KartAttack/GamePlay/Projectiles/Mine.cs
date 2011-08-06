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
    class Mine : Projectile
    {
        public int LengthOfLife { get; private set; }
        public int TimeLiving { get; private set; }

        public Mine(Vector2 position, Color source, float damage, Kart k, float angle = 0, bool first_shot = false)
          : base(position, source, damage, 16, 16, k, angle, 4, Utilities.blasts, 350, 10, 46, 46)
        {
            if (source == Color.Blue)
            {
                AnimationTextures = Utilities.blueMines;
            }
            else if (source == Color.Orange)
            {
                AnimationTextures = Utilities.orangeMines;
            }
            else if (source == Color.Green)
            {
                AnimationTextures = Utilities.greenMines;
            }
            else if (source == Color.Turquoise)
            {
                AnimationTextures = Utilities.turqoiseMines;
            }
            LengthOfLife = 10000;
            TimeLiving = 0;
        }

        public override void Update(float timeDelta)
        {
            base.Move(0.0f * Velocity * timeDelta);
            TimeLiving += (int)timeDelta;
        }

        public void PlayFireSound()
        {
            return;
        }

    }
}
