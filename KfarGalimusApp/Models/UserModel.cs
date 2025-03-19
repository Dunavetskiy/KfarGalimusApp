using CommunityToolkit.Mvvm.ComponentModel;

namespace KfarGalimusApp.Models
{
    public partial class UserModel : ObservableObject
    {
        public required string UserId { get; set; }
        public required string Email { get; set; } = string.Empty;
        public required string UserName { get; set; } = string.Empty;
        public ImageSource AvatarUrl { get; set; } = ImageSource.FromFile("avatardefault.png");

        public string? Role { get; set; } // "Teacher", "Student"
        public List<string>? Interests { get; set; } = new List<string>(); 
        public bool? IsTeacher => Role == "Teacher";
    }
}
