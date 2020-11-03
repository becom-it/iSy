using System;
using System.Collections.Generic;
using System.Text;

namespace iSy.Wordpress.Models.Post
{
    public class PostDetailModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorInfo { get; set; }
        public List<(string Category, string Link)> Categories { get; set; }
        public DateTime Date { get; set; }
    }
}
