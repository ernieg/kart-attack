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
    public struct KartInformation
    {
        public string Name;
        public List<string> Weapons;
        public string Ability;
        public int Cost;
        public float Speed;
        public int DisplaySpeed;
    }

    public struct PairForInput
    {
        public PlayerIndex player;
        public int index;
    }
    public struct PlayerUpDown
    {
        public bool up;
        public bool down;
    }

    class KartSelectMenu : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont font;

        Texture2D background;
        float pauseAlpha;

        List<KartInformation> kartList;
        List<PlayerKartSelectInfo> playerSelectInfo;
        PlayerUpDown[] playersUpDown;

        GameScreen previousScreen;
        bool canGoBack;
        #endregion

        #region Initialization

        public KartSelectMenu(GameScreen PreviousScreen, bool CanGoBack, List<Color>ActivePlayers, bool NewGame)
        {
            kartList = new List<KartInformation>();
            StreamReader reader = File.OpenText(Path.Combine(Environment.CurrentDirectory, "Kart List.txt"));
            string input = null;

            while ((input = reader.ReadLine()) != null)
            {
                string[] split = input.Split(new Char[] { '|' });
                KartInformation newKart;
                newKart.Name = split[0];
                newKart.Cost = (int)Convert.ToUInt32(split[1]);
                newKart.Speed = (float)Convert.ToDouble(split[2]);
                newKart.DisplaySpeed = (int)(newKart.Speed / 0.01f - 8);
                newKart.Weapons = new List<string>();
                newKart.Weapons.Add(split[3].Split(new Char[] { ',' })[0]);
                newKart.Weapons.Add(split[3].Split(new Char[] { ',' })[1]);
                newKart.Ability = split[4];
                kartList.Add(newKart);

                // checks that the information read in from Kart List.txt is valid
                Utilities.checkValidKartInformation(newKart);
            }
            reader.Close();

            playerSelectInfo = new List<PlayerKartSelectInfo>();
            if (NewGame)
            {
                playerSelectInfo.Add(new PlayerKartSelectInfo(kartList, new Vector2(0, 0), ActivePlayers[0], !Color.Black.Equals(ActivePlayers[0]), 1));
                playerSelectInfo.Add(new PlayerKartSelectInfo(kartList, new Vector2(650, 0), ActivePlayers[1], !Color.Black.Equals(ActivePlayers[1]), 2));
                playerSelectInfo.Add(new PlayerKartSelectInfo(kartList, new Vector2(0, 370), ActivePlayers[2], !Color.Black.Equals(ActivePlayers[2]), 3));
                playerSelectInfo.Add(new PlayerKartSelectInfo(kartList, new Vector2(650, 370), ActivePlayers[3], !Color.Black.Equals(ActivePlayers[3]), 4));
            }
            else
            {
                playerSelectInfo.Add(new PlayerKartSelectInfo(kartList, new Vector2(0, 0), Utilities.playerKartInformation[0], 1));
                playerSelectInfo.Add(new PlayerKartSelectInfo(kartList, new Vector2(650, 0), Utilities.playerKartInformation[1], 2));
                playerSelectInfo.Add(new PlayerKartSelectInfo(kartList, new Vector2(0, 370), Utilities.playerKartInformation[2], 3));
                playerSelectInfo.Add(new PlayerKartSelectInfo(kartList, new Vector2(650, 370), Utilities.playerKartInformation[3], 4));
            }

            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            pauseAlpha = 0;

            playersUpDown = new PlayerUpDown[4];
            for (int i = 0; i < 4; i++)
            {
                playersUpDown[i].up = false;
                playersUpDown[i].down = false;
            }

            previousScreen = PreviousScreen;
            canGoBack = CanGoBack;
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            background = content.Load<Texture2D>("Menu//KartSelectBackground");
            font = content.Load<SpriteFont>("menufont");

            foreach (PlayerKartSelectInfo info in playerSelectInfo)
            {
                info.LoadContent(content);
            }
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
            //spriteBatch.DrawString(font, "KART SELECTION", new Vector2(640 - font.MeasureString("KART SELECTION").X/2, 360 - font.MeasureString("0").Y/2), Color.Black);

            foreach (PlayerKartSelectInfo info in playerSelectInfo)
            {
                info.Draw(spriteBatch);
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
                bool canStart = true;
                for (int i = 0; i < playerSelectInfo.Count; i++)
                {
                    if (!playerSelectInfo[i].isReady)
                    {
                        canStart = false;
                        break;
                    }
                }

                if (canStart)
                {
                    PlayerKartInfo[] newInfo = new PlayerKartInfo[4];
                    for (int i = 0; i < playerSelectInfo.Count; i++)
                    {
                        if (playerSelectInfo[i].info.Active)
                        {
                            playerSelectInfo[i].isReady = false;
                        }
                        newInfo[i] = (playerSelectInfo[i].info);
                    }
                    Utilities.playerKartInformation = newInfo;
                    canStart = false;
                    LoadingScreen.Load(ScreenManager, false, null, new MapSelectScreen(this, true));
                }

                int playerIndex = 0;
                foreach (PlayerKartSelectInfo info in playerSelectInfo)
                {
                    info.Update(playersUpDown[playerIndex].down, playersUpDown[playerIndex].up, gameTime);
                    playerIndex++;
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
            List<PairForInput>players = new List<PairForInput>();
            PairForInput pair;
            pair.index = 0;
            pair.player = PlayerIndex.One;
            players.Add(pair);
            pair.index = 1;
            pair.player = PlayerIndex.Two;
            players.Add(pair);
            pair.index = 2;
            pair.player = PlayerIndex.Three;
            players.Add(pair);
            pair.index = 3;
            pair.player = PlayerIndex.Four;
            players.Add(pair);

            if (input.IsMenuCancel(null, out temp) && canGoBack)
            {
                foreach (PairForInput play in players)
                {
                    if (temp == play.player)
                    {
                        if (!playerSelectInfo[play.index].isReady && !playerSelectInfo[play.index].attemptToBuy)
                        {
                            LoadingScreen.Load(ScreenManager, false, null, previousScreen);
                            break;
                        }
                    }
                }

            }
            int playerIndex = 0;
            foreach(PairForInput index in players)
            {
                playersUpDown[playerIndex].up = input.CurrentGamePadStates[playerIndex].IsButtonDown(Buttons.LeftThumbstickUp);
                playersUpDown[playerIndex].down = input.CurrentGamePadStates[playerIndex].IsButtonDown(Buttons.LeftThumbstickDown);

                //if (input.IsMenuDown(index.player))
                //{
                //    playerSelectInfo[index.index].Decrement();
                //}
                //if (input.IsMenuUp(index.player))
                //{
                //    playerSelectInfo[index.index].Increment();
                //}
                if (input.IsNewButtonPress(Buttons.A, index.player, out temp))
                {
                    playerSelectInfo[index.index].Select();
                }
                if (input.IsNewButtonPress(Buttons.B, index.player, out temp))
                {
                    playerSelectInfo[index.index].BPressed();
                }

                if (input.IsNewButtonPress(Buttons.Start, index.player, out temp))
                {
                    playerSelectInfo[index.index].isReady = true;
                }
                playerIndex++;
            }
        }

        #endregion

    }
}
