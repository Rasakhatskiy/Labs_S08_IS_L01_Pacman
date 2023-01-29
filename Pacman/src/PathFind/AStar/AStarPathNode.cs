using System;
using Microsoft.Xna.Framework;

namespace Pacman.PathFind.AStar;

internal readonly struct AStarPathNode : IComparable<AStarPathNode>
{
    public Point Position { get; }
    public double TraverseDistance { get; }

    public int CompareTo(AStarPathNode other) => _estimatedTotalCost.CompareTo(other._estimatedTotalCost);
    
    public AStarPathNode(Point position, Point target, double traverseDistance, bool greedy)
    {
        Position = position;
        if (greedy)
            traverseDistance = 0;
        TraverseDistance = traverseDistance;
        var heuristicDistance = DistanceEstimate(position - target);
        _estimatedTotalCost = traverseDistance + heuristicDistance;
    }
    
    
    
    private static double DistanceEstimate(Point p)
    {
        var linearSteps = Math.Abs(Math.Abs(p.Y) - Math.Abs(p.X));
        var diagonalSteps = Math.Max(Math.Abs(p.Y), Math.Abs(p.X)) - linearSteps;
        return linearSteps + Sqr * diagonalSteps;
    }
    
    private readonly double _estimatedTotalCost;
    private static readonly double Sqr = Math.Sqrt(2);
}