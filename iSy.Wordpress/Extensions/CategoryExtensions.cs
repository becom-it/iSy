using iSy.Wordpress.Models.Generic;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace iSy.Wordpress.Extensions
{
    public static class CategoryExtensions
    {
        public static List<(string Category, string Link)> ToCategoryList(this Categories source, string baseCategory = "")
        {
            var ret = new List<(string, string)>();
            IEnumerable<string> cats;

            if (string.IsNullOrEmpty(baseCategory))
            {
                cats = source.Nodes.Select(x => x.Name);
            } else
            {
                cats = source.Nodes.Where(x => x.Name != baseCategory).Select(x => x.Name);
            }

            foreach(var c in cats)
            {
                ret.Add((c, $"/posts/{c}"));
            }


            return ret;
        }
    }
}
