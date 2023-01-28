using System.ComponentModel.DataAnnotations;

namespace Pacman.MazeGenerator;

public class Node
{
    public Node(Wall wall)
    {
        _node = wall;
    }
    
    public static Node NewAllWallsNode()
    {
        return new Node(Wall.All);
    }

    public void AddWall(Wall wall)
    {
        _node |= wall;
    }

    public void RemoveWall(Wall wall)
    {
        _node &= ~wall;
    }

    public void Visit()
    {
        _node |= Wall.Visited;
    }

    public bool Visited => _node.HasFlag(Wall.Visited);
    

    public Wall Value => _node;
    private Wall _node;
}