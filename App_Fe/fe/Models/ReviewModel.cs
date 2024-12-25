
using Models;

public class ReviewModel
{

    public string? Id { get; set; }

    public DateTime CreateDate { get; set; }
    public int ProductID { get; set; }
    public string? UserId { get; set; }
    public UserModel User { get; set; } = new UserModel();

    public ReviewModel Rep_review { get; set; }
    public string? Content { get; set; }

    public int Like { get; set; }
    public List<string> user_liked { get; set; } = new List<string>();

}
