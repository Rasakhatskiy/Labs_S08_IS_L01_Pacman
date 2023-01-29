using System;

namespace Pacman.MazeGenerator;

public class Node
{
    public struct NodeNeighbors
    {
        public Node W, N, E, S;
    }
    
    public Wall Value => _value;
    public NodeNeighbors Neighbors = new NodeNeighbors();
    
    public Node(Wall wall)
    {
        _value = wall;
    }
    
    public static Node NewAllWallsNode()
    {
        return new Node(Wall.All);
    }

    public void AddWall(Wall wall)
    {
        _value |= wall;
    }

    public void RemoveWall(Wall wall)
    {
        _value &= ~wall;
    }

    public void Visit()
    {
        _value |= Wall.Visited;
    }

    public Node GetNeighborByDirection(Wall wall)
    {
        switch (wall)
        {
            case Wall.N: return Neighbors.N;
            case Wall.E: return Neighbors.E;
            case Wall.S: return Neighbors.S;
            case Wall.W: return Neighbors.W;
            default:
                throw new ArgumentOutOfRangeException(nameof(wall), wall, null);
        }
    }
    
    public bool Visited => _value.HasFlag(Wall.Visited);
    
    private Wall _value;
}