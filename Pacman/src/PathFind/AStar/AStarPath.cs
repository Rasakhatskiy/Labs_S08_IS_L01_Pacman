using System;
using System.Collections.Generic;
using BebraProject.Slaves.AStar;
using Microsoft.Xna.Framework;

namespace Pacman.PathFind.AStar;

    public class AStarPath : IPath
    {
        public AStarPath(bool greedy = false, int maxSteps = int.MaxValue, int initialCapacity = 0)
        {
            if (maxSteps <= 0) throw new ArgumentOutOfRangeException(nameof(maxSteps));
            if (initialCapacity < 0) throw new ArgumentOutOfRangeException(nameof(initialCapacity));

            _maxSteps = maxSteps;
            _greedy = greedy;
            _frontier = new BinaryHeap<Point, AStarPathNode>(a => a.Position, initialCapacity);
            _ignoredPositions = new HashSet<Point>(initialCapacity);
            _output = new List<Point>(initialCapacity);
            _links = new Dictionary<Point, Point>(initialCapacity);
        }
        
        public bool Calculate(Point start, Point destination, List<Point> obstacles, out Stack<Point> path)
        {
            if (obstacles == null) throw new ArgumentNullException(nameof(obstacles));

            // if no path exists
            if (!GenerateNodes(start, destination, obstacles))
            {
                path = null;
                return false;
            }

            _output.Clear();
            _output.Add(destination);

            
            // go from end to start and write our path
            while (_links.TryGetValue(destination, out destination)) 
                _output.Add(destination);

            path = new ();
            foreach (var point in _output)
                path.Push(point);
            
            return true;
        }

        private bool GenerateNodes(Point start, Point target, IReadOnlyCollection<Point> obstacles)
        {
            _frontier.Clear();
            _ignoredPositions.Clear();
            _links.Clear();

            _frontier.Enqueue(new AStarPathNode(start, target, 0, _greedy));
            _ignoredPositions.UnionWith(obstacles);
            var step = 0;
            while (_frontier.Count > 0 && step++ <= _maxSteps)
            {
                var currentNode = _frontier.Dequeue();
                _ignoredPositions.Add(currentNode.Position);

                if (currentNode.Position.Equals(target)) 
                    return true;

                GenerateFrontierNodes(currentNode, target);
            }
            
            return false;
        }

        private void GenerateFrontierNodes(AStarPathNode parent, Point target)
        {
            _neighbours.Fill(parent, target, _greedy);
            foreach (AStarPathNode newNode in _neighbours)
            {
                // Position is already checked or occupied by an obstacle.
                if (_ignoredPositions.Contains(newNode.Position)) 
                    continue;

                // Node is not present in queue.
                if (!_frontier.TryGet(newNode.Position, out AStarPathNode existingNode))
                {
                    _frontier.Enqueue(newNode);
                    _links[newNode.Position] = parent.Position;
                }

                // Node is present in queue and new optimal path is detected.
                else if (newNode.TraverseDistance < existingNode.TraverseDistance)
                {
                    _frontier.Modify(newNode);
                    _links[newNode.Position] = parent.Position;
                }
            }
        }
        
        private const int MaxNeighbours = 8;
        private readonly AStarPathNode[] _neighbours = new AStarPathNode[MaxNeighbours];
        
        private readonly int _maxSteps;
        private readonly IBinaryHeap<Point, AStarPathNode> _frontier;
        private readonly HashSet<Point> _ignoredPositions;
        private readonly List<Point> _output;
        private readonly IDictionary<Point, Point> _links;
        private readonly bool _greedy;
    }