using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clustering.Models
{
    public class DbScanResult
    {
        public List<Cluster> Clusters { get; set; }
        public List<Node> Noise { get; set; }
    }
}
