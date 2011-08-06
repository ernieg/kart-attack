using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KartAttack
{
    class NumericUpDown
    {
        public Texture2D leftTriangle;
        public Texture2D rightTriangle;
        public SpriteFont font;

        public int currentValue;
        public int maxValue;
        public int minValue;
        public int increment;

        public Rectangle leftTriangleCollision
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, 81, 81);
            }
        }
        public Rectangle rightTriangleCollision
        {
            get
            {
                return new Rectangle((int)(position.X + 81 + font.MeasureString("000").X), (int)position.Y, 81, 81);
            }
        }
        public Vector2 position;

        public bool selected;
        private float timeBetweenIncrement;
        private float lastIncrementTime;
        private float timeBeforeMassIncrement;
        public int PlayerClicking;

        public NumericUpDown(int startValue, int max, int min, Vector2 numPosition, int incrementAmount)
        {
            currentValue = startValue;
            maxValue = max;
            minValue = min;
            position = numPosition;
            selected = false;
            timeBetweenIncrement = 100;
            lastIncrementTime = 0.0f;
            timeBeforeMassIncrement = 0.0f;
            PlayerClicking = -1;
            increment = incrementAmount;
        }

        public void Update(Rectangle cursor, GameTime gameTime, bool select, int player)
        {
            if (leftTriangleCollision.Intersects(cursor) || rightTriangleCollision.Intersects(cursor))
            {
                if (selected && player == PlayerClicking || !selected)
                {
                    if (select)
                    {
                        if (!selected)
                        {
                            timeBeforeMassIncrement = 0.0f;
                            lastIncrementTime = 0.0f;
                            PlayerClicking = player;
                            Console.WriteLine("ARGH");
                        }
                        Console.WriteLine(lastIncrementTime);
                        if ((!selected) || (selected && lastIncrementTime > timeBetweenIncrement))
                        {

                            if ((selected && gameTime.TotalGameTime.Milliseconds > (lastIncrementTime + timeBetweenIncrement)))
                            {
                                Console.WriteLine("BLAHHHH");
                            }
                            if (leftTriangleCollision.Intersects(cursor))
                            {
                                currentValue = currentValue - increment;
                                if (currentValue < minValue)
                                {
                                    currentValue = minValue;
                                }
                                lastIncrementTime = gameTime.ElapsedGameTime.Milliseconds;
                            }
                            else if (rightTriangleCollision.Intersects(cursor))
                            {
                                currentValue = currentValue + increment;
                                if (currentValue > maxValue)
                                {
                                    currentValue = maxValue;
                                }
                                lastIncrementTime = gameTime.ElapsedGameTime.Milliseconds;
                            }
                        }
                        //Console.WriteLine("Hi");
                        selected = true;
                        if (timeBeforeMassIncrement < 300)
                        {
                            timeBeforeMassIncrement += gameTime.ElapsedGameTime.Milliseconds;
                        }
                        else
                        {
                            lastIncrementTime += gameTime.ElapsedGameTime.Milliseconds;
                        }
                    }
                    else if (PlayerClicking == player)
                    {
                        Console.WriteLine("WTF");
                        selected = false;
                        PlayerClicking = -1;
                        lastIncrementTime = 0.0f;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(leftTriangle, position, Color.White);
            spriteBatch.Draw(rightTriangle, new Vector2(position.X + 81 + font.MeasureString("000").X, position.Y), Color.White);
            spriteBatch.DrawString(font, currentValue.ToString(), new Vector2(position.X + 81 + (font.MeasureString("000").X/2) - (font.MeasureString(currentValue.ToString()).X / 2), position.Y - 8), Color.Black);
        }
    }
}
