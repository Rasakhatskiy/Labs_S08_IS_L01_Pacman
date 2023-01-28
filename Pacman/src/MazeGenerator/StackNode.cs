using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pacman.MazeGenerator;

public class StackNode
{
    public StackNode(Point coodrs, List<Wall> directions)
    {
        _position = coodrs;
        _checkDir = directions;
    }

    public Point Position => _position;
    public bool CheckedAll => _indexChecked >= 4;

    public Wall NextDirection()
    {
        return _checkDir[_indexChecked++];
    }
    
    
    private Point _position;
    private List<Wall> _checkDir = new ();
    private int _indexChecked = 0;
}