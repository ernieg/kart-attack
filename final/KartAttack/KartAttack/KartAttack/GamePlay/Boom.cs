using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace KartAttack
{
    class Boom : GameObject
    {
        private int timeToLast;
        private int timeLasted = 0;
        public bool TimeToDie
        {
            get;
            private set;
        }

        public Boom(List<Texture2D> textures, int millisecondsToExist, Vector2 position, int sizeX, int sizeY, int frames, int millisBeforeTextureChange)
            : base(position, sizeX, sizeY, 0, frames, millisBeforeTextureChange)
        {
            TimeToDie = false;
            AnimationTextures = textures;
            timeToLast = millisecondsToExist;
        }

        public override void Update(float timeDelta)
        {
            timeLasted += (int)(timeDelta);
            if (timeLasted > timeToLast)
                TimeToDie = true;
        }
    }
}
