using iSy.Wordpress.Models.Post;
using iSy.Wordpress.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace iSy.Wordpress.Components
{
    public partial class PostListContainer
    {
        [Inject]
        public IWordpressService WordpressService { get; set; }

        [Parameter]
        public string Category { get; set; }

        [Parameter]
        public int PostCount { get; set; }

        public List<PostOverview> Posts { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            Posts = await WordpressService.GetPostsOverview(Category);
        }
    }
}
