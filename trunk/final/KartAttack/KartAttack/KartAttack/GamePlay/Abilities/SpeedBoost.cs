using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KartAttack
{
    class SpeedBoost : Ability
    {
        public SpeedBoost(float energyPenalty = 10.0f, float speedPenalty = 1.0f)
          : base(energyPenalty, speedPenalty)
        {

        }

        public override List<Projectile> Activate(Kart kart, int milliseconds_passed)
        {
            if (kart.ActivateSide(SideAttached, milliseconds_passed, 0))
            {
                Vector2 boostVelocity = new Vector2((float)Math.Sin(kart.Angle), (float)-Math.Cos(kart.Angle));
                boostVelocity.Normalize();

                // change this to tweak the boost strength
                boostVelocity *= 30.0f;

                kart.Velocity += boostVelocity;

                // debug
                //Console.WriteLine("kart angle=" + kart.Angle);
                //Console.WriteLine("boost vector x=" + boostVelocity.X);
                //Console.WriteLine("boost vector y=" + boostVelocity.Y);

                kart.Energy -= EnergyPenalty;

                Utilities.SpeedBoostSound.Play();
            }

            return null;
        }

        public override void Deactivate()
        {
            throw new NotImplementedException();
        }

        public override string GetAssetName()
        {
            return "Abilities/speed_boost_1";
        }

    }
}
