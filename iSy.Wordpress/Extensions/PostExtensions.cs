using iSy.Wordpress.Components;
using iSy.Wordpress.Models.Post;
using System.Collections.Generic;

namespace iSy.Wordpress.Extensions
{
    public static class PostExtensions
    {
        public static List<PostOverview> ToOverview(this PostsData<PostsNode> source, string baseCategory)
        {
            var ret = new List<PostOverview>();
            
            foreach (var edge in source.EdgeList.Edges)
            {
                var ov = new PostOverview();
                ov.Id = edge.Node.Id;
                ov.Cursor = edge.Cursor;
                ov.AuthorInfo = $"{edge.Node.Author.ToAuthorInfo()} am {edge.Node.Date.ToShortDateString()}";
                ov.Categories = edge.Node.Categories.ToCategoryList(baseCategory);
                ov.Excerpt = edge.Node.Excerpt;
                ov.IsSticky = edge.Node.IsSticky;
                ov.Title = edge.Node.Title;
                ov.Date = edge.Node.Date;
                ov.Link = $"/post/{ov.Id}";
                ret.Add(ov);
            }

            return ret;
        }

        public static PostDetailModel ToDetail(this PostData source)
        {
            return new PostDetailModel
            {
                Id = source.Post.Id,
                AuthorInfo = $"{source.Post.Author.ToAuthorInfo()} am {source.Post.Date.ToShortDateString()}",
                Categories = source.Post.Categories.ToCategoryList(),
                Title = source.Post.Title,
                Content = source.Post.Content,
                Date = source.Post.Date
            };
        }
    }
}
