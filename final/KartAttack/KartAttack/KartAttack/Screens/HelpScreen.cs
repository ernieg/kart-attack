using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KartAttack
{
    class HelpScreen : GameScreen
    {
        ContentManager content;

        Texture2D backgroundTexture;

        float pauseAlpha;

        public HelpScreen()
        {
            pauseAlpha = 0.0f;

            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            backgroundTexture = content.Load<Texture2D>("HelpScreen");
            
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.White, 0, 0);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            // draw helper image
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);

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

            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);



        }

        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            PlayerIndex temp = new PlayerIndex();
            if (input.IsMenuCancel(null, out temp))
            {
                LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
            }

        }

    }
}
