using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BebraProject.Slaves.AStar;

public interface IPath
{
    bool Calculate(Point start, Point destination, List<Point> obstacles, out Stack<Point> path);
}