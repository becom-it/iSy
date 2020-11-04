using System;
using System.Collections.Generic;

namespace iSy.Wordpress.Models.Post
{
    public class PostOverview
    {
        public string Id { get; set; }
        public string Cursor { get; set; }
        public string Title { get; set; }
        public string Excerpt { get; set; }
        public bool IsSticky { get; set; }
        public string AuthorInfo { get; set; }
        public List<(string Category, string Link)> Categories { get; set; }
        public DateTime Date { get; set; }
        public string Link { get; set; }
    }
}
