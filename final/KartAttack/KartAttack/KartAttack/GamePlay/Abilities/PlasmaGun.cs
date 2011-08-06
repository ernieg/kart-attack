using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KartAttack
{
    class PlasmaGun : Ability
    {
        public PlasmaGun(float energyPenalty = 7.0f, float speedPenalty = 2.0f)
          : base(energyPenalty, speedPenalty)
        {

        }

        public override List<Projectile> Activate(Kart kart, int milliseconds_passed)
        {
            if (kart.ActivateFullAutoSide(SideAttached, milliseconds_passed, 200))
            {
                List<Projectile> return_list = new List<Projectile>();

                return_list.Add(new Plasma(kart.ProjectileSpawnPositions[(int)SideAttached] - new Vector2(8.0f, 8.0f), kart.Team, 18.0f, kart, kart.Angle + ShootAngleAdjust()));

                kart.Energy -= EnergyPenalty;

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
            return "Weapons/plasma_gun_1";
        }

    }
}
