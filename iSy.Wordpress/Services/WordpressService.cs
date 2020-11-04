using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using iSy.Wordpress.Extensions;
using iSy.Wordpress.Models;
using iSy.Wordpress.Models.Post;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace iSy.Wordpress.Services
{
    public interface IWordpressService
    {
        Task<PostListViewModel> GetPostsOverview(string category, string after = "", string before = "");
        Task<PostDetailModel> GetPost(string id);
        Task<PostsInfo> LoadPostsInfo(string category);
    }

    public class WordpressService : IWordpressService
    {
        private readonly ILogger<WordpressService> _logger;

        public WordpressService(ILogger<WordpressService> logger)
        {
            _logger = logger;
        }

        public async Task<PostListViewModel> GetPostsOverview(string category, string after = "", string before = "")
        {
            using var graphQLClient = new GraphQLHttpClient("http://hitsrvwp1.becom.at/graphql", new SystemTextJsonSerializer());

            _logger.LogInformation($"Loading posts for category {category}...");
            var request = new GraphQLRequest
            {
                Query = @"
                    query GET_POSTS {
                        posts(where: {categoryName: ""{category}""},{paging}) {
                            edges {
                                cursor
                                node {
                                    author {
                                        node {
                                            firstName
                                            lastName
                                            email
                                        }
                                    }
                                    categories {
                                        nodes {
                                            name
                                        }
                                    }
                                    id
                                    date
                                    isSticky
                                    title
                                    excerpt
                               }
                           }
                           pageInfo {
                                hasNextPage
                                hasPreviousPage
                                startCursor
                                endCursor
                           }
                       }
                   }
                ".Replace("{category}", category)
            };

            if (!string.IsNullOrEmpty(after))
            {
                request.Query = request.Query.Replace("{paging}", $" after: \"{after}\", first: 10");
            } else if (!string.IsNullOrEmpty(before))
            {
                request.Query = request.Query.Replace("{paging}", $" before: \"{before}\", last: 10");
            } else
            {
                request.Query = request.Query.Replace("{paging}", $" first: 10");
            }

                var graphQLResponse = await graphQLClient.SendQueryAsync<PostsData<PostsNode>>(request);
            if (graphQLResponse.Errors != null && graphQLResponse.Errors.Length > 0)
            {
                foreach (var e in graphQLResponse.Errors)
                {
                    _logger.LogError($"Error loading posts for category {category}: {e.Message}");
                }
                return null;
            }
            _logger.LogInformation($"Posts loaded!");
            return graphQLResponse.Data.ToOverview(category);
        }

        public async Task<PostDetailModel> GetPost(string id)
        {
            using var graphQLClient = new GraphQLHttpClient("http://hitsrvwp1.becom.at/graphql", new SystemTextJsonSerializer());

            _logger.LogInformation($"Loading post with id {id}...");

            var request = new GraphQLRequest
            {
                Query = @"
                    query GET_POST {
                        post(id: ""{id}"") {
                            author {
                                node {
                                    firstName
                                    lastName
                                    email
                                }
                            }
                            categories {
                                nodes {
                                    name
                                }
                            }
                            content
                            date
                            id
                            title
                        }
                    }
                ".Replace("{id}", id)
            };

            var graphQLResponse = await graphQLClient.SendQueryAsync<PostData>(request);
            if (graphQLResponse.Errors != null && graphQLResponse.Errors.Length > 0)
            {
                foreach (var e in graphQLResponse.Errors)
                {
                    _logger.LogError($"Error loading post with id {id}: {e.Message}");
                }
                return null;
            }
            _logger.LogInformation($"Post loaded!");
            return graphQLResponse.Data.ToDetail();
        }

        public async Task<PostsInfo> LoadPostsInfo(string category)
        {
            using var graphQLClient = new GraphQLHttpClient("http://hitsrvwp1.becom.at/graphql", new SystemTextJsonSerializer());

            _logger.LogInformation($"Loading posts information for category {category}...");
            var request = new GraphQLRequest
            {
                Query = @"
                    query GET_POST {
                        posts(where: {categoryName: ""{category}""}) {
                            edges {
                                node {
                                    categories {
                                        nodes {
                                            name
                                        }
                                    }
                                    date
                                    title
                                }
                            }
                        }
                    }
                ".Replace("{category}", category)
            };

            var graphQLResponse = await graphQLClient.SendQueryAsync<PostsData<InfoNode>>(request);
            if (graphQLResponse.Errors != null && graphQLResponse.Errors.Length > 0)
            {
                foreach (var e in graphQLResponse.Errors)
                {
                    _logger.LogError($"Error loading posts information for category {category}: {e.Message}");
                }
                return null;
            }
            _logger.LogInformation($"Posts infos loaded! Calculating info data...");
            var data = graphQLResponse.Data.EdgeList.Edges.Select(x => x.Node);

            return new PostsInfo
            {
                Categories = data.Select(x => x.Categories.ToCategoryList()).SelectMany(x => x).Where(x => x.Category != category).GroupBy(x => x).Select(x => x.Key),
                RecentPosts = data.OrderByDescending(x => x.Date).Take(5),
                Archives = data.OrderByDescending(x => x.Date)
                .GroupBy(x => new { x.Date.Month, x.Date.Year })
                .Select(x => new ArchiveInfo { Month = x.Key.Month, Year = x.Key.Year, Count = x.Count() }),
                PostCount = data.Count()
            };
        }
    }




}
