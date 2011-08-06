using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace KartAttack
{
    class Base : GameObject
    {
        private SoundEffect DepositCoinSoundEffect;

        public Color Team;
        public int CoinAmount;
        public SpriteFont font;

        public Base(Vector2 position, Color team)
            : /*base(new Vector2(position.X + 15.0f, position.Y + 22.0f), 64, 64)*/ base(position, 64, 64, 0, 3)
        {
            Team = team;
            CoinAmount = 0;
        }

        public override void Update(float timeDelta)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch, int millisElapsed)
        {
            base.Draw(spriteBatch, millisElapsed);
            spriteBatch.DrawString(font, CoinAmount.ToString(),
                                   new Vector2(Position.X + 32 - (font.MeasureString(CoinAmount.ToString()).X / 2),
                                   Position.Y + 32 - (font.MeasureString(CoinAmount.ToString()).Y / 2)),
                                   Color.Black);
        }

        public void LoadContent(ContentManager content)
        {
            if (Team == Color.Blue)
            {
                base.LoadContent(content, "Bases\\BlueBase");
            }
            else if (Team == Color.Green)
            {
                base.LoadContent(content, "Bases\\GreenBase");
            }
            else if (Team == Color.Orange)
            {
                base.LoadContent(content, "Bases\\OrangeBase");
            }
            else
            {
                base.LoadContent(content, "Bases\\TurquoiseBase");
            }
            font = content.Load<SpriteFont>("menufont");

            DepositCoinSoundEffect = content.Load<SoundEffect>("SoundEffects\\coin_deposit_1");
        }

        public void PlayDepositCoinSound()
        {
            DepositCoinSoundEffect.Play();
        }
    }
}
