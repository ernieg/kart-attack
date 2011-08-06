using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KartAttack
{
  class Pistol : Ability
  {
    public Pistol(float energyPenalty = 2.0f, float speedPenalty = 1.0f)
      : base(energyPenalty, speedPenalty)
    {

    }

    public override List<Projectile> Activate(Kart kart, int milliseconds_passed)
    {
        if ( kart.ActivateFullAutoSide(SideAttached, milliseconds_passed, 400) )
        {
            List<Projectile> return_list = new List<Projectile>();

            //return_list.Add(new Bullet(kart.ProjectileSpawnPositions[(int)SideAttached], kart.Team, 30.0f, kart ,kart.Angle + ShootAngleAdjust(), 1.25f));
            return_list.Add(new Bullet(kart.Bounds.Center, kart.Team, 20.0f, kart, kart.Angle + ShootAngleAdjust(), 1.25f));

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

    public override string  GetAssetName()
    {
        return "Weapons/pistol_1";
    }
  }
}
