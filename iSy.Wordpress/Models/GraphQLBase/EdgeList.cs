using iSy.Wordpress.Models.GRaphQLBase;
using System.Collections.Generic;

namespace iSy.Wordpress.Models.GraphQLBase
{
    public class EdgeList<T>
    {
        public List<Edge<T>> Edges { get; set; }
    }
}
