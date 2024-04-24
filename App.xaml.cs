// Ignore Spelling: Semafor

using OnlyBelaSemafor.Models;

namespace OnlyBelaSemafor
{
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current!;
        public ServiceProvider Services { get; private set; } = null!;

        public App()
        {
            InitializeComponent();

            Services = ConfigureServices();

            MainPage = new AppShell();
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<DatabaseManager>();
            services.AddSingleton<GameModel>();
            services.AddSingleton<AppSettings>();

            return services.BuildServiceProvider();
        }
    }
}
