using System;
using System.Collections.Generic;
using System.Diagnostics;
using BebraProject.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pacman.Entities;

public class Pacman
{
    public Pacman(MapManager.MapManager mapManager)
    {
        _mapManager = mapManager;
        _spritePosition = mapManager.GetCentralEmptyPoint().ToVector2() * _tileSize;
    }

    public void Init()
    {
        var spritesheet = new Spritesheet("sprites", new Point(14, 10));
        _animationSet[Direction.E] = new Animation(spritesheet, 0.05f, 0, 0, 4);
        _animationSet[Direction.W] = new Animation(spritesheet, 0.05f, 1, 0, 4);
        _animationSet[Direction.N] = new Animation(spritesheet, 0.05f, 2, 0, 4);
        _animationSet[Direction.S] = new Animation(spritesheet, 0.05f, 3, 0, 4);
    }

    public void Draw()
    {
        _animationSet[_direction].Draw(_spritePosition);
    }

    public void Update()
    {
        UpdateControls();
        UpdatePosition();
        _animationSet[_direction].Update();
    }

    private void UpdatePosition()
    {
        var GridPosition = new Point(
            (int)Math.Round(_spritePosition.X / _tileSize.X),
            (int)Math.Round(_spritePosition.Y / _tileSize.Y));
        var add = Speed * Globals.TotalSeconds;


        if (_direction is Direction.E or Direction.W)
        {
            if (_lastDirection is Direction.N or Direction.S)
                GridPosition.Y = (int)Math.Round(_spritePosition.Y / _tileSize.Y);
            _spritePosition.Y = GridPosition.Y * _tileSize.Y;
        } 
        if (_direction is Direction.N or Direction.S)
        {
            if (_lastDirection is Direction.E or Direction.W)
                GridPosition.X = (int)Math.Round(_spritePosition.X / _tileSize.X);
            _spritePosition.X = GridPosition.X * _tileSize.X;
        } 
        
        
        switch (_direction)
        {
            case Direction.W:
                GridPosition.X = (int)Math.Ceiling(_spritePosition.X / _tileSize.X);
                if (_mapManager.Map[GridPosition.X - 1, GridPosition.Y] == 0)
                    _spritePosition.X -= add;
                break;
            case Direction.E:
                GridPosition.X = (int)Math.Floor(_spritePosition.X / _tileSize.X);
                if (_mapManager.Map[GridPosition.X + 1, GridPosition.Y] == 0)
                    _spritePosition.X += add;
                break;
            case Direction.N:
                GridPosition.Y = (int)Math.Ceiling(_spritePosition.Y / _tileSize.Y);

                if (_mapManager.Map[GridPosition.X, GridPosition.Y - 1] == 0)
                    _spritePosition.Y -= add;
                break;
            case Direction.S:
                GridPosition.Y = (int)Math.Floor(_spritePosition.Y / _tileSize.Y);
                if (_mapManager.Map[GridPosition.X, GridPosition.Y + 1] == 0)
                    _spritePosition.Y += add;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateControls()
    {
        _lastDirection = _direction;
        if (Input.Keyboard.IsKeyPressedOnce(Keys.A)) _direction = Direction.W;
        if (Input.Keyboard.IsKeyPressedOnce(Keys.D)) _direction = Direction.E;
        if (Input.Keyboard.IsKeyPressedOnce(Keys.W)) _direction = Direction.N;
        if (Input.Keyboard.IsKeyPressedOnce(Keys.S)) _direction = Direction.S;
    }
    

    private Vector2 _spritePosition;
    private Dictionary<Direction, Animation> _animationSet = new();
    private readonly Vector2 _tileSize = new(16, 16);
    private Direction _direction = Direction.E;
    private Direction _lastDirection = Direction.E;
    private float Speed = 100f;
    private MapManager.MapManager _mapManager;

    private enum Direction
    {
        W,
        E,
        N,
        S
    }
}