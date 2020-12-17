using iSy.Wordpress.Models.Post;
using iSy.Wordpress.Services;
using Microsoft.AspNetCore.Components;
using System.Reactive.PlatformServices;
using System.Reflection;
using System.Threading.Tasks;

namespace iSy.Pages
{
    public partial class Posts
    {
        [Inject]
        public IWordpressService WordpressService { get; set; }

        [Parameter]
        public string Cat { get; set; }

        [Parameter]
        public string Paging { get; set; } = "";

        public PostsInfo Info { get; set; }

        public string After { get; set; } = "";

        public string Before { get; set; } = "";





        protected override async Task OnParametersSetAsync()
        {
            Info = await WordpressService.LoadPostsInfo(Cat);

            After = string.Empty;
            Before = string.Empty;
            if (!string.IsNullOrEmpty(Paging))
            {
                if (Paging.StartsWith("A-")) After = Paging[2..];
                if (Paging.StartsWith("B-")) Before = Paging[2..];
            }
        }
    }
}
