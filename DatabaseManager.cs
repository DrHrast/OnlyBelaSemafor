using OnlyBelaSemafor.Models;
using SQLite;

namespace OnlyBelaSemafor
{
    public class DatabaseManager
    {
        private readonly string _dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "rezultsDb.db3");
        private readonly SQLiteConnection _database;
        private readonly GameModel _game;
        
        public DatabaseManager()
        {
            _game = App.Current.Services.GetRequiredService<GameModel>();
            
            _database = new SQLiteConnection(_dbPath);
            
            // DropAll();
            
            _database.CreateTable<SettingsModel>();
            _database.CreateTable<GameModel>();
            _database.CreateTable<ScoreModel>();

            SetDefaultSettings();
        }
        
        // Manual database reset
        public void DropAll()
        {
            _database.DropTable<SettingsModel>();
            _database.DropTable<GameModel>();
            _database.DropTable<ScoreModel>();
        }

        
        
        // Score
        public void AddScore(ScoreModel score)
        {
            _database.Insert(score);
        }
        public List<ScoreModel> GetScores()
        {
            return _database.Table<ScoreModel>().ToList();
        }
        public void DeleteLastScore()
        {
            _database.Delete(_database.Table<ScoreModel>().Last());
        }
        public void DeleteAllScores()
        {
            _database.DeleteAll<ScoreModel>();
        }

        // Settings
        private void SetDefaultSettings()
        {
            var themeExists = _database.Table<SettingsModel>().FirstOrDefault(s => s.Key == "Theme");
            if (themeExists is null)
            {
                SaveTheme("Dark");
            }
        }

        // Game
        public void SaveGame(GameModel game)
        {
            var gameExists = _database.Table<GameModel>().FirstOrDefault();
            if (gameExists != null)
            {
                gameExists.TeamOneName = game.TeamOneName;
                gameExists.TeamTwoName = game.TeamTwoName;
                gameExists.ScoreTarget = game.ScoreTarget;
                _database.Update(gameExists);
            }
            else
            {
                _database.Insert(game);
            }
        }
        public bool LoadGame()
        {
            var game = _database.Table<GameModel>().FirstOrDefault();
            if (game == null) return false;
            
            _game.TeamOneName = game.TeamOneName;
            _game.TeamTwoName = game.TeamTwoName;
            _game.ScoreTarget = game.ScoreTarget;
            
            return true;
        }

        // Theme
        public void SaveTheme(string theme)
        {
            var exists = _database.Table<SettingsModel>().FirstOrDefault(s => s.Key == "Theme");
            if (exists != null)
            {
                exists.Value = theme;
                _database.Update(exists);
            }
            else
            {
                _database.Insert(new SettingsModel
                {
                    Key = "Theme",
                    Value = theme
                });
            }
        }
        public string LoadTheme()
        {
            return _database.Table<SettingsModel>().First(s => s.Key == "Theme").Value;
        }
    }
}
