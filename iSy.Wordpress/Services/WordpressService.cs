using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using iSy.Wordpress.Extensions;
using iSy.Wordpress.Models;
using iSy.Wordpress.Models.Post;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace iSy.Wordpress.Services
{
    public interface IWordpressService
    {
        Task<List<PostOverview>> GetPostsOverview(string category);
        Task<PostDetailModel> GetPost(string id);
    }

    public class WordpressService : IWordpressService
    {
        private readonly ILogger<WordpressService> _logger;

        public WordpressService(ILogger<WordpressService> logger)
        {
            _logger = logger;
        }

        public async Task<List<PostOverview>> GetPostsOverview(string category)
        {
            using var graphQLClient = new GraphQLHttpClient("http://hitsrvwp1.becom.at/graphql", new SystemTextJsonSerializer());

            var request = new GraphQLRequest
            {
                Query = @"
                    query GET_POSTS {
                        posts(where: {categoryName: ""{category}""}, first: 10) {
                            edges {
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
                       }
                   }
                ".Replace("{category}", category)
            };

            var graphQLResponse = await graphQLClient.SendQueryAsync<PostsData<PostsNode>>(request);
            return graphQLResponse.Data.ToOverview(category);
        }

        public async Task<PostDetailModel> GetPost(string id)
        {
            using var graphQLClient = new GraphQLHttpClient("http://hitsrvwp1.becom.at/graphql", new SystemTextJsonSerializer());

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
            return graphQLResponse.Data.ToDetail();
        }
    }
}
