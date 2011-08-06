using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KartAttack
{
    class Shotgun : Ability
    {
        public Shotgun(float energyPenalty = 20.0f, float speedPenalty = 1.0f)
          : base(energyPenalty, speedPenalty)
        {

        }

        public override List<Projectile> Activate(Kart kart, int milliseconds_passed)
        {
            if (kart.ActivateFullAutoSide(SideAttached, milliseconds_passed, 500))
            {
                float baseAngle = kart.Angle + ShootAngleAdjust();
                float fireCone = (float)Math.PI / 3.0f;
                int numPellets = 10;
                float totalDamage = 70.0f;
                float damagePerPellet = totalDamage / (float)numPellets;

                List<Projectile> return_list = new List<Projectile>();

                // the first pellet makes the firing sound
                return_list.Add(new ShotgunPellet(kart.ProjectileSpawnPositions[(int)SideAttached], kart.Team, damagePerPellet, kart, baseAngle, true));

                for (float a = baseAngle - fireCone / 2.0f; a <= baseAngle + fireCone / 2.0f; a += fireCone / (float)numPellets)
                {
                    return_list.Add(new ShotgunPellet(kart.ProjectileSpawnPositions[(int)SideAttached], kart.Team, damagePerPellet, kart, a));
                }
                
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
            return "Weapons/shotgun_1";
        }

    }
}
