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
  class Bullet : Projectile
  {
    private float speedMultiplier;

    public Bullet(Vector2 position, Color source, float damage, Kart k, float angle = 0, float speed_mult = 0.85f)
      : base(position, source, damage, 5, 5, k, angle)
    {
        AnimationTextures = Utilities.bullets; 
        PlayFireSound();

        speedMultiplier = speed_mult;
    }

    public override void Update(float timeDelta)
    {
        base.Move(speedMultiplier * Velocity * timeDelta);
    }

    public void PlayFireSound()
    {
        Utilities.BulletSound.Play();
    }
  }
}
