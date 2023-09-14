namespace API.Models;

    public class IssueViewModel
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public string Url { get; set; }
        public string AuthorUsername { get; set; }
        public string AuthorAvatarUrl { get; set; } 
        public string AuthorProfileUrl { get; set; }
    }


