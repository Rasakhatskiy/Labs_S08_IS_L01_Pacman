using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pacman.MazeGenerator;

public class MazeGenerator
{
    public MazeGenerator(Point size, Point start, int seed = -1)
    {
        _size = size;
        Nodes = new Node[_size.X, _size.Y];
        for (int j = 0; j < _size.Y; j++)
        for (int i = 0; i < _size.X; i++)
            Nodes[i, j] = Node.NewAllWallsNode();

        _random = new Random();

        if (seed == -1)
            seed = _random.Next(0, 23);
        _seed = seed;


        Generate(start);
    }
   
    public readonly Node[,] Nodes;

    public int[,] GetArray()
    {
        var result = new int[
            _size.X * 2 + 1, 
            _size.Y * 2 + 1];
		
		
        for (int row = 0; row < _size.Y; ++row) {
            for (int col = 0; col < _size.X; ++col)
            {
                var v = Nodes[col,row].Value.HasFlag(Wall.N);
                result[2 * col, 2 * row] = 1;
                result[2 * col + 1, 2 * row] = v ? 1 : 0;
            }

            result[_size.X * 2, row * 2] = 1;

			
            for (int col = 0; col < _size.X; ++col)
            {
                var v = Nodes[col,row].Value.HasFlag(Wall.W);
                result[2 * col, 2 * row + 1] = v ? 1 : 0;
                result[2 * col + 1, 2 * row + 1] = 0;
            }

            result[_size.X * 2, row * 2 + 1] = 1;
        }

        for (int i = 0; i < _size.X * 2 + 1; i++)
            result[i, _size.Y * 2] = 1;

        var toDestroy = (int)(Math.Max(_size.X * 2, _size.Y * 2) * 1);

        for (int i = 0; i < toDestroy; i++)
        {
            var destroyed = false;
            while (!destroyed)
            {
                var x = _random.Next(1, _size.X * 2);
                var y = _random.Next(1, _size.Y * 2);
				
                if (result[x - 1, y - 1] == 0 &&
                    result[x - 1, y + 1] == 0 &&
                    result[x + 1, y - 1] == 0 &&
                    result[x + 1, y + 1] == 0)
                    continue;
				
                if (result[x, y] == 1)
                {
                    result[x, y] = 0;
                    destroyed = true;
                }
            }
        }
		
        return result;
    }
    
    private void Generate(Point start)
    {
        for (int j = 0; j < _size.Y; j++)
        for (int i = 0; i < _size.X; i++)
        {
            var node = Nodes[i, j];
            if (i > 0) 
                node.Neighbors.W = Nodes[i - 1, j];
            if (i < _size.X - 1)
                node.Neighbors.E = Nodes[i + 1, j];
            if (j > 0) 
                node.Neighbors.N = Nodes[i, j - 1];
            if (j < _size.Y - 1)
                node.Neighbors.S = Nodes[i, j + 1];
        }
        
        Stack<StackNode> stack = new ();
        _random = new(_seed);
        {
            var startNode = Nodes[start.X, start.Y];
            startNode.Visit();
            stack.Push(new StackNode(startNode, GenRandomDirectionOrder()));
        }
        
        while (stack.Count != 0)
        {
            var currentNode = stack.Peek();
            if (currentNode.CheckedAll)
            {
                stack.Pop();
                continue;
            }

            var keepChecking = true;
            while (keepChecking && !currentNode.CheckedAll)
            {
                var dir = currentNode.NextDirection();
                var nextNode = currentNode.Node.GetNeighborByDirection(dir);
                if (nextNode == null || nextNode.Visited)
                    continue;
				
                ClearWalls(currentNode.Node, nextNode, dir);
                nextNode.Visit();

                stack.Push(new StackNode(nextNode, GenRandomDirectionOrder()));
                keepChecking = false;
            }
        }
    }
    
    private void ClearWalls(Node from, Node to, Wall dir)
    {
        from.RemoveWall(dir);
        to.RemoveWall(GetOpposite(dir));
    }
    
    private Wall GetOpposite(Wall wall)
    {
        if (wall == Wall.E) return Wall.W;
        if (wall == Wall.W) return Wall.E;
        if (wall == Wall.N) return Wall.S;
        if (wall == Wall.S) return Wall.N;
        throw new Exception("Wall should be one sided");
    }
    
    private List<Wall> GenRandomDirectionOrder()
    {
        var available = new List<Wall> { Wall.E, Wall.N, Wall.W, Wall.S };
        var result = new List<Wall>();
        for (int i = 0; i < 3; i++)
        {
            var randDir = _random.Next(0, 4 - i);
            result.Add(available[randDir]);
            available.RemoveAt(randDir);
        }
        result.Add(available[0]);
        return result;
    }

    private Point _size;
    private int _seed;
    private Random _random;
}