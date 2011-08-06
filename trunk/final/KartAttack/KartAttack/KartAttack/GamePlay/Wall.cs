using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace KartAttack
{
  enum WallOutline { OneSided, TwoOppositeSides, Corner, ThreeSided, FourSided };

  class Wall : GameObject
  {
    public string WallType { get; private set; }
    
    private List<KeyValuePair<Point, WallOutline> > sections;

    private int numberOfWallsX;
    private int numberOfWallsY;
    private Vector2 upperLeft;

    private int sizeOfWallSegment;

    private List<List<int>> wallNum;

    public Wall(string wallType, int numberOfWallsXDirection, int numberOfWallsYDirection, int size, Vector2 position)
      //: base(new Vector2(position.X + 15.0f, position.Y + 22.0f), size * numberOfWallsXDirection, size * numberOfWallsYDirection) // worked better for cars
      //: base(new Vector2(position.X + 16.0f, position.Y + 16.0f), size * numberOfWallsXDirection, size * numberOfWallsYDirection) // makes more sense
      : base(new Vector2(position.X, position.Y), size * numberOfWallsXDirection, size * numberOfWallsYDirection, 0, 5)
    {
      WallType = wallType;
      numberOfWallsX = numberOfWallsXDirection;
      numberOfWallsY = numberOfWallsYDirection;
      sizeOfWallSegment = size;
      upperLeft = position;
      wallNum = new List<List<int>>();
    }

    public void LoadContent(ContentManager content)
    {
        base.LoadContent(content, "Walls\\wall_debug");
        for (int i = 0; i < numberOfWallsX; i++)
        {
            wallNum.Add(new List<int>());
            for (int j = 0; j < numberOfWallsY; j++)
            {
                wallNum[i].Add(Utilities.random.Next(0, AnimationTextures.Count - 1));
            }
        }
    }

    public override void Update(float timeDelta)
    {

    }

    public override void Draw(SpriteBatch spriteBatch, int millisElapsed)
    {
        //spriteBatch.Draw(Texture, Bounds.CollisionRectangle, Color.White);
        //spriteBatch.Draw(Texture, Position, Color.White);
        //Rectangle aAdjusted = new Rectangle(Bounds.X + (Bounds.Width / 2), Bounds.Y + (Bounds.Height / 2), Bounds.Width, Bounds.Height);
        //spriteBatch.Draw(Texture, aAdjusted, new Rectangle(0, 0, 2, 6), Color.White, Bounds.Rotation, new Vector2(2 / 2, 6 / 2), SpriteEffects.None, 0);
        //spriteBatch.Draw(Texture, Position, null, Color.White, Angle, new Vector2(0.0f, 0.0f)/*Bounds.Origin*/, 1.0f, SpriteEffects.None, 0);
        //spriteBatch.Draw(Texture, Position, null, Color.White, Angle, Bounds.Origin, 1.0f, SpriteEffects.None, 0);
        //return;

        Random random = new Random();

        if (WallType.Equals("Wall"))
        {
          //int beginX, endX, beginY, endY; // these are the beginning and ending positions of the sprites
          //if (numberOfWallsX % 2 == 0) // even # of walls in x direction
          
            for (int i = 0; i < numberOfWallsX; i++)
            {
              for (int j = 0; j < numberOfWallsY; j++)
              {
                  spriteBatch.Draw(AnimationTextures[wallNum[i][j]], new Vector2(upperLeft.X + (i * 32) + 16.0f, upperLeft.Y + (j * 32) + 16.0f), null, Color.White, Angle, new Vector2(16.0f, 16.0f), 1.0f, SpriteEffects.None, 0);
                //spriteBatch.Draw(Texture, Position, null, Color.White, Angle, Bounds.Origin, 1.0f, SpriteEffects.None, 0);
              }
            }
        }
    }
  }
}