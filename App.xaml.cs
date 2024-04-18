namespace OnlyBelaSemafor
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "rezultsDb.db3");
            var databaseManager = new DatabaseManager(dbPath);
                                  
            MainPage = new AppShell(databaseManager);
        }
    }
}
