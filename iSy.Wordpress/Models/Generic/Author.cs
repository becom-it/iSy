namespace iSy.Wordpress.Models.Generic
{
    public class Author
    {
        public AuthorNode Node { get; set; }
    }

    public class AuthorNode
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
