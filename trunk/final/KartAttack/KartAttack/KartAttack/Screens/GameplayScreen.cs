#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace KartAttack
{
    class Input
    {
        public Input()
        {
            forward = 0;
            backward = 0;
            a = false;
            b = false;
            x = false;
            y = false;
            turnLeft = 0;
            turnRight = 0;
            direction = new Vector2(1, 0);
            forwardG = false;
            backwardG = false;
        }
        public int forward;
        public int backward;
        public bool a;
        public bool b;
        public bool x;
        public bool y;
        public int turnLeft; // change for xbox controllers
        public int turnRight;
        public Vector2 direction;
        public bool forwardG;
        public bool backwardG;
    }

    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        Vector2 playerPosition = new Vector2(100, 100);
        Vector2 enemyPosition = new Vector2(100, 100);

        Random random = new Random();

        float pauseAlpha;

        Input p1Input;

        List<Kart> karts;
        List<Projectile> projectiles;
        public List<Wall> Map { get; private set; }
        public List<Base> Bases { get; private set; }
        public List<Coin> Coins { get; private set; }

        string mapName;

        private int timeLeft;
        private int numRounds;
        private bool roundOver;

        private bool quickGame;

        int countdown;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen(string mapName, bool QuickGame)
        {
            karts = new List<Kart>();
            projectiles = new List<Projectile>();
            Map = new List<Wall>();
            Bases = new List<Base>();
            Coins = new List<Coin>();
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            p1Input = new Input();
            this.mapName = mapName;
            numRounds = 2;
            roundOver = false;
            timeLeft = Utilities.timePerRound * 60000;
            //timeLeft = 300000;
            countdown = 3000;

            quickGame = QuickGame;

            // there are 0 mines in play
            Utilities.blueMinesInPlay = Utilities.orangeMinesInPlay = Utilities.greenMinesInPlay = Utilities.turqoiseMinesInPlay = 0;
            
            // there are 0 oil slicks in play
            Utilities.blueOilSlicksInPlay = Utilities.orangeOilSlicksInPlay = Utilities.greenOilSlicksInPlay = Utilities.turqoiseOilSlicksInPlay = 0;
        
            // clear the poofs and explosions so we don't see them from the previous game
            if (Utilities.booms != null) 
              Utilities.booms.Clear();
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            Console.WriteLine("Load Content");
            if (Utilities.content == null)
            {
                Utilities.content = new ContentManager(ScreenManager.Game.Services, "Content");
                Utilities.loadContent();
            }

            //Load Map
            Map.Clear();
            StreamReader reader = File.OpenText(Path.Combine(Environment.CurrentDirectory, mapName));
            string input = null;

            // Exterior walls
            Map.Add(new Wall("Wall", 42, 1, 32, new Vector2(-32.0f, -32.0f)));
            Map.Add(new Wall("Wall", 42, 1, 32, new Vector2(-32.0f, 720.0f)));
            Map.Add(new Wall("Wall", 1, 24, 32, new Vector2(-32.0f, -32.0f)));
            Map.Add(new Wall("Wall", 1, 24, 32, new Vector2(1280.0f, -32.0f)));
            
            while ((input = reader.ReadLine()) != null)
            {
                string[] split = input.Split(new Char[] { '|' });
                if (split[0].Equals("Wall"))
                {
                    Map.Add(new Wall(split[0], (int)Convert.ToUInt32(split[5]),
                                     (int)Convert.ToUInt32(split[6]), (int)Convert.ToUInt32(split[4]),
                                     new Vector2((int)Convert.ToUInt32(split[2]), (int)Convert.ToUInt32(split[3]))));
                }
                else if (split[0].Contains("Base"))
                {
                    if (split[0].Contains("1"))
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (quickGame || Utilities.playerKartInformation[i].Active && Utilities.playerKartInformation[i].Team.Equals(Color.Blue))
                            {
                                Bases.Add(new Base(new Vector2((int)Convert.ToUInt32(split[2]), (int)Convert.ToUInt32(split[3])), Color.Blue));
                                break;
                            }
                        }
                    }
                    else if (split[0].Contains("2"))
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (quickGame || Utilities.playerKartInformation[i].Active && Utilities.playerKartInformation[i].Team.Equals(Color.Turquoise))
                            {
                                Bases.Add(new Base(new Vector2((int)Convert.ToUInt32(split[2]), (int)Convert.ToUInt32(split[3])), Color.Turquoise));
                                break;
                            }
                        }
                    }
                    else if (split[0].Contains("3"))
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (quickGame || Utilities.playerKartInformation[i].Active && Utilities.playerKartInformation[i].Team.Equals(Color.Orange))
                            {
                                Bases.Add(new Base(new Vector2((int)Convert.ToUInt32(split[2]), (int)Convert.ToUInt32(split[3])), Color.Orange));
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (quickGame || Utilities.playerKartInformation[i].Active && Utilities.playerKartInformation[i].Team.Equals(Color.Green))
                            {
                                Bases.Add(new Base(new Vector2((int)Convert.ToUInt32(split[2]), (int)Convert.ToUInt32(split[3])), Color.Green));
                                break;
                            }
                        }
                    }
                }
                else if (split[0].Contains("Coin"))
                {
                    Coins.Add(new Coin(new Vector2((int)Convert.ToUInt32(split[2]), (int)Convert.ToUInt32(split[3]))));
                }
            }
            reader.Close();

            Utilities.loadNewBases(Bases);

            foreach (Wall wall in Map)
            {
                wall.LoadContent(Utilities.content);
            }

            foreach (Base flag in Bases)
            {
                flag.LoadContent(Utilities.content);
            }

            Utilities.loadCoins(Coins);


            if (!quickGame)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (Utilities.playerKartInformation[i].Active)
                    {
                        Kart tempKart = new Kart(Utilities.playerKartInformation[i].Team, i, Utilities.playerKartInformation[i].CurrentKart.Speed);
                        tempKart.AttachAbility(Utilities.getAbility(Utilities.playerKartInformation[i].CurrentKart.Ability), KartSides.REAR);
                        tempKart.AttachAbility(Utilities.getAbility(Utilities.playerKartInformation[i].CurrentKart.Weapons[0]), KartSides.LEFT);
                        tempKart.AttachAbility(Utilities.getAbility(Utilities.playerKartInformation[i].CurrentKart.Weapons[1]), KartSides.RIGHT);
                        tempKart.AttachAbility(new Pistol(), KartSides.FRONT);
                        karts.Add(tempKart);
                    }
                }
            }
            else
            {
                karts.Add(new Kart(Color.Blue, 0, 0.12f));
                karts.Add(new Kart(Color.Orange, 1, 0.12f));
                karts.Add(new Kart(Color.Green, 2, 0.12f));
                karts.Add(new Kart(Color.Turquoise, 3, 0.12f));
            }

            foreach (Kart kart in karts)
            {
                kart.LoadContent(Utilities.content, ScreenManager.Game.GraphicsDevice);
            }

            // stop the menu music
            MediaPlayer.Stop();

            // play the background music
            MediaPlayer.Volume = 0.25f;
            MediaPlayer.IsRepeating = true;

            MediaPlayer.Play(Utilities.backgroundTracks[Utilities.backgroundTrackIndex % Utilities.backgroundTracks.Count]);

            Utilities.backgroundTrackIndex++;

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            //Utilities.content.Unload();

            // stop playing the game background music
            MediaPlayer.Stop();

            MediaPlayer.Volume = 0.25f;
            MediaPlayer.IsRepeating = true;

            // play the menu music
            MediaPlayer.Play(Utilities.menuMusic);
        }


        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
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
                // We'll have to come up with a better way to do this eventually
                /* What should probably happen is the Update method in GameObject should be 
                 * made to pass a list of list of GameObjects and we can decide what to pass
                 * then within specific objects we can test what we might be colliding with and 
                 * then do whatever is appropriate.
                 * 
                 * That way we won't have to keep two lists of the same objects
                 */

                if (countdown > 0.0f)
                {
                    countdown -= gameTime.ElapsedGameTime.Milliseconds;
                    return;
                }

                timeLeft -= gameTime.ElapsedGameTime.Milliseconds;

                if (timeLeft < 0)
                {
                    Utilities.numRounds--;
                    Utilities.karts = new List<Kart>(karts);
                    //If time is up go through all the bases and add coins
                    List<Color> winningTeams = new List<Color>();
                    int winningAmount = -1;
                    for (int i = 0; i < Bases.Count; i++)
                    {
                        if (Bases[i].CoinAmount > winningAmount)
                        {
                            winningTeams.Clear();
                            winningTeams.Add(Bases[i].Team);
                            winningAmount = Bases[i].CoinAmount;
                        }
                        else if (Bases[i].CoinAmount == winningAmount)
                        {
                            winningTeams.Add(Bases[i].Team);
                        }
                        List<int> playerForBase = new List<int>();
                        for (int j = 0; j < 4; j++)
                        {
                            if (Utilities.playerKartInformation[j].Active && Utilities.playerKartInformation[j].Team.Equals(Bases[i].Team))
                            {
                                playerForBase.Add(j);
                            }
                        }
                        int coinAmount = Bases[i].CoinAmount / playerForBase.Count;
                        for (int j = 0; j < playerForBase.Count; j++)
                        {
                            Utilities.playerKartInformation[playerForBase[j]].Coins += coinAmount;
                        }
                    }

                    Utilities.winningTeams = new List<int>();
                    for (int i = 0; i < winningTeams.Count; i++)
                    {
                        if (winningTeams[i] == Color.Blue)
                        {
                            Utilities.roundsWon[0]++;
                            Utilities.winningTeams.Add(0);
                        }
                        else if (winningTeams[i] == Color.Turquoise)
                        {
                            Utilities.roundsWon[1]++;
                            Utilities.winningTeams.Add(1);
                        }
                        else if (winningTeams[i] == Color.Green)
                        {
                            Utilities.roundsWon[2]++;
                            Utilities.winningTeams.Add(2);
                        }
                        else if (winningTeams[i] == Color.Orange)
                        {
                            Utilities.roundsWon[3]++;
                            Utilities.winningTeams.Add(3);
                        }
                    }

                    if (Utilities.numRounds <= 0 || quickGame)
                    {
                        LoadingScreen.Load(ScreenManager, false, null, new ResultsScreen(true));
                    }
                    else
                    {
                        LoadingScreen.Load(ScreenManager, false, null, new ResultsScreen(false));
                    }
                    roundOver = true;
                    //LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());

                    // clear the booms list
                    // this should prevent seeing the poofs and explosions from the round before
                    // (I am doing this in the constructor now)
                    //Utilities.booms.Clear();
                }

                List<GameObject> objectsThatMakeKartsBounce = new List<GameObject>();
                foreach (Kart otherKart in karts)
                    objectsThatMakeKartsBounce.Add(otherKart);

                foreach (Wall wall in Map)
                {
                    objectsThatMakeKartsBounce.Add(wall);
                }

                foreach (Kart kart in karts)
                {

                    kart.Move(gameTime.ElapsedGameTime.Milliseconds, objectsThatMakeKartsBounce);
                    kart.Update(gameTime.ElapsedGameTime.Milliseconds);

                    if (kart.NumCoins > 0)
                    {
                        foreach (Base flag in Bases)
                        {
                            if (flag.Team.Equals(kart.Team))
                            {
                                if (kart.Collide(flag))
                                {
                                    //THIS IS WHERE WE WOULD ADD COINS TO THE SPECIFIC TEAM
                                    flag.CoinAmount += kart.NumCoins;
                                    kart.totalCoins += kart.NumCoins;
                                    kart.NumCoins = 0;

                                    // play a 'cha-ching!' sound
                                    flag.PlayDepositCoinSound();
                                }
                            }
                        }
                    }

                    Coins.AddRange(Utilities.coins);
                    Utilities.coins = new List<Coin>();
                    List<Coin> newCoins = new List<Coin>();
                    Coin[] cs = Coins.ToArray();
                    int numCoins = Coins.Count;
                    for (int i = 0; i < numCoins; i++)
                    {
                        Coin c = cs[i];
                        if (c.isSpawned)
                        {
                            if (kart.Collide(c))
                            {
                                c.isSpawned = false;
                                kart.NumCoins += c.worth;

                                // play a 'ding!' sound
                                c.PlayCollideSound();

                                // if it's a coin that came from a person's body, delete it
                                if (!c.Respawn)
                                    c = null;
                            }
                        }
                        if (c != null)
                            newCoins.Add(c);
                    }
                    Coins.Clear();
                    Coins = newCoins;
                    newCoins = null;
                    cs = null;

                    // attempt to activate each ability on the kart
                    foreach (Ability ability in kart.abilities)
                    {
                        if (ability == null)
                            continue;

                        // some abilities do useful stuff but still return null (the shields for example)
                        List<Projectile> projectiles_fired = ability.Activate(kart, gameTime.ElapsedGameTime.Milliseconds);

                        if (projectiles_fired != null)
                            projectiles.AddRange(projectiles_fired);
                    }
                }

                List<Projectile> newProjectiles = new List<Projectile>();
                Projectile[] ps = projectiles.ToArray();
                int numProjectiles = projectiles.Count;
                for (int i=0; i<numProjectiles; i++)
                {
                    Projectile p = ps[i];
                    p.Update(gameTime.ElapsedGameTime.Milliseconds);

                    // Projectile moved off screen
                    if (p.Position.X < -10.0f || p.Position.Y < -10.0f || p.Position.X > 1290.0f || p.Position.Y > 730.0f)
                        p = null;

                    else
                    {
                        if (p.GetType() == typeof(Mine))
                        {
                            if (((Mine)p).TimeLiving > ((Mine)p).LengthOfLife)
                            {
                                //if (((Mine)p).Team == Color.Blue)
                                //    Utilities.blueMinesInPlay--;
                                //else if (((Mine)p).Team == Color.Orange)
                                //    Utilities.orangeMinesInPlay--;
                                //else if (((Mine)p).Team == Color.Green)
                                //    Utilities.greenMinesInPlay--;
                                //else if (((Mine)p).Team == Color.Turquoise)
                                //    Utilities.turqoiseMinesInPlay--;
                                p = null;
                            }
                        }
                        else if (p.GetType() == typeof(OilSlick))
                        {
                            if (((OilSlick)p).TimeLiving > ((OilSlick)p).LengthOfLife)
                            {
                                //if (((OilSlick)p).Team == Color.Blue)
                                //    Utilities.blueOilSlicksInPlay--;
                                //else if (((OilSlick)p).Team == Color.Orange)
                                //    Utilities.orangeOilSlicksInPlay--;
                                //else if (((OilSlick)p).Team == Color.Green)
                                //    Utilities.greenOilSlicksInPlay--;
                                //else if (((OilSlick)p).Team == Color.Turquoise)
                                //    Utilities.turqoiseOilSlicksInPlay--;
                                p = null;
                            }
                        }
                        else if (p.GetType() == typeof(Shockwave))
                        {
                            if (((Shockwave)p).TimeLiving > ((Shockwave)p).LengthOfLife)
                                p = null;
                        }

                        if (p != null)
                        {
                            foreach (GameObject o in objectsThatMakeKartsBounce)
                            {
                                if (o.Collide(p))
                                {
                                    Utilities.booms.Add(p.GetNewBoom());
                                    p = null;
                                    break;
                                }
                            }
                        }
                    }
                    if (p != null)
                        newProjectiles.Add(p);
                }
                projectiles.Clear();
                projectiles = newProjectiles;
                newProjectiles = null;
                ps = null;

                foreach (Coin coin in Coins)
                {
                    coin.Move(gameTime.ElapsedGameTime.Milliseconds, Map);
                    coin.Update(gameTime.ElapsedGameTime.Milliseconds);
                }

                List<Boom> newBooms = new List<Boom>();
                Boom[] bs = Utilities.booms.ToArray();
                int numBooms = Utilities.booms.Count;
                for (int i = 0; i < numBooms; i++)
                {
                    Boom b = bs[i];
                    b.Update(gameTime.ElapsedGameTime.Milliseconds);
                    if (b.TimeToDie == true)
                        b = null;
                    if (b != null)
                        newBooms.Add(b);
                }
                Utilities.booms.Clear();
                Utilities.booms = newBooms;
                newBooms = null;
                bs = null;
                //GC.Collect();
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

            
            int playerIndex = 0;
            int currentIndex = 0;
            foreach (GamePadState controller in input.CurrentGamePadStates)
            {
                if (controller.IsConnected)
                {
                    if (quickGame || Utilities.playerKartInformation[playerIndex].Active)
                    {
                        karts[currentIndex].HandleInput(controller);
                        playerIndex++;
                        currentIndex++;
                    }
                    else
                    {
                        playerIndex++;
                    }
                }
            }

            if (input.IsPauseGame(null))
            {
                // I think this should be enough to stop the bug where the controllers keep vibrating
                GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
                GamePad.SetVibration(PlayerIndex.Two, 0.0f, 0.0f);
                GamePad.SetVibration(PlayerIndex.Three, 0.0f, 0.0f);
                GamePad.SetVibration(PlayerIndex.Four, 0.0f, 0.0f);

                ScreenManager.AddScreen(new PauseMenuScreen(), null);
                countdown = 3000;
                return;
            }

            for (int i = 0; i < karts.Count; i++)
            {
                PlayerIndex player;
                int index = karts[i].ID;
                if (index == 0)
                {
                    player = PlayerIndex.One;
                }
                else if (index == 1)
                {
                    player = PlayerIndex.Two;
                }
                else if (index == 2)
                {
                    player = PlayerIndex.Three;
                }
                else
                {
                    player = PlayerIndex.Four;
                }
                if (karts[i].wasColliding && countdown <= 0) // this keeps the controllers from vibrating when coming back from being paused
                {
                    GamePad.SetVibration(player, (float)1.0, (float)1.0);
                }
                else
                {
                    GamePad.SetVibration(player, (float)0.0, (float)0.0);
                }
            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.White, 0, 0);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            spriteBatch.Draw(Utilities.backgroundTexture, new Vector2(0.0f, 0.0f), Color.White);

            foreach (Wall wall in Map)
                wall.Draw(spriteBatch, gameTime.ElapsedGameTime.Milliseconds);

            foreach (Base flag in Bases)
                flag.Draw(spriteBatch, gameTime.ElapsedGameTime.Milliseconds);

            foreach (Coin coin in Coins)
                coin.Draw(spriteBatch, gameTime.ElapsedGameTime.Milliseconds);

            foreach (Kart kart in karts)
                kart.Draw(spriteBatch, gameTime.ElapsedGameTime.Milliseconds);

            foreach (Projectile projectile in projectiles)
                projectile.Draw(spriteBatch, gameTime.ElapsedGameTime.Milliseconds);

            foreach (Boom boom in Utilities.booms)
                boom.Draw(spriteBatch, gameTime.ElapsedGameTime.Milliseconds);
            /*
            float fps = 0.0f;
            int elapsed_millis = gameTime.ElapsedGameTime.Milliseconds;

            // this should prevent a weird divide by zero bug when you leave the game paused for a while then come back
            if (elapsed_millis != 0)
                fps = 1000 / elapsed_millis;

            spriteBatch.DrawString(Utilities.gameFont, fps.ToString(), new Vector2(0.0f, 0.0f), Color.Plum);
            */
            string seconds = ((timeLeft / 1000) % 60).ToString();
            if (seconds.Length == 1)
                seconds = "0" + seconds;
            string timeLeftStr = (timeLeft / 60000).ToString() + ":" + seconds; 

            Vector2 timeCenter = Utilities.menuFont.MeasureString(timeLeftStr);

            spriteBatch.DrawString(Utilities.menuFont, timeLeftStr, new Vector2((1280 / 2.0f) + 28, 50.0f), Color.DarkBlue, 0.0f, timeCenter, 1.0f, SpriteEffects.None, 0.0f);

            if (timeLeft/1000 < 6)
            {
                string countDownStr = (((timeLeft) / 1000)).ToString();
                if (timeLeft / 1000 == 0)
                {
                    countDownStr = "FINISH!";
                    spriteBatch.DrawString(Utilities.countdownFont, countDownStr, new Vector2(46.0f, 100.0f), Color.DarkBlue, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                }
                else
                {
                    timeCenter = Utilities.countdownFont.MeasureString(countDownStr);
                    spriteBatch.DrawString(Utilities.countdownFont, countDownStr, new Vector2((1280.0f / 2.0f) + 80.0f, 600.0f), Color.DarkBlue, 0.0f, timeCenter, 1.0f, SpriteEffects.None, 0.0f);
                }
            }

            if (IsActive)
            {
                if (countdown > 0)
                {
                    string countDownStr = (((countdown - 1) / 1000) + 1).ToString();
                    timeCenter = Utilities.countdownFont.MeasureString(countDownStr);
                    spriteBatch.DrawString(Utilities.countdownFont, countDownStr, new Vector2((1280 / 2.0f) + 80, 600.0f), Color.DarkBlue, 0.0f, timeCenter, 1.0f, SpriteEffects.None, 0.0f);
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


        #endregion
    }
}
