using System;
using Clustering.Models;

namespace Clustering
{
    public class MathHelpers
    {
        public static double ConvertToRadians(double angle)
        {
            return (angle * Math.PI) / 180;
        }

        public static double CalculateGeographicDistance(Node StartNode, Node EndNode)
        {
            var dLatitude = ConvertToRadians(EndNode.Latitude) - ConvertToRadians(StartNode.Latitude);
            var dLongitude = ConvertToRadians(EndNode.Longitude) - ConvertToRadians(StartNode.Longitude);

            dLatitude /= 2;
            dLongitude /= 2;

            return 2 * 
                   DbScanAlgorithm.EarthApproximateRadius *
                   Math.Asin(Math.Sqrt(
                       Math.Pow(Math.Sin(dLatitude), 2) +
                       Math.Cos(StartNode.Latitude) * Math.Cos(EndNode.Latitude) * Math.Pow(Math.Sin(dLongitude), 2)
                   ));
        }
    }
}
