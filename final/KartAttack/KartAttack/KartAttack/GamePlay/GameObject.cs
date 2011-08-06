#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace KartAttack
{
    abstract class GameObject
    {
        public float Angle
        {
            get { return Bounds.Rotation; }
            protected set { Bounds.Rotation = value; }
        }

        public Vector2 Position
        {
            get { return new Vector2(Bounds.X, Bounds.Y); }
        }

        public void Move(Vector2 v)
        {
            Bounds.ChangePosition(v.X, v.Y);
        }

        public Vector2 Velocity { get; set; }

        public Texture2D Texture { get; protected set; }

        public RotatedRectangle Bounds { get; protected set; }

        private int NumFrames;
        private int CurFrameNum;
        private int MillisOnCurrentFrame;
        private int millisBeforeChange;
        public List<Texture2D> AnimationTextures { get; protected set; }

        public GameObject(Vector2 position, int sizeX, int sizeY, float angle = 0, int number_of_frames = 1, int millis_before_change = 100)
        {
            Bounds = new RotatedRectangle(new Rect(position.X, position.Y, sizeX, sizeY), angle);
            Velocity = new Vector2(0.0f);

            NumFrames = number_of_frames;
            CurFrameNum = 0;
            millisBeforeChange = millis_before_change;
            MillisOnCurrentFrame = 0;
            AnimationTextures = new List<Texture2D>(NumFrames);
        }

        protected void ResetPosition(Vector2 position)
        {
            Bounds = new RotatedRectangle(new Rect(position.X, position.Y, Bounds.Width, Bounds.Height), Angle);
        }

        // make sure to pass in only the prefix for assetName.
        // for example, if the file is kart_blue_1, pass in kart_blue
        public void LoadContent(ContentManager contentManager, string assetName)
        {
            // only the first frame. this is so i don't break older code
            Texture = contentManager.Load<Texture2D>(assetName + "_1");

            for (int i = 0; i < NumFrames; i++)
            {
                string filename = assetName + "_" + (i + 1);

                AnimationTextures.Add(contentManager.Load<Texture2D>(filename));
            }
        }

        public virtual bool Collide(GameObject o)
        {
            if (Bounds.CollisionRectangle.Left > o.Bounds.CollisionRectangle.Right
                || Bounds.CollisionRectangle.Right > o.Bounds.CollisionRectangle.Left
                || Bounds.CollisionRectangle.Bottom > o.Bounds.CollisionRectangle.Top
                || Bounds.CollisionRectangle.Top > o.Bounds.CollisionRectangle.Bottom)
            {
                if (Bounds.Intersects(o.Bounds))
                    return true;
            }
            return false;
        }

        public virtual void Draw(SpriteBatch spriteBatch, int millisElapsed)
        {
            spriteBatch.Draw(AnimationTextures[CurFrameNum], Bounds.UpperLeftCorner(), null, Color.White, Angle, Vector2.Zero, 1.0f, SpriteEffects.None, 0);

            MillisOnCurrentFrame += millisElapsed;

            if (MillisOnCurrentFrame >= millisBeforeChange)
            {
                MillisOnCurrentFrame = 0;
                CurFrameNum = (CurFrameNum + 1) % NumFrames;
            }
        }

        abstract public void Update(float timeDelta);
    }
}
