public class PostModel
{
    public int Id { get; set; }
    public string UserName { get; set; } = "Anonim"; // Not Null
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? UserId { get; set; }

    public enum PostCategory
    {
        General,
        News,
        Esra,
        CulturalClub
    }
    public PostCategory? Category { get; set; } = PostCategory.General;

    //// New properties for likes, dislikes, and supports
    //public int Likes { get; set; } = 0;
    //public int Dislikes { get; set; } = 0;
    //public int Supports { get; set; } = 0;

    //// New properties to track the user's action
    //public bool HasLiked { get; set; } = false;
    //public bool HasDisliked { get; set; } = false;
    //public bool HasSupported { get; set; } = false;

}
