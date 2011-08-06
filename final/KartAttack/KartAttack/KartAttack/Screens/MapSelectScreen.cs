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

    //TODO Get graphics for this screen and set up selection.
    class MapSelectScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont font;
        SpriteFont font2;

        GameScreen previousScreen;
        bool canGoBack;

        Texture2D cursorTexture;
        Vector2 currentPosition;
        List<Vector2> controllerDirections;
        Vector2 direction;
        bool movement;
        bool select;
        Rectangle cursorCollisionBox
        {
            get
            {
                return new Rectangle((int)currentPosition.X, (int)currentPosition.Y, 35, 35);
            }
        }

        Texture2D background;
        List<MapDisplayTile> displayTiles;

        float pauseAlpha;

        #endregion

        #region Initialization

        public MapSelectScreen(GameScreen PreviousScreen, bool CanGoBack) 
        {
            displayTiles = new List<MapDisplayTile>();
            controllerDirections = new List<Vector2>();
            for (int i = 0; i < 4; i++)
            {
                controllerDirections.Add(Vector2.Zero);
            }

            int mapNumX = 0;
            int mapNumY = 0;
            foreach (string file in Directory.GetFiles(Environment.CurrentDirectory))
            {
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Name.ToLower().Contains("map"))
                {
                    displayTiles.Add(new MapDisplayTile(fileInfo.Name, new Vector2(mapNumX * 175 + 30, mapNumY * 108 + 30)));
                    mapNumX++;
                    if(mapNumX > 2)
                    {
                        mapNumX = 0;
                        mapNumY++;
                    }
                }
            }

            previousScreen = PreviousScreen;
            canGoBack = CanGoBack;
            currentPosition = new Vector2(200, 200);
            movement = false;
            direction = new Vector2(1, 1);
            select = false;

            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            pauseAlpha = 0;
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            cursorTexture = (content.Load<Texture2D>("Menu//TempCursor"));

            background = content.Load<Texture2D>("KartAttackBackground");
            font = content.Load<SpriteFont>("Menu");
            font2 = content.Load<SpriteFont>("menufont");

            for(int i = 0; i < displayTiles.Count; i++)
            {
                displayTiles[i].LoadContent(content);
                //displayTiles[i].mapImage = content.Load<Texture2D>("Menu//TempMapImage");
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

            foreach (MapDisplayTile tile in displayTiles)
            {
                tile.Draw(spriteBatch);
                if (tile.selected)
                {
                    spriteBatch.DrawString(font, tile.mapName, new Vector2(600, 30), Color.Black);
                    spriteBatch.DrawString(font2, "Press Start To Play!", new Vector2(780, 640), Color.Black);
                    spriteBatch.Draw(tile.mapImage, new Rectangle(550, 150, 712, 400), Color.White);
                }
            }
            spriteBatch.Draw(cursorTexture, currentPosition, Color.Black);

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
                foreach (MapDisplayTile tile in displayTiles)
                {
                    tile.Update(gameTime);
                }

                if (movement)
                {
                    Vector2 newPosition = currentPosition + (direction * 6);
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
                    currentPosition = newPosition;
                }

                if (select)
                {
                    for (int i = 0; i < displayTiles.Count; i++)
                    {
                        displayTiles[i].selected = false;
                    }

                    for (int i = 0; i < displayTiles.Count; i++)
                    {
                        if (cursorCollisionBox.Intersects(displayTiles[i].collisionBox))
                        {
                            displayTiles[i].selected = true;
                            break;
                        }
                    }
                    select = false;
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

            if (canGoBack)
            {
                if (input.IsMenuCancel(null, out temp))
                {
                    LoadingScreen.Load(ScreenManager, false, null, previousScreen);
                }
            }

            movement = false;
            int playerIndex = 0;
            foreach (GamePadState controller in input.CurrentGamePadStates)
            {
                controllerDirections[playerIndex] = Vector2.Zero;
                if (controller.IsConnected)
                {
                    select = (input.LastGamePadStates[playerIndex].IsButtonDown(Buttons.A) && input.CurrentGamePadStates[playerIndex].IsButtonUp(Buttons.A) || select);
                    if (select)
                    {
                        Console.WriteLine("Select");
                    }

                    //Connect Select to Checking for collision with cursor and Box Rectangles. Then set appropriate bools
                    if (controller.ThumbSticks.Left.X != 0.0f || controller.ThumbSticks.Left.Y != 0.0f)
                    {
                        controllerDirections[playerIndex] = new Vector2(controller.ThumbSticks.Left.X, -controller.ThumbSticks.Left.Y);
                        movement = true;
                    }

                    foreach (MapDisplayTile tile in displayTiles)
                    {
                        if (tile.selected)
                        {
                            if ((input.LastGamePadStates[playerIndex].IsButtonDown(Buttons.Start) && input.CurrentGamePadStates[playerIndex].IsButtonUp(Buttons.Start)))
                            {
                                LoadingScreen.Load(ScreenManager, false, null, new GameplayScreen(tile.mapFileName, false));
                            }
                            break;
                        }
                    }
                }
                playerIndex++;
            }
            direction = Vector2.Zero;
            foreach (Vector2 vect in controllerDirections)
            {
                direction += vect;
            }
            if (direction.X > 1)
            {
                direction.X = 1;
            }
            else if (direction.X < -1)
            {
                direction.X = -1;
            }

            if (direction.Y > 1)
            {
                direction.Y = 1;
            }
            else if (direction.Y < -1)
            {
                direction.Y = -1;
            }
        }

        #endregion
    }
}
