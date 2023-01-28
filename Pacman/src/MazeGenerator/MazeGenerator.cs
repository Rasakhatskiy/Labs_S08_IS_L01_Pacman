using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pacman.MazeGenerator;

public class MazeGenerator
{
	public Point Size => _size;
	
	public MazeGenerator(Point size, Point start, int seed = -1)
	{
		_size = size;
		var totalNodes = _size.X * _size.Y;
		for (int i = 0; i < totalNodes; i++)
			_nodes.Add(Node.NewAllWallsNode());

		_random = new();
		
		if (seed == -1)
			seed = _random.Next(0, 23);
		_seed = seed;


		Generate(start);

	}

	public Node GetNode(Point position)
	{
		return _nodes[position.X + position.Y * _size.Y];
	}

	public int[,] GetArray()
	{
		var result = new int[
			_size.X * 2 + 1, 
			_size.Y * 2 + 1];
		
		
		for (int row = 0; row < _size.Y; ++row) {
			for (int col = 0; col < _size.X; ++col)
			{
				var v = GetNode(new Point(col, row)).Value.HasFlag(Wall.N);
				result[2 * col, 2 * row] = 1;
				result[2 * col + 1, 2 * row] = v ? 1 : 0;
			}

			result[_size.X * 2, row * 2] = 1;

			
			for (int col = 0; col < _size.X; ++col)
			{
				var v = GetNode(new Point(col, row)).Value.HasFlag(Wall.W);
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
		Stack<StackNode> stack = new ();
		_random = new(_seed);
		GetNode(start).Visit();
		stack.Push(new StackNode(start, GenRandomDirectionOrder()));

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
				var nextPosition = PositionInDir(currentNode.Position, dir);
				
				if (!IsValidPosition(nextPosition))
					continue;

				var nextNode = GetNode(nextPosition);
				if (nextNode.Visited)
					continue;
				
				ClearWalls(currentNode.Position, nextPosition, dir);
				nextNode.Visit();

				stack.Push(new StackNode(nextPosition, GenRandomDirectionOrder()));
				keepChecking = false;
			}
		}
	}

	private void ClearWalls(Point from, Point to, Wall dir)
	{
		GetNode(from).RemoveWall(dir);
		GetNode(to).RemoveWall(GetOpposite(dir));
	}

	private Point PositionInDir(Point currentNodePosition, Wall dir)
	{
		var result = currentNodePosition;
		if (dir.HasFlag(Wall.W)) result.X--;
		if (dir.HasFlag(Wall.E)) result.X++;
		if (dir.HasFlag(Wall.N)) result.Y--;
		if (dir.HasFlag(Wall.S)) result.Y++;
		return result;
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

	private bool IsValidPosition(Point position)
	{
		return
			position.X >= 0 &&
			position.Y >= 0 &&
			position.X < _size.X &&
			position.Y < _size.Y;
	}

	private Wall GetOpposite(Wall wall)
	{
		if (wall == Wall.E) return Wall.W;
		if (wall == Wall.W) return Wall.E;
		if (wall == Wall.N) return Wall.S;
		if (wall == Wall.S) return Wall.N;
		throw new Exception("Wall should be one sided");
	}
	

	private Point _size;
	private int _seed;
	private List<Node> _nodes = new ();
	private Random _random;
}