#region Using Statements
using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace KartAttack
{
    public struct PlayerKartInfo
    {
        public Color Team;
        public int Coins;
        public List<int> OwnedKarts;
        public KartInformation CurrentKart;
        public int Kart;
        public bool Active;
    }

    public class PlayerKartSelectInfo
    {
        public int currentSelected;
        public PlayerKartInfo info;
        public List<KartInformation> kartList;

        public Texture2D listBox;
        public Texture2D inactiveBox;
        public Texture2D attemptToBuyBox;
        public Texture2D upArrow;
        public Texture2D downArrow;
        public SpriteFont font;
        public SpriteFont font2;
        public Vector2 position;

        private int topRange;
        private int bottomRange;
        public bool isReady;
        public bool attemptToBuy;
        private int playerNum;
        private bool up;
        private bool down;
        private float timeBetweenIncrement;
        private float lastIncrementTime;
        private float timeBeforeMassIncrement;

        public PlayerKartSelectInfo(List<KartInformation> list, Vector2 position, PlayerKartInfo player, int num)
        {
            info = player;
            kartList = list;
            this.position = position;
            currentSelected = 0;
            topRange = 0;
            bottomRange = 12;
            isReady = !player.Active;
            attemptToBuy = false;
            playerNum = num;
            up = false;
            down = false;
            timeBetweenIncrement = 100;
            lastIncrementTime = 0.0f;
            timeBeforeMassIncrement = 0.0f;
            info.OwnedKarts = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Cost <= info.Coins)
                {
                    info.OwnedKarts.Add(i);
                }
            }
        }

        public PlayerKartSelectInfo(List<KartInformation> list, Vector2 position, Color team, bool activity, int num)
        {
            info.Coins = Utilities.coinsToStart;
            info.OwnedKarts = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Cost <= info.Coins)
                {
                    info.OwnedKarts.Add(i);
                }
            }
            info.Team = team;
            info.CurrentKart = list[0];
            info.Kart = 0;
            info.Active = activity;

            currentSelected = 0;
            kartList = list;

            this.position = position;
            topRange = 0;
            bottomRange = 12;
            isReady = !activity;
            attemptToBuy = false;
            playerNum = num;
            up = false;
            down = false;
            timeBetweenIncrement = 100;
            lastIncrementTime = 0.0f;
            timeBeforeMassIncrement = 0.0f;
        }

        public void LoadContent(ContentManager content)
        {
            listBox = content.Load<Texture2D>("Menu//KartSelectListBox");
            inactiveBox = content.Load<Texture2D>("Menu//KartSelectInactive");
            attemptToBuyBox = content.Load<Texture2D>("Menu//KartSelectBuySquare");
            font = content.Load<SpriteFont>("menufont");
            font2 = content.Load<SpriteFont>("KartDescriptionFont");
            upArrow = content.Load<Texture2D>("Menu//UpArrowKartSelect");
            downArrow = content.Load<Texture2D>("Menu//DownArrowKartSelect");
        }

        public void Update(bool isDown, bool isUp, GameTime gameTime)
        {
            if (isDown)
            {
                up = false;
                if (!down)
                {
                    timeBeforeMassIncrement = 0.0f;
                    lastIncrementTime = 0.0f;
                }
                if ((!down) || (down && lastIncrementTime > timeBetweenIncrement))
                {
                    Decrement();
                    lastIncrementTime = 0.0f;
                }
                down = true;
                if (timeBeforeMassIncrement < 300)
                {
                    timeBeforeMassIncrement += gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    lastIncrementTime += gameTime.ElapsedGameTime.Milliseconds;
                }
            }
            else if (isUp)
            {
                down = false;
                if (!up)
                {
                    timeBeforeMassIncrement = 0.0f;
                    lastIncrementTime = 0.0f;
                }
                if ((!up) || (up && lastIncrementTime > timeBetweenIncrement))
                {
                    Increment();
                    lastIncrementTime = 0.0f;
                }
                up = true;
                if (timeBeforeMassIncrement < 300)
                {
                    timeBeforeMassIncrement += gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    lastIncrementTime += gameTime.ElapsedGameTime.Milliseconds;
                }
            }
            else if (!isUp && !isDown)
            {
                up = false;
                down = false;
                timeBeforeMassIncrement = 0.0f;
                lastIncrementTime = 0.0f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (info.Active && !isReady)
            {
                spriteBatch.DrawString(font2, "Press Start if Ready", new Vector2(position.X + 440, position.Y), Color.Black);
                spriteBatch.Draw(listBox, new Vector2(position.X + 10, position.Y + 15), Color.White);

                for (int i = topRange; i < bottomRange; i++)
                {
                    if (i < kartList.Count)
                    {
                        Color stringColor = Color.Black;
                        if (i == currentSelected)
                        {
                            stringColor = Color.Red;
                        }
                        else if (i == info.Kart)
                        {
                            stringColor = Color.Blue;
                        }
                        spriteBatch.DrawString(font2, kartList[i].Name, new Vector2(position.X + 30, position.Y + 33 + font2.LineSpacing * (i - topRange)), stringColor);
                        if (i == info.Kart)
                        {
                            spriteBatch.DrawString(font2, "Equipped", new Vector2(position.X + 175, position.Y + 33 + font2.LineSpacing * (i - topRange)), Color.Blue);
                        }
                        else if (info.OwnedKarts.Contains(i))
                        {
                            spriteBatch.DrawString(font2, "OWN", new Vector2(position.X + 175, position.Y + 33 + font2.LineSpacing * (i - topRange)), Color.Green);
                        }
                        else
                        {
                            spriteBatch.DrawString(font2, "$" + kartList[i].Cost, new Vector2(position.X + 175, position.Y + 33 + font2.LineSpacing * (i - topRange)), stringColor);
                        }
                    }
                }

                spriteBatch.DrawString(font, "Coin Tier: $" + info.Coins, new Vector2(position.X + 442, position.Y + font.LineSpacing + 70), Color.Black);
                spriteBatch.DrawString(font, "Player " + playerNum.ToString(), new Vector2(position.X + 442, position.Y + 70), info.Team);

                spriteBatch.DrawString(font, "Equipped Kart: " + info.CurrentKart.Name, new Vector2(position.X + 280, position.Y + 250), Color.Blue);
                //spriteBatch.DrawString(font, "Equipped Kart:", new Vector2(position.X + 340, position.Y + 180), Color.Black);
                //spriteBatch.DrawString(font, info.CurrentKart.Name, new Vector2(position.X + 360, position.Y + 180 + font.LineSpacing), Color.Black);
                //spriteBatch.DrawString(font2, "Speed: " + info.CurrentKart.DisplaySpeed, new Vector2(position.X + 360, position.Y + 180 + font.LineSpacing * 2), Color.Black);
                //spriteBatch.DrawString(font2, "Weapons: ", new Vector2(position.X + 360, position.Y + 180 + font.LineSpacing * 2 + font2.LineSpacing), Color.Black);
                //spriteBatch.DrawString(font2, "1: " + info.CurrentKart.Weapons[0], new Vector2(position.X + 360 + font2.MeasureString("Weapons: ").X + 5, position.Y + 180 + font.LineSpacing * 2 + font2.LineSpacing), Color.Black);
                //spriteBatch.DrawString(font2, "2: " + info.CurrentKart.Weapons[1], new Vector2(position.X + 360 + font2.MeasureString("Weapons: ").X + 5, position.Y + 180 + font.LineSpacing * 2 + font2.LineSpacing * 2), Color.Black);
                //spriteBatch.DrawString(font2, "Ability: " + info.CurrentKart.Ability, new Vector2(position.X + 360, position.Y + 180 + font.LineSpacing * 2 + font2.LineSpacing * 3), Color.Black);

                spriteBatch.DrawString(font, kartList[currentSelected].Name, new Vector2(position.X + 250, position.Y + font2.LineSpacing + 5), Color.Black);
                spriteBatch.DrawString(font2, "Speed: " + kartList[currentSelected].DisplaySpeed, new Vector2(position.X + 250, position.Y + font.LineSpacing + font2.LineSpacing + 5), Color.Black);
                spriteBatch.DrawString(font2, "Weapons: ", new Vector2(position.X + 250, position.Y + font.LineSpacing + font2.LineSpacing * 2 + 5), Color.Black);
                spriteBatch.DrawString(font2, "1: " + kartList[currentSelected].Weapons[0], new Vector2(position.X + 250, position.Y + font.LineSpacing + font2.LineSpacing * 3 + 5), Color.Black);
                spriteBatch.DrawString(font2, "2: " + kartList[currentSelected].Weapons[1], new Vector2(position.X + 250, position.Y + font.LineSpacing + font2.LineSpacing * 4 + 5), Color.Black);
                spriteBatch.DrawString(font2, "Ability: " + kartList[currentSelected].Ability, new Vector2(position.X + 250, position.Y + font.LineSpacing + font2.LineSpacing * 5 + 5), Color.Black);

                if (attemptToBuy)
                {
                    if (info.Coins >= kartList[currentSelected].Cost)
                    {
                        Vector2 drawOffset = new Vector2(position.X + 50, position.Y + 40);
                        spriteBatch.Draw(attemptToBuyBox, drawOffset, Color.White);
                        spriteBatch.DrawString(font, "Purchase " + kartList[currentSelected].Name + "?", new Vector2(drawOffset.X + 50, drawOffset.Y + 20), Color.Black);
                        spriteBatch.DrawString(font, "Current Coins: " + info.Coins, new Vector2(drawOffset.X + 120, drawOffset.Y + 80), Color.Gold);
                        spriteBatch.DrawString(font, "Cost: " + kartList[currentSelected].Cost, new Vector2(drawOffset.X + 170, drawOffset.Y + 120), Color.Red);
                        spriteBatch.DrawString(font, "Coins After: " + (info.Coins - kartList[currentSelected].Cost), new Vector2(drawOffset.X + 120, drawOffset.Y + 160), Color.Gold);
                        spriteBatch.DrawString(font2, "Press A to Purchase, Press B to Cancel", new Vector2(drawOffset.X + 60, drawOffset.Y + 220), Color.Black);
                    }
                    else
                    {
                        Vector2 drawOffset = new Vector2(position.X + 50, position.Y + 40);
                        spriteBatch.Draw(attemptToBuyBox, drawOffset, Color.White);
                        spriteBatch.DrawString(font, "Your Coin Tier Isn't High Enough!", new Vector2(drawOffset.X + 70, drawOffset.Y + 20), Color.Black);
                        spriteBatch.DrawString(font2, "Get more coins next round \nto increase your Coin Tier", new Vector2(drawOffset.X + 140, drawOffset.Y + 55), Color.Black);
                        spriteBatch.DrawString(font, "Current Coin Tier: " + info.Coins, new Vector2(drawOffset.X + 70, drawOffset.Y + 120), Color.Gold);
                        spriteBatch.DrawString(font, kartList[currentSelected].Name + " Coin Tier: " + kartList[currentSelected].Cost, new Vector2(drawOffset.X + 70, drawOffset.Y + 160), Color.Red);
                        spriteBatch.DrawString(font2, "Press A or B to go back", new Vector2(drawOffset.X + 120, drawOffset.Y + 220), Color.Black);
                    }
                }

                if (topRange > 0)
                {
                    spriteBatch.Draw(upArrow, new Vector2(position.X + 30, position.Y), Color.Blue);
                    spriteBatch.Draw(upArrow, new Vector2(position.X + 90, position.Y), Color.Blue);
                    spriteBatch.Draw(upArrow, new Vector2(position.X + 150, position.Y), Color.Blue);
                    spriteBatch.Draw(upArrow, new Vector2(position.X + 210, position.Y), Color.Blue);
                }
                if (bottomRange < kartList.Count)
                {
                    spriteBatch.Draw(downArrow, new Vector2(position.X + 30, position.Y + 330), Color.Blue);
                    spriteBatch.Draw(downArrow, new Vector2(position.X + 90, position.Y + 330), Color.Blue);
                    spriteBatch.Draw(downArrow, new Vector2(position.X + 150, position.Y + 330), Color.Blue);
                    spriteBatch.Draw(downArrow, new Vector2(position.X + 210, position.Y + 330), Color.Blue);
                }
            }
            else if (!info.Active)
            {
                spriteBatch.Draw(inactiveBox, new Vector2(position.X + 10, position.Y + 10), Color.White);
            }
            else
            {
                spriteBatch.DrawString(font, "READY!!", new Vector2(position.X + 310 - font2.MeasureString("READY!!").X / 2, position.Y + 150), Color.Red);
            }


        }

        public void Increment()
        {
            if (info.Active && !isReady && !attemptToBuy)
            {
                if (currentSelected - 1 >= 0)
                {
                    if (currentSelected - 1 < topRange)
                    {
                        bottomRange--;
                        topRange--;
                    }
                    currentSelected--;
                }
            }
        }

        public void Decrement()
        {
            if (info.Active && !isReady && !attemptToBuy)
            {
                if (currentSelected + 1 < kartList.Count)
                {
                    if (currentSelected + 1 >= bottomRange)
                    {
                        bottomRange++;
                        topRange++;
                    }
                    currentSelected++;
                }
            }
        }

        public void Select()
        {
            if (info.Active && !isReady)
            {
                if (attemptToBuy)
                {
                    if (info.Coins >= kartList[currentSelected].Cost)
                    {
                        //info.Coins -= kartList[currentSelected].Cost;
                        info.OwnedKarts.Add(currentSelected);
                        info.CurrentKart = kartList[currentSelected];
                        info.Kart = currentSelected;
                    }
                    attemptToBuy = false;
                }
                else if (info.OwnedKarts.Contains(currentSelected))
                {
                    info.CurrentKart = kartList[currentSelected];
                    info.Kart = currentSelected;
                }
                else
                {
                    attemptToBuy = true;
                }
            }
        }

        public void BPressed()
        {
            if (info.Active)
            {
                if (!isReady)
                {
                    if (attemptToBuy)
                    {
                        attemptToBuy = false;
                    }
                }
                else
                {
                    isReady = false;
                }
            }
        }
    }
}
