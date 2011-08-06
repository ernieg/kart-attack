using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace KartAttack
{
    class BurstLaser : Ability
    {
        private const int CHARGE_TIME = 1000;
        private int millisCounter;
        private PlayerIndex player_index;

        public BurstLaser(float energyPenalty = 40.0f, float speedPenalty = 5.0f)
          : base(energyPenalty, speedPenalty)
        {
            millisCounter = 0;
        }

        public override List<Projectile> Activate(Kart kart, int milliseconds_passed)
        {
            if (kart.ID == 0)
                player_index = PlayerIndex.One;
            else if (kart.ID == 1)
                player_index = PlayerIndex.Two;
            else if (kart.ID == 2)
                player_index = PlayerIndex.Three;
            else
                player_index = PlayerIndex.Four;

            if (kart.ActivateFullAutoSide(SideAttached, milliseconds_passed, 0)) // this tells us if the button is pressed down and we have enough energy
            {
                if (millisCounter == 0)
                {
                    // play the charging up sound
                    Utilities.LaserChargeSound.Play();
                }

                // make the controller vibrate as the laser is charging
                GamePad.SetVibration(player_index, (float)millisCounter/(float)CHARGE_TIME, (float)millisCounter/(float)CHARGE_TIME);

                kart.drawLaserSight = true;

                millisCounter += milliseconds_passed;

                if (millisCounter < CHARGE_TIME) // we still need to charge
                {
                    return null;
                }

                // we will fire now, so reset the charging timer 
                millisCounter = 0;
                kart.drawLaserSight = false;

                List<Projectile> return_list = new List<Projectile>();

                //return_list.Add(new LaserBeam(kart.ProjectileSpawnPositions[(int)SideAttached], kart.Team, 101.0f, kart, 0.0f)); //kart.Angle));// - (float)Math.PI/2.0f));
                return_list.Add(new LaserBeam(kart.Bounds.Center - new Vector2(16, 4) + 22 * new Vector2((float)Math.Sin(kart.Angle), -(float)Math.Cos(kart.Angle)), kart.Team, 120.0f, kart, kart.Angle + ShootAngleAdjust()));

                kart.Energy -= EnergyPenalty;

                return return_list;
            }
            else
            {
                millisCounter = 0;
                kart.drawLaserSight = false;
              
                return null;
            }
        }

        public override void Deactivate()
        {
            throw new NotImplementedException();
        }

        public override string GetAssetName()
        {
            return "Weapons/burst_laser_1";
        }
    }
}
