using iSy.Wordpress.Models.Generic;

namespace iSy.Wordpress.Extensions
{
    public static class AuthorExtensions
    {
        public static string ToAuthorInfo(this Author source)
        {
            return $"{source.Node.FirstName} {source.Node.LastName}";
        }
    }
}
