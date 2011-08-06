using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KartAttack
{
    class Minigun : Ability
    {
        public Minigun(float energyPenalty = 2.0f, float speedPenalty = 1.0f)
          : base(energyPenalty, speedPenalty)
        {

        }

        public override List<Projectile> Activate(Kart kart, int milliseconds_passed)
        {
            if (kart.ActivateFullAutoSide(SideAttached, milliseconds_passed, 100))
            {
                List<Projectile> return_list = new List<Projectile>();

                double perp;
                if ((int)SideAttached == 1)
                    perp = kart.Angle - Math.PI/2.0;
                else
                     perp = kart.Angle + Math.PI/2.0;
                
                // new
                return_list.Add(new Bullet(kart.Bounds.Center - new Vector2(2,2) + 4 * new Vector2((float)Math.Sin(perp), (float)-Math.Cos(perp)), kart.Team, 11.0f, kart, kart.Angle + ShootAngleAdjust(), 1.25f));
                
                // original
                //return_list.Add(new Bullet(kart.ProjectileSpawnPositions[(int)SideAttached], kart.Team, 10.0f, kart, kart.Angle + ShootAngleAdjust()));

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
            return "Weapons/machine_gun_1";
        }

    }
}
