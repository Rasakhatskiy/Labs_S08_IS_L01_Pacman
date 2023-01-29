using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pacman.MazeGenerator;

public class StackNode
{
    public StackNode(Node node, List<Wall> directions)
    {
        _node = node;
        _checkDir = directions;
    }

    public Node Node => _node;
    public bool CheckedAll => _indexChecked >= 4;

    public Wall NextDirection()
    {
        return _checkDir[_indexChecked++];
    }
    
    
    private Node _node;
    private List<Wall> _checkDir = new ();
    private int _indexChecked = 0;
}