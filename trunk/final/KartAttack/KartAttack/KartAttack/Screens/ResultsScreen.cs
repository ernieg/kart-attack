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
    class ResultsScreen : GameScreen
    {
        public struct Stat
        {
            public string StatName;
            public int[] StatList;
        }

        #region Fields

        ContentManager content;
        SpriteFont font;
        SpriteFont font2;
        SpriteFont font3;

        Texture2D background;
        float pauseAlpha;
        bool isFinalResults;

        int totalStats;

        List<Stat> stats;

        #endregion

        #region Initialization

        public ResultsScreen(bool isFinal)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            pauseAlpha = 0;

            isFinalResults = isFinal;
            stats = new List<Stat>();
            totalStats = Utilities.karts.Count;

            Stat tempStat;
            tempStat.StatName = "Kills";
            tempStat.StatList = new int[] { 0, 0, 0, 0 };
            for (int i = 0; i < Utilities.karts.Count; i++)
            {
                int total = Utilities.karts[i].kills[0] + Utilities.karts[i].kills[1] + Utilities.karts[i].kills[2] + Utilities.karts[i].kills[3];
                tempStat.StatList[i] = total;
            }
            stats.Add(tempStat);

            tempStat.StatName = "Deaths";
            tempStat.StatList = new int[] { 0, 0, 0, 0 };
            for (int i = 0; i < Utilities.karts.Count; i++)
            {
                int total = Utilities.karts[i].deaths[0] + Utilities.karts[i].deaths[1] + Utilities.karts[i].deaths[2] + Utilities.karts[i].deaths[3];
                tempStat.StatList[i] = total;
            }
            stats.Add(tempStat);

            tempStat.StatName = "Coins\nCollected";
            tempStat.StatList = new int[] { 0, 0, 0, 0 };
            for (int i = 0; i < Utilities.karts.Count; i++)
            {
                tempStat.StatList[i] = Utilities.karts[i].totalCoins;
            }
            stats.Add(tempStat);

            for (int i = 0; i < Utilities.karts.Count; i++)
            {
                tempStat.StatName = "Killed by\nPlayer " + (Utilities.karts[i].ID + 1).ToString();
                tempStat.StatList = new int[] { 0, 0, 0, 0 };
                for (int j = 0; j < Utilities.karts.Count; j++)
                {
                    tempStat.StatList[j] = Utilities.karts[j].deaths[Utilities.karts[i].ID];
                }
                stats.Add(tempStat);
            }
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            background = content.Load<Texture2D>("Menu//ResultScreenBackground");
            font = content.Load<SpriteFont>("menufont");
            font2 = content.Load<SpriteFont>("ResultScreenTitle");
            font3 = content.Load<SpriteFont>("MainMenuEntries");
        }

        #endregion

        #region Update Draw and Handle Input

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.White, 0, 0);
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            Color drawColor = Color.White * TransitionAlpha;

            spriteBatch.Draw(background, new Vector2(0, 0), drawColor);

            spriteBatch.DrawString(font2, "Kartage Report", new Vector2(25, 15), Color.Black);
            if (!isFinalResults)
            {
                spriteBatch.DrawString(font2, Utilities.numRounds.ToString() + " Rounds Remain", new Vector2(880, 15), Color.Black);
            }
            else
            {
                List<int> WinTeam = new List<int>();
                int currentMax = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (Utilities.roundsWon[i] > currentMax)
                    {
                        WinTeam.Clear();
                        WinTeam.Add(i);
                        currentMax = Utilities.roundsWon[i];
                    }
                    else if (Utilities.roundsWon[i] == currentMax)
                    {
                        WinTeam.Add(i);
                    }
                }

                string finalString;
                if (WinTeam.Count > 1)
                {
                    finalString = "Winning Teams: ";
                }
                else
                {
                    finalString = "Winning Team: ";
                }
                for (int i = 0; i < WinTeam.Count; i++)
                {
                    if (WinTeam[i] == 0)
                    {
                        finalString = finalString + "Blue ";
                    }
                    else if (WinTeam[i] == 1)
                    {
                        finalString = finalString + "Turquoise ";
                    }
                    else if (WinTeam[i] == 2)
                    {
                        finalString = finalString + "Green ";
                    }
                    else
                    {
                        finalString = finalString + "Orange ";
                    }
                }
                spriteBatch.DrawString(font2, finalString, new Vector2(1200 - font2.MeasureString(finalString).X , 15), Color.Black);
            }
            if (Utilities.winningTeams.Contains(0))
            {
                spriteBatch.DrawString(font3, "Blue Team: " + Utilities.roundsWon[0].ToString(), new Vector2(160 - font3.MeasureString("Blue Team: " + Utilities.roundsWon[0].ToString()).X / 2, 140 - font3.MeasureString("0").Y / 2), Color.Blue);
            }
            else
            {
                spriteBatch.DrawString(font, "Blue Team: " + Utilities.roundsWon[0].ToString(), new Vector2(160 - font.MeasureString("Blue Team: " + Utilities.roundsWon[0].ToString()).X / 2, 140 - font.MeasureString("0").Y / 2), Color.Blue);
            }

            if (Utilities.winningTeams.Contains(1))
            {
                spriteBatch.DrawString(font3, "Turquoise Team: " + Utilities.roundsWon[1].ToString(), new Vector2(480 - font3.MeasureString("Turquoise Team: " + Utilities.roundsWon[1].ToString()).X / 2, 140 - font3.MeasureString("0").Y / 2), Color.Turquoise);
            }
            else
            {
                spriteBatch.DrawString(font, "Turquoise Team: " + Utilities.roundsWon[1].ToString(), new Vector2(480 - font.MeasureString("Turquoise Team: " + Utilities.roundsWon[1].ToString()).X / 2, 140 - font.MeasureString("0").Y / 2), Color.Turquoise);
            }

            if (Utilities.winningTeams.Contains(2))
            {
                spriteBatch.DrawString(font3, "Green Team: " + Utilities.roundsWon[2].ToString(), new Vector2(800 - font3.MeasureString("Green Team: " + Utilities.roundsWon[2].ToString()).X / 2, 140 - font3.MeasureString("0").Y / 2), Color.Green);
            }
            else
            {
                spriteBatch.DrawString(font, "Green Team: " + Utilities.roundsWon[2].ToString(), new Vector2(800 - font.MeasureString("Green Team: " + Utilities.roundsWon[2].ToString()).X / 2, 140 - font.MeasureString("0").Y / 2), Color.Green);
            }

            if (Utilities.winningTeams.Contains(3))
            {
                spriteBatch.DrawString(font3, "Orange Team: " + Utilities.roundsWon[3].ToString(), new Vector2(1120 - font3.MeasureString("Orange Team: " + Utilities.roundsWon[3].ToString()).X / 2, 140 - font3.MeasureString("0").Y / 2), Color.Orange);
            }
            else
            {
                spriteBatch.DrawString(font, "Orange Team: " + Utilities.roundsWon[3].ToString(), new Vector2(1120 - font.MeasureString("Orange Team: " + Utilities.roundsWon[3].ToString()).X / 2, 140 - font.MeasureString("0").Y / 2), Color.Orange);
            }

            Vector2 start = new Vector2(10, 245 - font.MeasureString("0").Y/2);
            spriteBatch.DrawString(font, "Players", start, Color.Blue);
            for (int i = 0; i < stats.Count; i++)
            {
                spriteBatch.DrawString(font, stats[i].StatName, new Vector2(250 + (i) * 150, 245 - font.MeasureString(stats[i].StatName).Y/2), Color.Blue);
            }

            for (int i = 0; i < Utilities.karts.Count; i++)
            {
                spriteBatch.DrawString(font3, "Player " + (Utilities.karts[i].ID + 1).ToString(), new Vector2(start.X, start.Y + 104 * (i + 1)), Color.Black);
                for (int j = 0; j < stats.Count; j++)
                {
                    spriteBatch.DrawString(font3, stats[j].StatList[i].ToString(), new Vector2(295 + (j) * 150 - font3.MeasureString(stats[j].StatList[i].ToString()).X/2, start.Y + 104 * (i+1)), Color.Black);
                }
            }

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {

            }
        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            PlayerIndex temp = new PlayerIndex();

            if (input.IsMenuCancel(null, out temp) || input.IsMenuSelect(null, out temp))
            {
                if (!isFinalResults)
                {
                    LoadingScreen.Load(ScreenManager, false, null, new KartSelectMenu(this, true, null, false));
                }
                else
                {
                    LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
                }
            }
            GamePad.SetVibration(PlayerIndex.One, (float)0.0, (float)0.0);
            GamePad.SetVibration(PlayerIndex.Two, (float)0.0, (float)0.0);
            GamePad.SetVibration(PlayerIndex.Three, (float)0.0, (float)0.0);
            GamePad.SetVibration(PlayerIndex.Four, (float)0.0, (float)0.0);
        }

        #endregion
    }
}
