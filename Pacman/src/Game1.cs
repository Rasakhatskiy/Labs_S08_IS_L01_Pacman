using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacman.Entities;
using Pacman.Input;
using Pacman.MazeGenerator;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;

namespace Pacman;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private readonly Point GridSize = new (8, 8);
    private readonly Point UIScale = new (2, 2);
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _mapManager = new MapManager.MapManager(GridSize);
        _pacman = new Entities.Pacman(_mapManager);
        _ghost = new Ghost(_mapManager, _pacman, Ghost.GhostColor.Red);
    }

    protected override void Initialize()
    {
        Globals.Content = Content;
        _mapManager.Init();
        _pacman.Init();
        _ghost.Init();
        _graphics.PreferredBackBufferWidth = GridSize.X * ((int)_mapManager.TileSize.X * 2 + 1) * UIScale.X;
        _graphics.PreferredBackBufferHeight = GridSize.Y * ((int)_mapManager.TileSize.Y * 2 + 1) * UIScale.Y;
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
        InputManager.Update();
        _mapManager.Update();
        _pacman.Update();
        _ghost.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        var matrix = Matrix.CreateScale(UIScale.X, UIScale.Y, 1.0f);
        _spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp, null, null, null, matrix);
        _mapManager.Draw();
        _pacman.Draw();
        _ghost.Draw();
        _spriteBatch.End();
        base.Draw(gameTime);
    }

    private MapManager.MapManager _mapManager;
    private Entities.Pacman _pacman;
    private Entities.Ghost _ghost;
}