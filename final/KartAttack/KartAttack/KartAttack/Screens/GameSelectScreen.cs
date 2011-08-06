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
    class GameSelectScreen : GameScreen
    {
        struct PlayerMovement
        {
            public bool Connected;
            public Texture2D CursorTexture;
            public Vector2 CurrentPosition;
            public Vector2 Direction;
            public bool Movement;
            public int Player;
            public Rectangle CursorCollisionBox
            {
                get
                {
                    return new Rectangle((int)CurrentPosition.X, (int)CurrentPosition.Y, 35, 35);
                }
            }
            public bool Select;
            public bool Click;
        };

        #region Fields

        ContentManager content;
        SpriteFont font;
        SpriteFont font2;
        PlayerMovement[] players;

        Vector2 playerBoxStartLocation;
        int distBetweenBox;
        List<Texture2D> playerBoxTextures;
        Texture2D offBoxTexture;
        PlayerBox[] playerBoxes;
        NumericUpDown roundAmount;
        NumericUpDown timeAmount;
        NumericUpDown coinAmount;

        Texture2D background;

        float pauseAlpha;

        bool canStart;

        #endregion

        #region Initialization

        public GameSelectScreen() 
        {
            Utilities.damageHandicaps = new List<float>(4);
            for (int i = 0; i < 4; i++)
                Utilities.damageHandicaps.Add(1.0f);

            playerBoxStartLocation = new Vector2(160, 425);
            distBetweenBox = 250;

            players = new PlayerMovement[4];
            for (int i = 0; i < 4; i++)
            {
                players[i].Connected = false;
                players[i].CurrentPosition = new Vector2(playerBoxStartLocation.X + 84 + (distBetweenBox * i), 557);
                players[i].Movement = false;
                players[i].Direction = new Vector2(0, 0);
                players[i].Select = false;
                players[i].Click = false;
                players[i].Player = i + 1;
            }

            
            playerBoxes = new PlayerBox[4];
            Random rand = new Random();
            for (int i = 0; i < 4; i++)
            {
                playerBoxes[i] = new PlayerBox(new Vector2(playerBoxStartLocation.X + (i * distBetweenBox), playerBoxStartLocation.Y), playerBoxTextures, offBoxTexture, 4, rand.Next(0,3), i);
                playerBoxes[i].On = true;
                playerBoxes[i].currentColor = i;
            }

            playerBoxTextures = new List<Texture2D>();

            roundAmount = new NumericUpDown(3, 10, 1, new Vector2(670, 10), 1);
            timeAmount = new NumericUpDown(Utilities.timePerRound, 10, 1, new Vector2(670, 120), 1);
            coinAmount = new NumericUpDown(0, 990, 0, new Vector2(670, 230), 10);
            Utilities.roundsWon = new int[] { 0, 0, 0, 0 };

            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            pauseAlpha = 0;
            canStart = false;
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }
            
            playerBoxTextures.Add(content.Load<Texture2D>("Menu//WGameSelectPlayer1"));
            playerBoxTextures.Add(content.Load<Texture2D>("Menu//WGameSelectPlayer2"));
            playerBoxTextures.Add(content.Load<Texture2D>("Menu//WGameSelectPlayer3"));
            playerBoxTextures.Add(content.Load<Texture2D>("Menu//WGameSelectPlayer4"));

            offBoxTexture = content.Load<Texture2D>("Menu//OffBox");

            background = content.Load<Texture2D>("KartAttackBackground");

            for (int i = 0; i < 4; i++)
            {
                playerBoxes[i].offTexture = offBoxTexture;
                playerBoxes[i].textures = new List<Texture2D>(playerBoxTextures);
                playerBoxes[i].lineTexture = content.Load<Texture2D>("Menu//BoxLine");
                playerBoxes[i].topBox = content.Load<Texture2D>("Menu//GameSelectCurrentPlayerBox");
                playerBoxes[i].bottomBox = content.Load<Texture2D>("Menu//GameSelectTeamSelectBox");
                playerBoxes[i].font = content.Load<SpriteFont>("KartDescriptionFont");
                playerBoxes[i].cursorTexture = (content.Load<Texture2D>("Menu//TempCursor"));
                playerBoxes[i].font2 = content.Load<SpriteFont>("menufont");
            }

            for (int i = 0; i < 4; i++)
            {
                players[i].CursorTexture = (content.Load<Texture2D>("Menu//TempCursor"));
            }


            font = content.Load<SpriteFont>("Menu");
            font2 = content.Load<SpriteFont>("menufont");
            roundAmount.leftTriangle = content.Load<Texture2D>("Menu//DownArrow");
            roundAmount.rightTriangle = content.Load<Texture2D>("Menu//UpArrow");
            roundAmount.font = content.Load<SpriteFont>("NumericUpDownFont");
            timeAmount.leftTriangle = content.Load<Texture2D>("Menu//DownArrow");
            timeAmount.rightTriangle = content.Load<Texture2D>("Menu//UpArrow");
            timeAmount.font = content.Load<SpriteFont>("NumericUpDownFont");
            coinAmount.leftTriangle = content.Load<Texture2D>("Menu//DownArrow");
            coinAmount.rightTriangle = content.Load<Texture2D>("Menu//UpArrow");
            coinAmount.font = content.Load<SpriteFont>("NumericUpDownFont");
        }

        #endregion

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.White, 0, 0);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            roundAmount.Draw(spriteBatch);
            timeAmount.Draw(spriteBatch);
            coinAmount.Draw(spriteBatch);

            spriteBatch.DrawString(roundAmount.font, "Rounds: ", new Vector2(roundAmount.position.X - roundAmount.font.MeasureString("Rounds: ").X, roundAmount.position.Y - 8), Color.Black);
            spriteBatch.DrawString(timeAmount.font, "Minutes/Round: ", new Vector2(timeAmount.position.X - timeAmount.font.MeasureString("Minutes/Round: ").X, timeAmount.position.Y - 8), Color.Black);
            spriteBatch.DrawString(coinAmount.font, "Starting Coins: ", new Vector2(coinAmount.position.X - coinAmount.font.MeasureString("Starting Coins: ").X, coinAmount.position.Y - 8), Color.Black);


            for (int i = 0; i < playerBoxes.GetLength(0); i++)
            {
                playerBoxes[i].Draw(spriteBatch);
            }

            canStart = false;
            int numPlayers = 0;
            List<int> currentColors = new List<int>();
            foreach (PlayerBox box in playerBoxes)
            {
                if (box.On)
                {
                    numPlayers++;
                    if (numPlayers >= 2 && !currentColors.Contains(box.currentColor))
                    {
                        canStart = true;
                        break;
                    }
                    else
                    {
                        currentColors.Add(box.currentColor);
                    }
                }
            }

            if (canStart)
            {
                spriteBatch.DrawString(font, "Press Start to Begin!", new Vector2(330, 310), Color.Black);
            }

            for (int i = 0; i < 4; i++)
            {
                if (players[i].Connected)
                {
                    if (playerBoxes[i].On)
                    {
                        Color current = playerBoxes[i].possibleColors[playerBoxes[i].currentColor];
                        spriteBatch.Draw(players[i].CursorTexture, players[i].CurrentPosition, playerBoxes[i].possibleColors[playerBoxes[i].currentColor]);
                        Color contrastColor = new Color();
                        if (playerBoxes[i].possibleColors[playerBoxes[i].currentColor] == Color.Blue || playerBoxes[i].possibleColors[playerBoxes[i].currentColor] == Color.Green)
                        {
                            contrastColor = Color.White;
                        }
                        else
                        {
                            contrastColor = Color.Black;
                        }
                        spriteBatch.DrawString(font2, players[i].Player.ToString(), new Vector2(players[i].CurrentPosition.X + 17 - font2.MeasureString((i + 1).ToString()).X / 2,
                                                                                                players[i].CurrentPosition.Y + 17 - font2.MeasureString((i + 1).ToString()).Y / 2), contrastColor);
                    }
                    else
                    {
                        spriteBatch.Draw(players[i].CursorTexture, players[i].CurrentPosition, Color.Black);
                        spriteBatch.DrawString(font2, players[i].Player.ToString(), new Vector2(players[i].CurrentPosition.X + 17 - font2.MeasureString((i + 1).ToString()).X / 2,
                                                                                                players[i].CurrentPosition.Y + 17 - font2.MeasureString((i + 1).ToString()).Y / 2), Color.White);
                    }
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
                for(int i = 0; i < 4; i++)
                {
                    if (players[i].Movement && players[i].Connected)
                    {
                        Vector2 newPosition = players[i].CurrentPosition + (players[i].Direction * 8);
                        if (newPosition.X > 1245)
                        {
                            newPosition.X = 1245;
                        }
                        else if (newPosition.X < 0)
                        {
                            newPosition.X = 0;
                        }

                        if (newPosition.Y > 685)
                        {
                            newPosition.Y = 685;
                        }
                        else if (newPosition.Y < 0)
                        {
                            newPosition.Y = 0;
                        }
                        players[i].CurrentPosition = newPosition;
                    }

                    if (players[i].Click)
                    {
                        for(int j = 0; j < 4; j++)
                        {
                            if (players[j].Connected)
                            {
                                if (playerBoxes[j].topHalf.Intersects(players[i].CursorCollisionBox))
                                {
                                    playerBoxes[j].clickedTopHalf = true;
                                }
                                else if (playerBoxes[j].bottomHalf.Intersects(players[i].CursorCollisionBox))
                                {
                                    playerBoxes[j].clickedBottomHalf = true;
                                }
                                else if (playerBoxes[j].handicapThird.Intersects(players[i].CursorCollisionBox))
                                {
                                    playerBoxes[j].clickedHandicapThird = true;
                                }
                            }
                        }
                    }
                    roundAmount.Update(players[i].CursorCollisionBox, gameTime, players[i].Select, i);
                    timeAmount.Update(players[i].CursorCollisionBox, gameTime, players[i].Select, i);
                    coinAmount.Update(players[i].CursorCollisionBox, gameTime, players[i].Select, i);
                }

                for (int i = 0; i < 4; i++)
                {
                    playerBoxes[i].Update(gameTime);
                }
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
            if(input.IsMenuCancel(null, out temp))
            {
                Utilities.timePerRound = timeAmount.currentValue;
                Utilities.numRounds = roundAmount.currentValue;
                LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
            }

            int playerIndex = 0;
            foreach (GamePadState controller in input.CurrentGamePadStates)
            {
                if (controller.IsConnected)
                {
                    players[playerIndex].Click = (input.LastGamePadStates[playerIndex].IsButtonDown(Buttons.A) && input.CurrentGamePadStates[playerIndex].IsButtonUp(Buttons.A));
                    players[playerIndex].Select = input.CurrentGamePadStates[playerIndex].IsButtonDown(Buttons.A);
                    //Console.WriteLine("Select for " + playerIndex + " is " + players[playerIndex].Select);

                    if (canStart)
                    {
                        if ((input.LastGamePadStates[playerIndex].IsButtonDown(Buttons.Start) && input.CurrentGamePadStates[playerIndex].IsButtonUp(Buttons.Start)))
                        {
                            List<Color> activePlayers = new List<Color>();
                            for(int i = 0; i < 4; i++)
                            {
                                if(playerBoxes[i].On)
                                {
                                    activePlayers.Add(playerBoxes[i].possibleColors[playerBoxes[i].currentColor]);
                                }
                                else
                                {
                                    activePlayers.Add(Color.Black);
                                }
                            }
                            Utilities.timePerRound = timeAmount.currentValue;
                            Utilities.numRounds = roundAmount.currentValue;
                            Utilities.coinsToStart = coinAmount.currentValue;
                            for (int p = 0; p < 4; p++ )
                                Utilities.damageHandicaps[0] = playerBoxes[0].damageHandicap;
                            LoadingScreen.Load(ScreenManager, false, null, new KartSelectMenu(this, true, activePlayers, true));
                            break;
                        }
                    }


                    //Connect Select to Checking for collision with cursor and Box Rectangles. Then set appropriate bools
                    players[playerIndex].Connected = true;
                    if (controller.ThumbSticks.Left.X != 0.0f || controller.ThumbSticks.Left.Y != 0.0f)
                    {
                        players[playerIndex].Direction = new Vector2(controller.ThumbSticks.Left.X, -controller.ThumbSticks.Left.Y);
                        players[playerIndex].Movement = true;
                    }
                    else
                    {
                        players[playerIndex].Movement = false;
                    }

                }
                else
                {
                    players[playerIndex].Connected = false;
                    players[playerIndex].Movement = false;
                    playerBoxes[playerIndex].On = false;
                }
                playerIndex++;
            }
        }
    }
}
