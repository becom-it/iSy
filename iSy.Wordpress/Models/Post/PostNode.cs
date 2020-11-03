using iSy.Wordpress.Models.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace iSy.Wordpress.Models.Post
{
    public class PostNode
    {
        public string Id { get; set; }
        public Author Author { get; set; }
        public Categories Categories { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
