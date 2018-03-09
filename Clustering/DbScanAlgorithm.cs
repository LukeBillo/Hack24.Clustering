using System;
using System.Collections.Generic;
using System.Linq;
using Clustering.Models;

namespace Clustering
{
    public class DbScanAlgorithm
    {
        public static int EarthApproximateRadius = 6731;
        private List<Cluster> _clusters;
        private List<Node> _noise;
        private List<Node> _visitedNodes;
        private readonly List<Node> _nodesToCluster;
        private readonly int _minClusterSize;
        private readonly int _maxClusterSize;
        private readonly double _maxDistanceBetweenNodes;

        public DbScanAlgorithm(List<Node> nodesToCluster, double maxDistanceBetweenNodes = double.PositiveInfinity, int minClusterSize = 1, int maxClusterSize = int.MaxValue)
        {
            _nodesToCluster = nodesToCluster;
            _minClusterSize = minClusterSize;
            _maxClusterSize = maxClusterSize;
            _maxDistanceBetweenNodes = maxDistanceBetweenNodes;
        }

        public DbScanResult Execute()
        {
            _clusters = new List<Cluster>();
            _noise = new List<Node>();
            _visitedNodes = new List<Node>();

            foreach (var node in _nodesToCluster)
            {
                if (HasNodeBeenVisited(node))
                    continue;

                var nearbyNodes = FindNearbyNodes(node);
                if (nearbyNodes.Count >= _minClusterSize)
                {
                    var cluster = PopulateAndExpandCluster(node, nearbyNodes);
                    _clusters.Add(cluster);
                }
                else
                {
                    _noise.Add(node);
                }
            }

            return new DbScanResult { Clusters = _clusters, Noise = _noise };
        }

        private List<Node> FindNearbyNodes(Node inputNode)
        {
            return _nodesToCluster.Where(node => MathHelpers.CalculateGeographicDistance(inputNode, node) < _maxDistanceBetweenNodes).ToList();
        }

        private Cluster PopulateAndExpandCluster(Node node, List<Node> nearbyNodes)
        {
            var cluster = new Cluster();
            cluster.Nodes.Add(node);

            foreach (var nearbyNode in nearbyNodes)
            {
                if (cluster.Nodes.Count >= _maxClusterSize)
                    break;

                if (!HasNodeBeenVisited(nearbyNode))
                {
                    _visitedNodes.Add(nearbyNode);
                    var otherNearbyNodes = FindNearbyNodes(nearbyNode);

                    if (otherNearbyNodes.Count >= _minClusterSize)
                    {
                        nearbyNodes.AddRange(otherNearbyNodes);
                    }
                }
                else
                {
                    if (!IsNodeInACluster(nearbyNode))
                    {
                        cluster.Nodes.Add(nearbyNode);
                    }
                }
            }

            return cluster;
        }

        private bool HasNodeBeenVisited(Node node)
        {
            return _visitedNodes.FirstOrDefault(visitedNode => visitedNode == node) != null;
        }

        private bool IsNodeInACluster(Node node)
        {
            return _clusters.FirstOrDefault(cluster => cluster.Nodes.Contains(node)) != null;
        }
    }
}
