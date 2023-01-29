using System;
using System.Collections.Generic;
using System.IO;
using BebraProject.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pacman.PathFind.AStar;

namespace Pacman.Entities;

public class Ghost
{
    public enum GhostColor
    {
        Red = 0, 
        Pink = 1, 
        Blue = 2, 
        Orange = 3
    }
    
    public Ghost(MapManager.MapManager mapManager, Pacman pacman, GhostColor color)
    {
        _mapManager = mapManager;
        _pacman = pacman;
        _spritePosition = mapManager.GetCentralEmptyPoint().ToVector2() * _tileSize;
        _color = color;
    }
    
    public void Init()
    {
        var spritesheetGhost = new Spritesheet("sprites", new Point(14, 10));

        _animationSet[Direction.E] = new Animation(spritesheetGhost, 0.05f, 4 + (int)_color, 0, 2);
        _animationSet[Direction.W] = new Animation(spritesheetGhost, 0.05f, 4 + (int)_color, 2, 2);
        _animationSet[Direction.N] = new Animation(spritesheetGhost, 0.05f, 4 + (int)_color, 4, 2);
        _animationSet[Direction.S] = new Animation(spritesheetGhost, 0.05f, 4 + (int)_color, 6, 2);
        
        var spritesheetPath = new Spritesheet("path_marker", new Point(3, 1));
        _pathMarker = new StillObject(spritesheetPath, new Vector2(0, 0));

    }
    
    public void Draw()
    {
        _animationSet[_direction].Draw(_spritePosition);
        foreach (var point in _pathStack)
            _pathMarker.Draw(point.ToVector2() * _tileSize);
    }
    
    public void Update()
    {
        UpdateControls();
        
        if (_move)
            UpdatePosition();
        
        if (_pacman.GridPositionChanged)
            UpdatePathfind();
        
        _animationSet[_direction].Update();
    }

    private void UpdatePathfind()
    {
        var pathfindAlgorithm = new AStarPath(_greedy);
        
        var startPoint = new Point(
            (int)Math.Round(_spritePosition.X / _tileSize.X),
            (int)Math.Round(_spritePosition.Y / _tileSize.Y));

        var destinationPoint = _pacman.GridPosition;

        pathfindAlgorithm.Calculate(
            startPoint,
            destinationPoint,
            _mapManager.Obstacles,
            out var resultList);
        
        _pathStack.Clear();
        if (resultList != null)
            _pathStack = resultList;
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
                if (_pathStack != null && _pathStack.Count != 0)
                    _pathStack.Pop();
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
         var gridPosition = new Point(
             (int)Math.Round(_spritePosition.X / _tileSize.X),
             (int)Math.Round(_spritePosition.Y / _tileSize.Y));

         if (_pathStack.Count != 0)
         {
             if (_pathStack.Peek() == gridPosition)
                 _pathStack.Pop();
         }
         
         var nextPos = _pathStack == null || _pathStack.Count == 0
             ? gridPosition
             : _pathStack.Peek();

         var diff = nextPos - gridPosition;

         if (diff.X < 0)
             _directionQueued = Direction.W;
         
         if (diff.X > 0)
             _directionQueued = Direction.E;
         
         if (diff.Y < 0)
             _directionQueued = Direction.N;
         
         if (diff.Y > 0)
             _directionQueued = Direction.S;

         if (Input.Keyboard.IsKeyPressedOnce(Keys.G))
         {
             _greedy = !_greedy;
             UpdatePathfind();
         }
         
         if (Input.Keyboard.IsKeyPressedOnce(Keys.M))
         {
             _move = !_move;
         }
     }
    

    
    private Dictionary<Direction, Animation> _animationSet = new();
    private StillObject _pathMarker;

    private Stack<Point> _pathStack = new();

    private Vector2 _spritePosition;
    private Vector2 _tileSize = new(16, 16);
    private Direction _direction = Direction.E;
    private Direction _directionQueued = Direction.E;
    private float Speed = 50f;
    private MapManager.MapManager _mapManager;
    private bool _stop = false;
    private GhostColor _color;
    private Pacman _pacman;
    private bool _greedy = false;
    private bool _move = true;

    private enum Direction
    {
        W,
        E,
        N,
        S
    }
    
}