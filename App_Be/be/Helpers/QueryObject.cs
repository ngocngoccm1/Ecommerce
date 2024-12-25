namespace App.Helpers
{
    public class QueryObject
    {
        public int? CategoryId { set; get; }
        public string Name { set; get; } = null;
        public bool Isdescending { set; get; }
        public int PageNumber { set; get; } = 1;
        public int PageSize { set; get; } = 10;

    }
}