using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KartAttack
{
    class Shield : Ability
    {
        public Shield(float energyPenalty = 1.0f, float speedPenalty = 3.0f)
          : base(energyPenalty, speedPenalty)
        {

        }

        public override List<Projectile> Activate(Kart kart, int milliseconds_passed)
        {
            if (kart.ActivateFullAutoSide(SideAttached, milliseconds_passed, 0))
            {
                kart.shields_active = true;
            }
            else
            {
                kart.shields_active = false;
            }

            return null;
        }

        public override void Deactivate()
        {
            throw new NotImplementedException();
        }

        public override string GetAssetName()
        {
            return "Abilities/shield_1";
        }

    }
}
