using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KartAttack
{
  abstract class Ability
  {
    const int MAX_LEVEL = 3;

    public float EnergyPenalty { get; private set; }
    public float SpeedPenalty { get; private set; }
    public float Cost { get; private set; }
    public int Level { get; private set; }

    // which side of the kart is this ability attached to?
    public KartSides SideAttached { get; set; }

    public Ability(float energyPenalty, float speedPenalty)
    {
      EnergyPenalty = energyPenalty;
      SpeedPenalty = speedPenalty;
    }

    // Override (and prompty call) this method to modify the penalties when upgrading
    public virtual void LevelUp()
    {
      Level++;
      if (Level > MAX_LEVEL)
        Level = MAX_LEVEL;
    }

    // milliseconds_passed is just the time delta from the game loop
    // it is used for fully-automatic weapons
    abstract public List<Projectile> Activate(Kart kart, int milliseconds_passed);
    abstract public void Deactivate();

    abstract public string GetAssetName();

    // which angle do we have to add to shoot in the specified direction?
    public float ShootAngleAdjust()
    {
        if (SideAttached == KartSides.FRONT || SideAttached == KartSides.LEFT || SideAttached == KartSides.RIGHT)
        {
            return -(float)Math.PI / 2.0f;
        }
        else if (SideAttached == KartSides.REAR)
        {
            return (float)Math.PI / 2.0f;
        }
        else
        {
            return 0.0f;
        }
    }
  }
}