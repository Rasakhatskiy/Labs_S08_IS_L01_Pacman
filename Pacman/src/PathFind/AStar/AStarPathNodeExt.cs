using System;
using Microsoft.Xna.Framework;

namespace Pacman.PathFind.AStar;

internal static class AStarPathNodeExt
{
    public static void Fill(this AStarPathNode[] buffer, AStarPathNode parent, Point target, bool greedy)
    {
        int i = 0;
        foreach (var (position, cost) in NeighboursTemplate)
        {
            var nodePosition = position + parent.Position;
            var traverseDistance = parent.TraverseDistance + cost;
            buffer[i++] = new AStarPathNode(nodePosition, target, traverseDistance, greedy);
        }
    }
    
    private static readonly (Point position, double cost)[] NeighboursTemplate = {
        (new Point(1, 0), 1),
        (new Point(0, 1), 1),
        (new Point(-1, 0), 1),
        (new Point(0, -1), 1),
        // (new Point(1, 1), Math.Sqrt(2)),
        // (new Point(1, -1), Math.Sqrt(2)),
        // (new Point(-1, 1), Math.Sqrt(2)),
        // (new Point(-1, -1), Math.Sqrt(2))
    };
    
}