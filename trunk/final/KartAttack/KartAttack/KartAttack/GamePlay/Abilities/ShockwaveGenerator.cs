using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KartAttack
{
    class ShockwaveGenerator : Ability
    {
        public ShockwaveGenerator(float energyPenalty = 40.0f, float speedPenalty = 2.0f)
          : base(energyPenalty, speedPenalty)
        {

        }

        public override List<Projectile> Activate(Kart kart, int milliseconds_passed)
        {
            if (kart.ActivateSide(SideAttached, milliseconds_passed, 0))
            {
                float baseAngle = 0.0f;
                float fireCone = 2.0f * (float)Math.PI;
                int numProjectiles = 16;
                
                List<Projectile> return_list = new List<Projectile>();

                for (float a = 0.0f; a <= 2.0f * (float)Math.PI; a += fireCone / (float)numProjectiles)
                {
                    return_list.Add(new Shockwave(kart.ProjectileSpawnPositions[(int)SideAttached], kart.Team, 0.0f, kart, a));
                }

                kart.Energy -= EnergyPenalty;

                // play the firing sound
                Utilities.ShockwaveSound.Play();

                return return_list;
            }
            else
            {
                return null;
            }
        }

        public override void Deactivate()
        {
            throw new NotImplementedException();
        }

        public override string GetAssetName()
        {
            return "Abilities/shockwave_generator_1";
        }

    }
}
