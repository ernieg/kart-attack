#region File Description
//-----------------------------------------------------------------------------
// Game.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace KartAttack
{
  /// <summary>
  /// Sample showing how to manage different game states, with transitions
  /// between menu screens, a loading screen, the game itself, and a pause
  /// menu. This main game class is extremely simple: all the interesting
  /// stuff happens in the ScreenManager component.
  /// </summary>
  public class KartAttackGame : Microsoft.Xna.Framework.Game
  {
    #region Fields

    GraphicsDeviceManager graphics;
    ScreenManager screenManager;


    // By preloading any assets used by UI rendering, we avoid framerate glitches
    // when they suddenly need to be loaded in the middle of a menu transition.
    static readonly string[] preloadAssets =
        {
            "gradient",
        };


    #endregion

    #region Initialization


    /// <summary>
    /// The main game constructor.
    /// </summary>
    public KartAttackGame()
    {
      Content.RootDirectory = "Content";

      graphics = new GraphicsDeviceManager(this);
      graphics.PreferredBackBufferWidth = 1280;
      graphics.PreferredBackBufferHeight = 720;
      graphics.IsFullScreen = true;

      // Create the screen manager component.
      screenManager = new ScreenManager(this);

      Components.Add(screenManager);

      // Activate the first screens.
      screenManager.AddScreen(new BackgroundScreen(), null);
      screenManager.AddScreen(new MainMenuScreen(), null);
    }


    /// <summary>
    /// Loads graphics content.
    /// </summary>
    protected override void LoadContent()
    {
      foreach (string asset in preloadAssets)
      {
        Content.Load<object>(asset);
      }
    }


    #endregion

    #region Draw


    /// <summary>
    /// This is called when the game should Draw itself.
    /// </summary>
    protected override void Draw(GameTime gameTime)
    {
      graphics.GraphicsDevice.Clear(Color.Black);

      // The real Drawing happens inside the screen manager component.
      base.Draw(gameTime);
    }


    #endregion
  }


  #region Entry Point

  /// <summary>
  /// The main entry point for the application.
  /// </summary>
  static class Program
  {
    static void Main()
    {
      using (KartAttackGame game = new KartAttackGame())
      {
        game.Run();
      }
    }
  }

  #endregion
}
