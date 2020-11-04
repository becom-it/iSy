using iSy.Wordpress.Models.GraphQLBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace iSy.Wordpress.Models.Post
{
    public class PostListViewModel
    {
        public List<PostOverview> Posts { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
