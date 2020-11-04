using iSy.Wordpress.Models.GraphQLBase;
using iSy.Wordpress.Models.Post;
using iSy.Wordpress.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
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
        public int PostCount { get; set; } = 0;

        [Parameter]
        public string Before { get; set; } = "";

        [Parameter]
        public string After { get; set; } = "";

        public List<PostOverview> Posts { get; set; }

        public PageInfo PageInfo { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            var vm = await WordpressService.GetPostsOverview(Category, After, Before);
            Posts = vm.Posts;
            PageInfo = vm.PageInfo;
        }
    }
}
