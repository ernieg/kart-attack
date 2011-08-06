using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KartAttack
{
    class HealthPack : Ability
    {
        public HealthPack(float energyPenalty = 0.0f, float speedPenalty = 2.0f)
          : base(energyPenalty, speedPenalty)
        {

        }

        public override List<Projectile> Activate(Kart kart, int milliseconds_passed)
        {
            // the actual work of increasing the kart's max health is done in the kart's constructor

            return null;
        }

        public override void Deactivate()
        {
            throw new NotImplementedException();
        }

        public override string GetAssetName()
        {
            return "Abilities/health_pack_1";
        }

    }
}
