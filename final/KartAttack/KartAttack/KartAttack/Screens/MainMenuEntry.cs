#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace KartAttack
{
    class MainMenuEntry : MenuEntry
    {
        #region Fields
        double timeBetweenChange;
        double timeSinceChange;
        int currentFrame;
        int totalFrames;
        List<Texture2D> buttons;
        SpriteFont font;
        #endregion

        #region Initialization

        public MainMenuEntry(string text)
            : base(text)
        {
            timeBetweenChange = 0.3f;
            timeSinceChange = 0.0f;
            currentFrame = 0;
            totalFrames = 5;
            buttons = new List<Texture2D>();
        }

        #endregion

        public override void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            if (isSelected)
            {
                if (gameTime.TotalGameTime.TotalSeconds > (timeSinceChange + timeBetweenChange))
                {
                    timeSinceChange = gameTime.TotalGameTime.TotalSeconds;
                    currentFrame = (currentFrame + 1) % totalFrames;
                }
            }
        }

        public override void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            Color textureColor = (Color.White * screen.TransitionAlpha);

            // Draw the selected entry in yellow, otherwise white.
            Color color = isSelected ? Color.Blue : Color.Black;

            // Modify the alpha to fade text out during transitions.
            color *= screen.TransitionAlpha;

            // Draw text, centered on the middle of each line.
            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;

            Vector2 origin = new Vector2((font.MeasureString(Text).X/2), font.LineSpacing / 2);
            Vector2 textDrawLocation = new Vector2(Position.X + (buttons[0].Width/2), Position.Y + (buttons[0].Height/2));

            spriteBatch.Draw(buttons[currentFrame], Position, textureColor);
            spriteBatch.DrawString(font, Text, textDrawLocation, color, 0,
                                   origin, 1, SpriteEffects.None, 0);
        }

        public override int GetHeight(MenuScreen screen)
        {
            return buttons[0].Height;
        }

        public override int GetWidth(MenuScreen screen)
        {
            return buttons[0].Width;
        }

        public void LoadContent(ContentManager content)
        {
            buttons.Add(content.Load<Texture2D>("Menu\\MenuBox1"));
            buttons.Add(content.Load<Texture2D>("Menu\\MenuBox2"));
            buttons.Add(content.Load<Texture2D>("Menu\\MenuBox3"));
            buttons.Add(content.Load<Texture2D>("Menu\\MenuBox4"));
            buttons.Add(content.Load<Texture2D>("Menu\\MenuBox5"));
            font = content.Load<SpriteFont>("MainMenuEntries");
        }
    }
}
