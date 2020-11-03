using iSy.Wordpress.Models.GraphQLBase;
using System.Text.Json.Serialization;

namespace iSy.Wordpress.Models.Post
{
    public class PostsData<T>
    {
        [JsonPropertyName("Posts")]
        public EdgeList<T> EdgeList { get; set; }
    }
}
