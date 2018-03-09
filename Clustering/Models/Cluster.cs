using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clustering.Models
{
    public class Cluster
    {
        public Node Center { get; set; }
        public List<Node> Nodes { get; set; }
    }
}
