using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core.Views;
using CommunityToolkit.Maui.Views;
using OnlyBelaSemafor.Models;
using System.ComponentModel;

namespace OnlyBelaSemafor;

public partial class SettingsPopup : Popup
{
    private readonly GameModel _game;
    private readonly AppSettings _appSettings;
    private readonly DatabaseManager _databaseManager;
    private bool isPropertyChanged = false;

    public SettingsPopup()
    {
        //DID_IT 7: Switch not set as it should be
        InitializeComponent();

        _game = App.Current.Services.GetRequiredService<GameModel>();
        _appSettings = App.Current.Services.GetRequiredService<AppSettings>();
        _databaseManager = App.Current.Services.GetRequiredService<DatabaseManager>();
        cv_ScoreSettings.ItemsSource = ScoreDisplay;

        LoadData();
    }

    private void LoadData()
    {
        entry_Team1.Placeholder = _game.TeamOneName;
        entry_Team2.Placeholder = _game.TeamTwoName;

        var tempScoreTarget = ScoreDisplay.Where(s => s.Score == _game.ScoreTarget).FirstOrDefault();

        if(tempScoreTarget is not null)
        {
            tempScoreTarget.IsSelected = true;
        }

        switch_Theme.IsToggled = _appSettings.Theme == "Dark";
    }

    private void CloseWindow()
    {
        //DID_IT: 2. Implement database entry
        //TODO: 3. Implement control for reverting to default settings
        // DID_IT: 7. Create GameModel table and save game settings to database
        // DID_IT: 8. create AppSettings table and save app settings to database
        if (isPropertyChanged == true)
        {
            _databaseManager.UpdateScoreValue(_game.ScoreTarget.ToString());
            _databaseManager.UpdateModeValue(_appSettings.Theme);
        }

        isPropertyChanged = false;
        this.Close();
    }

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        CloseWindow();
    }

    // Team settings
    private void SetTeamsButton_Clicked(object sender, EventArgs e)
    {
        frame_ScoresSettings.IsVisible = false;
        frame_TeamSettings.IsVisible = true;
    }
    private void CloseTeamSettingsButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(entry_Team1.Text) || string.IsNullOrEmpty(entry_Team2.Text))
        {
            // todo: Show error message
            //return;
        }

        _game.TeamOneName = entry_Team1.Text;
        _game.TeamTwoName = entry_Team2.Text;
        isPropertyChanged = true;
    }

    //  Score settings 
    public class ScoreDisplayModel
    {
        public int Score { get; set; }
        public bool IsSelected { get; set; }
    }
    public List<ScoreDisplayModel> ScoreDisplay { get; set; } = new List<ScoreDisplayModel>
    {
        new ScoreDisplayModel { Score = 1301, IsSelected = false },
        new ScoreDisplayModel { Score = 1001, IsSelected = false },
        new ScoreDisplayModel { Score = 701, IsSelected = false },
        new ScoreDisplayModel { Score = 501, IsSelected = false },
        new ScoreDisplayModel { Score = 301, IsSelected = false }

    };

    private void SetScoresButton_Clicked(object sender, EventArgs e)
    {        
        ScoreDisplay.First(t => t.Score == _game.ScoreTarget).IsSelected = true;
        frame_TeamSettings.IsVisible = false;
        frame_ScoresSettings.IsVisible = true;
    }
    private void CloseScoresSettingsButton_Clicked(object sender, EventArgs e)
    {
        isPropertyChanged = true;
        _game.ScoreTarget = ScoreDisplay.First(s => s.IsSelected).Score;

        _databaseManager.UpdateScoreValue(_game.ScoreTarget.ToString());
        
        frame_ScoresSettings.IsVisible = false;
    }

    private void DarkModeSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        isPropertyChanged = true;
        _appSettings.Theme = e.Value ? "Dark" : "Light";
        _databaseManager.UpdateModeValue(_appSettings.Theme);
        App.Current.UserAppTheme = _appSettings.Theme == "Light" ? AppTheme.Light : AppTheme.Dark;
    }
}