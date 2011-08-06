using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace KartAttack
{
    class Projectile : GameObject
    {
        public Color Team { get; protected set; }
        public float Damage { get; protected set; }
        public Kart Source { get; private set; }
        public List<Texture2D> Explosions { get; protected set; }
        public int ExplosionLength { get; protected set; }
        public int ExplosionFrames { get; protected set; }
        public int ExplosionFrameLength { get; protected set; }
        public int ExplosionsSizeX { get; protected set; }
        public int ExplosionsSizeY { get; protected set; }

        public Projectile(Vector2 position, Color team, float damage, int sizeX, int sizeY, Kart k, float angle = 0, int number_of_frames = 1, 
            List<Texture2D> explosions = null, int explosionLength = 100, int explosionFrames = 1, int eSizeX = 8, int eSizeY = 8)
            : base(position, sizeX, sizeY, angle, number_of_frames)
        {
            Team = team;
            Damage = damage;
            Source = k;

            if (explosions == null)
                Explosions = Utilities.bulletBooms;
            else
                Explosions = explosions;

            ExplosionLength = explosionLength;
            ExplosionFrames = explosionFrames;
            ExplosionFrameLength = ExplosionLength / ExplosionFrames;
            ExplosionsSizeX = eSizeX;
            ExplosionsSizeY = eSizeY;

            // by default, the velocity of the projectile is a unit vector
            // subclasses should override Update() to add in a multiplier
            Velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin((double)angle));
            Velocity.Normalize();
        }

        public Boom GetNewBoom()
        {
            return new Boom(Explosions, ExplosionLength, new Vector2(Bounds.Center.X - ExplosionsSizeX/2, Bounds.Center.Y - ExplosionsSizeY/2), ExplosionsSizeX, ExplosionsSizeY, ExplosionFrames, ExplosionFrameLength);
        }

        public override void Update(float timeDelta)
        {
            base.Move(Velocity * timeDelta);

        }
    }
}
