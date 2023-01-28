﻿using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacman.MazeGenerator;

namespace Pacman;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private readonly Point GridSize = new (8, 8);
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _mapManager = new MapManager.MapManager(GridSize);
    }

    protected override void Initialize()
    {
        Globals.Content = Content;
        _mapManager.Init();
        
        _graphics.PreferredBackBufferWidth = GridSize.X * ((int)_mapManager.TileSize.X * 2 + 1);
        _graphics.PreferredBackBufferHeight = GridSize.Y * ((int)_mapManager.TileSize.Y * 2 + 1);
        _graphics.ApplyChanges();


        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Globals.SpriteBatch = _spriteBatch;
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        Globals.Update(gameTime);
        _mapManager.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp, null, null, null,
            null);
        _mapManager.Draw();
        _spriteBatch.End();
        base.Draw(gameTime);
    }

    private MapManager.MapManager _mapManager;
}