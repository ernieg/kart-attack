using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KartAttack
{
    class PlayerBox
    {
        public Vector2 position;
        public Rectangle topHalf;
        public Rectangle bottomHalf;
        public Rectangle handicapThird;
        public bool clickedTopHalf;
        public bool clickedBottomHalf;
        public bool clickedHandicapThird;
        public List<Texture2D> textures;
        public Texture2D offTexture;
        public int currentColor;
        public Texture2D lineTexture;
        public Texture2D bottomBox;
        public Texture2D topBox;
        public Texture2D cursorTexture;
        public List<Color> possibleColors;
        public SpriteFont font;
        public SpriteFont font2;
        public float damageHandicap;

        public bool On;

        private int frame;
        private int totalFrames;
        private float timeToWaitForFrame;
        private float timeLastUpdated;

        private int boxNumber;

        public PlayerBox(Vector2 Position, List<Texture2D> Textures, Texture2D OffTexture, int TotalFrames, int StartFrame, int boxNum)
        {
            boxNumber = boxNum + 1;
            position = Position;
            textures = Textures;
            totalFrames = TotalFrames;
            offTexture = OffTexture;
            frame = StartFrame;
            timeToWaitForFrame = 425.0f;
            timeLastUpdated = 0.0f;
            On = false;
            clickedBottomHalf = false;
            clickedTopHalf = false;
            clickedHandicapThird = false;
            topHalf = new Rectangle((int)Position.X + 25, (int)Position.Y, 158, 80);
            bottomHalf = new Rectangle((int)Position.X + 45, (int)(Position.Y + 100), 118, 75);
            handicapThird = new Rectangle((int)Position.X + 45, (int)(Position.Y + 190), 118, 75);
            currentColor = 0;
            damageHandicap = 1.0f;

            possibleColors = new List<Color>();

            possibleColors.Add(Color.Blue);
            possibleColors.Add(Color.Turquoise);
            possibleColors.Add(Color.Green);
            possibleColors.Add(Color.Orange);
        }

        public void Update(GameTime gameTime)
        {
            timeLastUpdated += gameTime.ElapsedGameTime.Milliseconds;
            if (timeLastUpdated > timeToWaitForFrame)
            {
                frame = (frame + 1) % totalFrames;
                timeLastUpdated = 0;
            }

            if (On)
            {
                if (clickedTopHalf)
                {
                    On = false;
                    clickedTopHalf = false;
                }
                if (clickedBottomHalf)
                {
                    clickedBottomHalf = false;
                    currentColor = (currentColor + 1) % possibleColors.Count;
                }
                if (clickedHandicapThird)
                {
                    clickedHandicapThird = false;
                    if (damageHandicap == 1.0f)
                        damageHandicap = 1.2f;
                    else if (damageHandicap == 1.2f)
                        damageHandicap = 1.4f;
                    else if (damageHandicap == 1.4f)
                        damageHandicap = 1.6f;
                    else if (damageHandicap == 1.6f)
                        damageHandicap = 1.8f;
                    else if (damageHandicap == 1.8f)
                        damageHandicap = 2.0f;
                    else if (damageHandicap == 2.0f)
                        damageHandicap = 3.0f;
                    else
                        damageHandicap = 1.0f;
                }
            }
            else
            {
                if (clickedTopHalf || clickedBottomHalf || clickedHandicapThird)
                {
                    On = true;
                    clickedBottomHalf = false;
                    clickedTopHalf = false;
                    clickedHandicapThird = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (On)
            {
                spriteBatch.Draw(textures[frame], position, possibleColors[currentColor]);
                //spriteBatch.Draw(lineTexture, new Vector2(position.X + 15, position.Y + 135), Color.White);
                spriteBatch.Draw(cursorTexture, new Vector2(position.X + 78, position.Y + 40), possibleColors[currentColor]);
                if (currentColor == 0 || currentColor == 2)
                {
                    spriteBatch.DrawString(font2, boxNumber.ToString(), new Vector2(position.X + 95 - font.MeasureString(boxNumber.ToString()).X / 2, position.Y + 39), Color.White);
                }
                else
                {               
                    spriteBatch.DrawString(font2, boxNumber.ToString(), new Vector2(position.X + 95 - font.MeasureString(boxNumber.ToString()).X / 2, position.Y + 39), Color.Black);
                }
                spriteBatch.Draw(topBox, new Vector2(position.X + 45, position.Y + 100), Color.White);
                spriteBatch.Draw(bottomBox, new Vector2(position.X + 45, position.Y + 190), Color.White);
                spriteBatch.DrawString(font, " Team \nChange", new Vector2(position.X + 72, position.Y + 115), Color.Black);
                if (damageHandicap == 1)
                {
                    spriteBatch.DrawString(font, "   No\nHandicap", new Vector2(position.X + 70, position.Y + 200), Color.Black);
                }
                else
                {
                    if (damageHandicap == 2 || damageHandicap == 3)
                    {
                        spriteBatch.DrawString(font, "Damage \n  " + damageHandicap.ToString() + ".0x", new Vector2(position.X + 70, position.Y + 200), Color.Black);
                    }
                    else
                    {
                        spriteBatch.DrawString(font, "Damage \n  " + damageHandicap.ToString() + "x", new Vector2(position.X + 70, position.Y + 200), Color.Black);
                    }
                }

            }
            else
            {
                spriteBatch.Draw(offTexture, position, Color.White);
                spriteBatch.DrawString(font, "Select to play\n   Player " + boxNumber.ToString(), new Vector2(position.X + 40, position.Y + 125), Color.Black);
            }
        }
    }
}
