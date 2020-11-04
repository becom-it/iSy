using System.Collections.Generic;

namespace iSy.Wordpress.Models.Post
{
    public class PostsInfo
    {
        public IEnumerable<(string Category, string Link)> Categories { get; set; }
        public IEnumerable<InfoNode> RecentPosts { get; set; }
        public IEnumerable<ArchiveInfo> Archives { get; set; }
        public int PostCount { get; set; }
    }
}
