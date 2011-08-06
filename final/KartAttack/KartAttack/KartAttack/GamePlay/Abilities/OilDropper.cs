using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KartAttack
{
    class OilDropper : Ability
    {
        public OilDropper(float energyPenalty = 18.0f, float speedPenalty = 2.0f)
          : base(energyPenalty, speedPenalty)
        {

        }

        public override List<Projectile> Activate(Kart kart, int milliseconds_passed)
        {
            if (kart.ActivateSide(SideAttached, milliseconds_passed, 0))
            {
                // prevent a player from having more than 5 oil slicks in play
                //if (kart.Team == Color.Blue && Utilities.blueOilSlicksInPlay >= 5)
                //{
                //    return null;
                //}
                //else if (kart.Team == Color.Orange && Utilities.orangeOilSlicksInPlay >= 5)
                //{
                //    return null;
                //}
                //else if (kart.Team == Color.Green && Utilities.greenOilSlicksInPlay >= 5)
                //{
                //    return null;
                //}
                //else if (kart.Team == Color.Turquoise && Utilities.turqoiseOilSlicksInPlay >= 5)
                //{
                //    return null;
                //}

                // increment the number of oil slicks in play for this color
                if (kart.Team == Color.Blue)
                {
                    Utilities.blueOilSlicksInPlay++;
                }
                else if (kart.Team == Color.Orange)
                {
                    Utilities.orangeOilSlicksInPlay++;
                }
                else if (kart.Team == Color.Green)
                {
                    Utilities.greenOilSlicksInPlay++;
                }
                else if (kart.Team == Color.Turquoise)
                {
                    Utilities.turqoiseOilSlicksInPlay++;
                }

                List<Projectile> return_list = new List<Projectile>();

                float randomAngle = (float)Utilities.random.NextDouble() * 2.0f * (float)Math.PI;

                return_list.Add(new OilSlick(kart.ProjectileSpawnPositions[(int)SideAttached], kart.Team, 0.0f, kart, randomAngle));

                // play the "splat!" sound
                Utilities.OilSound.Play();

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
            return "Abilities/oil_dropper_1";
        }



    }
}
