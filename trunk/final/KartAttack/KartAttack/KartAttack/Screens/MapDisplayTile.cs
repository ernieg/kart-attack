using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace KartAttack
{
    class MapDisplayTile
    {
        public Vector2 position;
        public Rectangle collisionBox
        {
            get
            {
                return new Rectangle((int)position.X + 5, (int)position.Y + 5, 142, 80);
            }
        }

        public Texture2D mapImage;
        public List<Texture2D> borderImages;
        public string mapName;
        public string mapFileName;
        public bool selected;
        int frame;
        int totalFrames;
        float timeLastUpdated;
        float timeToWaitForFrame;

        public MapDisplayTile(string name, Vector2 location)
        {
            mapFileName = name;
            position = location;
            string fileName = name.Split('.')[0];
            mapName = fileName.Remove(fileName.ToLower().IndexOf("map")).Trim();
            selected = false;
            frame = 0;
            totalFrames = 3;
            timeLastUpdated = 0.0f;
            timeToWaitForFrame = 300.0f;
            borderImages = new List<Texture2D>();
        }

        public void LoadContent(ContentManager content)
        {
            for (int i = 0; i < 3; i++)
            {
                borderImages.Add(content.Load<Texture2D>("Menu//MapBox" + (i + 1).ToString()));
            }
            try
            {
                mapImage = content.Load<Texture2D>("Menu//" + mapName);
            }
            catch(Exception)
            {
                mapImage = content.Load<Texture2D>("Menu//Basic");
            }
        }

        public void Update(GameTime gameTime)
        {
            if (selected)
            {
                timeLastUpdated += gameTime.ElapsedGameTime.Milliseconds;
                if (timeLastUpdated > timeToWaitForFrame)
                {
                    frame = (frame + 1) % totalFrames;
                    timeLastUpdated = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mapImage, new Rectangle((int)(position.X + 13), (int)(position.Y + 15), 122, 58), Color.White); 
            spriteBatch.Draw(borderImages[frame], position, Color.White);
        }
    }
}
