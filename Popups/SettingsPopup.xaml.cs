using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core.Views;
using CommunityToolkit.Maui.Views;
using OnlyBelaSemafor.Models;

namespace OnlyBelaSemafor;

public partial class SettingsPopup : Popup
{
    private readonly GameModel _game;
    private readonly AppSettings _appSettings;
    private readonly DatabaseManager _databaseManager;

    public SettingsPopup(MainPage mainPage)
    {
        InitializeComponent();

        _game = App.Current.Services.GetRequiredService<GameModel>();
        _appSettings = App.Current.Services.GetRequiredService<AppSettings>();

        cv_ScoreSettings.ItemsSource = ScoreDisplay;

        LoadData();
    }

    private void LoadData()
    {
        entry_Team1.Text = _game.TeamOneName;
        entry_Team2.Text = _game.TeamTwoName;

        var tempScoreTarget = ScoreDisplay.Where(s => s.Score == _game.ScoreTarget).FirstOrDefault();

        if(tempScoreTarget is not null)
        {
            tempScoreTarget.IsSelected = true;
        }

        switch_Theme.IsToggled = _appSettings.Theme == "Dark";
    }

    private void CloseWindow()
    {
        //TODO: 2. Implement database entry
        //TODO: 3. Implement control for reverting to default settings
        //TODO: 4. != null is idiotic and needs to be replaced with a respective value
        //if (scoreValue != null)
        //{
        //    mainPage.GameSettingsSetter(scoreKey, scoreValue);
        //}
        //if (modeValue != null)
        //{
        //    mainPage.GameSettingsSetter(modeKey, modeValue);
        //}

        // todo: Create GameModel table and save game settings to database
        // todo: create AppSettings table and save app settings to database

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
            return;
        }

        _game.TeamOneName = entry_Team1.Text;
        _game.TeamTwoName = entry_Team2.Text;
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
        frame_TeamSettings.IsVisible = false;
        frame_ScoresSettings.IsVisible = true;
    }
    private void CloseScoresSettingsButton_Clicked(object sender, EventArgs e)
    {
        _game.ScoreTarget = (ScoreDisplay.FirstOrDefault(s => s.IsSelected)).Score;
        frame_ScoresSettings.IsVisible = false;
    }

    private void DeleteLastResultButton_Clicked(object sender, EventArgs e)
    {
        //mainPage.DeleteLastResult();
        // todo: delete from database
        // on main page implement a method that will delete the last result
        CloseWindow();
    }

    private void DarkModeSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        _appSettings.Theme = e.Value ? "Dark" : "Light";
    }
}