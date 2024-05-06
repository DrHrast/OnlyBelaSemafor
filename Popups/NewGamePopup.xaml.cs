using CommunityToolkit.Maui.Views;
using OnlyBelaSemafor.Models;

namespace OnlyBelaSemafor.Popups;

public partial class NewGamePopup : Popup
{
    private readonly GameModel _game;
    private readonly DatabaseManager _databaseManager;
    private string _message = "";
    private string _teamOneName = "";
    private string _teamTwoName = "";
    private int _scoreTarget;
    private bool _isCloseButtonVisible;

    public string TeamOneName
    {
        get => _teamOneName;
        set
        {
            if (value == _teamOneName) return;
            _teamOneName = value;
            OnPropertyChanged();
        }
    }
    public string TeamTwoName
    {
        get => _teamTwoName;
        set
        {
            if (value == _teamTwoName) return;
            _teamTwoName = value;
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
    public bool IsCloseButtonVisible
    {
        get => _isCloseButtonVisible;
        set
        {
            if (value == _isCloseButtonVisible) return;
            _isCloseButtonVisible = value;
            OnPropertyChanged();
        }
    }
    public string Message
    {
        get => _message;
        set
        {
            if (value == _message) return;
            _message = value;
            OnPropertyChanged();
        }
    }
    
    public List<ScoreDisplayModel> ScoreDisplay { get; set; } = new()
    {
        new ScoreDisplayModel { Score = 1301, IsSelected = false },
        new ScoreDisplayModel { Score = 1001, IsSelected = false },
        new ScoreDisplayModel { Score = 701, IsSelected = false },
        new ScoreDisplayModel { Score = 501, IsSelected = false },
        new ScoreDisplayModel { Score = 301, IsSelected = false }
    };
    
    public NewGamePopup(bool isStartup)
    {
        InitializeComponent();
        BindingContext = this;

        _game = App.Current.Services.GetRequiredService<GameModel>();
        _databaseManager = App.Current.Services.GetRequiredService<DatabaseManager>();

        if (isStartup) return;
        
        LoadSavedSettings();
    }

    private void LoadSavedSettings()
    {
        TeamOneName = _game.TeamOneName;
        TeamTwoName = _game.TeamTwoName;
        ScoreTarget = _game.ScoreTarget;
        ScoreDisplay.FirstOrDefault(x => x.Score == ScoreTarget)!.IsSelected = true;
        
        IsCloseButtonVisible = true;
    }
    
    private void NewGameButton_Clicked(object sender, EventArgs e)
    {
        if (!ValidateInputs()) return;
        
        _game.TeamOneName = TeamOneName;
        _game.TeamTwoName = TeamTwoName;
        _game.ScoreTarget = ScoreTarget;
        
        _databaseManager.SaveGame(_game);
        
        CloseAsync(true);
    }
    private bool ValidateInputs()
    {
        if (string.IsNullOrWhiteSpace(TeamOneName) || string.IsNullOrWhiteSpace(TeamTwoName))
        {
            Message = "Please enter team names";
            return false;
        }
        
        var selectedScore = ScoreDisplay.FirstOrDefault(x => x.IsSelected);

        if (selectedScore != null)
        {
            ScoreTarget = selectedScore.Score;
        }
        else
        {
            Message = "Please choose score target";
            return false; 
        }

        return true;
    }

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        CloseAsync(false);
    }

}