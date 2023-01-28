using System;
using BebraProject.Animation;
using Microsoft.Xna.Framework;

namespace Pacman.MapManager;

public class MapManager
{
    public int[,] Map;
    
    public MapManager(Point size)
    {
        var generator = new MazeGenerator.MazeGenerator(size, Point.Zero);
        _size = new Point(size.X * 2 + 1, size.Y * 2 + 1);

        Map = generator.GetArray();
    }


    public void Init()
    {
        _tile = new StillObject(new Spritesheet("tile", new Point(1, 1)), Vector2.Zero);
    }

    public void Update()
    {
        
    }

    public void Draw()
    {
        for (int j = 0; j < _size.Y; j++)
        {
            for (int i = 0; i < _size.X; i++)
            {
                if (Map[i,j] == 0)
                    continue;
                
                var position = new Vector2(i, j);
                _tile.Draw(position * TileSize);
            }
        }
    }

    public Point GetCentralEmptyPoint()
    {
        var midldePoint = new Point(_size.X / 2, _size.Y / 2);
        while (Map[midldePoint.X, midldePoint.Y] == 1)
        {
            midldePoint.Y--;
        }

        if (midldePoint.Y > _size.Y * 2)
            throw new Exception("LOH");

        return midldePoint;
    }
    
    private StillObject _tile;
    private readonly Point _size;
    public readonly Vector2 TileSize = new Vector2(16, 16);
}