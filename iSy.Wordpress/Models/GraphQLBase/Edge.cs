namespace iSy.Wordpress.Models.GRaphQLBase
{
    public class Edge<T>
    {
        public T Node { get; set; }
        public string Cursor { get; set; }
    }
}
