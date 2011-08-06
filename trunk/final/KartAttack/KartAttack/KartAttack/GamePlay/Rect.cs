using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KartAttack
{
  class Rect
  {
    private float x;
    public float X
    {
      get { return x; }
      set
      {
        x = value;
        Left = x;
        Right = X + Width;
      }
    }

    private float y;
    public float Y
    {
      get { return y; }
      set
      {
        y = value;
        Top = y;
        Bottom = Y + Height;
      }
    }

    public float Width
    {
      get;
      private set;
    }
    public float Height
    {
      get;
      private set;
    }
    public float Right
    {
      get;
      private set;
    }
    public float Top
    {
      get;
      private set;
    }
    public float Bottom
    {
      get;
      private set;
    }
    public float Left
    {
      get;
      private set;
    }

    public Rect(float x_, float y_, float width_, float height_)
    {
      Width = width_;
      Height = height_;
      X = x_;
      Y = y_;
    }

    public Rect(Rect r)
    {

    }
  }
}
