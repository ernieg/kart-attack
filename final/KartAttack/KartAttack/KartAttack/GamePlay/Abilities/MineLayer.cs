using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KartAttack
{
    class MineLayer : Ability
    {
        public MineLayer(float energyPenalty = 50.0f, float speedPenalty = 4.0f)
          : base(energyPenalty, speedPenalty)
        {

        }

        public override List<Projectile> Activate(Kart kart, int milliseconds_passed)
        {
            if (kart.ActivateSide(SideAttached, milliseconds_passed, 0))
            {
                // prevent a player from having more than 3 mines in play
                //if (kart.Team == Color.Blue && Utilities.blueMinesInPlay >= 3)
                //{
                //    return null;
                //}
                //else if (kart.Team == Color.Orange && Utilities.orangeMinesInPlay >= 3)
                //{
                //    return null;
                //}
                //else if (kart.Team == Color.Green && Utilities.greenMinesInPlay >= 3)
                //{
                //    return null;
                //}
                //else if (kart.Team == Color.Turquoise && Utilities.turqoiseMinesInPlay >= 3)
                //{
                //    return null;
                //}

                // increment the number of mines in play for this color
                if (kart.Team == Color.Blue)
                {
                    Utilities.blueMinesInPlay++;
                }
                else if (kart.Team == Color.Orange)
                {
                    Utilities.orangeMinesInPlay++;
                }
                else if (kart.Team == Color.Green)
                {
                    Utilities.greenMinesInPlay++;
                }
                else if (kart.Team == Color.Turquoise)
                {
                    Utilities.turqoiseMinesInPlay++;
                }

                List<Projectile> return_list = new List<Projectile>();

                return_list.Add(new Mine(kart.ProjectileSpawnPositions[(int)SideAttached], kart.Team, 80.0f, kart, kart.Angle + ShootAngleAdjust()));

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
            return "Abilities/mine_layer_1";
        }

    }
}
