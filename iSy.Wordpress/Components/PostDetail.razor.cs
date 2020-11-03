using iSy.Wordpress.Models.Post;
using iSy.Wordpress.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace iSy.Wordpress.Components
{
    public partial class PostDetail
    {
        [Inject]
        public IWordpressService WordpressService { get; set; }

        [Parameter]
        public string Id { get; set; }

        public PostDetailModel Post { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            Post = await WordpressService.GetPost(Id);
        }
    }
}
