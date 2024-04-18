namespace OnlyBelaSemafor
{
    public partial class AppShell : Shell
    {
        private readonly DatabaseManager databaseManager;

        public AppShell(DatabaseManager databaseManager)
        {
            InitializeComponent();
            this.databaseManager = databaseManager;

            var database = databaseManager;

            var main = new MainPage(database);
        }
    }
}
