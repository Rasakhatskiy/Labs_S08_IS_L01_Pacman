using System;

namespace Pacman.MazeGenerator;

[Flags]
public enum Wall
{
    N = 8,
    E = 4,
    S = 2,
    W = 1,
    
    None = 0,
    
    NE = N | E,
    NW = N | W,
    SE = S | E,
    SW = S | W,
    All = N | E | S | W,
    Visited = 16,
}