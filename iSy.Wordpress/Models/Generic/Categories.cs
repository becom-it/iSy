using System.Collections.Generic;

namespace iSy.Wordpress.Models.Generic
{
    public class Categories
    {
        public List<CategorieNode> Nodes { get; set; }
    }

    public class CategorieNode
    {
        public string Name { get; set; }
    }
}
