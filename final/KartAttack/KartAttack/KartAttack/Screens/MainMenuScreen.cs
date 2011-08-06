#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
#endregion

namespace KartAttack
{
  /// <summary>
  /// The main menu screen is the first thing displayed when the game starts up.
  /// </summary>
  class MainMenuScreen : MenuScreen
  {

    #region Fields
      ContentManager content;
      SpriteFont font;
    #endregion

    #region Initialization


      /// <summary>
    /// Constructor fills in the menu contents.
    /// </summary>
    public MainMenuScreen() 
        : base("Kart Attack")
    {
      // Create our menu entries.
      MainMenuEntry playGameMenuEntry = new MainMenuEntry("Play Game");
      //MainMenuEntry optionsMenuEntry = new MainMenuEntry("Options");
      MainMenuEntry helpMenuEntry = new MainMenuEntry("Help");  
      MainMenuEntry exitMenuEntry = new MainMenuEntry("Exit");
      //MainMenuEntry menuTests = new MainMenuEntry("Instant Game");

      // Hook up menu event handlers.
      playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
      //optionsMenuEntry.Selected += OptionsMenuEntrySelected;
      helpMenuEntry.Selected += HelpMenuEntrySelected;
      exitMenuEntry.Selected += OnCancel;
      //menuTests.Selected += MenuTestEntrySelected;

      // Add entries to the menu.
      MenuEntries.Add(playGameMenuEntry);
      //MenuEntries.Add(optionsMenuEntry);
      MenuEntries.Add(helpMenuEntry);
      MenuEntries.Add(exitMenuEntry);
      //MenuEntries.Add(menuTests);
    }

    public override void LoadContent()
    {
        if (content == null)
        {
            content = new ContentManager(ScreenManager.Game.Services, "Content");
        }

        font = content.Load<SpriteFont>("Menu");

        foreach (MainMenuEntry entry in MenuEntries)
        {
            entry.LoadContent(content);
        }

        // set up the background music for the menus
        Utilities.menuMusic = content.Load<Song>("BackgroundMusic/menu_music_1");

        // stop any song playing before
        MediaPlayer.Stop();

        MediaPlayer.Volume = 0.25f;
        MediaPlayer.IsRepeating = true;

        MediaPlayer.Play(Utilities.menuMusic);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime, font);
    }


    #endregion

    #region Handle Input


    /// <summary>
    /// Event handler for when the Play Game menu entry is selected.
    /// </summary>
    void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
    {
        LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameSelectScreen());
    }


    /// <summary>
    /// Event handler for when the Options menu entry is selected.
    /// </summary>
    void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
    {
      ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
    }

    void HelpMenuEntrySelected(object sender, PlayerIndexEventArgs e)
    {
        ScreenManager.AddScreen(new HelpScreen(), e.PlayerIndex);
    }


    /// <summary>
    /// When the user cancels the main menu, ask if they want to exit
    /// </summary>
    protected override void OnCancel(PlayerIndex playerIndex)
    {
      const string message = "Are you sure you want to exit?";

      MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

      confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

      ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
    }

    /// <summary>
    /// Event handler for when the Play Game menu entry is selected.
    /// </summary>
    void MenuTestEntrySelected(object sender, PlayerIndexEventArgs e)
    {
        List<Color> active = new List<Color>();
        active.Add(Color.Blue);
        active.Add(Color.Black);
        active.Add(Color.Black);
        active.Add(Color.Black);
        LoadingScreen.Load(ScreenManager, false, null, new KartSelectMenu(this, true, active, true));
    }


    /// <summary>
    /// Event handler for when the user selects ok on the "are you sure
    /// you want to exit" message box.
    /// </summary>
    void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
    {
      ScreenManager.Game.Exit();
    }


    #endregion
  }
}
