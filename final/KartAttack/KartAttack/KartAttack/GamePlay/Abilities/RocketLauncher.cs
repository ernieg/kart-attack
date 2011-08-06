using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KartAttack
{
    class RocketLauncher : Ability
    {
        public RocketLauncher(float energyPenalty = 22.0f, float speedPenalty = 3.0f)
          : base(energyPenalty, speedPenalty)
        {

        }

        public override List<Projectile> Activate(Kart kart, int milliseconds_passed)
        {
            if (kart.ActivateFullAutoSide(SideAttached, milliseconds_passed, 500))
            {
                List<Projectile> return_list = new List<Projectile>();

                //return_list.Add(new Rocket(kart.ProjectileSpawnPositions[(int)SideAttached], kart.Team, 60.0f, kart, kart.Angle + ShootAngleAdjust()));
                return_list.Add(new Rocket(kart.Bounds.Center - new Vector2(16, 8) + 22 * new Vector2((float)Math.Sin(kart.Angle), -(float)Math.Cos(kart.Angle)), kart.Team, 52.0f, kart, kart.Angle + ShootAngleAdjust()));

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
            return "Weapons/rocket_launcher_1";
        }

    }
}
