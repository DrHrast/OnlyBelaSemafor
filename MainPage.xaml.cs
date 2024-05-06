using OnlyBelaSemafor.Models;
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Maui.Core.Extensions;
using OnlyBelaSemafor.Popups;

namespace OnlyBelaSemafor
{
    public partial class MainPage : ContentPage
    {
        #region Fields

        private readonly DatabaseManager _databaseManager;
        private readonly GameModel _game;
        private readonly AppSettings _appSettings;

        private const int MaxPoints = 162;
        private bool _isScoreUpdating;
        private ImageSource _selectedThemeIcon;
        private int _scoreTarget;
        private bool _canDeleteLastScore;        
        private ObservableCollection<ScoreModel> _scores = [];

        #endregion

        #region Properties

        public ObservableCollection<ScoreModel> Scores
        {
            get => _scores;
            set
            {
                if (Equals(value, _scores)) return;
                _scores = value;
                OnPropertyChanged();
            }
        }
        public TeamDisplayModel TeamOneDisplay { get; set; } = new();
        public TeamDisplayModel TeamTwoDisplay { get; set; } = new();

        public bool CanDeleteLastScore
        {
            get => _canDeleteLastScore;
            set
            {
                if (value == _canDeleteLastScore) return;
                _canDeleteLastScore = value;
                OnPropertyChanged();
            }
        }

        public int ScoreTarget
        {
            get => _scoreTarget;
            set
            {
                if (value == _scoreTarget) return;
                _scoreTarget = value;
                OnPropertyChanged();
            }
        }

        public ImageSource SelectedThemeIcon
        {
            get => _selectedThemeIcon;
            set
            {
                if (Equals(value, _selectedThemeIcon)) return;
                _selectedThemeIcon = value;
                OnPropertyChanged();
            }
        }

        #endregion

        
        // Todo: move error message to TeamDisplayModel
        private string _teamOneErrorMessage = "";
        public string TeamOneErrorMessage
        {
            get => _teamOneErrorMessage;
            set
            {
                if (value == _teamOneErrorMessage) return;
                _teamOneErrorMessage = value;
                OnPropertyChanged();
            }
        }


        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;

            // Dependency injection
            _databaseManager = App.Current.Services.GetRequiredService<DatabaseManager>();
            _game = App.Current.Services.GetRequiredService<GameModel>();
            _appSettings = App.Current.Services.GetRequiredService<AppSettings>();

            Scores.CollectionChanged += (_, _) => CanDeleteLastScore = Scores.Count > 0;

            LoadSettings();

            if (!LoadGame())
            {
                ShowNewGamePopup(true);
            }

            TeamOneDisplay.PropertyChanged += TeamOneDisplay_OnPropertyChanged;
            TeamTwoDisplay.PropertyChanged += TeamTwoDisplay_OnPropertyChanged;
        }

        // Load application settings
        private void LoadSettings()
        {
            _appSettings.Theme = _databaseManager.LoadTheme();
            App.Current.UserAppTheme = _appSettings.Theme == "Dark" ? AppTheme.Dark : AppTheme.Light;
            SelectedThemeIcon = _appSettings.Theme == "Dark" ? "sun_switch_sized.png" : "moon_switch_sized.png";
        }

        // Start new game or load existing one
        private void NewGameButton_Clicked(object sender, EventArgs e)
        {
            ShowNewGamePopup();
        }

        private async void ShowNewGamePopup(bool isStartup = false)
        {
            var result = (bool)(await this.ShowPopupAsync(new NewGamePopup(isStartup)))!;

            if (result)
            {
                NewGame();
            }
        }

        private void NewGame()
        {
            ResetInputData();
            _databaseManager.DeleteAllScores();

            TeamOneDisplay.Name = _game.TeamOneName;
            TeamTwoDisplay.Name = _game.TeamTwoName;
            ScoreTarget = _game.ScoreTarget;
            Scores.Clear();
        }

        private bool LoadGame()
        {
            var gameLoaded = _databaseManager.LoadGame();
            if (!gameLoaded)
            {
                return false;
            }

            //  Load game data
            TeamOneDisplay.Name = _game.TeamOneName;
            TeamTwoDisplay.Name = _game.TeamTwoName;
            ScoreTarget = _game.ScoreTarget;

            Scores = _databaseManager.GetScores().ToObservableCollection();

            return true;
        }

        // Input handling
        private void TeamOneDisplay_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (_isScoreUpdating) return;

            _isScoreUpdating = true;

            if (e.PropertyName == nameof(TeamDisplayModel.Score))
            {
                TeamOneErrorMessage = "";
                
                var validationContext = new ValidationContext(TeamOneDisplay)
                {
                    MemberName = nameof(TeamDisplayModel.Score)
                };

                var validationResults = new List<ValidationResult>();

                var isValid = Validator.TryValidateProperty(TeamOneDisplay.Score, validationContext, validationResults);

                if (!isValid)
                {
                    TeamOneErrorMessage = validationResults.FirstOrDefault()?.ErrorMessage ?? "";
                    _isScoreUpdating = false;
                    return;
                }

                TeamTwoDisplay.Score = MaxPoints - TeamOneDisplay.Score;
            }

            SetStiljaVisibility();
            _isScoreUpdating = false;
        }

        private void TeamTwoDisplay_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (_isScoreUpdating) return;

            _isScoreUpdating = true;

            if (e.PropertyName == nameof(TeamDisplayModel.Score))
            {
                TeamOneDisplay.Score = MaxPoints - TeamTwoDisplay.Score;
            }

            SetStiljaVisibility();
            _isScoreUpdating = false;
        }

        private void SetStiljaVisibility()
        {
            TeamOneDisplay.IsStiljaVisible = TeamTwoDisplay.Score == 0;
            TeamTwoDisplay.IsStiljaVisible = TeamOneDisplay.Score == 0;
        }

        // Add new result
        private void AddButton_Clicked(object sender, EventArgs e)
        {
            if (TeamOneDisplay.Score == 0 && TeamTwoDisplay.Score == 0)
            {
                return;
            }

            var teamOne = new TeamModel()
            {
                Name = TeamOneDisplay.Name,
                Score = TeamOneDisplay.Score,
                Call = TeamOneDisplay.Call,
                Bela = TeamOneDisplay.IsBela ? 20 : 0,
                IsCalling = TeamOneDisplay.IsCalling,
                IsStilja = TeamOneDisplay.IsStilja
            };
            
            var teamTwo = new TeamModel()
            {
                Name = TeamTwoDisplay.Name,
                Score = TeamTwoDisplay.Score,
                Call = TeamTwoDisplay.Call,
                Bela = TeamTwoDisplay.IsBela ? 20 : 0,
                IsCalling = TeamTwoDisplay.IsCalling,
                IsStilja = TeamTwoDisplay.IsStilja
            };
            
            var resultModel = new ResultModel(teamOne, teamTwo);
            resultModel.CalculateResult();

            var lastResult = Scores.LastOrDefault();
            
            Scores.Add(new ScoreModel()
            {
                Team1 = resultModel.TeamOneGameScore + (lastResult?.Team1 ?? 0), 
                Team2 = resultModel.TeamTwoGameScore + (lastResult?.Team2 ?? 0)
            });
            
            _databaseManager.AddScore(Scores.Last());
            CheckVictoryConditions();
            ResetInputData();
        }
        private async void CheckVictoryConditions()
        {
            var victoriousTeam = "";
            
            if (Scores.Last().Team1 >= ScoreTarget)
            {
                victoriousTeam = TeamOneDisplay.Name;
            }
            else if (Scores.Last().Team2 >= ScoreTarget)
            {
                victoriousTeam = TeamTwoDisplay.Name;
            }
            
            if (!string.IsNullOrEmpty(victoriousTeam))
            {
                await this.ShowPopupAsync(new VictoryPopup(victoriousTeam));
                NewGame();
            }
        }
        private void ResetInputData()
        {
            _isScoreUpdating = true;
            TeamOneDisplay.Reset();
            TeamTwoDisplay.Reset();
            _isScoreUpdating = false;
        }

        // Delete last score
        private void DeleteLastScoreButton_Clicked(object sender, EventArgs e)
        {
            DeleteLastScore();
        }
        private void DeleteLastScore()
        {
            _databaseManager.DeleteLastScore();
            Scores.RemoveAt(Scores.Count - 1);
        }

        // Show help popup
        private void HelpButton_Clicked(object sender, EventArgs e)
        {
            this.ShowPopup(new HelpPopup());
        }

        // Change theme
        private void ThemeButton_Clicked(object sender, EventArgs e)
        {
            if (_appSettings.Theme == "Dark")
            {
                _appSettings.Theme = "Light";
                _databaseManager.SaveTheme("Light");
                App.Current.UserAppTheme = AppTheme.Light;
                SelectedThemeIcon = "moon_switch_sized.png";
            }
            else
            {
                _appSettings.Theme = "Dark";
                _databaseManager.SaveTheme("Dark");
                App.Current.UserAppTheme = AppTheme.Dark;
                SelectedThemeIcon = "sun_switch_sized.png";
            }
        }
    }
}