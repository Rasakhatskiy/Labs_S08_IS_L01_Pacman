using Microsoft.Xna.Framework;

namespace Pacman;

public class MapManager
{
    public int[,] Map;
    public Point Size;
    
    public MapManager(Point size)
    {
        Size = size;
        // MazeGenerator mazeGenerator = new();
        // Map = mazeGenerator.Generate();
    }
}