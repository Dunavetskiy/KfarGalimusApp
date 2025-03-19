using Firebase.Database;
using KfarGalimusApp.Models;

namespace KfarGalimusApp.Pages
{
    public partial class MaOmerPage : ContentPage
    {
        private readonly FirebaseClient _firebaseClient;
        private PostModel.PostCategory? _selectedCategory = null;
        public MaOmerPage(FirebaseClient firebaseClient)
        {
            InitializeComponent();
            _firebaseClient = firebaseClient;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadPosts();
        }

        private async Task LoadPosts()
        {
            var posts = await _firebaseClient.Child("PostModel").OnceAsync<PostModel>();

            var filteredPosts = posts
                .Select(p => p.Object)
                .Where(p => _selectedCategory == null || p.Category == _selectedCategory)
                .OrderByDescending(p => DateTime.TryParse(p.Date, out var parsedDate) ? parsedDate : DateTime.MinValue)
                .ToList();

            var postStackLayout = new VerticalStackLayout { Spacing = 10 };

            foreach (var post in filteredPosts)
            {
                postStackLayout.Children.Add(CreatePostFrame(post));
            }

            mainStackLayout.Clear();
            mainStackLayout.Add(postStackLayout);
        }

        private Frame CreatePostFrame(PostModel post)
        {
            var userLogo = new Image
            {
                Source = ImageSource.FromFile("avatardefault.png"),
                WidthRequest = 23,
                HeightRequest = 23,
                Aspect = Aspect.AspectFill,
            };

            var usernameFrame = new Frame
            {
                BackgroundColor = Color.FromArgb("#81D4FA"),
                Padding = new Thickness(10, 5),
                CornerRadius = 15,
                HorizontalOptions = LayoutOptions.Start,
                Content = new Label
                {
                    Text = post.UserName,
                    FontSize = 14,
                    TextColor = Colors.White,
                    FontAttributes = FontAttributes.Bold,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                }
            };

            var categoryFrame = new Frame
            {
                BackgroundColor = PostCategoryModel.GetCategoryColor(post.Category.Value),
                Padding = new Thickness(10, 5),
                CornerRadius = 15,
                HorizontalOptions = LayoutOptions.End,
                Content = new Label
                {
                    Text = post.Category.ToString(),
                    FontSize = 14,
                    TextColor = PostCategoryModel.GetCategoryTextColor(post.Category.Value),
                    FontAttributes = FontAttributes.Bold,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                }
            };

            var grid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                },
                Children = { userLogo, usernameFrame, categoryFrame }
            };
            Grid.SetColumn(userLogo, 0);
            Grid.SetColumn(usernameFrame, 1);
            Grid.SetColumn(categoryFrame, 2);

            return new Frame
            {
                Padding = 0,
                BorderColor = PostCategoryModel.GetCategoryColor(post.Category.Value),
                CornerRadius = 10,
                HasShadow = true,
                BackgroundColor = Color.FromArgb("#F0F8FF"),
                Content = new VerticalStackLayout
                {
                    Padding = 10,
                    Children =
                    {
                        grid,
                        new Label
                        {
                            Text = post.Title,
                            FontSize = 18,
                            FontAttributes = FontAttributes.Bold,
                            Padding = 3,
                            TextColor = Colors.Blue,
                        },
                        new Frame
                        {
                            BackgroundColor = Colors.Black,
                            Padding = 10,
                            CornerRadius = 10,
                            Content = new Label
                            {
                                Text = post.Content,
                                FontSize = 14,
                                TextColor = Colors.White,
                            }
                        },
                        new Label
                        {
                            Text = $"Date: {post.Date}",
                            FontSize = 12,
                            TextColor = Colors.Gray,
                            Padding = 3,
                        }
                    }
                }
            };
        }

        private async void OnCategoryLabelTapped(object sender, EventArgs e)
        {
            if (sender is Label selectedLabel)
            {
                string newCategory = selectedLabel.Text;

                _selectedCategory = newCategory switch
                {
                    "All" => null,
                    "General" => PostModel.PostCategory.General,
                    "News" => PostModel.PostCategory.News,
                    "Esra" => PostModel.PostCategory.Esra,
                    "Cultural Club" => PostModel.PostCategory.CulturalClub,
                    _ => null
                };

                ResetCategoryColors();

                selectedLabel.TextColor = Colors.Black;

                await LoadPosts();
            }
        }

        private void ResetCategoryColors()
        {
            AllLabel.TextColor = Colors.Grey;
            GeneralLabel.TextColor = Colors.Grey;
            NewsLabel.TextColor = Colors.Grey;
            EsraLabel.TextColor = Colors.Grey;
            CulturalClubLabel.TextColor = Colors.Grey;
        }

        private async void OnAddPostClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreatePostPage(_firebaseClient));
        }
    }
}
