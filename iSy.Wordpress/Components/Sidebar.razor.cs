using iSy.Wordpress.Models.Post;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace iSy.Wordpress.Components
{
    public partial class Sidebar
    {
        [Parameter]
        public PostsInfo Info { get; set; }
    }
}
