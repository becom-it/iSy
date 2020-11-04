using iSy.Wordpress.Models.Post;
using iSy.Wordpress.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace iSy.Wordpress.Pages
{
    public partial class Posts
    {
        [Inject]
        public IWordpressService WordpressService { get; set; }

        [Parameter]
        public string Cat { get; set; }

        public PostsInfo Info { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            Info = await WordpressService.LoadPostsInfo(Cat);
        }
    }
}
