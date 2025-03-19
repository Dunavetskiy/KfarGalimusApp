using Microsoft.Maui.Controls.Shapes;

namespace KfarGalimusApp.Pages
{
    public partial class OnboardingPage : ContentPage
    {
        // Список приветствий на разных языках
        private List<string> greetings = new List<string>
        {
            "ברוכים הבאים לאפליקציה שלנו!", // Иврит
            "Welcome to our application!", // Английский
            "ჩვენი აპლიკაციაში მოგესალმებით!", // Грузинский
            "Біздің қосымшаға қош келдіңіз!", // Казахский
            "Ласкаво просимо до нашого додатку!", // Украинский
            "Witamy w naszej aplikacji!" // Польский
        };

        private int currentIndex = 0; // Текущий индекс приветствия

        public OnboardingPage()
        {
            InitializeComponent();
            StartGreetingAnimation(); // Запуск анимации при загрузке страницы
            UpdateWavePath(); // Обновляем путь при загрузке страницы

        }

        /// <summary>
        /// Запуск анимации смены приветствий
        /// </summary>
        private async void StartGreetingAnimation()
        {
            while (true)
            {
                // Ждём 3 секунды перед началом анимации
                await Task.Delay(3000);

                // Скрываем текст с помощью анимации прозрачности
                await WelcomeLabel.FadeTo(0, 500); // 500 мс для плавного исчезновения текста

                // Меняем текст на следующий язык
                currentIndex = (currentIndex + 1) % greetings.Count; // Переход к следующему приветствию
                WelcomeLabel.Text = greetings[currentIndex];

                // Показываем текст с новой анимацией
                await WelcomeLabel.FadeTo(1, 500); // 500 мс для плавного появления текста
            }
        }

        private async void OnSignInClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(SignInPage)}");
        }

        // Переход на страницу "Sign Up"
        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(SignUpPage)}");
        }


        // Обновление пути для волны при изменении размеров экрана
        private void UpdateWavePath()
        {
            double width = this.Width;  // Получаем ширину экрана
            double height = 150;        // Высота волны

            // Создаем новый PathGeometry
            var pathGeometry = new PathGeometry();
            var pathFigure = new PathFigure();

            // Начальная точка
            pathFigure.StartPoint = new Point(0, height);

            // Добавляем сегменты кривых для волны с учетом ширины
            pathFigure.Segments.Add(new BezierSegment(
                new Point(width / 4, height - 50),  // Точка кривой
                new Point(width / 2, height + 50),  // Точка кривой
                new Point(width, height)));         // Конечная точка

            // Добавляем фигуру в PathGeometry
            pathGeometry.Figures.Add(pathFigure);

            
        }

        // Обработчик изменения размера
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            UpdateWavePath();  // Обновляем путь волны, когда изменяется размер экрана
        }
    }
}
