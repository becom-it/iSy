using iSy.Wordpress.Models.Generic;
using System;

namespace iSy.Wordpress.Models.Post
{
    public class PostsNode
    {
        public string Id { get; set; }
        public Author Author { get; set; }
        public Categories Categories { get; set; }
        public DateTime Date { get; set; }
        public bool IsSticky { get; set; }
        public string Title { get; set; }
        public string Excerpt { get; set; }
    }
}
