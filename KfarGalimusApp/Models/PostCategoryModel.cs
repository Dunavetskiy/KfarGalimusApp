namespace KfarGalimusApp.Models
{
    public static class PostCategoryModel
    {
        // Method to get category color
        public static Color GetCategoryColor(PostModel.PostCategory category)
        {
            return category switch
            {
                PostModel.PostCategory.General => Colors.Black,
                PostModel.PostCategory.News => Colors.Blue,
                PostModel.PostCategory.Esra => Colors.Green,
                PostModel.PostCategory.CulturalClub => Colors.Yellow,
                _ => Colors.Black
            };
        }

        // Method to get category text color (for readability)
        public static Color GetCategoryTextColor(PostModel.PostCategory category)
        {
            return category switch
            {
                PostModel.PostCategory.CulturalClub => Colors.Black, // Black text for yellow background
                _ => Colors.White // White text for other backgrounds
            };
        }
    }
}
