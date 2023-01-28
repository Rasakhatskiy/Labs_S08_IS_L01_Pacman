using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
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

        var dir = Point.Zero;
        var dirQueued = Point.Zero;

        switch (_direction)
        {
            case Direction.W: GridPosition.X = (int)Math.Ceiling(_spritePosition.X / _tileSize.X); dir.X = -1; break;
            case Direction.E: GridPosition.X = (int)Math.Floor(_spritePosition.X / _tileSize.X); dir.X = 1; break;
            case Direction.N: GridPosition.Y = (int)Math.Ceiling(_spritePosition.Y / _tileSize.Y); dir.Y = -1; break;
            case Direction.S: GridPosition.Y = (int)Math.Floor(_spritePosition.Y / _tileSize.Y); dir.Y = 1; break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        switch (_directionQueued)
        {
            case Direction.W: dirQueued.X = -1; break;
            case Direction.E: dirQueued.X = 1; break;
            case Direction.N: dirQueued.Y = -1; break;
            case Direction.S: dirQueued.Y = 1; break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        var centered = false;
        {
            var center = GridPosition.ToVector2() * _tileSize + _tileSize / 2;
            var position = _spritePosition + _tileSize / 2;
            var e = new Vector2(2, 2);
            var diff = AbsV2(position - center);
            if (V2IsLessThanV2(diff, e))
                centered = true;
        }

        if (centered && _mapManager.Map[GridPosition.X + dir.X, GridPosition.Y + dir.Y] == 1)
        {
            _stop = true;
        }

        if (centered && _direction != _directionQueued)
        {
            if (_mapManager.Map[GridPosition.X + dirQueued.X, GridPosition.Y + dirQueued.Y] == 0)
            {
                _direction = _directionQueued;
                _stop = false;
                return;
            }
        }
        
        if (!_stop)
        {
            _spritePosition += dir.ToVector2() * add;
        }
    }

    private Vector2 AbsV2(Vector2 v2)
    {
        if (v2.X < 0) v2.X = -v2.X;
        if (v2.Y < 0) v2.Y = -v2.Y;
        return v2;
    }

    private bool V2IsLessThanV2(Vector2 a, Vector2 b)
    {
        return a.X < b.X && a.Y < b.Y;
    }
    
    private void UpdateControls()
    {
        if (Input.Keyboard.IsKeyPressedOnce(Keys.A)) _directionQueued = Direction.W;
        if (Input.Keyboard.IsKeyPressedOnce(Keys.D)) _directionQueued = Direction.E;
        if (Input.Keyboard.IsKeyPressedOnce(Keys.W)) _directionQueued = Direction.N;
        if (Input.Keyboard.IsKeyPressedOnce(Keys.S)) _directionQueued = Direction.S;
    }
    

    private Vector2 _spritePosition;
    private Dictionary<Direction, Animation> _animationSet = new();
    private readonly Vector2 _tileSize = new(16, 16);
    private Direction _direction = Direction.E;
    private Direction _directionQueued = Direction.E;
    private float Speed = 100f;
    private MapManager.MapManager _mapManager;
    private bool _stop = false;

    private enum Direction
    {
        W,
        E,
        N,
        S
    }
}